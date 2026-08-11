using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace nlApplication
{
    /// <summary>
    /// Файл appPathes.cs
    /// </summary>
    /// <remarks>Класс приложения для работы с путями приложения</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 10-30</version> // Дата-время последней корректировки
    public class appPathes
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public appPathes()
        {
            _fError = new appUnitError(_fClassFilePath_);
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Процедуры

        #region * Файлы

        /// <summary>
        /// Возвращает путь и имя файла текущего протокола приложения
        /// </summary>
        /// <param name="pFileExtension">Расширение файла протокола</param>
        public string __mFileProtocol(string pFileExtension = "")
        {
            DateTime vDateTime = DateTime.Now; // Текущие дата и время
            return Path.Combine(__fDirectoryProtocols_, appApplication.__fPrefix_ + "_" + appTypeDateTime.__mDateToFileName(vDateTime) + (pFileExtension.Length > 0 ? "." + pFileExtension : pFileExtension)); ; ;
        }
        /// <summary>
        /// Возврвщает путь и имя временного файла
        /// </summary>
        /// <param name="pFileExtension">Расширение файла</param>
        /// <returns></returns>
        public string __mFileTemp(string pFileExtension = "")
        {
            /// Получение уникального имени файла
            string vFileTempName = __mFileUnique(pFileExtension);
            /// Объединение папки для хранения временных файлов с уникальным именем файла 
            return Path.Combine(__fDirectoryTemp_, appApplication.__fPrefix_ + "_" + Path.GetFileName(vFileTempName));
        }
        /// <summary>
        /// Возвращает путь и имя файла для размещения настроек форм приложения 
        /// </summary>
        public virtual string __mFileFormTunes()
        {
            return Path.Combine(__fDirectoryTunes_, "forms.tun");
        }
        /// <summary>
        /// Выполняет удаление временных файлов текущего приложения
        /// </summary>
        public void __mFilesTempDelete()
        {
            /// Получение списка файлов в папке для временных файлов по маске 'префикс_*.*'
            string[] vFileList = Directory.GetFiles(__fDirectoryTemp_, appApplication.__fPrefix_ + "_*.*"); // Список файлов созданных приложением во временной папке
            int vFileDeleteCount = 0;
            /// Удаление файлов. Если удаление не получается, то оно пропускается 
            foreach (string vFile in vFileList)
            {
                try
                {
                    File.Delete(vFile);
                }
                catch { }
                vFileDeleteCount++;
            }
            /// Протоколирование количества удаленных файлов
            if (vFileDeleteCount > 0)
            {
                appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Message, appApplication.__oTunes.__mTranslate("Удалено {0} временных файлов", vFileDeleteCount.ToString()), 0);
            }
            return;
        }
        /// <summary>
        /// Возвращает путь и имя файла для размещения настроек форм приложения 
        /// </summary>
        public string __mFileTunes()
        {
            return Path.Combine(__fDirectoryTunes_, appApplication.__fProcessName_ + ".tun");
        }
        /// <summary>
        /// Возвращает уникальное имя файла
        /// </summary>
        /// <param name="pFileExtension">Расширение временного файла</param>
        public string __mFileUnique(string pFileExtension = "")
        {
            string vReturn = Path.GetRandomFileName();

            if (pFileExtension.Length > 0)
                vReturn = Path.GetFileNameWithoutExtension(vReturn) + Path.GetExtension(vReturn).Substring(1) + "." + pFileExtension;
            else
                vReturn = Path.GetFileNameWithoutExtension(vReturn) + Path.GetExtension(vReturn).Substring(1);

            return vReturn;
        }

        #endregion Файлы

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
        /// Сравнение названий двух папок и получение разницы двух названий
        /// </summary>
        /// <param name="pDirectoryLong">Полный путь. длинное название</param>
        /// <param name="pDirectoryShort">"Полный путь. Короткое название</param>
        /// <returns>Разница между двумя названиями: Вложенная папка</returns>
        public static string __mDirectoryCompare(string pDirectoryLong, string pDirectoryShort)
        {
            string vReturn = ""; // Возвращаемое значение
            int vDirectoryLongLength = pDirectoryLong.Length; // Количество символов в названии длинной папки
            for (int vCounter = 0; vCounter < vDirectoryLongLength; vCounter++)
            {
                if (vCounter > pDirectoryShort.Trim().Length)
                {
                    vReturn += pDirectoryLong.Substring(vCounter, 1);
                }
            }

            return vReturn;
        }
        public static bool __mPathIsValid(string pPath)
        {
            bool vReturn = true; // Возвращаемое значение
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fMessage_ = $"Путь '{pPath}' содержит недопустимые символы для пути";
            _fError.__fProcedure_ = "appTypeString.__mPathIsValid(string)";

            if (string.IsNullOrWhiteSpace(pPath))
            {
                _fError.__mPropertyAdd($"Значение пути не указано.");
                vReturn = false;
                goto LabelReturn; // Переход к метке LabelReturn
            }

            try
            {
                // Проверяем наличие недопустимых символов в пути
                char[] invalidPathChars = Path.GetInvalidPathChars();
                if (pPath.IndexOfAny(invalidPathChars) >= 0)
                {
                    _fError.__mPropertyAdd($"Путь '{pPath}' содержит недопустимые символы для пути.");
                    vReturn = false;
                    goto LabelReturn; // Переход к метке LabelReturn
                }

                // Проверяем наличие недопустимых символов в имени файла (если путь содержит имя файла)
                string fileName = Path.GetFileName(pPath);
                if (!string.IsNullOrEmpty(fileName))
                {
                    char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
                    if (fileName.IndexOfAny(invalidFileNameChars) >= 0)
                    {
                        _fError.__mPropertyAdd($"Имя файла в пути '{pPath}' содержит недопустимые символы.");
                        vReturn = false;
                        goto LabelReturn; // Переход к метке LabelReturn
                    }
                }

                //// Дополнительная проверка: Попытка получить полный путь.
                //// Это может отловить некоторые некорректные форматы,
                //// но не гарантирует существования.
                // string fullPath = Path.GetFullPath(pPath);
                //// Если сюда дошли, значит, синтаксически путь корректен.
                //goto LabelReturn; // Переход к метке LabelReturn
            }
            catch (ArgumentException vArgumentException) // Отлавливаем исключения, которые могут возникнуть при некорректном пути
            {
                _fError.__mPropertyAdd($"Путь '{pPath}' не является корректным (ArgumentException)");
                _fError.__fException = vArgumentException;
                vReturn = false;
                goto LabelReturn; // Переход к метке LabelReturn
            }
            catch (NotSupportedException vArgumentException) // Например, для очень длинных путей или специфических форматов
            {
                _fError.__mPropertyAdd($"Путь '{pPath}' не поддерживается (NotSupportedException)");
                _fError.__fException = vArgumentException;
                vReturn = false;
                goto LabelReturn; // Переход к метке LabelReturn
            }
            catch (PathTooLongException vArgumentException) // Путь слишком длинный
            {
                _fError.__mPropertyAdd($"Путь '{pPath}' слишком длинный (PathTooLongException)");
                _fError.__fException = vArgumentException;
                vReturn = false;
                goto LabelReturn; // Переход к метке LabelReturn
            }
        // Можно добавить другие специфические исключения

        LabelReturn:
            if (_fError.__fPropertieS_.Count > 0)
            {
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
            }
            return vReturn;
        }
        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Путь и имя папки из которой запущено приложение
        /// </summary>
        public string __fDirectoryStart = Environment.CurrentDirectory;

        #endregion Атрибуты

        #region - Закрытые

        /// <summary>
        /// Путь и имя папки для размещения файлов помощи
        /// </summary>
        private string fDirectoryHelp = "";
        /// <summary>
        /// Путь и имя папки для размещения файлов курсоров
        /// </summary>
        private string fDirectoryCursors = "";
        /// <summary>
        /// Путь и имя папки для размещения файлов протоколов
        /// </summary>
        private string fDirectoryProtocols = "";
        private string fDirectoryDatabases = "";
        /// <summary>
        /// Путь и имя папки для размещения файлов изображений протоколов
        /// </summary>
        private string fDirectoryProtocolsImages = "";
        /// <summary>
        /// Путь и имя папки для размещения файлов отчетов
        /// </summary>
        private string fDirectoryReports = "";
        /// <summary>
        /// Путь и имя папки для временных файлов
        /// </summary>
        private string fDirectoryTemp = "";
        /// <summary>
        /// Путь и имя папки для файлов настроек
        /// </summary>
        private string fDirectoryTunes = "";

        #endregion Закрытые

        #region - Объекты

        /// <summary>
        /// Объект ошибки
        /// </summary>
        protected static appUnitError _fError;

        #endregion Объекты

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

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        #region Папки

        /// <summary>
        /// Путь и имя папки для размещения файлов помощи
        /// </summary>
        public string __fDirectoryHelp_
        {
            get
            {
                if (fDirectoryHelp.Length == 0)
                {
                    fDirectoryHelp = Path.Combine(__fDirectoryStart, @"HELP\");
                }
                try
                {
                    if (Directory.Exists(fDirectoryHelp) == false)
                    {
                        Directory.CreateDirectory(fDirectoryHelp);
                    }
                }
                catch
                {
                    fDirectoryHelp = Path.Combine(__fDirectoryStart, @"HELP\");
                    _fError.__fErrorType_ = ERRORSTYPES.User;
                    _fError.__fProcedure_ = "__fDirectoryHelp_";
                    _fError.__mReasonAdd("Не верный путь в файле настроек");
                    _fError.__fMessage_ = appApplication.__oTunes.__mTranslate("Не верно указан путь к папке с файлами помощи");
                    appApplication.__oErrorsHandler.__mProtocol(_fError);
                    _fError.__mClear();
                }

                return fDirectoryHelp;
            }
            set { fDirectoryHelp = value.Trim(); }
        }
        /// <summary>
        /// Путь и имя папки для размещения файлов курсоров
        /// </summary>
        public string __fDirectoryCursors_
        {
            get
            {
                if (fDirectoryCursors.Length == 0)
                {
                    fDirectoryCursors = Path.Combine(__fDirectoryStart, @"CURSORs\");
                }
                try
                {
                    if (Directory.Exists(fDirectoryCursors) == false)
                    {
                        Directory.CreateDirectory(fDirectoryCursors);
                    }
                }
                catch
                {
                    fDirectoryCursors = Path.Combine(__fDirectoryStart, @"CURSORs\");
                    _fError.__fErrorType_ = ERRORSTYPES.User;
                    _fError.__fProcedure_ = "__fDirectoryCursors_";
                    _fError.__mReasonAdd("Не верный путь в файле настроек");
                    _fError.__fMessage_ = appApplication.__oTunes.__mTranslate("Не верно указан путь к папке с файлами протоколов");
                    appApplication.__oErrorsHandler.__mProtocol(_fError);
                    _fError.__mClear();
                }

                return fDirectoryCursors;
            }
            set { fDirectoryCursors = value.Trim(); }
        }
        /// <summary>
        /// Путь и имя папки для размещения файлов протоколов
        /// </summary>
        public string __fDirectoryProtocols_
        {
            get
            {
                if (fDirectoryProtocols.Length == 0)
                {
                    fDirectoryProtocols = Path.Combine(__fDirectoryStart, @"PROTOCOLs\");
                }
                try
                {
                    if (Directory.Exists(fDirectoryProtocols) == false)
                    {
                        Directory.CreateDirectory(fDirectoryProtocols);
                    }
                }
                catch
                {
                    fDirectoryProtocols = Path.Combine(__fDirectoryStart, @"PROTOCOLs\");
                    _fError.__fErrorType_ = ERRORSTYPES.User;
                    _fError.__fProcedure_ = "__fDirectoryProtocols_";
                    _fError.__mReasonAdd("Не верный путь в файле настроек");
                    _fError.__fMessage_ = appApplication.__oTunes.__mTranslate("Не верно указан путь к папке с файлами протоколов");
                    appApplication.__oErrorsHandler.__mProtocol(_fError);
                    _fError.__mClear();
                }

                return fDirectoryProtocols;
            }
            set { fDirectoryProtocols = value.Trim(); }
        }
        /// <summary>
        /// Путь и имя папки для размещения файлов баз данных ('Databases')
        /// </summary>
        public string __fDirectoryDatabases_
        {
            get
            {
                if (fDirectoryDatabases.Length == 0)
                {
                    fDirectoryDatabases = Path.Combine(__fDirectoryStart, @"Databases\");
                }
                try
                {
                    if (Directory.Exists(fDirectoryDatabases) == false)
                    {
                        Directory.CreateDirectory(fDirectoryDatabases);
                    }
                }
                catch
                {
                    fDirectoryDatabases = Path.Combine(__fDirectoryStart, @"Databases\");
                    _fError.__fErrorType_ = ERRORSTYPES.User;
                    _fError.__fProcedure_ = "__fDirectoryDatabases_";
                    _fError.__mReasonAdd("Не верный путь в файле настроек");
                    _fError.__fMessage_ = appApplication.__oTunes.__mTranslate("Не верно указан путь к папке с файлами баз данных");
                    appApplication.__oErrorsHandler.__mProtocol(_fError);
                    _fError.__mClear();
                }

                return fDirectoryDatabases;
            }
            set { fDirectoryDatabases = value.Trim(); }
        }
        /// <summary>
        /// Путь и имя папки для размещения файлов протоколов
        /// </summary>
        public string __fDirectoryProtocolsImages_
        {
            get
            {
                if (fDirectoryProtocolsImages.Length == 0)
                {
                    fDirectoryProtocolsImages = Path.Combine(__fDirectoryProtocols_, @"IMAGEs\");
                }
                try
                {
                    if (Directory.Exists(fDirectoryProtocolsImages) == false)
                    {
                        Directory.CreateDirectory(fDirectoryProtocolsImages);
                    }
                }
                catch
                {
                    fDirectoryProtocolsImages = Path.Combine(__fDirectoryProtocols_, @"IMAGEs\");
                    _fError.__fErrorType_ = ERRORSTYPES.User;
                    _fError.__fProcedure_ = "__fDirectoryProtocolsImages_";
                    _fError.__mReasonAdd("Не верный путь в файле настроек");
                    _fError.__fMessage_ = appApplication.__oTunes.__mTranslate("Не верно указан путь к папке с файлами изображения для протоколов");
                    appApplication.__oErrorsHandler.__mProtocol(_fError);
                    _fError.__mClear();
                }

                return fDirectoryProtocolsImages;
            }
            set { fDirectoryProtocolsImages = value.Trim(); }
        }
        /// <summary>
        /// Путь и имя папки для размещения файлов протоколов
        /// </summary>
        public string __fDirectoryReports_
        {
            get
            {
                if (fDirectoryReports.Length == 0)
                {
                    fDirectoryReports = Path.Combine(__fDirectoryStart, @"REPORTs\");
                }
                try
                {
                    if (Directory.Exists(fDirectoryReports) == false)
                    {
                        Directory.CreateDirectory(fDirectoryReports);
                    }
                }
                catch
                {
                    fDirectoryReports = Path.Combine(__fDirectoryStart, @"REPORTs\");
                    _fError.__fErrorType_ = ERRORSTYPES.User;
                    _fError.__fProcedure_ = "__fDirectoryReports_";
                    _fError.__mReasonAdd("Не верный путь в файле настроек");
                    _fError.__fMessage_ = appApplication.__oTunes.__mTranslate("Не верно указан путь к папке с файлами отчетов");
                    appApplication.__oErrorsHandler.__mProtocol(_fError);
                    _fError.__mClear();
                }

                return fDirectoryReports;
            }
            set { fDirectoryReports = value.Trim(); }
        }
        /// <summary>
        /// Путь и имя папки для размещения временных файлов 
        /// </summary>
        public string __fDirectoryTemp_
        {
            get
            {
                if (fDirectoryTemp.Length == 0)
                {
                    fDirectoryTemp = Path.Combine(__fDirectoryStart, @"TEMP\");
                }
                try
                {
                    if (Directory.Exists(fDirectoryTemp) == false)
                    {
                        Directory.CreateDirectory(fDirectoryTemp);
                    }
                }
                catch
                {
                    fDirectoryHelp = Path.Combine(__fDirectoryStart, @"TEMP\");
                    _fError.__fErrorType_ = ERRORSTYPES.User;
                    _fError.__fProcedure_ = "__fDirectoryTemp_";
                    _fError.__mReasonAdd("Не верный путь в файле настроек");
                    _fError.__fMessage_ = appApplication.__oTunes.__mTranslate("Не верно указан путь к папке с временными файлами");
                    appApplication.__oErrorsHandler.__mProtocol(_fError);
                    _fError.__mClear();
                }

                return fDirectoryTemp;
            }
            set { fDirectoryTemp = value.Trim(); }
        }
        /// <summary>
        /// Путь и имя папки для размещения файлов настроек 
        /// </summary>
        public string __fDirectoryTunes_
        {
            get
            {
                if (fDirectoryTunes.Length == 0)
                {
                    fDirectoryTunes = Path.Combine(__fDirectoryStart, @"TUNEs\");
                }
                try
                {
                    if (Directory.Exists(fDirectoryTunes) == false)
                    {
                        Directory.CreateDirectory(fDirectoryTunes);
                    }
                }
                catch
                {
                    fDirectoryTunes = Path.Combine(__fDirectoryStart, @"TUNEs\");
                    _fError.__fErrorType_ = ERRORSTYPES.User;
                    _fError.__fProcedure_ = "__fDirectoryTunes_";
                    _fError.__mReasonAdd("Не верный путь в файле настроек");
                    _fError.__fMessage_ = appApplication.__oTunes.__mTranslate("Не верно указан путь к папке с файлами настроек");
                    appApplication.__oErrorsHandler.__mProtocol(_fError);
                    _fError.__mClear();
                }

                return fDirectoryTunes;
            }
            set { fDirectoryTunes = value.Trim(); }
        }

        #endregion Папки

        #region Файлы

        /// <summary>
        /// Путь и имя файла текущего протокола приложения
        /// </summary>
        public string __fFileProtocol_
        {
            get
            {
                DateTime vDateTime = DateTime.Now; // Текущие дата и время
                return Path.Combine(__fDirectoryProtocols_, appApplication.__fPrefix_ + "_" + appTypeDateTime.__mDateToFileName(vDateTime) + ".pcl");
            }
        }

        #endregion Файлы

        #endregion СВОЙСТВА
    }
}