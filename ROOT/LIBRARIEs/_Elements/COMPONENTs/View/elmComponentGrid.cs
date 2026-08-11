using nlApplication;
using nlData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentBrowser.cs
    /// </summary>
    /// <remarks>Класс-компонент для просмотра табличных данных</remarks>
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 10-03</version> // Дата-время последней корректировки
    public class elmComponentGrid : DataGridView
    {
        #region = БИБЛИОТЕКИ

        [DllImport("user32.dll")]
        public static extern int SendMessage(ref IntPtr hWnd, int wMsg, ref bool wParam, Int32 lParam);

        #endregion БИБЛИОТЕКИ

        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public elmComponentGrid()
        {
            _mObjectAssembly();
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        {
            SuspendLayout();

            #region /// Настройка компонента

            AutoGenerateColumns = false;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToOrderColumns = true;
            AllowUserToResizeRows = true;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            DoubleBuffered = true; // Для ускорения перерисовки при подсветке строк
            MultiSelect = false;
            RowHeadersWidth = 25;

            // 1. Обязательно отключаем визуальные стили для заголовков
            EnableHeadersVisualStyles = false;

            // 2. Устанавливаем цвет фона заголовка
            ColumnHeadersDefaultCellStyle.BackColor = elmApplication.__oInterface.__mColor(COLORS.FormActive); //Color.Navy;

            // 3. Устанавливаем цвет текста заголовка
            ColumnHeadersDefaultCellStyle.ForeColor = elmApplication.__oInterface.__mColor(COLORS.Text); // Color.White;

            // 4. (Опционально) Настраиваем шрифт
            ColumnHeadersDefaultCellStyle.Font = elmApplication.__oInterface.__mFont(FONTS.TextTitle); //new Font("Arial", 10, FontStyle.Bold);

            EnableHeadersVisualStyles = false;

            RowHeadersDefaultCellStyle.BackColor = elmApplication.__oInterface.__mColor(COLORS.FormActive); // Color.Pink;

            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            EditMode = DataGridViewEditMode.EditOnEnter; //.EditOnF2; 

            VirtualMode = true; // Для выполнения сортировки связанных полей

            #endregion Настройка компонента

            ResumeLayout(false);

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected virtual void _mObjectPresentation()
        {
            __mSortingLoad();

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region * Информация о файле

        /// <summary>
        /// Получение пути и имени файла класса
        /// </summary>
        protected string _mClassFilePath([CallerFilePath] string pFilePath = "")
        {
            return pFilePath;
        }
        /// <summary>
        /// Получение значения номера строки в текущей процедуре
        /// </summary>
        protected int _mClassLine(string pMessage = "", [CallerLineNumber] int pLine = 0)
        {
            return pLine;
        }
        /// <summary>
        /// Получение названия текущей процедуры
        /// </summary>
        protected string _mClassProcedure(string message, [CallerMemberName] string pMember = "")
        {
            return pMember;
        }

        #endregion Информация о файле

        #region - Поведение

        /// <summary>
        /// Выполняется после создания объекта
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }
        /// <summary>
        /// Выполняется при клике мыши по заголовку колонки
        /// </summary>
        /// <param name="e"></param>
        protected override void OnColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e)
        {
            _fRecordClueBeforeSortChanged = __fRecordClue_;
            base.OnColumnHeaderMouseClick(e);
        }
        protected override void OnRowDirtyStateNeeded(QuestionEventArgs e)
        {
            base.OnRowDirtyStateNeeded(e);
            if (__fRowChanged != null)
                __fRowChanged(this, e);
        }
        /// <summary>
        /// Выполняется после изменения сортировки
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSorted(EventArgs e)
        {
            if (CurrentRow != null)
            {
                base.OnSorted(e);
                try
                {
                    __fRecordClue_ = _fRecordClueBeforeSortChanged;
                }
                catch { }
            }
        }
        /// <summary>
        /// Выполняется при изменении данных в ячейке сетки
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
        {
            base.OnCellValueChanged(e);
            if (__eCellValueChanged != null)
                __eCellValueChanged(this, new EventArgs());
        }
        protected override void OnHandleDestroyed(EventArgs e)
        {
            __mSortingSave();
            base.OnHandleDestroyed(e);
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Применить все изменения сделанные в сетке
        /// </summary>
        /// <param name="pColumnEditIndex">Номер редактируемой ячейки</param>
        public void __mAcceptChanges(int pColumnEditIndex)
        {
            int vRowIndexMax = Rows.Count - 1; // Индекс максимальной строки в сетке
            /// Перевод курсора вначале не первую ячейку последней записи, а затем на первую ячейку первой записи
            if (vRowIndexMax > 0)
            {
                DataGridViewCell vDataGridViewCell = Rows[vRowIndexMax].Cells[pColumnEditIndex];
                CurrentCell = vDataGridViewCell;
                CurrentCell.Selected = true;

                vDataGridViewCell = Rows[vRowIndexMax].Cells[pColumnEditIndex];
                CurrentCell = vDataGridViewCell;
                CurrentCell.Selected = true;
            }

            return;
        }
        /// <summary>
        /// Добавление колонки
        /// </summary>
        /// <param name="pCaption">Заголовок колонки</param>
        /// <param name="pPrompt">Подсказка при наведении курсора</param>
        /// <param name="pFieldName">Название поля</param>
        /// <param name="pReadOnly">Атрибут "Только чтение"</param>
        /// <param name="pVisible">Видимость колонки</param>
        /// <param name="pType">Вид колонки</param>
        /// <returns>[true] - Колонка добавлена, иначе - [false]</returns>
        public bool __mColumnAdd(string pCaption, string pPrompt, string pFieldName, bool pReadOnly, bool pVisible, DATAGRIDCOLUMNTYPE pType, GRIDCELLTYPE pCellStyle = GRIDCELLTYPE.Normal)
        {
            bool vReturn = true; // Возвращаемое значение

            DataGridViewCellStyle vDataGridViewCellStyle = new elmGridCellStyle(); ;
            switch (pCellStyle)
            {
                case GRIDCELLTYPE.NumericFractionalTwo:
                    vDataGridViewCellStyle = new elmGridCellStyleFractionalTwo();
                    break;
                case GRIDCELLTYPE.NumericFractionalThree:
                    vDataGridViewCellStyle = new elmGridCellStyleFractionalThree();
                    break;
            }
            elmUnitGridColumn vDataGridColumn = new elmUnitGridColumn();
            vDataGridColumn.__fCaption = pCaption;
            vDataGridColumn.__fField = pFieldName;
            vDataGridColumn.__fReadOnly = pReadOnly;
            vDataGridColumn.__fToolTipText = pPrompt;
            vDataGridColumn.__fType = pType;
            vDataGridColumn.__fVisible = pVisible;
            vDataGridColumn.__fCellStyle = vDataGridViewCellStyle;

            __fColumnsList.Add(vDataGridColumn);

            return vReturn;
        }
        /// <summary>
        /// Изменение видимости колонок сетки
        /// </summary>
        /// <param name="pFieldName">Название поля</param>
        /// <param name="pFieldVisible">Видимость поля</param>
        /// <returns></returns>
        public void __mColumnChangeVisible(string pFieldName, bool pFieldVisible)
        {
            foreach (elmUnitGridColumn vColumn in __fColumnsList)
            {
                if (vColumn.__fField == pFieldName)
                    vColumn.__fVisible = pFieldVisible;
            }

            return;
        }
        /// <summary>
        /// Добавление колонок в сетку
        /// </summary>
        /// <returns>[true] - колонки добавлены, иначе - [false]</returns>
        public bool __mColumnsBuild()
        {
            bool vReturn = true; // Возвращаемое значение
            elmForm vForm = this.FindForm() as elmForm; // Форма на которой расположен компонент

            foreach (elmUnitGridColumn vColumn in __fColumnsList)
            {
                if (vColumn.__fType == DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn)
                {
                    DataGridViewTextBoxColumn vDataColumn = new DataGridViewTextBoxColumn();
                    vDataColumn.Name = vColumn.__fField;
                    //if (vColumn.__fField.ToLower() == "chg" | vColumn.__fField.ToLower() == "clu" | vColumn.__fField.ToLower() == "cod" | vColumn.__fField.ToLower() == "dsi")
                    //    vDataColumn.Frozen = true;
                    vDataColumn.HeaderText = vColumn.__fCaption;
                    vDataColumn.DataPropertyName = vColumn.__fField;
                    vDataColumn.Visible = vColumn.__fVisible;
                    vDataColumn.ReadOnly = vColumn.__fReadOnly;
                    vDataColumn.ToolTipText = vColumn.__fToolTipText;
                    vDataColumn.DefaultCellStyle = vColumn.__fCellStyle;

                    Columns.Add(vDataColumn);
                }
                if (vColumn.__fType == DATAGRIDCOLUMNTYPE.DataGridViewCheckBoxColumn)
                {
                    DataGridViewCheckBoxColumn vDataColumn = new DataGridViewCheckBoxColumn();
                    vDataColumn.Name = vColumn.__fField;
                    vDataColumn.HeaderText = vColumn.__fCaption;
                    vDataColumn.DataPropertyName = vColumn.__fField;
                    vDataColumn.Visible = vColumn.__fVisible;
                    vDataColumn.ReadOnly = vColumn.__fReadOnly;
                    vDataColumn.ToolTipText = vColumn.__fToolTipText;

                    Columns.Add(vDataColumn);
                }
                if (vColumn.__fType == DATAGRIDCOLUMNTYPE.DataGridViewButtonColumn)
                {
                    DataGridViewButtonColumn vDataColumn = new DataGridViewButtonColumn();
                    vDataColumn.Name = vColumn.__fField;
                    vDataColumn.HeaderText = vColumn.__fCaption;
                    vDataColumn.DataPropertyName = vColumn.__fField;
                    vDataColumn.Visible = vColumn.__fVisible;
                    vDataColumn.ReadOnly = vColumn.__fReadOnly;
                    vDataColumn.ToolTipText = vColumn.__fToolTipText;

                    Columns.Add(vDataColumn);
                }
            }

            return vReturn;
        }
        ///// <summary>
        ///// Загрузка данных
        ///// </summary>
        ///// <param name="pRecordClue">Идентификатор записи который должен быть отображен</param>
        //public virtual bool __mDataLoad(int pRecordClue)
        //{
        //    return false;
        //}
        /// <summary>
        /// Загрузка данных
        /// </summary>
        /// <param name="pExpressionWhere">Условия выбора данных</param>
        /// <param name="pExpressionOrder">Поле сортировки данных </param>
        /// <param name="pRecordCount">Количество возвращаемых данных. -1 - все данные</param>
        /// <returns>[true] - данные загружены, иначе - [false]</returns>
        public bool __mDataLoad(string pExpressionWhere, string pExpressionOrder, int pRecordCount = -1)
        {
            bool vReturn = true; // Возвращаемое значение
            int vCurrentRowIndex = -1; // Индекс текущей записи
            /// Чтение фильтра из файла

            //https://overcoder.net/q/585303/c-%D0%BE%D1%87%D0%B5%D0%BD%D1%8C-%D0%BC%D0%B5%D0%B4%D0%BB%D0%B5%D0%BD%D0%BD%D0%BE-%D0%B7%D0%B0%D0%BF%D0%BE%D0%BB%D0%BD%D1%8F%D0%B5%D1%82-datagridview

            string vFilter = __fFilterConstant; // Фильтр для выбора данных из источника данных

            /// Если указан загруженный фильтр
            if (_fFilterExpression.Length > 0)
            {
                /// Если фильтр уже частично заполнен
                if (vFilter.Length > 0)
                {
                    vFilter = vFilter + " and " + _fFilterExpression;
                }
                else
                { /// Иначе
                    vFilter = _fFilterExpression;
                }
            }
            /// Указан фильтр в параметрах метода
            if (pExpressionWhere.Length > 0)
            {
                /// Фильтр уже частично заполнен
                if (vFilter.Length > 0)
                {
                    vFilter = vFilter + " and " + pExpressionWhere;
                }
                /// Фильтр не заполнен
                else
                {
                    vFilter = pExpressionWhere;
                }
            }

            /// Если есть выделенная строка, Сохраняем ее индекс
            if (CurrentRow != null)
            {
                vCurrentRowIndex = CurrentRow.Index; // Индекс текущей записи
            }

            if (__oEssence != null)
            {
                __oDataTable = __oEssence.__mGrid(vFilter, pExpressionOrder);

                DataSource = __oDataTable;
            }
            else
            {
                _fError.__fProcedure_ = _fClassProcedure_;
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fMessage_ = "Источник данных не определен";
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return false;
            }
            /// Если была выбрана запись, возвращаем курсор на нее
            if (vCurrentRowIndex > 0)
            {
                /// Если индекс предыдущей записи больше или равно количеству записей в сетке
                if (Rows.Count > vCurrentRowIndex)
                {
                    CurrentCell = Rows[vCurrentRowIndex].Cells.OfType<DataGridViewCell>().First(c => c.Visible);
                }
            }

            Focus();

            return vReturn;
        }
        /// <summary>
        /// Обновление сетки с условиями фильтра
        /// </summary>
        /// <param name="pFilter"></param>
        public void __mRefresh(string pFilter)
        {
            __oDataTable.DefaultView.RowFilter = pFilter;

            return;
        }
        /// <summary>
        /// Удаление данных
        /// </summary>
        /// <returns>[true] - данные удалены, иначе - [false]</returns>
        public bool __mRecordDelete()
        {
            bool vReturn = false; // Возвращаемое значение

            if (CurrentRow != null)
            {
                DataTable vDataTable = DataSource as DataTable;
                DataRow vDataRow = (this.Rows[SelectedRows[0].Index].DataBoundItem as DataRowView).Row;
                //int vRowIndex = SelectedRows[0].Index;
                vDataRow["ELD"] = 1;
                vReturn = __oEssence.__mUpdate(vDataTable);
            }

            return vReturn;
        }
        /// <summary>
        /// Восстановление данных
        /// </summary>
        public bool __mRecordRestore()
        {
            bool vReturn = false; // Возвращаемое значение

            if (CurrentRow != null)
            {
                DataTable vDataTable = DataSource as DataTable;
                DataRow vDataRow = (this.Rows[SelectedRows[0].Index].DataBoundItem as DataRowView).Row;
                //int vRowIndex = SelectedRows[0].Index;
                vDataRow["ELD"] = 0;
                vReturn = __oEssence.__mUpdate(vDataTable);
            }

            return vReturn;
        }
        /// <summary>
        /// Выполнение сортировки
        /// </summary>
        /// <param name="pColumnIndex">Индекс колонки по которой должна быть выполнена сортировка</param>
        /// <param name="pDirection">Направление сортироки ASCE, DESC</param>
        public void __mSorting(int pColumnIndex, string pDirection)
        {
            Columns[pColumnIndex].SortMode = DataGridViewColumnSortMode.Programmatic; // Установка программного режима сортировки

            if (pDirection.Substring(0, 4).ToUpper() == "ASCE")
                Sort(Columns[pColumnIndex], ListSortDirection.Ascending);
            if (pDirection.Substring(0, 4).ToUpper() == "DESC")
                Sort(Columns[pColumnIndex], ListSortDirection.Descending);

            Columns[pColumnIndex].SortMode = DataGridViewColumnSortMode.Automatic; // Установка пользовательского режима сортировки

            return;
        }
        /// <summary>
        /// Загрузка сортировки в сетку
        /// </summary>
        public void __mSortingLoad()
        {
            appFileIni vFileIni = (FindForm() as elmForm).__oFileIni; // Объект для работы с инициализационными файлами
            vFileIni.__fFilePath = elmApplication.__oPathes.__mFileFormTunes(); // Указание настроечного файла
            string vFormName = (FindForm() as elmForm).__fClassName_; // Название формы на которой расположен компонент

            try
            {
                string vSortColumnNumber = vFileIni.__mValueRead(vFormName, "SortColumnIndex"); // Номер колонки по которой будет выполнена сортировка
                string vSortDirection = vFileIni.__mValueRead(vFormName, "SortDirection"); // Направление сортировки в колонке
                if (Convert.ToInt32(vSortColumnNumber) > 0)
                    __mSorting(Convert.ToInt32(vSortColumnNumber), vSortDirection);
            }
            catch { }
            try
            {
                __fRecordClue_ = Convert.ToInt32(vFileIni.__mValueRead(vFormName, "LastClue")); // Идентификатор последней выбранной записи
            }
            catch { }

            return;
        }
        /// <summary>
        /// Сохранение сортировки в сетке
        /// </summary>
        public void __mSortingSave()
        {
            appFileIni vFileIni = (FindForm() as elmForm).__oFileIni; // Объект для работы с инициализационными файлами
            vFileIni.__fFilePath = elmApplication.__oPathes.__mFileFormTunes(); // Указание настроечного файла
            string vFormName = (FindForm() as elmForm).Name; // Название формы на которой расположен компонент

            vFileIni.__mValueWrite(__fSortColumnIndex_.ToString(), vFormName, "SortColumnIndex");
            vFileIni.__mValueWrite(__fSortColumnDirection_, vFormName, "SortDirection");
            if (DataSource != null)
                if ((DataSource as DataTable).Columns.Count > 0)
                    if ((DataSource as DataTable).Columns[0].ColumnName == "CLU")
                        vFileIni.__mValueWrite(__fRecordClue_.ToString(), vFormName, "LastClue"); // Идентификатор последней выбранной записи

            return;
        }
        /// <summary>
        /// Получение значения поля курсора в текущей ячейке
        /// </summary>
        /// <param name="pFieldName">Название поля курсора</param>
        /// <returns></returns>
        public object __mCurrentRowFieldValue(string pFieldName)
        {
            return ((DataRowView)this.SelectedRows[0].DataBoundItem).Row[pFieldName];
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Список отображаемых колонок
        /// </summary>
        public List<elmUnitGridColumn> __fColumnsList = new List<elmUnitGridColumn>();
        /// <summary>
        /// Вид формы вызываемой для правки записи
        /// </summary>
        public CONTROLsOPENEDTYPES __fFormOpenedType = CONTROLsOPENEDTYPES.FormRecord;
        /// <summary>
        /// Постоянно подключенный фильтр. Например тема файлов
        /// </summary>
        public string __fFilterConstant = "";
        /// <summary>
        /// Курсор с данными
        /// </summary>
        public DataTable __oDataTable = null;

        #endregion Атрибуты

        #region - Внутренние 

        /// <summary>
        /// Объект ошибки
        /// </summary>
        protected appUnitError _fError;
        /// <summary>
                                            /// Выражения фильтра данных
                                            /// </summary>
        protected string _fFilterExpression = "";
        /// <summary>
        /// Идентификатор записи перед изменением сортировки
        /// </summary>
        protected int _fRecordClueBeforeSortChanged = 0;

        #endregion Внутренние

        #region - Константы

        private const int WM_SETREDRAW = 11;

        #endregion Константы

        #region - Объекты

        /// <summary>
        /// Сушность данных
        /// </summary>
        public datUnitEssence __oEssence;
        /// <summary>
        /// Вид формы для построения фильтра
        /// </summary>
        public Type __oFormFilter;
        /// <summary>
        /// Вид формы для изменения записи
        /// </summary>
        public Type __oFormOpened;

        #endregion Объекты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        #region - Скрытые

        /// <summary>
        /// Путь к файлу текущего класса
        /// </summary>
        protected string _fClassFilePath_
        {
            get { return _mClassFilePath(); }
        }
        /// <summary>
        /// Текущая процедура класса
        /// </summary>
        protected string _fClassProcedure_
        {
            get { return _mClassProcedure(""); }
        }
        /// <summary>
        /// Номер строки в файле класса
        /// </summary>
        protected int _fClassLine_
        {
            get { return _mClassLine(""); }
        }

        #endregion Скрытые

        /// <summary>
        /// Идентификатор текущей записи
        /// </summary>
        public int __fRecordClue_
        {
            get
            {
                //Select(); Потребовалось удаление при использование OnRowEnter

                if (Rows.Count > 0 & CurrentRow != null)
                {
                    int vClue;
                    return Int32.TryParse(Convert.ToString(this[0, CurrentRow.Index].Value), out vClue) ? vClue : -1;
                }
                return -1;
            }
            set
            {
                Select();

                if (value > 0)
                {
                    if (Columns.Count > 0)
                    {
                        try
                        {
                            foreach (DataGridViewRow vGridRow in Rows)
                            {
                                foreach (DataGridViewColumn vGridColumn in Columns)
                                {
                                    if (vGridColumn.Visible == true & (Int32)vGridRow.Cells["CLU"].Value == value)
                                    {
                                        CurrentCell = vGridRow.Cells[1];
                                        break;
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
        }
        /// <summary>
        /// Индекс колонки по которой выполнена сортировка
        /// </summary>
        public int __fSortColumnIndex_
        {
            get
            {
                if (SortedColumn != null)
                    return SortedColumn.Index;
                else
                    return 0;
            }
        }
        /// <summary>
        /// Направление сортировки в колонке по которой выполнена сортировка
        /// </summary>
        public string __fSortColumnDirection_
        {
            get
            {
                if (SortedColumn != null)
                    return SortedColumn.HeaderCell.SortGlyphDirection.ToString();
                else
                    return "Ascending";
            }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при изменении данных в ячейке сетки
        /// </summary>
        public event EventHandler __eCellValueChanged;
        /// <summary>
        /// Возникает после смены строки в сетке
        /// </summary>
        public event EventHandler __fRowChanged;

        #endregion СОБЫТИЯ
    }
    /// <summary>
    /// Файл kvtGridStyleRows.cs
    /// </summary>
    /// <remarks>Класс настроек строк в сетке</remarks>
    public class elmGridCellStyle : DataGridViewCellStyle
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Контсруктор
        /// </summary>
        public elmGridCellStyle()
        {
            Alignment = DataGridViewContentAlignment.MiddleLeft;
            BackColor = elmApplication.__oInterface.__mColor(COLORS.DataBack);
            Font = elmApplication.__oInterface.__mFont(FONTS.Data);
            ForeColor = elmApplication.__oInterface.__mColor(COLORS.Text);
            //SelectionBackColor = elmApplication.__oInterface.__mColor(COLORS.DataBackDisabled);
            //SelectionForeColor = elmApplication.__oInterface.__mColor(COLORS.DataBackNecessarily);
            WrapMode = DataGridViewTriState.False;
        }

        #endregion ДИЗАЙНЕРЫ
    }
    /// <summary>
    /// Файл elmGridStyleRowsFractionalTwo.cs
    /// </summary>
    /// <remarks>Класс настроек строк в сетке</remarks>
    public class elmGridCellStyleFractionalTwo : DataGridViewCellStyle
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Контсруктор
        /// </summary>
        public elmGridCellStyleFractionalTwo()
        {
            Alignment = DataGridViewContentAlignment.MiddleRight;
            BackColor = elmApplication.__oInterface.__mColor(COLORS.DataBack);
            Font = elmApplication.__oInterface.__mFont(FONTS.Data);
            ForeColor = elmApplication.__oInterface.__mColor(COLORS.Text);
            Format = "#.#0";
            //SelectionBackColor = elmApplication.__oInterface.__mColor(COLORS.DataBackDisabled);
            //SelectionForeColor = elmApplication.__oInterface.__mColor(COLORS.DataBackNecessarily);
            WrapMode = DataGridViewTriState.False;
        }

        #endregion ДИЗАЙНЕРЫ
    }
    /// <summary>
    /// Файл elmGridStyleRowsFractionalThree.cs
    /// </summary>
    /// <remarks>Класс настроек строк в сетке</remarks>
    public class elmGridCellStyleFractionalThree : DataGridViewCellStyle
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Контсруктор
        /// </summary>
        public elmGridCellStyleFractionalThree()
        {
            Alignment = DataGridViewContentAlignment.MiddleRight;
            BackColor = elmApplication.__oInterface.__mColor(COLORS.DataBack);
            Font = elmApplication.__oInterface.__mFont(FONTS.Data);
            ForeColor = elmApplication.__oInterface.__mColor(COLORS.Text);
            Format = "#.##0";
            //SelectionBackColor = elmApplication.__oInterface.__mColor(COLORS.DataBackDisabled);
            //SelectionForeColor = elmApplication.__oInterface.__mColor(COLORS.DataBackNecessarily);
            WrapMode = DataGridViewTriState.False;
        }

        #endregion ДИЗАЙНЕРЫ
    }
}
