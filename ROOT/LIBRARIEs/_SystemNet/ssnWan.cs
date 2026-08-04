using System.Net;

namespace nlSystemNet
{
    /// <summary>
    /// Файл ssnWan.cs
    /// </summary>
    /// <remarks>Класс для работы с глобальной сетью</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.01.13 16-41</version> // Дата-время последней корректировки
    public class ssnWan
    {
        /// <summary>
        /// Получение IP адреса по имени сервера
        /// </summary>
        /// <param name="pHost"></param>
        /// <returns></returns>
        public IPAddress _mHostToIP(string pHost)
        {
            IPAddress vReturn = null; // Возвращаемое значение
            IPHostEntry vIPHostEntry = Dns.GetHostEntry(pHost);
            if (vIPHostEntry == null || vIPHostEntry.AddressList == null || vIPHostEntry.AddressList.Length <= 0)
            {
                //throw new Exception("Не удалось определить IP-адрес по хосту.");
                vReturn = null;
            }
            else
                vReturn = vIPHostEntry.AddressList[0];

            return vReturn;
        }
    }
}
