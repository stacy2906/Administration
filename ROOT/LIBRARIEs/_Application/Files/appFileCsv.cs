using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;

namespace nlApplication
{
    /// <summary>
    /// Файл appFileCsv.cs
    /// </summary>
    /// <remarks>Класс для работы с CSV файлами</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 13-58</version> // Дата-время последней корректировки    
    public sealed class appFileCsv
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Помещение данных из DataTable в CSV файл
        /// </summary>
        /// <param name="pFilePath">Путь к создаваемому файлу</param>
        /// <param name="pDataTable">DataTable с данными</param>
        public void __mDataTable2Csv(string pFilePath, DataTable pDataTable)
        {
            string vFileBody = ""; // Содержание создаваемого файла
            /// Формирование заголовка Csv файла
            foreach (DataColumn vDataColumn in pDataTable.Columns)
            {
                vFileBody += vDataColumn.ColumnName + ',';
            }
            vFileBody = vFileBody.Substring(0, vFileBody.ToString().Length - 1);
            vFileBody += CRLF;
            /// Формирование содержания CSV файла
            foreach (DataRow vDataRow in pDataTable.Rows)
            {
                for (int vCounter = 0; vCounter < pDataTable.Columns.Count; vCounter++)
                {
                    vFileBody += vDataRow[vCounter].ToString() + ",";
                }

                vFileBody = vFileBody.Substring(0, vFileBody.ToString().Length - 1);
                vFileBody += CRLF;
            }
            /// Если существует одноименный файл по указанному пути - он удаляется
            if (File.Exists(pFilePath) == true)
                File.Delete(pFilePath);
            /// Создается новый файл
            File.WriteAllText(pFilePath, vFileBody);
        }
        /// <summary>
        /// Помещение данных из CSV файла в DataTable
        /// </summary>
        /// <param name="pFilePath">Путь и имя CSV файла</param>
        public DataTable __mCsv2DataTable(string pFilePath)
        {
            string vDirectoryName = Path.GetDirectoryName(pFilePath); // Путь к читаемому файлу
            string vFileName = Path.GetFileName(pFilePath); // Имя читаемого файла
            /// Формирование запроса к провайдеру 'Microsoft.Jet.OLEDB.4.0'
            string sql = @"SELECT * FROM [" + vFileName + "]"; // Содержание запроса

            using (OleDbConnection oDbConnection = new OleDbConnection(
                      @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + vDirectoryName +
                      ";Extended Properties=\"Text;HDR=Yes" + "\""))
            /// Получение данных из запроса и запись их в таблицу
            using (OleDbCommand oDbCommand = new OleDbCommand(sql, oDbConnection))
            using (OleDbDataAdapter oDbDataAdapter = new OleDbDataAdapter(oDbCommand))
            {
                DataTable vDataTable = new DataTable();
                vDataTable.Locale = CultureInfo.CurrentCulture;
                oDbDataAdapter.Fill(vDataTable);
                return vDataTable;
            }
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Константы

        /// <summary>
        /// Перевод каретки
        /// </summary>
        private string CRLF = "\r\n";

        #endregion Константы

        #endregion ПОЛЯ
    }
}
