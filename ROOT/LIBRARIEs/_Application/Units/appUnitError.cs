using System;
using System.Collections;

namespace nlApplication
{
    /// <summary>
    /// Файл appUnitError.cs
    /// </summary>
    /// <remarks>Класс-единица 'Ошибка приложения'</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.01.29 18-15</version> // Дата-время последней корректировки
    /// <example>
    /* В коде метода:
       _fError.__fErrorType_ = ERRORSTYPES.Programming;
       _fError.__fProcedure_ = _fClassProcedure_;
       _fError.__fLineInProcedure_ = _fClassLine_;
       _fError.__fHelpFileName_ = "";
       _fError.__fHelpTopic_ = "";
       _fError.__mPropertyAdd("Параметр - Путь к файлу: {0}", pFilePath);
       _fError.__mReasonAdd("Полученный файл отсутствует");
       _fError.__fMessage_ = "Не возможно измерить размер файла";
       appApplication.__oErrorsHandler.__mShow(_fError);
       _fError.__mClear();
     */
    /* Объвление в конструкторе
       {
            _fError = new appUnitError(_fClassFilePath_);
       }
     */

    /// </example>
    public class appUnitError
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        /// <param name="pFilePath"></param>
        public appUnitError()
        {
        }
        /// <summary>
        /// Конструктор с указанием пути к файлу в котором возникла ошибка
        /// </summary>
        /// <param name="pFilePath"></param>
        public appUnitError(string pFilePath) 
        {
            fFilePath = pFilePath;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Подготовка для обработки следующей ошибки
        /// </summary>
        public void __mClear()
        {
            __fException = null;
            fHelpFileName = "";
            fHelpTopic = "";
            fLineInProcedure = -1;
            fMessage = "";
            fMessageNotTranslate = "";
            fProcedure = "";
            fPropertieS = new ArrayList();
            fReasonS = new ArrayList();
        }
        /// <summary>
        /// Перевод сообщения об ошибке на язык интерфейса приложения и подключение параметров
        /// </summary>
        /// <param name="pMessage">Сообщение об ошибке</param>
        /// <param name="pParameterS">Дополнительные, не переводимые на язык интерфейса приложения, параметры</param>
        /// <remarks>Используется, когда в сообщении нужно передать не переводимые данные, когда все выражение переводиться воспользуйтесь свойством '__fMessage'</remarks>
        /// <example>
        /// vError.__mMessageBuild("Не удалось удалить файл {0}", vFilePath);
        /// </example>
        public void __mMessageBuild(string pMessage, params object[] pParameterS)
        {
            fMessageNotTranslate = String.Format(pMessage, pParameterS);
            fMessage = appApplication.__oTunes.__mTranslate(pMessage, pParameterS);

            return;
        }
        /// <summary>
        /// Сохранение свойства класса объекта в котором возникла ошибка, которое может влиять на возникновение ошибки
        /// </summary>
        /// <param name="pProperty">Название свойства класса объекта в котором произошла ошибка</param>
        /// <param name="pParameterS">Значение свойства класса объекта</param>
        /// <example>
        /// vError.__mPropertyAdd("__fCaption_: {0}", pParameter);
        /// </example>
        public void __mPropertyAdd(string pProperty, params object[] pParameterS)
        {
            fPropertieS.Add(String.Format(pProperty, pParameterS));
            return;
        }
        /// <summary>
        /// Перевод причины возникновения ошибки на язык интерфейса приложения и подключение значений при необходимости
        /// </summary>
        /// <param name="pErrorReason">Причины возникновения ошибки</param>
        /// <param name="pParameterS">Дополнительные не переводимые параметры</param>
        /// <example>
        /// vError.__mReasonAdd("Файл {0} отсутсвует", pFilePath);
        /// </example>
        public void __mReasonAdd(string pErrorReason, params object[] pParameterS)
        {
            fReasonS.Add(appApplication.__oTunes.__mTranslate(pErrorReason, pParameterS));
            return;
        }
        /// <summary>
        /// Перевод списка причин возникновения ошибки на язык интерфейса приложения и подключение значений при необходимости
        /// </summary>
        /// <param name="pErrorReasonS">Причины возникновения ошибки</param>
        /// <example>
        /// vError.__mReasonAdd(pErrorReasonS);
        /// </example>
        public void __mReasonSAdd(ArrayList pErrorReasonS)
        {
            foreach (string vString in pErrorReasonS)
            {
                fReasonS.Add(appApplication.__oTunes.__mTranslate(vString));
            }

            return;
        }

        #endregion Процедуры 

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Исключение
        /// </summary>
        public Exception __fException;

        #endregion Атрибуты

        #region - Закрытые

        /// <summary>
        /// Вид ошибки
        /// </summary>
        private ERRORSTYPES fErrorType = ERRORSTYPES.Application;
        /// <summary>
        /// Путь к файлу в котором произошла ошибка
        /// </summary>
        private string fFilePath = "";
        /// <summary>
        /// Название файла помощи используемого для описания ошибки
        /// </summary>
        private string fHelpFileName = "";
        /// <summary>
        /// Название топика описывающего ошибку
        /// </summary>
        private string fHelpTopic = "";
        /// <summary>
        /// Номер строки в процедуре в которой создается этот класс
        /// </summary>
        private int fLineInProcedure = -1;
        /// <summary>
        /// Сообщение об ошибке, переведенное на язык интерфейса приложения
        /// </summary>
        private string fMessage = "";
        /// <summary>
        /// Сообщение об ошибке не переведенное на язык интерфейса
        /// </summary>
        /// <remarks>Только чтение</remarks>
        private string fMessageNotTranslate = "";
        /// <summary>
        /// Название процедуры в которой возникла ошибка
        /// </summary>
        private string fProcedure = "";
        /// <summary>
        /// Список элементов объекта в котором возникли ошибки, могущих иметь влияние на возникновение ошибки
        /// </summary>
        private ArrayList fPropertieS = new ArrayList();
        /// <summary>
        /// Список причин возникновения ошибки
        /// </summary>
        private ArrayList fReasonS = new ArrayList();

        #endregion Закрытые

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Вид ошибки
        /// </summary>
        public ERRORSTYPES __fErrorType_
        {
            get { return fErrorType; }
            set { fErrorType = value; }
        }
        /// <summary>
        /// Путь к файлу в котором произошла ошибка
        /// </summary>
        public string __fFilePath_
        {
            get { return fFilePath; }
            set { fFilePath = value.Trim(); }
        }
        /// <summary>
        /// Название файла помощи используемого для описания ошибки
        /// </summary>
        public string __fHelpFileName_
        {
            get { return fHelpFileName; }
            set { fHelpFileName = value.Trim(); }
        }
        /// <summary>
        /// Название топика описывающего ошибку
        /// </summary>
        public string __fHelpTopic_
        {
            get { return fHelpTopic; }
            set { fHelpTopic = value.Trim(); }
        }
        /// <summary>
        /// Сообщение об ошибке. Переводится на язык интерфейса
        /// </summary>
        public string __fMessage_
        {
            get { return fMessage; }
            set
            {
                fMessageNotTranslate = value;
                fMessage = appApplication.__oTunes.__mTranslate(value);
            }
        }
        /// <summary>
        /// Номер строки в процедуре в которой создается этот класс
        /// </summary>
        public int __fLineInProcedure_
        {
            get { return fLineInProcedure; }
            set { fLineInProcedure = value; }
        }
        /// <summary>
        /// Сообщение об ошибке не переведенное на язык интерфейса
        /// </summary>
        /// <remarks>Только чтение</remarks>
        public string __fMessageNotTranslate_
        {
            get { return fMessageNotTranslate; }
        }
        /// <summary>
        /// Название процедуры в которой возникла ошибка
        /// </summary>
        public string __fProcedure_
        {
            get { return fProcedure; }
            set { fProcedure = value.Trim(); }
        }
        /// <summary>
        /// Список элементов объекта в котором возникли ошибки, могущих иметь влияние на возникновение ошибки
        /// </summary>
        /// <remarks>Только для чтения</remarks>
        public ArrayList __fPropertieS_
        {
            get { return fPropertieS; }
        }
        /// <summary>
        /// Список причин возникновения ошибки
        /// </summary>
        /// <remarks>Только для чтения</remarks>
        public ArrayList __fReasonS_
        {
            get { return fReasonS; }
        }

        #endregion СВОЙСТВА
    }
}
