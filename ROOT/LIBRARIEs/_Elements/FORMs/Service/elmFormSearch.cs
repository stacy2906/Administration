using System.Windows.Forms;
using System;
using System.Collections.Generic;

namespace nlElements
{
    /// <summary>
    /// Файл elmFormSearch.cs
    /// </summary>
    /// <remarks>Класс-форма для отображения данных поиска</remarks>
    public class elmFormSearch : elmForm
    {
        #region = МЕТОДЫ

        #region - Объект

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {

            SuspendLayout();

            base._mObjectAssembly();

            #region Размещение компонентов

            Controls.Add(_cAreaSearch);
            Controls.SetChildIndex(_cAreaSearch, 0);

            #endregion Размещение компонентов

            #region Описание компонентов

            // _cAreaSearch
            {
                _cAreaSearch.Dock = DockStyle.Fill;
            }

            #endregion Описание компонентов

            ResumeLayout();

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();
            _cAreaSearch.__mGridFocus();
        }

        #endregion Объект

        #region - Поведение

        /// <summary>
        /// Выполняется при отпускании горячих клавиш
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Control == true & e.KeyCode == Keys.A) // Ctrl+A
                _cAreaSearch.__mButtonSelectClick();

            base.OnKeyUp(e);
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Загрузка настроек формы из файла
        /// </summary>
        /// <param name="pFormName">Название формы</param>
        protected override void _mTunesLoad(string pFormName)
        {
            base._mTunesLoad(pFormName);

            #region Загрузка видимости полей

            //foreach (DataGridViewColumn vGridColumn in _cAreaSearch.__fColumnsList_)
            //{
            //    string vString = __oFileIni.__mValueRead(pFormName.ToUpper(), "Field_" + vGridColumn.Name);
            //    try
            //    {
            //        vGridColumn.Visible = Convert.ToBoolean(vString);
            //    }
            //    catch
            //    {
            //        vGridColumn.Visible = true;
            //    }
            //}

            #endregion Загрузка видимости полей
        }
        /// <summary>
        /// Сохранение настроек формы в файл
        /// </summary>
        /// <param name="pFormName">Название формы</param>
        protected override void _mTunesSave(string pFormName)
        {
            base._mTunesSave(pFormName);

            #region Сохранение видимости полей

            //foreach (DataGridViewColumn vGridColumn in _cAreaSearch._fColumns)
            //{
            //    _oFileIni._mValueWrite(vGridColumn.Visible.ToString(), pFormName.ToUpper(), "Field_" + vGridColumn.Name);
            //}

            #endregion Сохранение видимости полей
            /// Сохранение сортировки
            //_cAreaSearch.__mSortingSave();
        }

        #endregion - Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Область для правки табличных данных
        /// </summary>
        public elmAreaSearch _cAreaSearch = new elmAreaSearch();

        #endregion Компоненты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Список отображаемых колонок
        /// </summary>
        public List<elmUnitGridColumn> _fColumnsList
        {
            get { return _cAreaSearch.__fColumnsList_; }
            set { _cAreaSearch.__fColumnsList_ = value; }
        }

        #endregion СВОЙСТВА
    }
}
