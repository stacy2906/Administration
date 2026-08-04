using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace nlEmail
{
    /// <summary>
    /// Файл emlMailProtocol.cs
    /// </summary>
    public class emlMailProtocol
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Создание сообщения
        /// </summary>
        public virtual void __mMessageCreate()
        { 
        }
        /// <summary>
        /// Проверка состояния сообщения
        /// </summary>
        /// <returns>[true] - Сообщение готово к дальнейшей работе, иначе - [false]</returns>
        public virtual bool __mMessageCheck()
        {
            return false;
        }
        /// <summary>
        /// Добавление адреса в список адресов отправителей
        /// </summary>
        /// <param name="pName">Имя получателя почтового сообщения</param>
        /// <param name="pAddress">Почтовый адрес</param>
        public void __mMailboxAddressesListFromAdd(string pAddress, string pName)
        {
            MailboxAddress vMailboxAddress = new MailboxAddress(pName, pAddress);
            _fMailboxAddressesListFrom.Add(vMailboxAddress);
        }
        /// <summary>
        /// Добавление адреса в список адресов получателей
        /// </summary>
        /// <param name="pName">Имя получателя почтового сообщения</param>
        /// <param name="pAddress">Почтовый адрес</param>
        public void __mMailboxAddressesListToAdd(string pAddress, string pName = "")
        {
            MailboxAddress vMailboxAddress = new MailboxAddress(pName, pAddress);
            _fMailboxAddressesListTo.Add(vMailboxAddress);
        }
        /// <summary>
        /// Очистка списка почтовых адресов отправителей
        /// </summary>
        public void __mMailboxAddressesListFromClear()
        {
            _fMailboxAddressesListFrom = null;
        }
        /// <summary>
        /// Очистка списка почтовых адресов получателей
        /// </summary>
        public void __mMailboxAddressesListToClear()
        {
            _fMailboxAddressesListTo = null;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Тема сообщения
        /// </summary>
        public string __fSubject = "";
        /// <summary>
        /// Тело сообщения
        /// </summary>
        public string __fBody = "";
        /// <summary>
        /// Вид тела сообщения
        /// </summary>
        public BODYTYPE __fBodyType = BODYTYPE.TextPlain;
        /// <summary>
        /// Название сервера
        /// </summary>
        public string __fServer = "";
        /// <summary>
        /// Номер порта подключения сервера
        /// </summary>
        public int __fServerPort = -1;
        /// <summary>
        /// Аккаутн авторизируемого подключения
        /// </summary>
        public string __fAuthenticationAccount = "";
        /// <summary>
        /// Пароль авторизированного подключения
        /// </summary>
        public string __fAuthenticationPassword = "";

        #endregion Атрибуты

        #region - Скрытые

        /// <summary>
        /// Список почтовых адресов отправителей
        /// </summary>
        protected List<InternetAddress> _fMailboxAddressesListFrom = new List<InternetAddress>();
        /// <summary>
        /// Список почтовых адресов получателей
        /// </summary>
        protected List<InternetAddress> _fMailboxAddressesListTo = new List<InternetAddress>();
        /// <summary>
        /// Название протокола
        /// </summary>
        protected string _fEmailProtocol = "";



        #endregion Скрытые

        #region - Объекты

        /// <summary>
        /// Объект сообщения
        /// </summary>
        public MimeMessage __fMimeMessage = new MimeMessage();
        /// <summary>
        /// Список сообщений
        /// </summary>
        public List<MimeMessage> __fMimeMessagesList = new List<MimeMessage>();

        #endregion Объекты

        #endregion ПОЛЯ

        #region = СВОЙСТВА


        #endregion СВОЙСТВА
    }
}
