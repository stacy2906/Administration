using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentText.cs
    /// </summary>
    /// <remarks>Класс-компонент для правки текстовых данных</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.01.14 08-42</version> // Дата-время последней корректировки
    public class elmComponentText : RichTextBox
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public elmComponentText()
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

            #endregion Настройка компонента

            ResumeLayout(false);

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected virtual void _mObjectPresentation()
        {
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется после создания объекта
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _mObjectPresentation();

            return;
        }
        /// <summary>
        /// Выполняется при нажатии клавиши на клавиатуре
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (__eKeyDown != null)
                __eKeyDown(this, e);

            base.OnKeyDown(e);

            fKeyPressNow = true;

            return;
        }
        /// <summary>
        /// Выполняется при отпускании клавиши на клавиатуре
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            fKeyPressNow = false;

            return;
        }
        /// <summary>
        /// Выполняется при изменении данных в компоненте
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            if (fKeyPressNow == false)
            {
                if (__eChangedByProgram != null)
                    __eChangedByProgram(this, e);
            }
            else
            {
                if (__eChangedByUser != null)
                    __eChangedByUser(this, e);
            }
            if (__eChanged != null)
                __eChanged(this, e);

            base.OnTextChanged(e);

            return;
        }
        /// <summary>
        /// Выполняется при проверке ввода данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnValidating(CancelEventArgs e)
        {
            if (__fFillType_ == FILLTYPES.Necessarily)
            {
                if (Text.Length == 0)
                {
                    (FindForm() as elmForm).__mBaloonMessage(this, elmApplication.__oTunes.__mTranslate("Поле должно быть обязательно заполненным"));
                    e.Cancel = true;
                }
            }

            base.OnValidating(e);

            return;
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Добавление текста в компонент
        /// </summary>
        /// <param name="pText">Добавляемый текст</param>
        public void __mTextAdd(string pText)
        {
            AppendText(pText + CRLF);
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Константы

        /// <summary>
        /// Первод каретки
        /// </summary>
        private const string CRLF = "\r\n";

        #endregion Константы

        #region - Скрытые

        /// <summary>
        /// Полное название класса
        /// </summary>
        protected string _fClassNameFull = "";

        #endregion Скрытые

        #region - Служебные

        /// <summary>
        /// Вид ввода данных
        /// </summary>
        private FILLTYPES fFillType = FILLTYPES.None;
        /// <summary>
        /// Состояние - нажата клавиша клавиатуры 
        /// </summary>
        private bool fKeyPressNow = false;

        #endregion Служебные 

        #endregion ПОЛЯ    

        #region = СВОЙСТВА

        /// <summary>
        ///  Обязательность заполнения
        /// </summary>
        public FILLTYPES __fFillType_
        {
            get { return fFillType; }
            set
            {
                fFillType = value;
                if (fFillType == FILLTYPES.None)
                    BackColor = elmApplication.__oInterface.__mColor(COLORS.DataBack);
                else
                    BackColor = elmApplication.__oInterface.__mColor(COLORS.DataBackNecessarily);
            }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при изменении данных
        /// </summary>
        public event EventHandler __eChanged;
        /// <summary>
        /// Возникает при изменении данных пользователем
        /// </summary>
        public event EventHandler __eChangedByUser;
        /// <summary>
        /// Возникает при изменении данных программой
        /// </summary>
        public event EventHandler __eChangedByProgram;
        /// <summary>
        /// Возникает при нажатии клавиши
        /// </summary>
        public event EventHandler __eKeyDown;

        #endregion СОБЫТИЯ

    }
}
