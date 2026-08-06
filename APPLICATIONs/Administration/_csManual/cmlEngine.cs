using nlApplication;
using nlSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace nlcsManual
{
    /// <summary>
    /// Файл cmlEngine.cs
    /// </summary>
    /// <remarks>Класс-движок документирования C# проекта. Выполняет полное сканирование папки проекта,
    /// разбор каждого файла *.cs классом 'cmlSourceParser', построение HTML-страниц классом
    /// 'cmlHtmlBuilder' и запись итоговой документации (по одному файлу на тип + 'index.html')
    /// в подпапку '# MANUAL' документируемого проекта</remarks>
    /// <adjustment>Реализована полная логика построения документации (была заготовка)</adjustment>
    /// <conception>Lucasin V.</conception>
    public class cmlEngine
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Выполнение главной фукции приложения: полное документирование C# проекта, расположенного
        /// по указанному пути, с формированием HTML файлов (по одному на класс/интерфейс/структуру/
        /// перечисление) и главной страницы 'index.html'
        /// </summary>
        /// <param name="pPathDirectoryProject">Путь к корневой папке документируемого проекта</param>
        /// <returns>0 - документирование выполнено успешно; -1 - путь к проекту указан не верно</returns>
        public int __mDo(string pPathDirectoryProject)
        {
            #region Объявление переменных

            int vReturn = 0; // Возвращаемое значение
            string vFolderPathManual = ""; // Путь и имя папки для размещения файла документации

            List<cmlUnitType> vTypeAllS = new List<cmlUnitType>(); // Все типы, обнаруженные в проекте
            List<string> vProtocolAllS = new List<string>(); // Все протокольные сообщения (недоработки документирования)
            Hashtable vHtmlFileNameUsedS = new Hashtable(); // Контроль уникальности имён генерируемых HTML файлов

            cmlSourceParser oParser = new cmlSourceParser();
            cmlHtmlBuilder oBuilder = new cmlHtmlBuilder();

            #endregion Объявление переменных

            #region /// Проверка существования папки проекта для документирования

            if (Directory.Exists(pPathDirectoryProject) == false)
            {
                appUnitError vError = new appUnitError();
                vError.__fErrorType_ = ERRORSTYPES.User;
                vError.__fProcedure_ = fClassNameFull + "__mDo";
                vError.__mMessageBuild("Путь '{0}' указан не верно", pPathDirectoryProject);
                cmlApplication.__oErrorsHandler.__mShow(vError);

                vReturn = -1;
                goto Exit;
            }

            #endregion Проверка существования папки проекта для документирования

            #region /// Создание папки для размещения документации проекта, если она отсутствует

            vFolderPathManual = Path.Combine(pPathDirectoryProject, "# MANUAL"); // Путь и имя папки для размещения файла документации

            if (Directory.Exists(vFolderPathManual) == false)
                Directory.CreateDirectory(vFolderPathManual);

            #endregion Создание папки для размещения документации проекта, если она отсутствует

            #region /// Удаление предыдущей документации, если она существует

            sstFileSystem oFile = new sstFileSystem();
            List<sstUnitFile> vFileOldS = oFile.__mFilesInDirectory(vFolderPathManual);
            foreach (sstUnitFile vFileUnit in vFileOldS)
            {
                File.Delete(Path.Combine(vFileUnit.__fDirectory, vFileUnit.__fName));
            }

            #endregion Удаление предыдущей документации, если она существует

            #region /// Поиск и разбор всех файлов *.cs проекта

            List<string> vFilePathS = mCollectSourceFiles(pPathDirectoryProject, vFolderPathManual);

            foreach (string vFilePath in vFilePathS)
            {
                cmlUnitParseResult vResult = oParser.__mParseFile(vFilePath);

                string vFilePathRelative = vFilePath.Length > pPathDirectoryProject.Length
                    ? vFilePath.Substring(pPathDirectoryProject.Length).TrimStart('\\', '/')
                    : vFilePath;

                foreach (cmlUnitType vType in vResult.__fTypeS)
                {
                    vType.__fFilePathRelative = vFilePathRelative;
                    vType.__fHtmlFileName = mBuildUniqueHtmlFileName(vType, vHtmlFileNameUsedS);
                    vTypeAllS.Add(vType);
                }

                if (vResult.__fProtocolS.Count > 0)
                {
                    foreach (string vMessage in vResult.__fProtocolS)
                    {
                        string vFullMessage = vFilePathRelative + " : " + vMessage;
                        vProtocolAllS.Add(vFullMessage);
                        mProtocol(vFilePathRelative, 0, vMessage);
                    }
                }
            }

            #endregion Поиск и разбор всех файлов *.cs проекта

            #region /// Формирование HTML файла документации для каждого обнаруженного типа

            foreach (cmlUnitType vType in vTypeAllS)
            {
                string vHtml = oBuilder.__mBuildTypePage(vType, vTypeAllS);
                string vFilePathOut = Path.Combine(vFolderPathManual, vType.__fHtmlFileName);
                File.WriteAllText(vFilePathOut, vHtml, Encoding.UTF8);
            }

            #endregion Формирование HTML файла документации для каждого обнаруженного типа

            #region /// Формирование главной страницы 'index.html'

            string vProjectName = new DirectoryInfo(pPathDirectoryProject).Name;
            string vIndexHtml = oBuilder.__mBuildIndexPage(vTypeAllS, vProjectName, vProtocolAllS.Count);
            File.WriteAllText(Path.Combine(vFolderPathManual, "index.html"), vIndexHtml, Encoding.UTF8);

            #endregion Формирование главной страницы 'index.html'

            #region /// Формирование протокола недоработок документирования ('Protocols.txt')

            if (vProtocolAllS.Count > 0)
            {
                File.WriteAllLines(Path.Combine(vFolderPathManual, "Protocols.txt"), vProtocolAllS, Encoding.UTF8);
            }

        #endregion Формирование протокола недоработок документирования

        Exit:
            return vReturn;
        }

        #endregion Процедуры

        #region - Функции закрытые

        /// <summary>
        /// Сбор списка путей всех документируемых файлов *.cs проекта, с исключением служебных,
        /// сгенерированных и вспомогательных файлов, не относящихся к прикладной логике проекта
        /// </summary>
        /// <param name="pPathDirectoryProject">Корневая папка документируемого проекта</param>
        /// <param name="pFolderPathManual">Папка размещения документации (исключается из сканирования)</param>
        /// <returns>Список полных путей файлов *.cs, подлежащих документированию</returns>
        private List<string> mCollectSourceFiles(string pPathDirectoryProject, string pFolderPathManual)
        {
            List<string> vReturn = new List<string>();

            string[] vExcludedDirectoryS = { "\\bin\\", "\\obj\\", "\\packages\\", "\\# MANUAL\\", "\\.git\\", "\\.vs\\" };
            string[] vExcludedFileSuffixS = { ".Designer.cs", ".designer.cs" };
            string[] vExcludedFileNameS = { "AssemblyInfo.cs" };

            foreach (string vFilePath in Directory.GetFiles(pPathDirectoryProject, "*.cs", SearchOption.AllDirectories))
            {
                string vFilePathPad = "\\" + vFilePath + "\\";

                bool vExcluded = vExcludedDirectoryS.Any(d => vFilePathPad.IndexOf(d, StringComparison.OrdinalIgnoreCase) >= 0)
                    || vExcludedFileSuffixS.Any(s => vFilePath.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                    || vExcludedFileNameS.Any(n => Path.GetFileName(vFilePath).Equals(n, StringComparison.OrdinalIgnoreCase));

                if (!vExcluded)
                    vReturn.Add(vFilePath);
            }

            return vReturn;
        }

        /// <summary>
        /// Формирование уникального имени HTML файла для документируемого типа вида
        /// 'Пространство.Имя.html'; при совпадении добавляется числовой суффикс
        /// </summary>
        /// <param name="pType">Документируемый тип</param>
        /// <param name="pUsedS">Таблица уже использованных имён файлов</param>
        /// <returns>Уникальное имя HTML файла</returns>
        private string mBuildUniqueHtmlFileName(cmlUnitType pType, Hashtable pUsedS)
        {
            string vBaseName = (pType.__fNamespace.Length > 0 ? pType.__fNamespace + "." : "") + pType.__fName;
            string vFileName = vBaseName + ".html";
            int vSuffix = 1;

            while (pUsedS.ContainsKey(vFileName.ToLower()))
            {
                vSuffix++;
                vFileName = vBaseName + "_" + vSuffix + ".html";
            }

            pUsedS[vFileName.ToLower()] = true;
            return vFileName;
        }

        /// <summary>
        /// Раскраска ключевых слов
        /// </summary>
        /// <param name="pKeyWord">Ключевое слово</param>
        /// <returns>Ключевое слово окруженное HTML тегами</returns>
        /// <remarks>Оставлено как вспомогательная утилита для будущей раскраски исходного кода
        /// в примерах использования; страницы типов ('cmlHtmlBuilder') используют собственное
        /// экранирование и в данной версии подсветку синтаксиса не применяют</remarks>
        private string mColoringKeyWord(string pKeyWord)
        {
            string vReturn = ""; // Возвращаемое значение

            switch (pKeyWord.ToLower())
            {
                /// Области видимости
                case "public":
                    vReturn = "<Font Color=\"#0066FF\"><B>public</B></Font>";
                    break;
                case "private":
                    vReturn = "<Font Color=\"#0066FF\"><B>private</B></Font>";
                    break;
                case "internal":
                    vReturn = "<Font Color=\"#0066FF\"><B>internal</B></Font>";
                    break;
                case "protected":
                    vReturn = "<Font Color=\"#0066FF\"><B>protected</B></Font>";
                    break;
                /// Порядок использования
                case "abstract":
                    vReturn = "<Font Color=\"#7766FF\"><B>static</B></Font>";
                    break;
                case "event":
                    vReturn = "<Font Color=\"#0066FF\"><B><I>event</I></B></Font>";
                    break;
                case "static":
                    vReturn = "<Font Color=\"#7766FF\"><B>static</B></Font>";
                    break;
                /// Наследственность
                case "virtual":
                    vReturn = "<Font Color=\"#4455FF\"><B>virtual</B></Font>";
                    break;
                case "override":
                    vReturn = "<Font Color=\"#4455FF\"><B>override</B></Font>";
                    break;
                /// Типы данных        
                case "arraylist":
                    vReturn = "<Font Color=\"#0066FF\"><B><I>ArrayList</I></B></Font>";
                    break;
                case "bool":
                    vReturn = "<Font Color=\"#0066FF\"><B><I>bool</I></B></Font>";
                    break;
                case "class":
                    vReturn = "<Font Color=\"#0066FF\"><B><I>class</I></B></Font>";
                    break;
                case "datetime":
                    vReturn = "<Font Color=\"#0066FF\"><B><I>datetime</I></B></Font>";
                    break;
                case "dialogresult":
                    vReturn = "<Font Color=\"#0066FF\"><B><I>DialogResult</I></B></Font>";
                    break;
                case "enum":
                    vReturn = "<Font Color=\"#0066FF\"><B><I>enum</I></B></Font>";
                    break;
                case "eventhandler":
                    vReturn = "<Font Color=\"#0066FF\"><B><I>EventHandler</I></B></Font>";
                    break;
                case "int":
                    vReturn = "<Font Color=\"#0066FF\"><B><I>int</I></B></Font>";
                    break;
                case "object":
                    vReturn = "<Font Color=\"#0066FF\"><B><I>object</I></B></Font>";
                    break;
                case "string":
                    vReturn = "<Font Color=\"#0066FF\"><B><I>string</I></B></Font>";
                    break;
                case "void":
                    vReturn = "<Font Color=\"#0066FF\"><B><I>void</I></B></Font>";
                    break;
                case "xmlnode":
                    vReturn = "<Font Color=\"#0066FF\"><B><I>XmlNode</I></B></Font>";
                    break;
                /// 
                case "params":
                    vReturn = "<Font Color=\"#0022FF\"><B><I>params</I></B></Font>";
                    break;

                default:

                    vReturn = pKeyWord;
                    break;
            }

            return vReturn;
        }
        /// <summary>
        /// Раскраска строки
        /// </summary>
        /// <param name="pLine">Содержание строки</param>
        /// <remarks>Строка окруженная HTML тэгами</remarks>
        private string mColoringLine(string pLine)
        {
            string vReturn = ""; // Возвращаемое значение
            /// Перебор слов в строке и обработка их методом ' mKeyWord(string)'

            foreach (string vWord in appTypeString.__mWordsList(pLine.Trim(), ' '))
            {
                vReturn = vReturn + mColoringKeyWord(vWord) + " ";
            }

            return vReturn;
        }
        /// <summary>
        /// Протоколоирование недоработок документируемого кода 
        /// </summary>
        /// <param name="pFileName">Относительный путь документируемого файла</param>
        /// <param name="pFileNumber">Номер строки в файле, к которой относится сообщение</param>
        /// <param name="pFileContent">Содержание протокольного сообщения</param>
        /// <param name="pErrorCharacter">Дополнительный признак/символ ошибки</param>
        private void mProtocol(string pFileName, int pFileNumber, string pFileContent, string pErrorCharacter = "")
        {
            string vMessage = pFileName + " " + pFileNumber.ToString() + " " + pFileContent + " " + pErrorCharacter;

            try
            {
                appApplication.__oProtocols.__mCreate(PROTOCOLSTYPES.ApplicationError, "");
                appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Message, vMessage);
            }
            catch
            {
                /// Протоколирование через 'appApplication' недоступно вне контекста запущенного
                /// приложения (например при модульном тестировании движка) - сообщение просто
                /// остаётся в списке 'vProtocolAllS' метода '__mDo' и попадает в 'Protocols.txt'
            }
        }

        #endregion Функции закрытые

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Служебные

        /// <summary>
        /// Полное имя класса
        /// </summary>
        private string fClassNameFull = "nlcsManual.cmlEngine.";

        #endregion Служебные

        #endregion ПОЛЯ
    }
}
