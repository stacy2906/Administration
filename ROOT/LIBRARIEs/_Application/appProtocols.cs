using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace nlApplication
{
    /// <summary>
    /// Файл appProtocols.cs
    /// </summary>
    /// <remarks>Класс приложения для работы с протоколами приложения</remarks>
    /// <conception>Lucasin V.</conception>
 	/// <version>2026.01.13 10-26</version> // Дата-время последней корректировки
    public class appProtocols
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Создание нового протокола
        /// </summary>
        /// <param name="pProtocolType">Вид записи в протоколе</param>
        /// <param name="pProcedure">Название процедуры в котором возникло событие</param>
        public virtual void __mCreate(PROTOCOLSTYPES pProtocolType, string pProcedure, bool pPrintScreen = false)
        {
            __mCreate(pProtocolType, pProcedure, DateTime.Now, GetFGuid());
            return;
        }

        public virtual string GetFGuid()
        {
            return appApplication.__fErrorLast;
        }

        /// <summary>
        /// Создание нового протокола
        /// </summary>
        /// <param name="pProtocolType">Вид записи в протоколе</param>
        /// <param name="pProcedure">Название процедуры в котором возникло событие</param>
        /// <param name="pDateTime">Дата и время возникновения события</param>
        public virtual void __mCreate(PROTOCOLSTYPES pProtocolType, string pProcedure, DateTime pDateTime, string fGuid, bool pPrintScreen = false)
        {
            DataTable vDataTable = new DataTable();
            appApplication.__fErrorLast = Guid.NewGuid().ToString();

            vDataTable.Columns.Add("CHG", typeof(string)); // Время создания записи
            vDataTable.Columns.Add("GID", typeof(string)); // Уникальный идентификатор
            vDataTable.Columns.Add("App", typeof(string)); // Приложение
            vDataTable.Columns.Add("AppDpn", typeof(string)); // Описание приложения
            vDataTable.Columns.Add("Pfx", typeof(string)); // Префикс
            vDataTable.Columns.Add("Hst", typeof(string)); // Хост
            vDataTable.Columns.Add("HstAnt", typeof(string)); // Аккаунт хоста
            vDataTable.Columns.Add("lnkCpu", typeof(string)); // Ссылка: Компьютер
            vDataTable.Columns.Add("lnkPclTyp", typeof(string)); // Ссылка: Вид протокола
            vDataTable.Columns.Add("lnkUsr", typeof(string)); // Ссылка: Пользователь
            vDataTable.Columns.Add("Prc", typeof(string)); // Название процедуры
            vDataTable.Columns.Add("Fil", typeof(string)); // Название файла изображения экрана

            DataRow vDataRow = vDataTable.NewRow();

            /// Формируется имя файла, чтобы при переходе времени за полночь, выполнялась запись в новый файл
            fFilePath = appApplication.__oPathes.__fFileProtocol_;
            fFilePathPrintScreen = "";
            DateTime vDateTime = DateTime.Now;
            //int fProtocolType = 0; // Вид протокола

            vDataRow["CHG"] = vDateTime.Ticks.ToString(); // Время создания записи
            vDataRow["GID"] = appApplication.__fErrorLast; // Уникальный идентификатор
            vDataRow["App"] = appApplication.__fProcessName_; // Название приложения
            vDataRow["AppDpn"] = appApplication.__fDescription_; // Название приложения
            vDataRow["Pfx"] = appApplication.__fPrefix_; // Префикс приложения
            vDataRow["Hst"] = Environment.MachineName; // Хост
            vDataRow["HstAnt"] = Environment.UserName; // Аккаунт хоста
            vDataRow["Prc"] = pProcedure; // Процедура

            /// Определение идентификатора вида протокола
            switch (pProtocolType)
            {
                case PROTOCOLSTYPES.ApplicationError:
                    vDataRow["lnkPclTyp"] = "1";
                    //fProtocolType = 1;
                    break;
                case PROTOCOLSTYPES.ApplicationErrorProgramatic:
                    vDataRow["lnkPclTyp"] = "2";
                    //fProtocolType = 2;
                    break;
                case PROTOCOLSTYPES.ApplicationException:
                    vDataRow["lnkPclTyp"] = "3";
                    //fProtocolType = 3;
                    break;
                case PROTOCOLSTYPES.ApplicationEvent:
                    vDataRow["lnkPclTyp"] = "4";
                    //fProtocolType = 4;
                    break;
                case PROTOCOLSTYPES.DataError:
                    vDataRow["lnkPclTyp"] = "5";
                    //fProtocolType = 5;
                    break;
                case PROTOCOLSTYPES.DataEvent:
                    vDataRow["lnkPclTyp"] = "6";
                    //fProtocolType = 6;
                    break;
                case PROTOCOLSTYPES.DeviceError:
                    vDataRow["lnkPclTyp"] = "7";
                    //fProtocolType = 7;
                    break;
                case PROTOCOLSTYPES.DeviceEvent:
                    vDataRow["lnkPclTyp"] = "8";
                    //fProtocolType = 8;
                    break;
                case PROTOCOLSTYPES.UserError:
                    vDataRow["lnkPclTyp"] = "9";
                    //fProtocolType = 9;
                    break;
                case PROTOCOLSTYPES.UserEvent:
                    vDataRow["lnkPclTyp"] = "10";
                    //fProtocolType = 10;
                    break;
                case PROTOCOLSTYPES.UserMessage:
                    vDataRow["lnkPclTyp"] = "11";
                    //fProtocolType = 11;
                    break;
                case PROTOCOLSTYPES.Other:
                    vDataRow["lnkPclTyp"] = "12";
                    //fProtocolType = 12;
                    break;
            }
            /// Формирование файла изображения экрана
            if (pPrintScreen == true)
                vDataRow["Fil"] = __mPrintScreen();

            if (File.Exists(fFilePath) == false)
            {
                appFileCsv vFileCsv = new appFileCsv();
                vDataTable.Rows.Add(vDataRow);
                vFileCsv.__mDataTable2Csv(fFilePath, vDataTable);
            }
            else
            {
                appFileText vFileText = new appFileText(); // Класс для работы с файлом
                vFileText.__mWriteToEnd(fFilePath,
                vDataRow["CHG"] + "," // Время создания записи
                + vDataRow["GID"] + ","  // Уникальный идентификатор
                + vDataRow["App"] + "," // Приложение
                + vDataRow["AppDpn"] + "," // Описание приложения
                + vDataRow["Pfx"] + "," // Префикс
                + vDataRow["Hst"] + "," // Хост
                + vDataRow["HstAnt"] + "," // Аккаунт хоста
                + vDataRow["lnkCpu"] + "," // Ссылка: Компьютер
                + vDataRow["lnkPclTyp"] + "," // Ссылка: Вид протокола
                + vDataRow["lnkUsr"] + "," // Ссылка: Пользователь
                + vDataRow["Prc"] + "," // Название процедуры
                + vDataRow["Fil"]);
            }
            /// Задержка времени для создания изображения экрана без формы сообщения, возможно добавить звуковой сигнал

            return;
        }
        /// <summary>
        /// Создание записи в протоколе
        /// </summary>
        /// <param name="pRecordType">Вид записи в протоколе</param>
        /// <param name="pRecordText">Текст записи</param>
        /// <param name="pTick">Количество тиков затраченных на выполнение операции</param>
        public virtual void __mRecord(PROTOCOLRECORDSTYPES pRecordType, string pRecordText, long pTick = -1)
        {
            DataTable vDataTable = new DataTable();
            DateTime vDateTime = DateTime.Now;

            vDataTable.Columns.Add("CHG", typeof(string)); // Время создания записи
            vDataTable.Columns.Add("GID", typeof(string)); // Уникальный идентификатор
            vDataTable.Columns.Add("lnkPcl", typeof(string)); // Ссылка: Протокол
            vDataTable.Columns.Add("lnkPclRrdTyp", typeof(string)); // Ссылка: Вид записи в протоколе
            vDataTable.Columns.Add("Msg", typeof(string)); // Сообщение
            vDataTable.Columns.Add("Tck", typeof(string)); // Затраченное время

            DataRow vDataRow = vDataTable.NewRow();

            vDataRow["CHG"] = vDateTime.Ticks.ToString(); // Время создания записи
            vDataRow["GID"] = Guid.NewGuid().ToString(); // Уникальный идентификатор
            vDataRow["lnkPcl"] = appApplication.__fErrorLast; // Идентификатор протокола
            vDataRow["Msg"] = pRecordText; // Префикс приложения
            vDataRow["Tck"] = pTick; // Аккаунт хоста

            fFilePath = Path.Combine(Path.GetDirectoryName(appApplication.__oPathes.__fFileProtocol_)
                , Path.GetFileNameWithoutExtension(appApplication.__oPathes.__fFileProtocol_) + "rrd" +
                Path.GetExtension(appApplication.__oPathes.__fFileProtocol_));

            /// Определение идентификаторв вида записи в протоколе
            switch (pRecordType)
            {
                case PROTOCOLRECORDSTYPES.Answer:
                    vDataRow["lnkPclRrdTyp"] = "0";
                    break;
                case PROTOCOLRECORDSTYPES.Detail:
                    vDataRow["lnkPclRrdTyp"] = "1";
                    break;
                case PROTOCOLRECORDSTYPES.Exception:
                    vDataRow["lnkPclRrdTyp"] = "2";
                    break;
                case PROTOCOLRECORDSTYPES.Image:
                    vDataRow["lnkPclRrdTyp"] = "3";
                    break;
                case PROTOCOLRECORDSTYPES.Message:
                    vDataRow["lnkPclRrdTyp"] = "4";
                    break;
                case PROTOCOLRECORDSTYPES.ObjectProperty:
                    vDataRow["lnkPclRrdTyp"] = "5";
                    break;
                case PROTOCOLRECORDSTYPES.Reason:
                    vDataRow["lnkPclRrdTyp"] = "6";
                    break;
                default:
                    vDataRow["lnkPclRrdTyp"] = "4";
                    break;
            }

            //vDataRow["AppDpn"] = appApplication.__fDescription_; // Название приложения

            if (File.Exists(fFilePath) == false)
            {
                appFileCsv vFileCsv = new appFileCsv();
                vDataTable.Rows.Add(vDataRow);
                vFileCsv.__mDataTable2Csv(fFilePath, vDataTable);
            }
            else
            {
                appFileText vFileText = new appFileText(); // Класс для работы с файлом
                vFileText.__mWriteToEnd(fFilePath,
                vDataRow["CHG"] + "," // Время создания записи
                + vDataRow["GID"] + ","  // Уникальный идентификатор
                + vDataRow["lnkPcl"] + "," // Ссылка: Протокол
                + vDataRow["lnkPclRrdTyp"] + "," // Ссылка: Вид записи в протоколе
                + vDataRow["Msg"] + "," // Сообщение
                + vDataRow["Tck"]); // Затраченное время
            }

            return;
        }
        /// <summary>
        /// Создание файла изображения экрана
        /// </summary>
        public virtual string __mPrintScreen()
        {
            string vReturn = ""; // Возвращаемое значение

            Bitmap vBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics vGraphics = Graphics.FromImage(vBitmap as Image);
            vGraphics.CopyFromScreen(0, 0, 0, 0, vBitmap.Size);
            fFilePathPrintScreen = appApplication.__oPathes.__fDirectoryProtocolsImages_ + appTypeDateTime.__mDateTimeToFileNameTillSecond(DateTime.Now) + ".jpg";
            vBitmap.Save(fFilePathPrintScreen, ImageFormat.Jpeg);
            vReturn = fFilePathPrintScreen;

            return vReturn;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Закрытые

        /// <summary>
        /// Имя файла протокола
        /// </summary>
        private string fFilePath = "";
        /// <summary>
        /// Имя файла изображения PrintScreen
        /// </summary>
        private string fFilePathPrintScreen = "";
        /// <summary>
        /// Идентификатор протокола
        /// </summary>
        //public static string __fGuid = "";

        #endregion Закрытые

        #endregion ПОЛЯ
    }
}
