using nlApplication;
using nlCsProtocols;
using nlData;
using nlDataSourceSqlite;
using nlElements;
using System;
using System.Collections;
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

            _cSplitter.Panel1.Controls.Add(_cAreaProtocols);
            _cSplitter.Panel2.Controls.Add(_cAreaProtocolsRecords);

            _cMenuFile.DropDownItems.Add(_cMenuFileOpen);
            _cMenuFile.DropDownItems.Add(_cMenuFileClose);

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
                _cAreaProtocols.__fDataSource_ = _oDataTableProtocols;

                _cAreaProtocols.__fHeaderVisible_ = true;
                _cAreaProtocols.__fHeaderCaption_ = "Протоколы";
                //_cAreaProtocols.__fHeaderImage_ = global::nlResourcesImages.Properties.Resources._Books_b32;
                _cAreaProtocols.__eGridCellEnter += mAreaProtocols_GridCellEnter;

                _cAreaProtocols.__mColumnAdd("Протокол", "Ключ протокола", "CLU", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mColumnAdd("Время", "Время создания протокола", "CHG", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mColumnAdd("Приложение", "Название приложения", "desApp", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mColumnAdd("Вид", "Вид протокола", "lnkPclTyp", true, false, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mColumnAdd("Вид", "Вид протокола", "desPclTyp", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mColumnAdd("Изображение", "Наличие PrintScreen", "FilPrnScr", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mColumnAdd("Хост", "Рабочая станиция на которой возникло событие", "Hst", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mColumnAdd("Процедура", "Название процедуры в которой возникло событие", "Prc", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mColumnAdd("Пользователь", "Пользователь приложения у которого возникло событие", "Usr", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocols.__mGridBuild();
            }
            // _cAreaProtocolsRecords
            {
                _cAreaProtocolsRecords.__fDataSource_ = _oDataTableProtocolsRecord;

                _cAreaProtocolsRecords.__fHeaderVisible_ = true;
                _cAreaProtocolsRecords.__fHeaderCaption_ = "Записи в протоколах";
                //_cAreaProtocolsRecords.__fHeaderImage_ = global::nlResourcesImages.Properties.Resources._BookOpen_b32C;

                _cAreaProtocolsRecords.__mColumnAdd("Протокол", "Ключ протокола", "lnkPcl", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocolsRecords.__mColumnAdd("Ключ", "Ключ записи в протоколе", "CLU", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocolsRecords.__mColumnAdd("Вид", "Вид записи в протоколе", "desRrdTyp", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocolsRecords.__mColumnAdd("Сообщение", "Сообщение", "Msg", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocolsRecords.__mColumnAdd("Время", "Время затраченное на операцию", "Msg", true, true, "DataGridViewTextBoxColumn");
                _cAreaProtocolsRecords.__mGridBuild();
            }

            _oDataTableProtocols.Columns.Add("CHG", typeof(String));
            _oDataTableProtocols.Columns.Add("App", typeof(String));
            _oDataTableProtocols.Columns.Add("PclTyp", typeof(String));
            _oDataTableProtocols.Columns.Add("FilPrnScr", typeof(String));
            _oDataTableProtocols.Columns.Add("Hst", typeof(String));
            _oDataTableProtocols.Columns.Add("Pcs", typeof(String));
            _oDataTableProtocols.Columns.Add("Usr", typeof(Double));

            _oDataTableProtocolsRecord.Columns.Add("lnkPcl", typeof(Int64));
            _oDataTableProtocolsRecord.Columns.Add("RrdTyp", typeof(String));
            _oDataTableProtocolsRecord.Columns.Add("Msg", typeof(String));
            _oDataTableProtocolsRecord.Columns.Add("Tim", typeof(String));

            #endregion Настройки компонентов

            ResumeLayout();

            Type vType = this.GetType();
            Name = vType.Name;
            _fClassNameFull = vType.FullName + ".";

            return;
        }

        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();

            
            //gridPtcl[3, e.RowIndex].Style.BackColor = Color.White;
            //if (gridPtcl[3, e.RowIndex].Value != null)
            //{
            //    if (gridPtcl[3, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Types), _Enumerations.Protocol_Types.Application_Error.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 255, 230, 230);
            //    if (gridPtcl[3, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Types), _Enumerations.Protocol_Types.Data_Error.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 255, 190, 190);
            //    if (gridPtcl[3, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Types), _Enumerations.Protocol_Types.Device_Error.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 255, 170, 170);
            //    if (gridPtcl[3, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Types), _Enumerations.Protocol_Types.Environment_Error.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 255, 150, 150);

            //    if (gridPtcl[3, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Types), _Enumerations.Protocol_Types.Application_Event.ToString())) //  gridPtcl[3, e.RowIndex].Style.BackColor = Color.FromArgb(255, 230, 255, 230);
            //        e.CellStyle.BackColor = Color.FromArgb(255, 230, 255, 230);
            //    if (gridPtcl[3, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Types), _Enumerations.Protocol_Types.Data_Event.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 210, 255, 210);
            //    if (gridPtcl[3, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Types), _Enumerations.Protocol_Types.Device_Event.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 190, 255, 190);
            //    if (gridPtcl[3, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Types), _Enumerations.Protocol_Types.System_Event.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 170, 255, 170);
            //    if (gridPtcl[3, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Types), _Enumerations.Protocol_Types.User_Event.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 150, 255, 150);

            //    if (gridPtcl[3, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Types), _Enumerations.Protocol_Types.Message.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 230, 230, 255);
            //    if (gridPtcl[3, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Types), _Enumerations.Protocol_Types.Operations.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 210, 210, 255);
            //    if (gridPtcl[3, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Types), _Enumerations.Protocol_Types.Other.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 190, 190, 255);
            //}

            //gridList[1, e.RowIndex].Style.BackColor = Color.White;
            //if (gridList[1, e.RowIndex].Value != null)
            //{
            //    if (gridList[1, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Record_Types), _Enumerations.Protocol_Record_Types.Answer.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 255, 230, 230);
            //    if (gridList[1, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Record_Types), _Enumerations.Protocol_Record_Types.Error_Message.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 255, 190, 190);
            //    if (gridList[1, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Record_Types), _Enumerations.Protocol_Record_Types.Error_Number.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 255, 170, 170);
            //    if (gridList[1, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Record_Types), _Enumerations.Protocol_Record_Types.Image_File.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 255, 150, 150);
            //    if (gridList[1, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Record_Types), _Enumerations.Protocol_Record_Types.Line_Content.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 230, 255, 230);
            //    if (gridList[1, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Record_Types), _Enumerations.Protocol_Record_Types.Line_Number.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 210, 255, 210);
            //    if (gridList[1, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Record_Types), _Enumerations.Protocol_Record_Types.Message.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 190, 255, 190);
            //    if (gridList[1, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Record_Types), _Enumerations.Protocol_Record_Types.Other.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 170, 255, 170);
            //    if (gridList[1, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Record_Types), _Enumerations.Protocol_Record_Types.Procedure.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 150, 255, 150);
            //    if (gridList[1, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Record_Types), _Enumerations.Protocol_Record_Types.Seconds.ToString()))
            //        e.CellStyle.BackColor = Color.FromArgb(255, 230, 230, 255);
            //}
        }
        private void mAreaProtocols_GridCellEnter(object sender, EventArgs e)
        {
            if (_cAreaProtocols.__fCurrentRow_ != null)
            {
                for (int i = 0; i < _cAreaProtocolsRecords.__fRowsCount_ - 1; i++)
                {
                    if (_cAreaProtocolsRecords.__mValue(0, i).ToString() == _cAreaProtocols.__mValue(0, _cAreaProtocols.__fCurrentRow_.Index).ToString())
                    {
                        _cAreaProtocolsRecords.__mSelect(0, i, true);
                        _cAreaProtocolsRecords.__mCurrentCell(0, i);
                    }
                    else
                    {
                        _cAreaProtocolsRecords.__mSelect(0, i, false);
                    }
                }
            }

            //if (gridPtcl[3, e.RowIndex].Value.ToString().Trim() == applEnumAttributes.Attribute(typeof(_Enumerations.Protocol_Types), _Enumerations.Protocol_Types.Application_Error.ToString()))
            if (_cAreaProtocols.__fCurrentRow_.Cells["lnkPclTyp"].Value.ToString() == "1")
                _cAreaProtocols.__mCellStyle(Color.FromArgb(255, 255, 230, 230));

            if (_cAreaProtocols.__fCurrentRow_.Cells["lnkPclTyp"].Value.ToString() == "2")
                _cAreaProtocols.__mCellStyle(Color.FromArgb(255, 255, 170, 170));
            if (_cAreaProtocols.__fCurrentRow_.Cells["lnkPclTyp"].Value.ToString() == "3")
                _cAreaProtocols.__mCellStyle(Color.FromArgb(255, 255, 150, 150));
            if (_cAreaProtocols.__fCurrentRow_.Cells["desPclTyp"].Value.ToString() == "4")
                _cAreaProtocols.__mCellStyle(Color.FromArgb(255, 230, 255, 230));
            if (_cAreaProtocols.__fCurrentRow_.Cells["lnkPclTyp"].Value.ToString() == "5")
                _cAreaProtocols.__mCellStyle(Color.FromArgb(255, 210, 255, 210));
            if (_cAreaProtocols.__fCurrentRow_.Cells["lnkPclTyp"].Value.ToString() == "6")
                _cAreaProtocols.__mCellStyle(Color.FromArgb(255, 190, 255, 190));
            if (_cAreaProtocols.__fCurrentRow_.Cells["lnkPclTyp"].Value.ToString() == "7")
                _cAreaProtocols.__mCellStyle(Color.FromArgb(255, 170, 255, 170));
            if (_cAreaProtocols.__fCurrentRow_.Cells["lnkPclTyp"].Value.ToString() == "8")
                _cAreaProtocols.__mCellStyle(Color.FromArgb(255, 150, 255, 150));
        }

        #endregion Объект

        #region - События

        /// <summary>
        /// Выполняется при выборе пункта меню 'Файл / Открыть'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMenuFileOpen_Click(object sender, EventArgs e)
        {
            string vFilePath = ""; // 

            OpenFileDialog vOpenFileDialog = new OpenFileDialog();
            vOpenFileDialog.AddExtension = true;
            vOpenFileDialog.AutoUpgradeEnabled = true;
            vOpenFileDialog.CheckFileExists = true;
            vOpenFileDialog.CheckPathExists = true;
            vOpenFileDialog.InitialDirectory = "f:\\CODING\\LuNA\\APPLICATIONs\\CsProtocols\\CsProtocols\\bin\\Debug\\Test_Data\\";
            if (vOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                vFilePath = vOpenFileDialog.FileName;
                datUnitDataSource vDataSource = new dsqDataSourceSqlite();
                vDataSource.__fDatabasePath = Path.GetDirectoryName(vFilePath);
                vDataSource.__fDatabaseName = Path.GetFileName(vFilePath);
                /// Чтение приложений
                {
                    string vQueryApp = "Select A.* "
                        + " From App as A";
                    DataTable vDataTableApp = vDataSource.__mSqlQuery(vQueryApp);
                    foreach (DataRow vDataRow in vDataTableApp.Rows)
                    {
                        string vApplicationName = vDataRow["desApp"].ToString();
                        foreach (DataRow vDataRowApp in _oDataTaleApplications.Rows)
                        {
                            bool vFound = false;
                            if (vDataRowApp["desApp"].ToString() == vApplicationName)
                            {
                                vFound = true;
                            }
                            if (vFound == false)
                            {
                                DataRow vDataRowAppNew = _oDataTaleApplications.NewRow();
                                vDataRowAppNew["desApp"] = vDataRowApp["desApp"];
                                vDataRowAppNew["dpnApp"] = vDataRowApp["dpnApp"];
                                vDataRowAppNew["Pfx"] = vDataRowApp["Pfx"];

                                _oDataTaleApplications.Rows.Add(vDataRowAppNew);
                            }
                        }
                    }
                    _cAreaProtocols.__fDataSource_ = _oDataTaleApplications;
                }
                /// Чтение протоколов
                {
                    string vQueryPcl = "Select P.* "
                        + ", A.desApp"
                        + ", PT.desPclTyp"
                        + " From Pcl as P"
                        + " Left Join App as A On A.CLU = P.lnkApp"
                        + " Left Join PclTyp as PT On PT.CLU = P.lnkPclTyp";

                    DataTable vDataTablePcl = vDataSource.__mSqlQuery(vQueryPcl);
                    _cAreaProtocols.__fDataSource_ = vDataTablePcl;
                    foreach (DataRow vDataRow in vDataTablePcl.Rows)
                    {
                        // vDataRow["CHG"] = appTypeDateTime.__mIntervalToString(new TimeSpan(Convert.ToInt64(vDataRow["CHG"]))).ToString();
                        vDataRow["CHG"] = new DateTime(Convert.ToInt64(vDataRow["CHG"])).ToString();
                    }
                }
                /// Чтение записей протоколов
                {
                    string vQueryPclRrd = "Select PR.*"
                        + ", RT.desRrdTyp"
                        + " From PclRrd as PR"
                        + " Left Join RrdTyp as RT On RT.CLU = PR.lnkRrdTyp";

                    DataTable vDataTablePclRrd = vDataSource.__mSqlQuery(vQueryPclRrd);
                    _cAreaProtocolsRecords.__fDataSource_ = vDataTablePclRrd;
                }
            }

        }
        /// <summary>
        /// Выполняется при выборе пункта меню 'Файл / Закрыть'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMenuFileClose_Click(object sender, EventArgs e)
        {

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
        /// Пункт иеню 'Файл / Открыть протокол'
        /// </summary>
        protected elmComponentMenuItem _cMenuFileOpen = new elmComponentMenuItem();
        /// <summary>
        /// Пункт иеню 'Файл / Закрыть протоколы'
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
        /// Таблица с приложениями
        /// </summary>
        protected DataTable _oDataTaleApplications = new DataTable();
        /// <summary>
        /// Таблица с протоколами
        /// </summary>
        protected DataTable _oDataTableProtocols = new DataTable();
        /// <summary>
        /// Таблица с записями в протоколе
        /// </summary>
        protected DataTable _oDataTableProtocolsRecord = new DataTable();

        #endregion Компоненты
        #region - Внутренние

        /// <summary>
        /// Полное название класса 
        /// </summary>
        protected string _fClassNameFull = "";
        /// <summary>
        /// Таблица со списком исправленнных полей
        /// </summary>
        protected DataTable _fTableChanges = new DataTable("ChangesValue");
        /// <summary>
        /// Список ошибок при выполнении триггеров
        /// </summary>
        protected ArrayList _fTriggerErrorsDescriptions = new ArrayList();

        #endregion Внутренние

        #region - Служебные

        /// <summary>
        /// Полный путь к узлу дерева
        /// </summary>
        private string fTreeFullName = "";

        #endregion Служебные 

        #endregion ПОЛЯ    

    }
}
