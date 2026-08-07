using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsProtocols.DATA.Models;

namespace CsProtocols.DATA.Loaders
{
    public class ProtocolLoader
    {
        public List<ProtocolRecord> Load(string folderPath)
        {
            var result = new List<ProtocolRecord>();

            if (!Directory.Exists(folderPath))
                return result;

            var files = Directory.GetFiles(folderPath, "*.pcl", SearchOption.AllDirectories)
                .Where(f => !Path.GetFileNameWithoutExtension(f).EndsWith("rr"))
                .ToList();

            foreach (var file in files)
            {
                try
                {
                    var records = ParseProtocolFile(file);
                    result.AddRange(records);
                }
                catch { }
            }

            return result;
        }

        public List<ProtocolRecord> LoadSingleFile(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<ProtocolRecord>();

            return ParseProtocolFile(filePath);
        }

        public List<ProtocolRecord> LoadRecordsForProtocol(string pclFilePath, string guid)
        {
            var result = new List<ProtocolRecord>();

            string dir = Path.GetDirectoryName(pclFilePath);
            string name = Path.GetFileNameWithoutExtension(pclFilePath);
            string rrdPath = Path.Combine(dir, name + "rrd.pcl");

            if (!File.Exists(rrdPath))
                return result;

            // ← ПРИНУДИТЕЛЬНО ЧИТАЕМ В КОДИРОВКЕ 1251
            var lines = File.ReadAllLines(rrdPath, System.Text.Encoding.GetEncoding(1251));
            bool isHeader = true;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (isHeader && line.StartsWith("CHG"))
                {
                    isHeader = false;
                    continue;
                }
                isHeader = false;

                var parts = line.Split(',');
                if (parts.Length < 5)
                    continue;

                if (parts.Length > 2 && parts[2].Trim() == guid)
                {
                    var record = new ProtocolRecord
                    {
                        Guid = parts[1].Trim(),
                        RecordType = parts[3].Trim(),
                        Message = parts[4].Trim(),
                        DateTime = DateTime.Now
                    };

                    if (parts.Length > 0 && long.TryParse(parts[0], out long ticks))
                        record.DateTime = new DateTime(ticks);

                    result.Add(record);
                }
            }

            return result;
        }

        private List<ProtocolRecord> ParseProtocolFile(string filePath)
        {
            var records = new List<ProtocolRecord>();
            var lines = File.ReadAllLines(filePath);

            if (lines.Length == 0)
                return records;

            string programName = Path.GetFileName(Path.GetDirectoryName(filePath)) ?? "PROTOCOLS";
            if (programName == "PROTOCOLS" || programName == "Release")
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                if (fileName.StartsWith("_"))
                    programName = fileName.Substring(1);
                else
                    programName = fileName;
            }

            bool isHeader = true;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (isHeader && line.StartsWith("CHG"))
                {
                    isHeader = false;
                    continue;
                }
                isHeader = false;

                var record = ParseCsvLine(line, programName);
                if (record != null)
                    records.Add(record);
            }

            return records;
        }

        private ProtocolRecord ParseCsvLine(string line, string programName)
        {
            try
            {
                var parts = line.Split(',');

                if (parts.Length < 11)
                    return null;

                var record = new ProtocolRecord
                {
                    Program = programName,
                    Computer = "",
                    User = "",
                    ProtocolType = "Application",
                    ErrorType = "None",
                    Procedure = "",
                    Description = "",
                    ImageFile = "",
                    UserSolution = ""
                };

                if (parts.Length > 0 && long.TryParse(parts[0], out long ticks))
                    record.DateTime = new DateTime(ticks);
                else
                    record.DateTime = DateTime.Now;

                if (parts.Length > 1)
                    record.Guid = parts[1].Trim();

                if (parts.Length > 2 && !string.IsNullOrEmpty(parts[2]))
                    record.Program = parts[2].Trim();

                if (parts.Length > 5)
                    record.Computer = parts[5].Trim();

                if (parts.Length > 6)
                    record.User = parts[6].Trim();

                if (parts.Length > 8 && int.TryParse(parts[8].Trim(), out int protocolTypeId))
                {
                    record.ProtocolType = MapProtocolType(protocolTypeId);
                    record.ErrorType = MapErrorType(protocolTypeId);
                }

                if (parts.Length > 10)
                    record.Procedure = parts[10].Trim();

                if (parts.Length > 11 && !string.IsNullOrEmpty(parts[11]))
                    record.ImageFile = parts[11].Trim();

                record.Description = $"Протокол {record.ProtocolType}";

                if (record.Procedure.Contains("_mBegin"))
                    record.RecordType = "Start";
                else if (record.Procedure.Contains("_mEnd"))
                    record.RecordType = "Finish";
                else if (record.Procedure.Contains("Error") || record.Procedure.Contains("Exception"))
                    record.RecordType = "Exception";
                else
                    record.RecordType = "Action";

                return record;
            }
            catch
            {
                return null;
            }
        }

        private string MapProtocolType(int id)
        {
            switch (id)
            {
                case 0: case 1: case 2: case 3: case 4: return "Application";
                case 5: case 6: return "Database";
                case 7: case 8: return "Network";
                case 9: case 10: case 11: return "User";
                case 12: return "Security";
                default: return "Application";
            }
        }

        private string MapErrorType(int id)
        {
            switch (id)
            {
                case 0: case 1: case 2: case 3: case 4: return "Information";
                case 5: case 6: return "Error";
                case 7: case 8: return "Warning";
                case 9: case 10: case 11: return "None";
                case 12: return "Critical";
                default: return "None";
            }
        }
    }
}