using nlApplication;
using nlData;
using System;
using System.Data;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmAreaDocument.cs
    /// </summary>
    /// <remarks>Класс-область для правки документа</remarks>
    public class elmAreaDocument : elmArea
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            _fError = new appUnitError();

            #region /// Размещение компонентов

            _cToolBar.Items.Insert(0, _cButtonSave);
            _cToolBar.Items.Insert(0, _cButtonOperations);
            _cToolBar.Items.Insert(0, _cButtonReports);

            Panel2.Controls.Add(_cSplitterCaptionContent);
            Panel2.Controls.SetChildIndex(_cSplitterCaptionContent, 0);
            _cSplitterCaptionContent.Panel1.Controls.Add(_cSplitterCaptionLeftRight);
            {
                _cSplitterCaptionLeftRight.Panel1.Controls.Add(__cBlockInputsLeft);
                _cSplitterCaptionLeftRight.Panel2.Controls.Add(__cBlockInputsRight);
            }
            __cBlockInputsLeft.__mInputAdd(__cInputNumber);
            __cBlockInputsLeft.__mInputAdd(__cInputDateTimeCreate);

            _cSplitterCaptionContent.Panel2.Controls.Add(_cToolBarContent);

            _cToolBarContent.Items.Add(_cButtonColumns);
            _cToolBarContent.Items.Add(_cButtonEdit);
            {
                _cButtonEdit.DropDownItems.Add(_cButtonEditCreate);
                _cButtonEdit.DropDownItems.Add(_cButtonEditCopy);
                _cButtonEdit.DropDownItems.Add(_cButtonEditEdit);
                _cButtonEdit.DropDownItems.Add(_cButtonEditRemove);
            }

            _cSplitterCaptionContent.Panel2.Controls.Add(_cPageBlock);
            _cSplitterCaptionContent.Panel2.Controls.SetChildIndex(_cPageBlock, 0);
            _cPageBlock.TabPages.Add(_cPageContent);
            _cPageContent.Controls.Add(__cGridContent);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            Dock = DockStyle.Fill;
            Orientation = Orientation.Horizontal;

            __cBlockInputsLeft.AutoScroll = true;
            __cBlockInputsRight.AutoScroll = true;

            // __cToolBar
            {
                // _cButtonSave
                {
                    _cButtonSave.__eClickLeft += mButtonSave_ClickLeft;
                    _cButtonSave.__eClickRight += mButtonSave_ClickRight;
                    _cButtonSave.Image = global::nlResourcesImages.Properties.Resources._Computer_Diskette_b32;
                    _cButtonSave.ToolTipText = "[ Ctrl + A ] " + elmApplication.__oTunes.__mTranslate("Сохранить");
                }
                // _cButtonOperations
                {
                    _cButtonOperations.Alignment = ToolStripItemAlignment.Right;
                    _cButtonOperations.Image = global::nlResourcesImages.Properties.Resources._PageGear_y32;
                }
                // _cButtonReports
                {
                    _cButtonReports.Image = global::nlResourcesImages.Properties.Resources._PagePrinter_y32;
                    _cButtonReports.Alignment = ToolStripItemAlignment.Right;
                }
            }
            // _cToolBarContent
            {
                // _cButtonEdit
                {
                    _cButtonEdit.Alignment = ToolStripItemAlignment.Right;
                    //_cButtonEdit.Click 
                    //_cButtonEdit._eMouseClickRight 
                    _cButtonEdit.Image = global::nlResourcesImages.Properties.Resources._Page_b32;
                    _cButtonEdit.ToolTipText = "[ Ctrl + E ] " + elmApplication.__oTunes.__mTranslate("Правка");
                    {
                        _cButtonEditCreate.Click += mButtonEditCreate_Click;
                        _cButtonEditCreate.Image = global::nlResourcesImages.Properties.Resources._Page_b16;
                        _cButtonEditCreate.__fCaption_ = "Создать";

                        _cButtonEditCopy.Click += mButtonEditCopy_Click;
                        _cButtonEditCopy.Image = global::nlResourcesImages.Properties.Resources._PageCopy_b16;
                        _cButtonEditCopy.__fCaption_ = "Копировать";

                        _cButtonEditEdit.Click += mButtonEditEdit_Click;
                        _cButtonEditEdit.Image = global::nlResourcesImages.Properties.Resources._PageEdit_b16;
                        _cButtonEditEdit.__fCaption_ = "Изменить";

                        _cButtonEditRemove.Click += mButtonEditRemove_Click;
                        _cButtonEditRemove.Image = global::nlResourcesImages.Properties.Resources._PageDelete_b16;
                        _cButtonEditRemove.__fCaption_ = "Удалить";
                    }
                }
                // _cButtonColumns
                {
                    _cButtonColumns.Alignment = ToolStripItemAlignment.Right;
                    //_cButtonColumns.DropDownOpened += mButtonDropDownOpened;
                    _cButtonColumns.Image = global::nlResourcesImages.Properties.Resources._Grid_Fields_b32;
                    _cButtonColumns.ToolTipText = "[ F12 ] " + elmApplication.__oTunes.__mTranslate("Видимость колонок");
                    //_cButtonColumns.__eClickRight += mButtonColumns_eMouseClickRight;
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
                                __cBlockInputsLeft.Dock = DockStyle.Fill;
                                __cBlockInputsLeft.__fMarkShow = false;
                            }
                            // _cBlockInputsRight
                            {
                                __cBlockInputsRight.Dock = DockStyle.Fill;
                                __cBlockInputsRight.__fMarkShow = false;
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

            _cPageContent.__fCaption_ = "Содержание";
            _cPageContent.BackColor = elmApplication.__oInterface.__fColorFormActive;

            // _cComponentGrid
            {
                __cGridContent.Dock = DockStyle.Fill;
                //__cGridContent.Row
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

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

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
            if (__eButtonContentEditEdit != null)
                __eButtonContentEditEdit(sender, e);

        }
        /// <summary>
        /// Выполняется при выборе меню 'Правка / Создать'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void mButtonEditCreate_Click(object sender, EventArgs e)
        {
            _fDropDownOpened = true;

            if (__eButtonContentEditCreate != null)
                __eButtonContentEditCreate(sender, e);

            //elmForm vForm = FindForm() as elmForm;

                //if (vForm != null & __oFormDocumentRecord != null)
                //{
                //    elmFormRecord vFormRecord = (elmFormRecord)Activator.CreateInstance(__oFormDocumentRecord);
                //    vFormRecord.__cAreaRecord.__fRecordClue = -1;
                //    vFormRecord.ShowDialog();
                //}

            return;
        }
        private void mButtonEditRemove_Click(object sender, EventArgs e)
        {
            _fDropDownOpened = true;

            if (__eButtonContentEditDelete != null)
                __eButtonContentEditDelete(sender, e);

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
            return __cGridContent.__mColumnAdd(pCaption, pPrompt, pFieldName, pReadOnly, pVisible, pType, pCellStyle);
        }
        /// <summary>
        /// Загрузка данных
        /// </summary>
        /// <returns>[true] - данные получены, иначе - [false]</returns>
        protected virtual bool __mDataLoad()
        {
            bool vReturn = false; // Возвращаемое значение
            _fError.__mClear();
            _fError.__fHelpFileName_ = "";
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Ключ документа", __fDocumentClue);

            /// Формирование события перед загрузкой данных
            if (__eDataLoadBefore != null)
                __eDataLoadBefore(this, new EventArgs());

            /// Режим не определен
            if(__fFormMode == FORMMODE.None)
            {
                _fError.__fMessage_ = "Не определен режим использования окна";
                _fError.__mReasonAdd("Поле '__fFormMode' опрелено как [FORMMODE.None]");
                elmApplication.__oErrorsHandler.__mShow(_fError);
                return vReturn;
            }
            /// Режим создания данных
            if (__fFormMode == FORMMODE.ForCreate)
            {
                __oDataTableCaption = __oEssenceCaption.__mRecord(-1);
                __oDataTableCaption.Rows.Add(__oEssenceCaption.__mRecordNew(__oDataTableCaption));
                ///* Привязка данных к полям ввода
                ///** Привязка компонентов левой панели
                foreach (Control oInput in __cBlockInputsLeft.Controls)
                {
                    if ((oInput is elmInput) == true)
                    {
                        if ((oInput as elmInput).__fFieldName.Length == 0)
                            continue;

                        if ((oInput is elmInputBool) == true)
                        {
                            (oInput as elmInputBool).__fValue_ = Convert.ToBoolean(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }

                        if ((oInput is elmInputCombo) == true)
                        {
                            (oInput as elmInputCombo).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }

                        if ((oInput is elmInputDateTime) == true)
                        {
                            (oInput as elmInputDateTime).__fValue_ = Convert.ToDateTime(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }
                        if ((oInput is elmInputFormCode) == true)
                        {
                            (oInput as elmInputFormCode).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }
                        if ((oInput is elmInputFormName) == true)
                        {
                            if (Convert.ToInt32((oInput as elmInputFormName).__fValue_) == 0)
                            {
                                (oInput as elmInputFormName).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            }
                            continue;
                        }
                        if (oInput is elmInputNumeric)
                        {
                            (oInput as elmInputNumeric).__fValue_ = Convert.ToDecimal(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }
                        if ((oInput is elmInputInteger) == true)
                        {
                            (oInput as elmInputInteger).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }
                        if ((oInput is elmInputQuote) == true)
                        {
                            (oInput as elmInputQuote).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            //(vInput as elmInputQuote).__mDataLoad();

                            continue;
                        }
                        if ((oInput is elmInputString) == true)
                        {
                            (oInput as elmInputString).__fValue_ = Convert.ToString(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }
                        if ((oInput is elmInputPathFile) == true)
                        {
                            (oInput as elmInputPathFile).__fValue_ = Convert.ToString(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }
                    }
                }
                ///** Привязка компонентов правой панели
                foreach (Control oInput in __cBlockInputsRight.Controls)
                {
                    if ((oInput is elmInput) == true)
                    {
                        if ((oInput as elmInput).__fFieldName.Length == 0)
                            continue;

                        if ((oInput is elmInputBool) == true)
                        {
                            (oInput as elmInputBool).__fValue_ = Convert.ToBoolean(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }

                        if ((oInput is elmInputCombo) == true)
                        {
                            (oInput as elmInputCombo).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }

                        if ((oInput is elmInputDateTime) == true)
                        {
                            (oInput as elmInputDateTime).__fValue_ = Convert.ToDateTime(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }
                        if ((oInput is elmInputFormCode) == true)
                        {
                            (oInput as elmInputFormCode).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }
                        if ((oInput is elmInputFormName) == true)
                        {
                            if (Convert.ToInt32((oInput as elmInputFormName).__fValue_) == 0)
                            {
                                (oInput as elmInputFormName).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            }
                            continue;
                        }
                        if (oInput is elmInputNumeric)
                        {
                            (oInput as elmInputNumeric).__fValue_ = Convert.ToDecimal(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }
                        if ((oInput is elmInputInteger) == true)
                        {
                            (oInput as elmInputInteger).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }
                        if ((oInput is elmInputQuote) == true)
                        {
                            (oInput as elmInputQuote).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            //(vInput as elmInputQuote).__mDataLoad();

                            continue;
                        }
                        if ((oInput is elmInputString) == true)
                        {
                            (oInput as elmInputString).__fValue_ = Convert.ToString(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }
                        if ((oInput is elmInputPathFile) == true)
                        {
                            (oInput as elmInputPathFile).__fValue_ = Convert.ToString(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                            continue;
                        }
                    }
                }

            }
            /// Режим правки данных
            if (__fFormMode == FORMMODE.ForEdit)
            { 
                ///* Загрузка заголовка документа
                {
                    __oDataTableCaption = __oEssenceCaption.__mRecord(__fDocumentClue);
                    ///** Если идентификатор документа не указан, возвращается ошибка
                    if (__fDocumentClue <= 0)
                    {
                        //            DataRow vDataRowHeader = __oEssence.__mRecordNew(__oDataTableHeader);
                        //            ///// Запись идентификатора местной валюты
                        //            //vDataRowHeader["lnkCurLcl"] = elmApplication.__oData.__mClueByOption("Cur", "mrkLcl");
                        //            ///// Если выполняется копирование текущего документа в новый 
                        //            //if (__fRecordClueForCopy > 0)
                        //            //{
                        //            //    DataTable vDataTableForCopy = __oEssence.__mRecord(__fRecordClueForCopy);
                        //            //    vDataRowHeader["lnkArrTyp"] = vDataTableForCopy.Rows[0]["lnkArrTyp"];
                        //            //    vDataRowHeader["lnkCli"] = vDataTableForCopy.Rows[0]["lnkCli"];
                        //            //    //vDataRowHeader["lnkAccCdt"] = vDataTableForCopy.Rows[0]["lnkAccCdt"];
                        //            //    //vDataRowHeader["lnkAccDbt"] = vDataTableForCopy.Rows[0]["lnkAccDbt"];
                        //            //    vDataRowHeader["lnkCurCli"] = vDataTableForCopy.Rows[0]["lnkCurCli"];
                        //            //}
                        //            __oDataTableHeader.Rows.Add(vDataRowHeader);
                    }
                    /// ** Если идентификатор записи указан, выполняется загрузка данных
                    if (__fDocumentClue <= 0)
                    {
                        _fError.__fMessage_ = "Не возможно открыть документ для правки";
                        ///*** Проверка идентификатора записи документа
                        {
                            if (__oEssenceCaption.__mClueExists(__fDocumentClue) == false)
                            {
                                _fError.__mReasonAdd("Документ отсутствует в базе данных");
                            }
                            if (__oEssenceCaption.__mClueIsDelete(__fDocumentClue) == false)
                            {
                                _fError.__mReasonAdd("Документ удален");
                            }
                            if (_fError.__fReasonS_.Count > 0)
                            {
                                elmApplication.__oErrorsHandler.__mShow(_fError);
                                return vReturn;
                            }
                            else
                                _fError.__fReasonS_.Clear();
                        }
                        __oDataTableCaption = __oEssenceCaption.__mGrid("CLU = " + __fDocumentClue.ToString(), "");
                    }
                    ///** Привязка данных к полям ввода
                    ///*** Привязка компонентов левой панели
                    foreach (Control oInput in __cBlockInputsLeft.Controls)
                    {
                        if ((oInput is elmInput) == true)
                        {
                            if ((oInput as elmInput).__fFieldName.Length == 0)
                                continue;

                            if ((oInput is elmInputBool) == true)
                            {
                                (oInput as elmInputBool).__fValue_ = Convert.ToBoolean(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }

                            if ((oInput is elmInputCombo) == true)
                            {
                                (oInput as elmInputCombo).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }

                            if ((oInput is elmInputDateTime) == true)
                            {
                                (oInput as elmInputDateTime).__fValue_ = Convert.ToDateTime(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }
                            if ((oInput is elmInputFormCode) == true)
                            {
                                (oInput as elmInputFormCode).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }
                            if ((oInput is elmInputFormName) == true)
                            {
                                if (Convert.ToInt32((oInput as elmInputFormName).__fValue_) == 0)
                                {
                                    (oInput as elmInputFormName).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                }
                                continue;
                            }
                            if (oInput is elmInputNumeric)
                            {
                                (oInput as elmInputNumeric).__fValue_ = Convert.ToDecimal(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }
                            if ((oInput is elmInputInteger) == true)
                            {
                                (oInput as elmInputInteger).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }
                            if ((oInput is elmInputQuote) == true)
                            {
                                (oInput as elmInputQuote).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                //(vInput as elmInputQuote).__mDataLoad();

                                continue;
                            }
                            if ((oInput is elmInputString) == true)
                            {
                                (oInput as elmInputString).__fValue_ = Convert.ToString(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }
                            if ((oInput is elmInputPathFile) == true)
                            {
                                (oInput as elmInputPathFile).__fValue_ = Convert.ToString(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }
                        }
                    }
                    ///*** Привязка компонентов правой панели
                    foreach (Control oInput in __cBlockInputsRight.Controls)
                    {
                        if ((oInput is elmInput) == true)
                        {
                            if ((oInput as elmInput).__fFieldName.Length == 0)
                                continue;

                            if ((oInput is elmInputBool) == true)
                            {
                                (oInput as elmInputBool).__fValue_ = Convert.ToBoolean(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }

                            if ((oInput is elmInputCombo) == true)
                            {
                                (oInput as elmInputCombo).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }

                            if ((oInput is elmInputDateTime) == true)
                            {
                                (oInput as elmInputDateTime).__fValue_ = Convert.ToDateTime(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }
                            if ((oInput is elmInputFormCode) == true)
                            {
                                (oInput as elmInputFormCode).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }
                            if ((oInput is elmInputFormName) == true)
                            {
                                if (Convert.ToInt32((oInput as elmInputFormName).__fValue_) == 0)
                                {
                                    (oInput as elmInputFormName).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                }
                                continue;
                            }
                            if (oInput is elmInputNumeric)
                            {
                                (oInput as elmInputNumeric).__fValue_ = Convert.ToDecimal(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }
                            if ((oInput is elmInputInteger) == true)
                            {
                                (oInput as elmInputInteger).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }
                            if ((oInput is elmInputQuote) == true)
                            {
                                (oInput as elmInputQuote).__fValue_ = Convert.ToInt32(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                //(vInput as elmInputQuote).__mDataLoad();

                                continue;
                            }
                            if ((oInput is elmInputString) == true)
                            {
                                (oInput as elmInputString).__fValue_ = Convert.ToString(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }
                            if ((oInput is elmInputPathFile) == true)
                            {
                                (oInput as elmInputPathFile).__fValue_ = Convert.ToString(__oDataTableCaption.Rows[0][(oInput as elmInput).__fFieldName]);
                                continue;
                            }
                        }
                    }
                }
            }

            /// Загрузка содержания документа
            //+__cGridContent.__mDataLoad("lnk" + __oEssenceCaption.__fTableName + " = " + __fRecordClue.ToString() + " and " + __oEssence.__fTableAlias + ".ELD = 0", "Pos", -1);
            __cGridContent.__mDataLoad("AC.ELD = 0", "Pos", -1);

            /// Формирование события после загрузки данных
            if (__eDataLoadAfter != null) 
                __eDataLoadAfter(this, new EventArgs());

            return vReturn;
        }
        /// <summary>
        /// Сохранение данных
        /// </summary>
        protected virtual bool _mDataSave()
        {
            __fTransactionStatus = true; // Статус завершения транзакции
            __cBlockInputsLeft.__mDataSave(); // Запись данных из левой части заголовка
            __cBlockInputsRight.__mDataSave(); // Запись данных из правой части заголовка

            /// Открытие транзакции
            elmApplication.__oData.__mTransactionOn();

            /// Формирование события перед сохранением данных
            if (__eDataSaveBefore != null)
                __eDataSaveBefore(this, new EventArgs());
            /// Сохранение заголовка прихода
            __fTransactionStatus = __fTransactionStatus & __oEssenceCaption.__mUpdate(__oDataTableCaption); 
            int vClue = __oEssenceCaption.__fLastInsertedKey; // Идентификатор только, что созданного документа
            /// Сохранение содержание прихода
            /// Заполнение идентификатора прихода в содержании
            foreach (DataRow vDataRow in (__cGridContent.DataSource as DataTable).Rows)
            {
                if (Convert.ToInt32(vDataRow["lnkArr"]) == 0)
                {
                    vDataRow["lnkArr"] = vClue;
                }
            }
            /// Сохранение содержания документа
            __fTransactionStatus = __fTransactionStatus & __cGridContent.__oEssence.__mUpdate(__cGridContent.DataSource as DataTable);

            /// Закрытие транзакции
            elmApplication.__oData.__mTransactionOff(__fTransactionStatus);

            /// Формирование события после сохранения данных                                                                                                          
            if (__eDataSaveAfter != null)
                __eDataSaveAfter(this, new EventArgs());


            return false;
        }
        /// <summary>
        /// Добавление колонок в сетку
        /// </summary>
        /// <returns>[true] - колонки добавлены, иначе - [false]</returns>
        public bool __mGridBuild()
        {
            bool vReturn = __cGridContent.__mColumnsBuild();
            mMenuFieldFill();
            return vReturn;
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
            return __cBlockInputsLeft.__mInputAdd(pInput);
        }
        /// <summary>
        /// Добавление поля ввода на правую панель заголовка
        /// </summary>
        /// <param name="pInput">Поле ввода</param>
        /// <returns>[true] - поле ввода добавлено, иначе - [false]</returns>
        public bool __mBlockInputsRightInputAdd(elmInput pInput)
        {
            return __cBlockInputsRight.__mInputAdd(pInput);
        }
        /// <summary>
        /// Заполнение меню кнопки "Колонки" данными
        /// </summary>
        private void mMenuFieldFill()
        {
            if (__cGridContent.__fColumnsList.Count > 0)
            {
                foreach (elmUnitGridColumn vColumn in __cGridContent.__fColumnsList)
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
                    if (_cToolStripMenuItemColumn.Name.ToUpper() == "DSI" + __cGridContent.__oEssence.__fTableName.ToUpper())
                    {
                        __cGridContent.Columns[vColumn.__fField].Visible = true;
                        _cToolStripMenuItemColumn.Enabled = false;
                    }
                    /// Определение видимости соответствующего поля в сетке
                    else
                        __cGridContent.Columns[vColumn.__fField].Visible = _cToolStripMenuItemColumn.Checked;
                    _cButtonColumns.DropDownItems.Add(_cToolStripMenuItemColumn);

                    #endregion Меню - видимость колонок
                }
            }
            _cButtonColumns.PerformClick();
            //-__mSortingLoad(); // Загрузка сортировки
        }
        /// <summary>
        /// Изменение статуса видимости любой колонки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mFieldsVisibleCheckedChanged(object sender, System.EventArgs e)
        {
            if (__cGridContent.Columns[(sender as ToolStripMenuItem).Name] != null)
            {
                __cGridContent.Columns[(sender as ToolStripMenuItem).Name].Visible = (sender as ToolStripMenuItem).Checked; /// Исправление видимости колонки в сетке
                __cGridContent.__mColumnChangeVisible((sender as ToolStripMenuItem).Name, (sender as ToolStripMenuItem).Checked); /// Исправление видимости колонки в настройках сетки
            }
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
        public datUnitEssence __oEssenceCaption;
        /// <summary>
        /// Режим использования формы
        /// </summary>
        public FORMMODE __fFormMode = FORMMODE.None;
        /// <summary>
        /// Тип формы для изменения данных
        /// </summary>
        public Type __oFormDocumentRecord;
        /// <summary>
        /// Идентификатор записи документа
        /// </summary>
        public int __fDocumentClue = -1;
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

        #region - Компоненты

        /// <summary>
        /// Панель инструментов для записей документа
        /// </summary>
        protected elmComponentToolbar _cToolBarContent = new elmComponentToolbar();

        /// <summary>
        /// Кнопка 'Правка'
        /// </summary>
        protected elmComponentToolbarButtonMenu _cButtonEdit = new elmComponentToolbarButtonMenu();
        #region Меню кнопки 'Правка'

        /// <summary>
        /// Пункт меню 'Копировать' кнопки 'Правка'
        /// </summary>
        public elmComponentMenuItem _cButtonEditCopy = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'Изменить' кнопки 'Правка'
        /// </summary>
        public elmComponentMenuItem _cButtonEditEdit = new elmComponentMenuItem();
        /// <summary>
        /// Кнопка 'Правка / Создать'
        /// </summary>
        public elmComponentMenuItem _cButtonEditCreate = new elmComponentMenuItem();
        /// <summary>
        /// Кнопка 'Правка / Удалить'
        /// </summary>
        public elmComponentMenuItem _cButtonEditRemove = new elmComponentMenuItem();

        #endregion Меню кнопки 'Правка'

        /// <summary>
        /// Кнопка 'Видимость колонок'
        /// </summary>
        protected elmComponentToolbarButtonMenu _cButtonColumns = new elmComponentToolbarButtonMenu();

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
        public elmBlockInputs __cBlockInputsLeft = new elmBlockInputs();
        /// <summary>
        /// Правая панель для размещения полей ввода
        /// </summary>
        public elmBlockInputs __cBlockInputsRight = new elmBlockInputs();


        /// <summary>
        /// Поле ввода номера документа
        /// </summary>
        public elmInputInteger __cInputNumber = new elmInputInteger();
        /// <summary>
        /// Поле ввода времени создания документа
        /// </summary>
        public elmInputDateTime __cInputDateTimeCreate = new elmInputDateTime();

        /// <summary>
        /// Блок вкладок
        /// </summary>
        protected elmComponentPagesBlockColor _cPageBlock = new elmComponentPagesBlockColor();

        /// <summary>
        /// Вкладка содержания документа
        /// </summary>
        protected elmComponentPage _cPageContent = new elmComponentPage();
        public elmComponentGrid __cGridContent = new elmComponentGrid();

        #endregion Компоненты

        #region - Скрытые

        /// <summary>
        /// Таблица с данными заголовка документа
        /// </summary>
        protected DataTable __oDataTableCaption;
        /// <summary>
        /// Таблица с данными содержанием документа
        /// </summary>
        protected DataTable __oDataTableContent;

        #endregion Скрытые

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        #region = СВОЙСТВА

        /// <summary>
        /// Видимость кнопки 'Правка'
        /// </summary>
        public bool __fButtonEditVisible_
        {
            get { return _cButtonEdit.Visible; }
            set { _cButtonEdit.Visible = value; }
        }
        #endregion СВОЙСТВА

        /// <summary>
        /// Сущность данных заголовка
        /// </summary>
        public datUnitEssence __oEssenceContent_
        {
            get { return __cGridContent.__oEssence; }
            set 
            {
                __cGridContent.__oEssence = value;
            }
        }


        /// <summary>
        /// Видимость кнопки 'Правка / Копировать'
        /// </summary>
        public bool __fButtonEditCopyVisible_
        {
            get { return _cButtonEditCopy.Visible; }
            set { _cButtonEditCopy.Visible = value; }
        }
        /// <summary>
        /// Видимость кнопки 'Правка / Изменить'
        /// </summary>
        public bool __fButtonEditEditVisible_
        {
            get { return _cButtonEditEdit.Visible; }
            set { _cButtonEditEdit.Visible = value; }
        }
        /// <summary>
        /// Видимость кнопки 'Правка / Создать'
        /// </summary>
        public bool __fButtonEditNewVisible_
        {
            get { return _cButtonEditCreate.Visible; }
            set { _cButtonEditCreate.Visible = value; }
        }
        /// <summary>
        /// Видимость кнопки 'Правка / Удалить'
        /// </summary>
        public bool __fButtonEditRemoveVisible_
        {
            get { return _cButtonEditRemove.Visible; }
            set { _cButtonEditRemove.Visible = value; }
        }
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

        public event EventHandler __eButtonContentEditCreate;
        public event EventHandler __eButtonContentEditEdit;
        public event EventHandler __eButtonContentEditDelete;

        #endregion СОБЫТИЯ
    }
}
