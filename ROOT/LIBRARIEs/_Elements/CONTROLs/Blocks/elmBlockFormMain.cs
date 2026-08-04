using System.Drawing;
using System.Windows.Forms;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmBlockFormMain.cs
    /// </summary>
    /// <remarks>Класс-БлокГлавногоОкна</remarks>
    public class elmBlockFormMain : elmComponentPanel
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public elmBlockFormMain()
        {
            _mObjectAssembly();
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region Объект

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region Размещение компонентов

            Controls.Add(__cMenu);
            __cMenu.Items.Add(_cMenuApplication);
            _cMenuApplication.DropDownItems.Add(_cMenuApplicationUserChange);
            _cMenuApplication.DropDownItems.Add(_cMenuApplicationUserRights);
            _cMenuApplication.DropDownItems.Add(_cMenuApplicationTunes);
            _cMenuApplication.DropDownItems.Add(_cMenuApplicationHelp);
            _cMenuApplication.DropDownItems.Add(_cMenuApplicationAbout);

            #endregion Размещение компонентов

            #region Настройка компонентов

            Dock = DockStyle.Fill;

            #endregion Настройка компонентов

            ResumeLayout();

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected override void _mObjectPresentation()
        {
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            TabStop = false;

            // _cMenuApplication
            {
                _cMenuApplication.Alignment = ToolStripItemAlignment.Right;
                _cMenuApplication.Image = nlResourcesImages.Properties.Resources._Form_b16;
                _cMenuApplication.__fCaption_ = "Приложение";
            }
            // _cMenuApplicationUserChange
            {
                _cMenuApplicationUserChange.Click += mMenuApplicationUserChange_Click;
                _cMenuApplicationUserChange.Image = nlResourcesImages.Properties.Resources._Person_Police_k16;
                _cMenuApplicationUserChange.Name = "_cMenuApplicationUserChange";
                _cMenuApplicationUserChange.__fCaption_ = "Смена пользователя";
            }
            // _cMenuApplicationUserRights
            {
                _cMenuApplicationUserRights.Click += mMenuApplicationUserRights_Click;
                _cMenuApplicationUserRights.Image = nlResourcesImages.Properties.Resources._Person_Police_k16;
                _cMenuApplicationUserRights.Name = "_cMenuApplicationUserChange";
                _cMenuApplicationUserRights.__fCaption_ = "Определение прав пользователей";
            }
            // _cMenuApplicationTunes
            {
                _cMenuApplicationTunes.Click += mMenuApplicationTunes_Click;
                _cMenuApplicationTunes.Image = nlResourcesImages.Properties.Resources._Gear_b16;
                _cMenuApplicationTunes.Name = "_cMenuApplicationTunes";
                _cMenuApplicationTunes.__fCaption_ = "Настройки приложения";
            }
            // _cMenuApplicationHelp
            {
                _cMenuApplicationHelp.Click += mMenuApplicationHelp_Click;
                _cMenuApplicationHelp.Image = nlResourcesImages.Properties.Resources._Sign_RoundHelp_b16;
                _cMenuApplicationHelp.Name = "_cMenuApplicationHelp";
                _cMenuApplicationHelp.__fCaption_ = "Помощь";
            }
            // __cMenuApplicationAbout
            {
                _cMenuApplicationAbout.Click += mMenuApplicationAbout_Click;
                _cMenuApplicationAbout.Image = nlResourcesImages.Properties.Resources._Sign_RoundInformation_b16;
                _cMenuApplicationAbout.Name = "_cMenuApplicationAbout";
                _cMenuApplicationAbout.__fCaption_ = "О приложении";
            }

            ResumeLayout(false);
            PerformLayout();
        }

        #endregion Объект

        #region - Поведение

        #region Пункты меню

        /// <summary>
        /// Выполняется при выборе пункта меню 'Приложение/Смена пользователя' левой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMenuApplicationUserChange_Click(object sender, System.EventArgs e)
        {
            if (__eMenuApplicationUserChangeClick != null)
                __eMenuApplicationUserChangeClick(this, new EventArgs());
        }
        private void mMenuApplicationUserRights_Click(object sender, EventArgs e)
        {
            if (__eMenuApplicationUserRightsClick != null)
                __eMenuApplicationUserRightsClick(this, new EventArgs());
        }
        /// <summary>
        /// Выполняется при выборе пункта меню 'Приложение/Настройки приложения' левой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMenuApplicationTunes_Click(object sender, System.EventArgs e)
        {
            elmFormTunes vFormTunes = new elmFormTunes();
            vFormTunes.ShowDialog();
        }
        /// <summary>
        /// Выполняется при выборе пункта меню 'Приложение/Помощь' левой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMenuApplicationHelp_Click(object sender, System.EventArgs e)
        {
            (FindForm() as elmForm).__mHelp();
        }
        /// <summary>
        /// Выполняется при выборе пункта меню 'Приложение/О приложении' левой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMenuApplicationAbout_Click(object sender, System.EventArgs e)
        {
            elmFormAbout vFormAbout = new elmFormAbout();
            vFormAbout.ShowDialog();
        }

        #endregion Пункты меню

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Вставляет пункт меню в меню по указанному индексу
        /// </summary>
        /// <param name="pMenuItem"></param>
        /// <param name="pIndex"></param>
        public void __mMenuAdd(elmComponentMenuItem pMenuItem, int pIndex = 0)
        {
            __cMenu.Items.Insert(pIndex, pMenuItem);
        }
        /// <summary>
        /// Вставляет пункт меню в меню по указанному индексу
        /// </summary>
        /// <param name="pMenuItem"></param>
        /// <param name="pIndex"></param>
        public void __mMenuApplicationAdd(elmComponentMenuItem pMenuItem, int pIndex = 0)
        {
            _cMenuApplication.DropDownItems.Insert(pIndex, pMenuItem);
        }
        /// <summary>
        /// Вставляет пункт меню в меню по указанному индексу
        /// </summary>
        /// <param name="pMenuItem"></param>
        /// <param name="pIndex"></param>
        public void __mMenuApplicationAddSeparator(int pIndex = 0)
        {
            ToolStripSeparator pMenuItem = new ToolStripSeparator();
            _cMenuApplication.DropDownItems.Insert(pIndex, pMenuItem);
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Меню главного окна
        /// </summary>
        public elmComponentMenu __cMenu = new elmComponentMenu();
        /// <summary>
        /// Пункт меню 'Приложение'
        /// </summary>
        protected elmComponentMenuItem _cMenuApplication = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'Приложение / Смена пользователя'
        /// </summary>
        protected elmComponentMenuItem _cMenuApplicationUserChange = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'Приложение / Права пользователя'
        /// </summary>
        protected elmComponentMenuItem _cMenuApplicationUserRights = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'Приложение / Настойки приложения'
        /// </summary>
        protected elmComponentMenuItem _cMenuApplicationTunes = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'Приложение / Помощь'
        /// </summary>
        protected elmComponentMenuItem _cMenuApplicationHelp = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'Приложение / О приложении'
        /// </summary>
        protected elmComponentMenuItem _cMenuApplicationAbout = new elmComponentMenuItem();

        #endregion Компоненты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Видимость пункта меню 'Смена пользователя'
        /// </summary>
        public bool __fMenuApplicationUserChangeVisible_
        {
            get { return _cMenuApplicationUserChange.Visible; }
            set { _cMenuApplicationUserChange.Visible = value; }
        }
        /// <summary>
        /// Видимость пункта меню 'Определение прав пользователей'
        /// </summary>
        public bool __fMenuApplicationUserRightsVisible_
        {
            get { return _cMenuApplicationUserRights.Visible; }
            set { _cMenuApplicationUserRights.Visible = value; }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при выборе пункта меню 'Приложение/Смена пользователя' левой кнопкой мыши
        /// </summary>
        public event EventHandler __eMenuApplicationUserChangeClick;
        /// <summary>
        /// Возникает при выборе пункта меню 'Приложение/Права пользователя' левой кнопкой мыши
        /// </summary>
        public event EventHandler __eMenuApplicationUserRightsClick;

        #endregion СОБЫТИЯ
    }
}
