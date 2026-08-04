using nlApplication;
using nlData;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmAreaRecord.cs
    /// </summary>
    /// <remarks>Класс-область для измерения записи данных</remarks>
    public class elmAreaRecord : elmArea
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public elmAreaRecord()
        {
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        public elmAreaRecord(bool pLoadOnCreate)
        {
            __fLoadOnCreate = pLoadOnCreate;
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Размещение компонентов

            Panel2.Controls.Add(_cBlockInputs);
            Panel2.Controls.SetChildIndex(_cBlockInputs, 0);
            _cToolBar.Items.Insert(0, _cButtonSave);
            _cToolBar.Items.Add(_cButtonReports);
            _cToolBar.Items.Add(_cButtonOperations);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            // _cButtonSave
            {
                _cButtonSave.Click += _cButtonSave_Click;
                _cButtonSave.Image = global::nlResourcesImages.Properties.Resources._Computer_Diskette_b32;
                _cButtonSave.ToolTipText = "[ Ctrl + A ]\n" + elmApplication.__oTunes.__mTranslate("Применить");
            }
            // __cButtonReports
            {
                _cButtonReports.Alignment = ToolStripItemAlignment.Right;
                //_cButtonReports.DropDownOpened += cButton_DropDownOpened;
                _cButtonReports.Image = global::nlResourcesImages.Properties.Resources._PagePrinter_y32;
                _cButtonReports.ToolTipText = "[ Ctrl + R ] " + elmApplication.__oTunes.__mTranslate("Отчеты");
                _cButtonReports.Visible = false;
            }
            // __cButtonOperations
            {
                _cButtonOperations.Alignment = ToolStripItemAlignment.Right;
                //_cButtonOperations.DropDownOpened += cButton_DropDownOpened;
                _cButtonOperations.Image = global::nlResourcesImages.Properties.Resources._PageGear_y32;
                _cButtonOperations.ToolTipText = "[ Ctrl + O ] " + elmApplication.__oTunes.__mTranslate("Операции");
                _cButtonOperations.Visible = false;
            }
            // _cBlockInputs
            {
                _cBlockInputs.Dock = DockStyle.Fill;
                _cBlockInputs.AutoScroll = true;
            }

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }
        /// <summary>
        /// Выполняется после создания объекта
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (__fLoadOnCreate == true)
                __mDataLoad();
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        #region Кнопки управления

        /// <summary>
        /// Выполняется при выборе кнопки 'Сохранить'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cButtonSave_Click(object sender, EventArgs e)
        {
            if (__mDataSave() == true) /// Закрытие формы при удачном сохранении
            {
                if(__fFormCloseAfterSaving == true)
                    (FindForm() as Form).Close();
            }
        }

        #endregion Кнопки управления

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Получение данных из источника данных
        /// </summary>
        /// <returns>[true] - данные получены, иначе - [false]</returns>
        public bool __mDataLoad()
        {
            bool vReturn = true; // Возвращаемое значение
            /// Запрос данных
            DataTable vDataTable = __oEssence.__mRecord(__fRecordClue);
            /// Если идентификатор записи не указан, создается новая запись
            if (__fRecordClue < 0)
            {
                DataRow vDataRow = __oEssence.__mRecordNew(vDataTable);
                /// Если идентификатор записи для копирования указан, читаются данные из этой записи и переписываются в новую
                if (__fRecordClueForCopy > 0)
                {
                    DataTable vDataTableCopy = __oEssence.__mRecord(__fRecordClueForCopy);
                    foreach (DataRow vDataRowCopy in vDataTableCopy.Rows)
                    {
                        foreach (DataColumn vDataColumnCopy in vDataTableCopy.Columns)
                        {
                            if (!(vDataColumnCopy.ColumnName.StartsWith("CLU")
                                | vDataColumnCopy.ColumnName.StartsWith("cod")
                                | vDataColumnCopy.ColumnName.StartsWith("GID")))
                            {
                                vDataRow[vDataColumnCopy.ColumnName] = vDataRowCopy[vDataColumnCopy.ColumnName];
                            }
                        }
                    }
                }
                /// Поиск в файле 'forms.tun' значений по умолчанию 
                foreach (DataColumn vDataColumn in vDataTable.Columns)
                {
                    string vColumnName = vDataColumn.ColumnName;
                    elmForm vForm = FindForm() as elmForm;
                    appFileIni vFileIni = new appFileIni(elmApplication.__oPathes.__mFileFormTunes());
                    string vValueNew = vFileIni.__mValueRead(vForm.Name.ToUpper(), "Field_" + vColumnName.Trim());
                    if (vValueNew.Trim().Length > 0)
                    {
                        try
                        {
                            vDataRow[vColumnName] = vValueNew;
                        }
                        catch { }
                    }
                }

                vDataTable.Rows.Add(vDataRow);
            }
            // Редактирование записи
            if (vDataTable.Rows.Count == 1)
            {
                _cBlockInputs.__oDataTable = vDataTable;
                _cBlockInputs.__mDataLoad();
                vReturn = true;
            }
            else
                vReturn = false;

            if (__eOnDataLoaded != null)
                __eOnDataLoaded(this, new EventArgs()); /// Формируется событие 'Возникает после загрузки данных'

            return vReturn;
        }
        /// <summary>
        /// Сохранение данных в источнике данных
        /// </summary>
        /// <returns>[true] - Данные сохранены, иначе - [false]</returns>
        public bool __mDataSave()
        {
            if (__fLoadOnCreate == false)
            { /// Данные не сохраняются, сохранение имитируется
                __fRecordSaved = true;
                return true;
            }
            _cBlockInputs.__mDataSave();

            bool vReturn = __oEssence.__mUpdate(_cBlockInputs.__oDataTable); /// Сохранение данных
            if (elmApplication.__oData.__mDataSourceGet(__oEssence.__fDataSourceAlias).__fDateTimeStore == DATETIMESTORE.DateTime)
            {
                if (vReturn == true & Convert.ToDateTime(_cBlockInputs.__oDataTable.Rows[0][0]) != appTypeDateTime.__mMsSqlDateEmpty())
                    __fRecordClue = datApplication.__oData.__mDataSourceGet(__oEssence.__fDataSourceAlias).__mClueLastInserted(__oEssence.__fTableName); /// Получение идентификатора вставленной записи
            }
            else
            {
                if (vReturn == true & Convert.ToInt64(_cBlockInputs.__oDataTable.Rows[0][0]) != 0)
                    __fRecordClue = datApplication.__oData.__mDataSourceGet(__oEssence.__fDataSourceAlias).__mClueLastInserted(__oEssence.__fTableName); /// Получение идентификатора вставленной записи
            }
            /// Исправление в таблице блокировок идентификатора записи заблокированной записи равного 0 на фактический идентификатор 
            datApplication.__oData.__mDataSourceGet(__oEssence.__fDataSourceAlias).__mLockLnkRidChange(__oEssence.__fLockClue, __fRecordClue);

            if (__eOnDataSaving != null)
                __eOnDataSaving(this, new EventArgs()); /// Формируется событие 'Возникает перед сохранением данных'

            return vReturn;
        }
        /// <summary>
        /// Добавление поля ввода на панель полей ввода
        /// </summary>
        /// <param name="pInput"></param>
        public bool __mInputAdd(elmInput pInput, int pHeight = 25)
        {
            return _cBlockInputs.__mInputAdd(pInput, pHeight);
        }
        /// <summary>
        /// Добавление блока вкладок на панель поля ввода
        /// </summary>
        /// <param name="pPageBlock"></param>
        public void __mPageBlockAdd(elmComponentPagesBlock pPageBlock, AnchorStyles pAnchorStyles = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right)
        {
            _cBlockInputs.__mPageBlockAdd(pPageBlock, pAnchorStyles);
        }
        /// <summary>
        /// Добавление поля ввода в блок для отображения полей ввода
        /// </summary>
        /// <param name="pInput"></param>
        /// <returns></returns>
        public bool __mBlockInputsAdd(elmInput pInput)
        {
            return _cBlockInputs.__mInputAdd(pInput);
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Сохранить'
        /// </summary>
        public void __mPressButtonSave()
        {
            _cButtonSave.PerformClick();
        }
        /// <summary>
        /// Очистка выпадающего меню кнопки управления
        /// </summary>
        public void __mButtonDropDownItemsClear(string pButtonName)
        {
            elmComponentToolbarButtonMenu vButton = new elmComponentToolbarButtonMenu();

            switch (pButtonName)
            {
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

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Разрешение закрывать форму после сохранения
        /// </summary>
        public bool __fFormCloseAfterSaving = true; // Не менять. Менять там где не надо
        /// <summary>
        /// Идентификатор редактируемой записи
        /// </summary>
        public int __fRecordClue = -1;
        /// <summary>
        /// Идентификатор родительской записи.
        /// </summary>
        /// <remarks>Для создания новой записи для 'elmFormTree'</remarks>
        public int __fRecordClueParent = -1;
        /// <summary>
        /// Идентификатор записи используемой для копирования
        /// </summary>
        public int __fRecordClueForCopy = 0;
        /// <summary>
        /// Запись должна быть сохранена при нажатии на кнопку сохранена
        /// </summary>
        public bool __fLoadOnCreate = true;
        /// <summary>
        /// Признак, что данные были сохранены
        /// </summary>
        public bool __fRecordSaved = false;
        /// <summary>
        /// Результат выполнения внешней транзакции
        /// </summary>
        public bool __fTransactionExternal = true;

        #endregion Атрибуты

        #region - Компоненты

        /// <summary>
        /// Блок для отображения полей ввода
        /// </summary>
        protected elmBlockInputs _cBlockInputs = new elmBlockInputs();
        /// <summary>
        /// Кнопка 'Сохранить'
        /// </summary>
        protected elmComponentToolbarButton _cButtonSave = new elmComponentToolbarButton();
        /// <summary>
        /// Кнопка 'Операции'
        /// </summary>
        protected elmComponentToolbarButtonMenu _cButtonOperations = new elmComponentToolbarButtonMenu();
        /// <summary>
        /// Кнопка 'Отчеты'
        /// </summary>
        protected elmComponentToolbarButtonMenu _cButtonReports = new elmComponentToolbarButtonMenu();

        #endregion - Компоненты

        #region - Объекты

        /// <summary>Сущность редактируемых данных
        /// </summary>
        public datUnitEssence __oEssence;

        #endregion - Объекты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        #region Доступность кнопок управления

        /// <summary>
        /// Доступность кнопки 'Сохранить'
        /// </summary>
        public bool __fButtonSaveEnabled_
        {
            get { return _cButtonSave.Enabled; }
            set { _cButtonSave.Enabled = value; }
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
        /// Доступность кнопки 'Отчеты'
        /// </summary>
        public bool __fButtonReportsEnabled_
        {
            get { return _cButtonReports.Enabled; }
            set { _cButtonReports.Enabled = value; }
        }

        #endregion Доступность кнопок управления

        #region Видимость кнопок управления

        /// <summary>
        /// Видимость кнопки 'Сохранить'
        /// </summary>
        public bool __fButtonSaveVisible_
        {
            get { return _cButtonSave.Visible; }
            set { _cButtonSave.Visible = value; }
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

        #endregion Видимость кнопок управления

        #region Подсказки к кнопкам

        /// <summary>
        /// Подсказка к кнопке 'Сохранить' переведенная на язык пользователя
        /// </summary>
        public string __fButtonSaveToolTipText
        {
            set { _cButtonSave.ToolTipText = elmApplication.__oTunes.__mTranslate(value); }
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
        /// Изображение на кнопке 'Сохранить'
        /// </summary>
        public Image __fButtonSaveImage
        {
            set { _cButtonSave.Image = value; }
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
        /// Разрешение отображения галочки во всех добавляемых полях ввода
        /// </summary>
        public bool __fBlockInputsCheckShow_
        {
            get { return _cBlockInputs.__fMarkShow; }
            set { _cBlockInputs.__fMarkShow = value; }
        }
        /// <summary>
        /// Получение списка полей ввода в блоке для отображения полей ввода
        /// </summary>
        public ControlCollection __fBlockInputsControls_
        {
            get { return _cBlockInputs.Controls; }

        }
        /// <summary>
        /// Отклонение от верхнего края последнего компонента
        /// </summary>
        public int __fBlockInputsTopCoordinate_
        {
            get { return _cBlockInputs.__fTopCoordinate; }
            set { _cBlockInputs.__fTopCoordinate = value; }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>Возникает после загрузки данных
        /// </summary>
        public event EventHandler __eOnDataLoaded;
        /// <summary>Возникает после сохранения данных, но до закрытия транзакции
        /// </summary>
        public event EventHandler __eOnDataSaving;

        #endregion = СОБЫТИЯ
    }
}
