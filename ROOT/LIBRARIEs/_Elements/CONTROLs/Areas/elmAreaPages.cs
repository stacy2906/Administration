using nlData;
using System.Data;
using System.Windows.Forms;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmAreaDocument.cs
    /// </summary>
    /// <remarks>Класс-область для правки документа</remarks>
    public class elmAreaPages : elmArea
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

            _cToolBar.Items.Insert(0, _cButtonSave);
            //__cToolBar.Items.Add(_cButtonEdit);
            //{
            //    _cButtonEdit.DropDownItems.Add(_cButtonEditNew);
            //    _cButtonEdit.DropDownItems.Add(_cButtonEditCopy);
            //    _cButtonEdit.DropDownItems.Add(_cButtonEditEdit);
            //    _cButtonEdit.DropDownItems.Add(_cButtonEditRemove);
            //}

            Panel2.Controls.Add(_cSplitterCaptionContent);
            Panel2.Controls.SetChildIndex(_cSplitterCaptionContent, 0);
            _cSplitterCaptionContent.Panel1.Controls.Add(_cSplitterCaptionLeftRight);
            {
                _cSplitterCaptionLeftRight.Panel1.Controls.Add(_cBlockInputsLeft);
                _cSplitterCaptionLeftRight.Panel2.Controls.Add(_cBlockInputsRight);
            }
            _cSplitterCaptionContent.Panel2.Controls.Add(_cPageBlock);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            Dock = DockStyle.Fill;
            Orientation = Orientation.Horizontal;

            // __cToolBar
            {
                //// _cButtonEdit
                //{
                //    _cButtonEdit.Alignment = ToolStripItemAlignment.Right;
                //    //_cButtonEdit.Click 
                //    //_cButtonEdit._eMouseClickRight 
                //    _cButtonEdit.Image = global::nlResourcesImages.Properties.Resources._PageEdit_y32;
                //    _cButtonEdit.ToolTipText = "[ Ctrl + E ] " + elmApplication.__oTunes.__mTranslate("Правка");
                //    {
                //        _cButtonEditNew.Click += mButtonEditNew_Click;
                //        _cButtonEditNew.Image = global::nlResourcesImages.Properties.Resources._Page_w16C;
                //        _cButtonEditNew.__fCaption_ = "Создать";

                //        _cButtonEditCopy.Click += mButtonEditCopy_Click;
                //        _cButtonEditCopy.Image = global::nlResourcesImages.Properties.Resources._PageCopy_w16;
                //        _cButtonEditCopy.__fCaption_ = "Копировать";

                //        _cButtonEditEdit.Click += mButtonEditEdit_Click;
                //        _cButtonEditEdit.Image = global::nlResourcesImages.Properties.Resources._PageEdit_w16;
                //        _cButtonEditEdit.__fCaption_ = "Изменить";

                //        _cButtonEditRemove.Click += mButtonEditRemove_Click;
                //        _cButtonEditRemove.Image = global::nlResourcesImages.Properties.Resources._PageRemove_w16;
                //        _cButtonEditRemove.__fCaption_ = "Удалить";
                //    }
                //}
                // _cButtonSave
                {
                    _cButtonSave.__eClickLeft += mButtonSave_ClickLeft;
                    _cButtonSave.__eClickRight += mButtonSave_ClickRight;
                    _cButtonSave.Image = global::nlResourcesImages.Properties.Resources._Computer_Diskette_b32;
                    _cButtonSave.ToolTipText = "[ Ctrl + A ] " + elmApplication.__oTunes.__mTranslate("Сохранить");
                }
            }
            // _cSplitterCaptionContent
            {
                _cSplitterCaptionContent.Dock = DockStyle.Fill;
                _cSplitterCaptionContent.Orientation = Orientation.Horizontal;

                // _cSplitterCaptionContent.Panel1
                {
                    // _cSplitterCaptionLeftRight
                    {
                        _cSplitterCaptionLeftRight.Orientation = Orientation.Vertical;
                        // _cSplitterCaptionLeftRight.Panel1
                        {
                            // _cBlockInputsLeft
                            {
                                _cBlockInputsLeft.Dock = DockStyle.Fill;
                                _cBlockInputsLeft.__fMarkShow = false;
                            }
                            // _cBlockInputsRight
                            {
                                _cBlockInputsRight.Dock = DockStyle.Fill;
                                _cBlockInputsRight.__fMarkShow = false;
                            }
                        }
                        // _cSplitterCaptionLeftRight.Panel2
                        {
                        }
                    }
                }
                // _cSplitterCaptionContent.Panel2
                {
                    _cPageBlock.Dock = DockStyle.Fill;
                    _cPageBlock.SelectedIndexChanged += _cPageBlock_SelectedIndexChanged;
                }
            }

            #endregion Настройка компонентов

            ResumeLayout();

            return;
        }
        /// <summary>
        /// Выполняется при первом отображении компонента
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();
            _cSplitterCaptionLeftRight.Dock = DockStyle.Fill;

            __mDataLoad();

            return;
        }

        #endregion Объект

        #region - Поведение

        /// <summary>
        /// Выполняется при смене выбранной вкладки блока вкладок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cPageBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (__ePageBlockSelectedIndexChanged != null)
                __ePageBlockSelectedIndexChanged(_cPageBlock, e);
        }

        #region Кнопки управления

        /// <summary>
        /// Выполняется при клике левой клавиши мыши по кнопке 'Сохранить'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonSave_ClickLeft(object sender, EventArgs e)
        {
            if (elmApplication.__oMessages.__mShow(nlApplication.MESSAGESTYPES.Question, "Сохранить документ") == DialogResult.Yes)
            {
                if (_mDataSave() == true)
                    FindForm().Close();
            }
            ///* Формирование события клика левой клавиши мыши по кнопке 'Сохранить'
            if (__eButtonSave_ClickLeft != null)
                __eButtonSave_ClickLeft(_cButtonSave, e);
        }
        /// <summary>
        /// Выполняется при клике правой клавиши мыши по кнопке 'Сохранить'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonSave_ClickRight(object sender, EventArgs e)
        {
            /// Формирование события клика правой клавиши мыши по кнопке 'Сохранить'
            if (__eButtonSave_ClickRight != null)
                __eButtonSave_ClickRight(_cButtonSave, e);
        }
        private void mButtonEditCopy_Click(object sender, EventArgs e)
        {
            _fDropDownOpened = true;
            elmForm vForm = FindForm() as elmForm;

            if (vForm != null & __oFormDocumentRecord != null)
            {
                elmFormRecord vFormRecord = (elmFormRecord)Activator.CreateInstance(__oFormDocumentRecord);
                vFormRecord.__cAreaRecord.__fRecordClue = -1;
                vFormRecord.ShowDialog();
            }
        }
        private void mButtonEditEdit_Click(object sender, EventArgs e)
        {
            _fDropDownOpened = true;
        }
        /// <summary>
        /// Выполняется при выборе меню 'Правка / Создать'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonEditNew_Click(object sender, EventArgs e)
        {
            _fDropDownOpened = true;

            return;
        }
        private void mButtonEditRemove_Click(object sender, EventArgs e)
        {
            _fDropDownOpened = true;

            return;
        }
        private void mButtonEditRestore_Click(object sender, EventArgs e)
        {
            _fDropDownOpened = true;

            return;
        }

        #endregion Кнопки управления

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Загрузка данных
        /// </summary>
        /// <returns>[true] - данные получены, иначе - [false]</returns>
        public bool __mDataLoad()
        {
            bool vReturn = true; // Возвращаемое значение

            /// Формирование события перед загрузкой данных
            if (__eDataLoadBefore != null)
                __eDataLoadBefore(this, new EventArgs());

            /// Загрузка заголовка документа
            {
                __oDataTableHeader = __oEssence.__mRecord(__fRecordClue);
                if (__fRecordClue <= 0)
                {
                    DataRow vDataRowHeader = __oEssence.__mRecordNew(__oDataTableHeader);
                    /// Запись идентификатора местной валюты
                    //vDataRowHeader["lnkCurLcl"] = elmApplication.__oData.__mClueByOption("Cur", "mrkLcl");
                    /// Если выполняется копирование текущего документа в новый 
                    if (__fRecordClueForCopy > 0)
                    {
                        DataTable vDataTableForCopy = __oEssence.__mRecord(__fRecordClueForCopy);
                        //vDataRowHeader["lnkArrTyp"] = vDataTableForCopy.Rows[0]["lnkArrTyp"];
                        //vDataRowHeader["lnkCli"] = vDataTableForCopy.Rows[0]["lnkCli"];
                        ////vDataRowHeader["lnkAccCdt"] = vDataTableForCopy.Rows[0]["lnkAccCdt"];
                        ////vDataRowHeader["lnkAccDbt"] = vDataTableForCopy.Rows[0]["lnkAccDbt"];
                        //vDataRowHeader["lnkCurCli"] = vDataTableForCopy.Rows[0]["lnkCurCli"];
                    }
                    __oDataTableHeader.Rows.Add(vDataRowHeader);

                } /// Если идентификатор записи не указан, создается новая запись

                _cBlockInputsLeft.__oDataTable = __oDataTableHeader;
                vReturn = vReturn & _cBlockInputsLeft.__mDataLoad();
                _cBlockInputsRight.__oDataTable = __oDataTableHeader;
                vReturn = vReturn & _cBlockInputsRight.__mDataLoad();
            }

            if (__eDataLoadAfter != null) /// Формирование события после загрузки данных
                __eDataLoadAfter(this, new EventArgs());

            return vReturn;
        }
        /// <summary>
        /// Сохранение данных
        /// </summary>
        protected bool _mDataSave()
        {
            __fTransactionStatus = true; // Статус завершения транзакции
            _cBlockInputsLeft.__mDataSave(); // Запись данных из левой части заголовка
            _cBlockInputsRight.__mDataSave(); // Запись данных из правой части заголовка

            /// Открытие транзакции
            elmApplication.__oData.__mTransactionOn();

            /// Формирование события перед сохранением данных
            if (__eDataSaveBefore != null)
                __eDataSaveBefore(this, new EventArgs());

            __fTransactionStatus = __fTransactionStatus & __oEssence.__mUpdate(__oDataTableHeader); /// Сохранение заголовка прихода
            int vClue = __oEssence.__fLastInsertedKey; /// Идентификатор только, что созданного документа

            /// Формирование события после сохранения данных
            if (__eDataSaveAfter != null)
                __eDataSaveAfter(this, new EventArgs());

            /// Закрытие транзакции
            elmApplication.__oData.__mTransactionOff(__fTransactionStatus);

            return __fTransactionStatus;
        }
        /// <summary>
        /// Проверка заполненности заголовка документа
        /// </summary>
        /// <returns>[true] - заголовок документа заполнен, иначе - [false]</returns>
        public void __mHeaderValid()
        {
            if (__eHeaderValid != null)
                __eHeaderValid(this, new EventArgs());
        }
        /// <summary>
        /// Добавление поля ввода на левую панель заголовка
        /// </summary>
        /// <param name="pInput">Поле ввода</param>
        /// <returns>[true] - поле ввода добавлено, иначе - [false]</returns>
        public bool __mBlockInputsLeftInputAdd(elmInput pInput)
        {
            return _cBlockInputsLeft.__mInputAdd(pInput);
        }
        /// <summary>
        /// Добавление поля ввода на правую панель заголовка
        /// </summary>
        /// <param name="pInput">Поле ввода</param>
        /// <returns>[true] - поле ввода добавлено, иначе - [false]</returns>
        public bool __mBlockInputsRightInputAdd(elmInput pInput)
        {
            return _cBlockInputsRight.__mInputAdd(pInput);
        }
        /// <summary>
        /// Добавление вкладки на блок вкладок
        /// </summary>
        /// <param name="pPage"></param>
        public void __mPageBlockAddPage(elmComponentPage pPage)
        {
            _cPageBlock.TabPages.Add(pPage);

            return;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Сущность данных заголовка
        /// </summary>
        public datUnitEssence __oEssence;
        /// <summary>
        /// Тип формы для изменения данных
        /// </summary>
        public Type __oFormDocumentRecord;
        /// <summary>
        /// Идентификатор записи документа
        /// </summary>
        public int __fRecordClue = -1;
        /// <summary>
        /// Идентификатор записи для копирования в новый документ 
        /// </summary>
        public int __fRecordClueForCopy = -1;
        /// <summary>
        /// Статус завершения транзакции
        /// </summary>
        public bool __fTransactionStatus = true;
        /// <summary>
        /// Тип формы для изменения данных
        /// </summary>
        public CONTROLsOPENEDTYPES __fFormOpenedType = CONTROLsOPENEDTYPES.FormPages;

        #endregion Атрибуты

        #region - Объекты

        /// <summary>
        /// Таблица с данными заголовка документа
        /// </summary>
        public DataTable __oDataTableHeader;

        #endregion Объекты

        #region - Компоненты

        ///// <summary>
        ///// Кнопка 'Правка'
        ///// </summary>
        //protected elmComponentToolbarButtonMenu _cButtonEdit = new elmComponentToolbarButtonMenu();

        //#region Меню кнопки 'Правка'

        ///// <summary>
        ///// Пункт меню 'Копировать' кнопки 'Правка'
        ///// </summary>
        //public elmComponentMenuItem _cButtonEditCopy = new elmComponentMenuItem();
        ///// <summary>
        ///// Пункт меню 'Изменить' кнопки 'Правка'
        ///// </summary>
        //public elmComponentMenuItem _cButtonEditEdit = new elmComponentMenuItem();
        ///// <summary>
        ///// Кнопка 'Правка / Создать'
        ///// </summary>
        //public elmComponentMenuItem _cButtonEditNew = new elmComponentMenuItem();
        ///// <summary>
        ///// Кнопка 'Правка / Удалить'
        ///// </summary>
        //public elmComponentMenuItem _cButtonEditRemove = new elmComponentMenuItem();

        //#endregion Меню кнопки 'Правка'

        /// <summary>
        /// Кнопка 'Операции'
        /// </summary>
        protected elmComponentToolbarButtonMenu _cButtonOperations = new elmComponentToolbarButtonMenu();
        /// <summary>
        /// Кнопка 'Отчеты'
        /// </summary>
        protected elmComponentToolbarButtonMenu _cButtonReports = new elmComponentToolbarButtonMenu();

        /// <summary>
        /// Кнопка 'Сохранить'
        /// </summary>
        protected elmComponentToolbarButton _cButtonSave = new elmComponentToolbarButton(); // Используется при сохранении приходов

        /// <summary>
        /// Разделитель 'Заголовок / Содержание'
        /// </summary>
        protected elmComponentSplitter _cSplitterCaptionContent = new elmComponentSplitter();
        /// <summary>
        /// Разделитель заголовка 'Левое / Правое'
        /// </summary>
        protected elmComponentSplitter _cSplitterCaptionLeftRight = new elmComponentSplitter();
        /// <summary>
        /// Левая панель для размещения полей ввода
        /// </summary>
        protected elmBlockInputs _cBlockInputsLeft = new elmBlockInputs();
        /// <summary>
        /// Правая панель для размещения полей ввода
        /// </summary>
        protected elmBlockInputs _cBlockInputsRight = new elmBlockInputs();

        /// <summary>
        /// Блок вкладок
        /// </summary>
        protected elmComponentPagesBlock _cPageBlock = new elmComponentPagesBlock();

        #endregion Компоненты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        ///// <summary>
        ///// Видимость кнопки 'Правка / Копировать'
        ///// </summary>
        //public bool __fButtonEditCopyVisible_
        //{
        //    get { return _cButtonEditCopy.Visible; }
        //    set { _cButtonEditCopy.Visible = value; }
        //}
        ///// <summary>
        ///// Видимость кнопки 'Правка / Изменить'
        ///// </summary>
        //public bool __fButtonEditEditVisible_
        //{
        //    get { return _cButtonEditEdit.Visible; }
        //    set { _cButtonEditEdit.Visible = value; }
        //}
        ///// <summary>
        ///// Видимость кнопки 'Правка / Создать'
        ///// </summary>
        //public bool __fButtonEditNewVisible_
        //{
        //    get { return _cButtonEditNew.Visible; }
        //    set { _cButtonEditNew.Visible = value; }
        //}
        ///// <summary>
        ///// Видимость кнопки 'Правка / Удалить'
        ///// </summary>
        //public bool __fButtonEditRemoveVisible_
        //{
        //    get { return _cButtonEditRemove.Visible; }
        //    set { _cButtonEditRemove.Visible = value; }
        //}
        /// <summary>
        /// Доступность кнопки 'Сохранить'
        /// </summary>
        public bool __fButtonSaveEnabled_
        {
            get { return _cButtonSave.Enabled; }
            set { _cButtonSave.Enabled = value; }
        }
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
        /// Положение разделителя 'Заголовок | Содержание'
        /// </summary>
        public int __fSplitterCaptionContentSplitterDistance_
        {
            get { return _cSplitterCaptionContent.SplitterDistance; }
            set { _cSplitterCaptionContent.SplitterDistance = value; }
        }
        /// <summary>
        /// Положение разделителя 'Заголовок - Левая часть | Правая часть'
        /// </summary>
        public int __fSplitterCaptionLeftRightSplitterDistance_
        {
            get { return _cSplitterCaptionLeftRight.SplitterDistance; }
            set { _cSplitterCaptionLeftRight.SplitterDistance = value; }
        }

        /// <summary>
        /// Индекс выбранной вкладки в блоке вкладок
        /// </summary>
        public int __fPageBlockSelectedPageIndex
        {
            get { return _cPageBlock.SelectedIndex; }
            set { _cPageBlock.SelectedIndex = value; }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при клике левой клавишей мыши по кнопке 'Сохранить'
        /// </summary>
        public event EventHandler __eButtonSave_ClickLeft;
        /// <summary>
        /// Возникает при клике правой клавишей мыши по кнопке 'Сохранить'
        /// </summary>
        public event EventHandler __eButtonSave_ClickRight;
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
        /// <summary>
        /// Возникает после сохранения данных, до закрытия транзакции
        /// </summary>
        public event EventHandler __eDataSaveAfter;
        /// <summary>
        /// Возникает перед сохранением данных, после открытия транзакции
        /// </summary>
        public event EventHandler __eDataSaveBefore;
        /// <summary>
        /// Возникает при проверке заполненности заголовка документа
        /// </summary>
        public event EventHandler __eHeaderValid;
        /// <summary>
        /// Возникает при смене выбранной вкладки блока вкладок
        /// </summary>
        public event EventHandler __ePageBlockSelectedIndexChanged;

        #endregion СОБЫТИЯ
    }
}
