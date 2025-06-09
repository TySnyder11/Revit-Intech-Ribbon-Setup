using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Intech
{
    public class SaveFileManager
    {
        private string filePath;
        private ISaveFileFormat format;

        public SaveFileManager(string filePath, ISaveFileFormat format)
        {
            this.filePath = Path.GetFullPath(filePath);
            this.format = format ?? throw new ArgumentNullException(nameof(format));
        }


        public List<SaveFileSection> ReadAllSections()
        {
            var sections = new List<SaveFileSection>();
            if (!File.Exists(filePath)) return sections;

            using (StreamReader sr = new StreamReader(filePath))
            {
                SaveFileSection currentSection = null;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("# "))
                    {
                        if (currentSection != null)
                            sections.Add(currentSection);

                        string fullName = line.Substring(2).Trim();
                        string[] parts = fullName.Split(new[] { "::" }, StringSplitOptions.None);
                        string projectName = parts[0];
                        string secondaryName = parts.Length > 1 ? parts[1] : "";

                        currentSection = new SaveFileSection(projectName, secondaryName, "");
                    }
                    else if (currentSection != null && string.IsNullOrEmpty(currentSection.Header))
                    {
                        currentSection.Header = line;
                    }
                    else if (currentSection != null)
                    {
                        currentSection.Rows.Add(format.DeserializeRow(line));
                    }
                }

                if (currentSection != null)
                    sections.Add(currentSection);
            }

            return sections;
        }
        public void WriteAllSections(List<SaveFileSection> sections)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                for (int i = 0; i < sections.Count; i++)
                {
                    var section = sections[i];
                    string fullName = section.ProjectName +
                    (string.IsNullOrEmpty(section.SecondaryName) ? "" : $"::{section.SecondaryName}");

                    sw.WriteLine($"# {fullName}");
                    sw.WriteLine(format.SerializeHeader(section.Header));

                    foreach (var row in section.Rows)
                    {
                        sw.WriteLine(format.SerializeRow(row));
                    }

                    if (i < sections.Count - 1)
                        sw.WriteLine(); // Only between sections
                }
            }
        }



        public void AddOrUpdateSection(SaveFileSection newSection)
        {
            var sections = ReadAllSections();
            var existing = sections.FirstOrDefault(s =>
                s.ProjectName == newSection.ProjectName &&
                s.SecondaryName == newSection.SecondaryName);

            if (existing != null)
            {
                existing.Header = newSection.Header;
                existing.Rows = newSection.Rows;
            }
            else
            {
                sections.Add(newSection);
            }
            WriteAllSections(sections);
        }

        public void RemoveSection(string projectName, string secondaryName)
        {
            var sections = ReadAllSections();
            sections.RemoveAll(s =>
                s.ProjectName == projectName &&
                s.SecondaryName == secondaryName);
            WriteAllSections(sections);
        }

        public List<SaveFileSection> GetSectionsByProject(string projectName)
        {
            return ReadAllSections().Where(s => s.ProjectName == projectName).ToList();
        }
    }

    public class SaveFileSection
    {
        public string ProjectName { get; set; }
        public string SecondaryName { get; set; }
        public string Header { get; set; }
        public List<string[]> Rows { get; set; } = new List<string[]>();

        public SaveFileSection(string projectName, string secondaryName, string header)
        {
            ProjectName = projectName;
            SecondaryName = secondaryName;
            Header = header;
        }
    }
}
