using nlApplication;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MsExcel = Microsoft.Office.Interop.Excel;
using MsWord = Microsoft.Office.Interop.Word;

namespace nlElements
{
    /// <summary>
    /// Файл elmAreaReportPreview.cs
    /// </summary>
    /// <remarks>Класс области для предварительного просмотра отчетов</remarks>
    public class elmAreaReportPreview : elmArea
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {

            SuspendLayout();

            base._mObjectAssembly();

            #region Размещение компонентов

            Panel2.Controls.Add(_cBrowser);
            Panel2.Controls.SetChildIndex(_cBrowser, 0);
            _cToolBar.Items.Add(_cButtonSave);
            _cToolBar.Items.Add(_cButtonHelp);
            _cToolBar.Items.Add(_cButtonEdit);
            _cToolBar.Items.Add(_cButtonOperations);

            _cButtonEdit.DropDownItems.Add(_cButtonOpenInBrowser);
            _cButtonEdit.DropDownItems.Add(_cButtonOpenInWord);
            _cButtonEdit.DropDownItems.Add(_cButtonOpenInExcel);

            _cButtonOperations.DropDownItems.Add(_cButtonOperationsUserRights);

            #endregion Размещение компонентов

            #region Настройка компонентов


            // __cButtonSave
            {
                _cButtonSave.Click += cButtonSave_Click;
                _cButtonSave.Image = nlResourcesImages.Properties.Resources._Computer_Diskette_b32;
            }
            //// __cButtonHelp
            //{
            //    __cButtonHelp.Image = nlResourcesImages.Properties.Resources._EmoutionSorrow_y32C;
            //}
            // __cButtonEdit
            {
                _cButtonEdit.Alignment = ToolStripItemAlignment.Right;
                _cButtonEdit.Image = nlResourcesImages.Properties.Resources._Page_y32;
                {
                    _cButtonOpenInBrowser.Click += cButtonBrowser_Click;
                    _cButtonOpenInBrowser.Image = nlResourcesImages.Properties.Resources._Application_Google_m16;
                    _cButtonOpenInBrowser.__mCaptionBuilding("Открыть в Интернет браузере");
                    _cButtonOpenInExcel.Click += cButtonExcel_Click;
                    _cButtonOpenInExcel.Image = nlResourcesImages.Properties.Resources._Application_MSExcel_g16;
                    _cButtonOpenInExcel.__mCaptionBuilding("Открыть в MS Excel");
                    _cButtonOpenInWord.Image = nlResourcesImages.Properties.Resources._Application_MSWord_b16;
                    _cButtonOpenInWord.__mCaptionBuilding("Открыть в MS Word");
                    _cButtonOpenInWord.Click += cButtonWord_Click;
                }
            }
            // __cButtonOperations
            {
                _cButtonOperations.Alignment = ToolStripItemAlignment.Right;
                _cButtonOperations.Image = nlResourcesImages.Properties.Resources._PageGear_y32;
                {
                    _cButtonOperationsUserRights.Image = nlResourcesImages.Properties.Resources._Person_Police_k16;
                    _cButtonOperationsUserRights.__mCaptionBuilding("Права пользователей");
                }
            }
            // __cBrowser
            {
                _cBrowser.Dock = DockStyle.Fill;
            }

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        #region Кнопки управления

        private void cButtonSave_Click(object sender, EventArgs e)
        {
            appFileHtml vFileHtml = new appFileHtml(); // Объект для работы с Html файлами
            string vFilePath = vFileHtml.__mAddressToString(_cBrowser.__fUrl_);
            // ; /// Чтение файла
            string vTagValue = vFileHtml.__mTagFromFile(vFilePath, "TITLE"); /// Чтение тэга 'Title';
            string vFileReportPath = ""; // Путь и имя файла сохраняемого отчета
            if (vTagValue.Length > 0)
                vFileReportPath = Path.Combine(elmApplication.__oPathes.__fDirectoryReports_, vTagValue + "_" + appTypeDateTime.__mDateTimeToFileNameTillSecond(DateTime.Now) + ".htm");
            else
                vFileReportPath = Path.Combine(elmApplication.__oPathes.__fDirectoryReports_, Path.GetFileNameWithoutExtension(vFilePath) + "_" + appTypeDateTime.__mDateTimeToFileNameTillSecond(DateTime.Now) + Path.GetExtension(vFilePath));

            File.Copy(vFilePath, vFileReportPath, true);

            if (File.Exists(vFileReportPath) == true)
                elmApplication.__oMessages.__mShow(MESSAGESTYPES.Info, "Отчет сохранен", elmApplication.__oTunes.__mTranslate("Файл") + ":" + vFileReportPath, _fClassProcedure_);
        }
        private void cButtonBrowser_Click(object sender, EventArgs e)
        {
            string vReportFilePath = appFileHtml.__mUrlToFile(_cBrowser.__fUrl_); // Путь и название файла отчета
            if (vReportFilePath.Length == 0)
            {
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                _fError.__fFilePath_ = _fClassFilePath_;
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mMessageBuild("Имя файла отчета не указано");
                _fError.__fProcedure_ = _fClassProcedure_;
                elmApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return;
            }
            if (File.Exists(vReportFilePath) == false)
            {
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                _fError.__fFilePath_= _fClassFilePath_;
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mMessageBuild("Имя файла указано не верно");
                _fError.__fProcedure_ = _fClassProcedure_;
                elmApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return;
            }
            System.Diagnostics.Process.Start(vReportFilePath);
        }
        /// <summary>Выполняется при выборе кнопки 'Открыть в MS Word'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cButtonWord_Click(object sender, EventArgs e)
        {
            object vReportFilePath = _cBrowser.__fUrl_; // Путь и название файла отчета

            if (appFileHtml.__mUrlToFile(vReportFilePath.ToString()).Length == 0)
            {
                _fError.__fFilePath_ = _fClassFilePath_;
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                _fError.__mMessageBuild("Имя файла отчета не указано");
                _fError.__fProcedure_ = _fClassProcedure_;
                elmApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return;
            }
            if (File.Exists(appFileHtml.__mUrlToFile(vReportFilePath.ToString())) == false)
            {
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                _fError.__fFilePath_ = _fClassFilePath_;
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                _fError.__mMessageBuild("Имя файла указано не верно");
                _fError.__fProcedure_ = _fClassProcedure_;
                elmApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return;
            }

            object vReadOnly = false;
            object vIsVisible = true;
            object vFormat = MsWord.WdOpenFormat.wdOpenFormatWebPages; // https://docs.microsoft.com/en-us/dotnet/api/microsoft.office.interop.word.wdopenformat?view=word-pia
            MsWord.Application wordObject = new MsWord.Application();
            object vNull = System.Reflection.Missing.Value;

            MsWord._Document docs = wordObject.Documents.Open(ref vReportFilePath, ref vIsVisible, ref vReadOnly, ref vNull, ref vNull, ref vNull, ref vNull, ref vNull, ref vNull, ref vFormat, ref vNull, ref vIsVisible, ref vNull, ref vNull, ref vNull, ref vNull);

            wordObject.Visible = true;
        }
        /// <summary>Выполняется при выборе кнопки 'Открыть в MS Excel'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cButtonExcel_Click(object sender, EventArgs e)
        {
            //ntwTypeString vTypeString = new ntwTypeString();
            string vReportFilePath = appFileHtml.__mUrlToFile(_cBrowser.__fUrl_); // Путь и название файла отчета
            if (vReportFilePath.Length == 0)
            {
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                _fError.__fFilePath_ = _fClassFilePath_;
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                _fError.__mMessageBuild("Имя файла отчета не указано");
                _fError.__fProcedure_ = _fClassProcedure_;
                elmApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return;
            }
            if (File.Exists(vReportFilePath) == false)
            {
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                _fError.__fFilePath_ = _fClassFilePath_;
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                _fError.__mMessageBuild("Имя файла указано не верно");
                _fError.__fProcedure_ = _fClassProcedure_;
                elmApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return;
            }
            MsExcel.Application vExcel;
            vExcel = new MsExcel.Application();
            MsExcel.Workbook voWorkBook = vExcel.Workbooks.Open(vReportFilePath);
            vExcel.Visible = true;
        }

        public override bool Equals(object obj)
        {
            return obj is elmAreaReportPreview preview &&
                   __fUrl_ == preview.__fUrl_;
        }

        #endregion Кнопки управления

        #endregion Поведение

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Кнопка 'Применить'
        /// </summary>
        protected elmComponentToolbarButton _cButtonSave = new elmComponentToolbarButton();
        /// <summary>
        /// Кнопка 'Правка'
        /// </summary>
        protected elmComponentToolbarButtonMenu _cButtonEdit = new elmComponentToolbarButtonMenu();
        /// <summary>
        /// Кнопка 'Операции'
        /// </summary>
        protected elmComponentToolbarButtonMenu _cButtonOperations = new elmComponentToolbarButtonMenu();

        /// <summary>
        /// Пункт меню 'Правка/Открыть в Браузере'
        /// </summary>
        protected elmComponentMenuItem _cButtonOpenInBrowser = new elmComponentMenuItem();

        /// <summary>
        /// Пункт меню 'Правка/Открыть в MS Word'
        /// </summary>
        protected elmComponentMenuItem _cButtonOpenInWord = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'Правка/Открыть в MS Excel'
        /// </summary>
        protected elmComponentMenuItem _cButtonOpenInExcel = new elmComponentMenuItem();

        /// <summary>
        /// Пункт меню 'Операции/Определение прав пользователей'
        /// </summary>
        protected elmComponentMenuItem _cButtonOperationsUserRights = new elmComponentMenuItem();

        /// <summary>
        /// WebBrowser
        /// </summary>
        protected elmComponentWeb _cBrowser = new elmComponentWeb();

        #endregion - Компоненты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Загружаемый адрес
        /// </summary>
        public string __fUrl_
        {
            get { return _cBrowser.__fUrl_; }
            set { _cBrowser.__fUrl_ = value; }
        }

        /// <summary>
        /// Видимость кнопки 'Сохранить'
        /// </summary>
        public bool __fButtonSaveVisible_
        {
            get { return _cButtonSave.Visible; }
            set
            {
                _cButtonSave.Visible = value;
            }
        }

        /// <summary>
        /// Доступность кнопки 'Операции'
        /// </summary>
        public bool __fButtonOperationsEnabled_
        {
            get { return _cButtonOperations.Enabled; }
            set { _cButtonOperations.Enabled = value; }
        }
        /// <summary>
        /// Видимость кнопки 'Операции'
        /// </summary>
        public bool __fButtonOperationsVisible_
        {
            get { return _cButtonOperations.Visible; }
            set { _cButtonOperations.Visible = value; }
        }
        /// <summary>
        /// Подсказка к кнопке 'Операции' переведенная на язык пользователя
        /// </summary>
        public string __fButtonOperationsToolTipText
        {
            set { _cButtonOperations.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Изображение на кнопке 'Операции'
        /// </summary>
        public Image __fButtonOperationsImage
        {
            set { _cButtonOperations.Image = value; }
        }


        /// <summary>
        /// Доступность кнопки 'Операции'
        /// </summary>
        public bool __fButtonOpenInExcelEnabled_
        {
            get { return _cButtonOpenInExcel.Enabled; }
            set { _cButtonOpenInExcel.Enabled = value; }
        }
        /// <summary>
        /// Видимость кнопки 'Операции'
        /// </summary>
        public bool __fButtonOpenInExcelVisible_
        {
            get { return _cButtonOpenInExcel.Visible; }
            set { _cButtonOpenInExcel.Visible = value; }
        }
        /// <summary>
        /// Подсказка к кнопке 'Операции' переведенная на язык пользователя
        /// </summary>
        public string __fButtonOpenInExcelToolTipText
        {
            set { _cButtonOpenInExcel.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Изображение на кнопке 'Операции'
        /// </summary>
        public Image __fButtonOpenInExcelImage
        {
            set { _cButtonOpenInExcel.Image = value; }
        }

        /// <summary>
        /// Доступность кнопки 'Операции'
        /// </summary>
        public bool __fButtonOpenInWordEnabled_
        {
            get { return _cButtonOpenInWord.Enabled; }
            set { _cButtonOpenInWord.Enabled = value; }
        }
        /// <summary>
        /// Видимость кнопки 'Операции'
        /// </summary>
        public bool __fButtonOpenInWordVisible_
        {
            get { return _cButtonOpenInWord.Visible; }
            set { _cButtonOpenInWord.Visible = value; }
        }
        /// <summary>
        /// Подсказка к кнопке 'Операции' переведенная на язык пользователя
        /// </summary>
        public string __fButtonOpenInWordToolTipText
        {
            set { _cButtonOpenInWord.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Изображение на кнопке 'Операции'
        /// </summary>
        public Image __fButtonOpenInWordImage
        {
            set { _cButtonOpenInWord.Image = value; }
        }

        #endregion = СВОЙСТВА
    }
}
