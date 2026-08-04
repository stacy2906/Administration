using System;

namespace nlSystem
{
    /// <summary>
    /// Файл appUnitFile.cs
    /// </summary>
    /// <remarks>Класс-единица основных свойств файла</remarks>
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 14-47</version> // Дата-время последней корректировки
    public class sstUnitFile
    {
        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Название файла
        /// </summary>
        public string __fName = "";
        /// <summary>
        /// Папка размещения файла
        /// </summary>
        public string __fDirectory = "";
        /// <summary>
        /// Время создания файла
        /// </summary>
        public DateTime __fDateTimeCreate = DateTime.Now;
        /// <summary>
        /// Время последней записи в файл
        /// </summary>
        public DateTime __fDateTimeWrite = DateTime.Now;
        /// <summary>
        /// Размер файла
        /// </summary>
        public long __fSize = -1;
        /// <summary>
        /// Размер файла
        /// </summary>
        public string __fVersion = "1.0.0.0";

        #endregion Атрибуты

        #endregion ПОЛЯ
    }
}
