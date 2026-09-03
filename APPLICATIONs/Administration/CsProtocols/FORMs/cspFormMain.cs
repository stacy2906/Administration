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
            _cBlockFormMain.__mMenuAdd(_cMenuView);

            // Левая панель: TableLayoutPanel — фильтры в фиксированной строке, сетка в оставшемся месте.
            // Dock Top+Fill на обычной Panel с cspAreaGrid (сам SplitContainer) давал перекрытие и «серую» сетку.
            _cLeftHost.Dock = DockStyle.Fill;
            _cLeftHost.ColumnCount = 1;
            _cLeftHost.RowCount = 2;
            _cLeftHost.ColumnStyles.Clear();
            _cLeftHost.RowStyles.Clear();
            _cLeftHost.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _cLeftHost.RowStyles.Add(new RowStyle(SizeType.Absolute, 260F)); // фильтры + подписи
            _cLeftHost.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // сетка протоколов
            _cLeftHost.Padding = new Padding(0);
            _cLeftHost.Margin = new Padding(0);

            _cPanelFilters.Dock = DockStyle.Fill;
            _cPanelFilters.Margin = new Padding(0);
            _cAreaProtocols.Dock = DockStyle.Fill;
            _cAreaProtocols.Margin = new Padding(0);

            _cLeftHost.Controls.Add(_cPanelFilters, 0, 0);
            _cLeftHost.Controls.Add(_cAreaProtocols, 0, 1);
            _cSplitter.Panel1.Controls.Add(_cLeftHost);

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
          
            {
                _cMenuView.__fCaption_ = "Вид";
                _cMenuView.DropDownItems.Add(_cMenuViewCombined);
                _cMenuViewCombined.__fCaption_ = "Совмещённый просмотр (записи + заголовок)";
                _cMenuViewCombined.Click += mMenuViewCombined_Click;
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
            // _cPanelFilters - панель фильтров с подписями к каждому полю
            {
                _cPanelFilters.Dock = DockStyle.Fill;
                _cPanelFilters.ColumnCount = 4;
                _cPanelFilters.RowCount = 7;
                _cPanelFilters.Padding = new Padding(4);
                _cPanelFilters.Enabled = true;
                _cPanelFilters.Visible = true;
                _cPanelFilters.TabStop = true;
                _cPanelFilters.BackColor = elmApplication.__oInterface.__mColor(COLORS.FormActive);
                _cPanelFilters.ColumnStyles.Clear();
                _cPanelFilters.RowStyles.Clear();
                // подпись | поле | подпись | поле
                _cPanelFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
                _cPanelFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                _cPanelFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
                _cPanelFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                for (int i = 0; i < 7; i++)
                    _cPanelFilters.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

                // Строка 0: процедура / описание ошибки
                mAddFilterLabel(_cPanelFilters, "Процедура:", 0, 0);
                _cFilterProcedure.Dock = DockStyle.Fill;
                _cFilterProcedure.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                _cFilterProcedure.AutoCompleteSource = AutoCompleteSource.CustomSource;
                _cFilterProcedure.AutoCompleteCustomSource = _oProcedureAutoComplete;
                mSetPlaceholder(_cFilterProcedure, "поиск по названию...");
                _cPanelFilters.Controls.Add(_cFilterProcedure, 1, 0);
                _cFilterProcedure.TextChanged += mFilter_Changed;

                mAddFilterLabel(_cPanelFilters, "Описание ошибки:", 2, 0);
                mSetPlaceholder(_cFilterErrorDesc, "текст в Msg...");
                _cFilterErrorDesc.Dock = DockStyle.Fill;
                _cPanelFilters.Controls.Add(_cFilterErrorDesc, 3, 0);
                _cFilterErrorDesc.TextChanged += mFilter_Changed;

                // Строка 1: решение пользователя (на всю ширину)
                mAddFilterLabel(_cPanelFilters, "Решение пользователя:", 0, 1);
                mSetPlaceholder(_cFilterSolution, "поиск в ответах...");
                _cFilterSolution.Dock = DockStyle.Fill;
                _cPanelFilters.Controls.Add(_cFilterSolution, 1, 1);
                _cPanelFilters.SetColumnSpan(_cFilterSolution, 3);
                _cFilterSolution.TextChanged += mFilter_Changed;

                // Строка 2: вид протокола / вид записи
                mAddFilterLabel(_cPanelFilters, "Вид протокола:", 0, 2);
                _cFilterType.Dock = DockStyle.Fill;
                _cFilterType.DropDownStyle = ComboBoxStyle.DropDownList;
                _cPanelFilters.Controls.Add(_cFilterType, 1, 2);
                _cFilterType.SelectedIndexChanged += mFilter_Changed;

                mAddFilterLabel(_cPanelFilters, "Вид записи:", 2, 2);
                _cFilterErrorType.Dock = DockStyle.Fill;
                _cFilterErrorType.DropDownStyle = ComboBoxStyle.DropDownList;
                _cPanelFilters.Controls.Add(_cFilterErrorType, 3, 2);
                _cFilterErrorType.SelectedIndexChanged += mFilter_Changed;

                // Строка 3: приложение / пользователь
                mAddFilterLabel(_cPanelFilters, "Приложение:", 0, 3);
                _cFilterApp.Dock = DockStyle.Fill;
                _cFilterApp.DropDownStyle = ComboBoxStyle.DropDownList;
                _cPanelFilters.Controls.Add(_cFilterApp, 1, 3);
                _cFilterApp.SelectedIndexChanged += mFilter_Changed;

                mAddFilterLabel(_cPanelFilters, "Пользователь:", 2, 3);
                _cFilterUser.Dock = DockStyle.Fill;
                _cFilterUser.DropDownStyle = ComboBoxStyle.DropDownList;
                _cPanelFilters.Controls.Add(_cFilterUser, 3, 3);
                _cFilterUser.SelectedIndexChanged += mFilter_Changed;

                // Строка 4: хост
                mAddFilterLabel(_cPanelFilters, "Компьютер (хост):", 0, 4);
                _cFilterHost.Dock = DockStyle.Fill;
                _cFilterHost.DropDownStyle = ComboBoxStyle.DropDownList;
                _cPanelFilters.Controls.Add(_cFilterHost, 1, 4);
                _cPanelFilters.SetColumnSpan(_cFilterHost, 3);
                _cFilterHost.SelectedIndexChanged += mFilter_Changed;

                // Строка 5: период дат
                mAddFilterLabel(_cPanelFilters, "Период с:", 0, 5);
                var vDateFromPanel = new Panel { Dock = DockStyle.Fill };
                _cFilterDateFromOn.Text = "";
                _cFilterDateFromOn.Dock = DockStyle.Left;
                _cFilterDateFromOn.Width = 20;
                _cFilterDateFromOn.Checked = false;
                _cFilterDateFrom.Dock = DockStyle.Fill;
                _cFilterDateFrom.Format = DateTimePickerFormat.Short;
                _cFilterDateFrom.Value = DateTime.Today.AddDays(-30);
                _cFilterDateFrom.Enabled = false;
                vDateFromPanel.Controls.Add(_cFilterDateFrom);
                vDateFromPanel.Controls.Add(_cFilterDateFromOn);
                _cPanelFilters.Controls.Add(vDateFromPanel, 1, 5);
                _cFilterDateFromOn.CheckedChanged += mFilterDate_CheckedChanged;
                _cFilterDateFrom.ValueChanged += mFilter_Changed;

                mAddFilterLabel(_cPanelFilters, "по:", 2, 5);
                var vDateToPanel = new Panel { Dock = DockStyle.Fill };
                _cFilterDateToOn.Text = "";
                _cFilterDateToOn.Dock = DockStyle.Left;
                _cFilterDateToOn.Width = 20;
                _cFilterDateToOn.Checked = false;
                _cFilterDateTo.Dock = DockStyle.Fill;
                _cFilterDateTo.Format = DateTimePickerFormat.Short;
                _cFilterDateTo.Value = DateTime.Today;
                _cFilterDateTo.Enabled = false;
                vDateToPanel.Controls.Add(_cFilterDateTo);
                vDateToPanel.Controls.Add(_cFilterDateToOn);
                _cPanelFilters.Controls.Add(vDateToPanel, 3, 5);
                _cFilterDateToOn.CheckedChanged += mFilterDate_CheckedChanged;
                _cFilterDateTo.ValueChanged += mFilter_Changed;

                // Строка 6: сброс
                _cFilterClear.Text = "Сбросить фильтры";
                _cFilterClear.Dock = DockStyle.Fill;
                _cPanelFilters.Controls.Add(_cFilterClear, 0, 6);
                _cPanelFilters.SetColumnSpan(_cFilterClear, 4);
                _cFilterClear.Click += mFilterClear_Click;

                mFilterPanelApplyTheme();
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
      
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();

            
            mProtocolsImportSilently();
            mClearProtocolsView();
            _cLabelStatus.Text = "База данных не открыта. Файл / Открыть протокол...";
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

            if (mHasActiveDatabase() == false)
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
        /// Импорт легаси '.pcl' файлов из папок 'PROTOCOLs' всех приложений решения в "родную" базу
        /// приложения ('dsqProtocols.__oActive_'), БЕЗ показа результата на форме - см. примечание у
        /// '_mObjectPresentation' о том, почему показ теперь строго по запросу пользователя. Использует
        /// локальную переменную, а не поле '_oProtocols' - иначе форма стала бы неявно "активной" для
        /// показа родной базы ещё до того, как пользователь сам что-то открыл
        /// </summary>
        private void mProtocolsImportSilently()
        {
            try
            {
                dsqProtocols vProtocols = dsqProtocols.__oActive_;
                int vImported = 0;
               
                List<string> vFolders = ProtocolSqliteImporter.__mProtocolsFoldersDiscover(AppDomain.CurrentDomain.BaseDirectory);
                ProtocolSqliteImporter vImporter = new ProtocolSqliteImporter();
                foreach (string vFolder in vFolders)
                    vImported += vImporter.__mImportFromFolder(vProtocols, vFolder);

                try
                {
                    string vOwn = nlApplication.appApplication.__oPathes.__fDirectoryProtocols_;
                    if (string.IsNullOrEmpty(vOwn) == false && System.IO.Directory.Exists(vOwn) == true && vFolders.Contains(vOwn) == false)
                        vImported += vImporter.__mImportFromFolder(vProtocols, vOwn);
                }
                catch { }
            }
            catch
            {
                
            }
        }

        /// <summary>
        /// Единая точка выполнения SELECT: либо активный dsqProtocols, либо вручную открытый .db
        /// </summary>
        private DataTable mQuery(string pQuery)
        {
            if (_manualDbMode == true && _oManualDataSource != null)
                return _oManualDataSource.__mSqlQuery(pQuery) ?? new DataTable();
            if (_oProtocols != null)
                return _oProtocols.__mQuery(pQuery) ?? new DataTable();
            return new DataTable();
        }

        /// <summary>
        /// Активна ли сейчас какая-либо база (авто или ручная) для фильтрации
        /// </summary>
        private bool mHasActiveDatabase()
        {
            return (_manualDbMode == true && _oManualDataSource != null) || (_oProtocols != null);
        }

        /// <summary>
        /// Определяет, хранится ли CHG как .NET ticks (по первой непустой строке Pcl)
        /// </summary>
        private bool mChgIsTicksDetect()
        {
            try
            {
                DataTable vSample = mQuery("SELECT CHG FROM Pcl WHERE CHG IS NOT NULL LIMIT 1");
                if (vSample == null || vSample.Rows.Count == 0 || vSample.Rows[0]["CHG"] == DBNull.Value)
                    return false;
                string vRaw = vSample.Rows[0]["CHG"].ToString().Trim();
                long vTicks;
                // .NET ticks ~ 18 цифр; дата-строка содержит '-' или ':' или пробел
                if (vRaw.IndexOfAny(new char[] { '-', ':', ' ', 'T' }) >= 0)
                    return false;
                if (long.TryParse(vRaw, out vTicks) == true && vTicks > 600000000000000000L)
                    return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Если фильтр «последние 30 дней» не захватывает ни одной записи — расширяем период по фактическим данным,
        /// иначе список выглядит «пустым» и фильтры кажутся сломанными (типично для тестовых баз 2025 года).
        /// </summary>
        private void mAdjustDateFilterToData()
        {
            if (mHasActiveDatabase() == false)
                return;

            ProtocolsSchemaInfo vSchema = mSchemaDetectActive();
            vSchema.ChgIsTicks = mChgIsTicksDetect();

            try
            {
                DataTable vCount = mQuery("SELECT COUNT(*) AS C FROM Pcl");
                int vTotal = 0;
                if (vCount != null && vCount.Rows.Count > 0 && vCount.Rows[0]["C"] != DBNull.Value)
                    int.TryParse(vCount.Rows[0]["C"].ToString(), out vTotal);
                if (vTotal == 0)
                    return;

                // Проверяем, есть ли строки в текущем диапазоне дат
                string vWhere = mFiltersWhereClauseBuild(vSchema);
                DataTable vFiltered = mQuery("SELECT COUNT(*) AS C FROM Pcl P "
                    + "LEFT JOIN App A ON A.CLU = P." + vSchema.AppLinkColumn + " "
                    + "LEFT JOIN PclTyp PT ON PT.CLU = P." + vSchema.PclTypLinkColumn + " "
                    + (vSchema.HasCpuUsrTables == true ? "LEFT JOIN Cpu C ON C.CLU = P.lnkCpu LEFT JOIN Usr U ON U.CLU = P.lnkUsr " : "")
                    + (vWhere.Length > 0 ? "WHERE " + vWhere : ""));
                int vInRange = 0;
                if (vFiltered != null && vFiltered.Rows.Count > 0 && vFiltered.Rows[0]["C"] != DBNull.Value)
                    int.TryParse(vFiltered.Rows[0]["C"].ToString(), out vInRange);

                if (vInRange > 0)
                    return;

                // Данных в диапазоне нет — подстраиваем даты под min/max CHG
                DataTable vMinMax = mQuery("SELECT MIN(CHG) AS Mn, MAX(CHG) AS Mx FROM Pcl WHERE CHG IS NOT NULL");
                if (vMinMax == null || vMinMax.Rows.Count == 0)
                    return;

                DateTime vMinDt, vMaxDt;
                if (mTryParseChg(vMinMax.Rows[0]["Mn"], vSchema.ChgIsTicks, out vMinDt) == false)
                    return;
                if (mTryParseChg(vMinMax.Rows[0]["Mx"], vSchema.ChgIsTicks, out vMaxDt) == false)
                    return;

                _fFiltersPopulating = true;
                try
                {
                    _cFilterDateFromOn.Checked = true;
                    _cFilterDateToOn.Checked = true;
                    _cFilterDateFrom.Value = vMinDt.Date;
                    _cFilterDateTo.Value = vMaxDt.Date;
                    _cFilterDateFrom.Enabled = true;
                    _cFilterDateTo.Enabled = true;
                }
                finally
                {
                    _fFiltersPopulating = false;
                }
            }
            catch { }
        }

        /// <summary>
        /// Разбор CHG: либо .NET ticks, либо DateTime.Parse
        /// </summary>
        private bool mTryParseChg(object pValue, bool pIsTicks, out DateTime pResult)
        {
            pResult = DateTime.MinValue;
            if (pValue == null || pValue == DBNull.Value)
                return false;
            string vRaw = pValue.ToString().Trim();
            if (pIsTicks == true)
            {
                long vTicks;
                if (long.TryParse(vRaw, out vTicks) == false)
                    return false;
                try
                {
                    pResult = new DateTime(vTicks);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return DateTime.TryParse(vRaw, out pResult);
        }
        /// <summary>
        /// Описание фактической схемы открытой базы протоколов - разные реальные копии 'protocols.db',
      
        /// </summary>
        private class ProtocolsSchemaInfo
        {
            public string AppNameColumn = "dsiApp";
            public string PclTypNameColumn = "dsiPclTyp";
            /// <summary>Столбец связи 'Pcl' -&gt; 'App' ('lnkApp' в правильной схеме, 'InkApp' - опечатка старой версии)</summary>
            public string AppLinkColumn = "lnkApp";
            /// <summary>Столбец связи 'Pcl' -&gt; 'PclTyp' ('lnkPclTyp', либо опечатка 'InkPclTyp')</summary>
            public string PclTypLinkColumn = "lnkPclTyp";
            /// <summary>Столбец связи 'PclRrd' -&gt; 'Pcl' ('lnkPcl', либо опечатка 'InkPcl')</summary>
            public string PclLinkColumn = "lnkPcl";
            /// <summary>[true] - хост/пользователь читаются прямо из 'Pcl.Hst'/'Pcl.Usr' (текстом)</summary>
            public bool HostUserDirectText = false;
            /// <summary>[true] - хост/пользователь читаются через JOIN на 'Cpu'/'Usr' по 'lnkCpu'/'lnkUsr'</summary>
            public bool HasCpuUsrTables = false;
            public string RrdTypTable = "PclRrdTyp";
            public string RrdTypNameColumn = "dsiPclRrdTyp";
            public string RrdLinkColumn = "lnkPclRrdTyp";
            public string MessageColumn = "Msg";
            /// <summary>[true] - Pcl.CHG хранится как .NET ticks (число), а не как дата-строка</summary>
            public bool ChgIsTicks = false;
        }
        /// <summary>
        /// Определение варианта схемы открытой базы протоколов по фактически существующим таблицам/столбцам
        /// - разные реальные копии 'protocols.db' в проекте создавались разными
        /// версиями логики записи и отличаются именованием.
        /// </summary>
        /// <param name="pTableExists">Проверка существования таблицы по имени</param>
        /// <param name="pColumnExists">Проверка существования столбца в таблице по именам таблицы/столбца</param>
        private ProtocolsSchemaInfo mSchemaDetectCore(Func<string, bool> pTableExists, Func<string, string, bool> pColumnExists)
        {
            ProtocolsSchemaInfo vSchema = new ProtocolsSchemaInfo();

            vSchema.AppNameColumn = pColumnExists("App", "dsiApp") == true ? "dsiApp" : "desApp";
            vSchema.PclTypNameColumn = pColumnExists("PclTyp", "dsiPclTyp") == true ? "dsiPclTyp" : "desPclTyp";

           
            vSchema.AppLinkColumn = pColumnExists("Pcl", "lnkApp") == true ? "lnkApp" : (pColumnExists("Pcl", "InkApp") == true ? "InkApp" : "lnkApp");
            vSchema.PclTypLinkColumn = pColumnExists("Pcl", "lnkPclTyp") == true ? "lnkPclTyp" : (pColumnExists("Pcl", "InkPclTyp") == true ? "InkPclTyp" : "lnkPclTyp");
            vSchema.PclLinkColumn = pColumnExists("PclRrd", "lnkPcl") == true ? "lnkPcl" : (pColumnExists("PclRrd", "InkPcl") == true ? "InkPcl" : "lnkPcl");

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

            vSchema.RrdLinkColumn = pColumnExists("PclRrd", "lnkPclRrdTyp") == true ? "lnkPclRrdTyp" : (pColumnExists("PclRrd", "InkPclRrdTyp") == true ? "InkPclRrdTyp" : "lnkRrdTyp");
            vSchema.MessageColumn = pColumnExists("PclRrd", "Msg") == true ? "Msg" : "Err";

           
            vSchema.ChgIsTicks = false;
            try
            {
                // эвристика: если столбец INTEGER — ticks; если TEXT с длинным числом — тоже ticks
                // точную проверку делает mChgIsTicksDetect через запрос
            }
            catch { }

            return vSchema;
        }
        /// <summary>
        /// 'mSchemaDetect' для активного логгера '_oProtocols' (авто-загрузка при старте формы)
        /// </summary>
        private ProtocolsSchemaInfo mSchemaDetect()
        {
            return mSchemaDetectActive();
        }
        /// <summary>
        /// Схема активной базы (авто dsqProtocols или вручную открытый .db)
        /// </summary>
        private ProtocolsSchemaInfo mSchemaDetectActive()
        {
            ProtocolsSchemaInfo vSchema;
            if (_manualDbMode == true && _oManualDataSource != null)
                vSchema = mSchemaDetectFor(_oManualDataSource);
            else if (_oProtocols != null)
                vSchema = mSchemaDetectCore(_oProtocols.__mTableExists, _oProtocols.__mColumnExists);
            else
                vSchema = new ProtocolsSchemaInfo();
            vSchema.ChgIsTicks = mChgIsTicksDetect();
            return vSchema;
        }
        /// <summary>
        /// 'mSchemaDetect' для стороннего файла '*.db', открытого вручную через 'Файл / Открыть протокол'
        /// </summary>
        private ProtocolsSchemaInfo mSchemaDetectFor(datUnitDataSource pDataSource)
        {
            return mSchemaDetectCore(
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
            if (mHasActiveDatabase() == false)
                return;

            ProtocolsSchemaInfo vSchema = mSchemaDetectActive();

            _fFiltersPopulating = true;
            try
            {
                mFilterComboFill(_cFilterType, "SELECT DISTINCT " + vSchema.PclTypNameColumn + " AS V FROM PclTyp WHERE " + vSchema.PclTypNameColumn + " IS NOT NULL ORDER BY " + vSchema.PclTypNameColumn);
                mFilterComboFill(_cFilterApp, "SELECT DISTINCT " + vSchema.AppNameColumn + " AS V FROM App WHERE " + vSchema.AppNameColumn + " IS NOT NULL ORDER BY " + vSchema.AppNameColumn);
                mFilterComboFill(_cFilterErrorType, "SELECT DISTINCT " + vSchema.RrdTypNameColumn + " AS V FROM " + vSchema.RrdTypTable + " WHERE " + vSchema.RrdTypNameColumn + " IS NOT NULL ORDER BY " + vSchema.RrdTypNameColumn);

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
                   
                    _cFilterUser.Items.Clear();
                    _cFilterUser.Items.Add(FILTERITEMALL);
                    _cFilterUser.SelectedIndex = 0;
                    _cFilterHost.Items.Clear();
                    _cFilterHost.Items.Add(FILTERITEMALL);
                    _cFilterHost.SelectedIndex = 0;
                }

                // Автодополнение названий процедур
                _oProcedureAutoComplete.Clear();
                try
                {
                    System.Data.DataTable vPrc = mQuery("SELECT DISTINCT Prc AS V FROM Pcl WHERE Prc IS NOT NULL AND Prc <> '' ORDER BY Prc");
                    if (vPrc != null)
                    {
                        foreach (System.Data.DataRow vRow in vPrc.Rows)
                        {
                            if (vRow["V"] != System.DBNull.Value)
                                _oProcedureAutoComplete.Add(vRow["V"].ToString());
                        }
                    }
                }
                catch { }
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
                DataTable vTable = mQuery(pQuery);
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
                
            }

            pCombo.SelectedIndex = 0;
        }
        /// <summary>
        /// Построение и выполнение запроса списка протоколов с учётом текущих значений панели фильтров,
        /// отображение результата в левой таблице
        /// </summary>
        private void mProtocolsLoad()
        {
            if (mHasActiveDatabase() == false)
                return;

            ProtocolsSchemaInfo vSchema = mSchemaDetectActive();
            string vWhere = mFiltersWhereClauseBuild(vSchema);

            string vHostColumn = vSchema.HostUserDirectText == true ? "P.Hst AS Hst" : (vSchema.HasCpuUsrTables == true ? "C.dsiCpu AS Hst" : "'' AS Hst");
            string vUserColumn = vSchema.HostUserDirectText == true ? "P.Usr AS Usr" : (vSchema.HasCpuUsrTables == true ? "U.dsiUsr AS Usr" : "'' AS Usr");

            string vQuery = "SELECT P.CLU, P.CHG, A." + vSchema.AppNameColumn + " AS App, P." + vSchema.PclTypLinkColumn + " AS lnkPclTyp, PT." + vSchema.PclTypNameColumn + " AS PclTyp, "
                + vHostColumn + ", P.Prc, " + vUserColumn + " "
                + "FROM Pcl P "
                + "LEFT JOIN App A ON A.CLU = P." + vSchema.AppLinkColumn + " "
                + "LEFT JOIN PclTyp PT ON PT.CLU = P." + vSchema.PclTypLinkColumn + " "
                + (vSchema.HasCpuUsrTables == true ? "LEFT JOIN Cpu C ON C.CLU = P.lnkCpu LEFT JOIN Usr U ON U.CLU = P.lnkUsr " : "")
                + (vWhere.Length > 0 ? "WHERE " + vWhere + " " : "")
                + "ORDER BY P.CHG DESC";

            DataTable vDataTable;
            try
            {
                vDataTable = mQuery(vQuery);
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

            // Поиск по названию процедуры (вхождение подстроки)
            string vProcedure = mFilterTextValue(_cFilterProcedure, "поиск по названию...");
            if (vProcedure.Length > 0)
            {
                string vEscaped = vProcedure.Replace("'", "''");
                vConditions.Add("P.Prc LIKE '%" + vEscaped + "%'");
            }

            // Поиск по описанию ошибок в Msg любой записи протокола
            string vErrorDesc = mFilterTextValue(_cFilterErrorDesc, "текст в Msg...");
            if (vErrorDesc.Length > 0)
            {
                string vEscaped = vErrorDesc.Replace("'", "''");
                vConditions.Add("EXISTS (SELECT 1 FROM PclRrd PR WHERE PR." + pSchema.PclLinkColumn + " = P.CLU AND PR." + pSchema.MessageColumn + " LIKE '%" + vEscaped + "%')");
            }

            // Поиск по решению пользователя (записи типа Answer / "Решение пользователя")
            string vSolution = mFilterTextValue(_cFilterSolution, "поиск в ответах...");
            if (vSolution.Length > 0)
            {
                string vEscaped = vSolution.Replace("'", "''");
                vConditions.Add("EXISTS (SELECT 1 FROM PclRrd PR"
                    + " LEFT JOIN " + pSchema.RrdTypTable + " PRT ON PRT.CLU = PR." + pSchema.RrdLinkColumn
                    + " WHERE PR." + pSchema.PclLinkColumn + " = P.CLU"
                    + " AND (PRT." + pSchema.RrdTypNameColumn + " LIKE '%Решение%' OR PRT." + pSchema.RrdTypNameColumn + " LIKE '%Answer%')"
                    + " AND PR." + pSchema.MessageColumn + " LIKE '%" + vEscaped + "%')");
            }

            mFilterComboConditionAdd(vConditions, _cFilterType, "PT." + pSchema.PclTypNameColumn);
            mFilterComboConditionAdd(vConditions, _cFilterApp, "A." + pSchema.AppNameColumn);

            // Фильтр по виду ошибки (типу записи): протоколы, у которых есть хотя бы одна запись выбранного типа
            if (_cFilterErrorType != null && _cFilterErrorType.SelectedItem != null)
            {
                string vErrType = _cFilterErrorType.SelectedItem.ToString();
                if (vErrType != FILTERITEMALL)
                {
                    string vEscaped = vErrType.Replace("'", "''");
                    vConditions.Add("EXISTS (SELECT 1 FROM PclRrd PR"
                        + " LEFT JOIN " + pSchema.RrdTypTable + " PRT ON PRT.CLU = PR." + pSchema.RrdLinkColumn
                        + " WHERE PR." + pSchema.PclLinkColumn + " = P.CLU"
                        + " AND PRT." + pSchema.RrdTypNameColumn + " = '" + vEscaped + "')");
                }
            }

            if (vUserColumn != null)
                mFilterComboConditionAdd(vConditions, _cFilterUser, vUserColumn);
            if (vHostColumn != null)
                mFilterComboConditionAdd(vConditions, _cFilterHost, vHostColumn);

            if (_cFilterDateFromOn.Checked == true)
            {
                if (pSchema.ChgIsTicks == true)
                    vConditions.Add("CAST(P.CHG AS INTEGER) >= " + _cFilterDateFrom.Value.Date.Ticks.ToString());
                else
                    vConditions.Add("P.CHG >= '" + _cFilterDateFrom.Value.Date.ToString("yyyy-MM-dd 00:00:00") + "'");
            }

            if (_cFilterDateToOn.Checked == true)
            {
                if (pSchema.ChgIsTicks == true)
                    vConditions.Add("CAST(P.CHG AS INTEGER) <= " + _cFilterDateTo.Value.Date.AddDays(1).AddTicks(-1).Ticks.ToString());
                else
                    vConditions.Add("P.CHG <= '" + _cFilterDateTo.Value.Date.ToString("yyyy-MM-dd 23:59:59") + "'");
            }

            return string.Join(" AND ", vConditions);
        }
        /// <summary>
        /// Текст из поля фильтра с учётом placeholder (пустой placeholder не считается фильтром)
        /// </summary>
        private string mFilterTextValue(TextBox pBox, string pPlaceholder)
        {
            if (pBox == null)
                return "";
            string vText = pBox.Text != null ? pBox.Text.Trim() : "";
            if (vText == pPlaceholder)
                return "";
            return vText;
        }
      
        /// <summary>
        /// Цвета панели фильтров из темы elmApplication.__oInterface (COLORS.FormActive / DataBack),
        /// как у elmForm и elmComponent* — без SystemColors.Control (белый/серый фон).
        /// </summary>
        private void mFilterPanelApplyTheme()
        {
            Color vForm = elmApplication.__oInterface.__mColor(COLORS.FormActive);
            Color vDataBack = elmApplication.__oInterface.__mColor(COLORS.DataBack);
            Color vText = elmApplication.__oInterface.__mColor(COLORS.Text);

            _cPanelFilters.BackColor = vForm;
            if (_cLeftHost != null)
                _cLeftHost.BackColor = vForm;

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
                else if (vCtrl is TextBox || vCtrl is ComboBox || vCtrl is DateTimePicker || vCtrl is Button)
                {
                    if (vCtrl is Button)
                    {
                        vCtrl.BackColor = vForm;
                        (vCtrl as Button).UseVisualStyleBackColor = false;
                    }
                    else
                        vCtrl.BackColor = vDataBack;
                }
            }

            _cFilterProcedure.BackColor = vDataBack;
            _cFilterErrorDesc.BackColor = vDataBack;
            _cFilterSolution.BackColor = vDataBack;
            _cFilterType.BackColor = vDataBack;
            _cFilterErrorType.BackColor = vDataBack;
            _cFilterApp.BackColor = vDataBack;
            _cFilterUser.BackColor = vDataBack;
            _cFilterHost.BackColor = vDataBack;
            _cFilterDateFrom.BackColor = vDataBack;
            _cFilterDateTo.BackColor = vDataBack;
            _cFilterDateFromOn.BackColor = vForm;
            _cFilterDateToOn.BackColor = vForm;
            _cFilterClear.BackColor = vForm;
            _cFilterClear.UseVisualStyleBackColor = false;
        }

        private void mAddFilterLabel(TableLayoutPanel pPanel, string pText, int pColumn, int pRow)
        {
            Label vLabel = new Label();
            vLabel.Text = pText;
            vLabel.Dock = DockStyle.Fill;
            vLabel.TextAlign = ContentAlignment.MiddleLeft;
            vLabel.AutoSize = false;
            pPanel.Controls.Add(vLabel, pColumn, pRow);
        }

        private void mSetPlaceholder(TextBox pBox, string pPlaceholder)
        {
            pBox.ForeColor = SystemColors.GrayText;
            pBox.Text = pPlaceholder;
            pBox.GotFocus += (s, e) =>
            {
                if (pBox.Text == pPlaceholder)
                {
                    pBox.Text = "";
                    pBox.ForeColor = SystemColors.WindowText;
                }
            };
            pBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(pBox.Text) == true)
                {
                    pBox.ForeColor = SystemColors.GrayText;
                    pBox.Text = pPlaceholder;
                }
            };
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
            if (_fFiltersPopulating == true || mHasActiveDatabase() == false)
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
                _cFilterProcedure.ForeColor = SystemColors.GrayText;
                _cFilterProcedure.Text = "поиск по названию...";
                _cFilterErrorDesc.ForeColor = SystemColors.GrayText;
                _cFilterErrorDesc.Text = "текст в Msg...";
                _cFilterSolution.ForeColor = SystemColors.GrayText;
                _cFilterSolution.Text = "поиск в ответах...";
                if (_cFilterType.Items.Count > 0) _cFilterType.SelectedIndex = 0;
                if (_cFilterErrorType.Items.Count > 0) _cFilterErrorType.SelectedIndex = 0;
                if (_cFilterApp.Items.Count > 0) _cFilterApp.SelectedIndex = 0;
                if (_cFilterUser.Items.Count > 0) _cFilterUser.SelectedIndex = 0;
                if (_cFilterHost.Items.Count > 0) _cFilterHost.SelectedIndex = 0;
                _cFilterDateFromOn.Checked = false;
                _cFilterDateToOn.Checked = false;
                _cFilterDateFrom.Value = DateTime.Today.AddDays(-30);
                _cFilterDateTo.Value = DateTime.Today;
                _cFilterDateFrom.Enabled = false;
                _cFilterDateTo.Enabled = false;
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
            _cFilterProcedure.Enabled = pEnabled;
            _cFilterErrorDesc.Enabled = pEnabled;
            _cFilterSolution.Enabled = pEnabled;
            _cFilterType.Enabled = pEnabled;
            _cFilterErrorType.Enabled = pEnabled;
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

                string vChgRaw = vSourceRow["CHG"] != DBNull.Value ? vSourceRow["CHG"].ToString() : "";
                DateTime vChgDateTime;
                long vChgTicks;
                if (long.TryParse(vChgRaw, out vChgTicks) == true && vChgTicks > 600000000000000000L)
                {
                    try { vRow["CHG"] = new DateTime(vChgTicks).ToString("yyyy-MM-dd HH:mm:ss"); }
                    catch { vRow["CHG"] = vChgRaw; }
                }
                else if (DateTime.TryParse(vChgRaw, out vChgDateTime) == true)
                    vRow["CHG"] = vChgDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                else
                    vRow["CHG"] = vChgRaw;

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

            string vQuery = "SELECT PR." + vSchema.PclLinkColumn + " AS Protocol, PR.CLU AS \"Key\", PRT." + vSchema.RrdTypNameColumn + " AS Type, PR." + vSchema.MessageColumn + " AS Message, PR.Tck AS Time "
                + "FROM PclRrd PR "
                + "LEFT JOIN " + vSchema.RrdTypTable + " PRT ON PRT.CLU = PR." + vSchema.RrdLinkColumn + " "
                + "WHERE PR." + vSchema.PclLinkColumn + " = " + pProtocolClue.ToString();

            DataTable vRecords;
            try
            {
                vRecords = mQuery(vQuery);
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

            datUnitDataSource vDataSource = new dsqDataSourceSqliteWithProtocol();
            vDataSource.__fDatabasePath = Path.GetDirectoryName(vFilePath);
            vDataSource.__fDatabaseName = Path.GetFileName(vFilePath);

            
            _manualDbMode = true;
            _oManualDataSource = vDataSource;

            dsqProtocols.__oViewing_ = vDataSource;

            mFiltersEnable(true);
            mFiltersPopulate();
            mAdjustDateFilterToData();
            mProtocolsLoad();

            _cLabelStatus.Text = "Открыта база: " + vFilePath;
        }
        private void mMenuFileClose_Click(object sender, EventArgs e)
        {
            _manualDbMode = false;
            _oManualDataSource = null;
            _oProtocols = null;

            dsqProtocols.__oViewing_ = null;

            mFilterClear_Click(sender, e);
            mClearProtocolsView();
            mFiltersEnable(true);

            _cLabelStatus.Text = "Файл закрыт. База данных не открыта.";
        }
        /// <summary>
        /// Открытие Form 3 (совмещённый просмотр) - см. примечание к '_cMenuView' о том, почему эта форма
        /// иначе была недостижима из работающего приложения
        /// </summary>
        private void mMenuViewCombined_Click(object sender, EventArgs e)
        {
            cspFormCombinedViewer vFormCombined = new cspFormCombinedViewer();
            vFormCombined.ShowDialog();
        }

        /// <summary>
        /// Очистка обеих таблиц на форме (список протоколов и записи)
        /// </summary>
        private void mClearProtocolsView()
        {
            _oDataTableProtocols.Rows.Clear();
            _cAreaProtocols.__fDataSource_ = _oDataTableProtocols;
            _cAreaProtocols.__mGridRefresh();

            _oDataTableProtocolsRecord.Rows.Clear();
            _cAreaProtocolsRecords.__fDataSource_ = _oDataTableProtocolsRecord;
            _cAreaProtocolsRecords.__mGridRefresh();
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
        /// Меню 'Вид' - навигация к Form 3 (совмещённый просмотр)
        /// </summary>
        protected elmComponentMenuItem _cMenuView = new elmComponentMenuItem();
        /// <summary>
        /// Пункт меню 'Вид / Совмещённый просмотр'
        /// </summary>
        protected elmComponentMenuItem _cMenuViewCombined = new elmComponentMenuItem();

        /// <summary>
        /// Контейнер левой панели (фильтры + список протоколов)
        /// </summary>
        protected TableLayoutPanel _cLeftHost = new TableLayoutPanel();
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

        /// <summary>Поиск по названию процедуры (Pcl.Prc) с автодополнением</summary>
        protected TextBox _cFilterProcedure = new TextBox();
        /// <summary>Поиск по описанию ошибок (PclRrd.Msg)</summary>
        protected TextBox _cFilterErrorDesc = new TextBox();
        /// <summary>Поиск по решению пользователя (записи типа Answer)</summary>
        protected TextBox _cFilterSolution = new TextBox();
        /// <summary>Вид протокола (PclTyp.dsiPclTyp)</summary>
        protected ComboBox _cFilterType = new ComboBox();
        /// <summary>Вид ошибки / тип записи (PclRrdTyp)</summary>
        protected ComboBox _cFilterErrorType = new ComboBox();
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
        /// <summary>Источник автодополнения для названий процедур</summary>
        private AutoCompleteStringCollection _oProcedureAutoComplete = new AutoCompleteStringCollection();
        /// <summary>[true] - идёт программное заполнение фильтров, пользовательские обработчики должны молчать</summary>
        private bool _fFiltersPopulating = false;

        #endregion Фильтры

        #region - Внутренние

        /// <summary>
        /// Данные протоколов из 'dsqProtocols' (общий экземпляр-логгер приложения)
        /// </summary>
        private dsqProtocols _oProtocols;
        /// <summary>
        /// [true] - открыт сторонний '.db' вручную через 'Файл / Открыть протокол'
        /// </summary>
        private bool _manualDbMode = false;
        /// <summary>
        /// Источник данных вручную открытого .db (фильтры и загрузка идут через mQuery)
        /// </summary>
        private datUnitDataSource _oManualDataSource = null;
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