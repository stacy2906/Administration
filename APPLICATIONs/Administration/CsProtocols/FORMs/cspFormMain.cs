using nlElements;
using System.Windows.Forms;
using System;
using nlCsProtocols;
using System.Data;
using nlData;
using nlDataSourceSqlite;
using System.IO;
using System.Drawing;
using nlApplication;
using System.Collections.Generic;
using System.Linq;
using CsProtocols.DATA.Loaders;
using CsProtocols.DATA.Models;
using CsProtocols;

namespace naCsProtocols
{
    public class cspFormMain : elmForm
    {
        #region = ПОЛЯ

        // Левая таблица - протоколы
        private cspAreaGrid _cAreaProtocols = new cspAreaGrid();
        private DataTable _oDataTableProtocols = new DataTable();
        private DataTable _oDataTableProtocolsAll = new DataTable();

        // Правая таблица - записи
        private cspAreaGrid _cAreaProtocolsRecords = new cspAreaGrid();
        private DataTable _oDataTableProtocolsRecord = new DataTable();

        // Данные
        private List<ProtocolRecord> _allRecords = new List<ProtocolRecord>();
        private ProtocolLoader _loader = new ProtocolLoader();

        /// <summary>
        /// Источник данных текущей открытой базы протоколов (устанавливается при "Файл → Открыть протокол"),
        /// используется при выборе протокола слева для загрузки его записей справа
        /// </summary>
        private datUnitDataSource _oDataSourceOpen = null;
        private string _databaseFile = "";
        private string _filterText = "";
        private string _filterType = "";
        private string _filterApplication = "";
        private string _filterHost = "";
        private string _filterUser = "";
        private DateTime? _filterDateFrom;
        private DateTime? _filterDateTo;

        private Label lblStatus;

        #endregion

        #region = МЕТОДЫ

        #region - Конструктор

        public cspFormMain()
        {
            _mObjectAssembly();
            _mObjectPresentation();
            Load += mForm_Load;
        }

        #endregion

        #region - Объект

        protected override void _mObjectAssembly()
        {
            base._mObjectAssembly();
            if (_cAreaProtocols == null)
                return;

            var mainBlock = new elmBlockFormMain();
            Controls.Add(mainBlock);

            var splitter = new elmComponentSplitter { Dock = DockStyle.Fill };
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(4, 12, 4, 2)
            };
            contentPanel.Controls.Add(splitter);
            mainBlock.Controls.Add(contentPanel);
            mainBlock.__cMenu.BringToFront();

            var leftPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            leftPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            splitter.Panel1.Controls.Add(leftPanel);

            var leftHeader = new Label
            {
                Text = "ПРОТОКОЛЫ",
                Dock = DockStyle.Top,
                Height = 32,
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            leftPanel.Controls.Add(leftHeader, 0, 0);

            _cAreaProtocols = new cspAreaGrid
            {
                Dock = DockStyle.Fill,
                __fHeaderVisible_ = false
            };
            leftPanel.Controls.Add(_cAreaProtocols, 0, 1);

            // Колонки для левой таблицы
            _oDataTableProtocols.Columns.Clear();
            _oDataTableProtocols.Columns.Add("CLU", typeof(string));
            _oDataTableProtocols.Columns.Add("CHG", typeof(string));
            _oDataTableProtocols.Columns.Add("desApp", typeof(string));
            _oDataTableProtocols.Columns.Add("desPclTyp", typeof(string));
            _oDataTableProtocols.Columns.Add("Hst", typeof(string));
            _oDataTableProtocols.Columns.Add("Prc", typeof(string));
            _oDataTableProtocols.Columns.Add("Usr", typeof(string));

            _cAreaProtocols.__fDataSource_ = _oDataTableProtocols;
            _cAreaProtocols.__fToolBarVisible_ = false;
            _cAreaProtocols.__fGrid_.VirtualMode = false;
            _cAreaProtocols.__fGrid_.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _cAreaProtocols.__fGrid_.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            _cAreaProtocols.__fGrid_.RowHeadersVisible = false;
            _cAreaProtocols.__fGrid_.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _cAreaProtocols.__fGrid_.ReadOnly = true;

            if (_cAreaProtocols.__fGrid_.Columns.Count == 0)
            {
                _cAreaProtocols.__mColumnAdd("Протокол", "Ключ протокола", "CLU", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Время", "Время создания", "CHG", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                /// БАГ (исправлено): запрос File→Open возвращает поле 'desApp' (не 'App') и
                /// 'desPclTyp' (не 'PclTyp') - см. 'mMenuFileOpen_Click'. При старом связывании эти
                /// две колонки всегда оставались пустыми, поскольку 'DataPropertyName' не совпадал
                /// ни с одним столбцом фактической таблицы
                _cAreaProtocols.__mColumnAdd("Приложение", "Приложение", "desApp", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Вид", "Вид протокола", "desPclTyp", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Хост", "Компьютер", "Hst", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Процедура", "Процедура", "Prc", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Пользователь", "Пользователь", "Usr", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mGridBuild();
                mGridDisplayConfigure(_cAreaProtocols.__fGrid_);
            }

            _cAreaProtocols.__eGridCellEnter += mAreaProtocols_GridCellEnter;
            _cAreaProtocols.__fGrid_.SelectionChanged += mAreaProtocols_GridCellEnter;

            var rightPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            rightPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rightPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            rightPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rightPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            splitter.Panel2.Controls.Add(rightPanel);

            var rightHeader = new Label
            {
                Text = "ЗАПИСИ В ПРОТОКОЛАХ",
                Dock = DockStyle.Top,
                Height = 32,
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            rightPanel.Controls.Add(rightHeader, 0, 0);

            _cAreaProtocolsRecords = new cspAreaGrid
            {
                Dock = DockStyle.Fill,
                __fHeaderVisible_ = false
            };
            rightPanel.Controls.Add(_cAreaProtocolsRecords, 0, 1);

            // Колонки для правой таблицы: записи выбранного протокола
            _oDataTableProtocolsRecord.Columns.Clear();
            _oDataTableProtocolsRecord.Columns.Add("CLU", typeof(string));
            _oDataTableProtocolsRecord.Columns.Add("lnkPcl", typeof(string));
            _oDataTableProtocolsRecord.Columns.Add("desRrdTyp", typeof(string));
            _oDataTableProtocolsRecord.Columns.Add("Msg", typeof(string));
            _oDataTableProtocolsRecord.Columns.Add("Tck", typeof(string));

            _cAreaProtocolsRecords.__fDataSource_ = _oDataTableProtocolsRecord;

            if (_cAreaProtocolsRecords.__fGrid_.Columns.Count == 0)
            {
                /// БАГ (исправлено): колонки были связаны с полями 'Protocol'/'Key'/'Type'/'Message'/'Time',
                /// которых не существует ни в исходной пустой таблице, ни в результате запроса
                /// 'SELECT PR.*, RT.desRrdTyp FROM PclRrd ...' (см. ниже, метод загрузки записей) -
                /// правая таблица была пустой при любых данных
                _cAreaProtocolsRecords.__mColumnAdd("Ключ", "Ключ записи", "CLU", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mColumnAdd("Протокол", "Ключ протокола", "lnkPcl", true, false, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mColumnAdd("Вид", "Вид записи", "desRrdTyp", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mColumnAdd("Сообщение", "Сообщение", "Msg", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mColumnAdd("Время", "Время", "Tck", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mGridBuild();
                mGridDisplayConfigure(_cAreaProtocolsRecords.__fGrid_);
            }

            // Статус
            lblStatus = new Label
            {
                Dock = DockStyle.Fill,
                Height = 25,
                BackColor = SystemColors.Info,
                Text = "Выберите протокол из списка слева",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(80, 80, 80),
                Padding = new Padding(10, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft
            };
            rightPanel.Controls.Add(lblStatus, 0, 2);

            // Меню
            var menuFile = new elmComponentMenuItem { __fCaption_ = "Файл" };
            var menuOpen = new elmComponentMenuItem { __fCaption_ = "Открыть протокол" };
            menuOpen.Click += mMenuFileOpen_Click;
            menuFile.DropDownItems.Add(menuOpen);

            //var menuClose = new elmComponentMenuItem { __fCaption_ = "Закрыть" };
            //menuClose.Click += (s, e) => this.Close();
            //menuFile.DropDownItems.Add(menuClose);

            mainBlock.__mMenuAdd(menuFile);

            var menuData = new elmComponentMenuItem { __fCaption_ = "Данные" };
            var menuRefresh = new elmComponentMenuItem { __fCaption_ = "Обновить из папок" };
            menuRefresh.Click += mMenuDataRefresh_Click;
            menuData.DropDownItems.Add(menuRefresh);
            var menuFilters = new elmComponentMenuItem { __fCaption_ = "Фильтры..." };
            menuFilters.Click += mMenuFilters_Click;
            menuData.DropDownItems.Add(menuFilters);
            mainBlock.__mMenuAdd(menuData, 1);

            __fCaption_ = cspApplication.__fCaption_;
            ShowInTaskbar = true;

            ResumeLayout();
        }

        #endregion

        #region - События

        private static void mGridDisplayConfigure(DataGridView pGrid)
        {
            pGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            foreach (DataGridViewColumn vColumn in pGrid.Columns)
                vColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (pGrid.Columns.Contains("Msg")) pGrid.Columns["Msg"].FillWeight = 300F;
            if (pGrid.Columns.Contains("Prc")) pGrid.Columns["Prc"].FillWeight = 160F;
            if (pGrid.Columns.Contains("desApp")) pGrid.Columns["desApp"].FillWeight = 130F;
            if (pGrid.Columns.Contains("desPclTyp")) pGrid.Columns["desPclTyp"].FillWeight = 120F;
        }

        private void mAreaProtocols_GridCellEnter(object sender, EventArgs e)
        {
           
            DataGridViewRow vRow = _cAreaProtocols.__fCurrentRow_;
            if (vRow == null || vRow.Cells["CLU"] == null || vRow.Cells["CLU"].Value == null)
                return;

            string vProtocolClue = vRow.Cells["CLU"].Value.ToString();

            if (_oDataSourceOpen == null)
            {
                ProtocolRecord vProtocol = _allRecords.FirstOrDefault(p => p.Guid == vProtocolClue);
                if (vProtocol == null)
                    return;
                List<ProtocolRecord> vRecords = _loader.LoadRecordsForProtocol(vProtocol.SourceFile, vProtocol.Guid);
                _oDataTableProtocolsRecord.Rows.Clear();
                foreach (ProtocolRecord vRecord in vRecords)
                    _oDataTableProtocolsRecord.Rows.Add(vRecord.Guid, vProtocol.Guid, vRecord.RecordType, vRecord.Message, vRecord.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                _cAreaProtocolsRecords.__fDataSource_ = _oDataTableProtocolsRecord;
                return;
            }

            string vProtocolLink = mColumnName(_oDataSourceOpen, "PclRrd", "InkPcl", "lnkPcl");
            string vRecordType = mColumnName(_oDataSourceOpen, "PclRrd", "desRrdTyp", "lnkRrdTyp", "InkPclRrdTyp");
            string vMessage = mColumnName(_oDataSourceOpen, "PclRrd", "Msg", "Err");
            string vTick = mColumnName(_oDataSourceOpen, "PclRrd", "Tck", "CHG");
            if (String.IsNullOrEmpty(vProtocolLink))
            {
                lblStatus.Text = "В базе отсутствует связь записей с протоколом.";
                return;
            }

            string vProtocolGid = "";
            if (!String.IsNullOrEmpty(mColumnName(_oDataSourceOpen, "Pcl", "GID")))
            {
                object vGid = _oDataSourceOpen.__mSqlValue("SELECT GID FROM Pcl WHERE CLU = " + vProtocolClue);
                if (vGid != null && vGid != DBNull.Value)
                    vProtocolGid = Convert.ToString(vGid).Replace("'", "''");
            }

            long vNumericClue;
            string vWhere = Int64.TryParse(vProtocolClue, out vNumericClue)
                ? "PR." + vProtocolLink + " = " + vNumericClue
                : "PR." + vProtocolLink + " = '" + vProtocolClue.Replace("'", "''") + "'";
            if (!String.IsNullOrEmpty(vProtocolGid))
                vWhere += " OR PR." + vProtocolLink + " = '" + vProtocolGid + "'";

            string vQueryPclRrd = "SELECT PR.CLU, PR." + vProtocolLink + " AS lnkPcl, "
                + (String.IsNullOrEmpty(vRecordType) ? "''" : "PR." + vRecordType) + " AS desRrdTyp, "
                + (String.IsNullOrEmpty(vMessage) ? "''" : "PR." + vMessage) + " AS Msg, "
                + (String.IsNullOrEmpty(vTick) ? "0" : "PR." + vTick) + " AS Tck "
                + "FROM PclRrd PR WHERE " + vWhere;

            DataTable vDataTablePclRrd = _oDataSourceOpen.__mSqlQuery(vQueryPclRrd);
            if (vDataTablePclRrd == null)
            {
                lblStatus.Text = "Не удалось прочитать записи выбранного протокола.";
                return;
            }

            _oDataTableProtocolsRecord.Rows.Clear();
            foreach (DataRow vDataRow in vDataTablePclRrd.Rows)
            {
                long vTicks;
                string vTime = long.TryParse(Convert.ToString(vDataRow["Tck"]), out vTicks) && vTicks > 0
                    ? new DateTime(vTicks).ToString("yyyy-MM-dd HH:mm:ss") : "";
                _oDataTableProtocolsRecord.Rows.Add(vDataRow["CLU"], vDataRow["lnkPcl"], vDataRow["desRrdTyp"], vDataRow["Msg"], vTime);
            }

            _cAreaProtocolsRecords.__fDataSource_ = _oDataTableProtocolsRecord;
            _cAreaProtocolsRecords.__fToolBarVisible_ = false;
            _cAreaProtocolsRecords.__fGrid_.VirtualMode = false;
            _cAreaProtocolsRecords.__fGrid_.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _cAreaProtocolsRecords.__fGrid_.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            _cAreaProtocolsRecords.__fGrid_.RowHeadersVisible = false;
            _cAreaProtocolsRecords.__fGrid_.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _cAreaProtocolsRecords.__fGrid_.ReadOnly = true;
            _cAreaProtocolsRecords.__mGridRefresh();

            lblStatus.Text = $"Протокол {vProtocolClue}: {vDataTablePclRrd.Rows.Count} записей";
        }

        private void DisplayRecords(List<ProtocolRecord> records)
        {
            _oDataTableProtocolsRecord.Rows.Clear();

            if (records.Count == 0)
            {
                _cAreaProtocolsRecords.__fDataSource_ = _oDataTableProtocolsRecord;
                _cAreaProtocolsRecords.__mGridRefresh();
                return;
            }

            foreach (var record in records)
            {
                DataRow row = _oDataTableProtocolsRecord.NewRow();
                row["Protocol"] = record.Program ?? "";

                string key = record.Key;
                if (string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(record.Guid))
                {
                    key = record.Guid.Length >= 8 ? record.Guid.Substring(0, 8) : record.Guid;
                }
                row["Key"] = key ?? "";

                row["Type"] = record.RecordType ?? "Action";
                row["Message"] = record.Description ?? record.Message ?? "";
                row["Time"] = record.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                _oDataTableProtocolsRecord.Rows.Add(row);
            }

            _cAreaProtocolsRecords.__fDataSource_ = _oDataTableProtocolsRecord;
            _cAreaProtocolsRecords.__mGridRefresh();
        }

        private void mForm_Load(object sender, EventArgs e)
        {
            string vDatabaseFile = Path.Combine(appApplication.__oPathes.__fDirectoryDatabases_, "protocols.db");
            // Импорт не должен выполняться при каждом запуске: старые файлы уже находятся в БД.
            // Иначе одна ошибка файла блокирует форму серией окон, а большой архив перегружает грид.
            if (!File.Exists(vDatabaseFile))
            {
                ProtocolsDbLoader vLoader = new ProtocolsDbLoader(vDatabaseFile);
                string vBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string vArchiveDirectory = Path.Combine(vBaseDirectory, "Protocols");
                vLoader.CopyFolderToArchive(Path.Combine(vBaseDirectory, "PROTOCOLs"), vArchiveDirectory);
                vLoader.CopyFolderToArchive(Path.Combine(vBaseDirectory, "RELEASE"), Path.Combine(vArchiveDirectory, "RELEASE"));
                vLoader.LoadFromFolder(vArchiveDirectory);
            }
            mDatabaseOpen(vDatabaseFile);
        }

        private void mDatabaseOpen(string pFilePath)
        {
            if (File.Exists(pFilePath) == false)
            {
                lblStatus.Text = "Файл базы не найден: " + pFilePath;
                return;
            }

            datUnitDataSource vDataSource = new dsqDataSourceSqlite();
            vDataSource.__fDatabasePath = Path.GetDirectoryName(pFilePath);
            vDataSource.__fDatabaseName = Path.GetFileName(pFilePath);
            vDataSource.__mDatabaseCreate();
            _oDataSourceOpen = vDataSource;
            _databaseFile = pFilePath;

            string vAppColumn = mColumnName(vDataSource, "App", "dsiApp", "desApp", "App");
            string vAppLink = mColumnName(vDataSource, "Pcl", "InkApp", "lnkApp");
            string vTypeColumn = mColumnName(vDataSource, "Pcl", "desPclTyp", "lnkPclTyp", "InkPclTyp", "PclTyp");
            string vHostColumn = mColumnName(vDataSource, "Pcl", "Hst", "InkCpu");
            string vUserColumn = mColumnName(vDataSource, "Pcl", "Usr", "InkUsr");
            string vAppExpression = String.IsNullOrEmpty(vAppColumn) ? "''" : "A." + vAppColumn;
            string vTypeExpression = String.IsNullOrEmpty(vTypeColumn) ? "''" : "P." + vTypeColumn;
            string vHostExpression = String.IsNullOrEmpty(vHostColumn) ? "''" : "P." + vHostColumn;
            string vUserExpression = String.IsNullOrEmpty(vUserColumn) ? "''" : "P." + vUserColumn;
            string vJoin = String.IsNullOrEmpty(vAppColumn) || String.IsNullOrEmpty(vAppLink) ? "" : " LEFT JOIN App A ON A.CLU = P." + vAppLink;
            DataTable vDataTablePcl = vDataSource.__mSqlQuery("SELECT P.CLU, P.CHG, " + vAppExpression + " AS desApp, " + vTypeExpression + " AS desPclTyp, " + vHostExpression + " AS Hst, P.Prc, " + vUserExpression + " AS Usr FROM Pcl P" + vJoin + " ORDER BY P.CHG DESC LIMIT 500");
            if (vDataTablePcl == null)
            {
                lblStatus.Text = "Структура базы не поддерживается или база недоступна.";
                return;
            }
            _oDataTableProtocols.Rows.Clear();
            foreach (DataRow vDataRow in vDataTablePcl.Rows)
            {
                long vTicks;
                string vTime = vDataRow["CHG"] != DBNull.Value && long.TryParse(vDataRow["CHG"].ToString(), out vTicks) && vTicks > 0
                    ? new DateTime(vTicks).ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToString(vDataRow["CHG"]);
                _oDataTableProtocols.Rows.Add(vDataRow["CLU"], vTime, vDataRow["desApp"], vDataRow["desPclTyp"], vDataRow["Hst"], vDataRow["Prc"], vDataRow["Usr"]);
            }
            mProtocolsFilterSourceSet();
            _cAreaProtocols.__fDataSource_ = _oDataTableProtocols;
            _cAreaProtocols.__mGridRefresh();
            _oDataTableProtocolsRecord.Rows.Clear();
            _cAreaProtocolsRecords.__fDataSource_ = _oDataTableProtocolsRecord;
            _cAreaProtocolsRecords.__mGridRefresh();
            lblStatus.Text = "Загружено из БД: " + _oDataTableProtocols.Rows.Count + " последних протоколов";

            if (_cAreaProtocols.__fGrid_.Rows.Count > 0 && _cAreaProtocols.__fGrid_.Columns.Count > 0)
            {
                _cAreaProtocols.__fGrid_.CurrentCell = _cAreaProtocols.__fGrid_[0, 0];
                mAreaProtocols_GridCellEnter(_cAreaProtocols.__fGrid_, EventArgs.Empty);
            }
        }

        private void mProtocolFileOpen(string pFilePath)
        {
            _oDataSourceOpen = null;
            _allRecords = _loader.LoadSingleFile(pFilePath);
            _oDataTableProtocols.Rows.Clear();
            foreach (ProtocolRecord vRecord in _allRecords.OrderByDescending(p => p.DateTime))
                _oDataTableProtocols.Rows.Add(vRecord.Guid, vRecord.DateTime.ToString("yyyy-MM-dd HH:mm:ss"), vRecord.Program, vRecord.ProtocolType, vRecord.Computer, vRecord.Procedure, vRecord.User);
            mProtocolsFilterSourceSet();
            _cAreaProtocols.__fDataSource_ = _oDataTableProtocols;
            _oDataTableProtocolsRecord.Rows.Clear();
            _cAreaProtocolsRecords.__fDataSource_ = _oDataTableProtocolsRecord;
            lblStatus.Text = "Загружено из файла: " + _allRecords.Count + " протоколов";
        }

        private void mProtocolsFilterSourceSet()
        {
            _oDataTableProtocolsAll = _oDataTableProtocols.Copy();
            // Фильтр применяется программно; отдельная панель здесь не нужна,
            // чтобы не занимать место над заголовками таблиц.
        }

        private void mProtocolsFilterApply()
        {
            if (_oDataTableProtocolsAll == null || _oDataTableProtocolsAll.Columns.Count == 0) return;
            _oDataTableProtocols.Rows.Clear();
            foreach (DataRow vRow in _oDataTableProtocolsAll.Rows)
            {
                string vAllText = String.Join(" ", vRow.ItemArray.Select(pValue => Convert.ToString(pValue)));
                if (!String.IsNullOrEmpty(_filterText) && vAllText.IndexOf(_filterText, StringComparison.OrdinalIgnoreCase) < 0) continue;
                if (!mFilterEquals(vRow, "desPclTyp", _filterType)) continue;
                if (!mFilterEquals(vRow, "desApp", _filterApplication)) continue;
                if (!mFilterEquals(vRow, "Hst", _filterHost)) continue;
                if (!mFilterEquals(vRow, "Usr", _filterUser)) continue;
                DateTime vProtocolDate;
                if ((_filterDateFrom.HasValue || _filterDateTo.HasValue) && (!DateTime.TryParse(Convert.ToString(vRow["CHG"]), out vProtocolDate)
                    || (_filterDateFrom.HasValue && vProtocolDate.Date < _filterDateFrom.Value.Date)
                    || (_filterDateTo.HasValue && vProtocolDate.Date > _filterDateTo.Value.Date))) continue;
                _oDataTableProtocols.Rows.Add(vRow.ItemArray);
            }
            if (_cAreaProtocols != null) _cAreaProtocols.__mGridRefresh();
            lblStatus.Text = "Найдено протоколов: " + _oDataTableProtocols.Rows.Count;
        }

        private static bool mFilterEquals(DataRow pRow, string pColumnName, string pValue)
        {
            return String.IsNullOrEmpty(pValue) || String.Equals(Convert.ToString(pRow[pColumnName]), pValue, StringComparison.OrdinalIgnoreCase);
        }

        private void mMenuFilters_Click(object sender, EventArgs e)
        {
            using (Form vForm = new Form())
            {
                vForm.Text = "Фильтры протоколов";
                vForm.StartPosition = FormStartPosition.CenterParent;
                vForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                vForm.MinimizeBox = false;
                vForm.MaximizeBox = false;
                vForm.ClientSize = new Size(430, 275);

                TableLayoutPanel vLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 9, Padding = new Padding(10) };
                vLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125));
                vLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                for (int vIndex = 0; vIndex < 8; vIndex++) vLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 27));
                vLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

                TextBox vText = new TextBox { Dock = DockStyle.Fill, Text = _filterText };
                ComboBox vType = mFilterComboCreate("desPclTyp", _filterType);
                ComboBox vApp = mFilterComboCreate("desApp", _filterApplication);
                ComboBox vHost = mFilterComboCreate("Hst", _filterHost);
                ComboBox vUser = mFilterComboCreate("Usr", _filterUser);
                DateTimePicker vDateFrom = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = _filterDateFrom.HasValue, Value = _filterDateFrom ?? DateTime.Today };
                DateTimePicker vDateTo = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = _filterDateTo.HasValue, Value = _filterDateTo ?? DateTime.Today };
                mFilterControlAdd(vLayout, "Поиск", vText, 0);
                mFilterControlAdd(vLayout, "Вид протокола", vType, 1);
                mFilterControlAdd(vLayout, "Приложение", vApp, 2);
                mFilterControlAdd(vLayout, "Компьютер", vHost, 3);
                mFilterControlAdd(vLayout, "Пользователь", vUser, 4);
                mFilterControlAdd(vLayout, "Дата с", vDateFrom, 5);
                mFilterControlAdd(vLayout, "Дата по", vDateTo, 6);

                FlowLayoutPanel vButtons = new FlowLayoutPanel { Dock = DockStyle.Right, FlowDirection = FlowDirection.RightToLeft };
                Button vApply = new Button { Text = "Применить", DialogResult = DialogResult.OK, Width = 90 };
                Button vClear = new Button { Text = "Сбросить", Width = 80 };
                vClear.Click += (pSender, pEvent) => { vText.Text = ""; vType.SelectedIndex = 0; vApp.SelectedIndex = 0; vHost.SelectedIndex = 0; vUser.SelectedIndex = 0; vDateFrom.Checked = false; vDateTo.Checked = false; };
                vButtons.Controls.Add(vApply);
                vButtons.Controls.Add(vClear);
                vLayout.Controls.Add(vButtons, 1, 8);
                vForm.Controls.Add(vLayout);

                if (vForm.ShowDialog(this) == DialogResult.OK)
                {
                    _filterText = vText.Text.Trim();
                    _filterType = Convert.ToString(vType.SelectedItem);
                    _filterApplication = Convert.ToString(vApp.SelectedItem);
                    _filterHost = Convert.ToString(vHost.SelectedItem);
                    _filterUser = Convert.ToString(vUser.SelectedItem);
                    _filterDateFrom = vDateFrom.Checked ? vDateFrom.Value.Date : (DateTime?)null;
                    _filterDateTo = vDateTo.Checked ? vDateTo.Value.Date : (DateTime?)null;
                    mProtocolsFilterApply();
                }
            }
        }

        private ComboBox mFilterComboCreate(string pColumnName, string pSelectedValue)
        {
            ComboBox vCombo = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            vCombo.Items.Add("");
            if (_oDataTableProtocolsAll.Columns.Contains(pColumnName))
                foreach (string vValue in _oDataTableProtocolsAll.AsEnumerable().Select(pRow => Convert.ToString(pRow[pColumnName])).Where(pValue => !String.IsNullOrWhiteSpace(pValue)).Distinct().OrderBy(pValue => pValue))
                    vCombo.Items.Add(vValue);
            vCombo.SelectedItem = pSelectedValue;
            if (vCombo.SelectedIndex < 0) vCombo.SelectedIndex = 0;
            return vCombo;
        }

        private static void mFilterControlAdd(TableLayoutPanel pLayout, string pCaption, Control pControl, int pRow)
        {
            pLayout.Controls.Add(new Label { Text = pCaption, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, pRow);
            pLayout.Controls.Add(pControl, 1, pRow);
        }

        private static string mColumnName(datUnitDataSource pDataSource, string pTable, params string[] pCandidates)
        {
            DataTable vColumns = pDataSource.__mSqlQuery("PRAGMA table_info(" + pTable + ")");
            if (vColumns == null)
                return "";
            foreach (string vCandidate in pCandidates)
                foreach (DataRow vRow in vColumns.Rows)
                    if (String.Equals(Convert.ToString(vRow["name"]), vCandidate, StringComparison.OrdinalIgnoreCase))
                        return Convert.ToString(vRow["name"]);
            return "";
        }

        private void mMenuDataRefresh_Click(object sender, EventArgs e)
        {
            string vDatabaseFile = String.IsNullOrEmpty(_databaseFile)
                ? Path.Combine(appApplication.__oPathes.__fDirectoryDatabases_, "protocols.db") : _databaseFile;
            string vBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string vArchiveDirectory = Path.Combine(vBaseDirectory, "Protocols");

            UseWaitCursor = true;
            try
            {
                ProtocolsDbLoader vLoader = new ProtocolsDbLoader(vDatabaseFile);
                int vCopied = vLoader.CopyFolderToArchive(Path.Combine(vBaseDirectory, "PROTOCOLs"), vArchiveDirectory);
                vCopied += vLoader.CopyFolderToArchive(Path.Combine(vBaseDirectory, "RELEASE"), Path.Combine(vArchiveDirectory, "RELEASE"));
                int vImported = vLoader.LoadFromFolder(vArchiveDirectory);
                mDatabaseOpen(vDatabaseFile);
                lblStatus.Text += "; скопировано: " + vCopied + ", добавлено: " + vImported;
            }
            finally
            {
                UseWaitCursor = false;
            }
        }

        #endregion

        #region - Меню "Открыть протокол"

        private void mMenuFileOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog vOpenFileDialog = new OpenFileDialog())
            {
                vOpenFileDialog.Filter = "Протоколы и базы (*.db;*.pcl)|*.db;*.pcl|База данных протоколов (*.db)|*.db|Файлы протоколов (*.pcl)|*.pcl|Все файлы (*.*)|*.*";
                if (vOpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (String.Equals(Path.GetExtension(vOpenFileDialog.FileName), ".pcl", StringComparison.OrdinalIgnoreCase))
                        mProtocolFileOpen(vOpenFileDialog.FileName);
                    else
                        mDatabaseOpen(vOpenFileDialog.FileName);
                }
            }
        }

        #endregion

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // __cPanelStatus
            // 
            this.@__cPanelStatus.Location = new System.Drawing.Point(0, 578);
            this.@__cPanelStatus.Size = new System.Drawing.Size(690, 27);
            // 
            // cspFormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(690, 605);
            this.Name = "cspFormMain";
            this.ResumeLayout(false);

        }
    }
}
