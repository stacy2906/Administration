using nlApplication;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentFractional.cs
    /// </summary>	
    /// <remarks>Класс-Компонент для ввода десятичных чисел</remarks>
    public class elmComponentNumeric : elmTextBox
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public elmComponentNumeric()
        {
            _fBehavior = new NumericBehavior(this);
        }
        /// <summary>
        /// Конструктор с определением размеров значения 
        /// </summary>
        /// <param name="pPartInt">Количество символов в целой части значения. Если значене меньше 1, устанавливается равным 1.</param>
        /// <param name="mPartFractional">Количество сиволов в дробной части значения. Если значение меньше 0, оно устанавливается равным 0.</param>
        public elmComponentNumeric(int pPartInt, int mPartFractional)
        {
            _fBehavior = new NumericBehavior(this, pPartInt, mPartFractional);
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса NumericTextBox, явно присваивая ему значение поля Behavior
        /// </summary>
        /// <param name="behavior">Объект <see cref="NumericBehavior" />, связывающий текстовое поле</param>
        public elmComponentNumeric(NumericBehavior behavior) : base(behavior)
        {
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Настройка компонента

            TextAlign = HorizontalAlignment.Right;

            #endregion Настройка компонента

            ResumeLayout(false);

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();

            if (__fSymbolsIntegerCount_ != 0 & __fSymbolsInGroupCount_ != 0)
                __fSymbolsCount_ = __fSymbolsFractionalCount_ + __fSymbolsIntegerCount_ / __fSymbolsInGroupCount_ + __fSymbolsIntegerCount_ + 1 + (__fNegative_ == true? 1: 0) ;
            else
                __fSymbolsCount_ = __fSymbolsFractionalCount_ + __fSymbolsIntegerCount_ + 1 + (__fNegative_ == true ? 1 : 0);
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

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
        /// Выполняется при нажатии на клавиши
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            /// Замена точки на запятую
            if (e.KeyChar == ',' | e.KeyChar == '.')
            {
                
                //if (e.KeyChar != __fBehavior_.__fSymbolSeparator_)
                //{
                    e.KeyChar = __fBehavior_.__fSymbolSeparator_;
                //}
            }

            base.OnKeyPress(e);
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
        /// Выполняется при проверке введенных данных
        /// </summary>
        /// <param name="e"></param>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e); // Всегда выполнять в начале метода!
            /// Данные начинаются с десятичного разделителя
            if (Text.Trim().StartsWith(__fSymbolSeparator_.ToString()) == true)
            {
                Text = "0" + Text.Trim();
            }
            /// Нет данных - пустая строка
            if (__fValue_.ToString().Length == 0)
            {
                Text = "0" + __fSymbolSeparator_ + new string('0', __fSymbolsFractionalCount_);
            }
            /// Добавление нулей к десятичному разделителю
            if (Convert.ToDecimal(__fValue_) == 0)
            {
                Text = "0" + __fSymbolSeparator_ + new string('0', __fSymbolsFractionalCount_);
            }
            else
            {
                string vPartFractional = appTypeString.__mWordNumber(Text, 1, __fSymbolSeparator_);
                if (vPartFractional.Contains(__fSymbolSeparator_.ToString()) == false)
                {
                    Text = Text + __fSymbolSeparator_.ToString();
                }
                if (vPartFractional.Length < __fSymbolsFractionalCount_)
                {
                    Text = Text + new String('0', __fSymbolsFractionalCount_ - vPartFractional.Length);
                }
            }
            /// Проверка ввода значений отличных от нуля
            if (__fFillType_ == FILLTYPES.Necessarily)
            {
                if (Convert.ToDecimal(__fValue_) == 0)
                {
                    (FindForm() as elmForm).__mBaloonMessage(this, elmApplication.__oTunes.__mTranslate("Поле должно быть обязательно заполненным"));
                    e.Cancel = true;
                }
            }
            /// Проверка на превышение максимального значения
            if (Convert.ToDecimal(__fValue_) > Convert.ToDecimal(__fValueMaximum_))
            {
                (FindForm() as elmForm).__mBaloonMessage(this, elmApplication.__oTunes.__mTranslate("Максимальное значение = {0}", __fValueMaximum_));
                e.Cancel = true;
            }
            /// Проверка на занижение минимального значения
            if (Convert.ToDouble(__fValue_) < Convert.ToDouble(__fValueMinimum_))
            {
                (FindForm() as elmForm).__mBaloonMessage(this, elmApplication.__oTunes.__mTranslate("Минимальное значение = {0}", __fValueMinimum_));
                e.Cancel = true;
            }
        }

        #endregion Поведение

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Скрытые

        /// <summary>
        /// Состояние - нажата клавиша клавиатуры 
        /// </summary>
        private bool fKeyPressNow = false;

        #endregion Скрытые

        #region - Закрытые

        /// <summary>
        /// Разрешить нулевое значение
        /// </summary>
        private FILLTYPES fFillType = FILLTYPES.None;

        #endregion Закрытые

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Получает объект Behavior, связанный с этим классом 
        /// </summary>
        public NumericBehavior __fBehavior_
        {
            get
            {
                return (NumericBehavior)_fBehavior;
            }
        }
        ///// <summary>
        ///// Получает или задает текст текстового поля в виде числа с плавающей запятой (типа double).
        ///// </summary>
        ///// <remarks>
        ///// Если текст пустой или не может быть преобразован в число с плавающей запятой, возвращается 0</remarks>
        //public double __fDouble_
        //{
        //    get
        //    {
        //        try
        //        {
        //            return Convert.ToDouble(__fBehavior_.NumericText);
        //        }
        //        catch
        //        {
        //            return 0;
        //        }
        //    }
        //    set
        //    {
        //        Text = value.ToString();
        //    }
        //}
        ///// <summary>
        ///// Получает или задает текст текстового поля в виде целого числа
        ///// </summary>
        ///// <remarks>
        /////   If the text empty or cannot be converted to an int, a 0 is returned. </remarks>
        //public int Int
        //{
        //    get
        //    {
        //        try
        //        {
        //            return Convert.ToInt32(__fBehavior_.NumericText);
        //        }
        //        catch
        //        {
        //            return 0;
        //        }
        //    }
        //    set
        //    {
        //        Text = value.ToString();
        //    }
        //}
        ///// <summary>
        ///// Получает или задает текст текстового поля в виде длинного целого числа
        ///// </summary>
        ///// <remarks>
        /////   If the text empty or cannot be converted to an long, a 0 is returned. </remarks>
        //public long Long
        //{
        //    get
        //    {
        //        try
        //        {
        //            return Convert.ToInt64(__fBehavior_.NumericText);
        //        }
        //        catch
        //        {
        //            return 0;
        //        }
        //    }
        //    set
        //    {
        //        Text = value.ToString();
        //    }
        //}
        ///// <summary>
        ///// Извлекает значение из текстового поля без каких-либо нечисловых символов
        ///// </summary>
        ///// <remarks>
        /////   This property delegates to <see cref="NumericBehavior.NumericText">NumericBehavior.NumericText</see>. </remarks>
        //public string NumericText
        //{
        //    get
        //    {
        //        return __fBehavior_.NumericText;
        //    }
        //}
   

        /// <summary>
        /// Получает или устанавливает разрешение ввода отрицательных значений
        /// </summary>
        public bool __fNegative_
        {
            get
            {
                return __fBehavior_.__fNegative_;
            }
            set
            {
                __fBehavior_.__fNegative_ = value;
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
        /// Символ валюты
        /// </summary>
        public String __fSymbolCurrency_
        {
            get
            {
                return __fBehavior_.__fSymbolCurrency_;
            }
            set
            {
                __fBehavior_.__fSymbolCurrency_ = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает символ разделителя групп
        /// </summary>
        /// <remarks>
        ///   This property delegates to <see cref="NumericBehavior.__fSymbolGroup_">NumericBehavior.GroupSeparator</see>. </remarks>
        public char __fSymbolGroup_
        {
            get
            {
                return __fBehavior_.__fSymbolGroup_;
            }
            set
            {
                __fBehavior_.__fSymbolGroup_ = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает символ отрицательного знака
        /// </summary>
        /// <remarks>
        public char __fSymbolNegative_
        {
            get
            {
                return __fBehavior_.__fSymbolNegative_;
            }
            set
            {
                __fBehavior_.__fSymbolNegative_ = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает символ десятичного разделителя
        /// </summary>
        /// <remarks>
        public char __fSymbolSeparator_
        {
            get
            {
                return __fBehavior_.__fSymbolSeparator_;
            }
            set
            {
                __fBehavior_.__fSymbolSeparator_ = value;
            }
        }
        /// <summary>
        /// Получает или задает количество cbvdjkjd, которые нужно разместить в каждой группе слева от десятичной точки
        /// </summary>
        public int __fSymbolsInGroupCount_
        {
            get
            {
                return __fBehavior_.__fSymbolsInGroupCount_;
            }
            set
            {
                __fBehavior_.__fSymbolsInGroupCount_ = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает количество символов в целой части
        /// </summary>
        public int __fSymbolsIntegerCount_
        {
            get
            {
                return __fBehavior_.__fSymbolsIntCount_;
            }
            set
            {
                __fBehavior_.__fSymbolsIntCount_ = value;
            }
        }
        /// <summary>
        /// Получает иди устанавливает количество символов в дробной части
        /// </summary>
        public int __fSymbolsFractionalCount_
        {
            get
            {
                return __fBehavior_.__fSymbolsFractionalCount_;
            }
            set
            {
                __fBehavior_.__fSymbolsFractionalCount_ = value;
            }
        }

        /// <summary>
        /// Получает или устанавливает значение компонента
        /// </summary>
        public object __fValue_
        {
            get { return Text; }
            set
            {
                if (value.Equals(DBNull.Value))
                {
                    if (value.Equals(0))
                    {
                        this.Text = Convert.ToString(0);
                        if (__eChangedByProgram != null)
                        {
                            __eChangedByProgram(this, new EventArgs());
                        }
                        return;
                    }
                }

                if (!value.Equals(Text))
                {
                    this.Text = Convert.ToString(value);
                    if (__eChangedByProgram != null)
                    {
                        __eChangedByProgram(this, new EventArgs());
                    }
                }
            }
        }
        /// <summary>
        /// Получает или устанавливает максимальное значение
        /// </summary>
        public double __fValueMaximum_
        {
            get
            {
                return __fBehavior_.__fValueMaximum_;
            }
            set
            {
                __fBehavior_.__fValueMaximum_ = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает минимальное значение
        /// </summary>
        public double __fValueMinimum_
        {
            get
            {
                return __fBehavior_.__fValueMinimum_;
            }
            set
            {
                __fBehavior_.__fValueMinimum_ = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает десятичное значение компонента
        /// </summary>
        public decimal __fValueToDecimal
        {
            get
            {
                return Convert.ToDecimal(__fBehavior_.NumericText);
            }
        }
        /// <summary>
        /// Получает или устанавливает строчный эквивалент значение
        /// </summary>
        /// <remarks>Строка формируется без нечисловых символов, с точкой в ​​качестве десятичной точки и знаком минус для отрицательного знака</remarks>
        public string __fValueToString_
        {
            get
            {
                return __fBehavior_.__fValueToString_;
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
