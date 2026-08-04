namespace nlApplication
{
    /// <summary>
    /// Файл appInterfaceAbout.cs
    /// </summary>
    /// <remarks>Интерфейс-Атрибуты приложения</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.29 18-11</version> // Дата-время последней корректировки
    public interface appInterfaceAbout 
    {
        #region = СВОЙСТВА

        /// <summary>
        /// Название и расширение файла помощи приложения
        /// </summary>
        string __fHelpFileName_ { get; }
        /// <summary>
        /// Пакет приложений которому принадлежит приложение
        /// </summary>
        string __fPacket_ { get; }
        /// <summary>
        /// Производственная версия продукта
        /// </summary>
        string __fProductionVersion_ { get; }
        /// <summary>
        /// Префикс файлов
        /// </summary>
        string __fPrefix_ { get; }

        #endregion СВОЙСТВА
    }
}
