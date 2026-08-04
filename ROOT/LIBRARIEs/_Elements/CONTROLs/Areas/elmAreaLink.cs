using nlApplication;
using nlData;
using nlReportHtml;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmAreaLink.cs
    /// </summary>
    /// <remarks>Класс-область для работы со связующими данными</remarks>
    /// <example>
    /* // _cAreaGrid
            {
                __cAreaLink.Dock = DockStyle.Fill;
                __cAreaLink.__fFieldDesignationName = "dsiUsrRol";
                __cAreaLink.__oEssence_ = new rtlEssenceUsrUsrRol();
                __cAreaLink.__oFormLinkedData = typeof(admFormGridUsrRol);
                __cAreaLink.__fFormOpenedType = CONTROLsOPENEDTYPES.FormGrid;

                #region Сетка / Определение колонок

                __cAreaLink.__mColumnAdd(elmApplication.__oTunes.__mTranslate("Ключ")
                        , elmApplication.__oTunes.__mTranslate("Указатель записи в таблице") + "."
                        , "CLU"
                        , true
                        , false
                        , "DataGridViewTextBoxColumn");
                __cAreaLink.__mColumnAdd(elmApplication.__oTunes.__mTranslate("Правка")
                        , elmApplication.__oTunes.__mTranslate("Время последнего изменения записи") + "."
                        , "CHG"
                        , true
                        , false
                        , "DataGridViewTextBoxColumn");
                __cAreaLink.__mColumnAdd(elmApplication.__oTunes.__mTranslate("Запись удалена")
                        , elmApplication.__oTunes.__mTranslate("Метка об удалении записи") + "."
                        , "ELD"
                        , true
                        , false
                        , "DataGridViewCheckBoxColumn");
                __cAreaLink.__mColumnAdd(elmApplication.__oTunes.__mTranslate("Идентификатор")
                        , elmApplication.__oTunes.__mTranslate("Уникальный указатель записи данных") + "."
                        , "GID"
                        , true
                        , false
                        , "DataGridViewTextBoxColumn");
                __cAreaLink.__mColumnAdd(elmApplication.__oTunes.__mTranslate("Код")
                        , ""
                        , "codUsrRol"
                        , true
                        , false
                        , "DataGridViewTextBoxColumn");
                __cAreaLink.__mColumnAdd(elmApplication.__oTunes.__mTranslate("Роль")
                        , elmApplication.__oTunes.__mTranslate("Роль пользователей") + "."
                        , "dsiUsrRol"
                        , true
                        , true
                        , "DataGridViewTextBoxColumn");

                __cAreaLink.__mGridBuild();

                #endregion Сетка / Определение колонок
            } */
    /// </example>
    public class elmAreaLink : elmArea
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

            _cToolBar.Items.Add(_cButtonEdit);

            _cButtonEdit.DropDownItems.Add(_cButtonEditAdd);
            _cButtonEdit.DropDownItems.Add(_cButtonEditExclude);

            Panel2.Controls.Add(_cSplitterFilterGrid);
            Panel2.Controls.SetChildIndex(_cSplitterFilterGrid, 0);

            _cSplitterFilterGrid.Panel1.Controls.Add(_cLabelFilterCaption);
            _cSplitterFilterGrid.Panel1.Controls.Add(_cLabelFilterExpression);

            _cSplitterFilterGrid.Panel2.Controls.Add(_cGrid);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            // __cButtonEdit
            {
                _cButtonEdit.Alignment = ToolStripItemAlignment.Right;
                _cButtonEdit.DropDownOpened += mButtonDropDownOpened;
                _cButtonEdit.Image = global::nlResourcesImages.Properties.Resources._Page_b32;
                _cButtonEdit.ToolTipText = "[ Ctrl + E ] " + elmApplication.__oTunes.__mTranslate("Правка");
                {
                    _cButtonEditAdd.Click += mButtonEditAddClick;
                    _cButtonEditAdd.Image = global::nlResourcesImages.Properties.Resources._Page_b16;
                    _cButtonEditAdd.__fCaption_ = "Добавить";

                    _cButtonEditExclude.Click += mButtonEditRemove_Click;
                    _cButtonEditExclude.Image = global::nlResourcesImages.Properties.Resources._PageDelete_b16;
                    _cButtonEditExclude.__fCaption_ = "Исключить";
                }
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
            }

            #endregion Настройка компонентов

            ResumeLayout();

            return;
        }
        
        #endregion Объект

        #region - Поведение

        #region Кнопки управления

        #region ! Внешние нажатия на кнопки управления

        /// <summary>
        /// Выполняется при выборе кнопки 'Правка'
        /// </summary>
        public void __mPressButtonEdit()
        {
            _cButtonEdit.ShowDropDown();

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
        /// <summary>
        /// Выполняется при выборе кнопки 'Правка / Создать' левой кнопкой мыши 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonEditAddClick(object sender, EventArgs e)
        {
            if (__eButtonEditAddClick != null)
                __eButtonEditAddClick(sender, e);

            int vSlaveKey = 0; // Ключ связанного, выбранного справочника
            string vSlaveDesignation = ""; // Название связанного выбранного справочника

            elmForm vForm = FindForm() as elmForm;
            if (vForm != null & __oFormLinkedData != null)
            {
                /// Вызов формы редактирования документа
                if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormGrid)
                {
                    elmFormGrid vFormGrid = (elmFormGrid)Activator.CreateInstance(__oFormLinkedData);
                    (vFormGrid as elmFormGrid).ShowDialog();
                    if(vFormGrid.__cAreaGrid.__fIsSelected == true)
                        vSlaveKey = vFormGrid.__cAreaGrid.__fRecordClue_;
                }
                /// Вызов формы редактирования записи
                if (__fFormOpenedType == CONTROLsOPENEDTYPES.FormTree)
                {
                    elmFormTree vFormTree = (elmFormTree)Activator.CreateInstance(__oFormLinkedData);
                    (vFormTree as elmFormTree).ShowDialog();
                    if (vFormTree.__cAreaTree.__fIsSelected == true)
                        vSlaveKey = vFormTree.__cAreaTree.__fRecordClue_;
                }
                /// Проверка наличия выбранной записи
                if(vSlaveKey <= 0)
                    return;
                
                vSlaveDesignation = elmApplication.__oData.__mNameByClue(__fLinkedTable, vSlaveKey, _cGrid.__oEssence.__fDataSourceAlias);
                /// Добавление новой записи
                DataRow vDataRowNew = _cGrid.__oEssence.__mRecordNew(_cGrid.DataSource as DataTable);
                vDataRowNew[__fParentKeyFieldName] = __fParentKey; 
                vDataRowNew[__fLinkedKeyFieldName] = vSlaveKey;
                vDataRowNew[__fLinkedDesignationFieldName] = vSlaveDesignation;

                _cLabelFilterExpression.Text = vSlaveDesignation;

                (_cGrid.DataSource as DataTable).Rows.Add(vDataRowNew);
                /// Если данные сохранены запись добавляется в сетку
                if (__mDataSave() == false)
                    vDataRowNew.Delete();
                _cGrid.Refresh();
                _cGrid.Update();
            }
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Правка / Удалить' левой кнопкой мыши 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonEditRemove_Click(object sender, EventArgs e)
        {
            __mSortingLoad();

            if (_cGrid.__fRecordClue_ == 0)
            {
                elmApplication.__oMessages.__mShow(MESSAGESTYPES.Warning, "Запись с нулевым кодом не исключается!");
                goto Exit;
            }
            if (elmApplication.__oMessages.__mShow(MESSAGESTYPES.Question, "Исключить запись с кодом связанные данные") == DialogResult.Yes)
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
                vReport.__fTitle = elmApplication.__oTunes.__mTranslate("Список '{0}'", (FindForm() as elmFormGrid).Text);
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
            vReport.__mCell(elmApplication.__oTunes.__mTranslate("История записи '{0}' id={1}", (FindForm() as elmFormGrid).Text, _cGrid.__fRecordClue_), "SC=Max");
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
                            vReport.__mCell(elmApplication.__oTunes.__mTranslate("Удалено"), "CL=HeaderCell");
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
        /// Выполняется при выборе кнопки 'Выбрать' левой кнопкой мыши 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonSave_Click(object sender, EventArgs e)
        {
            __mDataSave();
            (FindForm() as elmForm).Close();
            if (__eButtonSelectClick != null)
                __eButtonSelectClick(this, new EventArgs());
        }

        #endregion Кнопки управления

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
        public bool __mColumnAdd(string pCaption, string pPrompt, string pFieldName, bool pReadOnly, bool pVisible, DATAGRIDCOLUMNTYPE pType)
        {
            return _cGrid.__mColumnAdd(pCaption, pPrompt, pFieldName, pReadOnly, pVisible, pType);
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
            string vFilterExpression = __fParentKeyFieldName + " = " +  __fParentKey.ToString(); // Условия фильтра формы
            int vCurrentRowIndex = -1; // Индекс строки выбранной в текущий момент
            /// Определение индекса текущей строки
            if (_cGrid.CurrentRow != null)
                vCurrentRowIndex = _cGrid.CurrentRow.Index;

            /// Загрузка данных из источника данных
            vReturn = _cGrid.__mDataLoad(vFilterExpression + " and " + __oEssence_.__fTableAlias + ".ELD = 0", "");
            _fFilterExpression = vFilterExpression;
            _cGrid.__mSortingLoad();

            #region /// Перевод курсора на строку выбранную до загрузки

            if (vCurrentRowIndex >= 0 & _cGrid.Rows.Count > 0)
            {
                if (_cGrid.Rows.Count < vCurrentRowIndex + 1)
                    vCurrentRowIndex = _cGrid.Rows.Count - 1;

                _cGrid.CurrentCell = _cGrid.Rows[vCurrentRowIndex].Cells[__fLinkedDesignationFieldName];
            }

            /// Исправление логических значений
            vFilterExpression = vFilterExpression.Replace("False", "0");
            vFilterExpression = vFilterExpression.Replace("True", "1");

            #endregion Перевод курсора на строку выбранную до загрузки

            if (__eDataLoadAfter != null)
                __eDataLoadAfter(_cGrid, new EventArgs());

            _cGrid.Refresh();

            return vReturn;
        }
        /// <summary>
        /// Сохранение данных
        /// </summary>
        public bool __mDataSave()
        {
            return __oEssence_.__mUpdate(_cGrid.DataSource as DataTable);
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

                    _cToolStripMenuItemColumn.Checked = Convert.ToBoolean((FindForm() as elmForm).__oFileIni.__mValueReadWrite(vColumn.__fVisible.ToString(), (FindForm() as elmForm).Name, "Field_" + vColumn.__fField)); // Загрузка состояния видимости поля
                    _cToolStripMenuItemColumn.CheckedChanged += mFieldsVisibleCheckedChanged;
                    _cToolStripMenuItemColumn.CheckOnClick = true;
                    _cToolStripMenuItemColumn.Font = elmApplication.__oInterface.__mFont(FONTS.Text);
                    _cToolStripMenuItemColumn.ImageScaling = ToolStripItemImageScaling.None;
                    _cToolStripMenuItemColumn.Name = vColumn.__fField;
                    _cToolStripMenuItemColumn.Text = vColumn.__fCaption;

                    #endregion Меню - видимость колонок
                }
            }
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
            string vFormParentName = (FindForm() as elmForm).Name; // Название формы на которой расположен компонент
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
            string vFormParentName = (FindForm() as elmForm).Name; // Название формы на которой расположен компонент
            appFileIni vFileIni = (FindForm() as elmForm).__oFileIni; // Объект для работы с инициализационными файлами
            vFileIni.__fFilePath = elmApplication.__oPathes.__mFileFormTunes(); // Указание настроечного файла

            #region Сохранение видимости полей

            foreach (DataGridViewColumn vDataGridColumn in _cGrid.Columns)
            {
                vFileIni.__mValueWrite(vDataGridColumn.Visible.ToString(), vFormParentName.ToUpper(), "Field_" + vDataGridColumn.Name);
            }

            #endregion Сохранение видимости полей
        }

        public void __mFilterMessageShow(string pMessage)
        {
            _cLabelFilterCaption.Text = _cLabelFilterCaption.Text + " " +pMessage;    
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
        /// Название поля 'Название'
        /// </summary>
        public string __fLinkedDesignationFieldName = "";
        /// <summary>
        /// Название поля связанного идентификатора
        /// </summary>
        public string __fLinkedKeyFieldName = "";

        /// <summary>
        /// Название поля главного идентификатора
        /// </summary>
        public string __fParentKeyFieldName = "";
        /// <summary>
        /// Ключ главного идентификатора, к которому добавляюся связанные идентификаторы
        /// </summary>
        public int __fParentKey = -1;
        /// <summary>
        /// Название связанной таблицы
        /// </summary>
        public string __fLinkedTable = "";

        #endregion Атрибуты

        #region - Компоненты

        /// <summary>
        /// Кнопка 'Правка'
        /// </summary>
        protected elmComponentToolbarButtonMenu _cButtonEdit = new elmComponentToolbarButtonMenu();

        #region Меню кнопки 'Правка'

        /// <summary>
        /// Кнопка 'Правка / Добавить'
        /// </summary>
        protected elmComponentMenuItem _cButtonEditAdd = new elmComponentMenuItem();
        /// <summary>
        /// Кнопка 'Правка / Исключить'
        /// </summary>
        protected elmComponentMenuItem _cButtonEditExclude = new elmComponentMenuItem();

        #endregion Меню кнопки 'Правка'

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
        /// Тип формы для изменения данных
        /// </summary>
        public Type __oFormLinkedData;

        #endregion Объекты

        protected string _fFilterExpression = "";

        #endregion ПОЛЯ

        #region = СВОЙСТВА

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
        /// Доступность пункта меню 'Создать' кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditAddEnabled_
        {
            get { return _cButtonEditAdd.Enabled; }
            set { _cButtonEditAdd.Enabled = value; }
        }
        /// <summary>
        /// Доступность пункта меню 'Исключить' кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditExcludeEnabled_
        {
            get { return _cButtonEditExclude.Enabled; }
            set { _cButtonEditExclude.Enabled = value; }
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
        /// Видимость пункта меню 'Добавить' кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditAddVisible_
        {
            get { return _cButtonEditAdd.Visible; }
            set { _cButtonEditAdd.Visible = value; }
        }
        /// <summary>
        /// Видимость пункта меню 'Исключить' кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditExcludeVisible_
        {
            get { return _cButtonEditExclude.Visible; }
            set { _cButtonEditExclude.Visible = value; }
        }

        #endregion Видимость кнопок управления

        #region Надписи на кнопках 

        /// <summary>
        /// Надпись на кнопке 'Правка / Создать' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditAddCaption
        {
            set { _cButtonEditAdd.__fCaption_ = value; }
        }
        /// <summary>
        /// Надпись на кнопке 'Правка / Удалить' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditRemoveCaption
        {
            set { _cButtonEditExclude.__fCaption_ = value; }
        }

        #endregion Надписи на кнопках

        #region Подсказки к кнопкам

        /// <summary>
        /// Подсказка к кнопке 'Правка' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditToolTipText
        {
            set { _cButtonEdit.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Подсказка к кнопке 'Правка / Создать' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditAddToolTipText
        {
            set { _cButtonEditAdd.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }
        /// <summary>
        /// Подсказка к кнопке 'Правка / Удалить' переведенная на язык пользователя
        /// </summary>
        public string __fButtonEditRemoveToolTipText
        {
            set { _cButtonEditExclude.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
        }

        #endregion Подсказки к кнопкам

        #region Изображения на кнопках
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

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при выборе строки сетки
        /// </summary>
        public event EventHandler __eRowChanged;
        /// <summary>
        /// Возникает при выборе пункта меню 'Копировать' кнопки 'Правка'
        /// </summary>
        //public event EventHandler __eButtonEditCopyClick;
        /// <summary>
        /// Возникает при выборе пункта меню 'Добавить' кнопки 'Правка'
        /// </summary>
        public event EventHandler __eButtonEditAddClick;
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
        /// Возникает после загрузки данных
        /// </summary>
        public event EventHandler __eDataLoadAfter;
        /// <summary>
        /// Возникает перед загрузкой данных
        /// </summary>
        public event EventHandler __eDataLoadBefore;

        public event EventHandler __eButtonSelectClick;

        #endregion = СОБЫТИЯ
    }
}
