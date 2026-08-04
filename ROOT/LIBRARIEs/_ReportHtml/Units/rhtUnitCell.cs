namespace nlReportHtml
{
    /// <summary>
    /// Файл rhtUnitCell.cs
    /// </summary>
    /// <remarks>Класс-единица 'Ячейка отчета'</remarks>
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 14-37</version> // Дата-время последней корректировки
    public class rhtUnitCell
    {
        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Краткое описание содержимого
        /// </summary>
        internal string __fAbbreviation = "";
        /// <summary>
        /// Горизонтальная привязка текста
        /// </summary>
        /// <remarks>Допустимые значения
        /// left, center, right, justify
        /// </remarks>
        internal string __fAlign = "";
        /// <summary>
        /// Отображаемые линии рамки
        /// </summary>
        /// <remarks>
        /// l - левая линия рамки
        /// r - правая линия рамки
        /// t - верхняя линия рамки
        /// b - нижняя линия рамки
        /// </remarks>
        internal string __fBorder = "";
        /// <summary>
        /// Цвет линий рамки
        /// </summary>
        internal string __fBorderColor = "";
        /// <summary>
        /// Толщина линий рамки
        /// </summary>
        internal string __fBorderSize = "1";
        /// <summary>
        /// Используемые классы
        /// </summary>
        internal string __fClasses = "";
        /// <summary>
        /// Цвет фона
        /// </summary>
        internal string __fColorBack = "";
        /// <summary>
        /// Цвет текста
        /// </summary>
        internal string __fColorText = "";
        /// <summary>
        /// Толстый шрифт
        /// </summary>
        internal bool __fFontBold = false;
        /// <summary>
        /// Наклонный шрифт
        /// </summary>
        internal bool __fFontItalic = false;
        /// <summary>
        /// Подчеркнутый шрифт
        /// </summary>
        internal bool __fFontUnderline = false;
        /// <summary>
        /// Название шрифта
        /// </summary>
        internal string __fFontName = "";
        /// <summary>
        /// Размер шрифта
        /// </summary>
        internal string __fFontSize = "";
        /// <summary>
        /// Высота ячейки
        /// </summary>
        internal string __fHeight = "";
        /// <summary>
        /// Путь и имя файла изображения
        /// </summary>
        internal string __fImage = "";
        /// <summary>
        /// Отобразить линию в строке таблицы
        /// </summary>
        internal bool __fLine = false;
        /// <summary>
        /// Путь и имя ссылки на файл
        /// </summary>
        internal string __fLinkFile = "";
        /// <summary>
        /// Количество объеденяемых ячеек по горизонтали
        /// </summary>
        internal string __fSpanColumn = "";
        /// <summary>
        /// Количество объеденяемых ячеек по вертикали
        /// </summary>
        internal string __fSpanRow = "";
        /// <summary>
        /// Вертикальная привязка текста
        /// </summary>
        internal string __fValign = "";
        /// <summary>
        /// Значение ячейки
        /// </summary>
        internal object __fValue;
        /// <summary>
        /// Ширина
        /// </summary>
        internal string __fWidth = "";
        /// <summary>
        /// Разрешение отображения нулевого значения
        /// </summary>
        internal bool __fZero = false;

        #endregion Атрибуты

        #endregion ПОЛЯ
    }
}
