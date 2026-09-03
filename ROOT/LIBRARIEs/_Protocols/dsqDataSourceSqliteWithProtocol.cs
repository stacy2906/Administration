using System;
using System.Collections;
using System.Data;
using System.Data.SQLite;
using System.IO;
using nlData;

namespace nlDataSourceSqlite
{
    /// <summary>
    /// Файл dsqDataSourceSqliteWithProtocol.cs
    /// </summary>
    /// <remarks>Самостоятельный исполнитель SQL-команд для 'dsqProtocols'. Намеренно НЕ наследует 'dsqDataSourceSqlite'
    /// (его поля соединения '_fConnection'/'_fTransaction' объявлены как 'private' и недоступны из наследника, а его
    /// методы при сбое SQL вызывают 'appApplication.__oErrorsHandler.__mShow'). Это принципиально важно: как только
    /// 'dsqProtocols' становится активным 'appApplication.__oProtocols', обычная цепочка обработки ошибок вызвала бы
    /// бесконечную рекурсию при любом сбое SQL - ошибка формирует вызов в обработчик ошибок, тот обращается к
    /// 'appApplication.__oProtocols' (то есть снова к этому же классу), чтобы записать протокол об ошибке, что347
    /// снова выполняет SQL, который может снова дать сбой, и так далее. Поэтому здесь сбои SQL не поднимаются
    /// в общий обработчик ошибок, а пишутся напрямую в локальный текстовый файл рядом с базой данных.</remarks>
    /// <conception>Lucasin V.</conception>
    public class dsqDataSourceSqliteWithProtocol : datUnitDataSource
    {
        #region = МЕТОДЫ

        #region - Поведение

        protected override void _mObjectAssembly()
        {
            __fDataSourceType = DATASOURCETYPES.Sqlite;
        }

        #endregion Поведение

        #region - Процедуры

        #region Sql операции

        public override int __mSqlCommand(string pCommand)
        {
            int vReturn = -1;

            try
            {
                if (__fOnLine == false && fConnection == null)
                    __mConnectionOn();

                SQLiteCommand vSqliteCommand = new SQLiteCommand(pCommand, fConnection);
                if (fTransaction != null)
                    vSqliteCommand.Transaction = fTransaction;

                vReturn = vSqliteCommand.ExecuteNonQuery();

                if (fTransaction == null && __fOnLine == false && fConnection != null)
                    __mConnectionOff();
            }
            catch (Exception vException)
            {
                mLogFailureLocally("__mSqlCommand", pCommand, vException);
            }

            return vReturn;
        }
        public override DataTable __mSqlQuery(string pQuery)
        {
            DataTable vDataTable = new DataTable(); // Пустая, а не null - чтобы вызывающий код (foreach по .Rows) не падал при сбое

            try
            {
                if (__fOnLine == false && fConnection == null)
                    __mConnectionOn();

                SQLiteCommand vSqliteCommand = new SQLiteCommand(pQuery, fConnection);
                if (fTransaction != null)
                    vSqliteCommand.Transaction = fTransaction;

                SQLiteDataReader vSqlDataReader = vSqliteCommand.ExecuteReader();
                vDataTable.Load(vSqlDataReader);

                if (fTransaction == null && __fOnLine == false && fConnection != null)
                    __mConnectionOff();
            }
            catch (Exception vException)
            {
                mLogFailureLocally("__mSqlQuery", pQuery, vException);
            }

            return vDataTable;
        }
        public override object __mSqlValue(string pCommand)
        {
            object vReturn = null;

            try
            {
                if (__fOnLine == false && fConnection == null)
                    __mConnectionOn();

                SQLiteCommand vSqliteCommand = new SQLiteCommand(pCommand, fConnection);
                if (fTransaction != null)
                    vSqliteCommand.Transaction = fTransaction;

                vReturn = vSqliteCommand.ExecuteScalar();

                if (fTransaction == null && __fOnLine == false && fConnection != null)
                    __mConnectionOff();
            }
            catch (Exception vException)
            {
                mLogFailureLocally("__mSqlValue", pCommand, vException);
            }

            return vReturn;
        }
        public override object __mSqlValue(string pTableName, string pFieldName, string pExpressionWhere)
        {
            string vCommand = "Select " + pFieldName + " From " + pTableName + " Where " + pExpressionWhere;
            return __mSqlValue(vCommand);
        }
        /// <summary>
        /// Подсчёт количества записей в таблице, удовлетворяющих условию.
        /// </summary>
        /// <remarks>В базовом классе 'datUnitDataSource' не реализован (заглушка всегда возвращает -1),
        /// и 'dsqDataSourceSqlite' его тоже не переопределяет - здесь дана настоящая реализация.</remarks>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pExpressionWhere">Условие проверки</param>
        /// <returns>Количество записей, удовлетворяющих условию</returns>
        public override int __mSqlCount(string pTableName, string pExpressionWhere)
        {
            string vCommand = "Select Count(*) From " + pTableName
                + (string.IsNullOrEmpty(pExpressionWhere) ? "" : " Where " + pExpressionWhere);
            object vValue = __mSqlValue(vCommand);

            if (vValue == null || vValue == DBNull.Value)
                return 0;

            return Convert.ToInt32(vValue);
        }

        #endregion Sql операции

        #region База данных

        public override bool __mDatabaseCreate()
        {
            bool vReturn = true;

            if (File.Exists(Path.Combine(__fDatabasePath, __fDatabaseName)) == false)
            {
                try
                {
                    SQLiteConnection.CreateFile(Path.Combine(__fDatabasePath, __fDatabaseName));
                    SQLiteConnection vConnection = new SQLiteConnection(string.Format("Data Source={0}; Version=3;", Path.Combine(__fDatabasePath, __fDatabaseName)));
                    vConnection.Open();
                    vConnection.Close();
                }
                catch (Exception vException)
                {
                    mLogFailureLocally("__mDatabaseCreate", Path.Combine(__fDatabasePath, __fDatabaseName), vException);
                    vReturn = false;
                }
            }

            return vReturn;
        }
        public override ArrayList __mTablesList()
        {
            ArrayList vReturn = new ArrayList();
            DataTable vDataTable = __mSqlQuery("SELECT name FROM sqlite_master WHERE type='table'");

            foreach (DataRow vDataRow in vDataTable.Rows)
                vReturn.Add(vDataRow["name"].ToString());

            return vReturn;
        }
        public override int __mClueLastInserted(string pTableName)
        {
            DataTable vDataTable = __mSqlQuery("Select MAX(CLU) FROM " + pTableName);

            if (vDataTable.Rows.Count > 0 && vDataTable.Rows[0][0] != DBNull.Value)
                return Convert.ToInt32(vDataTable.Rows[0][0]);

            return -1;
        }

        #endregion База данных

        #region Соединение

        protected override bool __mConnectionOn()
        {
            bool vReturn = true;

            try
            {
                if (fConnection == null)
                    fConnection = new SQLiteConnection(string.Format("Data Source={0}; Version=3;", Path.Combine(__fDatabasePath, __fDatabaseName)));

                if (fConnection.State != ConnectionState.Open)
                    fConnection.Open();
            }
            catch (Exception vException)
            {
                mLogFailureLocally("__mConnectionOn", Path.Combine(__fDatabasePath, __fDatabaseName), vException);
                vReturn = false;
            }

            return vReturn;
        }
        protected override bool __mConnectionOff()
        {
            bool vReturn = true;

            try
            {
                if (fConnection != null)
                {
                    fConnection.Close();
                    fConnection.Dispose();
                    fConnection = null;
                }
            }
            catch (Exception vException)
            {
                mLogFailureLocally("__mConnectionOff", "", vException);
                vReturn = false;
            }

            return vReturn;
        }

        #endregion Соединение

        #region * Локальное журналирование сбоев

        /// <summary>
        /// Запись сведений о сбое SQL-операции напрямую в локальный текстовый файл, минуя 'appApplication.__oErrorsHandler'
        /// (см. примечание к классу - это единственный безопасный способ не уйти в рекурсию)
        /// </summary>
        /// <param name="pProcedure">Название процедуры, в которой произошёл сбой</param>
        /// <param name="pCommand">SQL-команда/запрос, вызвавшие сбой</param>
        /// <param name="pException">Возникшее исключение</param>
        private void mLogFailureLocally(string pProcedure, string pCommand, Exception pException)
        {
            try
            {
                string vLogPath = Path.Combine(
                    string.IsNullOrEmpty(__fDatabasePath) ? Path.GetTempPath() : __fDatabasePath,
                    "protocols_db_errors.log");

                string vLine = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    + " | " + pProcedure
                    + " | " + pCommand
                    + " | " + pException.Message
                    + Environment.NewLine;

                File.AppendAllText(vLogPath, vLine);
            }
            catch
            {
                /// Даже запись в локальный файл не должна иметь права уронить приложение или уйти в общий обработчик ошибок
            }
        }

        #endregion Локальное журналирование сбоев

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Служебные

        private SQLiteConnection fConnection = null;
        private SQLiteTransaction fTransaction = null;

        #endregion Служебные

        #endregion ПОЛЯ
    }
}