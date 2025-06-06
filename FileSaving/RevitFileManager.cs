using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Intech.Revit
{
    public class RevitFileManager
    {
        private readonly string tempPath;
        private readonly string localPath;
        private readonly string sharedPath;
        private readonly ISaveFileFormat format;
        private readonly string projectName;

        public RevitFileManager(Document doc, ISaveFileFormat format)
        {
            this.projectName = doc.Title;
            this.format = format ?? throw new ArgumentNullException(nameof(format));

            string baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Intech");
            Directory.CreateDirectory(baseDir);

            tempPath = Path.Combine(baseDir, "temp.txt");
            localPath = Path.Combine(baseDir, "local.txt");
            sharedPath = Path.Combine(@"\\SharedDrive\Intech", "shared.txt"); // One shared file
        }

        public string TempPath => tempPath;
        public string LocalPath => localPath;
        public string SharedPath => sharedPath;

        private SaveFileManager GetManager(string path) => new SaveFileManager(path, format);

        public void InitializeTempFromLocal()
        {
            var localManager = GetManager(localPath);
            var tempManager = GetManager(tempPath);

            var localSections = localManager.ReadAllSections();
            var projectSection = localSections.FirstOrDefault(s => s.SectionName == projectName);

            if (projectSection != null)
            {
                tempManager.WriteAllSections(new List<SaveFileSection> { projectSection });
            }
        }

        public void SaveToLocal()
        {
            var tempManager = GetManager(tempPath);
            var localManager = GetManager(localPath);

            var tempSection = tempManager.ReadAllSections().FirstOrDefault(s => s.SectionName == projectName);
            if (tempSection != null)
            {
                var localSections = localManager.ReadAllSections();
                localSections.RemoveAll(s => s.SectionName == projectName);
                localSections.Add(tempSection);
                localManager.WriteAllSections(localSections);
            }
        }

        public void SyncToSharedWithDeletions()
        {
            var tempManager = GetManager(tempPath);
            var localManager = GetManager(localPath);
            var sharedManager = GetManager(sharedPath);

            var tempSection = tempManager.ReadAllSections().FirstOrDefault(s => s.SectionName == projectName);
            var localSection = localManager.ReadAllSections().FirstOrDefault(s => s.SectionName == projectName);
            var sharedSections = sharedManager.ReadAllSections();
            var sharedSection = sharedSections.FirstOrDefault(s => s.SectionName == projectName)
                                ?? new SaveFileSection(projectName, tempSection?.Header ?? "");

            var comparer = new RowComparer();

            // Identify deletions
            var deletedRows = localSection?.Rows.Except(tempSection?.Rows ?? new List<string[]>(), comparer).ToList()
                              ?? new List<string[]>();

            // Identify additions
            var addedRows = tempSection?.Rows.Except(localSection?.Rows ?? new List<string[]>(), comparer).ToList()
                             ?? new List<string[]>();

            // Remove deleted rows from shared
            sharedSection.Rows = sharedSection.Rows
                .Where(row => !deletedRows.Any(del => comparer.Equals(row, del)))
                .ToList();

            // Add new rows to shared
            sharedSection.Rows.AddRange(addedRows);

            // Deduplicate
            sharedSection.Rows = sharedSection.Rows.Distinct(comparer).ToList();

            // Update section
            sharedSections.RemoveAll(s => s.SectionName == projectName);
            sharedSections.Add(sharedSection);
            sharedManager.WriteAllSections(sharedSections);

            // Pull back to local
            localManager.AddOrUpdateSection(sharedSection);
        }

        public void SyncGlobalSection()
        {
            var tempManager = GetManager(tempPath);
            var sharedManager = GetManager(sharedPath);

            var tempSection = tempManager.ReadAllSections()
                .FirstOrDefault(s => s.SectionName == "__GLOBAL__");
            if (tempSection == null) return;

            var sharedSections = sharedManager.ReadAllSections();
            var existing = sharedSections.FirstOrDefault(s => s.SectionName == "__GLOBAL__");

            if (existing != null)
            {
                var mergedRows = existing.Rows.Concat(tempSection.Rows)
                    .Distinct(new RowComparer()).ToList();
                existing.Rows = mergedRows;
                existing.Header = tempSection.Header;
            }
            else
            {
                sharedSections.Add(tempSection);
            }

            sharedManager.WriteAllSections(sharedSections);
        }

        private (List<string[]> added, List<string[]> removed) DiffRows(List<string[]> oldRows, List<string[]> newRows)
        {
            var comparer = new RowComparer();
            var added = newRows.Except(oldRows, comparer).ToList();
            var removed = oldRows.Except(newRows, comparer).ToList();
            return (added, removed);
        }

        private class RowComparer : IEqualityComparer<string[]>
        {
            public bool Equals(string[] x, string[] y) => x.SequenceEqual(y);
            public int GetHashCode(string[] obj) => string.Join("|", obj).GetHashCode();
        }
    }
}