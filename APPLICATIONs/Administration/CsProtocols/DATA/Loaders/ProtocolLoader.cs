using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
                .Where(f => !Path.GetFileNameWithoutExtension(f).EndsWith("rrd"))
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

            int lineNumber = 0;
            foreach (string line in lines)
            {
                lineNumber++;
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
                        RecordType = MapRecordType(parts[3].Trim()),
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
            int lineNumber = 0;

            foreach (string line in lines)
            {
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (isHeader && line.StartsWith("CHG"))
                {
                    isHeader = false;
                    continue;
                }
                isHeader = false;

                var record = line.StartsWith("[") ? ParseBracketLine(line, filePath, lineNumber) : ParseCsvLine(line, programName);
                if (record != null)
                {
                    record.SourceFile = filePath;
                    records.Add(record);
                }
            }

            return records;
        }

        private ProtocolRecord ParseBracketLine(string line, string filePath, int lineNumber)
        {
            MatchCollection matches = Regex.Matches(line, @"\[([^\]]*)\]");
            if (matches.Count < 8)
                return null;

            long ticks;
            var record = new ProtocolRecord
            {
                Guid = filePath + ":" + lineNumber,
                Program = matches[2].Groups[1].Value,
                Computer = matches.Count > 5 ? matches[5].Groups[1].Value : "",
                User = matches.Count > 6 ? matches[6].Groups[1].Value : "",
                ProtocolType = matches.Count > 7 ? MapProtocolType(ConvertToInt(matches[7].Groups[1].Value)) : "Application",
                ProtocolTypeId = matches.Count > 7 ? ConvertToInt(matches[7].Groups[1].Value) : 0,
                Procedure = matches.Count > 9 ? matches[9].Groups[1].Value : "",
                Description = line,
                Message = line,
                RecordType = "Message",
                ErrorType = "None"
            };
            record.DateTime = Int64.TryParse(matches[0].Groups[1].Value, out ticks) ? new DateTime(ticks) : DateTime.Now;
            return record;
        }

        private static int ConvertToInt(string value) { int result; return Int32.TryParse(value, out result) ? result : 0; }

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
                    record.ProtocolTypeId = protocolTypeId;
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
                case 1: return "Ошибка приложения";
                case 2: return "Ошибка программирования";
                case 3: return "Исключение";
                case 4: return "Событие приложения";
                case 5: return "Ошибка источника данных";
                case 6: return "Событие источника данных";
                case 7: return "Ошибка устройства";
                case 8: return "Событие устройства";
                case 9: return "Ошибка пользователя";
                case 10: return "Событие пользователя";
                case 11: return "Сообщение пользователю";
                case 12: return "Прочее";
                default: return "Не указан";
            }
        }

        private string MapRecordType(string pRawId)
        {
            int vId;
            if (int.TryParse(pRawId, out vId) == false)
                return pRawId; // Не число - возвращаем как есть, не портим данные

            switch (vId)
            {
                case 0: return "Решение пользователя";   // Answer
                case 1: return "Детали события";          // Detail
                case 2: return "Исключение";              // Exception
                case 3: return "Изображение";             // Image
                case 4: return "Сообщение";               // Message
                case 5: return "Свойство объекта";        // ObjectProperty
                case 6: return "Причина ошибки";          // Reason
                default: return pRawId;
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
