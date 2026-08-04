using nlData;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System;
using System.Data;
using nlApplication;

namespace nlElements
{
    /// <summary>
    /// Файл elmFormLogin.cs
    /// </summary>
    /// <remarks>Класс-форма для регистрации пользователей</remarks>
    public class elmFormLogin : elmForm
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

            Controls.Add(_cSplitter);
            Controls.Add(_cToolBar);
            Controls.SetChildIndex(__cPanelStatus, 0);
            _cSplitter.Panel1.Controls.Add(_cLogoType);
            _cSplitter.Panel2.Controls.Add(_cInputUserCode);
            _cSplitter.Panel2.Controls.Add(_cInputUserPassword);

            _cToolBar.Items.Add(_cButtonApply);
            _cToolBar.Items.Add(_cButtonHelp);

            #endregion Размещение компонентов

            #region Настройка компонентов

            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = true;
            Height = 270;
            SizeGripStyle = SizeGripStyle.Hide;
            Width = 350; /// Назначение ширины формы
            __fCaption_ = "Регистрация пользователя";
            _cToolBar.TabStop = false;

            // _cButtonApply
            {
                _cButtonApply.Click += _cButtonApply_Click;
                _cButtonApply.Image = global::nlResourcesImages.Properties.Resources._Sign_Tick_g32;
                _cButtonApply.ToolTipText = "[ Ctrl + A ] " + elmApplication.__oTunes.__mTranslate("Применить");
            }
            // _cButtonHelp
            {
                _cButtonHelp.Click += _cButtonHelp_Click;
                _cButtonHelp.Image = global::nlResourcesImages.Properties.Resources._Sign_Question_b32;
                _cButtonHelp.ToolTipText = "[ F1 ] " + elmApplication.__oTunes.__mTranslate("Помощь");
            }
            // _cSplitter
            {
                _cSplitter.Dock = DockStyle.Fill;
                _cSplitter.Orientation = Orientation.Horizontal;
                _cSplitter.SplitterDistance = 120;
                _cSplitter.IsSplitterFixed = true;
                _cSplitter.FixedPanel = FixedPanel.Panel1;
            }
            // _cLogoType
            {
                _cLogoType.SizeMode = PictureBoxSizeMode.CenterImage;
                _cLogoType.Top = elmInterface.__fIntervalVertical;
                _cLogoType.Left = elmInterface.__fIntervalHorizontal;
                _cLogoType.Dock = DockStyle.Fill;

                if (File.Exists(Path.Combine(elmApplication.__oPathes.__fDirectoryStart, "Logo.png")) == true)
                {
                    _cLogoType.BackColor = Color.Transparent;
                    _cLogoType.Image = Image.FromFile(elmApplication.__oPathes.__fDirectoryStart + "\\Logo.png");
                }
                else
                    _cLogoType.Image = global::nlResourcesImages.Properties.Resources.Logotype;

            }
            // _cInputUserCode
            {
                _cInputUserCode.Location = new Point(elmInterface.__fIntervalHorizontal, elmInterface.__fIntervalVertical + 10);
                _cInputUserCode.Width = Width - elmInterface.__fFormBorderWidth * 2 - elmInterface.__fIntervalHorizontal * 2;
                _cInputUserCode.__fCaption_ = "Код пользователя";
                _cInputUserCode.__fMarkVisible_ = false;
                _cInputUserCode.__fFillType_ = FILLTYPES.Necessarily;
                _cInputUserCode.__fValueMaximum_ = 999;
                _cInputUserCode.__fPartInt_ = 3;
                //_cInputUserCode.__f
            }
            // _cInputUserPassword
            {
                _cInputUserPassword.Location = new Point(elmInterface.__fIntervalHorizontal, elmInterface.__fIntervalVertical + 40);
                _cInputUserPassword.Width = Width - elmInterface.__fFormBorderWidth * 2 - elmInterface.__fIntervalHorizontal * 2;
                _cInputUserPassword.__fCaption_ = "Пароль";
                _cInputUserPassword.__fMarkVisible_ = false;
                _cInputUserPassword.__fPasswordChar_ = '*';
                _cInputUserPassword.__fSymbolsCount_ = 10;
                _cInputUserPassword.__fFillType_ = FILLTYPES.Necessarily;
            }

            _mTunesLoad();
            _cInputUserCode.Focus();

            #endregion Настойка компонентов

            ResumeLayout();

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();
            _cSplitter.SplitterDistance = 80;
            string vUserCode = __oFileIni.__mValueRead(_fClassFilePath_, "UserCod");
            string vUserPassword = __oFileIni.__mValueRead(_fClassFilePath_, "UserPassword");
            if (appTypeString.__mWordIsInt(vUserCode) == true)
                _cInputUserCode.__fValue_ = vUserCode;
            if (vUserPassword.Trim().Length > 0)
                _cInputUserPassword.__fValue_ = vUserPassword;
            _cInputUserCode.__mInputFocus();
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется при отпускании горячих клавиш
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1) // F1
                _cButtonHelp.PerformClick();
            if (e.Control == true & e.KeyCode == Keys.A) // Ctrl+A
                _cButtonApply.PerformClick();

            base.OnKeyUp(e);
        }

        #region Кнопки управления

        /// <summary>
        /// Выполняется при выборе кнопки 'Применить'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cButtonApply_Click(object sender, EventArgs e)
        {
            __mUserInfoLoad();
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Помощь'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cButtonHelp_Click(object sender, EventArgs e)
        {
            __mHelp();
        }

        #endregion Кнопки управления

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Загрузка информации о пользователе
        /// </summary>
        public virtual void __mUserInfoLoad()
        {
            int vFound = elmApplication.__oData.__mTableRowsCountWhere("Usr", "codUsr=" + _cInputUserCode.__fValue_.ToString().Trim(), __fDataSourceAlias); // Количество найденных значений
            /// Если количество пользователей с указанными кодом и паролем равно 0, выполняется регистрация пользователя и форма закрывается
            if (vFound > 0)
            {
                DataTable vDataTable = elmApplication.__oData.__mSqlQuery("Select * From Usr as U"
                    + " Where codUsr = " + _cInputUserCode.__fValue_.ToString().Trim()
                    , __fDataSourceAlias);

                string vPasswordCode = appTypeString.__mWordToHash(_cInputUserPassword.__fValue_.ToString().Trim()).ToString();
                if (vDataTable.Rows[0]["PswCod"].ToString().Trim() == vPasswordCode)
                {
                    datUnitDataSource vDataSource = elmApplication.__oData.__mDataSourceGet(__fDataSourceAlias);
                    vDataSource.__fUserAdministrator = Convert.ToBoolean(vDataTable.Rows[0]["mrkAdm"]);
                    vDataSource.__fUserDesign = Convert.ToBoolean(vDataTable.Rows[0]["mrkDgn"]);
                    vDataSource.__fUserAlias = Convert.ToString(vDataTable.Rows[0]["dsiUsr"]).Trim();
                    vDataSource.__fUserClue = Convert.ToInt32(vDataTable.Rows[0]["CLU"]);
                    vDataSource.__fUserCode = Convert.ToInt32(vDataTable.Rows[0]["codUsr"]);

                    __fRegistered = true;

                    //DataTable vDataTableRoles = elmApplication.__oData.__mSqlQuery("Select UR.* From UsrUsrRol as UUR Left Join UsrRol as UR On UR.CLU = UUR.lnkUsrRol"
                    //    + " Where UUR.lnkUsr = " + vDataSource.__fUserClue.ToString().Trim()
                    //    , __fDataSourceAlias
                    //    );
                    //foreach (DataRow vDataRow in vDataTableRoles.Rows)
                    //{
                    //    vDataSource.__fUserRoleName = Convert.ToString(vDataRow["dsiUsrRol"]);
                    //    vDataSource.__fUserRoleClue = Convert.ToInt32(vDataRow["CLU"]);
                    //}
                    //DataTable vDataTableRoles = elmApplication.__oData.__mSqlQuery("Select UR.* From UsrRol as UR "
                    //    + " Where UR.CLU = " + vUserRoleClue.ToString().Trim()
                    //    , __fDataSourceAlias
                    //    );
                    //foreach (DataRow vDataRow in vDataTableRoles.Rows)
                    //{ 
                    //    vDataSource.__fUserRoleName = Convert.ToString(vDataRow["dsiUsrRol"]);
                    //    //vDataSource.__fUserRoleClue = vUserRoleClue;
                    //}
                }
            }
            /// Выполняется приращение попыток проверки кода и пароля
            _fAttemptsAmount++;
            /// Так как форма не закрылась - введены не верные данные. Выводиться количество попыток ввода в строку статуса формы.
            if (_fAttemptsAmount == 1)
                __cPanelStatus.__mCaptionBuilding("Выполнена 1 попытка");
            else
                __cPanelStatus.__mCaptionBuilding("Выполнено {0} попытки", _fAttemptsAmount);
            /// Если количество попыток ввода завершившихся неудачей превышает 3, закрываем форму.
            if (_fAttemptsAmount >= 3)
            {
                Close();
            }
            if (__fRegistered == true)
                Close();
        }

        #endregion Процедуры

        #endregion = МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Статус регистрирующегося пользователя
        /// </summary>
        public bool __fRegistered = false;
        /// <summary>
        /// Псевдоним источника данных
        /// </summary>
        public string __fDataSourceAlias = "";

        #endregion Атрибуты

        #region - Внутренние

        /// <summary>
        /// Количество выполненных попыток
        /// </summary>
        protected int _fAttemptsAmount = 0;

        #endregion Внутренние

        #region - Компоненты

        /// <summary>
        /// Панель инструментов
        /// </summary>
        protected elmComponentToolbar _cToolBar = new elmComponentToolbar();
        /// <summary>
        /// Кнопка 'Применить'
        /// </summary>
        protected elmComponentToolbarButton _cButtonApply = new elmComponentToolbarButton();
        /// <summary>
        /// Кнопка 'Помощь'
        /// </summary>
        protected elmComponentToolbarButton _cButtonHelp = new elmComponentToolbarButton();
        /// <summary>
        /// Разделитель
        /// </summary>
        protected elmComponentSplitter _cSplitter = new elmComponentSplitter();
        /// <summary>
        /// Логотип пользователя программы
        /// </summary>
        protected elmComponentPicture _cLogoType = new elmComponentPicture();
        /// <summary>
        /// Поле ввода кода пользователя
        /// </summary>
        protected elmInputInteger _cInputUserCode = new elmInputInteger();
        /// <summary>
        /// Поле ввода пароля пользователя
        /// </summary>
        protected elmInputString _cInputUserPassword = new elmInputString();

        #endregion Компоненты

        #endregion ПОЛЯ
    }
}
