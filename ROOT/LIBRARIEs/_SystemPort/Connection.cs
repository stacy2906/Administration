using System;
using System.Collections;

namespace nlSystemPort
{
    public class CommsListArgs : EventArgs
    {

        //private string connMsg;
        public ArrayList msgList = null;


        public CommsListArgs(ArrayList messageList)
        {

            msgList = messageList;
        }

    }

    public class DataArgs : EventArgs
    {

        public byte[] data;
        public int size;
        public DataArgs(byte[] receivedData, int dataSize)
        {
            data = receivedData;
            size = dataSize;
        }

        /*public byte[] Data{
			get {
				return data;
			}
			set{
				data = value;
			}
		}*/

    }

    /// <summary>
    /// Summary description for Connection.
    /// </summary>
    public abstract class Connection
    {

        protected string connectionName = "Generic Abstract Connection";
        protected byte[] readBuffer = new Byte[1024];
        protected byte[] writeBuffer = new Byte[1024];
        //protected int tmpReadBufferSize = 1024;
        public const bool GET = true;
        public const bool SET = false;
        public bool sendType = false;
        //protected int bytesWritten=0;
        protected int bytesWritten = 0;
        protected int bytesRead = 0;
        protected Queue fifoQue = null;
        protected ArrayList commsList;
        protected CommsListArgs connEventListArgs = null;
        protected DataArgs dataArgs = null;
        protected bool isOpen = false;

        public delegate void ConnectionEventHandler(object sender, CommsListArgs e);
        public abstract event ConnectionEventHandler ConnectionEvent;

        public delegate void DataReceiveHandler(object sender, DataArgs e);
        public delegate void QuedDataReadyHandler(object sender);

        //needs to tell external object, data is ready for processing
        public abstract event QuedDataReadyHandler QuedDataReady;

        public abstract event DataReceiveHandler DataReceived;


        #region Properties ***********************************************************
        public string ConnectionName
        {
            get
            {
                return connectionName;
            }
            set
            {
                connectionName = value;
            }
        }


        public bool IsOpen
        {
            get
            {
                return isOpen;
            }
        }

        public byte[] GetMessage()
        {

            return (byte[])fifoQue.Dequeue();
        }

        public int MessageCount()
        {
            return fifoQue.Count;
        }

        public abstract System.Collections.ArrayList BasicSettings { get; set; }
        #endregion *******************************************************************

        public abstract void Open();    //TCP/IP Socket class needs create() and connect()
                                        //Serial port class needs createfile()
                                        //Bluetooth link class needs createfile()
        public abstract void Open(ArrayList basicSettings);

        public abstract void Close();

        public abstract bool Read();

        public abstract bool Write(int dataLen);
        public abstract bool Write(ushort dataLen, bool getSet);

        public abstract bool Write(byte[] bufferSent);

        public abstract bool Write(byte[] bufferSent, ushort dataLen);
        public abstract bool Write(byte[] bufferSent, ushort dataLen, bool getSet);

        public abstract void SendMessage(); //send unsolicited messages back to calling object ( errors and data)


    }
}
