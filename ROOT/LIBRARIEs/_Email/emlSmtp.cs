using MailKit.Net.Smtp;
using MimeKit;

namespace nlEmail
{
    /// <summary>
    /// Файл emlSmtp.cs
    /// </summary>
    public class emlSmtp : emlMailProtocol
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public emlSmtp() 
        {
            _fEmailProtocol = "smtp";
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Создание сообщения
        /// </summary>
        public override void __mMessageCreate()
        {
            if (__mMessageCheck() == false)
            {
                return;
            }
            //__fMimeMessage.From.Add(new MailboxAddress("Joey Tribbiani", "joey@friends.com"));
            __fMimeMessage.From.AddRange(_fMailboxAddressesListFrom);
            //__fMimeMessage.To.Add(new MailboxAddress("Mrs. Chanandler Bong", "chandler@friends.com"));
            __fMimeMessage.To.AddRange(_fMailboxAddressesListTo);
            __fMimeMessage.Subject = __fSubject;
            BodyBuilder vBodyBuilder = new BodyBuilder();
            if (__fBodyType == BODYTYPE.TextPlain)
            {
                vBodyBuilder.TextBody = __fBody;
            }
            if (__fBodyType == BODYTYPE.TextHtml)
            {
                vBodyBuilder.HtmlBody = __fBody;
            }
            //__fMimeMessage.Body = __fBody;

            __fMimeMessage.Body = vBodyBuilder.ToMessageBody();

            // Вложения https://mimekit.net/docs/html/Frequently-Asked-Questions.htm#CreateAttachments
            //---------------------------------------
            //            var message = new MimeMessage();
            //            message.From.Add(new MailboxAddress("Joey", "joey@friends.com"));
            //            message.To.Add(new MailboxAddress("Alice", "alice@wonderland.com"));
            //            message.Subject = "Как дела?";

            //            вар строитель = новый BodyBuilder();

            //            // Устанавливаем текстовую версию сообщения. 
            //            builder.TextBody = @"Привет, Алиса,

            //Чем вы занимаетесь на этих выходных? Моника устраивает одну из своих вечеринок.
            //В субботу, и я надеялся, что вы сможете приехать.

            //Вы будете моим спутником?

            //-- Джои
            //";

            //            // Также мы можем добавить событие в календарь для вечеринки Моники... 
            //            builder.Attachments.Add(@"C:\Users\Joey\Documents\party.ics");

            //            // Теперь нам осталось только установить текст сообщения, и всё готово. 
            //            message.Body = builder.ToMessageBody();
            //--------------------------------------

            using (SmtpClient vSmtpClient = new SmtpClient())
            {
                //vSmtpClient.Connect(__fServer, __fServerPort, true);
                vSmtpClient.Connect(__fServer, __fServerPort, MailKit.Security.SecureSocketOptions.SslOnConnect); // 

                //if (vSmtpClient.IsAuthenticated == true)
                //{
                    vSmtpClient.Authenticate(__fAuthenticationAccount, __fAuthenticationPassword);
                //}
                vSmtpClient.Send(__fMimeMessage);
                vSmtpClient.Disconnect(true);
            }
        }
        public override bool __mMessageCheck()
        {
            return true;
        }
        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #endregion ПОЛЯ
    }
}

