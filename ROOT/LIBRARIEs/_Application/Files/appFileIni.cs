using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System;
using System.Runtime.CompilerServices;

namespace nlApplication
{
    /// <summary>
    /// Файл appFileIni.cs
    /// </summary>
    /// <remarks>Класс для работы с инициализационными файлами</remarks>
 	/// <author>Lucasin V.</author> // автор
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.07.30 14-01</version> // Дата-время последней корректировки
    public sealed class appFileIni
    {
        #region = БИБЛИОТЕКИ

        [DllImport("kernel32", SetLastError = true)]
        static extern int WritePrivateProfileString(string Sec, string Key, string Val, string FilNam);
        [DllImport("kernel32", SetLastError = true)]
        static extern int WritePrivateProfileString(string section, string key, int value, string fileName);
        [DllImport("kernel32", SetLastError = true)]
        static extern int WritePrivateProfileString(string section, int key, string value, string fileName);
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder result, int size, string fileName);
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string section, int key, string defaultValue, [MarshalAs(UnmanagedType.LPArray)] byte[] result, int size, string fileName);
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(int section, string key, string defaultValue, [MarshalAs(UnmanagedType.LPArray)] byte[] result, int size, string fileName);
        [DllImport("kernel32", CharSet = CharSet.Auto)]
        static extern int GetPrivateProfileSectionNames(String retVal, int size, string filePath);

        #endregion БИБЛИОТЕКИ

        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public appFileIni()
        {
            /// 1. Создается объект ошибки
            _fError = new appUnitError(_fClassFilePath_);
        }
        /// <summary>
        /// Конструктор с указанием названия обрабатываемого файла
        /// </summary>
        /// <param name="pFileName">Путь и имя обрабатываемого файла</param>
        public appFileIni(string pFileName)
        {
            /// 1. Путь к обрабатываемому файлу прописывается в свойстве класса
            __fFilePath = pFileName.Trim();
            /// 2. Создается объект ошибки
            _fError = new appUnitError(_fClassFilePath_);
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Процедуры

        #region * Закрытые

        /// <summary>
        /// Получение пути и имени файла класса
        /// </summary>
        private string mClassFilePath([CallerFilePath] string pFilePath = "")
        {
            return pFilePath;
        }
        /// <summary>
        /// Получение значения номера строки в текущей процедуре
        /// </summary>
        private int mClassLine(string pMessage = "", [CallerLineNumber] int pLine = 0)
        {
            return pLine;
        }
        /// <summary>
        /// Получение названия текущей процедуры
        /// </summary>
        private string mClassProcedure(string message, [CallerMemberName] string pMember = "")
        {
            return pMember;
        }

        #endregion Закрытые

        /// <summary>
        /// Чтение значения параметра
        /// </summary>
        /// <param name="pSectionName">Имя секции</param>
        /// <param name="pKeyName">Имя параметра</param>
        /// <returns>Значение параметра.</returns>
        public string __mValueRead(string pSectionName, string pKeyName)
        {
            string vReturn = ""; // Возвращаемое значение
            const int cBuffLeng = 0x400;
            StringBuilder lStrgBuil = new StringBuilder(cBuffLeng);
            /// 1.T Попытка чтения значения параметра через WinAPI функцию "GetPrivateProfileString"
            try
            {
                GetPrivateProfileString(pSectionName, pKeyName, null, lStrgBuil, cBuffLeng, __fFilePath);
                vReturn = lStrgBuil.ToString();
            }
            /// 1.T.E Обработка исключения
            catch (Exception vException)
            {
                /// 1.T.E.1 Формирование, отображение и протоколирование ошибки
                _fError.__fErrorType_ = ERRORSTYPES.Exception;
                _fError.__fException = vException;
                _fError.__fProcedure_ = mClassProcedure("");
                _fError.__mMessageBuild("Ошибка при чтении файла");
                _fError.__mPropertyAdd("Файл: {0}", __fFilePath);
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                vReturn = "";
            }

            return vReturn;
        }
        /// <summary>
        /// Чтение значения параметра и создание его в случае отсутствия
        /// </summary>
        /// <param name="pValue">Значение записываемое в случае отстутсвия</param>
        /// <param name="pSectionName">Имя секции</param>
        /// <param name="pKeyName">Имя параметра</param>
        /// <returns>Значение параметра</returns>
        public string __mValueReadWrite(string pValue, string pSectionName, string pKeyName)
        {
            string vReturn = pValue; // Возвращаемое начение
            /// 1. Поиск параметра в секции
            bool vFind = __mParameterExist(pSectionName, pKeyName); 
            /// 2.Y Если параметр найден
            if (vFind == true)
            {
                /// 2.Y.1 Заполняется возвращаемое значение
                vReturn = __mValueRead(pSectionName, pKeyName);
            }
            /// 2.N Если параметр не найден
            else
            {
                /// 2.N.1 Записывается новое значение
                WritePrivateProfileString(pSectionName, pKeyName, pValue, __fFilePath);
            }

            return vReturn;
        }
        /// <summary>
        /// Запись нового значения параметру или создание параметра в случае его отсутствия
        /// </summary>
        /// <param name="pValue">Записываемое значение</param>
        /// <param name="pSectionName">Имя секции</param>
        /// <param name="pKeyName">Имя параметра</param>
        /// <returns>[true] - параметр записан, иначе - [false]</returns>
        public bool __mValueWrite(string pValue, string pSectionName, string pKeyName)
        {
            /// 1.T Попытка записи нового значения в файл через WinAPI функцию "WritePrivateProfileString"
            try
            {
                WritePrivateProfileString(pSectionName, pKeyName, pValue, __fFilePath);
            }
            /// 1.T.E Обработка исключения
            catch
            {
                /// 1.T.E.1 Возвращается [false]
                return false;
            }

            return true;
        }
        /// <summary>
        /// Удаление параметра в секции
        /// </summary>
        /// <param name="pSectionName">Название секции</param>
        /// <param name="pKeyName">Название параметра</param>
        /// <returns></returns>
        public bool __mParameterDelete(string pSectionName, string pKeyName)
        {
            bool vReturn = false; // Возвращаемое значение
            /// 1.Y Если удалось удалить указанное значения через WinAPI функцию "WritePrivateProfileString"
            if (WritePrivateProfileString(pSectionName, pKeyName, 0, __fFilePath) > 0)
            {
                /// 1.Y.1 Возвращается [true]
                vReturn = true;
            }
            /// 1.N Возвращается [false]

            return vReturn;
        }
        /// <summary>
        /// Получение списка параметров в секции
        /// </summary>
        /// <param name="pSectionName">Название секции</param>
        public ArrayList __mParametersList(string pSectionName)
        {
            ArrayList vReturn = new ArrayList(); // Возвращаемое значение

            for (int vMaxSize = 500; true; vMaxSize *= 2)
            {
                byte[] vByte = new byte[vMaxSize];
                /// 1. Получение списка параметров в секции через WinAPI функцию "GetPrivateProfileString"
                int vSize = GetPrivateProfileString(pSectionName, 0, "", vByte, vMaxSize, __fFilePath);
                if (vSize < vMaxSize - 2)
                {
                    /// 2. Конвертация списка в ASCII код
                    string vEnter = Encoding.ASCII.GetString(vByte, 0, vSize - (vSize > 0 ? 1 : 0));
                    /// 3.Y Если список не пустой
                    if (vEnter != "")
                    {
                        /// 3.Y.1 Чтение названия параметра и запись его в возвращаемое значение
                        int vWordCoun = appTypeString.__mWordCount(vEnter, '\0');
                        for (int vAmount = 0; vAmount < vWordCoun; vAmount++)
                        {
                            string lWord = appTypeString.__mWordNumber(vEnter, vAmount, '\0');
                            vReturn.Add(lWord);
                        }
                    }

                    break;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Получение списка параметров по вхождению маски
        /// </summary>
        /// <param name="pSectionName">Название секции</param>
        /// <param name="pMask">Маска</param>
        public ArrayList __mParametersListByMaskInput(string pSectionName, string pMask)
        {
            ArrayList vReturn = new ArrayList(); // Возвращаемое значение

            for (int vMaxSize = 500; true; vMaxSize *= 2)
            {
                byte[] vByte = new byte[vMaxSize];
                int vSize = GetPrivateProfileString(pSectionName, 0, "", vByte, vMaxSize, __fFilePath);
                if (vSize < vMaxSize - 2)
                {
                    string vEnter = Encoding.ASCII.GetString(vByte, 0, vSize - (vSize > 0 ? 1 : 0));
                    if (vEnter != "")
                    {
                        int vWordCount = appTypeString.__mWordCount(vEnter, '\0');
                        for (int vAmount = 0; vAmount < vWordCount; vAmount++)
                        {
                            string vParameter = appTypeString.__mWordNumber(vEnter, vAmount, '\0'); // Название параметра
                            if (appTypeString.__mMaskFits(vParameter, pMask) == true) /// Провера на соответствие маске
                                vReturn.Add(vParameter);
                        }
                    }

                    break;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Получение списка параметров начинающихся с маски
        /// </summary>
        /// <param name="pSectionName">Название секции</param>
        /// <param name="pMask">Маска</param>
        public ArrayList __mParametersListByMaskStart(string pSectionName, string pMask)
        {
            ArrayList vReturn = new ArrayList(); // Возвращаемое значение

            for (int vMaxSize = 500; true; vMaxSize *= 2)
            {
                byte[] vByte = new byte[vMaxSize];
                int vSize = GetPrivateProfileString(pSectionName, 0, "", vByte, vMaxSize, __fFilePath);
                if (vSize < vMaxSize - 2)
                {
                    string vEnter = Encoding.ASCII.GetString(vByte, 0, vSize - (vSize > 0 ? 1 : 0));
                    if (vEnter != "")
                    {
                        int vWordCount = appTypeString.__mWordCount(vEnter, '\0');
                        for (int vAmount = 0; vAmount < vWordCount; vAmount++)
                        {
                            string vParameter = appTypeString.__mWordNumber(vEnter, vAmount, '\0'); // Название параметра
                            if (vParameter.Length >= pMask.Length)
                            {
                                if (vParameter.Substring(0, pMask.Length) == pMask) /// Провера на соответствие маске
                                    vReturn.Add(vParameter);
                            }
                        }
                    }
                    break;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Проверка существования парметра в секции
        /// </summary>
        /// <param name="pSectionName">Имя секции</param>
        /// <param name="pKeyName">Имя искомого параметра</param>
        /// <returns>[true] - параметр существует, иначе - [false]</returns>
        public bool __mParameterExist(string pSectionName, string pKeyName)
        {
            bool vReturn = false; // Возвращаемое значение
            /// 1. Получение списка параметров в указанной секции
            ArrayList vKeyList = __mParametersList(pSectionName); // Список параметров в секции
            /// 2.F Перебор записей в списке параметров
            foreach (string vKey in vKeyList)
            {
                /// 2.F.Y Если имя искомого параметра совпадает с параметром в списке, перебор останавливается и возвращается [true]
                if (vKey.Trim().ToLower() == pKeyName.Trim().ToLower())
                {
                    vReturn = true;
                    break;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Очистка парметра от значения
        /// </summary>
        /// <param name="pSectionName">Имя секции</param>
        /// <param name="pKeyName">Имя ключа</param>
        /// <returns>[true] - параметр удален, иначе - [false]</returns>
        public bool __mParameterClear(string pSectionName, string pKeyName)
        {
            WritePrivateProfileString(pSectionName, pKeyName, "", __fFilePath);

            return true;
        }
        /// <summary>
        /// Получение списка секций в файле
        /// </summary>
        public ArrayList __mSectionList()
        {
            /// 1. Возвращаемон значение определено как пустой ArrayList
            ArrayList vReturn = new ArrayList(); // Возвращаемое значение
            string vString = new String('\0', 5000); // Пустая строка
            /// 2. Получение количества секций в файле
            int vSectionnCount = GetPrivateProfileSectionNames(vString, vString.Length, __fFilePath); // Количество секций в файле
            int vIndexNull = 0;

            vString = vString.Substring(0, vSectionnCount);
            /// 3.W Перебор списка секций в файле 
            while (vString.Length > 0)
            {
                vIndexNull = vString.IndexOf('\0');
                if (vIndexNull > 0)
                {
                    /// 3.W.1 Добавление названий секций в возвращаемое значений
                    vReturn.Add(vString.Substring(0, vIndexNull));
                    vString = vString.Substring(vIndexNull + 1);
                }
                else
                {
                    vReturn.Add(vString);
                    vString = "";
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Проверка существования секции в файле
        /// </summary>
        /// <param name="pSectionName">Имя секции</param>
        /// <returns>[true] - секция существует, иначе - [false]</returns>
        public bool __mSectionExists(string pSectionName)
        {
            /// 1. Возвращаемое значение определено как [false]
            bool vReturn = false; // Возвращаемое значение
            /// 2. Получение списка секций в файле 
            ArrayList vSectionList = __mSectionList(); // Список секций в файле
            /// 3.F Перебор секций в списке 
            foreach (string vSection in vSectionList)
            {
                /// 3.F.Y Если название секции в списке совпадает с полученным названием секции
                if (vSection.Trim().ToUpper() == pSectionName.Trim().ToUpper())
                {
                    /// 3.F.Y.1 Возвращаемому значению присваивается [true]
                    vReturn = true;
                    break;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Удаление секции
        /// </summary>
        /// <param name="pSectionName">Имя секции</param>
        public void __mSectionDelete(string pSectionName)
        {
            WritePrivateProfileString(pSectionName, 0, "", __fFilePath);
        }
        /// <summary>
        /// Чтение списка открытых ранее файлов из INI файла
        /// </summary>
        /// <param name="pSectionName">Название секции</param>
        public ArrayList __mOpenedFilesList(string pSectionName)
        {
            ArrayList vFileList = new ArrayList();
            /// 1.F Перебор записей в секции от 0 до определенного в параметрах программы номера
            for (int vIntAmount = 1; vIntAmount <= __fOpenedFilesCount; vIntAmount++)
            {
                /// 1.F.Y Если параметр с указанным номером существует
                if (__mParameterExist(pSectionName, "File" + vIntAmount) == true)
                {
                    /// 1.F.Y.1 Чтение значения параметра 
                    string vOptionString = __mValueRead(pSectionName, "File" + vIntAmount);
                    /// 1.F.Y.2 Если параметр не пустой он записывается в возвращаемое значение
                    if (vOptionString.Trim().Length > 0)
                        vFileList.Add(vOptionString);
                }
                /// 1.F.N Выход из цикла перебора
                else
                {
                    break;
                }
            }

            return vFileList;
        }
        /// <summary>
        /// Добавление последнего открытого файла в список открытых ранее файлов
        /// </summary>
        /// <param name="pSectionName">Название секции в файле</param>
        /// <param name="pFilePath">Добавляемый файл</param>
        public void __mOpenedFileAttach(string pSectionName, string pFilePath)
        {
            ArrayList vFileList = __mOpenedFilesList(pSectionName); // Текущий список открытых ранее файлов (Содержание секции - указанной формы)
            int vFileListCount = vFileList.Count; // Количество файлов в исходном списке
            int vAmount = 1; // Счетчик записанных файлов
            /// 1. Если длина пути равна нулю выполнение метода прекращается
            if (pFilePath.Length == 0)
                return;

            foreach (string vFile in vFileList)
            {
                __mParameterClear(pSectionName, "File" + vAmount.ToString());
                vAmount++;
            } /// Очистка от названий файлов открытых ранее

            vAmount = 1;
            __mValueWrite(pFilePath, pSectionName, "File1"); /// Запись последнего файла

            foreach (string vFile in vFileList)
            {
                if (vFile.Length == 0)
                {
                    
                    continue;
                }
                if (vFile.Trim().ToUpper() == pFilePath.Trim().ToUpper()) 
                {
                    //vAmount++;
                    continue;
                }
                else
                {
                    vAmount++;
                    __mValueWrite(vFile, pSectionName, "File" + (vAmount).ToString());
                }
                if (vAmount >= __fOpenedFilesCount)
                    break;
            } /// Добавление файлов открытых ранее

            return;
        }
        /// <summary>
        /// Отключение последнего открытого файла из списка открытых ранее файлов
        /// </summary>
        /// <param name="pSectionName">Название секции в файле</param>
        /// <param name="pFilePath">Добавляемый файл</param>
        public void __mOpenedFileDetach(string pSectionName, string pFilePath)
        {
            ArrayList vFileList = __mOpenedFilesList(pSectionName); // Текущий список открытых ранее файлов
            int vAmount = 1; // Счетчик записанных файлов

            foreach (string vFile in vFileList) /// Очистка от названий файлов открытых ранее
            {
                __mParameterClear(pSectionName, "File" + vAmount.ToString());
                vAmount++;
            }
            //ArrayList vFileList_New = new ArrayList(); // Новый список открытых ранее файлов
            vAmount = 1;

            //_mValueWrite(pFilePath, pSectionName, "File1"); /// Запись последнего файла
            foreach (string vFile in vFileList) /// Добавление файлов открытых ранее
            {
                if (vFile.Trim().ToUpper() == pFilePath.Trim().ToUpper()) /// Файл был открыть чуть ранее
                {
                    vAmount++;
                    continue;
                }
                else
                {
                    __mValueWrite(vFileList[vAmount - 1].ToString(), pSectionName, "File" + (vAmount + 1).ToString());
                    vAmount++;
                }
                if (vAmount >= __fOpenedFilesCount)
                    break;
            }

            return;
        }
        /// <summary>
        /// Выбор первого не пустого значения файла
        /// </summary>
        /// <param name="pSectionName"></param>
        public string __mOpenedFileGetFirst(string pSectionName)
        {
            string vReturn = "";
            ArrayList vFileList = __mOpenedFilesList(pSectionName); // Текущий список открытых ранее файлов
            int vAmount = 1; // Счетчик записанных файлов

            /// Очистка от названий файлов открытых ранее
            foreach (string vFile in vFileList)
            {
                if (vFile.Length > 0)
                {
                    vReturn = vFile.Trim();
                    break;
                }
                vAmount++;
            }

            return vReturn;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Путь и имя инициализационного файла
        /// </summary>
        public string __fFilePath = "";
        /// <summary>
        /// Ограниченное количество открытых ранее файлов из INI файла
        /// </summary>
        public int __fOpenedFilesCount = 10;

        #endregion Атрибуты

        #region - Скрытые

        /// <summary>
        /// Путь к файлу текущего класса
        /// </summary>
        protected string _fClassFilePath_
        {
            get { return mClassFilePath(); }
        }
        /// <summary>
        /// Текущая процедура класса
        /// </summary>
        protected string _fClassProcedure_
        {
            get { return mClassProcedure(""); }
        }
        /// <summary>
        /// Номер строки в файле класса
        /// </summary>
        protected int _fClassLine_
        {
            get { return mClassLine(""); }
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
