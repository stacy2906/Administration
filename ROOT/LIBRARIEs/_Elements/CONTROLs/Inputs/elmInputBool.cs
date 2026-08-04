using System;
using System.Drawing;

namespace nlElements
{
    /// <summary>
    /// Файл elmInputBool.cs
    /// </summary>
    /// <remarks>Класс-Поле ввода логических значений</remarks>
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 14-08</version> // Дата-время последней корректировки
    public class elmInputBool : elmInput
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
                _cLabelCaption.__fLabelType_ = LABELTYPES.Normal; // Назначение вида надписи-заголовка - 'Надпись' 
                fLabelCaptionStatus = _cLabelCaption.__fLabelType_; // Сохранение установленного статуса надписи-заголовка
                _cLabelCaption.__eClickRight += mCaption_ClickRight;
            }
            // _cInput
            {
                _cInput.Location = new Point(0, 0);
                _cInput.__fComboType_ = COMBOTYPES.Bool;
                _cInput.__eChangedByProgram += mInput_ChangedByUser;
                _cInput.__eChangedByUserAfter += mInput_ChangedByProgram;
                _cInput.DropDownHeight = 40;
                _cInput.DropDownWidth = 20;
            }

            #endregion Настойка компонентов

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
            __fMarkStatus_ = false; /// !!! Выключается использование фильтра
            __fValue_ = 0;
            _cInput.Focus(); /// Перемещается курсор в поле ввода

            return;
        }
        /// <summary>
        /// Выполняется при изменении введенных данных пользователем
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mInput_ChangedByUser(object sender, EventArgs e)
        {
            __fMarkStatus_ = true; // Включение использования фильтра
            if (__eChangedByUser != null)
                __eChangedByUser(_cInput, e);
            if (__eChanged != null)
                __eChanged(sender, e);

            return;
        }
        /// <summary>
        /// Выполняется при изменении введенных данных программно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mInput_ChangedByProgram(object sender, EventArgs e)
        {
            if (__eChangedByProgram != null)
                __eChangedByProgram(_cInput, e);
            if (__eChanged != null)
                __eChanged(sender, e);

            return;
        }

        #endregion Поведение

        #endregion МЕТОДЫ    

        #region = ПОЛЯ

        #region - Внутренние

        /// <summary>
        /// Вид надписи перед переходом в недоступный режим
        /// </summary>
        private LABELTYPES fLabelCaptionStatus = LABELTYPES.Normal;

        #endregion Внутренние

        #region - Компоненты

        /// <summary>
        /// Поле ввода логических данных
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
            get { return base.__fEnabled_; }
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
        /// Обязательность заполнения
        /// </summary>
        public override FILLTYPES __fFillType_
        {
            get { return _cInput.__fFillType_; }
            set { _cInput.__fFillType_ = value; }
        }
        /// <summary>
        /// Условие фильтра
        /// </summary>
        public override string __fFilterExpression_
        {
            get
            {
                string vReturn = ""; // Возвращаемое значение

                if (__fTableAlias.Length > 0)
                    vReturn = __fTableAlias + ".";
                vReturn = vReturn + __fFieldName + " = " + Convert.ToInt32(__fValue_);

                return vReturn;
            }

        }
        /// <summary>
        /// Выражение фильтра для отображения пользователю
        /// </summary>
        public override string __fFilterMessage_
        {
            get
            {
                string vReturn = ""; // Возвращаемое значение

                vReturn = vReturn + __fCaption_.Trim() + " = " + (Convert.ToBoolean(__fValue_) == false ? elmApplication.__oTunes.__mTranslate("Нет") : elmApplication.__oTunes.__mTranslate("Да"));

                return vReturn;
            }
        }
        /// <summary>
        /// Значение контрола
        /// </summary>
        public override object __fValue_
        {
            get { return Convert.ToBoolean(_cInput.SelectedIndex); }
            set
            {
                try
                {
                    if (Convert.ToBoolean(value) != true)
                        _cInput.SelectedIndex = 0;
                    else
                    {
                        _cInput.SelectedIndex = 1;
                        if(__fEnabled_ ==  false)
                            _cLabelValue.ForeColor = Color.Red;
                    }
                }
                catch
                {
                    _cInput.SelectedIndex = 0;
                }

                _cLabelValue.Text = _cInput.Text; // Запись значения по умолчанию
            }
        }
        /// <summary>
        /// Текст соответсвующий значению
        /// </summary>
        public override string __fValueToText_
        {
            get { return _cInput.__fValueToString_; }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при изменении данных
        /// </summary>
        public event EventHandler __eChanged;
        /// <summary>
        /// Возникает при изменении данных программой
        /// </summary>
        public event EventHandler __eChangedByProgram;
        /// <summary>
        /// Возникает при изменении данных пользователем
        /// </summary>
        public event EventHandler __eChangedByUser;

        #endregion СОБЫТИЯ    
    }
}
