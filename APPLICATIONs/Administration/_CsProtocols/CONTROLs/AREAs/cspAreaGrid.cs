using nlElements;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Data;

namespace nlCsProtocols
{
    public class cspAreaGrid : elmComponentSplitter
    {
        #region = МЕТОДЫ

        #region - Объекты

        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            Panel1.Controls.Add(_cHeaderPicture);
            Panel1.Controls.Add(_cHeaderLabel);
            Panel2.Controls.Add(_cToolBar);
            _cToolBar.Items.Add(_cButtonColumns);
            _cToolBar.Items.Add(_cButtonOperations);

            Panel2.Controls.Add(_cGrid);
            Panel2.Controls.SetChildIndex(_cGrid, 0);

            BorderStyle = BorderStyle.Fixed3D;
            Dock = DockStyle.Fill;
            IsSplitterFixed = true;
            FixedPanel = FixedPanel.Panel1;
            Orientation = Orientation.Horizontal;
            TabStop = false;
            Panel1Collapsed = true;

            _cHeaderPicture.BorderStyle = BorderStyle.Fixed3D;
            _cHeaderPicture.Location = new Point(elmInterface.__fIntervalHorizontal, elmInterface.__fIntervalVertical);
            _cHeaderPicture.Size = new Size(36, 36);

            _cHeaderLabel.Location = new Point(_cHeaderPicture.Left + _cHeaderPicture.Width + elmInterface.__fIntervalHorizontal, _cHeaderPicture.Height / 2);
            _cHeaderLabel.__fCaption_ = "Название области";
            _cHeaderLabel.__fLabelType_ = LABELTYPES.Title;

            _cButtonOperations.Alignment = ToolStripItemAlignment.Right;
            _cButtonOperations.ToolTipText = "[ Ctrl + O ] " + elmApplication.__oTunes.__mTranslate("Операции");

            _cButtonColumns.Alignment = ToolStripItemAlignment.Right;
            _cButtonColumns.ToolTipText = "[ F12 ] " + elmApplication.__oTunes.__mTranslate("Видимость колонок");

            _cGrid.Dock = DockStyle.Fill;
            _cGrid.CellEnter += mGrid_CellEnter;

            ResumeLayout();

            Type vType = this.GetType();
            Name = vType.Name;

            return;
        }

        private void mGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (__eGridCellEnter != null)
            {
                __eGridCellEnter(this, new EventArgs());
            }
        }

        public void _mObjectPresentation()
        {
            SplitterDistance = _cHeaderPicture.Top + _cHeaderPicture.Height + elmInterface.__fIntervalVertical * 2;
        }

        #endregion

        #region - Процедуры

        public void __mCellStyle(Color pColor)
        {
            if (_cGrid != null && _cGrid.CurrentRow != null)
            {
                foreach (DataGridViewCell vDataViewGridCell in _cGrid.CurrentRow.Cells)
                    vDataViewGridCell.Style.BackColor = pColor;
            }
        }

        public void __mCellStyle(Color pColor, int pRowIndex)
        {
            if (_cGrid != null && pRowIndex >= 0 && pRowIndex < _cGrid.Rows.Count)
            {
                foreach (DataGridViewCell vDataViewGridCell in _cGrid.Rows[pRowIndex].Cells)
                    vDataViewGridCell.Style.BackColor = pColor;
            }
        }

        public void __mCurrentCell(int pColumnNumber, int pRowNumber)
        {
            if (_cGrid == null)
                return;

            if (pRowNumber >= 0 && pRowNumber < _cGrid.Rows.Count &&
                pColumnNumber >= 0 && pColumnNumber < _cGrid.Columns.Count)
            {
                _cGrid.CurrentCell = _cGrid[pColumnNumber, pRowNumber];
            }
        }

        public bool __mColumnAdd(string pCaption, string pPrompt, string pFieldName, bool pReadOnly, bool pVisible, DATAGRIDCOLUMNTYPE pType)
        {
            if (_cGrid == null)
                return false;
            return _cGrid.__mColumnAdd(pCaption, pPrompt, pFieldName, pReadOnly, pVisible, pType);
        }

        public bool __mColumnAdd(string pCaption, string pPrompt, string pFieldName, bool pReadOnly, bool pVisible, string pType)
        {
            if (_cGrid == null)
                return false;

            DATAGRIDCOLUMNTYPE vType;
            if (Enum.TryParse(pType, out vType))
                return _cGrid.__mColumnAdd(pCaption, pPrompt, pFieldName, pReadOnly, pVisible, vType);
            else
                return _cGrid.__mColumnAdd(pCaption, pPrompt, pFieldName, pReadOnly, pVisible, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
        }

        public bool __mGridBuild()
        {
            if (_cGrid == null)
                return false;
            return _cGrid.__mColumnsBuild();
        }

        public void __mGridRefresh()
        {
            if (_cGrid != null)
                _cGrid.Refresh();
        }

        public void __mSelect(int pColumnNumber, int pRowNumber, bool pValue)
        {
            if (_cGrid == null)
                return;

            if (pRowNumber >= 0 && pRowNumber < _cGrid.Rows.Count &&
                pColumnNumber >= 0 && pColumnNumber < _cGrid.Columns.Count)
            {
                _cGrid[pColumnNumber, pRowNumber].Selected = pValue;
            }
        }

        public object __mValue(int pColumnNumber, int pRowNumber)
        {
            if (_cGrid == null)
                return null;

            if (pRowNumber < 0 || pRowNumber >= _cGrid.Rows.Count)
                return null;

            if (pColumnNumber < 0 || pColumnNumber >= _cGrid.Columns.Count)
                return null;

            var cell = _cGrid[pColumnNumber, pRowNumber];
            if (cell == null || cell.Value == null)
                return null;

            return cell.Value;
        }

        #endregion

        #endregion

        #region = ПОЛЯ

        #region - Компоненты

        protected elmComponentToolbar _cToolBar = new elmComponentToolbar();
        protected elmComponentToolbarButton _cButtonOperations = new elmComponentToolbarButton();
        protected elmComponentToolbarButton _cButtonColumns = new elmComponentToolbarButton();
        protected elmComponentPicture _cHeaderPicture = new elmComponentPicture();
        protected elmComponentLabel _cHeaderLabel = new elmComponentLabel();
        protected elmComponentGrid _cGrid = new elmComponentGrid();

        #endregion

        #region - Служебные

        private string fHeaderText = "";

        #endregion

        #endregion

        #region = СВОЙСТВА

        public Image __fHeaderImage_
        {
            set { if (_cHeaderPicture != null) _cHeaderPicture.Image = value; }
        }

        public string __fHeaderCaption_
        {
            get { return fHeaderText; }
            set
            {
                fHeaderText = value;
                if (_cHeaderLabel != null)
                    _cHeaderLabel.__fCaption_ = elmApplication.__oTunes.__mTranslate(value);
            }
        }

        public bool __fHeaderVisible_
        {
            get { return !Panel1Collapsed; }
            set { Panel1Collapsed = !value; }
        }

        public DataTable __fDataSource_
        {
            set
            {
                if (_cGrid != null)
                {
                    _cGrid.DataSource = value;
                    _cGrid.Refresh();
                }
            }
        }

        public DataGridViewRow __fCurrentRow_
        {
            get { return _cGrid != null ? _cGrid.CurrentRow : null; }
        }

        public int __fRowsCount_
        {
            get { return _cGrid != null ? _cGrid.Rows.Count : 0; }
        }

        public DataGridView __fGrid_
        {
            get { return _cGrid; }
        }

        /// <summary>
        /// Видимость служебной панели операций. Когда операции не требуются,
        /// её отключение отдаёт высоту строкам таблицы.
        /// </summary>
        public bool __fToolBarVisible_
        {
            get { return _cToolBar != null && _cToolBar.Visible; }
            set
            {
                if (_cToolBar != null)
                    _cToolBar.Visible = value;
            }
        }

        #endregion

        #region = СОБЫТИЯ

        public event EventHandler __eGridCellEnter;
        public event EventHandler __eGridRowEnter;

        #endregion
    }
}
