using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections;
using System.Data;
using System.Data.Sql;
using System.Windows.Forms;
using System.Xml.Linq;

namespace nlDataSourceMsSql
{
    /// <summary>
    /// Файл dmsServer.cs
    /// </summary>
    /// <remarks>Класс для работы с серверами MS SQL</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.01.13 15-12</version> // Дата-время последней корректировки
    public class dmsServer
    {
        /// <summary>
        /// Получение списка доступных серверов
        /// </summary>
        /// <returns>{ArrayList}</returns>
        public ArrayList _mServersList()
        {
            ArrayList vReturn = new ArrayList(); // Возвращаемое значение
            // Версия 2000-2005
            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            DataTable table = instance.GetDataSources();

            //DataTable table = SqlDataSourceEnumerator.Instance.GetDataSources();

            foreach (DataRow row in table.Rows)
            {
                vReturn.Add(row[0].ToString().Trim());

                //foreach (DataColumn col in table.Columns)
                //{
                //    vReturn.Add(String.Format("{0} = {1}", col.ColumnName, row[col]));
                //}
            }
            // Версия выше
            DataTable dataTable = SmoApplication.EnumAvailableSqlServers(true);
            foreach (DataRow row in dataTable.Rows)
            {
                vReturn.Add((string)row[0].ToString());
            }

            return vReturn;
        }
        public ArrayList _mSereversLocalNames()
        {
            ArrayList vReturn = new ArrayList(); // Возвращаемое значение

            string ServerName = Environment.MachineName;
            Microsoft.Win32.RegistryView registryView = Environment.Is64BitOperatingSystem ? Microsoft.Win32.RegistryView.Registry64 : Microsoft.Win32.RegistryView.Registry32;
            using (Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, registryView))
            {
                Microsoft.Win32.RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);
                if (instanceKey != null)
                {
                    foreach (var instanceName in instanceKey.GetValueNames())
                    {
                        if (instanceName == "MSSQLSERVER")
                        {
                            vReturn.Add(ServerName);

                        }
                        else
                        {
                            vReturn.Add(ServerName + "\\" + instanceName);
                        }
                    }
                }
            }

            return vReturn;
        }
        /// Получение данных по текущему серверу
        /*
        SELECT @@Servername AS ServerName,
        create_date AS ServerStarted,
        DATEDIFF(s, create_date, GETDATE()) / 86400.0 AS DaysRunning,
        DATEDIFF(s, create_date, GETDATE()) AS SecondsRunnig
        FROM sys.databases
        WHERE name = 'tempdb';
        */

        public ArrayList __mDatabaseS(string pServerName, string pLogin = "", string pPassword = "")
        {

            ArrayList vReturn = new ArrayList();

            string vQuery = "Select"
+ " @@SERVERNAME AS Server"
+ ", d.name AS DBName"
+ ", create_date"
+ ", compatibility_level"
+ ", m.physical_name AS FileName"
+ " FROM sys.databases d"
+ " JOIN sys.master_files m ON d.database_id = m.database_id"
+ " WHERE   m.[type] = 0 -- data files only"
+ " ORDER BY d.name";

            dmsDataSourceMsSql vDataSource = new dmsDataSourceMsSql();
            vDataSource.__fServer = pServerName;
            vDataSource.__fDatabaseName = "master";
            vDataSource.__fLocalDB = false;
            vDataSource.__fOnLine = false;
            DataTable vDataTable = vDataSource.__mSqlQuery(vQuery);
            DataView dv = new DataView(vDataTable, "", "DBName", DataViewRowState.CurrentRows);
            vDataTable = dv.ToTable();
            foreach (DataRow row in vDataTable.Rows)
            {
                vReturn.Add(row["DBName"]);
            }

            return vReturn;
        }

        /// Получение информации о базах данных
        /*
        SELECT @@SERVERNAME AS Server,
        name AS DBName,
        recovery_model_Desc AS RecoveryModel,
        Compatibility_level AS CompatiblityLevel,
        create_date,
        state_desc
        FROM sys.databases
        ORDER BY Name;
        */
        // 2-й вариант. Результирующие данные разные
        /*
        SELECT @@SERVERNAME AS Server,
                d.name AS DBName ,
        create_date ,
        compatibility_level ,
        m.physical_name AS FileName
        FROM    sys.databases d
        JOIN sys.master_files m ON d.database_id = m.database_id
        WHERE m.[type] = 0-- data files only
        ORDER BY d.name;
        */
    }

}
