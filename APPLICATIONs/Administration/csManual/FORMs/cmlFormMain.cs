using nlApplication;
using nlcsManual;
using nlElements;
using System;
using System.IO;
using System.Windows.Forms;

namespace naCsManual
{
    /// <summary>
    /// Файл cmlFormMain.cs
    /// </summary>
    /// <remarks>Класс - Главная форма приложения 'CsManual'</remarks>
    public class cmlFormMain : elmForm
    {
        #region = МЕТОДЫ

        #region - Объект

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Размещение компонентов

            Controls.Add(_cBlockFormMain);
            _cBlockFormMain.Controls.Add(_cBlockInput);
            _cBlockFormMain.Controls.SetChildIndex(_cBlockInput, 0);
            _cBlockFormMain.Controls.Add(_cToolbar);
            _cBlockFormMain.Controls.SetChildIndex(_cToolbar, 1);
            _cToolbar.Items.Add(_cButtonRun);
            _cToolbar.Items.Add(_cButtonHelp);
            _cBlockInput.__mInputAdd(_cFolderScan);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            __mCaptionBuilding("Документирование CS проектов");

            ShowInTaskbar = true;

            // _cButtonRun
            {
                _cButtonRun.Image = global::nlResourcesImages.Properties.Resources._Sign_Tick_g32;
                _cButtonRun.Click += _cButtonRun_Click;
            }
            // _cButtonHelp
            {
                _cButtonHelp.Image = global::nlResourcesImages.Properties.Resources._Sign_Question_b32;
                _cButtonHelp.Click += _cButtonHelp_Click;
            }
            // _cBlockInput
            {
                _cBlockInput.Dock = DockStyle.Fill;
            }
            // _cFolderScan
            {
                _cFolderScan.__fCaption_ = "Путь к проекту";
                _cFolderScan.__fPathType_ = PATHTYPES.Directory;
                _cFolderScan.__fMarkVisible_ = false;
            }

            #endregion Настройка компонентов

            ResumeLayout();

            Type vType = this.GetType();
            Name = vType.Name;
            _fClassNameFull = vType.FullName + ".";

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();
            _cFolderScan.__mItemsLoadFromFile("scan");
            _cFolderScan.__fSymbolsCount_ = -1;
        }

        #endregion Объект

        #region - Поведение

        /// <summary>
        /// Выполняется при выборе кнопки 'Помощь'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cButtonHelp_Click(object sender, EventArgs e)
        {
            __mHelp();

            return;
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Выполнить'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cButtonRun_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(_cFolderScan.__fValue_.ToString()) == true)
            {
                /// Сохранение папок проектов C#
                _cFolderScan.__mItemsSaveToFile("scan");

                string vPathProject = _cFolderScan.__fValue_.ToString().Trim();

                /// Курсор ожидания на время выполнения документирования (может занять время на больших проектах)
                Cursor.Current = Cursors.WaitCursor;
                cmlEngine vDocumating = new cmlEngine();
                int vResult = vDocumating.__mDo(vPathProject);
                Cursor.Current = Cursors.Default;

                /// 1.T Документирование выполнено успешно - отображение результата в форме предварительного просмотра
                if (vResult == 0)
                {
                    string vPathIndex = Path.Combine(Path.Combine(vPathProject, "# MANUAL"), "index.html");

                    if (File.Exists(vPathIndex) == true)
                    {
                        /// Отображение статуса выполнения в строке состояния главной формы
                        __cPanelStatus.__fCaption_ = "Документация сформирована: " + vPathIndex;

                        /// Отображение сформированной документации внутри приложения (вместо внешнего браузера)
                        /// '__fUrl_' указывает, ГДЕ расположен отчет; форма загружает и показывает его содержимое
                        elmFormReportPreview vFormReportPreview = new elmFormReportPreview();
                        vFormReportPreview.__fCaption_ = "Просмотр отчета - " + vPathIndex;
                        vFormReportPreview.__cAreaReportPreview.__fUrl_ = vPathIndex;
                        vFormReportPreview.ShowDialog();
                    }
                    else
                    {
                        cmlApplication.__oMessages.__mShow(MESSAGESTYPES.Info, "Документация сформирована",
                            "Файл: " + vPathIndex, "_cButtonRun_Click(object, EventArgs)");
                    }
                }
                /// 1.E Путь указан верно, но документирование не выполнено (например путь проекта не найден
                /// внутри 'cmlEngine.__mDo') - сообщение об ошибке уже показано самим движком, повторно не дублируется
            }
            else
            {
                appUnitError vError = new appUnitError();
                vError.__mMessageBuild("Путь к проекту указан не верно");
                vError.__fErrorType_ = ERRORSTYPES.User;
                vError.__fProcedure_ = _fClassNameFull + "_cButtonRun_Click(object, EventArgs)";
                cmlApplication.__oErrorsHandler.__mShow(vError);
            }

            return;
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary> 
        /// Опреление цвета ключевых слов
        /// </summary>
        /// <param name="pKeyWord">Ключевое слово</param>
        /// <returns>Ключевое слово окруженное HTML тегами</returns>
        private string mKeyWord(string pKeyWord)
        {
            string vReturn = "";

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
        /// Форматирование строки в HTML формате
        /// </summary>
        /// <param name="pLine">Содержание строки</param>
        /// <remarks>Строка окруженная HTML тэгами</remarks>
        private string mLineColoring(string pLine)
        {
            string vReturn = ""; // Возвращаемое значение
            /// Перебор слов в строке и обработка их методом ' mKeyWord(string)'

            foreach (string vWord in appTypeString.__mWordsList(pLine.Trim(), ' '))
            {
                vReturn = vReturn + mKeyWord(vWord) + " ";
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

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Панель управления
        /// </summary>
        protected elmComponentToolbar _cToolbar = new elmComponentToolbar();
        /// <summary>
        /// Кнопка 'Выполнить'
        /// </summary>
        protected elmComponentToolbarButton _cButtonRun = new elmComponentToolbarButton();
        /// <summary>
        /// Кнопка 'Помощь'
        /// </summary>
        protected elmComponentToolbarButton _cButtonHelp = new elmComponentToolbarButton();
        /// <summary>
        /// Блок главного окна
        /// </summary>
        protected elmBlockFormMain _cBlockFormMain = new elmBlockFormMain();
        /// <summary>
        /// Панель для размещения компонентов
        /// </summary>
        protected elmBlockInputs _cBlockInput = new elmBlockInputs();
        /// <summary>
        /// Путь и имя папки для сканирования документов CS
        /// </summary>
        protected elmInputPath _cFolderScan = new elmInputPath();

        #endregion Компоненты

        #region - Служебные

        /// <summary>
        /// Полное имя класса
        /// </summary>
        protected string _fClassNameFull = "";

        #endregion Служебные

        #endregion ПОЛЯ
    }
}