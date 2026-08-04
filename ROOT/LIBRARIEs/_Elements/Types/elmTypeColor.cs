using nlApplication;
using System.Drawing;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmTypeColor.cs
    /// </summary>
    /// <remarks>Класс для работы с типами 'Color'</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.01.14 09-31</version> // Дата-время последней корректировки
    public class elmTypeColor 
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Преобразование строчного выражения в {Color}
        /// </summary>
        /// <param name="pExpression">Строчное выражение вида: 'R=255, G=255, B=255'</param>
        /// <returns>{Color} с полученными цветами</returns>
        public static Color __mFromString(string pExpression)
        {
            Color vReturn = Color.Empty; // Возвращаемое значение

            int vColorRed = Convert.ToInt32(appTypeString.__mWordNumber(appTypeString.__mWordNumberSpace(pExpression, 0).Replace(',', ' ').Trim(), 1, '='));
            int vColorGreen = Convert.ToInt32(appTypeString.__mWordNumber(appTypeString.__mWordNumberSpace(pExpression, 1).Replace(',', ' ').Trim(), 1, '='));
            int vrColorBlue = Convert.ToInt32(appTypeString.__mWordNumber(appTypeString.__mWordNumberSpace(pExpression, 2), 1, '='));

            vReturn = Color.FromArgb(255,
                                     vColorRed,
                                     vColorGreen,
                                     vrColorBlue);
            return vReturn;
        }
        /// <summary>
        /// Преобразование {Color} в строку
        /// </summary>
        /// <param name="pColor">{Color}</param>
        /// <returns>Строчный эквивалент цветовых составляющих</returns>
        public static string __mToString(Color pColor)
        {
            return "R=" + pColor.R.ToString() + ", " +
                   "G=" + pColor.G.ToString() + ", " +
                   "R=" + pColor.B.ToString();
        }
        /// <summary>
        /// Преобразование строчного выражения в {Color}
        /// </summary>
        /// <param name="pString">Строчное выражение вида: '255, 255, 255'</param>
        /// <returns></returns>
        public static Color __mFromString2(string pString)
        {
            return Color.FromArgb(255
                , Convert.ToInt32(appTypeString.__mWordNumberComma(pString, 0))
                , Convert.ToInt32(appTypeString.__mWordNumberComma(pString, 1))
                , Convert.ToInt32(appTypeString.__mWordNumberComma(pString, 2)));
        }
        /// <summary>
        /// Преобразование {Color} в строку
        /// </summary>
        /// <param name="pColor">{Color}</param>
        /// <returns>Строчный эквивалент цветовых составляющих</returns>
        public static string __mToString2(Color pColor)
        {
            return pColor.R.ToString() + ", " +
                   pColor.G.ToString() + ", " +
                   pColor.B.ToString();
        }

        #endregion Процедуры

        #endregion МЕТОДЫ    
    }
}
