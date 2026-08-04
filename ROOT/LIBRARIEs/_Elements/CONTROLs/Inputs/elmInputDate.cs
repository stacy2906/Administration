using nlApplication;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmInputDate.cs
    /// </summary>
    public class elmInputDate : elmInput
    {
        #region = ДИЗАЙНЕР

        /// <summary>
        /// Загрузка контрола
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
                /// Назначение вида надписи-заголовка - 'Надпись' 
                _cLabelCaption.__fLabelType_ = LABELTYPES.Button;
                _cLabelCaption.__eClickRight += mCaption_ClickRight;
                _cLabelCaption.__eClickLeft += mCaption_ClickLeft;
            }
            /// Сохранение установленного статуса надписи-заголовка
            fLabelCaptionStatus = _cLabelCaption.__fLabelType_;
            // _cInput
            {
                _cInput.Location = new System.Drawing.Point(0, 0);
            }
            _cCalendar.DateSelected += _cCalendar_DateSelected;

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }

        private void _cCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            _cInput.__fValue_ = _cCalendar.__fValue_;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется при выборе надписи левой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mCaption_ClickLeft(object sender, EventArgs e)
        {
            _cCalendar._mShowCalendar(FindForm().Left + this.Left + _cInput.Left, FindForm().Top + this.Top + this.Height);
        }
        /// <summary>
        /// Выполняется при выборе надписи правой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mCaption_ClickRight(object sender, EventArgs e)
        {
            __mEmptyValue();
        }
        /// <summary>
        /// Выполняется при изменении введенных данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mInput_ValueInteractiveChanged(object sender, EventArgs e)
        {
            __fMarkStatus_ = true;  /// Включение использования фильтра
            if (__eInputChangedByUser != null)
                __eInputChangedByUser(_cInput, new EventArgs());
        }

        #endregion - Поведение

        #region - Процедуры

        /// <summary>
        /// Установка пустого значения
        /// </summary>
        public void __mEmptyValue()
        {
            _cInput.Text = ""; /// Очищается название искомого справочника
            __fMarkStatus_ = false; /// !!! Выключается использование фильтра
            __fValue_ = appTypeDateTime.__mMsSqlDateEmpty(); /// Установка пустого значчения даты-времени
            _cInput.Focus(); /// Перемещается курсор в поле ввода
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Закрытые

        /// <summary>
        /// Вид надписи перед переходом в недоступный режим
        /// </summary>
        private LABELTYPES fLabelCaptionStatus = LABELTYPES.Normal;

        #endregion Закрытые

        #region - Компоненты

        /// <summary>
        /// Поле ввода даты
        /// </summary>
        protected elmComponentDate _cInput = new elmComponentDate();
        /// <summary>
        /// Календарь
        /// </summary>
        protected elmComponentCalendar _cCalendar = new elmComponentCalendar();

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
                    //if (_cInput.Value != appTypeDateTime.__mMsSqlDateEmpty())
                    //    _cLabelValue.Text = _cInput.Value.ToString();
                    //else
                    //    _cLabelValue.Text = elmApplication.__oTunes.__mTranslate("нет данных");
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
        /// Условие фильтра для указанного поля
        /// </summary>
        public override string __fFilterExpression_
        {
            get
            {
                return __fFieldName + " = (CONVERT([datetime],'" + __fValue_ + "'))";
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
                //if (__fDateTimeType_ == DATETIMETYPES.DateTime)
                //    vReturn = _cLabelCaption.Text.Trim() + " = " + __fValue_.ToString();
                //else
                //    vReturn = _cLabelCaption.Text.Trim() + " = " + __fValue_.ToString().Substring(0, 10);

                return vReturn;
            }
        }

        /// <summary>
        /// Значение контрола
        /// </summary>
        public override object __fValue_
        {
            get { return _cInput.__fValue_; }
            set
            {
                _cInput.__fValue_ = Convert.ToDateTime(value);
                _cLabelValue.Text = _cInput.__fValue_.ToString();  // Запись значения по умолчанию
            }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при изменении данных в поле ввода пользователем
        /// </summary>
        public event EventHandler __eInputChangedByUser;

        #endregion СОБЫТИЯ

    }
}
