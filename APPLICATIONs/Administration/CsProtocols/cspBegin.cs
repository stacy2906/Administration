using nlCsProtocols;
using System;
using System.Windows.Forms;

namespace naCsProtocols
{
    internal static class cspBegin
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetCompatibleTextRenderingDefault(false);
            if (cspApplication.__oEventsHandler.__mBegin() == true)
            {
                cspFormMain vFormMain = new cspFormMain();
                vFormMain.ShowDialog();
            }
            cspApplication.__oEventsHandler.__mEnd();
        }
    }
}
