using nlApplication;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmArea.cs
    /// </summary>
    /// <remarks>Класс-область</remarks>
    public class elmArea : elmComponentSplitter
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            #region /// Размещение компонентов

            Panel1.Controls.Add(_cHeaderPicture);
            Panel1.Controls.Add(_cHeaderLabel);
            Panel2.Controls.Add(_cToolBar);
            _cToolBar.Items.Add(_cButtonHelp);
#if DEBUG
            _cToolBar.Items.Add(_cButtonDebug);
#endif

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            BorderStyle = BorderStyle.Fixed3D;
            Dock = DockStyle.Fill;
            IsSplitterFixed = true;
            FixedPanel = FixedPanel.Panel1;
            Orientation = Orientation.Horizontal;
            TabStop = false;
            Panel1Collapsed = true;

            // _cButtonHelp
            {
                _cButtonHelp.Image = global::nlResourcesImages.Properties.Resources._Sign_Question_b32;
                _cButtonHelp.ToolTipText = "[ F1 ] " + elmApplication.__oTunes.__mTranslate("Помощь");
                _cButtonHelp.__eClickLeft += mButtonHelp_eMouseClickLeft;
                _cButtonHelp.__eClickRight += mButtonHelp_eMouseClickRight;
            }
            // _cButtonDebug
            {
                _cButtonDebug.Image = global::nlResourcesImages.Properties.Resources._Gears_b32;
                _cButtonDebug.ToolTipText = "Операции тестирования";
                _cButtonDebug.__eClickLeft += mButtonDebug_eMouseClickLeft;
                _cButtonDebug.__eClickRight += mButtonDebug_eMouseClickRight;
            }
            // _cHeaderPicture
            {
                _cHeaderPicture.BorderStyle = BorderStyle.Fixed3D;
                _cHeaderPicture.Location = new Point(elmInterface.__fIntervalHorizontal, elmInterface.__fIntervalVertical);
                _cHeaderPicture.Size = new Size(36, 36);
            }
            // _cHeaderLabel
            {
                _cHeaderLabel.Location = new Point(_cHeaderPicture.Left + _cHeaderPicture.Width + elmInterface.__fIntervalHorizontal, _cHeaderPicture.Height / 2);
                _cHeaderLabel.__fCaption_ = "Название области";
                _cHeaderLabel.__fLabelType_ = LABELTYPES.Title;
            }

            SplitterDistance = _cHeaderPicture.Top + _cHeaderPicture.Height + elmInterface.__fIntervalVertical * 2;

            #endregion Настройка компонентов

            ResumeLayout();

            return;
        }

        #region - Кнопки управления

        /// <summary>
        /// Выполняется при выборе кнопки 'Помощь'
        /// </summary>
        public void __mPressButtonHelp()
        {
            mButtonHelp_eMouseClickLeft(_cButtonHelp, new EventArgs());
            return;
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Помощь' левой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonHelp_eMouseClickLeft(object sender, EventArgs e)
        {
            if (__eButtonHelp_ClickLeft != null)
                __eButtonHelp_ClickLeft(_cButtonHelp, e);
            else
                (FindForm() as elmForm).__mHelp();
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Помощь' правой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonHelp_eMouseClickRight(object sender, EventArgs e)
        {
            if (__eButtonHelp_ClickRight != null)
                __eButtonHelp_ClickRight(_cButtonHelp, e);
            else
            {
                Form vForm = FindForm() as Form;
                if (vForm.MinimumSize.Width > 0)
                {
                    vForm.MinimumSize = new Size(0, 0);
                    (FindForm() as elmForm).__cPanelStatus.__fCaption_ = "Минимальные размеры формы сброшены";
                }
                else
                {
                    vForm.MinimumSize = new Size(vForm.Width, vForm.Height);
                    (FindForm() as elmForm).__cPanelStatus.__fCaption_ = "Текущие размеры установлены как минимальные";
                }
            }
        }



        /// <summary>
        /// Выполняется при выборе кнопки 'Тест' левой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonDebug_eMouseClickLeft(object sender, EventArgs e)
        {
            if (__eButtonDebug_ClickLeft != null)
                __eButtonDebug_ClickLeft(_cButtonDebug, e);
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Тест' правой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonDebug_eMouseClickRight(object sender, EventArgs e)
        {
            if (__eButtonDebug_ClickRight != null)
                __eButtonDebug_ClickRight(_cButtonDebug, e);
        }

        #endregion Кнопки управления

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region * Информация о файле

        /// <summary>
        /// Получение пути и имени файла класса
        /// </summary>
        protected string _mClassFilePath([CallerFilePath] string pFilePath = "")
        {
            return pFilePath;
        }
        /// <summary>
        /// Получение значения номера строки в текущей процедуре
        /// </summary>
        protected int _mClassLine(string pMessage = "", [CallerLineNumber] int pLine = 0)
        {
            return pLine;
        }
        /// <summary>
        /// Получение названия текущей процедуры
        /// </summary>
        protected string _mClassProcedure(string message, [CallerMemberName] string pMember = "")
        {
            return pMember;
        }

        #endregion Информация о файле

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Идентификатор области 
        /// </summary>
        /// <remarks>Для разруливание двух одинаковых областей на одной форме</remarks>
        public string __fAreaId = "";

        #endregion Атрибуты

        #region - Закрытые

        /// <summary>
        /// Текст заголовка области
        /// </summary>
        private string fHeaderText = "";

        #endregion Закрытые

        #region - Скрытые

        /// <summary>
        /// Признак указывающий, что в данный момент открыто выпадающее меню кнопки
        /// </summary>
        protected bool _fDropDownOpened = false;
        /// <summary>
        /// Объект ошибки
        /// </summary>
        protected appUnitError _fError;

        #endregion Скрытые

        #region - Компоненты

        /// <summary>
        /// Полоса инструментов
        /// </summary>
        protected elmComponentToolbar _cToolBar = new elmComponentToolbar();
        /// <summary>
        /// Кнопка 'Помощь'
        /// </summary>
        protected elmComponentToolbarButton _cButtonHelp = new elmComponentToolbarButton();
        /// <summary>
        /// Кнопка 'Отладка'
        /// </summary>
        protected elmComponentToolbarButton _cButtonDebug = new elmComponentToolbarButton();
        /// <summary>
        /// Изображение в заголовке области
        /// </summary>
        protected elmComponentPicture _cHeaderPicture = new elmComponentPicture();
        /// <summary>
        /// Заголовок названия области
        /// </summary>
        protected elmComponentLabel _cHeaderLabel = new elmComponentLabel();

        #endregion Компоненты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        #region - Скрытые

        /// <summary>
        /// Путь к файлу текущего класса
        /// </summary>
        protected string _fClassFilePath_
        {
            get { return _mClassFilePath(); }
        }
        /// <summary>
        /// Текущая процедура класса
        /// </summary>
        protected string _fClassProcedure_
        {
            get { return _mClassProcedure(""); }
        }
        /// <summary>
        /// Номер строки в файле класса
        /// </summary>
        protected int _fClassLine_
        {
            get { return _mClassLine(""); }
        }

        #endregion Скрытые

        /// <summary>
        /// Видимость кнопки 'Отладка'
        /// </summary>
        public bool __fButtonDebugVisible_
        {
            get { return _cButtonDebug.Visible; }
            set
            {
                _cButtonDebug.Visible = value;
            }
        }
        /// <summary>
        /// Доступность кнопки 'Помощь'
        /// </summary>
        public bool __fButtonHelpEnabled_
        {
            get { return _cButtonHelp.Enabled; }
            set { _cButtonHelp.Enabled = value; }
        }
        /// <summary>
        /// Видимость кнопки 'Помощь'
        /// </summary>
        public bool __fButtonHelpVisible_
        {
            get { return _cButtonHelp.Visible; }
            set { _cButtonHelp.Visible = value; }
        }
        /// <summary>
        /// Подсказка к кнопке 'Помощь' переведенная на язык пользователя
        /// </summary>
        public string __fButtonHelpToolTipText
        {
            set { _cButtonHelp.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Изображение на кнопке 'Помощь'
        /// </summary>
        public Image __fButtonHelpImage
        {
            set { _cButtonHelp.Image = value; }
        }

        /// <summary>
        /// Изображение-логотип области 
        /// </summary>
        public Image __fHeaderImage_
        {
            set { _cHeaderPicture.Image = value; }
        }
        /// <summary>
        /// Текст заголовка области
        /// </summary>
        /// <remarks>Выполняется перевод на язык интерфейса. При чтении возвращается не переведенный текст</remarks>
        public string __fHeaderCaption_
        {
            get { return fHeaderText; }
            set
            {
                fHeaderText = value;
                _cHeaderLabel.__fCaption_ = elmApplication.__oTunes.__mTranslate(value);
            }
        }
        /// <summary>
        /// Видимость заголовка
        /// </summary>
        public bool __fHeaderVisible_
        {
            get { return !Panel1Collapsed; }
            set
            {
                Panel1Collapsed = !value;
            }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при клике левой клавиши мыши по кнопке 'Помощь'
        /// </summary>
        public event EventHandler __eButtonHelp_ClickLeft;
        /// <summary>
        /// Возникает при клике правой клавиши мыши по кнопке 'Помощь'
        /// </summary>
        public event EventHandler __eButtonHelp_ClickRight;
        /// <summary>
        /// Возникает при выборе кнопки 'Отладка' левой клавишей мыши
        /// </summary>
        public event EventHandler __eButtonDebug_ClickLeft;
        /// <summary>
        /// Возникает при выборе кнопки 'Отладка' левой клавишей мыши
        /// </summary>
        public event EventHandler __eButtonDebug_ClickRight;

        #endregion СОБЫТИЯ
    }
}
