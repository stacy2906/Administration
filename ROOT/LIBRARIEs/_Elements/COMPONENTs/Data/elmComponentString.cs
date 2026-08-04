using nlData;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentString.cs
    /// </summary>	
    /// <remarks>Класс-компонент для правки строчных данных
    /// </remarks>
    public class elmComponentString : elmTextBox
    {
        #region = БИБЛИОТЕКИ

        [DllImport("user32")]
        private static extern bool HideCaret(IntPtr hWnd);
        [DllImport("user32")]
        private static extern bool ShowCaret(IntPtr hWnd);

        #endregion БИБЛИОТЕКИ

        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public elmComponentString()
        {
            _fBehavior = new AlphanumericBehavior(this);
        }
        /// <summary>
        /// Конструктор получающий список запрещенных символов
        /// </summary>
        /// <param name="pChars">Набор запрещенных символов</param>
        public elmComponentString(char[] pChars)
        {
            _fBehavior = new AlphanumericBehavior(this, pChars);
        }
        
        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение
        
        /// <summary>
        /// Выполняется при получении фокуса элементом управления
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            if (__eChangedByUserBefore != null)
                __eChangedByUserBefore(this, e);

            return;
        }
        /// <summary>
        /// Выполняется при нажатии клавиши на клавиатуре, когда элемент управления находиться в фокусе
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
        /// Выполняется при отпускании клавиши на клавиатуре, когда элемент управления находиться в фокусе
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            fKeyPressNow = false;

            return;
        }
        /// <summary>
        /// Выполняется при потери фокуса элементом управления
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (__eChangedByUserAfter != null)
                __eChangedByUserAfter(this, e);

            return;
        }
        /// <summary>
        /// Выполняется при изменении данных в элементе управления
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
                if (__eChangedByUserAfter != null)
                    __eChangedByUserAfter(this, e);
            }
            if (__eChanged != null)
                __eChanged(this, e);

            base.OnTextChanged(e);

            return;
        }
        /// <summary>
        /// Выполняется при проверке ввода данных в элемент управления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e); // Всегда выполнять в начале метода!

            if (__fFillType_ == FILLTYPES.Necessarily)
            {
                if (Text.Length == 0)
                {
                    (FindForm() as elmForm).__mBaloonMessage(this, elmApplication.__oTunes.__mTranslate("Поле должно быть обязательно заполненным"));
                    e.Cancel = true;
                }
            }

            return;
        }

        #endregion Поведение

        #region - Процедуры

        #region Закрытые

        /// <summary>
        /// Получение пути и имени файла класса
        /// </summary>
        private string mFilePath([CallerFilePath] string filePath = "")
        {
            return filePath;
        }
        /// <summary>
        /// Получение значения номера строки в текущей процедуре
        /// </summary>
        private int mLine(string message = "", [CallerLineNumber] int line = 0)
        {
            return line;
        }
        /// <summary>
        /// Получение названия текущей процедуры
        /// </summary>
        private string mProcedure(string message, [CallerMemberName] string member = "")
        {
            return member;
        }

        #endregion Закрытые

        /// <summary>
        /// Скрытие каретки
        /// </summary>
        public void __mCaretHide()
        {
            HideCaret(this.Handle);
        }
        /// <summary>
        /// Отображение каретки
        /// </summary>
        public void __mCaretShow()
        {
            ShowCaret(this.Handle);
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Закрытые

        /// <summary>
        /// Состояние - нажата клавиша клавиатуры 
        /// </summary>
        private bool fKeyPressNow = false;

        #endregion Закрытые 

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        #region - Скрытые

        /// <summary>
        /// Путь к файлу текущего класса
        /// </summary>
        protected string _fCurrentFilePath_
        {
            get { return mFilePath(); }
        }
        /// <summary>
        /// Текущая процедура класса
        /// </summary>
        protected string _fCurrentProcedure_
        {
            get { return mProcedure(""); }
        }
        /// <summary>
        /// Номер строки в файле класса
        /// </summary>
        protected int _fCurrentLine_
        {
            get { return mLine(""); }
        }

        #endregion Скрытые

        /// <summary>
        /// Получает объект Behavior, связанный с этим классом
        /// </summary>
        [Browsable(false)]
        public AlphanumericBehavior __fBehavior_
        {
            get
            {
                return (AlphanumericBehavior)_fBehavior;
            }
        }
        /// <summary>
        /// Получает или задает массив символов, считающихся недопустимыми (запрещенными)
        /// </summary>
        public char[] __fCharsInvalid_
        {
            get
            {
                return __fBehavior_.__fInvalidChars_;
            }
            set
            {
                __fBehavior_.__fInvalidChars_ = value;
            }
        }
        /// <summary>
        /// Получает или задает режим 'Только чтение'
        /// </summary>
        public bool __fReadOnly_
        {
            get { return ReadOnly; }
            set
            {
                ReadOnly = value;
                if (ReadOnly == false)
                {
                    BackColor = elmApplication.__oInterface.__mColor(COLORS.DataBack);
                    TabStop = true;
                }
                else
                {
                    BackColor = elmApplication.__oInterface.__mColor(COLORS.DataBackDisabled);
                    TabStop = false;
                }

            }
        }        
        /// <summary>
        /// Значение
        /// </summary>
        public object __fValue_
        {
            get { return Text.Trim(); }
            set { Text = Convert.ToString(value).Trim(); }
        }
        /// <summary>
        /// Строчное значение
        /// </summary>
        public string __fValueToString_
        {
            get { return __fValue_.ToString(); }
            set { __fValue_ = value; }
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
        public event EventHandler __eChangedByUserAfter;
        /// <summary>
        /// Возникает перед изменением данных пользователем
        /// </summary>
        public event EventHandler __eChangedByUserBefore;
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
