// Enumerating ports that are available for printing on a specified server
namespace Win32.PortEnumerator
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [Flags]
    public enum PortType
    {
        Write = 0x1,
        Read = 0x2,
        Redirected = 0x4,
        NetAttached = 0x8
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PortInfo2
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public string PortName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string MonitorName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Description;
        public PortType PortType;
        internal uint Reserved;
    }

    public class Program
    {
        public static void Main()
        {
            EnumeratePorts();
            Console.Write("\n\nAny key...");
            Console.ReadKey();
        }

        private static void EnumeratePorts()
        {
            uint bufSize = 0;
            uint portCount = 0;
            var portType = PortType.Read;
            var serverName = string.Empty;  // local computer

            EnumPorts(serverName, portType, IntPtr.Zero, bufSize, ref bufSize, ref portCount);
            IntPtr buffer = IntPtr.Zero;
            var ports = new List<PortInfo2>();
            try
            {
                buffer = Marshal.AllocHGlobal((int)bufSize);
                if (EnumPorts(serverName, portType, buffer, bufSize, ref bufSize, ref portCount))
                {
                    var currentOffset = buffer;
                    for (var i = 0; i < portCount; i++)
                    {
                        ports.Add((PortInfo2)Marshal.PtrToStructure(currentOffset, typeof(PortInfo2)));
                        currentOffset = (IntPtr)(currentOffset.ToInt32() + Marshal.SizeOf(typeof(PortInfo2)));
                    }
                    foreach (var portInfo in ports)
                    {
                        Console.WriteLine("{0}\n{1}\n", portInfo.PortName, portInfo.Description);
                    }
                }
                else
                {
                    Console.WriteLine("EnumPorts call failed: {0}", Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }
        }

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool EnumPorts(string pName, PortType level, IntPtr lpbPorts, uint cbBuf, ref uint pcbNeeded, ref uint pcReturned);
    }
}