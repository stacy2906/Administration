/// Enumerating servers of the specified type
using System;
using System.Data;
using System.Data.Sql;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace SQLServerEnumerate
{
    class Program
    {
        static void Main()
        {
            EnumSQLServers_PInvoke();
            //EnumSQLServers_Managed();
            Console.Write("\n\nAny key...");
            Console.ReadKey();
        }

        static void EnumSQLServers_Managed()
        {
            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            DataTable table = instance.GetDataSources();
            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine("Server: {0}\n\tInstance: {1}\n\tVersion: {2}\n\tClustered: {3}\n", row["ServerName"], row["InstanceName"], row["Version"], row["IsClustered"]);
            }
        }

        static void EnumSQLServers_PInvoke()
        {
            IntPtr buffer = new IntPtr();
            int entriesRead = 0;
            int entriesTotal = 0;
            //the call may take long time to return
            int result = NetServerEnum(null,
                101, //SERVER_INFO_101
                out buffer,
                -1, //MAX_PREFERRED_LENGTH
                ref entriesRead, ref entriesTotal,
                4, //SV_TYPE_SQLSERVER
                null, //the primary domain
                IntPtr.Zero
                );

            List<SERVER_INFO_101> sqlServers = new List<SERVER_INFO_101>();

            if (result == 0)
            {
                Console.WriteLine("Servers found: {0}\n\n", entriesRead);
                if ((entriesRead > 0))
                {
                    IntPtr pServer = buffer;
                    for (int i = 0; i < entriesRead; i++)
                    {
                        SERVER_INFO_101 server = (SERVER_INFO_101)Marshal.PtrToStructure(pServer, typeof(SERVER_INFO_101));
                        sqlServers.Add(server);
                        //move the pointer
                        pServer = (IntPtr)((ulong)pServer + (ulong)Marshal.SizeOf(server));
                    }
                }
            }
            else 
                Console.WriteLine("NetServerEnum failed with {0}", result);

            if (buffer != IntPtr.Zero)
                NetApiBufferFree(buffer);

            foreach (SERVER_INFO_101 server in sqlServers)
            {
                Console.WriteLine("{0}\n", server);
            }
        }

        struct SERVER_INFO_101
        {
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 sv101_platform_id;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string sv101_name;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 sv101_version_major;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 sv101_version_minor;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 sv101_type;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string sv101_comment;

            public override string ToString()
            {
                return string.Format("Server: {0}\n\tVersion: {1}.{2}\n\tPlatform: {3}\n", this.sv101_name, this.sv101_version_major, this.sv101_version_minor, this.sv101_platform_id);
            }
        };

        [DllImport("netapi32.dll")]
        static extern int NetServerEnum([MarshalAs(UnmanagedType.LPWStr)] string servername, int level, out IntPtr bufptr, int prefmaxlen, ref int entriesread, ref int totalentries, uint servertype, [MarshalAs(UnmanagedType.LPWStr)] string domain, IntPtr resume_handle);

        [DllImport("netapi32.dll")]
        static extern int NetApiBufferFree(IntPtr buffer);
    }
}