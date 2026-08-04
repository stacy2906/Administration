using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmUnitGridColumn.cs
    /// </summary>
    /// <remarks>Класс дополнительных свойств 'DataGridColumn'</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 10-07</version> // Дата-время последней корректировки
    public class elmUnitGridColumn
    {
        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Заголовок колонки
        /// </summary>
        public string __fCaption = "";
        /// <summary>
        /// Доступность колонки для редактирования
        /// </summary>
        public bool __fReadOnly = true;
        /// <summary>
        /// Название поля
        /// </summary>
        public string __fField = "";
        /// <summary>
        /// Видимость колонки в 'DataGridView'
        /// </summary>
        public bool __fVisible = true;
        /// <summary>
        /// Всплывающая подсказка при наведении курсора мыши
        /// </summary>
        public string __fToolTipText = "";
        /// <summary>
        /// Объект для отображения данных в колонке
        /// </summary>
        public DATAGRIDCOLUMNTYPE __fType = DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn;
        /// <summary>
        /// Стиль ячейки
        /// </summary>
        public DataGridViewCellStyle __fCellStyle = null;

        #endregion Атрибуты

        #endregion ПОЛЯ
    }
}
