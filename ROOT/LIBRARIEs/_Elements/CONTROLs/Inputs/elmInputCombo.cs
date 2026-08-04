using nlData;
using System;
using System.Collections;
using System.Data;
using System.Drawing;

namespace nlElements
{
    /// <summary>
    /// Файл elmInputCombo.cs
    /// </summary>
    /// <remarks>Класс-поля ввода значений из выпадающего списка</remarks>
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.02.19 10-53</version> // Дата-время последней корректировки
    /* Пример использования
            - Заполнение в ручную
                _cInputCombo.Location = new System.Drawing.Point(10, 70);
                _cInputCombo.__fCaption_ = "Выпадающие значения";
                _cInputCombo.__mCaptionBuilding("Выпадающие значения {0}", 2);
                _cInputCombo.__fFillType_ = FILLTYPES.Necessarily;
                _cInputCombo.__fPromptCaption_ = "Выпадающие значения";
                _cInputCombo.__mItemAdd("Раз");
                _cInputCombo.__mItemAdd("Два");
                _cInputCombo.__mDataRefresh();
                _cInputCombo.__fDropDownHeight_ = 100;
                _cInputCombo.__fDropDownWidth_ = 40;
            - Заполнение из базы данных
                _cInputCombo.__oEssence_ = new rtlEssenceUsr(); 
                _cInputCombo.__mItemsEssenceLoad("CLU != 0", "dsiUsr");
                _cInputCombo.__mDataRefresh();
    */
    public class elmInputCombo : elmInput
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

            Panel2.Controls.Add(_cInput);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            // _cLabelCaption
            {
                _cLabelCaption.__fLabelType_ = LABELTYPES.Normal; /// Назначение вида надписи-заголовка - 'Надпись' 
                _cLabelCaption.__eClickRight += mCaption_ClickRight;
            }
            fLabelCaptionStatus = _cLabelCaption.__fLabelType_; /// Сохранение установленного статуса надписи-заголовка
            // _cInput
            {
                _cInput.Location = new Point(0, 0);
                _cInput.__eChanged += mInput_Changed;
                _cInput.__eChangedByProgram += mInput_ValueChangedByProgram;
                _cInput.__eChangedByUserAfter += mInput_ValueChangedByUserAfter;
                _cInput.__eChangedByUserBefore += mInput_eValueChangedByUserBefore;
            }

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется при выборе надписи правой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mCaption_ClickRight(object sender, EventArgs e)
        {
            _cInput.Text = ""; /// Очищается название искомого справочника
            __fMarkStatus_ = false; /// Выключается использование фильтра
            __fValue_ = 0;
            _cInput.Focus(); /// Перемещается курсор в поле ввода
        }
        /// <summary>
        /// Выполняется при любом изменении данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mInput_Changed(object sender, EventArgs e)
        {
            if (__eChanged != null)
                __eChanged(this, e);
        }
        /// <summary>
        /// Выполняется при ручным изменении введенных данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mInput_ValueChangedByUserAfter(object sender, EventArgs e)
        {
            __fMarkStatus_ = true; // Включение использования фильтра
            if (__eChangedByUserAfter != null)
                __eChangedByUserAfter(this, e);
            if (__eChanged != null)
                __eChanged(this, e);
        }
        /// <summary>
        /// Выполняется перед ручным изменении введенных данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mInput_eValueChangedByUserBefore(object sender, EventArgs e)
        {
            if (__eChangedByUserBefore != null)
                __eChangedByUserBefore(this, e);
        }
        /// <summary>
        /// Выполняется при программном изменении введенных данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mInput_ValueChangedByProgram(object sender, EventArgs e)
        {
            if (__eChangedByProgram != null)
                __eChangedByProgram(this, e);
            if (__eChanged != null)
                __eChanged(this, e);
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Обновление отображаемых данных
        /// </summary>
        public virtual void __mDataRefresh()
        {
            _cInput.__mDataRefresh();
            _cLabelValue.Text = _cInput.Text;

            return;
        }
        /// <summary>
        /// Перевод фокуса на поле ввода
        /// </summary>
        public override void __mInputFocus()
        {
            _cInput.Focus();

            return;
        }
        /// <summary>
        /// Добавление значения в конец списка значений компонента
        /// </summary>
        /// <param name="pValue">Добавляемое значение</param>
        /// <returns>Название переводиться на язык интерфейса. Идентификатор добавляемой записи, положительный - из таблицы, отрицательный - назначаемый компонентом</returns>
        public virtual int __mItemAdd(string pValue)
        {
            return _cInput.__mItemAdd(pValue);
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
        public bool __mItemsAddFromList(string pValueS)
        {
            return _cInput.__mItemsAddFromList(pValueS);
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
            return _cInput.__mItemsLoadFromDataTable(pDataTable);
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

        public virtual int __mGetSelectedIndex()
        {
            return _cInput.SelectedIndex;
        }
        public virtual string __mGetSelectedText()
        {
            return _cInput.Text;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ    

        #region = ПОЛЯ

        #region - Закрытые

        /// <summary>
        /// Вид надписи перед переходом в недоступный режим
        /// </summary>
        private LABELTYPES fLabelCaptionStatus = LABELTYPES.Normal;

        #endregion - Закрытые

        #region - Компоненты

        /// <summary>
        /// Поле выбора данных из выпадающего списка
        /// </summary>
        protected elmComponentCombo _cInput = new elmComponentCombo();

        #endregion Компоненты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Доступность контрола
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
                    if (_cInput.__fValueToString_.Length > 0)
                        _cLabelValue.Text = _cInput.__fValueToString_;
                    else
                        _cLabelValue.Text = elmApplication.__oTunes.__mTranslate("нет данных");
                }
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
        /// Построение условия фильтра
        /// </summary>
        public override string __fFilterExpression_
        {
            get
            {
                string vReturn = ""; // Возвращаемое значение

                if (__fTableAlias.Length > 0)
                    vReturn = __fTableAlias + ".";
                vReturn = vReturn + __fFieldName + " = " + Convert.ToInt32(__fValue_).ToString();

                return vReturn;
            }
        }
        /// <summary>
        /// Поучение условия фильтра для отображения пользователю
        /// </summary>
        public override string __fFilterMessage_
        {
            get
            {
                string vReturn = ""; // Возвращаемое значение

                vReturn = vReturn + __fCaption_.Trim() + " = '" + _cInput.__fValueToString_ + "'";

                return vReturn;
            }
        }
        /// <summary>
        /// Значение
        /// </summary>
        public override object __fValue_
        {
            get
            {
                return _cInput.__fValue_;
            }
            set
            {
                _cInput.__fValue_ = Convert.ToInt32(value);
            }
        }
        /// <summary>
        /// Название значения контрола
        /// </summary>
        public override string __fValueToText_
        {
            get { return _cInput.__fValueToString_; }
        }
        /// <summary>
        /// Высота выпадающего списка
        /// </summary>
        public int __fDropDownHeight_
        {
            get { return _cInput.DropDownHeight; }
            set { _cInput.DropDownHeight = value; }
        }
        /// <summary>
        /// Ширина выпадающего списка
        /// </summary>
        public int __fDropDownWidth_
        {
            get { return _cInput.DropDownWidth; }
            set { _cInput.DropDownWidth = value; }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при изменении данных в поле ввода
        /// </summary>
        public event EventHandler __eChanged;
        /// <summary>
        /// Возникает при изменении данных в поле ввода программой
        /// </summary>
        public event EventHandler __eChangedByProgram;
        /// <summary>
        /// Возникает при изменении данных в поле ввода пользователем
        /// </summary>
        public event EventHandler __eChangedByUserAfter;
        /// <summary>
        /// Возникает перед изменением данных в поле ввода пользователем
        /// </summary>
        public event EventHandler __eChangedByUserBefore;

        #endregion СОБЫТИЯ
    }
}
