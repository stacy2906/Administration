using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nlNetWork
{
    public class ssnOperations
    {
        #region = БИБЛИОТЕКИ

        [DllImport("Ws2_32.dll", CharSet = CharSet.Ansi)]
        private static extern uint inet_addr(string address);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        private static extern bool GetRTTAndHopCount(uint address, ref long hopCount, int maxHops, ref long roundTripTime);

        #endregion БИБЛИОТЕКИ

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNetAddress">Сетевой адрес</param>
        /// <remarks>Вид параметра "212.58.244.67"</remarks>
        /// <returns>[true] - Адрес обнаружен в сети, иначе - [false]</returns>
        public bool __mPingAddress(string pNetAddress)
        {
            bool vReturn = true; // Возвращаемое значение
            var inetAddr = inet_addr(pNetAddress);
            long hopCount = 0;
            long roundTripTime = 0;
            /* 
            var ipAddress = IPAddress.Parse(strAddress);  
            int inetAddr = BitConverter.ToInt32(ipAddress.GetAddressBytes(), 0); 
             */

            if (GetRTTAndHopCount(inetAddr, ref hopCount, 30, ref roundTripTime))
            {
                Console.WriteLine("Hops: {0}\nRTT: {1}", hopCount, roundTripTime);
            }
            else
            {
                vReturn = false;
            }

            return vReturn;
        }

        public void __mOpenLink(string pNetAddress)
        {
            try
            {
                System.Diagnostics.Process.Start(pNetAddress);
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
        }
    }
}
