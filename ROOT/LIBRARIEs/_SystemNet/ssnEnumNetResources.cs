using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
/// Enumerating network interfaces on the local computer
namespace netinfo
{
    class Program
    {
        static void Main(string[] args)
        {
            NetInterface();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
        private static void NetInterface()
        {
            NetworkInterface[] NetInfo = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface info in NetInfo)
            {
                IPv4InterfaceStatistics stat = info.GetIPv4Statistics();
                IPInterfaceProperties prop = info.GetIPProperties();
                Console.WriteLine(info.Description);
                Console.WriteLine("Internal name: {0}", info.Name);
                Console.WriteLine("MAC address: {0}", info.GetPhysicalAddress());
                Console.WriteLine("Id: {0}", info.Id);
                Console.WriteLine("Type: {0}", info.NetworkInterfaceType);
                Console.WriteLine("Speed (MBps): {0}", info.Speed / 1000000);
                Console.WriteLine("MBytes sent: {0}; MBytes received: {1}", stat.BytesSent / 1048576, stat.BytesReceived / 1048576);
                Console.WriteLine("Status: {0}\n", info.OperationalStatus);
            }
        }
    }
}