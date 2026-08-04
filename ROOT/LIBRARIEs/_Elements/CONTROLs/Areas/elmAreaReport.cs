using nlApplication;
using System.Globalization;
using System.Windows.Forms;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmAreaReport.cs
    /// </summary>
    /// <remarks>Класс-область для формирования отчетов</remarks>
    public class elmAreaReport : elmArea
    {
        #region = МЕТОДЫ

        #region - Объект

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
            _cToolBar.Items.Insert(0, _cButtonExecute);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            // _cButtonApply
            {
                _cButtonExecute.Click += _cButtonApply_Click;
                _cButtonExecute.Image = global::nlResourcesImages.Properties.Resources._Sign_Tick_g32;
                _cButtonExecute.ToolTipText = "[ Ctrl + A ]\n" + elmApplication.__oTunes.__mTranslate("Применить");
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

        #endregion Объект

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
            elmFormReport vFormReport = FindForm() as elmFormReport;
            if (__fCloseFormAfterReport == true)
            {
                if (vFormReport.__mBuildReport() == true)
                {
                    elmForm vForm = FindForm() as elmForm;
                    vForm.Close();
                }
            }
            else
            {
                vFormReport.__mBuildReport();
            }
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

            if (_fFormNameParent.Length == 0)
            {
                _fFormNameParent = (FindForm() as elmForm).__fClassName_;
            } /// Не указано название формы для которой строиться фильтр

            /// Перебор установленных компонентов фильтра
            foreach (Control vInput in _cBlockInputs.Controls)
            { /// Перебор установленных компонентов фильтра
                if ((vInput is elmInput) == true)
                { /// Компонент - поле ввода
                    string vFieldName = (vInput as elmInput).__fFieldName; // Название поля для которого строиться фильтр
                    try
                    {
                        (vInput as elmInput).__fMarkStatus_ = Convert.ToBoolean(oFileIni.__mValueRead(_fFormNameParent, "FilterStatus" + __fAreaId + "_" + vFieldName)); /// Загрузка статуса
                    }
                    catch
                    {
                        (vInput as elmInput).__fMarkStatus_ = false;
                    }
                    try
                    {
                        if (vInput is elmInputBool)
                            (vInput as elmInputBool).__fValue_ = Convert.ToBoolean(oFileIni.__mValueRead(_fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName));
                        if (vInput is elmInputCombo)
                            (vInput as elmInputCombo).__fValue_ = Convert.ToInt32(oFileIni.__mValueRead(_fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName));
                        if (vInput is elmInputDateTime)
                            (vInput as elmInputDateTime).__fValue_ = Convert.ToDateTime(oFileIni.__mValueRead(_fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName));
                        if (vInput is elmInputDateTimePeriod)
                        {
                            if ((vInput as elmInputDateTimePeriod).__fValueInTicks_ == false)
                            {
                                (vInput as elmInputDateTimePeriod).__fValue_ = DateTime.Parse(oFileIni.__mValueRead(_fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName), new CultureInfo("ru-RU", false));
                                (vInput as elmInputDateTimePeriod).__fValueTo_ = DateTime.Parse(oFileIni.__mValueRead(_fFormNameParent, "FilterValueTo" + __fAreaId + "_" + vFieldName), new CultureInfo("ru-RU", false));
                            } /// Данные храняться как дата-время
                            else
                            {
                                (vInput as elmInputDateTimePeriod).__fValue_ = new DateTime(Convert.ToInt64(oFileIni.__mValueRead(_fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName)));
                                (vInput as elmInputDateTimePeriod).__fValueTo_ = new DateTime(Convert.ToInt64(oFileIni.__mValueRead(_fFormNameParent, "FilterValueTo" + __fAreaId + "_" + vFieldName)));
                            } /// Данные храняться как тики
                        }
                        if (vInput is elmInputFormCode)
                            (vInput as elmInputFormCode).__fValue_ = Convert.ToInt32(oFileIni.__mValueRead(_fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName));
                        if (vInput is elmInputFormName)
                            (vInput as elmInputFormName).__fValue_ = Convert.ToInt32(oFileIni.__mValueRead(_fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName));
                        if (vInput is elmInputNumeric)
                            (vInput as elmInputNumeric).__fValue_ = Convert.ToDecimal(oFileIni.__mValueRead(_fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName));
                        if (vInput is elmInputInteger)
                            (vInput as elmInputInteger).__fValue_ = Convert.ToInt32(oFileIni.__mValueRead(_fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName));
                        if (vInput is elmInputPhone)
                            (vInput as elmInputPhone).__fValue_ = oFileIni.__mValueRead(_fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                        if (vInput is elmInputString)
                            (vInput as elmInputString).__fValue_ = oFileIni.__mValueRead(_fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
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
            _fFormFilterExpression = ""; // Сформированное условие фильтра
            _fFormFilterMessage = ""; // Условие фильтра отображаемое пользователю

            bool vReturn = true; // Возвращаемое значение
            elmForm vForm = FindForm() as elmForm;
            appFileIni oFileIni = vForm.__oFileIni; // Объект для работы с инициализационным файлом

            if (_fFormNameParent.Length == 0)
            {
                _fFormNameParent = FindForm().Name;
            } /// Не указано название формы для которой строиться фильтр

            foreach (Control vInput in _cBlockInputs.Controls)
            { /// Перебор установленных компонентов фильтра
                string vFieldName = (vInput as elmInput).__fFieldName; // Название поля для которого строиться фильтр
                /// Компонент - поле ввода
                if ((vInput is elmInput) == true)
                { 
                    if (vInput is elmInputList)
                        continue;

                    oFileIni.__mValueWrite((vInput as elmInput).__fMarkStatus_.ToString(), _fFormNameParent, "FilterStatus" + __fAreaId + "_" + vFieldName); /// Сохранение статуса использования текущего компонента
                    oFileIni.__mValueWrite((vInput as elmInput).__fCaption_, _fFormNameParent, "FilterCaption" + __fAreaId + "_" + vFieldName); /// Сохранение заголовка текущего компонента
                    oFileIni.__mValueWrite((vInput as elmInput).__fFilterExpression_, _fFormNameParent, "FilterExpression" + __fAreaId + "_" + vFieldName); /// Сохранение условия фильтра текущего компонента
                    oFileIni.__mValueWrite((vInput as elmInput).__fFilterMessage_, _fFormNameParent, "FilterMessage" + __fAreaId + "_" + vFieldName); /// Сохранение выражение фильтра текущего компонента

                    if (vInput is elmInputBool)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputBool).__fValue_.ToString(), _fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputCombo)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputCombo).__fValue_.ToString(), _fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputDateTime)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputDateTime).__fValue_.ToString(), _fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputDateTimePeriod)
                    {
                        oFileIni.__mValueWrite(appTypeDateTime.__mDateTimeToString(Convert.ToDateTime((vInput as elmInputDateTimePeriod).__fValue_)).ToString(), _fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                        oFileIni.__mValueWrite(appTypeDateTime.__mDateTimeToString(Convert.ToDateTime((vInput as elmInputDateTimePeriod).__fValueTo_)).ToString(), _fFormNameParent, "FilterValueTo" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputFormCode)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputFormCode).__fValue_.ToString(), _fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputFormName)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputFormName).__fValue_.ToString(), _fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputNumeric)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputNumeric).__fValue_.ToString(), _fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputInteger)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputInteger).__fValue_.ToString(), _fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputPhone)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputPhone).__fValue_.ToString(), _fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }
                    if (vInput is elmInputString)
                    {
                        oFileIni.__mValueWrite((vInput as elmInputString).__fValue_.ToString(), _fFormNameParent, "FilterValue" + __fAreaId + "_" + vFieldName);
                    }

                    string vFilterExpression = (vInput as elmInput).__fFilterExpression_.Trim();
                    string vFilterMessage = (vInput as elmInput).__fFilterMessage_.Trim();
                    if (vFilterExpression.Length > 0)
                    { /// Собрание всех условий в единый фильтр
                        if (_fFormFilterExpression.Length == 0)
                        {
                            _fFormFilterExpression = vFilterExpression;
                            _fFormFilterMessage = vFilterMessage;
                        }
                        else
                        {
                            if (vFilterExpression.Length > 0)
                            {
                                _fFormFilterExpression = _fFormFilterExpression + " AND " + vFilterExpression;
                                _fFormFilterMessage = _fFormFilterMessage + "\n" + vFilterMessage;
                            }
                        }
                    } /// Собрание всех условий в единый фильтр
                } /// Компонент - поле ввода
            } /// Перебор установленных компонентов фильтра

            return vReturn;
        }
        /// <summary>
        /// Отмена выбора всех компонентов в инициализационным файле
        /// </summary>
        public void __mFilterUnMarkAll(string pFieldName)
        {
            elmForm vForm = FindForm() as elmForm;
            appFileIni oFileIni = vForm.__oFileIni; // Объект для работы с инициализационным файлом
            string vFieldName = ""; // Название поля для которого строиться фильтр

            foreach (Control vInput in _cBlockInputs.Controls)
            {
                vFieldName = (vInput as elmInput).__fFieldName; // Название поля для которого строиться фильтр
                if (vFieldName != pFieldName)
                {
                    oFileIni.__mValueWrite("false", vForm.__fClassName_, "FilterStatus" + __fAreaId + "_" + vFieldName); /// Сохранение статуса использования текущего компонента
                }
            }

            return;
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
            _cButtonExecute.PerformClick();
            return;
        }
        /// <summary>
        /// Получение количества выбранных полей ввода
        /// </summary>
        public int __mInputsMarked()
        {
            int vReturn = 0; // Возвращаемое значение
            foreach (Control vInput in _cBlockInputs.Controls)
            {
                if ((vInput as elmInput).__fMarkStatus_ == true)
                    vReturn++;

            }
            return vReturn;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary> 
        /// Сформированное условие фильтра
        /// </summary>
        public string _fFormFilterExpression = "";
        /// <summary>
        /// Условие фильтра отображаемое пользователю
        /// </summary>
        public string _fFormFilterMessage = "";
        /// <summary>
        /// Имя родительской формы для которой строиться фильтр
        /// </summary>
        public string _fFormNameParent = "";
        /// <summary>
        /// Указание закрывать форму после формирования отчета
        /// </summary>
        public bool __fCloseFormAfterReport = true;

        #endregion Атрибуты

        #region - Компоненты

        /// <summary>
        /// Кнопка 'Выполнить'
        /// </summary>
        protected elmComponentToolbarButton _cButtonExecute = new elmComponentToolbarButton();
        /// <summary>
        /// Панель для отображения полей ввода
        /// </summary>
        protected elmBlockInputs _cBlockInputs = new elmBlockInputs();

        #endregion Компоненты

        #endregion ПОЛЯ
    }
}
