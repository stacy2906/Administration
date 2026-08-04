using System;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentMenu.cs
    /// </summary>
    /// <remarks>Класс-Компонент меню формы</remarks>
    public class elmComponentMenu : MenuStrip
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public elmComponentMenu()
        {
            _mObjectAssembly();
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        {
            SuspendLayout();

            #region Настройка компонента

            BackColor = Color.Transparent;
            TabStop = false;

            #endregion Настройка компонента

            ResumeLayout();

            return;
        }

        #endregion ДИЗАЙНЕРЫ
    }
}
