using nlDataSourceSqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace naCsProtocols
{
    /// <summary>
    /// Файл ProtocolSqliteImporter.cs
    /// </summary>
    /// <remarks>Импортирует легаси файлы протоколов ('.pcl', написанные файловым 'appProtocols' других
    /// приложений - Administration.exe, csManual.exe и т.д.) в SQLite базу данных 'dsqProtocols'
    /// </remarks>
    public class ProtocolSqliteImporter
    {
        #region = ПОЛЯ


        private const int cMaxLinesPerFile = 20000;

        #endregion ПОЛЯ

        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Импорт всех легаси '.pcl' файлов из указанной папки (и подпапок) в базу данных
        /// </summary>
        /// <param name="pProtocols">Экземпляр 'dsqProtocols', в базу данных которого выполняется импорт</param>
        /// <param name="pFolderPath">Папка для сканирования (обычно 'appApplication.__oPathes.__fDirectoryProtocols_')</param>
        /// <param name="pLog">
        /// Необязательный обратный вызов для подробной построчной диагностики (какая папка проверяется,
        /// сколько файлов найдено, что получилось для каждого файла). [null] - без диагностики, как раньше.
        /// </param>
        /// <returns>Количество импортированных строк (протоколов + записей)</returns>
        public int __mImportFromFolder(dsqProtocols pProtocols, string pFolderPath, Action<string> pLog = null)
        {
            int vImportedCount = 0;

            if (Directory.Exists(pFolderPath) == false)
            {
                pLog?.Invoke("  папка не существует - пропущена");
                return 0;
            }

            List<string> vHeaderFiles = Directory.GetFiles(pFolderPath, "*.pcl", SearchOption.AllDirectories)
                .Where(pFile => Path.GetFileNameWithoutExtension(pFile).EndsWith("rrd") == false)
                .ToList();

            pLog?.Invoke("  найдено файлов-заголовков ('*.pcl', без 'rrd'): " + vHeaderFiles.Count.ToString());

            if (vHeaderFiles.Count == 0)
                return 0;

            Dictionary<string, int> vImportedGidMap = pProtocols.__mImportedGidCluMapGet();
            pLog?.Invoke("  уже импортировано ранее (всего по базе, GID): " + vImportedGidMap.Count.ToString());

            foreach (string vHeaderFilePath in vHeaderFiles)
            {
                int vFromHeader = mImportHeaderFile(pProtocols, vHeaderFilePath, vImportedGidMap);

                string vRecordFilePath = Path.Combine(
                    Path.GetDirectoryName(vHeaderFilePath),
                    Path.GetFileNameWithoutExtension(vHeaderFilePath) + "rrd" + Path.GetExtension(vHeaderFilePath));

                int vFromRecords = 0;
                if (File.Exists(vRecordFilePath) == true)
                    vFromRecords = mImportRecordFile(pProtocols, vRecordFilePath, vImportedGidMap);

                pLog?.Invoke("    " + Path.GetFileName(vHeaderFilePath) + " -> строк: " + vFromHeader.ToString()
                    + (File.Exists(vRecordFilePath) == true ? " (+ записи: " + vFromRecords.ToString() + ")" : " (файла записей нет)"));

                vImportedCount += vFromHeader + vFromRecords;
            }

            return vImportedCount;
        }
        /// <summary>
        /// Поиск папок 'PROTOCOLs' всех приложений решения, а не только текущего запущенного.
        /// </summary>
        /// <param name="pStartDirectory">Папка, где лежит исполняемый файл текущего приложения</param>
      public static List<string> __mProtocolsFoldersDiscover(string pStartDirectory)
        {
            List<string> vReturn = new List<string>();

            try
            {
                DirectoryInfo vDirectory = new DirectoryInfo(pStartDirectory);

                for (int i = 0; i < 3 && vDirectory != null && vDirectory.Parent != null; i++)
                    vDirectory = vDirectory.Parent;

                if (vDirectory == null || vDirectory.Exists == false)
                    return vReturn;

                foreach (DirectoryInfo vAppDirectory in vDirectory.GetDirectories())
                {
                    if (vAppDirectory.Name.StartsWith("_") == true)
                        continue; // Библиотечный проект - самостоятельно не запускается, протоколов не пишет

                    // Варианты путей: bin\Debug|Release\PROTOCOLs|Protocols и корень приложения Protocols|PROTOCOLs
                    string[] vCandidates = new string[]
                    {
                        Path.Combine(vAppDirectory.FullName, @"bin\Debug\PROTOCOLs"),
                        Path.Combine(vAppDirectory.FullName, @"bin\Debug\Protocols"),
                        Path.Combine(vAppDirectory.FullName, @"bin\Release\PROTOCOLs"),
                        Path.Combine(vAppDirectory.FullName, @"bin\Release\Protocols"),
                        Path.Combine(vAppDirectory.FullName, "PROTOCOLs"),
                        Path.Combine(vAppDirectory.FullName, "Protocols"),
                    };

                    foreach (string vPath in vCandidates)
                    {
                        if (Directory.Exists(vPath) == true && vReturn.Contains(vPath) == false)
                            vReturn.Add(vPath);
                    }
                }

               
                DirectoryInfo vSolutionRoot = vDirectory;
                for (int i = 0; i < 2 && vSolutionRoot != null && vSolutionRoot.Parent != null; i++)
                    vSolutionRoot = vSolutionRoot.Parent;

                if (vSolutionRoot != null && vSolutionRoot.Exists == true)
                {
                    DirectoryInfo vReleaseDirectory = new DirectoryInfo(Path.Combine(vSolutionRoot.FullName, "RELEASE"));
                    if (vReleaseDirectory.Exists == true)
                    {
                        foreach (DirectoryInfo vReleasedApp in vReleaseDirectory.GetDirectories())
                        {
                            string[] vReleaseCandidates = new string[]
                            {
                                Path.Combine(vReleasedApp.FullName, "PROTOCOLs"),
                                Path.Combine(vReleasedApp.FullName, "Protocols"),
                            };
                            foreach (string vPath in vReleaseCandidates)
                            {
                                if (Directory.Exists(vPath) == true && vReturn.Contains(vPath) == false)
                                    vReturn.Add(vPath);
                            }
                        }
                    }
                }
            }
            catch
            {
               
            }

            return vReturn;
        }
        /// <summary>
        /// Импорт заголовков протоколов из одного '.pcl' файла (одной транзакцией)
        /// </summary>
        private int mImportHeaderFile(dsqProtocols pProtocols, string pFilePath, Dictionary<string, int> pImportedGidMap)
        {
            int vImportedCount = 0;
            string[] vLines;

            try
            {
         
                vLines = File.ReadAllLines(pFilePath, Encoding.GetEncoding(1251));
            }
            catch
            {
                return 0; // Файл занят/недоступен - пропускаем, попробуем в следующий раз
            }

            if (vLines.Length > cMaxLinesPerFile)
            {
                mOversizedFileLog(pFilePath, vLines.Length);
                return 0; 
            }

            Dictionary<string, int> vAppCache = new Dictionary<string, int>(); 
            int vRecognizedLines = 0;

            if (pProtocols.__mTransactionBegin() == false)
                return 0;

            try
            {
                foreach (string vLine in vLines)
                {
                    if (string.IsNullOrWhiteSpace(vLine) == true)
                        continue;

                    string[] vParts = vLine.Split(',');
                    if (vParts.Length < 12)
                        continue;

                    vRecognizedLines++;

                    string vGid = vParts[1].Trim();
                    if (string.IsNullOrEmpty(vGid) == true || pImportedGidMap.ContainsKey(vGid) == true)
                        continue;

                    long vChgTicks;
                    if (long.TryParse(vParts[0].Trim(), out vChgTicks) == false)
                        continue;

                    int vProtocolTypeRaw;
                    int.TryParse(vParts[8].Trim(), out vProtocolTypeRaw);

                    int vClu = pProtocols.__mProtocolImport(
                        vGid,
                        vChgTicks,
                        vParts[2].Trim(),  // App
                        vParts[3].Trim(),  // AppDpn
                        vParts[4].Trim(),  // Pfx
                        vProtocolTypeRaw,
                        vParts[5].Trim(),  // Hst
                        vParts[10].Trim(), // Prc
                        vParts[6].Trim()); // HstAnt (аккаунт хоста - используется как пользователь, см. appProtocols.cs)

                    pProtocols.__mProtocolMarkImported(vGid, vClu);
                    pImportedGidMap[vGid] = vClu; // Пополнение общей карты - последующие файлы этого же запуска увидят актуальное состояние
                    vImportedCount++;
                }

                pProtocols.__mTransactionCommit();
            }
            catch
            {
                pProtocols.__mTransactionRollback();
                throw;
            }

          
            if (vRecognizedLines == 0 && vLines.Any(pLine => string.IsNullOrWhiteSpace(pLine) == false))
            {
                int vLegacyImported = mImportLegacyBracketFile(pProtocols, vLines, pImportedGidMap);
                if (vLegacyImported > 0)
                    return vImportedCount + vLegacyImported;

                bool vLooksBracket = vLines.Any(pLine =>
                {
                    string t = (pLine ?? "").Trim();
                    return t.Length > 2 && t[0] == '[' && t.IndexOf(']') > 0;
                });
                if (vLooksBracket == true)
                    return vImportedCount; 

                return vImportedCount + mImportRawFallback(pProtocols, pFilePath, vLines, pImportedGidMap);
            }

            return vImportedCount;
        }

        private int mImportLegacyBracketFile(dsqProtocols pProtocols, string[] pLines, Dictionary<string, int> pImportedGidMap)
        {
           
            int vImportedCount = 0;

            if (pProtocols.__mTransactionBegin() == false)
                return 0;

            try
            {
                string vCurrentHeaderGid = null;
                int vCurrentPclClue = -1;
                int vRecordIndex = 0;

                foreach (string vRawLine in pLines)
                {
                    string vLine = (vRawLine ?? "").Trim();
                    if (vLine.Length == 0)
                        continue;

                    List<string> vBrackets = mSplitBrackets(vLine);
                    if (vBrackets == null || vBrackets.Count == 0)
                        continue;

                   
                    int vRecType;
                    if (vBrackets.Count >= 1
                        && int.TryParse(vBrackets[0], out vRecType)
                        && vRecType >= 0 && vRecType <= 20
                        && vBrackets[0].Length <= 3
                        && vLine.IndexOf(" - ") >= 0)
                    {
                        if (vCurrentHeaderGid == null || vCurrentPclClue <= 0)
                            continue;

                        vRecordIndex++;
                        string vRecordGid = vCurrentHeaderGid + "_R" + vRecordIndex.ToString();
                        if (pImportedGidMap.ContainsKey(vRecordGid) == true)
                            continue;

                        long vTick = -1;
                        if (vBrackets.Count >= 2)
                            long.TryParse(vBrackets[1], out vTick);

                        int vDash = vLine.IndexOf(" - ");
                        string vMessage = vDash >= 0 ? vLine.Substring(vDash + 3).Trim() : vLine;

 
                        pProtocols.__mProtocolRecordImport(vRecordGid, vCurrentPclClue, vRecType, vMessage, vTick);
                        pProtocols.__mProtocolMarkImported(vRecordGid, -1);
                        pImportedGidMap[vRecordGid] = -1;
                        vImportedCount++;
                        continue;
                    }

        
                    long vChgTicks;
                    if (vBrackets.Count >= 8
                        && long.TryParse(vBrackets[0], out vChgTicks)
                        && vBrackets[0].Length >= 10)
                    {
                        string vGid = "LEGACY_" + vBrackets[0];
                        vCurrentHeaderGid = vGid;
                        vRecordIndex = 0;

                        int vExistingClue;
                        if (pImportedGidMap.TryGetValue(vGid, out vExistingClue) == true)
                        {
                            vCurrentPclClue = vExistingClue;
                            continue;
                        }

                        string vApp = vBrackets.Count > 2 ? vBrackets[2] : "";
                        string vPfx = vBrackets.Count > 3 ? vBrackets[3] : "";
                        string vHost = vBrackets.Count > 5 ? vBrackets[5] : "";
                        string vUser = vBrackets.Count > 6 ? vBrackets[6] : "";
                        int vProtocolTypeRaw = 12;
                        if (vBrackets.Count > 7)
                            int.TryParse(vBrackets[7], out vProtocolTypeRaw);
                
                        string vPrc = vBrackets.Count > 9 ? vBrackets[9] : "";

                        vCurrentPclClue = pProtocols.__mProtocolImport(
                            vGid, vChgTicks,
                            vApp, "", vPfx,
                            vProtocolTypeRaw,
                            vHost, vPrc, vUser);

                        pProtocols.__mProtocolMarkImported(vGid, vCurrentPclClue);
                        pImportedGidMap[vGid] = vCurrentPclClue;
                        vImportedCount++;
                        continue;
                    }
                }

                pProtocols.__mTransactionCommit();
            }
            catch
            {
                pProtocols.__mTransactionRollback();
                throw;
            }

            return vImportedCount;
        }

        /// <summary>
        /// Разбивка строки вида [a][b][c] на список значений скобок (без самих скобок).
        /// </summary>
        private List<string> mSplitBrackets(string pLine)
        {
            List<string> vResult = new List<string>();
            if (string.IsNullOrEmpty(pLine) || pLine[0] != '[')
                return vResult;

            int vStart = -1;
            for (int i = 0; i < pLine.Length; i++)
            {
                if (pLine[i] == '[')
                    vStart = i + 1;
                else if (pLine[i] == ']' && vStart >= 0)
                {
                    vResult.Add(pLine.Substring(vStart, i - vStart));
                    vStart = -1;
                    // после ] может идти " - message" — дальше скобок заголовка нет
                    if (i + 1 < pLine.Length && pLine[i + 1] != '[')
                        break;
                }
            }
            return vResult;
        }

        private int mImportRawFallback(dsqProtocols pProtocols, string pFilePath, string[] pLines, Dictionary<string, int> pImportedGidMap)
        {
            int vImportedCount = 0;

            /// Один заголовок на файл, с детерминированным GID (от пути файла) - повторный импорт того
            /// же файла не создаст второй заголовок, а лишь пропустит уже сохранённые строки (обычная
            /// дедупликация по GID, как и во всех остальных путях импорта)
            string vHeaderGid = "RAWFILE_" + mStableHash(pFilePath);

            int vHeaderClue;
            bool vHeaderIsNew = pImportedGidMap.TryGetValue(vHeaderGid, out vHeaderClue) == false;

            if (pProtocols.__mTransactionBegin() == false)
            {
                mUnrecognizedFormatLog(pFilePath); 
                return 0;
            }

            try
            {
                if (vHeaderIsNew == true)
                {
                    vHeaderClue = pProtocols.__mProtocolImport(
                        vHeaderGid,
                        DateTime.Now.Ticks, // Точное время создания файла для нераспознанного формата не определить - используется момент импорта
                        Path.GetFileNameWithoutExtension(pFilePath), // App - имя файла, чтобы можно было отличить источник в списке протоколов
                        "", "",
                        12,
                        Environment.MachineName,
                        "Импорт нераспознанного файла: " + Path.GetFileName(pFilePath),
                        "");

                    pProtocols.__mProtocolMarkImported(vHeaderGid, vHeaderClue);
                    pImportedGidMap[vHeaderGid] = vHeaderClue;
                    vImportedCount++;
                }

                int vLineIndex = 0;
                foreach (string vLine in pLines)
                {
                    vLineIndex++;
                    if (string.IsNullOrWhiteSpace(vLine) == true)
                        continue;

                    
                    string vLineGid = vHeaderGid + "_L" + vLineIndex.ToString() + "_" + mStableHash(vLine);
                    if (pImportedGidMap.ContainsKey(vLineGid) == true)
                        continue;

                    pProtocols.__mProtocolRecordImport(vLineGid, vHeaderClue, 5 /* PclRrdTyp.CLU 5 = 'Сообщение' */, vLine, -1);
                    pProtocols.__mProtocolMarkImported(vLineGid, -1);
                    pImportedGidMap[vLineGid] = -1;
                    vImportedCount++;
                }

                pProtocols.__mTransactionCommit();
            }
            catch
            {
                pProtocols.__mTransactionRollback();
                throw;
            }

            mUnrecognizedFormatLog(pFilePath); // Информационная запись: формат не распознан автоматически, но содержимое всё равно полностью сохранено выше

            return vImportedCount;
        }
        /// <summary>
        /// Детерминированный, стабильный между запусками короткий хеш строки - используется для построения
        /// GID у строк без собственного идентификатора (нераспознанный формат), чтобы дедупликация по GID
        /// работала одинаково при повторном импорте одного и того же файла
        /// </summary>
        private string mStableHash(string pText)
        {
            using (SHA1 vSha = SHA1.Create())
            {
                byte[] vHash = vSha.ComputeHash(Encoding.UTF8.GetBytes(pText ?? ""));
                return BitConverter.ToString(vHash).Replace("-", "").Substring(0, 16);
            }
        }
        /// <summary>
        /// Импорт записей протоколов из одного 'rrd.pcl' файла (одной транзакцией)
        /// </summary>
        private int mImportRecordFile(dsqProtocols pProtocols, string pFilePath, Dictionary<string, int> pImportedGidMap)
        {
            int vImportedCount = 0;
            string[] vLines;

            try
            {
                vLines = File.ReadAllLines(pFilePath, Encoding.GetEncoding(1251));
            }
            catch
            {
                return 0;
            }

            if (vLines.Length > cMaxLinesPerFile)
            {
                mOversizedFileLog(pFilePath, vLines.Length);
                return 0;
            }

            if (pProtocols.__mTransactionBegin() == false)
                return 0;

            int vRecognizedLines = 0;

            try
            {
                foreach (string vLine in vLines)
                {
                    if (string.IsNullOrWhiteSpace(vLine) == true)
                        continue;

                    string[] vParts = vLine.Split(',');
                    if (vParts.Length < 6)
                        continue;

                    vRecognizedLines++;

                    string vGid = vParts[1].Trim();
                    if (string.IsNullOrEmpty(vGid) == true || pImportedGidMap.ContainsKey(vGid) == true)
                        continue;

                    string vParentGid = vParts[2].Trim();
                    int vLnkPcl;
                    if (pImportedGidMap.TryGetValue(vParentGid, out vLnkPcl) == false || vLnkPcl < 0)
                        continue; // Родительский протокол ещё не импортирован (например, файл заголовка был пропущен/повреждён) - пропускаем запись, попробуем при следующем запуске

                    int vRecordTypeRaw;
                    int.TryParse(vParts[3].Trim(), out vRecordTypeRaw);

                  
                    int vRecordTypeClu = vRecordTypeRaw + 1;

                    long vTick;
                    long.TryParse(vParts.Length > 5 ? vParts[5].Trim() : "-1", out vTick);

                    pProtocols.__mProtocolRecordImport(vGid, vLnkPcl, vRecordTypeClu, vParts[4].Trim(), vTick);
                    pProtocols.__mProtocolMarkImported(vGid, -1); // -1: маркер "это запись, а не протокол" (для протоколов здесь настоящий CLU)
                    pImportedGidMap[vGid] = -1;
                    vImportedCount++;
                }

                pProtocols.__mTransactionCommit();
            }
            catch
            {
                pProtocols.__mTransactionRollback();
                throw;
            }

            if (vRecognizedLines == 0 && vLines.Any(pLine => string.IsNullOrWhiteSpace(pLine) == false))
                return vImportedCount + mImportRawFallback(pProtocols, pFilePath, vLines, pImportedGidMap);

            return vImportedCount;
        }
        /// <summary>
        /// Регистрация факта пропуска аномально большого файла (без обращения к общему обработчику ошибок
        /// приложения - см. примечание к 'dsqDataSourceSqliteWithProtocol' о риске рекурсии через протоколирование)
        /// </summary>
        private void mOversizedFileLog(string pFilePath, int pLinesCount)
        {
            try
            {
                string vLine = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    + " | Файл пропущен (" + pLinesCount + " строк, лимит " + cMaxLinesPerFile + ") - похоже на повреждённый легаси-файл: "
                    + pFilePath + Environment.NewLine;

                File.AppendAllText(Path.Combine(Path.GetDirectoryName(pFilePath), "protocols_import_skipped.log"), vLine);
            }
            catch
            {
                /// Запись предупреждения не должна иметь права сорвать импорт остальных файлов
            }
        }
        /// <summary>
        /// Регистрация файла, содержимое которого не удалось разобрать ни в одной строке - непустой файл,
        /// но ни одна строка не совпала с ожидаемым CSV-форматом ('CHG,GID,...' с запятыми).
        /// </summary>
        private void mUnrecognizedFormatLog(string pFilePath)
        {
            try
            {
                string vLine = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    + " | Формат файла не распознан (ни одна строка не похожа на ожидаемый CSV) - пропущен: "
                    + pFilePath + Environment.NewLine;

                File.AppendAllText(Path.Combine(Path.GetDirectoryName(pFilePath), "protocols_import_skipped.log"), vLine);
            }
            catch
            {
                /// Запись предупреждения не должна иметь права сорвать импорт остальных файлов
            }
        }

        #endregion Процедуры

        #endregion МЕТОДЫ
    }
}