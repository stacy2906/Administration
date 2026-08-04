using System.Windows.Forms;
using System;
using nlReportHtml;

namespace nlElements
{
    /// <summary>
    /// Файл elmFormReport.cs
    /// </summary>
    /// <remarks>Класс-форма для формирования задач</remarks>
    public class elmFormReport : elmForm
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

            Controls.Add(__cAreaReport);
            Controls.SetChildIndex(__cAreaReport, 0);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            Text = "Базовая форма для построения отчетов";

            // _cAreaFilter
            {
                __cAreaReport.Dock = DockStyle.Fill;
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
            if (e.KeyCode == Keys.F1)
                __cAreaReport.__mPressButtonHelp();
            if (e.Control == true & e.KeyCode == Keys.A)
                __cAreaReport.__mPressButtonApply();

            base.OnKeyDown(e);

            return;
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Построение отчета
        /// </summary>
        public virtual bool __mBuildReport()
        { 
            return true;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Область для построения отчета
        /// </summary>
        public elmAreaReport __cAreaReport = new elmAreaReport();
        /// <summary>
        /// Объект для формирования отчетов
        /// </summary>
        protected rhtReport _oReport = new rhtReport();

        #endregion Компоненты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Указание закрывать форму после формирования отчета
        /// </summary>
        public bool __fCloseFormAfterReport_
        {
            get { return __cAreaReport.__fCloseFormAfterReport; }
            set { __cAreaReport.__fCloseFormAfterReport = value; }
        }

        #endregion СВОЙСТВА
    }
}
