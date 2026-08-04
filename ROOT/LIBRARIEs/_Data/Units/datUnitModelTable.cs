using System.Collections.Generic;
using System;

namespace nlData
{
    /// <summary>
    /// Файл datUnitModelTable.cs
    /// </summary>
    /// <remarks>Класс-единица 'Модель таблицы'</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.01.13 14-19</version> // Дата-время последней корректировки
    public class datUnitModelTable
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>Конструктор
        /// </summary>
        public datUnitModelTable()
        {
            _mObjectAssembly();
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        {
            Type vType = this.GetType();
            _fClassNameFull = vType.Namespace + "." + vType.Name + ".";

            return;
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Экспорт изменений данных
        /// </summary>
        /// <param name="pDateTimeChanges">Время для выбора отклонений</param>
        /// <returns>[true] - экспортные данные обработаны, иначе - [false]</returns>
        public virtual bool __mChangesExport(DateTime pDateTimeChanges)
        {
            bool vReturn = true;

            return vReturn;
        }
        /// <summary>
        /// Заполнение справочников исходными данными
        /// </summary>
        /// <returns>[true] - данные записаны, иначе - false</returns>
        public virtual bool __mModelInitialFill()
        {
            return false;
        }
        /// <summary>
        /// Построение модели таблицы в источнике данных
        /// </summary>
        public virtual void __mModelTableBuilding()
        {
            return;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Поля содержащиеся в таблице
        /// </summary>
        public List<datUnitModelField> __fFieldS = new List<datUnitModelField>();
        /// <summary>
        /// Псевдоним источника данных
        /// </summary>
        public string __fDataSourceAlias = "";
        /// <summary>
        /// Описание таблицы
        /// </summary>
        public string __fDescription = "";
        /// <summary>
        /// Название таблицы
        /// </summary>
        public string __fName = "";

        #endregion Атрибуты

        #region - Внутренние

        /// <summary>
        /// Полное название класса 
        /// </summary>
        protected string _fClassNameFull = "";

        #endregion Внутренние

        #region - Объекты

        /// <summary>
        /// Полное название класса сущности данных
        /// </summary>
        public datUnitEssence __oEssence;

        #endregion Объекты

        #endregion ПОЛЯ
    }
}
