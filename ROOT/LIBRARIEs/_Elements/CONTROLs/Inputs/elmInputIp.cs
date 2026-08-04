using nlApplication;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmInputIp.cs
    /// </summary>
    /// <remarks>Класс-поле ввода IP адреса</remarks>
    public class elmInputIp : elmInput
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
                _cLabelCaption.__eClickRight += _cLabelCaption___eMouseClickRight;
            }
            /// Сохранение установленного статуса надписи-заголовка
            fLabelCaptionStatus = _cLabelCaption.__fLabelType_;
            // _cInput
            {
                _cInput.Location = new System.Drawing.Point(0, 0);
                //_cInput.__eChangedByUser += _cInput___eValueInteractiveChanged;
                _cInput.Mask = "###.###.###.###";
                _cInput.__fSymbolsCount_ = 15;
            }

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        #region Объект

        /// <summary>
        /// Выполняется при выборе надписи правой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cLabelCaption___eMouseClickRight(object sender, EventArgs e)
        {
            // __mEmptyValue();
        }
        /// <summary>
        /// Выполняется при изменении введенных данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cInput___eValueInteractiveChanged(object sender, EventArgs e)
        {
            __fMarkStatus_ = true;  /// Включение использования фильтра
            if (__eInputChangedByUser != null)
                __eInputChangedByUser(_cInput, new EventArgs());
        }

        #endregion Объект

        #endregion - Поведение

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
        protected elmComponentMask _cInput = new elmComponentMask();

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
            get { return _cInput.Text; }
            set
            {
                _cInput.Text = value.ToString();
                _cLabelValue.Text = _cInput.Text.ToString();  // Запись значения по умолчанию
            }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при изменении данных в поле ввода пользователем
        /// </summary>
        public event EventHandler __eInputChangedByUser;
        /// <summary>
        /// Количество отображаемых символов данных
        /// </summary>
        public virtual int __fSymbolsCount_
        {
            get { return _cInput.__fSymbolsCount_; }
            set { _cInput.__fSymbolsCount_ = value; }
        }

        #endregion СОБЫТИЯ

    }
}
