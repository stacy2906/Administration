using System;
using System.Collections;
using System.Runtime.InteropServices;
using nlSystemPort.Serial;

namespace nlSystemPort
{

    public class SerialPort : Connection
    {
        private string portName;
        private IntPtr hPort = (IntPtr)CommAPI.INVALID_HANDLE_VALUE;
        private CommAPI m_CommAPI;
        private DCB dcb = new DCB();
        private nlSystemPort.Serial.BasicPortSettings portSettings;
        // default Rx buffer is 1024 bytes
        //private int tmpReadBufferSize = 1024;
        //private int txBufferSize = 1024;


        private OVERLAPPED ovlCommPort = new OVERLAPPED();
        public readonly CommCapabilities Capabilities = new CommCapabilities();

        //public delegate ConnectionEventHandler
        public override event ConnectionEventHandler ConnectionEvent = null;
        public override event DataReceiveHandler DataReceived = null;
        public override event QuedDataReadyHandler QuedDataReady = null;

        public SerialPort()
        {

            commsList = new ArrayList();
            connEventListArgs = new CommsListArgs(commsList);

        }

        private void Init()
        {

            //Determine Windows platform Windows 2K/XP or Windows CE (Pocket PC)
            string strPlatform = "unknown";
            if (System.Environment.OSVersion.Platform != PlatformID.WinCE)
            {
                m_CommAPI = new WinCommAPI();
                strPlatform = "Platform Detected: Windows NT/2K/XP";
            }
            else
            {
                m_CommAPI = new CECommAPI();
                strPlatform = "Platform Detected: Windows CE";
            }
            SendConnEvent(strPlatform);
            //rxFIFO = new Queue(tmpReadBufferSize);
            portSettings = new nlSystemPort.Serial.BasicPortSettings();

        }

        #region Properties	


        public override System.Collections.ArrayList BasicSettings
        {
            get
            {
                return portSettings.SettingList;
            }
            set
            {
                portSettings.SettingList = value;
            }
        }


        public string PortName
        {
            get
            {
                return portName;
            }
            set
            {
                if (!CommAPI.FullFramework)
                {
                    // for CE, ensure the port name is colon terminated "COMx:"
                    if (!value.EndsWith(":"))
                    {
                        portName = value + ":";
                        return;
                    }
                }
                portName = value;
            }
        }



        #endregion

        //default Open settings 
        public override void Open()
        {

            //Automatically open a COM port from range COM1 to COM10
            ArrayList changingPortList = new ArrayList(2);
            changingPortList.Add(" ");
            changingPortList.Add("115200");

            for (int i = 9; i <= changingPortList.Count; i++)
            {

                changingPortList[0] = "COM" + i;
                Open(changingPortList);
                if (isOpen)
                    break;
            }
        }


        public override void Open(ArrayList basicSettings)
        {

            Init();

            //Port Number
            PortName = (string)basicSettings[0];
            try
            {
                //Baudrate
                int rate = System.Convert.ToInt32(basicSettings[1]);
                portSettings.BaudRate = (nlSystemPort.Serial.BaudRates)rate;
            }
            catch (System.FormatException e)
            {
                SendConnEvent("CLOSE: ERROR: " + "Baudrate " + e.Message);
                return;

            }

            //Open the Port handle
            hPort = m_CommAPI.CreateFile(portName);

            if (hPort == (IntPtr)CommAPI.INVALID_HANDLE_VALUE)
            {
                int e = Marshal.GetLastWin32Error();
                string strClose = "";
                if (e == (int)APIErrors.ERROR_ACCESS_DENIED)
                    strClose = "CLOSE: ERROR:Access denied to serial port! Possibly in use.";
                else if (e == (int)APIErrors.ERROR_FILE_NOT_FOUND)
                    strClose = "CLOSE: ERROR: Serial port " + PortName + " not found!";
                else if (e == (int)APIErrors.ERROR_PATH_NOT_FOUND)
                    strClose = "CLOSE: Serial port " + PortName + " not found!";
                else if (e == (int)APIErrors.ERROR_INVALID_HANDLE)
                    strClose = "CLOSE: ERROR: Serial Com port handle is invalid!";
                else if (e == (int)APIErrors.ERROR_INVALID_NAME)
                    strClose = "CLOSE: ERROR: Serial port" + PortName + " name not found!";
                // ClearCommError failed!
                //string error = String.Format("CreateFile Failed: {0}", e);
                //throw new CommPortException(error);
                else
                    strClose = "CLOSE: ERROR: Unable to open port, Error Code " + e;

                SendConnEvent(strClose);
                return;

            }

            isOpen = true;

            // set queue sizes

            // Transfer the port settings to a DCB structure
            dcb.BaudRate = (uint)portSettings.BaudRate;
            dcb.ByteSize = portSettings.ByteSize;
            isOpen = m_CommAPI.SetCommState(hPort, dcb);

            // set the Comm timeouts
            CommTimeouts ct = new CommTimeouts();
            ct.ReadIntervalTimeout = uint.MaxValue; // this = 0xffffffff
            ct.ReadTotalTimeoutConstant = 0;
            ct.ReadTotalTimeoutMultiplier = 0;
            // writing we'll give 1 second
            ct.WriteTotalTimeoutConstant = 1;
            ct.WriteTotalTimeoutMultiplier = 0;
            isOpen = m_CommAPI.SetCommTimeouts(hPort, ct);

            SendConnEvent("OPEN:Serial port " + PortName + " opened successfully");
        }


        public override bool Write(byte[] bufferSent, ushort dataLen, bool getSet)
        {

            sendType = getSet;
            bool isWritten = Write(bufferSent, dataLen);
            return isWritten;

        }

        public override bool Write(byte[] bufferSent, ushort dataLen)
        {

            //writeBuffer = bufferSent;
            bool isWritten = m_CommAPI.WriteFile(hPort, bufferSent, dataLen, ref bytesWritten, IntPtr.Zero);

            Console.WriteLine("SerialPort.Write(): dataLen = " + dataLen + " bytesWritten = " + bytesWritten);
            return isWritten;
        }

        public override bool Write(ushort dataLen, bool getSet)
        {

            sendType = getSet;

            bool isWritten = Write(dataLen);
            return isWritten;

        }
        public override bool Write(int dataLen)
        {

            //writeBuffer = bufferSent;
            //bool isWritten =  m_CommAPI.WriteFile(hPort, writeBuffer, dataLen, ref bytesWritten, ref ovlCommPort);

            UInt32 length = Convert.ToUInt32(dataLen);
            bool isWritten = m_CommAPI.WriteFile(hPort, writeBuffer, length, ref bytesWritten, IntPtr.Zero);

            Console.WriteLine("SerialPort.Write(): dataLen = " + dataLen + " bytesWritten = " + bytesWritten);

            return isWritten;
        }

        public override bool Write(byte[] bufferSent)
        {

            UInt32 length = Convert.ToUInt32(bufferSent.Length);
            bool isWritten = m_CommAPI.WriteFile(hPort, bufferSent, length, ref bytesWritten, IntPtr.Zero);

            //bool isWritten =  m_CommAPI.WriteFile(hPort, bufferSent, bufferSent.Length, ref bytesWritten, IntPtr.Zero);

            Console.WriteLine("SerialPort.Write(): dataLen = " + bufferSent.Length + " bytesWritten = " + bytesWritten);

            return isWritten;
        }


        public override bool Read()
        {
            bool isRead = m_CommAPI.ReadFile(hPort, readBuffer, readBuffer.Length, ref bytesRead, IntPtr.Zero);

            //bool isRead =  m_CommAPI.ReadFile(hPort, readBuffer, readBuffer.Length, ref bytesRead, ref ovlCommPort);

            //Console.WriteLine("SerialPort.Read(): bytesRead = " + bytesRead);
            /*if (bytesRead >0){
				dataArgs = new DataArgs(readBuffer,bytesRead);
				DataReceived(this,  dataArgs);
			
			}*/

            return isRead;
        }


        public override void Close()
        {

            if (hPort.Equals((IntPtr)CommAPI.INVALID_HANDLE_VALUE))
                return;

            else if (m_CommAPI.CloseHandle(hPort))
            {
                isOpen = false;
                hPort = (IntPtr)CommAPI.INVALID_HANDLE_VALUE;
                Console.WriteLine("SerialPort.Close(): Closed Port Handle");

                SendConnEvent("Closed Serial port " + PortName);

            }
        }


        public override void SendMessage()
        {
            // TODO:  Add SerialPort.SendMessage implementation
        }


        protected void SendConnEvent(string evtMsg)
        {
            connEventListArgs.msgList.Add(evtMsg);
            ConnectionEvent(this, connEventListArgs);
            connEventListArgs.msgList.Clear();
        }
    }
}
