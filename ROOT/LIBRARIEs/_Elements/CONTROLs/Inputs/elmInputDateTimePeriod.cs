using nlApplication;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmInputDateTimePeriod.cs
    /// </summary>
    /// <remarks>Класс-поле ввода значений периода даты-времени</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 14-35</version> // Дата-время последней корректировки
    public class elmInputDateTimePeriod : elmInput
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Загрузка контрола
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region Размещение компонентов

            Panel2.Controls.Add(_cInput);
            Panel2.Controls.Add(_cInputTo);

            #endregion Размещение компонентов

            #region Настройка компонентов

            Height = 50;

            // _cLabelCaption
            {
                _cLabelCaption.__fLabelType_ = LABELTYPES.Button;
                _cLabelCaption.__eClickRight += mLabelCaption_MouseClickRight;
                _cLabelCaption.__eClickLeft += mLabelCaption_MouseClickLeft;
            }
            fLabelCaptionStatus = _cLabelCaption.__fLabelType_; /// Сохранение установленного статуса надписи-заголовка
            // _cInput
            {
                _cInput.Location = new Point(0, 0);
                _cInput.__eChangedByUser += mInput_ValueInteractiveChanged;
            }
            // _cInputTo
            {
                _cInputTo.Location = new Point(0, 25);
                _cInputTo.__eChangedByUser += mInput_ValueInteractiveChanged;
                _cInputTo.Validating += mInputTo_Validating;
            }
            _cInput.__fValue_ = appTypeDateTime.__mDayBegin(DateTime.Now);
            _cInputTo.__fValue_ = appTypeDateTime.__mDayEnd(DateTime.Now);

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        #region Контекстное меню

        /// <summary>
        /// Выполняется при клике левой кнопки мыши по надписи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mLabelCaption_MouseClickLeft(object sender, System.EventArgs e)
        {
            MenuItem[] vMenuItemCurrentS = new MenuItem[4];
            vMenuItemCurrentS[0] = new MenuItem(elmApplication.__oTunes.__mTranslate("Неделя"), MenuItemCurrent_Click);
            vMenuItemCurrentS[1] = new MenuItem(elmApplication.__oTunes.__mTranslate("Месяц"), MenuItemCurrent_Click);
            vMenuItemCurrentS[2] = new MenuItem(elmApplication.__oTunes.__mTranslate("Квартал"), MenuItemCurrent_Click);
            vMenuItemCurrentS[3] = new MenuItem(elmApplication.__oTunes.__mTranslate("Год"), MenuItemCurrent_Click);

            MenuItem[] vMenuItemBeforeS = new MenuItem[4];
            vMenuItemBeforeS[0] = new MenuItem(elmApplication.__oTunes.__mTranslate("Неделя"), MenuItemBefore_Click);
            vMenuItemBeforeS[1] = new MenuItem(elmApplication.__oTunes.__mTranslate("Месяц"), MenuItemBefore_Click);
            vMenuItemBeforeS[2] = new MenuItem(elmApplication.__oTunes.__mTranslate("Квартал"), MenuItemBefore_Click);
            vMenuItemBeforeS[3] = new MenuItem(elmApplication.__oTunes.__mTranslate("Год"), MenuItemBefore_Click);

            MenuItem[] vMenuItemS = new MenuItem[4];

            vMenuItemS[0] = new MenuItem(elmApplication.__oTunes.__mTranslate("Текущая"), vMenuItemCurrentS);
            vMenuItemS[1] = new MenuItem(elmApplication.__oTunes.__mTranslate("Предыдущая"), vMenuItemBeforeS);
            vMenuItemS[2] = new MenuItem(elmApplication.__oTunes.__mTranslate("С начала"), MenuItemEmptyStart_Click);
            vMenuItemS[3] = new MenuItem(elmApplication.__oTunes.__mTranslate("Пустой период"), MenuItemEmpty_Click);

            ContextMenu vContextMenu = new ContextMenu(vMenuItemS);
            vContextMenu.Show(_cLabelCaption, new Point(0, 0));

            return;
        }
        /// <summary>
        /// Выполняется при клике правой кнопки мыши по надписи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mLabelCaption_MouseClickRight(object sender, EventArgs e)
        {
            _cInput.__fValue_ = appTypeDateTime.__mDayBegin(DateTime.Now);
            _cInputTo.__fValue_ = appTypeDateTime.__mDayEnd(DateTime.Now.AddDays(1));
        }

        /// <summary>
        /// Период текущиц
        /// </summary>
        private void MenuItemCurrent_Click(object sender, EventArgs e)
        {
            MenuItem vMenuItem = sender as MenuItem;

            switch (vMenuItem.Index)
            {
                case 0:
                    _cInput.__fValue_ = appTypeDateTime.__mWeekBegin(appTypeDateTime.__mDayBegin(DateTime.Now));
                    _cInputTo.__fValue_ = appTypeDateTime.__mWeekEnd(appTypeDateTime.__mDayBegin(DateTime.Now));
                    break;
                case 1:
                    _cInput.__fValue_ = appTypeDateTime.__mMonthBegin(appTypeDateTime.__mDayBegin(DateTime.Now));
                    _cInputTo.__fValue_ = appTypeDateTime.__mMonthEnd(appTypeDateTime.__mDayBegin(DateTime.Now));
                    break;
                case 2:
                    _cInput.__fValue_ = appTypeDateTime.__mQuarterBegin(appTypeDateTime.__mDayBegin(DateTime.Now));
                    _cInputTo.__fValue_ = appTypeDateTime.__mQuarterEnd(appTypeDateTime.__mDayBegin(DateTime.Now));
                    break;
                case 3:
                    _cInput.__fValue_ = appTypeDateTime.__mYearBegin(appTypeDateTime.__mDayBegin(DateTime.Now));
                    _cInputTo.__fValue_ = appTypeDateTime.__mYearEnd(appTypeDateTime.__mDayBegin(DateTime.Now));
                    break;
            }
            __fMarkStatus_ = true;
        }
        /// <summary>
        /// Период предыдущий
        /// </summary>
        private void MenuItemBefore_Click(object sender, EventArgs e)
        {
            MenuItem vMenuItem = sender as MenuItem;
            switch (vMenuItem.Index)
            {
                case 0:
                    _cInput.__fValue_ = appTypeDateTime.__mWeekBegin(appTypeDateTime.__mDayBegin(DateTime.Now).AddDays(-7));
                    _cInputTo.__fValue_ = appTypeDateTime.__mWeekEnd(appTypeDateTime.__mDayBegin(DateTime.Now).AddDays(-7));
                    break;
                case 1:
                    _cInput.__fValue_ = appTypeDateTime.__mMonthBegin(appTypeDateTime.__mDayBegin(DateTime.Now.AddMonths(-1)));
                    _cInputTo.__fValue_ = appTypeDateTime.__mMonthEnd(appTypeDateTime.__mDayBegin(DateTime.Now.AddMonths(-1)));
                    break;
                case 2:
                    _cInput.__fValue_ = appTypeDateTime.__mQuarterBegin(appTypeDateTime.__mDayBegin(DateTime.Now).AddDays(-90));
                    _cInputTo.__fValue_ = appTypeDateTime.__mQuarterEnd(appTypeDateTime.__mDayBegin(DateTime.Now).AddDays(-90));
                    break;
                case 3:
                    _cInput.__fValue_ = appTypeDateTime.__mYearBegin(appTypeDateTime.__mDayBegin(DateTime.Now.AddYears(-1)));
                    _cInputTo.__fValue_ = appTypeDateTime.__mYearEnd(appTypeDateTime.__mDayBegin(DateTime.Now.AddYears(-1)));
                    break;
            }

            __fMarkStatus_ = true;
        }
        /// <summary>
        /// Период с начала отсчета 
        /// </summary>
        private void MenuItemEmpty_Click(object sender, EventArgs e)
        {
            _cInput.__fValue_ = appTypeDateTime.__mMsSqlDateEmpty();
            _cInputTo.__fValue_ = appTypeDateTime.__mMsSqlDateEmpty();

            __fMarkStatus_ = true;
        }
        /// <summary>
        /// Период с начала отсчета 
        /// </summary>
        private void MenuItemEmptyStart_Click(object sender, EventArgs e)
        {
            _cInput.__fValue_ = appTypeDateTime.__mMsSqlDateEmpty();
            __fMarkStatus_ = true;
        }

        #endregion Контекстное меню

        /// <summary>
        /// Выполняется при изменении введенных данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mInput_ValueInteractiveChanged(object sender, EventArgs e)
        {
            __fMarkStatus_ = true;  /// Включение использования фильтра
        }
        /// <summary>
        /// Проверка введенных данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mInputTo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Convert.ToDateTime(_cInput.__fValue_) > Convert.ToDateTime(_cInputTo.__fValue_))
            {
                (FindForm() as elmForm).__mBaloonMessage(_cInputTo, elmApplication.__oTunes.__mTranslate("Вторая дата должна быть больше первой"));
                e.Cancel = true;
            }
        }

        #endregion - Поведение

        #endregion = МЕТОДЫ  

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Разрешение не загружать сохраненные данные
        /// </summary>
        public bool __fNotLoad = false;

        #endregion Атрибуты

        #region - Внутренние

        /// <summary>
        /// Вид надписи перед переходом в недоступный режим
        /// </summary>
        private LABELTYPES fLabelCaptionStatus = LABELTYPES.Normal;

        #endregion - Внутренние

        #region - Компоненты

        /// <summary>
        /// Поле ввода даты времени с...
        /// </summary>
        protected elmComponentDateTime _cInput = new elmComponentDateTime();
        /// <summary>
        /// Поле ввода даты времени по...
        /// </summary>
        protected elmComponentDateTime _cInputTo = new elmComponentDateTime();

        #endregion - Компоненты

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
                /// Добавление префикса таблицы если он указан
                if (__fTableAlias.Trim().Length > 0)
                {
                    vFieldNameWithPrefix = __fTableAlias + "." + __fFieldName;
                }
                /// Префикс таблицы не указан
                else
                {
                    vFieldNameWithPrefix = __fFieldName;
                }
                /// Даты 'c' и 'по' больше нулевой даты
                if (Convert.ToDateTime(_cInput.__fValue_) > appTypeDateTime.__mMsSqlDateEmpty() & Convert.ToDateTime(_cInputTo.__fValue_) > appTypeDateTime.__mMsSqlDateEmpty())
                {
                    vReturn = vFieldNameWithPrefix
                        + " Between "
                        + "Convert(DateTime, '" + appTypeDateTime.__mMsSqlDateTimeToString(Convert.ToDateTime(__fValue_)) + "')"
                        + " and "
                        + "Convert(DateTime, '" + appTypeDateTime.__mMsSqlDateTimeToString(Convert.ToDateTime(__fValueTo_)) + "')";
                }
                /// Дата 'c' нулевая, а 'по' больше нулевой даты
                if (Convert.ToDateTime(_cInput.__fValue_) == appTypeDateTime.__mMsSqlDateEmpty() & Convert.ToDateTime(_cInputTo.__fValue_) > appTypeDateTime.__mMsSqlDateEmpty())
                {
                    vReturn = vFieldNameWithPrefix
                        + " < "
                        + "Convert(DateTime, '" + appTypeDateTime.__mMsSqlDateTimeToString(Convert.ToDateTime(__fValueTo_)) + "')";
                }
                /// Дата 'c' больше нулевой даты, а 'по' нулевая
                if (Convert.ToDateTime(_cInput.__fValue_) > appTypeDateTime.__mMsSqlDateEmpty() & Convert.ToDateTime(_cInputTo.__fValue_) == appTypeDateTime.__mMsSqlDateEmpty())
                {
                    vReturn = vFieldNameWithPrefix
                        + " > "
                        + "Convert(DateTime, '" + appTypeDateTime.__mMsSqlDateTimeToString(Convert.ToDateTime(__fValue_)) + "')";
                }

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

                if (__fMarkStatus_ == true)
                {
                    if (Convert.ToDateTime(_cInput.__fValue_) > appTypeDateTime.__mMsSqlDateEmpty() & Convert.ToDateTime(_cInputTo.__fValue_) > appTypeDateTime.__mMsSqlDateEmpty())
                    {
                        vReturn = __fCaption_ + " "
                            + elmApplication.__oTunes.__mTranslate("c") + " "
                            + _cInput.__fValue_.ToString() + " "
                            + elmApplication.__oTunes.__mTranslate("по") + " "
                            + _cInputTo.__fValue_.ToString();
                    } /// Даты 'c' и 'по' больше нулевой даты
                    if (Convert.ToDateTime(_cInput.__fValue_) == appTypeDateTime.__mMsSqlDateEmpty() & Convert.ToDateTime(_cInputTo.__fValue_) > appTypeDateTime.__mMsSqlDateEmpty())
                    {
                        vReturn = __fCaption_ + " "
                            + elmApplication.__oTunes.__mTranslate("до") + " "
                            + _cInputTo.__fValue_.ToString();
                    } /// Дата 'c' нулевая, а 'по' больше нулевой даты
                    if (Convert.ToDateTime(_cInput.__fValue_) > appTypeDateTime.__mMsSqlDateEmpty() & Convert.ToDateTime(_cInputTo.__fValue_) == appTypeDateTime.__mMsSqlDateEmpty())
                    {
                        vReturn = __fCaption_ + " "
                            + elmApplication.__oTunes.__mTranslate("c") + " "
                            + _cInput.__fValue_.ToString() + " ";
                    } /// Дата 'c' больше нулевой даты, а 'по' нулевая
                }

                return vReturn;
            }
        }
        /// <summary>
        /// Обязательность заполнения
        /// </summary>
        public virtual FILLTYPES __fInputFillType_
        {
            get { return _cInput.__fFillType_; }
            set
            {
                _cInput.__fFillType_ = value;
                _cInputTo.__fFillType_ = value;
            }
        }
        public bool __fValueInTicks_
        {
            get { return _cInput.__fValueInTicks_; }
            set { _cInput.__fValueInTicks_ = value; }
        }
        public override object __fValue_
        {
            get { return _cInput.__fValue_; }
            set
            {
                if (__fNotLoad == false)
                    _cInput.__fValue_ = Convert.ToDateTime(value);
            }
        }
        public object __fValueTo_
        {
            get { return _cInputTo.__fValue_; }
            set
            {
                if (__fNotLoad == false)
                    _cInputTo.__fValue_ = Convert.ToDateTime(value);
            }
        }

        #endregion = СВОЙСТВА

        #region = СОБЫТИЯ

        ///// <summary>
        ///// Возникает при изменении данных
        ///// </summary>
        //public event EventHandler __eChanged;
        /// <summary>
        /// Возникает при изменении данных пользователем
        /// </summary>
        public event EventHandler __eChangedByUser;
        ///// <summary>
        ///// Возникает при изменении данных программой
        ///// </summary>
        //public event EventHandler __eChangedByProgram;
        ///// <summary>
        ///// Возникает при нажатии клавиши
        ///// </summary>
        //public event EventHandler __eKeyDown;
        ///// <summary>
        ///// Возникает при изменении типа отображаемых данных
        ///// </summary>
        //public event EventHandler __eDateTimeTypeChanged;

        #endregion СОБЫТИЯ
    }
}
