using nlApplication;
using System;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmBlockInputs.cs
    /// </summary>
    /// <remarks>Класс-блок для размещения полей ввода</remarks>
    public class elmBlockInputs : elmComponentPanel
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Настройка контрола

            Dock = DockStyle.Fill;
            Width = 400; // Для нормального размещения полей ввода

            #endregion Настройка контрола

            ResumeLayout(false);

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Закрытые

        /// <summary>
        /// Получение пути и имени файла класса
        /// </summary>
        private string mFilePath([CallerFilePath] string filePath = "")
        {
            return filePath;
        }
        /// <summary>
        /// Получение значения номера строки в текущей процедуре
        /// </summary>
        private int mLine(string message = "", [CallerLineNumber] int line = 0)
        {
            return line;
        }
        /// <summary>
        /// Получение названия текущей процедуры
        /// </summary>
        private string mProcedure(string message, [CallerMemberName] string member = "")
        {
            return member;
        }

        #endregion Закрытые

        #region - Процедуры

        /// <summary>
        /// Загрузка данных из источника данных
        /// </summary>
        /// <returns>[true] - данные загружены, иначе - [false]</returns>
        public virtual bool __mDataLoad()
        {
            bool vReturn = true; // Возвращаемое значение

            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fFilePath_ = _fClassFilePath_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fMessage_ = "Не удалось загрузить данные";

            if (__oDataTable == null)
            {
                _fError.__mReasonAdd("не определены данные");
                elmApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return false;
            }
            if (__oDataTable.Rows.Count != 1)
            {
                _fError.__mReasonAdd("не верно указаны данные. Должна быть одна запись");
                elmApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return false;
            }
            /// Заполнение компонентов данными
            foreach (Control vInput in Controls)
            {
                /// Исключение блока вкладок
                if ((vInput is elmComponentPagesBlock) == true)
                    continue;
                /// Исключение компонентов с неуказанным названием поля или объекта - {null}
                if ((vInput as elmInput).__fFieldName == "")
                    continue;
                if ((vInput is elmInputBool) == true)
                {
                    (vInput as elmInputBool).__fValue_ = Convert.ToBoolean(__oDataTable.Rows[0][(vInput as elmInput).__fFieldName]);
                    continue;
                }
                if ((vInput is elmInputCombo) == true)
                {
                    (vInput as elmInputCombo).__fValue_ = Convert.ToInt32(__oDataTable.Rows[0][(vInput as elmInput).__fFieldName]);
                    continue;
                }
                if ((vInput is elmInputDateTime) == true)
                {
                    (vInput as elmInputDateTime).__fValue_ = Convert.ToDateTime(__oDataTable.Rows[0][(vInput as elmInput).__fFieldName]);
                    continue;
                }
                if ((vInput is elmInputFormCode) == true)
                {
                    (vInput as elmInputFormCode).__fValue_ = Convert.ToInt32(__oDataTable.Rows[0][(vInput as elmInput).__fFieldName]);
                    continue;
                }
                if ((vInput is elmInputFormName) == true)
                {
                    if (Convert.ToInt32((vInput as elmInputFormName).__fValue_) == 0)
                    {
                        (vInput as elmInputFormName).__fValue_ = Convert.ToInt32(__oDataTable.Rows[0][(vInput as elmInput).__fFieldName]);
                    }
                    continue;
                }
                if ((vInput is elmInputNumeric) == true)
                {
                    (vInput as elmInputNumeric).__fValue_ = Convert.ToDecimal(__oDataTable.Rows[0][(vInput as elmInput).__fFieldName]);
                    continue;
                }
                if ((vInput is elmInputInteger) == true)
                {
                    (vInput as elmInputInteger).__fValue_ = Convert.ToInt32(__oDataTable.Rows[0][(vInput as elmInput).__fFieldName]);
                    continue;
                }
                if ((vInput is elmInputQuote) == true)
                {
                    (vInput as elmInputQuote).__fValue_ = Convert.ToInt32(__oDataTable.Rows[0][(vInput as elmInput).__fFieldName]);
                    //(vInput as elmInputQuote).__mDataLoad();

                    continue;
                }
                if ((vInput is elmInputString) == true)
                {
                    (vInput as elmInputString).__fValue_ = Convert.ToString(__oDataTable.Rows[0][(vInput as elmInput).__fFieldName]);
                    continue;
                }
                if ((vInput is elmInputPathFile) == true)
                {
                    (vInput as elmInputPathFile).__fValue_ = Convert.ToString(__oDataTable.Rows[0][(vInput as elmInput).__fFieldName]);
                    continue;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Сохранение данных в источнике данных
        /// </summary>
        /// <returns>[true] - данные сохранены, иначе - [false]</returns>
        public virtual bool __mDataSave()
        {
            bool vReturn = true; // Возвращаемое значение
            /// Запись значений в таблицу
            if (__oDataTable != null)
            {
                foreach (Control vInput in Controls)
                {
                    if ((vInput is elmInput) == true)
                    {
                        if ((vInput as elmInput).__fFieldName.Length == 0)
                            continue;
                        if ((vInput is elmInputBool) == true)
                        {
                            __oDataTable.Rows[0][(vInput as elmInputBool).__fFieldName] = (vInput as elmInputBool).__fValue_;
                            continue;
                        }
                        if ((vInput is elmInputCombo) == true)
                        {
                            __oDataTable.Rows[0][(vInput as elmInputCombo).__fFieldName] = (vInput as elmInputCombo).__fValue_;
                            continue;
                        }
                        if ((vInput is elmInputDateTime) == true)
                        {
                            __oDataTable.Rows[0][(vInput as elmInputDateTime).__fFieldName] = (vInput as elmInputDateTime).__fValue_;
                            continue;
                        }
                        if ((vInput is elmInputFormCode) == true)
                        {
                            __oDataTable.Rows[0][(vInput as elmInputFormCode).__fFieldName] = (vInput as elmInputFormCode).__fValue_;
                            continue;
                        }
                        if ((vInput is elmInputFormName) == true)
                        {
                            __oDataTable.Rows[0][(vInput as elmInputFormName).__fFieldName] = (vInput as elmInputFormName).__fValue_;
                            continue;
                        }
                        if ((vInput is elmInputNumeric) == true)
                        {
                            __oDataTable.Rows[0][(vInput as elmInputNumeric).__fFieldName] = (vInput as elmInputNumeric).__fValue_;
                            continue;
                        }
                        if ((vInput is elmInputInteger) == true)
                        {
                            __oDataTable.Rows[0][(vInput as elmInputInteger).__fFieldName] = (vInput as elmInputInteger).__fValue_;
                            continue;
                        }
                        if ((vInput is elmInputQuote) == true)
                        {
                            (vInput as elmInputQuote).__mDataSave(); // __oDataTable.TableName
                            __oDataTable.Rows[0][(vInput as elmInputQuote).__fFieldName] = (vInput as elmInputQuote).__fValue_;
                            continue;
                        }
                        if ((vInput is elmInputPathFile) == true)
                        {
                            if ((vInput as elmInputPathFile).__fValue_.ToString().Length != 0 & Convert.ToInt32(__oDataTable.Rows[0]["CLU"]) == 0)
                            {
                                byte[] fileData = File.ReadAllBytes((vInput as elmInputPathFile).__fValue_.ToString());

                                string fileDataHex = "0x" + BitConverter.ToString(fileData).Replace("-", "");
                                __oDataTable.Rows[0][(vInput as elmInputPathFile).__fFieldName] = System.Text.Encoding.Default.GetBytes(fileDataHex);
                            }
                        }
                        if ((vInput is elmInputString) == true)
                        {
                            try
                            {
                                __oDataTable.Columns[(vInput as elmInputString).__fFieldName].ReadOnly = false; /// Для примечаний !!!
                                __oDataTable.Rows[0][(vInput as elmInputString).__fFieldName] = (vInput as elmInputString).__fValue_;
                            } catch { }

                            continue;
                        }
                        if ((vInput is elmInputPathFile) == true)
                        {
                            __oDataTable.Rows[0][(vInput as elmInputPathFile).__fFieldName] = (vInput as elmInputPathFile).__fValue_;
                            continue;
                        }
                    }
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Получение единого выражения фильтра на основе состояния компонентов
        /// </summary>
        /// <returns></returns>
        public virtual string _mFilterBuild()
        {
            string vReturn = ""; // Возвращаемое значение

            /// Перебор установленных компонентов фильтра
            foreach (elmInput vInput in Controls)
            {
                if (vInput.__fMarkStatus_ == true)
                {
                    if (vInput.__fFieldName.Length == 0)
                    {
                        _fError.__fErrorType_ = ERRORSTYPES.Programming;
                        _fError.__fFilePath_ = _fClassFilePath_;
                        _fError.__fLineInProcedure_ = _fClassLine_;
                        _fError.__mMessageBuild("Не указана имя поля для компоненты ввода");
                        _fError.__fProcedure_ = _fClassProcedure_;
                        _fError.__mReasonAdd("Компонент ввода: {0}", vInput.Name);
                        elmApplication.__oErrorsHandler.__mShow(_fError);
                        _fError.__mClear();

                        return vReturn;
                    }
                    if (string.IsNullOrEmpty(vReturn) == false)
                    {
                        vReturn = vReturn + " and ";
                    }
                    vReturn = vReturn + vInput.__fFilterExpression_;
                }
            } /// Перебор установленных компонентов фильтра

            return vReturn;
        }
        /// <summary>
        /// Загрузка настроек фильтра из файла
        /// </summary>
        /// <returns>[true] - данные загружены, иначе - [false]</returns>
        public virtual bool _mFilterLoad()
        {
            bool vReturn = true; // Возвращаемое значение
            appFileIni oFileIni = (FindForm() as elmForm).__oFileIni; // Объект для работы с инициализационным файлом
            /// Не указано название формы для которой строиться фильтр
            if (__fFormParentName.Length == 0)
            {
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                _fError.__fFilePath_ = _fClassFilePath_;
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mMessageBuild("Не указана форма для которой строиться фильтр");
                _fError.__fProcedure_ = _fClassProcedure_;
                elmApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear(); 

                return false;
            }
            /// Перебор установленных компонентов фильтра
            foreach (Control vInput in Controls)
            {
                /// Компонент - поле ввода
                if ((vInput is elmInput) == true)
                {
                    if ((vInput as elmInput).__fFieldName.Length == 0)
                    {
                        _fError.__fErrorType_ = ERRORSTYPES.Programming;
                        _fError.__fFilePath_ = _fClassFilePath_;
                        _fError.__fLineInProcedure_ = _fClassLine_;
                        _fError.__mMessageBuild("Не указана имя поля для компоненты ввода");
                        _fError.__fProcedure_ = _fClassProcedure_;
                        _fError.__mReasonAdd("Компонент ввода: {0}", (vInput as elmInput).Name);
                        elmApplication.__oErrorsHandler.__mShow(_fError);
                        _fError.__mClear();

                        return false;
                    }
                    string vFieldName = (vInput as elmInput).__fFieldName; // Название поля для которого строиться фильтр
                    try
                    {
                        switch ((vInput as elmInput).Name)
                        {
                            case "appInputDateTimePeriod":
                            //(vInput as appInputDateTimePeriod)._cCheckFilterUsed.Checked = Convert.ToBoolean(oFileIni._mValueRead(_fFormParentName, "FilterStatus_" + vFieldName)); // Загрузка статуса
                            //(vInput as appInputDateTimePeriod)._cCheckFilterUseTo.Checked = Convert.ToBoolean(oFileIni._mValueRead(_fFormParentName, "FilterStatusTo_" + vFieldName)); // Загрузка статуса

                            //if ((vInput as appInputDateTimePeriod)._cInput._fValueInTicks == false)
                            //{
                            //    (vInput as appInputDateTimePeriod)._cInput.Value = Convert.ToDateTime(oFileIni._mValueRead(_fFormParentName, "FilterValue_" + vFieldName)); // Загрузка значений
                            //    (vInput as appInputDateTimePeriod)._cInputTo.Value = Convert.ToDateTime(oFileIni._mValueRead(_fFormParentName, "FilterValueTo_" + vFieldName)); // Загрузка значений
                            //} /// Данные храняться как дата-время
                            //else
                            //{
                            //    (vInput as appInputDateTimePeriod)._cInput.Value = new DateTime(Convert.ToInt64(oFileIni._mValueRead(_fFormParentName, "FilterValue_" + vFieldName))); // Загрузка значений
                            //    (vInput as appInputDateTimePeriod)._cInputTo.Value = new DateTime(Convert.ToInt64(oFileIni._mValueRead(_fFormParentName, "FilterValueTo_" + vFieldName))); // Загрузка значений
                            //} /// Данные храняться как тики
                            //break;
                            case "crlInputText":
                                (vInput as elmInputString).__fMarkStatus_ = Convert.ToBoolean(oFileIni.__mValueRead(__fFormParentName, "FilterStatus_" + vFieldName)); /// Загрузка статуса
                                // Воссстановить (vInput as crlInputText)._fValue = oFileIni._mValueRead(_fFormParentName, "FilterValue_" + vFieldName); /// Загрузка значения
                                break;
                        }
                    }
                    catch
                    {
                        (vInput as elmInput).__fMarkStatus_ = false; // Первая загрузка статуса
                    }
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Сохранение настроек фильтра в файл
        /// </summary>
        /// <returns>[true] - данные загружены, иначе - [false]</returns>
        public virtual bool _mFilterSave()
        {
            __fFormFilterExpression = ""; // Сформированное условие фильтра
            __fFormFilterMessage = ""; // Условие фильтра отображаемое пользователю

            bool vReturn = true; // Возвращаемое значение
            appFileIni oFileIni = (FindForm() as elmForm).__oFileIni; // Объект для работы с инициализационным файлом

            if (__fFormParentName.Length == 0)
            {
                __fFormParentName = FindForm().Name;
            } /// Не указано название формы для которой строиться фильтр

            foreach (Control vInput in Controls)
            { /// Перебор установленных компонентов фильтра
                string vFieldName = (vInput as elmInput).__fFieldName; // Название поля для которого строиться фильтр

                if ((vInput is elmInput) == true)
                { /// Компонент - поле ввода

                    oFileIni.__mValueWrite((vInput as elmInput).__fMarkVisible_.ToString(), __fFormParentName, "FilterStatus_" + vFieldName); /// Сохранение статуса использования текущего компонента
                    oFileIni.__mValueWrite((vInput as elmInput).__fCaption_, __fFormParentName, "FilterCaption_" + vFieldName); /// Сохранение заголовка текущего компонента
                    oFileIni.__mValueWrite((vInput as elmInput).__fFilterExpression_, __fFormParentName, "FilterExpression_" + vFieldName); /// Сохранение условия фильтра текущего компонента
                    oFileIni.__mValueWrite((vInput as elmInput).__fFilterMessage_, __fFormParentName, "FilterMessage_" + vFieldName); /// Сохранение выражение фильтра текущего компонента
                    // Восстановить oFileIni._mValueWrite((vInput as crlInputText)._fValue.ToString(), _fFormParentName, "FilterValue_" + vFieldName); /// Сохранение значения текущего компонента

                    string vFilterExpression = (vInput as elmInput).__fFilterExpression_.Trim(); // 
                    string vFilterMessage = (vInput as elmInput).__fFilterMessage_.Trim(); // 
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
                                __fFormFilterMessage = __fFormFilterMessage + "\n";
                            }
                        }
                    } /// Собрание всех условий в единый фильтр

                } /// Компонент - поле ввода
            } /// Перебор установленных компонентов фильтра

            return vReturn;
        }
        /// <summary>
        /// Добавление поля ввода
        /// </summary>
        /// <param name="pInput">Поле ввода</param>
        /// <returns>[true] - поле ввода добавлено, иначе - [false]</returns>
        //public virtual bool __mInputAdd(crlInput pInput, AnchorStyles pAnchorStyles = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right)
        public virtual bool __mInputAdd(elmInput pInput, int pHeight = 25)
        {
            bool vReturn = true; // Возвращаемое значение
            bool vSeached = false; // Добавляемый элемент уже обнаружен в области

            /// Добавление разрыва между компонентами
            if (pInput == null)
            {
                __fTopCoordinate = __fTopCoordinate + 25;
                return true;
            }
            /// Добавление компонента для работы с сохраняемыми в базе данных файлами
            if (pInput is elmInputPathFile == true)
            {
                Controls.Add(pInput);
                pInput.__fMarkVisible_ = false;
                pInput.Visible = false;
                if (pInput.Height < pHeight)
                    pInput.Height = pHeight;
                pInput.Left = 0;
                pInput.Top = 0;
                pInput.Width = Width - elmInterface.__fIntervalHorizontal * 2;

                //__fTopCoordinate = __fTopCoordinate + pInput.Height + elmInterface.__fIntervalVertical;
            }
            /// Добавление компонента
            if (vSeached == false)
            {
                Controls.Add(pInput);
                pInput.__fMarkVisible_ = __fMarkShow;
                if (pInput.Height < pHeight)
                    pInput.Height = pHeight;
                pInput.Left = elmInterface.__fIntervalHorizontal;
                pInput.Top = __fTopCoordinate;
                pInput.Width = Width - elmInterface.__fIntervalHorizontal * 2;

                __fTopCoordinate = __fTopCoordinate + pInput.Height + elmInterface.__fIntervalVertical;
            }

            return vReturn;
        }
        /// <summary>
        /// Добавление панели вкладок
        /// </summary>
        /// <param name="pPageBlock"></param>
        /// <param name="pAnchorStyles"></param>
        public virtual void __mPageBlockAdd(elmComponentPagesBlock pPageBlock, AnchorStyles pAnchorStyles = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right)
        {
            pPageBlock.Name = "_cPageBlock";
            pPageBlock.Top = __fTopCoordinate;
            pPageBlock.Left = elmInterface.__fIntervalHorizontal;
            pPageBlock.Width = Width - elmInterface.__fIntervalHorizontal * 2;
            pPageBlock.Anchor = pAnchorStyles; // !!!
            Controls.Add(pPageBlock);
            pPageBlock.Height = 150;
            __fTopCoordinate = __fTopCoordinate + pPageBlock.Height + elmInterface.__fIntervalVertical;
            //delete Width = fTop - crlInterface.__fIntervalVertical;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ 

        #region - Атрибуты

        /// <summary>
        /// Разрешение отображения галочки во всех добавляемых контролах
        /// </summary>
        public bool __fMarkShow = true;
        /// <summary>
        /// Название родительской формы, для которой строиться фильтр
        /// </summary>
        public string __fFormParentName = "";
        /// <summary>
        /// Сформированное условие фильтра
        /// </summary>
        public string __fFormFilterExpression = "";
        /// <summary>
        /// Условие фильтра отображаемое пользователю
        /// </summary>
        public string __fFormFilterMessage = "";
        /// <summary>
        /// Отклонение компонента от верхнего края
        /// </summary>
        public int __fTopCoordinate = elmInterface.__fIntervalHorizontal;

        #endregion Атрибуты

        #region - Объекты 

        /// <summary>
        /// Таблица с обрабатываемой записью
        /// </summary>
        public DataTable __oDataTable;

        #endregion Объекты

        #endregion ПОЛЯ
    }
}
