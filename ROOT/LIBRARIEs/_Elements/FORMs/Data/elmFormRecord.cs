using System;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmFormRecord.cs
    /// </summary>
    /// <remarks>Класс-форма для измерения записи данных</remarks>
    /// <author>Lucasin V.</author> // автор
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.07.30 14-01</version> // Дата-время последней корректировки
    public class elmFormRecord : elmForm
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Размещение компонентов

            Controls.Add(__cAreaRecord);
            Controls.SetChildIndex(__cAreaRecord, 0);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            Text = "Базовая форма для изменения записи данных";

            // _cAreaRecord
            {
                __cAreaRecord.Dock = DockStyle.Fill;
                __cAreaRecord.__fBlockInputsCheckShow_ = false;
                __cAreaRecord.__eOnDataLoaded += mAreaRecord_OnDataLoaded;
                __cAreaRecord.__eOnDataSaving += mAreaRecord_OnDataSaving;
            }

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }
        
        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется при нажатии на клавиши
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            /// 1.Y Если нажата клавиша "F1"
            if (e.KeyCode == Keys.F1)
                /// 1.Y.1 Обрабатывается нажатие клавиши "__cAreaRecord.__cButtonHelp"
                __cAreaRecord.__mPressButtonHelp();
            /// 2.Y Если нажаты клавиши "Ctrl" и "A"
            if (e.Control == true & e.KeyCode == Keys.A)
                /// 2.Y.1 Обрабатывается нажатие клавиши "__cAreaRecord.__cButtonSave"
                __cAreaRecord.__mPressButtonSave();

            base.OnKeyDown(e);
        }
        /// <summary>
        /// Выполняется после загрузки данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mAreaRecord_OnDataLoaded(object sender, EventArgs e)
        {
            /// 1.Y Если в наследуемой форме подписано событие "__eOnDataLoaded", то оно вызывается
            if (__eOnDataLoaded != null)
                __eOnDataLoaded(__cAreaRecord, new EventArgs());
        }
        /// <summary>
        /// Выполняется перед сохранением данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mAreaRecord_OnDataSaving(object sender, EventArgs e)
        {
            /// 1.Y Если в наследуемой форме подписано событие "__eOnDataSaving", то оно вызывается
            if (__eOnDataSaving != null)
                __eOnDataSaving(__cAreaRecord, new EventArgs());
        }

        #endregion Поведение

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Область для правки табличных данных
        /// </summary>
        public elmAreaRecord __cAreaRecord = new elmAreaRecord();

        #endregion Компоненты

        #endregion ПОЛЯ

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает после загрузки данных
        /// </summary>
        public event EventHandler __eOnDataLoaded;
        /// <summary>
        /// Возникает перед сохранением данных
        /// </summary>
        public event EventHandler __eOnDataSaving;

        #endregion СОБЫТИЯ
    }
}
