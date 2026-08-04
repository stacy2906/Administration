using nlApplication;
using System.Drawing;
using System.Windows.Forms;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmAreaMessage.cs
    /// </summary>
    /// <remarks>Класс-область для диалога с пользователем</remarks>
    public class elmAreaDialog : elmArea
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

            _cToolBar.Items.Insert(0, _cButtonApply);
            _cToolBar.Items.Insert(1, _cButtonCancel);
            _cToolBar.Items.Add(_cButtonDetails);
            Panel2.Controls.Add(_cSplitterImageMessage);
            Panel2.Controls.SetChildIndex(_cSplitterImageMessage, 0);
            _cSplitterImageMessage.Panel1.Controls.Add(_cPicture);

            _cSplitterImageMessage.Panel1.Controls.Add(_cMark);

            _cSplitterImageMessage.Panel2.Controls.Add(_cLabel);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            // _cAreaMessage
            {
                // Кнопки управления
                {
                    // _cButtonApply
                    {
                        _cButtonApply.Click += _cButtonApply_Click;
                        _cButtonApply.Image = global::nlResourcesImages.Properties.Resources._Sign_Tick_g32;
                        _cButtonApply.ImageScaling = ToolStripItemImageScaling.None;
                    }
                    // _cButtonCancel
                    {
                        _cButtonCancel.Click += _cButtonCancel_Click;
                        _cButtonCancel.Image = global::nlResourcesImages.Properties.Resources._Sign_Cross_r32;
                        _cButtonCancel.ImageScaling = ToolStripItemImageScaling.None;
                    }
                    // _cButtonDetails
                    {
                        _cButtonDetails.Alignment = ToolStripItemAlignment.Right;
                        //_cButtonDetails.Image = global::nlResourcesImages.Properties.Resources._BookOpen_b32C;
                        _cButtonDetails.ImageScaling = ToolStripItemImageScaling.None;
                        _cButtonDetails.ToolTipText = "[ Ctrl + L ]\n" + elmApplication.__oTunes.__mTranslate("Подробно");
                        // _cButtonDetailHide
                        {
                            _cButtonDetailHide = _cButtonDetails.DropDownItems.Add(elmApplication.__oTunes.__mTranslate("Сообщение"));
                            _cButtonDetailHide.Click += _cButtonDetailHide_Click;
                            //_cButtonDetailHide.Image = global::nlResourcesImages.Properties.Resources._Book_b16C;
                            _cButtonDetailHide.ImageScaling = ToolStripItemImageScaling.None;
                        }
                        // _cButtonDetailShow
                        {
                            _cButtonDetailShow = _cButtonDetails.DropDownItems.Add(elmApplication.__oTunes.__mTranslate("Подробности"));
                            _cButtonDetailShow.Click += _cButtonDetailShow_Click;
                            //_cButtonDetailShow.Image = global::nlResourcesImages.Properties.Resources._BookOpen_b16C;
                            _cButtonDetailShow.ImageScaling = ToolStripItemImageScaling.None;
                        }
                    }
                }

                _cSplitterImageMessage.Orientation = Orientation.Horizontal;
                _cSplitterImageMessage.FixedPanel = FixedPanel.Panel1;
                _cSplitterImageMessage.IsSplitterFixed = true;
                _cSplitterImageMessage.BorderStyle = BorderStyle.Fixed3D;
                // _cPicture
                {
                    _cPicture.BackColor = Color.Transparent;
                    _cPicture.BorderStyle = BorderStyle.None;
                    _cPicture.Image = global::nlResourcesImages.Properties.Resources._Emotion_Smile_y;
                    _cPicture.SizeMode = PictureBoxSizeMode.CenterImage;
                    _cPicture.Size = new Size(60, 60);
                    _cPicture.Location = new Point(_cSplitterImageMessage.Width / 2 - _cPicture.Width / 2, 10);
                    _cPicture.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                }
                // _cMark
                {
                    _cMark.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    _cMark.Location = new Point(elmInterface.__fIntervalHorizontal * 2, 60);
                }
                // _cLabel
                {
                    _cLabel.AutoSize = true;
                    _cLabel.Location = new Point(elmInterface.__fIntervalHorizontal * 2, elmInterface.__fIntervalVertical * 2);
                    _cLabel.Size = new Size(_cSplitterImageMessage.Panel2.Width - elmInterface.__fIntervalHorizontal * 4, _cSplitterImageMessage.Panel2.Height - elmInterface.__fIntervalVertical * 4);
                    _cLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                }
            }

            #endregion Настройка компонентов

            ResumeLayout();

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();
            _cSplitterImageMessage.SplitterDistance = 90;
        }

        #endregion Объект

        #region - Поведение

        #region Кнопки управления

        /// <summary>
        /// Выполняется при выборе кнопки 'Ok, Да'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cButtonApply_Click(object sender, EventArgs e)
        {
            _cButtonDetailHide.PerformClick();
            switch (fMessageType)
            {
                case MESSAGESTYPES.None:
                    _fResult = DialogResult.OK;
                    break;
                case MESSAGESTYPES.Error:
                    _fResult = DialogResult.OK;
                    break;
                case MESSAGESTYPES.ErrorRetry:
                    _fResult = DialogResult.Retry;
                    break;
                case MESSAGESTYPES.Info:
                    _fResult = DialogResult.OK;
                    break;
                case MESSAGESTYPES.Question:
                    _fResult = DialogResult.Yes;
                    break;
                case MESSAGESTYPES.Warning:
                    _fResult = DialogResult.OK;
                    break;
            }
            Form vForm = FindForm();
            vForm.Close();
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Отмена, Нет'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cButtonCancel_Click(object sender, EventArgs e)
        {
            _cButtonDetailHide.PerformClick();
            switch (fMessageType)
            {
                case MESSAGESTYPES.ErrorRetry:
                    _fResult = DialogResult.Cancel;
                    break;
                case MESSAGESTYPES.Question:
                    _fResult = DialogResult.No;
                    break;
            }
            Form vForm = FindForm();
            vForm.Close();
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Помощь'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cButtonHelp_Click(object sender, EventArgs e)
        {
            elmForm vForm = FindForm() as elmForm;
            vForm.__mHelp();
        }


        /// <summary>
        /// Выполняется при выборе кнопки 'Подробно / Сообщение'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cButtonDetailHide_Click(object sender, EventArgs e)
        {
            _cLabel.Text = fMessage;
            if (FindForm() != null)
                FindForm().WindowState = FormWindowState.Normal;
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Подробно / Детали'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cButtonDetailShow_Click(object sender, EventArgs e)
        {
            _cLabel.Text = fMessage + "\n" + fMessageDetail;
            FindForm().WindowState = FormWindowState.Maximized;
        }

        #endregion Кнопки управления

        /// <summary>
        /// Выполняется после построения объекта
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (FindForm() != null)
                FindForm().WindowState = FormWindowState.Normal;
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Программный выбор кнопки 'Применить'
        /// </summary>
        public void __mPressButtonApply()
        {
            _cButtonApply.PerformClick();

            return;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Значение возвращаемый формой
        /// </summary>
        public DialogResult _fResult = DialogResult.None;

        #endregion Атрибуты

        #region - Внутренние

        /// <summary>
        /// Сообщение
        /// </summary>
        private string fMessage = "";
        /// <summary>
        /// Детали
        /// </summary>
        private string fMessageDetail = "";
        /// <summary>
        /// Вид сообщения
        /// </summary>
        private MESSAGESTYPES fMessageType = MESSAGESTYPES.None;

        #endregion Внутренние

        #region - Компоненты

        /// <summary>
        /// Разделитель 'Изображения / Сообщения' 
        /// </summary>
        protected elmComponentSplitter _cSplitterImageMessage = new elmComponentSplitter();
        /// <summary>
        /// Картинка вида сообщения 
        /// </summary>
        protected elmComponentPicture _cPicture = new elmComponentPicture();

        /// <summary>
        /// Кнопка 'Ok, Да'
        /// </summary>
        protected elmComponentToolbarButton _cButtonApply = new elmComponentToolbarButton();
        /// <summary>
        /// Кнопка 'Отмена, Нет'
        /// </summary>
        protected elmComponentToolbarButton _cButtonCancel = new elmComponentToolbarButton();
        /// <summary>
        /// Кнопка 'Подробно'
        /// </summary>
        protected elmComponentToolbarButtonMenu _cButtonDetails = new elmComponentToolbarButtonMenu();

        /// <summary>
        /// Меню 'Создать'
        /// </summary>
        protected ToolStripItem _cButtonDetailHide;
        /// <summary>
        /// Меню 'Изменить'
        /// </summary>
        protected ToolStripItem _cButtonDetailShow;

        /// <summary>
        /// Включатель
        /// </summary>
        protected elmComponentMark _cMark = new elmComponentMark();
        /// <summary>
        /// Текст
        /// </summary>
        protected elmComponentLabel _cLabel = new elmComponentLabel();

        #endregion Компоненты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string __fMessage_
        {
            set
            {
                fMessage = value;
                _cLabel.Text = fMessage;
            }
        }
        /// <summary>
        /// Вид сообщения
        /// </summary>
        public MESSAGESTYPES __fMessageType_
        {
            get { return fMessageType; }
            set
            {
                _cButtonApply.Visible = false;
                _cButtonCancel.Visible = false;
                Form vForm = FindForm(); // Форма на которой размещен контрол
                vForm.Text = appTypeString.__mWordPersonal(elmApplication.__fProcessName_);

                fMessageType = value;
                switch (fMessageType)
                {
                    case MESSAGESTYPES.None:
                        _cButtonApply.Visible = true;
                        _cButtonApply.Image = global::nlResourcesImages.Properties.Resources._Sign_Tick_g32;
                        _cButtonApply.ToolTipText = "[ Ctrl + 1 ]\n" + elmApplication.__oTunes.__mTranslate("Ok");
                        break;
                    case MESSAGESTYPES.Error:
                        vForm.Text = elmApplication.__oTunes.__mTranslate("Ошибка");
                        _cPicture.Image = global::nlResourcesImages.Properties.Resources._Emotion_Tear_y;

                        _cButtonApply.Visible = true;
                        _cButtonApply.Image = global::nlResourcesImages.Properties.Resources._Sign_Tick_g32;
                        _cButtonApply.ToolTipText = "[ Ctrl + 1 ]\n" + elmApplication.__oTunes.__mTranslate("Ok");
                        break;
                    case MESSAGESTYPES.ErrorRetry:
                        vForm.Text = elmApplication.__oTunes.__mTranslate("Ошибка");
                        _cPicture.Image = global::nlResourcesImages.Properties.Resources._Emotion_Tear_y;

                        _cButtonApply.Visible = true;
                        //_cButtonApply.Image = global::nlResourcesImages.Properties.Resources._Arrowrefresh_g32C; // ._Arrowrefresh_g32C;
                        _cButtonApply.ToolTipText = "[ Ctrl + 1 ]\n" + elmApplication.__oTunes.__mTranslate("Повторить");

                        _cButtonCancel.Visible = true;
                        _cButtonCancel.Image = global::nlResourcesImages.Properties.Resources._Sign_Cross_r32;
                        _cButtonCancel.ToolTipText = "[ Ctrl + 2 ]\n" + elmApplication.__oTunes.__mTranslate("Отмена");
                        break;
                    case MESSAGESTYPES.Info:
                        vForm.Text = elmApplication.__oTunes.__mTranslate("Информация");
                        _cPicture.Image = global::nlResourcesImages.Properties.Resources._Emotion_Glass_y;

                        _cButtonApply.Visible = true;
                        _cButtonApply.Image = global::nlResourcesImages.Properties.Resources._Sign_Tick_g32;
                        _cButtonApply.ToolTipText = "[ Ctrl + 1 ]\n" + elmApplication.__oTunes.__mTranslate("Ok");
                        break;
                    case MESSAGESTYPES.Question:
                        vForm.Text = elmApplication.__oTunes.__mTranslate("Вопрос");
                        _cPicture.Image = global::nlResourcesImages.Properties.Resources._Emotion_Sorrow_y;

                        _cButtonApply.Visible = true;
                        _cButtonApply.Image = global::nlResourcesImages.Properties.Resources._Sign_Tick_g32;
                        _cButtonApply.ToolTipText = "[ Ctrl + 1 ]\n" + elmApplication.__oTunes.__mTranslate("Да");

                        _cButtonCancel.Visible = true;
                        _cButtonCancel.Image = global::nlResourcesImages.Properties.Resources._Sign_Cross_r32;
                        _cButtonCancel.ToolTipText = "[ Ctrl + 2 ]\n" + elmApplication.__oTunes.__mTranslate("Нет");
                        break;
                    case MESSAGESTYPES.Warning:
                        vForm.Text = elmApplication.__oTunes.__mTranslate("Предупреждение");
                        _cPicture.Image = global::nlResourcesImages.Properties.Resources._Emotion_Glassdark_y;

                        _cButtonApply.Visible = true;
                        _cButtonApply.Image = global::nlResourcesImages.Properties.Resources._Sign_Tick_g32;
                        _cButtonApply.ToolTipText = "[ Ctrl + 1 ]\n" + elmApplication.__oTunes.__mTranslate("Ok");
                        break;
                }
            }
        }
        /// <summary>
        /// Видимость включателя
        /// </summary>
        public bool __fMarkVisible_
        {
            get { return _cMark.Visible; }
            set { _cMark.Visible = value; }
        }
        /// <summary>
        /// Статус включателя
        /// </summary>
        public bool __fMarkChecked_
        {
            get { return _cMark.Checked; }
            set { _cMark.Checked = value; }
        }
        /// <summary>
        /// Текст включателя
        /// </summary>
        public string __fMarkCaption_
        {
            get { return _cMark.__fCaption_; }
            set { _cMark.__fCaption_ = value; }
        }

        #endregion = СВОЙСТВА
    }
}
