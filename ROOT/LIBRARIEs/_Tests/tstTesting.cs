using nlApplication;
using System;

namespace nlTests
{
    /// <summary>
	/// Файл avpFormMain.cs
	/// </summary>
	/// <remarks>Класс-главная форма</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 16-51</version> // Дата-время последней корректировки
    public class tstTesting 
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Сревнение результата выполнения метода с ожидаемым результатом
        /// </summary>
        /// <param name="pValueExpected">Результат выполнения метода</param>
        /// <param name="pValueActual">Ожидаемый результат</param>
        /// <param name="pProcedureName">Название проверяемой процедуры с типами параметров</param>
        /// <returns>Результат проверки</returns>
        public bool __mCompare(object pValueExpected, object pValueActual, string pProcedureName)
        {
            Type vTypeExpected = pValueExpected.GetType();
            Type vTypeActual = pValueActual.GetType();
            fCyclesCount++;
            /// Сравнение типов данных
            /// Если типы данных совпадают
            if (vTypeExpected == vTypeActual)
            {
                fResult &= true;
                /// * Сравниваются типы:
                /// *- 'DateTime'
                if (vTypeActual == typeof(System.DateTime))
                {
                    if (Convert.ToDateTime(pValueExpected) == Convert.ToDateTime(pValueActual))
                        fResult &= true;
                    else
                        fResult &= false;
                }
                /// *- 'Int32'
                if (vTypeActual == typeof(System.Int32))
                {
                    if (Convert.ToInt32(pValueExpected) == Convert.ToInt32(pValueActual))
                        fResult &= true;
                    else
                        fResult &= false;
                }
                /// *- 'String'
                if (vTypeActual == typeof(System.String))
                {
                    if (Convert.ToString(pValueExpected) == Convert.ToString(pValueActual))
                        fResult &= true;
                    else
                        fResult &= false;
                }
            }
            /// Иначе возвращается [false]
            else
                fResult &= false;
            /// Выполняется протоколирование сравнения
            __mProtocol(pProcedureName.Trim().PadRight(90, '.') + (fResult == true & __mResult() == true ? "OK" : "Error"));
            /// Возвращается результат сравнения
            return fResult;
        }
        /// <summary>
        /// Протоколирование и вывод в консоль результата проверки 
        /// </summary>
        /// <param name="pMessage"></param>
        public void __mProtocol(string pMessage)
        {
            /// Протоколирование результат проверки
            //appApplication.__oProtocols.__mCreate(PROTOCOLSTYPES.UserMessage, __fCurrentProcedure_, false);
            appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Message, pMessage);
            /// Вывод в консоль результата проверки
            Console.WriteLine(pMessage);
        }
        /// <summary>
        /// Возвращает итоговый результат тестирования результатов выполнения метода и готовит класс к обработке следующего метода 
        /// </summary>
        /// <returns>Результат тестирования</returns>
        public bool __mResult()
        {
            bool vResult = fResult;
            fResult = true;
            /// Сброс счетчика циклов проверки
            fCyclesCount = 0;
            /// Возвращение результат проверки
            return vResult;
        }
        /// <summary>
        /// Проверка - является ли значение пустым
        /// </summary>
        /// <param name="pValueExpected">Проверяемое значение</param>
        /// <param name="pProcedureName">Название проверяемой процедуры с типами параметров</param>
        /// <returns>Результат проверки</returns>
        public bool __mValueIsEmpty(object pValueExpected, string pProcedureName)
        {
            Type vTypeExpected = pValueExpected.GetType();
            fCyclesCount++;
            DateTime vDateEmpty = DateTime.MinValue;
            bool vTypeFound = false; // Признак - полученный тип обработан
            /// Проверяются типы:
            /// - 'DateTime'
            if (vTypeExpected == typeof(System.DateTime))
            {
                if (Convert.ToDateTime(pValueExpected) == vDateEmpty)
                    fResult &= true;
                else
                    fResult &= false;

                vTypeFound = true;
            }
            /// - 'Int32'
            if (vTypeExpected == typeof(System.Int32))
            {
                if (Convert.ToInt32(pValueExpected) == 0)
                    fResult &= true;
                else
                    fResult &= false;

                vTypeFound = true;
            }
            /// - 'String'
            if (vTypeExpected == typeof(System.String))
            {
                if (String.IsNullOrEmpty(pValueExpected.ToString()) == true)
                    fResult &= true;
                else
                    fResult &= false;
                vTypeFound = true;
            }
            /// Если проверяемое значение другого типа возвращается [false]
            if (vTypeFound == false)
                fResult = false;
            /// Выполняется протоколирование сравнения
            __mProtocol(pProcedureName.Trim().PadRight(90, '.') + (fResult == true & __mResult() == true ? "OK" : "Error"));
            /// Возвращается результат сравнения
            return true;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Закрытые

        /// <summary>
        /// Количество циклов проверки
        /// </summary>
        private int fCyclesCount = 0;
        /// <summary>
        /// Результат проверки
        /// </summary>
        private bool fResult = true;

        #endregion Закрытые

        #endregion ПОЛЯ
    }
}
