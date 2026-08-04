using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms.VisualStyles;

namespace nlApplication
{
    /// <summary>
    /// Файл appTypeString.cs
    /// </summary>
	/// <remarks>Класс-тип для работы с строчными данными</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 13-54</version> // Дата-время последней корректировки
    public class appTypeString
    {
        #region = БИБЛИОТЕКИ

        [DllImport("shlwapi.dll", SetLastError = true)]
        static extern int HashData(string pbData, Int32 cbData, string pbHash, Int32 cbHash);

        #endregion БИБЛИОТЕКИ

        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public appTypeString() 
        {
            _fError = new appUnitError(_fClassFilePath_);
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Процедуры

        #region * Закрытые

        /// <summary>
        /// Получение пути и имени файла класса
        /// </summary>
        protected string _mClassFilePath([CallerFilePath] string pFilePath = "")
        {
            return pFilePath;
        }
        /// <summary>
        /// Получение значения номера строки в текущей процедуре
        /// </summary>
        protected int _mClassLine(string pMessage = "", [CallerLineNumber] int pLine = 0)
        {
            return pLine;
        }
        /// <summary>
        /// Получение названия текущей процедуры
        /// </summary>
        protected string _mClassProcedure(string message, [CallerMemberName] string pMember = "")
        {
            return pMember;
        }

        #endregion Закрытые

        #region /// Работа с символами

        /// <summary>
        /// Возвращает значение ANSI для крайнего левого символа в символьном выражении
        /// </summary>
        /// <param name="pString"></param>
        public static int __mSymbolAsciiCode(string pString)
        {
            int int32 = Convert.ToInt32(Convert.ToChar(pString.Substring(0, 1)));
            if (int32 < 128)
                return int32;
            try
            {
                Encoding fileIoEncoding = Encoding.Default;
                char[] chars = new char[1] { Convert.ToChar(pString.Substring(0, 1)) };
                if (fileIoEncoding.IsSingleByte)
                {
                    byte[] bytes = new byte[1];
                    fileIoEncoding.GetBytes(chars, 0, 1, bytes, 0);
                    return (int)bytes[0];
                }
                byte[] bytes1 = new byte[2];
                if (fileIoEncoding.GetBytes(chars, 0, 1, bytes1, 0) == 1)
                    return (int)bytes1[0];
                if (BitConverter.IsLittleEndian)
                {
                    byte num = bytes1[0];
                    bytes1[0] = bytes1[1];
                    bytes1[1] = num;
                }
                return (int)BitConverter.ToInt16(bytes1, 0);
            }
            catch (Exception vException)
            {
                _fError.__fException = vException;
                _fError.__fProcedure_ = "__mAsciiCode(string)";
                _fError.__mPropertyAdd("Параметр строка {0}", pString);
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return -1;
            }

        }
        /// <summary>
        /// Выполняется замена символов в выражении  
        /// </summary>
        /// <param name="pExpression">Проверяемое выражение</param>
        /// <param name="pSearchedChars">Список искомых символов</param>
        /// <param name="pChangedChars">Список заменяемых символов</param>
        public static string __mSymbolChange(string pExpression, string pSearchedChars, string pChangedChars)
        {
            string vString = "";
            for (int vAmount = 0; vAmount < pSearchedChars.Length; vAmount++)
            {
                if (vAmount < pChangedChars.Length)
                    vString = pChangedChars.Substring(vAmount, 1);
                else
                    vString = String.Empty;

                pExpression = pExpression.Replace(pSearchedChars.Substring(vAmount, 1), vString);
            }

            return pExpression;
        }
        /// <summary>
        /// Возвращает заданное количество символов из выражения, начиная с самого левого
        /// </summary>
        /// <param name="pExpression">Cтрока из которой возвращаются символы</param>
        /// <param name="pCount">Количество возвращаемых символов</param>
        public static string __mSymbolsFromLeft(string pExpression, int pCount)
        {
            if (pExpression.Length == 0)
                return "";
            else
            {
                if (pExpression.Length < pCount)
                {
                    return pExpression;
                }
                else
                {
                    return pExpression.Substring(0, pCount);
                }
            }
        }
        /// <summary>
        /// Возвращает заданное количество символов из выражения, начиная с самого правого
        /// </summary>
        /// <param name="pExpression">Cтрока из которой возвращаются символы</param>
        /// <param name="pCount">Количество возвращаемых символов</param>
        public static string __mSymbolsFromRight(string pExpression, int pCount)
        {
            if (pExpression.Length == 0)
                return "";
            else
            {
                if (pExpression.Length < pCount)
                {
                    return pExpression;
                }
                else
                {
                    return pExpression.Substring(pExpression.Length - pCount);
                }
            }
        }
        /// <summary>
        /// Удаление последних символов у строчного выражения, если они соответствуют /pSymbols/
        /// </summary>
        /// <param name="pExpression">Выражение в котором нужно проверить крайний правый символ</param>
        /// <param name="pSymbols">Проверяемый порядок символов</param>
        /// <remarks>Если символы не собпадают, выражение остается без изменений</remarks>
        public static string __mSymbolsLastDelete(string pExpression, string pSymbols)
        {
            if (__mSymbolsFromRight(pExpression, pSymbols.Length) == pSymbols)
            {
                return __mSymbolsFromLeft(pExpression, pExpression.Length - pSymbols.Length);
            }

            return pExpression;
        }
        /// <summary>
        /// Перевод первого символа выражения в верхний регистр
        /// </summary>
        /// <param name="pExpression">Выражение в котором нужно исправить первый символ</param>
        public static string __mSymbolFirstToUpper(string pExpression)
        {
            return pExpression.Substring(0, 1).ToUpper() + pExpression.Substring(1);
        }
        /// <summary>
        /// Выделение из строки только символов в верхнем регистре
        /// </summary>
        /// <param name="pString">Строка</param>
        public static string __mSymbolsOnlyUpperCase(string pString)
        {
            string vReturn = "";
            char[] vCharS = pString.ToCharArray();
            foreach (char vChar in vCharS)
            {
                if (vChar.ToString() == vChar.ToString().ToUpper())
                    vReturn += vChar.ToString();
            }

            return vReturn;
        }
        /// <summary>
        /// Удаляет из выражения числа
        /// </summary>
        /// <param name="pText">Проверяемое выражение</param>
        public static string __mSymbolsDeleteNumbers(string pText)
        {
            return string.Join("", from ch in pText where char.IsWhiteSpace(ch) || char.IsLetter(ch) select ch);
        }
        static string __mRemoveLeadingDigits(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            int i = 0;
            while (i < text.Length && char.IsDigit(text[i]))
            {
                i++;
            }

            return text.Substring(i);
        }
        /// <summary>
        /// Удаление лишних пробелов
        /// </summary>
        /// <param name="pText">Проверяемое выражение</param>
        public static string __mSymbolsDeleteNextSpaces(string pText)
        {
            return Regex.Replace(pText, @"\s+", " ");
        }
        /// <summary>
        /// Получение специального символа
        /// </summary>
        /// <param name="pSymbolCode">Код символа</param>
        public static string __mSymbolSpecial(string pSymbolCode)
        {
            char vReturn = ' '; // Возвращаемое значение

            switch (pSymbolCode)
            {
                case "3": // ♥ - Alt+3
                    vReturn = '\u0003';
                    break;
                case "5": // ♣ - Alt+5
                    vReturn = '\u0005';
                    break;
                case "6": // ♠ - Alt+6
                    vReturn = '\u0006';
                    break;
                case "7": // • - Alt+7
                    vReturn = '\u0007';
                    break;
                case "9": // ○ - Alt+9
                    vReturn = '\u0009';
                    break;
                case "11": // ♂ - Alt+11
                    vReturn = '\u000B';
                    break;
                case "12": // ♀ - Alt+12
                    vReturn = '\u000C';
                    break;
                case "13": // ♪ - Alt+13
                    vReturn = '\u000D';
                    break;
                case "18": // ↕ - Alt+18
                    vReturn = '\u0012';
                    break;
                case "21": // § - Alt+21
                    vReturn = '\u0015';
                    break;
                case "24": // ↑ - Alt+24
                    vReturn = '\u0018';
                    break;
                case "25": // ↓ - Alt+25
                    vReturn = '\u0019';
                    break;
                case "26": // → - Alt+26
                    vReturn = '\u001A';
                    break;
                case "27": // ← - Alt+27
                    vReturn = '\u001B';
                    break;
                case "28": // ∟ - Alt+28
                    vReturn = '\u001C';
                    break;
                case "29": // ↔ - Alt+29
                    vReturn = '\u001D';
                    break;
                case "30": // ▲ - Alt+30
                    vReturn = '\u001E';
                    break;
                case "31": // ▼ - Alt+31
                    vReturn = '\u001F';
                    break;

                case "0150": // – - Alt+0150
                    vReturn = '\u0096';
                    break;
                case "0151": // — - Alt+0151
                    vReturn = '\u0097';
                    break;
                case "0152": // ˜ - Alt+0152
                    vReturn = '\u0098';
                    break;
                case "0153": // ™ - Alt+0153
                    vReturn = '\u0099';
                    break;

                case "0163": // £ - Alt+0163
                    vReturn = '\u00A3';
                    break;

                case "0169": // © - Alt+0169
                    vReturn = '\u00A9';
                    break;

                case "0171": // « - Alt+0171
                    vReturn = '\u00AB';
                    break;

                case "0174": // ® - Alt+0174 R в кружочке
                    vReturn = '\u00AE';
                    break;

                case "0187": // » - Alt+0187
                    vReturn = '\u00BB';
                    break;

                case "0190": // » - Alt+0190
                    vReturn = '\u00BE';
                    break;
            }

            return vReturn.ToString().Trim();
        }
        /// <summary>
        /// Подсчет количества символов в выражении
        /// </summary>
        /// <param name="pExpression">Выражение</param>
        /// <param name="pChar">Символ</param>
        /// <returns></returns>
        public static int __mSymbolsCount(string pExpression, string pChar)
        {
            return pExpression.ToCharArray().Where(c => c == pChar[0]).Count();
        }

        #endregion Работа с символами

        #region /// Работа с выражениями

        /// <summary>
        /// Замена одной строки на другую в проверяемом выражении
        /// </summary>
        /// <param name="pCheckedExpression">Проверяемое выражение</param>
        /// <param name="pStringChanged">Заменяемая строка</param>
        /// <param name="pStringNew">Устанавливаемая строка</param>
        public static string __mChangeStringInExpression(string pCheckedExpression, string pStringChanged, string pStringNew)
        {
            string vReturn = ""; // Возвращаемое значение
            if (pCheckedExpression.Trim().Length == 0)
                return vReturn;
            if (pStringChanged.Trim().Length == 0)
                return pCheckedExpression;
            if (pStringNew.Trim().Length == 0)
                return pCheckedExpression;

            int vStartPosition = 0;
            do
            {
                vStartPosition = __mExpressionInExpression(pCheckedExpression, pStringChanged);
                if (vStartPosition < 0)
                    return pCheckedExpression;
                else
                {
                    pCheckedExpression = pCheckedExpression.Substring(0, vStartPosition) + pStringNew + pCheckedExpression.Substring(vStartPosition + pStringChanged.Trim().Length);
                }
            }
            while (vStartPosition > 0);

            return pCheckedExpression;
        }
        /// <summary>
        /// Проверка вхождения второго выражения в первое без учета регистра
        /// </summary>
        /// <param name="pExpressionBrowsed">Просматирваемое выражение</param>
        /// <param name="pExpressionSearched">Искомое выражение</param>
        public static int __mExpressionInExpression(string pExpressionBrowsed, string pExpressionSearched)
        {
            int vReturn = -1; // Возвращаемое значение
            int vExpressionBrowsedLenght = pExpressionBrowsed.Length; // Длина просматриваемого выражения
            int vExpressionSearchedLength = pExpressionSearched.Length; // Длина искомого выражения
            if (vExpressionSearchedLength == 0)
                return vReturn;
            if (vExpressionBrowsedLenght < vExpressionSearchedLength)
                return vReturn;

            for (int vAmount = 0; vAmount <= vExpressionBrowsedLenght - vExpressionSearchedLength; vAmount++)
            {
                if (pExpressionBrowsed.Substring(vAmount, vExpressionSearchedLength).ToUpper() == pExpressionSearched.ToUpper())
                    return vAmount;
            }

            return vReturn;
        }
        /// <summary>
        /// Получение количества слов в выражении, разделенных указанным символом
        /// </summary>
        /// <param name="pExpression">Строчное выражение</param>
        /// <param name="pSeparator">Символ - разделитель</param>
        public static int __mWordCount(string pExpression, char pSeparator)
        {
            int vReturn = 0; // Возвращаемое значение
            int vPosition = 0; // Номер позиции символа разделителя слов

            if (pExpression.Length > 0)
            {
                while (true)
                {
                    vPosition = pExpression.IndexOf(pSeparator, 0);
                    if (vPosition == 0)
                    {
                        pExpression = pExpression.Substring(1);
                        vReturn = vReturn + 1;
                    }
                    if (vPosition > 0)
                    {
                        pExpression = pExpression.Substring(vPosition + 1);
                        if (pExpression.Length != 0)
                        {
                            vReturn = vReturn + 1;
                        }
                    }
                    if (vPosition < 0)
                    {
                        vReturn = vReturn + 1;
                        break;
                    }
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Получение количества слов в выражении, разделенных строчным символом
        /// </summary>
        /// <param name="pExpression">Строчное выражение</param>
        /// <param name="pSeparator">Строчный разделитель</param>
        public static int __mWordCount(string pExpression, string pSeparator)
        {
            return __mWordCount(pExpression, pSeparator.ToCharArray()[0]);
        }
        /// <summary>
        /// Получение количества слов в выражении, разделенных запятой
        /// </summary>
        /// <param name="pExpression">Строчное выражение</param>
        public static int __mWordCountComma(string pExpression)
        {
            return __mWordCount(pExpression, ',');
        }
        /// <summary>
        /// Получение количества слов в выражении, разделенных точкой
        /// </summary>
        /// <param name="pExpression">Строчное выражение</param>
        public static int __mWordCountDot(string pExpression)
        {
            return __mWordCount(pExpression, '.');
        }
        /// <summary>
        /// Получение количества слов в выражении, разделенных пробелом
        /// </summary>
        /// <param name="pExpression">Строчное выражение</param>
        public static int __mWordCountSpace(string pExpression)
        {
            return __mWordCount(pExpression, ' ');
        }
        /// <summary>
        /// Поиск строчного выражения в списке строчных выражений без учета регистра
        /// </summary>
        /// <remarks>Метод находит первое совпадение и прерывает работу</remarks>
        /// <example>bool vResult = appTypeString._WordInList("abc", "abc", "bcd"); // Вернет [true]
        /// </example>
        /// <param name="pExpression">Искомое строчное выражение</param>
        /// <param name="pExpressionList">Группа выражений среди которых ведется поиск</param>
        /// <returns>{int} Первая позиция вхождения в массив строчных значений</returns>
        public static int __mWordInList(string pExpression, params string[] pExpressionList)
        {
            int vReturn = -1; // Возвращаемое значение

            for (int vAmount = 0; vAmount < pExpressionList.Length; vAmount++)
            {
                if (pExpression.ToUpper() == pExpressionList[vAmount].ToUpper())
                {
                    vReturn = vAmount;
                    break;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Поиск слова в выражении разделенных символом
        /// </summary>
        /// <param name="pWord">Искомое слово</param>
        /// <param name="pExpression">Выражение</param>
        /// <param name="pSeparator">Символ разделитель</param>
        /// <returns>[true] - слово найдено, иначе - [false]</returns>
        public static int __mWordInList(string pWord, string pExpression, char pSeparator)
        {
            int vReturn = -1; // Возвращаемое значение
            int vWordAmount = __mWordCount(pExpression, pSeparator); // Количество слов в выражении, разделенных 'pSeparator'
            string vWordInList = ""; // Слово читаемое из списка

            for (int vAmount = 0; vAmount < vWordAmount; vAmount++)
            { /// Перебор слов в выражении
                vWordInList = __mWordNumber(pExpression, vAmount, pSeparator); /// Чтение слов из выражения 'pWordList'
                if (pWord.ToUpper() == vWordInList.ToUpper())
                {
                    vReturn = vAmount;
                    break;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Поиск строчного выражения в {ArrayList}
        /// </summary>
        /// <remarks>Метод находит первое совпадение и прерывает работу</remarks>
        /// <example>bool vResult = appTypeString._WordInList("abc", new ArrayList { "abc", "bcd" }); // Вернет [true]
        /// </example>
        /// <param name="pExpression">Искомое строчное выражение</param>
        /// <param name="pExpressionList">Группа выражений среди которых ведется поиск</param>
        /// <returns>[true] - если индекс найден, иначе - [false]</returns>
        public static bool __mWordInArrayList(string pExpression, ArrayList pExpressionList)
        {
            bool vReturn = false; // Возвращаемое значение
            foreach (string vExpression in pExpressionList)
            {
                if (vExpression == pExpression)
                {
                    vReturn = true;
                    break;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Поиск индекса первого вхождения строчного выражения в {ArrayList}
        /// </summary>
        /// <remarks>Метод находит первое совпадение и прерывает работу</remarks>
        /// <example>int vResult = appTypeString._WordInArrayList("abc", new ArrayList { "abc", "bcd" }); // Вернет [true]
        /// </example>
        /// <param name="pExpression">Искомое строчное выражение</param>
        /// <param name="pExpressionList">Группа выражений среди которых ведется поиск</param>
        public static int __mWordIndexInArrayList(string pExpression, ArrayList pExpressionList)
        {
            int vReturn = -1; // Возвращаемое значение
            int vIndex = 0;
            foreach (string vExpression in pExpressionList)
            {
                if (vExpression == pExpression)
                {
                    vReturn = vIndex;
                    break;
                }
                vIndex++;
            }

            return vReturn;
        }
        /// <summary>
        /// Получение последнего слова в выражении
        /// </summary>
        /// <param name="pExpression">Строчное выражение</param>
        /// <param name="pSeparator">Символ разделителя</param>
        public static string __mWordLast(string pExpression, char pSeparator)
        {
            string vReturn = ""; // Возвращаемое значение
            int vWordCount = __mWordCount(pExpression, pSeparator); // Количество слов в выражении
            if (vWordCount == 0)
                return "";
            if (vWordCount == 1)
                vReturn = pExpression;
            else
                vReturn = __mWordNumber(pExpression, vWordCount - 1, pSeparator);

            return vReturn;
        }
        /// <summary>
        /// Получение последнего слова в выражении
        /// </summary>
        /// <param name="pExpression">Строчное выражение.</param>
        /// <param name="pSeparator">Строка разделителя.</param>
        public static string __mWordLast(string pExpression, string pSeparator)
        {
            string vReturn = ""; // Возвращаемое значение
            int vWordCount = __mWordCount(pExpression, pSeparator); // Количество слов в выражении
            if (vWordCount == 0)
                return "";
            if (vWordCount == 1)
                vReturn = pExpression;
            else
                vReturn = __mWordNumber(pExpression, vWordCount - 1, pSeparator);

            return vReturn;
        }
        /// <summary>
        /// Поиск слова по номеру в выражении, разделенных указанным символьным разделителем
        /// </summary>
        /// <param name="pExpression">Выражение в котром ведеться поиск</param>
        /// <param name="pWordPosition">Номер искомого слова</param>
        /// <param name="pSeparator">Символ разделителя</param>
        /// <returns>Слово</returns>
        public static string __mWordNumber(string pExpression, int pWordPosition, char pSeparator)
        {
            if (pExpression.Length == 0)
            { /// Строка пустая
                return "";
            }

            string vReturn = ""; // Возвращаемое значение
            int vWordCount = __mWordCount(pExpression, pSeparator); // Количество слов в выражении
            if (pWordPosition == -1)
                return "";
            if (vWordCount < pWordPosition + 1)
                return "";
            int vIndexPosition = 0; // Индекс последней буквы искомого слова в выражении

            if (vWordCount == 1 & vWordCount == pWordPosition)
            { /// Разделители не обнаружены
                return "";
            }
            for (int vAmount = 0; vAmount <= vWordCount; vAmount++)
            {
                vIndexPosition = pExpression.IndexOf(pSeparator);
                if (vIndexPosition > 0)
                {
                    if (vAmount == pWordPosition)
                    {
                        vReturn = pExpression.Substring(0, vIndexPosition);
                        break;
                    }
                }
                else
                {
                    vReturn = pExpression;
                    // 2024.04.05 vReturn = ""; // pExpression.Substring(0, vIndexPosition);
                }
                pExpression = pExpression.Substring(vIndexPosition + 1);
            }
            vReturn = vReturn.Trim();

            return vReturn;
        }
        /// <summary>
        /// Поиск слова по номеру в выражении, разделенных указанным строчным разделителем
        /// </summary>
        /// <param name="pExpression">Выражение в котором ведеться поиск</param>
        /// <param name="pWordPosition">Номер искомого слова</param>
        /// <param name="pSeparartor">Символ разделителя</param>
        public static string __mWordNumber(string pExpression, int pWordPosition, string pSeparartor)
        {
            return __mWordNumber(pExpression, pWordPosition, pSeparartor.ToCharArray()[0]);
        }
        /// <summary>
        /// Поиск слова по номеру в выражении, разделенных запятой
        /// </summary>
        /// <param name="pExpression">Выражение в котором ведеться поиск</param>
        /// <param name="pWordPosition">Номер искомого слова</param>
        public static string __mWordNumberComma(string pExpression, int pWordPosition)
        {
            return __mWordNumber(pExpression, pWordPosition, ',');
        }
        /// <summary>
        /// Поиск слова по номеру в выражении, разделенных точкой
        /// </summary>
        /// <param name="pExpression">Выражение в котором ведеться поиск</param>
        /// <param name="pWordPosition">Номер искомого слова</param>
        public static string __mWordNumberDot(string pExpression, int pWordPosition)
        {
            return __mWordNumber(pExpression, pWordPosition, '.');
        }
        /// <summary>
        /// Поиск слова по номеру в выражении, разделенных точкой с запятой
        /// </summary>
        /// <param name="pExpression">Выражение в котором ведеться поиск</param>
        /// <param name="pWordPosition">Номер искомого слова</param>
        public static string __mWordNumberSemicolon(string pExpression, int pWordPosition)
        {
            return __mWordNumber(pExpression, pWordPosition, ';');
        }
        /// <summary>
        /// Поиск слова по номеру в выражении, разделенных пробелом
        /// </summary>
        /// <param name="pExpression">Выражение в котором ведеться поиск</param>
        /// <param name="pWordPosition">Номер искомого слова</param>
        public static string __mWordNumberSpace(string pExpression, int pWordPosition)
        {
            return __mWordNumber(pExpression, pWordPosition, ' ');
        }
        /// <summary>
        /// Получение слова начинающегося с заглавной буквы
        /// </summary>
        /// <param name="pName">Слово</param>
        public static string __mWordPersonal(string pName)
        {
            if (pName.Length > 0)
                return pName.Substring(0, 1).ToUpper() + pName.Substring(1).ToLower();
            else
                return "";
        }
        /// <summary>
        /// Возвращает массив слов в выражении разделенных разделителем 'pSeparator'
        /// </summary>
        /// <param name="pExpression"></param>
        /// <param name="pSeparator"></param>
        public static ArrayList __mWordsList(string pExpression, char pSeparator)
        {
            ArrayList vReturn = new ArrayList();
            for (int vAmount = 0; vAmount < __mWordCount(pExpression, pSeparator); vAmount++)
            {
                vReturn.Add(__mWordNumber(pExpression, vAmount, pSeparator).Trim());
            }
            return vReturn;
        }
        /// <summary>
        /// Возвращаем выражение с замененным словом указанным символом
        /// </summary>
        /// <param name="pResult">Выражение состоящее из слов - Параметр для размещения полученных данных = Выражение с замененным словом</param>
        /// <param name="pCount">Номер слова</param>
        /// <param name="pWord">Новое слово</param>
        public static string __mWordReplicate(int pCount, string pWord)
        {
            string vReturn = "";

            for (int vAmount = 0; vAmount < pCount; vAmount++)
            {
                vReturn += pWord;
            }

            return vReturn;
        }
        /// <summary>
        /// Проверка, является ли строка целым числом
        /// </summary>
        /// <param name="pExpression"></param>
        public static bool __mWordIsInt(string pExpression)
        {
            bool vReturn = false; // Возвращаемое значение
            //string vString = appTypeString.__mWordNumber(_cStringSearch.Text.Trim(), vAmount, ' ');
            int vInt; // Целый тип 
            bool vIsNum = int.TryParse(pExpression, out vInt);
            if (vIsNum)
                vReturn = true;
            return vReturn;
        }

        /// <summary>
        /// Получение ANSII кода первого символа в выражении
        /// </summary>
        /// <param name="pExpression">Строчное выражение</param>
        /// <returns></returns>
        public static int __mGetAnsii(string pExpression)
        {
            return (int)pExpression[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pExpression"></param>
        /// <returns></returns>
        public static int __mWordToHash(string pExpression)
        {
            int vReturn = -1; // Возвращаемое значение
            string LSBUF = new string(' ', 4); // REPLICATE(CHR(0), 4)
            pExpression = pExpression.PadRight(4, ' ');
            if (HashData(@pExpression, pExpression.Length, @LSBUF, 4) == 0)
            {
                vReturn = __mGetAnsii(LSBUF.Substring(0, 1)) + __mGetAnsii(LSBUF.Substring(1, 1)) * 256 + __mGetAnsii(LSBUF.Substring(2, 1)) * 65536 + __mGetAnsii(LSBUF.Substring(3, 1)) * 16777216;
            }
            
            return vReturn;
        }

        #endregion Работа с выражениями

        #region /// Работа с параметрами

        /// <summary>
        /// Выделение названия тзга и его значения из полученного параметра
        /// </summary>
        /// <param name="pParameter">Ссылка на переменную содержащую полученный параметр и для размещения названия тэга</param>
        /// <param name="pValue">Ссылка на переменную для размещения значения тэга</param>
        /// <returns>[true] - выражение найдено, иначе - [false]</returns>
        public static bool __mParameterParse(ref string pParameter, ref string pValue)
        {
            string vTag = pParameter; // Название параметра
            int vSeparatorPosition = vTag.IndexOf('=');
            if (vSeparatorPosition > 0)
            {
                pParameter = vTag.Substring(0, vSeparatorPosition).ToUpper();
                pValue = vTag.Substring(vSeparatorPosition + 1);
            }
            else
            {
                pParameter = vTag;
                pValue = "";
            }

            return true;
        }

        #endregion Работа с параметрами

        #region /// Работа с масками

        /// <summary>
        /// Сравнение текста с маской
        /// </summary>
        /// <param name="pString">Текст</param>
        /// <param name="pMask">Маска</param>
        /// <returns>[true] - имя файла соответствует маске, иначе [false]</returns>
        public static bool __mMaskFits(string pString, string pMask)
        {
            pString = pString.ToUpper();
            pMask = pMask.ToUpper();
            Regex vRegex = new Regex(pMask.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."));
            return vRegex.IsMatch(pString);
        }

        #endregion Работа с масками

        public static string __mGetHash(string pString)
        {
            var sha = new SHA1Managed();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(pString));
            return Convert.ToBase64String(hash);
        }
        /// <summary>
        /// Проверка является ли строка Guid
        /// </summary>
        /// <param name="value"></param>
        public static bool __mIsGuid(string value)
        {
            Guid x;
            return Guid.TryParse(value, out x);
        }

        /// <summary>
        /// Построение фильтра для поиска на двух языках
        /// </summary>
        /// <param name="pSearchedExpression">Искомое выражение</param>
        /// <param name="pFieldName">Имя поля в котором будет вестись поиск</param>
        /// <param name="pOrAnd">Оператор And или Or </param>
        public static string __mFilterLikeTranslate(string pSearchedExpression, string pFieldName, string pOrAnd)
        {
            string resu = String.Join(" ", pSearchedExpression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)); // Удаление лишних пробелов между словами в искомом выражении
            string[] mass = resu.Split(new char[] { ' ' }); // Массив слов в искомом выражении
            string vReturn = "";
            string fs = "";
            string lfs1 = "";
            if (mass.Length > 0 && mass.Length < 7)
            {
                int vAmount = 0; // Счетчик
                foreach (string s in mass)
                {
                    fs = s.ToUpper();
                    /// Если строка состоит только из цифр
                    if (Regex.IsMatch(fs, @"^\d+$") || String.IsNullOrWhiteSpace(__mSymbolsDeleteNumbers(fs)))
                    {
                        fs = Regex.Replace(fs, "[^A-Za-zА-Яа-я0-9()-*/]", "");
                        if (vAmount == 0)
                            vReturn = pFieldName + " Like N'%" + fs + "%' ";
                        else
                            vReturn = vReturn + pOrAnd + pFieldName + " Like N'%" + fs + "%' ";
                    }
                    else
                    {   //MessageBox.Show("КРОМЕ ЧИСЛА ЕСТЬ СИМВОЛЫ");
                        string ss = __mSymbolsDeleteNumbers(fs).Trim().Substring(0, 1);
                        if ((__mSymbolAsciiCode(ss) >= 192 & __mSymbolAsciiCode(ss) <= 223) == true)  //  русская буква
                        {
                            lfs1 = __mChangeStringInExpression(fs, "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЯЧСМИТЬБЮ()/Э-", "QWERTYUIOP[]ASDFGHJKL;ZXCVBNM,.()/.-");
                        }
                        else
                        {
                            lfs1 = __mChangeStringInExpression(fs, "QWERTYUIOP[]ASDFGHJKL;ZXCVBNM,.()/'-", "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЯЧСМИТЬБЮ()/Э-");
                            fs = Regex.Replace(fs, "[^A-Za-zА-Яа-я0-9()-*/]", "");
                        }

                        fs = fs.Replace("'", ".");
                        if (String.IsNullOrWhiteSpace(lfs1))
                            vReturn = pFieldName + "like N'%" + lfs1 + "%' ";
                        else
                            if (vAmount == 0)
                            vReturn = "(" + pFieldName + " like N'%" + fs + "%' " + " OR " + pFieldName + " like N'%" + lfs1 + "%') ";
                        else
                            vReturn = vReturn + pOrAnd + "(" + pFieldName + " like N'%" + fs + "%' " + " OR " + pFieldName + " like N'%" + lfs1 + "%') ";
                    }
                    vAmount++;
                }
                // MessageBox.Show(slike);                            
                return vReturn;
            }
            else
                return vReturn = pFieldName + "like N'%" + pSearchedExpression.Trim() + "%' ";
        }
        /// <summary>
        /// Проверка, является ли строка адресом электронной почты
        /// </summary>
        /// <param name="pEMail"></param>
        /// <returns></returns>
        public static bool __mIsValidEmail(string pEMail)
        {
            if (string.IsNullOrWhiteSpace(pEMail))
                return false;

            try
            {
                // Normalize the domain
                pEMail = Regex.Replace(pEMail, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(pEMail,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        #region Шифрование

        /// <summary>
        /// Шифрование строки
        /// </summary>
        /// <param name="pString">Строка для шифрования</param>
        /// <param name="pPassword"></param>
        /// <param name="sol"></param>
        /// <param name="pCryptographicAlgorithm">Название алгоритма шифрования</param>
        /// <param name="passIter">Число итераций для данной операции</param>
        /// <param name="initVec"></param>
        /// <param name="keySize">Размер ключа шифрования</param>
        public string __mCrypt(string pString, string pPassword, string sol = "doberman", string pCryptographicAlgorithm = "SHA1", int passIter = 2, string initVec = "a8doSuDitOz1hZe#", int keySize = 256)
        {
            /// Если строка для шифрования не указана возвращается пустое значение
            if (string.IsNullOrEmpty(pString))
                return "";

            byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
            byte[] solB = Encoding.ASCII.GetBytes(sol); // Ключевая привязка, используемая для формирования ключа
            byte[] ishTextB = Encoding.UTF8.GetBytes(pString);

            PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pPassword, solB, pCryptographicAlgorithm, passIter);
            byte[] keyBytes = derivPass.GetBytes(keySize / 8);
            RijndaelManaged symmK = new RijndaelManaged();
            symmK.Mode = CipherMode.CBC;

            byte[] cipherTextBytes = null;

            using (ICryptoTransform encryptor = symmK.CreateEncryptor(keyBytes, initVecB))
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(ishTextB, 0, ishTextB.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memStream.ToArray();
                        memStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmK.Clear();
            return Convert.ToBase64String(cipherTextBytes);
        }

        //метод дешифрования строки
        public string __mDecrypt(string ciphText, string pass, string sol = "doberman", string cryptographicAlgorithm = "SHA1", int passIter = 2, string initVec = "a8doSuDitOz1hZe#", int keySize = 256)
        {
            if (string.IsNullOrEmpty(ciphText))
                return "";

            byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
            byte[] solB = Encoding.ASCII.GetBytes(sol);
            byte[] cipherTextBytes = Convert.FromBase64String(ciphText);

            PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
            byte[] keyBytes = derivPass.GetBytes(keySize / 8);

            RijndaelManaged symmK = new RijndaelManaged();
            symmK.Mode = CipherMode.CBC;

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int byteCount = 0;

            using (ICryptoTransform decryptor = symmK.CreateDecryptor(keyBytes, initVecB))
            {
                using (MemoryStream mSt = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mSt, decryptor, CryptoStreamMode.Read))
                    {
                        byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        mSt.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmK.Clear();

            return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
        }

        #endregion Шифрование

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Скрытые

        /// <summary>
        /// Путь к файлу текущего класса
        /// </summary>
        protected string _fClassFilePath_
        {
            get { return _mClassFilePath(); }
        }
        /// <summary>
        /// Текущая процедура класса
        /// </summary>
        protected string _fClassProcedure_
        {
            get { return _mClassProcedure(""); }
        }
        /// <summary>
        /// Номер строки в файле класса
        /// </summary>
        protected int _fClassLine_
        {
            get { return _mClassLine(""); }
        }

        #endregion Скрытые

        #region - Объекты

        /// <summary>
        /// Объект ошибки
        /// </summary>
        protected static appUnitError _fError;

        #endregion Объекты

        #endregion ПОЛЯ
    }
}
