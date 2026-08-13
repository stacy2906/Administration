using nlApplication;
using nlData;
using System;
using System.Collections.Generic;
using System.Data;

namespace nlDataSourceSqlite
{
    public class dsqProtocols : appProtocols
    {
        #region = ДИЗАЙНЕРЫ

        public dsqProtocols()
        {
            _mObjectAssembly();
        }

        #endregion ДИЗАНЕРЫ

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
            object vApplicationClueRaw = oDataSourceSqlite.__mSqlValue("App", "CLU", "desApp = '" + appApplication.__fProcessName_ + "'");
            int vApplicationClue = (vApplicationClueRaw == null || vApplicationClueRaw == DBNull.Value) ? -1 : Convert.ToInt32(vApplicationClueRaw);
            int vProtocolTypClue = mProtocolTypeClue(pProtocolType); // Верифицированное соответствие enum -> реальный CLU в 'PclTyp' (см. примечание к 'cProtocolTypeClueMap')
            string vPrintScreenFile = "";

            if (pPrintScreen == true)
            {
                vPrintScreenFile = __mPrintScreen();
            }

            string vCommand = "Insert Into Pcl (CHG, lnkApp, lnkPclTyp, FilPrnScr, Hst, Prc, Usr)"
                              + " Values("
                              + "'" + DateTime.Now.Ticks.ToString() + "'"
                              + ", " + vApplicationClue.ToString()
                              + ", " + vProtocolTypClue.ToString()
                              + ", '" + vPrintScreenFile + "'"
                              + ", '" + Environment.MachineName + "'"
                              + ", '" + pProcedure + "'"
                              + ", '" + Environment.UserName + "')";

            oDataSourceSqlite.__mSqlCommand(vCommand);
            fProtocolClue = oDataSourceSqlite.__mClueLastInserted("Pcl");
        }
        public override void __mRecord(PROTOCOLRECORDSTYPES pRecordType, string pRecordText, long pTick = -1)
        {
            string vCommand = "Insert Into PclRrd(lnkPcl, lnkRrdTyp, Msg, Tck)"
                              + " Values("
                              + fProtocolClue.ToString()
                              + ", " + mRecordTypeClue(pRecordType).ToString()
                              + ", '" + pRecordText + "'"
                              + ", " + pTick.ToString() + ")";
            oDataSourceSqlite.__mSqlCommand(vCommand);
        }
        /// <summary>
        /// Соответствие вида протокола (enum 'PROTOCOLSTYPES') реальному 'CLU' строки в таблице 'PclTyp'.
        /// </summary>
        /// <remarks>ВАЖНО: это НЕ простое смещение "+1" - строки в 'PclTyp' засеяны НЕ строго по порядку enum
        /// (перепутаны местами вставки для 'ApplicationException' и 'ApplicationErrorProgramatic' - позиции 2 и 3).
        /// Соответствие подтверждено по одноимённым флаговым колонкам ('optAplErr','optDatEve' и т.д. - они называются
        /// по членам enum и не подвержены ошибкам текста), а не по подписи 'desPclTyp' (некоторые подписи не совпадают
        /// с реальным enum или продублированы - см. CLU 10 и 11, у обоих одинаковый текст подписи).</remarks>
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
        /// Соответствие вида записи протокола (enum 'PROTOCOLRECORDSTYPES') реальному 'CLU' строки в таблице 'RrdTyp'.
        /// </summary>
        /// <remarks>'RrdTyp' засеяна на 6 строк в чистом порядке enum (CLU = enum + 1), но для
        /// 'PROTOCOLRECORDSTYPES.Reason' (значение 6) строки в таблице нет вовсе - таблица недосеяна.
        /// Не меняю схему/сидинг 'RrdTyp' здесь во избежание риска затронуть уже работающие 6 типов;
        /// пока 'Reason' безопасно резервируется на CLU=5 ('Сообщение') как ближайший по смыслу тип.</remarks>
        /// <param name="pRecordType">Вид записи протокола</param>
        /// <returns>Реальный CLU строки в 'RrdTyp'</returns>
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
                case PROTOCOLRECORDSTYPES.Reason: return 5; // Строки для 'Reason' в 'RrdTyp' нет - см. примечание выше
                default: return 5;
            }
        }

        public void __mTablesFill()
        {
            if (oDataSourceSqlite.__mDatabaseCreate() == false)
                return; /// База данных не создана/недоступна - причина уже записана в 'oDataSourceSqlite.__fLastError_' и в локальный лог; создавать таблицы бессмысленно, они всё равно не создадутся на той же неоткрытой связи


            /// Создание таблицы 'App' - Приложения
            if (oDataSourceSqlite.__mTableExists("App") == false)
            {
                string vQuery = "CREATE TABLE App ("
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "desApp TEXT,"
                                + "dpnApp TEXT,"
                                + "Pfx TEXT,"
                                + "PRIMARY KEY('CLU'))";
                oDataSourceSqlite.__mSqlCommand(vQuery);
            }
            /// Регистрация приложения в базе данных (только если ещё не зарегистрировано)
            if (oDataSourceSqlite.__mSqlCount("App", "desApp = '" + datApplication.__fProcessName_ + "'") == 0)
            {
                string vCommand = "Insert Into App (desApp, dpnApp, Pfx)"
                                  + " Values("
                                  + "'" + datApplication.__fProcessName_ + "'"
                                  + ",'" + datApplication.__fDescription_ + "'"
                                  + ",'" + datApplication.__fPrefix_ + "')";
                oDataSourceSqlite.__mSqlCommand(vCommand);

            }
            /// Создание таблицы 'Pcl' - Протоколы
            if (oDataSourceSqlite.__mTableExists("Pcl") == false)
            {
                string vQuery = "CREATE TABLE Pcl("
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "CHG TEXT,"
                                + "lnkApp INTEGER,"
                                + "lnkPclTyp INTEGER,"
                                + "FilPrnScr TEXT,"
                                + "Hst TEXT,"
                                + "Prc TEXT,"
                                + "Usr TEXT,"
                                + "GID TEXT,"
                                + "PRIMARY KEY('CLU' AUTOINCREMENT))";
                oDataSourceSqlite.__mSqlCommand(vQuery);
            }
            else
            {
                oDataSourceSqlite.__mSqlCommand("ALTER TABLE Pcl ADD COLUMN GID TEXT"); // Миграция для БД, созданных до появления GID - если колонка уже есть, команда безопасно попадёт в локальный журнал сбоев и будет проигнорирована
            }
            /// Создание таблицы "PclTyp" - Виды протоколов
            if (oDataSourceSqlite.__mTableExists("PclTyp") == false)
            {
                string vQuery = "CREATE TABLE PclTyp("
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "desPclTyp TEXT,"
                                + "optAplErr INTEGER,"
                                + "optAplErrPgr INTEGER,"
                                + "optAplExc INTEGER,"
                                + "optAplEve INTEGER,"
                                + "optDatErr INTEGER,"
                                + "optDatEve INTEGER,"
                                + "optDevErr INTEGER,"
                                + "optDevEve INTEGER,"
                                + "optUsrErr INTEGER,"
                                + "optUsrEve INTEGER,"
                                + "optUsrMsg INTEGER,"
                                + "optOth INTEGER,"
                                + "PRIMARY KEY('CLU' AUTOINCREMENT))";
                oDataSourceSqlite.__mSqlCommand(vQuery);
                string vCommand = "";
                vCommand = "Insert Into PclTyp (desPclTyp,optAplErr,optAplErrPgr,optAplExc,optAplEve,optDatErr,optDatEve,optDevErr,optDevEve,optUsrErr,optUsrEve,optUsrMsg,optOth)"
                           + " Values("
                           + "'Ошибка приложения'"
                           + ",1"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into PclTyp (desPclTyp,optAplErr,optAplErrPgr,optAplExc,optAplEve,optDatErr,optDatEve,optDevErr,optDevEve,optUsrErr,optUsrEve,optUsrMsg,optOth)"
                           + " Values("
                           + "'Ошибка программирования'"
                           + ",0"
                           + ",1"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into PclTyp (desPclTyp,optAplErr,optAplErrPgr,optAplExc,optAplEve,optDatErr,optDatEve,optDevErr,optDevEve,optUsrErr,optUsrEve,optUsrMsg,optOth)"
                           + " Values("
                           + "'Исключение'"
                           + ",0"
                           + ",0"
                           + ",1"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into PclTyp (desPclTyp,optAplErr,optAplErrPgr,optAplExc,optAplEve,optDatErr,optDatEve,optDevErr,optDevEve,optUsrErr,optUsrEve,optUsrMsg,optOth)"
                           + " Values("
                           + "'Событие приложения'"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",1"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into PclTyp (desPclTyp,optAplErr,optAplErrPgr,optAplExc,optAplEve,optDatErr,optDatEve,optDevErr,optDevEve,optUsrErr,optUsrEve,optUsrMsg,optOth)"
                           + " Values("
                           + "'Ошибка источника данных'"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",1"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into PclTyp (desPclTyp,optAplErr,optAplErrPgr,optAplExc,optAplEve,optDatErr,optDatEve,optDevErr,optDevEve,optUsrErr,optUsrEve,optUsrMsg,optOth)"
                           + " Values("
                           + "'Ошибка устройства'"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",1"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into PclTyp (desPclTyp,optAplErr,optAplErrPgr,optAplExc,optAplEve,optDatErr,optDatEve,optDevErr,optDevEve,optUsrErr,optUsrEve,optUsrMsg,optOth)"
                           + " Values("
                           + "'События устройства'"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",1"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into PclTyp (desPclTyp,optAplErr,optAplErrPgr,optAplExc,optAplEve,optDatErr,optDatEve,optDevErr,optDevEve,optUsrErr,optUsrEve,optUsrMsg,optOth)"
                           + " Values("
                           + "'Ошибка пользователя'"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",1"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into PclTyp (desPclTyp,optAplErr,optAplErrPgr,optAplExc,optAplEve,optDatErr,optDatEve,optDevErr,optDevEve,optUsrErr,optUsrEve,optUsrMsg,optOth)"
                           + " Values("
                           + "'События пользователя'"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",1"
                           + ",0"
                           + ",0"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into PclTyp (desPclTyp,optAplErr,optAplErrPgr,optAplExc,optAplEve,optDatErr,optDatEve,optDevErr,optDevEve,optUsrErr,optUsrEve,optUsrMsg,optOth)"
                           + " Values("
                           + "'Сообщения показанные пользователю'"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",1"
                           + ",0"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into PclTyp (desPclTyp,optAplErr,optAplErrPgr,optAplExc,optAplEve,optDatErr,optDatEve,optDevErr,optDevEve,optUsrErr,optUsrEve,optUsrMsg,optOth)"
                           + " Values("
                           + "'Сообщения показанные пользователю'"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",1"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into PclTyp (desPclTyp,optAplErr,optAplErrPgr,optAplExc,optAplEve,optDatErr,optDatEve,optDevErr,optDevEve,optUsrErr,optUsrEve,optUsrMsg,optOth)"
                           + " Values("
                           + "'Прочее'"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",1)";
                oDataSourceSqlite.__mSqlCommand(vCommand);

            }
            ///
            if (oDataSourceSqlite.__mTableExists("PclRrd") == false)
            {
                string vQuery = "CREATE TABLE PclRrd ("
                                + "CLU INTEGER NOT NULL UNIQUE"
                                + ", lnkPcl INTEGER"
                                + ", lnkRrdTyp INTEGER"
                                + ", Msg TEXT"
                                + ", Tck INTEGER"
                                + ", GID TEXT"
                                + ", PRIMARY KEY(CLU AUTOINCREMENT));";
                oDataSourceSqlite.__mSqlCommand(vQuery);
            }
            else
            {
                oDataSourceSqlite.__mSqlCommand("ALTER TABLE PclRrd ADD COLUMN GID TEXT"); // Миграция для БД, созданных до появления GID
            }
            /// Создание таблицы "RrdTyp" - Записи в протоколе
            if (oDataSourceSqlite.__mTableExists("RrdTyp") == false)
            {
                string vQuery = "CREATE TABLE RrdTyp("
                                + "CLU INTEGER NOT NULL UNIQUE,"
                                + "desRrdTyp TEXT,"
                                + "optAns TEXT,"
                                + "optDet TEXT,"
                                + "optExc TEXT,"
                                + "optImg TEXT,"
                                + "optMsg TEXT,"
                                + "optPrp TEXT,"
                                + "PRIMARY KEY('CLU'))";
                oDataSourceSqlite.__mSqlCommand(vQuery);
                string vCommand = "";
                vCommand = "Insert Into RrdTyp (desRrdTyp,optAns,optDet,optExc,optImg,optMsg,optPrp)"
                           + " Values("
                           + "'Решение пользователя'"
                           + ",1"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into RrdTyp (desRrdTyp,optAns,optDet,optExc,optImg,optMsg,optPrp)"
                           + " Values("
                           + " 'Детали'"
                           + ",0"
                           + ",1"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into RrdTyp (desRrdTyp,optAns,optDet,optExc,optImg,optMsg,optPrp)"
                           + " Values("
                           + "'Исключение'"
                           + ",0"
                           + ",0"
                           + ",1"
                           + ",0"
                           + ",0"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into RrdTyp (desRrdTyp,optAns,optDet,optExc,optImg,optMsg,optPrp)"
                           + " Values("
                           + "'Изображение'"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",1"
                           + ",0"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into RrdTyp (desRrdTyp,optAns,optDet,optExc,optImg,optMsg,optPrp)"
                           + " Values("
                           + "'Сообщение'"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",1"
                           + ",0)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
                vCommand = "Insert Into RrdTyp (desRrdTyp,optAns,optDet,optExc,optImg,optMsg,optPrp)"
                           + " Values("
                           + "'Свойства объекта'"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",0"
                           + ",1)";
                oDataSourceSqlite.__mSqlCommand(vCommand);
            }
            /// Создание таблицы 'ImportedGid' - отслеживание уже импортированных строк из легаси '.pcl' файлов (защита от повторного импорта)
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
        /// Проверка, был ли уже импортирован конкретный 'GID' из легаси '.pcl' файла
        /// </summary>
        /// <param name="pGid">Идентификатор строки (GID) из '.pcl' файла</param>
        /// <returns>[true] - строка с таким GID уже импортирована</returns>
        public bool __mProtocolIsImported(string pGid)
        {
            return oDataSourceSqlite.__mSqlCount("ImportedGid", "GID = '" + pGid + "'") > 0;
        }
        /// <summary>
        /// Пакетное получение соответствия ВСЕХ уже импортированных 'GID' их 'lnkPcl' одним запросом - для
        /// использования вместо многократных вызовов '__mProtocolIsImported'/'__mProtocolClueByGid' в цикле
        /// по строкам большого файла (при импорте файла с десятками тысяч строк проверка и поиск CLU на
        /// каждую строку превращались в десятки тысяч отдельных обращений к базе данных, что и приводило
        /// к катастрофическому замедлению и сбоям импорта больших/повреждённых файлов)
        /// </summary>
        /// <returns>Словарь [GID] -&gt; [lnkPcl] (для записей 'rrd.pcl' значение равно -1, см. '__mProtocolMarkImported')</returns>
        public Dictionary<string, int> __mImportedGidCluMapGet()
        {
            Dictionary<string, int> vReturn = new Dictionary<string, int>();
            DataTable vDataTable = oDataSourceSqlite.__mSqlQuery("Select GID, lnkPcl From ImportedGid");

            if (vDataTable != null)
            {
                foreach (DataRow vDataRow in vDataTable.Rows)
                {
                    string vGid = Convert.ToString(vDataRow["GID"]);
                    if (vReturn.ContainsKey(vGid) == false)
                        vReturn.Add(vGid, Convert.ToInt32(vDataRow["lnkPcl"]));
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Начало транзакции - см. 'dsqDataSourceSqliteWithProtocol.__mTransactionBegin'
        /// </summary>
        //public bool __mTransactionBegin()
        //{
        //    return oDataSourceSqlite.__mTransactionBegin();
        //}
        ///// <summary>
        ///// Подтверждение транзакции - см. 'dsqDataSourceSqliteWithProtocol.__mTransactionCommit'
        ///// </summary>
        //public bool __mTransactionCommit()
        //{
        //    return oDataSourceSqlite.__mTransactionCommit();
        //}
        ///// <summary>
        ///// Откат транзакции - см. 'dsqDataSourceSqliteWithProtocol.__mTransactionRollback'
        ///// </summary>
        //public void __mTransactionRollback()
        //{
        //    oDataSourceSqlite.__mTransactionRollback();
        //}
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
            int vProtocolTypClue = mProtocolTypeClueByRawId(pProtocolType);

            string vCommand = "Insert Into Pcl (CHG, lnkApp, lnkPclTyp, FilPrnScr, Hst, Prc, Usr, GID)"
                              + " Values("
                              + "'" + pChgTicks.ToString() + "'"
                              + ", " + vApplicationClue.ToString()
                              + ", " + vProtocolTypClue.ToString()
                              + ", ''"
                              + ", '" + pHost + "'"
                              + ", '" + pProcedure + "'"
                              + ", '" + pUser + "'"
                              + ", '" + pGid + "')";
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

            string vCommand = "Insert Into PclRrd(lnkPcl, lnkRrdTyp, Msg, Tck, GID)"
                              + " Values("
                              + pLnkPcl.ToString()
                              + ", " + vRecordTypClue.ToString()
                              + ", '" + pMessage.Replace("'", "''") + "'"
                              + ", " + pTick.ToString()
                              + ", '" + pGid + "')";
            oDataSourceSqlite.__mSqlCommand(vCommand);
        }
        /// <summary>
        /// Поиск (или создание) строки приложения-источника по имени - для приложений, отличных от текущего запущенного
        /// (легаси файлы протоколов могли быть созданы другими приложениями - Administration.exe, csManual.exe и т.д.)
        /// </summary>
        /// <param name="pAppName">Имя приложения</param>
        /// <param name="pAppDescription">Описание приложения</param>
        /// <param name="pAppPrefix">Префикс приложения</param>
        /// <returns>Реальный CLU строки в 'App'</returns>
        private int mApplicationEnsure(string pAppName, string pAppDescription, string pAppPrefix)
        {
            object vExisting = oDataSourceSqlite.__mSqlValue("App", "CLU", "desApp = '" + pAppName + "'");
            if (vExisting != null && vExisting != DBNull.Value)
                return Convert.ToInt32(vExisting);

            string vCommand = "Insert Into App (desApp, dpnApp, Pfx)"
                              + " Values("
                              + "'" + pAppName + "'"
                              + ",'" + (pAppDescription ?? "") + "'"
                              + ",'" + (pAppPrefix ?? "") + "')";
            oDataSourceSqlite.__mSqlCommand(vCommand);

            return oDataSourceSqlite.__mClueLastInserted("App");
        }
        /// <summary>
        /// Соответствие сырого числового id вида протокола, как он записан в легаси '.pcl' файле, реальному CLU в 'PclTyp'.
        /// </summary>
        /// <remarks>ВАЖНО: 'appProtocols.__mCreate' (реальный код, которым были написаны легаси '.pcl' файлы) пишет в файл
        /// уже готовое число 1-12 через собственный switch (ApplicationError=1 .. Other=12), а НЕ сырое значение enum (0-11).
        /// Это число уже точно совпадает с реальными CLU в 'PclTyp' (см. 'mProtocolTypeClue') - реинтерпретация через
        /// приведение к enum здесь была бы ошибкой (сдвинула бы значение на единицу неверно).</remarks>
        private int mProtocolTypeClueByRawId(int pRawId)
        {
            if (pRawId >= 1 && pRawId <= 12)
                return pRawId;
            return 1;
        }
        /// <summary>
        /// Соответствие сырого числового id вида записи протокола реальному CLU в 'RrdTyp' - для импорта легаси файлов
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

        #region = СВОЙСТВА

        /// <summary>
        /// Текст последнего сбоя SQL-операции - см. примечание к 'dsqDataSourceSqliteWithProtocol.__fLastError_'
        /// </summary>
        //public string __fLastError_
        //{
        //    get { return oDataSourceSqlite.__fLastError_; }
        //}

        #endregion СВОЙСТВА
    }
}