using System;
using System.Windows.Forms;

namespace nlApplication
{
    /// <summary>
    /// Файл appMessages.cs
    /// </summary>
    /// <remarks>Класс приложения для работы с сообщениями приложения</remarks>
    /// <conception>Lucasin V.</conception>
    /// <version>2026.01.13 00-00</version>
    public class appMessages
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Перевод выражений на язык пользователя и отображение сообщения пользователю
        /// </summary>
        /// <param name="pMessageType">Вид сообщения</param>
        /// <param name="pMessageText">Текст сообщения</param>
        /// <returns>{ DialogResult }</returns>
        public virtual DialogResult __mShow(MESSAGESTYPES pMessageType, string pMessageText)
        {
            return __mShow(pMessageType, pMessageText, "", false, "", "");
        }
        /// <summary>
        /// Перевод выражений на язык пользователя и отображение сообщения пользователю
        /// </summary>
        /// <param name="pMessageType">Вид сообщения</param>
        /// <param name="pMessageText">Текст сообщения</param>
        /// <param name="pMessageDetails">Детали сообщения</param>
        /// <param name="pProcedure">Процедура выдавшая сообщение</param>
        /// <returns>{ DialogResult }</returns>
        public virtual DialogResult __mShow(MESSAGESTYPES pMessageType, string pMessageText, string pMessageDetails = "", string pProcedure = "")
        {
            return __mShow(pMessageType, pMessageText, pMessageDetails, false, "", pProcedure);
        }
        /// <summary>
        /// Перевод выражений на язык пользователя и отображение сообщения пользователю
        /// </summary>
        /// <param name="pMessageType">Вид сообщения</param>
        /// <param name="pMessageText">Текст сообщения</param>
        /// <param name="pMessageDetails">Детали сообщения</param>
        /// <param name="pCheckStatus">Состояние checkbox</param>
        /// <param name="pCheckText">Текст checkbox</param>
        /// <param name="pProcedure">Процедура выдавшая сообщение</param>
        /// <returns>{ DialogResult }</returns>
        public virtual DialogResult __mShow(MESSAGESTYPES pMessageType, string pMessageText, string pMessageDetails = "", bool pCheckStatus = false, string pCheckText = "", string pProcedure = "")
        {
            DialogResult vReturn = DialogResult.None; // Возвращаемое значение

            /// Если запрещено отображение сообщения пользователю, выход из метода
            if (__fShowForUser == false)
                return vReturn;

            MessageBoxButtons vMessageButton = MessageBoxButtons.OK; // Используемые кнопки
            MessageBoxDefaultButton vMessageButtonDefault = MessageBoxDefaultButton.Button3; // Кнопка по умолчанию
            MessageBoxIcon vMessageIcon = MessageBoxIcon.None; // Используемая кнопка
            switch (pMessageType)
            {
                case MESSAGESTYPES.Error:
                    vMessageButton = MessageBoxButtons.OK;
                    vMessageButtonDefault = MessageBoxDefaultButton.Button3;
                    vMessageIcon = MessageBoxIcon.Error;
                    break;
                case MESSAGESTYPES.ErrorRetry:
                    vMessageButton = MessageBoxButtons.RetryCancel;
                    vMessageButtonDefault = MessageBoxDefaultButton.Button2;
                    vMessageIcon = MessageBoxIcon.Error;
                    break;
                case MESSAGESTYPES.Info:
                    vMessageButton = MessageBoxButtons.OK;
                    vMessageButtonDefault = MessageBoxDefaultButton.Button3;
                    vMessageIcon = MessageBoxIcon.Information;
                    break;
                case MESSAGESTYPES.Question:
                    vMessageButton = MessageBoxButtons.YesNo;
                    vMessageButtonDefault = MessageBoxDefaultButton.Button2;
                    vMessageIcon = MessageBoxIcon.Question;
                    pMessageText = pMessageText.Trim() + "?";
                    break;
                case MESSAGESTYPES.Warning:
                    vMessageButton = MessageBoxButtons.OK;
                    vMessageButtonDefault = MessageBoxDefaultButton.Button3;
                    vMessageIcon = MessageBoxIcon.Warning;
                    break;
            }
            /// Отображение сообщения
            vReturn = MessageBox.Show(pMessageText + "\n" + pMessageDetails
                                , appApplication.__fProcessName_
                                , vMessageButton
                                , vMessageIcon
                                , vMessageButtonDefault);
            /// Протоколирование сообщения
            appApplication.__oProtocols.__mCreate(PROTOCOLSTYPES.UserMessage, pProcedure, DateTime.Now,
            /// Протоколирование сообщения
            appApplication.__oProtocols.GetFGuid());
            appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Answer, vReturn.ToString());
            appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Message, pMessageText);

            return vReturn;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты 

        /// <summary>
        /// Разрешение отображения сообщения пользователю
        /// </summary>
        public bool __fShowForUser = true;

        #endregion Атрибуты

        #endregion ПОЛЯ    
    }
}
