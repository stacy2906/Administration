using nlDataSourceSqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace naCsProtocols
{
    /// <summary>
    /// Файл ProtocolSqliteImporter.cs
    /// </summary>
    /// <remarks>Импортирует легаси файлы протоколов ('.pcl', написанные файловым 'appProtocols' других
    /// приложений - Administration.exe, csManual.exe и т.д.) в SQLite базу данных 'dsqProtocols'.
    /// Формат файлов проверен напрямую по 'appProtocols.cs': заголовок протокола - CHG,GID,App,AppDpn,Pfx,Hst,
    /// HstAnt,lnkCpu,lnkPclTyp,lnkUsr,Prc,Fil (12 колонок); запись протокола (файл с суффиксом 'rrd' перед
    /// расширением) - CHG,GID,lnkPcl,lnkPclRrdTyp,Msg,Tck (6 колонок). Дедупликация - по 'GID' через таблицу
    /// 'ImportedGid' в самой базе данных, что позволяет безопасно запускать импорт повторно (новые дозаписи
    /// в тот же файл в течение дня не приведут к дублям).
    ///
    /// ВАЖНО (см. историю правок): изначально дедупликация и поиск CLU выполнялись отдельным SQL-запросом
    /// НА КАЖДУЮ СТРОКУ файла, без транзакции - для файла с несколькими тысячами строк это означало
    /// десятки тысяч отдельных открытий соединения с базой данных. Легаси-файлы протоколов, повреждённые
    /// более ранней ошибкой (когда сбой протоколирования сам порождал протокол о себе), разрастались до
    /// сотен тысяч строк, и попытка импортировать такой файл построчно приводила к "malloc() failed / out
    /// of memory" в самом SQLite. Теперь: (1) соответствие GID -&gt; CLU загружается ОДНИМ запросом перед
    /// циклом (см. 'dsqProtocols.__mImportedGidCluMapGet'), (2) весь файл импортируется в ОДНОЙ транзакции,
    /// (3) файлы с аномально большим числом строк пропускаются с предупреждением, а не импортируются целиком
    /// </remarks>
    /// <conception>Lucasin V.</conception>
    public class ProtocolSqliteImporter
    {
        #region = ПОЛЯ

        /// <summary>
        /// Максимальное количество строк в одном файле, которое считается допустимым для импорта.
        /// Легаси-файлы такого размера (сотни тысяч строк) практически никогда не бывают настоящими данными -
        /// это след ранее исправленной ошибки (см. примечание к классу). Такие файлы пропускаются, а не
        /// импортируются целиком, чтобы не подвесить импорт и не исчерпать память
        /// </summary>
        private const int cMaxLinesPerFile = 20000;

        #endregion ПОЛЯ

        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Импорт всех легаси '.pcl' файлов из указанной папки (и подпапок) в базу данных
        /// </summary>
        /// <param name="pProtocols">Экземпляр 'dsqProtocols', в базу данных которого выполняется импорт</param>
        /// <param name="pFolderPath">Папка для сканирования (обычно 'appApplication.__oPathes.__fDirectoryProtocols_')</param>
        /// <returns>Количество импортированных строк (протоколов + записей)</returns>
        public int __mImportFromFolder(dsqProtocols pProtocols, string pFolderPath)
        {
            int vImportedCount = 0;

            if (Directory.Exists(pFolderPath) == false)
                return 0;

            List<string> vHeaderFiles = Directory.GetFiles(pFolderPath, "*.pcl", SearchOption.AllDirectories)
                .Where(pFile => Path.GetFileNameWithoutExtension(pFile).EndsWith("rrd") == false)
                .ToList();

            if (vHeaderFiles.Count == 0)
                return 0;

            /// Соответствие GID -> CLU загружается ОДИН раз для всего запуска импорта (а не на каждую строку
            /// каждого файла - см. примечание к классу), и пополняется по мере импорта новых строк
            Dictionary<string, int> vImportedGidMap = pProtocols.__mImportedGidCluMapGet();

            foreach (string vHeaderFilePath in vHeaderFiles)
            {
                vImportedCount += mImportHeaderFile(pProtocols, vHeaderFilePath, vImportedGidMap);

                string vRecordFilePath = Path.Combine(
                    Path.GetDirectoryName(vHeaderFilePath),
                    Path.GetFileNameWithoutExtension(vHeaderFilePath) + "rrd" + Path.GetExtension(vHeaderFilePath));

                if (File.Exists(vRecordFilePath) == true)
                    vImportedCount += mImportRecordFile(pProtocols, vRecordFilePath, vImportedGidMap);
            }

            return vImportedCount;
        }
        /// <summary>
        /// Поиск папок 'PROTOCOLs' всех приложений решения, а не только текущего запущенного.
        /// </summary>
        /// <remarks>Каждое приложение решения пишет легаси файлы протоколов в СВОЮ СОБСТВЕННУЮ папку
        /// '&lt;приложение&gt;\PROTOCOLs\' (см. 'appPathes.__fDirectoryProtocols_': путь строится от
        /// 'Environment.CurrentDirectory' - то есть от папки, откуда запущен КОНКРЕТНЫЙ .exe, а не от
        /// общей папки решения). Поэтому 'cspApplication.__oPathes.__fDirectoryProtocols_' в 'cspBegin.cs'
        /// даёт только папку самого 'CsProtocols' - протоколы Administration.exe, csManual.exe и т.д.
        /// в неё не попадают. Этот метод поднимается от папки запуска текущего приложения на 3 уровня
        /// вверх ('bin\Debug' -&gt; '&lt;Приложение&gt;' -&gt; 'APPLICATIONs\Administration'), затем
        /// проверяет 'bin\Debug\PROTOCOLs' и 'bin\Release\PROTOCOLs' у каждого соседнего проекта
        /// приложения (пропуская папки, название которых начинается с '_' - это библиотеки, они
        /// самостоятельно не запускаются и протоколов не пишут)</remarks>
        /// <param name="pStartDirectory">Папка запуска текущего приложения (обычно 'Environment.CurrentDirectory' / 'bin\Debug')</param>
        /// <returns>Список найденных папок с файлами легаси протоколов (включая папку текущего приложения, если она существует)</returns>
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
            }
            catch
            {
                /// Обнаружение папок - вспомогательная операция при запуске; ошибка (например нет прав
                /// доступа к какой-то из папок) не должна мешать запуску приложения - возвращается то,
                /// что успело быть найдено до возникновения ошибки
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
                vLines = File.ReadAllLines(pFilePath);
            }
            catch
            {
                return 0; // Файл занят/недоступен - пропускаем, попробуем в следующий раз
            }

            if (vLines.Length > cMaxLinesPerFile)
            {
                mOversizedFileLog(pFilePath, vLines.Length);
                return 0; // Похоже на повреждённый/раздутый легаси-файл - не импортируется целиком (см. примечание к классу)
            }

            Dictionary<string, int> vAppCache = new Dictionary<string, int>(); // Кэш 'Имя приложения -> CLU' на время импорта этого файла

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
                        continue; // Не строка заголовка протокола (либо заголовок CSV, либо повреждённая строка)

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

            return vImportedCount;
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
                vLines = File.ReadAllLines(pFilePath);
            }
            catch
            {
                return 0;
            }

            if (vLines.Length > cMaxLinesPerFile)
            {
                mOversizedFileLog(pFilePath, vLines.Length);
                return 0; // См. примечание в 'mImportHeaderFile'
            }

            if (pProtocols.__mTransactionBegin() == false)
                return 0;

            try
            {
                foreach (string vLine in vLines)
                {
                    if (string.IsNullOrWhiteSpace(vLine) == true)
                        continue;

                    string[] vParts = vLine.Split(',');
                    if (vParts.Length < 6)
                        continue;

                    string vGid = vParts[1].Trim();
                    if (string.IsNullOrEmpty(vGid) == true || pImportedGidMap.ContainsKey(vGid) == true)
                        continue;

                    string vParentGid = vParts[2].Trim();
                    int vLnkPcl;
                    if (pImportedGidMap.TryGetValue(vParentGid, out vLnkPcl) == false || vLnkPcl < 0)
                        continue; // Родительский протокол ещё не импортирован (например, файл заголовка был пропущен/повреждён) - пропускаем запись, попробуем при следующем запуске

                    int vRecordTypeRaw;
                    int.TryParse(vParts[3].Trim(), out vRecordTypeRaw);

                    long vTick;
                    long.TryParse(vParts.Length > 5 ? vParts[5].Trim() : "-1", out vTick);

                    pProtocols.__mProtocolRecordImport(vGid, vLnkPcl, vRecordTypeRaw, vParts[4].Trim(), vTick);
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

        #endregion Процедуры

        #endregion МЕТОДЫ
    }
}