using System.Collections;
using System.Data;
using System.Windows.Forms;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmFormBrowse.cs
    /// </summary>
    /// <remarks>Класс-форма для просмотра данных 'DataTable'</remarks>
    /// <example>
    /// elmFormBrowse vFormBrowse = new elmFormBrowse();
    /// vFormBrowse.__mDataSourceDataTable(vDataTable);
    /// vFormBrowse.ShowDialog();
    ///</example>
    public class elmFormBrowse : elmForm
    {
        #region = МЕТОДЫ

        #region - Объекты

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Размещение компонентов

            Controls.Add(_cGrid);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            _cGrid.Dock = DockStyle.Fill;

            #endregion Настройка компонентов

            ResumeLayout();

            return;
        }

        #endregion Объекты

        #region - Процедуры

        /// <summary>
        /// Подключение источника данных
        /// </summary>
        /// <param name="pDataTable"></param>
        public void __mDataSourceDataTable(DataTable pDataTable)
        {
            _cGrid.Columns.Clear();

            _cGrid.DataSource = pDataTable;
            _cGrid.Refresh();
            _cGrid.Focus();
        }
        public void __mDataSourceArrayList(ArrayList pArrayList)
        {
            _cGrid.Columns.Clear();

            DataTable vDataTable = new DataTable();
            vDataTable.Columns.Add("List", typeof(String));
            foreach (string vList in pArrayList)
            {
                DataRow vDataRowNew = vDataTable.NewRow();
                vDataRowNew["List"] = vList.Trim();
                vDataTable.Rows.Add(vDataRowNew);
            }

            _cGrid.DataSource = vDataTable;
            _cGrid.Refresh();
            _cGrid.Focus();
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        protected DataGridView _cGrid = new DataGridView();

        #endregion Компоненты

        #endregion ПОЛЯ
    }
}
