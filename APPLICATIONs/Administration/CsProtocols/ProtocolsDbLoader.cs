//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
//using CsProtocols.DATA.Loaders;
//using CsProtocols.DATA.Models;
//using nlData;
//using nlDataSourceSqlite;

//namespace CsProtocols
//{
//    /// <summary>
//    /// Загружает файлы *.pcl и *rrd.pcl в единую базу protocols.db.
//    /// Формат таблиц совместим с уже созданными базами протоколов: Pcl/App/PclRrd.
//    /// </summary>
//    public class ProtocolsDbLoader
//    {
//        // Обычный dsqDataSourceSqlite показывает модальное окно на каждую строку с ошибкой.
//        // Для фонового импорта используем вариант, который записывает одну техническую ошибку в лог.
//        private readonly datUnitDataSource _dataSource;
//        private readonly ProtocolLoader _protocolLoader = new ProtocolLoader();

//        public ProtocolsDbLoader(string pDatabasePath)
//        {
//            Directory.CreateDirectory(Path.GetDirectoryName(pDatabasePath));
//            _dataSource = new dsqDataSourceSqliteWithProtocol();
//            _dataSource.__fDatabasePath = Path.GetDirectoryName(pDatabasePath);
//            _dataSource.__fDatabaseName = Path.GetFileName(pDatabasePath);
//            _dataSource.__mDatabaseCreate();
//            mTablesEnsure();
//        }

//        /// <summary>Загружает известную папку рекурсивно. Повторный запуск не создаёт дубликатов.</summary>
//        public int LoadFromFolder(string pFolderPath)
//        {
//            if (!Directory.Exists(pFolderPath)) return 0;

//            int vImported = 0;
//            foreach (string vPclFile in Directory.GetFiles(pFolderPath, "*.pcl", SearchOption.AllDirectories)
//                .Where(pFile => !Path.GetFileNameWithoutExtension(pFile).EndsWith("rrd", StringComparison.OrdinalIgnoreCase)))
//            {
//                vImported += mImportPclFile(vPclFile);
//                string vRrdFile = Path.Combine(Path.GetDirectoryName(vPclFile), Path.GetFileNameWithoutExtension(vPclFile) + "rrd.pcl");
//                if (File.Exists(vRrdFile)) vImported += mImportRrdFile(vRrdFile);
//            }
//            return vImported;
//        }

//        /// <summary>
//        /// Копирует исходные файлы в архив приложения, не перемещая их и не меняя источники.
//        /// Относительная структура папок сохраняется, поэтому протоколы разных программ не смешиваются.
//        /// </summary>
//        public int CopyFolderToArchive(string pSourceFolder, string pArchiveFolder)
//        {
//            if (!Directory.Exists(pSourceFolder)) return 0;
//            int vCopied = 0;
//            foreach (string vSourceFile in Directory.GetFiles(pSourceFolder, "*.pcl", SearchOption.AllDirectories))
//            {
//                string vRelativePath = vSourceFile.Substring(pSourceFolder.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
//                string vDestinationFile = Path.Combine(pArchiveFolder, vRelativePath);
//                Directory.CreateDirectory(Path.GetDirectoryName(vDestinationFile));
//                if (!File.Exists(vDestinationFile) || File.GetLastWriteTimeUtc(vSourceFile) > File.GetLastWriteTimeUtc(vDestinationFile) || new FileInfo(vSourceFile).Length != new FileInfo(vDestinationFile).Length)
//                {
//                    File.Copy(vSourceFile, vDestinationFile, true);
//                    vCopied++;
//                }
//            }
//            return vCopied;
//        }

//        private void mTablesEnsure()
//        {
//            _dataSource.__mSqlCommand("CREATE TABLE IF NOT EXISTS App (CLU INTEGER PRIMARY KEY AUTOINCREMENT, dsiApp TEXT, GID TEXT, Pfx TEXT)");
//            _dataSource.__mSqlCommand("CREATE TABLE IF NOT EXISTS Pcl (CLU INTEGER PRIMARY KEY AUTOINCREMENT, CHG TEXT, GID TEXT, InkApp INTEGER, InkPclTyp INTEGER, Prc TEXT, Fil TEXT, Hst TEXT, Usr TEXT, desPclTyp TEXT)");
//            _dataSource.__mSqlCommand("CREATE TABLE IF NOT EXISTS PclRrd (CLU INTEGER PRIMARY KEY AUTOINCREMENT, CHG TEXT, GID TEXT, InkPcl TEXT, InkPclRrdTyp INTEGER, Err TEXT, Msg TEXT, Tck TEXT, desRrdTyp TEXT)");

//            mColumnEnsure("App", "dsiApp", "TEXT");
//            mColumnEnsure("App", "GID", "TEXT");
//            mColumnEnsure("App", "Pfx", "TEXT");
//            mColumnEnsure("Pcl", "GID", "TEXT");
//            mColumnEnsure("Pcl", "InkApp", "INTEGER");
//            mColumnEnsure("Pcl", "InkPclTyp", "INTEGER");
//            mColumnEnsure("Pcl", "Fil", "TEXT");
//            mColumnEnsure("Pcl", "Hst", "TEXT");
//            mColumnEnsure("Pcl", "Usr", "TEXT");
//            mColumnEnsure("Pcl", "desPclTyp", "TEXT");
//            mColumnEnsure("PclRrd", "CHG", "TEXT");
//            mColumnEnsure("PclRrd", "GID", "TEXT");
//            mColumnEnsure("PclRrd", "InkPcl", "TEXT");
//            mColumnEnsure("PclRrd", "InkPclRrdTyp", "INTEGER");
//            mColumnEnsure("PclRrd", "Err", "TEXT");
//            mColumnEnsure("PclRrd", "Msg", "TEXT");
//            mColumnEnsure("PclRrd", "Tck", "TEXT");
//            mColumnEnsure("PclRrd", "desRrdTyp", "TEXT");
//        }

//        private void mColumnEnsure(string pTableName, string pColumnName, string pType)
//        {
//            DataTable vColumns = _dataSource.__mSqlQuery("PRAGMA table_info(" + pTableName + ")");
//            bool vExists = vColumns != null && vColumns.AsEnumerable().Any(pRow => String.Equals(Convert.ToString(pRow["name"]), pColumnName, StringComparison.OrdinalIgnoreCase));
//            if (!vExists) _dataSource.__mSqlCommand("ALTER TABLE " + pTableName + " ADD COLUMN " + pColumnName + " " + pType);
//        }

//        private int mImportPclFile(string pFilePath)
//        {
//            int vImported = 0;
//            foreach (ProtocolRecord vRecord in _protocolLoader.LoadSingleFile(pFilePath))
//            {
//                string vGid = String.IsNullOrWhiteSpace(vRecord.Guid) ? pFilePath + ":" + vRecord.DateTime.Ticks : vRecord.Guid;
//                if (mExists("Pcl", "GID", vGid))
//                {
//                    _dataSource.__mSqlCommand("UPDATE Pcl SET InkPclTyp = " + mProtocolTypeId(vRecord) + ", desPclTyp = '" + mSql(vRecord.ProtocolType) + "' WHERE GID = '" + mSql(vGid) + "'");
//                    continue;
//                }

//                int vAppClu = mAppEnsure(vRecord.Program);
//                string vCommand = "INSERT INTO Pcl (CHG, GID, InkApp, InkPclTyp, Prc, Fil, Hst, Usr, desPclTyp) VALUES ("
//                    + "'" + mSql(vRecord.DateTime.Ticks.ToString()) + "', '" + mSql(vGid) + "', " + vAppClu + ", " + mProtocolTypeId(vRecord) + ", "
//                    + "'" + mSql(vRecord.Procedure) + "', '" + mSql(pFilePath) + "', '" + mSql(vRecord.Computer) + "', '" + mSql(vRecord.User) + "', '" + mSql(vRecord.ProtocolType) + "')";
//                _dataSource.__mSqlCommand(vCommand);
//                vImported++;
//            }
//            return vImported;
//        }

//        private int mImportRrdFile(string pFilePath)
//        {
//            int vImported = 0;
//            foreach (string vLine in File.ReadAllLines(pFilePath, Encoding.GetEncoding(1251)))
//            {
//                if (String.IsNullOrWhiteSpace(vLine) || vLine.StartsWith("CHG", StringComparison.OrdinalIgnoreCase)) continue;
//                string[] vParts = vLine.Split(',');
//                if (vParts.Length < 5) continue;

//                string vGid = vParts[1].Trim();
//                string vPclGid = vParts[2].Trim();
//                if (String.IsNullOrEmpty(vGid) || mExists("PclRrd", "GID", vGid)) continue;

//                string vTick = vParts.Length > 5 ? vParts[5].Trim() : vParts[0].Trim();
//                string vRecordType = mRecordTypeName(vParts[3].Trim());
//                string vMessage = String.Join(",", vParts.Skip(4).Take(vParts.Length > 5 ? vParts.Length - 5 : 1)).Trim();
//                _dataSource.__mSqlCommand("INSERT INTO PclRrd (CHG, GID, InkPcl, InkPclRrdTyp, Err, Msg, Tck, desRrdTyp) VALUES ('"
//                    + mSql(vParts[0].Trim()) + "', '" + mSql(vGid) + "', '" + mSql(vPclGid) + "', " + mNumber(vParts[3]) + ", '"
//                    + mSql(vMessage) + "', '" + mSql(vMessage) + "', '" + mSql(vTick) + "', '" + mSql(vRecordType) + "')");
//                vImported++;
//            }
//            return vImported;
//        }

//        private int mAppEnsure(string pAppName)
//        {
//            string vAppName = String.IsNullOrWhiteSpace(pAppName) ? "Неизвестное приложение" : pAppName.Trim();
//            object vClu = _dataSource.__mSqlValue("SELECT CLU FROM App WHERE dsiApp = '" + mSql(vAppName) + "' LIMIT 1");
//            if (vClu != null && vClu != DBNull.Value) return Convert.ToInt32(vClu);
//            _dataSource.__mSqlCommand("INSERT INTO App (dsiApp, GID, Pfx) VALUES ('" + mSql(vAppName) + "', '" + Guid.NewGuid() + "', '')");
//            vClu = _dataSource.__mSqlValue("SELECT CLU FROM App WHERE dsiApp = '" + mSql(vAppName) + "' ORDER BY CLU DESC LIMIT 1");
//            return vClu == null || vClu == DBNull.Value ? 0 : Convert.ToInt32(vClu);
//        }

//        private bool mExists(string pTableName, string pFieldName, string pValue)
//        {
//            object vCount = _dataSource.__mSqlValue("SELECT COUNT(*) FROM " + pTableName + " WHERE " + pFieldName + " = '" + mSql(pValue) + "'");
//            return vCount != null && vCount != DBNull.Value && Convert.ToInt32(vCount) > 0;
//        }

//        private static string mSql(string pValue) { return (pValue ?? String.Empty).Replace("'", "''"); }
//        private static int mNumber(string pValue) { int vValue; return Int32.TryParse(pValue, out vValue) ? vValue : 0; }
//        private static int mProtocolTypeId(ProtocolRecord pRecord) { return pRecord == null ? 0 : pRecord.ProtocolTypeId; }
//        private static string mRecordTypeName(string pValue)
//        {
//            switch (mNumber(pValue))
//            {
//                case 0: return "Решение пользователя";
//                case 1: return "Детали";
//                case 2: return "Исключение";
//                case 3: return "Изображение";
//                case 4: return "Сообщение";
//                case 5: return "Свойства объекта";
//                default: return "Запись протокола";
//            }
//        }
//    }
//}
