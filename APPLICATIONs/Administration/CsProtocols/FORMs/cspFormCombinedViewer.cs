using nlApplication;
using nlCsProtocols;
using nlDataSourceSqlite;
using nlElements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace naCsProtocols
{
    /// <summary>
    /// Файл cspFormCombinedViewer.cs
    /// </summary>
    /// <remarks>Форма 3 - совмещённый просмотр: слева список записей протоколов ('PclRrd', с учётом фильтров),
    /// справа - заголовок протокола ('Pcl'), которому принадлежит выбранная слева запись.</remarks>

    public class cspFormCombinedViewer : elmForm
    {
        #region = МЕТОДЫ

        #region - Объект

        protected override void _mObjectAssembly()
        {
            base._mObjectAssembly();

            __fCaption_ = "Совмещённый просмотр протоколов";
            ClientSize = new Size(1200, 700);

            _oDataSource = dsqProtocols.__oViewing_;

            __cPanelStatus.__fCaption_ = _oDataSource != null
                ? "База данных: " + System.IO.Path.Combine(_oDataSource.__fDatabasePath, _oDataSource.__fDatabaseName)
                : "База данных не открыта. Откройте протокол через \"Файл / Открыть протокол\" в главном окне.";

            Controls.Add(_cBlockFormMain);
            _cBlockFormMain.Controls.Add(_cSplitter);
            _cBlockFormMain.Controls.SetChildIndex(_cSplitter, 0);

            mFilterPanelBuild();
            mGridsBuild();

            mFiltersPopulate();
            mProtocolsLoad();
            mFilterPanelInheritFormColor();
        }

        /// <summary>
        /// Панель фильтров - TableLayoutPanel (не абсолютные координаты), несколько строк
        /// </summary>
        private void mFilterPanelBuild()
        {
            // Та же сетка, что в cspFormMain: подпись | поле | подпись | поле, строки по 30 px
            _cPanelFilters.Dock = DockStyle.Fill;
            _cPanelFilters.Margin = new Padding(0);
            _cPanelFilters.Padding = new Padding(4);
            // BackColor не задаём явно — наследует цвет формы (elmForm), как соседние области
            _cPanelFilters.ColumnCount = 4;
            _cPanelFilters.RowCount = 8;
            _cPanelFilters.ColumnStyles.Clear();
            _cPanelFilters.RowStyles.Clear();
            _cPanelFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            _cPanelFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            _cPanelFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            _cPanelFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            for (int i = 0; i < 7; i++)
                _cPanelFilters.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            _cPanelFilters.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F)); // виды протокола (чекбоксы)

            // Строка 0: процедура / сообщение
            mAddFilterLabel("Процедура:", 0, 0);
            _cFilterProcedure.Dock = DockStyle.Fill;
            _cFilterProcedure.TextChanged += mFilter_Changed;
            _cPanelFilters.Controls.Add(_cFilterProcedure, 1, 0);

            mAddFilterLabel("Сообщение:", 2, 0);
            _cFilterMessage.Dock = DockStyle.Fill;
            _cFilterMessage.TextChanged += mFilter_Changed;
            _cPanelFilters.Controls.Add(_cFilterMessage, 3, 0);

            // Строка 1: решение пользователя (чекбокс на всю ширину полей)
            mAddFilterLabel("Решение:", 0, 1);
            _cFilterUserSolution.Text = "Только решения пользователя (Ответ)";
            _cFilterUserSolution.Dock = DockStyle.Fill;
            _cFilterUserSolution.CheckedChanged += mFilter_Changed;
            _cPanelFilters.Controls.Add(_cFilterUserSolution, 1, 1);
            _cPanelFilters.SetColumnSpan(_cFilterUserSolution, 3);

            // Строка 2: приложение / пользователь
            mAddFilterLabel("Приложение:", 0, 2);
            _cFilterApp.Dock = DockStyle.Fill;
            _cFilterApp.DropDownStyle = ComboBoxStyle.DropDownList;
            _cFilterApp.SelectedIndexChanged += mFilter_Changed;
            _cPanelFilters.Controls.Add(_cFilterApp, 1, 2);

            mAddFilterLabel("Пользователь:", 2, 2);
            _cFilterUser.Dock = DockStyle.Fill;
            _cFilterUser.DropDownStyle = ComboBoxStyle.DropDownList;
            _cFilterUser.SelectedIndexChanged += mFilter_Changed;
            _cPanelFilters.Controls.Add(_cFilterUser, 3, 2);

            // Строка 3: компьютер
            mAddFilterLabel("Компьютер (хост):", 0, 3);
            _cFilterHost.Dock = DockStyle.Fill;
            _cFilterHost.DropDownStyle = ComboBoxStyle.DropDownList;
            _cFilterHost.SelectedIndexChanged += mFilter_Changed;
            _cPanelFilters.Controls.Add(_cFilterHost, 1, 3);
            _cPanelFilters.SetColumnSpan(_cFilterHost, 3);

            // Строка 4: период с / по (как в главной форме)
            mAddFilterLabel("Период с:", 0, 4);
            Panel vDateFromPanel = new Panel();
            vDateFromPanel.Dock = DockStyle.Fill;
            _cFilterDateEnabled.Text = "";
            _cFilterDateEnabled.Dock = DockStyle.Left;
            _cFilterDateEnabled.Width = 20;
            _cFilterDateEnabled.Checked = false;
            _cFilterDateEnabled.CheckedChanged += mFilterDateEnabled_Changed;
            _cFilterDateFrom.Dock = DockStyle.Fill;
            _cFilterDateFrom.Format = DateTimePickerFormat.Short;
            _cFilterDateFrom.Value = DateTime.Today.AddDays(-30);
            _cFilterDateFrom.Enabled = false;
            _cFilterDateFrom.ValueChanged += mFilter_Changed;
            vDateFromPanel.Controls.Add(_cFilterDateFrom);
            vDateFromPanel.Controls.Add(_cFilterDateEnabled);
            _cPanelFilters.Controls.Add(vDateFromPanel, 1, 4);

            mAddFilterLabel("по:", 2, 4);
            _cFilterDateTo.Dock = DockStyle.Fill;
            _cFilterDateTo.Format = DateTimePickerFormat.Short;
            _cFilterDateTo.Value = DateTime.Today;
            _cFilterDateTo.Enabled = false;
            _cFilterDateTo.ValueChanged += mFilter_Changed;
            _cPanelFilters.Controls.Add(_cFilterDateTo, 3, 4);

            // Строка 5: сброс
            _cButtonClear.Text = "Сбросить фильтры";
            _cButtonClear.Dock = DockStyle.Fill;
            _cButtonClear.Click += mButtonClear_Click;
            _cPanelFilters.Controls.Add(_cButtonClear, 0, 5);
            _cPanelFilters.SetColumnSpan(_cButtonClear, 4);

            // Строка 6–7: виды протокола (мультивыбор, как раньше, но в той же сетке)
            mAddFilterLabel("Вид протокола:", 0, 6);
            _cLabelType.Text = "(пусто = все)";
            _cLabelType.Dock = DockStyle.Fill;
            _cLabelType.TextAlign = ContentAlignment.MiddleLeft;
            _cPanelFilters.Controls.Add(_cLabelType, 1, 6);
            _cPanelFilters.SetColumnSpan(_cLabelType, 3);

            _cFilterType.Dock = DockStyle.Fill;
            _cFilterType.CheckOnClick = true;
            _cFilterType.MultiColumn = true;
            _cFilterType.ItemCheck += mFilterType_ItemCheck;
            _cPanelFilters.Controls.Add(_cFilterType, 0, 7);
            _cPanelFilters.SetColumnSpan(_cFilterType, 4);

            // Подгонка цвета под форму: без перекраски сеток в белый
            mFilterPanelInheritFormColor();
        }

        /// <summary>
        /// Цвета из темы интерфейса (elmApplication.__oInterface / COLORS) —
        /// тот же источник, что у elmForm, elmComponentCombo, elmInput*.
        /// FormActive = фон формы/панелей; DataBack = фон полей ввода.
        /// </summary>
        private void mFilterPanelInheritFormColor()
        {
            Color vForm = elmApplication.__oInterface.__mColor(COLORS.FormActive);
            Color vDataBack = elmApplication.__oInterface.__mColor(COLORS.DataBack);
            Color vText = elmApplication.__oInterface.__mColor(COLORS.Text);

            _cPanelFilters.BackColor = vForm;
            _cLeftHost.BackColor = vForm;
            _cFilterType.BackColor = vForm;
            _cFilterUserSolution.BackColor = vForm;
            _cFilterDateEnabled.BackColor = vForm;
            _cLabelType.BackColor = vForm;
            _cLabelType.ForeColor = vText;
            _cButtonClear.BackColor = vForm;
            _cButtonClear.UseVisualStyleBackColor = false;

            foreach (Control vCtrl in _cPanelFilters.Controls)
            {
                if (vCtrl is Label)
                {
                    vCtrl.BackColor = vForm;
                    vCtrl.ForeColor = vText;
                }
                else if (vCtrl is CheckBox || vCtrl is Panel)
                {
                    vCtrl.BackColor = vForm;
                }
                else if (vCtrl is TextBox || vCtrl is ComboBox || vCtrl is DateTimePicker)
                {
                    vCtrl.BackColor = vDataBack;
                }
            }

            // Явно поля фильтров
            _cFilterProcedure.BackColor = vDataBack;
            _cFilterMessage.BackColor = vDataBack;
            _cFilterApp.BackColor = vDataBack;
            _cFilterHost.BackColor = vDataBack;
            _cFilterUser.BackColor = vDataBack;
            _cFilterDateFrom.BackColor = vDataBack;
            _cFilterDateTo.BackColor = vDataBack;
            _cFilterUserSolution.ForeColor = vText;
        }

        private void mAddFilterLabel(string pText, int pColumn, int pRow)
        {
            Label vLabel = new Label();
            vLabel.Text = pText;
            vLabel.Dock = DockStyle.Fill;
            vLabel.TextAlign = ContentAlignment.MiddleLeft;
            vLabel.Padding = new Padding(2, 0, 0, 0);
            _cPanelFilters.Controls.Add(vLabel, pColumn, pRow);
        }

        private void mFilterDateEnabled_Changed(object sender, EventArgs e)
        {
            bool vOn = _cFilterDateEnabled.Checked;
            _cFilterDateFrom.Enabled = vOn;
            _cFilterDateTo.Enabled = vOn;
            mFilter_Changed(sender, e);
        }

        /// <summary>
        /// Две таблицы: слева записи протоколов (под панелью фильтров, тот же приём TableLayoutPanel,
        /// что и в рабочей 'cspFormMain'), справа - заголовок выбранного протокола
        /// </summary>
        private void mGridsBuild()
        {
            _cSplitter.Dock = DockStyle.Fill;
            _cSplitter.Orientation = Orientation.Vertical;

            _cLeftHost.Dock = DockStyle.Fill;
            _cLeftHost.ColumnCount = 1;
            _cLeftHost.RowCount = 2;
            _cLeftHost.ColumnStyles.Clear();
            _cLeftHost.RowStyles.Clear();
            _cLeftHost.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _cLeftHost.RowStyles.Add(new RowStyle(SizeType.Absolute, 290F)); // Фильтры + виды протокола
            _cLeftHost.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Сетка записей
            _cLeftHost.Padding = new Padding(0);
            _cLeftHost.Margin = new Padding(0);

            _cAreaRecords.Dock = DockStyle.Fill;
            _cAreaRecords.Margin = new Padding(0);
            _cAreaRecords.__fHeaderCaption_ = "Записи протоколов";
            _cAreaRecords.__fHeaderVisible_ = true;

            _cLeftHost.Controls.Add(_cPanelFilters, 0, 0);
            _cLeftHost.Controls.Add(_cAreaRecords, 0, 1);
            _cSplitter.Panel1.Controls.Add(_cLeftHost);

            _cAreaRecords.__mColumnAdd("Протокол", "CLU протокола", "lnkPcl", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
            _cAreaRecords.__mColumnAdd("Вид", "Вид записи", "Type", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
            _cAreaRecords.__mColumnAdd("Сообщение", "Текст сообщения", "Message", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
            _cAreaRecords.__mColumnAdd("Приложение", "Приложение протокола", "App", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
            _cAreaRecords.__mColumnAdd("Вид протокола", "Вид протокола-владельца", "PclTyp", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
            _cAreaRecords.__mColumnAdd("Процедура", "Процедура протокола-владельца", "Prc", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
            _cAreaRecords.__mGridBuild();
            if (_cAreaRecords.__fGrid_ != null)
            {
                _cAreaRecords.__fGrid_.AutoGenerateColumns = false;
                _cAreaRecords.__fGrid_.ReadOnly = true;
                _cAreaRecords.__fGrid_.AllowUserToAddRows = false;
                _cAreaRecords.__fGrid_.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                _cAreaRecords.__fGrid_.MultiSelect = false;
                _cAreaRecords.__fGrid_.SelectionChanged += mAreaRecords_SelectionChanged;
            }
            _cAreaRecords.__eGridCellEnter += mAreaRecords_GridCellEnter;

            _cAreaProtocolHeader.Dock = DockStyle.Fill;
            _cAreaProtocolHeader.__fHeaderCaption_ = "Заголовки протоколов";
            _cAreaProtocolHeader.__fHeaderVisible_ = true;
            _cSplitter.Panel2.Controls.Add(_cAreaProtocolHeader);

            _cAreaProtocolHeader.__mColumnAdd("CLU", "Ключ протокола", "CLU", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
            _cAreaProtocolHeader.__mColumnAdd("Время", "Время создания", "CHG", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
            _cAreaProtocolHeader.__mColumnAdd("Приложение", "Приложение", "App", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
            _cAreaProtocolHeader.__mColumnAdd("Вид", "Вид протокола", "PclTyp", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
            _cAreaProtocolHeader.__mColumnAdd("Процедура", "Процедура", "Prc", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
            _cAreaProtocolHeader.__mColumnAdd("Хост", "Компьютер", "Hst", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
            _cAreaProtocolHeader.__mColumnAdd("Пользователь", "Пользователь", "Usr", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
            _cAreaProtocolHeader.__mGridBuild();
            if (_cAreaProtocolHeader.__fGrid_ != null)
            {
                _cAreaProtocolHeader.__fGrid_.AutoGenerateColumns = false;
                _cAreaProtocolHeader.__fGrid_.ReadOnly = true;
                _cAreaProtocolHeader.__fGrid_.AllowUserToAddRows = false;
                _cAreaProtocolHeader.__fGrid_.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                _cAreaProtocolHeader.__fGrid_.MultiSelect = false;
                _cAreaProtocolHeader.__fGrid_.SelectionChanged += mAreaProtocolHeader_SelectionChanged;
            }
            _cAreaProtocolHeader.__eGridCellEnter += mAreaProtocolHeader_GridCellEnter;
        }

        #endregion Объект

        #region - Процедуры

        /// <summary>
        /// Произвольный SQL-запрос к базе, выбранной для просмотра ('dsqProtocols.__oViewing_') - см.
        /// примечание в '_mObjectAssembly'
        /// </summary>
        private DataTable mQuery(string pQuery)
        {
            if (_oDataSource == null)
                return new DataTable();

            try
            {
                return _oDataSource.__mSqlQuery(pQuery) ?? new DataTable();
            }
            catch
            {
                return new DataTable();
            }
        }
        /// <summary>
        /// Заполнение выпадающих списков и списка видов протокола реальными значениями из базы
        /// </summary>
        private void mFiltersPopulate()
        {
            _fPopulating = true;
            try
            {
                ProtocolsSchemaInfo vSchema = ProtocolsSchemaDetector.DetectFor(_oDataSource);

                mComboFill(_cFilterApp, "SELECT DISTINCT " + vSchema.AppNameColumn + " AS V FROM App WHERE " + vSchema.AppNameColumn + " IS NOT NULL ORDER BY V");

                if (vSchema.HasCpuUsrTables == true)
                {
                    mComboFill(_cFilterHost, "SELECT DISTINCT dsiCpu AS V FROM Cpu WHERE dsiCpu IS NOT NULL ORDER BY V");
                    mComboFill(_cFilterUser, "SELECT DISTINCT dsiUsr AS V FROM Usr WHERE dsiUsr IS NOT NULL ORDER BY V");
                }
                else if (vSchema.HostUserDirectText == true)
                {
                    mComboFill(_cFilterHost, "SELECT DISTINCT Hst AS V FROM Pcl WHERE Hst IS NOT NULL AND Hst <> '' ORDER BY V");
                    mComboFill(_cFilterUser, "SELECT DISTINCT Usr AS V FROM Pcl WHERE Usr IS NOT NULL AND Usr <> '' ORDER BY V");
                }
                else
                {
                    _cFilterHost.Items.Clear(); _cFilterHost.Items.Add("(все)"); _cFilterHost.SelectedIndex = 0; _cFilterHost.Enabled = false;
                    _cFilterUser.Items.Clear(); _cFilterUser.Items.Add("(все)"); _cFilterUser.SelectedIndex = 0; _cFilterUser.Enabled = false;
                }

                _fTypeClueByCaption.Clear();
                _cFilterType.Items.Clear();
                DataTable vTypes = mQuery("SELECT CLU, " + vSchema.PclTypNameColumn + " AS V FROM PclTyp WHERE " + vSchema.PclTypNameColumn + " IS NOT NULL ORDER BY CLU");
                foreach (DataRow vRow in vTypes.Rows)
                {
                    string vCaption = vRow["V"].ToString();
                    int vClu = Convert.ToInt32(vRow["CLU"]);
                    if (_fTypeClueByCaption.ContainsKey(vCaption) == false)
                        _fTypeClueByCaption[vCaption] = vClu;
                    _cFilterType.Items.Add(vCaption, false); // По умолчанию - все виды сняты (фильтр не сужает список)
                }
            }
            finally
            {
                _fPopulating = false;
            }
        }
        /// <summary>
        /// Заполнение выпадающего списка результатами запроса (первый пункт всегда "(все)")
        /// </summary>
        private void mComboFill(ComboBox pCombo, string pQuery)
        {
            pCombo.Items.Clear();
            pCombo.Items.Add("(все)");

            DataTable vDataTable = mQuery(pQuery);
            foreach (DataRow vRow in vDataTable.Rows)
            {
                if (vRow["V"] != DBNull.Value)
                {
                    string vValue = vRow["V"].ToString();
                    if (string.IsNullOrEmpty(vValue) == false)
                        pCombo.Items.Add(vValue);
                }
            }

            pCombo.Enabled = true;
            pCombo.SelectedIndex = 0;
        }
        /// <summary>
        /// Построение ОБЕИХ таблиц целиком с учётом активных фильтров: слева - все подходящие записи
        /// ('PclRrd'), справа - все подходящие заголовки протоколов ('Pcl'), одновременно, а не только
        /// заголовок текущей выбранной записи (было раньше - см. историю правок класса). Позволяет
        /// сравнивать протоколы за одну и ту же дату/период от разных приложений, компьютеров и
        /// пользователей одним взглядом, без выбора по одной записи.
        /// </summary>
        private void mProtocolsLoad()
        {
            if (_oDataSource == null)
                return;

            ProtocolsSchemaInfo vSchema = ProtocolsSchemaDetector.DetectFor(_oDataSource);

            /// Условия, применимые к заголовку протокола ('Pcl' и его связанные 'App'/'PclTyp'/'Cpu'/'Usr') -
            /// общие для ОБОИХ запросов: и для списка заголовков справа, и (через JOIN на Pcl) для списка
            /// записей слева. Именно эти условия дают "несколько протоколов за одну дату от разных
            /// приложений/компьютеров/пользователей" - см. запрос пользователя.
            List<string> vHeaderConditions = new List<string>();

            if (_cFilterApp.SelectedIndex > 0)
                vHeaderConditions.Add("A." + vSchema.AppNameColumn + " = '" + mEscape(_cFilterApp.Text) + "'");

            if (_cFilterHost.Enabled == true && _cFilterHost.SelectedIndex > 0)
                vHeaderConditions.Add((vSchema.HasCpuUsrTables == true ? "C.dsiCpu" : "P.Hst") + " = '" + mEscape(_cFilterHost.Text) + "'");

            if (_cFilterUser.Enabled == true && _cFilterUser.SelectedIndex > 0)
                vHeaderConditions.Add((vSchema.HasCpuUsrTables == true ? "U.dsiUsr" : "P.Usr") + " = '" + mEscape(_cFilterUser.Text) + "'");

            if (_cFilterProcedure.Text.Trim().Length > 0)
                vHeaderConditions.Add("P.Prc LIKE '%" + mEscape(_cFilterProcedure.Text.Trim()) + "%'");

            List<int> vCheckedTypeClueS = new List<int>();
            foreach (object vCheckedItem in _cFilterType.CheckedItems)
            {
                int vClu;
                if (_fTypeClueByCaption.TryGetValue(vCheckedItem.ToString(), out vClu) == true)
                    vCheckedTypeClueS.Add(vClu);
            }
            if (vCheckedTypeClueS.Count > 0)
            {
                List<string> vClueStrings = new List<string>();
                foreach (int vClu in vCheckedTypeClueS) vClueStrings.Add(vClu.ToString());
                vHeaderConditions.Add("P." + vSchema.PclTypLinkColumn + " IN (" + string.Join(",", vClueStrings) + ")");
            }

            if (_cFilterDateEnabled.Checked == true)
            {
                if (vSchema.ChgIsTicks == true)
                {
                    vHeaderConditions.Add("P.CHG >= " + _cFilterDateFrom.Value.Date.Ticks.ToString());
                    vHeaderConditions.Add("P.CHG <= " + _cFilterDateTo.Value.Date.AddDays(1).AddTicks(-1).Ticks.ToString());
                }
                else
                {
                    vHeaderConditions.Add("P.CHG >= '" + _cFilterDateFrom.Value.Date.ToString("yyyy-MM-dd 00:00:00") + "'");
                    vHeaderConditions.Add("P.CHG <= '" + _cFilterDateTo.Value.Date.ToString("yyyy-MM-dd 23:59:59") + "'");
                }
            }

            /// Условия, применимые ТОЛЬКО к записи ('PclRrd') - у заголовка ('Pcl') нет ни текста
            /// сообщения, ни вида записи напрямую
            List<string> vRecordOnlyConditions = new List<string>();
            if (_cFilterMessage.Text.Trim().Length > 0)
                vRecordOnlyConditions.Add("PR." + vSchema.MessageColumn + " LIKE '%" + mEscape(_cFilterMessage.Text.Trim()) + "%'");
            if (_cFilterUserSolution.Checked == true)
                vRecordOnlyConditions.Add("(PRT." + vSchema.RrdTypNameColumn + " LIKE '%Решение%' OR PR." + vSchema.RrdLinkColumn + " = 1)");

            string vJoins = "LEFT JOIN App A ON A.CLU = P." + vSchema.AppLinkColumn + " "
                + "LEFT JOIN PclTyp PT ON PT.CLU = P." + vSchema.PclTypLinkColumn + " "
                + (vSchema.HasCpuUsrTables == true ? "LEFT JOIN Cpu C ON C.CLU = P.lnkCpu LEFT JOIN Usr U ON U.CLU = P.lnkUsr " : "");

            /// Запрос слева: записи, с учётом ОБЩИХ условий (через JOIN на Pcl) И условий, специфичных для записи
            List<string> vLeftConditions = new List<string>(vHeaderConditions);
            vLeftConditions.AddRange(vRecordOnlyConditions);
            string vLeftWhere = vLeftConditions.Count > 0 ? "WHERE " + string.Join(" AND ", vLeftConditions) + " " : "";

            string vLeftQuery = "SELECT PR.CLU, PR." + vSchema.PclLinkColumn + " AS lnkPcl, PRT." + vSchema.RrdTypNameColumn + " AS Type, "
                + "PR." + vSchema.MessageColumn + " AS Message, A." + vSchema.AppNameColumn + " AS App, PT." + vSchema.PclTypNameColumn + " AS PclTyp, P.Prc AS Prc "
                + "FROM PclRrd PR "
                + "LEFT JOIN Pcl P ON P.CLU = PR." + vSchema.PclLinkColumn + " "
                + vJoins
                + "LEFT JOIN " + vSchema.RrdTypTable + " PRT ON PRT.CLU = PR." + vSchema.RrdLinkColumn + " "
                + vLeftWhere
                + "ORDER BY PR.CLU DESC";

            /// Запрос справа: ВСЕ подходящие заголовки, а не только владелец выбранной записи. Если задан
            /// фильтр, специфичный для записи (сообщение/решение), заголовок включается, только если у него
            /// ЕСТЬ хотя бы одна подходящая запись (иначе список заголовков и список записей рассинхронизировались бы)
            string vRightWhere = vHeaderConditions.Count > 0 ? "WHERE " + string.Join(" AND ", vHeaderConditions) + " " : "";
            if (vRecordOnlyConditions.Count > 0)
            {
                string vExistsWhere = string.Join(" AND ", vRecordOnlyConditions);
                string vExists = "EXISTS (SELECT 1 FROM PclRrd PR "
                    + "LEFT JOIN " + vSchema.RrdTypTable + " PRT ON PRT.CLU = PR." + vSchema.RrdLinkColumn + " "
                    + "WHERE PR." + vSchema.PclLinkColumn + " = P.CLU AND " + vExistsWhere + ")";
                vRightWhere = vRightWhere.Length > 0 ? vRightWhere + "AND " + vExists + " " : "WHERE " + vExists + " ";
            }

            string vHostColumn = vSchema.HostUserDirectText == true ? "P.Hst AS Hst" : (vSchema.HasCpuUsrTables == true ? "C.dsiCpu AS Hst" : "'' AS Hst");
            string vUserColumn = vSchema.HostUserDirectText == true ? "P.Usr AS Usr" : (vSchema.HasCpuUsrTables == true ? "U.dsiUsr AS Usr" : "'' AS Usr");

            string vRightQuery = "SELECT P.CLU, P.CHG, A." + vSchema.AppNameColumn + " AS App, PT." + vSchema.PclTypNameColumn + " AS PclTyp, P.Prc, "
                + vHostColumn + ", " + vUserColumn + " "
                + "FROM Pcl P "
                + vJoins
                + vRightWhere
                + "ORDER BY P.CHG DESC";

            DataTable vLeftTable = mQuery(vLeftQuery);
            DataTable vRightTable = mQuery(vRightQuery);

            // Всегда приводим CHG к читаемой дате: в базе часто лежат .NET-тики (Int64/TEXT),
            // а детектор ChgIsTicks иногда ошибается — из-за этого в сетке оставались числа вроде 6392315...
            if (vRightTable != null && vRightTable.Columns.Contains("CHG") == true)
                mFormatChgColumn(vRightTable);

            mGridSet(_cAreaRecords, vLeftTable);
            mGridSet(_cAreaProtocolHeader, vRightTable);
        }

        /// <summary>
        /// Преобразует столбец CHG в строку "dd.MM.yyyy HH:mm:ss".
        /// Поддерживает: .NET ticks (число &gt; 600000000000000000), DateTime, ISO/обычные строки дат.
        /// Столбец пересоздаётся как string, чтобы не было ArgumentException при записи в Int64.
        /// </summary>
        private void mFormatChgColumn(DataTable pTable)
        {
            if (pTable == null || pTable.Columns.Contains("CHG") == false)
                return;

            List<object> vRawValues = new List<object>();
            foreach (DataRow vRow in pTable.Rows)
                vRawValues.Add(vRow["CHG"]);

            int vOrdinal = pTable.Columns["CHG"].Ordinal;
            pTable.Columns.Remove("CHG");
            DataColumn vCol = pTable.Columns.Add("CHG", typeof(string));
            vCol.SetOrdinal(vOrdinal);

            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                object vRaw = vRawValues[i];
                pTable.Rows[i]["CHG"] = mChgToDisplay(vRaw);
            }
        }

        private static string mChgToDisplay(object pRaw)
        {
            if (pRaw == null || pRaw == DBNull.Value)
                return "";

            if (pRaw is DateTime)
                return ((DateTime)pRaw).ToString("dd.MM.yyyy HH:mm:ss");

            string vText = pRaw.ToString().Trim();
            if (vText.Length == 0)
                return "";

            long vTicks;
            // .NET ticks: 18-значное число (DateTime.MinValue.Ticks = 0, 2000-01-01 ≈ 630822816000000000)
            if (long.TryParse(vText, out vTicks) == true && vTicks > 600000000000000000L)
            {
                try { return new DateTime(vTicks).ToString("dd.MM.yyyy HH:mm:ss"); }
                catch { return vText; }
            }

            DateTime vDt;
            if (DateTime.TryParse(vText, out vDt) == true)
                return vDt.ToString("dd.MM.yyyy HH:mm:ss");

            return vText;
        }

        /// <summary>
        /// Переустановка источника данных сетки (со сбросом, как требует 'DataGridView' при повторной привязке)
        /// </summary>
        private void mGridSet(cspAreaGrid pArea, DataTable pDataTable)
        {
            if (pArea.__fGrid_ != null)
            {
                pArea.__fGrid_.DataSource = null;
                pArea.__fGrid_.DataSource = pDataTable;
            }
            else
            {
                pArea.__fDataSource_ = pDataTable;
            }
            pArea.__mGridRefresh();
        }
        /// <summary>
        /// Экранирование одинарных кавычек для безопасной вставки текста в SQL-запрос
        /// </summary>
        private string mEscape(string pText)
        {
            return pText == null ? "" : pText.Replace("'", "''");
        }

        /// <summary>
        /// Очистка подсветки в указанной сетке (возврат к обычным цветам по умолчанию)
        /// </summary>
        private void mHighlightClear(cspAreaGrid pArea)
        {
            if (pArea.__fGrid_ == null)
                return;

            foreach (DataGridViewRow vRow in pArea.__fGrid_.Rows)
                vRow.DefaultCellStyle.BackColor = Color.Empty;
        }
        /// <summary>
        /// Нормализация ключа протокола для сравнения (CLU / lnkPcl могут быть int или string).
        /// </summary>
        private static string mKeyNormalize(object pValue)
        {
            if (pValue == null || pValue == DBNull.Value)
                return "";
            string vText = pValue.ToString().Trim();
            int vNum;
            if (int.TryParse(vText, out vNum))
                return vNum.ToString();
            return vText;
        }
        /// <summary>
        /// Подсветка всех строк с совпадающим ключом + переход на первую совпавшую строку
        /// (Select + ScrollIntoView), чтобы связанный протокол/запись сразу был виден.
        /// </summary>
        private void mHighlightMatching(cspAreaGrid pArea, string pColumnName, string pValue)
        {
            if (pArea.__fGrid_ == null)
                return;

            DataGridView vGrid = pArea.__fGrid_;
            string vWanted = mKeyNormalize(pValue);
            DataGridViewRow vFirstMatch = null;

            foreach (DataGridViewRow vRow in vGrid.Rows)
            {
                if (vRow.IsNewRow)
                    continue;

                object vCellValue = null;
                try { vCellValue = vRow.Cells[pColumnName].Value; }
                catch { continue; }

                bool vMatch = mKeyNormalize(vCellValue) == vWanted && vWanted.Length > 0;
                vRow.DefaultCellStyle.BackColor = vMatch ? Color.FromArgb(255, 245, 190) : Color.Empty;
                if (vMatch && vFirstMatch == null)
                    vFirstMatch = vRow;
            }

            if (vFirstMatch == null)
                return;

            // Переход к связанной строке без рекурсии SelectionChanged
            _fSyncingSelection = true;
            try
            {
                vGrid.ClearSelection();
                vFirstMatch.Selected = true;
                if (vFirstMatch.Cells.Count > 0)
                {
                    try { vGrid.CurrentCell = vFirstMatch.Cells[0]; }
                    catch { }
                }
                try { vGrid.FirstDisplayedScrollingRowIndex = vFirstMatch.Index; }
                catch { }
            }
            finally
            {
                _fSyncingSelection = false;
            }
        }

        #endregion Процедуры

        #region - События

        private void mFilter_Changed(object sender, EventArgs pEventArgs)
        {
            if (_fPopulating == true)
                return;
            mProtocolsLoad();
        }
        private void mFilterType_ItemCheck(object sender, ItemCheckEventArgs pEventArgs)
        {
            if (_fPopulating == true)
                return;

            BeginInvoke(new Action(mProtocolsLoad));
        }
        private void mButtonClear_Click(object sender, EventArgs pEventArgs)
        {
            _fPopulating = true;
            try
            {
                if (_cFilterApp.Items.Count > 0) _cFilterApp.SelectedIndex = 0;
                if (_cFilterHost.Enabled == true && _cFilterHost.Items.Count > 0) _cFilterHost.SelectedIndex = 0;
                if (_cFilterUser.Enabled == true && _cFilterUser.Items.Count > 0) _cFilterUser.SelectedIndex = 0;
                _cFilterProcedure.Text = "";
                _cFilterMessage.Text = "";
                _cFilterUserSolution.Checked = false;
                _cFilterDateEnabled.Checked = false;
                _cFilterDateFrom.Enabled = false;
                _cFilterDateTo.Enabled = false;
                for (int i = 0; i < _cFilterType.Items.Count; i++)
                    _cFilterType.SetItemChecked(i, false);
            }
            finally
            {
                _fPopulating = false;
            }
            mProtocolsLoad();
        }
        /// <summary>
        /// Выбор записи слева ('PclRrd') - подсветка её протокола-владельца справа ('Pcl') по 'lnkPcl' = 'CLU'.
        /// Обе таблицы уже полностью загружены (см. 'mProtocolsLoad') - здесь только подсветка, без запроса к базе
        /// </summary>
        private void mAreaRecords_SelectionChanged(object sender, EventArgs pEventArgs)
        {
            if (_fSyncingSelection)
                return;
            mHighlightFromRecordsSelection();
        }
        private void mAreaRecords_GridCellEnter(object sender, EventArgs pEventArgs)
        {
            if (_fSyncingSelection)
                return;
            mHighlightFromRecordsSelection();
        }
        private void mHighlightFromRecordsSelection()
        {
            DataGridViewRow vRow = _cAreaRecords.__fCurrentRow_;
            object vLnk = null;
            try
            {
                if (vRow != null)
                    vLnk = vRow.Cells["lnkPcl"].Value;
            }
            catch { vLnk = null; }

            if (vRow == null || vLnk == null || vLnk == DBNull.Value || mKeyNormalize(vLnk).Length == 0)
            {
                mHighlightClear(_cAreaProtocolHeader);
                return;
            }

            // Слева выбрана запись → справа подсветка + переход к заголовку этого протокола
            mHighlightMatching(_cAreaProtocolHeader, "CLU", mKeyNormalize(vLnk));
        }
        /// <summary>
        /// Выбор заголовка справа ('Pcl') - подсветка всех его записей слева + переход к первой записи
        /// </summary>
        private void mAreaProtocolHeader_SelectionChanged(object sender, EventArgs pEventArgs)
        {
            if (_fSyncingSelection)
                return;
            mHighlightFromHeaderSelection();
        }
        private void mAreaProtocolHeader_GridCellEnter(object sender, EventArgs pEventArgs)
        {
            if (_fSyncingSelection)
                return;
            mHighlightFromHeaderSelection();
        }
        private void mHighlightFromHeaderSelection()
        {
            DataGridViewRow vRow = _cAreaProtocolHeader.__fCurrentRow_;
            object vClu = null;
            try
            {
                if (vRow != null)
                    vClu = vRow.Cells["CLU"].Value;
            }
            catch { vClu = null; }

            if (vRow == null || vClu == null || vClu == DBNull.Value || mKeyNormalize(vClu).Length == 0)
            {
                mHighlightClear(_cAreaRecords);
                return;
            }

            // Справа выбран заголовок → слева подсветка + переход к первой записи этого протокола
            mHighlightMatching(_cAreaRecords, "lnkPcl", mKeyNormalize(vClu));
        }

        #endregion События

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        private elmBlockFormMain _cBlockFormMain = new elmBlockFormMain();
        private elmComponentSplitter _cSplitter = new elmComponentSplitter();
        private TableLayoutPanel _cLeftHost = new TableLayoutPanel();
        private TableLayoutPanel _cPanelFilters = new TableLayoutPanel();

        private ComboBox _cFilterApp = new ComboBox();
        private ComboBox _cFilterHost = new ComboBox();
        private ComboBox _cFilterUser = new ComboBox();
        private TextBox _cFilterProcedure = new TextBox();
        private TextBox _cFilterMessage = new TextBox();
        private CheckBox _cFilterUserSolution = new CheckBox();
        private CheckBox _cFilterDateEnabled = new CheckBox();
        private DateTimePicker _cFilterDateFrom = new DateTimePicker();
        private DateTimePicker _cFilterDateTo = new DateTimePicker();
        private Button _cButtonClear = new Button();
        private Label _cLabelType = new Label();
        private CheckedListBox _cFilterType = new CheckedListBox();

        private cspAreaGrid _cAreaRecords = new cspAreaGrid();
        private cspAreaGrid _cAreaProtocolHeader = new cspAreaGrid();

        #endregion Компоненты

        #region - Служебные

        private nlData.datUnitDataSource _oDataSource;
        private bool _fPopulating = false;
        private bool _fSyncingSelection = false;
        private Dictionary<string, int> _fTypeClueByCaption = new Dictionary<string, int>();

        #endregion Служебные

        #endregion ПОЛЯ

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // cspFormCombinedViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "cspFormCombinedViewer";
            this.Load += new System.EventHandler(this.cspFormCombinedViewer_Load);
            this.ResumeLayout(false);

        }

        private void cspFormCombinedViewer_Load(object sender, EventArgs e)
        {

        }
    }
}