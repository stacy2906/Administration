using CsProtocols;
using CsProtocols.DATA.Loaders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace naCsProtocols.FORMs
{
    public partial class elmFormProtocolLoad : Form
    {
        private Label lblTitle;
        private Label lblStatus;
        private Label lblFile;
        private Label lblCount;
        private ProgressBar progressBar;
        private Button btnCancel;
        private Label lblHint;

        private bool _isCancelled = false;
        private string _dbPath;
        private int _totalFiles = 0;
        private int _processedFiles = 0;

        public elmFormProtocolLoad(string dbPath)
        {
            _dbPath = dbPath;
            InitializeComponent();
            SetupUI();
            LoadProtocolsAsync();
        }

        private void SetupUI()
        {
            this.Text = "Загрузка протоколов";
            this.Size = new Size(500, 270);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ControlBox = false;
            this.BackColor = Color.White;
            this.MinimumSize = new Size(450, 220);

            // Заголовок
            lblTitle = new Label
            {
                Text = "⏳ ЗАГРУЗКА ПРОТОКОЛОВ",
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(0)
            };
            this.Controls.Add(lblTitle);

            // Панель для статуса
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 10, 20, 10)
            };
            this.Controls.Add(panel);

            // Текущий файл
            lblFile = new Label
            {
                Text = "📂 Подготовка...",
                Location = new Point(0, 5),
                Size = new Size(460, 30),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(50, 50, 50),
                TextAlign = ContentAlignment.MiddleLeft
            };
            panel.Controls.Add(lblFile);

            // Статус
            lblStatus = new Label
            {
                Text = "🔍 Поиск папок PROTOCOLS...",
                Location = new Point(0, 38),
                Size = new Size(460, 25),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(80, 80, 80),
                TextAlign = ContentAlignment.MiddleLeft
            };
            panel.Controls.Add(lblStatus);

            // Количество обработанных
            lblCount = new Label
            {
                Text = "Файлов: 0",
                Location = new Point(0, 65),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(80, 80, 80),
                TextAlign = ContentAlignment.MiddleLeft
            };
            panel.Controls.Add(lblCount);

            // Прогресс-бар
            progressBar = new ProgressBar
            {
                Location = new Point(0, 98),
                Size = new Size(460, 20),
                Style = ProgressBarStyle.Continuous,
                Minimum = 0,
                Maximum = 100,
                Value = 0
            };
            panel.Controls.Add(progressBar);

            // Кнопка "Отмена"
            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(385, 135),
                Size = new Size(75, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 240, 240),
                Cursor = Cursors.Hand
            };
            btnCancel.Click += (s, e) => { _isCancelled = true; btnCancel.Enabled = false; btnCancel.Text = "Отмена..."; };
            panel.Controls.Add(btnCancel);

            // Подсказка
            lblHint = new Label
            {
                Text = "⏱️ Процесс может занять несколько минут",
                Location = new Point(0, 140),
                Size = new Size(380, 20),
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.Gray
            };
            panel.Controls.Add(lblHint);
        }

        private async void LoadProtocolsAsync()
        {
            try
            {
                UpdateStatus("🔍 Поиск папок PROTOCOLS...", 0);

                var loader = new ProtocolsDbLoader(_dbPath);

                // Находим папки
                var folders = FindAllProtocolFolders();

                if (folders.Count == 0)
                {
                    MessageBox.Show("Папки PROTOCOLS не найдены!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                UpdateStatus($"📁 Найдено папок: {folders.Count}", 5);

                int totalPcl = 0;
                int totalRrd = 0;
                int processed = 0;

                // Считаем общее количество файлов
                foreach (string folder in folders)
                {
                    var files = Directory.GetFiles(folder, "*.pcl")
                        .Where(f => !Path.GetFileNameWithoutExtension(f).EndsWith("rr"))
                        .ToList();
                    _totalFiles += files.Count;
                }

                if (_totalFiles == 0)
                {
                    MessageBox.Show("Файлы протоколов (.pcl) не найдены!", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                UpdateStatus($"📄 Найдено файлов: {_totalFiles}", 10);

                // Обрабатываем каждую папку
                foreach (string folder in folders)
                {
                    if (_isCancelled)
                    {
                        UpdateStatus("⛔ Загрузка отменена", 0);
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                        return;
                    }

                    var files = Directory.GetFiles(folder, "*.pcl")
                        .Where(f => !Path.GetFileNameWithoutExtension(f).EndsWith("rr"))
                        .ToList();

                    foreach (var file in files)
                    {
                        if (_isCancelled)
                        {
                            UpdateStatus("⛔ Загрузка отменена", 0);
                            this.DialogResult = DialogResult.Cancel;
                            this.Close();
                            return;
                        }

                        string fileName = Path.GetFileName(file);
                        UpdateFile($"📄 Обработка: {fileName}", processed, _totalFiles);

                        // Загружаем файл через ProtocolLoader
                        var protocolLoader = new ProtocolLoader();
                        var records = protocolLoader.LoadSingleFile(file);

                        if (records.Count > 0)
                        {
                            totalPcl++;
                            processed++;
                            UpdateCount(processed, _totalFiles);
                            UpdateProgress(10 + (processed * 80 / _totalFiles));
                        }

                        // Загружаем .rrd
                        string rrdFile = Path.ChangeExtension(file, null) + "rrd.pcl";
                        if (File.Exists(rrdFile))
                        {
                            totalRrd++;
                        }
                    }
                }

                // Завершение
                UpdateStatus($"✅ Готово! Pcl: {totalPcl}, PclRrd: {totalRrd}", 100);
                UpdateFile("✅ Загрузка завершена", _totalFiles, _totalFiles);
                UpdateCount(_totalFiles, _totalFiles);

                await Task.Delay(500);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private List<string> FindAllProtocolFolders()
        {
            var result = new List<string>();

            try
            {
                var found = Directory.GetDirectories(@"C:\", "PROTOCOLS", SearchOption.AllDirectories);
                result.AddRange(found);
            }
            catch { }

            try
            {
                var found = Directory.GetDirectories(@"U:\", "PROTOCOLS", SearchOption.AllDirectories);
                result.AddRange(found);
            }
            catch { }

            string[] knownPaths = {
                @"C:\KviNA\APPLICATIONS\Administration\Administration\bin\Debug\PROTOCOLS",
                @"C:\KviNA\ADDITIVE\CsProtocols\CsProtocols\bin\Debug\PROTOCOLS",
                @"C:\KviNA\ADDITIVE\csManual\csManual\bin\Debug\PROTOCOLS"
            };

            foreach (string path in knownPaths)
            {
                if (Directory.Exists(path) && !result.Contains(path))
                    result.Add(path);
            }

            return result.Distinct().ToList();
        }

        private void UpdateStatus(string text, int progress)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateStatus(text, progress)));
                return;
            }

            lblStatus.Text = text;
            if (progress >= 0)
                progressBar.Value = Math.Min(progress, 100);
        }

        private void UpdateFile(string text, int current, int total)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateFile(text, current, total)));
                return;
            }

            lblFile.Text = text;
        }

        private void UpdateCount(int current, int total)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateCount(current, total)));
                return;
            }

            lblCount.Text = $"Файлов: {current} из {total}";
        }

        private void UpdateProgress(int value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateProgress(value)));
                return;
            }

            progressBar.Value = Math.Min(value, 100);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_isCancelled)
            {
                MessageBox.Show("Загрузка отменена", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            base.OnFormClosing(e);
        }
    }
}