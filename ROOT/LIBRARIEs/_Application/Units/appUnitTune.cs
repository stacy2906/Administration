using System;

namespace nlApplication
{
    /// <summary>
    /// Файл appUnitTune.cs
    /// </summary>
    /// <remarks>Класс-единица 'Настройка приложения'</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.01.29 18-42</version> // Дата-время последней корректировки    
    public class appUnitTune 
    {
        #region = ПОЛЯ

        #region - Аргументы

        /// <summary>
        /// Описание настройки
        /// </summary>
        public string __fDescription = "";
        /// <summary>
        /// Доступность на форме настроек приложения
        /// </summary>
        public bool __fEdited = false;
        /// <summary>
        /// Список описаний допустимых значений
        /// </summary>
        public string __fListDescriptions = "";
        /// <summary>
        /// Разрешение загрузки настройки из файла (если она там определена, сохранение запрещено)
        /// </summary>
        public bool __fLoadFromFile = false;
        /// <summary>
        /// Название настройки
        /// </summary>
        public string __fName = "";
        /// <summary>
        /// Объект для отображения настройки
        /// </summary>
        public string __fObjectForEdit = null;
        /// <summary>
        /// Разрешение хранения настройки в файле
        /// </summary>
        public bool __fSaveInFile = false;
        /// <summary>
        /// Название секции настройки
        /// </summary>
        public string __fSection = "";
        /// <summary>
        /// Значение настройки
        /// </summary>
        public string __fValue = "";
        /// <summary>
        /// Тип данных значения настройки
        /// </summary>
        public Type __fValueDataType = null;
        /// <summary>
        /// Список допустимых значений
        /// </summary>
        public string __fValueList = "";

        #endregion Аргументы

        #endregion ПОЛЯ
    }
}
