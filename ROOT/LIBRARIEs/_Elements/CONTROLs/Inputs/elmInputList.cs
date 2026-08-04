using nlApplication;
using nlData;
using System;
using System.Collections;
using System.Data;
using System.Drawing;

namespace nlElements
{
    /// <summary>
    /// Файл elmInputList.cs
    /// </summary>
    /// <remarks>Класс-поле ввода множества значений</remarks>
    public class elmInputList : elmInput
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

            Panel2.Controls.Add(_cInput);

            #endregion Размещение компонентов

            #region Настройка компонентов

            //if (Height < 50)
            //    Height = 50;
            //if (Height > 150)
            //    Height = 150;
            SizeChanged += ElmInputDatePeriod_SizeChanged;

            // _cLabel
            {
                _cLabelCaption.__fLabelType_ = LABELTYPES.Button;
                _cLabelCaption.__eClickLeft += _cLabelCaption___eMouseClickLeft;
                _cLabelCaption.__eClickRight += _cLabelCaption___eMouseClickRight;
            }
            // _cInput
            {
                _cInput.Location = new Point(0, 0);
                //_cInput.__eValueInteractiveChanged +
                //_cInput.Dock = DockStyle.Fill;
                _cInput.DoubleClick += _cInput_DoubleClick;
                _cInput.ThreeDCheckBoxes = false;
            }

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();
            _cInput.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        #region Объект

        /// <summary>
        /// Выполняется при двойном клике по записи в поле ввода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cInput_DoubleClick(object sender, EventArgs e)
        {
            if (__eInput_DoubleClick != null)
                __eInput_DoubleClick(this, new EventArgs());
        }

        private void _cLabelCaption___eMouseClickLeft(object sender, EventArgs e)
        {
            //crlForm vForm = FindForm() as crlForm;
            //if (vForm != null & __oFormSelect != null)
            //{
            //    crlFormRecord vFormSelect = (crlFormRecord)Activator.CreateInstance(__oFormSelect);
            //    /// Восстановить vFormFilter._cAreaFilter._fFormNameParent = vForm.Name;
            //    (vFormSelect as crlFormRecord).ShowDialog();
            //    //fValue = vFormSelect.__cAreaGrid.__pRecordClue;
            //    //DataTable vDataTable = __oEssence._mRecord(fValue);
            //    //_cInput.Text = Convert.ToString(vDataTable.Rows[0]["dsi" + __oEssence.__fTableName]).Trim();
            //    //_cLabelValue.Text = _cInput.Text;
            //    //if (__eOnInteractivatChange != null)
            //    //    __eOnInteractivatChange(this, new EventArgs());
            //}
            //else
            //{
            //    appError vError = new appError();
            //    vError.__fErrorsType = ERRORSTYPES.Programming;
            //    vError.__mMessageBuild("Форма для построения выбора значений из справочника не определена");
            //    vError.__fProcedure = _fClassNameFull + "__cLabelCaption__eMouseClickLeft(object, EventsArgs)";
            //    crlApplication.__oErrorsHandler.__mShow(vError);
            //}
            if (__eLabelCaption_MouseClickLeft != null)
                __eLabelCaption_MouseClickLeft(this, new EventArgs());

        }

        private void _cLabelCaption___eMouseClickRight(object sender, EventArgs e)
        {
            //if (crlApplication.__oMessages.__mShow(nlApplication.MESSAGESTYPES.Question, "Удалить {0}", __fEssenceObjectName, false, "", "") == DialogResult.Yes)
            //{ 
            //}
            if (__eLabelCaption_MouseClickRight != null)
                __eLabelCaption_MouseClickRight(this, new EventArgs());
        }

        #endregion Объект

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Обновление отображаемых данных
        /// </summary>
        public virtual void __mDataRefresh()
        {
            _cInput.__mDataRefresh();
        }
        /// <summary>
        /// Перевод фокуса на поле ввода
        /// </summary>
        public override void __mInputFocus()
        {
            _cInput.Focus();
        }
        /// <summary>
        /// Добавление значения в конец списка значений компонента
        /// </summary>
        /// <param name="pValue">Добавляемое значение</param>
        /// <returns>Идентификатор добавляемой записи, положительный - из таблицы, отрицательный - назначаемый компонентом</returns>
        /// <return>Индекс добавленнной записи</return>
        public virtual int __mItemAdd(string pValue, bool pChecked = false)
        {
            int vIndex = 0; // Возвращаемое значение
            vIndex = _cInput.__mItemAdd(pValue, pChecked);
            return vIndex;
        }
        /// <summary>
        /// Добавление значения в конец списка значений компонента
        /// </summary>
        /// <param name="pValue">Добавляемое значение</param>
        /// <returns>Идентификатор добавляемой записи, положительный - из таблицы, отрицательный - назначаемый компонентом</returns>
        public virtual int __mItemAdd(appUnitItem pItem)
        {
            return _cInput.__mItemAdd(pItem);
        }
        /// <summary>
        /// Изменение имени в уже отображаемом списке
        /// </summary>
        /// <param name="pClue">Идентификатор записи в котором нужно исправить название</param>
        /// <param name="pNameNew">Новое название</param>
        public virtual void __mItemChangeName(int pClue, string pNameNew)
        {
            _cInput.__mItemChangeName(pClue, pNameNew);
        }

        public virtual void __mItemRemove()
        {
            _cInput.__mItemRemove();
        }
        /// <summary>
        /// Добавление списка новых значений
        /// </summary>
        /// <param name="pValueS">Список значений в порядке определения индексов</param>
        /// <returns>[true] - Значение добавлено, иначе - [false]</returns>
        public virtual bool __mItemsAdd(ArrayList pValueS)
        {
            return _cInput.__mItemsAdd(pValueS);
        }
        /// <summary>
        /// Добавление списка новых значений
        /// </summary>
        /// <param name="pValueS">Список значений в порядке определения индексов</param>
        /// <returns>[true] - Значение добавлено, иначе - [false]</returns>
        public virtual bool __mItemsAdd(params string[] pValueS)
        {
            return _cInput.__mItemsAdd(pValueS);
        }
        /// <summary>
        /// Очистка всех данных и подготовка к вводу новых данных
        /// </summary>
        public virtual void __mItemsClear()
        {
            _cInput.__mItemsClear();
        }
        /// <summary>
        /// Загрузка данных из сущности данных
        /// </summary>
        /// <param name="pWhereExpression">Выражение выбора получаемых данных</param>
        /// <param name="pOrderExpression">Выражение сортировки получаемых данных</param>
        /// <returns>[true] - данные загружены, иначе - [false]</returns>
        public virtual bool __mItemsEssenceLoad(string pWhereExpression, string pOrderExpression)
        {
            return _cInput.__mItemsEssenceLoad(pWhereExpression, pOrderExpression);
        }
        /// <summary>
        /// Загрузка данных из {DataTable}, со столбцами clu(идентификатор) и dsi(название)
        /// </summary>
        /// <param name="pDataTable">таблица</param>
        /// <returns>[true] - данные загружены, иначе - [false]</returns>
        public virtual bool __mItemsLoad(DataTable pDataTable)
        {
            return _cInput.__mItemsLoad(pDataTable);
        }
        /// <summary>
        /// Получение индекса значения по идентификатору значения
        /// </summary>
        /// <param name="pClue">Идентификатор записи</param>
        /// <returns></returns>
        public virtual int __mGetIndexByClue(int pClue)
        {
            return _cInput.__mGetIndexByClue(pClue);
        }
        /// <summary>
        /// Получение значения метки выбора по индексу в списке
        /// </summary>
        /// <param name="pIndex"></param>
        /// <returns>[true] - строка выбрана, иначе - [false]</returns>
        public virtual bool __mGetMarkByIndex(int pIndex)
        { 
            return _cInput.__mGetMarkBuIndex(pIndex);
        }
        /// <summary>
        /// Возвращает идентификатор выбраной записи
        /// </summary>
        /// <returns>[int] - идентификатор выбраной записи</returns>
        public virtual int __mGetSelectedItemClue()
        {
            return _cInput.__mGetSelectedItemClue();
        }
        /// <summary>
        /// Возвращает статус выбора
        /// </summary>
        /// <returns></returns>
        public virtual bool __mGetSelectedItemMark()
        {
            return _cInput.__mGetSelectedItemMark();
        }

        //public string __mCheckedGet()
        //{
        //    return _cInput.__fCheckedValueList_;
        //}
        //public void __mCheckedSet(string vCheckedList)
        //{
        //    _cInput.__fCheckedValueList_ = vCheckedList;
        //}

        #endregion - Процедуры

        #endregion = МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Название объекта сущности
        /// </summary>
        //public string __fEssenceObjectName = "";
        /// <summary>
        /// Заголовок надписи
        /// </summary>
        public string __fFormSearchCaption = "";

        #endregion Атрибуты

        #region - Закрытые

        /// <summary>
        /// Максимальная ширина поля ввода
        /// </summary>
        private int fWidthMax = 100;

        #endregion Закрытые

        #region - Компоненты

        /// <summary>
        /// Поле ввода множественных значений
        /// </summary>
        protected elmComponentList _cInput = new elmComponentList();

        #endregion Компоненты

        #region - Объект

        /// <summary>
        /// Форма для выбора записи
        /// </summary>
        public Type __oFormSelect;

        #endregion Объект

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        public new string __fValue_
        {
            get
            {
                return _cInput.__fCheckedValueList_;
            }
            set
            {
                _cInput.__fCheckedValueList_ = value;
            }
        }
        /// <summary>
        /// Сущность данных
        /// </summary>
        public datUnitEssence __oEssence_
        {
            get { return _cInput.__oEssence; }
            set { _cInput.__oEssence = value; }
        }
        /// <summary>
        /// Условие фильтра для указанного поля
        /// </summary>
        public override string __fFilterExpression_
        {
            get
            {
                string vReturn = __oEssence_.__fTableAlias + "CLU In ("; // Возвращаемое значение
                /// Перебор загруженных записей
                for (int vAmount = 0; vAmount < _cInput.__fItemS.Count; vAmount++)
                {
                    if (_cInput.__fItemS[vAmount].__fCheck_ == true)
                    {
                        vReturn = vReturn.Trim();
                        if (vReturn.Substring(vReturn.Length - 1) != "(")
                            vReturn += ",";
                        vReturn = _cInput.__fItemS[vAmount].__fClue_.ToString();
                    }
                }
                appTypeString.__mSymbolsLastDelete(vReturn, ",");
                vReturn += ")";

                return vReturn;
            }
        }
        /// <summary>
        /// Выражение фильтра для указанного поля для отображения пользователю
        /// </summary>
        public override string __fFilterMessage_
        {
            get
            {
                string vReturn = ""; // Возвращаемое значение
                /// Перебор загруженных записей
                for (int vAmount = 0; vAmount < _cInput.__fItemS.Count; vAmount++)
                {
                    if (_cInput.__fItemS[vAmount].__fCheck_ == true)
                    {
                        vReturn = _cInput.__fItemS[vAmount].__fDesignation_ + ",";
                    }
                }
                appTypeString.__mSymbolsLastDelete(vReturn, ",");

                return vReturn;
            }
        }

        /// <summary>
        /// Максимальная ширина поля ввода
        /// </summary>
        public int __fWidthMax
        {
            get { return fWidthMax; }
            set
            {
                fWidthMax = value;
            }
        }

        private void ElmInputDatePeriod_SizeChanged(object sender, EventArgs e)
        {
            if (Width > 210 + fWidthMax)
                _cInput.Width = fWidthMax;
            if (Width < 210 + fWidthMax)
                _cInput.Width = Width - 210;
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        public event EventHandler __eLabelCaption_MouseClickLeft;
        public event EventHandler __eLabelCaption_MouseClickRight;
        /// <summary>
        /// Возникает при двойном клике по поллю ввода
        /// </summary>
        public event EventHandler __eInput_DoubleClick;

        #endregion = СОБЫТИЯ
    }
}
