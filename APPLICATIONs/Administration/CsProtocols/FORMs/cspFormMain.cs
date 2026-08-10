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
using System.Data.SQLite;


namespace naCsProtocols
{
    public class cspFormMain : elmForm
    {
        #region = ПОЛЯ

        // Левая таблица - Список протоколов (Таблица Pcl)
        private cspAreaGrid _cAreaProtocols = new cspAreaGrid();
        private DataTable _oDataTableProtocols = new DataTable();

        // Правая таблица - Записи протокола (Таблица PclRrd)
        private cspAreaGrid _cAreaProtocolsRecords = new cspAreaGrid();
        private DataTable _oDataTableProtocolsRecord = new DataTable();

        private Label lblStatus;
        private datUnitDataSource _currentDataSource = null;
        private string _currentFilePath = string.Empty;

        #endregion

        #region = МЕТОДЫ

        #region - Конструктор

        public cspFormMain()
        {
            _mObjectAssembly();
            _mObjectPresentation();
        }

        #endregion

        public void BindDataToGrid()
        {
            if (string.IsNullOrEmpty(_currentFilePath) || _currentDataSource == null) return;

            try
            {
                string vQueryPcl = "SELECT * FROM Pcl";
                DataTable vDataTablePcl = _currentDataSource.__mSqlQuery(vQueryPcl);

                if (vDataTablePcl != null)
                {
                    foreach (DataRow vDataRow in vDataTablePcl.Rows)
                    {
                        if (vDataRow["CHG"] != DBNull.Value && long.TryParse(vDataRow["CHG"].ToString(), out long ticks))
                        {
                            vDataRow["CHG"] = new DateTime(ticks).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }

                    _cAreaProtocols.__fDataSource_ = vDataTablePcl;
                    _cAreaProtocols.__mGridRefresh();
                }

                string vQueryPclRrd = "SELECT * FROM PclRrd";
                DataTable vDataTablePclRrd = _currentDataSource.__mSqlQuery(vQueryPclRrd);

                if (vDataTablePclRrd != null)
                {
                    foreach (DataRow vDataRow in vDataTablePclRrd.Rows)
                    {
                        if (vDataRow["CHG"] != DBNull.Value && long.TryParse(vDataRow["CHG"].ToString(), out long ticks))
                        {
                            vDataRow["CHG"] = new DateTime(ticks).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }

                    _cAreaProtocolsRecords.__fDataSource_ = vDataTablePclRrd;
                    _cAreaProtocolsRecords.__mGridRefresh();
                }

                if (lblStatus != null)
                {
                    lblStatus.Text = "Данные успешно обновлены из БД";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка привязки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadDataAndShow()
        {
            string dbPath = @"C:\Users\doy\Documents\GitHub\Administration\DATABASES\Protocols.db";

            _currentFilePath = dbPath;
            _currentDataSource = new dsqDataSourceSqlite();
            _currentDataSource.__fDatabasePath = Path.GetDirectoryName(dbPath);
            _currentDataSource.__fDatabaseName = Path.GetFileName(dbPath);

            string vQueryPcl = "SELECT * FROM Pcl";
            DataTable vDataTablePcl = _currentDataSource.__mSqlQuery(vQueryPcl);

            if (vDataTablePcl != null)
            {
                foreach (DataRow vDataRow in vDataTablePcl.Rows)
                {
                    if (vDataRow["CHG"] != DBNull.Value && long.TryParse(vDataRow["CHG"].ToString(), out long ticks))
                    {
                        vDataRow["CHG"] = new DateTime(ticks).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }

                _cAreaProtocols.__fDataSource_ = vDataTablePcl;
                _cAreaProtocols.__mGridRefresh();
            }
        }

        #region - Объект

        protected override void _mObjectAssembly()
        {
            base._mObjectAssembly();

            var mainBlock = new elmBlockFormMain();
            Controls.Add(mainBlock);

            var splitter = new elmComponentSplitter();
            mainBlock.Controls.Add(splitter);

            // ============================================================
            // ЛЕВАЯ ПАНЕЛЬ: Список протоколов (Pcl)
            // ============================================================
            var leftPanel = new Panel { Dock = DockStyle.Fill };
            splitter.Panel1.Controls.Add(leftPanel);

            var leftHeader = new Label
            {
                Text = "Список протоколов",
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

            _oDataTableProtocols.Columns.Clear();
            _oDataTableProtocols.Columns.Add("CLU", typeof(string));
            _oDataTableProtocols.Columns.Add("CHG", typeof(string));
            _oDataTableProtocols.Columns.Add("lnkApp", typeof(string));
            _oDataTableProtocols.Columns.Add("lnkPclTyp", typeof(string));
            _oDataTableProtocols.Columns.Add("lnkCpu", typeof(string));
            _oDataTableProtocols.Columns.Add("Prc", typeof(string));
            _oDataTableProtocols.Columns.Add("lnkUsr", typeof(string));
            _oDataTableProtocols.Columns.Add("Fil", typeof(string));

            _cAreaProtocols.__fDataSource_ = _oDataTableProtocols;

            if (_cAreaProtocols.__fGrid_.Columns.Count == 0)
            {
                _cAreaProtocols.__mColumnAdd("Протокол", "CLU", "CLU", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Время", "CHG", "CHG", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Приложение", "lnkApp", "lnkApp", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Вид", "lnkPclTyp", "lnkPclTyp", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Хост", "lnkCpu", "lnkCpu", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Процедура", "Prc", "Prc", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mColumnAdd("Пользователь", "lnkUsr", "lnkUsr", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocols.__mGridBuild();
            }

            _cAreaProtocols.__eGridCellEnter += mAreaProtocols_GridCellEnter;
            _cAreaProtocols.__fGrid_.CellEndEdit += mAreaProtocols_CellEndEdit;

            // ============================================================
            // ПРАВАЯ ПАНЕЛЬ: Записи протокола (PclRrd)
            // ============================================================
            var rightPanel = new Panel { Dock = DockStyle.Fill };
            splitter.Panel2.Controls.Add(rightPanel);

            var rightHeader = new Label
            {
                Text = "Записи протокола (PclRrd)",
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

            _oDataTableProtocolsRecord.Columns.Clear();
            _oDataTableProtocolsRecord.Columns.Add("CLU", typeof(string));
            _oDataTableProtocolsRecord.Columns.Add("CHG", typeof(string));
            _oDataTableProtocolsRecord.Columns.Add("lnkPcl", typeof(string));
            _oDataTableProtocolsRecord.Columns.Add("lnkPclRrdTyp", typeof(string));
            _oDataTableProtocolsRecord.Columns.Add("Err", typeof(string));
            _oDataTableProtocolsRecord.Columns.Add("Tck", typeof(string));

            _cAreaProtocolsRecords.__fDataSource_ = _oDataTableProtocolsRecord;

            if (_cAreaProtocolsRecords.__fGrid_.Columns.Count == 0)
            {
                _cAreaProtocolsRecords.__mColumnAdd("Протокол", "lnkPcl", "lnkPcl", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mColumnAdd("Ключ", "CLU", "CLU", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mColumnAdd("Вид", "lnkPclRrdTyp", "lnkPclRrdTyp", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mColumnAdd("Сообщение", "Err", "Err", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mColumnAdd("Время", "CHG", "CHG", true, true, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
                _cAreaProtocolsRecords.__mGridBuild();
            }

            _cAreaProtocolsRecords.__fGrid_.CellEndEdit += mAreaProtocolsRecords_CellEndEdit;

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

        #region - События и Автосохранение

        private void mAreaProtocols_GridCellEnter(object sender, EventArgs e)
        {
            if (_cAreaProtocols.__fGrid_.CurrentRow != null && _currentDataSource != null)
            {
                var rowView = _cAreaProtocols.__fGrid_.CurrentRow.DataBoundItem as DataRowView;
                if (rowView != null)
                {
                    string protocolClu = rowView["CLU"].ToString();
                    string vQueryPclRrd = $"SELECT * FROM PclRrd WHERE lnkPcl = '{protocolClu}'";
                    DataTable vDataTablePclRrd = _currentDataSource.__mSqlQuery(vQueryPclRrd);

                    foreach (DataRow vDataRow in vDataTablePclRrd.Rows)
                    {
                        if (vDataRow["CHG"] != DBNull.Value && long.TryParse(vDataRow["CHG"].ToString(), out long ticks))
                        {
                            vDataRow["CHG"] = new DateTime(ticks).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }

                    _cAreaProtocolsRecords.__fDataSource_ = vDataTablePclRrd;
                    _cAreaProtocolsRecords.__mGridRefresh();
                }
            }
        }

        private void mAreaProtocols_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SaveRowToDatabase(e.RowIndex, _cAreaProtocols.__fGrid_, "Pcl");
        }

        private void mAreaProtocolsRecords_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SaveRowToDatabase(e.RowIndex, _cAreaProtocolsRecords.__fGrid_, "PclRrd");
        }

        private void SaveRowToDatabase(int rowIndex, DataGridView grid, string tableName)
        {
            if (string.IsNullOrEmpty(_currentFilePath)) return;

            var row = grid.Rows[rowIndex];
            if (row.DataBoundItem is DataRowView rowView)
            {
                DataRow dataRow = rowView.Row;
                string clu = dataRow["CLU"]?.ToString();

                if (string.IsNullOrEmpty(clu)) return;

                using (var connection = new SQLiteConnection($"Data Source={_currentFilePath};Version=3;"))
                {
                    connection.Open();

                    string checkQuery = $"SELECT COUNT(1) FROM {tableName} WHERE CLU = @CLU";
                    using (var checkCmd = new SQLiteCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@CLU", clu);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            string updateCols = "";
                            var cmd = new SQLiteCommand(connection);
                            int paramIndex = 0;

                            foreach (DataColumn col in dataRow.Table.Columns)
                            {
                                if (col.ColumnName == "CLU") continue;
                                if (updateCols.Length > 0) updateCols += ", ";
                                updateCols += $"{col.ColumnName} = @p{paramIndex}";
                                cmd.Parameters.AddWithValue($"@p{paramIndex}", dataRow[col] ?? DBNull.Value);
                                paramIndex++;
                            }

                            cmd.CommandText = $"UPDATE {tableName} SET {updateCols} WHERE CLU = @CLU";
                            cmd.Parameters.AddWithValue("@CLU", clu);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            string cols = "";
                            string paramsNames = "";
                            var cmd = new SQLiteCommand(connection);
                            int paramIndex = 0;

                            foreach (DataColumn col in dataRow.Table.Columns)
                            {
                                if (cols.Length > 0)
                                {
                                    cols += ", ";
                                    paramsNames += ", ";
                                }
                                cols += col.ColumnName;
                                paramsNames += $"@p{paramIndex}";
                                cmd.Parameters.AddWithValue($"@p{paramIndex}", dataRow[col] ?? DBNull.Value);
                                paramIndex++;
                            }

                            cmd.CommandText = $"INSERT INTO {tableName} ({cols}) VALUES ({paramsNames})";
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                lblStatus.Text = $"Изменения в таблице {tableName} сохранены в БД";
            }
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
                _currentFilePath = vOpenFileDialog.FileName;
                datUnitDataSource vDataSource = new dsqDataSourceSqlite();
                vDataSource.__fDatabasePath = Path.GetDirectoryName(_currentFilePath);
                vDataSource.__fDatabaseName = Path.GetFileName(_currentFilePath);

                _currentDataSource = vDataSource;

                string vQueryPcl = "SELECT * FROM Pcl";
                DataTable vDataTablePcl = vDataSource.__mSqlQuery(vQueryPcl);

                foreach (DataRow vDataRow in vDataTablePcl.Rows)
                {
                    if (vDataRow["CHG"] != DBNull.Value && long.TryParse(vDataRow["CHG"].ToString(), out long ticks))
                    {
                        vDataRow["CHG"] = new DateTime(ticks).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }

                _cAreaProtocols.__fDataSource_ = vDataTablePcl;
                _cAreaProtocols.__mGridRefresh();

                string vQueryPclRrd = "SELECT * FROM PclRrd";
                DataTable vDataTablePclRrd = vDataSource.__mSqlQuery(vQueryPclRrd);

                foreach (DataRow vDataRow in vDataTablePclRrd.Rows)
                {
                    if (vDataRow["CHG"] != DBNull.Value && long.TryParse(vDataRow["CHG"].ToString(), out long ticks))
                    {
                        vDataRow["CHG"] = new DateTime(ticks).ToString("yyyy-MM-dd HH:mm:ss");
                    }
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