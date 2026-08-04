using nlcsManual;
using System;
using System.Windows.Forms;

namespace naCsManual
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

            if (cmlApplication.__oEventsHandler.__mBegin())
            {
                cmlFormMain vFormMain = new cmlFormMain();
                vFormMain.ShowDialog();
                cmlApplication.__oEventsHandler.__mEnd();
            }
        }

        #endregion Процедуры

        #endregion МЕТОДЫ
    }
}
