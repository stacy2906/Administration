using System.Drawing;
using System.Windows.Forms;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmFormReportPreview.cs
    /// </summary>
    /// <remarks>Класс-форма для предварительного просмотра отчетов</remarks>
    public class elmFormReportPreview : elmForm
    {
        #region = МЕТОДЫ

        #region - Объект

        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Размещение компонентов

            Controls.Add(__cAreaReportPreview);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            __fCaption_ = "Просмотр отчета";
            ShowInTaskbar = true;

            #endregion Настройка компонентов

            ResumeLayout();

            return;
        }

        #endregion Объект

        #endregion МЕТОДы

        #region = ПОЛЯ

        #region - Компоненты

        public elmAreaReportPreview __cAreaReportPreview = new elmAreaReportPreview();

        #endregion Компоненты

        #endregion ПОЛЯ
    }
}
