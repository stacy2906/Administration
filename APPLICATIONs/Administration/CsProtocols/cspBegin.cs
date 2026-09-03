using nlCsProtocols;
using nlDataSourceSqlite;
using System;
using System.Windows.Forms;

namespace naCsProtocols
{
    /// <summary>
    /// Файл cspBegin.cs
    /// </summary>
    /// <remarks>Главная точка входа приложения 'CsProtocols'. Сначала показывает форму загрузки
    /// протоколов ('cspFormLoad' - Form 1), затем - главный просмотрщик ('cspFormMain' - Form 2)</remarks>
    internal static class cspBegin
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Точка входа
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);


            cspApplication.__oProtocols = new dsqProtocols();

            if (cspApplication.__oEventsHandler.__mBegin())
            {
                cspFormLoader vFormLoad = new cspFormLoader();
                vFormLoad.ShowDialog();

                cspFormMain vFormMain = new cspFormMain();
                vFormMain.ShowDialog();

                cspApplication.__oEventsHandler.__mEnd();
            }
        }

        #endregion Процедуры

        #endregion МЕТОДЫ
    }
}

