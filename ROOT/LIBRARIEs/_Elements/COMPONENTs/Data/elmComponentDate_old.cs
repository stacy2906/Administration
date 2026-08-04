using System;
using System.Diagnostics;

namespace nlElements
{
    /// <summary>
    ///   TextBox class which supports the <see cref="DateBehavior">Date</see> behavior. 
    /// </summary>	
    public class elmComponentDate : elmTextBox
    {
        #region = ПЕРЕЧИСЛЕНИЯ

        /// <summary>
        /// Внутренние значения, которые добавляются/удаляются из свойства <see cref="elmTypeBehavior.Flags" /> другими свойствами этого класса
        /// </summary>
        [Flags]
        protected enum FLAG
        {
            /// <summary> 
            /// День недели отображается перед месяцем
            /// </summary>
            DayBeforeMonth = 0x00010000,
        };

        #endregion ПЕРЕЧИСЛЕНИЯ

        #region = ДИЗАЙНЕРЫ

        /// <summary>
        ///   Initializes a new instance of the DateTextBox class by assigning its Behavior field
        ///   to an instance of <see cref="DateBehavior" />. </summary>
        public elmComponentDate()
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

            Height = 23;
            __fSymbolsCount_ = 10;
            _fSelection = new elmTypeSelection(this);

            #endregion Настройка компонента

            ResumeLayout(false);

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Корректирует день (до максимального значения), если значение недействительно
        /// </summary>
        /// <returns>
        /// Если дата корректируется, возвращаемое значение равно true; в противном случае — false
        /// </returns>
        protected bool __mAdjustMaxDay()
        {
            int day = __fDay_;
            if (day != 0 && !__mIsValidDay(day))
            {
                __fDay_ = GetMaxDay();
                return true;
            }

            return false;   // nothing had to be adjusted
        }
        /// <summary>
        /// Извлекает начальную позицию дня из текстового поля
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это начальная позиция дня
        /// </returns>
        /// <remarks>
        /// Это зависит от того, указан ли день до или после месяца
        /// </remarks>
        protected int __mGetDayStartPosition()
        {
            return __fShowDayBeforeMonth_ ? 0 : 3;
        }
        /// <summary>
        /// Преобразует значения года, месяца и дня в строку в соответствии с заданным форматом (мм/дд/гггг или дд/мм/гггг)
        /// </summary>
        /// <param name="year">Годовое значение</param>
        /// <param name="month">Значение за месяц</param>
        /// <param name="day">Дневная стоимость</param>
        /// <returns>
        /// Возвращаемое значение представляет собой отформатированное значение даты.
        /// </returns>
        public string __mGetFormattedDate(int year, int month, int day)
        {
            if (__fShowDayBeforeMonth_)
                return String.Format("{0,2:00}{1}{2,2:00}{3}{4,4:0000}", day, fSeparator, month, fSeparator, year);
            return String.Format("{0,2:00}{1}{2,2:00}{3}{4,4:0000}", month, fSeparator, day, fSeparator, year);
        }
        /// <summary>
        /// Извлекает максимальное значение за месяц на основе года и допустимого диапазона
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это максимальное значение за месяц
        /// </returns>
        protected int __mGetMaxMonth()
        {
            if (__mGetValidYear() == fValueMax.Year)
                return fValueMax.Month;

            return 12;
        }
        /// <summary>
        /// Извлекает минимальное значение за месяц на основе года и допустимого диапазона
        /// </summary>
        /// <returns>
        /// Возвратная стоимость — это минимальное значение за месяц
        /// </returns>
        protected int __mGetMinMonth()
        {
            if (__mGetValidYear() == fValueMin.Year)
                return fValueMin.Month;
            return 1;
        }
        /// <summary>
        /// Получает начальную позицию месяца в текстовом поле
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это начальная позиция месяца
        /// </returns>
        /// <remarks>
        /// Это зависит от того, указан ли месяц до или после дня недели
        /// </remarks>
        protected int __mGetMonthStartPosition()
        {
            return __fShowDayBeforeMonth_ ? 3 : 0;
        }
        /// <summary>
        /// Извлекает месяц из текстового поля как допустимое значение
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это допустимое значение за месяц (1–12)
        /// </returns>
        /// <remarks>
        /// Метод проверяет значение месяца в текстовом поле.
        /// Если оно находится в допустимом диапазоне, возвращает его.
        /// Если оно меньше минимально допустимого значения, возвращается минимальное значение.
        /// Если оно больше максимально допустимого значения, возвращается максимальное значение
        /// </remarks>
        protected int __mGetValidMonth()
        {
            int month = __fMonth_;

            // It it's outside the range, fix it
            if (month < __mGetMinMonth())
                month = __mGetMinMonth();
            else if (month > __mGetMaxMonth())
                month = __mGetMaxMonth();

            return month;
        }
        /// <summary>
        /// Извлекает год из текстового поля как допустимое значение
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это допустимое значение за год
        /// </returns>
        /// <remarks>
        /// Метод проверяет значение года в текстовом поле.
        /// Если оно находится в допустимом диапазоне, возвращает его.
        /// Если оно меньше минимально допустимого значения, возвращается минимальное значение.
        /// Если оно больше максимально допустимого значения, возвращается максимальное значение
        /// </remarks>
        protected int __mGetValidYear()
        {
            int year = __fYear_;
            if (year < fValueMin.Year)
            {
                year = DateTime.Today.Year;
                if (year < fValueMin.Year)
                    year = fValueMin.Year;
            }
            if (year > fValueMax.Year)
                year = fValueMax.Year;

            return year;
        }
        /// <summary>
        /// Проверяет, действителен ли день — находится ли он в допустимом диапазоне
        /// </summary>
        /// <param name="day">День проверки</param>
        /// <returns>
        /// Если дата попадает в допустимый диапазон, возвращается значение true; в противном случае — false
        /// </returns>
        protected bool __mIsValidDay(int day)
        {
            try
            {
                return __mIsWithinRange(new DateTime(__mGetValidYear(), __mGetValidMonth(), day));
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Проверяет, действителен ли год — находится ли он в допустимом диапазоне
        /// </summary>
        /// <param name="year">Год для проверки</param>
        /// <returns>
        /// Если год находится в допустимом диапазоне, возвращаемое значение равно true; в противном случае — false
        /// </returns>
        protected bool __mIsValidYear(int year)
        {
            return (year >= fValueMin.Year && year <= fValueMax.Year);
        }
        /// <summary>
        /// Проверяет, находится ли значение даты в допустимом диапазоне
        /// </summary>
        /// <param name="dt">Значение даты, которое нужно проверить</param>
        /// <returns>
        /// Если значение находится в допустимом диапазоне, возвращается значение true; в противном случае — false
        /// </returns>
        /// <remarks>
        /// Проверяется только дата; время игнорируется
        /// </remarks>
        public bool __mIsWithinRange(DateTime dt)
        {
            DateTime date = new DateTime(dt.Year, dt.Month, dt.Day);
            return (date >= fValueMin && date <= fValueMax);
        }
        /// <summary>
        /// Устанавливает месяц, день и год в текстовом поле 
        /// </summary>
        /// <param name="year">Год</param>
        /// <param name="month">Месяц</param>
        /// <param name="day">День</param>
        public void __mSetDate(int year, int month, int day)
        {
            __fValue_ = new DateTime(year, month, day);
        }
        /// <summary>
        /// Преобразует заданный текст в целое число
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это текст в виде целого числа или 0, если преобразование невозможно
        /// </returns>
        /// <remarks>
        /// Этот метод удобен для производных классов поведения, которым необходимо преобразовывать строку в целое число, не опасаясь возникновения исключения System.FormatException
        /// </remarks>
        protected int __mToInt(String text)
        {
            try
            {
                // Make it work like "atoi" -- ignore any trailing non-digit characters
                for (int i = 0, length = text.Length; i < length; i++)
                {
                    if (!Char.IsDigit(text[i]))
                        return Convert.ToInt32(text.Substring(0, i));
                }

                return Convert.ToInt32(text);
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// Преобразует целое число в двухзначную строку (00–99)
        /// </summary>
        /// <param name="value">Число для конвертации</param>
        /// <returns>
        /// Возвращаемое значение представляет собой отформатированную строку
        /// </returns>
        /// <remarks>
        /// Это удобный способ форматирования двузначных значений, таких как месяц и день
        /// </remarks>
        protected static string __mTwoDigits(int value)
        {
            return String.Format("{0,2:00}", value);
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        /// <summary>
        /// Максимально допустимое значение 
        /// </summary>
        private DateTime fValueMax = new DateTime(9998, 12, 31);
        /// <summary>
        /// Минимально допустимое значение
        /// </summary>
        private DateTime fValueMin = new DateTime(1900, 1, 1);
        /// <summary>
        /// Символ, используемый для разделения значений месяца, дня и года в дате
        /// </summary>
        private char fSeparator = '/';
        /// <summary> 
        /// Для этого поведения были включены соответствующие флаги
        /// </summary>
        protected int _fFlags;
        /// <summary> 
        /// Вспомогательный объект, используемый для управления выделением объекта TextBox
        /// </summary>
        protected elmTypeSelection _fSelection;

        #endregion ПОЛЯ

        /// <summary>
        /// Получает или устанавливает день
        /// </summary>
        public int __fDay_
        {
            //d get
            //{
            //    return Behavior.Day;
            //}
            //set
            //{
            //    Behavior.Day = value;
            //}
            get
            {
                string text = Text;

                int startPos = __mGetDayStartPosition();
                int slash = text.IndexOf(fSeparator);

                if (startPos != 0 && slash > 0)
                    startPos = slash + 1;

                if (text.Length >= startPos + 2)
                    return __mToInt(text.Substring(startPos, 2));
                return 0;
            }
            set
            {
                // Verify it's in range
                if (!__mIsValidDay(value))
                    throw new ArgumentOutOfRangeException();

                using (elmTypeSelection.Saver savedSelection = new elmTypeSelection.Saver(this))   // remember the current selection
                {
                    if (__fDay_ > 0)        // see if there's already a day
                        _fSelection.Set(__mGetDayStartPosition(), __mGetDayStartPosition() + 3);

                    _fSelection.Replace(__mTwoDigits(value) + fSeparator);    // set the day
                }
            }
        }
        /// <summary>
        /// Получает или устанавливает месяц
        /// </summary>
        public int __fMonth_
        {
            //get
            //{
            //    return Behavior.__fMonth_;
            //}
            //set
            //{
            //    Behavior.__fMonth_ = value;
            //}
            get
            {
                string text = this.Text;

                int startPos = __mGetMonthStartPosition();
                int slash = text.IndexOf(fSeparator);

                if (startPos != 0 && slash > 0)
                    startPos = slash + 1;

                if (text.Length >= startPos + 2)
                    return __mToInt(text.Substring(startPos, 2));
                return 0;
            }
            set
            {
                using (elmTypeSelection.Saver savedSelection = new elmTypeSelection.Saver(this))   // remember the current selection
                {
                    if (__fMonth_ > 0)      // see if there's already a month
                        _fSelection.Set(__mGetMonthStartPosition(), __mGetMonthStartPosition() + 3);

                    _fSelection.Replace(__mTwoDigits(value) + fSeparator);    // set the month

                    __mAdjustMaxDay(); // adjust the day if it's out of range

                    // Verify it's in range
                    if (!IsValidMonth(value))
                        throw new ArgumentOutOfRangeException();
                }
            }

        }
        /// <summary>
        /// Получает или устанавливает год
        /// </summary>
        public int __fYear_
        {
            //get
            //{
            //    return Behavior.Year;
            //}
            //set
            //{
            //    Behavior.Year = value;
            //}
            get
            {
                string text = this.Text;
                int length = text.Length;

                int slash = text.LastIndexOf(fSeparator);
                if (slash > 0 && slash < length - 1)
                    return __mToInt(text.Substring(slash + 1, Math.Min(4, length - slash - 1)));
                return 0;
            }
            set
            {
                // Verify it's in range
                if (!__mIsValidYear(value))
                    throw new ArgumentOutOfRangeException();

                using (elmTypeSelection.Saver savedSelection = new elmTypeSelection.Saver(this))   // remember the current selection
                {
                    if (__fYear_ > 0)       // see if there's already a year
                        _fSelection.Set(GetYearStartPosition(), GetYearStartPosition() + 4);

                    _fSelection.Replace(String.Format("{0,4:0000}", value));    // set the year

                    AdjustMaxMonthAndDay(); // adjust the month and/or day if they're out of range
                }
            }

        }

        /// <summary>
        /// Получает или задает месяц, день и год в текстовом поле, используя объект <see cref="DateTime" /></summary>
        /// <remarks>
        ///   This property delegates to <see cref="DateBehavior.__fValue_">DateBehavior.Value</see>. </remarks>
        /// <seealso cref="__fMonth_" />
        /// <seealso cref="__fDay_" />
        /// <seealso cref="__fYear_" />
        public object Value
        {
            get
            {
                return __fValue_;
            }
            set
            {
                __fValue_ = value;
            }
            //get
            //{
            //    try
            //    {
            //        return new DateTime(__fYear_, __fMonth_, __fDay_);
            //    }
            //    catch
            //    {
            //        return null;
            //    }
            //}
            //set
            //{
            //    DateTime dt = (DateTime)value;
            //    this.Text = __mGetFormattedDate(dt.Year, dt.Month, dt.Day);
            //}

        }

        /// <summary>
        /// Получает или устанавливает максимально допустимое значение
        /// </summary>
        public DateTime __fValueMaximum_
        {
            //get
            //{
            //    return Behavior.__fValueMax_;
            //}
            //set
            //{
            //    Behavior.__fValueMax_ = value;
            //}
            get
            {
                return fValueMax;
            }
            set
            {
                fValueMax = value;
                UpdateText();
            }

        }
        /// <summary>
        /// Получает или устанавливает минимально допустимое значение
        /// </summary>
        public DateTime __fValueMinimum_
        {
            //get
            //{
            //    return Behavior.__fValueMin_;
            //}
            //set
            //{
            //    Behavior.__fValueMin_ = value;
            //}
            get
            {
                return fValueMin;
            }
            set
            {
                if (value < new DateTime(1900, 1, 1))
                    throw new ArgumentOutOfRangeException("RangeMin", value, "Minimum value may not be older than January 1, 1900");

                fValueMin = value;
                UpdateText();
            }

        }


        /// <summary>
        /// Получает или устанавливает символ разделения значений месяца, дня и года
        /// </summary>
        public char __fSymbolSeparatorDate_
        {
            //get
            //{
            //    return Behavior.__fSeparator_;
            //}
            //set
            //{
            //    Behavior.__fSeparator_ = value;
            //}
            get
            {
                return fSeparator;
            }
            set
            {
                if (fSeparator == value)
                    return;

                Debug.Assert(value != 0);
                Debug.Assert(!Char.IsDigit(value));

                fSeparator = value;
                UpdateText();
            }

        }

        /// <summary>
        ///   Определяет, следует ли отображать день недели перед месяцем или после него.. </summary>
        /// <remarks>
        ///   This property delegates to <see cref="DateBehavior.__fShowDayBeforeMonth_">DateBehavior.ShowDayBeforeMonth</see>. </remarks>
        /// <seealso cref="DateBehavior.FLAG.DayBeforeMonth" />
        public bool __fDayBeforeMonth_
        {
            get
            {
                return __fShowDayBeforeMonth_;
            }
            set
            {
                __fShowDayBeforeMonth_ = value;
            }
        }
        ///// <summary>
        ///// Определяет, следует ли отображать день недели перед месяцем или после него
        ///// </summary>
        ///// <remarks>
        ///// По умолчанию это свойство устанавливается в соответствии с настройками системы пользователя.
        ///// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />.        /// </remarks>
        //public bool __fShowDayBeforeMonth_
        //{
        //    get
        //    {
        //        return HasFlag((int)FLAG.DayBeforeMonth);
        //    }
        //    set
        //    {
        //        ModifyFlags((int)FLAG.DayBeforeMonth, value);
        //    }
        //}

        /// <summary>
        /// Значение контрола
        /// </summary>
        public object __fValue_
        {
            get { return Convert.ToDateTime(Text); }
            set
            {
                Text = Convert.ToDateTime(value).ToString();
                //_cLabelValue.Text = _cInput.Value.ToString();  // Запись значения по умолчанию
            }
        }
        public bool __fValueInTicks_
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the year on the textbox
        /// </summary>
        /// <remarks>
        /// If the year is not valid on the textbox, this property will return 0.
        /// This property must be set with a year that falls within the allowed range
        /// </remarks>
        //public int Year
        //{
        //    get
        //    {
        //        string text = this.Text;
        //        int length = text.Length;

        //        int slash = text.LastIndexOf(fSeparator);
        //        if (slash > 0 && slash < length - 1)
        //            return ToInt(text.Substring(slash + 1, Math.Min(4, length - slash - 1)));
        //        return 0;
        //    }
        //    set
        //    {
        //        // Verify it's in range
        //        if (!IsValidYear(value))
        //            throw new ArgumentOutOfRangeException();

        //        using (elmTypeSelection.Saver savedSelection = new elmTypeSelection.Saver(this))   // remember the current selection
        //        {
        //            if (Year > 0)       // see if there's already a year
        //                m_selection.Set(GetYearStartPosition(), GetYearStartPosition() + 4);

        //            m_selection.Replace(String.Format("{0,4:0000}", value));    // set the year

        //            AdjustMaxMonthAndDay(); // adjust the month and/or day if they're out of range
        //        }
        //    }
        //}

        /// <summary>
        /// Определяет, следует ли отображать день недели перед месяцем или после него
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство устанавливается в соответствии с настройками системы пользователя.
        /// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />.        /// </remarks>
        public bool __fShowDayBeforeMonth_
        {
            get
            {
                return HasFlag((int)FLAG.DayBeforeMonth);
            }
            set
            {
                ModifyFlags((int)FLAG.DayBeforeMonth, value);
            }
        }
        /// <summary>
        /// Проверяет, установлено ли значение флага (включено ли оно).
        /// </summary>
        /// <param name="flag">Флаг для проверки</param>
        /// <returns>
        /// Если флаг установлен, возвращаемое значение равно true; в противном случае — false
        /// </returns>
        public bool HasFlag(int flag)
        {
            return (_fFlags & flag) != 0;
        }
        /// <summary>
        /// Если значение недействительно, корректирует месяц (до минимального значения); если значение недействительно, корректирует день (до максимального значения)
        /// </summary>
        /// <returns>
        /// Если месяц и/или день изменяются, возвращаемое значение равно true; в противном случае — false
        /// </returns>
        protected bool AdjustMaxMonthAndDay()
        {
            int month = __fMonth_;
            if (month != 0 && !IsValidMonth(month))
            {
                __fMonth_ = __mGetMinMonth();  // this adjusts the day automatically
                return true;
            }

            return __mAdjustMaxDay();
        }
        /// <summary>
        /// Проверяет, является ли месяц действительным — находится ли он в допустимом диапазоне
        /// </summary>
        /// <param name="month">Месяц для проверки</param>
        /// <returns>
        /// Если месяц попадает в допустимый диапазон, возвращается значение true; в противном случае — false
        /// </returns>
        protected bool IsValidMonth(int month)
        {
            int year = __mGetValidYear();
            int day = GetValidDay();
            try
            {
                return __mIsWithinRange(new DateTime(year, month, day));
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Извлекает максимальное значение за день на основе месяца, года и допустимого диапазона
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это максимальное значение за день
        /// </returns>
        protected int GetMaxDay()
        {
            int year = __mGetValidYear();
            int month = __mGetValidMonth();

            if (year == fValueMax.Year && month == fValueMax.Month)
                return fValueMax.Day;

            return GetMaxDayOfMonth(month, year);
        }
        /// <summary>
        /// Извлекает максимальное количество дней за заданный месяц и год
        /// </summary>
        /// <param name="month">Месяц (1 - 12)</param>
        /// <param name="year">Год (1900 - 9999)</param>
        /// <returns>
        /// Возвращаемое значение — максимальный день (1–31)
        /// </returns>
        protected static int GetMaxDayOfMonth(int month, int year)
        {
            Debug.Assert(month >= 1 && month <= 12);

            switch (month)
            {
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;

                case 2:
                    return DateTime.IsLeapYear(year) ? 29 : 28;
            }
            return 31;
        }

        /// <summary>
        /// Извлекает день из текстового поля как допустимое значение
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это допустимое значение на текущий день (с 1 по 31)
        /// </returns>
        /// <remarks>
        /// Метод проверяет значение дня в текстовом поле.
        /// Если значение находится в допустимом диапазоне, он возвращает его.
        /// Если значение меньше минимально допустимого, возвращается минимальное значение.
        /// Если значение больше максимально допустимого, возвращается максимальное значение
        /// </remarks>
        protected int GetValidDay()
        {
            int day = __fDay_;

            // It it's outside the range, fix it
            if (day < GetMinDay())
                day = GetMinDay();
            else if (day > GetMaxDay())
                day = GetMaxDay();

            return day;
        }
        /// <summary>
        /// Определяет минимальное значение за день на основе месяца, года и допустимого диапазона
        /// </summary>
        /// <returns>
        /// Возвращаемая сумма — это минимальное значение за день
        /// </returns>
        protected int GetMinDay()
        {
            int year = __mGetValidYear();
            int month = __mGetValidMonth();

            if (year == fValueMin.Year && month == fValueMin.Month)
                return fValueMin.Day;

            return 1;
        }

        /// <summary>
        /// Добавляет или удаляет флаги из поведения
        /// </summary>
        /// <param name="flags">Биты, которые нужно включить (объединить ИЛИ) или выключить во внутреннем параметре флагов</param>
        /// <param name="addOrRemove">If true the flags are added, otherwise they're removed</param>
        /// <remarks>
        /// Этот метод — удобный способ изменить свойство <see cref="Flags" /> без перезаписи его значения. Если изменяется внутреннее значение флагов, автоматически вызывается метод <see cref="UpdateText" />
        /// </remarks>
        public void ModifyFlags(int flags, bool addOrRemove)
        {
            if (addOrRemove)
                Flags = _fFlags | flags;
            else
                Flags = _fFlags & ~flags;
        }
        /// <summary>
        /// Извлекает нулевой индекс года из текстового поля
        /// </summary>
        /// <returns>
        /// Доходность представляет собой исходное положение в году
        /// </returns>
        /// <remarks>
        /// Это всегда 6
        /// </remarks>
        protected int GetYearStartPosition()
        {
            return 6;
        }

        /// <summary>
        /// Проверяет, является ли текст в текстовом поле допустимым, и если нет, обновляет его допустимым значением
        /// </summary>
        /// <returns>
        /// Если текст в текстовом поле обновляется (поскольку он был недействительным), возвращаемое значение равно true; в противном случае — false
        /// </returns>
        /// <remarks>
        /// Этот метод используется производными классами для обеспечения корректности текста в текстовом поле
        /// </remarks>
        public virtual bool UpdateText()
        {
            string validText = GetValidText();
            if (validText != this.Text)
            {
                this.Text = validText;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Извлекает текст из текстового поля в корректном виде
        /// </summary>
        /// <returns>
        /// Если текст в текстовом поле допустим, он возвращается; в противном случае возвращается допустимая версия текста
        /// </returns>
        /// <remarks>
        /// Этот метод предназначен для переопределения производными классами Behavior. Здесь он просто возвращает текст из текстового поля
        /// </remarks>
        protected virtual string GetValidText()
        {
            return this.Text;
        }


        /// <summary>
        /// Получает или задает флаги, связанные с этим объектом поведения
        /// </summary>
        /// <remarks>
        /// Это свойство служит для удобства производных классов поведения, которые могут использовать его для хранения бинарных атрибутов (флагов) внутри отдельных битов. Если это свойство изменяется, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />
        /// </remarks>
        public virtual int Flags
        {
            get
            {
                return _fFlags;
            }
            set
            {
                if (_fFlags == value)
                    return;

                _fFlags = value;
                UpdateText();
            }
        }
        #region = СОБЫТИЯ

        ///// <summary>
        ///// Возникает при изменении данных
        ///// </summary>
        //public event EventHandler __eChanged;
        /// <summary>
        /// Возникает при изменении данных пользователем
        /// </summary>
        public event EventHandler __eChangedByUser;
        ///// <summary>
        ///// Возникает при изменении данных программой
        ///// </summary>
        //public event EventHandler __eChangedByProgram;
        ///// <summary>
        ///// Возникает при нажатии клавиши
        ///// </summary>
        //public event EventHandler __eKeyDown;
        ///// <summary>
        ///// Возникает при изменении типа отображаемых данных
        ///// </summary>
        //public event EventHandler __eDateTimeTypeChanged;

        #endregion СОБЫТИЯ
        ///// <summary>
        /////   Designer class used to prevent the Text property from being set to
        /////   some default value (ie. textBox1) and to remove properties the designer 
        /////   should not generate code for. </summary>
        //internal new class Designer : elmTextBox.Designer
        //{
        //    /// <summary>
        //    ///   Removes properties that the form designer should not generate code for
        //    ///   when the DateTextBox control is added to a form. </summary>
        //    /// <param name="properties">
        //    ///   The dictionary of properties to be manipulated. </param>
        //    protected override void PostFilterProperties(IDictionary properties)
        //    {
        //        properties.Remove("Month");
        //        properties.Remove("Day");
        //        properties.Remove("Year");
        //        properties.Remove("Value");
        //        properties.Remove("Separator");
        //        properties.Remove("ShowDayBeforeMonth");

        //        base.PostFilterProperties(properties);
        //    }
        //}


    }

}
