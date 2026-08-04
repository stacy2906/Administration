using nlApplication;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmInputDateTime.cs
    /// </summary>
    /// <remarks>Класс-Поля ввода значений даты-времени</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 14-15</version> // Дата-время последней корректировки
    public class elmInputDateTime : elmInput
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
                _cLabelCaption.__eClickRight += _cLabelCaption___eMouseClickRight;
                _cLabelCaption.__eClickLeft += mLabelCaption_ClickLeft;

            }
            /// Сохранение установленного статуса надписи-заголовка
            fLabelCaptionStatus = _cLabelCaption.__fLabelType_;
            // _cInput
            {
                _cInput.Location = new System.Drawing.Point(0, 0);
                _cInput.__eChangedByUser += _cInput___eValueInteractiveChanged;
            }
            _cCalendar.DateSelected += _cCalendar_DateSelected;

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        private void _cCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            _cInput.__fValue_ = _cCalendar.__fValue_;
            //_cInput.SetDate(_cCalendar.__fValue_.Year, _cCalendar.__fValue_.Month, _cCalendar.__fValue_.Day);

        }

        #region Объект

        /// <summary>
        /// Выполняется при выборе надписи правой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cLabelCaption___eMouseClickRight(object sender, EventArgs e)
        {
            __mEmptyValue();
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

        /// <summary>
        /// Выполняется при выборе надписи левой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mLabelCaption_ClickLeft(object sender, EventArgs e)
        {
            // 1. Получаем абсолютные координаты верхнего левого угла кнопки
            Point vPoint = _cInput.PointToScreen(Point.Empty); // 1035, 242

            // 2. Переводим их в координаты рабочей области формы
            Point formPoint = FindForm().PointToClient(vPoint); // 625, 48

            // Теперь formPoint.X и formPoint.Y содержат координаты на форме
            //Console.WriteLine($"X: {formPoint.X}, Y: {formPoint.Y}");
            //-_cCalendar._mShowCalendar(FindForm().Left + this.Left + _cInput.Left, FindForm().Top + this.Top + this.Height);
            //+_cCalendar._mShowCalendar(formPoint.X, formPoint.Y);
            _cCalendar._mShowCalendar(vPoint.X - 200, vPoint.Y);
            //_cInput.Value = _cCalendar.__fValue_;
            //_cInput.Refresh();
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

        public void SetDateTime(int pYear, int pMonth, int pDay, int pHour, int pMinute, int pSecond)
        { 
            _cInput.SetDateTime(pYear, pMonth, pDay, pHour, pMinute, pSecond);
        }

        #endregion Процедуры

        #endregion = МЕТОДЫ  

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
        protected elmComponentDateTime _cInput = new elmComponentDateTime();
        /// <summary>
        /// Календарь
        /// </summary>
        protected elmComponentCalendar _cCalendar = new elmComponentCalendar();

        #endregion Компоненты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        ///// <summary>
        ///// Вид отображения даты - времени
        ///// </summary>
        //public DATETIMETYPES __fDateTimeType_
        //{
        //    get 
        //    { 
        //        return _cInput.__fDateTimeType_; 
        //    }
        //    set { _cInput.__fDateTimeType_ = value; }
        //}
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
            get 
            { 
                return _cInput.__fValue_; 
            }
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
