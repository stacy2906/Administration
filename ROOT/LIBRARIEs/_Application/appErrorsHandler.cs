using System.Data;
using System.Windows.Forms;

namespace nlApplication
{
    /// <summary>
    /// Файл appErrorsHandler.cs
    /// </summary>
    /// <remarks>Класс для работы с ошибками приложения</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 12-57</version> // Дата-время последней корректировки
    public class appErrorsHandler
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Протоколирование ошибки описанной в 'appUnitError'
        /// </summary>
        /// <param name="pUnitError">Ошибка приложения</param>
        public virtual void __mProtocol(appUnitError pUnitError)
        {
            PROTOCOLSTYPES vProtocolType = PROTOCOLSTYPES.ApplicationError; // Вид протокола

            /// Выделение вида ошибки для фиксации в заголовке протокола
            switch (pUnitError.__fErrorType_)
            {
                case ERRORSTYPES.Application:
                    vProtocolType = PROTOCOLSTYPES.ApplicationError;
                    break;
                case ERRORSTYPES.Data:
                    vProtocolType = PROTOCOLSTYPES.DataError;
                    break;
                case ERRORSTYPES.Device:
                    vProtocolType = PROTOCOLSTYPES.DeviceError;
                    break;
                case ERRORSTYPES.Exception:
                    vProtocolType = PROTOCOLSTYPES.ApplicationException;
                    break;
                case ERRORSTYPES.Programming:
                    vProtocolType = PROTOCOLSTYPES.ApplicationErrorProgramatic;
                    break;
                case ERRORSTYPES.User:
                    vProtocolType = PROTOCOLSTYPES.UserError;
                    break;
            }
            /// Создание заголовка протокола
            appApplication.__oProtocols.__mCreate(vProtocolType, pUnitError.__fProcedure_);

            #region /// Создание записей в протоколе

            ///*- Протоколирование сообщения пользователя
            appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Message, pUnitError.__fMessage_, -1);
            ///*- Протоколирование причин возникновения ошибки
            if (pUnitError.__fReasonS_.Count > 0)
                foreach (string vReason in pUnitError.__fReasonS_)
                    appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Detail, vReason, -1);
            ///*- Протоколирование сведений об объекте в котором возникла ошибка
            if (pUnitError.__fPropertieS_.Count > 0)
                foreach (string vProperty in pUnitError.__fPropertieS_)
                    appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.ObjectProperty, vProperty, -1);
            ///*- Протоколирование исключения
            if (pUnitError.__fException != null)
            {
                string vStackTrace = pUnitError.__fException.StackTrace.Trim(); // Свойство исключения 'StackTrace'
                int vIndex = vStackTrace.ToLower().IndexOf(":строка");
                appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Exception, "LineNumber:" + pUnitError.__fException.StackTrace.Trim().Substring(vIndex + 7).Trim(), -1);
                vIndex = vStackTrace.Trim().ToLower().Substring(2).IndexOf("в ");
                appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Exception, "Procedure:" + pUnitError.__fException.StackTrace.Trim().Substring(2, vIndex).Trim(), -1);
                //string vLine = StackTrace.Trim();
                vIndex = vStackTrace.ToLower().IndexOf(" в ");
                vStackTrace = vStackTrace.Trim().Substring(vIndex + 2).Trim(); // vLine + "" + vIndex.ToString(); // 
                vIndex = vStackTrace.ToLower().IndexOf(":строка");
                //return vStackTrace.Trim().Substring(0, vIndex).Trim();
                appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Exception, "File:" + vStackTrace.Trim().Substring(0, vIndex).Trim(), -1);

                appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Exception, "Message:" + pUnitError.__fMessage_, -1);
                appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Exception, "TargetSite:" + pUnitError.__fException.TargetSite, -1);
                appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Exception, "Source:" + pUnitError.__fException.Source, -1);
                appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Exception, "StackTrace:" + pUnitError.__fException.StackTrace, -1);
                appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Exception, "HelpLink:" + pUnitError.__fException.HelpLink, -1);
            }

            #endregion Создание записей в протоколе

            return;
        }
        /// <summary>
        /// Протоколирование и отображение сообщения об ошибке пользователю из объекта ошибки
        /// </summary>
        /// <param name="pUnitError">Объект ошибки</param>
        public virtual DialogResult __mShow(appUnitError pUnitError)
        {
            /// Протоколирование ошибки
            __mProtocol(pUnitError);
            string vMessage = pUnitError.__fMessage_ + "."; // Текст ошибки
            string vMessageDetails = ""; // Текст деталей ошибки
            /// Добавление названия файла
            if (pUnitError.__fProcedure_.Length > 0)
            {
                vMessageDetails = vMessageDetails + "\n" + appApplication.__oTunes.__mTranslate("Файл{0} {1}", ":", pUnitError.__fFilePath_) + "\n"; // Текст с путем к файлу в котором возникла ошибка
            }
            /// Добавление названия процедуры к сообщению
            if (pUnitError.__fProcedure_.Length > 0)
            {
                vMessageDetails = vMessageDetails + "\n" + appApplication.__oTunes.__mTranslate("Процедура{0} {1}", ":", pUnitError.__fProcedure_) + "\n"; // Текст с описанием процедуры в которой возникла ошибка
            }
            /// Добавление номера строки
            vMessageDetails = vMessageDetails + "\n" + appApplication.__oTunes.__mTranslate("Строка{0} {1}", ":", pUnitError.__fLineInProcedure_) + "\n"; 
            /// Добавление причин возникновения ошибок к сообщению:
            if (pUnitError.__fReasonS_.Count > 0)
            {
                vMessageDetails = "\n" + appApplication.__oTunes.__mTranslate("Причины:") + "\n";
                ///* - Перебор причин возникновения ошибок и добавление их к сообщению
                foreach (string pMessageParameter in pUnitError.__fReasonS_)
                    vMessageDetails = vMessageDetails + "    - " + pMessageParameter + ".\n";
            }
            /// Добавление сведений об объекте в котором возникла ошибка к сообщению:
            if (pUnitError.__fPropertieS_.Count > 0)
            {
                vMessageDetails = vMessageDetails + "\n" + appApplication.__oTunes.__mTranslate("Сведения:") + "\n"; // Текст с описанием свойств объекта
                ///* - Перебор сведений об объекте и добавление их к сообщению
                foreach (string pMessageParameter in pUnitError.__fPropertieS_)
                    vMessageDetails = vMessageDetails + "    - " + pMessageParameter + ".\n";
            }
            /// Добавление сообщения исключения
            if (pUnitError.__fException != null)
            {
                vMessageDetails += "\n" + appApplication.__oTunes.__mTranslate("Исключение:") + "\n";
                vMessageDetails = vMessageDetails + pUnitError.__fException.Message;
            }

            return __mShowMessage(MESSAGESTYPES.Error, vMessage, vMessageDetails, pUnitError.__fProcedure_); // Возвращаемое значение
        }
        /// <summary>
        /// Вывод на экран сообщения об ошибке. Для возможности замены стандартного окна VStudio
        /// </summary>
        /// <param name="pMessageType">Вид ошибки</param>
        /// <param name="pMessage">Сообщение</param>
        /// <param name="pMessageDetails">Детали сообщения</param>
        /// <param name="pProcedure">Процедура</param>
        /// <returns>Решение пользователя</returns>
        public virtual DialogResult __mShowMessage(MESSAGESTYPES pMessageType, string pMessage, string pMessageDetails, string pProcedure)
        {
            return appApplication.__oMessages.__mShow(MESSAGESTYPES.Error, pMessage, pMessageDetails, pProcedure); // Возвращаемое значение
        }

        #endregion Процедуры

        #endregion МЕТОДЫ    
    }
}
