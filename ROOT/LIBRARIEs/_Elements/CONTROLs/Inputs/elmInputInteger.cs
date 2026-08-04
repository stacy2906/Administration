using System;
using System.ComponentModel;
using System.Drawing;

namespace nlElements
{
    public class elmInputInteger : elmInput
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
                _cLabelCaption.__fLabelType_ = LABELTYPES.Normal; /// Назначение вида надписи-заголовка - 'Надпись' 
                _cLabelCaption.__eClickRight += mLabelCaptionMouseClickRight;
                fLabelCaptionStatus = _cLabelCaption.__fLabelType_; /// Сохранение установленного статуса надписи-заголовка
            }
            // _cInput
            {
                _cInput.Location = new Point(0, 0);
                _cInput.__eChangedByUser += mInputChangedByUser;
                _cInput.__fSymbolsFractionalCount_ = 0;
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
        private void mLabelCaptionMouseClickRight(object sender, EventArgs e)
        {
            _cInput.Text = ""; /// Очищается название искомого справочника
            __fMarkStatus_ = false; /// !!! Выключается использование фильтра
            _cInput.Focus(); /// Перемещается курсор в поле ввода
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
        /// <summary>
        /// Выполняется при проверке ввода данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e); // Всегда выполнять в начале метода!

            /// Нет данных - пустая строка
            if (_cInput.__fValue_.ToString().Length == 0)
            {
                _cInput.Text = "0";
            }

            if (__fFillType_ == FILLTYPES.Necessarily)
            {
                if (_cInput.__fValue_.ToString() == "0" | _cInput.__fValue_.ToString() == "")
                {
                    (FindForm() as elmForm).__mBaloonMessage(_cInput, elmApplication.__oTunes.__mTranslate("Поле должно быть обязательно заполненным"));
                    e.Cancel = true;
                }
            }
            
            return;
        }

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
        /// Количество символов в целой части
        /// </summary>
        public int __fPartInt_
        {
            get
            {
                return _cInput.__fSymbolsIntegerCount_;
            }
            set
            {
                _cInput.__fSymbolsIntegerCount_ = value;
            }
        }
        /// <summary>
        /// Дотупность контрола
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
                    if (_cInput.Text.Length > 0)
                        _cLabelValue.Text = _cInput.Text;
                    else
                        _cLabelValue.Text = elmApplication.__oTunes.__mTranslate("нет данных");
                }
            }
        }
        /// <summary>
        ///  Обязательность заполнения
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

                string vFieldNameWithPrefix = ""; // Название поля с префиксом таблицы
                if (__fTableAlias.Trim().Length > 0)
                { /// Добавление префикса таблицы если он указан
                    vFieldNameWithPrefix = __fTableAlias + "." + __fFieldName;
                }
                else
                { /// Префикс таблицы не указан
                    vFieldNameWithPrefix = __fFieldName;
                }

                vReturn = vFieldNameWithPrefix + "=" + __fValue_.ToString();

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

                if (_cMark.Checked == true)
                {
                    vReturn = __fCaption_ + " = '" + _cInput.__fValue_ + "'";
                }

                return vReturn;
            }
        }

        public override object __fValue_
        {
            get { return _cInput.__fValue_; }
            set { _cInput.__fValue_ = value; }
        }
        public int __fValueMaximum_
        {
            get { return Convert.ToInt32(_cInput.__fValueMaximum_); }
            set { _cInput.__fValueMaximum_ = value; }
        }
        public int __fValueMinimum_
        {
            get { return Convert.ToInt32(_cInput.__fValueMinimum_); }
            set { _cInput.__fValueMinimum_ = value; }
        }

        #endregion СВОЙСТВА
    }
}
