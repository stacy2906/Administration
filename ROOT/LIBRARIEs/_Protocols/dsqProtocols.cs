using nlApplication;
using nlData;
using System;

namespace nlDataSourceSqlite
{
    /// <summary>
    /// Файл dsqProtocols.cs
    /// </summary>
    /// <remarks>SQLite-реализация протоколирования ('appApplication.__oProtocols'). Пишет напрямую в базу
    /// данных вместо файлов '.pcl'.</remarks>
    /// <fixed>ПРИВЕДЕНО К ПОДТВЕРЖДЁННОЙ ЭТАЛОННОЙ СХЕМЕ (файл 'Protocols.db', сверено построчно по
    /// фактическому 'sqlite_master'): 'App' (CHG,CLU,ELD,GID,cgzApp,dsiApp,Pfx), 'Pcl' (CHG,CLU,ELD,GID,
    /// lnkApp,lnkCpu,lnkPclTyp,lnkUsr,Prc,Fil), 'PclTyp'/'PclRrdTyp' - только (CHG,CLU,ELD,GID,cgz*,dsi*),
    /// БЕЗ столбцов-флагов 'opt*', 'PclRrd' (CHG,CLU,ELD,GID,lnkPcl,lnkPclRrdTyp,Err,Tck) - столбец
    /// сообщения называется 'Err', не 'Msg'. ПОДТВЕРЖДЕНО ОТДЕЛЬНО: в этой схеме НЕТ таблиц 'Cpu'/'Usr' -
    /// 'Pcl.lnkCpu'/'Pcl.lnkUsr' это внешние ключи без цели внутри этой базы; см. пометку 'ВНИМАНИЕ' у
    /// 'mComputerEnsure'/'mUserEnsure' - разрешение имени компьютера/пользователя пока не реализовано</fixed>
    /// <conception>Lucasin V.</conception>
    public class dsqProtocols : appProtocols
    {
        #region = ДИЗАЙНЕРЫ

        public dsqProtocols()
        {
            _mObjectAssembly();
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Объект

        /// <fixed>Раньше три разных места ('cspFormMain', 'cspFormLoader', 'cspFormCombinedViewer') сами
        /// делали 'cspApplication.__oProtocols as dsqProtocols' и сами решали, что делать при [null] -
        /// в 'cspFormLoader' это была видимая строка в логе, в 'cspFormCombinedViewer' - тихо пустая
        /// таблица без всякого объяснения. Раз причина одна ('appApplication.__oProtocols' по умолчанию
        /// это 'appProtocols', а не 'dsqProtocols', и что-то - обычно 'cspBegin.Main()' - должно успеть
        /// его переопределить до того как форма делает приведение типов), решение тоже должно быть одно -
        /// здесь, а не в каждой форме. Это ещё и защищает от значения по умолчанию для ЛЮБОЙ будущей точки
        /// входа/тестового запуска, которая забудет сделать переопределение в своём 'Main()' - вместо
        /// молчаливого [null] это самовосстанавливающееся свойство просто создаст 'dsqProtocols' само</fixed>
        /// <summary>
        /// Активный экземпляр 'dsqProtocols'. Приводит 'appApplication.__oProtocols' к этому типу; если
        /// он ещё не был переопределён (равен базовому 'appProtocols' по умолчанию), создаёт и
        /// устанавливает 'dsqProtocols' сам - возвращаемое значение никогда не бывает [null]
        /// </summary>
        public static dsqProtocols __oActive_
        {
            get
            {
                dsqProtocols vProtocols = appApplication.__oProtocols as dsqProtocols;
                if (vProtocols == null)
                {
                    vProtocols = new dsqProtocols();
                    appApplication.__oProtocols = vProtocols;
                }
                return vProtocols;
            }
        }
        /// <fixed>ДОБАВЛЕНО: раньше 'cspFormCombinedViewer' сам читал 'dsqProtocols.__oActive_' - то есть
        /// ВСЕГДА показывал "родную" базу приложения ('Databases\protocols.db'), независимо от того, что
        /// пользователь вручную открыл через "Файл / Открыть" в 'cspFormMain' - две формы показывали разные
        /// базы данных без какой-либо связи между собой. '__oActive_' для этого не годится: он специально
        /// самовосстанавливающийся и всегда указывает на "родную" базу - смешивать в нём же "то, что сейчас
        /// открыто для просмотра" означало бы либо сломать самовосстановление, либо запутать оба смысла.
        /// Это ОТДЕЛЬНОЕ свойство: "какая база сейчас выбрана для просмотра в интерфейсе", не привязанное
        /// к типу 'dsqProtocols' (открытый вручную файл - это 'dsqDataSourceSqliteWithProtocol', не
        /// 'dsqProtocols'), по умолчанию [null] - НИЧЕГО не показывается, пока пользователь сам не откроет
        /// базу. 'cspFormMain' обновляет его при "Файл / Открыть" и "Файл / Закрыть"; 'cspFormCombinedViewer'
        /// только читает</fixed>
        /// <summary>
        /// База данных, выбранная пользователем для просмотра в интерфейсе (через "Файл / Открыть протокол"
        /// в 'cspFormMain'). [null] - ничего не открыто, формы просмотра должны показывать пустые таблицы
        /// </summary>
        public static nlData.datUnitDataSource __oViewing_ = null;

        protected void _mObjectAssembly()
        {
            if (__fDatabasePath.Length == 0)
                __fDatabasePath = appApplication.__oPathes.__fDirectoryDatabases_;

            oDataSourceSqlite.__fDatabaseName = "protocols.db";
            oDataSourceSqlite.__fDatabasePath = __fDatabasePath;

            __mTablesFill();

            return;
        }

        #endregion Объект

        #region - Процедуры

        public override void __mCreate(PROTOCOLSTYPES pProtocolType, string pProcedure, bool pPrintScreen = false)
        {
            int vApplicationClue = mApplicationEnsure(datApplication.__fProcessName_, datApplication.__fDescription_, datApplication.__fPrefix_);
            int vCpuClue = mComputerEnsure(Environment.MachineName);
            int vUsrClue = mUserEnsure(Environment.UserName);
            int vProtocolTypClue = mProtocolTypeClue(pProtocolType);
            string vDateTimeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string vPrintScreenFile = pPrintScreen == true ? __mPrintScreen() : "";

            string vCommand = "Insert Into Pcl (CHG, GID, lnkApp, lnkCpu, lnkPclTyp, lnkUsr, Prc, Fil)"
                              + " Values("
                              + "'" + vDateTimeNow + "'"
                              + ", '" + Guid.NewGuid().ToString() + "'"
                              + ", " + vApplicationClue.ToString()
                              + ", " + vCpuClue.ToString()
                              + ", " + vProtocolTypClue.ToString()
                              + ", " + vUsrClue.ToString()
                              + ", '" + pProcedure.Replace("'", "''") + "'"
                              + ", " + (string.IsNullOrEmpty(vPrintScreenFile) == true ? "NULL" : "'" + vPrintScreenFile.Replace("'", "''") + "'") + ")";

            oDataSourceSqlite.__mSqlCommand(vCommand);
            fProtocolClue = oDataSourceSqlite.__mClueLastInserted("Pcl");
        }
        public override void __mRecord(PROTOCOLRECORDSTYPES pRecordType, string pRecordText, long pTick = -1)
        {
            string vCommand = "Insert Into PclRrd (CHG, GID, lnkPcl, lnkPclRrdTyp, Err, Tck)"
                              + " Values("
                              + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                              + ", '" + Guid.NewGuid().ToString() + "'"
                              + ", " + fProtocolClue.ToString()
                              + ", " + mRecordTypeClue(pRecordType).ToString()
                              + ", '" + (pRecordText ?? "").Replace("'", "''") + "'"
                              + ", " + pTick.ToString() + ")";
            oDataSourceSqlite.__mSqlCommand(vCommand);
        }
        /// <summary>
        /// Соответствие вида протокола (enum 'PROTOCOLSTYPES') реальному 'CLU' строки в таблице 'PclTyp'.
        /// </summary>
        /// <remarks>Строки в 'PclTyp' сеются в '__mTablesFill' СТРОГО в этом порядке (одна 'Insert' на
        /// значение enum, от 'ApplicationError' до 'Other'), и 'cgzPclTyp' у каждой строки явно
        /// установлен равным тому же числу - поэтому 'CLU' построчно совпадает со значением enum</remarks>
        /// <param name="pProtocolType">Вид протокола</param>
        /// <returns>Реальный CLU строки в 'PclTyp'</returns>
        private int mProtocolTypeClue(PROTOCOLSTYPES pProtocolType)
        {
            switch (pProtocolType)
            {
                case PROTOCOLSTYPES.ApplicationError: return 1;
                case PROTOCOLSTYPES.ApplicationErrorProgramatic: return 2;
                case PROTOCOLSTYPES.ApplicationException: return 3;
                case PROTOCOLSTYPES.ApplicationEvent: return 4;
                case PROTOCOLSTYPES.DataError: return 5;
                case PROTOCOLSTYPES.DataEvent: return 6;
                case PROTOCOLSTYPES.DeviceError: return 7;
                case PROTOCOLSTYPES.DeviceEvent: return 8;
                case PROTOCOLSTYPES.UserError: return 9;
                case PROTOCOLSTYPES.UserEvent: return 10;
                case PROTOCOLSTYPES.UserMessage: return 11;
                case PROTOCOLSTYPES.Other: return 12;
                default: return 1;
            }
        }
        /// <summary>
        /// Соответствие вида записи протокола (enum 'PROTOCOLRECORDSTYPES') реальному 'CLU' строки в таблице 'PclRrdTyp'.
        /// </summary>
        /// <remarks>Строки в 'PclRrdTyp' сеются в этом же порядке; седьмая ('Причина', CLU=7) не имеет
        /// соответствия в enum 'PROTOCOLRECORDSTYPES' - зарезервирована на будущее, из enum пока недостижима</remarks>
        /// <param name="pRecordType">Вид записи протокола</param>
        /// <returns>Реальный CLU строки в 'PclRrdTyp'</returns>
        private int mRecordTypeClue(PROTOCOLRECORDSTYPES pRecordType)
        {
            switch (pRecordType)
            {
                case PROTOCOLRECORDSTYPES.Answer: return 1;
                case PROTOCOLRECORDSTYPES.Detail: return 2;
                case PROTOCOLRECORDSTYPES.Exception: return 3;
                case PROTOCOLRECORDSTYPES.Image: return 4;
                case PROTOCOLRECORDSTYPES.Message: return 5;
                case PROTOCOLRECORDSTYPES.ObjectProperty: return 6;
                case PROTOCOLRECORDSTYPES.Reason: return 7;
                default: return 5;
            }
        }

        public void __mTablesFill()
        {
            oDataSourceSqlite.__mDatabaseCreate();

            /// Самовосстановление: более старая версия этого класса создавала столбцы связей с опечаткой
            /// ('InkApp'/'InkPclTyp'/'InkPcl'/'InkPclRrdTyp' - заглавная 'I' вместо строчной 'l' в 'lnk').
            /// Поскольку таблицы создаются только "если их ещё нет", уже созданная с опечаткой база
            /// никогда не исправилась бы сама по себе, а весь текущий код (включая фильтры) обращается
            /// к правильным именам 'lnk*' - без этой миграции реальная база с накопленными данными
            /// (например 'CsProtocols\bin\Debug\Databases\protocols.db') падала бы на каждом запросе
            /// с ошибкой "no such column: lnkApp"
            mColumnRenameIfExists("Pcl", "InkApp", "lnkApp");
            mColumnRenameIfExists("Pcl", "InkPclTyp", "lnkPclTyp");
            mColumnRenameIfExists("PclRrd", "InkPcl", "lnkPcl");
            mColumnRenameIfExists("PclRrd", "InkPclRrdTyp", "lnkPclRrdTyp");
            /// <fixed>ДОБАВЛЕНО: ещё более старая версия этого класса писала текст сообщения в столбец
            /// 'Msg' - подтверждённая эталонная схема ('Protocols.db', см. обсуждение) называет этот
            /// столбец 'Err'. Та же причина, что и для опечатки 'Ink*' выше: без миграции уже
            /// накопленная локальная база осталась бы навсегда с 'Msg' и падала бы на "no such column: Err"</fixed>
            mColumnRenameIfExists("PclRrd", "Msg", "Err");

            /// Таблица 'App' - Приложения (реальные столбцы: 'admEssenceApp.__mRecordNew')
            if (oDataSourceSqlite.__mTableExists("App") == false)
            {
                string vQuery = "CREATE TABLE App ("
                                + "CHG TEXT,"
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "ELD INTEGER DEFAULT 0,"
                                + "GID TEXT,"
                                + "cgzApp INTEGER,"
                                + "dsiApp TEXT,"
                                + "Pfx TEXT,"
                                + "PRIMARY KEY('CLU' AUTOINCREMENT))";
                oDataSourceSqlite.__mSqlCommand(vQuery);
            }
            mApplicationEnsure(datApplication.__fProcessName_, datApplication.__fDescription_, datApplication.__fPrefix_);

            /// <fixed>УДАЛЕНО: таблицы 'Cpu' и 'Usr' здесь больше НЕ создаются. Подтверждённая эталонная
            /// схема ('Protocols.db') не содержит таких таблиц вообще - 'Pcl.lnkCpu'/'Pcl.lnkUsr' в ней
            /// это внешние ключи без локальной таблицы-цели внутри этой базы (см. примечание к
            /// 'mComputerEnsure'/'mUserEnsure' ниже - реальное разрешение имени компьютера/пользователя
            /// пока не определено и требует уточнения)</fixed>

            /// Таблица 'Pcl' - Протоколы (реальные столбцы - сверено с подтверждённой эталонной 'Protocols.db')
            if (oDataSourceSqlite.__mTableExists("Pcl") == false)
            {
                string vQuery = "CREATE TABLE Pcl ("
                                + "CHG INTEGER NOT NULL,"
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "ELD INTEGER NOT NULL DEFAULT 0,"
                                + "GID TEXT NOT NULL,"
                                + "lnkApp INTEGER NOT NULL,"
                                + "lnkCpu INTEGER NOT NULL,"
                                + "lnkPclTyp INTEGER NOT NULL,"
                                + "lnkUsr INTEGER NOT NULL,"
                                + "Prc TEXT NOT NULL,"
                                + "Fil INTEGER,"
                                + "PRIMARY KEY('CLU' AUTOINCREMENT))";
                oDataSourceSqlite.__mSqlCommand(vQuery);
            }

            /// Таблица 'PclTyp' - Виды протоколов (упрощено до подтверждённой эталонной схемы: только
            /// 'cgzPclTyp'/'dsiPclTyp' - никаких столбцов-флагов 'opt*', которых в 'Protocols.db' нет)
            if (oDataSourceSqlite.__mTableExists("PclTyp") == false)
            {
                string vQuery = "CREATE TABLE PclTyp ("
                                + "CHG TEXT,"
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "ELD INTEGER DEFAULT 0,"
                                + "GID TEXT,"
                                + "cgzPclTyp INTEGER,"
                                + "dsiPclTyp TEXT,"
                                + "PRIMARY KEY('CLU' AUTOINCREMENT))";
                oDataSourceSqlite.__mSqlCommand(vQuery);

                /// Засеяно СТРОГО в порядке значений enum 'PROTOCOLSTYPES' (1..12), чтобы CLU совпадал со
                /// значением enum - см. примечание к 'mProtocolTypeClue'. 'cgzPclTyp' также равен тому же
                /// числу явно (а не оставлен пустым), т.к. флаговых столбцов для различения видов больше нет
                mPclTypSeed(1, "Ошибка приложения");
                mPclTypSeed(2, "Ошибка программирования");
                mPclTypSeed(3, "Исключение");
                mPclTypSeed(4, "Событие приложения");
                mPclTypSeed(5, "Ошибка источника данных");
                mPclTypSeed(6, "Событие источника данных");
                mPclTypSeed(7, "Ошибка устройства");
                mPclTypSeed(8, "Событие устройства");
                mPclTypSeed(9, "Ошибка пользователя");
                mPclTypSeed(10, "Событие пользователя");
                mPclTypSeed(11, "Сообщение пользователю");
                mPclTypSeed(12, "Прочее");
            }

            /// Таблица 'PclRrd' - Записи протокола (столбец сообщения - 'Err', подтверждено эталонной схемой)
            if (oDataSourceSqlite.__mTableExists("PclRrd") == false)
            {
                string vQuery = "CREATE TABLE PclRrd ("
                                + "CHG INTEGER NOT NULL,"
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "ELD INTEGER NOT NULL DEFAULT 0,"
                                + "GID INTEGER NOT NULL,"
                                + "lnkPcl INTEGER NOT NULL,"
                                + "lnkPclRrdTyp INTEGER NOT NULL,"
                                + "Err TEXT NOT NULL,"
                                + "Tck INTEGER NOT NULL,"
                                + "PRIMARY KEY('CLU' AUTOINCREMENT))";
                oDataSourceSqlite.__mSqlCommand(vQuery);
            }

            /// Таблица 'PclRrdTyp' - Виды записей протокола (упрощено до 'cgzPclRrdTyp'/'dsiPclRrdTyp' -
            /// без столбцов-флагов 'opt*', как и 'PclTyp' выше)
            if (oDataSourceSqlite.__mTableExists("PclRrdTyp") == false)
            {
                string vQuery = "CREATE TABLE PclRrdTyp ("
                                + "CHG TEXT,"
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "ELD INTEGER DEFAULT 0,"
                                + "GID TEXT,"
                                + "cgzPclRrdTyp INTEGER,"
                                + "dsiPclRrdTyp TEXT,"
                                + "PRIMARY KEY('CLU' AUTOINCREMENT))";
                oDataSourceSqlite.__mSqlCommand(vQuery);

                /// Засеяно строго в порядке enum 'PROTOCOLRECORDSTYPES' (1..6) плюс 'Причина' (7) -
                /// см. примечание к 'mRecordTypeClue'
                mPclRrdTypSeed(1, "Решение пользователя");
                mPclRrdTypSeed(2, "Детали");
                mPclRrdTypSeed(3, "Исключение");
                mPclRrdTypSeed(4, "Изображение");
                mPclRrdTypSeed(5, "Сообщение");
                mPclRrdTypSeed(6, "Свойства объекта");
                mPclRrdTypSeed(7, "Причина");
            }

            /// Таблица 'ImportedGid' - отслеживание уже импортированных строк из легаси '.pcl' файлов
            /// (не является частью реальной Essence-схемы - служебная таблица только этого класса)
            if (oDataSourceSqlite.__mTableExists("ImportedGid") == false)
            {
                string vQuery = "CREATE TABLE ImportedGid ("
                                + "GID TEXT NOT NULL UNIQUE"
                                + ", lnkPcl INTEGER" // Для строк протокола (.pcl) - реальный CLU в 'Pcl'; для строк записи (rrd.pcl) - маркер -1
                                + ", PRIMARY KEY('GID'))";
                oDataSourceSqlite.__mSqlCommand(vQuery);
            }
        }
        /// <summary>
        /// Переименование столбца таблицы, если он существует под старым именем (самовосстановление
        /// от опечатки 'Ink*' в более старой версии этого класса - см. примечание в '__mTablesFill')
        /// </summary>
        /// <param name="pTableName">Таблица</param>
        /// <param name="pOldColumnName">Старое (ошибочное) имя столбца</param>
        /// <param name="pNewColumnName">Правильное имя столбца</param>
        private void mColumnRenameIfExists(string pTableName, string pOldColumnName, string pNewColumnName)
        {
            if (oDataSourceSqlite.__mTableExists(pTableName) == false)
                return; // Таблицы ещё нет - переименовывать нечего, __mTablesFill создаст её ниже с правильным именем

            if (__mColumnExists(pTableName, pOldColumnName) == false)
                return; // Опечатки нет - таблица уже в правильной схеме (или ещё пустая свежесозданная)

            oDataSourceSqlite.__mSqlCommand("ALTER TABLE " + pTableName + " RENAME COLUMN " + pOldColumnName + " TO " + pNewColumnName);
        }
        /// <summary>
        /// Вставка одной засеваемой строки 'PclTyp' с ровно одним установленным флагом (по имени столбца)
        /// </summary>
        private void mPclTypSeed(int pCgz, string pCaption)
        {
            string vCommand = "Insert Into PclTyp (cgzPclTyp, dsiPclTyp) Values(" + pCgz.ToString() + ", '" + pCaption + "')";
            oDataSourceSqlite.__mSqlCommand(vCommand);
        }
        /// <summary>
        /// Вставка одной засеваемой строки 'PclRrdTyp'
        /// </summary>
        private void mPclRrdTypSeed(int pCgz, string pCaption)
        {
            string vCommand = "Insert Into PclRrdTyp (cgzPclRrdTyp, dsiPclRrdTyp) Values(" + pCgz.ToString() + ", '" + pCaption + "')";
            oDataSourceSqlite.__mSqlCommand(vCommand);
        }
        /// <summary>
        /// Проверка, был ли уже импортирован конкретный 'GID' из легаси '.pcl' файла
        /// </summary>
        /// <param name="pGid">Идентификатор строки (GID) из '.pcl' файла</param>
        /// <returns>[true] - строка с таким GID уже импортирована</returns>
        public bool __mProtocolIsImported(string pGid)
        {
            return oDataSourceSqlite.__mSqlCount("ImportedGid", "GID = '" + pGid + "'") > 0;
        }
        /// <summary>
        /// Отметка о том, что строка протокола (заголовок из '.pcl') импортирована, вместе с полученным реальным CLU
        /// </summary>
        /// <param name="pGid">Идентификатор строки (GID) из '.pcl' файла</param>
        /// <param name="pClu">Реальный CLU созданной строки в 'Pcl'</param>
        public void __mProtocolMarkImported(string pGid, int pClu)
        {
            oDataSourceSqlite.__mSqlCommand("Insert Into ImportedGid (GID, lnkPcl) Values('" + pGid + "', " + pClu.ToString() + ")");
        }
        /// <summary>
        /// Реальный CLU в 'Pcl', соответствующий уже импортированному GID протокола (для связывания импортируемых записей)
        /// </summary>
        /// <param name="pGid">Идентификатор строки (GID) протокола из '.pcl' файла</param>
        /// <returns>CLU в 'Pcl', [-1] если не найден</returns>
        public int __mProtocolClueByGid(string pGid)
        {
            object vValue = oDataSourceSqlite.__mSqlValue("ImportedGid", "lnkPcl", "GID = '" + pGid + "'");
            if (vValue == null || vValue == DBNull.Value)
                return -1;
            return Convert.ToInt32(vValue);
        }
        /// <summary>
        /// Прямая вставка строки протокола, импортируемой из легаси '.pcl' файла (в отличие от '__mCreate' - используется
        /// не для протоколирования работы текущего приложения, а для одноразового переноса уже существующих файлов)
        /// </summary>
        /// <param name="pGid">Идентификатор строки (GID) из '.pcl' файла</param>
        /// <param name="pChgTicks">Момент создания (тики)</param>
        /// <param name="pAppName">Имя приложения-источника</param>
        /// <param name="pAppDescription">Описание приложения-источника</param>
        /// <param name="pAppPrefix">Префикс приложения-источника</param>
        /// <param name="pProtocolType">Вид протокола</param>
        /// <param name="pHost">Имя компьютера</param>
        /// <param name="pProcedure">Название процедуры</param>
        /// <param name="pUser">Имя пользователя</param>
        /// <returns>Реальный CLU созданной строки в 'Pcl'</returns>
        public int __mProtocolImport(string pGid, long pChgTicks, string pAppName, string pAppDescription, string pAppPrefix, int pProtocolType, string pHost, string pProcedure, string pUser)
        {
            int vApplicationClue = mApplicationEnsure(pAppName, pAppDescription, pAppPrefix);
            int vCpuClue = mComputerEnsure(pHost);
            int vUsrClue = mUserEnsure(pUser);
            int vProtocolTypClue = mProtocolTypeClueByRawId(pProtocolType);
            string vChg = new DateTime(pChgTicks > 0 ? pChgTicks : DateTime.Now.Ticks).ToString("yyyy-MM-dd HH:mm:ss");

            string vCommand = "Insert Into Pcl (CHG, GID, lnkApp, lnkCpu, lnkPclTyp, lnkUsr, Prc, Fil)"
                              + " Values("
                              + "'" + vChg + "'"
                              + ", '" + pGid + "'"
                              + ", " + vApplicationClue.ToString()
                              + ", " + vCpuClue.ToString()
                              + ", " + vProtocolTypClue.ToString()
                              + ", " + vUsrClue.ToString()
                              + ", '" + (pProcedure ?? "").Replace("'", "''") + "'"
                              + ", NULL)";
            oDataSourceSqlite.__mSqlCommand(vCommand);

            return oDataSourceSqlite.__mClueLastInserted("Pcl");
        }
        /// <summary>
        /// Прямая вставка записи протокола, импортируемой из легаси 'rrd.pcl' файла
        /// </summary>
        /// <param name="pGid">Идентификатор строки (GID) записи из 'rrd.pcl' файла</param>
        /// <param name="pLnkPcl">Реальный CLU родительского протокола в 'Pcl' (см. '__mProtocolClueByGid')</param>
        /// <param name="pRawRecordType">Сырой числовой вид записи, как он записан в исходном файле</param>
        /// <param name="pMessage">Текст сообщения</param>
        /// <param name="pTick">Затраченное время выполнения (тики), если было записано</param>
        public void __mProtocolRecordImport(string pGid, int pLnkPcl, int pRawRecordType, string pMessage, long pTick)
        {
            int vRecordTypClue = mRecordTypeClueByRawId(pRawRecordType);

            string vCommand = "Insert Into PclRrd (CHG, GID, lnkPcl, lnkPclRrdTyp, Err, Tck)"
                              + " Values("
                              + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                              + ", '" + pGid + "'"
                              + ", " + pLnkPcl.ToString()
                              + ", " + vRecordTypClue.ToString()
                              + ", '" + (pMessage ?? "").Replace("'", "''") + "'"
                              + ", " + pTick.ToString() + ")";
            oDataSourceSqlite.__mSqlCommand(vCommand);
        }
        /// <summary>
        /// Поиск (или создание) строки приложения-источника по имени - для приложений, отличных от текущего запущенного
        /// (легаси файлы протоколов могли быть созданы другими приложениями - Administration.exe, csManual.exe и т.д.)
        /// </summary>
        /// <param name="pAppName">Имя приложения</param>
        /// <param name="pAppDescription">Описание приложения (не хранится отдельно - реальная 'App' не имеет поля описания, только 'dsiApp' и 'Pfx')</param>
        /// <param name="pAppPrefix">Префикс приложения</param>
        /// <returns>Реальный CLU строки в 'App'</returns>
        private int mApplicationEnsure(string pAppName, string pAppDescription, string pAppPrefix)
        {
            if (string.IsNullOrEmpty(pAppName) == true)
                pAppName = "(неизвестно)";

            object vExisting = oDataSourceSqlite.__mSqlValue("App", "CLU", "dsiApp = '" + pAppName.Replace("'", "''") + "'");
            if (vExisting != null && vExisting != DBNull.Value)
                return Convert.ToInt32(vExisting);

            string vCommand = "Insert Into App (CHG, GID, dsiApp, Pfx)"
                              + " Values("
                              + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                              + ", '" + Guid.NewGuid().ToString() + "'"
                              + ", '" + pAppName.Replace("'", "''") + "'"
                              + ", '" + (pAppPrefix ?? "").Replace("'", "''") + "')";
            oDataSourceSqlite.__mSqlCommand(vCommand);

            return oDataSourceSqlite.__mClueLastInserted("App");
        }
        /// <fixed>ВНИМАНИЕ - ТРЕБУЕТ УТОЧНЕНИЯ: подтверждённая эталонная схема ('Protocols.db') не содержит
        /// таблицы 'Cpu' - только 'Pcl.lnkCpu' как внешний ключ без цели внутри этой базы. Раньше этот
        /// метод сам создавал строки в локальной 'Cpu' и возвращал их настоящий CLU; теперь, раз такой
        /// таблицы нет, реальный CLU получить неоткуда. Возвращается заглушка [0] ("не определено") -
        /// имя компьютера ('pHostName') сейчас НИКУДА не сохраняется и теряется. Если разрешение имени
        /// компьютера/пользователя на самом деле должно идти в другую (общую для всего решения) базу
        /// данных - например туда, где 'admEssenceCpu'/'admEssenceUsr' основного приложения 'Administration'
        /// действительно хранят свои записи - этому методу нужен доступ к ней, которого сейчас нет</fixed>
        /// <summary>
        /// Заглушка вместо поиска/создания строки компьютера - см. пометку 'ВНИМАНИЕ' выше
        /// </summary>
        /// <param name="pHostName">Имя компьютера (сейчас не сохраняется никуда)</param>
        /// <returns>[0] - "не определено" (таблицы 'Cpu' в подтверждённой схеме нет)</returns>
        private int mComputerEnsure(string pHostName)
        {
            return 0;
        }
        /// <summary>
        /// Заглушка вместо поиска/создания строки пользователя - см. пометку 'ВНИМАНИЕ' у 'mComputerEnsure'
        /// </summary>
        /// <param name="pUserName">Имя пользователя (сейчас не сохраняется никуда)</param>
        /// <returns>[0] - "не определено" (таблицы 'Usr' в подтверждённой схеме нет)</returns>
        private int mUserEnsure(string pUserName)
        {
            return 0;
        }
        /// <summary>
        /// Соответствие сырого числового id вида протокола, как он записан в легаси '.pcl' файле, реальному CLU в 'PclTyp'.
        /// </summary>
        /// <remarks>'appProtocols.__mCreate' (реальный код, которым были написаны легаси '.pcl' файлы) пишет в файл
        /// уже готовое число 1-12 через собственный switch (ApplicationError=1 .. Other=12), а НЕ сырое значение enum (0-11).
        /// Это число уже точно совпадает с реальными CLU в 'PclTyp' (см. 'mProtocolTypeClue')</remarks>
        private int mProtocolTypeClueByRawId(int pRawId)
        {
            if (pRawId >= 1 && pRawId <= 12)
                return pRawId;
            return 1;
        }
        /// <summary>
        /// Соответствие сырого числового id вида записи протокола реальному CLU в 'PclRrdTyp' - для импорта легаси файлов
        /// </summary>
        private int mRecordTypeClueByRawId(int pRawId)
        {
            if (Enum.IsDefined(typeof(PROTOCOLRECORDSTYPES), pRawId) == true)
                return mRecordTypeClue((PROTOCOLRECORDSTYPES)pRawId);
            return 5;
        }

        /// <summary>
        /// Выполнение произвольного SELECT-запроса к базе данных протоколов (для отображения в вьювере)
        /// </summary>
        /// <param name="pQuery">Текст SQL-запроса</param>
        /// <returns>Результат запроса</returns>
        public System.Data.DataTable __mQuery(string pQuery)
        {
            return oDataSourceSqlite.__mSqlQuery(pQuery);
        }
        /// <summary>
        /// Проверка существования таблицы в текущей базе данных протоколов (для вьювера - определение,
        /// какой вариант схемы у открытой базы: старой/легаси или новой, с 'Cpu'/'Usr')
        /// </summary>
        public bool __mTableExists(string pTableName)
        {
            return oDataSourceSqlite.__mTableExists(pTableName);
        }
        /// <summary>
        /// Проверка существования столбца в указанной таблице текущей базы данных протоколов (для вьювера -
        /// определение, какой вариант схемы у открытой базы, например 'PclRrd.Msg' против легаси 'PclRrd.Err')
        /// </summary>
        public bool __mColumnExists(string pTableName, string pColumnName)
        {
            System.Data.DataTable vColumns = oDataSourceSqlite.__mSqlQuery("PRAGMA table_info(" + pTableName + ")");
            if (vColumns == null)
                return false;

            foreach (System.Data.DataRow vColumn in vColumns.Rows)
            {
                if (string.Equals(Convert.ToString(vColumn["name"]), pColumnName, StringComparison.OrdinalIgnoreCase) == true)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Полная карта GID -&gt; CLU из таблицы 'ImportedGid' одним запросом (для пакетного импорта легаси '.pcl').
        /// Для записей протоколов (не заголовков) 'lnkPcl' хранится как -1.
        /// </summary>
        public System.Collections.Generic.Dictionary<string, int> __mImportedGidCluMapGet()
        {
            System.Collections.Generic.Dictionary<string, int> vMap = new System.Collections.Generic.Dictionary<string, int>();
            try
            {
                System.Data.DataTable vTable = oDataSourceSqlite.__mSqlQuery("SELECT GID, lnkPcl FROM ImportedGid");
                if (vTable == null)
                    return vMap;

                foreach (System.Data.DataRow vRow in vTable.Rows)
                {
                    if (vRow["GID"] == System.DBNull.Value)
                        continue;
                    string vGid = Convert.ToString(vRow["GID"]);
                    if (string.IsNullOrEmpty(vGid) == true)
                        continue;
                    int vClu = -1;
                    if (vRow["lnkPcl"] != System.DBNull.Value)
                        int.TryParse(Convert.ToString(vRow["lnkPcl"]), out vClu);
                    vMap[vGid] = vClu;
                }
            }
            catch
            {
                /// Пустая/ещё не созданная таблица - карта остаётся пустой, импорт начнёт с нуля
            }
            return vMap;
        }

        /// <summary>
        /// Открытие транзакции на источнике данных SQLite (для пакетного импорта легаси-файлов)
        /// </summary>
        public bool __mTransactionBegin()
        {
            return oDataSourceSqlite.__mTransactionOn();
        }

        /// <summary>
        /// Фиксация открытой транзакции
        /// </summary>
        public bool __mTransactionCommit()
        {
            return oDataSourceSqlite.__mTransactionOff(true);
        }

        /// <summary>
        /// Откат открытой транзакции
        /// </summary>
        public bool __mTransactionRollback()
        {
            return oDataSourceSqlite.__mTransactionOff(false);
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Объекты

        /// <summary>
        /// Объект для работы с Sqlite
        /// </summary>
        private dsqDataSourceSqliteWithProtocol oDataSourceSqlite = new dsqDataSourceSqliteWithProtocol();

        #endregion Объекты

        /// <summary>
        /// Путь к базе данных
        /// </summary>
        public string __fDatabasePath = "";
        /// <summary>
        /// Идентификатор последнего созданного протокола
        /// </summary>
        private int fProtocolClue = -1;

        #endregion ПОЛЯ
    }
}