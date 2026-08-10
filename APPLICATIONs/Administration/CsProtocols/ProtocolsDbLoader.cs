using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using nlDataSourceSqlite;

namespace CsProtocols
{
    public class ProtocolsDbLoader
    {
        private dsqDataSourceSqlite _dataSource;
        private List<string> _foundFolders = new List<string>();

        public ProtocolsDbLoader(string dbPath)
        {
            // Обязательная регистрация провайдера кодировок для поддержки Windows-1251
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            _dataSource = new dsqDataSourceSqlite();
            _dataSource.__fDatabasePath = Path.GetDirectoryName(dbPath);
            _dataSource.__fDatabaseName = Path.GetFileName(dbPath);
            CreateTablesIfNotExist();
        }

        private void CreateTablesIfNotExist()
        {
            _dataSource.__mSqlCommand(@"
                CREATE TABLE IF NOT EXISTS App (
                    CHG INTEGER NOT NULL,
                    CLU INTEGER NOT NULL UNIQUE,
                    ELD INTEGER NOT NULL,
                    GID TEXT NOT NULL,
                    cgzApp INTEGER NOT NULL,
                    dsiApp TEXT NOT NULL,
                    Pfx TEXT,
                    PRIMARY KEY(CLU AUTOINCREMENT)
                )");

            _dataSource.__mSqlCommand(@"
                CREATE TABLE IF NOT EXISTS Pcl (
                    CHG INTEGER NOT NULL,
                    CLU INTEGER NOT NULL UNIQUE,
                    ELD INTEGER NOT NULL,
                    GID TEXT NOT NULL,
                    InkApp INTEGER NOT NULL,
                    InkCpu INTEGER NOT NULL,
                    InkPclTyp INTEGER NOT NULL,
                    InkUsr INTEGER NOT NULL,
                    Prc TEXT NOT NULL,
                    Fil INTEGER,
                    PRIMARY KEY(CLU AUTOINCREMENT)
                )");

            _dataSource.__mSqlCommand(@"
                CREATE TABLE IF NOT EXISTS PclRrd (
                    CHG INTEGER NOT NULL,
                    CLU INTEGER NOT NULL UNIQUE,
                    ELD INTEGER NOT NULL,
                    GID TEXT NOT NULL,
                    InkPcl INTEGER NOT NULL,
                    InkPclRrdTyp INTEGER NOT NULL,
                    Err TEXT,
                    Tck INTEGER,
                    PRIMARY KEY(CLU AUTOINCREMENT)
                )");
        }

        public void LoadAllFromDisk()
        {
            FindAllProtocolFolders();

            if (_foundFolders.Count == 0)
                throw new Exception("Папки PROTOCOLS не найдены на диске C:!");

            int totalPcl = 0;
            int totalRrd = 0;

            foreach (string folder in _foundFolders)
            {
                if (folder.StartsWith("U", StringComparison.OrdinalIgnoreCase) || folder.Contains("U:"))
                    continue;

                try
                {
                    var files = Directory.GetFiles(folder, "*.pcl")
                        .Where(f => !Path.GetFileNameWithoutExtension(f).EndsWith("rr") && !f.StartsWith("U", StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    foreach (var file in files)
                    {
                        totalPcl += InsertPclFile(file);

                        string rrdFile = Path.ChangeExtension(file, null) + "rrd.pcl";
                        if (File.Exists(rrdFile))
                        {
                            totalRrd += InsertRrdFile(rrdFile);
                        }
                    }
                }
                catch { }
            }

            // Вызываем обновление отображения данных в интерфейсе
            RefreshProtocolsView();

            Console.WriteLine($"Загружено Pcl: {totalPcl}, PclRrd: {totalRrd}");
        }


        private void RefreshProtocolsView()
        {
            // Здесь можно вызвать обновление формы, если у класса есть доступ к элементам интерфейса
        }

        private void FindAllProtocolFolders()
        {
            try
            {
                var found = Directory.GetDirectories(@"C:\", "PROTOCOLS", SearchOption.AllDirectories)
                    .Where(p => !p.StartsWith("U", StringComparison.OrdinalIgnoreCase) && !p.Contains("U:"));
                _foundFolders.AddRange(found);
            }
            catch { }

            string[] knownPaths = {
                @"C:\KviNA\APPLICATIONS\Administration\Administration\bin\Debug\PROTOCOLS",
                @"C:\KviNA\ADDITIVE\CsProtocols\CsProtocols\bin\Debug\PROTOCOLS",
                @"C:\KviNA\ADDITIVE\csManual\csManual\bin\Debug\PROTOCOLS"
            };

            foreach (string path in knownPaths)
            {
                if (Directory.Exists(path) && !path.StartsWith("U", StringComparison.OrdinalIgnoreCase) && !_foundFolders.Contains(path))
                    _foundFolders.Add(path);
            }

            _foundFolders = _foundFolders
                .Where(p => !p.StartsWith("U", StringComparison.OrdinalIgnoreCase) && !p.Contains("U:"))
                .Distinct()
                .ToList();
        }

        private string[] ReadFileSafe(string filePath)
        {
            // Регистрируем провайдер кодировок (если еще не зарегистрирован)
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // Читаем файл строго в Windows-1251 для корректного отображения русского текста
            Encoding win1251 = Encoding.GetEncoding(1251);
            string text = File.ReadAllText(filePath, win1251);

            text = text.Replace("\uFEFF", "").Replace("ï»¿", "");

            return text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }
        private int InsertPclFile(string filePath)
        {
            if (filePath.StartsWith("U", StringComparison.OrdinalIgnoreCase) || filePath.Contains("U:"))
                return 0;

            int count = 0;
            var lines = ReadFileSafe(filePath);
            bool isHeader = true;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (isHeader && line.StartsWith("CHG"))
                {
                    isHeader = false;
                    continue;
                }
                isHeader = false;

                var parts = line.Split(',');
                if (parts.Length < 11) continue;

                try
                {
                    string chg = parts[0].Trim();
                    string guid = parts[1].Trim();
                    string appName = parts[2].Trim();
                    string user = parts[6].Trim();
                    string pclTyp = parts[8].Trim();
                    string prc = parts[10].Trim();
                    string fil = parts.Length > 11 ? parts[11].Trim() : "";

                    if (!int.TryParse(pclTyp, out _))
                    {
                        continue;
                    }

                    int appClu = InsertApp(appName);

                    if (!PclExists(guid))
                    {
                        InsertPclRecord(chg, guid, appClu, pclTyp, user, prc, fil);
                        count++;
                    }
                }
                catch { }
            }

            return count;
        }

        private int InsertRrdFile(string filePath)
        {
            if (filePath.StartsWith("U", StringComparison.OrdinalIgnoreCase) || filePath.Contains("U:"))
                return 0;

            int count = 0;
            var lines = ReadFileSafe(filePath);
            bool isHeader = true;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (isHeader && line.StartsWith("CHG"))
                {
                    isHeader = false;
                    continue;
                }
                isHeader = false;

                try
                {
                    int idx1 = line.IndexOf(',');
                    if (idx1 == -1) continue;
                    int idx2 = line.IndexOf(',', idx1 + 1);
                    if (idx2 == -1) continue;
                    int idx3 = line.IndexOf(',', idx2 + 1);
                    if (idx3 == -1) continue;
                    int idx4 = line.IndexOf(',', idx3 + 1);
                    if (idx4 == -1) continue;

                    string chg = line.Substring(0, idx1).Trim();
                    string guid = line.Substring(idx1 + 1, idx2 - idx1 - 1).Trim();
                    string inkPcl = line.Substring(idx2 + 1, idx3 - idx2 - 1).Trim();
                    string rrdTyp = line.Substring(idx3 + 1, idx4 - idx3 - 1).Trim();

                    if (!long.TryParse(inkPcl, out _) || !int.TryParse(rrdTyp, out _))
                    {
                        continue;
                    }

                    int lastComma = line.LastIndexOf(',');
                    string err = "";
                    string tck = "-1";

                    if (lastComma > idx4)
                    {
                        string potentialTck = line.Substring(lastComma + 1).Trim();
                        potentialTck = new string(potentialTck.Where(char.IsDigit).ToArray());

                        if (int.TryParse(potentialTck, out int parsedTck))
                        {
                            tck = parsedTck.ToString();
                            err = line.Substring(idx4 + 1, lastComma - idx4 - 1).Trim();
                        }
                        else
                        {
                            err = line.Substring(idx4 + 1).Trim();
                        }
                    }
                    else
                    {
                        err = line.Substring(idx4 + 1).Trim();
                    }

                    if (!RrdExists(inkPcl, guid))
                    {
                        InsertRrdRecord(chg, guid, inkPcl, rrdTyp, err, tck);
                        count++;
                    }
                }
                catch { }
            }

            return count;
        }

        private int InsertApp(string appName)
        {
            if (string.IsNullOrEmpty(appName)) return -1;

            string safeAppName = EscapeSql(appName);
            string checkQuery = $"SELECT CLU FROM App WHERE dsiApp = '{safeAppName}'";
            var result = _dataSource.__mSqlValue(checkQuery);
            if (result != null && result != DBNull.Value)
                return Convert.ToInt32(result);

            string chg = DateTime.Now.Ticks.ToString();
            string guid = Guid.NewGuid().ToString();
            string insertQuery = $"INSERT INTO App (CHG, GID, ELD, cgzApp, dsiApp, Pfx) " +
                                 $"VALUES ('{chg}', '{guid}', 0, 0, '{safeAppName}', '')";
            _dataSource.__mSqlCommand(insertQuery);

            result = _dataSource.__mSqlValue(checkQuery);
            return Convert.ToInt32(result);
        }

        private bool PclExists(string guid)
        {
            string query = $"SELECT COUNT(*) FROM Pcl WHERE GID = '{guid}'";
            var result = _dataSource.__mSqlValue(query);
            return result != null && Convert.ToInt32(result) > 0;
        }

        private void InsertPclRecord(string chg, string guid, int appClu, string pclTyp, string user, string prc, string fil)
        {
            string newGuid = Guid.NewGuid().ToString();
            string safeUser = EscapeSql(user);
            string safePrc = EscapeSql(prc);
            string safeFil = EscapeSql(fil);
            string safePclTyp = string.IsNullOrEmpty(pclTyp) ? "0" : pclTyp;

            string query = $"INSERT INTO Pcl (CHG, GID, ELD, InkApp, InkPclTyp, InkUsr, Prc, Fil) " +
                           $"VALUES ('{chg}', '{newGuid}', 0, {appClu}, {safePclTyp}, '{safeUser}', '{safePrc}', '{safeFil}')";
            _dataSource.__mSqlCommand(query);
        }

        private bool RrdExists(string inkPcl, string guid)
        {
            string query = $"SELECT COUNT(*) FROM PclRrd WHERE InkPcl = '{inkPcl}' AND GID = '{guid}'";
            var result = _dataSource.__mSqlValue(query);
            return result != null && Convert.ToInt32(result) > 0;
        }

        private void InsertRrdRecord(string chg, string guid, string inkPcl, string rrdTyp, string err, string tck)
        {
            string newGuid = Guid.NewGuid().ToString();

            string safeInkPcl = string.IsNullOrEmpty(inkPcl) ? "0" : inkPcl;
            string safeRrdTyp = string.IsNullOrEmpty(rrdTyp) ? "0" : rrdTyp;
            string safeTck = string.IsNullOrEmpty(tck) ? "-1" : tck;
            string safeErr = EscapeSql(err);

            string query = $"INSERT INTO PclRrd (CHG, GID, ELD, InkPcl, InkPclRrdTyp, Err, Tck) " +
                           $"VALUES ('{chg}', '{newGuid}', 0, {safeInkPcl}, {safeRrdTyp}, '{safeErr}', {safeTck})";
            _dataSource.__mSqlCommand(query);
        }

        private string EscapeSql(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            // Вырезаем любые упоминания диска U из текста ошибок на всякий случай
            string cleaned = Regex.Replace(input, @"[Uu]:[/\\][^\s""',]*", "[путь удален]");

            return cleaned
                .Replace("'", "''")
                .Replace("\"", "''")
                .Replace("‘", "''")
                .Replace("’", "''")
                .Replace("“", "''")
                .Replace("”", "''")
                .Replace("«", "''")
                .Replace("»", "''")
                .Replace("„", "''");
        }
    }
}