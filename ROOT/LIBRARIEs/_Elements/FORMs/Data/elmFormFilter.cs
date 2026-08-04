using System.Windows.Forms;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmFormFilter.cs
    /// </summary>
    /// <remarks>Класс-форма для построения фильтра</remarks>
    public class elmFormFilter : elmForm
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

            Controls.Add(__cAreaFilter);
            Controls.SetChildIndex(__cAreaFilter, 0);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            __fCaption_ = "Фильтр брендов товаров";
            _fHelpTopic = "";

            // _cAreaFilter
            {
                __cAreaFilter.Dock = DockStyle.Fill;
            }

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }

        #endregion Объект

        #region - Поведение

        /// <summary>
        /// Выполняется при нажатии на клавиши
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                __cAreaFilter.__mPressButtonHelp();
            if (e.Control == true & e.KeyCode == Keys.A)
                __cAreaFilter.__mPressButtonApply();

            base.OnKeyDown(e);

            return;
        }

        #endregion Поведение

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Область для построения фильтра
        /// </summary>
        public elmAreaFilter __cAreaFilter = new elmAreaFilter();

        #endregion Компоненты

        #endregion ПОЛЯ
    }
}
