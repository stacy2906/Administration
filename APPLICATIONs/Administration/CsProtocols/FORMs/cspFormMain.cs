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
        private List<ProtocolRecord> _allRecords = new List<ProtocolRecord>();
        private ProtocolLoader _loader = new ProtocolLoader();
        private string _currentProtocolGuid = "";

        private Label lblStatus;

        #endregion

        #region = МЕТОДЫ

        #region - Конструктор

        public cspFormMain()
        {
            _mObjectAssembly();
            _mObjectPresentation();
            // ← НИЧЕГО НЕ ЗАГРУЖАЕМ
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

        private void mAreaProtocols_GridCellEnter(object sender, EventArgs e)
        {
            // Таблица пустая, ничего не делаем
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
                datUnitDataSource vDataSource = new dsqDataSourceSqlite();
                vDataSource.__fDatabasePath = Path.GetDirectoryName(vFilePath);
                vDataSource.__fDatabaseName = Path.GetFileName(vFilePath);

                string vQueryPcl = "SELECT P.*, A.desApp, PT.desPclTyp "
                    + "FROM Pcl P "
                    + "LEFT JOIN App A ON A.CLU = P.lnkApp "
                    + "LEFT JOIN PclTyp PT ON PT.CLU = P.lnkPclTyp";

                DataTable vDataTablePcl = vDataSource.__mSqlQuery(vQueryPcl);
                _cAreaProtocols.__fDataSource_ = vDataTablePcl;
                foreach (DataRow vDataRow in vDataTablePcl.Rows)
                {
                    vDataRow["CHG"] = new DateTime(Convert.ToInt64(vDataRow["CHG"])).ToString();
                }

                string vQueryPclRrd = "SELECT PR.*, RT.desRrdTyp "
                    + "FROM PclRrd PR "
                    + "LEFT JOIN RrdTyp RT ON RT.CLU = PR.lnkRrdTyp";

                DataTable vDataTablePclRrd = vDataSource.__mSqlQuery(vQueryPclRrd);
                _cAreaProtocolsRecords.__fDataSource_ = vDataTablePclRrd;
                _cAreaProtocolsRecords.__mGridRefresh();

                lblStatus.Text = $"Загружено из БД: {vDataTablePcl.Rows.Count} протоколов";
            }
        }

        #endregion

        #endregion
    }
}