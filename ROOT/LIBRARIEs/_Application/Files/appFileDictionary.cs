using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace nlApplication
{
    /// <summary>
    /// Файл appFileDictionary.cs
    /// </summary>
    /// <remarks>Класс для работы с файлами-словарями</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.01.13 13-59</version> // Дата-время последней корректировки
    public sealed class appFileDictionary
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор с указанием файла словаря
        /// </summary>
        /// <param name="pFilePath">Путь и имя файла</param>
        public appFileDictionary(string pFilePath)
        {
            __fFilePath = pFilePath;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ 

        #region - Процедуры

        /// <summary>
        /// Чтение словаря из файла
        /// </summary>
        /// <returns>Dictionary<string, string></returns>
        public Dictionary<string, string> __mLoad()
        {
            Dictionary<string, string> vReturn = new Dictionary<string, string>(); // Возвращаемое значение
            /// Если файл существует, выполняется перебор всех строк и запись в 'Dictionary'
            if (File.Exists(__fFilePath) == true)
            {
                string[] vFileContent = File.ReadAllLines(__fFilePath, Encoding.Default); // Построчное содержание файла
                foreach (string vLine in vFileContent)
                {
                    if (vLine.Length > 0)
                    {
                        int vSeparatorPosition = vLine.IndexOf('='); // Позиция разделителя выражений [ = ]
                        if (vSeparatorPosition > 0) // Разделитель обнаружен
                            try
                            {
                                vReturn.Add(vLine.Substring(0, vSeparatorPosition).Trim(), vLine.Substring(vSeparatorPosition + 1).Trim());
                            }
                            catch
                            { }
                    }
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Чтение словаря из файла с сортировкой по ключу (Первый параметр 'SortedDictionary')
        /// </summary>
        /// <returns>SortedDictionary<string, string></returns>
        public SortedDictionary<string, string> __mLoadSorted()
        {
            SortedDictionary<string, string> vReturn = new SortedDictionary<string, string>(); // Возвращаемое значение
            /// Если файл существует, выполняется перебор всех строк и запись в 'Dictionary'
            if (File.Exists(__fFilePath) == true)
            {
                string[] vFileContent = File.ReadAllLines(__fFilePath, Encoding.Default); // Построчное содержание файла
                foreach (string vLine in vFileContent)
                {
                    if (vLine.Length > 0)
                    {
                        int vSeparatorPosition = vLine.IndexOf('='); // Позиция разделителя выражений [ = ]
                        if (vSeparatorPosition > 0) // Разделитель обнаружен
                            try
                            {
                                vReturn.Add(vLine.Substring(0, vSeparatorPosition).Trim(), vLine.Substring(vSeparatorPosition + 1).Trim());
                            }
                            catch
                            { }
                    }
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Запись словаря в файл
        /// </summary>
        /// <param name="pDictionary">Словарь</param>
        public void __mSave(Dictionary<string, string> pDictionary)
        {
            appFileText vFileText = new appFileText(); // Объект для работы с текстовыми файлами
            /// Если файл уже существует - он удаляется
            if (File.Exists(__fFilePath) == true)
                File.Delete(__fFilePath);
            foreach (string vKey in pDictionary.Keys)
            {
                string vValue = "";
                pDictionary.TryGetValue(vKey, out vValue);
                vFileText.__mWriteToEnd(__fFilePath, vKey.Trim() + " = " + vValue.Trim());
            }

            return;
        }
        /// <summary>
        /// Запись словаря в файл с сортировкой по ключу
        /// </summary>
        /// <param name="pDictionary">Словарь</param>
        public void __mSaveSorted(SortedDictionary<string, string> pDictionary)
        {
            appFileText vFileText = new appFileText(); // Объект для работы с текстовыми файлами
            /// Если файл уже существует - он удаляется
            if (File.Exists(__fFilePath) == true)
                File.Delete(__fFilePath);
            foreach (string vKey in pDictionary.Keys)
            {
                string vValue = "";
                pDictionary.TryGetValue(vKey, out vValue);
                vFileText.__mWriteToEnd(__fFilePath, vKey.Trim() + " = " + vValue.Trim());
            }

            return;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Аргументы

        /// <summary>
        /// Путь и имя файла
        /// </summary>
        public string __fFilePath = "";

        #endregion Аргументы

        #endregion ПОЛЯ
    }
}
