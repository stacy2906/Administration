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
    /// <fixed>ПРИВЕДЕНО К РЕАЛЬНОЙ Essence-СХЕМЕ: раньше таблицы 'App'/'Pcl'/'PclTyp'/'PclRrd' были
    /// придуманы с нуля (например 'desApp' вместо 'dsiApp', хост/пользователь хранились прямо текстом
    /// в 'Pcl.Hst'/'Pcl.Usr' вместо связей 'lnkCpu'/'lnkUsr' на таблицы 'Cpu'/'Usr') - именно это вызывало
    /// ошибку "no such column: A.dsiApp" (запрос вьювера уже ожидал реальную схему, а таблица в базе
    /// была создана по старой). Столбцы сверены построчно с реальными классами-сущностями проекта:
    /// 'admEssenceApp.cs', 'admEssenceCpu.cs', 'admEssenceUsr.cs', 'admEssencePcl.cs', 'admEssencePclTyp.cs',
    /// 'admEssencePclRrd.cs', 'admEssencePclRrdTyp.cs' (метод '__mRecordNew' каждого класса - точный список
    /// столбцов). Общие для всех Essence-таблиц столбцы: 'CHG' (дата изменения), 'CLU' (первичный ключ),
    /// 'ELD' (0 - не удалено), 'GID' (глобальный идентификатор).
    /// ЕДИНСТВЕННОЕ намеренное отклонение от реальной схемы: в 'admEssencePclRrd' НЕТ текстового поля
    /// сообщения вообще ('dsrErr'/'dsrExc' там - целые числа, не текст) - у просмотрщика протоколов
    /// тогда нечего было бы показывать в качестве текста лога. Поэтому здесь добавлен столбец 'Msg TEXT',
    /// которого в реальной Essence-таблице нет - см. пометку 'РАСШИРЕНИЕ' в '__mTablesFill'</fixed>
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

            string vCommand = "Insert Into Pcl (CHG, GID, dtmPclCre, lnkApp, lnkCpu, lnkPclTyp, lnkUsr, Prc, Fil)"
                              + " Values("
                              + "'" + vDateTimeNow + "'"
                              + ", '" + Guid.NewGuid().ToString() + "'"
                              + ", '" + vDateTimeNow + "'"
                              + ", " + vApplicationClue.ToString()
                              + ", " + vCpuClue.ToString()
                              + ", " + vProtocolTypClue.ToString()
                              + ", " + vUsrClue.ToString()
                              + ", '" + pProcedure.Replace("'", "''") + "'"
                              + ", '" + vPrintScreenFile.Replace("'", "''") + "')";

            oDataSourceSqlite.__mSqlCommand(vCommand);
            fProtocolClue = oDataSourceSqlite.__mClueLastInserted("Pcl");
        }
        public override void __mRecord(PROTOCOLRECORDSTYPES pRecordType, string pRecordText, long pTick = -1)
        {
            string vCommand = "Insert Into PclRrd (CHG, GID, lnkPcl, lnkPclRrdTyp, Msg, Tck)"
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
        /// значение enum, от 'ApplicationError' до 'Other') - поэтому 'CLU' построчно совпадает со
        /// значением enum, независимо от того, в каком порядке объявлены сами флаговые столбцы
        /// ('optAppErr'/'optAppExc'/... - в реальном 'admEssencePclTyp.cs' они идут НЕ в порядке enum,
        /// но это не важно: значение CLU определяется порядком ВСТАВКИ строк, а не порядком столбцов)</remarks>
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
        /// <remarks>Реальный 'admEssencePclRrdTyp.cs' определяет СЕМЬ флагов ('optAns','optDtl','optExc',
        /// 'optImg','optMsg','optObjPrp','optRsn'), а не шесть - у исходного enum 'PROTOCOLRECORDSTYPES'
        /// нет соответствия для 'optRsn' ('Причина'). Строки сеются в этом же порядке, поэтому CLU=7
        /// теперь занят типом 'Причина' - зарезервировано на будущее, из enum пока недостижимо</remarks>
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

            /// Таблица 'Cpu' - Компьютеры (реальные столбцы: 'admEssenceCpu.__mRecordNew')
            if (oDataSourceSqlite.__mTableExists("Cpu") == false)
            {
                string vQuery = "CREATE TABLE Cpu ("
                                + "CHG TEXT,"
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "ELD INTEGER DEFAULT 0,"
                                + "GID TEXT,"
                                + "cgzCpu INTEGER,"
                                + "dsiCpu TEXT,"
                                + "PRIMARY KEY('CLU' AUTOINCREMENT))";
                oDataSourceSqlite.__mSqlCommand(vQuery);
            }

            /// Таблица 'Usr' - Пользователи (реальные столбцы: 'admEssenceUsr.__mRecordNew')
            if (oDataSourceSqlite.__mTableExists("Usr") == false)
            {
                string vQuery = "CREATE TABLE Usr ("
                                + "CHG TEXT,"
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "ELD INTEGER DEFAULT 0,"
                                + "GID TEXT,"
                                + "codUsr INTEGER,"
                                + "dsiUsr TEXT,"
                                + "mrkAdm INTEGER DEFAULT 0,"
                                + "PswCod TEXT,"
                                + "PRIMARY KEY('CLU' AUTOINCREMENT))";
                oDataSourceSqlite.__mSqlCommand(vQuery);
            }

            /// Таблица 'Pcl' - Протоколы (реальные столбцы: 'admEssencePcl.__mRecordNew')
            if (oDataSourceSqlite.__mTableExists("Pcl") == false)
            {
                string vQuery = "CREATE TABLE Pcl ("
                                + "CHG TEXT,"
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "ELD INTEGER DEFAULT 0,"
                                + "GID TEXT,"
                                + "cgzPcl INTEGER,"
                                + "dtmPclCre TEXT,"
                                + "lnkApp INTEGER,"
                                + "lnkCpu INTEGER,"
                                + "lnkPclTyp INTEGER,"
                                + "lnkUsr INTEGER,"
                                + "Prc TEXT,"
                                + "Fil TEXT,"
                                + "PRIMARY KEY('CLU' AUTOINCREMENT))";
                oDataSourceSqlite.__mSqlCommand(vQuery);
            }

            /// Таблица 'PclTyp' - Виды протоколов (реальные столбцы: 'admEssencePclTyp.__mRecordNew')
            if (oDataSourceSqlite.__mTableExists("PclTyp") == false)
            {
                string vQuery = "CREATE TABLE PclTyp ("
                                + "CHG TEXT,"
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "ELD INTEGER DEFAULT 0,"
                                + "GID TEXT,"
                                + "cgzPclTyp INTEGER,"
                                + "dsiPclTyp TEXT,"
                                + "optAppErr INTEGER,"
                                + "optAppExc INTEGER,"
                                + "optAppErrPrg INTEGER,"
                                + "optAppEvn INTEGER,"
                                + "optDatErr INTEGER,"
                                + "optDatEvn INTEGER,"
                                + "optDevErr INTEGER,"
                                + "optDevEvn INTEGER,"
                                + "optOth INTEGER,"
                                + "optUsrErr INTEGER,"
                                + "optUsrEvn INTEGER,"
                                + "optUsrMsg INTEGER,"
                                + "PRIMARY KEY('CLU' AUTOINCREMENT))";
                oDataSourceSqlite.__mSqlCommand(vQuery);

                /// Засеяно СТРОГО в порядке значений enum 'PROTOCOLSTYPES' (1..12), чтобы CLU совпадал со
                /// значением enum - см. примечание к 'mProtocolTypeClue'. У каждой строки установлен ровно
                /// ОДИН верный флаг по имени (не по позиции) - соответствие проверено по названию столбца
                mPclTypSeed("Ошибка приложения", "optAppErr");
                mPclTypSeed("Ошибка программирования", "optAppErrPrg");
                mPclTypSeed("Исключение", "optAppExc");
                mPclTypSeed("Событие приложения", "optAppEvn");
                mPclTypSeed("Ошибка источника данных", "optDatErr");
                mPclTypSeed("Событие источника данных", "optDatEvn");
                mPclTypSeed("Ошибка устройства", "optDevErr");
                mPclTypSeed("Событие устройства", "optDevEvn");
                mPclTypSeed("Ошибка пользователя", "optUsrErr");
                mPclTypSeed("Событие пользователя", "optUsrEvn");
                mPclTypSeed("Сообщение пользователю", "optUsrMsg");
                mPclTypSeed("Прочее", "optOth");
            }

            /// Таблица 'PclRrd' - Записи протокола (реальные столбцы: 'admEssencePclRrd.__mRecordNew' + 'Msg')
            if (oDataSourceSqlite.__mTableExists("PclRrd") == false)
            {
                string vQuery = "CREATE TABLE PclRrd ("
                                + "CHG TEXT,"
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "ELD INTEGER DEFAULT 0,"
                                + "GID TEXT,"
                                + "lnkPcl INTEGER,"
                                + "lnkPclRrdTyp INTEGER,"
                                + "dsrErr INTEGER DEFAULT 0,"
                                + "dsrExc INTEGER DEFAULT 0,"
                                + "Tck INTEGER,"
                                /// РАСШИРЕНИЕ сверх реальной Essence-схемы: у 'admEssencePclRrd' в проекте
                                /// нет текстового поля сообщения вообще ('dsrErr'/'dsrExc' - целые числа,
                                /// не текст). Без текста сообщения просмотрщик протоколов не может показать
                                /// сам лог - добавлен столбец 'Msg TEXT', отсутствующий в реальной таблице
                                + "Msg TEXT,"
                                + "PRIMARY KEY('CLU' AUTOINCREMENT))";
                oDataSourceSqlite.__mSqlCommand(vQuery);
            }

            /// Таблица 'PclRrdTyp' - Виды записей протокола (реальные столбцы: 'admEssencePclRrdTyp.__mRecordNew';
            /// реальное название таблицы - 'PclRrdTyp', а НЕ 'RrdTyp', как было раньше)
            if (oDataSourceSqlite.__mTableExists("PclRrdTyp") == false)
            {
                string vQuery = "CREATE TABLE PclRrdTyp ("
                                + "CHG TEXT,"
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "ELD INTEGER DEFAULT 0,"
                                + "GID TEXT,"
                                + "cgzPclRrdTyp INTEGER,"
                                + "dsiPclRrdTyp TEXT,"
                                + "optAns INTEGER,"
                                + "optDtl INTEGER,"
                                + "optExc INTEGER,"
                                + "optImg INTEGER,"
                                + "optMsg INTEGER,"
                                + "optObjPrp INTEGER,"
                                + "optRsn INTEGER,"
                                + "PRIMARY KEY('CLU' AUTOINCREMENT))";
                oDataSourceSqlite.__mSqlCommand(vQuery);

                /// Засеяно строго в порядке enum 'PROTOCOLRECORDSTYPES' (1..6) плюс 'Причина' (7-й реальный
                /// флаг 'optRsn', которому раньше не было соответствия) - см. примечание к 'mRecordTypeClue'
                mPclRrdTypSeed("Решение пользователя", "optAns");
                mPclRrdTypSeed("Детали", "optDtl");
                mPclRrdTypSeed("Исключение", "optExc");
                mPclRrdTypSeed("Изображение", "optImg");
                mPclRrdTypSeed("Сообщение", "optMsg");
                mPclRrdTypSeed("Свойства объекта", "optObjPrp");
                mPclRrdTypSeed("Причина", "optRsn");
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
        /// Вставка одной засеваемой строки 'PclTyp' с ровно одним установленным флагом (по имени столбца)
        /// </summary>
        private void mPclTypSeed(string pCaption, string pFlagColumnName)
        {
            string vCommand = "Insert Into PclTyp (dsiPclTyp, " + pFlagColumnName + ") Values('" + pCaption + "', 1)";
            oDataSourceSqlite.__mSqlCommand(vCommand);
        }
        /// <summary>
        /// Вставка одной засеваемой строки 'PclRrdTyp' с ровно одним установленным флагом (по имени столбца)
        /// </summary>
        private void mPclRrdTypSeed(string pCaption, string pFlagColumnName)
        {
            string vCommand = "Insert Into PclRrdTyp (dsiPclRrdTyp, " + pFlagColumnName + ") Values('" + pCaption + "', 1)";
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

            string vCommand = "Insert Into Pcl (CHG, GID, dtmPclCre, lnkApp, lnkCpu, lnkPclTyp, lnkUsr, Prc, Fil)"
                              + " Values("
                              + "'" + vChg + "'"
                              + ", '" + pGid + "'"
                              + ", '" + vChg + "'"
                              + ", " + vApplicationClue.ToString()
                              + ", " + vCpuClue.ToString()
                              + ", " + vProtocolTypClue.ToString()
                              + ", " + vUsrClue.ToString()
                              + ", '" + (pProcedure ?? "").Replace("'", "''") + "'"
                              + ", '')";
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

            string vCommand = "Insert Into PclRrd (CHG, GID, lnkPcl, lnkPclRrdTyp, Msg, Tck)"
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
        /// <summary>
        /// Поиск (или создание) строки компьютера по имени
        /// </summary>
        /// <remarks>НОВОЕ: раньше имя компьютера писалось прямо текстом в 'Pcl.Hst' - реальная схема
        /// связывает 'Pcl.lnkCpu' с отдельной таблицей 'Cpu' (см. 'admEssencePcl.__mRecordNew')</remarks>
        /// <param name="pHostName">Имя компьютера</param>
        /// <returns>Реальный CLU строки в 'Cpu'</returns>
        private int mComputerEnsure(string pHostName)
        {
            if (string.IsNullOrEmpty(pHostName) == true)
                pHostName = "(неизвестно)";

            object vExisting = oDataSourceSqlite.__mSqlValue("Cpu", "CLU", "dsiCpu = '" + pHostName.Replace("'", "''") + "'");
            if (vExisting != null && vExisting != DBNull.Value)
                return Convert.ToInt32(vExisting);

            string vCommand = "Insert Into Cpu (CHG, GID, dsiCpu)"
                              + " Values("
                              + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                              + ", '" + Guid.NewGuid().ToString() + "'"
                              + ", '" + pHostName.Replace("'", "''") + "')";
            oDataSourceSqlite.__mSqlCommand(vCommand);

            return oDataSourceSqlite.__mClueLastInserted("Cpu");
        }
        /// <summary>
        /// Поиск (или создание) строки пользователя по имени
        /// </summary>
        /// <remarks>НОВОЕ: раньше имя пользователя писалось прямо текстом в 'Pcl.Usr' - реальная схема
        /// связывает 'Pcl.lnkUsr' с отдельной таблицей 'Usr' (см. 'admEssencePcl.__mRecordNew')</remarks>
        /// <param name="pUserName">Имя пользователя</param>
        /// <returns>Реальный CLU строки в 'Usr'</returns>
        private int mUserEnsure(string pUserName)
        {
            if (string.IsNullOrEmpty(pUserName) == true)
                pUserName = "(неизвестно)";

            object vExisting = oDataSourceSqlite.__mSqlValue("Usr", "CLU", "dsiUsr = '" + pUserName.Replace("'", "''") + "'");
            if (vExisting != null && vExisting != DBNull.Value)
                return Convert.ToInt32(vExisting);

            string vCommand = "Insert Into Usr (CHG, GID, dsiUsr)"
                              + " Values("
                              + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'"
                              + ", '" + Guid.NewGuid().ToString() + "'"
                              + ", '" + pUserName.Replace("'", "''") + "')";
            oDataSourceSqlite.__mSqlCommand(vCommand);

            return oDataSourceSqlite.__mClueLastInserted("Usr");
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