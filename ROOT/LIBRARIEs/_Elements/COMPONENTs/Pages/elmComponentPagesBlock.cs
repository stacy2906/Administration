using System;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentPageBlock.cs
    /// </summary>
    /// <remarks>Класс-Компонент блок вкладок</remarks>
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 00-00</version> // Дата-время последней корректировки
    public class elmComponentPagesBlock : TabControl
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public elmComponentPagesBlock()
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
            SuspendLayout();

            #region /// Настройка компонента

            #endregion Настройка компонента

            ResumeLayout();

            return;
        }

        #endregion Поведение

        #endregion МЕТОДЫ   

    }
}
