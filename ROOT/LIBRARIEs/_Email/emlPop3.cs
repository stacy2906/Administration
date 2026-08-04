using MailKit.Net.Pop3;
using MimeKit;
using System;

namespace nlEmail
{
    public class emlPop3 : emlMailProtocol
    {
        public void Main()
        {
            using (Pop3Client vPop3Client = new Pop3Client())
            {
                vPop3Client.Connect(__fServer, __fServerPort, false);
                if (vPop3Client.IsAuthenticated == true)
                {
                    vPop3Client.Authenticate("joey", "password");
                }
                for (int i = 0; i < vPop3Client.Count; i++)
                {
                    MimeMessage __fMimeMessage = vPop3Client.GetMessage(i);
                    __fMimeMessagesList.Add(__fMimeMessage);
                }

                vPop3Client.Disconnect(true);
            }

        }
    }
}

