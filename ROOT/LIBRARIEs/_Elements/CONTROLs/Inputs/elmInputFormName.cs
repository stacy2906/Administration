using nlApplication;
using nlData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmInputFormName.cs
    /// </summary>
    /// <remarks>Класс-поле ввода значений поиском по названию</remarks>
    /* <example>
    _cInputLnkPrj.__fCaption_ = elmApplication.__oTunes.__mTranslate("Проект");
    _cInputLnkPrj.__fFieldName = "lnkPrj";
    _cInputLnkPrj.__oEssence = new cbnEssencePrj();
    _cInputLnkPrj.__oFormSelect = typeof(cbnFormGridPrj);
    _cInputLnkPrj.__fFormSelectledType = FORMSELECTEDTYPES.FormGrid;
    _cInputLnkPrj.__fFieldsCharList.Add("dsiPrj");
    _cInputLnkPrj.__fFieldCode = "codPrj";
    _cInputLnkPrj.__fFormSearchCaption_ = elmApplication.__oTunes.__mTranslate("Поиск проекта");

    #region Сетка / Определение колонок

    _cInputLnkPrj.__mColumnAdd(prjApplication.__oTunes.__mTranslate("Ключ")
    , "CLU"
    , true
    , false
    , DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
    _cInputLnkPrj.__mColumnAdd(prjApplication.__oTunes.__mTranslate("Код")
    , "codPrj"
    , true
    , true
    , DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
    _cInputLnkPrj.__mColumnAdd(prjApplication.__oTunes.__mTranslate("Название")
        , "dsiPrj"
    , true
    , true
    , DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);

    #endregion Сетка / Определение колонок
    </example> */
    public class elmInputFormName : elmInput
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

            Panel2.Controls.Add(_cInput);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            _fError = new appUnitError();

            // _cLabelCaption
            {
                _cLabelCaption.__fLabelType_ = LABELTYPES.Button; /// Назвначение вида надписи-заголовка - 'Кнопка' 
                _cLabelCaption.__eClickLeft += mLabelCaption_MouseClickLeft;
                _cLabelCaption.__eClickRight += mLabelCaption_MouseClickRight;
            }
            fLabelCaptionStatus = _cLabelCaption.__fLabelType_; /// Сохранение установленного статуса надписи-заголовка
            // _cLabelValue
            {
                _cLabelValue.__fCaption_ = "Значение не определено";
                _cLabelValue.Visible = false;
            }
            // _cInput
            {
                _cInput.Location = new Point(0, 0);
                _cInput.TextChanged += cSearch_TextChanged;
                _cInput.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                _cInput.__eChangedByUserAfter += mInput_InteractiveChanged;
            }

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }

        #endregion Объект

        private void mInput_InteractiveChanged(object sender, EventArgs e)
        {
            __fMarkStatus_ = true; /// Включение использования фильтра
        }

        /// <summary>
        /// Выполняется при выборе надписи левой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mLabelCaption_MouseClickLeft(object sender, EventArgs e)
        {
            elmForm vForm = FindForm() as elmForm;
            if (vForm != null & __oFormSelect != null)
            {
                switch (__fFormSelectledType)
                {
                    case FORMSELECTEDTYPES.FormGrid:
                        elmFormGrid vFormGrid = (elmFormGrid)Activator.CreateInstance(__oFormSelect);
                        vFormGrid.__cAreaGrid.__oEssence_ = this.__oEssence;
                        /// Восстановить vFormFilter._cAreaFilter._fFormNameParent = vForm.Name;
                        (vFormGrid as elmFormGrid).ShowDialog();
                        if ((vFormGrid as elmForm).__fClosedByXButtonOrAltF4_ == false)
                        {
                            fValue = vFormGrid.__cAreaGrid.__fRecordClue_;
                            DataTable vDataTableGrid = __oEssence.__mRecord(fValue);
                            if (vDataTableGrid.Rows.Count > 0)
                            {
                                _cInput.Text = Convert.ToString(vDataTableGrid.Rows[0]["dsi" + __oEssence.__fTableName]).Trim();
                                _cLabelValue.Text = _cInput.Text;
                                if (__eOnInteractivatChange != null)
                                    __eOnInteractivatChange(this, new EventArgs());
                            }
                        }
                        break;
                    case FORMSELECTEDTYPES.FormTree:
                        elmFormTree vFormTree = (elmFormTree)Activator.CreateInstance(__oFormSelect);
                        vFormTree.__cAreaTree.__oEssence_ = this.__oEssence;
                        (vFormTree as elmFormTree).ShowDialog();
                        if ((vFormTree as elmForm).__fClosedByXButtonOrAltF4_ == false)
                        {
                            fValue = vFormTree.__cAreaTree.__fRecordClue_;
                            DataTable vDataTableGrid = __oEssence.__mRecord(fValue);
                            if (vDataTableGrid.Rows.Count > 0)
                            {
                                _cInput.Text = Convert.ToString(vDataTableGrid.Rows[0]["dsi" + __oEssence.__fTableName]).Trim();
                                _cLabelValue.Text = _cInput.Text;
                                if (__eOnInteractivatChange != null)
                                    __eOnInteractivatChange(this, new EventArgs());
                            }
                        }
                        break;
                        //case FORMSELECTEDTYPES.FormGridFolder:
                        //elmFormGridFolder vFormGridFolder = (elmFormGridFolder)Activator.CreateInstance(__oFormSelect);
                        ///// Восстановить vFormFilter._cAreaFilter._fFormNameParent = vForm.Name;
                        //(vFormGridFolder as elmFormGridFolder).ShowDialog();
                        //if ((vFormGridFolder as elmForm).__fClosedByXButtonOrAltF4_ == false)
                        //{
                        //    fValue = vFormGridFolder.__cAreaGridFolder.__fRecordClue;
                        //    DataTable vDataTableGridFolder = __oEssence._mRecord(fValue);
                        //    if (vDataTableGridFolder.Rows.Count > 0)
                        //{
                        //    _cInput.Text = Convert.ToString(vDataTableGridFolder.Rows[0]["dsi" + __oEssence.__fTableName]).Trim();
                        //    _cLabelValue.Text = _cInput.Text;
                        //    if (__eOnInteractivatChange != null)
                        //        __eOnInteractivatChange(this, new EventArgs());
                        //}
                        //}
                        //break;
                }
            }
            else
            {
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                _fError.__fFilePath_ = _fClassFilePath_;
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mMessageBuild("Форма для построения выбора значений из справочника не определена");
                _fError.__fProcedure_ = _fClassProcedure_;
                elmApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
            }

            _cInput.Focus();
        }
        /// <summary>
        /// Выполняется при выборе надписи правой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mLabelCaption_MouseClickRight(object sender, EventArgs e)
        {
            //_cLabelValue.Text = crlApplication.__oTunes.__mTranslate("Значение не определено");
            fValue = 0; /// Значение приравниватся [0]
            _cInput.Text = ""; /// Очищается название искомого справочника
            __fMarkStatus_ = false; /// !!! Выключается использование фильтра
            _cInput.Focus(); /// Перемещается курсор в поле ввода

        }
        /// <summary>
        /// Выполняется при изменении текста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cSearch_TextChanged(object sender, EventArgs e)
        {
            if (_cInput.Text.EndsWith(" ") == true)
            {
                if (_cInput.Text.StartsWith("0") == true)
                {
                    try
                    {
                        fValue = elmApplication.__oData.__mClueByCode(__oEssence.__fTableName, Convert.ToInt32(_cInput.Text.Trim().Substring(1)));
                        _cInput.Text = elmApplication.__oData.__mNameByClue(__oEssence.__fTableName, fValue);
                    }
                    catch
                    {
                        fValue = 0;
                    }
                } /// Поиск справочника по учетному коду
                else
                {
                    elmFormSearch vFormSearch = new elmFormSearch();
                    vFormSearch.Text = fFormSearchCaption;
                    vFormSearch._cAreaSearch.__fColumnsList_ = fColumnsList;
                    vFormSearch._cAreaSearch.__fEssence_ = __oEssence;
                    vFormSearch._cAreaSearch.__fFieldsCharList = __fFieldsCharList;
                    vFormSearch._cAreaSearch.__fFieldCode = __fFieldCode;
                    vFormSearch._cAreaSearch.__fFilterAdditional = __fFilterAdditional;
                    vFormSearch._cAreaSearch.__fFilterAdditionalInversion = __fFilterAdditionalInversion;
                    vFormSearch._cAreaSearch.__mColumnsBuild();
                    vFormSearch._cAreaSearch.__fStringSearchText_ = _cInput.Text;
                    vFormSearch._cAreaSearch.__fStringSearchSelectionStart_ = vFormSearch._cAreaSearch.__fStringSearchText_.Trim().Length;
                    vFormSearch._cAreaSearch.__fStringSearchSelectionLength_ = 0;
                    vFormSearch._cAreaSearch.__mStringSearchFocus();

                    (vFormSearch as elmFormSearch).ShowDialog();
                    //if (vFormSearch._cAreaSearch.__fValueClue > 0)
                    //{
                    _cInput.Text = vFormSearch._cAreaSearch.__fValueString.Trim();
                    _cLabelValue.Text = _cInput.Text;
                    fValue = vFormSearch._cAreaSearch.__fValueClue;
                    if (__eOnInteractivatChange != null)
                        __eOnInteractivatChange(this, new EventArgs());
                    //}
                }
            }
            else
            {
                if (__eOnInteractivatChange != null)
                    __eOnInteractivatChange(this, new EventArgs());
            }
        }

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
        public void __mColumnAdd(string pCaption, string pFieldName, bool pReadOnly, bool pVisible, DATAGRIDCOLUMNTYPE pType)
        {
            elmUnitGridColumn vColumn = new elmUnitGridColumn();
            vColumn.__fCaption = pCaption;
            vColumn.__fField = pFieldName;
            vColumn.__fReadOnly = pReadOnly;
            vColumn.__fVisible = pVisible;
            vColumn.__fType = pType;
            fColumnsList.Add(vColumn);
        }
        /// <summary>
        /// Сброс значения
        /// </summary>
        public void __mValueClear()
        {
            fValue = 0;
            _cInput.Text = "";
            _cLabelValue.Text = "";
        }

        #endregion - Процедуры

        #endregion = МЕТОДЫ    

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Список колей по которым будет вестись поиск
        /// </summary>
        public ArrayList __fFieldsCharList = new ArrayList();
        /// <summary>
        /// Дополнительный фильтр
        /// </summary>
        public string __fFilterAdditional = "";
        /// <summary>
        /// Инверсия дополнительного фильтра
        /// </summary>
        public bool __fFilterAdditionalInversion = false;
        /// <summary>
        /// Название поля учетного кодо
        /// </summary>
        public string __fFieldCode = "";
        public FORMSELECTEDTYPES __fFormSelectledType = FORMSELECTEDTYPES.FormGrid;

        #endregion Атрибуты

        #region - Компоненты

        /// <summary>
        /// Поле ввода символьных данных
        /// </summary>
        protected elmComponentString _cInput = new elmComponentString();

        #endregion Компоненты

        #region - Объекты

        /// <summary>
        /// Форма для выбора записи
        /// </summary>
        public Type __oFormSelect;
        /// <summary>
        /// Сущность данных
        /// </summary>
        public datUnitEssence __oEssence;

        #endregion Объекты

        #region - Специальные

        /// <summary>
        /// Значение контрола
        /// </summary>
        private int fValue = 0;
        /// <summary>
        /// Список отображаемых колонок
        /// </summary>
        private List<elmUnitGridColumn> fColumnsList = new List<elmUnitGridColumn>();
        /// <summary>
        /// Вид надписи перед переходом в недоступный режим
        /// </summary>
        private LABELTYPES fLabelCaptionStatus = LABELTYPES.Normal;
        /// <summary>
        /// Заголовок формы для поиска без перевода
        /// </summary>
        private string fFormSearchCaptionNotTranslate = "";
        /// <summary>
        /// Заголовок формы для поиска
        /// </summary>
        private string fFormSearchCaption = "";

        #endregion Специальные

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Дотупность контрола
        /// </summary>
        public override bool __fEnabled_
        {
            get => base.__fEnabled_;
            set
            {
                base.__fEnabled_ = value;
                _cInput.Visible = value;
                if (value == true)
                {
                    _cLabelCaption.__fLabelType_ = fLabelCaptionStatus;
                }
                else
                {
                    _cLabelCaption.__fLabelType_ = LABELTYPES.Normal;
                    if (_cInput.Text.Length > 0)
                        _cLabelValue.Text = _cInput.Text;
                    else
                        _cLabelValue.Text = elmApplication.__oTunes.__mTranslate("нет данных");
                }
            }
        }
        /// <summary>
        /// Условие фильтра
        /// </summary>
        public override string __fFilterExpression_
        {
            get
            {
                string vReturn = ""; // Возвращаемое значение

                string vFieldNameWithPrefix = ""; // Название поля с префиксом таблицы
                if (__fTableAlias.Trim().Length > 0)
                { /// Добавление префикса таблицы если он указан
                    vFieldNameWithPrefix = __fTableAlias + "." + __fFieldName;
                }
                else
                { /// Префикс таблицы не указан
                    vFieldNameWithPrefix = __fFieldName;
                }

                vReturn = vFieldNameWithPrefix + "=" + fValue.ToString();

                return vReturn;
            }
        }
        /// <summary>
        /// Получение условия фильтра для отображения пользователю
        /// </summary>
        public override string __fFilterMessage_
        {
            get
            {
                string vReturn = ""; // Возвращаемое значение

                if (_cMark.Checked == true)
                {
                    vReturn = __fCaption_ + " = '" + _cInput.__fValue_ + "'";
                }

                return vReturn;
            }
        }
        /// <summary>
        /// Значение контрола
        /// </summary>
        public override object __fValue_
        {
            get { return fValue; }
            set
            {
                fValue = Convert.ToInt32(value);
                if (fValue > 0)
                {
                    DataTable vDataTable = __oEssence.__mRecord(fValue);
                    _cInput.Text = Convert.ToString(vDataTable.Rows[0]["dsi" + __oEssence.__fTableName]).Trim();
                    _cLabelValue.Text = _cInput.Text;
                }
                else
                    _cInput.Text = elmApplication.__oTunes.__mTranslate("нет данных");

            }
        }
        /// <summary>
        /// Заголовок формы для поиска
        /// </summary>
        /// <remarks>Отображаемый текст переводиться на язык интерфейса. Возвращается не переведенный текст</remarks>
        public string __fFormSearchCaption_
        {
            get { return fFormSearchCaptionNotTranslate; }
            set
            {
                fFormSearchCaptionNotTranslate = value;
                fFormSearchCaption = elmApplication.__oTunes.__mTranslate(value);
            }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        public event EventHandler __eOnInteractivatChange;

        #endregion = СОБЫТИЯ
    }
}
