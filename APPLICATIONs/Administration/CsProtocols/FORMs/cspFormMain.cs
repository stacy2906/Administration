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
using System.Linq;

namespace naCsProtocols
{
    public class cspFormMain : elmForm
    {
        #region = ПОЛЯ

        private cspAreaGrid _cAreaProtocols = new cspAreaGrid();
        private DataTable _oDataTableProtocols = new DataTable();
        private cspAreaGrid _cAreaProtocolsRecords = new cspAreaGrid();
        private DataTable _oDataTableProtocolsRecord = new DataTable();
        private Label lblStatus;
        private datUnitDataSource _currentDataSource = null;
        private string _currentFilePath = string.Empty;
        private string _dbPath = string.Empty;

        #endregion

        #region = МЕТОДЫ

        #region - Конструктор

        public cspFormMain()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _dbPath = Path.Combine(baseDir, @"..\..\..\..\DATABASES\Protocols.db");

            if (!File.Exists(_dbPath))
            {
                _dbPath = @"C:\Users\doy\Documents\GitHub\Administration\DATABASES\Protocols.db";
            }

            _mObjectAssembly();
            _mObjectPresentation();

            if (File.Exists(_dbPath))
            {
                LoadDataAndShow();
            }
            else
            {
                lblStatus.Text = "Выберите папку с протоколами через меню 'Файл -> Открыть протокол...'";
                lblStatus.ForeColor = Color.DarkRed;
            }
        }

        #endregion

        #region - Загрузка данных из БД

        public void LoadDataAndShow()
        {
            if (string.IsNullOrEmpty(_dbPath) || !File.Exists(_dbPath))
            {
                lblStatus.Text = "Файл БД не найден!";
                lblStatus.ForeColor = Color.DarkRed;
                return;
            }

            _currentFilePath = _dbPath;
            _currentDataSource = new dsqDataSourceSqlite();
            _currentDataSource.__fDatabasePath = Path.GetDirectoryName(_dbPath);
            _currentDataSource.__fDatabaseName = Path.GetFileName(_dbPath);

            BindDataToGrid();
        }

        public void BindDataToGrid()
        {
            if (string.IsNullOrEmpty(_currentFilePath) || _currentDataSource == null) return;

            try
            {
                string vQueryPcl = @"
                    SELECT CLU, CHG, Prc, InkApp, InkPclTyp, Fil, InkUsr
                    FROM Pcl
                    ORDER BY CHG DESC";

                DataTable vDataTablePcl = _currentDataSource.__mSqlQuery(vQueryPcl);

                if (vDataTablePcl != null && vDataTablePcl.Rows.Count > 0)
                {
                    _cAreaProtocols.__fGrid_.VirtualMode = false;
                    _cAreaProtocols.__fDataSource_ = vDataTablePcl;
                    _cAreaProtocols.__mGridRefresh();

                    lblStatus.Text = $"✅ Загружено протоколов: {vDataTablePcl.Rows.Count}";
                    lblStatus.ForeColor = Color.DarkGreen;
                }
                else
                {
                    lblStatus.Text = "⚠️ Нет данных в таблице Pcl";
                    lblStatus.ForeColor = Color.Orange;
                }

                _cAreaProtocolsRecords.__fDataSource_ = null;
                _cAreaProtocolsRecords.__mGridRefresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region - Объект (Форма)

        protected override void _mObjectAssembly()
        {
            base._mObjectAssembly();

            var mainBlock = new elmBlockFormMain();
            Controls.Add(mainBlock);

            var splitter = new elmComponentSplitter();
            mainBlock.Controls.Add(splitter);

            // ЛЕВАЯ ПАНЕЛЬ
            var leftPanel = new Panel { Dock = DockStyle.Fill };
            splitter.Panel1.Controls.Add(leftPanel);

            var leftHeader = new Label
            {
                Text = "Протоколы",
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
            _cAreaProtocols.__fGrid_.VirtualMode = false;
            leftPanel.Controls.Add(_cAreaProtocols);

            if (_cAreaProtocols.__fGrid_.Columns.Count == 0)
            {
                _cAreaProtocols.__mColumnAdd("Протокол", "CLU", "CLU", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Время", "CHG", "CHG", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Приложение", "InkApp", "InkApp", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Вид", "InkPclTyp", "InkPclTyp", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Изображение", "Fil", "Fil", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Процедура", "Prc", "Prc", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Пользователь", "InkUsr", "InkUsr", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mGridBuild();
            }

            _cAreaProtocols.__eGridCellEnter += mAreaProtocols_GridCellEnter;

            // ПРАВАЯ ПАНЕЛЬ
            var rightPanel = new Panel { Dock = DockStyle.Fill };
            splitter.Panel2.Controls.Add(rightPanel);

            var rightHeader = new Label
            {
                Text = "Записи в протоколах",
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
            _cAreaProtocolsRecords.__fGrid_.VirtualMode = false;
            rightPanel.Controls.Add(_cAreaProtocolsRecords);

            if (_cAreaProtocolsRecords.__fGrid_.Columns.Count == 0)
            {
                _cAreaProtocolsRecords.__mColumnAdd("Протокол", "InkPcl", "InkPcl", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mColumnAdd("Ключ", "CLU", "CLU", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                // ✅ ПРАВИЛЬНО: InkPclRrdTyp
                _cAreaProtocolsRecords.__mColumnAdd("Вид", "InkPclRrdTyp", "InkPclRrdTyp", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mColumnAdd("Сообщение", "Err", "Err", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mColumnAdd("Время", "CHG", "CHG", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mGridBuild();
            }

            lblStatus = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 25,
                BackColor = SystemColors.Info,
                Text = "Готово к работе",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(80, 80, 80),
                Padding = new Padding(10, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft
            };
            rightPanel.Controls.Add(lblStatus);

            // МЕНЮ
            var menuFile = new elmComponentMenuItem { __fCaption_ = "Файл" };

            var menuOpen = new elmComponentMenuItem { __fCaption_ = "Открыть протокол..." };
            menuOpen.Click += mMenuFileOpen_Click;
            menuFile.DropDownItems.Add(menuOpen);

            var menuSeparator = new ToolStripSeparator();
            menuFile.DropDownItems.Add(menuSeparator);

            var menuClose = new elmComponentMenuItem { __fCaption_ = "Закрыть" };
            menuClose.Click += (s, e) => this.Close();
            menuFile.DropDownItems.Add(menuClose);

            mainBlock.__mMenuAdd(menuFile);

            __fCaption_ = cspApplication.__fCaption_;
            ShowInTaskbar = true;

            ResumeLayout();
        }

        #endregion

        #region - ОТКРЫТЬ ПРОТОКОЛ (ВЫБОР ПАПКИ)

        private void mMenuFileOpen_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Выберите папку с протоколами (.pcl файлы)";
                folderDialog.ShowNewFolderButton = false;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;

                    string dbPath = Path.Combine(selectedPath, "Protocols.db");

                    LoadPclFilesFromFolder(selectedPath, dbPath);

                    _dbPath = dbPath;
                    LoadDataAndShow();

                    lblStatus.Text = $"Загружено из папки: {selectedPath}";
                    lblStatus.ForeColor = Color.DarkGreen;
                }
            }
        }

        // ✅ ЗАГРУЗКА .pcl ФАЙЛОВ ИЗ ПАПКИ (ИСПРАВЛЕННАЯ)
        private void LoadPclFilesFromFolder(string folderPath, string dbPath)
        {
            try
            {
                var pclFiles = Directory.GetFiles(folderPath, "*.pcl", SearchOption.AllDirectories);

                if (pclFiles.Length == 0)
                {
                    MessageBox.Show("В папке нет .pcl файлов!", "Внимание");
                    return;
                }

                var dataSource = new dsqDataSourceSqlite();
                dataSource.__fDatabasePath = Path.GetDirectoryName(dbPath);
                dataSource.__fDatabaseName = Path.GetFileName(dbPath);

                // СОЗДАЕМ ТАБЛИЦЫ
                dataSource.__mSqlCommand(@"
                    CREATE TABLE IF NOT EXISTS Pcl (
                        CHG INTEGER,
                        CLU INTEGER PRIMARY KEY AUTOINCREMENT,
                        ELD INTEGER,
                        GID TEXT,
                        InkApp INTEGER,
                        InkCpu INTEGER,
                        InkPclTyp INTEGER,
                        InkUsr INTEGER,
                        Prc TEXT,
                        Fil INTEGER
                    )");

                dataSource.__mSqlCommand(@"
                    CREATE TABLE IF NOT EXISTS App (
                        CHG INTEGER,
                        CLU INTEGER PRIMARY KEY AUTOINCREMENT,
                        ELD INTEGER,
                        GID TEXT,
                        cgzApp INTEGER,
                        dsiApp TEXT,
                        Pfx TEXT
                    )");

                int totalInserted = 0;

                foreach (string pclFile in pclFiles)
                {
                    try
                    {
                        var lines = File.ReadAllLines(pclFile);
                        bool isHeader = true;

                        foreach (string line in lines)
                        {
                            if (string.IsNullOrWhiteSpace(line)) continue;
                            if (isHeader && line.StartsWith("CHG"))
                            {
                                isHeader = false;
                                continue;
                            }
                            isHeader = false;

                            var parts = line.Split(',');
                            if (parts.Length < 11) continue;

                            try
                            {
                                string chg = parts.Length > 0 ? parts[0].Trim() : "";
                                string guid = parts.Length > 1 ? parts[1].Trim() : Guid.NewGuid().ToString();
                                string appName = parts.Length > 2 ? parts[2].Trim() : "";
                                string user = parts.Length > 6 ? parts[6].Trim() : "0";
                                string pclTyp = parts.Length > 8 ? parts[8].Trim() : "0";
                                string prc = parts.Length > 10 ? parts[10].Trim() : "";
                                string fil = parts.Length > 11 ? parts[11].Trim() : "";

                                // ✅ ПРЕОБРАЗУЕМ В ЧИСЛА
                                int userInt = 0;
                                int.TryParse(user, out userInt);

                                int pclTypInt = 0;
                                int.TryParse(pclTyp, out pclTypInt);

                                int filInt = 0;
                                if (!string.IsNullOrEmpty(fil))
                                    int.TryParse(fil, out filInt);

                                string safeAppName = appName.Replace("'", "''");
                                string safePrc = prc.Replace("'", "''");

                                // Вставляем приложение
                                int appClu = -1;
                                if (!string.IsNullOrEmpty(safeAppName))
                                {
                                    string checkApp = $"SELECT CLU FROM App WHERE dsiApp = '{safeAppName}'";
                                    var result = dataSource.__mSqlValue(checkApp);
                                    if (result != null && result != DBNull.Value)
                                    {
                                        appClu = Convert.ToInt32(result);
                                    }
                                    else
                                    {
                                        string newGuid = Guid.NewGuid().ToString();
                                        string chgNow = DateTime.Now.Ticks.ToString();
                                        string insertApp = $"INSERT INTO App (CHG, GID, ELD, cgzApp, dsiApp, Pfx) VALUES ('{chgNow}', '{newGuid}', 0, 0, '{safeAppName}', '')";
                                        dataSource.__mSqlCommand(insertApp);
                                        result = dataSource.__mSqlValue(checkApp);
                                        if (result != null && result != DBNull.Value)
                                            appClu = Convert.ToInt32(result);
                                    }
                                }

                                // Вставляем протокол
                                if (appClu > -1)
                                {
                                    string newGuid = Guid.NewGuid().ToString();
                                    string insertPcl = $@"
                                        INSERT INTO Pcl (CHG, GID, ELD, InkApp, InkPclTyp, InkUsr, Prc, Fil) 
                                        VALUES ('{chg}', '{newGuid}', 0, {appClu}, {pclTypInt}, {userInt}, '{safePrc}', {filInt})";
                                    dataSource.__mSqlCommand(insertPcl);
                                    totalInserted++;
                                }
                            }
                            catch { }
                        }
                    }
                    catch { }
                }

                MessageBox.Show($"Загружено записей: {totalInserted}", "Готово");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка");
            }
        }

        #endregion

        #region - События

        private void mAreaProtocols_GridCellEnter(object sender, EventArgs e)
        {
            if (_cAreaProtocols.__fGrid_.CurrentRow != null && _currentDataSource != null)
            {
                try
                {
                    var rowView = _cAreaProtocols.__fGrid_.CurrentRow.DataBoundItem as DataRowView;
                    if (rowView != null)
                    {
                        string protocolClu = rowView["CLU"].ToString();

                        // ✅ ПРАВИЛЬНО: InkPclRrdTyp
                        string vQueryPclRrd = $@"
                    SELECT CLU, CHG, Err, InkPclRrdTyp
                    FROM PclRrd
                    WHERE InkPcl = '{protocolClu}'
                    ORDER BY CHG DESC";

                        DataTable vDataTablePclRrd = _currentDataSource.__mSqlQuery(vQueryPclRrd);

                        if (vDataTablePclRrd != null && vDataTablePclRrd.Rows.Count > 0)
                        {
                            _cAreaProtocolsRecords.__fDataSource_ = vDataTablePclRrd;
                            _cAreaProtocolsRecords.__mGridRefresh();
                        }
                        else
                        {
                            _cAreaProtocolsRecords.__fDataSource_ = null;
                            _cAreaProtocolsRecords.__mGridRefresh();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
                }
            }
        }

        #endregion

        #endregion
    }
}