using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentTime.cs
    /// </summary>	
    /// <remarks>Класс-компонент для ввода значений времени</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.02.20 09-40</version> // Дата-время последней корректировки
    public class elmComponentTime : elmTextBox
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public elmComponentTime()
        {
            _fBehavior = new TimeBehavior(this);
        }
        /// <summary>
        /// Конструктор с определением поведения
        /// </summary>
        /// <param name="behavior">Поведение</param>
        public elmComponentTime(TimeBehavior behavior) : base(behavior)
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
            __fSymbolsCount_ = 5;

            #endregion Настройка компонента

            ResumeLayout(false);

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        /// <summary>
        /// Устанавливает часы и минуты
        /// </summary>
        /// <param name="hour">Устанавливаемые часы</param>
        /// <param name="minute">Устанавливаемые минуты</param>
        public void SetTime(int hour, int minute)
        {
            Behavior.SetTime(hour, minute);
        }
        /// <summary>
        /// Устанавливает часы, минуты и секунды
        /// </summary>
        /// <param name="hour">Устанавливаемые часы</param>
        /// <param name="minute">Устанавливаемые минуты</param>
        /// <param name="second">Устанавливаемые секунды</param>
        public void SetTime(int hour, int minute, int second)
        {
            Behavior.SetTime(hour, minute, second);
        }

        #endregion МЕТОДЫ

        #region = СВОЙСТВА

        /// <summary>
        /// Получает или устанавливает AM/PM
        /// </summary>
        public string __fAMPM_
        {
            get
            {
                return Behavior.AMPM;
            }
        }
        /// <summary>
        /// Получает объект Behavior, связанный с этим классом
        /// </summary>
        public TimeBehavior Behavior
        {
            get
            {
                return (TimeBehavior)_fBehavior;
            }
        }
        /// <summary>
        /// Получает или устанавливает формат отображения времени (12/24)
        /// </summary>
        public bool __fFormat_
        {
            get
            {
                return Behavior.Show24HourFormat;
            }
            set
            {
                Behavior.Show24HourFormat = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает значение часа
        /// </summary>
        public int __fHour_
        {
            get
            {
                return Behavior.Hour;
            }
            set
            {
                Behavior.Hour = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает значение минут
        /// </summary>
        public int __fMinute_
        {
            get
            {
                return Behavior.Minute;
            }
            set
            {
                Behavior.Minute = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает значение секунд
        /// </summary>
        public int __fSecond_
        {
            get
            {
                return Behavior.Second;
            }
            set
            {
                Behavior.Second = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает Определяет, следует ли отображать секунды (после минут)
        /// </summary>
        public bool __fSecondsShow_
        {
            get
            {
                return Behavior.ShowSeconds;
            }
            set
            {
                Behavior.ShowSeconds = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает символ разделителя часов, минут и секунд
        /// </summary>
        public char __fSymbolSeparator_
        {
            get
            {
                return Behavior.Separator;
            }
            set
            {
                Behavior.Separator = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает значение времени
        /// </summary>
        public object __fValue_
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
        /// Получает или устанавливает максимально допустимое значение
        /// </summary>
        public DateTime __fValueMaximum_
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
        /// Получает или устанавливает минимально допустимое значение
        /// </summary>
        public DateTime __fValueMinimum_
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

        #endregion СВОЙСТВА
        ///// <summary>
        /////   Designer class used to prevent the Text property from being set to
        /////   some default value (ie. textBox1) and to remove properties the designer 
        /////   should not generate code for. </summary>
        //internal new class Designer : elmTextBox.Designer
        //{
        //    /// <summary>
        //    ///   Removes properties that the form designer should not generate code for
        //    ///   when the TimeTextBox control is added to a form. </summary>
        //    /// <param name="properties">
        //    ///   The dictionary of properties to be manipulated. </param>
        //    protected override void PostFilterProperties(IDictionary properties)
        //    {
        //        properties.Remove("Hour");
        //        properties.Remove("Minute");
        //        properties.Remove("Second");
        //        properties.Remove("Value");
        //        properties.Remove("Separator");
        //        properties.Remove("Show24HourFormat");

        //        base.PostFilterProperties(properties);
        //    }
        //}
    }

}
