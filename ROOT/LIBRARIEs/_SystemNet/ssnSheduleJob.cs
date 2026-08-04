using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace nlNetWork
{
    /// <summary>
    /// Файл ssnSheduleJob.cs
    /// </summary>
    internal class ssnSheduleJob
    {
        #region = СТРУКТУРЫ

        [StructLayout(LayoutKind.Sequential)]
        public struct AtEnumStruct
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint JobId;
            [MarshalAs(UnmanagedType.U4)]
            public uint JobTime;
            [MarshalAs(UnmanagedType.U4)]
            public uint DaysOfMonth;
            [MarshalAs(UnmanagedType.U1)]
            public byte DaysOfWeek;
            [MarshalAs(UnmanagedType.U1)]
            public byte Flags;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Command;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct AtInfoStruct
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint JobTime;
            [MarshalAs(UnmanagedType.U4)]
            public uint DaysOfMonth;
            [MarshalAs(UnmanagedType.U1)]
            public byte DaysOfWeek;
            [MarshalAs(UnmanagedType.U1)]
            public byte Flags;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Command;
        }

        #endregion СТРУКТУРЫ

        public class NetScheduleJobManager : IDisposable
        {
            #region = БИБЛИОТЕКИ

            [DllImport("Netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            private static extern int NetScheduleJobEnum(string serverName, out IntPtr buffer, int preferredMaximumLength, out uint entriesRead, out uint totalEntries, ref IntPtr resumeHandle);

            [DllImport("Netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            private static extern int NetScheduleJobAdd(string serverName, IntPtr buffer, out uint jobId);

            [DllImport("Netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            private static extern int NetScheduleJobDel(string serverName, uint minJobId, uint maxJobId);

            [DllImport("Netapi32.dll", SetLastError = true)]
            private static extern int NetApiBufferFree(IntPtr buffer);

            #endregion БИБЛИОТЕКИ

            private readonly List<AtEnumStruct> _atEnumStructs;
            private readonly string _serverName;
            private bool _disposed;
            public string ServerName
            {
                get { return this._serverName; }
            }
            public List<AtEnumStruct> AtEnumStructs
            {
                get { return this._atEnumStructs; }
            }
            public NetScheduleJobManager(string serverName = "")
            {
                _serverName = serverName ?? string.Empty;
                _atEnumStructs = new List<AtEnumStruct>();
                this.QueryNetScheduleJobs();
            }
            public void QueryNetScheduleJobs()
            {
                _atEnumStructs.Clear();
                var resumeHandle = IntPtr.Zero;
                var buffer = IntPtr.Zero;
                try
                {
                    uint entriesRead;
                    uint totalEntries;
                    var result = NetScheduleJobEnum(this._serverName, out buffer, -1, out entriesRead, out totalEntries, ref resumeHandle);
                    if (result != 0)
                    {
                        ThrowException("NetScheduleJobEnum failed", result);
                    }
                    var ptr = buffer;
                    for (var index = 0; index < entriesRead; index++)
                    {
                        var atEnumStruct = (AtEnumStruct)Marshal.PtrToStructure(ptr, typeof(AtEnumStruct));
                        this._atEnumStructs.Add(atEnumStruct);
                        ptr = (IntPtr)((int)ptr + Marshal.SizeOf(typeof(AtEnumStruct)));
                    }
                }
                finally
                {
                    if (buffer != IntPtr.Zero)
                    {
                        NetApiBufferFree(buffer);
                    }
                }
            }
            public void DeleteNetScheduleJob(uint jobId)
            {
                DeleteNetScheduleJob(jobId, jobId);
            }
            public void DeleteNetScheduleJob(uint minJobId, uint maxJobId)
            {
                var result = NetScheduleJobDel(this.ServerName, minJobId, maxJobId);
                if (result != 0)
                {
                    ThrowException("NetScheduleJobDel failed", result);
                }
                QueryNetScheduleJobs();
            }
            public uint AddNetScheduleJob(AtInfoStruct atInfoStruct)
            {
                var buffer = IntPtr.Zero;
                uint jobId;
                try
                {
                    buffer = Marshal.AllocHGlobal(Marshal.SizeOf(atInfoStruct));
                    Marshal.StructureToPtr(atInfoStruct, buffer, false);
                    var result = NetScheduleJobAdd(this.ServerName, buffer, out jobId);
                    if (result != 0)
                    {
                        ThrowException("NetScheduleJobAdd failed", result);
                    }
                    QueryNetScheduleJobs();
                }
                finally
                {
                    if (buffer != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(buffer);
                    }
                }

                return jobId;
            }
            private static void ThrowException(string message, int result)
            {
                var errorMessage = string.Format("{0}: {1}. Last error code: {2}", message, result, Marshal.GetLastWin32Error());
                throw new Exception(errorMessage);
            }

            #region IDisposable members



            public void Dispose()
            {
                Dispose(true);
                // Use SupressFinalize in case a subclass
                // of this type implements a finalizer.
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        // Dispose managed resources.
                        _atEnumStructs.Clear();
                    }
                    // Call the appropriate methods to clean up 
                    // unmanaged resources here.
                    // Indicate that the instance has been disposed.
                    _disposed = true;
                }
            }

            #endregion
        }
    }
}
