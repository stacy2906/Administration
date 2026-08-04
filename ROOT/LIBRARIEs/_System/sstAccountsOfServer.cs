using System;
using System.Collections;
using System.Runtime.InteropServices;
/// Enumerating global and local group accounts on a server 
namespace EnumGroups
{
    class Class1
    {
        [STAThread]
        static void Main(string[] args)
        {
            Tgroups groups = new Tgroups("", 0);
            if (groups.errorcode == 0)
            {
                foreach (Tgroup g in groups)
                {
                    Console.WriteLine(g.groupname);
                }
            }
            else
            {
                Console.WriteLine("Error code: " + groups.errorcode);
            }
        }

        class Tgroups : CollectionBase
        {
            int _groupmode = 0;
            string _servername = "";
            public uint errorcode = 0;
            public Tgroups(string server, int mode)
            {
                _groupmode = mode;
                _servername = server;
                EnumGroups();
            }
            void EnumGroups()
            {
                GroupEnum f;
                if (_groupmode == 0)
                {
                    f = new GroupEnum(NetLocalGroupEnum);
                }
                else
                {
                    f = new GroupEnum(NetGroupEnum);
                }

                IntPtr groups = IntPtr.Zero;
                uint entriesread = 0, totalentries = 0;
                IntPtr s = Marshal.StringToBSTR(_servername);
                LOCALGROUP_INFO_1 info;
                errorcode = f(s, 1, ref groups, MAX_PREFERRED_LENGTH, ref entriesread, ref totalentries, IntPtr.Zero);

                for (int i = 0; i < entriesread; i++)
                {
                    int offset = groups.ToInt32() + LOCALGROUP_INFO_1_SIZE * i;
                    info = (LOCALGROUP_INFO_1)Marshal.PtrToStructure(new IntPtr(offset), typeof(LOCALGROUP_INFO_1));
                    Tgroup g = new Tgroup(info);
                    List.Add(g);
                    g = null;
                }
                NetApiBufferFree(groups);
            }
        }
        class Tgroup
        {
            string _groupname = "";
            string _comment = "";
            public string groupname
            {
                get { return _groupname; }
            }

            public string comment
            {
                get { return _comment; }
            }

            public Tgroup(LOCALGROUP_INFO_1 info)
            {
                _groupname = Marshal.PtrToStringAuto(info.lpszGroupName);
                _comment = Marshal.PtrToStringAuto(info.lpszComment);
            }
        }

        [DllImport("netapi32", EntryPoint = "NetApiBufferFree")]
        internal static extern void NetApiBufferFree(IntPtr bufptr);

        [DllImport("netapi32", EntryPoint = "NetLocalGroupEnum")]
        internal static extern uint NetLocalGroupEnum(IntPtr ServerName, uint level, ref IntPtr siPtr, uint prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resumeHandle);

        [DllImport("netapi32", EntryPoint = "NetGroupEnum")]
        internal static extern uint NetGroupEnum(IntPtr ServerName, uint level, ref IntPtr siPtr, uint prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resumeHandle);

        //a delegate for NetGroupEnum and NetLocalGroupEnum
        delegate uint GroupEnum(IntPtr ServerName, uint level, ref IntPtr siPtr, uint prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resumeHandle);

        const int LOCALGROUP_INFO_1_SIZE = 8;
        const uint MAX_PREFERRED_LENGTH = 0xffffffff;

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct LOCALGROUP_INFO_1
        {
            public IntPtr lpszGroupName;
            public IntPtr lpszComment;

        }
    }
}