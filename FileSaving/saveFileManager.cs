using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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

            string[] lines = File.ReadAllLines(filePath);
            SaveFileSection currentSection = null;

            foreach (string line in lines)
            {
                if (line.StartsWith("# "))
                {
                    if (currentSection != null)
                        sections.Add(currentSection);

                    string sectionName = line.Substring(2).Trim();
                    currentSection = new SaveFileSection(sectionName, "");
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

            return sections;
        }

        public void WriteAllSections(List<SaveFileSection> sections)
        {
            StreamWriter sw = new StreamWriter(filePath, false);
            foreach (var section in sections)
            {
                sw.WriteLine($"# {section.SectionName}");
                sw.WriteLine(format.SerializeHeader(section.Header));
                foreach (var row in section.Rows)
                {
                    sw.WriteLine(format.SerializeRow(row));
                }
                sw.WriteLine(); // Blank line between sections
            }
        }

        public void AddOrUpdateSection(SaveFileSection newSection)
        {
            var sections = ReadAllSections();
            var existing = sections.FirstOrDefault(s => s.SectionName == newSection.SectionName);
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

        public void RemoveSection(string sectionName)
        {
            var sections = ReadAllSections();
            sections.RemoveAll(s => s.SectionName.Equals(sectionName, StringComparison.OrdinalIgnoreCase));
            WriteAllSections(sections);
        }

        public void ExportSection(string sectionName, string exportPath, ISaveFileFormat exportFormat)
        {
            var section = ReadAllSections().FirstOrDefault(s => s.SectionName == sectionName);
            if (section == null) return;

            StreamWriter sw = new StreamWriter(exportPath);
            sw.WriteLine(exportFormat.SerializeHeader(section.Header));
            foreach (var row in section.Rows)
            {
                sw.WriteLine(exportFormat.SerializeRow(row));
            }
        }
    }


    public class SaveFileSection
    {
        public string SectionName { get; set; }
        public string Header { get; set; }
        public List<string[]> Rows { get; set; } = new List<string[]>();

        public SaveFileSection(string sectionName, string header)
        {
            SectionName = sectionName;
            Header = header;
        }
    }
}
