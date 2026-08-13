using nlCsProtocols;
using nlDataSourceSqlite;
using CsProtocols.DATA.Importers;
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

            dsqProtocols vProtocols = new dsqProtocols();
            cspApplication.__oProtocols = vProtocols; // dsqProtocols становится активным логгером приложения (пишет в SQLite вместо файлов .pcl)

            if (cspApplication.__oEventsHandler.__mBegin() == true)
            {
                try
                {
                    /// Импорт легаси '.pcl' файлов не только из папки самого 'CsProtocols', а из
                    /// 'PROTOCOLs' каждого приложения решения - см. примечание к
                    /// 'ProtocolSqliteImporter.__mProtocolsFoldersDiscover'. Раньше сканировалась
                    /// только 'cspApplication.__oPathes.__fDirectoryProtocols_' (папка самого
                    /// CsProtocols), из-за чего протоколы Administration.exe/csManual.exe и т.д.
                    /// никогда не попадали в базу данных
                    ProtocolSqliteImporter vImporter = new ProtocolSqliteImporter();
                    foreach (string vFolderPath in ProtocolSqliteImporter.__mProtocolsFoldersDiscover(cspApplication.__oPathes.__fDirectoryStart))
                    {
                        vImporter.__mImportFromFolder(vProtocols, vFolderPath);
                    }
                }
                catch
                {
                    /// Сбой импорта не должен препятствовать запуску приложения - импорт можно повторить при следующем старте
                }

                cspFormMain vFormMain = new cspFormMain();
                vFormMain.ShowDialog();
            }
            cspApplication.__oEventsHandler.__mEnd();
        }
    }
}
