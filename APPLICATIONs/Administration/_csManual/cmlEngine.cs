using nlApplication;
using nlSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace nlcsManual
{
    /// <summary>
    /// Файл cmlEngine.cs
    /// </summary>
    /// <remarks>Класс-движок документирования C# проекта</remarks>
    /// <adjustment></adjustment>
    /// <conception>Lucasin V.</conception>
    public class cmlEngine
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Выполнение главной фукции приложения
        /// </summary>
        /// <param name="pPathDirectoryProject"></param>
        /// <returns></returns>
        public int __mDo(string pPathDirectoryProject)
        {
            #region Объявление переменных

            int vReturn = 0; // Возвращаемое значение
            string vFolderPathManual = ""; // Путь и имя папки для размещения файла документации

            ArrayList vClassUsingS = new ArrayList(); // Массив подключаемых библиотек

            ArrayList vAttributesAuthorS = new ArrayList();
            ArrayList vAttributesExampleS = new ArrayList();
            ArrayList vAttributesParamS = new ArrayList();
            ArrayList vAttributesRemarkS = new ArrayList();
            ArrayList vAttributesReturnS = new ArrayList();
            ArrayList vAttributesSummarieS = new ArrayList();

            #endregion Объявление переменных

            #region /// Проверка существования папки проекта для документирования

            if (Directory.Exists(pPathDirectoryProject) == false)
            {
                appUnitError vError = new appUnitError();
                vError.__fErrorType_ = ERRORSTYPES.User;
                vError.__fProcedure_ = fClassNameFull + "__mManualing";
                vError.__mMessageBuild("Путь '{0}' указан не верно", pPathDirectoryProject);
                cmlApplication.__oErrorsHandler.__mShow(vError);

                goto Exit;
            }

            #endregion Проверка существования папки проекта для документирования

            #region /// Создание папки для размещения документации проекта, если она отсутствует

            vFolderPathManual = Path.Combine(pPathDirectoryProject, "# MANUAL"); // Путь и имя папки для размещения файла документации

            if (Directory.Exists(vFolderPathManual) == false)
                Directory.CreateDirectory(vFolderPathManual);

            #endregion Создание папки для размещения документации проекта, если она отсутсвует

            #region /// Удаление предыдущей документации, если она существует

            sstFileSystem oFile = new sstFileSystem();
            List<sstUnitFile> vFileOldS = oFile.__mFilesInDirectory(vFolderPathManual);
            foreach (sstUnitFile vFileUnit in vFileOldS)
            {
                File.Delete(Path.Combine(vFileUnit.__fDirectory, vFileUnit.__fName));
            }

        #endregion Удаление предыдущей документации, если она существует

        Exit:
            return vReturn;
        }

        #region Закрытые

        /// <summary> 
        /// Раскраска ключевых слов
        /// </summary>
        /// <param name="pKeyWord">Ключевое слово</param>
        /// <returns>Ключевое слово окруженное HTML тегами</returns>
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
        /// <param name="pMessage">Протоколированое сообщение</param>
        private void mProtocol(string pFileName, int pFileNumber, string pFileContent, string pErrorCharacter = "")
        {
            string vMessage = pFileName + " " + pFileNumber.ToString() + " " + pFileContent + " " + pErrorCharacter;
            appApplication.__oProtocols.__mCreate(PROTOCOLSTYPES.ApplicationError, "");
            appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Message, vMessage);
        }

        #endregion Закрытые

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Служебные

        /// <summary>
        /// Полное имя класса
        /// </summary>
        private string fClassNameFull = "";

        #endregion Служебные

        #endregion ПОЛЯ
    }
}
