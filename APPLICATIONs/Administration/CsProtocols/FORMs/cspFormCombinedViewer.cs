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
    /// <fixed>Первая версия размещала панель фильтров через 'Dock = Top' на обычной 'Panel' поверх сетки
    /// с 'Dock = Fill' и вручную дёргала 'Controls.SetChildIndex' - тот же антипаттерн, что уже был один раз
    /// найден и исправлен в рабочей 'cspFormMain.cs' (см. её комментарий: "Dock Top+Fill на обычной Panel с
    /// cspAreaGrid (сам SplitContainer) давал перекрытие и «серую» сетку"). Переписано на тот же проверенный
    /// приём: 'elmBlockFormMain' + 'TableLayoutPanel' с явными строками (фильтры - Absolute высота, сетка -
    /// Percent 100%), без ручного докинга и без ручной перестановки Z-order.</fixed>
    /// <conception>Lucasin V.</conception>
    public class cspFormCombinedViewer : elmForm
    {
        #region = МЕТОДЫ

        #region - Объект

        protected override void _mObjectAssembly()
        {
            base._mObjectAssembly();

            __fCaption_ = "Совмещённый просмотр протоколов";
            ClientSize = new Size(1200, 700);

            /// ИСПРАВЛЕНО: раньше здесь стояло 'dsqProtocols.__oActive_' - самовосстанавливающийся указатель
            /// на "родную" базу приложения ('Databases\protocols.db'), НЕ связанный с тем, что пользователь
            /// открывает через "Файл / Открыть протокол" в 'cspFormMain'. Из-за этого эта форма всегда
            /// показывала одну и ту же (часто постороннюю/загрязнённую) базу, независимо от того, какую
            /// базу пользователь только что открыл в главном окне. 'dsqProtocols.__oViewing_' - это как раз
            /// то поле, которое 'cspFormMain' обновляет при "Файл / Открыть" и "Файл / Закрыть" (см.
            /// примечания к обоим полям в 'dsqProtocols.cs') - оно и должно читаться здесь. Тип - общий
            /// 'datUnitDataSource', а не 'dsqProtocols' (вручную открытый файл - это 'dsqDataSourceSqliteWithProtocol'),
            /// поэтому запросы и определение схемы ниже используют соответствующую перегрузку
            _oDataSource = dsqProtocols.__oViewing_;

            /// [null] означает "в главном окне ничего не открыто" - показываем пустые таблицы и понятную
            /// подсказку, а не подставляем родную базу приложения по умолчанию (тот же принцип, что и в
            /// 'cspFormMain.mMenuFileClose_Click' - закрытие означает по-настоящему "ничего не открыто")
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
        }
        /// <summary>
        /// Панель фильтров - TableLayoutPanel (не абсолютные координаты), несколько строк
        /// </summary>
        private void mFilterPanelBuild()
        {
            _cPanelFilters.Dock = DockStyle.Fill;
            _cPanelFilters.Margin = new Padding(0);
            _cPanelFilters.ColumnCount = 6;
            _cPanelFilters.RowCount = 4;
            _cPanelFilters.ColumnStyles.Clear();
            for (int i = 0; i < 6; i++)
                _cPanelFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / 6F));
            _cPanelFilters.RowStyles.Clear();
            _cPanelFilters.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));  // Заголовок "Вид протокола"
            _cPanelFilters.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));  // Список видов протокола
            _cPanelFilters.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));  // App/Host/User/Procedure/Message
            _cPanelFilters.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));  // UserSolution/дата/очистить

            _cLabelType.Text = "Вид протокола (отметьте нужные; пусто = все)";
            _cLabelType.Dock = DockStyle.Fill;
            _cPanelFilters.Controls.Add(_cLabelType, 0, 0);
            _cPanelFilters.SetColumnSpan(_cLabelType, 6);

            _cFilterType.Dock = DockStyle.Fill;
            _cFilterType.CheckOnClick = true;
            _cFilterType.MultiColumn = true;
            _cFilterType.ItemCheck += mFilterType_ItemCheck;
            _cPanelFilters.Controls.Add(_cFilterType, 0, 1);
            _cPanelFilters.SetColumnSpan(_cFilterType, 6);

            mAddFilterLabelAndCombo("Приложение", _cFilterApp, 0);
            mAddFilterLabelAndCombo("Компьютер", _cFilterHost, 1);
            mAddFilterLabelAndCombo("Пользователь", _cFilterUser, 2);
            mAddFilterLabelAndText("Процедура содержит", _cFilterProcedure, 3);
            mAddFilterLabelAndText("Сообщение содержит", _cFilterMessage, 4);

            _cFilterUserSolution.Text = "Только решения пользователя (Ответ)";
            _cFilterUserSolution.Dock = DockStyle.Fill;
            _cFilterUserSolution.CheckedChanged += mFilter_Changed;
            _cPanelFilters.Controls.Add(_cFilterUserSolution, 0,3);
            _cPanelFilters.SetColumnSpan(_cFilterUserSolution, 2);

            _cFilterDateEnabled.Text = "Период:";
            _cFilterDateEnabled.Dock = DockStyle.Fill;
            _cFilterDateEnabled.CheckedChanged += mFilter_Changed;
            _cPanelFilters.Controls.Add(_cFilterDateEnabled, 2, 3);

            _cFilterDateFrom.Dock = DockStyle.Fill;
            _cFilterDateFrom.Format = DateTimePickerFormat.Short;
            _cFilterDateFrom.Value = DateTime.Now.AddDays(-30);
            _cFilterDateFrom.ValueChanged += mFilter_Changed;
            _cPanelFilters.Controls.Add(_cFilterDateFrom, 3, 3);

            _cFilterDateTo.Dock = DockStyle.Fill;
            _cFilterDateTo.Format = DateTimePickerFormat.Short;
            _cFilterDateTo.Value = DateTime.Now;
            _cFilterDateTo.ValueChanged += mFilter_Changed;
            _cPanelFilters.Controls.Add(_cFilterDateTo, 4, 3);

            _cButtonClear.Text = "Очистить фильтры";
            _cButtonClear.Dock = DockStyle.Fill;
            _cButtonClear.Click += mButtonClear_Click;
            _cPanelFilters.Controls.Add(_cButtonClear, 5, 2);
        }
        /// <summary>
        /// Добавление пары "подпись сверху / выпадающий список" в строку 2 (индекс 2) панели фильтров
        /// </summary>
        private void mAddFilterLabelAndCombo(string pLabel, ComboBox pCombo, int pColumn)
        {
            TableLayoutPanel vCell = new TableLayoutPanel();
            vCell.Dock = DockStyle.Fill;
            vCell.ColumnCount = 1;
            vCell.RowCount = 2;
            vCell.RowStyles.Add(new RowStyle(SizeType.Absolute, 16F));
            vCell.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Label vLabel = new Label();
            vLabel.Text = pLabel;
            vLabel.Dock = DockStyle.Fill;
            vCell.Controls.Add(vLabel, 0, 0);

            pCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            pCombo.Dock = DockStyle.Fill;
            pCombo.SelectedIndexChanged += mFilter_Changed;
            vCell.Controls.Add(pCombo, 0, 1);

            _cPanelFilters.Controls.Add(vCell, pColumn, 2);
        }
        /// <summary>
        /// Добавление пары "подпись сверху / текстовое поле" в строку 2 (индекс 2) панели фильтров
        /// </summary>
        private void mAddFilterLabelAndText(string pLabel, TextBox pTextBox, int pColumn)
        {
            TableLayoutPanel vCell = new TableLayoutPanel();
            vCell.Dock = DockStyle.Fill;
            vCell.ColumnCount = 1;
            vCell.RowCount = 2;
            vCell.RowStyles.Add(new RowStyle(SizeType.Absolute, 16F));
            vCell.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Label vLabel = new Label();
            vLabel.Text = pLabel;
            vLabel.Dock = DockStyle.Fill;
            vCell.Controls.Add(vLabel, 0, 0);

            pTextBox.Dock = DockStyle.Fill;
            pTextBox.TextChanged += mFilter_Changed;
            vCell.Controls.Add(pTextBox, 0, 1);

            _cPanelFilters.Controls.Add(vCell, pColumn, 2);
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
            _cLeftHost.RowStyles.Add(new RowStyle(SizeType.Absolute, 260F)); // Фильтры + виды протокола
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
            _cAreaProtocolHeader.__fHeaderCaption_ = "Заголовок протокола (выбранной записи)";
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
            }
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
        /// Построение списка записей протоколов с учётом активных фильтров
        /// </summary>
        private void mProtocolsLoad()
        {
            if (_oDataSource == null)
                return;

            ProtocolsSchemaInfo vSchema = ProtocolsSchemaDetector.DetectFor(_oDataSource);

            List<string> vConditions = new List<string>();

            if (_cFilterApp.SelectedIndex > 0)
                vConditions.Add("A." + vSchema.AppNameColumn + " = '" + mEscape(_cFilterApp.Text) + "'");

            if (_cFilterHost.Enabled == true && _cFilterHost.SelectedIndex > 0)
                vConditions.Add((vSchema.HasCpuUsrTables == true ? "C.dsiCpu" : "P.Hst") + " = '" + mEscape(_cFilterHost.Text) + "'");

            if (_cFilterUser.Enabled == true && _cFilterUser.SelectedIndex > 0)
                vConditions.Add((vSchema.HasCpuUsrTables == true ? "U.dsiUsr" : "P.Usr") + " = '" + mEscape(_cFilterUser.Text) + "'");

            if (_cFilterProcedure.Text.Trim().Length > 0)
                vConditions.Add("P.Prc LIKE '%" + mEscape(_cFilterProcedure.Text.Trim()) + "%'");

            if (_cFilterMessage.Text.Trim().Length > 0)
                vConditions.Add("PR." + vSchema.MessageColumn + " LIKE '%" + mEscape(_cFilterMessage.Text.Trim()) + "%'");

            if (_cFilterUserSolution.Checked == true)
            {
                /// "Решение пользователя" - вид записи 'Answer' (CLU 1 при штатном сидинге, см. dsqProtocols.mRecordTypeClue);
                /// подпись используется как более надёжный резервный признак, если сидинг когда-либо изменится
                vConditions.Add("(PRT." + vSchema.RrdTypNameColumn + " LIKE '%Решение%' OR PR." + vSchema.RrdLinkColumn + " = 1)");
            }

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
                vConditions.Add("P." + vSchema.PclTypLinkColumn + " IN (" + string.Join(",", vClueStrings) + ")");
            }

            if (_cFilterDateEnabled.Checked == true)
            {
                if (vSchema.ChgIsTicks == true)
                {
                    vConditions.Add("P.CHG >= " + _cFilterDateFrom.Value.Date.Ticks.ToString());
                    vConditions.Add("P.CHG <= " + _cFilterDateTo.Value.Date.AddDays(1).AddTicks(-1).Ticks.ToString());
                }
                else
                {
                    vConditions.Add("P.CHG >= '" + _cFilterDateFrom.Value.Date.ToString("yyyy-MM-dd 00:00:00") + "'");
                    vConditions.Add("P.CHG <= '" + _cFilterDateTo.Value.Date.ToString("yyyy-MM-dd 23:59:59") + "'");
                }
            }

            string vWhere = vConditions.Count > 0 ? "WHERE " + string.Join(" AND ", vConditions) + " " : "";

            string vQuery = "SELECT PR.CLU, PR." + vSchema.PclLinkColumn + " AS lnkPcl, PRT." + vSchema.RrdTypNameColumn + " AS Type, "
                + "PR." + vSchema.MessageColumn + " AS Message, A." + vSchema.AppNameColumn + " AS App, PT." + vSchema.PclTypNameColumn + " AS PclTyp, P.Prc AS Prc "
                + "FROM PclRrd PR "
                + "LEFT JOIN Pcl P ON P.CLU = PR." + vSchema.PclLinkColumn + " "
                + "LEFT JOIN App A ON A.CLU = P." + vSchema.AppLinkColumn + " "
                + "LEFT JOIN PclTyp PT ON PT.CLU = P." + vSchema.PclTypLinkColumn + " "
                + "LEFT JOIN " + vSchema.RrdTypTable + " PRT ON PRT.CLU = PR." + vSchema.RrdLinkColumn + " "
                + (vSchema.HasCpuUsrTables == true ? "LEFT JOIN Cpu C ON C.CLU = P.lnkCpu LEFT JOIN Usr U ON U.CLU = P.lnkUsr " : "")
                + vWhere
                + "ORDER BY PR.CLU DESC"; 

            DataTable vDataTable = mQuery(vQuery);
            if (_cAreaRecords.__fGrid_ != null)
            {
                _cAreaRecords.__fGrid_.DataSource = null;
                _cAreaRecords.__fGrid_.DataSource = vDataTable;
            }
            else
            {
                _cAreaRecords.__fDataSource_ = vDataTable;
            }
            _cAreaRecords.__mGridRefresh();

            /// ИСПРАВЛЕНО: раньше здесь безусловно очищалась правая таблица ('_cAreaProtocolHeader.DataSource
            /// = new DataTable()') - если назначение 'DataSource' слева синхронно вызывало 'SelectionChanged'
            /// и правая таблица успевала заполниться, эта строка тут же стирала результат. Теперь вместо
            /// слепой очистки - явный вызов той же функции, что и обработчик выбора строки: она сама покажет
            /// заголовок текущей выделенной записи, либо (если после смены фильтров строк не осталось и
            /// выделения нет) сама корректно очистит таблицу через свой ранний return
            mProtocolHeaderLoadFromCurrentRow();
        }
        /// <summary>
        /// Экранирование одинарных кавычек для безопасной вставки текста в SQL-запрос
        /// </summary>
        private string mEscape(string pText)
        {
            return pText == null ? "" : pText.Replace("'", "''");
        }

        /// <summary>
        /// Очистка правой таблицы (заголовка протокола) - когда слева нет выделенной строки, либо после
        /// смены фильтров список записей стал пустым
        /// </summary>
        private void mProtocolHeaderClear()
        {
            if (_cAreaProtocolHeader.__fGrid_ != null)
                _cAreaProtocolHeader.__fGrid_.DataSource = new DataTable();
            else
                _cAreaProtocolHeader.__fDataSource_ = new DataTable();
            _cAreaProtocolHeader.__mGridRefresh();
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
            /// 'ItemCheck' срабатывает ДО фактического изменения состояния - откладываем обновление на "после клика"
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
        /// Выбор записи слева - справа показывается заголовок её протокола-владельца
        /// </summary>
        private void mAreaRecords_SelectionChanged(object sender, EventArgs pEventArgs)
        {
            mProtocolHeaderLoadFromCurrentRow();
        }
        private void mAreaRecords_GridCellEnter(object sender, EventArgs pEventArgs)
        {
            mProtocolHeaderLoadFromCurrentRow();
        }
        private void mProtocolHeaderLoadFromCurrentRow()
        {
            DataGridViewRow vRow = _cAreaRecords.__fCurrentRow_;
            if (vRow == null)
            {
                mProtocolHeaderClear();
                return;
            }

            object vLnkPclValue;
            try { vLnkPclValue = vRow.Cells["lnkPcl"].Value; }
            catch { mProtocolHeaderClear(); return; }

            int vLnkPcl;
            if (vLnkPclValue == null || int.TryParse(vLnkPclValue.ToString(), out vLnkPcl) == false)
            {
                mProtocolHeaderClear();
                return;
            }

            ProtocolsSchemaInfo vSchema = ProtocolsSchemaDetector.DetectFor(_oDataSource);
            string vHostColumn = vSchema.HostUserDirectText == true ? "P.Hst AS Hst" : (vSchema.HasCpuUsrTables == true ? "C.dsiCpu AS Hst" : "'' AS Hst");
            string vUserColumn = vSchema.HostUserDirectText == true ? "P.Usr AS Usr" : (vSchema.HasCpuUsrTables == true ? "U.dsiUsr AS Usr" : "'' AS Usr");

            string vQuery = "SELECT P.CLU, P.CHG, A." + vSchema.AppNameColumn + " AS App, PT." + vSchema.PclTypNameColumn + " AS PclTyp, P.Prc, "
                + vHostColumn + ", " + vUserColumn + " "
                + "FROM Pcl P "
                + "LEFT JOIN App A ON A.CLU = P." + vSchema.AppLinkColumn + " "
                + "LEFT JOIN PclTyp PT ON PT.CLU = P." + vSchema.PclTypLinkColumn + " "
                + (vSchema.HasCpuUsrTables == true ? "LEFT JOIN Cpu C ON C.CLU = P.lnkCpu LEFT JOIN Usr U ON U.CLU = P.lnkUsr " : "")
                + "WHERE P.CLU = " + vLnkPcl.ToString();

            DataTable vHeader = mQuery(vQuery);
            foreach (DataRow vHeaderRow in vHeader.Rows)
            {
                if (vSchema.ChgIsTicks == true && vHeaderRow["CHG"] != DBNull.Value)
                {
                    long vTicks;
                    if (long.TryParse(vHeaderRow["CHG"].ToString(), out vTicks) == true)
                        vHeaderRow["CHG"] = new DateTime(vTicks).ToString("dd.MM.yyyy HH:mm:ss");
                }
            }

            if (_cAreaProtocolHeader.__fGrid_ != null)
            {
                _cAreaProtocolHeader.__fGrid_.DataSource = null;
                _cAreaProtocolHeader.__fGrid_.DataSource = vHeader;
            }
            else
            {
                _cAreaProtocolHeader.__fDataSource_ = vHeader;
            }
            _cAreaProtocolHeader.__mGridRefresh();
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