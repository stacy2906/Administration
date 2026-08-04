using System;

namespace nlApplication
{
    /// <summary>
    /// Файл appUnitItem.cs
    /// </summary>
	/// <remarks>Класс-единица 'Объект'</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 13-56</version> // Дата-время последней корректировки    
    public class appUnitItem
    {
        #region = СВОЙСТВА

        /// <summary>
        /// Включатель
        /// </summary>
        public bool __fCheck_ { get; set; }
        /// <summary>
        /// Ключ записи
        /// </summary>
        public int __fClue_ { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        public string __fDesignation_ { get; set; }
        /// <summary>
        /// Тип данных значения
        /// </summary>
        public Type __fType_ { get; set; }
        /// <summary>
        /// Значение
        /// </summary>
        public object __fValue_ { get; set; }

        #endregion СВОЙСТВА    
    }
}
