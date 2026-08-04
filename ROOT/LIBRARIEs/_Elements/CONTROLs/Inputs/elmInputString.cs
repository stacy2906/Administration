using System;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmInputString.cs
    /// </summary>
    /// <remarks>Класс-поле ввода строчных значений</remarks>
    /* Пример использования 
                _cInputString.Location = new System.Drawing.Point(10, 40);
                _cInputString.__fCaption_ = "Строка";
                _cInputString.__mCaptionBuilding("Строка {0}", 2);
                _cInputString.__fFillType_ = FILLTYPES.Necessarily;
                _cInputString.__fPromptCaption_ = "Ввод строки";
                _cInputString.__fSymbolsCount_ = 30;
                _cInputString.__fValue_ = "Значение";
    */
    public class elmInputString : elmInput
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

            Panel2.Controls.Add(_cInput);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            // _cLabelCaption
            {
                _cLabelCaption.__fLabelType_ = LABELTYPES.Normal; /// Назначение вида надписи-заголовка - 'Надпись' 
                _cLabelCaption.__eClickLeft += mEventCaptionMouseClickLeft;
                _cLabelCaption.__eClickRight += mEventCaptionMouseClickRight;
            }
            /// Сохранение установленного статуса надписи-заголовка
            fLabelCaptionStatus = _cLabelCaption.__fLabelType_; 
            // _cInput
            {
                _cInput.Location = new Point(0, 0);
                _cInput.__eChanged += mEventInputChanged;
                _cInput.__eChangedByProgram += mEventInputChangedByProgram;
                _cInput.__eChangedByUserAfter += mEventInputChangedByUser;
                _cInput.__eKeyDown += mEventInputKeyDown;
            }

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется при выборе надписи левой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mEventCaptionMouseClickLeft(object sender, EventArgs e)
        {
            if (_cLabelCaption.__fLabelType_ == LABELTYPES.Button)
                if (__eCaptionMouseClickLeft != null)
                    __eCaptionMouseClickLeft(sender, e);
        }
        /// <summary>
        /// Выполняется при выборе надписи правой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mEventCaptionMouseClickRight(object sender, EventArgs e)
        {
            if (__fSymbolsCount_ > 0)
            {
                /// Если разрешено очищается поле ввода от данных
                if (__fInputClearOnClickLeft == true)
                    _cInput.Text = "";
                /// Выключается использование фильтра
                __fMarkStatus_ = false;
                /// Перемещается курсор в поле ввода
                _cInput.Focus();
            }
            else
            {
                //elmFormNotice vFormNotice = new elmFormNotice();
                //vFormNotice.__cAreaNotice.__fTextSizeMax_ = 1000;
                //vFormNotice.__cAreaNotice.__fText_ = _cInput.Text;
                //vFormNotice.ShowDialog();
                //_cInput.Text = vFormNotice.__cAreaNotice.__fText_;
            }
            if (__eCaptionMouseClickRight != null)
                __eCaptionMouseClickRight(sender, e);
        }
        /// <summary>
        /// Выполняется при изменении данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mEventInputChanged(object sender, EventArgs e)
        {
            if (__eInputChanged != null)
                __eInputChanged(sender, e);
        }
        /// <summary>
        /// Выполняется при изменении данных программой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mEventInputChangedByProgram(object sender, EventArgs e)
        {
            if (__eInputChangedByProgram != null)
                __eInputChangedByProgram(sender, e);
        }
        /// <summary>
        /// Выполняется при изменении данных пользователем
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mEventInputChangedByUser(object sender, EventArgs e)
        {
            if (__eInputChangedByUser != null)
                __eInputChangedByUser(sender, e);
            /// Включение использования фильтра
            __fMarkStatus_ = true;

            return;
        }
        /// <summary>
        /// Выполняется при нажатии на клавиши
        /// </summary>
        /// <param name="e"></param>
        private void mEventInputKeyDown(object sender, EventArgs e)
        {
            if (__eInputKeyDown != null)
                __eInputKeyDown(sender, e);
        }

        #endregion Поведение 

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Внутренние

        /// <summary>
        /// Вид надписи перед переходом в недоступный режим
        /// </summary>
        private LABELTYPES fLabelCaptionStatus = LABELTYPES.Normal;

        #endregion Внутренние

        #region - Компоненты

        /// <summary>
        /// Поле ввода символьных данных
        /// </summary>
        protected elmComponentString _cInput = new elmComponentString();

        #endregion Компоненты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Доступность контрола
        /// </summary>
        public override bool __fEnabled_
        {
            get { return base.__fEnabled_; }
            set
            {
                base.__fEnabled_ = value;
                _cInput.Visible = value;
                if (value == true)
                {
                    _cLabelCaption.__fLabelType_ = fLabelCaptionStatus;
                }
                else
                {
                    _cLabelCaption.__fLabelType_ = LABELTYPES.Normal;
                    if (_cInput.Text.Trim().Length > 0)
                        _cLabelValue.Text = _cInput.Text.Trim();
                    else
                        _cLabelValue.Text = elmApplication.__oTunes.__mTranslate("нет данных");
                }
            }
        }
        /// <summary>
        /// Обязательность заполнения
        /// </summary>
        public override FILLTYPES __fFillType_
        {
            get { return _cInput.__fFillType_; }
            set { _cInput.__fFillType_ = value; }
        }
        /// <summary>
        /// Условие фильтра для указанного поля
        /// </summary>
        public override string __fFilterExpression_
        {
            get
            {
                string vFilter = ""; // Условие фильтра
                string vString = _cInput.Text.Trim();
                string[] vWordsList = vString.Split(' '); // Количество слов в условии
                foreach (string vWord in vWordsList)
                {
                    if (vWord.Length == 0)
                        continue;
                    if (vFilter.Length != 0)
                        vFilter = vFilter + " and ";
                    if (__fTableAlias.Length > 0)
                        vFilter = vFilter + __fTableAlias + ".";
                    vFilter = vFilter + __fFieldName + " Like N'%" + vWord + "%'";
                }

                return vFilter;
            }
        }
        /// <summary>
        /// Выражение фильтра для указанного поля для отображения пользователю
        /// </summary>
        public override string __fFilterMessage_
        {
            get
            {
                return _cLabelCaption.Text.Trim() + " = '" + __fValue_.ToString().Trim() + "'";
            }
        }
        /// <summary>
        /// Многострочное использование
        /// </summary>
        public virtual bool __fMultiline_
        {
            get { return _cInput.Multiline; }
            set
            {
                _cInput.Multiline = value;
                if (_cInput.Multiline == true)
                {
                    _cInput.Dock = DockStyle.Fill;
                    _cInput.ScrollBars = ScrollBars.Both;
                    _cInput.Width = 50;
                    _cInput.WordWrap = false;
                }
                else
                {
                    //Height = fHeightNormal;
                    _cInput.Dock = DockStyle.None;
                    _cInput.ScrollBars = ScrollBars.None;
                    _cInput.WordWrap = true;
                }
            }
        }
        /// <summary>
        /// Символ для маскировки введенного пароля
        /// </summary>
        public char __fPasswordChar_
        {
            get { return _cInput.PasswordChar; }
            set { _cInput.PasswordChar = value; }
        }
        /// <summary>
        /// Количество отображаемых символов данных
        /// </summary>
        public virtual int __fSymbolsCount_
        {
            get { return _cInput.__fSymbolsCount_; }
            set { _cInput.__fSymbolsCount_ = value; }
        }
        /// <summary>
        /// Значение поля ввода
        /// </summary>
        public override object __fValue_
        {
            get { return _cInput.Text; }
            set
            {
                _cInput.Text = value.ToString().Trim();
                _cLabelValue.Text = _cInput.Text;  // Запись значения по умолчанию
            }
        }
        /// <summary>
        /// Вид надписи
        /// </summary>
        public LABELTYPES __fLabelType_
        {
            get { return _cLabelCaption.__fLabelType_; }
            set { _cLabelCaption.__fLabelType_ = value; }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при клике левой кнопки мыши по надписи-заголовку
        /// </summary>
        public event EventHandler __eCaptionMouseClickLeft;
        /// <summary>
        /// Возникает при клике правой кнопки мыши по надписи-заголовку
        /// </summary>
        public event EventHandler __eCaptionMouseClickRight;
        /// <summary>
        /// Возникает при изменении данных в поле ввода 
        /// </summary>
        public event EventHandler __eInputChanged;
        /// <summary>
        /// Возникает при изменении данных в поле ввода программой
        /// </summary>
        public event EventHandler __eInputChangedByProgram;
        /// <summary>
        /// Возникает при изменении данных в поле ввода пользователем
        /// </summary>
        public event EventHandler __eInputChangedByUser;
        /// <summary>
        /// Возникает при нажатии клавиши в поле ввода
        /// </summary>
        public event EventHandler __eInputKeyDown;

        #endregion СОБЫТИЯ
    }
}
