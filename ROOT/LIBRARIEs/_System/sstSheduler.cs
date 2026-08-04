using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace nlSystem
{

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

    public class sstSheduler : IDisposable
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

        #endregion = БИБЛИОТЕКИ

        #region = ДИЗАЙНЕР

        /// <summary>Конструктор
        /// </summary>
        /// <param name="serverName"></param>
        public sstSheduler(string serverName = "")
        {
            _serverName = serverName ?? string.Empty;
            _atEnumStructs = new List<AtEnumStruct>();
            this.QueryNetScheduleJobs();
        }
        /// <summary>Очистка всех используемых ресурсов
        /// </summary>
        /// <param name="disposing">[true] - если все используемые ресурсы бвли удалены, иначе - [false]</param>
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

        #endregion = ДИЗАЙНЕР

        #region = ПОЛЯ

        #region - Внутренние

        private readonly List<AtEnumStruct> _atEnumStructs;
        private readonly string _serverName;
        private bool _disposed;

        #endregion - Внутренние

        #endregion = ПОЛЯ
        public string ServerName
        {
            get { return this._serverName; }
        }

        public List<AtEnumStruct> AtEnumStructs
        { 
            get { return this._atEnumStructs; }
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

        public void Dispose()
        {
            Dispose(true);
            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }
    }
}



