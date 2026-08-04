using nlApplication;
using nlData;
using nlReportHtml;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmAreaGrid.cs
    /// </summary>
    /// <remarks>Класс-область для правки табличных данных</remarks>
    public class elmAreaGrid : elmArea
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Размещение компонентов

            _cToolBar.Items.Insert(0, _cButtonSelect);
            _cToolBar.Items.Insert(1, _cButtonRefresh);
            _cToolBar.Items.Add(_cButtonColumns);
            _cToolBar.Items.Add(__cButtonReports);
            _cToolBar.Items.Add(__cButtonOperations);
            _cToolBar.Items.Add(_cButtonEdit);

            _cButtonEdit.DropDownItems.Add(_cButtonEditCreate);
            _cButtonEdit.DropDownItems.Add(_cButtonEditCopy);
            _cButtonEdit.DropDownItems.Add(_cButtonEditEdit);
            _cButtonEdit.DropDownItems.Add(_cButtonEditRemove);
            _cButtonEdit.DropDownItems.Add(_cButtonEditRestore);

            __cButtonReports.DropDownItems.Add(_cButtonReportsCurrentList);
            __cButtonReports.DropDownItems.Add(_cButtonReportsHistory);

            Panel2.Controls.Add(_cSplitterFilterGrid);
            Panel2.Controls.SetChildIndex(_cSplitterFilterGrid, 0);

            _cSplitterFilterGrid.Panel1.Controls.Add(_cLabelFilterCaption);
            _cSplitterFilterGrid.Panel1.Controls.Add(_cLabelFilterExpression);

            _cSplitterFilterGrid.Panel2.Controls.Add(_cGrid);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            // _cButtonColumns
            {
                _cButtonColumns.Alignment = ToolStripItemAlignment.Right;
                _cButtonColumns.DropDownOpened += mButtonDropDownOpened;
                _cButtonColumns.Image = global::nlResourcesImages.Properties.Resources._Grid_Fields_b32;
                _cButtonColumns.ToolTipText = "[ F12 ] " + elmApplication.__oTunes.__mTranslate("Видимость колонок");
                _cButtonColumns.__eClickRight += mButtonColumns_eMouseClickRight;
            }
            // _cButtonEdit
            {
                _cButtonEdit.Alignment = ToolStripItemAlignment.Right;
                _cButtonEdit.DropDownOpened += mButtonDropDownOpened;
                _cButtonEdit.Image = global::nlResourcesImages.Properties.Resources._Page_b32;
                _cButtonEdit.ToolTipText = "[ Ctrl + E ] " + elmApplication.__oTunes.__mTranslate("Правка записи");
                {
                    _cButtonEditCreate.Click += mButtonEditCreateClick;
                    _cButtonEditCreate.Image = global::nlResourcesImages.Properties.Resources._Page_b16;
                    _cButtonEditCreate.__fCaption_ = "Создать";

                    _cButtonEditCopy.Click += mButtonEditCopyClick;
                    _cButtonEditCopy.Image = global::nlResourcesImages.Properties.Resources._PageCopy_b16;
                    _cButtonEditCopy.__fCaption_ = "Копировать";

                    _cButtonEditEdit.Click += mButtonEditEdit_Click;
                    _cButtonEditEdit.Image = global::nlResourcesImages.Properties.Resources._PageEdit_b16;
                    _cButtonEditEdit.__fCaption_ = "Изменить";

                    _cButtonEditRemove.Click += mButtonEditRemove_Click;
                    _cButtonEditRemove.Image = global::nlResourcesImages.Properties.Resources._PageDelete_b16;
                    _cButtonEditRemove.__fCaption_ = "Исключить";

                    _cButtonEditRestore.Click += mButtonEditRestore_Click;
                    _cButtonEditRestore.Image = global::nlResourcesImages.Properties.Resources._PageAdd_b16;
                    _cButtonEditRestore.__fCaption_ = "Восстановить";
                }
            }
            // _cButtonOperations
            {
                __cButtonOperations.Alignment = ToolStripItemAlignment.Right;
                __cButtonOperations.DropDownOpened += mButtonDropDownOpened;
                __cButtonOperations.Image = global::nlResourcesImages.Properties.Resources._PageGear_y32;
                __cButtonOperations.ToolTipText = "[ Ctrl + O ] " + elmApplication.__oTunes.__mTranslate("Операции");
            }
            // _cButtonRefresh
            {
                _cButtonRefresh.__eClickRight += mButtonRefresh_eMouseClickRight;
                _cButtonRefresh.Click += mButtonRefresh_Click;
                _cButtonRefresh.Image = global::nlResourcesImages.Properties.Resources._Arrow_Refresh_g32;
                _cButtonRefresh.ToolTipText = "[ F5 ] " + elmApplication.__oTunes.__mTranslate("Обновить");
            }
            // __cButtonReports
            {
                __cButtonReports.Alignment = ToolStripItemAlignment.Right;
                __cButtonReports.DropDownOpened += mButtonDropDownOpened;
                __cButtonReports.Image = global::nlResourcesImages.Properties.Resources._PagePrinter_y32;
                __cButtonReports.ToolTipText = "[ Ctrl + R ] " + elmApplication.__oTunes.__mTranslate("Отчеты");
                {
                    // _cButtonReportsCurrentList
                    {
                        _cButtonReportsCurrentList.Click += mButtonReportsCurrentList_Click;
                        _cButtonReportsCurrentList.Image = global::nlResourcesImages.Properties.Resources._Folder_Tree_a16;
                        _cButtonReportsCurrentList.__fCaption_ = "Текущий список";
                    }
                    // _cButtonReportsHistory
                    {
                        _cButtonReportsHistory.Click += mButtonReportsHistory_Click;
                        _cButtonReportsHistory.Image = global::nlResourcesImages.Properties.Resources._Folder_Tree_a16;
                        _cButtonReportsHistory.__fCaption_ = "История корректировок";
                    }
                }
            }
            // __cButtonSelect
            {
                _cButtonSelect.Click += mButtonSelect_Click;
                _cButtonSelect.Image = global::nlResourcesImages.Properties.Resources._Sign_Tick_g32;
                _cButtonSelect.ToolTipText = "[ Ctrl + S ] " + elmApplication.__oTunes.__mTranslate("Выбрать");
            }
            // _cSplitterFilterGrid
            {
                _cSplitterFilterGrid.SplitterDistance = 20;
                _cSplitterFilterGrid.Dock = DockStyle.Fill;
                _cSplitterFilterGrid.Orientation = Orientation.Horizontal;
                _cSplitterFilterGrid.IsSplitterFixed = true;
                _cSplitterFilterGrid.FixedPanel = FixedPanel.Panel1;
            }
            // __cLabelFilterCaption
            {
                _cLabelFilterCaption.Location = new Point(elmInterface.__fIntervalHorizontal, elmInterface.__fIntervalVertical);
                _cLabelFilterCaption.__fCaption_ = "Показаны данные для:";
            }
            // __cLabelFilterExpression
            {
                _cLabelFilterExpression.Location = new Point(elmInterface.__fIntervalHorizontal * 2
                    , _cLabelFilterCaption.Top
                    + _cLabelFilterCaption.Height
                    + elmInterface.__fIntervalVertical);
            }
            // __cGrid
            {
                _cGrid.Dock = DockStyle.Fill;
                _cGrid.CellDoubleClick += mGrid_CellDoubleClick;
                _cGrid.KeyDown += mGrid_KeyDown;
                _cGrid.__fRowChanged += mGrid_RowEnter;
            }

            #endregion Настройка компонентов

            ResumeLayout();

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();
            __mColumnsVisibilityLoad();
            __mSortingLoad();
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        #region Кнопки управления

        #region ! Внешние нажатия на кнопки управления

        /// <summary>
        /// Выполняется при выборе кнопки 'Выбрать'
        /// </summary>
        public void __mPressButtonSelect()
        {
            _cButtonSelect.PerformClick();

            return;
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Обновить'
        /// </summary>
        public void __mPressButtonRefresh()
        {
            _cButtonRefresh.PerformClick();

            return;
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Правка'
        /// </summary>
        public void __mPressButtonEdit()
        {
            _cButtonEdit.ShowDropDown();

            return;
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Операции'
        /// </summary>
        public void __mPressButtonOperations()
        {
            __cButtonOperations.ShowDropDown();

            return;
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Отчеты'
        /// </summary>
        public void __mPressButtonReports()
        {
            __cButtonReports.ShowDropDown();

            return;
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Колонки'
        /// </summary>
        public void __mPressButtonColumns()
        {
            _cButtonColumns.PerformClick();

            return;
        }

        #endregion Внешние нажатия на кнопки управления

        /// <summary>
        /// Выполняется при открытии меню кнопки 'Правка'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonDropDownOpened(object sender, EventArgs e)
        {
            _fDropDownOpened = true;
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Правка / Создать' левой кнопкой мыши 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonEditCreateClick(object sender, EventArgs e)
        {
            if (__eButtonEditCreateClickBefore != null)
                __eButtonEditCreateClickBefore(sender, e);

            elmForm vForm = FindForm() as elmForm;
            if (vForm != null & __oFormOpened != null)
            {
                /// Вызов формы редактирования документа
                if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormDocument)
                {
                    elmFormDocument vFormDocument = (elmFormDocument)Activator.CreateInstance(__oFormOpened);
                    vFormDocument.__cAreaDocument.__fFormMode = FORMMODE.ForCreate;
                    vFormDocument.__cAreaDocument.__oEssenceCaption = __oEssence_;
                    vFormDocument.__cAreaDocument.__oEssenceContent_ = __oEssenceContent;
                    if (__eButtonEditCreateClickBeforeShowForm != null)
                        __eButtonEditCreateClickBeforeShowForm(sender, e);
                    (vFormDocument as elmFormDocument).ShowDialog();
                }
                /// Вызов формы редактирования документа
                if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormPages)
                {
                    elmFormPages vFormPages = (elmFormPages)Activator.CreateInstance(__oFormOpened);
                    if (__eButtonEditCreateClickBeforeShowForm != null)
                        __eButtonEditCreateClickBeforeShowForm(sender, e);
                    (vFormPages as elmFormPages).ShowDialog();
                }
                /// Вызов формы редактирования записи
                if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormRecord)
                {
                    elmFormRecord vFormRecord = (elmFormRecord)Activator.CreateInstance(__oFormOpened);
                    __oFormRecord = vFormRecord;
                    if (__eButtonEditCreateClickBeforeShowForm != null)
                        __eButtonEditCreateClickBeforeShowForm(sender, e);
                    (vFormRecord as elmFormRecord).ShowDialog();
                }

                /// Перегрузка данных
                __mDataLoad();
            }
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Правка / Создать' левой кнопкой мыши 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonEditCopyClick(object sender, EventArgs e)
        {
            _fDropDownOpened = true;
            elmForm vForm = FindForm() as elmForm;
            if (vForm != null & __oFormOpened != null)
            {
                /// Вызов формы редактирования документа
                if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormPages)
                {
                    elmFormPages vFormDocument = (elmFormPages)Activator.CreateInstance(__oFormOpened);
                    (vFormDocument as elmFormPages).__cAreaPages.__fRecordClueForCopy = _cGrid.__fRecordClue_;
                    (vFormDocument as elmFormPages).ShowDialog();
                }
                /// Вызов формы редактирования записи
                if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormRecord)
                {
                    elmFormRecord vFormRecord = (elmFormRecord)Activator.CreateInstance(__oFormOpened);
                    (vFormRecord as elmFormRecord).__cAreaRecord.__fRecordClueForCopy = _cGrid.__fRecordClue_;
                    (vFormRecord as elmFormRecord).ShowDialog();
                }
                /// Перегрузка данных
                __mDataLoad();
            }
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Правка / Изменить' левой кнопкой мыши 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonEditEdit_Click(object sender, EventArgs e)
        {
            if (_cGrid.__fRecordClue_ == 0)
            {
                elmApplication.__oMessages.__mShow(MESSAGESTYPES.Warning, "Запись с нулевым кодом не редактируется!");
                goto Exit;
            }
            if (__eButtonEditEditClick != null)
                __eButtonEditEditClick(sender, e);

            _fDropDownOpened = true;

            if (__fEditLock == false)
            {
                elmForm vForm = FindForm() as elmForm;

                if (vForm != null & __oFormOpened != null)
                {
                    /// Вызов формы редактирования документа
                    if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormDocument)
                    {
                        elmFormDocument vFormDocument = (elmFormDocument)Activator.CreateInstance(__oFormOpened);
                        vFormDocument.__cAreaDocument.__fFormMode = FORMMODE.ForEdit;
                        vFormDocument.__cAreaDocument.__fDocumentClue = __fRecordClue_; // Указание идентификатора документа
                        if (__eButtonEditCreateClickBeforeShowForm != null)
                            __eButtonEditCreateClickBeforeShowForm(sender, e);
                        (vFormDocument as elmFormDocument).ShowDialog();
                    }
                    /// Вызов формы редактирования документа
                    if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormPages)
                    {
                        elmFormPages vFormDocument = (elmFormPages)Activator.CreateInstance(__oFormOpened);
                        vFormDocument.__cAreaPages.__fRecordClue = _cGrid.__fRecordClue_;
                        (vFormDocument as elmFormPages).ShowDialog();
                    }
                    ///// Вызов формы для подписи документа
                    //if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormDocumentSignature)
                    //{
                    //    elmFormDocumentSignature vFormRecord = (elmFormDocumentSignature)Activator.CreateInstance(__oFormOpened);
                    //    //vFormRecord.__cAreaDocumentSignature.__fRecordClue = _cGrid.__fRecordClue_;
                    //    (vFormRecord as elmFormDocumentSignature).ShowDialog();
                    //}
                    /// Вызов формы редактирования записи
                    if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormRecord)
                    {
                        elmFormRecord vFormRecord = (elmFormRecord)Activator.CreateInstance(__oFormOpened);
                        vFormRecord.__cAreaRecord.__fRecordClue = _cGrid.__fRecordClue_;
                        (vFormRecord as elmFormRecord).ShowDialog();
                    }
                    /// Перегрузка данных
                    __mDataLoad();
                }
            }
            else
                __fEditLock = false;

            Exit:
            return;
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Правка / Удалить' левой кнопкой мыши 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonEditRemove_Click(object sender, EventArgs e)
        {
            if (_cGrid.__fRecordClue_ == 0)
            {
                elmApplication.__oMessages.__mShow(MESSAGESTYPES.Warning, "Запись с нулевым кодом не исключается!");
                goto Exit;
            }
            if (elmApplication.__oMessages.__mShow(MESSAGESTYPES.Question, "Исключить запись с кодом '" + _cGrid.__mCurrentRowFieldValue("cod" + _cGrid.__oEssence.__fTableName.Trim()) + "'") == DialogResult.Yes)
            {
                __mSortingSave();
                _cGrid.__mRecordDelete();
                /// Перегрузка данных
                __mDataLoad();
            }
            
        Exit:
            return;
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Правка / Восстановить' левой кнопкой мыши 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonEditRestore_Click(object sender, EventArgs e)
        {
            __mSortingSave();
            _cGrid.__mRecordRestore();
            elmApplication.__oMessages.__mShow(MESSAGESTYPES.Warning, String.Format("Запись с кодом {0} восстановлена", _cGrid.__mCurrentRowFieldValue("cod" + _cGrid.__oEssence.__fTableName.Trim())));
            /// Перегрузка данных
            __mDataLoad();
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Операции / Определение прав пользователей'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonOperationsAccess_Click(object sender, EventArgs e)
        {
            if (__eButtonUsersAccessClick != null)
                __eButtonUsersAccessClick(this, new EventArgs());
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Обновить' левой кнопкой мыши 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonRefresh_Click(object sender, EventArgs e)
        {
            if(__eButtonRefreshClickBefore != null)
                __eButtonRefreshClickBefore(this, new EventArgs());   

            elmForm vForm = FindForm() as elmForm;
            if (vForm != null & __oFormFilter != null)
            {
                __mSortingSave();
                elmFormFilter vFormFilter = (elmFormFilter)Activator.CreateInstance(__oFormFilter);
                vFormFilter.__cAreaFilter.__fAreaId = __fAreaId;
                vFormFilter.__cAreaFilter.__fFormNameParent = (FindForm() as elmForm).__fClassName_;

                (vFormFilter as elmFormFilter).ShowDialog();
                /// Перегрузка данных
                __mDataLoad(); // Перегрузка данных
            }
            else
            {
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                _fError.__fFilePath_ = _fClassFilePath_;
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mMessageBuild("Форма для построения фильтра не определена");
                _fError.__fProcedure_ = _fClassProcedure_;
                elmApplication.__oErrorsHandler.__mShow(_fError);
            }

            if (__eButtonRefreshClickAfter != null)
                __eButtonRefreshClickAfter(this, new EventArgs());

        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Обновить' правой кнопкой мыши 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonRefresh_eMouseClickRight(object sender, EventArgs e)
        {
            if (__eButtonRefresh_ClickRight != null)
                __eButtonRefresh_ClickRight(this, new EventArgs());
            else
            {
                string vFormParentName = (FindForm() as elmForm).__fClassName_; // Название формы на которой расположен компонент
                appFileIni vFileIni = (FindForm() as elmForm).__oFileIni; // Объект для работы с инициализационными файлами
                vFileIni.__fFilePath = elmApplication.__oPathes.__mFileFormTunes(); // Указание настроечного файла
                ArrayList vParametersList = new ArrayList(); // Список параметров в конфигурационном файле для текущей формы
                vParametersList = vFileIni.__mParametersList(vFormParentName);

                /// Перебор параметров в секции формы
                foreach (string vParameter in vParametersList)
                {
                    /// Чтение статуса условия фильтра
                    if (vParameter.StartsWith("FilterStatus") == true)
                    {
                        vFileIni.__mValueWrite("False", vFormParentName, vParameter); /// Сброс статуса использования 
                    }
                }
                /// Перегрузка данных
                __mDataLoad();
                (FindForm() as elmForm).__cPanelStatus.__fCaption_ = "Фильтр сброшен";
            }
        }
        /// <summary>
        /// Выполняется при выборе меню "Отчеты/Текущий список"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonReportsCurrentList_Click(object sender, EventArgs e)
        {
            _fDropDownOpened = true;
            if (_fButtonReportsListDefaultCode == true)
            {
                rhtReport vReport = new rhtReport();
                vReport.__mCreate();
                if(FindForm().GetType() == typeof(elmFormGrid))
                    vReport.__fTitle = elmApplication.__oTunes.__mTranslate("Список '{0}'", (FindForm() as elmFormGrid).Text);
                else
                    vReport.__fTitle = elmApplication.__oTunes.__mTranslate("Список '{0}'", (FindForm() as elmForm).Text);
                vReport.__fColumnsCountInReport = 0;

                /// Подсчет отображаемых колонок
                for (int vColumnNumber = 0; vColumnNumber < _cGrid.Columns.Count; vColumnNumber++)
                {
                    if (vColumnNumber != _cGrid.Columns.Count)
                        if (_cGrid.Columns[vColumnNumber].Visible == true)
                            vReport.__fColumnsCountInReport++;
                }
                /// Отображение заголовка
                vReport.__mRow();
                vReport.__mCell(vReport.__fTitle, "CL=Caption", "SC=" + vReport.__fColumnsCountInReport.ToString());
                vReport.__mRow();
                vReport.__mCell(_cLabelFilterCaption.Text, "SC=" + vReport.__fColumnsCountInReport.ToString(), "CL=TimeUser");
                vReport.__mRow();
                vReport.__mCell(_cLabelFilterExpression.Text.Replace("\n", "<BR>"), "SC=" + vReport.__fColumnsCountInReport.ToString(), "CL=TimeUser");
                vReport.__mRowEmpty();
                vReport.__mTime("CL=TimeUser");
                vReport.__mUser(elmApplication.__oData.__mUserAlias(), "CL=TimeUser");
                vReport.__mRowEmpty();

                /// Построение заголовка таблицы
                vReport.__mRow();
                for (int vColumnNumber = 0; vColumnNumber < _cGrid.Columns.Count; vColumnNumber++)
                {
                    if (vColumnNumber != _cGrid.Columns.Count)
                        if (_cGrid.Columns[vColumnNumber].Visible == true)
                            vReport.__mCell(_cGrid.Columns[vColumnNumber].HeaderCell.Value, "CL=HeaderCell");
                        else
                            if (_cGrid.Columns[vColumnNumber].Visible == true)
                            vReport.__mCell(_cGrid.Columns[vColumnNumber].HeaderCell.Value, "CL=HeaderCell-Last");
                }
                /// Отображение данных
                /// Перебор строк в курсоре
                foreach (DataGridViewRow vViewRow in _cGrid.Rows)
                {
                    vReport.__mRow();
                    /// Перебор полей
                    for (int vColumnNumber = 0; vColumnNumber < _cGrid.Columns.Count; vColumnNumber++)
                    {
                        if (vColumnNumber != _cGrid.Columns.Count)
                            if (_cGrid.Columns[vColumnNumber].Visible == true)
                                if (vViewRow.Cells[vColumnNumber].Value != null)
                                {
                                    if (vViewRow.Cells[vColumnNumber].Value.GetType() == typeof(bool))
                                        vReport.__mCell(Convert.ToBoolean(vViewRow.Cells[vColumnNumber].Value) == true ? elmApplication.__oTunes.__mTranslate("Да") : elmApplication.__oTunes.__mTranslate("Нет"), "CL=DataCell");
                                    else
                                        vReport.__mCell(vViewRow.Cells[vColumnNumber].Value.ToString(), "CL=DataCell");
                                }
                                else
                                    if (_cGrid.Columns[vColumnNumber].Visible == true)
                                    if (vViewRow.Cells[vColumnNumber].Value != null)
                                    {
                                        if (vViewRow.Cells[vColumnNumber].Value.GetType() == typeof(bool))
                                            vReport.__mCell(Convert.ToBoolean(vViewRow.Cells[vColumnNumber].Value) == true ? elmApplication.__oTunes.__mTranslate("Да") : elmApplication.__oTunes.__mTranslate("Нет"), "CL=DataCell-Last");
                                        else
                                            vReport.__mCell(vViewRow.Cells[vColumnNumber].Value.ToString(), "CL=DataCell-Last");
                                    }
                    }
                }

                vReport.__mFile();
                elmFormReportPreview vFormReportPreview = new elmFormReportPreview();
                vFormReportPreview.__cAreaReportPreview.__fUrl_ = vReport.__fFilePath;
                vFormReportPreview.ShowDialog();
            }
            else
            {
                if (__eButtonReportsListClick != null)
                    __eButtonReportsListClick(_cGrid, new EventArgs());
            }
        }
        /// <summary>
        /// Выполняется при выборе меню "Отчеты/История"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonReportsHistory_Click(object sender, EventArgs e)
        {
            _fDropDownOpened = true;
            DataTable vDataTable = _cGrid.__oEssence.__mRecordChanges(_cGrid.__fRecordClue_);
            /// Выполнение обратной сортировки истории
            DataView vDataView = vDataTable.DefaultView;
            vDataView.Sort = "dtmRrdLck desc"; // Обратная сортировка по времени корректировки
            vDataTable = vDataView.ToTable();
            string vTableName = _cGrid.__oEssence.__fTableName; // Название текущей таблицы
            rhtReport vReport = new rhtReport();
            vReport.__mCreate();
            vReport.__fTitle = elmApplication.__oTunes.__mTranslate("Таблица '{0}'", __oEssence_.__fTableName);

            /// Отображение заголовка
            vReport.__mRow();
            vReport.__mCell(vReport.__fTitle, "CL=Caption", "SC=" + vReport.__fColumnsCountInReport.ToString(), "A=center", "SC=Max");
            vReport.__mRow();
            if (FindForm().GetType() == typeof(elmFormGrid))
                vReport.__mCell(elmApplication.__oTunes.__mTranslate("История записи '{0}' id={1}", (FindForm() as elmFormGrid).Text, _cGrid.__fRecordClue_), "SC=Max");
            else
                vReport.__mCell(elmApplication.__oTunes.__mTranslate("История записи '{0}' id={1}", (FindForm() as elmForm).Text, _cGrid.__fRecordClue_), "SC=Max");

            vReport.__fColumnsCountInReport = vDataTable.Columns.Count;
            vReport.__mRowEmpty();

            /// Построение заголовка таблицы
            vReport.__mRow();
            for (int vColumnNumber = 0; vColumnNumber < vDataTable.Columns.Count; vColumnNumber++)
            {
                if (vColumnNumber != vDataTable.Columns.Count)
                {
                    switch (vDataTable.Columns[vColumnNumber].ColumnName)
                    {
                        case "CHG":
                            //vReport.__mCell(elmApplication.__oTunes.__mTranslate("Правка"), "CL=HeaderCell");
                            break;
                        case "CLU":
                            vReport.__mCell(elmApplication.__oTunes.__mTranslate("Ключ"), "CL=HeaderCell");
                            break;
                        case "ELD":
                            vReport.__mCell(elmApplication.__oTunes.__mTranslate("Исключена"), "CL=HeaderCell");
                            break;
                        case "GID":
                            vReport.__mCell(elmApplication.__oTunes.__mTranslate("Идентификатор"), "CL=HeaderCell");
                            break;
                        case "dsiUsrChg":
                            vReport.__mCell(elmApplication.__oTunes.__mTranslate("Пользователь"), "CL=HeaderCell");
                            break;
                        case "dtmRrdLck":
                            vReport.__mCell(elmApplication.__oTunes.__mTranslate("Время"), "CL=HeaderCell");
                            break;
                        case "lnkRrdLck":
                            vReport.__mCell(elmApplication.__oTunes.__mTranslate("Блокировка"), "CL=HeaderCell");
                            break;
                        default:
                            string vCaption = elmApplication.__oData.__mModelFieldCaption(__oEssence_.__fTableName, vDataTable.Columns[vColumnNumber].ColumnName);
                            if (vCaption.Trim().Length > 0)
                                vReport.__mCell(vCaption, "CL=HeaderCell");
                            else
                                vReport.__mCell(vDataTable.Columns[vColumnNumber].ColumnName, "CL=HeaderCell");
                            break;
                    }
                }
            }

            /// Отображение данных
            /// Перебор строк в курсоре
            foreach (DataRow vDataRow in vDataTable.Rows)
            {
                vReport.__mRow();
                foreach (DataColumn vDataColumn in vDataTable.Columns)
                {
                    if (vDataColumn.ColumnName == "CHG")
                        continue;
                    if (Convert.ToString(vDataRow["dtmRrdLck"]).Length > 0)
                    {
                        if (vDataRow[vDataColumn.ColumnName].GetType() == typeof(bool))
                            vReport.__mCell(Convert.ToBoolean(vDataRow[vDataColumn.ColumnName]) == true ? elmApplication.__oTunes.__mTranslate("Да") : elmApplication.__oTunes.__mTranslate("Нет"), "CL=DataCell");
                        else
                            vReport.__mCell(vDataRow[vDataColumn.ColumnName], "CL=DataCell");
                    }
                }
            }

            vReport.__mFile();
            elmFormReportPreview vFormReportPreview = new elmFormReportPreview();
            vFormReportPreview.__cAreaReportPreview.__fUrl_ = vReport.__fFilePath;
            vFormReportPreview.ShowDialog();
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Выбрать' левой кнопкой мыши 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonSelect_Click(object sender, EventArgs e)
        {
            __fIsSelected = true;
            if (__eButtonSelectClick != null)
                __eButtonSelectClick(this, new EventArgs());
           
        }
        private void mButtonColumns_eMouseClickRight(object sender, EventArgs e)
        {
            #region Отображение всех колонок

            foreach (elmComponentMenuItem vMenu in _cButtonColumns.DropDownItems)
            {
                vMenu.Checked = true;
            }

            #endregion Отображение всех колонок

            #region Восстановление порядка колонок

            ArrayList vColumnsIndexes = (FindForm() as elmForm).__oFileIni.__mParametersListByMaskInput(Name, "Column_");
            int vColumnIndexDefault = 0;
            foreach (string vColumn in vColumnsIndexes)
            {
                (FindForm() as elmForm).__oFileIni.__mParameterClear(Name, vColumn);
                _cGrid.Columns[vColumn.Substring(7)].DisplayIndex = vColumnIndexDefault;
                vColumnIndexDefault++;
            }

            #endregion Восстановление порядка колонок

            /// Перегрузка данных
            __mDataLoad();
        }
        /// <summary>
        /// Изменение статуса видимости любой колонки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mFieldsVisibleCheckedChanged(object sender, System.EventArgs e)
        {
            if (_cGrid.Columns[(sender as ToolStripMenuItem).Name] != null)
            {
                _cGrid.Columns[(sender as ToolStripMenuItem).Name].Visible = (sender as ToolStripMenuItem).Checked; /// Исправление видимости колонки в сетке
                _cGrid.__mColumnChangeVisible((sender as ToolStripMenuItem).Name, (sender as ToolStripMenuItem).Checked); /// Исправление видимости колонки в настройках сетки
            }
        }

        #endregion Кнопки управления

        /// <summary>
        /// Выполняется при выборе записи сетки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mGrid_RowEnter(object sender, EventArgs e)
        {
            /// Формируется событие завершения перехода на новую запись сетки
            if (__eRowChanged != null)
                __eRowChanged(sender, e);
            /// Нажатие на кнопку '_cButtonSelect'
            if (__fOnRowEnterClickSelect == true)
                __mPressButtonSelect();
        }
        /// <summary>
        /// Выполняется при нажатии клавиши при фокусе находящемся в _cGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mGrid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Enter:
                    if (__fButtonSelectVisible_ == true)
                        mGrid_CellDoubleClick(sender, null);
                    break;
            }

            return;
        }
        /// <summary>
        /// Выполняется при двойном клике по ячейке сетки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            /// Поле __fIsSelected устанавдивается в 'true'
            __fIsSelected = true;

            if(__eRowDoubleClick != null)
                __eRowDoubleClick(sender, e);

            return;
        }
        /// <summary>
        /// Выполняется при разрушении объекта
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            __mSortingSave();
            __mColumnsVisibilitySave();
            base.OnHandleDestroyed(e);
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Добавление колонки
        /// </summary>
        /// <param name="pCaption">Заголовок колонки</param>
        /// <param name="pFieldName">Название поля</param>
        /// <param name="pReadOnly">Атрибут "Только чтение"</param>
        /// <param name="pVisible">Видимость колонки</param>
        /// <param name="pType">Вид колонки</param>
        /// <returns>[true] - Колонка добавлена, иначе - [false]</returns>
        public bool __mColumnAdd(string pCaption, string pPrompt, string pFieldName, bool pReadOnly, bool pVisible, DATAGRIDCOLUMNTYPE pType, GRIDCELLTYPE pCellStyle = GRIDCELLTYPE.Normal)
        {
            return _cGrid.__mColumnAdd(pCaption, pPrompt, pFieldName, pReadOnly, pVisible, pType, pCellStyle);
        }
        /// <summary>
        /// Добавление колонок в сетку
        /// </summary>
        /// <returns>[true] - колонки добавлены, иначе - [false]</returns>
        public bool __mGridBuild()
        {
            bool vReturn = _cGrid.__mColumnsBuild();
            mMenuFieldFill();
            return vReturn;
        }
        /// <summary>
        /// Установка фокуса на сетку
        /// </summary>
        public void __mGridFocus()
        {
            _cGrid.Focus();
        }
        /// <summary>
        /// Загрузка данных
        /// </summary>
        /// <returns>[true] - данные загружены без ошибок, иначе - [false]</returns>
        /// <remarks>Вызывается из отображаемой формы [trdFormGrid...]</remarks>
        public bool __mDataLoad(string pQuery = "")
        {
            if (__eDataLoadBefore != null)
                __eDataLoadBefore(_cGrid, new EventArgs());

            bool vReturn = false; // Возвращаемое значение
            string vFilterExpression = ""; // Условия фильтра формы
            string vFormParentName = (FindForm() as elmForm).__fClassName_; // Название формы на которой расположен компонент
            appFileIni vFileIni = (FindForm() as elmForm).__oFileIni; // Объект для работы с инициализационными файлами
            ArrayList vParametersList = new ArrayList(); // Список параметров в конфигурационном файле для текущей формы
            string vFilterMessage = ""; // Выражение отображения фильтра
            int vCurrentRowIndex = -1; // Индекс строки выбранной в текущий момент
            /// Определение индекса текущей строки
            if (_cGrid.CurrentRow != null)
                vCurrentRowIndex = _cGrid.CurrentRow.Index;

            /// Получение списка параметров в секции формы 
            vParametersList = vFileIni.__mParametersList(vFormParentName);

            bool vUsed = true;

            /// Перебор параметров в секции формы
            foreach (string vParameter in vParametersList)
            {
                /// Чтение статуса условия фильтра
                if (vParameter.StartsWith("FilterStatus" + __fAreaId) == true)
                    vUsed = Convert.ToBoolean(vFileIni.__mValueRead(vFormParentName, vParameter));

                if (vUsed == true)
                {
                    if (vParameter.StartsWith("FilterExpression" + __fAreaId) == true)
                    {
                        if (vFilterExpression.Length > 0)
                            vFilterExpression = vFilterExpression + " and ";
                        vFilterExpression = vFilterExpression + vFileIni.__mValueRead(vFormParentName, vParameter);
                    }
                    if (vParameter.StartsWith("FilterMessage" + __fAreaId) == true)
                    {
                        if (vFilterMessage.Length > 0)
                            vFilterMessage = vFilterMessage + "\n";
                        vFilterMessage = vFilterMessage + vFileIni.__mValueRead(vFormParentName, vParameter);
                    }
                }
            }

            /// Отображение условия фильтра
            if (vFilterMessage.Length > 0)
            {
                _cSplitterFilterGrid.Panel1Collapsed = false;
                _cLabelFilterExpression.Text = vFilterMessage;
            }
            else
            {
                _cSplitterFilterGrid.Panel1Collapsed = true;
                _cLabelFilterExpression.Text = "";
            }
            if (_cLabelFilterExpression.Top + _cLabelFilterExpression.Height + elmInterface.__fIntervalVertical * 2 > _cSplitterFilterGrid.Panel1MinSize)
            {
                try
                {
                    _cSplitterFilterGrid.SplitterDistance = _cLabelFilterExpression.Top + _cLabelFilterExpression.Height + elmInterface.__fIntervalVertical * 2;
                }
                catch { }
            }
            __fDataFilter_ = vFilterExpression;
            /// Загрузка данных из источника данных
            if (pQuery.Trim().Length == 0)
            {
                vReturn = _cGrid.__mDataLoad(vFilterExpression, "");
                _cGrid.__mSortingLoad();
                #region /// Перевод курсора на строку выбранную до загрузки

                if (vCurrentRowIndex >= 0 & _cGrid.Rows.Count > 0)
                {
                    if (_cGrid.Rows.Count < vCurrentRowIndex + 1)
                        vCurrentRowIndex = _cGrid.Rows.Count - 1;
                    if(_cGrid.__oEssence.__mFieldExists("dsi" + __oEssence_.__fTableName) == true)
                        _cGrid.CurrentCell = _cGrid.Rows[vCurrentRowIndex].Cells["dsi" + __oEssence_.__fTableName];
                }

                /// Исправление логических значений
                vFilterExpression = vFilterExpression.Replace("False", "0");
                vFilterExpression = vFilterExpression.Replace("True", "1");

                #endregion Перевод курсора на строку выбранную до загрузки
            }
            else
            {
                _cGrid.__oDataTable = elmApplication.__oData.__mSqlQuery(pQuery);
                _cGrid.DataSource = _cGrid.__oDataTable;

                //elmFormBrowse vFormBrowse = new elmFormBrowse();
                //vFormBrowse.__mDataSourceDataTable(_cGrid.__oDataTable);
                //vFormBrowse.ShowDialog();

                //_cGrid.Refresh();
                vReturn = true;
            }

            if (__eDataLoadAfter != null)
                __eDataLoadAfter(_cGrid, new EventArgs());

            _cGrid.Refresh();

            return vReturn;
        }
        /// <summary>
        /// Очистка выпадающего меню кнопки управления
        /// </summary>
        public void __mButtonDropDownItemsClear(string pButtonName)
        {
            elmComponentToolbarButtonMenu vButton = new elmComponentToolbarButtonMenu();

            switch (pButtonName)
            {
                case "_cButtonEdit":
                    vButton = _cButtonEdit;
                    break;
                case "_cButtonOperations":
                    vButton = __cButtonOperations;
                    break;
                case "_cButtonReports":
                    vButton = __cButtonReports;
                    break;
                case "_cButtonColumns":
                    vButton = _cButtonColumns;
                    break;
            }

            vButton.DropDownItems.Clear();

            return;
        }
        /// <summary>
        /// Добавление меню в кнопку управления
        /// </summary>
        /// <param name="pMenuItem"></param>
        public void __mButtonDropDownItemAdd(string pButtonName, elmComponentMenuItem pMenuItem)
        {
            elmComponentToolbarButtonMenu vButton = new elmComponentToolbarButtonMenu();
            switch (pButtonName)
            {
                case "_cButtonEdit":
                    vButton = _cButtonEdit;
                    break;
                case "_cButtonOperations":
                    vButton = __cButtonOperations;
                    break;
                case "_cButtonReports":
                    vButton = __cButtonReports;
                    break;
                case "_cButtonColumns":
                    vButton = _cButtonColumns;
                    break;
            }
            vButton.DropDownItems.Add(pMenuItem);

            return;
        }
        /// <summary>
        /// Вставка меню в кнопку управления
        /// </summary>
        /// <param name="pButtonName"></param>
        /// <param name="pMenuItem"></param>
        /// <param name="pIndex"></param>
        public void __mButtonDropDownItemInsert(string pButtonName, elmComponentMenuItem pMenuItem, int pIndex)
        {
            elmComponentToolbarButtonMenu vButton = new elmComponentToolbarButtonMenu();
            switch (pButtonName)
            {
                case "_cButtonEdit":
                    vButton = _cButtonEdit;
                    break;
                case "_cButtonOperations":
                    vButton = __cButtonOperations;
                    break;
                case "_cButtonReports":
                    vButton = __cButtonReports;
                    break;
                case "_cButtonColumns":
                    vButton = _cButtonColumns;
                    break;
            }
            vButton.DropDownItems.Insert(pIndex, pMenuItem);

            return;
        }
        /// <summary>
        /// Добавление меню в кнопку управления
        /// </summary>
        /// <param name="pMenuItem"></param>
        public void __mButtonDropDownItemAdd(string pButtonName, string pMenuItem)
        {
            elmComponentToolbarButtonMenu vButton = new elmComponentToolbarButtonMenu();
            switch (pButtonName)
            {
                case "_cButtonEdit":
                    vButton = _cButtonEdit;
                    break;
                case "_cButtonOperations":
                    vButton = __cButtonOperations;
                    break;
                case "_cButtonReports":
                    vButton = __cButtonReports;
                    break;
                case "_cButtonColumns":
                    vButton = _cButtonColumns;
                    break;
            }
            vButton.DropDownItems.Add(pMenuItem);

            return;
        }
        /// <summary>
        /// Заполнение меню кнопки "Колонки" данными
        /// </summary>
        private void mMenuFieldFill()
        {
            if (_cGrid.__fColumnsList.Count > 0)
            {
                foreach (elmUnitGridColumn vColumn in _cGrid.__fColumnsList)
                {
                    elmComponentMenuItem _cToolStripMenuItemColumn = new elmComponentMenuItem();

                    #region Меню - видимость колонок

                    _cToolStripMenuItemColumn.Checked = Convert.ToBoolean((FindForm() as elmForm).__oFileIni.__mValueReadWrite(vColumn.__fVisible.ToString(), (FindForm() as elmForm).__fClassName_, "Field_" + vColumn.__fField)); // Загрузка состояния видимости поля
                    _cToolStripMenuItemColumn.CheckedChanged += mFieldsVisibleCheckedChanged;
                    _cToolStripMenuItemColumn.CheckOnClick = true;
                    _cToolStripMenuItemColumn.Font = elmApplication.__oInterface.__mFont(FONTS.Text);
                    _cToolStripMenuItemColumn.ImageScaling = ToolStripItemImageScaling.None;
                    _cToolStripMenuItemColumn.Name = vColumn.__fField;
                    _cToolStripMenuItemColumn.Text = vColumn.__fCaption;

                    /// Определение видимости соответствующего поля в сетке
                    if (_cToolStripMenuItemColumn.Name.ToUpper() == "DSI" + _cGrid.__oEssence.__fTableName.ToUpper())
                    {
                        _cGrid.Columns[vColumn.__fField].Visible = true;
                        _cToolStripMenuItemColumn.Enabled = false;
                    }
                    /// Определение видимости соответствующего поля в сетке
                    else
                        _cGrid.Columns[vColumn.__fField].Visible = _cToolStripMenuItemColumn.Checked;
                    _cButtonColumns.DropDownItems.Add(_cToolStripMenuItemColumn);

                    #endregion Меню - видимость колонок
                }
            }
            _cButtonColumns.PerformClick();
            __mSortingLoad(); // Загрузка сортировки
        }
        /// <summary>
        /// Загрузка сортировки в сетку
        /// </summary>
        public void __mSortingLoad()
        {
            _cGrid.__mSortingLoad();

            return;
        }
        /// <summary>
        /// Сохранение сортировки в сетке
        /// </summary>
        public void __mSortingSave()
        {
            _cGrid.__mSortingSave();

            return;
        }
        /// <summary>
        /// Получение значения поля курсора в текущей ячейке
        /// </summary>
        /// <param name="pFieldName">Название поля курсора</param>
        /// <returns></returns>
        public object __mCurrentRowFieldValue(string pFieldName)
        {
            return _cGrid.__mCurrentRowFieldValue(pFieldName);
        }
        /// <summary>
        /// Загрузка видимости колонок
        /// </summary>
        public void __mColumnsVisibilityLoad()
        {
            string vFormParentName = (FindForm() as elmForm).__fClassName_; // Название формы на которой расположен компонент
            appFileIni vFileIni = (FindForm() as elmForm).__oFileIni; // Объект для работы с инициализационными файлами
            vFileIni.__fFilePath = elmApplication.__oPathes.__mFileFormTunes(); // Указание настроечного файла

            foreach (DataGridViewColumn vGridColumn in _cGrid.Columns)
            {
                string vString = vFileIni.__mValueRead(vFormParentName.ToUpper(), "Field_" + vGridColumn.Name);
                try
                {
                    vGridColumn.Visible = Convert.ToBoolean(vString);
                }
                catch
                {
                    vGridColumn.Visible = true;
                }
            }

        }
        /// <summary>
        /// Сохранение видимости колонок
        /// </summary>
        public void __mColumnsVisibilitySave()
        {
            string vFormParentName = (FindForm() as elmForm).__fClassName_; // Название формы на которой расположен компонент
            appFileIni vFileIni = (FindForm() as elmForm).__oFileIni; // Объект для работы с инициализационными файлами
            vFileIni.__fFilePath = elmApplication.__oPathes.__mFileFormTunes(); // Указание настроечного файла

            #region Сохранение видимости полей

            foreach (DataGridViewColumn vDataGridColumn in _cGrid.Columns)
            {
                vFileIni.__mValueWrite(vDataGridColumn.Visible.ToString(), vFormParentName.ToUpper(), "Field_" + vDataGridColumn.Name);
            }

            #endregion Сохранение видимости полей
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Указывает форме, что есть открытые DropDown у кнопок
        /// </summary>
        //      public bool __fDropDownOpened = false;
        /// <summary>
        /// Использование кода по умолчанию для меню 'Отчеты / Список'
        /// </summary>
        public bool _fButtonReportsListDefaultCode = true;
        /// <summary>
        /// Блокировка изменения документа
        /// </summary>
        public bool __fEditLock = false;
        /// <summary>
        /// Тип формы для изменения данных
        /// </summary>
        public CONTROLsOPENEDTYPES __fFormOpenedType = CONTROLsOPENEDTYPES.FormRecord;
        /// <summary>
        /// Отметка, о том что форма была закрыта при нажатии на клавишу 'Выбрать'
        /// </summary>
        public bool __fIsSelected = false;
        /// <summary>
        /// При выборе записи в таблице нажимать кнопку '_cButtonSelect'
        /// </summary>
        public bool __fOnRowEnterClickSelect = false;

        /// <summary>
        /// Объект открытой формы для правки записи
        /// </summary>
        public elmFormRecord __oFormRecord = null;

        #endregion Атрибуты

        #region - Компоненты

        /// <summary>
        /// Кнопка 'Видимость колонок'
        /// </summary>
        protected elmComponentToolbarButtonMenu _cButtonColumns = new elmComponentToolbarButtonMenu();
        /// <summary>
        /// Кнопка 'Правка'
        /// </summary>
        protected elmComponentToolbarButtonMenu _cButtonEdit = new elmComponentToolbarButtonMenu();

        #region Меню кнопки 'Правка'

        /// <summary>
        /// Кнопка 'Правка / Копировать'
        /// </summary>
        protected elmComponentMenuItem _cButtonEditCopy = new elmComponentMenuItem();
        /// <summary>
        /// Кнопка 'Правка / Создать'
        /// </summary>
        protected elmComponentMenuItem _cButtonEditCreate = new elmComponentMenuItem();
        /// <summary>
        /// Кнопка 'Правка / Изменить'
        /// </summary>
        protected elmComponentMenuItem _cButtonEditEdit = new elmComponentMenuItem();
        /// <summary>
        /// Кнопка 'Правка / Удалить'
        /// </summary>
        protected elmComponentMenuItem _cButtonEditRemove = new elmComponentMenuItem();
        /// <summary>
        /// Кнопка 'Правка / Восстановить'
        /// </summary>
        protected elmComponentMenuItem _cButtonEditRestore = new elmComponentMenuItem();

        #endregion Меню кнопки 'Правка'

        /// <summary>
        /// Кнопка 'Операции'
        /// </summary>
        public elmComponentToolbarButtonMenu __cButtonOperations = new elmComponentToolbarButtonMenu();

        #region Меню кнопки 'Операции'
        #endregion Меню кнопки 'Операции'

        /// <summary>
        /// Кнопка 'Отчеты'
        /// </summary>
        public elmComponentToolbarButtonMenu __cButtonReports = new elmComponentToolbarButtonMenu();

        #region Меню кнопки 'Отчеты'

        /// <summary>
        /// Пункт меню 'Текущий список' кнопки 'Отчеты'
        /// </summary>
        protected elmComponentMenuItem _cButtonReportsCurrentList = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'История записи' кнопки 'Отчеты'
        /// </summary>
        protected elmComponentMenuItem _cButtonReportsHistory = new elmComponentMenuItem();

        #endregion Меню кнопки 'Отчеты'

        /// <summary>
        /// Кнопка 'Обновить'
        /// </summary>
        protected elmComponentToolbarButton _cButtonRefresh = new elmComponentToolbarButton();
        /// <summary>
        /// Кнопка 'Выбрать'
        /// </summary>
        protected elmComponentToolbarButton _cButtonSelect = new elmComponentToolbarButton();
        /// <summary>
        /// Разделитель
        /// </summary>
        protected elmComponentSplitter _cSplitterFilterGrid = new elmComponentSplitter();
        /// <summary>
        /// Заголовок условия фильтра
        /// </summary>
        protected elmComponentLabel _cLabelFilterCaption = new elmComponentLabel();
        /// <summary>
        /// Содержание условия фильтра
        /// </summary>
        protected elmComponentLabel _cLabelFilterExpression = new elmComponentLabel();
        /// <summary>
        /// Сетка
        /// </summary>
        protected elmComponentGrid _cGrid = new elmComponentGrid();

        #endregion Компоненты

        #region - Объекты

        /// <summary>
        /// Тип формы для построения фильтра
        /// </summary>
        public Type __oFormFilter;
        /// <summary>
        /// Тип формы для изменения данных
        /// </summary>
        public Type __oFormOpened;
        /// <summary>
        /// Сущность данных содержания документа
        /// </summary>
        public datUnitEssence __oEssenceContent;

        #endregion Объекты

        /// <summary>
        /// Значение текущего установленного фильтра
        /// </summary>
        private string fFilterExpression = "";

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        #region Доступность кнопок управления

        /// <summary>
        /// Доступность кнопки 'Колонки'
        /// </summary>
        public bool __fButtonColumnsEnabled_
        {
            get { return _cButtonColumns.Enabled; }
            set { _cButtonColumns.Enabled = value; }
        }
        /// <summary>
        /// Доступность кнопки 'Обновить'
        /// </summary>
        public bool __fButtonRefreshEnabled_
        {
            get { return _cButtonRefresh.Enabled; }
            set { _cButtonRefresh.Enabled = value; }
        }
        /// <summary>
        /// Доступность кнопки 'Выбрать'
        /// </summary>
        public bool __fButtonSelectEnabled_
        {
            get { return _cButtonSelect.Enabled; }
            set { _cButtonSelect.Enabled = value; }
        }
        /// <summary>
        /// Доступность кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditEnabled_
        {
            get { return _cButtonEdit.Enabled; }
            set { _cButtonEdit.Enabled = value; }
        }
        /// <summary>
        /// Доступность пункта меню 'Копировать' кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditCopyEnabled_
        {
            get { return _cButtonEditCopy.Enabled; }
            set { _cButtonEditCopy.Enabled = value; }
        }
        /// <summary>
        /// Доступность пункта меню 'Создать' кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditCreateEnabled_
        {
            get { return _cButtonEditCreate.Enabled; }
            set { _cButtonEditCreate.Enabled = value; }
        }
        /// <summary>
        /// Доступность пункта меню 'Изменить' кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditEditEnabled_
        {
            get { return _cButtonEditEdit.Enabled; }
            set { _cButtonEditEdit.Enabled = value; }
        }
        /// <summary>
        /// Доступность пункта меню 'Удалить' кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditRemoveEnabled_
        {
            get { return _cButtonEditRemove.Enabled; }
            set { _cButtonEditRemove.Enabled = value; }
        }
        /// <summary>
        /// Доступность пункта меню 'Восстановить' кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditRestoreEnabled_
        {
            get { return _cButtonEditRestore.Enabled; }
            set { _cButtonEditRestore.Enabled = value; }
        }
        /// <summary>
        /// Доступность кнопки 'Операции'
        /// </summary>
        public bool __fButtonOperationsEnabled_
        {
            get { return __cButtonOperations.Enabled; }
            set { __cButtonOperations.Enabled = value; }
        }

        /// <summary>
        /// Доступность кнопки 'Отчеты'
        /// </summary>
        public bool __fButtonReportsEnabled_
        {
            get { return __cButtonReports.Enabled; }
            set { __cButtonReports.Enabled = value; }
        }
        /// <summary>
        /// Доступность пункта меню 'Текущий список' кнопки 'Отчеты'
        /// </summary>
        public bool __fButtonReportsCurrentListEnabled_
        {
            get { return _cButtonReportsCurrentList.Enabled; }
            set { _cButtonReportsCurrentList.Enabled = value; }
        }
        /// <summary>
        /// Доступность пункта меню 'История корректировок' кнопки 'Отчеты'
        /// </summary>
        public bool __fButtonReportsHistoryEnabled_
        {
            get { return _cButtonReportsHistory.Enabled; }
            set { _cButtonReportsHistory.Enabled = value; }
        }

        #endregion Доступность кнопок управления

        #region Видимость кнопок управления

        /// <summary>
        /// Видимость кнопки 'Обновить'
        /// </summary>
        public bool __fButtonRefreshVisible_
        {
            get { return _cButtonRefresh.Visible; }
            set { _cButtonRefresh.Visible = value; }
        }
        /// <summary>
        /// Видимость кнопки 'Выбрать'
        /// </summary>
        public bool __fButtonSelectVisible_
        {
            get { return _cButtonSelect.Visible; }
            set { _cButtonSelect.Visible = value; }
        }

        /// <summary>
        /// Видимость кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditVisible_
        {
            get { return _cButtonEdit.Visible; }
            set { _cButtonEdit.Visible = value; }
        }
        /// <summary>
        /// Видимость пункта меню 'Копировать' кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditCopyVisible_
        {
            get { return _cButtonEditCopy.Visible; }
            set { _cButtonEditCopy.Visible = value; }
        }
        /// <summary>
        /// Видимость пункта меню 'Создать' кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditCreateVisible_
        {
            get { return _cButtonEditCreate.Visible; }
            set { _cButtonEditCreate.Visible = value; }
        }
        /// <summary>
        /// Видимость пункта меню 'Изменить' кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditEditVisible_
        {
            get { return _cButtonEditEdit.Visible; }
            set { _cButtonEditEdit.Visible = value; }
        }
        /// <summary>
        /// Видимость пункта меню 'Удалить' кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditRemoveVisible_
        {
            get { return _cButtonEditRemove.Visible; }
            set { _cButtonEditRemove.Visible = value; }
        }
        /// <summary>
        /// Видимость пункта меню 'Восстановить' кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditRestoreVisible_
        {
            get { return _cButtonEditRestore.Visible; }
            set { _cButtonEditRestore.Visible = value; }
        }

        /// <summary>
        /// Видимость кнопки 'Операции'
        /// </summary>
        public bool __fButtonOperationsVisible_
        {
            get { return __cButtonOperations.Visible; }
            set { __cButtonOperations.Visible = value; }
        }

        /// <summary>
        /// Видимость кнопки 'Отчеты'
        /// </summary>
        public bool __fButtonReportsVisible_
        {
            get { return __cButtonReports.Visible; }
            set { __cButtonReports.Visible = value; }
        }
        /// <summary>
        /// Видимость пункта меню 'Текущий список' кнопки 'Отчеты'
        /// </summary>
        public bool __fButtonReportsCurrentListVisible_
        {
            get { return _cButtonReportsHistory.Visible; }
            set { _cButtonReportsHistory.Visible = value; }
        }
        /// <summary>
        /// Видимость пункта меню 'История изменений' кнопки 'Отчеты'
        /// </summary>
        public bool __fButtonReportsHistoryVisible_
        {
            get { return _cButtonReportsHistory.Visible; }
            set { _cButtonReportsHistory.Visible = value; }
        }

        #endregion Видимость кнопок управления

        #region Надписи на кнопках 

        /// <summary>
        /// Надпись на кнопке 'Правка / Копировать' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditCopyCaption
        {
            set { _cButtonEditCopy.__fCaption_ = value; }
        }
        /// <summary>
        /// Надпись на кнопке 'Правка / Создать' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditCreateCaption
        {
            set { _cButtonEditCreate.__fCaption_ = value; }
        }
        /// <summary>
        /// Надпись на кнопке 'Правка / Изменить' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditEditCaption
        {
            set { _cButtonEditEdit.__fCaption_ = value; }
        }
        /// <summary>
        /// Надпись на кнопке 'Правка / Удалить' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditRemoveCaption
        {
            set { _cButtonEditRemove.__fCaption_ = value; }
        }
        /// <summary>
        /// Надпись на кнопке 'Правка / Восстановить' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditRestoreCaption
        {
            set { _cButtonEditRestore.__fCaption_ = value; }
        }

        #endregion Надписи на кнопках

        #region Подсказки к кнопкам

        /// <summary>
        /// Подсказка к кнопке 'Выбрать' переведенная на язык пользователя
        /// </summary>
        public string __fButtonSelectToolTipText
        {
            set { _cButtonSelect.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Подсказка к кнопке 'Обновить' переведенная на язык пользователя
        /// </summary>
        public string __fButtonRefreshToolTipText
        {
            set { _cButtonRefresh.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Подсказка к кнопке 'Правка' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditToolTipText
        {
            set { _cButtonEdit.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Подсказка к кнопке 'Правка / Копировать' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditCopyToolTipText
        {
            set { _cButtonEditCopy.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Подсказка к кнопке 'Правка / Создать' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditCreateToolTipText
        {
            set { _cButtonEditCreate.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Подсказка к кнопке 'Правка / Изменить' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditEditToolTipText
        {
            set { _cButtonEditEdit.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Подсказка к кнопке 'Правка / Удалить' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditRemoveToolTipText
        {
            set { _cButtonEditRemove.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Подсказка к кнопке 'Правка / Восстановить' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditRestoryToolTipText
        {
            set { _cButtonEditRestore.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Подсказка к кнопке 'Операции' переведенная на язык пользователя
        /// </summary>
        public string __fButtonOperationsToolTipText
        {
            set { __cButtonOperations.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Подсказка к кнопке 'Отчеты' переведенная на язык пользователя
        /// </summary>
        public string __fButtonReportsToolTipText
        {
            set { __cButtonReports.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Подсказка к кнопке 'Отчеты' переведенная на язык пользователя
        /// </summary>
        public string __fButtonColumnsToolTipText
        {
            set { _cButtonColumns.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }

        #endregion Подсказки к кнопкам

        #region Изображения на кнопках

        /// <summary>
        /// Изображение на кнопке 'Помощь'
        /// </summary>
        public Image __fButtonSelectImage
        {
            set { _cButtonSelect.Image = value; }
        }
        /// <summary>
        /// Изображение на кнопке 'Обновить'
        /// </summary>
        public Image __fButtonRefreshImage
        {
            set { _cButtonRefresh.Image = value; }
        }
        /// <summary>
        /// Изображение на кнопке 'Правка'
        /// </summary>
        public Image __fButtonEditImage
        {
            set { _cButtonEdit.Image = value; }
        }
        /// <summary>
        /// Изображение на кнопке 'Операции'
        /// </summary>
        public Image __fButtonOperationsImage
        {
            set { __cButtonOperations.Image = value; }
        }
        /// <summary>
        /// Изображение на кнопке 'Отчеты'
        /// </summary>
        public Image __fButtonReportsImage
        {
            set { __cButtonReports.Image = value; }
        }
        /// <summary>
        /// Изображение на кнопке 'Отчеты'
        /// </summary>
        public Image __fButtonColumnsImage
        {
            set { _cButtonColumns.Image = value; }
        }

        #endregion Изображения на кнопках

        /// <summary>
        /// Сущность данных
        /// </summary>
        public datUnitEssence __oEssence_
        {
            get { return _cGrid.__oEssence; }
            set { _cGrid.__oEssence = value; }
        }
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int __fRecordClue_
        {
            get { return _cGrid.__fRecordClue_; }
            set { _cGrid.__fRecordClue_ = value; }
        }
        /// <summary>
        /// Выбранная строка в сетке
        /// </summary>
        public DataGridViewRow __fSelectedRow_
        {
            get { return _cGrid.SelectedRows[0]; }
        }
        /// <summary>
        /// Коллекция колонок добавленных в сетку
        /// </summary>
        public DataGridViewColumnCollection __fColumns_
        {
            get { return _cGrid.Columns; }
        }
        public string __fDataFilter_
        {
            get { return fFilterExpression; }
            set { fFilterExpression = value; }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает после выполнения выбора кнопки 'Обновить' левой клавишей мыши 
        /// </summary>
        public event EventHandler __eButtonRefreshClickAfter;
        /// <summary>
        /// Возникает перед выбором кнопки 'Обновить' левой клавишей мыши 
        /// </summary>
        public event EventHandler __eButtonRefreshClickBefore;
        /// <summary>
        /// Возникает при выборе кнопки 'Обновить' правой клавишей мыши 
        /// </summary>
        public event EventHandler __eButtonRefresh_ClickRight;
        /// <summary>
        /// Возникает при выборе строки сетки
        /// </summary>
        public event EventHandler __eRowChanged;
        /// <summary>
        /// Возникает при двойном клике по строке сетки
        /// </summary>
        public event EventHandler __eRowDoubleClick;
        /// <summary>
        /// Возникает при выборе пункта меню 'Копировать' кнопки 'Правка'
        /// </summary>
        //public event EventHandler __eButtonEditCopyClick;
        /// <summary>
        /// Возникает перед выбора пункта меню 'Создать' кнопки 'Правка'
        /// </summary>
        public event EventHandler __eButtonEditCreateClickBefore;
        /// <summary>
        /// Возникает после создания формы при выборе пункта меню 'Создать' кнопки 'Правка'
        /// </summary>
        public event EventHandler __eButtonEditCreateClickBeforeShowForm;
        /// <summary>
        /// Возникает при выборе пункта меню 'Изменить' кнопки 'Правка'
        /// </summary>
        public event EventHandler __eButtonEditEditClick;
        /// <summary>
        /// Возникает при выборе пункта меню 'Отчеты / Список'
        /// </summary>
        public event EventHandler __eButtonReportsListClick;
        /// <summary>
        /// Возникает при выборе пункта меню 'Операции / Права пользователей'
        /// </summary>
        public event EventHandler __eButtonUsersAccessClick;
        /// <summary>
        /// Возникает при выборе кнопки 'Выбрать запись' левой клавишей мыши 
        /// </summary>
        public event EventHandler __eButtonSelectClick;
        /// <summary>
        /// Возникает после загрузки данных
        /// </summary>
        public event EventHandler __eDataLoadAfter;
        /// <summary>
        /// Возникает перед загрузкой данных
        /// </summary>
        public event EventHandler __eDataLoadBefore;

        #endregion = СОБЫТИЯ
    }
}
