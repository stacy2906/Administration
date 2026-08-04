using System;
using System.Windows.Forms;

namespace nlElements
{
    public class elmFormQuote : elmForm
    {
        #region = МЕТОДЫ

        #region - Объект

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Размещение компонентов

            Controls.Add(__cAreaQuote);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            // _cAreaNotice
            {
                __cAreaQuote.Dock = DockStyle.Fill;
            }

            #endregion Настройка компонентов

            ResumeLayout();

            return;
        }

        #endregion Объект

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Область для правки примечаний
        /// </summary>
        public elmAreaQuote __cAreaQuote = new elmAreaQuote();

        #endregion Компоненты

        #endregion ПОЛЯ
    }
}
