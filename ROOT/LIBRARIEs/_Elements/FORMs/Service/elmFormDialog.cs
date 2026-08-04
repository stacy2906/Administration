using nlApplication;
using System.Drawing;
using System.Windows.Forms;
using System;
using nlResourcesSounds;

namespace nlElements
{
    /// <summary>
    /// Файл elmFormDialog.cs
    /// </summary>
    /// <remarks>Класс-форма для диалога с пользователем</remarks>
    public class elmFormDialog : elmForm
    {
        #region = КОНСТРУКТОРЫ
        public elmFormDialog()
        {
            Type myType = this.GetType();
            __Class = myType.Namespace + "." + myType.Name + ".";
        }

        #endregion = КОНСТРУКТОРЫ

        #region = МЕТОДЫ

        #region - Данные

        /// <summary>Загрузка данных.
        /// </summary>
        public void _DataLoad()
        {
            //int vrFormHght = _pct.Top + _pct.Height; // Расчетная высота формы
            //int vrFormWdth = 300; // Расчетная ширина формы

            //#region * Отображение кнопок

            //switch (_ButtonList)
            //{
            //    case MessageBoxButtons.AbortRetryIgnore:
            //        _btnFirst._Value = DialogResult.Abort;
            //        _btnSecond._Value = DialogResult.Retry;
            //        _btnThird._Value = DialogResult.Ignore;
            //        break;
            //    case MessageBoxButtons.OK:
            //        _btnFirst.Visible = false;
            //        _btnFirst.TabStop = false;
            //        _btnSecond.Visible = false;
            //        _btnSecond.TabStop = false;
            //        _btnThird._Value = DialogResult.OK;
            //        break;
            //    case MessageBoxButtons.OKCancel:
            //        _btnFirst.Visible = false;
            //        _btnFirst.TabStop = false;
            //        _btnSecond._Value = DialogResult.OK;
            //        _btnThird._Value = DialogResult.Cancel;
            //        break;
            //    case MessageBoxButtons.RetryCancel:
            //        _btnFirst.Visible = false;
            //        _btnFirst.TabStop = false;
            //        _btnSecond._Value = DialogResult.Retry;
            //        _btnThird._Value = DialogResult.Cancel;
            //        break;
            //    case MessageBoxButtons.YesNo:
            //        _btnFirst.Visible = false;
            //        _btnFirst.TabStop = false;
            //        _btnSecond._Value = DialogResult.Yes;
            //        _btnThird._Value = DialogResult.No;
            //        break;
            //    case MessageBoxButtons.YesNoCancel:
            //        _btnFirst._Value = DialogResult.Yes;
            //        _btnSecond._Value = DialogResult.No;
            //        _btnThird._Value = DialogResult.Cancel;
            //        break;
            //}

            //if (__HelpTopic.Length > 0)
            //{
            //    _btnHelp.Visible = true;
            //}
            //else
            //    _btnHelp.Visible = false;

            //#endregion * Отображение кнопок

            //#region * Назначение кнопки по умолчанию

            //switch (_ButtonDefault)
            //{
            //    case MessageBoxDefaultButton.Button1:
            //        _btnFirst.Select();
            //        __ButtonDefaultSelected = _btnFirst;
            //        __Value = _btnFirst._Value; // Для того, чтобы форма вернула значение по умолчанию, если будет закрыта красной кнопкой с крестиком
            //        break;
            //    case MessageBoxDefaultButton.Button2:
            //        _btnSecond.Select();
            //        __ButtonDefaultSelected = _btnSecond;
            //        __Value = _btnSecond._Value;  // Для того, чтобы форма вернула значение по умолчанию, если будет закрыта красной кнопкой с крестиком
            //        break;
            //    case MessageBoxDefaultButton.Button3:
            //        _btnThird.Select();
            //        __ButtonDefaultSelected = _btnThird;
            //        __Value = _btnThird._Value;  // Для того, чтобы форма вернула значение по умолчанию, если будет закрыта красной кнопкой с крестиком
            //        break;
            //}

            //#endregion * Назначение кнопки по умолчанию

            //#region * Определение размера текста сообщения

            //SizeF vrSize___F = new SizeF();
            //vrSize___F = elmTypeFont.__mMeasureText(_Message, _lblMesg.Font);
            //_lblMesg.Text = _Message;
            //_lblMesg.RightToLeft = RightToLeft.Inherit;
            //_lblMesg.Height = (int)vrSize___F.Height;
            //_lblMesg.Width = (int)vrSize___F.Width;

            //#endregion * Определение размера текста сообщения

            //#region * Определение размеров формы

            //if (vrFormWdth >= _lblMesg.Left + _lblMesg.Width) // Определение высоты.
            //    vrFormWdth = vrFormWdth + __Margin;
            //else
            //    vrFormWdth = _lblMesg.Left + _lblMesg.Width + __Margin;

            //if (vrFormHght >= _lblMesg.Top + _lblMesg.Height) // Определение ширины.
            //    vrFormHght = vrFormHght + __Margin;
            //else
            //    vrFormHght = _lblMesg.Top + _lblMesg.Height + __Margin;

            //_mrk.Text = _CheckText;
            //if (_mrk.Text.Length > 0) // Используется метка.
            //{
            //    _mrk.Visible = true;
            //    _mrk.Top = vrFormHght;
            //    if (vrFormWdth < _mrk.Left + _mrk.Width)
            //        vrFormWdth = _mrk.Left + _mrk.Width + __Margin;
            //    vrFormHght = vrFormHght + _mrk.Height + __Margin;
            //}

            //_btnFirst.Top = vrFormHght;
            //_btnSecond.Top = vrFormHght;
            //_btnThird.Top = vrFormHght;
            //_btnHelp.Top = vrFormHght;

            //vrFormHght = vrFormHght + _btnFirst.Height + __Margin;

            //#region ** Настройка таймера

            //if (_Wait > 0)
            //{
            //    _stt._CyclesCount = _Wait;
            //    _stt._CycleCurrent = 0;
            //    _stt._Text = appApplication.__oTunes.__mTranslate("Ожидается решение пользователя");

            //    __Cycle = 0;
            //    _tmr.Enabled = true;
            //    _tmr.Interval = 1000;
            //    _tmr.Start();
            //    vrFormHght = vrFormHght + _stt.Height + __Margin;
            //    _stt.Visible = true;
            //}
            //else
            //    _stt.Visible = false;

            //#endregion ** Настройка таймера

            //if (vrFormWdth < _btnThird.Width + __Margin + _BorderWidth)
            //    vrFormWdth = _btnThird.Width + __Margin + _BorderWidth;
            //else
            //{
            //    _btnThird.Left = vrFormWdth - _btnThird.Width;
            //    _btnSecond.Left = _btnThird.Left - _btnSecond.Width - __Margin;
            //    _btnFirst.Left = _btnSecond.Left - _btnFirst.Width - __Margin;
            //}

            //this.Height = vrFormHght + _HeaderHeight + _BorderWidth;
            //this.Width = vrFormWdth + _HeaderHeight;
            //this.MinimumSize = this.Size; // Текущие размеры формы принимаются как минимальные
            //this.MaximumSize = this.Size; // Текущие размеры формы принимаются как максимальные
            //this.StartPosition = FormStartPosition.CenterScreen;

            //#endregion * Определение размеров формы

            //#region * Определение кнопки по умолчанию

            //switch (_ButtonDefault)
            //{
            //    case MessageBoxDefaultButton.Button1:
            //        _btnFirst.Focus();
            //        break;
            //    case MessageBoxDefaultButton.Button2:
            //        _btnSecond.Focus();
            //        break;
            //    case MessageBoxDefaultButton.Button3:
            //        _btnThird.Focus();
            //        break;
            //}

            //#endregion * Определение кнопки по умолчанию
        }

        #endregion - Данные

        #region - Действия

        /// <summary>Клик по любой кнопке решения пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Button_Click(object sender, System.EventArgs e)
        {
            //if (sender == null)
            //    __Value = (__ButtonDefaultSelected as apComButton)._Value;
            //else
            //    __Value = (sender as apComButton)._Value;
            //if (_tmr != null)
            //    _tmr.Stop();
            Close();
        }

        /// <summary>Клик по кнопке вызова помощи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _butHelp_Click(object sender, EventArgs e)
        {
            if (_tmr != null)
                _tmr.Stop();
            //_stt._CycleCurrent = 0;
            //_stt._Text = "";
        }

        /// <summary>Клик по изображению
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _pct_Click(object sender, EventArgs e)
        {
            _tmr.Stop();
            //_stt._CycleCurrent = 0;
            //_stt._Text = "";
        }

        /// <summary>Выполняется при завершении цикла таймера.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tmr_Tick(object sender, EventArgs e)
        {
            if (_Wait == __Cycle)
            {
                _tmr.Stop();
                __Beep._TickFinish();
                this._Button_Click(null, null);
                return;
            }
            __Cycle++;
           // _stt._CycleCurrent = __Cycle;
            __Beep._Tick();
        }

        #endregion - Действия

        #region - Форма

        /// <summary>Построение объекта.
        /// </summary>
        /// <returns></returns>
        protected override void _mObjectAssembly()
        {
            base._mObjectAssembly();

            #region * Определение типов компонентов

            //_stt = new apPanStatus();
            //_pct = new apCmpPicture();
            //_lblMesg = new apCmpLabel();
            //_mrk = new apComCheck();
            //_btnFirst = new apComButton();
            //_btnSecond = new apComButton();
            //_btnThird = new apComButton();
            //_tmr = new Timer();
            //_btnHelp = new apComButton();

            #endregion * Определение типов компонентов

            #region * Форма

            #region ** _pct

            //switch (_MessageType)
            //{
            //    case apItmEnum._MessageType.Error:
            //        _pct.Image = System.Drawing.SystemIcons.Error.ToBitmap();
            //        break;
            //    case apItmEnum._MessageType.Info:
            //        _pct.Image = System.Drawing.SystemIcons.Asterisk.ToBitmap();
            //        break;
            //    case apItmEnum._MessageType.Question:
            //        _pct.Image = System.Drawing.SystemIcons.Question.ToBitmap();
            //        break;
            //    case apItmEnum._MessageType.Warning:
            //        _pct.Image = System.Drawing.SystemIcons.Warning.ToBitmap();
            //        break;
            //}
            _pct.Location = new System.Drawing.Point(12, 12);
            _pct.Name = "_pct";
            _pct.Size = new System.Drawing.Size(40, 40);
            _pct.TabStop = false;
            _pct.Click += new System.EventHandler(_pct_Click);

            #endregion ** _pct

            #region ** _lblMssg

            _lblMesg.AutoSize = true;
            _lblMesg.Cursor = Cursors.Default;
            _lblMesg.Location = new Point(58, 24);
            _lblMesg.Name = "_lblMesg";
            _lblMesg.Size = new Size(62, 15);
            _lblMesg.Text = _Message;

            #endregion ** _lblMssg

            #region ** _mrk

            //_mrk._Enabled = true;
            //_mrk._LabelType = apItmEnum._TypeLabel.Normal;
            _mrk.AutoSize = true;
            _mrk.Cursor = Cursors.Default;
            _mrk.Location = new Point(55, 85);
            _mrk.Name = "_mrk";
            _mrk.Size = new Size(82, 19);
            _mrk.TabIndex = 2;
            _mrk.TabStop = false;
            _mrk.Text = "";
            _mrk.UseVisualStyleBackColor = true;
            _mrk.Visible = false;

            #endregion ** _mrk

            #region ** _btnFirst

            //_btnFirst._Value = DialogResult.None;
            //_btnFirst.Location = new Point(155, 125);
            _btnFirst.Name = "_btnFirst";
            _btnFirst.Size = new Size(75, 25);
            //_btnFirst.TabIndex = 4;
            //_btnFirst.TabStop = true;
            _btnFirst.Text = "";
            //_btnFirst.UseVisualStyleBackColor = true;
            _btnFirst.Click += _Button_Click;

            #endregion ** _btnFirst

            #region ** _btnSecond

            //_btnSecond._Value = DialogResult.None;
            //_btnSecond.Location = new Point(236, 125);
            _btnSecond.Name = "_btnSecond";
            _btnSecond.Size = new Size(75, 25);
            //_btnSecond.TabIndex = 5;
            //_btnSecond.TabStop = true;
            _btnSecond.Text = "";
            //_btnSecond.UseVisualStyleBackColor = true;
            _btnSecond.Click += _Button_Click;

            #endregion ** _btnSecond

            #region ** _btnThird

            //_btnThird._Value = DialogResult.None;
            //_btnThird.Location = new Point(317, 125);
            _btnThird.Name = "_btnThird";
            _btnThird.Size = new Size(75, 25);
            //_btnThird.TabIndex = 6;
            //_btnThird.TabStop = true;
            _btnThird.Text = "";
            //_btnThird.UseVisualStyleBackColor = true;
            _btnThird.Click += _Button_Click;

            #endregion ** _btnThird

            #region ** _tmr

            _tmr.Tick += _tmr_Tick;

            #endregion ** _tmr

            #region ** _btnHelp

            //_btnHelp._Value = DialogResult.None;
            //_btnHelp.Location = new Point(13, 125);
            _btnHelp.Name = "_btnHelp";
            _btnHelp.Size = new Size(75, 25);
            //_btnHelp.TabStop = false;
            _btnHelp.Text = appApplication.__oTunes.__mTranslate("Помощь");
            //_btnHelp.UseVisualStyleBackColor = true;
            _btnHelp.Click += this._butHelp_Click;

            #endregion ** _btnHelp

            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(404, 177);
            Controls.Add(_stt);
            //Controls.Add(_btnHelp);
            //Controls.Add(_btnThird);
            //Controls.Add(_btnSecond);
            //Controls.Add(_btnFirst);
            Controls.Add(_mrk);
            Controls.Add(_lblMesg);
            Controls.Add(_pct);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fmMessage";
            Text = appApplication.__oTunes.__mTranslate("Сообщение приложения");

            #endregion * Форма

        }

        #endregion - Форма

        #endregion = МЕТОДЫ

        #region = ПЕРЕМЕННЫЕ

        /// <summary>Название класса.
        /// </summary>
        private string __Class = "";

        /// <summary>
        /// Объект для воспроизведения звуков
        /// </summary>
        private rssBeep __Beep = new rssBeep();

        private Button __ButtonDefaultSelected = null;

        /// <summary>Счетчик интервалов таймера.
        /// </summary>
        private int __Cycle = -1;

        /// <summary>
        /// Топик помощи описывающий событие.
        /// </summary>
        private string __HelpTopic = "";

        /// <summary>
        /// Размер отступов между компонентами.
        /// </summary>
        const int __Margin = 6;

        /// <summary>
        /// Объект для чтения текста.
        /// </summary>
        private rssSpeech __Speech = new rssSpeech();

        #region - Компоненты

        private elmComponentPicture _pct;
        private elmComponentMark _mrk;
        private elmComponentLabel _lblMesg;
        private elmComponentToolbarButton _btnHelp;
        private elmComponentToolbarButton _btnFirst;
        private elmComponentToolbarButton _btnSecond;
        private elmComponentToolbarButton _btnThird;
        private Timer _tmr;
        private elmPanelStatus _stt;

        #endregion - Компоненты

        #endregion = ПЕРЕМЕННЫЕ

        #region = ПОЛЯ

        /// <summary>Отображаемое сообщение.
        /// </summary>
        public string _Message = "";

        /// <summary>Вид отображаемого сообщения.
        /// </summary>
        public MESSAGESTYPES _MessageType = MESSAGESTYPES.None;

        /// <summary>Отображаемые кнопки.
        /// </summary>
        public MessageBoxButtons _ButtonList = MessageBoxButtons.OK;

        /// <summary>Кнопка по умолчанию.
        /// </summary>
        public MessageBoxDefaultButton _ButtonDefault = MessageBoxDefaultButton.Button1;

        /// <summary>Значение возвращаемое формой.
        /// </summary>
        public DialogResult __Value = DialogResult.None;

        /// <summary>Время ожидания решения пользователя.
        /// </summary>
        public int _Wait = -1;

        #endregion = ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>Значение выбора.
        /// </summary>
        public bool _CheckValue
        {
            get { return _mrk.Checked; }
            set { _mrk.Checked = Convert.ToBoolean(value); }
        }

        /// <summary>Текст выбора.
        /// </summary>
        public string _CheckText
        {
            get { return _mrk.Text; }
            set { _mrk.Text = value; }
        }

        #endregion = СВОЙСТВА
    }
}
