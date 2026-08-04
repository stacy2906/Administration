using System;
using System.Collections;
using System.ComponentModel;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentDateTime.cs
    /// </summary>	
    public class elmComponentDateTime : elmComponentTime
    {
        /// <summary>
        ///   Initializes a new instance of the DateTimeTextBox class by assigning its Behavior field
        ///   to an instance of <see cref="DateTimeBehavior" />. </summary>
        public elmComponentDateTime() : base(null)
        {
            _fBehavior = new DateTimeBehavior(this);
        }

        /// <summary>
        ///   Initializes a new instance of the DateTimeTextBox class by explicitly assigning its Behavior field. </summary>
        /// <param name="behavior">
        ///   The <see cref="DateTimeBehavior" /> object to associate the textbox with. </param>
        public elmComponentDateTime(DateTimeBehavior behavior) :
            base(behavior)
        {
        }

        /// <summary>
        ///   Gets the Behavior object associated with this class. </summary>
        [Browsable(false)]
        public new DateTimeBehavior Behavior
        {
            get
            {
                return (DateTimeBehavior)_fBehavior;
            }
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
            __fSymbolsCount_ = 16;

            #endregion Настройка компонента

            ResumeLayout(false);

            return;
        }

        /// <summary>
        /// Получает и устанавливает месяц
        /// </summary>
        public int __fMonth_
        {
            get
            {
                return Behavior.__fMonth_;
            }
            set
            {
                Behavior.__fMonth_ = value;
            }
        }

        /// <summary>
        ///   Gets or sets the day on the textbox. </summary>
        /// <exception cref="ArgumentOutOfRangeException">Setting this property with an invalid day. </exception>
        /// <remarks>
        ///   This property delegates to <see cref="DateTimeBehavior.Day">DateTimeBehavior.Day</see>. </remarks>
        /// <seealso cref="__fMonth_" />
        /// <seealso cref="Year" />
        [Browsable(false)]
        public int Day
        {
            get
            {
                return Behavior.Day;
            }
            set
            {
                Behavior.Day = value;
            }
        }

        /// <summary>
        ///   Gets or sets the year on the textbox. </summary>
        /// <exception cref="ArgumentOutOfRangeException">Setting this property with an invalid year. </exception>
        /// <remarks>
        ///   This property delegates to <see cref="DateTimeBehavior.Year">DateTimeBehavior.Year</see>. </remarks>
        /// <seealso cref="__fMonth_" />
        /// <seealso cref="Day" />
        [Browsable(false)]
        public int Year
        {
            get
            {
                return Behavior.Year;
            }
            set
            {
                Behavior.Year = value;
            }
        }

        /// <summary>
        ///   Gets or sets the month, day, and year on the textbox using a <see cref="DateTime" /> object. </summary>
        /// <remarks>
        ///   This property delegates to <see cref="DateTimeBehavior.Value">DateTimeBehavior.Value</see>. </remarks>
        /// <seealso cref="__fMonth_" />
        /// <seealso cref="Day" />
        /// <seealso cref="Year" />
        [Browsable(false)]
        public new object Value
        {
            get
            {
                return Behavior.Value;
            }
            set
            {
                Behavior.Value = value;
            }
        }

        /// <summary>
        ///   Gets or sets the minimum value allowed. </summary>
        /// <remarks>
        ///   This property delegates to <see cref="DateTimeBehavior.RangeMin">DateTimeBehavior.RangeMin</see>. </remarks>
        /// <seealso cref="RangeMax" />
        [Category("Behavior")]
        [Description("The minimum value allowed.")]
        public new DateTime RangeMin
        {
            get
            {
                return Behavior.RangeMin;
            }
            set
            {
                Behavior.RangeMin = value;
            }
        }

        /// <summary>
        ///   Gets or sets the maximum value allowed. </summary>
        /// <remarks>
        ///   This property delegates to <see cref="DateTimeBehavior.RangeMax">DateTimeBehavior.RangeMax</see>. </remarks>
        /// <seealso cref="RangeMin" />
        [Category("Behavior")]
        [Description("The maximum value allowed.")]
        public new DateTime RangeMax
        {
            get
            {
                return Behavior.RangeMax;
            }
            set
            {
                Behavior.RangeMax = value;
            }
        }

        /// <summary>
        ///   Gets or sets the character used to separate the month, day, and year values of the date. </summary>
        /// <remarks>
        ///   This property delegates to <see cref="DateTimeBehavior.DateSeparator">DateTimeBehavior.DateSeparator</see>. </remarks>
        [Browsable(false)]
        public char DateSeparator
        {
            get
            {
                return Behavior.DateSeparator;
            }
            set
            {
                Behavior.DateSeparator = value;
            }
        }

        /// <summary>
        ///   Gets or sets the character used to separate the hour, minute, and second values of the time. </summary>
        /// <remarks>
        ///   This property delegates to <see cref="DateTimeBehavior.TimeSeparator">DateTimeBehavior.TimeSeparator</see>. </remarks>
        [Browsable(false)]
        public char TimeSeparator
        {
            get
            {
                return Behavior.TimeSeparator;
            }
            set
            {
                Behavior.TimeSeparator = value;
            }
        }

        /// <summary>
        ///   Gets the character used to separate the date or time value. </summary>
        /// <remarks>
        ///   This property delegates to <see cref="DateTimeBehavior.Separator">DateTimeBehavior.Separator</see>. </remarks>
        [Browsable(false)]
        private new char Separator
        {
            get
            {
                return Behavior.Separator;
            }
        }

        /// <summary>
        ///   Gets or sets whether the day should be shown before the month or after it. </summary>
        /// <remarks>
        ///   This property delegates to <see cref="DateBehavior.__fShowDayBeforeMonth_">DateBehavior.ShowDayBeforeMonth</see>. </remarks>
        /// <seealso cref="DateBehavior.FLAG.DayBeforeMonth" />
        [Browsable(false)]
        public bool ShowDayBeforeMonth
        {
            get
            {
                return Behavior.ShowDayBeforeMonth;
            }
            set
            {
                Behavior.ShowDayBeforeMonth = value;
            }
        }

        /// <summary>
        ///   Sets the month, day, and year on the textbox. </summary>
        /// <param name="year">
        ///   The year to set. </param>
        /// <param name="month">
        ///   The month to set. </param>
        /// <param name="day">
        ///   The day to set. </param>
        /// <remarks>
        ///   This method delegates to <see cref="DateTimeBehavior.SetDate">DateTimeBehavior.SetDate</see>. </remarks>
        public void SetDate(int year, int month, int day)
        {
            Behavior.SetDate(year, month, day);
        }

        /// <summary>
        ///   Sets the month, day, year, hour, minute, and second on the textbox. </summary>
        /// <param name="year">
        ///   The year to set. </param>
        /// <param name="month">
        ///   The month to set. </param>
        /// <param name="day">
        ///   The day to set. </param>
        /// <param name="hour">
        ///   The hour to set, between 0 and 23. </param>
        /// <param name="minute">
        ///   The minute to set, between 0 and 59. </param>
        /// <remarks>
        ///   This method delegates to <see cref="DateTimeBehavior.SetDateTime">DateTimeBehavior.SetDateTime</see>. </remarks>
        public void SetDateTime(int year, int month, int day, int hour, int minute)
        {
            Behavior.SetDateTime(year, month, day, hour, minute);
        }

        /// <summary>
        ///   Sets the month, day, year, hour, minute, and second on the textbox. </summary>
        /// <param name="year">
        ///   The year to set. </param>
        /// <param name="month">
        ///   The month to set. </param>
        /// <param name="day">
        ///   The day to set. </param>
        /// <param name="hour">
        ///   The hour to set, between 0 and 23. </param>
        /// <param name="minute">
        ///   The minute to set, between 0 and 59. </param>
        /// <param name="second">
        ///   The second to set, between 0 and 59. </param>
        /// <remarks>
        ///   This method delegates to <see cref="DateTimeBehavior.SetDateTime">DateTimeBehavior.SetDateTime</see>. </remarks>
        public void SetDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            Behavior.SetDateTime(year, month, day, hour, minute, second);
        }


        /// <summary>
        /// Значение контрола
        /// </summary>
        public object __fValue_
        {
            get { return Convert.ToDateTime(Text); }
            set
            {
                Value = Convert.ToDateTime(value);
                //_cLabelValue.Text = _cInput.Value.ToString();  // Запись значения по умолчанию
            }
        }
        public bool __fValueInTicks_
        {
            get; set;
        }

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при изменении данных в поле ввода пользователем
        /// </summary>
        public event EventHandler __eChangedByUser;

        #endregion СОБЫТИЯ

        ///// <summary>
        /////   Designer class used to prevent the Text property from being set to
        /////   some default value (ie. textBox1) and to remove properties the designer 
        /////   should not generate code for. </summary>
        //internal new class Designer : elmComponentTime.Designer
        //{
        //    /// <summary>
        //    ///   Removes properties that the form designer should not generate code for
        //    ///   when the DateTimeTextBox control is added to a form. </summary>
        //    /// <param name="properties">
        //    ///   The dictionary of properties to be manipulated. </param>
        //    protected override void PostFilterProperties(IDictionary properties)
        //    {
        //        properties.Remove("Month");
        //        properties.Remove("Day");
        //        properties.Remove("Year");
        //        properties.Remove("DateSeparator");
        //        properties.Remove("TimeSeparator");
        //        properties.Remove("ShowDayBeforeMonth");

        //        base.PostFilterProperties(properties);
        //    }
        //}
    }


}
