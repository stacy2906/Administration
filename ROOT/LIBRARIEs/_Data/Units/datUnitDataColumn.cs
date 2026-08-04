namespace nlData
{
    /// <summary>
    /// Файл datUnitDataColumn.cs
    /// </summary>
    /// <remarks>Класс-единица 'DataColumn'</remarks>
  	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 14-15</version> // Дата-время последней корректировки
    public class datUnitDataColumn
    {
        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Заголовок компонента 
        /// </summary>
        public string __fControlCaption = "";
        /// <summary>
        /// Размер поля ввода
        /// </summary>
        public string __fControlSize = "";
        /// <summary>
        /// Название контрола используемого для изменения данных
        /// </summary>
        public string __fControlEdit = "";
        /// <summary>
        /// Название контрола используемого для построения фильтра
        /// </summary>
        public string __fControlFilter = "";
        /// <summary>
        /// Описание поля
        /// </summary>
        public string __fFieldDescription = "";
        /// <summary>
        /// Название
        /// </summary>
        public string __fFieldName = "";
        /// <summary>
        /// Размер поля в таблице
        /// </summary>
        public decimal __fFieldSize = 0;
        /// <summary>
        /// Тип данных
        /// </summary>
        public string __fFieldType = "";

        #endregion Атрибуты

        #endregion ПОЛЯ
    }
}
