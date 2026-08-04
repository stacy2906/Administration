using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace nlApplication
{
    /// <summary>
    /// Файл appFileHtml.cs
    /// </summary>
    /// <remarks>Класс для работы с Html файлами</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 14-00</version> // Дата-время последней корректировки    
    public class appFileHtml
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Преобразование Url адреса в локальный адрес
        /// </summary>
        /// <param name="pAddress">Url адрес</param>
        public string __mAddressToString(string pAddress)
        {
            if (pAddress.StartsWith("file:///"))
                pAddress = pAddress.Substring(8);

            return pAddress.Replace('/', '\\');
        }
        /// <summary>
        /// Чтение указанного тэга из файла
        /// </summary>
        /// <param name="vTagName">Имя тэга</param>
        public string __mTagFromFile(string pFilePath, string vTagName)
        {
            WebClient wc = new WebClient();
            string html = __mOpen(pFilePath);
            string pattern = string.Format(@"\<{0}.*?\>(?<tegData>.+?)\<\/{0}\>", vTagName.Trim());
            // \<{0}.*?\> - открывающий тег
            // \<\/{0}\> - закрывающий тег
            // (?<tegData>.+?) - содержимое тега, записываем в группу tegData

            Regex regex = new Regex(pattern, RegexOptions.ExplicitCapture);

            MatchCollection matches = regex.Matches(html);
            if (matches.Count > 0 && matches[0].Groups.Count > 0)
            {
                return matches[0].Groups[1].Value;
            }

            return "";
        }
        /// <summary>
        /// Чтение значения тэга из строки
        /// </summary>
        /// <param name="pLine">Содержание строки</param>
        /// <returns></returns>
        public string __mTagFromLine(string pLine)
        {
            for (int i = 1; i < 100; i++)
            {
                string expr = @"<src[^>]*?" + @"[^>]*?>((.|\s)*?(<\/src>)){" + i.ToString() + @"}";
                Regex rgx1 = new Regex(expr, RegexOptions.Compiled);
                Match mc1 = rgx1.Match(pLine);
                Regex rgx2 = new Regex(@"<src[^>]*?>", RegexOptions.Compiled);
                MatchCollection mc2 = rgx2.Matches(mc1.Value);
                if ((i - mc2.Count) == 0)
                {
                    return mc1.Value;
                }
            }

            return "no result";
        }
        /// <summary>
        /// Открытие и чтение файла
        /// </summary>
        /// <param name="url">Адрес файла</param>
        public string __mOpen(string url)
        {
            WebClient client = new WebClient();
            using (Stream data = client.OpenRead(url))
            {
                using (StreamReader reader = new StreamReader(data, Encoding.Default))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        /// <summary>
        /// Извлекает путь к файлу из адреса Url
        /// </summary>
        /// <param name="pUrl">Адрес URL</param>
        public static string __mUrlToFile(string pUrl)
        {
            if (pUrl.StartsWith("file:///") == true)
                return pUrl.Substring(8);

            return pUrl;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ
    }
}
