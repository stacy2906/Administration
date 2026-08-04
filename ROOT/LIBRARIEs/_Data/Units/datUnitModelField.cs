namespace nlData
{
    /// <summary>
    /// Файл datUnitModelField.cs
    /// </summary>
    /// <remarks>Класс-единица 'Модель поля таблицы'</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.01.13 14-18</version> // Дата-время последней корректировки
    public class datUnitModelField
    {
        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Автоматическое приращение
        /// </summary>
        public bool __fAutoIncrement = false;
        /// <summary>
        /// Заголовок компонента
        /// </summary>
        public string __fCaption = "";
        /// <summary>
        /// Тип хранимых данных
        /// </summary>
        public COLUMNSTYPES __fDataType = COLUMNSTYPES.Text;
        /// <summary>
        /// Значение по умолчанию
        /// </summary>
        public object __fDefaultValue = null;
        /// <summary>
        /// Описание поля
        /// </summary>
        public string __fDescription = "";
        /// <summary>
        /// Поле является ключем
        /// </summary>
        public bool __fIsClue = false;
        /// <summary>
        /// Поле может содержать нулевые значения
        /// </summary>
        public bool __fIsNull = false;
        /// <summary>
        /// Название поля
        /// </summary>
        public string __fName = "";
        /// <summary>
        /// Размер данных
        /// </summary>
        public int __fSize = 10;
        /// <summary>
        /// Размер десятичной части для десятичных данных
        /// </summary>
        public int __fSizeDecimal = -1;

        #endregion Атрибуты

        #endregion ПОЛЯ
    }
}
