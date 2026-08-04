using nlApplication;
using nlData;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace naKvinto4ka
{
    /// <summary>
    /// Файл kvlUnitDataSource.cs
    /// </summary>
    /// <remarks>Класс-источник данных 'Oracle'</remarks>
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 15-24</version> // Дата-время последней корректировки
    public class kvtUnitDataSource 
    {
        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Сборка объект
        /// </summary>
        protected void _mObjectAssembly()
        {
            __fServer = "UNIACC_ORA2.KVINT.MD";
            __fServerLogin = "KVINT4";
            __fServerPassword = "KVINT4";
        }

        #endregion Поведение

        #region - Процедуры

        #region Sql операции

        /// <summary>
        /// Отправка команды источнику данных
        /// </summary>
        /// <param name="pCommand">Команда отправляемая источнику данных</param>
        /// <returns>Количество обработанных командой записей</returns>
        public int __mSqlCommand(string pCommand)
        {
            int vReturn = -1; // Возвращаемое значение

            appUnitError vError = new appUnitError();
            vError.__fErrorType_ = ERRORSTYPES.Programming;
            vError.__fProcedure_ = _fClassNameFull + "__mSqlCommand(string)";
            vError.__mPropertyAdd("Команда{0} {1}", ":", pCommand);

            if (String.IsNullOrEmpty(pCommand) == true)
            {
                vError.__mReasonAdd("Не указана команда");
                appApplication.__oErrorsHandler.__mShow(vError);
                return vReturn;
            }

            bool vTransactionUsed = false; // Использование транзакции

            try
            {
                if (__fOnLine == false & fConnection == null)
                {
                    __mConnectionOn(); /// Установка соединения
                }

                OracleCommand vSqlCommand = new OracleCommand(pCommand, fConnection);
                vSqlCommand.CommandType = CommandType.Text;

                /// Если открыта транзакция команда включается в транзакцию
                if (fTransaction != null)
                {
                    vSqlCommand.Transaction = fTransaction;
                    vTransactionUsed = true;
                }

                vReturn = vSqlCommand.ExecuteNonQuery();

                /// Если транзакция отсутствует, выполняется разрыв соединения
                if (fTransaction == null)
                {
                    if (__fOnLine == false & fConnection != null)
                        __mConnectionOff();
                }
            }
            catch (OracleException vException)
            {
                vError.__fException = vException;
                vError.__fErrorType_ = ERRORSTYPES.Data;
                vError.__fMessage_ = "Oracle сервер не может выполнить команду";
                vError.__mPropertyAdd("Сервер: {0}", __fServer);
                vError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                vError.__mPropertyAdd("Содержание команды: {0}", pCommand);
                vError.__mPropertyAdd("Транзакция: {0}", vTransactionUsed.ToString());
                vError.__mPropertyAdd("Команда{0} {1}", ":", pCommand);
                appApplication.__oErrorsHandler.__mShow(vError);
            }

            return vReturn;
        }
        /// <summary>
        /// Выполнение функции пакета источника данных
        /// </summary>
        /// <param name="pStoredProcedure">Название функции пакета</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных</param>
        /// <param name="pParameters">Список параметров</param>
        public object __mSqlExecutePackage(string pPackageFunction, params object[] pParameters)
        {
            object vReturn = null;

            appUnitError vError = new appUnitError();
            vError.__fErrorType_ = ERRORSTYPES.Programming;
            vError.__fProcedure_ = _fClassNameFull + "__mSqlExecutePackage(string, params object[])";
            vError.__mPropertyAdd("Функция пакета{0} {1}", ":", pPackageFunction);

            if (String.IsNullOrEmpty(pPackageFunction) == true)
            {
                vError.__mReasonAdd("Не указана функция пакета");
                appApplication.__oErrorsHandler.__mShow(vError);
                return null;
            }

            bool vTransactionUsed = false; // Использование транзакции

            try
            {
                if (__fOnLine == false & fConnection == null)
                {
                    __mConnectionOn(); /// Установка соединения
                }

                OracleCommand vSqlCommand = new OracleCommand(pPackageFunction, fConnection);
                vSqlCommand.CommandType = CommandType.StoredProcedure;
                /// Добавление параметров
                for (int vAmount = 0; vAmount < pParameters.Length; vAmount++)
                {
                    if ((pParameters[vAmount] is appUnitItem) == true)
                    {
                        switch ((pParameters[vAmount] as appUnitItem).__fType_.Name)
                        {
                            case "DateTime":
                                OracleParameter vParameterDateTime = new OracleParameter();
                                vParameterDateTime.DbType = DbType.DateTime;
                                vParameterDateTime.ParameterName = (pParameters[vAmount] as appUnitItem).__fDesignation_;
                                vParameterDateTime.Value = Convert.ToDateTime((pParameters[vAmount] as appUnitItem).__fValue_);
                                vSqlCommand.Parameters.Add(vParameterDateTime);
                                break;
                            case "Int32":
                                OracleParameter vParameterInt = new OracleParameter();
                                vParameterInt.DbType = DbType.Int32;
                                vParameterInt.ParameterName = (pParameters[vAmount] as appUnitItem).__fDesignation_;
                                vParameterInt.Value = Convert.ToInt32((pParameters[vAmount] as appUnitItem).__fValue_);
                                vSqlCommand.Parameters.Add(vParameterInt);
                                break;
                            case "String":
                                OracleParameter vParameterString = new OracleParameter();
                                vParameterString.DbType = DbType.String;
                                vParameterString.ParameterName = (pParameters[vAmount] as appUnitItem).__fDesignation_;
                                vParameterString.Value = Convert.ToString((pParameters[vAmount] as appUnitItem).__fValue_);
                                vSqlCommand.Parameters.Add(vParameterString);
                                break;
                        }
                    }
                }

                /// Если открыта транзакция команда включается в транзакцию
                if (fTransaction != null)
                {
                    vSqlCommand.Transaction = fTransaction;
                    vTransactionUsed = true;
                }

                vReturn = vSqlCommand.ExecuteScalar();

                /// Если транзакция отсутствует, выполняется разрыв соединения
                if (fTransaction == null)
                {
                    if (__fOnLine == false & fConnection != null)
                        __mConnectionOff();
                }
            }
            catch (OracleException vException)
            {
                vError.__fException = vException;
                vError.__fErrorType_ = ERRORSTYPES.Data;
                vError.__fMessage_ = "Oracle сервер не может выполнить функцию пакета";
                vError.__mPropertyAdd("Сервер: {0}", __fServer);
                vError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                vError.__mPropertyAdd("Транзакция: {0}", vTransactionUsed.ToString());
                appApplication.__oErrorsHandler.__mShow(vError);
            }
            return vReturn;
        }
        /// <summary>
        /// Отправка запроса источнику данных
        /// </summary>
        /// <param name="pQuery">Условие запроса</param>
        /// <returns>{DataTable} - с данными удовлетворяющими условию "pQuer"</returns>
        public DataTable __mSqlQuery(string pQuery)
        {
            DataTable vDataTable = null; // Возвращаемое значение
            bool vTransactionUsed = false; // Использование транзакции
            try
            {
                /// Установка соединения
                if (__fOnLine == false & fConnection == null)
                {
                    __mConnectionOn();
                }
                OracleCommand vSqlCommand = new OracleCommand(pQuery, fConnection);
                vSqlCommand.CommandType = CommandType.Text;

                OracleDataReader vSqlDataReader = vSqlCommand.ExecuteReader();
                vDataTable = new DataTable();
                vDataTable.Load(vSqlDataReader);

                if (__fOnLine == false & fConnection != null)
                    __mConnectionOff();
            }
            catch (OracleException vException)
            {
                appUnitError vError = new appUnitError();
                vError.__fException = vException;
                vError.__fProcedure_ = _fClassNameFull + "_sqlQuery(string)";
                vError.__fErrorType_ = ERRORSTYPES.Data;
                vError.__mMessageBuild("Ошибка при выполнении запроса");
                vError.__mPropertyAdd("Сервер: {0}", __fServer);
                vError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                vError.__mPropertyAdd("Содержание запроса: {0}", pQuery);
                vError.__mPropertyAdd("Транзакция: {0}", vTransactionUsed.ToString());

                appApplication.__oErrorsHandler.__mShow(vError);
            }

            return vDataTable;
        }
        /// <summary>
        /// Выполнение хранимой процедуры
        /// </summary>
        /// <param name="pStoredProcedure">Название хранимой процедуры</param>
        /// <param name="pParameters">Параметры хранимой процедуры</param>
        /// <returns></returns>
        public DataTable __mSqlExecuteStoredProcedure(string pStoredProcedure, params object[] pParameters)
        {
            DataTable vReturn = new DataTable(); // Возвращаемое значение

            appUnitError vError = new appUnitError();
            vError.__fErrorType_ = ERRORSTYPES.Programming;
            vError.__fProcedure_ = _fClassNameFull + "__mSqlExecuteStoredProcedure(string, params object[])";
            vError.__mPropertyAdd("Хранимая процедура{0} {1}", ":", pStoredProcedure);

            if (String.IsNullOrEmpty(pStoredProcedure) == true)
            {
                vError.__mReasonAdd("Не указана хранимая процедура");
                appApplication.__oErrorsHandler.__mShow(vError);
                return null;
            }

            bool vTransactionUsed = false; // Использование транзакции

            try
            {
                if (__fOnLine == false & fConnection == null)
                {
                    __mConnectionOn(); /// Установка соединения
                }

                OracleCommand vSqlCommand = new OracleCommand(pStoredProcedure, fConnection);
                vSqlCommand.CommandType = CommandType.StoredProcedure;
                /// Добавление параметров
                for (int vAmount = 0; vAmount < pParameters.Length; vAmount++)
                {
                    if ((pParameters[vAmount] is appUnitItem) == true)
                    {
                        switch ((pParameters[vAmount] as appUnitItem).__fType_.Name)
                        {
                            case "DateTime":
                                OracleParameter vParameterDateTime = new OracleParameter();
                                vParameterDateTime.DbType = DbType.DateTime;
                                vParameterDateTime.ParameterName = (pParameters[vAmount] as appUnitItem).__fDesignation_;
                                vParameterDateTime.Value = Convert.ToDateTime((pParameters[vAmount] as appUnitItem).__fValue_);
                                vSqlCommand.Parameters.Add(vParameterDateTime);
                                break;
                            case "Int32":
                                OracleParameter vParameterInt = new OracleParameter();
                                vParameterInt.DbType = DbType.Int32;
                                vParameterInt.ParameterName = (pParameters[vAmount] as appUnitItem).__fDesignation_;
                                vParameterInt.Value = Convert.ToInt32((pParameters[vAmount] as appUnitItem).__fValue_);
                                vSqlCommand.Parameters.Add(vParameterInt);
                                break;
                            case "String":
                                OracleParameter vParameterString = new OracleParameter();
                                vParameterString.DbType = DbType.String;
                                vParameterString.ParameterName = (pParameters[vAmount] as appUnitItem).__fDesignation_;
                                vParameterString.Value = Convert.ToString((pParameters[vAmount] as appUnitItem).__fValue_);
                                vSqlCommand.Parameters.Add(vParameterString);
                                break;
                        }
                    }
                }

                /// Если открыта транзакция команда включается в транзакцию
                if (fTransaction != null)
                {
                    vSqlCommand.Transaction = fTransaction;
                    vTransactionUsed = true;
                }

                OracleDataAdapter vSqlDataAdapter = new OracleDataAdapter(vSqlCommand);
                vSqlDataAdapter.Fill(vReturn);
                /// Если транзакция отсутствует, выполняется разрыв соединения
                if (fTransaction == null)
                {
                    if (__fOnLine == false & fConnection != null)
                        __mConnectionOff();
                }
            }
            catch (OracleException vException)
            {
                vError.__fException = vException;
                vError.__fErrorType_ = ERRORSTYPES.Data;
                vError.__fMessage_ = "Oracle сервер не может выполнить хранимую процедуру";
                vError.__mPropertyAdd("Сервер: {0}", __fServer);
                vError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                vError.__mPropertyAdd("Транзакция: {0}", vTransactionUsed.ToString());
                appApplication.__oErrorsHandler.__mShow(vError);
            }

            return vReturn;

        }
        /// <summary>
        /// Получение значения поля удовлетворяющего команде. Заполняется в предметном источнике данных 
        /// </summary>
        /// <param name="pCommand">Команда для получения значения поля</param>
        /// <param name="pParameters">Параметры хранимой процедуры</param>
        /// <returns>{object} - значение поля</returns>
        public object __mSqlValue(string pCommand)
        {
            object vReturn = null; // Возвращаемое значение
            bool vTransactionUsed = false; // Использование транзакции
            try
            {
                if (__fOnLine == false & fConnection == null)
                {
                    __mConnectionOn();
                }  /// Установка соединения
                OracleCommand vSqlCommand = new OracleCommand(pCommand, fConnection);

                if (fTransaction != null)
                {
                    vSqlCommand.Transaction = fTransaction;
                    vTransactionUsed = true;
                } /// Открыта транзакция
                vReturn = vSqlCommand.ExecuteScalar();
                if (fTransaction == null)
                {
                    if (__fOnLine == false & fConnection != null)
                        __mConnectionOff();
                } /// Если транзакция отсутствует, выполняется разрыв соединения
            }
            catch (OracleException vException)
            {
                appUnitError vError = new appUnitError();
                vError.__fException = vException;
                vError.__fProcedure_ = _fClassNameFull + "_Value(string)";
                vError.__fErrorType_ = ERRORSTYPES.Data;
                vError.__mMessageBuild("Не возможно получить значение");
                vError.__mPropertyAdd("Сервер: {0}", __fServer);
                vError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                vError.__mPropertyAdd("Содержание команды: {0}", pCommand);
                vError.__mPropertyAdd("Транзакция: {0}", vTransactionUsed.ToString());

                appApplication.__oErrorsHandler.__mShow(vError);
            }

            return vReturn;
        }
        /// <summary>
        /// Получение значения поля по имени таблицы, имени поля и условию
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldName">Название поля</param>
        /// <param name="pExpressionWhere">Условие поиска записи</param>
        /// <returns>[null] - если значение не найдено, иначе - {object} - значение поля</returns>
        public object __mSqlValue(string pTableName, string pFieldName, string pExpressionWhere)
        {
            string vCommand = "Select " + pFieldName + " From " + pTableName + " Where " + pExpressionWhere;
            return __mSqlValue(vCommand);
        }

        #endregion Sql операции

        #region Подключение

        /// <summary>
        /// Построение строки подключения к источнику данных
        /// </summary>
        /// <returns>[true] - строка построена, иначе - [false]</returns>
        protected bool __mConnectionLineBuild()
        {
            bool vReturn = true; // Возвращаемое значение

            /// Сброс строки подключения
            if (__fServer.Length == 0) /// Имя сервера не указано
                vReturn = vReturn & false;
            //if (__fDatabaseName.Length == 0) /// Имя базы данных не указано
            //    vReturn = vReturn & false;

            /// Использовать логин для подключения
            if (__fServerLogin.Length == 0) /// Логин не указан
                vReturn = vReturn & false;
            if (__fServerPassword.Length == 0) /// Пароль не указан
                vReturn = vReturn & false;

            #region old

            ////DATA SOURCE=XE;DBA PRIVILEGE=SYSDBA;USER ID=SYS
            //OracleConnectionStringBuilder vOracleConnectionStringBuilder = new OracleConnectionStringBuilder();
            //vOracleConnectionStringBuilder.DataSource = __fServer; ;
            //vOracleConnectionStringBuilder.DBAPrivilege = "SYSDBA";
            //vOracleConnectionStringBuilder.UserID = __fServerLogin;   // Добавить авторизацию
            //vOracleConnectionStringBuilder.Password = __fServerPassword;
            //vOracleConnectionStringBuilder.ConnectionTimeout = 10000;
            //__fConnectionLine = vOracleConnectionStringBuilder.ConnectionString;
            //// USER ID=KVINT4;DBA PRIVILEGE=SYSDBA;DATA SOURCE=UNIACC_ORA2.KVINT.MD;PASSWORD=KVINT4 
            ////__fConnectionLine = "Data Source = " + __fServer + "; User Id = " + __fServerLogin + "; password = " + __fServerPassword; // ; Unicode = True
            ////__fConnectionLine = "DataSource = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = KVINT4.UNIACC_ORA2.KVINT.MD)(PORT = 1521))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = ORCL))); User_Id = " + __fServerLogin + "; password = " + __fServerPassword + ";";
            ////__fConnectionLine = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = UNIACC_ORA2.KVINT.MD)(PORT = 1521))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = ORCL))); User Id = " + __fServerLogin + "; password = " + __fServerPassword + ";";

            //// рабочая строка подключения к главной базе данных __fConnectionLine = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = ORA2.KVINT.MD)(PORT = 1521))) (CONNECT_DATA = (SID = UNIACC) (SERVER = DEDICATED))); User Id = " + __fServerLogin + "; password = " + __fServerPassword + ";";

            ////__fConnectionLine = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP) (HOST = fronto)(PORT = 1521)) (CONNECT_DATA = (SERVICE_NAME = XE))); User Id = " + __fServerLogin + "; password = " + __fServerPassword + ";";

            #endregion old

            __fConnectionLine = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP) (HOST = fronto)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = XE)));User Id=POS;Password=POS";
            
            return vReturn;
        }
        /// <summary>
        /// Разрыв соединения с источником данных
        /// </summary>
        protected bool __mConnectionOff()
        {
            bool vReturn = true; // Возвращаемое значение

            if (fConnection != null)
            {
                try
                {
                    fConnection.Close();
                    fConnection.Dispose();
                    fConnection = null;
                }
                catch (Exception vException)
                {
                    appUnitError vError = new appUnitError();
                    vError.__fException = vException;
                    vError.__fProcedure_ = _fClassNameFull + "_ConnectionOff()";
                    vError.__fErrorType_ = ERRORSTYPES.Data;
                    vError.__mPropertyAdd("Сервер: {0}", __fServer);
                    vError.__mPropertyAdd("Логин: {0}", __fServerLogin);

                    appApplication.__oErrorsHandler.__mShow(vError);

                    vReturn = false;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Установка соединения с источником данных
        /// </summary>
        /// <returns>[true] - соединение установлено, иначе - [false]</returns>
        protected bool __mConnectionOn()
        {
            bool vReturn = true; // Возвращаемое значение

            if (__fConnectionLine.Length == 0)
            {
                __mConnectionLineBuild();
            }
            try
            {
                fConnection = new OracleConnection(__fConnectionLine);
                fConnection.Open();
            }
            catch (OracleException vException)
            {
                appUnitError vError = new appUnitError();
                vError.__fException = vException;
                vError.__fProcedure_ = _fClassNameFull + "_ConnectionOn()";
                vError.__fErrorType_ = ERRORSTYPES.Data;
                vError.__mPropertyAdd("Строка подключения: {0}", __fConnectionLine);
                vError.__mPropertyAdd("Сервер: {0}", __fServer);
                vError.__mPropertyAdd("Логин: {0}", __fServerLogin);

                appApplication.__oErrorsHandler.__mShow(vError);

                vReturn = false;
            }

            return vReturn;
        }

        #endregion Подключение

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Псевдоним источника данных
        /// </summary>
        public string __fAlias = "";
        /// <summary>
        /// Строка подключения к источнику данных
        /// </summary>
        public string __fConnectionLine = "";
        /// <summary>
        /// Вид хранения даты
        /// </summary>
        public DATETIMESTORE __fDateTimeStore = DATETIMESTORE.DateTime;
        /// <summary>
        /// Работа в режиме не разрывного соедиения 
        /// </summary>
        public bool __fOnLine = false;
        /// <summary>
        /// Название сервера
        /// </summary>
        public string __fServer = "";
        /// <summary>
        /// Имя входа на сервер
        /// </summary>
        public string __fServerLogin = "";
        /// <summary>
        /// Пароль входа на сервер
        /// </summary>
        public string __fServerPassword = "";
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string __fUserLogin = "";
        /// <summary>
        /// Код пользователя
        /// </summary>
        public int __fUserCode = -1;
        /// <summary>
        /// Пользователь - администратор
        /// </summary>
        public bool __fUserIsAdministrator = false;

        #endregion Атрибуты

        #region - Внутренние

        /// <summary>
        /// Полное имя класса
        /// </summary>
        protected string _fClassNameFull = "";
        /// <summary>
        /// Указатель на соединение с источником данных
        /// </summary>
        private OracleConnection fConnection = null;
        /// <summary>
        /// Указатель на открытую транзакцию
        /// </summary>
        private OracleTransaction fTransaction = null;
        /// <summary>
        /// Указатель на объединяющую транзакцию
        /// </summary>
        private OracleTransaction fTransactionUnion = null;

        #endregion Внутренние

        #endregion ПОЛЯ
    }
}
