using System.ComponentModel;
using System.Drawing;
using System;
using nlApplication;

namespace nlElements
{
    /// <summary>
    /// Файл elmInputFractional.cs
    /// </summary>
    /// <remarks>Класс-поле ввода дробных числовых значений</remarks>
    /* Пример использования
                _cInputNumeric.Location = new System.Drawing.Point(10, 10);
                _cInputNumeric.__fCaption_ = "десятичное число";
                _cInputNumeric.__mCaptionBuilding("Десятичное число {0}", 2);
                _cInputNumeric.__fFillType_ = FILLTYPES.Necessarily;
                _cInputNumeric.__fPromptCaption_ = "Ввод десятичного числа";
                _cInputNumeric.__fValueMaximum_ = 1500;
                _cInputNumeric.__fValueMinimum_ = -2;
                _cInputNumeric.__fSymbolsFractionalCount_ = 3;
                _cInputNumeric.__fSymbolsIntegerCount_ = 8;
                _cInputNumeric.__fValue_ = 14.010;
                _cInputNumeric.__fSignsCountInGroup_ = 3;
                _cInputNumeric.__fNegative_ = true;
    */
    public class elmInputNumeric : elmInput
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Загрузка объекта
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
                _cLabelCaption.__eClickRight += mCaption_MouseClickRight;
                fLabelCaptionStatus = _cLabelCaption.__fLabelType_; // Сохранение установленного статуса надписи-заголовка
            }
            // _cInput
            {
                _cInput.__eChangedByUser += mInputChangedByUser;
                //d_cInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
                OnValidating(new CancelEventArgs());
                __eChanged += mInputFractional_Changed;
                __eChangedByProgram += mInputFractional_ChangedByProgram;
                __eChangedByUser += mInputFractional_ChangedByUser;
                __eKeyDown += mInputFractional_KeyDown;
            }

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется при изменении данных
        /// </summary>
        private void mInputFractional_Changed(object sender, EventArgs e)
        {
            if(__eChanged != null)
            {
                __eChanged(sender, e);
            }
        }
        /// <summary>
        /// Выполняется при изменении данных программой
        /// </summary>
        private void mInputFractional_ChangedByProgram(object sender, EventArgs e)
        {
            if (__eChangedByProgram != null)
            {
                __eChangedByProgram(sender, e);
            }
        }
        /// <summary>
        /// Выполняется при изменении данных пользователем
        /// </summary>
        private void mInputFractional_ChangedByUser(object sender, EventArgs e)
        {
            if (__eChangedByUser != null)
            {
                __eChangedByUser(sender, e);
            }
        }
        /// <summary>
        /// Выполняется при нажатии клавиши
        /// </summary>
        private void mInputFractional_KeyDown(object sender, EventArgs e)
        {
            if (__eKeyDown != null)
            {
                __eKeyDown(sender, e);
            }
        }

        /// <summary>
        /// Выполняется при выборе надписи правой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mCaption_MouseClickRight(object sender, EventArgs e)
        {
            __fMarkStatus_ = false; /// !!! Выключается использование фильтра
            FILLTYPES vFillType = __fFillType_;
            __fFillType_ = FILLTYPES.None;
            _cInput.Text = "0" + (__fSymbolsFractionalCount_ > 0 ? __fSymbolSeparator_ + new String('0', __fSymbolsFractionalCount_) : ""); 
            _cInput.Focus(); /// Перемещается курсор в поле ввода
            __fFillType_ = vFillType;
        }
        /// <summary>
        /// Выполняется при ручном изменении введенных данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mInputChangedByUser(object sender, EventArgs e)
        {
            __fMarkStatus_ = true; /// Включение использования фильтра
        }
        ///// <summary>
        ///// Выполняется при проверке ввода данных
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected override void OnValidating(CancelEventArgs e)
        //{
        //    base.OnValidating(e); // Всегда выполнять в начале метода!
        //    ///// Данные начинаются с десятичного разделителя
        //    //if (_cInput.Text.Trim().StartsWith(_cInput.__fSymbolSeparator_.ToString()) == true)
        //    //{
        //    //    _cInput.Text = "0" + _cInput.Text.Trim();
        //    //}
        //    /// Нет данных - пустая строка
        //    //if (__fValue_.ToString().Length == 0)
        //    //{
        //    //    Text = "0" + __fSymbolSeparator_ + new string('0', __fSymbolsFractionalCount_);
        //    //}
        //    /// Добавление нулей к десятичному разделителю
        //    //if (Convert.ToDecimal(__fValue_) == 0)
        //    //{
        //    //    Text = "0" + __fSymbolSeparator_ + new string('0', __fSymbolsFractionalCount_);
        //    //}
        //    //else
        //    //{
        //    //    string vPartFractional = appTypeString.__mWordNumber(Text, 1, __fSymbolSeparator_);
        //    //    if (vPartFractional.Contains(__fSymbolSeparator_.ToString()) == false)
        //    //    {
        //    //        Text = Text + __fSymbolSeparator_.ToString();
        //    //    }
        //    //    if (vPartFractional.Length < __fSymbolsFractionalCount_)
        //    //    {
        //    //        Text = Text + new String('0', __fSymbolsFractionalCount_ - vPartFractional.Length);
        //    //    }
        //    //}
        //    //if (__fFillType_ == FILLTYPES.Necessarily)
        //    //{
        //    //    if (Convert.ToDecimal(__fValue_) == 0)
        //    //    {
        //    //        (FindForm() as elmForm).__mBaloonMessage(_cInput, elmApplication.__oTunes.__mTranslate("Поле должно быть обязательно заполненным"));
        //    //        e.Cancel = true;
        //    //    }
        //    //}

        //    return;
        //}

        #endregion Поведение

        #endregion МЕТОДЫ   

        #region = ПОЛЯ

        #region - Закрытые

        /// <summary>
        /// Разрешить нулевое значение
        /// </summary>
        private FILLTYPES fFillType = FILLTYPES.None;
        /// <summary>
        /// Вид надписи перед переходом в недоступный режим
        /// </summary>
        private LABELTYPES fLabelCaptionStatus = LABELTYPES.Normal;

        #endregion Закрытые

        #region - Компоненты

        /// <summary>
        /// Поле ввода дробных десятичных данных
        /// </summary>
        protected elmComponentNumeric _cInput = new elmComponentNumeric();

        #endregion Компоненты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Получает или устанавливает признак обязательности заполнения
        /// </summary>
        public override FILLTYPES __fFillType_
        {
            get { return _cInput.__fFillType_; }
            set { _cInput.__fFillType_ = value; }
        }
        /// <summary>
        /// Получает или устанавливает условие фильтра
        /// </summary>
        public override string __fFilterExpression_
        {
            get
            {
                string vReturn = ""; // Возвращаемое значение

                if (__fTableAlias.Length > 0)
                    vReturn = __fTableAlias + ".";
                vReturn = vReturn + __fFieldName + " = " + __fValue_.ToString();

                return vReturn;
            }

        }
        /// <summary>
        /// Получает или устанавливает выражение фильтра для отображения пользователю
        /// </summary>
        public override string __fFilterMessage_
        {
            get
            {
                string vReturn = ""; // Возвращаемое значение

                vReturn = vReturn + __fCaption_.Trim() + " = " + __fValueToText_;

                return vReturn;
            }
        }
        /// <summary>
        /// Получает или устанавливает доступность контрола
        /// </summary>
        public override bool __fEnabled_
        {
            get { return base.__fEnabled_; }
            set
            {
                base.__fEnabled_ = value;
                Visible = value;
                if (value == true)
                {
                    _cLabelCaption.__fLabelType_ = fLabelCaptionStatus;
                }
                else
                {
                    _cLabelCaption.__fLabelType_ = LABELTYPES.Normal;
                    if (Text.Length > 0)
                        _cLabelValue.Text = Text;
                    else
                        _cLabelValue.Text = elmApplication.__oTunes.__mTranslate("нет данных");
                }
            }
        }
        /// <summary>
        /// Получаето или устанавливает разрешение ввода отрицательных значений
        /// </summary>
        public bool __fNegative_
        {
            get
            {
                return _cInput.__fNegative_;
            }
            set
            {
                _cInput.__fNegative_ = value;
            }
        }

        /// <summary>
        /// Получает или устанавливает символ отрицательного знака
        /// </summary>
        public char __fSymbolNegative_
        {
            get { return _cInput.__fSymbolNegative_; }
            set { _cInput.__fSymbolNegative_ = value; }
        }
        /// <summary>
        /// Получает символ разделителя целой и десятичной частей
        /// </summary>
        public char __fSymbolSeparator_
        {
            get { return _cInput.__fSymbolSeparator_; }
            //set { _cInput.__fSymbolSeparator_ = value; }
        }
        /// <summary>
        /// Получает или устанавливает значение контрола
        /// </summary>
        public override object __fValue_
        {
            get { return _cInput.__fValue_; }
            set
            {
                _cInput.__fValue_ = Convert.ToDecimal(value);
                _cLabelValue.Text = value.ToString();  // Запись значения по умолчанию
            }
        }
        /// <summary>
        /// Получает или устанавливает максимально допустимое значение
        /// </summary>
        public decimal __fValueMaximum_
        {
            get { return Convert.ToDecimal(__fValueMaximum_); }
            set { _cInput.__fValueMaximum_ = Convert.ToDouble(value); }
        }
        /// <summary>
        /// Получает или устанавливает минимально допустимое значение
        /// </summary>
        public decimal __fValueMinimum_
        {
            get { return Convert.ToDecimal(__fValueMinimum_); }
            set { _cInput.__fValueMinimum_ = Convert.ToDouble(value); }
        }
        /// <summary>
        /// Получает строчный эквивалент значенияя
        /// </summary>
        public override string __fValueToText_
        {
            get { return Text; }
        }
        /// <summary>
        /// Получает или устанавливает количество символов в дробной части
        /// </summary>
        public int __fSymbolsFractionalCount_
        {
            get { return _cInput.__fSymbolsFractionalCount_; }
            set 
            {
                _cInput.__fSymbolsFractionalCount_ = value;
                __fSymbolsCount_ = _cInput.__fSymbolsIntegerCount_ + _cInput.__fSymbolsFractionalCount_ + 2; // Знак и символ разделителя
            }
        }
        /// <summary>
        /// Получает или устанавливает количество символов в целой части
        /// </summary>
        public int __fSymbolsIntegerCount_
        {
            get { return _cInput.__fSymbolsIntegerCount_; }
            set 
            { 
                _cInput.__fSymbolsIntegerCount_ = value;
                __fSymbolsCount_ = _cInput.__fSymbolsIntegerCount_ + _cInput.__fSymbolsFractionalCount_ + 2; // Знак и символ разделителя
            }
        }
        /// <summary>
        /// Получает или устанавливает количество символов, которые нужно разместить в каждой группе слева от десятичной точки
        /// </summary>
        public int __fSignsCountInGroup_
        {
            get { return _cInput.__fSymbolsInGroupCount_; }
            set { _cInput.__fSymbolsInGroupCount_ = value; }
        }
        /// <summary>
        /// Получает или устанавливает количество отображаемых символов
        /// </summary>
        public int __fSymbolsCount_
        {
            get { return _cInput.__fSymbolsCount_; }
            set { _cInput.__fSymbolsCount_ = value; }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при изменении данных
        /// </summary>
        public event EventHandler __eChanged;
        /// <summary>
        /// Возникает при изменении данных пользователем
        /// </summary>
        public event EventHandler __eChangedByUser;
        /// <summary>
        /// Возникает при изменении данных программой
        /// </summary>
        public event EventHandler __eChangedByProgram;
        /// <summary>
        /// Возникает при нажатии клавиши
        /// </summary>
        public event EventHandler __eKeyDown;

        #endregion СОБЫТИЯ
    }
}
