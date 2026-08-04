using System.Drawing;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmTypeFont.cs
    /// </summary>
    /// 1<remarks>Класс для работы с типами 'Font'</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.14 09-32</version> // Дата-время последней корректировки
    public sealed class elmTypeFont
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Проверка: Является ли шрифт моноширинным
        /// </summary>
        /// <param name="pFont">Шрифт</param>
        /// <returns>[true] - шрифт является моноширинным, иначе [false]</returns>
        public static bool __mIsFontMonoSpace(Font pFont)
        {
            return Math.Abs(__mMeasureText("iii", pFont).Width - __mMeasureText("WWW", pFont).Width) < 1e-3;
        }
        /// <summary>
        /// Определение размера отображаемого на экране текста для указанного шрифта
        /// </summary>
        /// <param name="pExpression">Отображаемый текст</param>
        /// <param name="pFont">Шрифт отображаемого текста</param>
        /// <returns>Размер текста с указанным шрифтом</returns>
        public static SizeF __mMeasureText(string pExpression, Font pFont)
        {
            Bitmap vBitMap = new Bitmap(100, 20);
            Graphics vGraphics = Graphics.FromImage(vBitMap);
            return vGraphics.MeasureString(pExpression, pFont);
        }
        /// <summary>
        /// Определение размера отображаемого на экране текста для указанного шрифта
        /// </summary>
        /// <param name="pSymbolCount">Количество символов в выражении</param>
        /// <param name="pFont">Шрифт отображаемого текста</param>
        /// <returns>Размер текста с указанным шрифтом</returns>
        public static SizeF __mMeasureText(int pSymbolCount, Font pFont)
        {
            Bitmap vBitMap = new Bitmap(100, 20);
            Graphics vGraphics = Graphics.FromImage(vBitMap);
            if (pSymbolCount <= 0)
                pSymbolCount = 10;
            return vGraphics.MeasureString(new String('W', pSymbolCount), pFont);
        }
        /// <summary>
        /// Определение размера кнопки в зависимости от текста для указанного шрифта
        /// </summary>
        /// <param name="pString">Строчное выржение</param>
        /// <param name="pFont">Используемый шрифт</param>
        /// <returns>Размер кнопки</returns>
        public static SizeF __mMeasureButton(string pString, Font pFont)
        {
            SizeF vSize = __mMeasureText(pString, pFont); /// Размер текста.

            return new SizeF((int)(1.6f * vSize.Width), (int)(1.75f * vSize.Height));
        }

        #endregion Процедуры

        #endregion МЕТОДЫ 
    }
}
