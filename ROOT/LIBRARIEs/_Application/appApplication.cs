using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace nlApplication
{
    #region = ПЕРЕЧИСЛЕНИЯ

    /// <summary>
    /// Вид ошибки
    /// </summary>
    public enum ERRORSTYPES
    {
        /// <summary>
        /// Ошибка приложения
        /// </summary>
        Application = 0,
        /// <summary>
        /// Ошибка источника данных
        /// </summary>
        Data = 1,
        /// <summary>
        /// Ошибка устройства
        /// </summary>
        Device = 2,
        /// <summary>
        /// Критическая ошибка
        /// </summary>
        Exception = 3,
        /// <summary>
        /// Ошибка программирования
        /// </summary>
        Programming = 4,
        /// <summary>
        /// Ошибка пользователя
        /// </summary>
        User = 5
    }
    /// <summary>
    /// Виды сообщений
    /// </summary>
    public enum MESSAGESTYPES
    {
        /// <summary>
        /// Ошибка
        /// </summary>
        Error = 0,
        /// <summary>
        /// Ошибка с повтором
        /// </summary>
        ErrorRetry = 1,
        /// <summary>
        /// Информация
        /// </summary>
        Info = 2,
        /// <summary>
        /// Вид не определен
        /// </summary>
        None = 3,
        /// <summary>
        /// Вопрос к пользователю
        /// </summary>
        Question = 4,
        /// <summary>
        /// Предупреждение
        /// </summary>
        Warning = 5
    }
    /// <summary>
    /// Виды протоколов
    /// </summary>
    public enum PROTOCOLSTYPES
    {
        /// <summary>
        /// Ошибка приложения
        /// </summary>
        ApplicationError = 0,
        /// <summary>
        /// Критическая ошибка приложения
        /// </summary>
        ApplicationException = 1,
        /// <summary>
        /// Ошибка программирования
        /// </summary>
        ApplicationErrorProgramatic = 2,
        /// <summary>
        /// Событие приложения
        /// </summary>
        ApplicationEvent = 3,
        /// <summary>
        /// Ошибка источника данных
        /// </summary>
        DataError = 4,
        /// <summary>
        /// Событие источника данных - изменение полей
        /// </summary>
        DataEvent = 5,
        /// <summary>
        /// Ошибка устройства
        /// </summary>
        DeviceError = 6,
        /// <summary>
        /// Событие устройства
        /// </summary>
        DeviceEvent = 7,
        /// <summary>
        /// Ошибка пользователя
        /// </summary>
        UserError = 8,
        /// <summary>
        /// Действия пользователя
        /// </summary>
        UserEvent = 9,
        /// <summary>
        /// Сообщения отображенные пользователю
        /// </summary>
        UserMessage = 10,
        /// <summary>
        /// Прочие события
        /// </summary>
        Other = 11
    }
    /// <summary>
    /// Виды записей в протоколе
    /// </summary>
    public enum PROTOCOLRECORDSTYPES
    {
        /// <summary>
        /// Решение пользователя
        /// </summary>
        Answer = 0,
        /// <summary>
        /// Детали события
        /// </summary>
        Detail = 1,
        /// <summary>
        /// Исключения
        /// </summary>
        Exception = 2,
        /// <summary>
        /// Изображение
        /// </summary>
        Image = 3,
        /// <summary>
        /// Сообщение
        /// </summary>
        Message = 4,
        /// <summary>
        /// Свойство объекта
        /// </summary>
        ObjectProperty = 5,
        /// <summary>
        /// Причина возникновения ошибки
        /// </summary>
        Reason = 6
    }

    #endregion ПЕРЕЧИСЛЕНИЯ

    /// <summary>
    /// Файл appApplication.cs
    /// </summary>
    /// <remarks>Класс-базис приложения</remarks>
    /// <example>
    /* Использование метода __mGetTypesInNamespace:
       Type[] vEssenseS = appApplication.__mGetTypesInNamespace(Assembly.GetExecutingAssembly(), "nlCabinet"); // Список классов сущностей
       string fFileName = "c_";
       foreach (Type vType in vEssenseS)
       {
            if (appTypeString.__mExpressionInExpression(vType.Name, "Essence") > 0)
            {
                string v = vType.Name;
                datUnitEssence vEssence = (datUnitEssence)Activator.CreateInstance(vType);
                (vEssence as datUnitEssence).__mDistributionsExport(DateTime.Now, fFileName);
            }
       }
     */
    /// </example>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.29 18-12</version> // Дата-время последней корректировки
    public class appApplication
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Получение списка классов в приложении
        /// </summary>
        /// <param name="pAssembly">Имя сборки в которой выполняется поиск классов</param>
        /// <param name="pNameSpace">Пространство имен в котором выполняется поиск классов</param>
        /// <returns>Type[]</returns>
        public static Type[] __mGetTypesInNamespace(Assembly pAssembly, string pNameSpace)
        {
            return pAssembly.GetTypes().Where(t => String.Equals(t.Namespace, pNameSpace, StringComparison.Ordinal)).ToArray();
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Имя и расширение файла помощи
        /// </summary>
        public static string __fHelpFileName_ = "";
        /// <summary>
        /// Пакет приложений которому принадлежит приложение
        /// </summary>
        public static string __fPacket_ = "";
        /// <summary>
        /// Префикс файлов используемых в приложении и создаваемых приложением
        /// </summary>
        public static string __fPrefix_ = "";

        public static string __fErrorLast = "";

        #endregion Атрибуты

        #region - Объекты

        /// <summary>
        /// Объект для обработки ошибок
        /// </summary>
        public static appErrorsHandler __oErrorsHandler = new appErrorsHandler();
        /// <summary>
        /// Объект для обработки основных событий приложения
        /// </summary>
        public static appEventsHandler __oEventsHandler = new appEventsHandler();
        /// <summary>
        /// Объект для отображения сообщений пользователю
        /// </summary>
        public static appMessages __oMessages = new appMessages();
        /// <summary>
        /// Объект для работы с путями приложения
        /// </summary>
        public static appPathes __oPathes = new appPathes();
        /// <summary>
        /// Объект для протоколирования событий приложения
        /// </summary>
        public static appProtocols __oProtocols = new appProtocols();
        /// <summary>
        /// Объект для работы с настройками приложения
        /// </summary>
        public static appTunes __oTunes = new appTunes();
        /// <summary>
        /// Объект интерфейса
        /// </summary>
        public static appInterfaceAbout __oInterfaceAbout;

        #endregion Объекты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Заголовок главного окна приложения
        /// </summary>
        public static string __fCaption_
        {
            get
            {
                FileVersionInfo vFileVersionInfo = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location.ToString());
                return vFileVersionInfo.ProductName;
            }
        }
        /// <summary>
        /// Чтение комментария из 'AssemblyInfo-AssemblyDescription' 
        /// </summary>
        /// <remarks>Указывается на английском языке, есть возможность перевода на язык интерфейса</remarks>
        public static string __fDescription_
        {
            get
            {
                FileVersionInfo vFileVersionInfo = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location.ToString());
                return appApplication.__oTunes.__mTranslate(vFileVersionInfo.Comments);
            }
        }
        /// <summary>
        /// Чтение владельца из 'AssemblyInfo-AssemblyCompany' 
        /// </summary>
        /// <remarks>Не переводится</remarks>
        public static string __fOwner_
        {
            get
            {
                FileVersionInfo vFileVersionInfo = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location.ToString());
                return vFileVersionInfo.CompanyName;
            }
        }
        /// <summary>
        /// Идентификатор текущего процесса
        /// </summary>
        public static int __fProcessClue_
        {
            get { return (int)Process.GetCurrentProcess().Id; }
        }
        /// <summary>
        /// Название процесса выполняемой программы
        /// </summary>
        public static string __fProcessName_
        {
            get
            {
                return System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            }
        }
        /// <summary>
        /// Определение торговой марки из 'AssemblyInfo-AssemblyTrademark'
        /// </summary>
        /// <remarks>Не переводится</remarks>
        public static string __fTradeMark_
        {
            get
            {
                FileVersionInfo vFileVersionInfo = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location.ToString());
                return vFileVersionInfo.LegalTrademarks;
            }
        }
        /// <summary>
        /// Определение версии приложения из 'AssemblyInfo-AssemblyVersion'
        /// </summary>
        public static string __fVersion_
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
     

        #endregion СВОЙСТВА
    }
}
