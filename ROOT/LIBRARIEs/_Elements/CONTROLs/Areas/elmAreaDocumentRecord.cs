using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace nlElements
{
    public class elmAreaDocumentRecord : elmArea
    {
        #region = ДИЗАЙНЕРЫ

        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Размещение компонентов

            _cToolBar.Items.Insert(0, _cButtonSave);
            Panel2.Controls.Add(_cBlockInputs);
            Panel2.Controls.SetChildIndex(_cBlockInputs, 0);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            // _cButtonSave
            {
                _cButtonSave.Click += mButtonSave_Click;
                _cButtonSave.Image = global::nlResourcesImages.Properties.Resources._Computer_Diskette_b32;
                _cButtonSave.ToolTipText = "[ Ctrl + A ]\n" + elmApplication.__oTunes.__mTranslate("Применить");
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
        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется при выборе кнопки 'Сохранить'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mButtonSave_Click(object sender, EventArgs e)
        {
            __mDataSave();
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Добавление поля ввода на панель полей ввода
        /// </summary>
        /// <param name="pInput"></param>
        public bool __mInputAdd(elmInput pInput, int pHeight = 25)
        {
            return _cBlockInputs.__mInputAdd(pInput, pHeight);
        }
        public bool __mDataLoad()
        {
            bool vReturn = true; // Возвращаемое значение

            /// Перебор установленных компонентов фильтра
            foreach (Control vInput in _cBlockInputs.Controls)
            {
                /// Компонент - поле ввода
                if ((vInput is elmInput) == true)
                {
                    string vFieldName = (vInput as elmInput).__fFieldName; // Название поля для которого строиться фильтр

                    try
                    {
                        if (vInput is elmInputBool)
                            (vInput as elmInputBool).__fValue_ = Convert.ToBoolean(__oDataRow[vFieldName]);
                        if (vInput is elmInputCombo)
                            (vInput as elmInputCombo).__fValue_ = Convert.ToInt32(__oDataRow[vFieldName]);
                        if (vInput is elmInputDateTime)
                            (vInput as elmInputDateTime).__fValue_ = Convert.ToDateTime(__oDataRow[vFieldName]);
                        if (vInput is elmInputFormCode)
                            (vInput as elmInputFormCode).__fValue_ = Convert.ToInt32(__oDataRow[vFieldName]);
                        if (vInput is elmInputFormName)
                            (vInput as elmInputFormName).__fValue_ = Convert.ToInt32(__oDataRow[vFieldName]);
                        if (vInput is elmInputNumeric)
                            (vInput as elmInputNumeric).__fValue_ = Convert.ToDecimal(__oDataRow[vFieldName]);
                        if (vInput is elmInputInteger)
                            (vInput as elmInputInteger).__fValue_ = Convert.ToInt32(__oDataRow[vFieldName]);
                        if (vInput is elmInputPhone)
                            (vInput as elmInputPhone).__fValue_ = __oDataRow[vFieldName].ToString();
                        if (vInput is elmInputString)
                            (vInput as elmInputString).__fValue_ = __oDataRow[vFieldName].ToString();
                        if (vInput is elmInputQuote)
                            (vInput as elmInputQuote).__fValue_ = __oDataRow[vFieldName].ToString();
                    }
                    catch
                    {
                        (vInput as elmInput).__fMarkStatus_ = false; /// Первая загрузка статуса
                    }
                }
            }

            return vReturn;
        }
        public bool __mDataSave()
        {
            bool vReturn = true;

            /// Перебор установленных компонентов фильтра
            foreach (Control vInput in _cBlockInputs.Controls)
            {
                /// Компонент - поле ввода
                if ((vInput is elmInput) == true)
                {
                    string vFieldName = (vInput as elmInput).__fFieldName; // Название поля для которого строиться фильтр

                    try
                    {
                        if (vInput is elmInputBool)
                            __oDataRow[vFieldName] = Convert.ToBoolean((vInput as elmInputBool).__fValue_);
                        if (vInput is elmInputCombo)
                            __oDataRow[vFieldName] = Convert.ToInt32((vInput as elmInputCombo).__fValue_);
                        if (vInput is elmInputDateTime)
                            __oDataRow[vFieldName] = Convert.ToDateTime((vInput as elmInputDateTime).__fValue_);
                        if (vInput is elmInputFormCode)
                            __oDataRow[vFieldName] = Convert.ToInt32((vInput as elmInputFormCode).__fValue_);
                        if (vInput is elmInputFormName)
                            __oDataRow[vFieldName] = Convert.ToInt32((vInput as elmInputFormName).__fValue_);
                        if (vInput is elmInputNumeric)
                            __oDataRow[vFieldName] = Convert.ToDecimal((vInput as elmInputNumeric).__fValue_);
                        if (vInput is elmInputInteger)
                            __oDataRow[vFieldName] = Convert.ToInt32((vInput as elmInputInteger).__fValue_);
                        if (vInput is elmInputPhone)
                            __oDataRow[vFieldName] = (vInput as elmInputPhone).__fValue_.ToString();
                        if (vInput is elmInputString)
                            __oDataRow[vFieldName] = (vInput as elmInputString).__fValue_.ToString();
                        if (vInput is elmInputQuote)
                            __oDataRow[vFieldName] = (vInput as elmInputQuote).__fValue_.ToString();
                    } catch{}
                }
            }
            __fRecordSaved = true;

            FindForm().Close();

            return vReturn;
        }
        //public bool __mDataLoadForEdit()
        //{
        //    bool vReturn = true;

        //    /// Перебор установленных компонентов фильтра
        //    foreach (Control vInput in _cBlockInputs.Controls)
        //    {
        //        /// Компонент - поле ввода
        //        if ((vInput is elmInput) == true)
        //        {
        //            string vFieldName = (vInput as elmInput).__fFieldName; // Название поля для которого строиться фильтр

        //            try
        //            {
        //                if (vInput is elmInputBool)
        //                    (vInput as elmInputBool).__fValue_ = Convert.ToBoolean(__oDataRow[vFieldName]);
        //                if (vInput is elmInputCombo)
        //                    (vInput as elmInputCombo).__fValue_ = Convert.ToInt32(__oDataRow[vFieldName]);
        //                if (vInput is elmInputDateTime)
        //                    (vInput as elmInputDateTime).__fValue_ = Convert.ToDateTime(__oDataRow[vFieldName]);
        //                if (vInput is elmInputFormCode)
        //                    (vInput as elmInputFormCode).__fValue_ = Convert.ToInt32(__oDataRow[vFieldName]);
        //                if (vInput is elmInputFormName)
        //                    (vInput as elmInputFormName).__fValue_ = Convert.ToInt32(__oDataRow[vFieldName]);
        //                if (vInput is elmInputNumeric)
        //                    (vInput as elmInputNumeric).__fValue_ = Convert.ToDecimal(__oDataRow[vFieldName]);
        //                if (vInput is elmInputInteger)
        //                    (vInput as elmInputInteger).__fValue_ = Convert.ToInt32(__oDataRow[vFieldName]);
        //                if (vInput is elmInputPhone)
        //                    (vInput as elmInputPhone).__fValue_ = __oDataRow[vFieldName].ToString();
        //                if (vInput is elmInputString)
        //                    (vInput as elmInputString).__fValue_ = __oDataRow[vFieldName].ToString();
        //                if (vInput is elmInputQuote)
        //                    (vInput as elmInputQuote).__fValue_ = __oDataRow[vFieldName].ToString();
        //            }
        //            catch
        //            {
        //                (vInput as elmInput).__fMarkStatus_ = false; /// Первая загрузка статуса
        //            }
        //        }
        //    }

        //    return vReturn;
        //    return vReturn;
        //}

        #endregion Процедуры

        #endregion МЕТОДЫ

        /// <summary>
        /// Разрешение отображения галочки во всех добавляемых полях ввода
        /// </summary>
        public bool __fBlockInputsCheckShow_
        {
            get { return _cBlockInputs.__fMarkShow; }
            set { _cBlockInputs.__fMarkShow = value; }
        }

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Данные условно сохранены
        /// </summary>
        public bool __fRecordSaved = false;

        #endregion Атрибуты 

        #region - Компоненты

        /// <summary>
        /// Кнопка 'Сохранить'
        /// </summary>
        protected elmComponentToolbarButton _cButtonSave = new elmComponentToolbarButton();
        /// <summary>
        /// Блок для отображения полей ввода
        /// </summary>
        protected elmBlockInputs _cBlockInputs = new elmBlockInputs();
        
        #endregion Компоненты

        #region - Объекты

        public DataRow __oDataRow;

        #endregion Объекты

        #endregion ПОЛЯ
    }
}
