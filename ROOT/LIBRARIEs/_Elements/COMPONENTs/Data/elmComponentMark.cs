using System;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentMark.cs
    /// </summary>
    /// <remarks>Класс-компонент для правки логических данных</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.01.14 08-42</version> // Дата-время последней корректировки
    public class elmComponentMark : CheckBox
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public elmComponentMark()
        {
            _mObjectAssembly();
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        {
            SuspendLayout();

            #region /// Настройка компонента

            AutoSize = true;
            BackColor = Color.Transparent;
            ForeColor = elmApplication.__oInterface.__fColorText;

            #endregion Настройка компонента

            ResumeLayout();

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется при отпускании правой кнопки мыши
        /// </summary>
        /// <param name="pEvent"></param>
        protected override void OnMouseUp(MouseEventArgs pEvent)
        {
            if (fCaptionClickable == true & Enabled == true)
            {
                if (pEvent.Button == MouseButtons.Left)
                {
                    if (__eClickLeft != null)
                        __eClickLeft(this, new EventArgs());
                }
                if (pEvent.Button == MouseButtons.Right)
                {
                    if (__eClickRight != null)
                        __eClickRight(this, new EventArgs());
                }
            }

            base.OnMouseUp(pEvent);
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
        }

        protected override void OnCheckStateChanged(EventArgs e)
        {
            base.OnCheckStateChanged(e);
        }

        #endregion Поведение

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Внутренние

        /// <summary>
        /// Полное название класса
        /// </summary>
        protected string _fClassNameFull = "";

        #endregion Внутренние

        #region - Служебные

        /// <summary>
        /// Разрешение обрабатывать клики мыши на надписи
        /// </summary>
        private bool fCaptionClickable = true;
        /// <summary>
        /// Вид надписи
        /// </summary>
        private LABELTYPES fLabelType = LABELTYPES.Normal;
        /// <summary>
        /// Вид надписи определенный программой
        /// </summary>
        private readonly LABELTYPES fLabelTypeOnLoad = LABELTYPES.Normal;
        /// <summary>
        /// Строка заголовка без перевода
        /// </summary>
        private string fTextWithOutTranslate = "";

        #endregion Служебные

        #endregion ПОЛЯ  

        #region = СВОЙСТВА

        /// <summary>
        /// Текст заголовка
        /// </summary>
        /// <remarks>Отображаемый текст переводиться на язык интерфейса. Возвращается не переведенный текст</remarks>
        public string __fCaption_
        {
            get { return fTextWithOutTranslate; }
            set
            {
                fTextWithOutTranslate = value.Trim();
                Text = elmApplication.__oTunes.__mTranslate(fTextWithOutTranslate);
            }
        }
        /// <summary>
        /// Разрешение обрабатывать клики мыши на надписи
        /// </summary>
        public bool __fCaptionClickable
        {
            get { return fCaptionClickable; }
            set { fCaptionClickable = value; }
        }
        /// <summary>
        /// Доступность компонента
        /// </summary>
        /// <remarks>Определяет доступность компонента и работу Lustar функционала</remarks>
        public bool __fEnabled_
        {
            get { return Enabled; }
            set
            {
                Enabled = value;
                fCaptionClickable = Enabled;
                if (value == false)
                    __fLabelType_ = LABELTYPES.Normal;
                else
                    __fLabelType_ = fLabelTypeOnLoad;
            }
        }
        /// <summary>
        /// Вид надписи
        /// </summary>
        public LABELTYPES __fLabelType_
        {
            get { return fLabelType; }
            set
            {
                Cursor = Cursors.Default;
                fLabelType = value;
                switch (fLabelType)
                {
                    case LABELTYPES.Normal:
                        Font = elmApplication.__oInterface.__mFont(FONTS.Text);
                        ForeColor = elmApplication.__oInterface.__mColor(COLORS.Text);
                        break;
                    case LABELTYPES.Button:
                        Font = elmApplication.__oInterface.__mFont(FONTS.TextButton);
                        ForeColor = elmApplication.__oInterface.__mColor(COLORS.TextButton);
                        Cursor = Cursors.Hand;
                        break;
                    case LABELTYPES.Title:
                        Font = elmApplication.__oInterface.__mFont(FONTS.TextTitle);
                        ForeColor = elmApplication.__oInterface.__mColor(COLORS.TextTitle);
                        break;
                }
            }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при изменении данных
        /// </summary>
        public event EventHandler __eChanged;
        /// <summary>
        /// Возникает при изменении данных программой
        /// </summary>
        public event EventHandler __eChangedByProgram;
        /// <summary>
        /// Возникает при изменении данных пользователем
        /// </summary>
        public event EventHandler __eChangedByUser;
        /// <summary>
        /// Возникает при клике левой кнопки мыши по компоненту
        /// </summary>
        public event EventHandler __eClickLeft;
        /// <summary>
        /// Возникает при клике правой кнопки мыши по компоненту
        /// </summary>
        public event EventHandler __eClickRight;

        #endregion СОБЫТИЯ    

    }
}
