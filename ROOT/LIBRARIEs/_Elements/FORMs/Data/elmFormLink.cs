using nlData;
using System;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmFormLink.cs
    /// </summary>
    /// <remarks>Класс-форма для работы со связующими данными</remarks>
    /// <example>
    //__cAreaLink.Dock = DockStyle.Fill;
    //__cAreaLink.__fLinkedDesignationFieldName = "dsiSnd";
    //__cAreaLink.__oEssence_ = new cbnEssenceFstSnd();
    //__cAreaLink.__oFormLinkedData = typeof(cbnFormGridSnd);
    //__cAreaLink.__fFormOpenedType = CONTROLsOPENEDTYPES.FormGrid;
    //__cAreaLink.__fLinkedTable = "Snd";
    //__cAreaLink.__fParentKeyFieldName = "lnkFst";
    //__cAreaLink.__fLinkedKeyFieldName = "lnkSnd";
    /// </example>
    public class elmFormLink : elmForm
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

            Controls.Add(__cAreaLink);
            Controls.SetChildIndex(__cAreaLink, 0);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            Text = "Базовая форма для правки табличных данных";

            // __cAreaLink
            {
                __cAreaLink.Dock = DockStyle.Fill;
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

            __cAreaLink.__mDataLoad();
            __cAreaLink.__mGridFocus();

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется при нажатии на клавиши
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                __cAreaLink.__mPressButtonHelp();
            if (e.Control == true & e.KeyCode == Keys.E)
                __cAreaLink.__mPressButtonEdit();

            base.OnKeyDown(e);
        }

        #endregion Поведение

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Область для работы со связующими данными
        /// </summary>
        public elmAreaLink __cAreaLink = new elmAreaLink();

        #endregion Компоненты

        #region - Константы

        #region Права пользователей

        protected string _nButtonEditCreate = elmApplication.__oTunes.__mTranslate("Кнопка `Правка` меню `Создать`");
        protected string _nButtonEditRemove = elmApplication.__oTunes.__mTranslate("Кнопка `Правка` меню `Исключить`");

        #endregion Права пользователей

        #endregion Константы

        #region - Объекты

        /// <summary>
        /// Сущность прав
        /// </summary>
        public datUnitEssence __oEssenceRights;
        /// <summary>
        /// Тип формы для изменения данных
        /// </summary>
        public Type __oFormUserAccess;
        /// <summary>
        /// Ключ главного поля к которому добавляются значения
        /// </summary>
        public int __fRecordParentKey = 0;

        #endregion Объекты

        #endregion ПОЛЯ
    }
}
