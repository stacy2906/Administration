using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace nlNetWork
{
    /// <summary>
    /// Файл ssnFunctions.cs
    /// </summary>
    public class ssnFunctions
    {
        #region = БИБЛИОТЕКИ

        [DllImport("Ws2_32.dll", CharSet = CharSet.Ansi)]
        private static extern uint inet_addr(string address);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        private static extern bool GetRTTAndHopCount(uint address, ref long hopCount, int maxHops, ref long roundTripTime);

        #endregion = БИБЛИОТЕКИ

        #region = МЕТОДЫ

        private static void PingAddress(string strAddress)
        {
            Console.WriteLine("Pinging {0}...", strAddress);
            var inetAddr = inet_addr(strAddress);
            long hopCount = 0;
            long roundTripTime = 0;

            if (GetRTTAndHopCount(inetAddr, ref hopCount, 30, ref roundTripTime))
            {
                Console.WriteLine("Hops: {0}\nRTT: {1}", hopCount, roundTripTime);
            }
            else
            {
                Console.WriteLine("Error: {0}", Marshal.GetLastWin32Error());
            }
        }

        #endregion = МЕТОДЫ
    }
}
