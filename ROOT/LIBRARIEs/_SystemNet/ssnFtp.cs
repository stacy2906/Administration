using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace nlNetWork
{
    public class ssnFtp
    {
        //usage: CreateRemoteDirectory("ftp://myftp.com", 
        //          "anonymous", "myemail@mydomain.com",
        //          "newDirectory")

        static void CreateRemoteDirectory(string remotePath, string userName, string password, string newDirectory)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(remotePath + "/" + newDirectory);
            request.Credentials = new NetworkCredential(userName, password);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
            }
        }
        private static string ChoiceFile()
        {
            OpenFileDialog fo = new OpenFileDialog();

            fo.InitialDirectory = "c:\\";
            fo.Filter = "text files (*.txt)|*.txt|html files (*.html)|*.html";
            fo.FilterIndex = 1;
            fo.RestoreDirectory = true;

            if (fo.ShowDialog() == DialogResult.OK)
            {
                return fo.FileName;
            }
            else return "";
        }

        private static void FtpUpload(string fname)
        {
            try
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpurl + Path.GetFileName(fname));
                req.Method = WebRequestMethods.Ftp.UploadFile;
                req.Credentials = new NetworkCredential(username, password);

                StreamReader filereader = new StreamReader(fname);
                byte[] filebody = Encoding.Unicode.GetBytes(filereader.ReadToEnd());
                filereader.Close();
                req.ContentLength = filebody.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(filebody, 0, filebody.Length);
                requestStream.Close();
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                Console.WriteLine("Upload completed with status: {0}\n", response.StatusDescription);
                response.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static string FtpGetList()
        {
            string tmp = "";
            try
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpurl);
                // use "ListDirectory" for short list
                req.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                req.Credentials = new NetworkCredential(username, password);
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                tmp = reader.ReadToEnd();
                Console.WriteLine("GetList completed with status: {0}\n", response.StatusDescription);
                reader.Close();
                response.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine(e.ToString());
            }

            return tmp;
        }

        #region = ПОЛЯ

        #region - Константы

        const string ftpurl = "ftp://ftp.testserver.com/";

        const string username = "anonymous";

        const string password = "12345678";

        #endregion Константы

        #endregion ПОЛЯ
    }
}
