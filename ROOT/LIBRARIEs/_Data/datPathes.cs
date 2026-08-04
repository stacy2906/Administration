using nlApplication;
using System;
using System.IO;

namespace nlData
{
    /// <summary>
    /// Файл datPathes.cs
    /// </summary>
    /// <remarks>Класс приложения для работы с путями приложения</remarks>
  	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 14-23</version> // Дата-время последней корректировки
    public class datPathes : appPathes
    {
        #region = МЕТОДЫ

        #region - Файлы

        /// <summary>
        /// Формирование имени файла страховой копии базы данных
        /// </summary>
        /// <param name="pDatabaseName">Название базы данных</param>
        /// <param name="pExtension">Расширение копии файла</param>
        /// <returns>Путь и имя файла копии базы данных</returns>
        public string __mFileDataBaseBackUp(string pDatabaseName, string pExtension)
        {
            string vFileName = datApplication.__fPrefix_ + "_" + pDatabaseName + "_" + appTypeDateTime.__mDateTimeToFileNameTillSecond(DateTime.Now) + "." + pExtension;
            return Path.Combine(__fDirectoryDataBackUp_, vFileName);
        }

        #endregion Файлы

        #endregion МЕТОДЫ

        #region = СВОЙСТВА

        #region - Папки

        /// <summary>
        /// Путь и имя папки для размещения файлов данных
        /// </summary>
        public string __fDirectoryData_
        {
            get
            {
                if (fDirectoryData.Length == 0)
                {
                    fDirectoryData = Path.Combine(__fDirectoryStart, "Data\\");
                }
                try
                {
                    if (Directory.Exists(fDirectoryData) == false)
                    {
                        Directory.CreateDirectory(fDirectoryData);
                    }
                }
                catch { }

                return fDirectoryData;

            }
            set
            {
                fDirectoryData = value.Trim();
            }
        }
        /// <summary>
        /// Путь и имя папки для размещения копий файлов данных
        /// </summary>
        public string __fDirectoryDataBackUp_
        {
            get
            {
                if (fDirectoryDataBackUp.Length == 0)
                {
                    fDirectoryDataBackUp = Path.Combine(__fDirectoryData_, "BackUp\\");
                }
                try
                {
                    if (Directory.Exists(fDirectoryDataBackUp) == false)
                    {
                        Directory.CreateDirectory(fDirectoryDataBackUp);
                    }
                }
                catch { }

                return fDirectoryDataBackUp;

            }
            set
            {
                fDirectoryDataBackUp = value.Trim();
            }
        }
        /// <summary>
        /// Путь и имя папки для размещения файлов с данными и ответами для отправки 
        /// </summary>
        public string __fDirectoryDataForSending_
        {
            get
            {
                if (fDirectoryDataForSending.Length == 0)
                {
                    fDirectoryDataForSending = Path.Combine(__fDirectoryData_, "Sending\\");
                }
                try
                {
                    if (Directory.Exists(fDirectoryDataForSending) == false)
                    {
                        Directory.CreateDirectory(fDirectoryDataForSending);
                    }
                }
                catch { }

                return fDirectoryDataForSending;

            }
            set
            {
                fDirectoryDataForSending = value.Trim();
            }
        }
        /// <summary>
        /// Путь и имя папки для размещения полученных файлов с данными и ответов
        /// </summary>
        public string __fDirectoryDataReceived_
        {
            get
            {
                if (fDirectoryDataReceived.Length == 0)
                {
                    fDirectoryDataReceived = Path.Combine(__fDirectoryData_, "Received\\");
                }
                try
                {
                    if (Directory.Exists(fDirectoryDataReceived) == false)
                    {
                        Directory.CreateDirectory(fDirectoryDataReceived);
                    }
                }
                catch { }

                return fDirectoryDataReceived;

            }
            set
            {
                fDirectoryDataReceived = value.Trim();
            }
        }

        #endregion Папки

        #endregion СВОЙСТВА

        #region = ПОЛЯ

        #region - Скрытые

        /// <summary>
        /// Путь и имя папки для размещения файлов запросов
        /// </summary>
        protected string _fDirectoryQueries = "";

        #endregion Скрытые

        #region - Закрытые

        /// <summary>
        /// Путь и имя папки для размещения файлов данных
        /// </summary>
        private string fDirectoryData = "";
        /// <summary>
        /// Путь и имя папки для размещения копий файлов данных
        /// </summary>
        private string fDirectoryDataBackUp = "";
        /// <summary>
        /// Путь и имя папки для размещения файлов с данными и ответами для отправки 
        /// </summary>
        private string fDirectoryDataForSending = "";
        /// <summary>
        /// Путь и имя папки для размещения файлов с данными и ответами для отправки 
        /// </summary>
        private string fDirectoryDataReceived = "";

        #endregion Закрытые

        #endregion ПОЛЯ
    }
}
