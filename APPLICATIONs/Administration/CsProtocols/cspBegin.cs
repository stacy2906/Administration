using nlCsProtocols;
using nlDataSourceSqlite;
using System;
using System.Windows.Forms;

namespace naCsProtocols
{

        /// <summary>
        /// Файл cmlBegin.cs
        /// </summary>
        /// <remarks>Главная точка входа для приложения 'CsManual'</remarks>
        internal static class cmlBegin
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

                if (cspApplication.__oEventsHandler.__mBegin())
                {
                    cspFormMain vFormMain = new cspFormMain();
                    vFormMain.ShowDialog();
                    cspApplication.__oEventsHandler.__mEnd();
                }
            }

            #endregion Процедуры

            #endregion МЕТОДЫ
       }
    

}

