using System;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentToolbar.cs
    /// </summary>
    /// <remarks>Класс-Компонент панели управления</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 09-54</version> // Дата-время последней корректировки
    public class elmComponentToolbar : ToolStrip
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public elmComponentToolbar()
        {
            _mObjectAssembly();
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        {
            SuspendLayout();

            #region /// Настройка компонента

            BackColor = Color.Transparent;
            ImageScalingSize = new Size(32, 32);
            TabStop = false;

            #endregion Настройка компонента

            ResumeLayout(false);

            return;
        }

        #endregion ДИЗАЙНЕРЫ
    }
}
