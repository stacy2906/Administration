using System;
using MimeKit;
using MailKit;
using MailKit.Search;
using MailKit.Net.Imap;


namespace nlEmail
{
    public class emlImap : emlMailProtocol
    {
        public void Main()
        {
            using (ImapClient vImapClient = new ImapClient())
            {
                vImapClient.Connect(__fServer, __fServerPort, true);
                vImapClient.Authenticate(__fAuthenticationAccount, __fAuthenticationPassword);

                // The Inbox folder is always available on all IMAP servers...
                var inbox = vImapClient.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                Console.WriteLine("Total messages: {0}", inbox.Count);
                Console.WriteLine("Recent messages: {0}", inbox.Recent);

                for (int i = 0; i < inbox.Count; i++)
                {
                    __fMimeMessage = inbox.GetMessage(i);
                    Console.WriteLine("Subject: {0}", __fMimeMessage.Subject);
                }

                vImapClient.Disconnect(true);
            }
        }
    }
}

