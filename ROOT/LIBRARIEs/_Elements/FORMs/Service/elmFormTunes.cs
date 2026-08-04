using System.Drawing;
using System.Windows.Forms;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmFormTunes.cs
    /// </summary>
    /// <remarks>Класс-форма для изменения настроек приложения</remarks>
    public class elmFormTunes : elmForm
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Размещение компонентов

            Controls.Add(__cAreaTunes);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            __mCaptionBuilding("Настройки приложения");

            #endregion Настройка компонентов

            ResumeLayout();

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Область для изменения настроек приложения
        /// </summary>
        public elmAreaTunes __cAreaTunes = new elmAreaTunes();

        #endregion Компоненты

        #endregion ПОЛЯ
    }
}
