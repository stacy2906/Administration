//using Microsoft.Office.Interop.Excel;
using nlData;
using System;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmFormGrid.cs
    /// </summary>
    /// <remarks>Класс-форма для правки табличных данных</remarks>
    public class elmFormGrid : elmForm
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

            Controls.Add(__cAreaGrid);
            Controls.SetChildIndex(__cAreaGrid, 0);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            Text = "Базовая форма для правки табличных данных";

            // _cAreaGrid
            {
                __cAreaGrid.Dock = DockStyle.Fill;
                __cAreaGrid.__eButtonSelectClick += mAreaGrid_ButtonSelect_Click;
                __cAreaGrid.__eRowDoubleClick += mAreaGrid_RowDoubleClick;
            }

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();

            __cAreaGrid.__mDataLoad();
            __cAreaGrid.__mGridFocus();

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region Кнопки панели управления
        #endregion Кнопки панели управления

        #region - Поведение

        /// <summary>
        /// Выполняется при отпускании клавиши пользователем
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                __cAreaGrid.__mPressButtonHelp();
            if (e.KeyCode == Keys.F5)
                __cAreaGrid.__mPressButtonRefresh();
            if (e.KeyCode == Keys.F12)
                __cAreaGrid.__mPressButtonColumns();
            if (e.Control == true & e.KeyCode == Keys.A)
                __cAreaGrid.__mPressButtonSelect();
            if (e.Control == true & e.KeyCode == Keys.E)
                __cAreaGrid.__mPressButtonEdit();
            if (e.Control == true & e.KeyCode == Keys.O)
                __cAreaGrid.__mPressButtonOperations();
            if (e.Control == true & e.KeyCode == Keys.R)
                __cAreaGrid.__mPressButtonReports();

            base.OnKeyDown(e);
        }
        /// <summary>
        /// Выполняется при двойном клике по строке сетки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mAreaGrid_RowDoubleClick(object sender, EventArgs e)
        {
            /// Форма закрывается
            Close();
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Выбрать'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mAreaGrid_ButtonSelect_Click(object sender, EventArgs e)
        {
            (FindForm() as elmForm).Close();
        }

        #endregion Поведение

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Область для правки табличных данных
        /// </summary>
        public elmAreaGrid __cAreaGrid = new elmAreaGrid();

        #endregion Компоненты

        #region - Объекты


        public datUnitEssence __oEssenceRights = new datUnitEssence(); /// ??? __oEssenceUsersRights
        public datUnitEssence __oEssenceUsersRoles = new datUnitEssence(); /// ??? __oEssenceUsersRolesRights

        #endregion Объекты

        #endregion ПОЛЯ
    }
}
