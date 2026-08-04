using nlApplication;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.ServiceProcess;

namespace nlSystem
{
    /// <summary>
    /// Файл ssmServices.cs
    /// </summary>
    /// <remarks>
    // vServiceController.DisplayName
    // "Служба маршрутизатора AllJoyn"
    // vServiceController.CanStop
    // false
    // vServiceController.Status
    // Stopped
    // vServiceController.CanPauseAndContinue
    // false
    // vServiceController.CanShutdown
    // false
    // vServiceController.DependentServices
    // {System.ServiceProcess.ServiceController[0]
    // }
    // vServiceController.GetType
    // { Method = { System.Type GetType()} }
    // Method: { System.Type GetType()}
    // Target: { System.ServiceProcess.ServiceController}
    // vServiceController.MachineName
    // "."
    // vServiceController.ServiceHandle
    // "vServiceController.ServiceHandle" выдал исключение типа "System.InvalidOperationException"
    //     Data: { System.Collections.ListDictionaryInternal}
    // HResult: -2146233079
    //     HelpLink: null
    //     InnerException: { "Отказано в доступе"}
    // Message: "Не удалось открыть службу AJRouter на компьютере '.'."
    //     Source: "System.ServiceProcess"
    //     StackTrace: "   at System.ServiceProcess.ServiceController.GetServiceHandle(Int32 desiredAccess)\r\n   at System.ServiceProcess.ServiceController.get_ServiceHandle()"
    //     TargetSite: { IntPtr GetServiceHandle(Int32)}
    // vServiceController.ServiceName
    // "AJRouter"
    // vServiceController.ServicesDependedOn
    // { System.ServiceProcess.ServiceController[0]}
    // vServiceController.ServiceType
    // Win32ShareProcess
    // vServiceController.StartType
    // Manual
    // vServiceController.WaitForStatus
    // error CS8917: Не удалось вывести тип делегата.
    // Close
    // Continue
    // Dispose
    // ExecuteCommand
    // Pause
    // Refresh
    // Start
    // StartType
    // Stop
    // WaitForStatus
    //  </remarks>
    //  <remarks>Класс для работы со службами</remarks>
    public class sstServices
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public sstServices()
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
        /// Получение списка служб установленных на компьютере
        /// </summary>
        public ArrayList __mList()
        {
            ServiceController[] vServiceControllerS = ServiceController.GetServices();
            ArrayList vReturn = new ArrayList();
            foreach (ServiceController vServiceController in vServiceControllerS)
            {
                vReturn.Add(vServiceController.ServiceName);
            }
            /// Сортировка по алфавиту
            IComparer myComparer = new MyServiceComparer();
            vReturn.Sort(myComparer);

            return vReturn;
        }
        /// <summary>
        /// Получение объекта службы
        /// </summary>
        /// <param name="pServiceName"></param>
        public ServiceController __mGetService(string pServiceName)
        {
            try
            {
                return new ServiceController(pServiceName);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Закрытие службы
        /// </summary>
        /// <param name="pServiceName">Название службы</param>
        /// <returns>[true] - служба закрыта, иначе - [false]</returns>
        public bool __mClose(string pServiceName)
        {
            bool vReturn = true; // Возвращаемое значение

            _fError.__fMessage_ = "Не удалось закрыть службу";
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Параметр - Название службы: {0}", pServiceName);
            _fError.__mPropertyAdd("Статус службы: {0}", __mStatus(pServiceName));

            /// Если статус службы не равен 1 (Stopped), фиксируется ошибка
            if (__mStatus(pServiceName) != 1)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Статус службы не соответствует вполняемой операции");
                goto Exit;
            }
            /// Попытка закрытия службы
            try
            {
                ServiceController vServiceController = __mGetService(pServiceName);
                if (vServiceController != null)
                {
                    vServiceController.Close();
                    vServiceController.Dispose();
                }
            }
            catch (Exception vException)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fException = vException;
                _fError.__mReasonAdd(vException.Message);
            }
        Exit:
            /// Если обнаружены зафиксируемые ошибки, выводиться сообщение об ошибке 
            if (_fError.__fPropertieS_.Count > 0)
            {
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                vReturn = false;
            }

            return vReturn;
        }
        /// <summary>
        /// Продолжение выполнения приостановленной службы
        /// </summary>
        /// <param name="pServiceName">Название службы</param>
        /// <returns>[true] - служба приостановлена, иначе - [false]</returns>
        public bool __mContinue(string pServiceName)
        {
            bool vReturn = true; // Возвращаемое значение

            _fError.__fMessage_ = "Не удалось продолжить выполнение службы";
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Параметр - Название службы: {0}", pServiceName);
            _fError.__mPropertyAdd("Статус службы: {0}", __mStatus(pServiceName));

            /// Если статус службы не равен 7 (Paused), фиксируется ошибка
            if (__mStatus(pServiceName) != 7)
            {
                _fError.__fLineInProcedure_ = _fClassLine_; 
                _fError.__mReasonAdd("Статус службы не соответствует вполняемой операции");
                goto Exit;
            }
            /// Попытка продолжение работы службой
            try
            {
                ServiceController vServiceController = __mGetService(pServiceName);
                if (vServiceController != null)
                {
                    vServiceController.Continue();
                }
            }
            catch (Exception vException)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fException = vException;
                _fError.__mReasonAdd(vException.Message);
            }
        Exit:
            /// Если обнаружены зафиксируемые ошибки, выводиться сообщение об ошибке 
            if (_fError.__fPropertieS_.Count > 0)
            {
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                vReturn = false;
            }

            return vReturn;
        }
        /// <summary>
        /// Выполнение команды службой
        /// </summary>
        /// <param name="pServiceName">Название службы</param>
        /// <param name="pCommandCode">Код команды передаваемой службе</param>
        /// <returns>[true] - Команда выполнена, иначе - [false]</returns>
        public bool __mExecuteCommand(string pServiceName, int pCommandCode)
        {
            bool vReturn = true; // Возвращаемое значение

            _fError.__fMessage_ = "Службе не удалось выполнить команду";
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Параметр - Название службы: {0}", pServiceName);
            _fError.__mPropertyAdd("Параметр - Код команды: {0}", pCommandCode);
            _fError.__mPropertyAdd("Статус службы: {0}", __mStatus(pServiceName));
            /// Если код не находиться в допустимых пределах, фиксируется ошибка
            if (pCommandCode < 128 | pCommandCode > 256)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Код команды за допустимыми пределами");
                goto Exit;
            }
            /// Если статус службы не равен 4 (Running), фиксируется ошибка
            if (__mStatus(pServiceName) != 4)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Статус службы не соответствует вполняемой операции");
                goto Exit;
            }
            /// Попытка выполнения команды службой
            try
            {
                ServiceController vServiceController = __mGetService(pServiceName);
                if (vServiceController != null)
                {
                    vServiceController.ExecuteCommand(pCommandCode);
                }
            }
            catch (Exception vException)
            {
                _fError.__fException = vException;
                _fError.__mReasonAdd(vException.Message);
            }
        Exit:
            /// Если обнаружены зафиксируемые ошибки, выводиться сообщение об ошибке 
            if (_fError.__fPropertieS_.Count > 0)
            {
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                vReturn = false;
            }

            return vReturn;
        }
        public bool __mMashineName(string pServiceName)
        {
            return false;
        }
        /// <summary>
        /// Приостановка выполнения службы
        /// </summary>
        /// <param name="pServiceName"></param>
        /// <returns></returns>
        public bool __mPause(string pServiceName)
        {
            bool vReturn = true; // Возвращаемое значение

            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fMessage_ = "Службе не удалось выполнить команду";
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Служба: {0}", pServiceName);
            _fError.__mPropertyAdd("Статус службы: {0}", __mStatus(pServiceName));

            /// Если статус службы не равен 4 (Running), фиксируется ошибка
            if (__mStatus(pServiceName) != 4)
            {
                _fError.__fLineInProcedure_ = _fClassLine_; 
                _fError.__mReasonAdd("Статус службы не соответствует вполняемой операции");
                goto Exit;
            }
            /// Попытка выполнения команды службой
            try
            {
                ServiceController vServiceController = __mGetService(pServiceName);
                if (vServiceController != null)
                {
                    vServiceController.Pause();
                }
            }
            catch (Exception vException)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fException = vException;
                _fError.__mReasonAdd(vException.Message);
            }

        Exit:
            /// Если обнаружены зафиксируемые ошибки, выводиться сообщение об ошибке 
            if (_fError.__fPropertieS_.Count > 0)
            {
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                vReturn = false;
            }

            return vReturn;
        }
        public bool __mServiceHandle(string pServiceName)
        {
            return false;
        }
        public bool __mServiceType(string pServiceName)
        {
            return false;
        }
        /// <summary>
        /// Запуск службы
        /// </summary>
        /// <param name="pServiceName">Название службы</param>
        /// <returns></returns>
        public bool __mStart(string pServiceName)
        {
            bool vReturn = true; // Возвращаемое значение

            _fError.__fMessage_ = "Не удалось запустить службу";
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Параметр - Название службы: {0}", pServiceName);
            _fError.__mPropertyAdd("Статус службы: {0}", __mStatus(pServiceName));

            /// Если статус службы не равен 1 (Stopped), фиксируется ошибка
            if (__mStatus(pServiceName) != 1)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Статус службы не соответствует вполняемой операции");
                goto Exit;
            }
            /// Попытка выполнения команды службой
            try
            {
                ServiceController vServiceController = __mGetService(pServiceName);
                if (vServiceController != null)
                {
                    vServiceController.Start();
                }
            }
            catch (Exception vException)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fException = vException;
                _fError.__mReasonAdd(vException.Message);
            }

        Exit:
            /// Если обнаружены зафиксируемые ошибки, выводиться сообщение об ошибке 
            if (_fError.__fPropertieS_.Count > 0)
            {
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                vReturn = false;
            }

            return vReturn;
        }
        public bool __mStartType(string pServiceName)
        {
            return false;
        }
        /// <summary>
        /// Получение текущего состояния службы
        /// </summary>
        /// <param name="pServiceName">Название службы</param>
        /// <returns>Возвращается код статуса:
        /// = 1 (Stopped),         Служба не запущена.Это соответствует константе SERVICE_STOPPED Win32, равной 0x00000001.
        /// = 2 (StartPending),    Служба запускается.Это соответствует константе SERVICE_START_PENDING Win32, равной 0x00000002.
        /// = 3 (StopPending),     Служба останавливается. Это соответствует константе SERVICE_STOP_PENDING Win32, равной 0x00000003.
        /// = 4 (Running),         Служба запущена. Это соответствует константе SERVICE_RUNNING Win32, равной 0x00000004.
        /// = 5 (ContinuePending), Ожидается возобновление работы службы. Это соответствует константе SERVICE_CONTINUE_PENDING Win32, равной 0x00000005.
        /// = 6 (PausePending),    Ожидается приостановка службы. Это соответствует константе SERVICE_PAUSE_PENDING Win32, равной 0x00000006.
        /// = 7 (Paused),          Служба приостановлена. Это соответствует константе SERVICE_PAUSED Win32, равной 0x00000007.
        /// </returns>
        public int __mStatus(string pServiceName)
        {
            int vReturn = -1; // Возвращаемое значение

            _fError.__fMessage_ = "Не удалось получить статус службы";
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Параметр - Название службы: {0}", pServiceName);

            ServiceController vServiceController = __mGetService(pServiceName);
            try
            {
                vReturn = (int)vServiceController.Status;
            }
            catch (InvalidOperationException vException)
            {
                _fError.__fLineInProcedure_ = _fClassLine_; 
                _fError.__mReasonAdd("Исключение 'InvalidOperationException': " + vException.Message);
                goto Exit;
            }
            catch (Exception vException)
            {
                _fError.__fLineInProcedure_ = _fClassLine_; 
                _fError.__mReasonAdd("Исключение 'Exception': " + vException.Message);
                goto Exit;
            }

        Exit:
            /// Если обнаружены ошибки, возвращается 
            if (_fError.__fPropertieS_.Count > 0)
            {
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                vReturn = -1;
            }

            return vReturn;
        }
        public bool __mStatusCanStop(string pServiceName)
        {
            return false;
        }
        public bool __mStatusCanPauseAndContinue(string pServiceName)
        {
            return false;
        }
        public bool __mStatusCanShutdown(string pServiceName)
        {
            return false;
        }
        /// <summary>
        /// Остановка службы
        /// </summary>
        /// <param name="pServiceName">Название службы</param>
        /// <returns></returns>
        public bool __mStop(string pServiceName)
        {
            bool vReturn = true; // Возвращаемое значение

            _fError.__fMessage_ = "Не удалось остановить службу";
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Служба: {0}", pServiceName);
            _fError.__mPropertyAdd("Статус службы: {0}", __mStatus(pServiceName));

            /// Если статус службы не равен 4 (Running) или 7 (Paused), фиксируется ошибка
            if (__mStatus(pServiceName) != 4 & __mStatus(pServiceName) != 4)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Статус службы не соответствует вполняемой операции");
                goto Exit;
            }
            /// Попытка выполнения команды службой
            try
            {
                ServiceController vServiceController = __mGetService(pServiceName);
                if (vServiceController != null)
                {
                    vServiceController.Stop();
                }
            }
            catch (Exception vException)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__fException = vException;
                _fError.__mReasonAdd(vException.Message);
            }

        Exit:
            /// Если обнаружены зафиксируемые ошибки, выводиться сообщение об ошибке 
            if (_fError.__fPropertieS_.Count > 0)
            {
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                vReturn = false;
            }

            return vReturn;
        }
        public bool __mWaitForStatus(string pServiceName)
        {
            return false;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Скрытые

        /// <summary>
        /// Объект ошибки
        /// </summary>
        protected appUnitError _fError;

        #endregion Скрытые

        #endregion ПОЛЯ

        #region = СВОЙСТВА

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

        #endregion = СВОЙСТВА
    }

    public class MyServiceComparer : IComparer
    {
        #region IComparer Members

        public int Compare(object s1, object s2)
        {
            //provides reverse abc sorting
            return ((ServiceController)s2).ServiceName.CompareTo(((ServiceController)s1).ServiceName);
        }

        #endregion
    }
}
