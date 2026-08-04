using nlApplication;
using nlData;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace nlDataSourceMsSql
{
    	/// <summary>
	/// Файл avpFormMain.cs
	/// </summary>
	/// <remarks>Класс-главная форма</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 15-03</version> // Дата-время последней корректировки
    public class dmsDataChange
    {
        #region = МЕТОДЫ

        /// <summary>
        /// Извлечение данных
        /// </summary>
        /// <param name="pFilesFolder">Путь и имя папки для размещения Csv файлов</param>
        /// <returns></returns>
        public bool __mExtarct(string pFilesFolder)
        {
            bool vReturn = true; // Возвращаемое значение
            /// Определение даты 
            string vTimeLastSend = appTypeDateTime.__mMsSqlDateToString(__mLastSendDateTimeGet());
            DataTable vDataTableTablesList = __mTablesList(); // Список таблиц в базе данных

            // Перебор таблиц в базе данных
            foreach (DataRow vDataRowTablesList in vDataTableTablesList.Rows)
            {
                string vTableName = vDataRowTablesList["Name"].ToString();
                // Если таблица создана для пересылки
                if (__mTableIsForward(vTableName) == true)
                {
                    //d DataTable vDataTableFieldsList = __mTableFieldsList(vTableName);
                    DataTable vDataTableFieldsChanges = datApplication.__oData.__mSqlQuery("Select * From " + vTableName + " Where CHG >= Convert(datetime, '" + vTimeLastSend + "')", __fDataBaseAlias);

                    appFileCsv oFileCsv = new appFileCsv();
                    oFileCsv.__mDataTable2Csv(Path.Combine(pFilesFolder, vTableName + ".csv"), vDataTableFieldsChanges);
                }
            }

            return vReturn;
        }
        public bool __mInsert(string pFolderPath)
        {
            bool vReturn = true;
            appFileCsv oFileChg = new appFileCsv(); // Объект для работы с Csv-файлами

            string vTableName = Path.GetFileNameWithoutExtension(pFolderPath);
            DataTable vDataTable = datApplication.__oData.__mSqlQuery("Select * From " + vTableName + " Where CLU < 0", __fDataBaseAlias);
            DataTable vDataTableChanges = oFileChg.__mCsv2DataTable(vTableName);
            vDataTable.Merge(vDataTableChanges);
            //vDataTable.Sa
            return vReturn;
        }
        /// <summary>
        /// Получение списка таблиц в базе данных
        /// </summary>
        public DataTable __mTablesList()
        {
            return datApplication.__oData.__mSqlQuery("SELECT Name FROM sys.objects WHERE type in (N'U') Order By name");
        }
        /// <summary>
        /// Получение списка полей в таблице
        /// </summary>
        /// <param name="pTableName"></param>
        public DataTable __mTableFieldsList(string pTableName)
        {
            return datApplication.__oData.__mSqlQuery("SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = '" + pTableName + "'");
        }
        /// <summary>
        /// Проверка необходимости пересылки таблицы
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        public bool __mTableIsForward(string pTableName)
        {
            bool vReturn = false; // Возвращаемое значение
            DataTable vDataTable = __mTableFieldsList(pTableName);

            foreach (DataRow vDataRow in vDataTable.Rows)
            {
                if (vDataRow["COLUMN_NAME"].ToString() == "GID")
                    vReturn = true;
            }

            return vReturn;
        }
        /// <summary>
        /// Получение времени последней отправки данных
        /// </summary>
        public DateTime __mLastSendDateTimeGet()
        {
            DataTable vDataTable = datApplication.__oData.__mSqlQuery("Select Max(dtmChgSndCrn) as dtmMax From ChgSnd", __fDataBaseAlias);
            DateTime vReturn;

            if (vDataTable.Rows.Count > 0 & vDataTable.Rows[0][0] != DBNull.Value)
                vReturn = Convert.ToDateTime(vDataTable.Rows[0][0]);
            else
                vReturn = new DateTime(2000, 1, 1);

            return vReturn;
        }

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        public string __fDataBaseAlias = "";

        #endregion Атрибуты

        #endregion ПОЛЯ
    }
}
