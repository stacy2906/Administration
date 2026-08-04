using nlApplication;
using nlData;
using nlSystem;
using System;
using System.Collections;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace nlDataSourceMsSql
{
    /// <summary>
    /// Файл dmsDataSourceMsSql.cs
    /// </summary>
    /// <remarks>Класс-источник данных 'MS Sql'</remarks>
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 15-06</version> // Дата-время последней корректировки
    public class dmsDataSourceMsSql : datUnitDataSource
    {
        #region = МЕТОДЫ

        #region - Поведение

        #region Объект

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            base._mObjectAssembly();

            __fDataSourceType = DATASOURCETYPES.MsSql;

            return;
        }

        #endregion Объект

        #endregion Поведение

        #region - Процедуры

        #region Sql операции

        /// <summary>
        /// Отправка команды источнику данных
        /// </summary>
        /// <param name="pCommand">Команда отправляемая источнику данных</param>
        /// <returns>Количество обработанных командой записей</returns>
        public override int __mSqlCommand(string pCommand)
        {
            int vReturn = -1; // Возвращаемое значение

            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_ ;
            _fError.__mPropertyAdd("Команда{0} {1}", ":", pCommand);

            if (String.IsNullOrEmpty(pCommand) == true)
            {
                _fError.__mReasonAdd("Не указана команда");
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return vReturn;
            }

            bool vTransactionUsed = false; // Использование транзакции

            try
            {
                if (__fOnLine == false & fConnection == null)
                {
                    __mConnectionOn(); /// Установка соединения
                }

                SqlCommand vSqlCommand = new SqlCommand(pCommand, fConnection);
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
            catch (SqlException vException)
            {
                _fError.__fException = vException;
                _fError.__fErrorType_ = ERRORSTYPES.Data;
                _fError.__fMessage_ = "MS SQL сервер не может выполнить команду";
                _fError.__mPropertyAdd("Вид источника данных: {0}", __fDataSourceType.ToString());
                _fError.__mPropertyAdd("Сервер: {0}", __fServer);
                _fError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                _fError.__mPropertyAdd("База данных: {0}", __fDatabaseName);
                _fError.__mPropertyAdd("Содержание команды: {0}", pCommand);
                _fError.__mPropertyAdd("Транзакция: {0}", vTransactionUsed.ToString());
                _fError.__mPropertyAdd("Команда{0} {1}", ":", pCommand);
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

            }

            return vReturn;
        }
        /// <summary>
        /// Подсчет количества записей в таблице удовлетворяющих условию
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pExpressionWhere">Условия проверки</param>
        /// <returns></returns>
        public override int __mSqlCount(string pTableName, string pExpressionWhere)
        {
            DataTable vDataTable = __mSqlQuery("Select Count(*) as Cou From " + pTableName + " Where " + pExpressionWhere);
            return Convert.ToInt32(vDataTable.Rows[0]["Cou"]);
        }
        /// <summary>
        /// Отправка запроса источнику данных
        /// </summary>
        /// <param name="pQuery">Условие запроса</param>
        /// <returns>{DataTable} - с данными удовлетворяющими условию "pQuer"</returns>
        public override DataTable __mSqlQuery(string pQuery)
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
                SqlCommand vSqlCommand = new SqlCommand(pQuery, fConnection);
                vSqlCommand.CommandType = CommandType.Text;

                SqlDataReader vSqlDataReader = vSqlCommand.ExecuteReader();
                vDataTable = new DataTable();
                vDataTable.Load(vSqlDataReader);

                if (__fOnLine == false & fConnection != null)
                    __mConnectionOff();
            }
            catch (SqlException vException)
            {
                _fError.__fException = vException;
                _fError.__fProcedure_ = _fClassProcedure_;
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fErrorType_ = ERRORSTYPES.Data;
                _fError.__mMessageBuild("Ошибка при выполнении запроса");
                _fError.__mPropertyAdd("Вид источника данных: {0}", __fDataSourceType.ToString());
                _fError.__mPropertyAdd("Сервер: {0}", __fServer);
                _fError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                _fError.__mPropertyAdd("База данных: {0}", __fDatabaseName);
                _fError.__mPropertyAdd("Содержание запроса: \n{0}", pQuery);
                _fError.__mPropertyAdd("Транзакция: {0}", vTransactionUsed.ToString());

                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
            }

            return vDataTable;
        }
        /// <summary>
        /// Выполнение хранимой процедуры
        /// </summary>
        /// <param name="pStoredProcedure">Название хранимой процедуры</param>
        /// <param name="pParameters">Параметры хранимой процедуры</param>
        /// <returns></returns>
        public override DataTable __mSqlStoredProcedures(string pStoredProcedure, params object[] pParameters)
        {
            DataTable vReturn = new DataTable(); // Возвращаемое значение

            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Хранимая процедура{0} {1}", ":", pStoredProcedure);

            if (String.IsNullOrEmpty(pStoredProcedure) == true)
            {
                _fError.__mReasonAdd("Не указана хранимая процедура");
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return null;
            }

            bool vTransactionUsed = false; // Использование транзакции

            try
            {
                if (__fOnLine == false & fConnection == null)
                {
                    __mConnectionOn(); /// Установка соединения
                }

                SqlCommand vSqlCommand = new SqlCommand(pStoredProcedure, fConnection);
                vSqlCommand.CommandType = CommandType.StoredProcedure;

                for (int vAmount = 0; vAmount < pParameters.Length; vAmount++)
                {
                    if ((pParameters[vAmount] is appUnitItem) == true)
                    {
                        switch ((pParameters[vAmount] as appUnitItem).__fType_.Name)
                        {
                            case "Int32":
                                SqlParameter vParameterInt = new SqlParameter();
                                vParameterInt.DbType = DbType.Int32;
                                vParameterInt.ParameterName = (pParameters[vAmount] as appUnitItem).__fDesignation_;
                                vParameterInt.Value = Convert.ToInt32((pParameters[vAmount] as appUnitItem).__fValue_);
                                vSqlCommand.Parameters.Add(vParameterInt);
                                break;
                            case "DateTime":
                                SqlParameter vParameterDateTime = new SqlParameter();
                                vParameterDateTime.DbType = DbType.DateTime;
                                vParameterDateTime.ParameterName = (pParameters[vAmount] as appUnitItem).__fDesignation_;
                                vParameterDateTime.Value = Convert.ToDateTime((pParameters[vAmount] as appUnitItem).__fValue_);
                                vSqlCommand.Parameters.Add(vParameterDateTime);
                                break;
                        }
                    }
                } /// Добавление параметров

                /// Если открыта транзакция команда включается в транзакцию
                if (fTransaction != null)
                {
                    vSqlCommand.Transaction = fTransaction;
                    vTransactionUsed = true;
                }

                SqlDataAdapter vSqlDataAdapter = new SqlDataAdapter(vSqlCommand);
                vSqlDataAdapter.Fill(vReturn);
                //vReturn = (DataTable)vSqlCommand.ExecuteScalar();

                /// Если транзакция отсутствует, выполняется разрыв соединения
                if (fTransaction == null)
                {
                    if (__fOnLine == false & fConnection != null)
                        __mConnectionOff();
                }
            }
            catch (SqlException vException)
            {
                _fError.__fException = vException;
                _fError.__fErrorType_ = ERRORSTYPES.Data;
                _fError.__fProcedure_ = _fClassProcedure_;
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fMessage_ = "MS SQL сервер не может выполнить хранимую процедуру";
                _fError.__mPropertyAdd("Вид источника данных: {0}", __fDataSourceType.ToString());
                _fError.__mPropertyAdd("Сервер: {0}", __fServer);
                _fError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                _fError.__mPropertyAdd("База данных: {0}", __fDatabaseName);
                _fError.__mPropertyAdd("Транзакция: {0}", vTransactionUsed.ToString());
                
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
            }

            return vReturn;

        }
        /// <summary>
        /// Получение значения поля удовлетворяющего команде. Заполняется в предметном источнике данных 
        /// </summary>
        /// <param name="pCommand">Команда для получения значения поля</param>
        /// <returns>{object} - значение поля</returns>
        public override object __mSqlValue(string pCommand)
        {
            object vReturn = null; // Возвращаемое значение
            bool vTransactionUsed = false; // Использование транзакции
            try
            {
                if (__fOnLine == false & fConnection == null)
                {
                    __mConnectionOn();
                }  /// Установка соединения
                SqlCommand vSqlCommand = new SqlCommand(pCommand, fConnection);
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
            catch (SqlException vException)
            {
                _fError.__fException = vException;
                _fError.__fProcedure_ = _fClassProcedure_;
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fErrorType_ = ERRORSTYPES.Data;
                _fError.__mMessageBuild("Не возможно получить значение");
                _fError.__mPropertyAdd("Вид источника данных: {0}", __fDataSourceType.ToString());
                _fError.__mPropertyAdd("Сервер: {0}", __fServer);
                _fError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                _fError.__mPropertyAdd("База данных: {0}", __fDatabaseName);
                _fError.__mPropertyAdd("Содержание команды: {0}", pCommand);
                _fError.__mPropertyAdd("Транзакция: {0}", vTransactionUsed.ToString());

                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
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
        public override object __mSqlValue(string pTableName, string pFieldName, string pExpressionWhere)
        {
            string vCommand = "Select " + pFieldName + " From " + pTableName + " Where " + pExpressionWhere;
            return __mSqlValue(vCommand);
        }
        /// <summary>
        /// Выполнение скрипта
        /// </summary>
        /// <param name="pFilePath">Путь и имя файла скрипта</param>
        /// <param name="pWaitForFinish">Ожидать завершения выполнения</param>
        /// <returns>[true] - скрипт выполнен, иначе - [false]</returns>
        public override bool __mSqlSriptFileRun(string pFilePath, bool pWaitForFinish = true)
        {
            string vCommand = "-S " + __fServer + " -i " + pFilePath;
            sstProcesses vProcess = new sstProcesses();
            return vProcess.__mRun("sqlcmd.exe", vCommand, pWaitForFinish);
        }

        #endregion Sql операции

        #region База данных

        /// <summary>
        /// Создание резервной копии базы данных
        /// </summary>
        /// <returns>[true] - Файл копии базы данных создан, иначе - [false]</returns>
        /// <see cref=">https://www.mssqltips.com/sqlservertutorial/20/sql-server-backup-database-command/"/>
        public override string __mDatabaseBackUp()
        {
            string vFilePath = datApplication.__oPathes.__mFileDataBaseBackUp(__fDatabaseName, "bak");
            string vCommand = "BACKUP DATABASE " + __fDatabaseName + " TO DISK = '" + vFilePath + "' WITH NOINIT, STATS = 10";
            __mSqlCommand(vCommand);
            if (File.Exists(vFilePath) == false)
            {
                vFilePath = "";
            }

            return vFilePath;
        }
        /// <summary>
        /// Сравнение структуры таблиц в базе данных с моделью приложения
        /// </summary>
        /// <returns>[true] - структуры одинаковы, иначе - [false]</returns>
        public override bool __mDatabaseCompareWithModel()
        {
            bool vReturn = true; // Возвращаемое значение
            ArrayList vTableS = __mTablesList(); // Список таблиц в базе данных
            string vPrimaryIndex = ""; // Выражение создания главного индекса
            __fConnectionLine = ""; // Для переключения с таблицы 'Master' (при создании базы данных) на рабочую базу данных
            __mModelBuild();

            #region Проверка на вставку и изменение

            /// Перебор таблиц в модели базы данных
            foreach (datUnitModelTable vModelTable in __fModelTableS)
            {
                /// Таблица отсутствует в источнике данных
                if (__mTableExists(vModelTable.__fName) == false)
                {
                    string vCommand = "Create Table " + vModelTable.__fName + "(";
                    foreach (datUnitModelField vModelField in vModelTable.__fFieldS)
                    {
                        vCommand = vCommand + vModelField.__fName + " " + __mModelTableFieldType(vModelField);
                        if (vModelField.__fIsClue == true)
                        {
                            vPrimaryIndex = "CONSTRAINT [PK_" + vModelTable.__fName + "] PRIMARY KEY CLUSTERED ([" + vModelField.__fName + "] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]";
                        }
                        if (vModelField.__fAutoIncrement == true) /// Поле с автоматическим приращением
                            vCommand = vCommand + " IDENTITY (1, 1)";
                        if (vModelField.__fIsNull == false) /// Разрешение NULL данных
                            vCommand = vCommand + " Not Null";
                        //                        else
                        //                            vCommand = vCommand + ",";
                        if (vModelField.__fIsClue != true & vModelField.__fAutoIncrement != true)
                        {
                            vCommand = vCommand + " Default " + vModelField.__fDefaultValue + ",";
                        }
                        else
                            vCommand = vCommand + ",";
                    }
                    vCommand = vCommand + " " + vPrimaryIndex + ") On [PRIMARY]";
                    if (__mSqlCommand(vCommand) > 0) /// Создание таблицы
                    {
                        __fStructureChanges.Add(datApplication.__oTunes.__mTranslate("Создана таблица '{0}'", vModelTable.__fName));
                        string vTicks = DateTime.Now.Ticks.ToString();
                    }
                }  /// Таблица отсутствует в источнике данных
                else
                { /// Таблица присутствует в источнике данных
                    foreach (datUnitModelField vModelField in vModelTable.__fFieldS)
                    { /// Проверка существования полей
                        if (__mTableColumnExists(vModelTable.__fName, vModelField.__fName) == false)
                        { /// Поле отсутствует в таблице базы данных
                            string vCommand = "ALTER TABLE " + vModelTable.__fName + " ADD " + vModelField.__fName + " " + __mModelTableFieldType(vModelField);
                            if (vModelField.__fIsClue == true) /// Идентификатор записи в таблице
                                vCommand = vCommand + " Primary Key";
                            if (vModelField.__fIsNull == false) /// Разрешение NULL данных
                                vCommand = vCommand + " Not Null";
                            else
                                vCommand = vCommand + ",";
                            //if (vField._fDefaultValue.Length > 0) /// Значение по умолчанию
                            //    vCommand = vCommand + " Default " + vField._fDefaultValue + ",";
                            //else
                            vCommand = vCommand + " Default " + vModelField.__fDefaultValue + ",";
                            vCommand = vCommand.Substring(0, vCommand.Length - 1); /// Удаление последней запятой
                            __mSqlCommand(vCommand);
                            __fStructureChanges.Add(datApplication.__oTunes.__mTranslate("Создано поле '{0}' тип '{1}'", vModelField.__fName, __mModelTableFieldType(vModelField)));
                        } /// Поле отсутствует в таблице базы данных
                        else
                        { /// Поле отсутствует в таблице базы данных
                        } /// Поле отсутствует в таблице базы данных
                    }

                    #region Удаление полей

                    foreach (string vTableInDatabase in vTableS)
                    {
                        if (vModelTable.__fName.Trim().ToUpper() != vTableInDatabase.Trim().ToUpper())
                            continue;

                        DataTable vDataTable = __mSqlQuery("Select Top 1 * From " + vTableInDatabase);
                        foreach (DataColumn vDataColumn in vDataTable.Columns)
                        {
                            bool vSearched = false; // Обнаружение таблицы в эталонном списке таблиц
                            string vColumnName = ""; // Название колонки которую нужно удалить из истоника данных
                            foreach (datUnitModelField vModelField in vModelTable.__fFieldS)
                            {
                                vColumnName = vDataColumn.ColumnName;
                                if (vDataColumn.ColumnName.Trim().ToUpper() == vModelField.__fName.Trim().ToUpper())
                                    vSearched = true;
                            }
                            if (vSearched == false)
                            { /// Поле отсутствует в эталоне
                                __fStructureChanges.Add(datApplication.__oTunes.__mTranslate("Поле '{0}' может быть удалено", vTableInDatabase + "." + vColumnName));
                            }
                        }
                    }

                    #endregion Удаление полей
                } /// Таблица присутствует в источнике данных
            }

            #endregion Проверка на вставку и изменение

            #region Проверка на необходимость удаления таблиц

            if (__fModelTableS.Count > 0)
            { /// Эталонный список существует
                foreach (string vDataRow in vTableS)
                {
                    bool vSearched = false; // Обнаружение таблицы в эталонном списке таблиц
                    foreach (datUnitModelTable vModelTable in __fModelTableS)
                    {
                        if (vDataRow.Trim().ToUpper() == vModelTable.__fName.Trim().ToUpper())
                        {
                            vSearched = true;
                            break;
                        }
                    }
                    if (vSearched == false)
                    {
                        //string vCommand = "Drop Table " + vTable;
                        //_Command(vCommand); /// Удаление таблицы
                        //_StructureChanges.Add("Удалена таблица '" + vTable + "'");
                        __fStructureChanges.Add("Необходимо удалить таблицу '" + vDataRow.Trim() + "'");

                    }
                }
            }

            #endregion Проверка на необходимость удаления таблиц

            return vReturn;
        }
        /// <summary>
        /// Создание базы данных
        /// </summary>
        /// <param name="pDatabasePathFull">Полный путь и имя создаваемой базы данных</param>
        /// <returns>[true] - база данных создана, иначе - [false]</returns>
        public override bool __mDatabaseCreate()
        {
            bool vReturn = true; // Возвращаемое значение
            bool vExists = false; // База данных существует
            string vFileNameShort = Path.GetFileNameWithoutExtension(__fDatabaseName); // Имя файла базы данных без расширения
            string vFileNameFull = Path.Combine(__fDatabasePath, vFileNameShort); // Полное имя создаваемого файла без расширения

            /// Создание локальной базы данных
            if (__fLocalDB == true)
            {
                // http://qaru.site/questions/1242619/creating-local-database-at-run-time-with-visual-studio
                if (File.Exists(__fDatabasePath + "\\" + __fDatabaseName + ".mdf") == false)
                {
                    __fConnectionLine = "Data Source = (LocalDB)\\MSSQLLocalDB; Integrated Security = True; Connect Timeout = 30; ";
                    if (__mConnectionOn() == true)
                    {
                        if (__mSqlCommand("Create Database [" + vFileNameShort + "]" +
                                      " CONTAINMENT = NONE" +
                                      " On Primary (Name='" + vFileNameShort + "', FileName = '" + vFileNameFull + ".mdf' , SIZE = 8192KB, MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB)" +
                                      " Log On (Name='" + vFileNameShort + "_Log', FileName = '" + vFileNameFull + "_Log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )" +
                                      " COLLATE Cyrillic_General_100_CI_AI") == 0)
                        {
                            vReturn = true;
                        }
                        else
                            __mConnectionOff();
                    }
                } /// Базы данных нет в папке
                else
                    vExists = true;
            }
            /// Создание серверной базы данных 
            else
            {
                __fConnectionLine = "Server=" + __fServer + ";Integrated security=SSPI;database=master";
                if (__mConnectionOn() == true)
                {
                    ArrayList vDatabasesList = __mDataSourceDatabasesList();
                    foreach (string vDatabase in vDatabasesList)
                    {
                        if (vDatabase.Trim().ToUpper() == __fDatabaseName.Trim().ToUpper())
                        {
                            vExists = true;
                        } /// База данных существует
                    }
                    if (vExists == false)
                    {
                        if (__mSqlCommand("CREATE DATABASE " + __fDatabaseName + " ON PRIMARY " +
                                       "(NAME = " + __fDatabaseName + "_Data, " +
                                       "FILENAME = '" + __fDatabasePath + "\\" + __fDatabaseName + ".mdf', " +
                                       "SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%) " +
                                       "LOG ON (NAME = " + __fDatabaseName + "_Log, " +
                                       "FILENAME = '" + __fDatabasePath + "\\" + __fDatabaseName + ".ldf', " +
                                       "SIZE = 1MB, " +
                                       "MAXSIZE = 5MB, " +
                                       "FILEGROWTH = 10%)") == 0)
                        {
                            vReturn = false;
                        }
                        else
                        {
                            __mConnectionOff();

                            #region Протоколирование создания базы данных

                            datApplication.__oProtocols.__mCreate(PROTOCOLSTYPES.ApplicationEvent, _fClassProcedure_);
                            datApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Message
                                , datApplication.__oTunes.__mTranslate("База данных '{0}' создана", vFileNameFull + ".mdf")
                                , 0);

                            #endregion Протоколирование создания базы данных
                        }
                    }
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Создание скрипта создания базы данных
        /// </summary>
        /// <param name="pFileScriptPath">Путь к создаваемому файлу</param>
        /// <param name="pFileDatabasePath">Путь к создаваемой базе данных</param>
        /// <param name="pFileStream">База данных должна работать с File Stream</param>
        /// <param name="pDatabaseCollate">Название сортировки базы данных</param>
        /// <param name="pProductionVersion">Номер производственной версии</param>
        /// <returns>[true] - Файл скрипта создан, иначе - [false]</returns>
        public override bool __mDatabaseCreateScriptForCreateDatabase(string pFileScriptPath, string pFileDatabasePath = "", bool pFileStream = false, string pDatabaseCollate = "Latin1_General_100_CI_AS", string pProductionVersion = "1.0.0")
        {
            __fDatabaseName = Path.GetFileNameWithoutExtension(pFileScriptPath);
            __fDatabasePath = pFileDatabasePath;
            if (String.IsNullOrEmpty(__fDatabasePath) == true)
            {
                __fDatabasePath = Path.GetDirectoryName(pFileScriptPath);
            }

            string vScriptBody = "Use [master]" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "Create Database [" + __fDatabaseName + "] Containment = None" + CRLF;
            vScriptBody += "On Primary" + CRLF;
            vScriptBody += "(Name = N'" + __fDatabaseName + "', FileName = N'" + Path.Combine(__fDatabasePath, __fDatabaseName + ".mdf") + "', Size = 2MB, MaxSize = UNLIMITED, FileGrowth = 65536KB )" + CRLF;
            if (pFileStream == false)
                vScriptBody += "LOG ON" + CRLF;
            else
            {
                vScriptBody += "FILEGROUP [FileStream_Files] CONTAINS FILESTREAM  DEFAULT" + CRLF;
                vScriptBody += "(NAME = N'FileStream_Files', FILENAME = N'" + Path.Combine(pFileDatabasePath, "Files_FS") + "' , MAXSIZE = UNLIMITED)" + CRLF;
                vScriptBody += "LOG ON" + CRLF;
            }
            vScriptBody += "(Name = N'" + __fDatabaseName + "_log', FileName = N'" + Path.Combine(__fDatabasePath, __fDatabaseName + "_log.ldf") + "', Size = 2MB, MaxSize = UNLIMITED, FileGrowth = 65536KB)" + CRLF;
            vScriptBody += "Collate " + pDatabaseCollate + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "Alter Database [" + __fDatabaseName + "] Set Compatibility_Level = 140" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "IF(1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))" + CRLF;
            vScriptBody += "begin" + CRLF;
            vScriptBody += "EXEC[" + __fDatabaseName + "].[dbo].[sp_fulltext_database] @action = 'enable'" + CRLF;
            vScriptBody += "end" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET ANSI_NULL_DEFAULT OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET ANSI_NULLS OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET ANSI_PADDING OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET ANSI_WARNINGS OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET ARITHABORT OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET AUTO_CLOSE ON" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET AUTO_SHRINK OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET AUTO_UPDATE_STATISTICS ON" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET CURSOR_CLOSE_ON_COMMIT OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET CURSOR_DEFAULT  GLOBAL" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET CONCAT_NULL_YIELDS_NULL OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET NUMERIC_ROUNDABORT OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET QUOTED_IDENTIFIER OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET RECURSIVE_TRIGGERS OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET DISABLE_BROKER" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET AUTO_UPDATE_STATISTICS_ASYNC OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET DATE_CORRELATION_OPTIMIZATION OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET TRUSTWORTHY OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET ALLOW_SNAPSHOT_ISOLATION OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET PARAMETERIZATION SIMPLE" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET READ_COMMITTED_SNAPSHOT OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET HONOR_BROKER_PRIORITY OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET RECOVERY SIMPLE" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET MULTI_USER" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET PAGE_VERIFY CHECKSUM" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            if (pFileStream == true)
            {
                vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET FILESTREAM(NON_TRANSACTED_ACCESS = OFF, DIRECTORY_NAME = N'Files_" + __fDatabaseName + "')" + CRLF;
                vScriptBody += "Go" + CRLF + CRLF;
            }
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET DB_CHAINING OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET FILESTREAM(NON_TRANSACTED_ACCESS = OFF)" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET TARGET_RECOVERY_TIME = 60 SECONDS" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET DELAYED_DURABILITY = DISABLED" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET QUERY_STORE = OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "USE[" + __fDatabaseName + "]" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;
            vScriptBody += "ALTER DATABASE[" + __fDatabaseName + "] SET READ_WRITE" + CRLF;
            vScriptBody += "Go" + CRLF + CRLF;

            vScriptBody += "EXEC sp_addextendedproperty" + CRLF;
            vScriptBody += "@name = N'DatabaseVersion'," + CRLF;
            vScriptBody += "@value = N'" + pProductionVersion + "'" + CRLF;

            try
            {
                File.Delete(pFileScriptPath);
            }
            catch { }
            appFileText vFileText = new appFileText();
            Directory.CreateDirectory(Path.GetDirectoryName(pFileScriptPath));
            vFileText.__mWriteToEnd(pFileScriptPath, vScriptBody);

            return File.Exists(pFileScriptPath);
        }
        /// <summary>
        /// Проверка существования базы данных на текущей сервере
        /// </summary>
        /// <param name="vDatabaseName"></param>
        /// <returns>[true] - база данных существует, иначе - [false]</returns>
        public override bool __mDatabaseExists(string vDatabaseName)
        {
            ArrayList vDatabaseS = __mDatabasesList();
            return appTypeString.__mWordInArrayList(vDatabaseName, vDatabaseS);
        }
        /// <summary>
        /// Восстановление базы данных из копии
        /// </summary>
        /// <param name="pFileName">Путь и имя файла страховой копии</param>
        /// <returns>[true] - Файл копии базы данных создан, иначе - [false]</returns>
        public override bool __mDatabaseRestore(string pFilePath)
        {
            if (File.Exists(pFilePath) == true)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                string pCommand = "RESTORE DATABASE " + __fDatabaseName + " FROM DISK '" + pFilePath + "' WITH REPLACE RECOVERY";
                __mSqlCommand(pCommand);
                return true;
            }
            else
            {
                _fError.__fProcedure_ = _fClassProcedure_;
                _fError.__fMessage_ = "Не удалось восстанвить базу данных";
                _fError.__mPropertyAdd("Путь к файлу копии {0}", pFilePath);

                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return false;
            }
        }
        /// <summary>
        /// Сжатие базы данных
        /// </summary>
        /// <returns>[true] - база данных сжата без ошибок, иначе - [false]</returns>
        public override bool __mDatabaseShrink()
        {
            /// Перемещение заполненных страниц в начало файла
            int a = __mSqlCommand("DBCC SHRINKDATABASE ('" + __fDatabaseName + "', NOTRUNCATE)");
            /// Должно остаться 5% свободного пространства
            int b = __mSqlCommand("DBCC SHRINKDATABASE ('" + __fDatabaseName + "', 5, TRUNCATEONLY)");
            /// Сжатие файла с данными до 2 Мб
            int c = __mSqlCommand("DBCC SHRINKFILE (" + __fDatabaseName + ", 2)");
            /// Попытка сжать файл транзакций до 2Мб
            int d = __mSqlCommand("DBCC SHRINKFILE(" + __fDatabaseName + "_log, 2)");
            /// Выполненение BackUp без копирования. только удаление пустых страниц
            //int e = __mSqlCommand("BACKUP '" + __fDatabaseName + "' WITH COMPRESSION");
            /// Попытка сжать файл транзакций до 2Мб
            //int f = __mSqlCommand("DBCC SHRINKFILE(" + __fDatabaseName + "_log, 2)");

            return base.__mDatabaseShrink();
        }
        /// <summary>
        /// Получение списка баз данных на сервере
        /// </summary>
        /// <returns>Список баз данных на сервере</returns>
        public override ArrayList __mDatabasesList()
        {
            ArrayList vReturn = new ArrayList(); // Возвращаемое значение

            DataTable vDataTable = __mSqlQuery("Select Name From sys.databases");
            foreach (DataRow vDataRow in vDataTable.Rows)
            {
                vReturn.Add(vDataRow[0].ToString());
            }

            return vReturn;
        }
        /// <summary>
        /// Удаление базы данных
        /// </summary>
        /// <param name="pDatabaseName"></param>
        /// <returns></returns>
        public override bool __mDatabaseDrop(string pDatabaseName)
        {
            bool vReturn = false; // Возвращаемое значение
            int vRecordsCount = __mSqlCommand("Drop Database If Exists " + pDatabaseName);

            if (vRecordsCount == 1)
                vReturn = true;

            return vReturn;
        }

        #endregion База данных

        #region Блокировки

        /// <summary>
        /// Закрытие блокировок текущего пользователя
        /// </summary>
        /// <param name="pUserClue">Идентификатор пользователя</param>
        public override void __mLockClear(int pUserClue = -1)
        {
            if (pUserClue == -1)
                __mSqlCommand("Update RrdLck Set dtmRrdLckOff = GetDate() Where lnkUsr = " + __fUserClue.ToString() + " and dtmRrdLckOff = CONVERT([datetime],'01.01.1900')");
            else
                __mSqlCommand("Update RrdLck Set dtmRrdLckOff = GetDate() Where lnkUsr = " + pUserClue.ToString() + " and dtmRrdLckOff = CONVERT([datetime],'01.01.1900')");
        }
        /// <summary>
        /// Снятие блокировки
        /// </summary>
        /// <param name="pLockClue">Идентификатор блокировки</param>
        /// <returns>[true] - блокировка снята, иначе - [false]</returns>
        public override bool __mLockOff(int pLockClue)
        {
            bool vReturn = true; // Возвращаемое значение

            if (__mSqlCommand("Update RrdLck Set dtmRrdLckOff = Cast('" + appTypeDateTime.__mMsSqlDateTimeToString(DateTime.Now) + "' as DateTime) Where CLU = " + pLockClue.ToString()) <= 0) /// Снятие блокировки
                vReturn = false;

            return vReturn;
        }
        /// <summary>
        /// Выполнение блокировки таблицы или записи в таблице 
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pLockClue">Идентификатор блокируемой записи</param>
        /// <remarks>Если 'pRecord' = 0, то блокируется вся таблица</remarks>
        /// <returns>Идентификатор заблокированной записи, [0] - запись не удалось заблокировать, [-1] - Блокировки отключены</returns>
        public override int __mLockOn(string pTableName, int pLockClue)
        {
            int vLockClue = -1; // Идентификатор заблокированной записи
            int vRecordObjectClue = -1; // Идентификатор объекта блокировки

            /// Получение идентификатора объекта блокировки
            string vQuery = "Select CLU From Tbl Where dsiTbl = N'" + pTableName + "'";
            string vCommand = "Insert Into Tbl (dsiTbl) Values(N'" + pTableName + "')";
            DataTable vDataTable = __mSqlQuery(vQuery);
            if (vDataTable.Rows.Count > 0)
            {
                vRecordObjectClue = Convert.ToInt32(vDataTable.Rows[0][0]);
            }
            else
            {
                __mSqlCommand(vCommand);
                vRecordObjectClue = __mClueLastInserted("Tbl");
            }
            /// Проверка наличия зависших блокировок текущего пользователя
            /// Данные даты-времени храняться с типом 'DateTime'
            if (__fDateTimeStore == DATETIMESTORE.DateTime)
            {
                /// Обнаружена блокировка текущего пользователя (Зависшая блокировка)
                if (__mTableRowsCountWhere("RrdLck"
                                     , " lnkTbl = " + vRecordObjectClue.ToString() +
                                       " and lnkRrdClu = " + pLockClue.ToString() +
                                       " and dtmRrdLckOff = Cast('19000101' as DateTime)" +
                                       " and lnkUsr = " + __fUserClue.ToString() +
                                       " and PcsClu = " + datApplication.__fProcessClue_.ToString()) > 0)
                {
                    vLockClue = Convert.ToInt32(__mSqlValue("Select CLU From RrdLck Where" +
                                                        " lnkTbl = " + vRecordObjectClue.ToString() +
                                                        " and lnkRrdClu = " + pLockClue.ToString() +
                                                        " and dtmRrdLckOff = Cast('19000101' as DateTime)" +
                                                        " and lnkUsr = " + __fUserClue.ToString() +
                                                        " and PcsClu = " + datApplication.__fProcessClue_.ToString()));
                    if (vLockClue > 0)
                        return vLockClue; /// Блокировка принимается для использования
                }
            }

            /// Поиск чужих блокировок 
            // Количество чужих не закрытых блокировок для полученных таблицы и идентификатора
            int vLockCoun = __mTableRowsCountWhere("RrdLck",
                                                "lnkTbl = " + vRecordObjectClue.ToString() + 
                                                " and lnkRrdClu = " + pLockClue.ToString() +
                                                " and dtmRrdLckOff = Cast('19000101' as DateTime)");
            /// Обнаружены чужие блокировки
            if (vLockCoun > 0)
            {

                int vUserClue = Convert.ToInt32(__mSqlValue("RrdLck" // Идентификатор пользователя забокировавшего запись
                    , "lnkUsr"
                    , "lnkTbl = " + vRecordObjectClue.ToString() +
                    " and lnkRrdClu = " + pLockClue.ToString() +
                    " and dtmRrdLckOff = Cast('19000101' as DateTime)"));

                string vUserName = Convert.ToString(__mSqlValue("Usr" // Псевдоним пользователя заблокировашего запись
                    , "dsiUsr"
                    , "CLU = " + vUserClue.ToString()));

                DateTime vLockTime = Convert.ToDateTime(__mSqlValue("RrdLck" // Время создания блокировки
                    , "dtmRrdLck_On"
                    , "lnkTbl = '" + vRecordObjectClue.ToString() +
                    "' and lnkRrdClu = " + pLockClue.ToString() +
                    " and dtmRrdLckOff = Cast('19000101' as DateTime)"));

                /// Отображение ошибки блокировки
                _fError.__mMessageBuild(datApplication.__oTunes.__mTranslate("Запись заблокирована пользователем") + " '{0}' в {1}", vUserName.Trim(), vLockTime.ToString().Trim());
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fProcedure_ = _fClassProcedure_;
                _fError.__fErrorType_ = ERRORSTYPES.Data;
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return -1;
            }
            /// Создание блокировки для полученных таблицы и идентификатора
            if (vLockCoun == 0)
            {
                /// Выполнение блокировки
                if (__mSqlCommand("Insert Into RrdLck(lnkTbl, dtmRrdLck_On, lnkRrdClu, lnkUsr, PcsClu) "
                    + "Values(" + vRecordObjectClue.ToString()
                    + ", Cast('" + appTypeDateTime.__mMsSqlDateTimeToString(DateTime.Now) + "' as DateTime)"
                    + ", " + pLockClue.ToString()
                    + ", " + __fUserClue.ToString()
                    + ", " + datApplication.__fProcessClue_.ToString() + ")") > 0)
                    /// Получение идентификатора блокировки    
                    vLockClue = __mClueLastInserted("RrdLck");
            }

            return vLockClue;
        }
        /// <summary>
        /// Выполнение исправления 0 вновь созданной записи на рассчитанный идентификатор записи
        /// </summary>
        /// <param name="pLockClue">Идентификатор записи в таблице блокировок</param>
        /// <param name="pLinkRid">Идентификатор заблокированной записи</param>
        /// <returns>[true] - данные исправлены, иначе [false]</returns>
        public override bool __mLockLnkRidChange(int pLockClue, int pLinkRid)
        {
            bool vReturn = true; // Возвращаемое значение

            if (__mSqlCommand("Update RrdLck Set lnkRrdClu = " + pLinkRid + " Where lnkRrdClu = 0 and CLU = " + pLockClue.ToString()) <= 0)
                vReturn = false;

            return vReturn;
        }

        #endregion Блокировки

        #region Выражения

        /// <summary>
        /// Создание выражения 'Like' с использованием транслита
        /// </summary>
        /// <param name="pFieldName">Название поля для которого строиться выражение</param>
        /// <param name="pText">Текст условия на одной из раскладок клавиатуры</param>
        public override string __mExpressionLikeEntryTranslit(string pFieldName, string pText)
        {
            pText = pText.Trim().ToUpper(); // Подготовка выражения
            string vReturn = ""; // Возвращаемое значение
            int vWordCount = appTypeString.__mWordCountSpace(pText); // Количество слов разделенных пробелом в полученном выражении
            for (int vAmount = 0; vAmount < vWordCount; vAmount++)
            {
                string vWord = appTypeString.__mWordNumberSpace(pText, vAmount); // Выбор обрабатываемого слова из полученного выражения
                string vWordTranslite = ""; // Обрабатываемое слово на транслите
                if (Regex.IsMatch(vWord, @"^\d+$") || String.IsNullOrWhiteSpace(appTypeString.__mSymbolsDeleteNumbers(vWord)))
                {
                    vWord = Regex.Replace(vWord, "[^A-Za-zА-Яа-я0-9()-*/]", "");
                    if (vAmount == 0) // Первое слово
                        vReturn = pFieldName + " like N'%" + vWord + "%' ";
                    else // Следующее слово
                        vReturn = vReturn + " or " + pFieldName + " like N'%" + vWord + "%' ";
                } /// Введены только цифры
                else
                {
                    char ss = Convert.ToChar(appTypeString.__mSymbolsDeleteNumbers(vWord).Trim().Substring(0, 1));
                    if (appTypeString.__mSymbolAsciiCode(ss.ToString()) > 192 & appTypeString.__mSymbolAsciiCode(ss.ToString()) < 223)  //  русская буква
                    {
                        vWordTranslite = appTypeString.__mSymbolChange(vWord, "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЯЧСМИТЬБЮ()/Э", "QWERTYUIOP[]ASDFGHJKL;ZXCVBNM,.()/.");
                    }
                    else
                    {
                        vWordTranslite = appTypeString.__mSymbolChange(vWord, "QWERTYUIOP[]ASDFGHJKL;ZXCVBNM,.()/'", "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЯЧСМИТЬБЮ()/Э");
                        vWord = Regex.Replace(vWord, "[^A-Za-zА-Яа-я0-9()-*/]", "");
                    }
                    vWord = vWord.Replace("'", ".");
                    if (String.IsNullOrWhiteSpace(vWordTranslite))
                        vReturn = pFieldName + "like N'%" + vWordTranslite + "%' ";
                    else
                        if (vAmount == 0)
                        vReturn = "(" + pFieldName + " like N'%" + vWord + "%' " + " or " + pFieldName + " like N'%" + vWordTranslite + "%') ";
                    else
                        vReturn = vReturn + " and (" + pFieldName + " like N'%" + vWord + "%' " + " or " + pFieldName + " like N'%" + vWordTranslite + "%') ";
                } /// Введены числа и буквы или только буквы
            }

            return vReturn;
        }
        /// <summary>
        /// Создание выражения 'Like' с использованием транслита
        /// </summary>
        /// <param name="pFieldName">Название поля для которого строиться выражение</param>
        /// <param name="pText">Текст условия на одной из раскладок клавиатуры</param>
        public override string __mExpressionLikeStartTranslit(string pFieldName, string pText)
        {
            pText = pText.Trim().ToUpper(); // Подготовка выражения
            string vReturn = ""; // Возвращаемое значение
            int vWordCount = appTypeString.__mWordCountSpace(pText); // Количество слов разделенных пробелом в полученном выражении
                                                                     //               string vWord = appTypeString.__mWordNumberSpace(pText, vAmount); // Выбор обрабатываемого слова из полученного выражения
            string vWordTranslite = ""; // Обрабатываемое слово на транслите
            if (Regex.IsMatch(pText, @"^\d+$") || String.IsNullOrWhiteSpace(appTypeString.__mSymbolsDeleteNumbers(pText)))
            {
                pText = Regex.Replace(pText, "[^A-Za-zА-Яа-я0-9()-*/]", "");
                //                   if (vAmount == 0) // Первое слово
                vReturn = pFieldName + " like N'" + pText + "%' ";
                //                   else // Следующее слово
                //                       vReturn = vReturn + " or " + pFieldName + " like N'" + pText + "%' ";
            } /// Введены только цифры
            else
            {
                char ss = Convert.ToChar(appTypeString.__mSymbolsDeleteNumbers(pText).Trim().Substring(0, 1));
                if (appTypeString.__mSymbolAsciiCode(ss.ToString()) > 192 & appTypeString.__mSymbolAsciiCode(ss.ToString()) < 223)  //  русская буква
                {
                    vWordTranslite = appTypeString.__mSymbolChange(pText, "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЯЧСМИТЬБЮ()/Э", "QWERTYUIOP[]ASDFGHJKL;ZXCVBNM,.()/.");
                }
                else
                {
                    vWordTranslite = appTypeString.__mSymbolChange(pText, "QWERTYUIOP[]ASDFGHJKL;ZXCVBNM,.()/'", "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЯЧСМИТЬБЮ()/Э");
                    pText = Regex.Replace(pText, "[^A-Za-zА-Яа-я0-9()-*/]", "");
                }
                pText = pText.Replace("'", ".");
                if (String.IsNullOrWhiteSpace(vWordTranslite))
                    vReturn = pFieldName + "like N'%" + vWordTranslite + "%' ";
                else
                    //                       if (vAmount == 0)
                    vReturn = "(" + pFieldName + " like N'" + pText + "%' " + " or " + pFieldName + " like N'" + vWordTranslite + "%') ";
                //                   else
                //                       vReturn = vReturn + " and (" + pFieldName + " like N'" + pText + "%' " + " or " + pFieldName + " like N'" + vWordTranslite + "%') ";
            } /// Введены числа и буквы или только буквы

            return vReturn;
        }

        #endregion Выражения

        #region Модель

        /// <summary>
        /// Получение типа данных поля для текущего типа источника данных
        /// </summary>
        /// <param name="pModelField">Значение перечисления типа данных полей</param>
        /// <returns>Название типа данных</returns>
        public override string __mModelTableFieldType(datUnitModelField pModelField)
        {
            string vReturn = ""; // Возвращаемое значение
            switch (pModelField.__fDataType)
            {
                case COLUMNSTYPES.Bigint:
                    vReturn = "bigint";
                    break;
                case COLUMNSTYPES.Binary:
                    vReturn = "binary(" + pModelField.__fSize.ToString() + ")";
                    break;
                case COLUMNSTYPES.Bit:
                    vReturn = "bit";
                    break;
                case COLUMNSTYPES.Char:
                    vReturn = "char(" + pModelField.__fSize.ToString() + ")";
                    break;
                case COLUMNSTYPES.Date:
                    vReturn = "date";
                    break;
                case COLUMNSTYPES.Datetime:
                    vReturn = "datetime";
                    break;
                case COLUMNSTYPES.Datetime2:
                    vReturn = "datetime2(7)";
                    break;
                case COLUMNSTYPES.Datetimeoffset:
                    vReturn = "datetimeoffset";
                    break;
                case COLUMNSTYPES.Decimal:
                    vReturn = "decimal(" + pModelField.__fSize.ToString() + ", " + pModelField.__fSizeDecimal.ToString() + ")";
                    break;
                case COLUMNSTYPES.Float:
                    vReturn = "float(" + pModelField.__fSize.ToString() + ", " + pModelField.__fSizeDecimal.ToString() + ")";
                    break;
                case COLUMNSTYPES.Geography:
                    vReturn = "geography";
                    break;
                case COLUMNSTYPES.Geomentry:
                    vReturn = "geomentry";
                    break;
                case COLUMNSTYPES.Hierarchyid:
                    vReturn = "hierarchyid";
                    break;
                case COLUMNSTYPES.Image:
                    vReturn = "image";
                    break;
                case COLUMNSTYPES.Int:
                    vReturn = "int";
                    break;
                case COLUMNSTYPES.Money:
                    vReturn = "money(" + pModelField.__fSize.ToString() + ", " + pModelField.__fSizeDecimal.ToString() + ")";
                    break;
                case COLUMNSTYPES.Nchar:
                    vReturn = "nchar(" + pModelField.__fSize.ToString() + ")";
                    break;
                case COLUMNSTYPES.Ntext:
                    vReturn = "ntext(" + pModelField.__fSize.ToString() + ")";
                    break;
                case COLUMNSTYPES.Numeric:
                    vReturn = "numeric(" + pModelField.__fSize.ToString() + ", " + pModelField.__fSizeDecimal.ToString() + ")";
                    break;
                case COLUMNSTYPES.Nvarchar:
                    vReturn = "nvarChar(" + pModelField.__fSize.ToString() + ")";
                    break;
                case COLUMNSTYPES.Real:
                    vReturn = "real(" + pModelField.__fSize.ToString() + ", " + pModelField.__fSizeDecimal.ToString() + ")";
                    break;
                case COLUMNSTYPES.Rowversion:
                    vReturn = "rowversion";
                    break;
                case COLUMNSTYPES.Smalldatetime:
                    vReturn = "smalldatetime";
                    break;
                case COLUMNSTYPES.Smallint:
                    vReturn = "smallint";
                    break;
                case COLUMNSTYPES.Smallmoney:
                    vReturn = "smallmoney(" + pModelField.__fSize.ToString() + ", " + pModelField.__fSizeDecimal.ToString() + ")";
                    break;
                case COLUMNSTYPES.Sql_variant:
                    vReturn = "sql_variant";
                    break;
                case COLUMNSTYPES.Text:
                    vReturn = "text(" + pModelField.__fSize.ToString() + ")";
                    break;
                case COLUMNSTYPES.Time:
                    vReturn = "time";
                    break;
                case COLUMNSTYPES.Timestamp:
                    vReturn = "timestamp";
                    break;
                case COLUMNSTYPES.Tinyint:
                    vReturn = "tinyInt";
                    break;
                case COLUMNSTYPES.Uniqueidentifier:
                    vReturn = "uniqueidentifier";
                    break;
                case COLUMNSTYPES.Varbinary:
                    vReturn = "varbinary(" + pModelField.__fSize + ")";
                    break;
                case COLUMNSTYPES.Varchar:
                    vReturn = "varchar(" + pModelField.__fSize + ")";
                    break;
                case COLUMNSTYPES.Varcharmax:
                    vReturn = "varchar(max)";
                    break;
                case COLUMNSTYPES.Xml:
                    vReturn = "xml";
                    break;
            }
            return vReturn;
        }

        #endregion Модель

        #region Источник данных

        /// <summary>
        /// Получение списка баз данных в источнике данных
        /// </summary>
        /// <returns>{ArrayList} - Список баз данных</returns>
        public override ArrayList __mDataSourceDatabasesList()
        {
            ArrayList vReturn = new ArrayList(); // Возвращаемое значение

            DataTable vDataTable = __mSqlQuery("Select name From sys.databases Order By name");
            foreach (DataRow vDataRow in vDataTable.Rows)
            {
                vReturn.Add(vDataRow["name"].ToString());
            }

            return vReturn;
        }
        /// <summary>
        /// Получение списка доступных серверов
        /// </summary>
        /// <returns>{DataTable} - Таблица со списком доступных серверов</returns>
        public override DataTable __mDataSourceServersList()
        {
            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            return instance.GetDataSources();
        }

        #endregion Источник данных

        #region Подключение

        /// <summary>
        /// Построение строки подключения к источнику данных
        /// </summary>
        /// <param name="pLogin">Использование логина с паролем</param>
        /// <returns>[true] - строка построена, иначе - [false]</returns>
        protected override bool __mConnectionLineBuild(bool pLogin)
        {
            bool vReturn = true; // Возвращаемое значение

            __fConnectionLine = ""; /// Сброс строки подключения
                                    /// LocalDB
            if (__fLocalDB == true)
            {
                if (__fDatabaseName.Length == 0) /// Имя базы данных указано
                    vReturn = vReturn & false;
                else
                {
                    if (__fLocalDBOldVersion == true)
                        __fConnectionLine = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=" + __fDatabaseName + ";Integrated Security=False";
                    else
                        __fConnectionLine = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + __fDatabaseName + ";Integrated Security=False";
                }
            }
            /// Server Database
            else
            {
                if (__fServer.Length == 0) /// Имя сервера не указано
                    vReturn = vReturn & false;
                if (__fDatabaseName.Length == 0) /// Имя базы данных не указано
                    vReturn = vReturn & false;

                if (pLogin == true)
                { /// Использовать логин для подключения
                    if (__fServerLogin.Length == 0) /// Логин не указан
                        vReturn = vReturn & false;
                    if (__fServerPassword.Length == 0) /// Пароль не указан
                        vReturn = vReturn & false;
                    __fConnectionLine = "Persist Security Info=False;User ID=" + __fServerLogin + ";Password=" + __fServerPassword + ";Initial Catalog=" + __fDatabaseName + ";Server=" + __fServer;
                }
                else
                { // Не использовать логин для подключения
                    __fConnectionLine = "Persist Security Info=False;Trusted_Connection=True;Initial Catalog=" + __fDatabaseName + ";Server=" + __fServer;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Разрыв соединения с источником данных
        /// </summary>
        protected override bool __mConnectionOff()
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
                    _fError.__fException = vException;
                    _fError.__fLineInProcedure_ = _fClassLine_;
                    _fError.__fProcedure_ = _fClassProcedure_;
                    _fError.__fErrorType_ = ERRORSTYPES.Data;
                    _fError.__mMessageBuild("Не удалось отключить '{0}'", __fDatabaseName);
                    _fError.__mPropertyAdd("Тип источника данных: {0}", __fDataSourceType.ToString());
                    _fError.__mPropertyAdd("Сервер: {0}", __fServer);
                    _fError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                    _fError.__mPropertyAdd("База данных: {0}", __fDatabaseName);

                    datApplication.__oErrorsHandler.__mShow(_fError);
                    _fError.__mClear();

                    vReturn = false;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Установка соединения с источником данных
        /// </summary>
        /// <returns>[true] - соединение установлено, иначе - [false]</returns>
        protected override bool __mConnectionOn()
        {
            bool vReturn = true; // Возвращаемое значение

            if (__fConnectionLine.Length == 0)
            {
                try
                {
                    __mConnectionLineBuild(true);
                    if (__fConnectionLine.Length > 0)
                    {
                        fConnection = new SqlConnection(__fConnectionLine);
                        fConnection.Open();
                    }
                    else
                        vReturn = false;
                }
                catch
                {
                    vReturn = false;
                } /// Подключение с идентификацией пользователя
                if (vReturn == false)
                {
                    try
                    {
                        vReturn = true;
                        __mConnectionLineBuild(false);
                        fConnection = new SqlConnection(__fConnectionLine);
                        fConnection.Open();
                    }
                    catch (SqlException vException)
                    {
                        _fError.__fException = vException;
                        _fError.__fLineInProcedure_ = _fClassLine_;
                        _fError.__fProcedure_ = _fClassProcedure_;
                        _fError.__fErrorType_ = ERRORSTYPES.Data;
                        _fError.__mMessageBuild("Не удалось подключить {0}", __fDatabaseName);
                        _fError.__mPropertyAdd("Тип источника данных: {0}", __fDataSourceType.ToString());
                        _fError.__mPropertyAdd("Сервер: {0}", __fServer);
                        _fError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                        _fError.__mPropertyAdd("База данных: {0}", __fDatabaseName);

                        datApplication.__oErrorsHandler.__mShow(_fError);
                        _fError.__mClear();

                        vReturn = false;
                    }
                } /// Подключение с использованием Windows идентификации
            }
            else
            {
                try
                {
                    fConnection = new SqlConnection(__fConnectionLine);
                    fConnection.Open();
                }
                catch (SqlException vException)
                {
                    _fError.__fException = vException;
                    _fError.__fLineInProcedure_ = _fClassLine_;
                    _fError.__fProcedure_ = _fClassProcedure_;
                    _fError.__fErrorType_ = ERRORSTYPES.Data;
                    _fError.__mMessageBuild("Ошибка подключения к источнику данных {0}", __fDatabaseName);
                    _fError.__mPropertyAdd("Вид источника данных: {0}", __fDataSourceType.ToString());
                    _fError.__mPropertyAdd("Сервер: {0}", __fServer);
                    _fError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                    _fError.__mPropertyAdd("База данных: {0}", __fDatabaseName);

                    datApplication.__oErrorsHandler.__mShow(_fError);

                    vReturn = false;
                }
            }

            return vReturn;
        }

        #endregion Подключение

        #region Пользователи

        /// <summary>
        /// Получение доступа пользователя к объекту
        /// </summary>
        /// <param name="pRight">Право</param>
        /// <param name="pClueUserRole">Роль пользователя</param>
        /// <param name="pClueUser">Пользователь</param>
        /// <returns>[true] - доступ разрешен, иначе - [false]</returns>
        public override bool __mUserAccess(int pRight, int pClueUser)
        {
            //-int vClueUserRole = Convert.ToInt32(__mSqlValue("Usr", "lnkUsrRol", "CLU = " + pClueUser.ToString())); // Роль пользователя
            string vCommand = "Select Rht " +
                              " Select Rht" +
                              " From UsrRht as UR Where lnkUsr = " + pClueUser + " and Obj = '" + pRight + "' ";
            //-" From UsrRht as UR Where lnkUsrRol = " + vClueUserRole.ToString() + " and lnkUsr = 0 and Obj = '" + pRight + "'" +
            //-" Union" +

DataTable vDataTable = __mSqlQuery(vCommand);
            bool vUserRoleAccess = Convert.ToBoolean(vDataTable.Rows[0][1]); // Право роли
            bool vUserAccess = Convert.ToBoolean(vDataTable.Rows[1][1]); // Право пользователя

            return vUserRoleAccess | vUserAccess;
        }

        #endregion Пользователи

        #region Таблицы

        /// <summary>
        /// Получение списка таблиц в базе данных
        /// </summary>
        /// <returns>Список таблиц в базе данных</returns>
        public override ArrayList __mTablesList()
        {
            ArrayList vReturn = new ArrayList(); // Возвращаемое значение

            string vQuery = "Select S.TABLE_NAME as dsiTbl"
+ ", (Select value From fn_listextendedproperty(NULL, 'user', 'dbo', 'table', S.TABLE_NAME, NULL, NULL)) as TblDsp"
+ " From INFORMATION_SCHEMA.TABLES as S"
+ " Where S.TABLE_CATALOG = '" + __fDatabaseName + "'"
+ " Order By S.TABLE_NAME";
            DataTable vDataTable = __mSqlQuery(vQuery);
            foreach (DataRow vDataRow in vDataTable.Rows)
            {
                vReturn.Add(vDataRow["dsiTbl"].ToString());
            }

            return vReturn;
        }
        /// <summary>
        /// Получение описание таблицы в базе данных
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns>Описание таблицы</returns>
        public override string __mTableDescription(string pTableName)
        {
            string vQuery = "Select S.TABLE_NAME as dsiTbl"
+ ", (Select value From fn_listextendedproperty(NULL, 'user', 'dbo', 'table', S.TABLE_NAME, NULL, NULL)) as TblDsp"
+ " From INFORMATION_SCHEMA.TABLES as S"
+ " Where S.TABLE_CATALOG = '" + __fDatabaseName + "' and S.TABLE_NAME = '" + pTableName + "'"
+ " Order By S.TABLE_NAME";
            DataTable vDataTable = __mSqlQuery(vQuery);
            return vDataTable.Rows[0]["TblDsp"].ToString();
        }
        /// <summary>
        /// Очистка таблицы со сбросом идентификатора в 0
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns>[true] - таблица очищена, иначе - [false]</returns>
        public override bool __mTableTruncate(string pTableName)
        {
            __mSqlCommand("Truncate table " + pTableName);

            return true;
        }

        #endregion Таблицы

        #region Таблицы - Строки

        /// <summary>
        /// Получение записи из таблицы указанной идентификатором
        /// </summary>
        /// <param name="pClue">Идентификатор записи</param>
        /// <returns>[DataTable]</returns>
        public override DataTable __mTableRow(string pTableName, int pClue)
        {
            string vQuery = "Select Top 1 * From " + pTableName + " Where CLU = " + pClue.ToString();

            return __mSqlQuery(vQuery);
        }
        /// <summary>
        /// Получение записи из таблицы указанной идентификатором
        /// </summary>
        /// <param name="pGuid">Уникальный идентификатор записи</param>
        /// <returns>[DataTable]</returns>
        public override DataTable __mTableRow(string pTableName, Guid pGuid)
        {
            string vQuery = "Select Top 1 * From " + pTableName + " Where CLU = '" + pGuid.ToString() + "'";

            return __mSqlQuery(vQuery);
        }
        /// <summary>
        /// Установка текущего времени в качестве последнего времени изменения записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор записи</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        public override bool __mTableRowChangeTimeNow(string pTableName, int pClue)
        {
            bool vReturn = false; // Возвращаемое значение

            if (__mSqlCommand("Update " + pTableName + " Set CHG = GetDate() Where CLU = " + pClue.ToString()) > 0)
                vReturn = true;

            return vReturn;
        }
        /// <summary>
        /// Подсчет количества дублирующихся записей
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldS">Список полей</param>
        /// <returns>{DataTable} - Таблица с названием поля и количеством повторений</returns>
        public override DataTable __mTableRowsCountDouble(string pTableName, params string[] pFieldS)
        {
            string vQuery = "Select";
            string vGroup = "Group By";
            int vFieldNumber = 0;
            foreach (string vField in pFieldS)
            {
                if (vFieldNumber != 0)
                {
                    vQuery = vQuery + ", ";
                    vGroup = vGroup + ", ";
                }
                vQuery = vQuery + vField;
                vGroup = vGroup + vField;
            }
            vQuery = vQuery + "Count(*)";

            return __mSqlQuery(vQuery + " From " + pTableName + vGroup + " Having Count(*) > 1");
        }
        /// <summary>
        /// Подсчет количества записей удовлетворяющих условию
        /// </summary>
        /// <param name="pTableName"></param>
        /// <param name="pExpressionWhere"></param>
        /// <returns></returns>
        public override int __mTableRowsCountWhere(string pTableName, string pExpressionWhere)
        {
            int vReturn = -1; // Возвращаемое значение

            DataTable vDataTabl = __mSqlQuery("Select Count(*) as Cou From " + pTableName + " Where " + pExpressionWhere);
            if (vDataTabl != null)
            {
                if (vDataTabl.Rows.Count > 0)
                    vReturn = Convert.ToInt32(vDataTabl.Rows[0][0]);
            }
            return vReturn;
        }

        #endregion Таблицы - Строки

        #region Таблицы - Поля

        /// <summary>
        /// Проверка существования поля в таблице источника данных
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldName">Название поля</param>
        /// <returns>[true] - Поле существует, иначе - [false]</returns>
        public override bool __mTableColumnExists(string pTableName, string pFieldName)
        {
            bool vReturn = false; // Возвращаемое значение

            DataTable vDataTable = __mSqlQuery("Select Top 1 * From " + pTableName);
            vReturn = vDataTable.Columns.Contains(pFieldName);

            return vReturn;
        }
        /// <summary>
        /// Получение списка полей в таблице
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns>{DataTable} - заполненная списком полей указанной таблицы</returns>
        public override DataTable __mTableColumnS(string pTableName)
        {
            string vCommand = "Select Ordinal_Position as Ord,"
+ " Column_Name as dsiFld,"
+ "	IsNull((SELECT value FROM fn_listextendedproperty(NULL, 'schema', 'dbo', 'table', isc.Table_Name, 'column', Column_Name)), '') As Dcr,"
+ " IsNull(Data_Type, '') as Typ,"
+ " Case"
+ " When IsNull(Numeric_Precision, 0) != 0"
+ " Then IsNull(Numeric_Precision, 0)"
+ "	Else IsNull(Character_Maximum_Length, 0)"
+ " End Pre,"
+ " IsNull(Numeric_Scale, 0) as Sca,"
+ " Is_Nullable as Nul,"
+ " IsNull(Column_Default, '') as Dft,"
+ " IsNull(Collation_Name, '') as Cll"
+ " From INFORMATION_SCHEMA.COLUMNS as ISC"
+ " Inner Join  information_schema.tables IST ON ISC.table_name = IST.table_name"
+ " Where ISC.Table_Name = '" + pTableName + "' and Table_Type = 'Base Table'"
+ " Order By Ordinal_position";

            //"SELECT C.name as dsiFld" +
            //              ", DataType.name as Typ" +
            //              ", C.max_length as Pre" +
            //              ", C.scale as Sca" +
            //              ", C.is_nullable as Nul" +
            //              ",(SELECT value FROM fn_listextendedproperty (NULL, 'schema', 'dbo', 'table', T.name, 'column', C.name)) As Dcr" +
            //              " FROM sys.tables T" +
            //              " LEFT JOIN sys.columns C ON T.object_id = C.object_id " +
            //              " LEFT JOIN sys.types DataType ON C.user_type_id = DataType.user_type_id" +
            //              " WHERE T.name = '" + pTableName + "'";

            return __mSqlQuery(vCommand);
        }
        /// <summary>
        /// Получение информации о поле таблицы 
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pField">Название поля</param>
        /// <param name="pFieldInfo">Вид операции</param>
        /// <returns>Запрашиваемое значение, иначе [null]</returns>
        public override object __mTableColumnInfo(string pTableName, string pField, FIELDINFO pFieldInfo)
        {
            object vReturn = null; // Возвращаемое значение

            DataTable vDataTable = __mTableColumnS(pTableName);

            foreach (DataRow vDataRow in vDataTable.Rows)
            {
                if (Convert.ToString(vDataRow["dsiFld"]).ToUpper() == pField.ToUpper())
                {
                    switch (pFieldInfo)
                    {
                        case FIELDINFO.Description:
                            vReturn = Convert.ToString(vDataRow["Dcr"]);
                            break;
                        case FIELDINFO.Precision:
                            switch (Convert.ToString(vDataRow["Typ"]))
                            {
                                case "bit":
                                    vReturn = 1;
                                    break;
                                case "datetime":
                                    vReturn = 8;
                                    break;
                                case "varcharmax":
                                    vReturn = 0;
                                    break;
                                case "uniqueidentifier":
                                    vReturn = 16;
                                    break;
                                default:
                                    vReturn = Convert.ToInt32(vDataRow["Pre"]);
                                    break;
                            }
                            break;
                        case FIELDINFO.Scale:
                            vReturn = Convert.ToInt32(vDataRow["Sca"]);
                            break;
                        case FIELDINFO.Type:
                            vReturn = Convert.ToString(vDataRow["Typ"]);
                            break;
                        case FIELDINFO.Null:
                            vReturn = Convert.ToString(vDataRow["Nul"]);
                            break;
                    }
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Добавление колонки в таблицу
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pColumnName">Название колонки</param>
        /// <param name="pDataType">Тип колонки</param>
        /// <param name="pIsNull">Допустимость 'Null' значений</param>
        /// <param name="pColumnScale">Размер колонки</param>
        /// <param name="pColumnPrecision">Точность колонки</param>
        /// <returns>[true] - Колонка добавлена, иначе - [false]</returns>
        public override bool __mTableColumnAdd(string pTableName, string pColumnName, COLUMNSTYPES pDataType, bool pIsNull, int pColumnScale = 0, int pColumnPrecision = 0, string pDefaultValue = "0")
        {
            /// Формирование командной строки
            string vCommand = "Alter Table " + pTableName + " Add " + pColumnName + " " + pDataType.ToString().ToLower();
            /// Добавление в командную строку размерности поля
            switch (pDataType)
            {
                case COLUMNSTYPES.Bigint:
                    break;
                case COLUMNSTYPES.Binary:
                    vCommand += "(" + pColumnScale.ToString() + ")";
                    break;
                case COLUMNSTYPES.Bit:
                    break;
                case COLUMNSTYPES.Char:
                    vCommand += "(" + pColumnScale.ToString() + ")";
                    break;
                case COLUMNSTYPES.Date:
                    break;
                case COLUMNSTYPES.Datetime:
                    break;
                case COLUMNSTYPES.Datetime2:
                    break;
                case COLUMNSTYPES.Datetimeoffset:
                    break;
                case COLUMNSTYPES.Decimal:
                    vCommand += "(" + pColumnScale.ToString() + ", " + pColumnPrecision.ToString() + ")";
                    break;
                case COLUMNSTYPES.Float:
                    vCommand += "(" + pColumnScale.ToString() + ", " + pColumnPrecision.ToString() + ")";
                    break;
                case COLUMNSTYPES.Geography:
                    break;
                case COLUMNSTYPES.Geomentry:
                    break;
                case COLUMNSTYPES.Hierarchyid:
                    break;
                case COLUMNSTYPES.Image:
                    break;
                case COLUMNSTYPES.Int:
                    break;
                case COLUMNSTYPES.Money:
                    vCommand += "(" + pColumnScale.ToString() + ", " + pColumnPrecision.ToString() + ")";
                    break;
                case COLUMNSTYPES.Nchar:
                    break;
                case COLUMNSTYPES.Ntext:
                    break;
                case COLUMNSTYPES.Numeric:
                    vCommand += "(" + pColumnScale.ToString() + ", " + pColumnPrecision.ToString() + ")";
                    break;
                case COLUMNSTYPES.Nvarchar:
                    break;
                case COLUMNSTYPES.Real:
                    vCommand += "(" + pColumnScale.ToString() + ", " + pColumnPrecision.ToString() + ")";
                    break;
                case COLUMNSTYPES.Rowversion: // Уникальный идентификатор (в пределах базы данных). Увеличивается. Не основан на дате и времени
                    break;
                case COLUMNSTYPES.Smalldatetime:
                    break;
                case COLUMNSTYPES.Smallint:
                    break;
                case COLUMNSTYPES.Smallmoney:
                    vCommand += "(" + pColumnScale.ToString() + ", " + pColumnPrecision.ToString() + ")";
                    break;
                case COLUMNSTYPES.Sql_variant:
                    break;
                case COLUMNSTYPES.Text:
                    break;
                case COLUMNSTYPES.Time:
                    break;
                case COLUMNSTYPES.Timestamp: // Уникальный идентификатор (в пределах базы данных). Увеличивается. Не основан на дате и времени
                    break;
                case COLUMNSTYPES.Tinyint:
                    break;
                case COLUMNSTYPES.Uniqueidentifier: // 16-байтовый идентификатор GID
                    break;
                case COLUMNSTYPES.Varbinary:
                    break;
                case COLUMNSTYPES.Varchar:
                    break;
                case COLUMNSTYPES.Varcharmax:
                    break;
                case COLUMNSTYPES.Xml:
                    break;
            }
            /// Добавление в командную строку допустимости 'Null' значений
            if (pIsNull == true)
                vCommand += " Null";
            else
                vCommand += " Not Null";
            /// Добавление в командную строку значения по умолчанию
            if (String.IsNullOrEmpty(pDefaultValue) == false)
                vCommand += " Default " + pDefaultValue;

            return __mSqlCommand(vCommand) == 0 ? false : true;
        }
        /// <summary>
        /// Удаление колонки из таблицы
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pColumnName">Название колонки</param>
        /// <returns>[true] - Колонка удалена, иначе - [false]</returns>
        public override bool __mTableColumnDrop(string pTableName, string pColumnName)
        {
            /// Формирование командной строки
            string vCommand = "Alter Table " + pTableName + " Drop " + pColumnName;
            return __mSqlCommand(vCommand) == 0 ? false : true;
        }

        #endregion Таблицы - Поля

        #region Транзакции

        /// <summary>
        /// Закрытие транзакции
        /// </summary>
        /// <param name="pCommit">Условие закрытия транзакции. [true] - [Commit], [false] - [RollBack]</param>
        /// <returns>[true] - Транзакция закрыта, иначе - [false]</returns>
        public override bool __mTransactionOff(bool pCommit)
        {
            bool vReturn = true; // Возвращаемое значение

            if (fTransaction != null)
            {
                if (pCommit == true)
                {
                    try
                    {
                        fTransaction.Commit();
                    }
                    catch (SqlException vException)
                    {
                        _fError.__fException = vException;
                        _fError.__fLineInProcedure_ = _fClassLine_;
                        _fError.__fProcedure_ = _fClassProcedure_;
                        _fError.__fErrorType_ = ERRORSTYPES.Data;
                        _fError.__mMessageBuild("Невозможно завершить транзакцию");
                        _fError.__mPropertyAdd("Источник данных: {0}", __fDataSourceType.ToString());
                        _fError.__mPropertyAdd("Север: {0}", __fServer);
                        _fError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                        _fError.__mPropertyAdd("База данных: {0}", __fDatabaseName);
                        if (fConnection == null)
                            _fError.__mPropertyAdd("Поключение: {0}", "null");
                        else
                            _fError.__mPropertyAdd("Подключение: {0}", fConnection.State.ToString());
                        if (pCommit == true)
                            _fError.__mPropertyAdd("Команда: {0}", datApplication.__oTunes.__mTranslate("Закрыть транзакцию"));
                        else
                            _fError.__mPropertyAdd("Команда: {0}", datApplication.__oTunes.__mTranslate("Отменить транзакцию"));

                        datApplication.__oErrorsHandler.__mShow(_fError);
                        _fError.__mClear();
                        vReturn = false;
                    }
                    vReturn = true;
                }
                else
                {
                    try
                    {
                        fTransaction.Rollback();
                        vReturn = true;
                    }
                    catch
                    {
                        vReturn = false;
                    }
                }
                if (vReturn == true)
                    fTransaction = null; // ???
            }

            return vReturn;
        }
        /// <summary>
        /// Открытие транзакции
        /// </summary>
        /// <returns>[true] - транзация создана, иначе - [false]</returns>
        public override bool __mTransactionOn()
        {
            bool vReturn = true; // Возвращаемое значение

            if (fConnection != null)
            {
                try
                {
                    fTransaction = fConnection.BeginTransaction();
                }
                catch (SqlException vException)
                {
                    _fError.__fException = vException;
                    _fError.__fLineInProcedure_ = _fClassLine_;
                    _fError.__fProcedure_ = _fClassProcedure_;
                    _fError.__fErrorType_ = ERRORSTYPES.Data;
                    _fError.__mMessageBuild("Не возможно открыть транзакцию");
                    _fError.__mPropertyAdd("Источник данных: {0}", __fDataSourceType.ToString());
                    _fError.__mPropertyAdd("Сервер: {0}", __fServer);
                    _fError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                    _fError.__mPropertyAdd("База данных: {0}", __fDatabaseName);
                    if (fConnection == null)
                        _fError.__mPropertyAdd("Connection: null");
                    else
                        _fError.__mPropertyAdd("Connection: {0}", fConnection.State.ToString());

                    datApplication.__oErrorsHandler.__mShow(_fError);
                    _fError.__mClear();

                    vReturn = false;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Закрытие объединяющей транзакции
        /// </summary>
        /// <param name="pCommit">Условие закрытия транзакции. [true] - [Commit], [false] - [RollBack]</param>
        /// <returns>[true] - Транзакция закрыта, иначе - [false]</returns>
        public override bool __mTransactionUnionOff(bool pCommit)
        {
            bool vReturn = true; // Возвращаемое значение

            if (fTransactionUnion != null)
            {
                if (pCommit == true)
                {
                    try
                    {
                        fTransactionUnion.Commit();
                    }
                    catch (SqlException vException)
                    {
                        _fError.__fException = vException;
                        _fError.__fLineInProcedure_ = _fClassLine_;
                        _fError.__fProcedure_ = _fClassProcedure_;
                        _fError.__fErrorType_ = ERRORSTYPES.Data;
                        _fError.__mMessageBuild("Невозможно завершить объединяющую транзакцию");
                        _fError.__mPropertyAdd("Источник данных: {0}", __fDataSourceType.ToString());
                        _fError.__mPropertyAdd("Север: {0}", __fServer);
                        _fError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                        _fError.__mPropertyAdd("База данных: {0}", __fDatabaseName);
                        if (fConnection == null)
                            _fError.__mPropertyAdd("Поключение: {0}", "null");
                        else
                            _fError.__mPropertyAdd("Подключение: {0}", fConnection.State.ToString());
                        if (pCommit == true)
                            _fError.__mPropertyAdd("Команда: {0}", datApplication.__oTunes.__mTranslate("Закрыть объединяющую транзакцию"));
                        else
                            _fError.__mPropertyAdd("Команда: {0}", datApplication.__oTunes.__mTranslate("Отменить объединяющую  транзакцию"));

                        datApplication.__oErrorsHandler.__mShow(_fError);
                        _fError.__mClear();

                        vReturn = false;
                    }
                    vReturn = true;
                }
                else
                {
                    try
                    {
                        fTransactionUnion.Rollback();
                        vReturn = true;
                    }
                    catch
                    {
                        vReturn = false;
                    }
                }
                if (vReturn == true)
                    fTransactionUnion = null; // ???
            }

            return vReturn;
        }
        /// <summary>
        /// Открытие объединяющей транзакции
        /// </summary>
        /// <returns>[true] - транзация создана, иначе - [false]</returns>
        public override bool __mTransactionUnionOn()
        {
            bool vReturn = true; // Возвращаемое значение

            if (fConnection != null)
            {
                try
                {
                    fTransactionUnion = fConnection.BeginTransaction();
                }
                catch (SqlException vException)
                {
                    _fError.__fException = vException;
                    _fError.__fLineInProcedure_ = _fClassLine_;
                    _fError.__fProcedure_ = _fClassProcedure_;
                    _fError.__fErrorType_ = ERRORSTYPES.Data;
                    _fError.__mMessageBuild("Не возможно открыть объединяющую транзакцию");
                    _fError.__mPropertyAdd("Источник данных: {0}", __fDataSourceType.ToString());
                    _fError.__mPropertyAdd("Сервер: {0}", __fServer);
                    _fError.__mPropertyAdd("Логин: {0}", __fServerLogin);
                    _fError.__mPropertyAdd("База данных: {0}", __fDatabaseName);
                    if (fConnection == null)
                        _fError.__mPropertyAdd("Connection: null");
                    else
                        _fError.__mPropertyAdd("Connection: {0}", fConnection.State.ToString());

                    datApplication.__oErrorsHandler.__mShow(_fError);
                    _fError.__mClear();

                    vReturn = false;
                }
            }

            return vReturn;
        }

        #endregion Транзакции

        #region Функции - Идентификаторы

        /// <summary>
        /// Получение идентификатора по учетному коду
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pCode">Учетный код</param>
        /// <returns>[<=0] - Запись не найдена, иначет - идентификатор</returns>
        public override int __mClueByCode(string pTableName, int pCode)
        {
            int vReturn = -1; // Возвращаемое значение
            object vValue = null;
            if (__mTableColumnExists(pTableName, "cod" + pTableName) == true)
                vValue = __mSqlValue("Select CLU From " + pTableName + " Where cod" + pTableName + "=" + pCode.ToString());
            if (__mTableColumnExists(pTableName, "cgz" + pTableName) == true)
                vValue = __mSqlValue("Select CLU From " + pTableName + " Where cgz" + pTableName + "=" + pCode.ToString());

            if (vValue != null)
                if (vValue.ToString() != "0")
                    vReturn = Convert.ToInt32(vValue);

            return vReturn;
        }
        /// <summary>
        /// Получение идентификатора записи по значению поля названия
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldName">Значение поля названия</param>
        /// <returns>[<=0] - Запись не найдена, иначет - идентификатор</returns>
        public override int __mClueByName(string pTableName, string pFieldName)
        {
            int vReturn = -1; // Возвращаеое значение

            DataTable vDataTabl = __mSqlQuery("Select CLU From " + pTableName + " Where dsi" + pTableName + " = '" + pFieldName + "'");
            if (vDataTabl.Rows.Count > 0)
                vReturn = Convert.ToInt32(vDataTabl.Rows[0][0]);

            return vReturn;
        }
        /// <summary>
        /// Получение идентификатора записи по названию поля опции
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldOptionName">Имя поля опции</param>
        /// <returns>[<=0] - Запись не найдена, иначет - идентификатор</returns>
        public override int __mClueByOption(string pTableName, string pFieldOptionName)
        {
            int vReturn = -1; // Возвращаеое значение

            DataTable vDataTabl = __mSqlQuery("Select CLU From " + pTableName + " Where " + pFieldOptionName + " = 1");
            if (vDataTabl.Rows.Count > 0)
                vReturn = Convert.ToInt32(vDataTabl.Rows[0][0].ToString());

            return vReturn;
        }
        /// <summary>
        /// Проверка существования идентификатора в таблице
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор записи</param>
        /// <returns>[<=0] - Запись не найдена, иначет - идентификатор</returns>
        public override bool __mClueExists(string pTableName, int pClue)
        {
            if (__mTableRowsCountWhere(pTableName, " CLU = " + pClue.ToString()) > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Получение идентификатора последеней вставленной записи в таблицу
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns>[<=0] - Запись не найдена, иначе - идентификатор</returns>
        public override int __mClueLastInserted(string pTableName)
        {
            int vReturn = -1; // Возвращаемое значение
            DataTable vDataTable = __mSqlQuery("Select IDENT_CURRENT('" + pTableName + "')");
            if (vDataTable.Rows.Count > 0)
                vReturn = Convert.ToInt32(vDataTable.Rows[0][0].ToString());

            return vReturn;
        }

        #endregion Функции - Идентификаторы

        #region Функции - Названия

        /// <summary>
        /// Получение названия по идентификатору записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор искомой строки</param>
        /// <returns>Значение поля 'Название'</returns>
        public override string __mNameByClue(string pTableName, int pClue)
        {
            string vReturn = datApplication.__oTunes.__mTranslate("не определено"); // Возвращаемое значение

            DataTable vDataTabl = __mSqlQuery("Select dsi" + pTableName + " From " + pTableName + " Where  CLU = " + pClue.ToString());
            if (vDataTabl != null)
                if (vDataTabl.Rows.Count > 0)
                    vReturn = vDataTabl.Rows[0][0].ToString().Trim();

            return vReturn;
        }
        /// <summary>
        /// Получение названия по идентификатору записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pCode">Учетный код искомой строки</param>
        /// <returns>Значение поля 'Название'</returns>
        public override string __mNameByCode(string pTableName, int pCode)
        {
            string vReturn = "null"; // Возвращаемое значение
            DataTable vDataTabl = new DataTable();

            if (__mTableColumnExists(pTableName, "cod" + pTableName) == true)
                vDataTabl = __mSqlQuery("Select dsi" + pTableName + " From " + pTableName + " Where cod" + pTableName + " = " + pCode.ToString());
            if (__mTableColumnExists(pTableName, "cgz" + pTableName) == true)
                vDataTabl = __mSqlQuery("Select dsi" + pTableName + " From " + pTableName + " Where cgz" + pTableName + " = " + pCode.ToString());

            if (vDataTabl.Rows.Count > 0)
                vReturn = vDataTabl.Rows[0][0].ToString().Trim();

            return vReturn;
        }
        /// <summary>
        /// Получение названия справочника по названию и значению опции
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pOptionName">Название опции</param>
        /// <returns>Значение поля 'Название' </returns>
        public override string __mNameByOption(string pTableName, string pOptionName)
        {
            string vReturn = datApplication.__oTunes.__mTranslate("не определено"); // Возвращаемое значение

            DataTable vDataTabl = __mSqlQuery("Select dsi" + pTableName + " From " + pTableName + " Where  " + pOptionName + " = 1");
            if (vDataTabl != null)
                if (vDataTabl.Rows.Count > 0)
                    vReturn = vDataTabl.Rows[0][0].ToString().Trim();

            return vReturn;
        }
        /// <summary>
        /// Проверка существования названия
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pValue">Значение поля названия</param>
        /// <param name="pClueSkip">Идентификатор исключаемой записи</param>
        /// <returns>[true] - Указанное название уже существует, иначе - [false]</returns>
        public override bool __mNameExists(string pTableName, string pValue, int pClueSkip)
        {
            bool vReturn = false; // Возвращаемое значение
            string vQuery = "Select dsi" + pTableName + " as dsi" + pTableName + " From " + pTableName + " Where dsi" + pTableName + " = '" + pValue + "' and CLU != '" + pClueSkip.ToString() + "'";
            DataTable vDataTabl = __mSqlQuery(vQuery);

            if (vDataTabl.Rows.Count > 0)
                vReturn = true;

            return vReturn;
        }
        /// <summary>
        /// Проверка существования названия
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pValue">Значение поля названия</param>
        /// <param name="pClueSkip">Идентификатор исключаемой записи</param>
        /// <returns>[true] - Указанное название уже существует, иначе - [false]</returns>
        public override bool __mNameExists(string pTableName, string pValue, string pWhereExpression)
        {
            bool vReturn = false; // Возвращаемое значение
            string vQuery = "Select dsi" + pTableName + " as dsi" + pTableName + " From " + pTableName + " Where " + pWhereExpression;
            DataTable vDataTabl = __mSqlQuery(vQuery);

            if (vDataTabl.Rows.Count > 0)
                vReturn = true;

            return vReturn;
        }

        #endregion Функции - Названия

        #region Функции - Учетные коды

        /// <summary>
        /// Получение значения поля учетного кода по идентификатору записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор записи</param>
        /// <returns>Значение поля учетного кода</returns>
        public override int __mCodeByClue(string pTableName, int pClue)
        {
            return Convert.ToInt32(__mSqlValue("Select cod" + pTableName + " From " + pTableName + " Where CLU = " + pClue.ToString()));
        }
        /// <summary>
        /// Проверка существования учетного кода исключая идентификатор записи указанный в 'pClueSkip'
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pCodeCheck">Проверяемый учетный код</param>
        /// <param name="pClueSkip">Идентификатор записи который нужно исключить из поиска</param>
        /// <returns>[true] - Дублирующийся учетный код найден, иначе - [false]</returns>
        public override bool __mCodeExists(string pTableName, int pCodeCheck, int pClueSkip)
        {
            bool vReturn = false; // Возвращаемое значение
            if (__mTableColumnExists(pTableName, "cod" + pTableName) == true)
                if (__mTableRowsCountWhere(pTableName, "cod" + pTableName + " = " + pCodeCheck.ToString() + " and CLU != " + pClueSkip.ToString()) > 0)
                    vReturn = true;
            if (__mTableColumnExists(pTableName, "cgz" + pTableName) == true)
                if (__mTableRowsCountWhere(pTableName, "cgz" + pTableName + " = " + pCodeCheck.ToString() + " and CLU != " + pClueSkip.ToString()) > 0)
                    vReturn = true;
            return vReturn;
        }
        /// <summary>
        /// Вычисление нового учетного кода
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClueSkip">Идентификатор записи исключаемой из обработки</param>
        /// <param name="pCodeNewCalculateType">Порядок расчета нового учетного кода</param>
        /// <param name="pValueMaximal">Минимальное значение кода</param>
        /// <param name="pValueMinimal">Максимальное значение кода</param>
        /// <returns>Новый учетный код</returns>
        public override int __mCodeNew(string pTableName, int pClueSkip, CODESNEWTYPES pCodeNewCalculateType, int pValueMinimal = 1, int pValueMaximal = 999999)
        {
            // virtual - так как у товара нужно переопределить - код товара ведеться с 1000
            int vReturn = -1; // Возвращаемый результат
            int vLockClue = __mLockOn(pTableName, pClueSkip); // Идентификатор блокировки

            /// Выполнена блокировка
            if (vLockClue > 0)
            {
                /// Расчет нового кода приращением
                if (pCodeNewCalculateType == CODESNEWTYPES.Next)
                {
                    DataTable vDataTabl = __mSqlQuery("Select Max(cod" + pTableName + ") as cod" + pTableName + " From " + pTableName);
                    if (vDataTabl.Rows.Count > 0)
                    {
                        int vCodeLast = Convert.ToInt32(vDataTabl.Rows[0][0]);
                        if (vCodeLast < pValueMaximal)
                            vReturn = vCodeLast + 1;
                        else
                        {
                            _fError.__fErrorType_ = ERRORSTYPES.Programming;
                            _fError.__mMessageBuild("Количество используемых учетных кодов исчерпано");
                            _fError.__fHelpFileName_ = "Errors";
                            _fError.__fHelpTopic_ = "";
                            _fError.__fLineInProcedure_ = _fClassLine_;
                            _fError.__fProcedure_ = _fClassProcedure_;
                            _fError.__mPropertyAdd("Последний учетный код = {0}", vCodeLast);
                            _fError.__mPropertyAdd("Минимальный учетный код = {0}", pValueMinimal);
                            _fError.__mPropertyAdd("Максимальный учетный код = {0}", pValueMaximal);

                            datApplication.__oErrorsHandler.__mShow(_fError);
                            _fError.__mClear();

                            vReturn = -1;
                        }
                    }
                    else
                        vReturn = pValueMinimal; // Если записей нет присваиваем минимальное значение
                }
                /// Расчет нового кода методом поиска попущенных кодов
                if (pCodeNewCalculateType == CODESNEWTYPES.Skiped)
                {
                    int vAmount = pValueMinimal; // Счетчик
                    do
                    {
                        if (__mCodeExists(pTableName, vAmount, -1) == true)
                            vAmount++;
                        else
                            vReturn = vAmount;
                    } while (vReturn == -1 & vAmount >= pValueMinimal & vAmount <= pValueMaximal);
                    if (vReturn == -1)
                    {
                        _fError.__fErrorType_ = ERRORSTYPES.Programming;
                        _fError.__mMessageBuild("Количество используемых учетных кодов исчерпано");
                        _fError.__fHelpFileName_ = "Errors";
                        _fError.__fHelpTopic_ = "";
                        _fError.__fLineInProcedure_ = _fClassLine_;
                        _fError.__fProcedure_ = _fClassProcedure_ ;
                        _fError.__mPropertyAdd("Минимальный учетный код = {0}", pValueMinimal);
                        _fError.__mPropertyAdd("Максимальный учетный код = {0}", pValueMaximal);

                        datApplication.__oErrorsHandler.__mShow(_fError);
                        _fError.__mClear();
                    }
                } /// Расчет нового кода поиском пропущенных кодов

                __mLockOff(vLockClue); /// Снятие блокировки
            }

            return vReturn;

        }
        /// <summary>
        /// Вычисление нового учетного кода по нескольким полям
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClueSkip">Идентификатор записи исключаемой из обработки</param>
        /// <param name="pCodeNewCalculateType">Порядок расчета нового учетного кода</param>
        /// <param name="pValueMaximal">Минимальное значение кода</param>
        /// <param name="pValueMinimal">Максимальное значение кода</param>
        /// <param name="pFieldS">Список дополнительных полей</param>
        /// <returns>[0] - неудалось вычислить учетный код, иначе - новый учетный код</returns>
        public override int __mCodeNewGroup(string pTableName, int pClueSkip, CODESNEWTYPES pCodeNewCalculateType, int pValueMinimal, int pValueMaximal, ArrayList pFieldS)
        {
            return -1;
        }

        #endregion Функции - Учетные коды

        #region Функции - Позиция в документе

        /// <summary>
        /// Вычисление новой позиции в документе
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClueSkip">Идентификатор записи исключаемой из обработки</param>
        /// <param name="pWhere">Условие отбирающее документ</param>
        /// <returns></returns>
        public override int __mPositionNew(string pTableName, int pClueSkip, string pWhere)
        {
            int vReturn = -1; // Возвращаемое значение

            int vAmount = 1; // Максимальное количество позиций в докумменте
            do
            {
                if (__mTableRowsCountWhere(pTableName, pWhere + " and Pos = " + vAmount.ToString() + " and CLU != " + pClueSkip.ToString()) > 0) // Позиция с номером vAmount найдена
                    vAmount++;
                else
                    vReturn = vAmount; // Найдена не используемая позиция
            } while (vReturn == -1 & vAmount >= 1 & vAmount <= 1000);
            if (vReturn == -1)
            {
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                _fError.__mMessageBuild("Количество используемых учетных кодов исчерпано");
                _fError.__fHelpFileName_ = "Errors";
                _fError.__fHelpTopic_ = "";
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fProcedure_ = _fClassProcedure_;
                _fError.__mPropertyAdd("Минимальный номер позиции = 1");
                _fError.__mPropertyAdd("Максимальный номер позиции = 1000");

                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
            }

            return vReturn;
        }

        #endregion Функции - Позиция в документе

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Работа с локальной базой данных
        /// </summary>
        public bool __fLocalDB = false;
        /// <summary>
        /// Использование старой версии LocalDB
        /// </summary>
        public bool __fLocalDBOldVersion = false;

        #endregion Атрибуты

        #region - Внутренние

        /// <summary>
        /// Указатель на соединение с источником данных
        /// </summary>
        private SqlConnection fConnection = null;
        /// <summary>
        /// Указатель на открытую транзакцию
        /// </summary>
        private SqlTransaction fTransaction = null;
        /// <summary>
        /// Указатель на объединяющую транзакцию
        /// </summary>
        private SqlTransaction fTransactionUnion = null;

        #endregion Внутренние

        #region - Константы

        /// <summary>
        /// Первод каретки
        /// </summary>
        private const string CRLF = "\r\n";

        #endregion Константы

        #endregion ПОЛЯ   
    }
}
