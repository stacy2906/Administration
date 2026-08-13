using System;

namespace CsProtocols.DATA.Models
{
    public class ProtocolRecord
    {
        public DateTime DateTime { get; set; }
        public string Program { get; set; }
        public string User { get; set; }
        public string Computer { get; set; }
        public string ProtocolType { get; set; }
        public string ErrorType { get; set; }
        public string Procedure { get; set; }
        public string Description { get; set; }
        public string UserSolution { get; set; }
        public string RecordType { get; set; }
        public string Guid { get; set; }
        public string ImageFile { get; set; }
        public string Message { get; set; }
        public string Key { get; set; }  // ← ДОБАВЛЕНО
        public string SourceFile { get; set; }  // ← ДОБАВЛЕНО: путь к .pcl файлу-источнику, нужен для загрузки записей протокола
    }
}