using nlData;
using System;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmFormTree.cs
    /// </summary>
    /// <remarks>Класс-форма для правки древовидных данных</remarks>
    public class elmFormTree : elmForm
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region Размещение компонентов

            Controls.Add(__cAreaTree);
            Controls.SetChildIndex(__cAreaTree, 0);

            #endregion Размещение компонентов

            #region Настройка компонентов

            Text = "Базовая форма для правки древовидных данных";

            // _cAreaTree
            {
                __cAreaTree.__fButtonEditCopyVisible_ = false;
                __cAreaTree.Dock = DockStyle.Fill;
                __cAreaTree.__eButtonSelect_Click += mAreaTree_ButtonSelectClick;
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

            __cAreaTree.__mDataLoad();

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ 

        #region - Поведение

        /// <summary>
        /// Выполняется при отпускании клавиши пользователем
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                __cAreaTree.__mPressButtonHelp();
            if (e.KeyCode == Keys.F5)
                __cAreaTree.__mPressButtonRefresh();
            if (e.Control == true & e.KeyCode == Keys.A)
                __cAreaTree.__mPressButtonSelect();
            if (e.Control == true & e.KeyCode == Keys.E)
                __cAreaTree.__mPressButtonEdit();
            if (e.Control == true & e.KeyCode == Keys.O)
                __cAreaTree.__mPressButtonOperations();
            if (e.Control == true & e.KeyCode == Keys.R)
                __cAreaTree.__mPressButtonReports();

            base.OnKeyDown(e);
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Выбрать'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mAreaTree_ButtonSelectClick(object sender, EventArgs e)
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
        public elmAreaTree __cAreaTree = new elmAreaTree();

        #endregion - Компоненты

        #region - Объекты

        /// <summary>
        /// Сущность прав
        /// </summary>
        public datUnitEssence __oEssenceRights;
        /// <summary>
        /// Сущность ролей пользователей
        /// </summary>
        public datUnitEssence __oEssenceUsersRoles;

        #endregion Объекты

        #endregion ПОЛЯ
    }
}
