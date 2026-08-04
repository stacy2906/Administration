using nlApplication;
using System.Globalization;
using System.Windows.Forms;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmAreaFilter.cs
    /// </summary>
    /// <remarks>Класс-область для построения фильтра</remarks>
    public class elmAreaFilter : elmArea
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

            Panel2.Controls.Add(_cBlockInputs);
            Panel2.Controls.SetChildIndex(_cBlockInputs, 0);
            Panel2.Controls.SetChildIndex(_cToolBar, 1);
            _cToolBar.Items.Insert(0, _cButtonApply);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            // _cButtonApply
            {
                _cButtonApply.Click += _cButtonApply_Click;
                _cButtonApply.Image = global::nlResourcesImages.Properties.Resources._Sign_Tick_g32;
                _cButtonApply.ToolTipText = "[ Ctrl + A ]\n" + elmApplication.__oTunes.__mTranslate("Применить");
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
        /// <summary>
        /// Выполняется при первом отображении объекта
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();
            __mFilterLoad();
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        #region Кнопки управления

        /// <summary>
        /// Выполняется при выборе кнопки 'Применить'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cButtonApply_Click(object sender, EventArgs e)
        {
            __mFilterSave();
            elmForm vForm = FindForm() as elmForm;
            vForm.Close();
        }

        #endregion Кнопки управления

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Загрузка настроек фильтра из файла
        /// </summary>
        /// <returns>[true] - данные загружены, иначе - [false]</returns>
        public bool __mFilterLoad()
        {
            bool vReturn = true; // Возвращаемое значение
            elmForm vForm = FindForm() as elmForm;
            appFileIni oFileIni = vForm.__oFileIni; // Объект для работы с инициализационным файлом
            oFileIni.__fFilePath = elmApplication.__oPathes.__mFileFormTunes();

            if (__fFormNameParent.Length == 0)
            {
                __fFormNameParent = vForm.__fClassName_;
            } /// Не указано название формы для которой строиться фильтр

            /// Перебор установленных компонентов фильтра
            foreach (Control vInput in _cBlockInputs.Controls)
            { /// Перебор установленных компонентов фильтра
                if ((vInput is elmInput) == true)
                { /// Компонент - поле ввода
                    string vFieldName = (vInput as elmInput).__fFieldName; // Название поля для которого строиться фильтр
                    try
                    {
                        (vInput as elmInput).__fMarkStatus_ = Convert.ToBoolean(oFileIni.__mValueRead(__fFormNameParent, "FilterStatus" + __fAreaId + "_" + vFieldName)); /// Загрузка статуса
                    }
                    catch
                    {
                        (vInput as elmInput).__fMarkStatus_ = false;
                    }
                    try
                    {
                        if (vInput is elmInputBool)
                            (vInput as elmInputBool).__fValue_ = Convert.ToBoolean(oFileIni.__mValueRead(__fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName));
                        if (vInput is elmInputCombo)
                            (vInput as elmInputCombo).__fValue_ = Convert.ToInt32(oFileIni.__mValueRead(__fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName));
                        if (vInput is elmInputDateTime)
                            (vInput as elmInputDateTime).__fValue_ = Convert.ToDateTime(oFileIni.__mValueRead(__fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName));
                        if (vInput is elmInputDateTimePeriod)
                        {
                            if ((vInput as elmInputDateTimePeriod).__fValueInTicks_ == false)
                            {
                                (vInput as elmInputDateTimePeriod).__fValue_ = DateTime.Parse(oFileIni.__mValueRead(__fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName), new CultureInfo("ru-RU", false));
                                (vInput as elmInputDateTimePeriod).__fValueTo_ = DateTime.Parse(oFileIni.__mValueRead(__fFormNameParent, "FilterValueTo" + __fAreaId + "_" + vFieldName), new CultureInfo("ru-RU", false));
                            } /// Данные храняться как дата-время
                            else
                            {
                                (vInput as elmInputDateTimePeriod).__fValue_ = new DateTime(Convert.ToInt64(oFileIni.__mValueRead(__fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName)));
                                (vInput as elmInputDateTimePeriod).__fValueTo_ = new DateTime(Convert.ToInt64(oFileIni.__mValueRead(__fFormNameParent, "FilterValueTo" + __fAreaId + "_" + vFieldName)));
                            } /// Данные храняться как тики
                        }
                        if (vInput is elmInputFormCode)
                            (vInput as elmInputFormCode).__fValue_ = Convert.ToInt32(oFileIni.__mValueRead(__fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName));
                        if (vInput is elmInputFormName)
                            (vInput as elmInputFormName).__fValue_ = Convert.ToInt32(oFileIni.__mValueRead(__fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName));
                        if (vInput is elmInputNumeric)
                            (vInput as elmInputNumeric).__fValue_ = Convert.ToDecimal(oFileIni.__mValueRead(__fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName));
                        if (vInput is elmInputInteger)
                            (vInput as elmInputInteger).__fValue_ = Convert.ToInt32(oFileIni.__mValueRead(__fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName));
                        if (vInput is elmInputPhone)
                            (vInput as elmInputPhone).__fValue_ = oFileIni.__mValueRead(__fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                        if (vInput is elmInputString)
                            (vInput as elmInputString).__fValue_ = oFileIni.__mValueRead(__fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                        if (vInput is elmInputQuote)
                            (vInput as elmInputQuote).__fValue_ = oFileIni.__mValueRead(__fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    catch
                    {
                        (vInput as elmInput).__fMarkStatus_ = false; /// Первая загрузка статуса
                    }
                } /// Компонент - поле ввода

            }

            return vReturn;
        }
        /// <summary>
        /// Сохранение настроек фильтра в файл
        /// </summary>
        /// <returns>[true] - данные загружены, иначе - [false]</returns>
        public bool __mFilterSave()
        {
            __fFormFilterExpression = ""; // Сформированное условие фильтра
            __fFormFilterMessage = ""; // Условие фильтра отображаемое пользователю

            bool vReturn = true; // Возвращаемое значение
            elmForm vForm = FindForm() as elmForm;
            appFileIni oFileIni = vForm.__oFileIni; // Объект для работы с инициализационным файлом

            if (__fFormNameParent.Length == 0)
            {
                __fFormNameParent = (FindForm() as elmForm).__fClassName_;
            } /// Не указано название формы для которой строиться фильтр

            foreach (Control vInput in _cBlockInputs.Controls)
            { /// Перебор установленных компонентов фильтра
                string vFieldName = (vInput as elmInput).__fFieldName; // Название поля для которого строиться фильтр

                if ((vInput is elmInput) == true)
                { /// Компонент - поле ввода
                    oFileIni.__mValueWrite((vInput as elmInput).__fMarkStatus_.ToString(), __fFormNameParent, "FilterStatus" + __fAreaId + "_" + vFieldName); /// Сохранение статуса использования текущего компонента
                    oFileIni.__mValueWrite((vInput as elmInput).__fCaption_, __fFormNameParent, "FilterCaption" + __fAreaId + "_" + vFieldName); /// Сохранение заголовка текущего компонента
                    oFileIni.__mValueWrite((vInput as elmInput).__fFilterExpression_, __fFormNameParent, "FilterExpression" + __fAreaId + "_" + vFieldName); /// Сохранение условия фильтра текущего компонента
                    oFileIni.__mValueWrite((vInput as elmInput).__fFilterMessage_, __fFormNameParent, "FilterMessage" + __fAreaId + "_" + vFieldName); /// Сохранение выражение фильтра текущего компонента

                    if (vInput is elmInputBool)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputBool).__fValue_.ToString(), __fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputCombo)
                    {
                        if((vInput as elmInputCombo).__fValue_ != null)
                            oFileIni.__mValueWrite((vInput as elmInputCombo).__fValue_.ToString(), __fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputDateTime)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputDateTime).__fValue_.ToString(), __fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputDateTimePeriod)
                    {
                        oFileIni.__mValueWrite(appTypeDateTime.__mDateTimeToString(Convert.ToDateTime((vInput as elmInputDateTimePeriod).__fValue_)).ToString(), __fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                        oFileIni.__mValueWrite(appTypeDateTime.__mDateTimeToString(Convert.ToDateTime((vInput as elmInputDateTimePeriod).__fValueTo_)).ToString(), __fFormNameParent, "FilterValueTo" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputFormCode)
                    {
                        if((vInput as elmInputFormCode).__fValue_ != null   )
                            oFileIni.__mValueWrite((vInput as elmInputFormCode).__fValue_.ToString(), __fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputFormName)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputFormName).__fValue_.ToString(), __fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputNumeric)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputNumeric).__fValue_.ToString(), __fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputInteger)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputInteger).__fValue_.ToString(), __fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputPhone)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputPhone).__fValue_.ToString(), __fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputString)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputString).__fValue_.ToString(), __fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputQuote)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputQuote).__fValue_.ToString(), __fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    string vFilterExpression = "";
                    string vFilterMessage = "";
                    if ((vInput as elmInput).__fFilterExpression_ != null)
                        vFilterExpression = (vInput as elmInput).__fFilterExpression_.Trim();
                    if ((vInput as elmInput).__fFilterMessage_ != null)
                        vFilterMessage = (vInput as elmInput).__fFilterMessage_.Trim();
                    if (vFilterExpression.Length > 0)
                    { /// Собрание всех условий в единый фильтр
                        if (__fFormFilterExpression.Length == 0)
                        {
                            __fFormFilterExpression = vFilterExpression;
                            __fFormFilterMessage = vFilterMessage;
                        }
                        else
                        {
                            if (vFilterExpression.Length > 0)
                            {
                                __fFormFilterExpression = __fFormFilterExpression + " AND " + vFilterExpression;
                                __fFormFilterMessage = __fFormFilterMessage + "\n" + vFilterMessage;
                            }
                        }
                    } /// Собрание всех условий в единый фильтр
                } /// Компонент - поле ввода
            } /// Перебор установленных компонентов фильтра

            return vReturn;
        }
        /// <summary>
        /// Добавление поля ввода на панель полей ввода
        /// </summary>
        /// <param name="pInput">Поле ввода</param>
        /// <param name="pHeight">Высота создаваемоно компонента</param>
        /// <returns></returns>
        public bool __mInputAdd(elmInput pInput, int pHeight = 25)
        {
            return _cBlockInputs.__mInputAdd(pInput, pHeight);
        }
        /// <summary>
        /// Выполняется при выборе кнопки 'Применить' 
        /// </summary>
        public void __mPressButtonApply()
        {
            _cButtonApply.PerformClick();
            return;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary> 
        /// Сформированное условие фильтра
        /// </summary>
        public string __fFormFilterExpression = "";
        /// <summary>
        /// Условие фильтра отображаемое пользователю
        /// </summary>
        public string __fFormFilterMessage = "";
        /// <summary>
        /// Имя родительской формы для которой строиться фильтр
        /// </summary>
        public string __fFormNameParent = "";

        #endregion Атрибуты

        #region - Компоненты

        /// <summary>
        /// Кнопка 'Применить'
        /// </summary>
        protected elmComponentToolbarButton _cButtonApply = new elmComponentToolbarButton();
        /// <summary>
        /// Панель для отображения полей ввода
        /// </summary>
        protected elmBlockInputs _cBlockInputs = new elmBlockInputs();

        #endregion Компоненты

        #endregion ПОЛЯ
    }
}
