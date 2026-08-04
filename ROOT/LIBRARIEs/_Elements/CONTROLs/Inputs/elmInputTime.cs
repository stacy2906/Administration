using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmInputTime.cs
    /// </summary>
    /// <remarks>Класс-поле ввода времени</remarks>
    public class elmInputTime : elmInput
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
                _cLabelCaption.__fLabelType_ = LABELTYPES.Normal;
                _cLabelCaption.__eClickRight += mCaption_ClickRight;
            }
            /// Сохранение установленного статуса надписи-заголовка
            fLabelCaptionStatus = _cLabelCaption.__fLabelType_;
            // _cInput
            {
                _cInput.Location = new System.Drawing.Point(0, 0);
                //_cInput.__eChangedByUser += _cInput___eValueInteractiveChanged;
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
            //__mEmptyValue();
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

        #endregion Поведение

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Внутренние

        /// <summary>
        /// Вид надписи перед переходом в недоступный режим
        /// </summary>
        private LABELTYPES fLabelCaptionStatus = LABELTYPES.Normal;

        #endregion - Внутренние

        #region - Компоненты

        /// <summary>
        /// Поле ввода даты-времени
        /// </summary>
        protected elmComponentTime _cInput = new elmComponentTime();

        #endregion Компоненты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        ///// <summary>
        ///// Вид отображения даты - времени
        ///// </summary>
        //public DATETIMETYPES __fDateTimeType_
        //{
        //    get { return _cInput.__fDateTimeType_; }
        //    set { _cInput.__fDateTimeType_ = value; }
        //}
        
        /// <summary>
        /// Получает или устанавливает AM/PM
        /// </summary>
        public string __fAMPM_
        {
            get
            {
                return _cInput.__fAMPM_;
            }
        }
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
        /// Получает или устанавливает формат отображения времени (12/24)
        /// </summary>
        public bool __fFormat_
        {
            get
            {
                return _cInput.__fFormat_;
            }
            set
            {
                _cInput.__fFormat_ = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает значение часа
        /// </summary>
        public int __fHour_
        {
            get
            {
                return _cInput.__fHour_;
            }
            set
            {
                _cInput.__fHour_ = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает значение минут
        /// </summary>
        public int __fMinute_
        {
            get
            {
                return _cInput.__fMinute_;
            }
            set
            {
                _cInput.__fMinute_ = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает значение секунд
        /// </summary>
        public int __fSecond_
        {
            get
            {
                return _cInput.__fSecond_;
            }
            set
            {
                _cInput.__fSecond_ = value;
            }
        }
        /// <summary>
        /// Получает или устанавливает символ разделителя часов, минут и секунд
        /// </summary>
        public char __fSymbolSeparator_
        {
            get
            {
                return _cInput.__fSymbolSeparator_;
            }
            set
            {
                _cInput.__fSymbolSeparator_ = value;
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
