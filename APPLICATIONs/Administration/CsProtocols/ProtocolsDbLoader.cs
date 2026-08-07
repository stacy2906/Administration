using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using nlDataSourceSqlite;

namespace CsProtocols
{
    public class ProtocolsDbLoader
    {
        private dsqDataSourceSqlite _dataSource;
        private List<string> _foundFolders = new List<string>();

        public ProtocolsDbLoader(string dbPath)
        {
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
                throw new Exception("Папки PROTOCOLS не найдены!");

            int totalPcl = 0;
            int totalRrd = 0;

            foreach (string folder in _foundFolders)
            {
                try
                {
                    var files = Directory.GetFiles(folder, "*.pcl")
                        .Where(f => !Path.GetFileNameWithoutExtension(f).EndsWith("rr"))
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

            Console.WriteLine($"Загружено Pcl: {totalPcl}, PclRrd: {totalRrd}");
        }

        private void FindAllProtocolFolders()
        {
            // Поиск на диске U:\
            try
            {
                var found = Directory.GetDirectories(@"U:\", "PROTOCOLS", SearchOption.AllDirectories);
                _foundFolders.AddRange(found);
            }
            catch { }

            // Поиск на диске C:\
            try
            {
                var found = Directory.GetDirectories(@"C:\", "PROTOCOLS", SearchOption.AllDirectories);
                _foundFolders.AddRange(found);
            }
            catch { }

            // Добавляем явные пути (диск C и U)
            string[] knownPaths = {
                @"C:\KviNA\APPLICATIONS\Administration\Administration\bin\Debug\PROTOCOLS",
                @"C:\KviNA\ADDITIVE\CsProtocols\CsProtocols\bin\Debug\PROTOCOLS",
                @"C:\KviNA\ADDITIVE\csManual\csManual\bin\Debug\PROTOCOLS",
                @"U:\KviNA\APPLICATIONS\Administration\Administration\bin\Debug\PROTOCOLS",
                @"U:\KviNA\ADDITIVE\CsProtocols\CsProtocols\bin\Debug\PROTOCOLS",
                @"U:\KviNA\ADDITIVE\csManual\csManual\bin\Debug\PROTOCOLS"
            };

            foreach (string path in knownPaths)
            {
                if (Directory.Exists(path) && !_foundFolders.Contains(path))
                    _foundFolders.Add(path);
            }

            _foundFolders = _foundFolders.Distinct().ToList();
        }

        private int InsertPclFile(string filePath)
        {
            int count = 0;
            var lines = File.ReadAllLines(filePath);
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
            int count = 0;
            var lines = File.ReadAllLines(filePath);
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
                if (parts.Length < 5) continue;

                try
                {
                    string chg = parts[0].Trim();
                    string guid = parts[1].Trim();
                    string inkPcl = parts[2].Trim();
                    string rrdTyp = parts[3].Trim();
                    string err = parts[4].Trim();
                    string tck = parts.Length > 5 ? parts[5].Trim() : "-1";

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

            string checkQuery = $"SELECT CLU FROM App WHERE dsiApp = '{appName}'";
            var result = _dataSource.__mSqlValue(checkQuery);
            if (result != null && result != DBNull.Value)
                return Convert.ToInt32(result);

            string chg = DateTime.Now.Ticks.ToString();
            string guid = Guid.NewGuid().ToString();
            string insertQuery = $"INSERT INTO App (CHG, GID, ELD, cgzApp, dsiApp, Pfx) " +
                                 $"VALUES ('{chg}', '{guid}', 0, 0, '{appName}', '')";
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
            string query = $"INSERT INTO Pcl (CHG, GID, ELD, InkApp, InkPclTyp, InkUsr, Prc, Fil) " +
                           $"VALUES ('{chg}', '{newGuid}', 0, {appClu}, {pclTyp}, '{user}', '{prc}', '{fil}')";
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
            string query = $"INSERT INTO PclRrd (CHG, GID, ELD, InkPcl, InkPclRrdTyp, Err, Tck) " +
                           $"VALUES ('{chg}', '{newGuid}', 0, {inkPcl}, {rrdTyp}, '{err}', {tck})";
            _dataSource.__mSqlCommand(query);
        }
    }
}