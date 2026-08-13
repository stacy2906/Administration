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

namespace naCsProtocols
{
    public class cspFormMain : elmForm
    {
        #region = ПОЛЯ

        // Левая таблица - протоколы
        private cspAreaGrid _cAreaProtocols = new cspAreaGrid();
        private DataTable _oDataTableProtocols = new DataTable();

        // Правая таблица - записи
        private cspAreaGrid _cAreaProtocolsRecords = new cspAreaGrid();
        private DataTable _oDataTableProtocolsRecord = new DataTable();

        // Данные
        private dsqProtocols _oProtocols;
        private bool _manualDbMode = false; // [true] - открыт сторонний .db вручную; клик по строке не должен запрашивать записи из _oProtocols (другая база)

        private Label lblStatus;

        #endregion

        #region = МЕТОДЫ

        #region - Конструктор

        public cspFormMain()
        {
            _mObjectAssembly();
            _mObjectPresentation();
            mProtocolsAutoLoad();
        }

        #endregion

        #region - Объект

        protected override void _mObjectAssembly()
        {
            base._mObjectAssembly();

            var mainBlock = new elmBlockFormMain();
            Controls.Add(mainBlock);

            var splitter = new elmComponentSplitter();
            mainBlock.Controls.Add(splitter);

            // ============================================================
            // ЛЕВАЯ ПАНЕЛЬ - ПРОТОКОЛЫ
            // ============================================================
            var leftPanel = new Panel { Dock = DockStyle.Fill };
            splitter.Panel1.Controls.Add(leftPanel);

            var leftHeader = new Label
            {
                Text = "📋 ПРОТОКОЛЫ",
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            leftPanel.Controls.Add(leftHeader);

            _cAreaProtocols = new cspAreaGrid
            {
                Dock = DockStyle.Fill,
                __fHeaderVisible_ = false
            };
            leftPanel.Controls.Add(_cAreaProtocols);

            // Колонки для левой таблицы
            _oDataTableProtocols.Columns.Clear();
            _oDataTableProtocols.Columns.Add("CLU", typeof(string));
            _oDataTableProtocols.Columns.Add("CHG", typeof(string));
            _oDataTableProtocols.Columns.Add("App", typeof(string));
            _oDataTableProtocols.Columns.Add("PclTyp", typeof(string));
            _oDataTableProtocols.Columns.Add("Hst", typeof(string));
            _oDataTableProtocols.Columns.Add("Prc", typeof(string));
            _oDataTableProtocols.Columns.Add("Usr", typeof(string));

            _cAreaProtocols.__fDataSource_ = _oDataTableProtocols;

            if (_cAreaProtocols.__fGrid_.Columns.Count == 0)
            {
                _cAreaProtocols.__mColumnAdd("Протокол", "Ключ протокола", "CLU", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Время", "Время создания", "CHG", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Приложение", "Приложение", "App", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Вид", "Вид протокола", "PclTyp", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Хост", "Компьютер", "Hst", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Процедура", "Процедура", "Prc", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Пользователь", "Пользователь", "Usr", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mGridBuild();
            }

            _cAreaProtocols.__eGridCellEnter += mAreaProtocols_GridCellEnter;

            // ============================================================
            // ПРАВАЯ ПАНЕЛЬ - ЗАПИСИ В ПРОТОКОЛАХ
            // ============================================================
            var rightPanel = new Panel { Dock = DockStyle.Fill };
            splitter.Panel2.Controls.Add(rightPanel);

            var rightHeader = new Label
            {
                Text = "📝 ЗАПИСИ В ПРОТОКОЛАХ",
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            rightPanel.Controls.Add(rightHeader);

            _cAreaProtocolsRecords = new cspAreaGrid
            {
                Dock = DockStyle.Fill,
                __fHeaderVisible_ = false
            };
            rightPanel.Controls.Add(_cAreaProtocolsRecords);

            // Колонки для правой таблицы: Протокол, Ключ, Вид, Сообщение, Время
            _oDataTableProtocolsRecord.Columns.Clear();
            _oDataTableProtocolsRecord.Columns.Add("Protocol", typeof(string));
            _oDataTableProtocolsRecord.Columns.Add("Key", typeof(string));
            _oDataTableProtocolsRecord.Columns.Add("Type", typeof(string));
            _oDataTableProtocolsRecord.Columns.Add("Message", typeof(string));
            _oDataTableProtocolsRecord.Columns.Add("Time", typeof(string));

            _cAreaProtocolsRecords.__fDataSource_ = _oDataTableProtocolsRecord;

            if (_cAreaProtocolsRecords.__fGrid_.Columns.Count == 0)
            {
                _cAreaProtocolsRecords.__mColumnAdd("Протокол", "Ключ протокола", "Protocol", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mColumnAdd("Ключ", "Ключ записи", "Key", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mColumnAdd("Вид", "Вид записи", "Type", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mColumnAdd("Сообщение", "Сообщение", "Message", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mColumnAdd("Время", "Время", "Time", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mGridBuild();
            }

            // Статус
            lblStatus = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 25,
                BackColor = SystemColors.Info,
                Text = "Выберите протокол из списка слева",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(80, 80, 80),
                Padding = new Padding(10, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft
            };
            rightPanel.Controls.Add(lblStatus);

            // Меню
            var menuFile = new elmComponentMenuItem { __fCaption_ = "Файл" };
            var menuOpen = new elmComponentMenuItem { __fCaption_ = "Открыть протокол" };
            menuOpen.Click += mMenuFileOpen_Click;
            menuFile.DropDownItems.Add(menuOpen);

            var menuClose = new elmComponentMenuItem { __fCaption_ = "Закрыть" };
            menuClose.Click += (s, e) => this.Close();
            menuFile.DropDownItems.Add(menuClose);

            mainBlock.__mMenuAdd(menuFile);

            __fCaption_ = cspApplication.__fCaption_;
            ShowInTaskbar = true;

            ResumeLayout();
        }

        #endregion

        #region - События

        /// <summary>
        /// Загрузка списка протоколов из SQLite базы данных 'dsqProtocols' (общий экземпляр, назначенный
        /// активным логгером приложения в 'cspBegin.cs' - легаси '.pcl' файлы импортируются в неё же при старте)
        /// </summary>
        private void mProtocolsAutoLoad()
        {
            _oProtocols = cspApplication.__oProtocols as dsqProtocols;

            if (_oProtocols == null)
            {
                lblStatus.Text = "Логгер протоколов не инициализирован как SQLite (dsqProtocols)";
                return;
            }

            string vQuery = "SELECT P.CLU, P.CHG, A.desApp AS App, PT.desPclTyp AS PclTyp, P.Hst, P.Prc, P.Usr "
                + "FROM Pcl P "
                + "LEFT JOIN App A ON A.CLU = P.lnkApp "
                + "LEFT JOIN PclTyp PT ON PT.CLU = P.lnkPclTyp "
                + "ORDER BY P.CHG DESC"; // По умолчанию - сначала новые

            DataTable vDataTable;
            try
            {
                vDataTable = _oProtocols.__mQuery(vQuery);
            }
            catch (Exception vException)
            {
                lblStatus.Text = "Ошибка загрузки протоколов из БД: " + vException.Message;
                return;
            }

            mProtocolsDisplay(vDataTable);
        }
        /// <summary>
        /// Отображение списка протоколов в левой таблице
        /// </summary>
        /// <param name="pDataTable">Результат запроса к 'Pcl' (с полями CLU, CHG, App, PclTyp, Hst, Prc, Usr)</param>
        private void mProtocolsDisplay(DataTable pDataTable)
        {
            _oDataTableProtocols.Rows.Clear();

            foreach (DataRow vSourceRow in pDataTable.Rows)
            {
                DataRow vRow = _oDataTableProtocols.NewRow();
                vRow["CLU"] = vSourceRow["CLU"] != DBNull.Value ? vSourceRow["CLU"].ToString() : "";

                long vTicks;
                string vChgRaw = vSourceRow["CHG"] != DBNull.Value ? vSourceRow["CHG"].ToString() : "";
                vRow["CHG"] = long.TryParse(vChgRaw, out vTicks) ? new DateTime(vTicks).ToString("dd.MM.yyyy HH:mm:ss") : vChgRaw;

                vRow["App"] = vSourceRow["App"] != DBNull.Value ? vSourceRow["App"].ToString() : "";
                vRow["PclTyp"] = vSourceRow["PclTyp"] != DBNull.Value ? vSourceRow["PclTyp"].ToString() : "";
                vRow["Hst"] = vSourceRow["Hst"] != DBNull.Value ? vSourceRow["Hst"].ToString() : "";
                vRow["Prc"] = vSourceRow["Prc"] != DBNull.Value ? vSourceRow["Prc"].ToString() : "";
                vRow["Usr"] = vSourceRow["Usr"] != DBNull.Value ? vSourceRow["Usr"].ToString() : "";
                _oDataTableProtocols.Rows.Add(vRow);
            }

            _cAreaProtocols.__fDataSource_ = _oDataTableProtocols;
            _cAreaProtocols.__mGridRefresh();

            _oDataTableProtocolsRecord.Rows.Clear();
            _cAreaProtocolsRecords.__fDataSource_ = _oDataTableProtocolsRecord;
            _cAreaProtocolsRecords.__mGridRefresh();

            lblStatus.Text = "Загружено протоколов: " + pDataTable.Rows.Count + " (SQLite: " + Path.Combine(appApplication.__oPathes.__fDirectoryDatabases_, "protocols.db") + ")";
        }

        private void mAreaProtocols_GridCellEnter(object sender, EventArgs e)
        {
            if (_manualDbMode == true || _oProtocols == null)
                return;

            DataGridViewRow vRow = _cAreaProtocols.__fCurrentRow_;
            if (vRow == null)
                return;

            object vCluValue = vRow.Cells["CLU"].Value;
            int vClu;
            if (vCluValue == null || int.TryParse(vCluValue.ToString(), out vClu) == false)
                return;

            string vQuery = "SELECT PR.lnkPcl AS Protocol, PR.CLU AS \"Key\", RT.desRrdTyp AS Type, PR.Msg AS Message, PR.Tck AS Time "
                + "FROM PclRrd PR "
                + "LEFT JOIN RrdTyp RT ON RT.CLU = PR.lnkRrdTyp "
                + "WHERE PR.lnkPcl = " + vClu.ToString();

            DataTable vRecords;
            try
            {
                vRecords = _oProtocols.__mQuery(vQuery);
            }
            catch
            {
                vRecords = new DataTable();
            }

            _cAreaProtocolsRecords.__fDataSource_ = vRecords;
            _cAreaProtocolsRecords.__mGridRefresh();

            lblStatus.Text = "Протокол CLU=" + vClu + " \u2014 записей: " + vRecords.Rows.Count;
        }

        #endregion

        #region - Меню "Открыть протокол"

        private void mMenuFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog vOpenFileDialog = new OpenFileDialog();
            vOpenFileDialog.AddExtension = true;
            vOpenFileDialog.AutoUpgradeEnabled = true;
            vOpenFileDialog.CheckFileExists = true;
            vOpenFileDialog.CheckPathExists = true;
            vOpenFileDialog.Filter = "База данных протоколов (*.db)|*.db|Все файлы (*.*)|*.*";

            if (vOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string vFilePath = vOpenFileDialog.FileName;
                _manualDbMode = true;
                datUnitDataSource vDataSource = new dsqDataSourceSqlite();
                vDataSource.__fDatabasePath = Path.GetDirectoryName(vFilePath);
                vDataSource.__fDatabaseName = Path.GetFileName(vFilePath);

                /// Примечание: реальная схема 'Protocols.db' (сгенерированная Model/Essence в _Administration) отличается
                /// от схемы, которую создаёт 'dsqProtocols': App.dsiApp (не desApp), PclTyp.dsiPclTyp (не desPclTyp),
                /// таблица записей называется 'PclRrdTyp' (не 'RrdTyp'), связь - 'lnkPclRrdTyp' (не 'lnkRrdTyp'),
                /// текст записи хранится в 'Err' (не 'Msg'). У 'Pcl' нет отдельных текстовых полей Hst/Usr - только
                /// нерасшифрованные связи lnkCpu/lnkUsr (таблиц Cpu/Usr в этой базе пока нет) - показываем как есть.
                string vQueryPcl = "SELECT P.CLU, P.CHG, A.dsiApp AS App, PT.dsiPclTyp AS PclTyp, P.Prc, P.lnkCpu AS Hst, P.lnkUsr AS Usr "
                    + "FROM Pcl P "
                    + "LEFT JOIN App A ON A.CLU = P.lnkApp "
                    + "LEFT JOIN PclTyp PT ON PT.CLU = P.lnkPclTyp";

                DataTable vDataTablePcl = vDataSource.__mSqlQuery(vQueryPcl);
                if (vDataTablePcl == null)
                {
                    lblStatus.Text = "Не удалось загрузить протоколы из '" + vFilePath + "' - см. окно ошибки выше";
                    return;
                }

                _cAreaProtocols.__fDataSource_ = vDataTablePcl;
                foreach (DataRow vDataRow in vDataTablePcl.Rows)
                {
                    long vChgTicks;
                    if (vDataRow["CHG"] != DBNull.Value && long.TryParse(vDataRow["CHG"].ToString(), out vChgTicks) == true)
                        vDataRow["CHG"] = new DateTime(vChgTicks).ToString();
                }

                string vQueryPclRrd = "SELECT PR.lnkPcl AS Protocol, PR.CLU AS \"Key\", RT.dsiPclRrdTyp AS Type, PR.Err AS Message, PR.Tck AS Time "
                    + "FROM PclRrd PR "
                    + "LEFT JOIN PclRrdTyp RT ON RT.CLU = PR.lnkPclRrdTyp";
                /// Примечание: 'Tck' в 'PclRrd' - затраченное время выполнения (тики-длительность), а не момент времени
                /// создания записи - у 'PclRrd' нет отдельной колонки CHG, поэтому выводится как есть, без форматирования в дату

                DataTable vDataTablePclRrd = vDataSource.__mSqlQuery(vQueryPclRrd);
                if (vDataTablePclRrd == null)
                {
                    lblStatus.Text = $"Протоколы загружены ({vDataTablePcl.Rows.Count}), но записи не удалось загрузить - см. окно ошибки выше";
                    return;
                }

                _cAreaProtocolsRecords.__fDataSource_ = vDataTablePclRrd;
                _cAreaProtocolsRecords.__mGridRefresh();

                lblStatus.Text = $"Загружено из БД: {vDataTablePcl.Rows.Count} протоколов";
            }
        }

        #endregion

        #endregion
    }
}