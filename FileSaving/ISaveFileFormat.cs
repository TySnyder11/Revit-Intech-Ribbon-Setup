using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace Intech
{
    public interface ISaveFileFormat
    {
        string SerializeRow(string[] row);
        string[] DeserializeRow(string line);
        string SerializeHeader(string header);
    }

    public class TxtFormat : ISaveFileFormat
    {
        public string SerializeRow(string[] row) => string.Join("\t", row);
        public string[] DeserializeRow(string line) => line.Split('\t');
        public string SerializeHeader(string header) => header;
    }

    public class CsvFormat : ISaveFileFormat
    {
        public string SerializeRow(string[] row) =>
            string.Join(",", row.Select(r => $"\"{r.Replace("\"", "\"\"")}\""));
        public string[] DeserializeRow(string line) =>
            line.Split(','); // Simplified; consider using a CSV parser for edge cases
        public string SerializeHeader(string header) => header;
    }

    public class JsonFormat : ISaveFileFormat
    {
        public string SerializeRow(string[] row) => JsonSerializer.Serialize(row);
        public string[] DeserializeRow(string line) => JsonSerializer.Deserialize<string[]>(line);
        public string SerializeHeader(string header) => $"// {header}";
    }
}

