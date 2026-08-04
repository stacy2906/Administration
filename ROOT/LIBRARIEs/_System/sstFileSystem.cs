using nlApplication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace nlSystem
{
    /// <summary>
    /// Файл sysFileSystem.cs
    /// </summary>
    /// <remarks>Класс для работы с файловой системой</remarks>
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 14-56</version> // Дата-время последней корректировки
    public class sstFileSystem
    {
        #region = БИБЛИОТЕКИ

        [DllImport("kernel32.dll")]
        private static extern int GetLastError();

        [DllImport("kernel32.dll")]
        private static extern bool GetVolumeInformation(string PathName, StringBuilder VolumeNameBuffer, uint VolumeNameSize, ref uint VolumeSerialNumber, ref uint MaximumComponentLength, ref uint FileSystemFlags, StringBuilder FileSystemNameBuffer, UInt32 FileSystemNameSize);

        [DllImport("kernel32")]
        public static extern int GetShortPathName(string lpszLongPath, StringBuilder lpszShortPath, int bufSize);

        [DllImport("kernel32")]
        public static extern int GetLongPathName(string lpszShortPath, StringBuilder lpszLongPath, int bufSize);

        #endregion БИБЛИОТЕКИ

        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public sstFileSystem()
        {
            _fError = new appUnitError(_fClassFilePath_);
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ 

        #region - Процедуры

        #region * Информация о файле

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

        #endregion Информация о файле

        #region * Специальные папки

        /// <summary>
        /// Получение списка всех служебных папок
        /// </summary>
        public ArrayList __mSpecialDirectorys()
        {
            ArrayList vReturn = new ArrayList(); // Возвращаемое значение

            IEnumerable<Environment.SpecialFolder> vSpecialDirectorys = Enum.GetValues(typeof(Environment.SpecialFolder)).Cast<Environment.SpecialFolder>();
            foreach (Environment.SpecialFolder vSpecialDirectory in vSpecialDirectorys)
            {
                vReturn.Add(Environment.GetFolderPath(vSpecialDirectory, Environment.SpecialFolderOption.DoNotVerify));
            }

            return vReturn;
        }
        /// <summary>
        /// Получение пути к специальной папке
        /// </summary>
        /// <param name="pSpecialDirectory">Тип специальной папки</param>
        public string __mSpecialDirectory(Environment.SpecialFolder pSpecialDirectory)
        {
            return Environment.GetFolderPath(pSpecialDirectory);
        }

        #endregion Специальные папки

        #region * Файлы в папках

        /// <summary>
        /// Получение списка файлов в папке и во всех вложенных папках
        /// </summary>
        /// <remarks>Метод выполняется с рекурсивной обработкой</remarks>
        /// <param name="pDirectoryPath">Сканируемая папка</param>
        /// <example>List&lt;appFileUnit&gt; vList = __mFilesInDirectory(@'D:\Temp')</example>
        public List<sstUnitFile> __mFilesInDirectory(string pDirectoryPath)
        {
            List<sstUnitFile> vFileInDirectoryUnit = mGetRecursiveFiles(pDirectoryPath);

            return vFileInDirectoryUnit;
        }
        /// <summary>
        /// Рекурсивный поиск файлов в папке
        /// </summary>
        /// <param name="pDirectoryPath">Путь и имя папки</param>
        /// <returns>Список файлов в полученной папке</returns>
        private List<sstUnitFile> mGetRecursiveFiles(string pDirectoryPath)
        {
            List<sstUnitFile> vResult = new List<sstUnitFile>(); // Возвращаемое значение
            try
            {
                string[] Directorys = Directory.GetDirectories(pDirectoryPath);
                foreach (string Directory in Directorys)
                {
                    sstUnitFile vFileDirectoryUnit = new sstUnitFile();
                    vFileDirectoryUnit.__fDirectory = pDirectoryPath;

                    vResult.AddRange(mGetRecursiveFiles(Directory));
                }
                string[] files = Directory.GetFiles(pDirectoryPath);
                foreach (string filename in files)
                {
                    FileInfo vFileInfo = new FileInfo(filename);

                    sstUnitFile vFileDirectoryUnit = new sstUnitFile();
                    vFileDirectoryUnit.__fName = Path.GetFileName(filename);
                    vFileDirectoryUnit.__fDirectory = pDirectoryPath;
                    vFileDirectoryUnit.__fDateTimeCreate = File.GetCreationTime(filename);
                    vFileDirectoryUnit.__fDateTimeWrite = File.GetLastWriteTime(filename);
                    vFileDirectoryUnit.__fSize = vFileInfo.Length;
                    if (Path.GetExtension(filename) == ".exe" | Path.GetExtension(filename) == ".dll")
                    {
                        FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(filename);
                        vFileDirectoryUnit.__fVersion = myFileVersionInfo.FileVersion;
                    }
                    vResult.Add(vFileDirectoryUnit);
                }
            }
            catch { }

            return vResult;
        }

        #endregion Файлы в папках

        #region * Размер

        /// <summary>
        /// Вычисление размера файла
        /// </summary>
        /// <param name="pFilePath">Путь и имя файла</param>
        /// <returns>Строчный эквивалент размера файла</returns>
        public string __mFileStringSize(string pFilePath)
        {
            string vReturn = "-1"; // Возвращаемое значение
            /// Если файл отсутствует, формируется сообщение об ошибке.
            if (File.Exists(pFilePath) == false)
            {
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                _fError.__fProcedure_ = _fClassProcedure_;
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fHelpFileName_ = "";
                _fError.__fHelpTopic_ = "";
                _fError.__mPropertyAdd("Параметр - Путь к файлу: {0}", pFilePath);
                _fError.__mReasonAdd("Полученный файл отсутствует");
                _fError.__fMessage_ = "Не возможно измерить размер файла";
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                /// 1 Выводиться ошибка пользователю и метод завершает работу.
                return vReturn;
            }
            else
            {
                FileInfo vFileInfo = new FileInfo(pFilePath);
                long vSize = vFileInfo.Length;
                string[] vList = { "bt", "KB", "MB", "GB", "TB", "PB", "EB" };
                if (vSize == 0)
                    vReturn = "0" + vList[0];
                else
                {
                    long vLong = Math.Abs(vSize);
                    int vPlace = Convert.ToInt32(Math.Floor(Math.Log(vLong, 1024)));
                    double vNumber = Math.Round(vLong / Math.Pow(1024, vPlace), 1);
                    vReturn = (Math.Sign(vSize) * vNumber).ToString() + vList[vPlace];
                }
            }

            return vReturn;
        }

        #endregion Размер

        #region * Работа с описаниями

        /// <summary>
        /// Удаление описания файла
        /// </summary>
        /// <param name="pFilePath">Путь и имя файла</param>
        /// <returns></returns>
        public bool __mDescriptionDelete(string pFilePath)
        {
            bool vReturn = true; // Возвращаемое значение
            string vDescriptionFilePath = Path.Combine(Path.GetDirectoryName(pFilePath), "descript.ion"); // Путь и имя файла описаний
            appFileText vFileText = new appFileText(); // Объект для работы с текстовыми файлами

            /// Если файл отсутсвует, то возвращается пустое значение
            if (File.Exists(vDescriptionFilePath) == false)
            {
                return vReturn;
            }

            string[] vDescriptionFileLines = File.ReadAllLines(vDescriptionFilePath); // Список строк файла описаний
            int vArrayIndex = 0; // Индекс записи в массиве
            foreach (string vLine in vDescriptionFileLines)
            {
                string[] vWords = vLine.Split('"'); // Список слов с разделителем ["]
                /// Искомый файл найден
                if (vWords[1].ToLower() == Path.GetFileName(pFilePath).ToLower())
                {
                    vDescriptionFileLines[vArrayIndex] = null;
                }
                vArrayIndex++;
            }
            /// Перезапись массива строк
            vFileText.__mCreateFromArray(vDescriptionFilePath, vDescriptionFileLines);

            return vReturn;
        }
        /// <summary>
        /// Чтение описания файла
        /// </summary>
        /// <param name="pFilePath">Путь и имя файла</param>
        /// <returns></returns>
        public string __mDescriptionRead(string pFilePath)
        {
            string vReturn = ""; // Возвращаемое значение
            string vDescriptionFilePath = Path.Combine(Path.GetDirectoryName(pFilePath), "descript.ion"); // Путь и имя файла описаний
            /// Если файл отсутсвует, то возвращается пустое значение
            if (File.Exists(vDescriptionFilePath) == false)
            {
                return vReturn;
            }

            string[] vDescriptionFileLines = File.ReadAllLines(vDescriptionFilePath); // Список строк файла описаний
            int vArrayIndex = 0; // Индекс записи в массиве
            foreach (string vLine in vDescriptionFileLines)
            {
                string[] vWords = vLine.Split('"'); // Список слов с разделителем ["]
                if (vWords[1].ToLower() == Path.GetFileName(pFilePath).ToLower())
                { /// Искомый файл найден
                    vReturn = vLine.Substring(vLine.IndexOf('"', 1));
                }
                vArrayIndex++;
            }

            return vReturn;
        }
        /// <summary>
        /// Добавление описания файлу
        /// </summary>
        /// <param name="pFilePath">Путь и имя файла</param>
        /// <param name="pDescription">Описание файла</param>
        /// <returns>[true] - описание записано, иначе - [false] </returns>
        public bool __mDescriptionWrite(string pFilePath, string pDescription)
        {
            appFileText vFileText = new appFileText(); // Объект для работы с текстовыми файлами
            bool vReturn = true; // Возвращаемое значение
            string vDescriptionFilePath = Path.Combine(Path.GetDirectoryName(pFilePath), "descript.ion"); // Путь и имя файла описаний
            bool vFileFound = false; // Имя файла найдено в файле описаний

            /// Если файл отсутствует, то он создается
            if (File.Exists(vDescriptionFilePath) == false)
                File.Create(vDescriptionFilePath);
            /// Установка атрибута 'нормальный' для видимости файла
            File.SetAttributes(vDescriptionFilePath, FileAttributes.Normal);

            #region /// Поиск наличия файла которому пишется описание

            string[] vDescriptionFileLines = File.ReadAllLines(vDescriptionFilePath); // Список строк файла описаний
            int vArrayIndex = 0; // Индекс записи в массиве
            foreach (string vLine in vDescriptionFileLines)
            {
                string[] vWords = vLine.Split('"'); // Список слов с разделителем ["]
                /// Искомый файл найден
                if (vWords[1].ToLower() == Path.GetFileName(pFilePath).ToLower())
                {
                    /// Изменение записи в файле описаний
                    vDescriptionFileLines[vArrayIndex] = "\"" + Path.GetFileName(pFilePath) + "\" " + pDescription;
                    vFileFound = true;
                }
                vArrayIndex++;
            }

            #endregion Поиск наличия файла которому пишется описание

            /// Если файл не найден в файле описаний, добавление записи в файл описаний
            if (vFileFound == false)
            {
                vReturn = vFileText.__mWriteToEnd(vDescriptionFilePath, "\"" + Path.GetFileName(pFilePath) + "\" " + pDescription);
            }
            /// Если файл найден, выполняется перезапись массива строк
            else
            {
                vReturn = vFileText.__mCreateFromArray(vDescriptionFilePath, vDescriptionFileLines);
            }

            /// Установка атрибута 'Скрытый'
            File.SetAttributes(vDescriptionFilePath, FileAttributes.Hidden);

            return vReturn;
        }

        #endregion Работа с описаниями

        #region * Шифрование

        /// <summary>
        /// Симметричное шифрование файла простым ключом
        /// </summary>
        /// <param name="pPassword">Пароль</param>
        /// <param name="pFilePathInput">Путь к шифруемому файлу</param>
        /// <param name="pFilePathOutput">Путь к зашифрованному файлу</param>
        public void __mEncrypt(string pPassword, string pFilePathInput, string pFilePathOutput)
        {
            try
            {
                UnicodeEncoding vUnicodeEncoding = new UnicodeEncoding(); // Объект кодирования Юникод символов в UTF-16
                FileStream vFileStreamInput = new FileStream(pFilePathInput, FileMode.Open); // Поток для работы с выходным файлом
                FileStream vFileStreamOutput = new FileStream(pFilePathOutput, FileMode.Create); // Поток для работы с выходным файлом
                RijndaelManaged vRijndaelManaged = new RijndaelManaged(); // Объект базовой реализации симметричного шифрования
                byte[] vKey = vUnicodeEncoding.GetBytes(pPassword); // Массив байт пароля

                CryptoStream oCryptoStream = new CryptoStream(vFileStreamOutput
                                                             , vRijndaelManaged.CreateEncryptor(vKey, vKey)
                                                             , CryptoStreamMode.Write); // Объект шифрования в потоке

                int vByteCount; // Количество байт прочитанных из входного файла
                while ((vByteCount = vFileStreamInput.ReadByte()) != -1)
                    oCryptoStream.WriteByte((byte)vByteCount); // Запись данных в выходной файл

                vFileStreamInput.Close();
                oCryptoStream.Close();
                vFileStreamInput.Close();
            }
            catch (Exception vException)
            {
                _fError.__fMessage_ = "Шифрование файла не выполненено"; 
                _fError.__fErrorType_ = ERRORSTYPES.Exception;
                _fError.__fException = vException;
                _fError.__mPropertyAdd("Параметр - Входной файл: {0}", pFilePathInput);
                _fError.__mPropertyAdd("Параметр - Выходной файл: {0}", pFilePathOutput);
                _fError.__fProcedure_ = _fClassProcedure_;
                _fError.__fLineInProcedure_ = _fClassLine_;
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
            }

            return;
        }
        /// <summary>
        /// Encrypt a file. Симметричная криптография
        /// </summary>
        /// <param name="sourceFilename">The full path and name of the file to be encrypted</param>
        /// <param name="destinationFilename">The full path and name of the file to be output</param>
        /// <param name="password">The password for the encryption</param>
        /// <param name="salt">The salt to be applied to the password</param>
        /// <param name="iterations">The number of iterations Rfc2898DeriveBytes should use before generating the key and initialization vector for the decryption</param>
        public void __mEncryptGenerator(string sourceFilename, string destinationFilename, string password, byte[] salt, int iterations)
        {
            AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            // NB: Rfc2898DeriveBytes initialization and subsequent calls to   GetBytes   must be eactly the same, including order, on both the encryption and decryption sides.
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, iterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);

            using (FileStream destination = new FileStream(destinationFilename, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using (CryptoStream cryptoStream = new CryptoStream(destination, transform, CryptoStreamMode.Write))
                {
                    using (FileStream source = new FileStream(sourceFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        source.CopyTo(cryptoStream);
                    }
                }
            }

            return;
        }
        /// <summary>
        /// Расшифровка файла зашифрованного симетрично с простым ключом
        /// </summary>
        /// <param name="pPassword">Пароль</param>
        /// <param name="pFilePathInput">Входной файл</param>
        /// <param name="pFilePathOutput">Выходной файл</param>
        public void __mDecrypt(string pPassword, string pFilePathInput, string pFilePathOutput)
        {
            UnicodeEncoding oUnicodeEncoding = new UnicodeEncoding(); // Объект кодирования Юникод символов в UTF-16
            FileStream oFileStreamInput = new FileStream(pFilePathInput, FileMode.Open); // Поток для работы с выходным файлом
            FileStream oFileStreamOutput = new FileStream(pFilePathOutput, FileMode.Create); // Поток для работы с выходным файлом
            RijndaelManaged oRijndaelManaged = new RijndaelManaged(); // Объект базовой реализации симметричного шифрования
            byte[] vKey = oUnicodeEncoding.GetBytes(pPassword); // Массив байт пароля

            CryptoStream oCryptoStream = new CryptoStream(oFileStreamInput
                                                         , oRijndaelManaged.CreateDecryptor(vKey, vKey)
                                                         , CryptoStreamMode.Read);  // Объект шифрования в потоке

            int vByteCount;  // Количество байт прочитанных из входного файла
            while ((vByteCount = oCryptoStream.ReadByte()) != -1)
                oFileStreamOutput.WriteByte((byte)vByteCount);  // Запись данных в выходной файл

            oFileStreamOutput.Close();
            oCryptoStream.Close();
            oFileStreamInput.Close();

            return;
        }
        public void __mDecryptGenerator(string sourceFilename, string destinationFilename, string password, byte[] salt, int iterations)
        {
            AesManaged aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            // NB: Rfc2898DeriveBytes initialization and subsequent calls to   GetBytes   must be eactly the same, including order, on both the encryption and decryption sides.
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, iterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);

            using (FileStream destination = new FileStream(destinationFilename, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using (CryptoStream cryptoStream = new CryptoStream(destination, transform, CryptoStreamMode.Write))
                {
                    try
                    {
                        using (FileStream source = new FileStream(sourceFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            source.CopyTo(cryptoStream);
                        }
                    }
                    catch (CryptographicException exception)
                    {
                        if (exception.Message == "Padding is invalid and cannot be removed.")
                            throw new ApplicationException("Universal Microsoft Cryptographic Exception (Not to be believed!)", exception);
                        else
                            throw;
                    }
                }
            }

            return;
        }

        #endregion Шифрование


        /// <summary>
        /// Определяет вид элемента файловой системы
        /// </summary>
        public FILESYSTEMTYPEs __mFileSystemType(string pPath)
        {
            FILESYSTEMTYPEs vReturn = FILESYSTEMTYPEs.None;
            FileInfo fi = new FileInfo(pPath);
            if (fi.Exists)
            {
                vReturn = FILESYSTEMTYPEs.File;
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(pPath);
                if (di.Exists)
                {
                    vReturn = FILESYSTEMTYPEs.Directory;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// compares two directories using names or dates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="pCompareMode">Критерий сравнения</param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public int __mCompareDirectorys(object x, object y, string pCompareMode)
        {
            DirectoryInfo d1 = x as DirectoryInfo;
            if (d1 == null)
                throw new InvalidCastException();
            DirectoryInfo d2 = y as DirectoryInfo;
            if (d2 == null)
                throw new InvalidCastException();
            switch (pCompareMode)
            {
                case "DC":
                    return DateTime.Compare(d1.CreationTime, d2.CreationTime);
                case "DA":
                    return DateTime.Compare(d1.LastAccessTime, d2.LastAccessTime);
                case "DW":
                    return DateTime.Compare(d1.LastWriteTime, d2.LastWriteTime);
                case "NM":
                    return string.Compare(d1.FullName, d2.FullName);
                default:
                    throw new ArgumentException();
            }
        }

        public void __mVolumeInfo()
        {
            StringBuilder volname = new StringBuilder();
            StringBuilder fsname = new StringBuilder();
            uint vsnumber = 0, mslen = 0, fsflags = 0;
            bool r = GetVolumeInformation("c:\\", volname, (uint)volname.Capacity, ref vsnumber, ref mslen, ref fsflags, fsname, (uint)fsname.Capacity);

            if (r)
            {
                Console.WriteLine("File System: " + fsname.ToString());

                Console.WriteLine("Volume Name: " + volname.ToString());

                Console.WriteLine("Volume Serial Number: " + vsnumber.ToString());

                Console.WriteLine("File System Flags: " + fsflags);
            }
            else
            {
                //3=ERROR_PATH_NOT_FOUND
                //21=ERROR_NOT_READY
                int err = GetLastError();
                Console.WriteLine("GetVolumeInformation failed " + "with error code: " + err);
            }
            Console.ReadLine();
        }

        public static bool IsFileReadOnly(string filename)
        {
            var fileAttributes = File.GetAttributes(filename);
            return (fileAttributes & FileAttributes.ReadOnly) != 0;
        }

        public static void SetFileReadOnly(string filename, bool readOnly)
        {
            var fileAttributes = File.GetAttributes(filename);
            if (readOnly)
            {
                fileAttributes = fileAttributes | FileAttributes.ReadOnly;
            }
            else
            {
                fileAttributes = fileAttributes ^ FileAttributes.ReadOnly;
            }
            File.SetAttributes(filename, fileAttributes);
        }

        static void LongShortFilenames()
        {
            //executing assembly file name
            //used as an example
            string longFileName = Assembly.GetExecutingAssembly().Location;
            Console.WriteLine("Original path:\n\t{0}\n", longFileName);
            StringBuilder sb = new StringBuilder(1024);

            GetShortPathName(longFileName, sb, sb.Capacity);

            string shortFileName = sb.ToString();

            Console.WriteLine("To short form:\n\t{0}\n", shortFileName);

            GetLongPathName(shortFileName, sb, sb.Capacity);

            Console.WriteLine("Back to long form:\n\t{0}", sb.ToString());
        }

        #endregion Процедуры

        #endregion МЕТОДЫ 

        #region = ПОЛЯ

        #region - Внутренние

        // Rfc2898DeriveBytes constants: Передавать как параметры
        public readonly byte[] salt = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; // Must be at least eight bytes.  MAKE THIS SALTIER!
        public const int iterations = 1042; // Recommendation is >= 1000.

        #endregion Внутренние

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
        /// <summary>
        /// Объект ошибки
        /// </summary>
        protected appUnitError _fError;

        #endregion Скрытые

        #endregion ПОЛЯ

        #region Удаление не пустой папки

        internal enum SHFileOpEnum : uint
        {
            Move = 0x0001,
            Copy = 0x0002,
            Delete = 0x0003,
            Rename = 0x0004,
        }

        [Flags]
        internal enum SHFileOpFlags : ushort
        {
            AllowUndo = 64,
            Silent = 4,
            NoConfirmation = 0x10,
            NoErrorUI = 0x400,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SHFileOpStruct
        {
            internal IntPtr Handle;
            internal SHFileOpEnum Operation;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string Source;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string Target;
            internal SHFileOpFlags Flags;
            [MarshalAs(UnmanagedType.Bool)]
            internal bool AnyOperationsAborted;
            internal IntPtr NameMappings;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string ProgressTitle;
        }

        public static class DirectoryHelper
        {
            public static bool Delete(string path, bool toRecycleBin = false)
            {
                bool anyOperationsAborted;
                var result = Delete(path, out anyOperationsAborted, toRecycleBin);
                return result == 0 && !anyOperationsAborted;
            }
            public static int Delete(string path, out bool anyOperationsAborted, bool toRecycleBin = false)
            {
                var fileOpStruct = new SHFileOpStruct
                {
                    Handle = IntPtr.Zero,
                    Source = path,
                    Operation = SHFileOpEnum.Delete,
                    Flags = SHFileOpFlags.Silent
                                | SHFileOpFlags.NoConfirmation
                                | SHFileOpFlags.NoErrorUI
                                | (toRecycleBin ? SHFileOpFlags.AllowUndo : 0)
                };
                var result = SHFileOperation(ref fileOpStruct);
                anyOperationsAborted = fileOpStruct.AnyOperationsAborted;

                return result;
            }

            [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
            private static extern int SHFileOperation([In] ref SHFileOpStruct fileOpStruct);

        }

        #endregion
    }
}
