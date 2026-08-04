using nlApplication;
using nlData;
using System;

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
            oDataSourceSqlite.__fDatabaseName = "protocols.db";
            oDataSourceSqlite.__fDatabasePath = __fDatabasePath;

            //__mTablesFill();

            return;
        }

        #endregion Объект

        #region - Процедуры

        public override void __mCreate(PROTOCOLSTYPES pProtocolType, string pProcedure, bool pPrintScreen = false)
        {
            long vPclClu = DateTime.Now.Ticks; //
            int vApplicationClue = Convert.ToInt32(oDataSourceSqlite.__mSqlValue("App", "CLU", "desApp = '" + appApplication.__fProcessName_ + "'"));
            int vProtocolTyp = (int) pProtocolType; // Идентификатор вида протокола
            //int vApplicationClue = oDataSourceSqlite.__mSqlValue("App", "CLU", "desApp = " + )
            string vPrintScreenFile = "";

            if (pPrintScreen == true)
            {
                vPrintScreenFile = __mPrintScreen();
            }

            string vCommand = "Insert Into Pcl (CHG, lnkApp, lnkPclTyp, FilPrnScr, Hst, Prc, Usr)"
                              + " Values(" 
                              + "'" + DateTime.Now.Ticks.ToString() + "'"
                              + ", " + vProtocolTyp.ToString()
                              + ", " + ((int) pProtocolType).ToString()
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
                              + ", " + ((int) pRecordType).ToString()
                              + ", '" + pRecordText + "'"
                              + ", " + pTick.ToString() + ")";
            oDataSourceSqlite.__mSqlCommand(vCommand);
        }

        public void __mTablesFill()
        {
            oDataSourceSqlite.__mDatabaseCreate();

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
            /// Регистрация приложения в базе данных
            if (oDataSourceSqlite.__mSqlCount("App", "desApp = " + datApplication.__fProcessName_) != 0)
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
                                + "PRIMARY KEY('CLU' AUTOINCREMENT))";
                oDataSourceSqlite.__mSqlCommand(vQuery);
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
                                + ", PRIMARY KEY(CLU AUTOINCREMENT));";
                oDataSourceSqlite.__mSqlCommand(vQuery);
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
