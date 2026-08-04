using nlAdministration;
using System;
using System.Windows.Forms;

namespace naAdministration
{
	internal static class admBegin
	{
		/// <summary>
		/// Главная точка входа для приложения.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.SetCompatibleTextRenderingDefault(false);

			admApplication.__oInterfaceAbout = new admAbout();
			admApplication.__oEventsHandler = new admEventsHandler();

			if (admApplication.__oEventsHandler.__mBegin() == true)
			{
				admFormMain vFormMain = new admFormMain();
				vFormMain.ShowDialog();

				admApplication.__oEventsHandler.__mEnd();
			}
		}
	}
}

