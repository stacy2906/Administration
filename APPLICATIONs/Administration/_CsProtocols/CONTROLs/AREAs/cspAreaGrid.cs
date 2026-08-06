using nlElements;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace nlCsProtocols
{
    /// <summary>
    /// Файл cspAreaGrid.cs
    /// </summary>
    /// <remarks>Класс табличной области</remarks>
    public class cspAreaGrid : elmComponentSplitter
    {
        #region = МЕТОДЫ

        #region - Объекты

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            #region /// Размещение компонентов

            Panel1.Controls.Add(_cHeaderPicture);
            Panel1.Controls.Add(_cHeaderLabel);
            Panel2.Controls.Add(_cToolBar);
            _cToolBar.Items.Add(_cButtonColumns);
            _cToolBar.Items.Add(_cButtonOperations);

            Panel2.Controls.Add(_cGrid);
            Panel2.Controls.SetChildIndex(_cGrid, 0);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            BorderStyle = BorderStyle.Fixed3D;
            Dock = DockStyle.Fill;
            IsSplitterFixed = true;
            FixedPanel = FixedPanel.Panel1;
            Orientation = Orientation.Horizontal;
            TabStop = false;
            Panel1Collapsed = true;

            // _cHeaderPicture
            {
                _cHeaderPicture.BorderStyle = BorderStyle.Fixed3D;
                _cHeaderPicture.Location = new Point(elmInterface.__fIntervalHorizontal, elmInterface.__fIntervalVertical);
                _cHeaderPicture.Size = new Size(36, 36);
            }
            // _cHeaderLabel
            {
                _cHeaderLabel.Location = new Point(_cHeaderPicture.Left + _cHeaderPicture.Width + elmInterface.__fIntervalHorizontal, _cHeaderPicture.Height / 2);
                _cHeaderLabel.__fCaption_ = "Название области";
                _cHeaderLabel.__fLabelType_ = LABELTYPES.Title;
            }
            // __cButtonOperations
            {
                _cButtonOperations.Alignment = ToolStripItemAlignment.Right;
                //_cButtonOperations.DropDownOpened += mButtonDropDownOpened;
                _cButtonOperations.Image = global::nlResourcesImages.Properties.Resources._PageGear_y32;
                _cButtonOperations.ToolTipText = "[ Ctrl + O ] " + elmApplication.__oTunes.__mTranslate("Операции");
                {
                    //_cButtonOperationsAccess.Click += mButtonOperationsAccess_Click;
                    //_cButtonOperationsAccess.Image = global::nlResourcesImages.Properties.Resources._PeopleEdit_b16;
                    //_cButtonOperationsAccess.__fCaption_ = "Определение прав пользователей";
                }
            }
            // _cButtonColumns
            {
                _cButtonColumns.Alignment = ToolStripItemAlignment.Right;
                //_cButtonColumns.DropDownOpened += mButtonDropDownOpened;
                //_cButtonColumns.Image = global::nlResourcesImages.Properties.Resources._TableColumn_b32C;
                _cButtonColumns.ToolTipText = "[ F12 ] " + elmApplication.__oTunes.__mTranslate("Видимость колонок");
                //_cButtonColumns.__eMouseClickRight += mButtonColumns_eMouseClickRight;
            }
            // _cGrid
            {
                _cGrid.Dock = DockStyle.Fill;
                _cGrid.CellEnter += mGrid_CellEnter;
            }
            
            #endregion Настройка компонентов

            ResumeLayout();

            Type vType = this.GetType();
            Name = vType.Name;
            _fClassNameFull = vType.FullName + ".";

            return;
        }

        /// <summary>
        /// Выполняется при выборе ячейки сетки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (__eGridCellEnter != null)
            {
                __eGridCellEnter(this, new EventArgs());
            }
        }

        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();
            SplitterDistance = _cHeaderPicture.Top + _cHeaderPicture.Height + elmInterface.__fIntervalVertical * 2;
        }

        #endregion Объекты

        #region - Процедуры

        public void __mCellStyle(Color pColor)
        {
            foreach(DataGridViewCell vDataViewGridCell in _cGrid.CurrentRow.Cells)
                vDataViewGridCell.Style.BackColor = pColor;
        }
        public void __mCurrentCell(int pColumnNumber, int pRowNumber)
        {
            _cGrid.CurrentCell = _cGrid[pColumnNumber, pRowNumber];
        }
        /// <summary>
        /// Добавление колонки
        /// </summary>
        /// <param name="pCaption">Заголовок колонки</param>
        /// <param name="pFieldName">Название поля</param>
        /// <param name="pReadOnly">Атрибут "Только чтение"</param>
        /// <param name="pVisible">Видимость колонки</param>
        /// <param name="pType">Вид колонки</param>
        /// <returns>[true] - Колонка добавлена, иначе - [false]</returns>
        public bool __mColumnAdd(string pCaption, string pPrompt, string pFieldName, bool pReadOnly, bool pVisible, nlElements.DATAGRIDCOLUMNTYPE pType)
        {
            return _cGrid.__mColumnAdd(pCaption, pPrompt, pFieldName, pReadOnly, pVisible, pType);
        }
        /// <summary>
        /// Добавление колонок в сетку
        /// </summary>
        /// <returns>[true] - колонки добавлены, иначе - [false]</returns>
        public bool __mGridBuild()
        {
            bool vReturn = _cGrid.__mColumnsBuild();
            //mMenuFieldFill();
            return vReturn;
        }
        public void __mSelect(int pColumnNumber, int pRowNumber, bool pValue)
        {
            _cGrid[pColumnNumber, pRowNumber].Selected = pValue;
        }
        public object __mValue(int pColumnNumber, int pRowNumber)
        {
            return _cGrid[pColumnNumber, pRowNumber].Value;
        }

        #endregion Процедуры


        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Полоса инструментов
        /// </summary>
        protected elmComponentToolbar _cToolBar = new elmComponentToolbar();
        /// <summary>
        /// Кнопка 'Помощь'
        /// </summary>
        protected elmComponentToolbarButton _cButtonOperations = new elmComponentToolbarButton();
        /// <summary>
        /// Кнопка 'Отладка'
        /// </summary>
        protected elmComponentToolbarButton _cButtonColumns = new elmComponentToolbarButton();
        /// <summary>
        /// Изображение в заголовке области
        /// </summary>
        protected elmComponentPicture _cHeaderPicture = new elmComponentPicture();
        /// <summary>
        /// Заголовок названия области
        /// </summary>
        protected elmComponentLabel _cHeaderLabel = new elmComponentLabel();
        /// <summary>
        /// Сетка для отбора данных
        /// </summary>
        protected elmComponentGrid _cGrid = new elmComponentGrid();

        #endregion Компоненты

        #region - Служебные

        /// <summary>
        /// Текст заголовка области
        /// </summary>
        private string fHeaderText = "";

        #endregion Служебные  
        
        #region - Внутренние
        /// <summary>
        /// Полное название класса 
        /// </summary>
        protected string _fClassNameFull = "";

        #endregion Внутренние

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Изображение-логотип области 
        /// </summary>
        public Image __fHeaderImage_
        {
            set { _cHeaderPicture.Image = value; }
        }
        /// <summary>
        /// Текст заголовка области
        /// </summary>
        /// <remarks>Выполняется перевод на язык интерфейса. При чтении возвращается не переведенный текст</remarks>
        public string __fHeaderCaption_
        {
            get { return fHeaderText; }
            set
            {
                fHeaderText = value;
                _cHeaderLabel.__fCaption_ = elmApplication.__oTunes.__mTranslate(value);
            }
        }
        /// <summary>
        /// Видимость заголовка
        /// </summary>
        public bool __fHeaderVisible_
        {
            get { return !Panel1Collapsed; }
            set
            {
                Panel1Collapsed = !value;
            }
        }

        /// <summary>
        /// Источник данных
        /// </summary>
        public DataTable __fDataSource_
        {
            set 
            { 
                _cGrid.DataSource = value; 
                _cGrid.Refresh();
            }
        }

        public DataGridViewRow __fCurrentRow_
        {
            get { return _cGrid.CurrentRow; }
        }
        public int __fRowsCount_
        {
            get { return _cGrid.Rows.Count; }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при клике левой кнопки мыши по компоненту
        /// </summary>
        public event EventHandler __eGridCellEnter;
        /// <summary>
        /// Возникает при клике левой кнопки мыши по компоненту
        /// </summary>
        public event EventHandler __eGridRowEnter;

        #endregion СОБЫТИЯ
    }
}
