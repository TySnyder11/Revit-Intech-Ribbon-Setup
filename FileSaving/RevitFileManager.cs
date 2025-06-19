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
        private readonly string localSharedPath;
        private readonly ISaveFileFormat format;
        private readonly string projectName;

        public RevitFileManager(Document doc, ISaveFileFormat format)
        {
            this.projectName = doc.Title;
            this.format = format ?? throw new ArgumentNullException(nameof(format));

            string baseDir = Path.Combine(App.BasePath, "SaveFileManager");
            Directory.CreateDirectory(baseDir);

            tempPath = Path.Combine(baseDir, "temp.txt");
            localPath = Path.Combine(baseDir, "local.txt");
            sharedPath = Path.Combine(@"V:\VCS Tools\Revit\Intech Ribbon\Ribbon Settings\SharedRibbonSync", "shared.txt");
            localSharedPath = Path.Combine(baseDir, "LocalShared.txt");
        }

        public string TempPath => tempPath;
        public string LocalPath => localPath;
        public string SharedPath => sharedPath;
        public string LocalSharedPath => localSharedPath;

        private SaveFileManager GetManager(string path) => new SaveFileManager(path, format);

        public void InitializeTempFromLocal()
        {
            var localManager = GetManager(localPath);
            var tempManager = GetManager(tempPath);

            var localSections = localManager.ReadAllSections();
            var projectSections = localSections.Where(s => s.ProjectName == projectName).ToList();

            tempManager.WriteAllSections(projectSections);
        }

        public void SaveToLocal()
        {
            var tempManager = GetManager(tempPath);
            var localManager = GetManager(localPath);

            var tempSections = tempManager.ReadAllSections().Where(s => s.ProjectName == projectName).ToList();
            var localSections = localManager.ReadAllSections();

            localSections.RemoveAll(s => s.ProjectName == projectName);
            localSections.AddRange(tempSections);

            localManager.WriteAllSections(localSections);
        }

        public void SyncToSharedWithDeletions()
        {
            var tempManager = GetManager(tempPath);
            var localSharedManager = GetManager(LocalSharedPath);
            var sharedManager = GetManager(SharedPath);

            var tempSections = tempManager.ReadAllSections().Where(s => s.ProjectName == projectName).ToList();
            var localSharedSections = localSharedManager.ReadAllSections().Where(s => s.ProjectName == projectName).ToList();
            var sharedSections = sharedManager.ReadAllSections();

            SaveToLocal();
            RowComparer comparer = new RowComparer();
            foreach (var tempSection in tempSections)
            {

                var localSaveSection = localSharedSections.FirstOrDefault(s => s.SecondaryName == tempSection.SecondaryName)
                                    ?? new SaveFileSection(projectName, tempSection.SecondaryName, tempSection.Header);
                var sharedSection = sharedSections.FirstOrDefault(s => s.ProjectName == projectName && s.SecondaryName == tempSection.SecondaryName)
                                    ?? new SaveFileSection(projectName, tempSection.SecondaryName, tempSection.Header);

                var deletedRows = localSaveSection?.Rows.Except(tempSection.Rows, comparer).ToList() ?? new List<string[]>();
                var addedRows = tempSection.Rows.Except(localSaveSection?.Rows ?? new List<string[]>(), comparer).ToList();

                foreach (string[] rowD in deletedRows)
                {
                    foreach (string[] rowS in sharedSection.Rows)
                    {
                        if (rowD.SequenceEqual(rowS))
                        {
                            sharedSection.Rows.Remove(rowS);
                        }
                    }
                }

                foreach (string[] row in addedRows)
                {
                    bool isNotBlank = row.All(cell => !string.IsNullOrWhiteSpace(cell));
                    bool isUniqueInSection = !sharedSection.Rows.Any(existing => comparer.Equals(existing, row));

                    if (isNotBlank && isUniqueInSection)
                    {
                        sharedSection.Rows.Add(row);
                    }
                }

                sharedManager.AddOrUpdateSection(sharedSection);
            }

            var localManager = GetManager(localPath);

            var importantSharedSections = sharedManager.ReadAllSections().Where(s => s.ProjectName == projectName).ToList();
            var localSections = localManager.ReadAllSections();

            tempSections.RemoveAll(s => s.ProjectName == projectName);
            localSharedSections.RemoveAll(s => s.ProjectName == projectName);
            localSections.RemoveAll(s => s.ProjectName == projectName);
            tempSections.AddRange(importantSharedSections);
            localSharedSections.AddRange(importantSharedSections);
            localSections.AddRange(importantSharedSections);

            localManager.WriteAllSections(localSections);
            localSharedManager.WriteAllSections(localSharedSections);
            tempManager.WriteAllSections(tempSections);

        }
        private class RowComparer : IEqualityComparer<string[]>
        {
            public bool Equals(string[] x, string[] y) => x.SequenceEqual(y);
            public int GetHashCode(string[] obj) => string.Join("|", obj).GetHashCode();
        }
    }
}
