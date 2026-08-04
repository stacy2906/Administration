using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace nlApplication
{
    /// <summary>
    /// Файл appEventsHandler.cs
    /// </summary>
    /// <remarks>Класс приложения для работы с основными событиями приложения</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.01.13 12-56</version> // Дата-время последней корректировки
    public class appEventsHandler
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public appEventsHandler()
        {
            _fError = new appUnitError(_fClassFilePath_);
        }
        
        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Процедуры

        #region * Информация о файле

        /// <summary>
        /// Получение пути и имени файла класса
        /// </summary>
        protected string _mClassFilePath([CallerFilePath] string pFilePath = "")
        {
            return pFilePath;
        }
        /// <summary>
        /// Получение значения номера строки в текущей процедуре
        /// </summary>
        protected int _mClassLine(string pMessage = "", [CallerLineNumber] int pLine = 0)
        {
            return pLine;
        }
        /// <summary>
        /// Получение названия текущей процедуры
        /// </summary>
        protected string _mClassProcedure(string message, [CallerMemberName] string pMember = "")
        {
            return pMember;
        }

        #endregion Информация о файле

        /// <summary>
        /// Начало выполнения приложения
        /// </summary>
        /// <returns>[true] - приложение готово к выполнению, иначе - [false]</returns>
        public virtual bool __mBegin()
        {
            bool vReturn = true; // Возвращаемое значение
            _fError.__fMessage_ = "Приложение не готово к выполнению"; // Сообщение об ошибке
            /// Удаление временных файлов предыдущего сеанса, если они не удалились перед закрытием приложения
            appApplication.__oPathes.__mFilesTempDelete();
            /// Загрузка настроек приложения
            appApplication.__oTunes.__mLoad();

            #region /// Проверка указания версии приложения

#if DEBUG
            //if (Convert.ToInt32(appTypeString.__mWordNumberDot(appApplication.__fVersion_, 0)) == DateTime.Now.Year)
            //{
            //    if (Convert.ToInt32(appTypeString.__mWordNumberDot(appApplication.__fVersion_, 1)) == DateTime.Now.Month)
            //    {
            //        if (Convert.ToInt32(appTypeString.__mWordNumberDot(appApplication.__fVersion_, 2)) != DateTime.Now.Day)
            //            _fError.__mReasonAdd("Не верно указана версия приложения");
            //    }
            //    else
            //        _fError.__mReasonAdd("Не верно указана версия приложения");
            //}
            //else
            //    _fError.__mReasonAdd("Не верно указана версия приложения");
#endif
            /// Отображение ошибки если версия не указана или указана не верно
            if (_fError.__fReasonS_.Count > 0)
            {
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return false;
            }

            #endregion Проверка указания версии приложения

            /// Протоколирование начала выполнения приложения
            appApplication.__oProtocols.__mCreate(PROTOCOLSTYPES.ApplicationEvent, "_mBegin()");
            appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Message, appApplication.__oTunes.__mTranslate("Начало выполнения приложения"), -1);

            return vReturn;
        }
        /// <summary>
        /// Завершение выполнения приложения
        /// </summary>
        public virtual void __mEnd()
        {
            /// Удаление временных файлов текущего сеанса
            appApplication.__oPathes.__mFilesTempDelete();
            /// Сохранение настроек приложения
            appApplication.__oTunes.__mSave();
            /// Протоколирование завершения выполнения приложения
            appApplication.__oProtocols.__mCreate(PROTOCOLSTYPES.ApplicationEvent, "_mEnd()");
            appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Message, appApplication.__oTunes.__mTranslate("Завершение выполнения приложения"), -1);
        }
        /// <summary>
        /// Отображения топика помощи вызываемого из файла помощи приложения
        /// </summary>
        /// <param name="pHelpTopicName">Название топика</param>
        public void __mHelp(string pHelpTopicName)
        {
            __mHelp(appApplication.__fHelpFileName_, pHelpTopicName);
        }
        /// <summary>
        /// Вызов топика помощи из указанного файла помощи
        /// </summary>
        /// <param name="pHelpFileName">Путь и имя файла</param>
        /// <param name="pHelpTopicName">Название топика</param>
        public void __mHelp(string pHelpFileName, string pHelpTopicName)
        {
            string vHelpFileName = Path.Combine(appApplication.__oPathes.__fDirectoryHelp_, pHelpFileName); // Полный путь и имя файла

            if (pHelpFileName.Length == 0)
                _fError.__mReasonAdd("Файл помощи не определен");

            if (File.Exists(vHelpFileName) == false)
                _fError.__mReasonAdd("Файл помощи '{0}' отсутствует", pHelpFileName);

            if (_fError.__fReasonS_.Count > 0)
            {
                _fError.__mMessageBuild("Не возможно отобразить помощь");
                _fError.__mPropertyAdd("Имя файла помощи '{0}'", pHelpFileName);
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return;
            }

            ProcessStartInfo vProcssInfo;
            if (pHelpTopicName.Length == 0)
                vProcssInfo = new ProcessStartInfo("hh.exe", "mk:@MSITStore:" + vHelpFileName); // Открытие файла помощи
            else
                vProcssInfo = new ProcessStartInfo("hh.exe", "mk:@MSITStore:" + vHelpFileName + "::/" + pHelpTopicName); // Открытие топика помощи
            Process vProcess = new Process();
            vProcssInfo.UseShellExecute = false;
            vProcess.StartInfo = vProcssInfo;
            vProcess.Start();
        }
        /// <summary>
        /// Приостановка работы программы
        /// </summary>
        /// <param name="pSeconds">Время в секундах на которое нужно приостановить работу программы</param>
        /// <remarks>Работа приложения приостанавливается, оно не реагирует на внешние события и действия пользователя</remarks>
        public void __mPause(int pSeconds)
        {
            int vUnit = 1000;
            vUnit = vUnit * pSeconds;
            System.Threading.Thread.Sleep(vUnit);
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Скрытые

        /// <summary>
        /// Путь к файлу текущего класса
        /// </summary>
        protected string _fClassFilePath_
        {
            get { return _mClassFilePath(); }
        }
        /// <summary>
        /// Текущая процедура класса
        /// </summary>
        protected string _fClassProcedure_
        {
            get { return _mClassProcedure(""); }
        }
        /// <summary>
        /// Номер строки в файле класса
        /// </summary>
        protected int _fClassLine_
        {
            get { return _mClassLine(""); }
        }

        #endregion Скрытые

        #region - Объекты

        /// <summary>
        /// Объект ошибки
        /// </summary>
        protected appUnitError _fError;

        #endregion Объекты

        #endregion ПОЛЯ
    }
}
