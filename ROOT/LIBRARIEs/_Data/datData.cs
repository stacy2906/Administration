using nlApplication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;

namespace nlData
{
    /// <summary>
    /// Файл datData.cs
    /// </summary>
    /// <remarks>Класс для работы с источниками данных</remarks>
 	/// <author>Lucasin V.</author> // Автор
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 14-23</version> // Дата-время последней корректировки
    public class datData
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public datData()
        {
            _mObjectAssembly();
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected void _mObjectAssembly()
        {
            _fError = new appUnitError(_fClassFilePath_);
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            return;
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

        #region * Sql операции

        /// <summary>
        /// Отправка команды источнику данных
        /// </summary>
        /// <param name="pCommand">Команда отправляемая источнику данных</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>Количество обработанных командой записей</returns>
        public int __mSqlCommand(string pCommand, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Параметр - Команда {0} {1}", ":", pCommand);
            _fError.__mPropertyAdd("Источник данных {0} {1}", ":", pDataSourceAlias);
            /// Проверка указания команды
            if (String.IsNullOrEmpty(pCommand) == true)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Не указана команда");
            }
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return -1;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mSqlCommand(pCommand);
        }
        /// <summary>
        /// Выполнение хранимой процедуры источника данных
        /// </summary>
        /// <param name="pStoredProcedure">Название хранимой процедуры</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных</param>
        /// <param name="pParameters">Список параметров</param>
        public DataTable __mSqlProcedures(string pStoredProcedure, string pDataSourceAlias = "", params object[] pParameters)
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Параметр - Источник данных{0} {1}", ":", pDataSourceAlias);
            _fError.__mPropertyAdd("Хранимая процедура{0} {1}", ":", pStoredProcedure);
            foreach (object vObject in pParameters)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mPropertyAdd("Параметр{0} {1}", ":", vObject.ToString());
            }
            /// Проверка указания команды
            if (String.IsNullOrEmpty(pStoredProcedure) == true)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Не указана хранимая процедура");
            }
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mSqlStoredProcedures(pStoredProcedure, pParameters);
        }
        /// <summary>
        /// Отправка запроса источнику данных
        /// </summary>
        /// <param name="pQuery">Команда отправляемая источнику данных</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>Количество обработанных командой записей</returns>
        public DataTable __mSqlQuery(string pQuery, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Параметр - Команда{0} {1}", ":", pQuery);
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Проверка указания запроса
            if (String.IsNullOrEmpty(pQuery) == true)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Отсутствует содержание запроса");
            }
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mSqlQuery(pQuery);
        }
        /// <summary>
        /// Получение значения поля удовлетворяющего команде. Заполняется в предметном источнике данных 
        /// </summary>
        /// <param name="pCommand">Команда для получения значения поля</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[null] - если значение не найдено, иначе - {object} - значение поля</returns>
        public object __mSqlValue(string pCommand, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Параметр - Команда{0} {1}", ":", pCommand);
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Проверка указания команды
            if (String.IsNullOrEmpty(pCommand) == true)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Отсутствует команда");
            }
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return (vDataSource.__mSqlQuery(pCommand) as DataTable).Rows[0][0];
        }
        /// <summary>
        /// Получение значения поля удовлетворяющего команде. Заполняется в предметном источнике данных 
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldName">Название поля</param>
        /// <param name="pExpressionWhere">Условие поиска записи</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[null] - если значение не найдено, иначе - {object} - значение поля</returns>
        public object __mSqlValue(string pTableName, string pFieldName, string pExpressionWhere, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Проверка указания таблицы
            if (String.IsNullOrEmpty(pTableName) == true)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Отсутствует таблица");
            }
            /// Проверка указания поля
            if (String.IsNullOrEmpty(pTableName) == true)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Отсутствует поле");
            }
            /// Проверка указания условия поиска
            if (String.IsNullOrEmpty(pTableName) == true)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Отсутствует условие поиска");
            }
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                _fError.__fLineInProcedure_ = _mClassLine("");
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mSqlValue(pTableName, pFieldName, pExpressionWhere);
        }
        public int __mSqlCount(string pTableName, string pExpressionWhere)
        {
            string vQuery = "Select Count(*) as Cou From " + pTableName + " Where " + pExpressionWhere;
            DataTable vDataTable = __mSqlQuery(vQuery);

            return Convert.ToInt32(vDataTable.Rows[0][0]);
        }

        #endregion Sql операции

        #region * Базы данных

        /// <summary>
        /// Создание резервной копии базы данных
        /// </summary>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>Путь созданного файла копии базы данных</returns>
        public string __mDatabaseBackUp(string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Параметр - Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear(); 
                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mDatabaseBackUp();
        }
        /// <summary>
        /// Сравнение структуры таблиц в базе данных с моделью базы данных
        /// </summary>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будет выполняться сравнение</param>
        /// <returns>[true] - структуры одинаковы, иначе - [false]</returns>
        public bool? __mDatabaseCompareWithModel(string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mDatabaseCompareWithModel();
        }
        /// <summary>
        /// Создание базы данных
        /// </summary>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[true] - база данных создана или уже существует, иначе - [false]</returns>
        public bool? __mDatabaseCreate(string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Параметр - Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__mClear();
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                _fError.__mClear();
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return __mDataSourceGet(pDataSourceAlias).__mDatabaseCreate();
        }
        /// <summary>
        /// Удаление базы данных
        /// </summary>
        /// <param name="pDataBaseName"></param>
        /// <param name="pDataSourceAlias"></param>
        /// <returns>[true] </returns>
        public bool? __mDatabaseDrop(string pDataBaseName, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return __mDataSourceGet(pDataSourceAlias).__mDatabaseDrop(pDataBaseName);
        }
        /// <summary>
        /// Проверка существования базы данных на текущей сервере
        /// </summary>
        /// <param name="pDatabaseName"></param>
        /// <returns>[true] - база данных существует, иначе - [false]</returns>
        public bool? __mDataBaseExists(string pDatabaseName, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Параметр - Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return __mDataSourceGet(pDataSourceAlias).__mDatabaseExists(pDatabaseName);
        }
        /// <summary>
        /// Получение списка баз данных на сервере
        /// </summary>
        /// <returns>Список баз данных на сервере</returns>
        public ArrayList __mDatabasesList(string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Параметр - Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return __mDataSourceGet(pDataSourceAlias).__mDatabasesList();
        }
        /// <summary>
        /// Восстановление базы данных из копии
        /// </summary>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[true] - Файл копии базы данных создан, иначе - [false]</returns>
        public bool? __mDatabaseRestore(string pFilePath, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Параметр - Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Проверка указания пути к архиву
            if (String.IsNullOrEmpty(pFilePath) == true)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Отсутствует путь к архиву");
            }
            else
            {
                if (File.Exists(pFilePath) == false)
                {
                    _fError.__fLineInProcedure_ = _fClassLine_;
                    _fError.__mReasonAdd("Файл архива указан не верно");
                }
            }
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear(); 
                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mDatabaseRestore(pFilePath);
        }
        /// <summary>
        /// Печать структуры базы данных в источнике
        /// </summary>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>Путь и имя файла отчета</returns>
        public string __mDatabaseStructurePrint(string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Параметр - Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_= _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mDatabaseStructurePrint();
        }

        #endregion Базы данных

        #region * Блокировки

        /// <summary>
        /// Закрытие блокировок пользователя
        /// </summary>
        /// <param name="pUserClue">Идентификатор пользователя</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        public void __mLockClear(int pUserClue, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Проверка указания идентификатора пользователя
            if (pUserClue <= 0)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Идентификатор пользователя указан не верно");
            }
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
            {
                /// Добавить: Получение списка процессов удерживающих записи
                /// Добавить: Проверка процессов в списке запущенных процессов по идентификатору и названию
                /// Добавить: Снятие блокировки с остановленных процессов
                vDataSource.__mLockClear(pUserClue);
            }
        }
        /// <summary>
        /// Снятие блокировок пользователя во всех областях
        /// </summary>
        /// <param name="pUserClue"></param>
        public void __mLockClearAll(int pUserClue)
        {
            /// Перебор текущих источников данных
            foreach (datUnitDataSource vDataSource in fDataSourceS)
            {
                /// Снятие блокировок в выбранной области
                __mLockClear(pUserClue, vDataSource.__fAlias);
            }
        }
        /// <summary>
        /// Снятие блокировки
        /// </summary>
        /// <param name="pLockClue">Идентификатор блокировки</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[true] - блокировка снята, иначе - [false]</returns>
        public bool __mLockOff(int pLockClue, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Параметр - Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Проверка указания идентификатора блокировки
            if (pLockClue <= 0)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Идентификатор блокировки указан не верно");
            }
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return false;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mLockOff(pLockClue);
        }
        /// <summary>
        /// Блокировка таблицы или записи в таблице 
        /// </summary>
        /// <param name="pLockName">Название объекта блокировки</param>
        /// <param name="pClue">Идентификатор записи</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <remarks>Если 'pRecord' = 0, то блокируется вся таблица</remarks>
        /// <returns>Идентификатор заблокированной записи, [0] - запись не удалось заблокировать, [-1] - Блокировки отключены</returns>
        public int __mLockOn(string pLockName, int pClue, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Параметр - Блокировка{0} {1}", ":", pLockName);
            _fError.__mPropertyAdd("Параметр - Ключ{0} {1}", ":", pClue);
            _fError.__mPropertyAdd("Параметр - Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Проверка указания объекта блокировки
            if (String.IsNullOrEmpty(pLockName) == true)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Объект блокировки указан не верно");
            }
            /// Проверка указания идентификатора блокировки
            if (pClue < 0)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Идентификатор записи указан не верно");
            }
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return -1;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mLockOn(pLockName, pClue);
        }

        #endregion Блокировки

        #region * Выражения

        /// <summary>
        /// Создание выражения 'Like' с использованием транслита
        /// </summary>
        /// <param name="pFieldName">Название поля для которого строиться выражение</param>
        /// <param name="pText">Текст условия на одной из раскладок клавиатуры</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных</param>
        public string __mExpressionLikeWithTranslit(string pFieldName, string pText, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _mClassProcedure("");
            _fError.__fFilePath_ = _mClassFilePath("");
            _fError.__mPropertyAdd("Параметр - Название поля{0} {1}", ":", pFieldName);
            _fError.__mPropertyAdd("Параметр - Условие{0} {1}", ":", pText);
            _fError.__mPropertyAdd("Параметр - Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return "";
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mExpressionLikeEntryTranslit(pFieldName, pText);
        }

        #endregion Выражения

        #region * Источники данных

        /// <summary>
        /// Подключение источника данных
        /// </summary>
        /// <param name="pDataSource">Источник данных</param>
        /// <returns>[true] - источник данных подключен, иначе - [false]</returns>
        public bool __mDataSourceAdd(datUnitDataSource pDataSource)
        {
            bool vReturn = false; // Возвращаемое значение
            long vTicksStart = DateTime.Now.Ticks;

            _fError.__fMessage_ = "Не удалось подключить к приложению источник данных";
            _fError.__fProcedure_ = _fClassProcedure_;
            /// Если не указан источник данных, формируется сообщение об ошибке
            if (pDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;  
                _fError.__mPropertyAdd("Источник данных не определен");
                _fError.__mReasonAdd("Параметр - источник данных{0} {1}", " =", "[null]");
                goto Exit;
            }
            /// 
            else
            {
                /// * Проверка указания псевдонима у источника данных
                if (pDataSource.__fAlias.Length == 0)
                {
                    _fError.__fLineInProcedure_ = _fClassLine_;
                    _fError.__mReasonAdd("Псевдоним источник данных не определен");
                }
                /// * Поиск подключенного источика данных с псевдонимом одинаковым с псевдонимом полученного источника данных
                foreach (datUnitDataSource vDataSourceConnected in fDataSourceS)
                {
                    /// ** Если среди подключенных источников данных, обнаружен источник данных с псевдонимом как у полученного источника данных, формируется сообщение об ошибке и работа метода завершается
                    if (vDataSourceConnected.__fAlias.Trim().ToUpper() == pDataSource.__fAlias.Trim().ToUpper())
                    {
                        _fError.__fLineInProcedure_ = _fClassLine_;
                        _fError.__mPropertyAdd("Псевдоним источника данных{0} '{1}'", ":", pDataSource.__fAlias);
                        _fError.__mReasonAdd("Источник данных с указанным псевдонимом уже подключен");
                        break;
                    }
                }
            }
            /// Если обнаружены ошибки, выдается сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mPropertyAdd("Псевдоним источника данных: {0}", pDataSource.__fAlias);
                _fError.__mPropertyAdd("База данных: {0}", Path.Combine(pDataSource.__fDatabasePath, pDataSource.__fDatabaseName));
                _fError.__mPropertyAdd("Строка подключения: {0}", pDataSource.__fConnectionLine);
                _fError.__mMessageBuild("Не удалось подключить источник данных");
                _fError.__fErrorType_ = ERRORSTYPES.Data;
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
            }
            /// Иначе подключается источник данных
            else
            {
                fDataSourceS.Add(pDataSource);
                datApplication.__oProtocols.__mCreate(PROTOCOLSTYPES.DataEvent, _fClassProcedure_);
                datApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Message, datApplication.__oTunes.__mTranslate("Подключен источник данных '{0}'", pDataSource.__fAlias), DateTime.Now.Ticks - vTicksStart);
                vReturn = true;
            }
        Exit:
            return vReturn;
        }
        /// <summary>
        /// Получение подключенного к приложению источника данных по псевдониму
        /// </summary>
        /// <param name="pDataSourceAlias">Псевдоним источника данных</param>
        /// <returns>{datDataSource}</returns>
        public datUnitDataSource __mDataSourceGet(string pDataSourceAlias = "")
        {
            datUnitDataSource vReturn = null; // Возвращаемое значение

            /// Если псевдоним источника данных не указан, то береться назначенный по умолчанию
            if (String.IsNullOrEmpty(pDataSourceAlias) == true)
            {
                ///* Если источник данных указан - он выбирается
                if (String.IsNullOrEmpty(__fDataSourceCurrentAlias) == false)
                {
                    pDataSourceAlias = __fDataSourceCurrentAlias;
                }
                ///* Иначе отображается окно с ошибкой и возвращается [null]
                else
                {
                    _fError.__fErrorType_ = ERRORSTYPES.Data;
                    _fError.__fProcedure_ = _fClassProcedure_;
                    _fError.__fMessage_ = "Не возможно получить источник данных";
                    _fError.__mReasonAdd("Название источника данных не указано");
                    _fError.__mPropertyAdd("Параметр{0} {1}", ":", pDataSourceAlias);
                    _fError.__mPropertyAdd("Название источника данных по умолчанию{0} '{1}'", ":", __fDataSourceCurrentAlias);
                    datApplication.__oErrorsHandler.__mShow(_fError);
                    _fError.__mClear();
                    return null;
                }
            }
            /// Поиск подключенного источика данных в списке подключенных источников данных
            foreach (datUnitDataSource vDataSourceConnected in fDataSourceS)
            {
                ///* Если среди подключенных источников данных, обнаружен источник данных с полученным псевдонимом, работа метода завершается и возвращается источник данных
                if (vDataSourceConnected.__fAlias.Trim().ToUpper() == pDataSourceAlias.Trim().ToUpper())
                {
                    vReturn = vDataSourceConnected;
                    break;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Удаление источника данных из списка используемых источников данных
        /// </summary>
        /// <param name="pDataSourceAlias">Псевдоним источника данных</param>
        public bool __mDataSourceRemove(string pDataSourceAlias)
        {
            /// Если Псевдоним источника данных не указан, формируется сообщение об ошибке и работа метода завершается
            if (String.IsNullOrEmpty(pDataSourceAlias) == true)
            {
                _fError.__fErrorType_ = ERRORSTYPES.Data;
                _fError.__fProcedure_ = _fClassProcedure_;
                _fError.__fMessage_ = "Не возможно получить источник данных";
                _fError.__mReasonAdd("Название источника данных не указано");
                _fError.__mPropertyAdd("Параметр{0} {1}", ":", pDataSourceAlias);
                _fError.__mPropertyAdd("Название источника данных по умолчанию{0} '{1}'", ":", __fDataSourceCurrentAlias);
                datApplication.__oErrorsHandler.__mShow(_fError);
                return false;
            }
            /// Получаем источник данных с полученным псевдонимом
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias); // Отключаемый источник данных
            /// Если источник данных существует он удаляется из списка используемых источников данных
            if (vDataSource != null)
            {
                fDataSourceS.Remove(vDataSource);
            }

            return true;
        }
        /// <summary>
        /// Отключение всех источников данных
        /// </summary>
        public bool __mDataSourceRemoveAll()
        {
            List<datUnitDataSource> vDataSourceS = fDataSourceS; // Список подключенных источников данных
            ArrayList vDataSourcesAliaseS = new ArrayList(); // Список псевдонимов подключенных источников данных
            /// Получение списка псевдонимов подключенных источников данных
            foreach (datUnitDataSource vDataSource in vDataSourceS)
            {
                vDataSourcesAliaseS.Add(vDataSource.__fAlias);
            }
            /// Отключение источников данных по списку подключенных
            foreach (string vAlias in vDataSourcesAliaseS)
            {
                __mDataSourceRemove(vAlias);
            }

            return true;
        }
        /// <summary>
        /// Получение количества подключенных источников данных
        /// </summary>
        public int __mDataSourcesCount()
        {
            return fDataSourceS.Count;
        }

        #endregion Источники данных

        #region * Модель

        /// <summary>
        /// Создание отчета сравнения модели с базой данных 
        /// </summary>
        public string __mModelCompareWithDatabase(string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Параметр - Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return "";
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource._mModelCompareWithDatabase();
        }
        /// <summary>
        /// Печать модели структуры базы данных
        /// </summary>
        /// <returns>Путь и имя файла отчета</returns>
        public string __mModelStructurePrint(string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_= _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return null;
            }
            else
                /// Иначе выполняется метод с тем же названием в источнике данных
                return vDataSource.__mModelStructurePrint();
        }
        /// <summary>
        /// Получение описания таблицы из модели 
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных</param>
        public string __mModelTableDescription(string pTableName, string pDataSourceAlias = "")
        {
            string vReturn = null; // Возвращаемое значение
            /// Перебор моделей таблиц в указанном источнике данных
            foreach (datUnitModelTable vTable in __mDataSourceGet(pDataSourceAlias).__fModelTableS)
            {
                /// * Если таблица с полученным именем существует, возвращается ее описание
                if (vTable.__fName == pTableName)
                {
                    vReturn = vTable.__fDescription;
                    break;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Получение описания поля таблицы  из модели
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldName">Название поля</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных</param>
        /// <returns>Описание поля таблицы</returns>
        public string __mModelFieldCaption(string pTableName, string pFieldName, string pDataSourceAlias = "")
        {
            string vReturn = ""; // Возвращаемое значение
            /// Перебор моделей таблиц
            foreach (datUnitModelTable vTable in datApplication.__oData.__mDataSourceGet(pDataSourceAlias).__fModelTableS)
            {
                /// * Если таблица с полученным именем существует:
                if (vTable.__fName == pTableName)
                {
                    bool vFieldFound = false; // Поле таблицы найдено
                    /// ** Перебор полей в таблице
                    foreach (datUnitModelField vField in vTable.__fFieldS)
                    {
                        /// *** Если поле с указанным именем существует, возвращается его описание
                        if (vField.__fName == pFieldName)
                        {
                            vReturn = vField.__fCaption;
                            vFieldFound = true;
                            break;
                        }
                    }
                    /// ** Если в указанной таблице, поле не обнаружено, работа метода завершается
                    if (vFieldFound == true)
                        break;
                }
            }

            return vReturn;
        }

        /// <summary>
        /// Получение описания поля таблицы  из модели
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldName">Название поля</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных</param>
        /// <returns>Описание поля таблицы</returns>
        public string __mModelFieldDescription(string pTableName, string pFieldName, string pDataSourceAlias = "")
        {
            string vReturn = ""; // Возвращаемое значение
            /// Перебор моделей таблиц
            foreach (datUnitModelTable vTable in datApplication.__oData.__mDataSourceGet(pDataSourceAlias).__fModelTableS)
            {
                /// * Если таблица с полученным именем существует:
                if (vTable.__fName == pTableName)
                {
                    bool vFieldFound = false; // Поле таблицы найдено
                    /// ** Перебор полей в таблице
                    foreach (datUnitModelField vField in vTable.__fFieldS)
                    {
                        /// *** Если поле с указанным именем существует, возвращается его описание
                        if (vField.__fName == pFieldName)
                        {
                            vReturn = vField.__fDescription;
                            vFieldFound = true;
                            break;
                        }
                    }
                    /// ** Если в указанной таблице, поле не обнаружено, работа метода завершается
                    if (vFieldFound == true)
                        break;
                }
            }

            return vReturn;
        }

        #endregion Модель

        #region * Подключение

        ///// <summary>
        ///// Построение строки подключения к источнику данных
        ///// </summary>
        ///// <param name="pLogin">Использование логина с паролем</param>
        ///// <returns>[true] - строка построена, иначе - [false]</returns>
        //public bool __mConnectionLineBuild(bool pLogin)
        //{
        //    return false;
        //}
        ///// <summary>
        ///// Разрыв соединения с источником данных
        ///// </summary>
        //private bool mConnectionOff()
        //{
        //    return false;
        //}
        ///// <summary>
        ///// Установка соединения с источником данных
        ///// </summary>
        ///// <returns>[true] - соединение установлено, иначе - [false]</returns>
        //private bool mConnectionOn()
        //{
        //    return false;
        //}

        #endregion Подключение

        #region * Пользователи

        /// <summary>
        /// Получение доступа пользователя к объекту
        /// </summary>
        /// <param name="pRightClue">Право</param>
        /// <param name="pUserClue">Пользователь</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных</param>
        /// <returns>[true] - доступ разрешен, иначе - [false], в случае ошибки [null]</returns>
        public bool? __mUserRightAccess(int pRightClue, int pUserClue, string pDataSourceAlias = "")
        {
            bool vReturn = false; // Возвращаемое значение
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Параметр - Ключ права{0} {1}", ":", pRightClue);
            _fError.__mPropertyAdd("Параметр - Идентификатор пользователя{0} {1}", ":", pUserClue);
            _fError.__mPropertyAdd("Параметр - Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Проверка полученного ключа права
            if (pRightClue <= 0)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Право указано не верно");
            }
            /// Проверка полученного ключа пользователя
            if (pUserClue <= 0)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Пользователь указан не верно");
            }
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                vReturn = vDataSource.__mUserAccess(pRightClue, pUserClue);

            return vReturn;
        }
        /// <summary>
        /// Пользователь - администратор
        /// </summary>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[true] - текущий пользователь администратор, иначе - [false], в случае ошибки - [null]</returns>
        public bool __mUserAdministrator(string pDataSourceAlias = "")
        {
            bool vReturn = false; // Возвращаемое значение
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Параметр - Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                vReturn = false;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                vReturn = vDataSource.__fUserAdministrator;

            return vReturn;
        }
        /// <summary>
        /// Псевдоним пользователя
        /// </summary>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>Псевдоним пользователя, в случае ошибки - [null]</returns>
        public string __mUserAlias(string pDataSourceAlias = "")
        {
            string vReturn = null; // Возвращаемое значение

            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Параметр - Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                vReturn = vDataSource.__fUserAlias.Trim();

            return vReturn;
        }
        /// <summary>
        /// Идентификатор пользователя полученного источника данных
        /// </summary>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>Идентификатор пользователя, если пользователь не определен - [-1], в случае ошибки - [null]</returns>
        public int __mUserClue(string pDataSourceAlias = "")
        {
            int vReturn = -1; // Возвращаемое значение

            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                return vReturn;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                vReturn = vDataSource.__fUserClue;

            return vReturn;
        }

        public bool __mUserDesign(string pDataSourceAlias = "")
        {
            bool vReturn = false; // Возвращаемое значение
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Параметр - Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
            {
                _fError.__fLineInProcedure_ = _fClassLine_;
                _fError.__mReasonAdd("Источник данных указан не верно");
            }
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                vReturn = false;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                vReturn = vDataSource.__fUserDesign;

            return vReturn;
        }
        /// <summary>
        /// Идентификатор роли пользователя полученного источника данных
        /// </summary>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>Идентификатор роли пользователя, если роль пользователя не определена - [-1], в случае ошибки - [null]</returns>
        public int __mUserRoleClue(string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);

                return -1;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__fUserRoleClue;
        }
        /// <summary>
        /// Название роли пользователя
        /// </summary>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>Роль пользователя, если роль пользователя не определена - [""], в случае ошибки - [null]</returns>
        public string __mUserRoleName(string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__fUserRoleName.Trim();
        }

        #endregion Пользователи

        #region * Таблицы

        /// <summary>
        /// Получение пустого курсора со структурой таблицы из базы данных
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[null] - таблица не получена, иначе {DataTable}</returns>
        public DataTable __mTableEmpty(string pTableName, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Проверка указания названия таблицы
            if (String.IsNullOrEmpty(pTableName) == true)
                _fError.__mReasonAdd("Не указано имя таблицы");
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mTableEmpty(pTableName);
        }
        /// <summary>
        /// Проверка существования таблицы в базе данных источника данных
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[true] - таблица существует, иначе - [false]</returns>
        public bool __mTableExists(string pTableName, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            if (String.IsNullOrEmpty(pTableName) == true)
                _fError.__mReasonAdd("Не указано имя таблицы");
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return false;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mTableExists(pTableName);
        }
        /// <summary>
        /// Получение списка таблиц в базе данных
        /// </summary>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[null] - таблиц нет, иначе - список таблиц в базе данных</returns>
        public ArrayList __mTablesList(string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return new ArrayList();
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mTablesList();
        }
        /// <summary>
        /// Очистка таблицы со сбросом идентификатора в 0
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[true] - таблица очищена, иначе - [false]</returns>
        public bool __mTableTruncate(string pTableName, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Проверка указания названия таблицы
            if (String.IsNullOrEmpty(pTableName) == true)
                _fError.__mReasonAdd("Не указано имя таблицы");
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return false;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mTableTruncate(pTableName);
        }

        #endregion Таблицы

        #region * Таблицы - Строки

        /// <summary>
        /// Получение записи из таблицы указанной идентификатором
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор записи</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[null] - таблиц нет, иначе - {DataTable}</returns>
        public DataTable __mTableRow(string pTableName, int pClue, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Проверка указания названия таблицы
            if (String.IsNullOrEmpty(pTableName) == true)
                _fError.__mReasonAdd("Не указано имя таблицы");
            /// Проверка указания ключа записи
            if (pClue <= 0)
                _fError.__mReasonAdd("Не указан идентификатор записи");
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mTableRow(pTableName, pClue);
        }
        /// <summary>
        /// Получение записи из таблицы указанной уникальным идентификатором
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pGuid">Уникальный идентификатор записи</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[null] - таблиц нет, иначе - {DataTable}</returns>
        public DataTable __mTableRow(string pTableName, Guid pGuid, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_= _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Проверка указания названия таблицы
            if (String.IsNullOrEmpty(pTableName) == true)
                _fError.__mReasonAdd("Не указано имя таблицы");
            /// Проверка указания идентификатора записи
            if (pGuid == Guid.Empty)
                _fError.__mReasonAdd("Не указан уникальный идентификатор записи");
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return null;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mTableRow(pTableName, pGuid);
        }
        /// <summary>
        /// Установка текущего времени в качестве последнего времени изменения записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор записи</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[true] - Время исправлено, иначе - [false]</returns>
        public bool __mTableRowChangeTimeNow(string pTableName, int pClue, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            /// Проверка указания названия таблицы
            if (String.IsNullOrEmpty(pTableName) == true)
                _fError.__mReasonAdd("Не указано имя таблицы");
            /// Проверка указания ключа записи
            if (pClue <= 0)
                _fError.__mReasonAdd("Не указан уникальный идентификатор записи");
            /// Выбор источника данных
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            /// Если есть ошибки, то выводиться сообщение об ошибке
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return false;
            }
            /// Иначе выполняется метод с тем же названием в источнике данных
            else
                return vDataSource.__mTableRowChangeTimeNow(pTableName, pClue);
        }
        /// <summary>
        /// Подсчет количества дублирующихся записей
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldName">Название поля</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[null] - если данные не найдены, иначе - {DataTable} - таблица с названием поля и количеством повторений</returns>
        public DataTable __mTableRowCountDouble(string pTableName, string pFieldName, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            if (String.IsNullOrEmpty(pTableName) == true)
                _fError.__mReasonAdd("Не указано имя таблицы");
            if (String.IsNullOrEmpty(pFieldName) == true)
                _fError.__mReasonAdd("Не указан имя поля");
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return null;
            }
            else
                return vDataSource.__mTableRowsCountDouble(pTableName, pFieldName);
        }
        /// <summary>
        /// Подсчет количества записей удовлетворяющих условию
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pExpressionWhere">Условие для подсчета записей</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>Количество подсчитанных записей</returns>
        public int __mTableRowsCountWhere(string pTableName, string pExpressionWhere, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            if (String.IsNullOrEmpty(pTableName) == true)
                _fError.__mReasonAdd("Не указано имя таблицы");
            if (String.IsNullOrEmpty(pExpressionWhere) == true)
                _fError.__mReasonAdd("Не указано условие для подсчета записей");
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return -1;
            }
            else
                return vDataSource.__mTableRowsCountWhere(pTableName, pExpressionWhere);
        }

        #endregion Таблицы - Строки

        #region * Таблицы - Поля

        /// <summary>
        /// Проверка существования поля в таблице источника данных
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldName">Название поля</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[true] - Поле существует, иначе - [false]</returns>
        public bool __mTableColumnExists(string pTableName, string pFieldName, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);
            if (String.IsNullOrEmpty(pTableName) == true)
                _fError.__mReasonAdd("Не указано имя таблицы");
            if (String.IsNullOrEmpty(pFieldName) == true)
                _fError.__mReasonAdd("Не указано имя поля");
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return false;
            }
            else
                return vDataSource.__mTableColumnExists(pTableName, pFieldName);
        }
        /// <summary>
        /// Получение списка полей в таблице
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[null] - данные не обнаружены, иначе {DataTable} заполненная списком полей указанной таблицы</returns>
        public DataTable __mTableColumnS(string pTableName, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            if (String.IsNullOrEmpty(pTableName) == true)
                _fError.__mReasonAdd("Не указано имя таблицы");
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return null;
            }
            else
                return vDataSource.__mTableColumnS(pTableName);
        }
        /// <summary>
        /// Получение информации о поле таблицы 
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pField">Название поля</param>
        /// <param name="pFieldInfo">Вид операции</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>Запрашиваемое значение, иначе [null]</returns>
        public object __mTableColumnInfo(string pTableName, string pFieldName, FIELDINFO pFieldInfo, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            if (String.IsNullOrEmpty(pTableName) == true)
                _fError.__mReasonAdd("Не указано имя таблицы");
            if (String.IsNullOrEmpty(pFieldName) == true)
                _fError.__mReasonAdd("Не указано имя поля");
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return null;
            }
            else
                return vDataSource.__mTableColumnInfo(pTableName, pFieldName, pFieldInfo);
        }
        /// <summary>
        /// Добавление колонки в таблицу
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pColumnName">Название колонки</param>
        /// <param name="pDataType">Тип колонки</param>
        /// <param name="IsNull">Допустимость 'Null' значений</param>
        /// <param name="pColumnScale">Размер колонки</param>
        /// <param name="pColumnPrecision">Точность колонки</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[true] - Колонка добавлена, иначе - [false]</returns>
        public bool __mTableColumnAdd(string pTableName, string pColumnName, COLUMNSTYPES pDataType, bool IsNull, int pColumnScale = 0, int pColumnPrecision = 0, string pDefaultValue = "0", string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            if (String.IsNullOrEmpty(pTableName) == true)
                _fError.__mReasonAdd("Не указано имя таблицы");
            if (String.IsNullOrEmpty(pColumnName) == true)
                _fError.__mReasonAdd("Не указано имя поля");
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");

            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return false;
            }
            else
                return vDataSource.__mTableColumnAdd(pTableName, pColumnName, pDataType, IsNull, pColumnScale, pColumnPrecision, pDefaultValue);
        }
        /// <summary>
        /// Удаление колонки из таблицы
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pColumnName">Название колонки</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[true] - Колонка удалена, иначе - [false]</returns>
        public bool __mTableColumnDrop(string pTableName, string pColumnName, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            if (String.IsNullOrEmpty(pTableName) == true)
                _fError.__mReasonAdd("Не указано имя таблицы");
            if (String.IsNullOrEmpty(pColumnName) == true)
                _fError.__mReasonAdd("Не указано имя поля");
            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");

            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return false;
            }
            else
                return vDataSource.__mTableColumnDrop(pTableName, pColumnName);
        }

        #endregion Таблицы - Поля

        #region * Транзакции

        /// <summary>
        /// Закрытие транзакции
        /// </summary>
        /// <param name="pCommit">Условие закрытия транзакции. [true] - [Commit], [false] - [RollBack]</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[true] - Транзакция закрыта, иначе - [false]</returns>
        public bool __mTransactionOff(bool pCommit, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return false;
            }
            else
                return vDataSource.__mTransactionOff(pCommit);
        }
        /// <summary>
        /// Открытие транзакции
        /// </summary>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[true] - транзация создана, иначе - [false]</returns>
        public bool __mTransactionOn(string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return false;
            }
            else
                return vDataSource.__mTransactionOn();
        }

        #endregion Транзакции

        #region * Функции - Идентификаторы

        /// <summary>
        /// Получение идентификатора по учетному коду
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pCode">Учетный код</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        /// <returns>[<=0] - Запись не найдена, иначе - идентификатор</returns>
        public int __mClueByCode(string pTableName, int pCode, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Параметр 'Название таблицы' {0} {1}", ":", pDataSourceAlias);
            _fError.__mPropertyAdd("Параметр 'Учетный код' {0} {1}", ":", pDataSourceAlias);
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return -1;
            }
            else
                return vDataSource.__mClueByCode(pTableName, pCode);

        }
        /// <summary>
        /// Получение идентификатора записи по значению поля названия
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldName">Значение поля названия</param>
        /// <returns></returns>
        public int __mClueByName(string pTableName, string pFieldName, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return -1;
            }
            else
                return vDataSource.__mClueByName(pTableName, pFieldName);
        }
        /// <summary>
        /// Получение идентификатора записи по названию поля опции
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldOptionName">Имя поля опции</param>
        /// <returns>Значение поля идентификатор записи</returns>
        public int __mClueByOption(string pTableName, string pFieldOptionName, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return -1;
            }
            else
                return vDataSource.__mClueByOption(pTableName, pFieldOptionName);
        }
        /// <summary>
        /// Проверка существования идентификатора в таблице
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатоор записи</param>
        /// <returns></returns>
        public bool __mClueExists(string pTableName, int pClue, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return false;
            }
            else
                return vDataSource.__mClueExists(pTableName, pClue);
        }
        /// <summary>
        /// Получение идентификатора последеней вставленной записи в таблицу
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns>[<=0] - Запись не найдена, иначе - идентификатор</returns>
        public int __mClueLastInserted(string pTableName, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return -1;
            }
            else
                return vDataSource.__mClueLastInserted(pTableName);
        }

        #endregion Функции - Идентификаторы

        #region * Функции - Названия

        /// <summary>
        /// Получение названия по идентификатору записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор искомой строки</param>
        /// <returns>Значение поля 'Название'</returns>
        public string __mNameByClue(string pTableName, int pClue, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return datApplication.__oTunes.__mTranslate("не определен");
            }
            else
                return vDataSource.__mNameByClue(pTableName, pClue);
        }
        /// <summary>
        /// Получение названия по идентификатору записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор искомой строки</param>
        /// <returns>[""] - значение не найдено, иначе - значение поля 'Название'</returns>
        public string __mNameByCode(string pTableName, int pClue)
        {
            return "null";
        }
        /// <summary>
        /// Получение названия справочника по названию и значению опции
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pOptionName">Название опции</param>
        /// <returns>Значение поля 'Название' </returns>
        public string __mNameByOption(string pTableName, string pOptionName, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return datApplication.__oTunes.__mTranslate("не определен");
            }
            else
                return vDataSource.__mNameByOption(pTableName, pOptionName);
        }
        /// <summary>
        /// Проверка существования названия
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pValue">Значение поля названия</param>
        /// <param name="pClueSkip">Идентификатор исключаемой записи</param>
        /// <returns>[true] - Указанное название уже существует, иначе - [false]</returns>
        public bool __mNameExists(string pTableName, string pValue, int pClueSkip)
        {
            return false;
        }

        #endregion Функции - Названия

        #region * Функции - Учетные коды

        /// <summary>
        /// Получение значения поля учетного кода по идентификатору записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор записи</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных</param>
        /// <returns>Значение поля учетного кода</returns>
        public int __mCodeByClue(string pTableName, int pClue, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return -1;
            }
            else
                return vDataSource.__mCodeByClue(pTableName, pClue);
        }
        /// <summary>
        /// Проверка существования учетного кода исключая идентификатор записи указанный в 'pClueSkip'
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pCodeCheck">Проверяемый учетный код</param>
        /// <param name="pClueSkip">Идентификатор записи который нужно исключить из поиска</param>
        /// <returns>[true] - Дублирующийся учетный код найден, иначе - [false]</returns>
        public bool __mCodeExists(string pTableName, int pCodeCheck, int pClueSkip)
        {
            return false;
        }
        /// <summary>
        /// Вычисление нового учетного кода
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClueSkip">Идентификатор записи исключаемой из обработки</param>
        /// <param name="pCodeNewCalculateType">Порядок расчета нового учетного кода</param>
        /// <param name="pValueMaximal">Минимальное значение кода</param>
        /// <param name="pValueMinimal">Максимальное значение кода</param>
        /// <returns>Новый учетный код</returns>
        public int __mCodeNew(string pTableName, int pClueSkip, CODESNEWTYPES pCodeNewCalculateType, int pValueMinimal = 1, int pValueMaximal = 999999)
        {
            return -1;
        }
        /// <summary>
        /// Вычисление нового учетного кода по нескольким полям
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClueSkip">Идентификатор записи исключаемой из обработки</param>
        /// <param name="pCodeNewCalculateType">Порядок расчета нового учетного кода</param>
        /// <param name="pValueMaximal">Минимальное значение кода</param>
        /// <param name="pValueMinimal">Максимальное значение кода</param>
        /// <param name="pFieldS">Список дополнительных полей</param>
        /// <returns>[0] - неудалось вычислить учетный код, иначе - новый учетный код</returns>
        public int __mCodeNewGroup(string pTableName, int pClueSkip, CODESNEWTYPES pCodeNewCalculateType, int pValueMinimal, int pValueMaximal, ArrayList pFieldS)
        {
            return -1;
        }

        #endregion Функции - Учетные коды

        #region * Функции - Позиция в документе

        /// <summary>
        /// Вычисление новой позиции в документе
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClueSkip">Идентификатор записи исключаемой из обработки</param>
        /// <param name="pWhere">Условие отбирающее документ</param>
        /// <returns></returns>
        public int __mPositionNew(string pTableName, int pClueSkip, string pWhere, string pDataSourceAlias = "")
        {
            _fError.__fErrorType_ = ERRORSTYPES.Programming;
            _fError.__fProcedure_ = _fClassProcedure_;
            _fError.__fLineInProcedure_ = _fClassLine_;
            _fError.__mPropertyAdd("Источник данных{0} {1}", ":", pDataSourceAlias);

            datUnitDataSource vDataSource = __mDataSourceGet(pDataSourceAlias);
            /// Проверка достоверности указания источника данных
            if (vDataSource == null)
                _fError.__mReasonAdd("Источник данных указан не верно");
            if (_fError.__fReasonS_.Count > 0)
            {
                datApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();
                return -1;
            }
            else
                return vDataSource.__mPositionNew(pTableName, pClueSkip, pWhere);
        }

        #endregion Функция - Позиция в документе

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Псевдоним текущего источника данных
        /// </summary>
        public string __fDataSourceCurrentAlias = "";

        #endregion Атрибуты

        #region - Закрытые

        /// <summary>
        /// Подключенные источники данных
        /// </summary>
        private List<datUnitDataSource> fDataSourceS = new List<datUnitDataSource>();

        #endregion Закрытые

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

        #endregion СВОЙСТВА
    }
}
