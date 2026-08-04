using nlApplication;
using nlData;
using nlReportHtml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmAreaTree.cs
    /// </summary>
    /// <remarks>Класс-область для правки древовидных данных</remarks>
    public class elmAreaTree : elmArea
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

            Panel2.Controls.Add(_cTree);
            Panel2.Controls.SetChildIndex(_cTree, 0);

            _cToolBar.Items.Insert(0, _cButtonSelect);
            _cToolBar.Items.Insert(1, _cButtonRefresh);
            _cToolBar.Items.Add(_cButtonReports);
            _cToolBar.Items.Add(_cButtonOperations);
            _cToolBar.Items.Add(_cButtonEdit);

            _cButtonEdit.DropDownItems.Add(_cButtonEditCreate);
            _cButtonEdit.DropDownItems.Add(_cButtonEditCopy);
            _cButtonEdit.DropDownItems.Add(_cButtonEditEdit);
            _cButtonEdit.DropDownItems.Add(_cButtonEditRemove);
            _cButtonEdit.DropDownItems.Add(_cButtonEditRestore);

            _cButtonOperations.DropDownItems.Add(_cButtonOperationsAccess);
            _cButtonReports.DropDownItems.Add(_cButtonReportsCurrentList);
            _cButtonReports.DropDownItems.Add(_cButtonReportsHistory);

            #endregion Размешение компонентов

            #region /// Настройка компонентов

            // __cToolBar
            {
                // __cButtonRefresh
                {
                    _cButtonRefresh.__eClickLeft += mButtonRefresh_eMouseClickLeft;
                    _cButtonRefresh.__eClickRight += mButtonRefresh_eMouseClickRight;
                    _cButtonRefresh.Image = global::nlResourcesImages.Properties.Resources._Arrow_Refresh_g32;
                    _cButtonRefresh.ToolTipText = "[ F5 ] " + elmApplication.__oTunes.__mTranslate("Обновить");
                }
                // __cButtonEdit
                {
                    _cButtonEdit.Alignment = ToolStripItemAlignment.Right;
                    _cButtonEdit.DropDownOpened += mButton_DropDownOpened;
                    _cButtonEdit.Image = global::nlResourcesImages.Properties.Resources._Page_b32;
                    _cButtonEdit.ToolTipText = "[ Ctrl + E ] " + elmApplication.__oTunes.__mTranslate("Правка");
                    {
                        _cButtonEditCopy.Click += mButtonEditCopy_Click;
                        _cButtonEditCopy.Image = global::nlResourcesImages.Properties.Resources._PageCopy_b16;
                        _cButtonEditCopy.__fCaption_ = "Копировать";                     

                        _cButtonEditEdit.Click += mButtonEditEdit_Click;
                        _cButtonEditEdit.Image = global::nlResourcesImages.Properties.Resources._PageEdit_b16;
                        _cButtonEditEdit.__fCaption_ = "Изменить";

                        _cButtonEditCreate.Image = global::nlResourcesImages.Properties.Resources._Page_b16;
                        _cButtonEditCreate.__fCaption_ = "Создать";
                        _cButtonEditCreate.Click += mButtonEditCreate_Click;

                        _cButtonEditRemove.Image = global::nlResourcesImages.Properties.Resources._PageDelete_b16;
                        _cButtonEditRemove.__fCaption_ = "Исключить";
                        _cButtonEditRemove.Click += mButtonEditRemove_Click;

                        //_cButtonEditRestore.Click += _mButtonEditRestore_Click;
                        _cButtonEditRestore.Image = global::nlResourcesImages.Properties.Resources._PageAdd_b16;
                        _cButtonEditRestore.__fCaption_ = "Восстановить";
                    }

                }
                // __cButtonOperations
                {
                    _cButtonOperations.Alignment = ToolStripItemAlignment.Right;
                    _cButtonOperations.Image = global::nlResourcesImages.Properties.Resources._PageGear_y32;
                    _cButtonOperations.ToolTipText = "[ Ctrl + O ] " + elmApplication.__oTunes.__mTranslate("Операции");
                    {
                        //t _cButtonOperationsAccess.Click += mButtonOperationsAccess_Click;
                        _cButtonOperationsAccess.Image = global::nlResourcesImages.Properties.Resources._Person_Police_k16;
                        _cButtonOperationsAccess.__fCaption_ = "Определение прав пользователей";
                    }
                }
                // __cButtonReports
                {
                    _cButtonReports.Alignment = ToolStripItemAlignment.Right;
                    _cButtonReports.Image = global::nlResourcesImages.Properties.Resources._PagePrinter_y32;
                    _cButtonReports.ToolTipText = "[ Ctrl + R ] " + elmApplication.__oTunes.__mTranslate("Отчеты");
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
                    _cButtonSelect.__eClickLeft += mButtonSelect_ClickLeft;
                    _cButtonSelect.__eClickRight += mButtonSelect_ClickRight;
                    _cButtonSelect.Image = global::nlResourcesImages.Properties.Resources._Sign_Tick_g32;
                    _cButtonSelect.ToolTipText = "[ Ctrl + A ] " + elmApplication.__oTunes.__mTranslate("Выбрать");
                }
            }
            // __cConsoleInputs
            {
                _cTree.Dock = DockStyle.Fill;
            }
            // __cTree
            {
                _cTree.Dock = DockStyle.Fill;
                _cTree.DrawNode += __cTree_DrawNode;
                _cTree.__eClickLeft += mTree_eMouseClickLeft;
                _cTree.__eClickRight += mTree_eMouseClickRight;
                _cTree.DoubleClick += mTree_DoubleClick;
            }

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }

        #endregion Объект

        #region - Поведение

        /// <summary>
        /// Выполняется при разрушении объекта
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            __mNodesStatusesSave();
            base.OnHandleDestroyed(e);
        }
        /// <summary>
        /// Выполняется при двойном клике мыши по узлу дерева
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mTree_DoubleClick(object sender, EventArgs e)
        {
            /// Вызывается событие __eNodeMouse_DoubleClick
            if (__eNodeMouse_Double != null)
                __eNodeMouse_Double(_cTree.SelectedNode, e);
            /// Поле __fIsSelected устанавливается в 'true' 
            __fIsSelected = true;
        }
        /// <summary>
        /// Выполняется при смене вкладки  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mPageBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            __mDataLoad();
        }

        #region Кнопки управления

        #region ! Выполнение нажатия на кнопки управления из внешних объектов

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
            _cButtonOperations.ShowDropDown();

            return;
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Отчеты'
        /// </summary>
        public void __mPressButtonReports()
        {
            _cButtonReports.ShowDropDown();

            return;
        }

        #endregion Выполнение нажатия на кнопки управления из внешних объектов

        /// <summary>
        /// Выполняется при выборе кнопки 'Правка'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButton_DropDownOpened(object sender, EventArgs e)
        {
            _fDropDownOpened = true;
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Правка / Копировать' левой кнопкой мыши 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonEditCopy_Click(object sender, EventArgs e)
        {
            _fDropDownOpened = true;
            elmForm vForm = FindForm() as elmForm;
            if (vForm != null & __oFormOpened != null)
            {
                /// Вызов формы редактирования документа
                if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormPages)
                {
                    elmFormPages vFormDocument = (elmFormPages)Activator.CreateInstance(__oFormOpened);
                    //(vFormDocument as crlFormDocument).__cAreaDocument.__fRecordClueForCopy = _cTree.__fRecordClue_;
                    //(vFormDocument as crlFormDocument).ShowDialog();
                }
                /// Вызов формы редактирования записи
                if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormRecord)
                {
                    elmFormRecord vFormRecord = (elmFormRecord)Activator.CreateInstance(__oFormOpened);
                    //(vFormRecord as crlFormRecord).__cAreaRecord.__fRecordClueForCopy = _cGrid.__fRecordClue_;
                    //(vFormRecord as crlFormRecord).ShowDialog();
                }
                __mDataLoad(); /// Перегрузка данных
            }
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Правка / Создать' левой кнопкой мыши 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonEditCreate_Click(object sender, EventArgs e)
        {
            int vRecordNewClue = -1; // Ключ созданной записи

            if (__eButtonEditCreate_Click != null)
                __eButtonEditCreate_Click(sender, e);

            elmForm vForm = FindForm() as elmForm;
            if (vForm != null & __oFormOpened != null)
            {
                /// Вызов формы редактирования документа
                if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormPages)
                {
                    elmFormPages vFormDocument = (elmFormPages)Activator.CreateInstance(__oFormOpened);
                    (vFormDocument as elmFormPages).ShowDialog();
                }
                /// Вызов формы редактирования записи
                if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormRecord)
                {
                    elmFormRecord vFormRecord = (elmFormRecord)Activator.CreateInstance(__oFormOpened);
                    vFormRecord.__cAreaRecord.__fRecordClueParent = _cTree.__fRecordClue_;
                    (vFormRecord as elmFormRecord).ShowDialog();
                    vRecordNewClue = (vFormRecord as elmFormRecord).__cAreaRecord.__fRecordClue;
                }
                /// Перегрузка данных
                __mDataLoad();
                /// Разворачивание нового узла
                if (vRecordNewClue > 0)
                { 
                    elmUnitTreeNode vTreeNodeNew = __mNodeGetByClueOnLoad(vRecordNewClue); 
                    _cTree.SelectedNode = vTreeNodeNew;
                    _cTree.Refresh();
                }
            }
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Правка / Изменить' левой кнопкой мыши 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonEditEdit_Click(object sender, EventArgs e)
        {
            if (__eButtonEditEdit_Click != null)
                __eButtonEditEdit_Click(sender, e);
            
            int vRecordClue = __fRecordClue_;

            _fDropDownOpened = true;

            if (__fEditLock == false)
            {
                elmForm vForm = FindForm() as elmForm;

                if (vForm != null & __oFormOpened != null)
                {
                    /// Вызов формы редактирования документа
                    if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormPages)
                    {
                        elmFormPages vFormDocument = (elmFormPages)Activator.CreateInstance(__oFormOpened);
                        vFormDocument.__cAreaPages.__fRecordClue = _cTree.__fRecordClue_;
                        (vFormDocument as elmFormPages).ShowDialog();
                    }
                    /// Вызов формы редактирования записи
                    if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormRecord)
                    {
                        elmFormRecord vFormRecord = (elmFormRecord)Activator.CreateInstance(__oFormOpened);
                        vFormRecord.__cAreaRecord.__fRecordClue = _cTree.__fRecordClue_;
                        (vFormRecord as elmFormRecord).ShowDialog();
                    }
                    __mDataLoad(); /// Перегрузка данных
                }
            }
            else
                __fEditLock = false;

            elmUnitTreeNode vTreeNodeNew = __mNodeGetByClueOnLoad(vRecordClue);
            _cTree.SelectedNode = vTreeNodeNew;
            _cTree.Refresh();

        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Правка / Удалить' левой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonEditRemove_Click(object sender, EventArgs e)
        {
            if (__eButtonEditRemove_Click != null)
                __eButtonEditRemove_Click(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполняется при выборе кнопки 'Помощь' левой клавишей мыши
        /// </summary>
        private void mButtonRefresh_eMouseClickLeft(object sender, EventArgs e)
        {
            if (__eButtonRefresh_Click != null)
                __eButtonRefresh_Click(_cButtonRefresh, new EventArgs());
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Помощь' правой клавишей мыши
        /// </summary>
        private void mButtonRefresh_eMouseClickRight(object sender, EventArgs e)
        {
            if (__eButtonRefresh_ClickRight != null)
                __eButtonRefresh_ClickRight(_cButtonRefresh, new EventArgs());
        }
        /// <summary>
        /// Выполняется при выборе меню "Отчеты/Текущий список"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonReportsCurrentList_Click(object sender, EventArgs e)
        {
            //_fDropDownOpened = true;
            //rhtReport vReport = new rhtReport();
            //vReport.__mCreate();
            //vReport.__fTitle = elmApplication.__oTunes.__mTranslate("Список '{0}'", (FindForm() as elmFormGrid).Text);
            //vReport.__fColumnsCountInReport = 0;

            ///// Подсчет отображаемых колонок
            //for (int vColumnNumber = 0; vColumnNumber < _cTree.__.Columns.Count; vColumnNumber++)
            //{
                
            //    if (vColumnNumber != _cTree.Columns.Count)
            //        if (_cTree.Columns[vColumnNumber].Visible == true)
            //            vReport.__fColumnsCountInReport++;
            //}
            ///// Отображение заголовка
            //vReport.__mRow();
            //vReport.__mCell(vReport.__fTitle, "CL=Caption", "SC=" + vReport.__fColumnsCountInReport.ToString());
            //vReport.__mRow();
            //vReport.__mCell(_cLabelFilterCaption.Text, "SC=" + vReport.__fColumnsCountInReport.ToString(), "CL=TimeUser");
            //vReport.__mRow();
            //vReport.__mCell(_cLabelFilterExpression.Text.Replace("\n", "<BR>"), "SC=" + vReport.__fColumnsCountInReport.ToString(), "CL=TimeUser");
            //vReport.__mRowEmpty();
            //vReport.__mTime("CL=TimeUser");
            //vReport.__mUser(elmApplication.__oData.__mUserAlias(), "CL=TimeUser");
            //vReport.__mRowEmpty();

            ///// Построение заголовка таблицы
            //vReport.__mRow();
            //for (int vColumnNumber = 0; vColumnNumber < _cGrid.Columns.Count; vColumnNumber++)
            //{
            //    if (vColumnNumber != _cGrid.Columns.Count)
            //        if (_cGrid.Columns[vColumnNumber].Visible == true)
            //            vReport.__mCell(_cGrid.Columns[vColumnNumber].HeaderCell.Value, "CL=HeaderCell");
            //        else
            //            if (_cGrid.Columns[vColumnNumber].Visible == true)
            //            vReport.__mCell(_cGrid.Columns[vColumnNumber].HeaderCell.Value, "CL=HeaderCell-Last");
            //}
            ///// Отображение данных
            ///// Перебор строк в курсоре
            //foreach (DataGridViewRow vViewRow in _cGrid.Rows)
            //{
            //    vReport.__mRow();
            //    /// Перебор полей
            //    for (int vColumnNumber = 0; vColumnNumber < _cGrid.Columns.Count; vColumnNumber++)
            //    {
            //        if (vColumnNumber != _cGrid.Columns.Count)
            //            if (_cGrid.Columns[vColumnNumber].Visible == true)
            //                if (vViewRow.Cells[vColumnNumber].Value != null)
            //                {
            //                    if (vViewRow.Cells[vColumnNumber].Value.GetType() == typeof(bool))
            //                        vReport.__mCell(Convert.ToBoolean(vViewRow.Cells[vColumnNumber].Value) == true ? elmApplication.__oTunes.__mTranslate("Да") : elmApplication.__oTunes.__mTranslate("Нет"), "CL=DataCell");
            //                    else
            //                        vReport.__mCell(vViewRow.Cells[vColumnNumber].Value.ToString(), "CL=DataCell");
            //                }
            //                else
            //                    if (_cGrid.Columns[vColumnNumber].Visible == true)
            //                    if (vViewRow.Cells[vColumnNumber].Value != null)
            //                    {
            //                        if (vViewRow.Cells[vColumnNumber].Value.GetType() == typeof(bool))
            //                            vReport.__mCell(Convert.ToBoolean(vViewRow.Cells[vColumnNumber].Value) == true ? elmApplication.__oTunes.__mTranslate("Да") : elmApplication.__oTunes.__mTranslate("Нет"), "CL=DataCell-Last");
            //                        else
            //                            vReport.__mCell(vViewRow.Cells[vColumnNumber].Value.ToString(), "CL=DataCell-Last");
            //                    }
            //    }
            //}

            //vReport.__mFile();
            //elmFormReportPreview vFormReportPreview = new elmFormReportPreview();
            //vFormReportPreview._cAreaReportPreview.__fUrl_ = vReport.__fFilePath;
            //vFormReportPreview.ShowDialog();
        }
        /// <summary>
        /// Выполняется при выборе меню "Отчеты/История"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonReportsHistory_Click(object sender, EventArgs e)
        {
            _fDropDownOpened = true;
            DataTable vDataTable = _cTree.__oEssence.__mRecordChanges(_cTree.__fRecordClue_);
            /// Выполнение обратной сортировки истории
            DataView vDataView = vDataTable.DefaultView;
            vDataView.Sort = "dtmRrdLck desc"; // Обратная сортировка по времени корректировки
            vDataTable = vDataView.ToTable();
            string vTableName = _cTree.__oEssence.__fTableName; // Название текущей таблицы
            rhtReport vReport = new rhtReport();
            vReport.__mCreate();
            vReport.__fTitle = elmApplication.__oTunes.__mTranslate("Таблица '{0}'", __oEssence_.__fTableName);

            /// Отображение заголовка
            vReport.__mRow();
            vReport.__mCell(vReport.__fTitle, "CL=Caption", "SC=" + vReport.__fColumnsCountInReport.ToString(), "A=center", "SC=Max");
            vReport.__mRow();
            if (FindForm().GetType() == typeof(elmFormGrid))
                vReport.__mCell(elmApplication.__oTunes.__mTranslate("История записи '{0}' id={1}", (FindForm() as elmFormGrid).Text, _cTree.__fRecordClue_), "SC=Max");
            else
                vReport.__mCell(elmApplication.__oTunes.__mTranslate("История записи '{0}' id={1}", (FindForm() as elmForm).Text, _cTree.__fRecordClue_), "SC=Max");

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
                        case "CLU":
                            vReport.__mCell(elmApplication.__oTunes.__mTranslate("Ключ"), "CL=HeaderCell");
                            break;
                        case "GID":
                            vReport.__mCell(elmApplication.__oTunes.__mTranslate("Идентификатор"), "CL=HeaderCell");
                            break;
                        case "ELD":
                            vReport.__mCell(elmApplication.__oTunes.__mTranslate("Исключена"), "CL=HeaderCell");
                            break;
                        case "CHG":
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
                            string vCaption = elmApplication.__oData.__mModelFieldDescription(__oEssence_.__fTableName, vDataTable.Columns[vColumnNumber].ColumnName);
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
        /// Выполняется при выборе кнопки 'Выбрать' левой клавишей мыши
        /// </summary>
        private void mButtonSelect_ClickLeft(object sender, EventArgs e)
        {
            __fIsSelected = true;
            if (__eButtonSelect_Click != null)
                __eButtonSelect_Click(this, new EventArgs());

        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Выбрать' првой клавишей мыши
        /// </summary>
        private void mButtonSelect_ClickRight(object sender, EventArgs e)
        {
            __fIsSelected = true;
            if (__eButtonSelect_ClickRight != null)
                __eButtonSelect_ClickRight(this, new EventArgs());

        }

        #endregion Кнопки управления

        #region Дерево

        /// <summary>
        /// Выполняется при выборе узла дерева левой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mTree_eMouseClickLeft(object sender, EventArgs e)
        {
            if (__eNodeMouse_Click != null)
                __eNodeMouse_Click(this._cTree, new EventArgs());
        }
        /// <summary>
        /// Выполняется при выборе узла дерева правой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mTree_eMouseClickRight(object sender, EventArgs e)
        {
            if (__eNodeMouse_ClickRight != null)
                __eNodeMouse_ClickRight(this._cTree, new EventArgs());
        }
        /// <summary>
        /// Выполняется для перерисовки ячеек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void __cTree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            //https://coderoad.ru/35155193/%D0%A0%D0%B0%D1%81%D0%BA%D1%80%D0%B0%D1%81%D1%8C%D1%82%D0%B5-%D1%83%D0%B7%D0%B5%D0%BB-treeview-%D0%B4%D1%80%D1%83%D0%B3%D0%B8%D0%BC-%D1%86%D0%B2%D0%B5%D1%82%D0%BE%D0%BC
        }

        #endregion Дерево

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Загрузка данных из источника данных
        /// </summary>
        public void __mDataLoad(bool pRefreshData = false)
        {
            if (__eDataLoadBefore != null)
                __eDataLoadBefore(_cTree, new EventArgs());

            _cTree.__mDataLoad("", pRefreshData);

            if (__eDataLoadAfter != null)
                __eDataLoadAfter(_cTree, new EventArgs());
        }
        /// <summary>
        /// Загрузка дерева из [DataTable] и состояния развернутости дерева
        /// </summary>
        /// <example>
        ///        DataRow vDataRowTable = vDataTable.NewRow();
        ///        vDataRowTable["clu"] = vIdentifier;
        ///        vDataRowTable["dsi"] = vModelTable.__fName + "   " + vModelTable.__fDescription;
        ///        vDataRowTable["tag"] = "0, " + vModelTable.__fEssenceName;
        ///        vDataTable.Rows.Add(vDataRowTable);
        /// </example>
        /// <param name="pDataTable"></param>
        public void __mDataLoad(DataTable pDataTable)
        {
            if (__eDataLoadBefore != null)
                __eDataLoadBefore(_cTree, new EventArgs());

            _cTree.Nodes.Clear();

            elmUnitTreeNode vNode = new elmUnitTreeNode();
            _cTree.Font = new Font("Microsoft Sans Serif", 9F);
            foreach (DataRow vDataRow in pDataTable.Rows)
            {
                if (appTypeString.__mWordNumberComma(Convert.ToString(vDataRow["tag"]), 0) == "0")
                    vNode = _cTree.__mNodeNew(Convert.ToString(vDataRow["dsi"]) + new String(' ', 20), Convert.ToString(vDataRow["tag"]), new Font("Microsoft Sans Serif", 9F, FontStyle.Bold), _cTree.ForeColor, -1, -1);
                else
                    _cTree.__mNodeSupply(vNode, Convert.ToString(vDataRow["dsi"]) + new String(' ', 20), Convert.ToString(vDataRow["tag"]), new Font("Microsoft Sans Serif", 9F), _cTree.ForeColor, -1, -1);
            }

            if (__eDataLoadAfter != null)
                __eDataLoadAfter(_cTree, new EventArgs());
        }
        /// <summary>
        /// Сохранение текущего состояния развернутости дерева
        /// </summary>
        public void __mNodesStatusesSave()
        {
            _cTree.__mNodesStatusesSave();
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
                    vButton = _cButtonOperations;
                    break;
                case "_cButtonReports":
                    vButton = _cButtonReports;
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
                    vButton = _cButtonOperations;
                    break;
                case "_cButtonReports":
                    vButton = _cButtonReports;
                    break;
            }
            vButton.DropDownItems.Add(pMenuItem);

            return;
        }
        /// <summary>
        /// Добавление меню в кнопку управления
        /// </summary>
        /// <param name="pMenuItem"></param>
        public void __mButtonDropDownItemsAdd(string pButtonName, string pMenuItem)
        {
            elmComponentToolbarButtonMenu vButton = new elmComponentToolbarButtonMenu();
            switch (pButtonName)
            {
                case "_cButtonEdit":
                    vButton = _cButtonEdit;
                    break;
                case "_cButtonOperations":
                    vButton = _cButtonOperations;
                    break;
                case "_cButtonReports":
                    vButton = _cButtonReports;
                    break;
            }
            vButton.DropDownItems.Add(pMenuItem);

            return;
        }
        /// <summary>
        /// Обновление вида '_cTree'
        /// </summary>
        public void __mRefresh()
        {
            _cTree.Refresh();
        }
        /// <summary>
        /// Размернуть все узлы дерева
        /// </summary>
        public void __mExpandAll()
        {
            _cTree.ExpandAll();
        }
        /// <summary>
        /// Свернуть все узлы дерева
        /// </summary>
        public void __mCollapseAll()
        {
            _cTree.CollapseAll();
        }
        /// <summary>
        /// Установка / Снятие метки
        /// </summary>
        /// <param name="pTreeNode">Узел</param>
        /// <param name="pChecked">Состояние метки</param>
        public void __mNodeMark(elmUnitTreeNode pTreeNode, bool pChecked)
        {
            pTreeNode.Checked = pChecked;
            elmUnitTreeNode vTreeNode = pTreeNode;
            do
            {
                if (vTreeNode.Parent != null)
                {
                    vTreeNode = vTreeNode.Parent as elmUnitTreeNode;
                    //vTreeNode.Expand();
                }
            } while (vTreeNode.Parent != null);
        }
        /// <summary>
        /// Получение списка выбранных узлов
        /// </summary>
        public List<elmUnitTreeNode> __mNodeListMark()
        { 
            return _cTree.__mNodeListMark();
        }
        /// <summary>
        /// Получение узла дерева по ключу записи
        /// </summary>
        /// <param name="pClue">Ключ записи</param>
        public elmUnitTreeNode __mNodeGetByClueOnLoad(int pClue)
        {
            return _cTree.__mNodeGetByClueOnLoad(pClue);
        }
        public void __mFocus()
        {
            _cTree.Focus();
        }
        /// <summary>
        /// Добавление пункта в контекстное меню
        /// </summary>
        /// <param name="pMenuContextItem"></param>
        public void __mMenuContextItemAdd(elmComponentMenuItem pMenuContextItem)
        {
            _cTree.__mContextItemAdd(pMenuContextItem);
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Блокировка изменения документа
        /// </summary>
        public bool __fEditLock = false;
        /// <summary>
        /// Отметка, о том что форма была закрыта при нажатии на клавишу 'Выбрать'
        /// </summary>
        public bool __fIsSelected = false;

        #endregion Атрибуты

        #region - Компоненты

        /// <summary>
        /// Кнопка 'Обновить'
        /// </summary>
        protected elmComponentToolbarButton _cButtonRefresh = new elmComponentToolbarButton();
        /// <summary>
        /// Кнопка 'Правка'
        /// </summary>
        protected elmComponentToolbarButtonMenu _cButtonEdit = new elmComponentToolbarButtonMenu();

        #region Меню кнопки 'Правка'

        /// <summary>
        /// Пункт меню 'Копировать' кнопки 'Правка'
        /// </summary>
        protected elmComponentMenuItem _cButtonEditCopy = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'Изменить' кнопки 'Правка'
        /// </summary>
        protected elmComponentMenuItem _cButtonEditEdit = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'Создать' кнопки 'Правка'
        /// </summary>
        protected elmComponentMenuItem _cButtonEditCreate = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'Удалить' кнопки 'Правка'
        /// </summary>
        protected elmComponentMenuItem _cButtonEditRemove = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'Восстановить' кнопки 'Правка'
        /// </summary>
        protected elmComponentMenuItem _cButtonEditRestore = new elmComponentMenuItem();

        #endregion Меню кнопки 'Правка'

        /// <summary>
        /// Кнопка 'Операции'
        /// </summary>
        protected elmComponentToolbarButtonMenu _cButtonOperations = new elmComponentToolbarButtonMenu();

        #region Меню кнопки 'Операции'

        /// <summary>Кнопка 'Операции / Доступ'
        /// </summary>
        protected elmComponentMenuItem _cButtonOperationsAccess = new elmComponentMenuItem();

        #endregion Меню кнопки 'Операции'

        /// <summary>
        /// Кнопка 'Отчеты'
        /// </summary>
        protected elmComponentToolbarButtonMenu _cButtonReports = new elmComponentToolbarButtonMenu();

        #region Меню кнопки 'Отчеты'

        /// <summary>
        /// Пункт меню 'Текущий список' кнопки 'Отчеты'
        /// </summary>
        protected elmComponentMenuItem _cButtonReportsCurrentList = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'История записи' кнопки 'Отчеты'
        /// </summary>
        protected elmComponentMenuItem _cButtonReportsHistory = new elmComponentMenuItem();

        #endregion Менб кнопки 'Отчеты'

        /// <summary>
        /// Кнопка 'Выбрать'
        /// </summary>
        protected elmComponentToolbarButton _cButtonSelect = new elmComponentToolbarButton();

        /// <summary>
        /// Дерево
        /// </summary>
        protected elmComponentTree _cTree = new elmComponentTree();

        #endregion - Компоненты

        #region - Объекты

        /// <summary>
        /// Форма для формирования фильтра
        /// </summary>
        public Type __oFormFilter;
        /// <summary>
        /// Форма для редактирования записи
        /// </summary>
        public Type __oFormOpened;
        /// <summary>
        /// Тип формы для изменения данных
        /// </summary>
        public CONTROLsOPENEDTYPES __fFormOpenedType = CONTROLsOPENEDTYPES.FormRecord;

        /// <summary>
        /// Форма для определения прав пользователей
        /// </summary>
        public Type __oFormUsersAccess;

        #endregion Объекты

        #region - Служебные

        /// <summary>
        /// Отображение 'CheckBox'
        /// </summary>
        private bool fCheckBoxShow = false;

        #endregion Служебные

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Выбранный узел дерева
        /// </summary>
        public elmUnitTreeNode __fSelectedTreeNode_
        {
            get { return _cTree.SelectedNode as elmUnitTreeNode; }
        }
        /// <summary>
        /// Текущее значение 
        /// </summary>
        public int __fValue_
        {
            get { return __fSelectedTreeNode_.__fClue; }
        }
        public string __fValueText_
        {
            get { return _cTree.SelectedNode.Text.Trim(); }
        }

        #region Доступность кнопок управления

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
            get { return _cButtonOperations.Enabled; }
            set { _cButtonRefresh.Enabled = value; }
        }
        /// <summary>
        /// Доступность кнопки 'Отчеты'
        /// </summary>
        public bool __fButtonReportsEnabled_
        {
            get { return _cButtonOperations.Enabled; }
            set { _cButtonRefresh.Enabled = value; }
        }
        /// <summary>
        /// Доступность пункта меню 'Текущий список' кнопки 'Отчеты'
        /// </summary>
        public bool __fButtonReportsCurrentListEnabled_
        {
            get { return __fButtonReportsCurrentListEnabled_; }
            set { __fButtonReportsCurrentListEnabled_ = value; }
        }
        /// <summary>
        /// Доступность пункта меню 'История корректировок' кнопки 'Отчеты'
        /// </summary>
        public bool __fButtonReportsHistoryEnabled_
        {
            get { return __fButtonReportsHistoryEnabled_; }
            set { __fButtonReportsHistoryEnabled_ = value; }
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

        #endregion Доступность кнопок управления

        #region Видимость кнопок управления

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
            get { return _cButtonOperations.Visible; }
            set { _cButtonOperations.Visible = value; }
        }
        /// <summary>
        /// Видимость кнопки 'Отчеты'
        /// </summary>
        public bool __fButtonReportsVisible_
        {
            get { return _cButtonReports.Visible; }
            set { _cButtonReports.Visible = value; }
        }
        /// <summary>
        /// Видимость пункта меню 'История изменений' кнопки 'Отчеты'
        /// </summary>
        public bool __fButtonReportsHistoryVisible_
        {
            get { return _cButtonReportsHistory.Visible; }
            set { _cButtonReportsHistory.Visible = value; }
        }
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
            set { _cButtonOperations.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Подсказка к кнопке 'Отчеты' переведенная на язык пользователя
        /// </summary>
        public string __fButtonReportsToolTipText
        {
            set { _cButtonReports.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
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
            set { _cButtonOperations.Image = value; }
        }
        /// <summary>
        /// Изображение на кнопке 'Отчеты'
        /// </summary>
        public Image __fButtonReportsImage
        {
            set { _cButtonReports.Image = value; }
        }

        #endregion Изображения на кнопках

        /// <summary>
        /// Сущность данных
        /// </summary>
        public datUnitEssence __oEssence_
        {
            get { return _cTree.__oEssence; }
            set { _cTree.__oEssence = value; }
        }
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int __fRecordClue_
        {
            get { return _cTree.__fRecordClue_; }
        }
        /// <summary>
        /// Отображение 'CheckBox'
        /// </summary>
        public bool __fCheckBoxShow_
        {
            get { return fCheckBoxShow; }
            set 
            { 
                fCheckBoxShow = value; 
                if(fCheckBoxShow == true)
                    _cTree.CheckBoxes = true;
                else
                    _cTree.CheckBoxes = false;
            }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при выборе кнопки 'Обновить' левой клавишей мыши 
        /// </summary>
        public event EventHandler __eButtonRefresh_Click;
        /// <summary>
        /// Возникает при выборе кнопки 'Обновить' правой клавишей мыши 
        /// </summary>
        public event EventHandler __eButtonRefresh_ClickRight;
        /// <summary>
        /// Возникает при выборе кнопки 'Правка / Создать' левой клавишей мыши 
        /// </summary>
        public event EventHandler __eButtonEditCreate_Click;
        /// <summary>
        /// Возникает при выборе кнопки 'Правка / Изменить' левой клавишей мыши 
        /// </summary>
        public event EventHandler __eButtonEditEdit_Click;

        public event EventHandler __eButtonEditRemove_Click;

        /// <summary>
        /// Возникает при выборе кнопки 'Выбрать запись' левой клавишей мыши 
        /// </summary>
        public event EventHandler __eButtonSelect_Click;
        /// <summary>
        /// Возникает при выборе кнопки 'Выбрать запись' правой клавишей мыши 
        /// </summary>
        public event EventHandler __eButtonSelect_ClickRight;
        /// <summary>
        /// Возникает при выборе узла дерева левой кнопкой мыши
        /// </summary>
        public event EventHandler __eNodeMouse_Click;
        /// <summary>
        /// Возникает при выборе узла дерева правой кнопкой мыши
        /// </summary>
        public event EventHandler __eNodeMouse_ClickRight;
        /// <summary>
        /// Возникает при двойном клике мыши по узлу дерева
        /// </summary>
        public event EventHandler __eNodeMouse_Double;
        /// <summary>
        /// Возникает после загрузки данных
        /// </summary>
        public event EventHandler __eDataLoadAfter;
        /// <summary>
        /// Возникает перед загрузкой данных
        /// </summary>
        public event EventHandler __eDataLoadBefore;

        #endregion СОБЫТИЯ
    }
}
