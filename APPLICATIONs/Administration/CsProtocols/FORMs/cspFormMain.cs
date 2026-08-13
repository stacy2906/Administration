using nlApplication;
using nlCsProtocols;
using nlData;
using nlDataSourceSqlite;
using nlElements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace naCsProtocols
{
    /// <summary>
    /// Файл cspFormMain.cs
    /// </summary>
    /// <remarks>Класс главной формы приложения 'CsProtocols'</remarks>
    /// <fixed>
    /// Восстановлено относительно присланной версии - без этого форма НЕ показывала протоколы вообще:
    /// 1) Не было автозагрузки: конструктор/презентация нигде не вызывали чтение из 'cspApplication.__oProtocols' -
    ///    таблица слева заполнялась ТОЛЬКО через ручное меню 'Файл / Открыть протокол'. Возвращён вызов
    ///    'mProtocolsAutoLoad()' и сам метод.
    /// 2) Имена таблиц/столбцов в SQL не совпадали с реальной схемой, которую создаёт 'dsqProtocols.__mTablesFill'
    ///    ('desApp' -> 'dsiApp', 'desPclTyp' -> 'dsiPclTyp', 'desRrdTyp'/'RrdTyp'/'lnkRrdTyp' -> 'dsiPclRrdTyp'/
    ///    'PclRrdTyp'/'lnkPclRrdTyp', хост/пользователь читаются через JOIN на 'Cpu'/'Usr', а не как текстовые
    ///    столбцы 'Hst'/'Usr' прямо в 'Pcl') - при реальном подключении такие запросы упали бы с "no such column".
    /// 3) В 'mMenuFileOpen_Click' обратно стоял 'dsqDataSourceSqlite' - тот самый источник крэша
    ///    NullReferenceException-ом на устаревшей/битой базе (подробности - в 'README_corrections.md' из
    ///    предыдущей правки). Возвращён 'dsqDataSourceSqliteWithProtocol'.
    /// 4) Блок "Чтение приложений" в 'mMenuFileOpen_Click' был нерабочим (перебирал ПУСТУЮ '_oDataTaleApplications'
    ///    вместо только что прочитанной 'vDataTableApp', и результат тут же затирался следующим блоком) - удалён.
    /// Добавлено по заданию: панель фильтров (поиск, вид протокола, приложение, пользователь, хост, период дат).
    /// Сортировка по клику на заголовок столбца уже работает "из коробки" в 'elmComponentGrid' - код не добавлялся.
    /// </fixed>
    public class cspFormMain : elmForm
    {
        #region = МЕТОДЫ

        #region - Объект

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            base._mObjectAssembly();

            #region /// Размещение компонентов

            Controls.Add(_cBlockFormMain);
            _cBlockFormMain.Controls.Add(_cSplitter);
            _cBlockFormMain.Controls.SetChildIndex(_cSplitter, 0);
            _cBlockFormMain.__mMenuAdd(_cMenuFile);

            _cSplitter.Panel1.Controls.Add(_cAreaProtocols);
            _cSplitter.Panel1.Controls.Add(_cPanelFilters);
            _cSplitter.Panel2.Controls.Add(_cAreaProtocolsRecords);

            _cMenuFile.DropDownItems.Add(_cMenuFileOpen);
            _cMenuFile.DropDownItems.Add(_cMenuFileClose);

            Controls.Add(_cLabelStatus);

            #endregion Размещение компонентов

            #region /// Настройки компонентов

            __fCaption_ = cspApplication.__fCaption_;
            ShowInTaskbar = true;

            // _cMenuFile
            {
                _cMenuFile.__fCaption_ = "Файл";
                // _cMenuFileOpen
                {
                    _cMenuFileOpen.__fCaption_ = "Открыть протокол";
                    _cMenuFileOpen.Click += mMenuFileOpen_Click;
                }
                // _cMenuFileClose
                {
                    _cMenuFileClose.__fCaption_ = "Закрыть протокол";
                    _cMenuFileClose.Click += mMenuFileClose_Click;
                }
            }
            // _cAreaProtocols
            {
                _cAreaProtocols.Dock = DockStyle.Fill;
                _cAreaProtocols.__fDataSource_ = _oDataTableProtocols;

                _cAreaProtocols.__fHeaderVisible_ = true;
                _cAreaProtocols.__fHeaderCaption_ = "Протоколы";
                _cAreaProtocols.__eGridCellEnter += mAreaProtocols_GridCellEnter;

                _oDataTableProtocols.Columns.Add("CLU", typeof(string));
                _oDataTableProtocols.Columns.Add("CHG", typeof(string));
                _oDataTableProtocols.Columns.Add("App", typeof(string));
                _oDataTableProtocols.Columns.Add("lnkPclTyp", typeof(string)); // Скрытый - реальный CLU вида протокола, используется только для подсветки строки
                _oDataTableProtocols.Columns.Add("PclTyp", typeof(string));
                _oDataTableProtocols.Columns.Add("Hst", typeof(string));
                _oDataTableProtocols.Columns.Add("Prc", typeof(string));
                _oDataTableProtocols.Columns.Add("Usr", typeof(string));

                _cAreaProtocols.__mColumnAdd("Протокол", "Ключ протокола", "CLU", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mColumnAdd("Время", "Время создания протокола", "CHG", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mColumnAdd("Приложение", "Название приложения", "App", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mColumnAdd("Вид (CLU)", "Реальный CLU вида протокола", "lnkPclTyp", true, false, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mColumnAdd("Вид", "Вид протокола", "PclTyp", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mColumnAdd("Хост", "Рабочая станция, на которой возникло событие", "Hst", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mColumnAdd("Процедура", "Название процедуры, в которой возникло событие", "Prc", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mColumnAdd("Пользователь", "Пользователь приложения, у которого возникло событие", "Usr", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mGridBuild();
            }
            // _cAreaProtocolsRecords
            {
                _cAreaProtocolsRecords.Dock = DockStyle.Fill;
                _cAreaProtocolsRecords.__fDataSource_ = _oDataTableProtocolsRecord;

                _cAreaProtocolsRecords.__fHeaderVisible_ = true;
                _cAreaProtocolsRecords.__fHeaderCaption_ = "Записи в протоколах";

                _oDataTableProtocolsRecord.Columns.Add("Protocol", typeof(string));
                _oDataTableProtocolsRecord.Columns.Add("Key", typeof(string));
                _oDataTableProtocolsRecord.Columns.Add("Type", typeof(string));
                _oDataTableProtocolsRecord.Columns.Add("Message", typeof(string));
                _oDataTableProtocolsRecord.Columns.Add("Time", typeof(string));

                _cAreaProtocolsRecords.__mColumnAdd("Протокол", "Ключ протокола", "Protocol", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocolsRecords.__mColumnAdd("Ключ", "Ключ записи в протоколе", "Key", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocolsRecords.__mColumnAdd("Вид", "Вид записи в протоколе", "Type", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocolsRecords.__mColumnAdd("Сообщение", "Сообщение", "Message", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocolsRecords.__mColumnAdd("Время", "Затраченное время выполнения (тики)", "Time", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocolsRecords.__mGridBuild();
            }
            // _cPanelFilters - панель фильтров над таблицей протоколов (задание: поиск/вид/приложение/пользователь/хост/период)
            {
                _cPanelFilters.Dock = DockStyle.Top;
                _cPanelFilters.Height = 150;
                _cPanelFilters.ColumnCount = 2;
                _cPanelFilters.RowCount = 5;
                _cPanelFilters.Padding = new Padding(4);
                _cPanelFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                _cPanelFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

                _cFilterSearch.Dock = DockStyle.Fill;
                _cPanelFilters.Controls.Add(_cFilterSearch, 0, 0);
                _cPanelFilters.SetColumnSpan(_cFilterSearch, 2);
                _cFilterSearch.TextChanged += mFilter_Changed;

                _cFilterType.Dock = DockStyle.Fill;
                _cFilterType.DropDownStyle = ComboBoxStyle.DropDownList;
                _cPanelFilters.Controls.Add(_cFilterType, 0, 1);
                _cFilterType.SelectedIndexChanged += mFilter_Changed;

                _cFilterApp.Dock = DockStyle.Fill;
                _cFilterApp.DropDownStyle = ComboBoxStyle.DropDownList;
                _cPanelFilters.Controls.Add(_cFilterApp, 1, 1);
                _cFilterApp.SelectedIndexChanged += mFilter_Changed;

                _cFilterUser.Dock = DockStyle.Fill;
                _cFilterUser.DropDownStyle = ComboBoxStyle.DropDownList;
                _cPanelFilters.Controls.Add(_cFilterUser, 0, 2);
                _cFilterUser.SelectedIndexChanged += mFilter_Changed;

                _cFilterHost.Dock = DockStyle.Fill;
                _cFilterHost.DropDownStyle = ComboBoxStyle.DropDownList;
                _cPanelFilters.Controls.Add(_cFilterHost, 1, 2);
                _cFilterHost.SelectedIndexChanged += mFilter_Changed;

                var vDateFromPanel = new Panel { Dock = DockStyle.Fill };
                _cFilterDateFromOn.Text = "С:";
                _cFilterDateFromOn.Dock = DockStyle.Left;
                _cFilterDateFromOn.AutoSize = true;
                _cFilterDateFrom.Dock = DockStyle.Fill;
                _cFilterDateFrom.Format = DateTimePickerFormat.Short;
                _cFilterDateFrom.Enabled = false;
                vDateFromPanel.Controls.Add(_cFilterDateFrom);
                vDateFromPanel.Controls.Add(_cFilterDateFromOn);
                _cPanelFilters.Controls.Add(vDateFromPanel, 0, 3);
                _cFilterDateFromOn.CheckedChanged += mFilterDate_CheckedChanged;
                _cFilterDateFrom.ValueChanged += mFilter_Changed;

                var vDateToPanel = new Panel { Dock = DockStyle.Fill };
                _cFilterDateToOn.Text = "По:";
                _cFilterDateToOn.Dock = DockStyle.Left;
                _cFilterDateToOn.AutoSize = true;
                _cFilterDateTo.Dock = DockStyle.Fill;
                _cFilterDateTo.Format = DateTimePickerFormat.Short;
                _cFilterDateTo.Enabled = false;
                vDateToPanel.Controls.Add(_cFilterDateTo);
                vDateToPanel.Controls.Add(_cFilterDateToOn);
                _cPanelFilters.Controls.Add(vDateToPanel, 1, 3);
                _cFilterDateToOn.CheckedChanged += mFilterDate_CheckedChanged;
                _cFilterDateTo.ValueChanged += mFilter_Changed;

                _cFilterClear.Text = "Сбросить фильтры";
                _cFilterClear.Dock = DockStyle.Fill;
                _cPanelFilters.Controls.Add(_cFilterClear, 0, 4);
                _cPanelFilters.SetColumnSpan(_cFilterClear, 2);
                _cFilterClear.Click += mFilterClear_Click;
            }
            // _cLabelStatus
            {
                _cLabelStatus.Dock = DockStyle.Bottom;
                _cLabelStatus.Height = 25;
                _cLabelStatus.BackColor = SystemColors.Info;
                _cLabelStatus.ForeColor = Color.FromArgb(80, 80, 80);
                _cLabelStatus.Font = new Font("Segoe UI", 9);
                _cLabelStatus.Padding = new Padding(10, 0, 0, 0);
                _cLabelStatus.TextAlign = ContentAlignment.MiddleLeft;
                _cLabelStatus.Text = "Загрузка...";
            }

            #endregion Настройки компонентов

            ResumeLayout();

            Type vType = this.GetType();
            Name = vType.Name;
            _fClassNameFull = vType.FullName + ".";

            return;
        }

        /// <summary>
        /// Презентация объекта
        /// </summary>
        /// <remarks>
        /// ИСПРАВЛЕНО: раньше здесь (и в отдельном явном конструкторе 'cspFormMain()') повторно вызывался
        /// '_mObjectAssembly()' - но базовый 'elmForm()' УЖЕ вызывает его сам (см. 'elmForm.cs', строка 25),
        /// а виртуальный вызов из БАЗОВОГО конструктора уходит именно в этот, переопределённый метод. Второй
        /// явный вызов приводил к повторному "_oDataTableProtocols.Columns.Add("CLU", ...)" -
        /// 'System.Data.DuplicateNameException: Столбец с именем "CLU" уже принадлежит этому DataTable'.
        /// Явный конструктор удалён целиком; загрузка протоколов перенесена сюда, поскольку '_mObjectPresentation()'
        /// и так вызывается ОДИН раз - из 'elmForm.OnCreateControl()', уже после того как все контролы собраны.
        /// </remarks>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();

            mProtocolsAutoLoad();
        }

        /// <summary>
        /// Подсветка строки в левой таблице по реальному 'CLU' вида протокола (столбец 'lnkPclTyp', скрытый) -
        /// ошибки оттенками красного, события оттенками зелёного, сообщение пользователю и "прочее" - синим/серым.
        /// Соответствие CLU значениям 'PROTOCOLSTYPES' - см. 'dsqProtocols.mProtocolTypeClue' (порядок посева
        /// в 'PclTyp' СТРОГО совпадает с порядком объявления enum, от 1 до 12).
        /// </summary>
        private void mAreaProtocols_GridCellEnter(object sender, EventArgs e)
        {
            DataGridViewRow vRow = _cAreaProtocols.__fCurrentRow_;
            if (vRow == null || vRow.Cells["lnkPclTyp"].Value == null)
                return;

            int vPclTypClue;
            if (int.TryParse(vRow.Cells["lnkPclTyp"].Value.ToString(), out vPclTypClue) == false)
                return;

            switch (vPclTypClue)
            {
                case 1: // Ошибка приложения
                case 2: // Ошибка программирования
                case 3: // Исключение
                    _cAreaProtocols.__mCellStyle(Color.FromArgb(255, 230, 230));
                    break;
                case 5: // Ошибка источника данных
                case 7: // Ошибка устройства
                case 9: // Ошибка пользователя
                    _cAreaProtocols.__mCellStyle(Color.FromArgb(255, 210, 210));
                    break;
                case 4: // Событие приложения
                case 6: // Событие источника данных
                case 8: // Событие устройства
                case 10: // Событие пользователя
                    _cAreaProtocols.__mCellStyle(Color.FromArgb(230, 255, 230));
                    break;
                case 11: // Сообщение пользователю
                    _cAreaProtocols.__mCellStyle(Color.FromArgb(230, 230, 255));
                    break;
                default: // Прочее
                    _cAreaProtocols.__mCellStyle(Color.White);
                    break;
            }

            if (_manualDbMode == true || _oProtocols == null)
                return;

            object vCluValue = vRow.Cells["CLU"].Value;
            int vClu;
            if (vCluValue == null || int.TryParse(vCluValue.ToString(), out vClu) == false)
                return;

            mProtocolRecordsLoad(vClu);
        }

        #endregion Объект

        #region - Протоколы (авто-загрузка из 'dsqProtocols' + фильтры)

        /// <summary>
        /// Загрузка списка протоколов из SQLite базы данных 'dsqProtocols' (общий экземпляр, назначенный
        /// активным логгером приложения в 'cspBegin.cs')
        /// </summary>
        private void mProtocolsAutoLoad()
        {
            _oProtocols = cspApplication.__oProtocols as dsqProtocols;

            if (_oProtocols == null)
            {
                _cLabelStatus.Text = "Логгер протоколов не инициализирован как SQLite (dsqProtocols)";
                mFiltersEnable(false);
                return;
            }

            mFiltersPopulate();
            mProtocolsLoad();
        }
        /// <summary>
        /// Описание фактической схемы открытой базы протоколов - разные реальные копии 'protocols.db',
        /// встреченные в проекте, используют разные варианты именования (см. 'mSchemaDetect'):
        /// 'dsiApp'/'desApp', 'PclRrdTyp'/'RrdTyp', 'Msg'/'Err', хост/пользователь текстом в 'Pcl.Hst'/'Pcl.Usr'
        /// или через связи 'lnkCpu'/'lnkUsr' на таблицы 'Cpu'/'Usr' (которых может не быть вообще).
        /// </summary>
        private class ProtocolsSchemaInfo
        {
            public string AppNameColumn = "dsiApp";
            public string PclTypNameColumn = "dsiPclTyp";
            /// <summary>[true] - хост/пользователь читаются прямо из 'Pcl.Hst'/'Pcl.Usr' (текстом)</summary>
            public bool HostUserDirectText = false;
            /// <summary>[true] - хост/пользователь читаются через JOIN на 'Cpu'/'Usr' по 'lnkCpu'/'lnkUsr'</summary>
            public bool HasCpuUsrTables = false;
            public string RrdTypTable = "PclRrdTyp";
            public string RrdTypNameColumn = "dsiPclRrdTyp";
            public string RrdLinkColumn = "lnkPclRrdTyp";
            public string MessageColumn = "Msg";
        }
        /// <summary>
        /// Определение варианта схемы открытой базы протоколов по фактически существующим таблицам/столбцам
        /// (а не по предположению) - разные реальные копии 'protocols.db' в проекте создавались разными
        /// версиями логики записи и отличаются именованием. Без этой проверки запросы вьювера падали с
        /// "no such table"/"no such column" (см. 'protocols_db_errors.log') и список протоколов оставался
        /// пустым, даже если данные в базе есть.
        /// </summary>
        /// <param name="pTableExists">Проверка существования таблицы по имени</param>
        /// <param name="pColumnExists">Проверка существования столбца в таблице по именам таблицы/столбца</param>
        private ProtocolsSchemaInfo mSchemaDetect(Func<string, bool> pTableExists, Func<string, string, bool> pColumnExists)
        {
            ProtocolsSchemaInfo vSchema = new ProtocolsSchemaInfo();

            vSchema.AppNameColumn = pColumnExists("App", "dsiApp") == true ? "dsiApp" : "desApp";
            vSchema.PclTypNameColumn = pColumnExists("PclTyp", "dsiPclTyp") == true ? "dsiPclTyp" : "desPclTyp";

            if (pColumnExists("Pcl", "Hst") == true && pColumnExists("Pcl", "Usr") == true)
                vSchema.HostUserDirectText = true;
            else if (pTableExists("Cpu") == true && pTableExists("Usr") == true)
                vSchema.HasCpuUsrTables = true;

            if (pTableExists("PclRrdTyp") == true)
            {
                vSchema.RrdTypTable = "PclRrdTyp";
                vSchema.RrdTypNameColumn = pColumnExists("PclRrdTyp", "dsiPclRrdTyp") == true ? "dsiPclRrdTyp" : "desPclRrdTyp";
            }
            else
            {
                vSchema.RrdTypTable = "RrdTyp";
                vSchema.RrdTypNameColumn = pColumnExists("RrdTyp", "dsiRrdTyp") == true ? "dsiRrdTyp" : "desRrdTyp";
            }

            vSchema.RrdLinkColumn = pColumnExists("PclRrd", "lnkPclRrdTyp") == true ? "lnkPclRrdTyp" : "lnkRrdTyp";
            vSchema.MessageColumn = pColumnExists("PclRrd", "Msg") == true ? "Msg" : "Err";

            return vSchema;
        }
        /// <summary>
        /// 'mSchemaDetect' для активного логгера '_oProtocols' (авто-загрузка при старте формы)
        /// </summary>
        private ProtocolsSchemaInfo mSchemaDetect()
        {
            if (_oProtocols == null)
                return new ProtocolsSchemaInfo();

            return mSchemaDetect(_oProtocols.__mTableExists, _oProtocols.__mColumnExists);
        }
        /// <summary>
        /// 'mSchemaDetect' для стороннего файла '*.db', открытого вручную через 'Файл / Открыть протокол'
        /// (не связан с '_oProtocols' - см. 'mMenuFileOpen_Click')
        /// </summary>
        private ProtocolsSchemaInfo mSchemaDetectFor(datUnitDataSource pDataSource)
        {
            return mSchemaDetect(
                pTable => pDataSource.__mTableExists(pTable),
                (pTable, pColumn) => mColumnExistsFor(pDataSource, pTable, pColumn));
        }
        /// <summary>
        /// Проверка существования столбца в таблице стороннего файла '*.db' через 'PRAGMA table_info'
        /// (у 'datUnitDataSource' нет готового метода проверки столбцов - см. 'mSchemaDetectFor')
        /// </summary>
        private bool mColumnExistsFor(datUnitDataSource pDataSource, string pTableName, string pColumnName)
        {
            DataTable vColumns = pDataSource.__mSqlQuery("PRAGMA table_info(" + pTableName + ")");
            if (vColumns == null)
                return false;

            foreach (DataRow vColumn in vColumns.Rows)
            {
                if (vColumn["name"] != DBNull.Value && string.Equals(vColumn["name"].ToString(), pColumnName, StringComparison.OrdinalIgnoreCase) == true)
                    return true;
            }

            return false;
        }
        /// <summary>
        /// Заполнение выпадающих списков фильтра (Вид протокола / Приложение / Пользователь / Хост)
        /// реальными значениями из справочных таблиц базы. Первый пункт каждого списка - "(все)".
        /// </summary>
        private void mFiltersPopulate()
        {
            if (_oProtocols == null)
                return;

            ProtocolsSchemaInfo vSchema = mSchemaDetect();

            _fFiltersPopulating = true;
            try
            {
                mFilterComboFill(_cFilterType, "SELECT DISTINCT " + vSchema.PclTypNameColumn + " AS V FROM PclTyp WHERE " + vSchema.PclTypNameColumn + " IS NOT NULL ORDER BY " + vSchema.PclTypNameColumn);
                mFilterComboFill(_cFilterApp, "SELECT DISTINCT " + vSchema.AppNameColumn + " AS V FROM App WHERE " + vSchema.AppNameColumn + " IS NOT NULL ORDER BY " + vSchema.AppNameColumn);

                if (vSchema.HostUserDirectText == true)
                {
                    mFilterComboFill(_cFilterUser, "SELECT DISTINCT Usr AS V FROM Pcl WHERE Usr IS NOT NULL AND Usr <> '' ORDER BY Usr");
                    mFilterComboFill(_cFilterHost, "SELECT DISTINCT Hst AS V FROM Pcl WHERE Hst IS NOT NULL AND Hst <> '' ORDER BY Hst");
                }
                else if (vSchema.HasCpuUsrTables == true)
                {
                    mFilterComboFill(_cFilterUser, "SELECT DISTINCT dsiUsr AS V FROM Usr WHERE dsiUsr IS NOT NULL ORDER BY dsiUsr");
                    mFilterComboFill(_cFilterHost, "SELECT DISTINCT dsiCpu AS V FROM Cpu WHERE dsiCpu IS NOT NULL ORDER BY dsiCpu");
                }
                else
                {
                    /// Ни текстовых 'Pcl.Hst'/'Pcl.Usr', ни таблиц 'Cpu'/'Usr' - взять значения неоткуда,
                    /// список остаётся с одним пунктом "(все)"
                    _cFilterUser.Items.Clear();
                    _cFilterUser.Items.Add(FILTERITEMALL);
                    _cFilterUser.SelectedIndex = 0;
                    _cFilterHost.Items.Clear();
                    _cFilterHost.Items.Add(FILTERITEMALL);
                    _cFilterHost.SelectedIndex = 0;
                }
            }
            finally
            {
                _fFiltersPopulating = false;
            }
        }
        /// <summary>
        /// Заполнение одного выпадающего списка фильтра результатом запроса (один текстовый столбец "V")
        /// </summary>
        private void mFilterComboFill(ComboBox pCombo, string pQuery)
        {
            pCombo.Items.Clear();
            pCombo.Items.Add(FILTERITEMALL);

            try
            {
                DataTable vTable = _oProtocols.__mQuery(pQuery);
                if (vTable != null)
                {
                    foreach (DataRow vRow in vTable.Rows)
                    {
                        if (vRow["V"] != DBNull.Value)
                            pCombo.Items.Add(vRow["V"].ToString());
                    }
                }
            }
            catch
            {
                /// Справочник мог ещё не заполниться (пустая база) - выпадающий список просто останется с "(все)"
            }

            pCombo.SelectedIndex = 0;
        }
        /// <summary>
        /// Построение и выполнение запроса списка протоколов с учётом текущих значений панели фильтров,
        /// отображение результата в левой таблице
        /// </summary>
        private void mProtocolsLoad()
        {
            if (_oProtocols == null)
                return;

            ProtocolsSchemaInfo vSchema = mSchemaDetect();
            string vWhere = mFiltersWhereClauseBuild(vSchema);

            string vHostColumn = vSchema.HostUserDirectText == true ? "P.Hst AS Hst" : (vSchema.HasCpuUsrTables == true ? "C.dsiCpu AS Hst" : "'' AS Hst");
            string vUserColumn = vSchema.HostUserDirectText == true ? "P.Usr AS Usr" : (vSchema.HasCpuUsrTables == true ? "U.dsiUsr AS Usr" : "'' AS Usr");

            string vQuery = "SELECT P.CLU, P.CHG, A." + vSchema.AppNameColumn + " AS App, P.lnkPclTyp, PT." + vSchema.PclTypNameColumn + " AS PclTyp, "
                + vHostColumn + ", P.Prc, " + vUserColumn + " "
                + "FROM Pcl P "
                + "LEFT JOIN App A ON A.CLU = P.lnkApp "
                + "LEFT JOIN PclTyp PT ON PT.CLU = P.lnkPclTyp "
                + (vSchema.HasCpuUsrTables == true ? "LEFT JOIN Cpu C ON C.CLU = P.lnkCpu LEFT JOIN Usr U ON U.CLU = P.lnkUsr " : "")
                + (vWhere.Length > 0 ? "WHERE " + vWhere + " " : "")
                + "ORDER BY P.CHG DESC"; // По умолчанию - сначала новые (клик по заголовку столбца сортирует иначе)

            DataTable vDataTable;
            try
            {
                vDataTable = _oProtocols.__mQuery(vQuery);
            }
            catch (Exception vException)
            {
                _cLabelStatus.Text = "Ошибка загрузки протоколов из БД: " + vException.Message;
                return;
            }

            mProtocolsDisplay(vDataTable);
        }
        /// <summary>
        /// Сборка условия WHERE (без слова "WHERE") по текущему состоянию контролов панели фильтров.
        /// Возвращает пустую строку, если ни один фильтр не активен.
        /// </summary>
        private string mFiltersWhereClauseBuild(ProtocolsSchemaInfo pSchema)
        {
            List<string> vConditions = new List<string>();

            string vHostColumn = pSchema.HostUserDirectText == true ? "P.Hst" : (pSchema.HasCpuUsrTables == true ? "C.dsiCpu" : null);
            string vUserColumn = pSchema.HostUserDirectText == true ? "P.Usr" : (pSchema.HasCpuUsrTables == true ? "U.dsiUsr" : null);

            string vSearch = _cFilterSearch != null ? _cFilterSearch.Text.Trim() : "";
            if (vSearch.Length > 0)
            {
                string vEscaped = vSearch.Replace("'", "''");
                vConditions.Add("(P.Prc LIKE '%" + vEscaped + "%'"
                    + " OR A." + pSchema.AppNameColumn + " LIKE '%" + vEscaped + "%'"
                    + " OR PT." + pSchema.PclTypNameColumn + " LIKE '%" + vEscaped + "%'"
                    + (vHostColumn != null ? " OR " + vHostColumn + " LIKE '%" + vEscaped + "%'" : "")
                    + (vUserColumn != null ? " OR " + vUserColumn + " LIKE '%" + vEscaped + "%'" : "")
                    + " OR EXISTS (SELECT 1 FROM PclRrd PR WHERE PR.lnkPcl = P.CLU AND PR." + pSchema.MessageColumn + " LIKE '%" + vEscaped + "%'))");
            }

            mFilterComboConditionAdd(vConditions, _cFilterType, "PT." + pSchema.PclTypNameColumn);
            mFilterComboConditionAdd(vConditions, _cFilterApp, "A." + pSchema.AppNameColumn);
            /// Фильтры по пользователю/хосту опираются на текстовые 'Pcl.Usr'/'Pcl.Hst' либо на 'Usr'/'Cpu' -
            /// на базе без обоих вариантов выпадающие списки и так остаются на "(все)" (см. 'mFiltersPopulate')
            if (vUserColumn != null)
                mFilterComboConditionAdd(vConditions, _cFilterUser, vUserColumn);
            if (vHostColumn != null)
                mFilterComboConditionAdd(vConditions, _cFilterHost, vHostColumn);

            if (_cFilterDateFromOn.Checked == true)
                vConditions.Add("P.CHG >= '" + _cFilterDateFrom.Value.Date.ToString("yyyy-MM-dd 00:00:00") + "'");

            if (_cFilterDateToOn.Checked == true)
                vConditions.Add("P.CHG <= '" + _cFilterDateTo.Value.Date.ToString("yyyy-MM-dd 23:59:59") + "'");

            return string.Join(" AND ", vConditions);
        }
        /// <summary>
        /// Если выпадающий список фильтра не установлен в "(все)" - добавляет условие равенства
        /// </summary>
        private void mFilterComboConditionAdd(List<string> pConditions, ComboBox pCombo, string pColumnName)
        {
            if (pCombo == null || pCombo.SelectedItem == null)
                return;

            string vSelected = pCombo.SelectedItem.ToString();
            if (vSelected == FILTERITEMALL)
                return;

            pConditions.Add(pColumnName + " = '" + vSelected.Replace("'", "''") + "'");
        }
        /// <summary>
        /// Изменение любого контрола фильтра - перезапрашивает список протоколов (кроме программного
        /// заполнения списков в 'mFiltersPopulate' / 'mFilterClear_Click')
        /// </summary>
        private void mFilter_Changed(object sender, EventArgs e)
        {
            if (_fFiltersPopulating == true || _manualDbMode == true || _oProtocols == null)
                return;

            mProtocolsLoad();
        }
        /// <summary>
        /// Включение/выключение поля даты одновременно с его флажком
        /// </summary>
        private void mFilterDate_CheckedChanged(object sender, EventArgs e)
        {
            _cFilterDateFrom.Enabled = _cFilterDateFromOn.Checked;
            _cFilterDateTo.Enabled = _cFilterDateToOn.Checked;

            mFilter_Changed(sender, e);
        }
        /// <summary>
        /// Сброс всех фильтров панели и обновление списка протоколов
        /// </summary>
        private void mFilterClear_Click(object sender, EventArgs e)
        {
            _fFiltersPopulating = true;
            try
            {
                _cFilterSearch.Text = "";
                if (_cFilterType.Items.Count > 0) _cFilterType.SelectedIndex = 0;
                if (_cFilterApp.Items.Count > 0) _cFilterApp.SelectedIndex = 0;
                if (_cFilterUser.Items.Count > 0) _cFilterUser.SelectedIndex = 0;
                if (_cFilterHost.Items.Count > 0) _cFilterHost.SelectedIndex = 0;
                _cFilterDateFromOn.Checked = false;
                _cFilterDateToOn.Checked = false;
            }
            finally
            {
                _fFiltersPopulating = false;
            }

            mProtocolsLoad();
        }
        /// <summary>
        /// Включение/выключение всей панели фильтров разом (выключается при переходе в '_manualDbMode')
        /// </summary>
        private void mFiltersEnable(bool pEnabled)
        {
            _cFilterSearch.Enabled = pEnabled;
            _cFilterType.Enabled = pEnabled;
            _cFilterApp.Enabled = pEnabled;
            _cFilterUser.Enabled = pEnabled;
            _cFilterHost.Enabled = pEnabled;
            _cFilterDateFromOn.Enabled = pEnabled;
            _cFilterDateToOn.Enabled = pEnabled;
            _cFilterDateFrom.Enabled = pEnabled && _cFilterDateFromOn.Checked;
            _cFilterDateTo.Enabled = pEnabled && _cFilterDateToOn.Checked;
            _cFilterClear.Enabled = pEnabled;
        }
        /// <summary>
        /// Отображение списка протоколов в левой таблице
        /// </summary>
        /// <param name="pDataTable">Результат запроса к 'Pcl' (CLU, CHG, App, lnkPclTyp, PclTyp, Hst, Prc, Usr)</param>
        private void mProtocolsDisplay(DataTable pDataTable)
        {
            _oDataTableProtocols.Rows.Clear();

            foreach (DataRow vSourceRow in pDataTable.Rows)
            {
                DataRow vRow = _oDataTableProtocols.NewRow();
                vRow["CLU"] = vSourceRow["CLU"] != DBNull.Value ? vSourceRow["CLU"].ToString() : "";

                DateTime vChgDateTime;
                string vChgRaw = vSourceRow["CHG"] != DBNull.Value ? vSourceRow["CHG"].ToString() : "";
                vRow["CHG"] = DateTime.TryParse(vChgRaw, out vChgDateTime) ? vChgDateTime.ToString("dd.MM.yyyy HH:mm:ss") : vChgRaw;

                vRow["App"] = vSourceRow["App"] != DBNull.Value ? vSourceRow["App"].ToString() : "";
                vRow["lnkPclTyp"] = vSourceRow["lnkPclTyp"] != DBNull.Value ? vSourceRow["lnkPclTyp"].ToString() : "";
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

            _cLabelStatus.Text = "Загружено протоколов: " + pDataTable.Rows.Count + " (SQLite: " + Path.Combine(appApplication.__oPathes.__fDirectoryDatabases_, "protocols.db") + ")";
        }
        /// <summary>
        /// Загрузка и отображение записей выбранного протокола в правой таблице
        /// </summary>
        /// <param name="pProtocolClue">Реальный CLU строки в 'Pcl'</param>
        private void mProtocolRecordsLoad(int pProtocolClue)
        {
            ProtocolsSchemaInfo vSchema = mSchemaDetect();

            string vQuery = "SELECT PR.lnkPcl AS Protocol, PR.CLU AS \"Key\", PRT." + vSchema.RrdTypNameColumn + " AS Type, PR." + vSchema.MessageColumn + " AS Message, PR.Tck AS Time "
                + "FROM PclRrd PR "
                + "LEFT JOIN " + vSchema.RrdTypTable + " PRT ON PRT.CLU = PR." + vSchema.RrdLinkColumn + " "
                + "WHERE PR.lnkPcl = " + pProtocolClue.ToString();

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

            _cLabelStatus.Text = "Протокол CLU=" + pProtocolClue + " \u2014 записей: " + vRecords.Rows.Count;
        }

        #endregion Протоколы (авто-загрузка из 'dsqProtocols' + фильтры)

        #region - События

        /// <summary>
        /// Выполняется при выборе пункта меню 'Файл / Открыть' - ручной просмотр стороннего файла '*.db'
        /// (например скопированного с другой машины). Не связан с '_oProtocols' - собственный источник данных.
        /// </summary>
        private void mMenuFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog vOpenFileDialog = new OpenFileDialog();
            vOpenFileDialog.AddExtension = true;
            vOpenFileDialog.AutoUpgradeEnabled = true;
            vOpenFileDialog.CheckFileExists = true;
            vOpenFileDialog.CheckPathExists = true;
            vOpenFileDialog.Filter = "База данных протоколов (*.db)|*.db|Все файлы (*.*)|*.*";

            if (vOpenFileDialog.ShowDialog() != DialogResult.OK)
                return;

            string vFilePath = vOpenFileDialog.FileName;
            _manualDbMode = true;
            mFiltersEnable(false); // Фильтры работают только против '_oProtocols' - для сторонней базы отключаются

            /// ИСПРАВЛЕНО: 'dsqDataSourceSqlite.__mSqlQuery' при сбое SQL (например "no such table" у устаревшей
            /// базы) показывает диалог ошибки и возвращает null - на следующей строке 'foreach' по '.Rows' валил
            /// форму NullReferenceException-ом. 'dsqDataSourceSqliteWithProtocol' пишет сбой в
            /// 'protocols_db_errors.log' рядом с файлом базы и всегда возвращает пустую (не null) DataTable.
            datUnitDataSource vDataSource = new dsqDataSourceSqliteWithProtocol();
            vDataSource.__fDatabasePath = Path.GetDirectoryName(vFilePath);
            vDataSource.__fDatabaseName = Path.GetFileName(vFilePath);

            /// ИСПРАВЛЕНО: у разных реальных копий 'protocols.db' (легаси, тестовые, ещё не тронутые
            /// текущей версией 'dsqProtocols.__mTablesFill') отличается именование таблиц/столбцов - см.
            /// 'mSchemaDetectFor'/'ProtocolsSchemaInfo'. Без этой проверки запрос падал с "no such table"/
            /// "no such column" (см. 'protocols_db_errors.log') и обе таблицы оставались пустыми, даже
            /// если данные в базе есть.
            ProtocolsSchemaInfo vSchema = mSchemaDetectFor(vDataSource);

            string vHostColumn = vSchema.HostUserDirectText == true ? "P.Hst AS Hst" : (vSchema.HasCpuUsrTables == true ? "C.dsiCpu AS Hst" : "'' AS Hst");
            string vUserColumn = vSchema.HostUserDirectText == true ? "P.Usr AS Usr" : (vSchema.HasCpuUsrTables == true ? "U.dsiUsr AS Usr" : "'' AS Usr");

            string vQueryPcl = "SELECT P.CLU, P.CHG, A." + vSchema.AppNameColumn + " AS App, PT." + vSchema.PclTypNameColumn + " AS PclTyp, "
                + vHostColumn + ", P.Prc, " + vUserColumn + " "
                + "FROM Pcl P "
                + "LEFT JOIN App A ON A.CLU = P.lnkApp "
                + "LEFT JOIN PclTyp PT ON PT.CLU = P.lnkPclTyp "
                + (vSchema.HasCpuUsrTables == true ? "LEFT JOIN Cpu C ON C.CLU = P.lnkCpu LEFT JOIN Usr U ON U.CLU = P.lnkUsr" : "");

            DataTable vDataTablePcl = vDataSource.__mSqlQuery(vQueryPcl) ?? new DataTable(); // Доп. страховка, если источник всё же вернёт null
            foreach (DataRow vDataRow in vDataTablePcl.Rows)
            {
                DateTime vChgDateTime;
                if (vDataRow["CHG"] != DBNull.Value && DateTime.TryParse(vDataRow["CHG"].ToString(), out vChgDateTime) == true)
                    vDataRow["CHG"] = vChgDateTime.ToString("dd.MM.yyyy HH:mm:ss");
            }
            _cAreaProtocols.__fDataSource_ = vDataTablePcl;
            _cAreaProtocols.__mGridRefresh();

            string vQueryPclRrd = "SELECT PR.lnkPcl AS Protocol, PR.CLU AS \"Key\", PRT." + vSchema.RrdTypNameColumn + " AS Type, PR." + vSchema.MessageColumn + " AS Message, PR.Tck AS Time "
                + "FROM PclRrd PR "
                + "LEFT JOIN " + vSchema.RrdTypTable + " PRT ON PRT.CLU = PR." + vSchema.RrdLinkColumn;
            /// Примечание: 'Tck' в 'PclRrd' - затраченное время выполнения (тики-длительность), а не момент
            /// времени создания записи - выводится как есть, без форматирования в дату.

            DataTable vDataTablePclRrd = vDataSource.__mSqlQuery(vQueryPclRrd) ?? new DataTable();
            _cAreaProtocolsRecords.__fDataSource_ = vDataTablePclRrd;
            _cAreaProtocolsRecords.__mGridRefresh();

            /// Если запрос не дал ни одной строки - вероятно, у выбранной базы устаревшая схема (нет таблиц
            /// 'Cpu'/'Usr' и т.п.). Подробности сбоя - в 'protocols_db_errors.log' рядом с файлом базы.
            if (vDataTablePcl.Rows.Count == 0)
                _cLabelStatus.Text = "Протоколы не загружены - возможно, база устарела или повреждена. Подробности: " + Path.Combine(Path.GetDirectoryName(vFilePath), "protocols_db_errors.log");
            else
                _cLabelStatus.Text = "Загружено из БД: " + vDataTablePcl.Rows.Count + " протоколов (" + vFilePath + ")";
        }
        /// <summary>
        /// Выполняется при выборе пункта меню 'Файл / Закрыть протокол' - выход из режима просмотра
        /// стороннего файла и возврат к обычному виду ('dsqProtocols' / 'Databases\protocols.db')
        /// </summary>
        private void mMenuFileClose_Click(object sender, EventArgs e)
        {
            _manualDbMode = false;
            mFiltersEnable(true);
            mProtocolsAutoLoad();
        }

        #endregion События

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Блок главного окна приложения
        /// </summary>
        protected elmBlockFormMain _cBlockFormMain = new elmBlockFormMain();
        /// <summary>
        /// Разделители
        /// </summary>
        protected elmComponentSplitter _cSplitter = new elmComponentSplitter();
        /// <summary>
        /// Пункт меню 'Файл'
        /// </summary>
        protected elmComponentMenuItem _cMenuFile = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'Файл / Открыть протокол'
        /// </summary>
        protected elmComponentMenuItem _cMenuFileOpen = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'Файл / Закрыть протокол'
        /// </summary>
        protected elmComponentMenuItem _cMenuFileClose = new elmComponentMenuItem();

        /// <summary>
        /// Область для просмотра протоколов
        /// </summary>
        protected cspAreaGrid _cAreaProtocols = new cspAreaGrid();
        /// <summary>
        /// Область для просмотра записей в протоколе
        /// </summary>
        protected cspAreaGrid _cAreaProtocolsRecords = new cspAreaGrid();
        /// <summary>
        /// Панель фильтров над таблицей протоколов
        /// </summary>
        protected TableLayoutPanel _cPanelFilters = new TableLayoutPanel();
        /// <summary>
        /// Строка состояния внизу формы
        /// </summary>
        protected Label _cLabelStatus = new Label();

        /// <summary>
        /// Таблица с протоколами
        /// </summary>
        protected DataTable _oDataTableProtocols = new DataTable();
        /// <summary>
        /// Таблица с записями в протоколе
        /// </summary>
        protected DataTable _oDataTableProtocolsRecord = new DataTable();

        #endregion Компоненты

        #region - Фильтры

        /// <summary>Свободный текст: Prc / App / PclTyp / Hst / Usr / Msg записей</summary>
        protected TextBox _cFilterSearch = new TextBox();
        /// <summary>Вид протокола (PclTyp.dsiPclTyp)</summary>
        protected ComboBox _cFilterType = new ComboBox();
        /// <summary>Приложение (App.dsiApp)</summary>
        protected ComboBox _cFilterApp = new ComboBox();
        /// <summary>Пользователь (Usr.dsiUsr)</summary>
        protected ComboBox _cFilterUser = new ComboBox();
        /// <summary>Компьютер (Cpu.dsiCpu)</summary>
        protected ComboBox _cFilterHost = new ComboBox();
        protected CheckBox _cFilterDateFromOn = new CheckBox();
        protected DateTimePicker _cFilterDateFrom = new DateTimePicker();
        protected CheckBox _cFilterDateToOn = new CheckBox();
        protected DateTimePicker _cFilterDateTo = new DateTimePicker();
        protected Button _cFilterClear = new Button();
        private const string FILTERITEMALL = "(все)";
        /// <summary>[true] - идёт программное заполнение фильтров, пользовательские обработчики должны молчать</summary>
        private bool _fFiltersPopulating = false;

        #endregion Фильтры

        #region - Внутренние

        /// <summary>
        /// Данные протоколов из 'dsqProtocols' (общий экземпляр-логгер приложения)
        /// </summary>
        private dsqProtocols _oProtocols;
        /// <summary>
        /// [true] - открыт сторонний '.db' вручную через 'Файл / Открыть протокол'; фильтры и клик по строке
        /// не должны обращаться к '_oProtocols' (другая база)
        /// </summary>
        private bool _manualDbMode = false;
        /// <summary>
        /// Полное название класса
        /// </summary>
        protected string _fClassNameFull = "";
        /// <summary>
        /// Таблица со списком исправленных полей
        /// </summary>
        protected DataTable _fTableChanges = new DataTable("ChangesValue");
        /// <summary>
        /// Список ошибок при выполнении триггеров
        /// </summary>
        protected ArrayList _fTriggerErrorsDescriptions = new ArrayList();

        #endregion Внутренние

        #endregion ПОЛЯ

    }
}