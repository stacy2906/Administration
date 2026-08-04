using nlApplication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nlData
{
    internal class datDataChange
    {
        #region = МЕТОДЫ

        /// <summary>
        /// Получение времени последней отправки
        /// </summary>
        public DateTime __mDateTimeLastSend()
        {
             return Convert.ToDateTime(__fDataSourceAdministartion.__mSqlQuery("Select dtmChgSndCrn From ChgSnd Where dtmChgRcvRmt != (CONVERT([datetime],'01.01.1900'))"));
        }
        /// <summary>
        /// Формирование файлов для отправки
        /// </summary>
        public void __mCreateFilesForSend()
        {
            DateTime vDateTimeLastSend = __mDateTimeLastSend(); // Время последних уже отправленных данных
            ArrayList vTablesList = __fDataSourceChanged.__mTablesList(); // Список таблиц в отсылаемой таблице
            foreach (string vTableName in vTablesList)
            {
                DataTable vDataTableChanges = __fDataSourceChanged.__mSqlQuery("Select * From " + vTableName + " Where CHG > " + appTypeDateTime.__mMsSqlDateTimeToString(vDateTimeLastSend) + " Order By CHG");
                vDataTableChanges.WriteXml(Path.Combine(__fPathFolderForSend, vTableName + ".xml"));
            }
            /// Создание архива
            /// Отправка архива по почте
            /// Создание записи об отправке данных
        }
        public void __mApplyChangesFiles()
        {
            /// Разархивировать файл
        }
        #endregion МЕТОДЫ

        #region = ПОЛЯ

        /// <summary>
        /// Источник данных административной базы данных
        /// </summary>
        public datUnitDataSource __fDataSourceAdministartion;
        /// <summary>
        /// Источник данных пересылаемой базы данных
        /// </summary>
        public datUnitDataSource __fDataSourceChanged;
        /// <summary>
        /// Путь и имя папки для входящих данных
        /// </summary>
        public string __fPathFolderForReceive = "";
        /// <summary>
        /// Путь и имя папки для исходящих данных
        /// </summary>
        public string __fPathFolderForSend = "";
        /// <summary>
        /// Название сервера получателя
        /// </summary>
        public string __fServerReceiver = "";
        /// <summary>
        /// Название сервера отправителя
        /// </summary>
        public string __fServerSender = "";

        #endregion ПОЛЯ
    }
}
