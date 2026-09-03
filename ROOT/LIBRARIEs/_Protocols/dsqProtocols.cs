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
            int vProtocolTypClue = mProtocolTypeClue(pProtocolType);
            string vDateTimeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string vPrintScreenFile = pPrintScreen == true ? __mPrintScreen() : "";
            string vHost = (Environment.MachineName ?? "").Replace("'", "''");
            string vUser = (Environment.UserName ?? "").Replace("'", "''");

            // Hst/Usr - текст прямо в Pcl (без таблиц Cpu/Usr). GID - новый GUID для живых записей.
            string vCommand = "Insert Into Pcl (CHG, GID, lnkApp, Hst, lnkPclTyp, Usr, Prc, Fil)"
                              + " Values("
                              + "'" + vDateTimeNow + "'"
                              + ", '" + Guid.NewGuid().ToString() + "'"
                              + ", " + vApplicationClue.ToString()
                              + ", '" + vHost + "'"
                              + ", " + vProtocolTypClue.ToString()
                              + ", '" + vUser + "'"
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

           
            mColumnRenameIfExists("Pcl", "InkApp", "lnkApp");
            mColumnRenameIfExists("Pcl", "InkPclTyp", "lnkPclTyp");
            mColumnRenameIfExists("PclRrd", "InkPcl", "lnkPcl");
            mColumnRenameIfExists("PclRrd", "InkPclRrdTyp", "lnkPclRrdTyp");

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

            /// Таблица 'Pcl' - заголовки протоколов.
            /// Хост/пользователь - текстовые Hst/Usr (как в тестовой базе с данными).
            /// Таблиц Cpu/Usr нет - они не входят в целевую схему.
            if (oDataSourceSqlite.__mTableExists("Pcl") == false)
            {
                string vQuery = "CREATE TABLE Pcl ("
                                + "CHG TEXT,"
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "ELD INTEGER DEFAULT 0,"
                                + "GID TEXT,"
                                + "lnkApp INTEGER,"
                                + "Hst TEXT,"
                                + "lnkPclTyp INTEGER,"
                                + "Usr TEXT,"
                                + "Prc TEXT,"
                                + "Fil TEXT,"
                                + "PRIMARY KEY('CLU' AUTOINCREMENT))";
                oDataSourceSqlite.__mSqlCommand(vQuery);
            }
            else
            {
                // Миграция существующих баз: если есть lnkCpu/lnkUsr, а Hst/Usr нет - добавить Hst/Usr
                if (__mColumnExists("Pcl", "Hst") == false)
                    oDataSourceSqlite.__mSqlCommand("ALTER TABLE Pcl ADD COLUMN Hst TEXT");
                if (__mColumnExists("Pcl", "Usr") == false)
                    oDataSourceSqlite.__mSqlCommand("ALTER TABLE Pcl ADD COLUMN Usr TEXT");
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

             
                mPclRrdTypSeed("Решение пользователя", "optAns");
                mPclRrdTypSeed("Детали", "optDtl");
                mPclRrdTypSeed("Исключение", "optExc");
                mPclRrdTypSeed("Изображение", "optImg");
                mPclRrdTypSeed("Сообщение", "optMsg");
                mPclRrdTypSeed("Свойства объекта", "optObjPrp");
                mPclRrdTypSeed("Причина", "optRsn");
            }

          
            if (oDataSourceSqlite.__mTableExists("ImportedGid") == true)
            {
                try { oDataSourceSqlite.__mSqlCommand("DROP TABLE ImportedGid"); }
                catch { }
            }
        }
        /// <summary>
        /// Переименование столбца таблицы, если он существует под старым именем 
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
            if (string.IsNullOrEmpty(pGid) == true)
                return false;
            string vEsc = pGid.Replace("'", "''");
            // Заголовок: Pcl.GID; запись: PclRrd.GID (текстовый legacy-id)
            if (oDataSourceSqlite.__mSqlCount("Pcl", "GID = '" + vEsc + "'") > 0)
                return true;
            if (oDataSourceSqlite.__mTableExists("PclRrd") == true
                && oDataSourceSqlite.__mSqlCount("PclRrd", "CAST(GID AS TEXT) = '" + vEsc + "'") > 0)
                return true;
            return false;
        }
        /// <summary>
        /// Отметка импорта больше не пишет в отдельную таблицу: GID уже сохранён в Pcl/PclRrd.
        /// Метод оставлен для совместимости с ProtocolSqliteImporter (no-op).
        /// </summary>
        public void __mProtocolMarkImported(string pGid, int pClu)
        {
            // no-op: дедупликация по Pcl.GID / PclRrd.GID
        }
        /// <summary>
        /// Реальный CLU в 'Pcl' по GID протокола (для связывания записей при импорте)
        /// </summary>
        public int __mProtocolClueByGid(string pGid)
        {
            if (string.IsNullOrEmpty(pGid) == true)
                return -1;
            object vValue = oDataSourceSqlite.__mSqlValue("Pcl", "CLU", "GID = '" + pGid.Replace("'", "''") + "'");
            if (vValue == null || vValue == DBNull.Value)
                return -1;
            return Convert.ToInt32(vValue);
        }
        /// <summary>
        /// Прямая вставка строки протокола, импортируемой из легаси '.pcl' файла 
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
            int vProtocolTypClue = mProtocolTypeClueByRawId(pProtocolType);
            // CHG как тики (.NET) - как в тестовой protocols.db; вьювер сам форматирует дату
            string vChg = (pChgTicks > 0 ? pChgTicks : DateTime.Now.Ticks).ToString();
            string vHost = (pHost ?? "").Replace("'", "''");
            string vUser = (pUser ?? "").Replace("'", "''");
            string vGid = (pGid ?? Guid.NewGuid().ToString()).Replace("'", "''");

            string vCommand = "Insert Into Pcl (CHG, GID, lnkApp, Hst, lnkPclTyp, Usr, Prc, Fil)"
                              + " Values("
                              + "'" + vChg + "'"
                              + ", '" + vGid + "'"
                              + ", " + vApplicationClue.ToString()
                              + ", '" + vHost + "'"
                              + ", " + vProtocolTypClue.ToString()
                              + ", '" + vUser + "'"
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
        /// Соответствие сырого числового id вида протокола, как он записан в легаси '.pcl' файле, реальному CLU в 'PclTyp'.
        /// </summary>
      
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
           
            if (pRawId >= 1 && pRawId <= 7)
                return pRawId;
            return 5; // Сообщение по умолчанию
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
        /// Полная карта GID -&gt; CLU из Pcl/PclRrd одним запросом (для пакетного импорта легаси '.pcl').
        /// Для записей протоколов (не заголовков) значение карты = -1.
        /// </summary>
        public System.Collections.Generic.Dictionary<string, int> __mImportedGidCluMapGet()
        {
            // Карта GID -> CLU без таблицы ImportedGid: заголовки из Pcl, записи из PclRrd (CLU=-1).
            System.Collections.Generic.Dictionary<string, int> vMap = new System.Collections.Generic.Dictionary<string, int>();
            try
            {
                System.Data.DataTable vPcl = oDataSourceSqlite.__mSqlQuery("SELECT GID, CLU FROM Pcl WHERE GID IS NOT NULL AND GID <> ''");
                if (vPcl != null)
                {
                    foreach (System.Data.DataRow vRow in vPcl.Rows)
                    {
                        if (vRow["GID"] == System.DBNull.Value)
                            continue;
                        string vGid = Convert.ToString(vRow["GID"]);
                        if (string.IsNullOrEmpty(vGid))
                            continue;
                        int vClu = -1;
                        if (vRow["CLU"] != System.DBNull.Value)
                            int.TryParse(Convert.ToString(vRow["CLU"]), out vClu);
                        vMap[vGid] = vClu;
                    }
                }

                System.Data.DataTable vRrd = oDataSourceSqlite.__mSqlQuery(
                    "SELECT CAST(GID AS TEXT) AS GID FROM PclRrd WHERE GID IS NOT NULL AND CAST(GID AS TEXT) <> '' AND CAST(GID AS TEXT) <> '0'");
                if (vRrd != null)
                {
                    foreach (System.Data.DataRow vRow in vRrd.Rows)
                    {
                        if (vRow["GID"] == System.DBNull.Value)
                            continue;
                        string vGid = Convert.ToString(vRow["GID"]);
                        if (string.IsNullOrEmpty(vGid) || vMap.ContainsKey(vGid))
                            continue;
                        vMap[vGid] = -1; // запись, не заголовок
                    }
                }
            }
            catch
            {
                /// Таблицы ещё нет - карта пустая, импорт с нуля
            }
            return vMap;
        }

        /// <summary>
        /// Открытие транзакции на источнике данных SQLite (для пакетного импорта легаси-файлов)
        /// </summary>

        /// <summary>
        /// Удаление мусора от старых нераспознанных импортов (mImportRawFallback).
        /// </summary>
      
        public int __mPurgeUnrecognizedImports()
        {
            int vDeletedHeaders = 0;
            try
            {
                if (oDataSourceSqlite.__mTableExists("Pcl") == false)
                    return 0;

                // CLU заголовков-мусора
                System.Data.DataTable vJunk = oDataSourceSqlite.__mSqlQuery(
                    "SELECT CLU, GID FROM Pcl WHERE GID LIKE 'RAWFILE_%'");
                if (vJunk == null || vJunk.Rows.Count == 0)
                {
                    // На всякий случай подчистить осиротевшие записи с RAWFILE_ GID
                    if (oDataSourceSqlite.__mTableExists("PclRrd") == true)
                        oDataSourceSqlite.__mSqlCommand(
                            "DELETE FROM PclRrd WHERE CAST(GID AS TEXT) LIKE 'RAWFILE_%'");
                    return 0;
                }

                System.Text.StringBuilder vCluList = new System.Text.StringBuilder();
                foreach (System.Data.DataRow vRow in vJunk.Rows)
                {
                    if (vRow["CLU"] == System.DBNull.Value)
                        continue;
                    if (vCluList.Length > 0)
                        vCluList.Append(',');
                    vCluList.Append(Convert.ToString(vRow["CLU"]));
                    vDeletedHeaders++;
                }

                if (vCluList.Length == 0)
                    return 0;

                string vIn = vCluList.ToString();

                // Сначала записи, потом заголовки
                if (oDataSourceSqlite.__mTableExists("PclRrd") == true)
                {
                    oDataSourceSqlite.__mSqlCommand(
                        "DELETE FROM PclRrd WHERE lnkPcl IN (" + vIn + ") OR CAST(GID AS TEXT) LIKE 'RAWFILE_%'");
                }

                oDataSourceSqlite.__mSqlCommand(
                    "DELETE FROM Pcl WHERE CLU IN (" + vIn + ") OR GID LIKE 'RAWFILE_%'");
            }
            catch
            {
                return 0;
            }

            return vDeletedHeaders;
        }

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
        public static nlData.datUnitDataSource __oViewing_ = null;
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