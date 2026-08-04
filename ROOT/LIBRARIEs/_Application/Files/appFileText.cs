using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace nlApplication
{
    /// <summary>
    /// Файл appFileText.cs
    /// </summary>
    /// <remarks>Класс для работы с текстовыми файлами</remarks>
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 14-02</version> // Дата-время последней корректировки
    public sealed class appFileText
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public appFileText()
        {
            _fError = new appUnitError();
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

        /// <summary>
        /// Создание файла из строчного массива
        /// </summary>
        /// <remarks>Если массив строк пустой файл будет удален</remarks>
        /// <param name="pFilePath">Путь и имя файла</param>
        /// <param name="pFileLines">Массив строк файла</param>
        /// <returns>[true] - Файл создан, иначе - [false]</returns>
        public bool __mCreateFromArray(string pFilePath, string[] pFileLines)
        {
            bool vReturn = true; // Возвращаемое значение

            File.Delete(pFilePath); /// Удаление файла

            foreach (string vLine in pFileLines)
            {
                if (vLine != null)
                    __mWriteToEnd(pFilePath, vLine);
            }

            return vReturn;
        }
        /// <summary>
        /// Поиск выражения в файле
        /// </summary>
        /// <param name="pFilePath">Путь и имя проверяемого файла</param>
        /// <param name="pSearchedExpression">Искомое строчное выражение</param>
        /// <returns>[true] - выражение найдено, иначе - [false]</returns>
        public bool _mSearchExpression(string pFilePath, string pSearchedExpression)
        {
            bool vReturn = false;
            /// > Если файл не существует, возвращается [false]. 
            if (File.Exists(pFilePath) == false)
                return false;

            StreamReader vStreamReader = new StreamReader(pFilePath, Encoding.Default);
            while (!vStreamReader.EndOfStream)
            {
                string vLine = vStreamReader.ReadLine();
                if (appTypeString.__mExpressionInExpression(vLine, pSearchedExpression) >= 0)
                {
                    vReturn = true;
                    break;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Запись строки в конец файла
        /// </summary>
        /// <remarks>Строка добавляется в файл с новой строки</remarks>
        /// <param name="pFilePath">Путь и имя файла в который идет запись</param>
        /// <param name="pString">Записываемая строка</param>
        /// <returns>[true] - запись добавлена, иначе - [false]</returns>
        public bool __mWriteToEnd(string pFilePath, string pString)
        {
            FileStream vFileStream;

            try
            {
                vFileStream = new FileStream(pFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
            catch (Exception vException)
            {
                _fError.__mMessageBuild("Не возможно создать файл");
                _fError.__mPropertyAdd("параметр - файл: {0}", pFilePath);
                _fError.__fException = vException;
                _fError.__fProcedure_ = "_WriteToEnd(string, string)";
                _fError.__fErrorType_ = ERRORSTYPES.Exception;
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return false;
            }
            if (vFileStream != null & pString.Length > 0)
            {
                string vLineContent = pString + Environment.NewLine;
                int vLineLength = vLineContent.Length;
                byte[] vByteLine = new byte[vLineLength];
                vFileStream.Seek(0, SeekOrigin.End);
                vFileStream.Write(Encoding.Default.GetBytes(vLineContent), 0, vLineLength);
                vFileStream.Close();
            }

            return true;
        }

        ///// <summary>
        ///// Перезапись из файла в файл
        ///// </summary>
        //public bool __mFileToFile()
        //{
        //    bool vReturn = true;

        //    ////Читаем текст в файле
        //    //var Reader = new System.IO.StreamReader(@"C:/путь_к_файлу/",
        //    //    System.Text.Encoding.GetEncoding(1251));

        //    //Reader.ReadToEnd();//Читать весь файл
        //    //Reader.Close();//Закрыть. ОБЯЗАТЕЛЬНО!

        //    ////Записываем текст в файл
        //    //var Writer = new System.IO.StreamWriter(@"C:/куда_надо_записывать", true,
        //    //    System.Text.Encoding.GetEncoding(1251));

        //    //Writer.Write();//Записать
        //    //Reader.Close();//Закрыть. ОБЯЗАТЕЛЬНО!

        //    return vReturn;
        //}

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Скрытые

        /// <summary>
        /// Путь к файлу текущего класса
        /// </summary>
        public string _fClassFilePath_
        {
            get { return _mClassFilePath(); }
        }
        /// <summary>
        /// Текущая процедура класса
        /// </summary>
        public string _fClassProcedure_
        {
            get { return _mClassProcedure(""); }
        }
        /// <summary>
        /// Номер строки в файле класса
        /// </summary>
        public int _fClassLine_
        {
            get { return _mClassLine(""); }
        }

        #endregion Скрытые

        #region - Объекты

        /// <summary>
        /// Объект ошибки
        /// </summary>
        protected appUnitError _fError;

        #endregion Объекты

        #endregion ПОЛЯ
    }
}
