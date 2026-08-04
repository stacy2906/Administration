using System;
using System.Collections;

namespace nlApplication
{
    /// <summary>
    /// Файл appTypeDateTime.cs
    /// </summary>
	/// <remarks>Класс-тип для работы с данными даты и времени</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 00-00</version> // Дата-время последней корректировки
    public class appTypeDateTime 
    {
        #region = МЕТОДЫ

        #region - Процедуры

        #region Извлечение части дня

        /// <summary>
        /// Получение номера дня недели
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        /// <returns>
        /// 1 - Понедельник
        /// 2 - Вторник
        /// 3 - Среда
        /// 4 - Четверг
        /// 5 - Пятница
        /// 6 - Суббота
        /// 7 - Воскресенье
        /// </returns>
        public static int __mDayOfWeekNumber(DateTime pDateTime)
        {
            int vReturn = 0; // Возвращаемое значение

            switch (pDateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    vReturn = 1;
                    break;
                case DayOfWeek.Tuesday:
                    vReturn = 2;
                    break;
                case DayOfWeek.Wednesday:
                    vReturn = 3;
                    break;
                case DayOfWeek.Thursday:
                    vReturn = 4;
                    break;
                case DayOfWeek.Friday:
                    vReturn = 5;
                    break;
                case DayOfWeek.Saturday:
                    vReturn = 6;
                    break;
                case DayOfWeek.Sunday:
                    vReturn = 7;
                    break;
            }

            return vReturn;
        }

        #endregion  Извлечение части дня

        #region Даты для MS SQL сервера

        /// <summary>
        /// Получение пустой даты для MSSQL
        /// </summary>
        public static DateTime __mMsSqlDateEmpty()
        {
            return new DateTime(1900, 1, 1);
        }
        /// <summary>
        /// Получение строчного эквивалента пустой даты для MSSQL
        /// </summary>
        /// <returns>Строка "19000101"</returns>
        public static string __mMsSqlDateEmptyToString()
        {
            return "19000101";
        }
        /// <summary>
        /// Получение строчного эквивалента даты в универсальном формате MSSQL
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        /// <remarks>Используется для MSSQL на разных языках</remarks>
        public static string __mMsSqlDateToString(DateTime pDateTime)
        {
            return pDateTime.Year.ToString().Trim() + pDateTime.Month.ToString().PadLeft(2, '0') + pDateTime.Day.ToString().PadLeft(2, '0');
        }
        /// <summary>
        /// Получение строчного эквивалента даты и времени из {DateTime} в универсальном формате MSSQL
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        /// <remarks>Используется для MSSQL на разных языках</remarks>
        public static string __mMsSqlDateTimeToString(DateTime pDateTime)
        {
            return pDateTime.Year +
                   pDateTime.Month.ToString().PadLeft(2, '0') +
                   pDateTime.Day.ToString().PadLeft(2, '0') + " " +
                   pDateTime.Hour.ToString().PadLeft(2, '0') + ":" +
                   pDateTime.Minute.ToString().PadLeft(2, '0') + ":" +
                   pDateTime.Second.ToString().PadLeft(2, '0');
        }

        #endregion Даты для MS SQL сервера

        #region Имена файлов

        /// <summary>
        /// Получение строчного эквивалента даты для формирования названия файла
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static string __mDateToFileName(DateTime pDateTime)
        {
            return pDateTime.Year.ToString() + "_" +
                   pDateTime.Month.ToString().PadLeft(2, '0') + "_" +
                   pDateTime.Day.ToString().PadLeft(2, '0');
        }
        /// <summary>
        /// Получение строчного эквивалента даты и времени для формирования названия файла с точностью до минут
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static string __mDateTimeToFileNameTillMinute(DateTime pDateTime)
        {
            return pDateTime.Year.ToString() +
                   pDateTime.Month.ToString().PadLeft(2, '0') +
                   pDateTime.Day.ToString().PadLeft(2, '0') + "_" +
                   pDateTime.Hour.ToString().PadLeft(2, '0') +
                   pDateTime.Minute.ToString().PadLeft(2, '0');
        }
        /// <summary>
        /// Получение строчного эквивалента даты и времени для формирования названия файла с точностью до секунд
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static string __mDateTimeToFileNameTillSecond(DateTime pDateTime)
        {
            return pDateTime.Year.ToString() +
                   pDateTime.Month.ToString().PadLeft(2, '0') +
                   pDateTime.Day.ToString().PadLeft(2, '0') + "_" +
                   pDateTime.Hour.ToString().PadLeft(2, '0') +
                   pDateTime.Minute.ToString().PadLeft(2, '0') +
                   pDateTime.Second.ToString().PadLeft(2, '0');
        }
        /// <summary>
        /// Получение строчного эквивалента даты и времени для формирования названия файла с точностью до милисекунд
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static string __mDateTimeToFileNameTillMillisecond(DateTime pDateTime)
        {
            return pDateTime.Year.ToString() +
                   pDateTime.Month.ToString().PadLeft(2, '0') +
                   pDateTime.Day.ToString().PadLeft(2, '0') + "_" +
                   pDateTime.Hour.ToString().PadLeft(2, '0') +
                   pDateTime.Minute.ToString().PadLeft(2, '0') +
                   pDateTime.Second.ToString().PadLeft(2, '0') + "_" +
                   pDateTime.Millisecond.ToString().PadLeft(3, '0');
        }
        /// <summary>
        /// Получение строчного эквивалента даты и времени с точностью до минут
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static string __mDateToStringTillMinute(DateTime pDateTime)
        {
            return pDateTime.Day.ToString().PadLeft(2, '0') + "." +
                   pDateTime.Month.ToString().PadLeft(2, '0') + "." +
                   pDateTime.Year.ToString() + " " +
                   pDateTime.Hour.ToString().PadLeft(2, '0') + ":" +
                   pDateTime.Minute.ToString().PadLeft(2, '0');
        }
        /// <summary>
        /// Получение строчного эквивалента даты и времени с точностью до секунд
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static string __mDateToStringTillSecond(DateTime pDateTime)
        {
            return pDateTime.Day.ToString().PadLeft(2, '0') + "." +
                   pDateTime.Month.ToString().PadLeft(2, '0') + "." +
                   pDateTime.Year.ToString() + " " +
                   pDateTime.Hour.ToString().PadLeft(2, '0') + ":" +
                   pDateTime.Minute.ToString().PadLeft(2, '0') + ":" +
                   pDateTime.Second.ToString().PadLeft(2, '0');
        }
        /// <summary>
        /// Получение строчного эквивалента даты и времени с точностью до милисекунд
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static string __mDateToStringTillMillisecond(DateTime pDateTime)
        {
            return pDateTime.Day.ToString().PadLeft(2, '0') + "." +
                   pDateTime.Month.ToString().PadLeft(2, '0') + "." +
                   pDateTime.Year.ToString() + " " +
                   pDateTime.Hour.ToString().PadLeft(2, '0') + ":" +
                   pDateTime.Minute.ToString().PadLeft(2, '0') + ":" +
                   pDateTime.Second.ToString().PadLeft(2, '0') + "." +
                   pDateTime.Millisecond.ToString().PadLeft(3, '0');
        }

        #endregion Имена файлов

        #region Конвертирование даты-времени

        /// <summary>
        /// Получение начала даты для указанного даты времени
        /// </summary>
        /// <param name="pDateTime">Дата-время</param>
        public static DateTime __mDateTimeToDate(DateTime pDateTime)
        {
            return new DateTime(pDateTime.Year, pDateTime.Month, pDateTime.Day);
        }
        /// <summary>
        /// Получение строчного эквивалента даты в формате<German>
        /// </summary>
        /// <param name="pDateTime">Дата-время</param>
        public static string __mDateToString(DateTime pDateTime)
        {
            return pDateTime.Day.ToString().PadLeft(2, '0') + "." + pDateTime.Month.ToString().PadLeft(2, '0') + "." + pDateTime.Year.ToString().PadLeft(4, '0');
        }
        /// <summary>
        /// Получение строчного эквивалента даты-времени в формате <German>
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static string __mDateTimeToString(DateTime pDateTime)
        {
            return pDateTime.Day.ToString().PadLeft(2, '0') + "." + pDateTime.Month.ToString().PadLeft(2, '0') + "." + pDateTime.Year.ToString().PadLeft(4, '0') + " " +
                pDateTime.Hour.ToString().PadLeft(2, '0') + ":" + pDateTime.Minute.ToString().PadLeft(2, '0') + ":" + pDateTime.Second.ToString().PadLeft(2, '0');
        }
 
        /// <summary>
        /// Преобразование секунд во время
        /// </summary>
        /// <param name="pSeconts">Секунды</param>
        public static string __mIntervalToString(TimeSpan pTimeSpan)
        {
            string vReturn = ""; // Возвращаемое значение
            bool vMaxObject = false; // Максимальный объект
            if (pTimeSpan.Days > 0)
            {
                vReturn = vReturn + pTimeSpan.Days.ToString() + appApplication.__oTunes.__mTranslate("д");
                vMaxObject = true;
            } /// Обнаружены сутки
            if (pTimeSpan.Hours > 0 | vMaxObject == true)
            {
                vReturn = vReturn + " " + pTimeSpan.Hours.ToString() + appApplication.__oTunes.__mTranslate("ч");
                vMaxObject = true;
            } /// Обнаружены часы
            if (pTimeSpan.Minutes > 0 | vMaxObject == true)
            {
                vReturn = vReturn + " " + pTimeSpan.Minutes.ToString() + appApplication.__oTunes.__mTranslate("м");
                vMaxObject = true;
            } /// Обнаружены минуты
            if (pTimeSpan.Seconds > 0)
            {
                vReturn = vReturn + " " + pTimeSpan.Seconds.ToString() + appApplication.__oTunes.__mTranslate("с");
            } /// Обнаружены минуты
            if (vReturn.Length == 0)
                vReturn = "0" + appApplication.__oTunes.__mTranslate("c");
            return vReturn.Trim();
        }

        #endregion Конвертирование даты-времени 

        #region Получение начального и конечного времени

        /// <summary>
        /// Получение получение начала суток для указанной даты
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        /// <returns>{DateTime} в формате DD.MM.YYYY 00 00 00.</returns>
        public static DateTime __mDayBegin(DateTime pDateTime)
        {
            return new DateTime(pDateTime.Year, pDateTime.Month, pDateTime.Day, 0, 0, 0, DateTimeKind.Local);
        }
        /// <summary>
        /// Получение получение конца суток для указанной даты
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        /// <returns>'DateTime' в формате DD.MM.YYYY 23 59 59</returns>
        public static DateTime __mDayEnd(DateTime pDateTime)
        {
            return new DateTime(pDateTime.Year, pDateTime.Month, pDateTime.Day, 23, 59, 59, DateTimeKind.Local);
        }
        /// <summary>
        /// Получение минимального времени первого дня месяца для указанной даты
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static DateTime __mMonthBegin(DateTime pDateTime)
        {
            return new DateTime(pDateTime.Year, pDateTime.Month, 1, 0, 0, 0, DateTimeKind.Local);
        }
        /// <summary>
        /// Получение максимального времени последнего дня месяца для указанной даты
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static DateTime __mMonthEnd(DateTime pDateTime)
        {
            DateTime vReturn = new DateTime(); // = new DateTime(pDateTime.Year, pDateTime.Month, 30, 23, 59, 59, DateTimeKind.Local); // Возвращаемое значение
            bool vYearLeap = DateTime.IsLeapYear(pDateTime.Year); // Признак високосного года
            ArrayList vMonthDayCount = new ArrayList(); // Список месяцев с 31 днем

            vMonthDayCount.Add(1);
            vMonthDayCount.Add(3);
            vMonthDayCount.Add(5);
            vMonthDayCount.Add(7);
            vMonthDayCount.Add(8);
            vMonthDayCount.Add(10);
            vMonthDayCount.Add(12);

            foreach (int vDayCount in vMonthDayCount)
            {
                if (pDateTime.Month == vDayCount)
                {
                    vReturn = new DateTime(pDateTime.Year, pDateTime.Month, 31, 23, 59, 59, DateTimeKind.Local);
                    break;
                }
            }
            if(vReturn == DateTime.MinValue)
                vReturn = new DateTime(pDateTime.Year, pDateTime.Month, 30, 23, 59, 59, DateTimeKind.Local);
            if (vYearLeap == true & pDateTime.Month == 2)
                vReturn = new DateTime(pDateTime.Year, pDateTime.Month, 29, 23, 59, 59, DateTimeKind.Local);
            if (vYearLeap != true & pDateTime.Month == 2)
                vReturn = new DateTime(pDateTime.Year, pDateTime.Month, 28, 23, 59, 59, DateTimeKind.Local);

            return vReturn;
        }
        /// <summary>
        /// Получение минимального времени первого дня месяца для указанной даты
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static DateTime __mQuarterBegin(DateTime pDateTime)
        {
            DateTime vReturn = new DateTime(); // Возврашаемое значение

            if (pDateTime.Month >= 1 & pDateTime.Month <= 3)
                vReturn = __mDayBegin(Convert.ToDateTime("01.01." + pDateTime.Year.ToString()));
            if (pDateTime.Month >= 4 & pDateTime.Month <= 6)
                vReturn = __mDayBegin(Convert.ToDateTime("01.04." + pDateTime.Year.ToString()));
            if (pDateTime.Month >= 7 & pDateTime.Month <= 9)
                vReturn = __mDayBegin(Convert.ToDateTime("01.07." + pDateTime.Year.ToString()));
            if (pDateTime.Month >= 10 & pDateTime.Month <= 12)
                vReturn = __mDayBegin(Convert.ToDateTime("01.10." + pDateTime.Year.ToString()));

            return vReturn;
        }
        /// <summary>
        /// Получение максимального времени последнего дня месяца для указанной даты
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static DateTime __mQuarterEnd(DateTime pDateTime)
        {
            DateTime vReturn = new DateTime(); // Возвращаемое значение

            if (pDateTime.Month >= 1 & pDateTime.Month <= 3)
                vReturn = __mMonthEnd(Convert.ToDateTime("15.03." + pDateTime.Year.ToString()));
            if (pDateTime.Month >= 4 & pDateTime.Month <= 6)
                vReturn = __mMonthEnd(Convert.ToDateTime("15.06." + pDateTime.Year.ToString()));
            if (pDateTime.Month >= 7 & pDateTime.Month <= 9)
                vReturn = __mMonthEnd(Convert.ToDateTime("15.09." + pDateTime.Year.ToString()));
            if (pDateTime.Month >= 10 & pDateTime.Month <= 12)
                vReturn = __mMonthEnd(Convert.ToDateTime("15.12." + pDateTime.Year.ToString()));

            return vReturn;
        }
        /// <summary>
        /// Получение минимального времени первого дня недели для указанной даты
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static DateTime __mWeekBegin(DateTime pDateTime)
        {
            int vDayWeek = __mDayOfWeekNumber(pDateTime);// Номер полученого времени в неделе

            pDateTime = __mDayBegin(pDateTime); /// Установка минимального времени для полученного времени
            switch (vDayWeek)
            {
                case 2:
                    return pDateTime.AddDays(-1);
                case 3:
                    return pDateTime.AddDays(-2);
                case 4:
                    return pDateTime.AddDays(-3);
                case 5:
                    return pDateTime.AddDays(-4);
                case 6:
                    return pDateTime.AddDays(-5);
                case 7:
                    return pDateTime.AddDays(-6);
                default:
                    return pDateTime;
            }
        }
        /// <summary>
        /// Получение максимального времени последнего дня недели для указанной даты
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static DateTime __mWeekEnd(DateTime pDateTime)
        {
            int vDayWeek = __mDayOfWeekNumber(pDateTime); // Номер полученного времени в неделе

            pDateTime = __mDayEnd(pDateTime); /// Установка максимального времени
            switch (vDayWeek)
            {
                case 1:
                    return pDateTime.AddDays(6);
                case 2:
                    return pDateTime.AddDays(5);
                case 3:
                    return pDateTime.AddDays(4);
                case 4:
                    return pDateTime.AddDays(3);
                case 5:
                    return pDateTime.AddDays(2);
                case 6:
                    return pDateTime.AddDays(1);
                default:
                    return pDateTime;
            }
        }
        /// <summary>
        /// Получение минимального времени первого дня года для указанной даты
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static DateTime __mYearBegin(DateTime pDateTime)
        {
            return __mDayBegin(Convert.ToDateTime("01.01." + pDateTime.Year.ToString()));
        }
        /// <summary>
        /// Получение максимального времени последнего дня года для указанной даты
        /// </summary>
        /// <param name="pDateTime">Дата и время</param>
        public static DateTime __mYearEnd(DateTime pDateTime)
        {
            return __mDayEnd(Convert.ToDateTime("31.12." + pDateTime.Year.ToString()));
        }

        #endregion Получение начального и конечного времени

        #endregion Процедуры

        #endregion МЕТОДЫ
    }
}
