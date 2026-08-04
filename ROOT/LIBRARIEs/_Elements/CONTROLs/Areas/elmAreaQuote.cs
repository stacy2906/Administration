using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmAreaNotice.cs
    /// </summary>
    /// <remarks>Класс-область для правки примечаний</remarks>
    public class elmAreaQuote : elmArea
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

            __fButtonHelpVisible_ = false;

            _cToolBar.Items.Add(_cButtonEdit);
            _cButtonEdit.DropDownItems.Add(_cMenuEditCut);
            _cButtonEdit.DropDownItems.Add(_cMenuEditCopy);
            _cButtonEdit.DropDownItems.Add(_cMenuEditPaste);
            _cButtonEdit.DropDownItems.Add("-");
            _cButtonEdit.DropDownItems.Add(_cMenuEditUndo);
            _cButtonEdit.DropDownItems.Add(_cMenuEditRedo);

            _cToolBar.Items.Add(_cButtonFont);
            _cButtonFont.DropDownItems.Add(_cMenuFontBold);
            _cButtonFont.DropDownItems.Add(_cMenuFontItalic);
            _cButtonFont.DropDownItems.Add(_cMenuFontUnderline);
            _cButtonFont.DropDownItems.Add(_cMenuFontStrikethroungh);
            _cButtonFont.DropDownItems.Add("-");
            _cButtonFont.DropDownItems.Add(_cMenuFontIncrease);
            _cButtonFont.DropDownItems.Add(_cMenuFontDecrease);
            _cButtonFont.DropDownItems.Add("-");
            _cButtonFont.DropDownItems.Add(_cMenuFontColor);
            _cButtonFont.DropDownItems.Add(_cMenuFontBackColor);

            _cToolBar.Items.Add(_cButtonText);
            _cButtonText.DropDownItems.Add(_cMenuTextAlignLeft);
            _cButtonText.DropDownItems.Add(_cMenuTextAlignCenter);
            _cButtonText.DropDownItems.Add(_cMenuTextAlignRight);
            _cButtonText.DropDownItems.Add(_cMenuTextAlignJustity);
            _cButtonText.DropDownItems.Add("-");
            _cButtonText.DropDownItems.Add(_cMenuTextListBullets);
            _cButtonText.DropDownItems.Add(_cMenuTextListNumbers);

            Panel2.Controls.Add(_cSplitter);
            Panel2.Controls.SetChildIndex(_cSplitter, 0);
            _cSplitter.Panel1.Controls.Add(_cLabel);
            _cSplitter.Panel2.Controls.Add(_cText);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            // _cButtonEdit
            {
                _cButtonEdit.DropDownOpened += mButtonDropDownOpened;
                _cButtonEdit.Image = global::nlResourcesImages.Properties.Resources._Page_b32;
                _cButtonEdit.ToolTipText = "[ Ctrl + I ] " + elmApplication.__oTunes.__mTranslate("Правка документа");

                _cMenuEditCut.Image = global::nlResourcesImages.Properties.Resources._Text_Scissors_b32;
                _cMenuEditCut.__fCaption_ = "Вырезать";
                _cMenuEditCopy.Image = global::nlResourcesImages.Properties.Resources._PageCopy_b32;
                _cMenuEditCopy.__fCaption_ = "Копировать";
                _cMenuEditPaste.Image = global::nlResourcesImages.Properties.Resources._Tablet_Page_o32;
                _cMenuEditPaste.__fCaption_ = "Вставить";
                _cMenuEditUndo.Image = global::nlResourcesImages.Properties.Resources._Text_ChangesRedo_b32;
                _cMenuEditUndo.__fCaption_ = "Отменить";
                _cMenuEditRedo.Image = global::nlResourcesImages.Properties.Resources._Text_ChangesUndo_b32;
                _cMenuEditRedo.__fCaption_ = "Вернуться";
            }
            // _cButtonFont
            {
                _cButtonFont.DropDownOpened += mButtonDropDownOpened;
                _cButtonFont.Image = global::nlResourcesImages.Properties.Resources._Font_b32;

                _cMenuFontBold.Image = global::nlResourcesImages.Properties.Resources._Font_Bold_w32;
                _cMenuFontBold.__fCaption_ = "Полужирный";
                _cMenuFontBold.Click += mMenuFontBold_Click;

                _cMenuFontItalic.Image = global::nlResourcesImages.Properties.Resources._Font_Italic_w32;
                _cMenuFontItalic.__fCaption_ = "Курсив";
                _cMenuFontItalic.Click += mMenuFontItalic_Click;

                _cMenuFontUnderline.Image = global::nlResourcesImages.Properties.Resources._Font_Underline_w32;
                _cMenuFontUnderline.__fCaption_ = "Подчеркнутый";
                _cMenuFontUnderline.Click += mMenuFontUnderline_Click;

                _cMenuFontStrikethroungh.Image = global::nlResourcesImages.Properties.Resources._Font_StrikeThroungh_w32;
                _cMenuFontStrikethroungh.__fCaption_ = "Зачеркнутый";
                _cMenuFontStrikethroungh.Click += mMenuFontStrikethroungh_Click;

                _cMenuFontIncrease.Image = global::nlResourcesImages.Properties.Resources._Font_SizeIncrease_w32;
                _cMenuFontIncrease.__fCaption_ = "Уменьшить шрифт";
                _cMenuFontIncrease.Click += mMenuFontIncrease_Click;

                _cMenuFontDecrease.Image = global::nlResourcesImages.Properties.Resources._Font_SizeDecrease_w32;
                _cMenuFontDecrease.__fCaption_ = "Увеличить шрифт";
                _cMenuFontDecrease.Click += mMenuFontDecrease_Click;

                _cMenuFontColor.Image = global::nlResourcesImages.Properties.Resources._Font_Color_d32;
                _cMenuFontColor.__fCaption_ = "Цвет шрифта";
                _cMenuFontColor.Click += mMenuFontColor_Click;

                _cMenuFontBackColor.Image = global::nlResourcesImages.Properties.Resources._Font_BackColor_d32;
                _cMenuFontBackColor.__fCaption_ = "Фон шрифта";
                _cMenuFontBackColor.Click += mMenuFontBackColor_Click;
            }
            // _cButtonText
            {
                _cButtonText.DropDownOpened += mButtonDropDownOpened;
                _cButtonText.Image = global::nlResourcesImages.Properties.Resources._Text_w32;
                _cButtonText.ToolTipText = "[Ctrl + T] - " + elmApplication.__oTunes.__mTranslate("Размещение выделенного текста по середине страницы");

                _cMenuTextAlignCenter.Image = global::nlResourcesImages.Properties.Resources._Text_AlignCenter_w32;
                _cMenuTextAlignCenter.__fCaption_ = "Привязка по центру";
                _cMenuTextAlignCenter.Click += mMenuTextAlignCenter_Click;

                _cMenuTextAlignLeft.Image = global::nlResourcesImages.Properties.Resources._Text_AlignLeft_w32;
                _cMenuTextAlignLeft.__fCaption_ = "Привязка по левому краю";
                _cMenuTextAlignLeft.Click += mMenuTextAlignLeft_Click;

                _cMenuTextAlignRight.Image = global::nlResourcesImages.Properties.Resources._Text_AlignRight_w32;
                _cMenuTextAlignRight.__fCaption_ = "Привязка по правому краю";
                _cMenuTextAlignRight.Click += mMenuTextAlignRight_Click;

                _cMenuTextAlignJustity.Image = global::nlResourcesImages.Properties.Resources._Text_AlignJustity_w32;
                _cMenuTextAlignJustity.__fCaption_ = "Растянуть по ширине страницы";
                _cMenuTextAlignJustity.Click += mMenuTextAlignJustity_Click;

                _cMenuTextListBullets.Image = global::nlResourcesImages.Properties.Resources._Text_ListBullets_w32;
                _cMenuTextListBullets.__fCaption_ = "Перечисление";
                _cMenuTextListBullets.Click += mMenuTextListBullets_Click;

                _cMenuTextListNumbers.Image = global::nlResourcesImages.Properties.Resources._Text_ListNumbers_w32;
                _cMenuTextListNumbers.__fCaption_ = "По порядку";
                _cMenuTextListNumbers.Click += mMenuTextListNumbers_Click;
            }
            // _cSplitter
            {
                _cSplitter.Orientation = Orientation.Horizontal;
                _cSplitter.SplitterDistance = 30;
                _cSplitter.IsSplitterFixed = true;
                _cSplitter.FixedPanel = FixedPanel.Panel1;
            }
            // _cLabel
            {
                _cLabel.Text = "Заполнено:";
            }
            // _cText
            {
                _cText.Dock = DockStyle.Fill;
                _cText.__eChangedByUser += mText_ChangedByUser;
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

            mText_ChangedByUser(null, null);
        }

        #endregion Объект

        #region - Поведение

        #region Кнопки управления

        /// <summary>
        /// Выполняется при выборе кнопки 'Правка'
        /// </summary>
        public void __mPressButtonEdit()
        {
            _cButtonEdit.ShowDropDown();

            return;
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Шрифт'
        /// </summary>
        public void __mPressButtonFont()
        {
            _cButtonFont.ShowDropDown();

            return;
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Текст'
        /// </summary>
        public void __mPressButtonText()
        {
            _cButtonText.ShowDropDown();
        }

        #endregion Кнопки управления

        /// <summary>
        /// Выполняется при открытии меню кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonDropDownOpened(object sender, EventArgs e)
        {
            _fDropDownOpened = true;
        }

        private void mText_ChangedByUser(object sender, EventArgs e)
        {
            _cLabel.Text = elmApplication.__oTunes.__mTranslate("Введено")
                + " " + _cText.Text.Length.ToString()
                + " " + elmApplication.__oTunes.__mTranslate("из")
                + " " + fTextSizeMax.ToString()
                + " " + elmApplication.__oTunes.__mTranslate("символов");
        }

        #region Шрифт

        /// <summary>
        /// Выполняется при выборе меню 'Шрифт / Полужирный'
        /// </summary>
        private void mMenuFontBold_Click(object sender, EventArgs e)
        {
            FontStyle newFontStyle = FontStyle.Regular;
            Font currentFont = _cText.SelectionFont;

            if (_cText.SelectionFont != null)
            {
                if (_cText.SelectionFont.Bold == false)
                {
                    newFontStyle = newFontStyle | FontStyle.Bold;
                }
                if (_cText.SelectionFont.Italic == true)
                {
                    newFontStyle = newFontStyle | FontStyle.Italic;
                }
                if (_cText.SelectionFont.Strikeout == true)
                {
                    newFontStyle = newFontStyle | FontStyle.Strikeout;
                }
                if (_cText.SelectionFont.Underline == true)
                {
                    newFontStyle = newFontStyle | FontStyle.Underline;
                }
            }

            _cText.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
        }
        /// <summary>
        /// Выполняется при выборе меню 'Шрифт / Курсив'
        /// </summary>
        private void mMenuFontItalic_Click(object sender, EventArgs e)
        {
            FontStyle newFontStyle = FontStyle.Regular;
            Font currentFont = _cText.SelectionFont;

            if (_cText.SelectionFont != null)
            {
                if (_cText.SelectionFont.Bold == true)
                {
                    newFontStyle = newFontStyle | FontStyle.Bold;
                }
                if (_cText.SelectionFont.Italic == false)
                {
                    newFontStyle = newFontStyle | FontStyle.Italic;
                }
                if (_cText.SelectionFont.Strikeout == true)
                {
                    newFontStyle = newFontStyle | FontStyle.Strikeout;
                }
                if (_cText.SelectionFont.Underline == true)
                {
                    newFontStyle = newFontStyle | FontStyle.Underline;
                }
            }

            _cText.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
        }
        /// <summary>
        /// Выполняется при выборе меню 'Шрифт / Подчеркнутый'
        /// </summary>
        private void mMenuFontUnderline_Click(object sender, EventArgs e)
        {
            FontStyle newFontStyle = FontStyle.Regular;
            Font currentFont = _cText.SelectionFont;

            if (_cText.SelectionFont != null)
            {
                if (_cText.SelectionFont.Bold == true)
                {
                    newFontStyle = newFontStyle | FontStyle.Bold;
                }
                if (_cText.SelectionFont.Italic == true)
                {
                    newFontStyle = newFontStyle | FontStyle.Italic;
                }
                if (_cText.SelectionFont.Strikeout == true)
                {
                    newFontStyle = newFontStyle | FontStyle.Strikeout;
                }
                if (_cText.SelectionFont.Underline == false)
                {
                    newFontStyle = newFontStyle | FontStyle.Underline;
                }
            }

            _cText.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
        }
        /// <summary>
        /// Выполняется при выборе меню 'Шрифт / Зачеркнутый'
        /// </summary>
        private void mMenuFontStrikethroungh_Click(object sender, EventArgs e)
        {
            FontStyle newFontStyle = FontStyle.Regular;
            Font currentFont = _cText.SelectionFont;

            if (_cText.SelectionFont != null)
            {
                if (_cText.SelectionFont.Bold == true)
                {
                    newFontStyle = newFontStyle | FontStyle.Bold;
                }
                if (_cText.SelectionFont.Italic == true)
                {
                    newFontStyle = newFontStyle | FontStyle.Italic;
                }
                if (_cText.SelectionFont.Strikeout == false)
                {
                    newFontStyle = newFontStyle | FontStyle.Strikeout;
                }
                if (_cText.SelectionFont.Underline == true)
                {
                    newFontStyle = newFontStyle | FontStyle.Underline;
                }
            }

            _cText.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
        }

        /// <summary>
        /// Выполняется при выборе меню 'Шрифт / Увеличить'
        /// </summary>
        private void mMenuFontDecrease_Click(object sender, EventArgs e)
        {
            Font currentFont = _cText.SelectionFont;
            _cText.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size + 2);
        }
        /// <summary>
        /// Выполняется при выборе меню 'Шрифт / Уменьшить'
        /// </summary>
        private void mMenuFontIncrease_Click(object sender, EventArgs e)
        {
            Font currentFont = _cText.SelectionFont;
            _cText.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size - 2);
        }

        /// <summary>
        /// Выполняется при выборе меню 'Шрифт / Цвет фона'
        /// </summary>
        private void mMenuFontBackColor_Click(object sender, EventArgs e)
        {
            ColorDialog vColorDialog = new ColorDialog();
            vColorDialog.AllowFullOpen = false;
            vColorDialog.FullOpen = true;
            vColorDialog.Color = _cText.SelectionBackColor;

            if (vColorDialog.ShowDialog() == DialogResult.Cancel)
                return;

            _cText.SelectionBackColor = vColorDialog.Color;
        }
        /// <summary>
        /// Выполняется при выборе меню 'Шрифт / Цвет шрифта'
        /// </summary>
        private void mMenuFontColor_Click(object sender, EventArgs e)
        {
            ColorDialog vColorDialog = new ColorDialog();
            vColorDialog.AllowFullOpen = false;
            vColorDialog.FullOpen = true;
            vColorDialog.Color = _cText.SelectionColor;

            if (vColorDialog.ShowDialog() == DialogResult.Cancel)
                return;

            _cText.SelectionColor = vColorDialog.Color;
        }

        #endregion Шрифт

        #region Текст

        /// <summary>
        /// Выполняется при выборе меню 'Текст / Привязка по центру'
        /// </summary>
        private void mMenuTextAlignCenter_Click(object sender, EventArgs e)
        {
            _cText.SelectionAlignment = HorizontalAlignment.Center;
        }
        /// <summary>
        /// Выполняется при выборе меню 'Текст / Привязка по левому краю'
        /// </summary>
        private void mMenuTextAlignLeft_Click(object sender, EventArgs e)
        {
            _cText.SelectionAlignment = HorizontalAlignment.Left;
        }
        /// <summary>
        /// Выполняется при выборе меню 'Текст / Привязка по правому краю'
        /// </summary>
        private void mMenuTextAlignRight_Click(object sender, EventArgs e)
        {
            _cText.SelectionAlignment = HorizontalAlignment.Right;
        }
        /// <summary>
        /// Выполняется при выборе меню 'Текст / Привязка по ширине'
        /// </summary>
        private void mMenuTextAlignJustity_Click(object sender, EventArgs e)
        {
            //_cText.SelectionAlignment = HorizontalAlignment.Justity;
        }

        private void mMenuTextListBullets_Click(object sender, EventArgs e)
        {
        }

        private void mMenuTextListNumbers_Click(object sender, EventArgs e)
        {
        }



        #endregion Текст

        #endregion Поведение

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Разделитель между полем ввода и надписью о количестве записей
        /// </summary>
        protected elmComponentSplitter _cSplitter = new elmComponentSplitter();
        /// <summary>
        /// Надпись о заполнении данных
        /// </summary>
        protected elmComponentLabel _cLabel = new elmComponentLabel();
        /// <summary>
        /// Элемент для правки текста
        /// </summary>
        protected elmComponentText _cText = new elmComponentText();

        protected elmComponentToolbarButtonMenu _cButtonFont = new elmComponentToolbarButtonMenu();
        protected elmComponentMenuItem _cMenuFontBold = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuFontItalic = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuFontUnderline = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuFontStrikethroungh = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuFontIncrease = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuFontDecrease = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuFontColor = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuFontBackColor = new elmComponentMenuItem();

        protected elmComponentToolbarButtonMenu _cButtonEdit = new elmComponentToolbarButtonMenu();
        protected elmComponentMenuItem _cMenuEditCut = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuEditCopy = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuEditPaste = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuEditUndo = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuEditRedo = new elmComponentMenuItem();

        protected elmComponentToolbarButtonMenu _cButtonText = new elmComponentToolbarButtonMenu();
        protected elmComponentMenuItem _cMenuTextAlignLeft = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuTextAlignCenter = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuTextAlignRight = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuTextAlignJustity = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuTextListBullets = new elmComponentMenuItem();
        protected elmComponentMenuItem _cMenuTextListNumbers = new elmComponentMenuItem();

        #endregion Компоненты

        #region - Служебные

        /// <summary>
        /// Максимальный размер вводимого текста
        /// </summary>
        private int fTextSizeMax = 100;

        #endregion Служебные

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        public string __fText_
        {
            get { return _cText.Text; }
            set { _cText.Text = value; }
        }
        /// <summary>
        /// Максимальный размер вводимого текста
        /// </summary>
        public int __fTextSizeMax_
        {
            get { return fTextSizeMax; }
            set { fTextSizeMax = value; }
        }

        #endregion СВОЙСТВА
    }
}
