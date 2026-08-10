using nlAdministartion;
using nlApplication;
using nlData;
using System.Data;

namespace nlAdministration
{
    /// <summary>
    /// Файл admEventsHandler.cs
    /// </summary>
    /// <remarks>Класс - Обработчик основных событий пакета программ "Administration"</remarks>
    public class admEventsHandler : appEventsHandler
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Начало выполнения приложения
        /// </summary>
        /// <returns>[true] - Приложение готово к выполнению, иначе - [false]</returns>
        public override bool __mBegin()
        {
            bool vReturn = true; // Возвращаемое значение

            #region Создание настроек приложения

            admApplication.__oTunes.__mNew(admApplication.__oTunes.__mTranslate("Интерфейс")
                        , "Language"
                        , "Russian"
                        , admApplication.__oTunes.__mTranslate("Язык интнрфейса")
                        , "Russian, English, Polish, Romain"
                        , ""
                        , "elmInputComboList"
                        , true
                        , true
                        , true);
            admApplication.__oTunes.__mNew(admApplication.__oTunes.__mTranslate("Интерфейс")
                        , "AskForQuit"
                        , "true"
                        , admApplication.__oTunes.__mTranslate("Спрашивать о закрытии приложения")
                        , ""
                        , ""
                        , "elmInputBool"
                        , true
                        , true
                        , true);
            admApplication.__oTunes.__mNew(admApplication.__oTunes.__mTranslate("Данные")
                        , "Server"
                        , ""
                        , admApplication.__oTunes.__mTranslate("Название сервера")
                        , ""
                        , ""
                        , "elmInputString"
                        , true
                        , true
                        , true);
            admApplication.__oTunes.__mNew(admApplication.__oTunes.__mTranslate("Данные")
                        , "ServerDatabase"
                        , ""
                        , admApplication.__oTunes.__mTranslate("Название базы данных")
                        , ""
                        , ""
                        , "elmInputString"
                        , true
                        , true
                        , true);
            admApplication.__oTunes.__mNew(admApplication.__oTunes.__mTranslate("Данные")
                        , "ServerLogin"
                        , ""
                        , admApplication.__oTunes.__mTranslate("Логин сервера")
                        , ""
                        , ""
                        , "elmInputString"
                        , true
                        , true
                        , true);
            admApplication.__oTunes.__mNew(admApplication.__oTunes.__mTranslate("Данные")
                        , "ServerPassword"
                        , ""
                        , admApplication.__oTunes.__mTranslate("Пароль логина")
                        , ""
                        , ""
                        , "elmInputString"
                        , true
                        , true
                        , true);
            admApplication.__oTunes.__mNew(admApplication.__oTunes.__mTranslate("Данные")
                        , "DatabaseProtocols"
                        , ""
                        , admApplication.__oTunes.__mTranslate("Путь к базе данных 'Protocols'")
                        , ""
                        , ""
                        , "elmInputPath"
                        , true
                        , true
                        , true);
            #endregion Создание настроек приложения

            vReturn = vReturn & base.__mBegin();

            if (vReturn == false)
                return false;

            /// Подключение источника данных для работы с протоколами
            /// Создание источника данных 'Protocols':
            admDataSourceProtocols vDataSourceProtocols = new admDataSourceProtocols();
            /// Указание базы данных
            vDataSourceProtocols.__fDatabasePath = "..\\..\\..\\..\\..\\DATABASEs\\";

            /// Указание названия базы данных
            vDataSourceProtocols.__fDatabaseName = "Protocols.db";
            /// Назначение псевдонима источнику данных 'Protocols'
            vDataSourceProtocols.__fAlias = "Protocols";
            /// Хранение данных в базе данных в тиках
			vDataSourceProtocols.__fDateTimeStore = DATETIMESTORE.Ticks;
            admApplication.__oData.__mDataSourceAdd(vDataSourceProtocols);
            //admApplication.__oData.__fDataSourceCurrentAlias = vDataSourceProtocols.__fAlias;


            DataTable vDataTable = admApplication.__oData.__mDataSourceGet("Protocols").__mSqlQuery("Select * From Pcl");

            /// Подключение источников данных UNA
            {
                ///// Создание источника данных 'Administration':
                //admDataSourceUna vDataSourceAdministration = new admDataSourceUna();
                ///// - Хранение времени в типе данных {datetime}
                //vDataSourceAdministration.__fDateTimeStore = DATETIMESTORE.DateTime;
                ///// - Назначатся псевдоним 'Administration'
                //vDataSourceAdministration.__fAlias = "Administration";
                ///// - Основная база данных
                //vDataSourceAdministration.__fDatabaseName = "Administration";
                ///// - База данных - серверная
                //vDataSourceAdministration.__fLocalDB = false;
                ///// - Запретить постоянное подключение
                //vDataSourceAdministration.__fOnLine = false;
                //vDataSourceAdministration.__fServer = admApplication.__oTunes.__mTuneRead("Server").Length > 0 ? admApplication.__oTunes.__mTuneRead("Server") : @"OIT6\LUSTAR";
                //vDataSourceAdministration.__fServerLogin = admApplication.__oTunes.__mTuneRead("ServerLogin").Length > 0 ? admApplication.__oTunes.__mTuneRead("ServerLogin") : ""; /// - Логин сервера, если нет в файле настроек, береться рабочий
                //vDataSourceAdministration.__fServerPassword = admApplication.__oTunes.__mTuneRead("ServerPassword").Length > 0 ? admApplication.__oTunes.__mTuneRead("ServerPassword") : ""; /// - Пароль логина, если нет в файле настроек, береться рабочий
                //admApplication.__oData.__mDataSourceAdd(vDataSourceAdministration);

                ///// Назначение источника данных используемым по умолчанию
                //admApplication.__oData.__fDataSourceCurrentAlias = vDataSourceAdministration.__fAlias;

                //vDataSourceAdministration.__mModelBuild();
            }
            /// Регистрация пользователя в источнике данных UNA
            {
                ///// Вызов формы регистрации пользователя
                //elmFormLogin vFormLogin = new elmFormLogin();
                //vFormLogin.__fDataSourceAlias = vDataSourceAdministration.__fAlias;
                //vFormLogin.ShowDialog();
                ///// Если регистрация пользователя прошла, удаляем его зависшие блокировки
                //if (vFormLogin.__fRegistered == true)
                //{
                //	/// - Снятие зависших блокировок для зашедшего пользователя
                //	admApplication.__oData.__mDataSourceGet().__mLockClear();
                //}
                //else
                //	vReturn = false;
            }

            return vReturn;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ
    }
}

