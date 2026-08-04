using System;
using System.Management;
using System.Diagnostics;
/// Enumerating Processes
namespace ConsoleApplication1
{
    partial class Program
    {
        static void EnumeratingProcesses()
        {
            foreach (Process p in Process.GetProcesses())
            {
                Console.WriteLine("Id={0}, {1}, [{2}]", p.Id, p.ProcessName, GetProcessFileName(p));
            }
        }

        static string GetProcessFileName(Process p)
        {
            try
            {
                string FileName = p.MainModule.FileName;
                return FileName;
            }
            catch
            {
                return "";
            }
        }
        //processes can also be enumerated
        //through using WMI objects
        static void EnumeratingProcesses_WMI()
        {
            ManagementScope scope = new ManagementScope(@"\\.\root\cimv2");
            scope.Connect();
            ObjectQuery query = new ObjectQuery(@"SELECT * FROM Win32_Process");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            foreach (ManagementObject obj in searcher.Get())
            {
                Console.WriteLine("Id={0}, {1}, [{2}]", obj["ProcessId"], obj["Name"], obj["ExecutablePath"]);
            }
        }
    }
}