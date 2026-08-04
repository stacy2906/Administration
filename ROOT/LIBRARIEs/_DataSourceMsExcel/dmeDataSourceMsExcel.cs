using nlApplication;
using nlData;
using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

namespace nlDataSourceMsExcel
{
    /// <summary>
    /// Файл dmeDataSourceMsExcel.cs
    /// </summary>
    /// <remarks>Класс-источник данных 'MS Excel'</remarks>
    public class dmeDataSourceMsExcel : datUnitDataSource
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

            __fDataSourceType = DATASOURCETYPES.MsExcel;

            return;
        }

        #endregion Объект

        #endregion Поведение

        #region - Процедуры

        #region Sql операции

        public override int __mSqlCommand(string pCommand)
        {
            int vReturn = -1; // Возвращаемое значение

            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Команда{0} {1}", ":", pCommand);

            if (String.IsNullOrEmpty(pCommand) == true)
            {
                _fError.__mReasonAdd("Не указана команда");
                _fError.__fLineInProcedure_ = _fClassLine_;
                datApplication.__oErrorsHandler.__mShow(_fError);
                return vReturn;
            }

            bool vTransactionUsed = false; // Использование транзакции

            try
            {
                if (__fOnLine == false & fConnection == null)
                {
                    __mConnectionOn(); /// Установка соединения
                }

                OleDbCommand vSqlCommand = new OleDbCommand(pCommand, fConnection);
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
            catch (OleDbException vException)
            {
                _fError.__fException = vException;
                _fError.__fErrorType_ = ERRORSTYPES.Data;
                _fError.__fMessage_ = "MS Excel не может выполнить команду";
                _fError.__mPropertyAdd("Вид источника данных: {0}", __fDataSourceType.ToString());
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
        /// Отправка запроса источнику данных
        /// </summary>
        /// <param name="pQuery">Условие запроса</param>
        /// <returns>{DataTable} - с данными удовлетворяющими условию "pQuery"</returns>
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

                OleDbDataAdapter vDataAdapter = new OleDbDataAdapter(pQuery, fConnection);

                vDataTable = new DataTable();
                vDataAdapter.Fill(vDataTable);

                if (__fOnLine == false & fConnection != null)
                    __mConnectionOff();
            }
            catch (SqlException vException)
            {
                _fError.__fException = vException;
                _fError.__fProcedure_ = _fClassProcedure_;
                _fError.__fErrorType_ = ERRORSTYPES.Data;
                _fError.__mMessageBuild("Ошибка при выполнении запроса");
                _fError.__mPropertyAdd("Вид источника данных: {0}", __fDataSourceType.ToString());
                _fError.__mPropertyAdd("База данных: {0}", __fDatabaseName);
                _fError.__mPropertyAdd("Содержание запроса: {0}", pQuery);
                _fError.__mPropertyAdd("Транзакция: {0}", vTransactionUsed.ToString());

                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
            }

            return vDataTable;
        }

        #endregion Sql операции

        public override bool __mDatabaseExists(string pDatabaseName)
        {
            //    public class ScriptMain
            //{
            //    public void Main()
            //    {
            //        string fileToTest;
            //        string tableToTest;
            //        string connectionString;
            //        OleDbConnection excelConnection;
            //        DataTable excelTables;
            //        string currentTable;

            //        fileToTest = Dts.Variables["ExcelFile"].Value.ToString();
            //        tableToTest = Dts.Variables["ExcelTable"].Value.ToString();

            //        Dts.Variables["ExcelTableExists"].Value = false;
            //        if (File.Exists(fileToTest))
            //        {
            //            connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
            //            "Data Source=" + fileToTest + ";Extended Properties=Excel 12.0";
            //            excelConnection = new OleDbConnection(connectionString);
            //            excelConnection.Open();
            //            excelTables = excelConnection.GetSchema("Tables");
            //            foreach (DataRow excelTable in excelTables.Rows)
            //            {
            //                currentTable = excelTable["TABLE_NAME"].ToString();
            //                if (currentTable == tableToTest)
            //                {
            //                    Dts.Variables["ExcelTableExists"].Value = true;
            //                }
            //            }
            //        }

            //        Dts.TaskResult = (int)ScriptResults.Success;

            //    }
            //}  
            return false;
        }
        /// <summary>
        /// Получение списка листов в Excel документе
        /// </summary>
        public override ArrayList __mTablesList()
        {
            ArrayList vReturn = new ArrayList(); // Возвращаемое значение 
            DataTable vDataTable = new DataTable();

            try
            {
                /// Установка соединения
                if (__fOnLine == false & fConnection == null)
                {
                    __mConnectionOn();
                }
                vDataTable = fConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                if (__fOnLine == false & fConnection != null)
                    __mConnectionOff();
            }
            catch (SqlException vException)
            {
                int dfd = 0;
            }

            foreach (DataRow vDataRow in vDataTable.Rows)
            {
                vReturn.Add(vDataRow["TABLE_NAME"]);
            }

            return vReturn;
        }

        #region Подключение

        /// <summary>
        /// Построение строки подключения к источнику данных
        /// </summary>
        /// <param name="pXlsxExtension">Использование файла с расширением XLSX</param>
        /// <returns>[true] - строка построена, иначе - [false]</returns>
        protected override bool __mConnectionLineBuild(bool pXlsxExtension = true)
        {
            switch (pXlsxExtension)
            {
                // параметр: IMEX=1 для обработки первой колонки, Это принудительно говорит драйверу: читай все данные как текст 
                case false:
                    __fConnectionLine = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path.Combine(__fDatabasePath, __fDatabaseName) + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;'";
                    break;
                case true:
                    __fConnectionLine = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path.Combine(__fDatabasePath, __fDatabaseName) + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'";
                    //__fConnectionLine = @"Provider=SQLOLEDB.1;Data Source=" + Path.Combine(__fDatabasePath, __fDatabaseName) + ";Extended Properties='Excel 12.0;HDR=YES;'";
                    break;
                default:
                    break;
            }

            return true;
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
                    _fError.__fProcedure_ = _fClassProcedure_;
                    _fError.__fLineInProcedure_ = _fClassLine_;
                    _fError.__fErrorType_ = ERRORSTYPES.Data;
                    _fError.__mMessageBuild("Не удалось отключить '{0}'", __fDatabaseName);
                    _fError.__mPropertyAdd("Тип источника данных: {0}", __fDataSourceType.ToString());
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
                    __mConnectionLineBuild(__fOledbVersion12);
                    fConnection = new OleDbConnection(String.Format(__fConnectionLine, Path.Combine(__fDatabasePath, __fDatabaseName)));
                    fConnection.Open();
                }
                catch (OleDbException vException)
                {
                    _fError.__fException = vException;
                    _fError.__fProcedure_ = _fClassProcedure_;
                    _fError.__fLineInProcedure_ = _fClassLine_;
                    _fError.__fErrorType_ = ERRORSTYPES.Data;
                    _fError.__mMessageBuild("Не удалось подключить {0}", __fDatabaseName);
                    _fError.__mPropertyAdd("Тип источника данных: {0}", __fDataSourceType.ToString());
                    _fError.__mPropertyAdd("База данных: {0}", __fDatabaseName);

                    datApplication.__oErrorsHandler.__mShow(_fError);
                    _fError.__mClear();

                    vReturn = false;
                }
            }
            else
            {
                try
                {
                    fConnection = new OleDbConnection(String.Format(__fConnectionLine, Path.Combine(__fDatabasePath, __fDatabaseName)));
                    fConnection.Open();
                }
                catch (OleDbException vException)
                {
                    _fError.__fException = vException;
                    _fError.__fProcedure_ = _fClassProcedure_;
                    _fError.__fLineInProcedure_ = _fClassLine_;
                    _fError.__fErrorType_ = ERRORSTYPES.Data;
                    _fError.__mMessageBuild("Не удалось подключить {0}", __fDatabaseName);
                    _fError.__mPropertyAdd("Тип источника данных: {0}", __fDataSourceType.ToString());
                    _fError.__mPropertyAdd("База данных: {0}", __fDatabaseName);

                    datApplication.__oErrorsHandler.__mShow(_fError);
                    _fError.__mClear();

                    vReturn = false;
                }
            }

            return vReturn;
        }

        #endregion Подключение

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Использование провайдера 'Microsoft.ACE.OLEDB.12.0'
        /// </summary>
        public bool __fOledbVersion12 = true;

        #endregion Атрибуты

        #region - Внутренние

        /// <summary>
        /// Указатель на соединение с источником данных
        /// </summary>
        private OleDbConnection fConnection = null;
        /// <summary>
        /// Указатель на открытую транзакцию
        /// </summary>
        private OleDbTransaction fTransaction = null;

        #endregion Внутренние

        #endregion ПОЛЯ
    }
}
