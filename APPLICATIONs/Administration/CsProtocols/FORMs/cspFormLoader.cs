using nlCsProtocols;
using nlDataSourceSqlite;
using nlElements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace naCsProtocols
{
    /// <summary>
    /// Файл cspFormLoad.cs
    /// </summary>
    /// <remarks>Form 1  - отдельная форма загрузки протоколов: сканирует папки 'PROTOCOLs'
    /// всех приложений решения и импортирует
    /// найденные '.pcl' файлы в базу данных ('DATABASEs\protocols.db') через 'ProtocolSqliteImporter' </remarks>

    public class cspFormLoader : elmForm
    {
        #region = МЕТОДЫ

        #region - Объект

        protected override void _mObjectAssembly()
        {
            base._mObjectAssembly();

            #region /// Размещение компонентов

            Controls.Add(_cPanelMain);
            _cPanelMain.Controls.Add(_cLabelInfo, 0, 0);
            _cPanelMain.Controls.Add(_cButtonLoad, 0, 1);
            //_cPanelMain.Controls.Add(_cButtonCleanup, 0, 2);
            _cPanelMain.Controls.Add(_cListLog, 0, 3);
            _cPanelMain.Controls.Add(_cButtonContinue, 0, 4);

            #endregion Размещение компонентов

            #region /// Настройки компонентов

            __fCaption_ = "Загрузка протоколов";
            ShowInTaskbar = true;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(960, 360);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            _cPanelMain.Dock = DockStyle.Fill;
            _cPanelMain.Padding = new Padding(16);
            _cPanelMain.ColumnCount = 1;
            _cPanelMain.RowCount = 5;
            _cPanelMain.ColumnStyles.Clear();
            _cPanelMain.RowStyles.Clear();
            _cPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _cPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));  // подпись
            _cPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));  // кнопка загрузки
            _cPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));  // кнопка очистки мусора
            _cPanelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // журнал - остальное место
            _cPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));  // кнопка продолжить

            _cLabelInfo.Dock = DockStyle.Fill;
            _cLabelInfo.Text = "Сканирование папок 'PROTOCOLs' всех приложений решения и импорт файлов '.pcl' в базу данных протоколов.";
            _cLabelInfo.Font = new Font("Segoe UI", 9);

            _cButtonLoad.Dock = DockStyle.Fill;
            _cButtonLoad.Text = "Загрузить протоколы";
            _cButtonLoad.Margin = new Padding(0, 4, 0, 8);
            _cButtonLoad.Click += mButtonLoad_Click;

            //_cButtonCleanup.Dock = DockStyle.Fill;
            //_cButtonCleanup.Text = "Удалить мусор от старых нераспознанных импортов";
            //_cButtonCleanup.Margin = new Padding(0, 0, 0, 8);
            //_cButtonCleanup.Click += mButtonCleanup_Click;

            _cListLog.Dock = DockStyle.Fill;
            _cListLog.Font = new Font("Consolas", 9);
            _cListLog.IntegralHeight = false;
            _cListLog.Margin = new Padding(0, 0, 0, 8);

            _cButtonContinue.Dock = DockStyle.Fill;
            _cButtonContinue.Text = "Продолжить \u2192";
            _cButtonContinue.Click += mButtonContinue_Click;

            __cPanelStatus.__fCaption_ = "Готово к загрузке";
            __cPanelStatus.__fPercent_ = 0;
            __cPanelStatus.BringToFront(); 

            #endregion Настройки компонентов

            ResumeLayout();

            Type vType = this.GetType();
            Name = vType.Name;

            return;
        }

        #endregion Объект

        #region - Процедуры

        /// <summary>
        /// Загрузка протоколов по нажатию кнопки - сканирование папок всех приложений и импорт '.pcl' файлов
        /// </summary>
        //private void mButtonCleanup_Click(object sender, EventArgs e)
        //{
        //    dsqProtocols vProtocols = dsqProtocols.__oActive_;
        //    int vDeleted = vProtocols.__mPurgeUnrecognizedImports();
        //    _cListLog.Items.Add("Удалено мусорных протоколов (старые нераспознанные импорты): " + vDeleted.ToString());
        //}
        private void mButtonLoad_Click(object sender, EventArgs e)
        {
            _cButtonLoad.Enabled = false;
            _cListLog.Items.Clear();

            dsqProtocols vProtocols = dsqProtocols.__oActive_;

            int vPurged = vProtocols.__mPurgeUnrecognizedImports();
            if (vPurged > 0)
                _cListLog.Items.Add("Удалено мусорных протоколов от старых нераспознанных импортов: " + vPurged.ToString());

            string vStartDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _cListLog.Items.Add("Папка запуска (AppDomain.BaseDirectory): " + vStartDirectory);
            _cListLog.Items.Add("База данных: " + System.IO.Path.Combine(vProtocols.__fDatabasePath, "protocols.db"));
            _cListLog.Items.Add("---");

      
            List<string> vFolders = ProtocolSqliteImporter.__mProtocolsFoldersDiscover(vStartDirectory);

          
            try
            {
                string vOwn = nlApplication.appApplication.__oPathes.__fDirectoryProtocols_;
                if (string.IsNullOrEmpty(vOwn) == false && Directory.Exists(vOwn) == true && vFolders.Contains(vOwn) == false)
                    vFolders.Add(vOwn);
            }
            catch { }

            if (vFolders.Count == 0)
            {
                _cListLog.Items.Add("Папки 'PROTOCOLs' не найдены - импортировать нечего.");
                __cPanelStatus.__fCaption_ = "Папки не найдены";
                __cPanelStatus.__fPercent_ = 100;
                _cButtonLoad.Enabled = true;
                return;
            }

            _cListLog.Items.Add("Найдено папок для проверки: " + vFolders.Count.ToString());
            foreach (string vFoundFolder in vFolders)
                _cListLog.Items.Add("  - " + vFoundFolder);
            _cListLog.Items.Add("---");

            ProtocolSqliteImporter vImporter = new ProtocolSqliteImporter();
            int vImportedTotal = 0;

            for (int i = 0; i < vFolders.Count; i++)
            {
                string vFolder = vFolders[i];
                __cPanelStatus.__fCaption_ = "Импорт: " + vFolder;
                __cPanelStatus.__fPercent_ = (int)((i / (double)vFolders.Count) * 100);
                Application.DoEvents(); 

                _cListLog.Items.Add(vFolder + ":");

                int vImportedFromFolder = 0;
                try
                {
                   
                    vImportedFromFolder = vImporter.__mImportFromFolder(vProtocols, vFolder, pLine => _cListLog.Items.Add("  " + pLine));
                }
                catch (Exception vException)
                {
                    _cListLog.Items.Add("  ошибка: " + vException.Message);
                    continue;
                }

                vImportedTotal += vImportedFromFolder;
                _cListLog.Items.Add("  итого из этой папки: " + vImportedFromFolder.ToString());
            }

            __cPanelStatus.__fPercent_ = 100;
            __cPanelStatus.__fCaption_ = "Готово - импортировано строк: " + vImportedTotal.ToString();
            _cListLog.Items.Add("---");
            _cListLog.Items.Add("Всего импортировано новых строк: " + vImportedTotal.ToString() + " (из папок: " + vFolders.Count.ToString() + ")");

            _cButtonLoad.Enabled = true;
        }
        /// <summary>
        /// Переход к главному просмотрщику протоколов (закрывает форму загрузки)
        /// </summary>
        private void mButtonContinue_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        private TableLayoutPanel _cPanelMain = new TableLayoutPanel();
        private Label _cLabelInfo = new Label();
        private Button _cButtonLoad = new Button();
        private Button _cButtonCleanup = new Button();
        private Button _cButtonContinue = new Button();
        private ListBox _cListLog = new ListBox();

        #endregion Компоненты

        #endregion ПОЛЯ
    }
}