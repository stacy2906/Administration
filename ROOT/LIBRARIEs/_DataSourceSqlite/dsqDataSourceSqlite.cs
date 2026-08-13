using nlData;
using System.Collections;
using System.Data;
using System.IO;
using System;
using System.Data.SQLite;
using nlApplication;

namespace nlDataSourceSqlite
{
    /// <summary>
    /// Файл dsqDataSourceSqlite.cs
    /// </summary>
    /// <remarks>Класс-источник данных 'Sqlite'</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.01.13 15-35</version> // Дата-время последней корректировки
    public class dsqDataSourceSqlite : datUnitDataSource
    {
        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            Type vType = this.GetType();
            __fDataSourceType = DATASOURCETYPES.Sqlite;

            return;
        }

        #endregion Поведение

        #region - Процедуры

        #region Sql операции

        /// <summary>
        /// Отправка команды источнику данных. Заполняется в предметном источнике данных 
        /// </summary>
        /// <param name="pCommand">Команда отправляемая источнику данных</param>
        /// <returns>Количество обработанных командой записей</returns>
        public override int __mSqlCommand(string pCommand)
        {
            int vReturn = -1; // Возвращаемое значение
            bool vTransactionUsed = false; // Использование транзакции

            try
            {
                /// Установка соединения
                if (__fOnLine == false & (_fConnection == null || _fConnection.State != ConnectionState.Open))
                {
                    __mConnectionOn();
                }
                SQLiteCommand vSqliteCommand = new SQLiteCommand(pCommand, _fConnection);
                /// Открыта транзакция
                if (_fTransaction != null)
                {
                    vSqliteCommand.Transaction = _fTransaction;
                    vTransactionUsed = true;
                }
                vReturn = vSqliteCommand.ExecuteNonQuery();
                /// Если транзакция отсутствует, выполняется разрыв соединения
                if (_fTransaction == null)
                {
                    if (__fOnLine == false & _fConnection != null)
                        __mConnectionOff();
                }
            }
            catch (SQLiteException vException)
            {
                appUnitError vError = new appUnitError();
                vError.__fException = vException;
                vError.__fProcedure_ = _fClassProcedure_;
                vError.__fErrorType_ = ERRORSTYPES.Data;
                vError.__mMessageBuild("Ошибка при выполнении команды");
                vError.__mPropertyAdd("Вид источника данных: {0}", __fDataSourceType.ToString());
                vError.__mPropertyAdd("База ланных: {0}", Path.Combine(__fDatabasePath, __fDatabaseName));
                vError.__mPropertyAdd("Команда: {0}", pCommand);
                vError.__mPropertyAdd("Транзакция: {0}", vTransactionUsed.ToString());

                appApplication.__oErrorsHandler.__mShow(vError);
            }

            return vReturn;
        }
        /// <summary>
        /// Отправка запроса источнику данных. Заполняется в предметном источнике данных 
        /// </summary>
        /// <param name="pQuery">Условие запроса</param>
        /// <returns>{DataTable} - с данными удовлетворяющими условию "pQuer"</returns>
        public override DataTable __mSqlQuery(string pQuery)
        {
            DataTable vDataTable = null; // Возвращаемое значение
            bool vTransactionUsed = false; // Использование транзакции

            try
            {
                if (__fOnLine == false & (_fConnection == null || _fConnection.State != ConnectionState.Open))
                {
                    __mConnectionOn();
                } /// Установка соединения
                SQLiteCommand vSqliteCommand = new SQLiteCommand(pQuery, _fConnection);
                if (_fTransaction != null)
                {
                    vSqliteCommand.Transaction = _fTransaction;
                    vTransactionUsed = true;
                } /// Открыта транзакция
                SQLiteDataReader vSqlDataReader = vSqliteCommand.ExecuteReader();
                vDataTable = new DataTable();
                vDataTable.Load(vSqlDataReader);
                if (_fTransaction == null)
                {
                    if (__fOnLine == false & _fConnection != null)
                        __mConnectionOff();
                } /// Транзакция отсутствует
            }
            catch (SQLiteException vException)
            {
                appUnitError vUnitError = new appUnitError();
                vUnitError.__fException = vException;
                vUnitError.__fProcedure_ = _fClassProcedure_;
                vUnitError.__fErrorType_ = ERRORSTYPES.Data;
                vUnitError.__mMessageBuild("Ошибка при выполнении запроса");
                vUnitError.__mPropertyAdd("Вид источника данных: {0}", " " + __fDataSourceType.ToString());
                vUnitError.__mPropertyAdd("База данных: {0}", Path.Combine(__fDatabasePath, __fDatabaseName));
                vUnitError.__mPropertyAdd("Содержание запроса: {0}", " " + pQuery);
                vUnitError.__mPropertyAdd("Транзакция: {0}", vTransactionUsed.ToString());

                appApplication.__oErrorsHandler.__mShow(vUnitError);
            }

            return vDataTable;
        }
        /// <summary>
        /// Получение значения поля удовлетворяющего команде. Заполняется в предметном источнике данных 
        /// </summary>
        /// <param name="pCommand">Команда для получения значения поля</param>
        /// <returns>{object} - значение поля</returns>
        public override object __mSqlValue(string pCommand)
        {
            object vReturn = null; // Возвращаемое значение
            bool vTransactionUsed = false; // Использование транзакции

            try
            {
                if (__fOnLine == false & (_fConnection == null || _fConnection.State != ConnectionState.Open))
                {
                    __mConnectionOn();
                } /// * Установка соединения
                SQLiteCommand vSqliteCommand = new SQLiteCommand(pCommand, _fConnection);
                if (_fTransaction != null)
                {
                    vSqliteCommand.Transaction = _fTransaction;
                    vTransactionUsed = true;
                } /// * Открыта транзакция
                vReturn = vSqliteCommand.ExecuteScalar();
                if (_fTransaction == null)
                {
                    if (__fOnLine == false & _fConnection != null)
                        __mConnectionOff();
                } /// * Если транзакция отсутствует, выполняется разрыв соединения
            }
            catch (SQLiteException vException)
            {
                appUnitError vUnitError = new appUnitError();
                vUnitError.__fException = vException;
                vUnitError.__fProcedure_ = _fClassProcedure_;
                vUnitError.__fErrorType_ = ERRORSTYPES.Data;
                vUnitError.__mMessageBuild("Не возможно получить значение");
                vUnitError.__mPropertyAdd("Вид источника данных: {0}", __fDataSourceType.ToString());
                vUnitError.__mPropertyAdd("База данных: {0}", Path.Combine(__fDatabasePath, __fDatabaseName));
                vUnitError.__mPropertyAdd("Команда: {0}", pCommand);
                vUnitError.__mPropertyAdd("Транзакция: {0}", vTransactionUsed.ToString());

                appApplication.__oErrorsHandler.__mShow(vUnitError);
            }

            return vReturn;
        }
        /// <summary>
        /// Получение значения поля по имени таблицы, имени поля и условию
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldName">Название поля</param>
        /// <param name="pExpressionWhere">Условие поиска записи</param>
        /// <returns>[null] - если значение не найдено, иначе - {object} - значение поля</returns>
        public override object __mSqlValue(string pTableName, string pFieldName, string pExpressionWhere)
        {
            string vCommand = "Select " + pFieldName + " From " + pTableName + " Where " + pExpressionWhere;
            return __mSqlValue(vCommand);
        }

        #endregion Sql операции

        #region База данных

        /// <summary>
        /// Печать структуры базы данных в источнике
        /// </summary>
        /// https://www.codeproject.com/Questions/1213783/Restore-sqlite-database-in-Csharp
        /// <returns>Путь созданного файла копии базы данных</returns>
        public override string __mDatabaseBackUp()
        {
            string vReturn = datApplication.__oPathes.__mFileDataBaseBackUp(__fDatabaseName, "db");

            return vReturn;
        }
        /// <summary>
        /// Сравнение структуры таблиц в базе данных с моделью приложения
        /// </summary>
        /// <returns>[true] - структуры одинаковы, иначе - [false]</returns>
        public override bool __mDatabaseCompareWithModel()
        {
            bool vReturn = true; // Возвращаемое значение
            ArrayList vTablesInDatabase = __mTablesList(); // Список уже созданных таблиц в базе данных

            #region Проверка на вставку и изменение

            foreach (datUnitModelTable vTableModel in __fModelTableS)
            {
                /// Таблица отсутствует в источнике данных
                if (__mTableExists(vTableModel.__fName) == false)
                {
                    string vCommand = "Create Table If Not Exists " + vTableModel.__fName + "(";
                    foreach (datUnitModelField vField in vTableModel.__fFieldS)
                    {
                        vCommand = vCommand + vField.__fName + " " + __mModelTableFieldType(vField);

                        if (vField.__fIsClue == true) /// Идентификатор записи в таблице
                            vCommand = vCommand + " Primary Key";
                        if (vField.__fAutoIncrement == true)
                            vCommand = vCommand + " AUTOINCREMENT";
                        if (vField.__fIsNull == false) /// Разрешение NULL данных
                            vCommand = vCommand + " Not Null";
                        //else
                        //    vCommand = vCommand + ",";
                        if (vField.__fDefaultValue != null)
                        {
                            if (vField.__fDefaultValue.ToString().Length > 0) /// Значение по умолчанию
                                vCommand = vCommand + " Default " + vField.__fDefaultValue + ",";
                            else
                                vCommand = vCommand + " Default " + vField.__fDefaultValue + ",";
                        }
                    }
                    vCommand = vCommand.Substring(0, vCommand.Length - 1); /// Удаление последней запятой
                    vCommand = vCommand + ")";
                    if (__mSqlCommand(vCommand) > 0)
                    {
                        __fStructureChanges.Add(appApplication.__oTunes.__mTranslate("Создана таблица '{0}'", vTableModel.__fName)); // Создана таблица
                    } /// Создание таблицы
                } /// Таблица отсутствует в источнике данных
                  /// Таблица присутствует в источнике данных
                else
                {
                    /// Проверка существования и добавление полей
                    foreach (datUnitModelField vField in vTableModel.__fFieldS)
                    {
                        /// Поле отсутствует
                        if (__mTableColumnExists(vTableModel.__fName, vField.__fName) == false)
                        {
                            string vCommand = "ALTER TABLE " + vTableModel.__fName + " ADD COLUMN " + vField.__fName + " " + vField.__fDataType;
                            /// Идентификатор записи в таблице
                            if (vField.__fIsClue == true)
                                vCommand = vCommand + " Primary Key";
                            /// Разрешение NULL данных
                            if (vField.__fIsNull == false)
                                vCommand = vCommand + " Not Null";
                            else
                                vCommand = vCommand + ",";
                            if (vField.__fDefaultValue.ToString().Length > 0) /// Значение по умолчанию
                                vCommand = vCommand + " Default " + vField.__fDefaultValue + ",";
                            else
                                vCommand = vCommand + " Default " + vField.__fDefaultValue + ",";
                            vCommand = vCommand.Substring(0, vCommand.Length - 1); /// Удаление последней запятой
                            __mSqlCommand(vCommand);
                            __fStructureChanges.Add(appApplication.__oTunes.__mTranslate("Создано поле '{0}' тип: '{1}'", vField.__fName, vField.__fDataType));
                        }
                        /// Проверка размерности поля
                        else
                        {
                            /// У Sqlite размерностей нет
                        }
                    }

                    /// Удаление полей
                    foreach (string vDataRow in vTablesInDatabase)
                    {
                        /// Пропускаем служебную таблицу 'sqlite_sequence'
                        if (vDataRow.Trim().ToLower() == "sqlite_sequence")
                            break;

                        if (vTableModel.__fName.Trim().ToUpper() != vDataRow.Trim().ToUpper())
                            continue;

                        DataTable vDataTable = __mSqlQuery("Select * From " + vDataRow + " Limit 1");
                        foreach (DataColumn vDataColumn in vDataTable.Columns)
                        {
                            bool vSearched = false; // Обнаружение таблицы в эталонном списке таблиц
                            string vColumnName = ""; // Название колонки которую нужно удалить из истоника данных
                            foreach (datUnitModelField vField in vTableModel.__fFieldS)
                            {
                                vColumnName = vDataColumn.ColumnName;
                                if (vDataColumn.ColumnName.Trim().ToUpper() == vField.__fName.Trim().ToUpper())
                                {
                                    vSearched = true;
                                    break;
                                }
                            }
                            if (vSearched == false)
                            { /// Поле отсутствует в эталоне
                                __fStructureChanges.Add(appApplication.__oTunes.__mTranslate("Поле '{0}' нужно удалить", vDataRow + "." + vColumnName)); /// Необходимо удалить поле
                            }
                        }
                    }

                } /// Таблица присутствует в источнике данных
            }

            #endregion Проверка на вставку и изменение

            #region Проверка на необходимость удаления таблиц

            if (__fModelTableS.Count > 0)
            { /// Эталонный список существует
                foreach (string vTable in vTablesInDatabase)
                {
                    bool vSearched = false; // Обнаружение таблицы в эталонном списке таблиц
                    foreach (datUnitModelTable vTableModel in __fModelTableS)
                    {
                        if (vTable.Trim().ToUpper() == vTableModel.__fName.Trim().ToUpper())
                        {
                            vSearched = true;
                            break;
                        }
                    }
                    if (vSearched == false)
                    {
                        //string vCommand = "Drop Table " + vTable;
                        //_Command(vCommand); /// Удаление таблицы
                        //_StructureChanges.Add("Удалена таблица '" + vTable + "'");
                        __fStructureChanges.Add(appApplication.__oTunes.__mTranslate("Таблица '{0}' должна быть удалена", vTable)); /// Необходимо удалить таблицу
                    }
                }
            }

            #endregion Проверка на необходимость удаления таблиц

            /// Протоколирование операций которые нужно сделать в ручную 
            if (__fStructureChanges.Count > 0)
            {
                foreach (string vString in __fStructureChanges)
                {
                    appFileText vFileText = new appFileText();
                    /// Проверка наличия информации в файле
                    if (vFileText._mSearchExpression(Path.GetFileNameWithoutExtension(__fDatabaseName) + "_Correct.log", vString) == true)
                    {/// Добавление информации
                        vFileText.__mWriteToEnd(Path.Combine(__fDatabasePath, Path.GetFileNameWithoutExtension(__fDatabaseName) + "_Correct.log"), appTypeDateTime.__mDateToStringTillSecond(DateTime.Now) + " " + vString);
                    }
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Создание базы данных
        /// </summary>
        /// <param name="pDatabasePathFull">Полный путь и имя создаваемой базы данных</param>
        /// <returns>[true] - база данных создана, иначе - [false]</returns>
        public override bool __mDatabaseCreate()
        {
            bool vReturn = true; // Возвращаемое значение
            /// Если база данных отсутсвует - создается пустая база данных
            if (File.Exists(Path.Combine(__fDatabasePath, __fDatabaseName)) == false)
            {
                SQLiteConnection.CreateFile(Path.Combine(__fDatabasePath, __fDatabaseName));
                SQLiteConnection vConnection = new SQLiteConnection(string.Format("Data Source={0}; Version=3;", Path.Combine(__fDatabasePath, __fDatabaseName)));
                vConnection.Open();
                vConnection.Close();
                vReturn = true;
            }

            return vReturn;
        }
        /// <summary>
        /// Получение списка баз данных в источнике данных (Заполняется в профильном источнике данных)
        /// </summary>
        /// <returns>{ArrayList} - Список баз данных</returns>
        public override ArrayList __mDatabasesList()
        {
            ArrayList vReturn = new ArrayList(); // Возвращаемое значение

            string vCommand = "SELECT name FROM my_db.sqlite_master WHERE type='table'";
            DataTable vDataTable = __mSqlQuery(vCommand);
            foreach (DataRow vDataRow in vDataTable.Rows)
            {
                vReturn.Add(vDataRow["name"].ToString());
            }

            return vReturn;
        }
        /// <summary>
        /// Восстановление базы данных из копии
        /// </summary>
        /// <returns>[true] - Файл копии базы данных создан, иначе - [false]</returns>
        public override bool __mDatabaseRestore(string pFileName)
        {
            return false;
        }

        #endregion База данных

        #region Блокировки

        /// <summary>
        /// Закрытие блокировок текущего пользователя
        /// </summary>
        /// <param name="pUserClue">Идентификатор пользователя</param>
        public override void __mLockClear(int pUserClue = -1)
        {
            string vDateNull = Convert.ToDateTime("01.01.1900").Ticks.ToString(); // Нулевая дата

            if (pUserClue == -1)
                __mSqlCommand("Update RecLck Set dtmRecLckOff = " + vDateNull + " Where lnkUsr = " + __fUserClue.ToString() + " and dtmRecLckOff != " + vDateNull);
            else
                __mSqlCommand("Update RecLck Set dtmRecLckOff = " + vDateNull + " Where lnkUsr = " + pUserClue.ToString() + " and dtmRecLckOff != " + vDateNull);

            return;
        }
        /// <summary>
        /// Снятие блокировки
        /// </summary>
        /// <param name="pLockClue">Идентификатор блокировки</param>
        /// <returns>[true] - блокировка снята, иначе - [false]</returns>
        public override bool __mLockOff(int pLockClue)
        {
            bool vReturn = true; // Возвращаемое значение
            string vDateNull = Convert.ToDateTime("01.01.1900").Ticks.ToString(); // Нулевая дата

            if (__mSqlCommand("Update RecLck Set dtmRecLckOff = " + vDateNull + " Where CLU = " + pLockClue.ToString()) <= 0) /// Снятие блокировки
                vReturn = false;

            return vReturn;
        }
        /// <summary>
        /// Выполнение блокировки таблицы или записи в таблице 
        /// </summary>
        /// <param name="pRecord">Идентификатор записи</param>
        /// <remarks>Если 'pRecord' = 0, то блокируется вся таблица</remarks>
        /// <returns>Идентификатор заблокированной записи, [0] - запись не удалось заблокировать, [-1] - Блокировки отключены</returns>
        public override int __mLockOn(string pTableName, int pClue)
        {
            int vLockClue = -1; // Идентификатор заблокированной записи
            string vDateNull = Convert.ToDateTime("01.01.1900").Ticks.ToString(); // Нулевая дата

            /// Обнаружена блокировка текущего пользователя (Зависшая блокировка)
            if (__mTableRowsCountWhere("RecLck"
                                 , "desRecLck = '" + pTableName + "' " +
                                 " and lnkRID = " + pClue.ToString() +
                                 " and dtmRecLckOff = " + vDateNull +
                                 " and lnkUsr = " + __fUserClue.ToString()) > 0)
            {
                vLockClue = Convert.ToInt32(__mSqlValue("Select CLU From RecLck Where" +
                                                       " desRecLck = '" + pTableName + "'" +
                                                       " and lnkRID = " + pClue.ToString() +
                                                       " and dtmRecLckOff = " + vDateNull +
                                                       " and lnkUsr = " + __fUserClue.ToString()));
                /// Блокировка принимается для использования
                if (vLockClue > 0)
                    return vLockClue;
            }

            /// Поиск чужих блокировок 
            // Количество чужих не закрытых блокировок для полученных таблицы и идентификатора
            int vLockCount = __mTableRowsCountWhere("RecLck",
                                             "desRecLck = '" + pTableName + "'" +
                                             " and lnkRID = " + pClue.ToString() +
                                             " and dtmRecLckOff = " + vDateNull);
            /// Обнаружены чужие блокировки
            if (vLockCount > 0)
            {
                // Идентификатор пользователя забокировавшего запись
                int vUserClue = Convert.ToInt32(__mSqlValue("RecLck"
                    , "lnkUsr"
                    , "desRecLck = '" + pTableName + "'"
                    + " and lnkRid = " + pClue.ToString()
                    + " and dtmRecLckOff = " + vDateNull));
                // Псевдоним пользователя заблокировашего запись
                string vUserName = Convert.ToString(__mSqlValue("Usr"
                    , "desUsr"
                    , "CLU = " + vUserClue.ToString()));
                // Время создания блокировки
                DateTime vLockTime = Convert.ToDateTime(__mSqlValue("RecLck"
                    , "dtmRecLck_On"
                    , "desRecLck = '" + pTableName +
                    "' and lnkRid = " + pClue.ToString() +
                    " and dtmRecLckOff = " + vDateNull));

                /// Отображение ошибки блокировки
                appUnitError vUnitError = new appUnitError();
                vUnitError.__mMessageBuild(appApplication.__oTunes.__mTranslate("Запись заблокирована пользователем") + " '{0}' в {1}", vUserName.Trim(), vLockTime.ToString().Trim());
                vUnitError.__fProcedure_ = _fClassProcedure_;
                vUnitError.__fErrorType_ = ERRORSTYPES.Data;
                appApplication.__oErrorsHandler.__mShow(vUnitError);
                return -1;
            }
            /// Создание блокировки для полученных таблицы и идентификатора
            if (vLockCount == 0)
            {
                if (__mSqlCommand("Insert Into RecLck(desRecLck, dtmRecLck_On, lnkRid, lnkUsr) "
                    + "Values('" + pTableName + "'"
                    + ", " + vDateNull
                    + "," + pClue.ToString()
                    + "," + __fUserClue.ToString() + ")") > 0)
                    /// Получение идентификатора блокировки
                    vLockClue = __mClueLastInserted("RecLck");
            }

            return vLockClue;
        }

        #endregion Блокировки

        #region Выражения

        /// <summary>
        /// Создание выражения 'Like' с поиском на вхождение строки с использованием транслита
        /// </summary>
        /// <param name="pFieldName">Название поля для которого строиться выражение</param>
        /// <param name="pText">Текст условия на одной из раскладок клавиатуры</param>
        public override string __mExpressionLikeEntryTranslit(string pFieldName, string pText)
        {
            return "";
        }
        /// <summary>
        /// Создание выражения 'Like' с поиском сначало строки с использованием транслита
        /// </summary>
        /// <param name="pFieldName">Название поля для которого строиться выражение</param>
        /// <param name="pText">Текст условия на одной из раскладок клавиатуры</param>
        public override string __mExpressionLikeStartTranslit(string pFieldName, string pText)
        {
            return "";
        }

        #endregion Выражения

        #region Модель

        /// <summary>
        /// Получение типа данных поля для текущего типа источника данных
        /// </summary>
        /// <param name="pType">Значение перечисления типа данных полей</param>
        /// <returns></returns>
        public override string __mModelTableFieldType(datUnitModelField pModelField)
        {
            string vReturn = ""; // Возвращаемое значение
            switch (pModelField.__fDataType)
            {
                case COLUMNSTYPES.Bigint:
                    vReturn = "Int";
                    break;
                case COLUMNSTYPES.Binary:
                    vReturn = "Text";
                    break;
                case COLUMNSTYPES.Bit:
                    vReturn = "Int";
                    break;
                case COLUMNSTYPES.Char:
                    vReturn = "Text";
                    break;
                case COLUMNSTYPES.Date:
                    vReturn = "Int";
                    break;
                case COLUMNSTYPES.Datetime:
                    vReturn = "int";
                    break;
                case COLUMNSTYPES.Datetime2:
                    vReturn = "Int";
                    break;
                case COLUMNSTYPES.Datetimeoffset:
                    vReturn = "Int";
                    break;
                case COLUMNSTYPES.Decimal:
                    vReturn = "Real";
                    break;
                case COLUMNSTYPES.Float:
                    vReturn = "Real";
                    break;
                case COLUMNSTYPES.Geography:
                    vReturn = "Text";
                    break;
                case COLUMNSTYPES.Geomentry:
                    vReturn = "Text";
                    break;
                case COLUMNSTYPES.Hierarchyid:
                    vReturn = "Text";
                    break;
                case COLUMNSTYPES.Image:
                    vReturn = "Text";
                    break;
                case COLUMNSTYPES.Int:
                    vReturn = "Int";
                    break;
                case COLUMNSTYPES.Money:
                    vReturn = "Real";
                    break;
                case COLUMNSTYPES.Nchar:
                    vReturn = "Text";
                    break;
                case COLUMNSTYPES.Ntext:
                    vReturn = "Text";
                    break;
                case COLUMNSTYPES.Numeric:
                    vReturn = "Real)";
                    break;
                case COLUMNSTYPES.Nvarchar:
                    vReturn = "Text";
                    break;
                case COLUMNSTYPES.Real:
                    vReturn = "Real";
                    break;
                case COLUMNSTYPES.Rowversion:
                    vReturn = "Text";
                    break;
                case COLUMNSTYPES.Smalldatetime:
                    vReturn = "Int";
                    break;
                case COLUMNSTYPES.Smallint:
                    vReturn = "Int";
                    break;
                case COLUMNSTYPES.Smallmoney:
                    vReturn = "Real";
                    break;
                case COLUMNSTYPES.Sql_variant:
                    vReturn = "Sql_Variant";
                    break;
                case COLUMNSTYPES.Text:
                    vReturn = "Text";
                    break;
                case COLUMNSTYPES.Time:
                    vReturn = "Int";
                    break;
                case COLUMNSTYPES.Timestamp:
                    vReturn = "Int";
                    break;
                case COLUMNSTYPES.Tinyint:
                    vReturn = "Int";
                    break;
                case COLUMNSTYPES.Uniqueidentifier:
                    vReturn = "Text";
                    break;
                case COLUMNSTYPES.Varbinary:
                    vReturn = "Text";
                    break;
                case COLUMNSTYPES.Varchar:
                    vReturn = "Text";
                    break;
                case COLUMNSTYPES.Varcharmax:
                    vReturn = "Text";
                    break;
                case COLUMNSTYPES.Xml:
                    vReturn = "Text";
                    break;
            }

            return vReturn;
        }

        #endregion Модель

        #region Подключение

        /// <summary>
        /// Построение строки подключения к источнику данных
        /// </summary>
        /// <param name="pLogin">Использование логина с паролем</param>
        /// <returns>[true] - строка построена, иначе - [false]</returns>
        protected override bool __mConnectionLineBuild(bool pLogin)
        {
            bool vReturn = true; // Возвращаемое значение

            if (__fDatabaseName.Length != 0)
            {
                __fConnectionLine = "Data Source=" + Path.Combine(__fDatabasePath, __fDatabaseName) + ";Version=3;New=False;Compress=True;";
            }
            else
            {
                __fConnectionLine = "";

                appUnitError vUnitError = new appUnitError();
                vUnitError.__fProcedure_ = _fClassProcedure_;
                vUnitError.__fErrorType_ = ERRORSTYPES.Data;
                vUnitError.__mMessageBuild("Не указано имя базы данных");
                vUnitError.__mPropertyAdd("Вид источника данных: {0}", __fDataSourceType.ToString());

                appApplication.__oErrorsHandler.__mShow(vUnitError);

                return false;
            } ///  Имя базы данных не указаны

            if (pLogin == true) // Использовать логин для подключения
            {
                __fConnectionLine = "Data Source=" + Path.Combine(__fDatabasePath, __fDatabaseName) + ";Version=3;New=False;Password='" + __fServerPassword + "';Compress=True;";
            }
            else
            {
                __fConnectionLine = "Data Source=" + Path.Combine(__fDatabasePath, __fDatabaseName) + ";Version=3;New=False;Compress=True;";
            }

            return vReturn;
        }
        /// <summary>
        /// Разрыв соединения с источником данных
        /// </summary>
        protected override bool __mConnectionOff()
        {
            bool vReturn = true; // Возвращаемое значение

            if (_fConnection != null)
            {
                try
                {
                    _fConnection.Close();
                    _fConnection.Dispose();
                    _fConnection = null;
                }
                catch (Exception vException)
                {
                    appUnitError vError = new appUnitError();
                    vError.__fException = vException;
                    vError.__fProcedure_ = _fClassProcedure_;
                    vError.__fErrorType_ = ERRORSTYPES.Data;
                    vError.__mMessageBuild("Ошибка при отключении '{0}'", __fDatabaseName);
                    vError.__mPropertyAdd("Вид источника данных: {0}", __fDataSourceType.ToString());
                    vError.__mPropertyAdd("База данных: {0}", Path.Combine(__fDatabasePath, __fDatabaseName));

                    appApplication.__oErrorsHandler.__mShow(vError);

                    vReturn = false;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Установка соединения с источником данных
        /// </summary>
        /// <returns>[true] - соединение установлено, иначе - [false]</returns>
        protected override bool __mConnectionOn()
        {
            bool vReturn = true; // Возвращаемое значение

            if (__fConnectionLine.Length == 0)
            {
                try // Подключение с идентификацией пользователя
                {
                    __mConnectionLineBuild(true);
                    if (__fConnectionLine.Length > 0)
                    {
                        _fConnection = new SQLiteConnection(__fConnectionLine);
                        _fConnection.Open();
                    }
                    else
                        vReturn = false;
                }
                catch
                {
                    vReturn = false;
                }
                if (vReturn == false) // Подключение без Идентификации пользователя
                {
                    vReturn = true;
                    try
                    {
                        __mConnectionLineBuild(false);
                        _fConnection = new SQLiteConnection(__fConnectionLine);
                        _fConnection.Open();
                    }
                    catch
                    {
                        vReturn = false;
                    }
                }
            }
            else
            {
                try
                {
                    _fConnection = new SQLiteConnection(__fConnectionLine);
                    _fConnection.Open();
                }
                catch (SQLiteException vException)
                {
                    appUnitError vError = new appUnitError();
                    vError.__fException = vException;
                    vError.__fProcedure_ = _fClassProcedure_;
                    vError.__fErrorType_ = ERRORSTYPES.Data;
                    vError.__mMessageBuild("Ошибка подключения базы данных '{0}'", __fDatabaseName);
                    vError.__mPropertyAdd("Вид источника данных: {0}", __fDataSourceType.ToString());
                    vError.__mPropertyAdd("База данных: {0}", Path.Combine(__fDatabasePath, __fDatabaseName));

                    appApplication.__oErrorsHandler.__mShow(vError);

                    vReturn = false;
                }
            }

            return vReturn;
        }

        #endregion Подключение

        #region Пользователи

        /// <summary>
        /// Получение права пользователя 
        /// </summary>
        /// <param name="pRight">Право</param>
        /// <param name="pClueUser">Пользователь</param>
        /// <returns>[true] - доступ разрешен, иначе - [false]</returns>
        public override bool __mUserAccess(int pRight, int pClueUser)
        {
            return false;
        }

        #endregion Пользователи

        #region Таблицы

        /// <summary>
        /// Получение пустого курсора со структурой таблицы из базы данных (заполняется в базовом классе)
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns>? Таблица с одной пустой записью</returns>
        public override DataTable __mTableEmpty(string pTableName)
        {
            return __mSqlQuery("Select * From " + pTableName + " Where CLU < 0");
        }
        /// <summary>
        /// Получение списка таблиц в базе данных
        /// </summary>
        /// <returns>Список таблиц в базе данных</returns>
        public override ArrayList __mTablesList()
        {
            ArrayList vReturn = new ArrayList(); // Возвращаемое значение

            string vCommand = "SELECT name FROM sqlite_master WHERE type='table'";
            DataTable vDataTable = __mSqlQuery(vCommand);
            foreach (DataRow vDataRow in vDataTable.Rows)
            {
                vReturn.Add(vDataRow["name"].ToString());
            }

            return vReturn;
        }
        /// <summary>
        /// Очистка таблицы со сбросом идентификатора в 0
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns>[true] - таблица очищена, иначе - [false]</returns>
        public override bool __mTableTruncate(string pTableName)
        {
            bool vReturn = true; // Возвращаемое значение

            try
            {
                __mSqlCommand("Delete From '" + pTableName + "'");
                __mSqlCommand("Update 'sqlite_sequence' Set 'Seq' = 0 Where Name = '" + pTableName + "'");
                __mSqlCommand("reindex '" + pTableName + "'");
            }
            catch
            {
                vReturn = false;
            }

            return vReturn;
        }

        #endregion Таблицы

        #region Таблицы - Строки

        /// <summary>
        /// Получение записи из таблицы указанной идентификатором
        /// </summary>
        /// <param name="pClue">Идентификатор записи</param>
        /// <returns>[DataTable]</returns>
        public override DataTable __mTableRow(string pTableName, int pClue)
        {
            string vQuery = "Select * From " + pTableName + " Where CLU = " + pClue.ToString() + " Limit 1";
            DataTable vReturn = __mSqlQuery(vQuery);
            if (vReturn.Rows.Count == 0)
                vReturn = null;

            return vReturn;
        }
        /// <summary>
        /// Получение записи из таблицы указанной идентификатором
        /// </summary>
        /// <param name="pGuid">Уникальный идентификатор записи</param>
        /// <returns>[DataTable]</returns>
        public override DataTable __mTableRow(string pTableName, Guid pGuid)
        {
            string vQuery = "Select * From " + pTableName + " Where GUI = '" + pGuid.ToString() + "' Limit 1";
            DataTable vReturn = __mSqlQuery(vQuery);
            if (vReturn.Rows.Count == 0)
                vReturn = null;

            return vReturn;
        }
        /// <summary>
        /// Установка текущего времени в качестве последнего времени изменения записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор записи</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        public override bool __mTableRowChangeTimeNow(string pTableName, int pClue)
        {
            bool vReturn = false; // Возвращаемое значение

            if (__mSqlCommand("Update " + pTableName + " Set CHG = " + DateTime.Now.Ticks + " Where CLU = " + pClue.ToString()) > 0)
                vReturn = true;

            return vReturn;
        }
        /// <summary>
        /// Подсчет количества дублирующихся записей
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldS">Список полей</param>
        /// <returns>[null] - если данные не найдены, иначе - {DataTable} - таблица с названием поля и количеством повторений</returns>
        public override DataTable __mTableRowsCountDouble(string pTableName, params string[] pFieldS)
        {
            string vQuery = "Select";
            string vGroup = "Group By";
            int vFieldNumber = 0;
            foreach (string vField in pFieldS)
            {
                if (vFieldNumber != 0)
                {
                    vQuery = vQuery + ", ";
                    vGroup = vGroup + ", ";
                }
                vQuery = vQuery + vField;
                vGroup = vGroup + vField;
            }
            vQuery = vQuery + "Count(*)";

            return __mSqlQuery(vQuery + " From " + pTableName + vGroup + " Having Count(*) > 1");
        }
        /// <summary>
        /// Подсчет количества записей удовлетворяющих условию
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pExpressionWhere">Условие для подсчета записей</param>
        /// <returns>Количество подсчитанных записей</returns>
        public override int __mTableRowsCountWhere(string pTableName, string pExpressionWhere)
        {
            int vReturn = -1; // Возвращаемое значение

            DataTable vDataTabl = __mSqlQuery("Select Count(*) as NumCou From " + pTableName + " Where " + pExpressionWhere);
            if (vDataTabl != null)
            {
                if (vDataTabl.Rows.Count > 0)
                    vReturn = Convert.ToInt32(vDataTabl.Rows[0][0]);
            }

            return vReturn;
        }

        #endregion Таблицы - Строки

        #region Таблицы - Поля

        /// <summary>
        /// Проверка существования поля в таблице источника данных
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldName">Название поля</param>
        /// <returns>[true] - Поле существует, иначе - [false]</returns>
        public override bool __mTableColumnExists(string pTableName, string pFieldName)
        {
            bool vReturn = false; // Возвращаемое значение

            DataTable vDataTable = __mSqlQuery("Select * From " + pTableName + " Limit 1");
            vReturn = vDataTable.Columns.Contains(pFieldName);

            return vReturn;
        }
        /// <summary>
        /// Получение списка полей в таблице
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns>[null] - данные не обнаружены, иначе {DataTable} заполненная списком полей указанной таблицы</returns>
        public override DataTable __mTableColumnS(string pTableName)
        {
            DataTable vReturn = new DataTable(); /// Возвращаемое значение
            vReturn.Columns.Add("Ord", typeof(int));
            vReturn.Columns.Add("desFld", typeof(string));
            vReturn.Columns.Add("Dcr", typeof(string));
            vReturn.Columns.Add("Typ", typeof(string));
            vReturn.Columns.Add("Pre", typeof(decimal));
            vReturn.Columns.Add("Sca", typeof(decimal));
            vReturn.Columns.Add("Nul", typeof(bool));
            vReturn.Columns.Add("Dft", typeof(string));
            vReturn.Columns.Add("Cll", typeof(string));

            string vQuery = "PRAGMA table_info(" + pTableName + ");";
            DataTable vDataTable = __mSqlQuery(vQuery);
            foreach (DataRow vDataRow in vDataTable.Rows)
            {
                DataRow vReturnRow = vReturn.NewRow();
                vReturnRow["Ord"] = Convert.ToInt32(vDataRow["cid"]) + 1;
                vReturnRow["desFld"] = Convert.ToString(vDataRow["name"]);
                vReturnRow["Dsc"] = "";
                vReturnRow["Typ"] = Convert.ToString(vDataRow["type"]);
                vReturnRow["Pre"] = 0;
                vReturnRow["Sca"] = 0;
                vReturnRow["Nul"] = (Convert.ToBoolean(vDataRow["notnull"]) ? false : true);
                vReturnRow["Dft"] = Convert.ToString(vDataRow["dflt_value"]);
                vReturnRow["Cll"] = "";
            }

            return vReturn;
        }
        /// <summary>
        /// Получение информации о поле таблицы 
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pField">Название поля</param>
        /// <param name="pFieldInfo">Вид операции</param>
        /// <returns>Запрашиваемое значение, иначе [null]</returns>
        public override object __mTableColumnInfo(string pTableName, string pField, FIELDINFO pFieldInfo)
        {
            return null;
        }

        #endregion Таблицы - Поля

        #region Транзакции

        /// <summary>
        /// Закрытие транзакции
        /// </summary>
        /// <param name="pCommit">Условие закрытия транзакции. [true] - [Commit], [false] - [RollBack]</param>
        /// <returns>[true] - Транзакция закрыта, иначе - [false]</returns>
        public override bool __mTransactionOff(bool pCommit)
        {
            bool vReturn = true; /// Возвращаемое значение

            if (_fTransaction != null)
            {
                if (pCommit == true)
                {
                    try
                    {
                        _fTransaction.Commit();
                    }
                    catch (SQLiteException vException)
                    {
                        appUnitError vUnitError = new appUnitError();
                        vUnitError.__fException = vException;
                        vUnitError.__fProcedure_ = _fClassProcedure_;
                        vUnitError.__fErrorType_ = ERRORSTYPES.Data;
                        vUnitError.__mMessageBuild("Невозможно завершить транзакцию");
                        vUnitError.__mPropertyAdd("Вид источника данных: {0}", __fDataSourceType.ToString());
                        vUnitError.__mPropertyAdd("База данных: {0}", Path.Combine(__fDatabasePath, __fDatabaseName));
                        if (_fConnection == null)
                            vUnitError.__mPropertyAdd("Соединение: {0}", "null");
                        else
                            vUnitError.__mPropertyAdd("Соединение: {0}", _fConnection.State.ToString());
                        if (pCommit == true)
                            vUnitError.__mPropertyAdd("Команда: {0}", appApplication.__oTunes.__mTranslate("Закрытие транзакции"));
                        else
                            vUnitError.__mPropertyAdd("Команда: {0}", appApplication.__oTunes.__mTranslate("Откат транзакции"));

                        appApplication.__oErrorsHandler.__mShow(vUnitError);

                        vReturn = false;
                    }
                    vReturn = true;
                }
                else
                {
                    try
                    {
                        _fTransaction.Rollback();
                        vReturn = true;
                    }
                    catch
                    {
                        vReturn = false;
                    }
                }
                if (vReturn == true)
                    _fTransaction = null; // ???
            }

            return vReturn;
        }
        /// <summary>
        /// Открытие транзакции
        /// </summary>
        /// <returns>[true] - транзация создана, иначе - [false]</returns>
        public override bool __mTransactionOn()
        {
            bool vReturn = true; // Возвращаемое значение
            if (_fConnection != null)
            {
                try
                {
                    _fTransaction = _fConnection.BeginTransaction();
                }
                catch (SQLiteException vException)
                {
                    appUnitError vUnitError = new appUnitError();
                    vUnitError.__fException = vException;
                    vUnitError.__fProcedure_ = _fClassProcedure_;
                    vUnitError.__fErrorType_ = ERRORSTYPES.Data;
                    vUnitError.__mMessageBuild("Не возможно открыть транзакцию");
                    vUnitError.__mPropertyAdd("Вид источника данных: {0}", __fDataSourceType.ToString());
                    vUnitError.__mPropertyAdd("База данных: {0}", Path.Combine(__fDatabasePath, __fDatabaseName));
                    if (_fConnection == null)
                        vUnitError.__mPropertyAdd("Соединение: {0}", "null");
                    else
                        vUnitError.__mPropertyAdd("Сoединение: {0}", _fConnection.State.ToString());

                    appApplication.__oErrorsHandler.__mShow(vUnitError);

                    vReturn = false;
                }
            }

            return vReturn;
        }

        #endregion Транзакции

        #region Функции - Идентификаторы

        /// <summary>
        /// Получение идентификатора по учетному коду
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pCode">Учетный код</param>
        /// <returns>[<=0] - Запись не найдена, иначет - идентификатор</returns>
        public override int __mClueByCode(string pTableName, int pCode)
        {
            int vReturn = -1; // Возвращаемое значение

            vReturn = Convert.ToInt32(__mSqlValue("Select CLU From " + pTableName + " Where cod" + pTableName + "=" + pCode.ToString()).ToString());

            return vReturn;
        }
        /// <summary>
        /// Получение идентификатора записи по значению поля названия
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldName">Значение поля названия</param>
        /// <returns>[<=0] - Запись не найдена, иначет - идентификатор</returns>
        public override int __mClueByName(string pTableName, string pFieldNameValue)
        {
            int vReturn = -1; // Возвращаеое значение

            DataTable vDataTabl = __mSqlQuery("Select CLU From " + pTableName + " Where des" + pTableName + " = '" + pFieldNameValue + "'");
            if (vDataTabl.Rows.Count > 0)
                vReturn = Convert.ToInt32(vDataTabl.Rows[0][0]);

            return vReturn;
        }
        /// <summary>
        /// Получение идентификатора записи по названию поля опции
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldOptionName">Имя поля опции</param>
        /// <returns>[<=0] - Запись не найдена, иначет - идентификатор</returns>
        public override int __mClueByOption(string pTableName, string pFieldOptionName)
        {
            int vReturn = -1; // Возвращаеое значение

            DataTable vDataTabl = __mSqlQuery("Select CLU From " + pTableName + " Where " + pFieldOptionName + " = 1");
            if (vDataTabl.Rows.Count > 0)
                vReturn = Convert.ToInt32(vDataTabl.Rows[0][0].ToString());

            return vReturn;
        }
        /// <summary>
        /// Проверка существования идентификатора в таблице
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор записи</param>
        /// <returns>[<=0] - Запись не найдена, иначет - идентификатор</returns>
        public override bool __mClueExists(string pTableName, int pClue)
        {
            bool vReturn = false;
            if (__mTableRowsCountWhere(pTableName, " CLU = " + pClue.ToString()) > 0)
                vReturn = true;

            return vReturn;
        }
        /// <summary>
        /// Получение идентификатора последеней вставленной записи в таблицу
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns>[<=0] - Запись не найдена, иначе - идентификатор</returns>
        public override int __mClueLastInserted(string pTableName)
        {
            DataTable vrDataTable = __mSqlQuery("Select MAX(CLU) FROM " + pTableName);
            // SELECT MAX(ochki) FROM db
            if (vrDataTable.Rows.Count > 0)
            {
                return Convert.ToInt32(vrDataTable.Rows[0][0]);
            }
            else
            {
                return -1;
            }

        }

        #endregion Функции - Идентификаторы

        #region Функции - Названия

        /// <summary>
        /// Получение названия по идентификатору записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор искомой строки</param>
        /// <returns>[""] - значение не найдено, иначе - значение поля 'Название'</returns>
        public override string __mNameByClue(string pTableName, int pClue)
        {
            string vReturn = "null"; // Возвращаемое значение

            DataTable vDataTabl = __mSqlQuery("Select des" + pTableName + " From " + pTableName + " Where  CLU = " + pClue.ToString());
            if (vDataTabl.Rows.Count > 0)
                vReturn = vDataTabl.Rows[0][0].ToString().Trim();

            return vReturn;
        }
        /// <summary>
        /// Получение названия по идентификатору записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pCode">Идентификатор искомой строки</param>
        /// <returns>[""] - значение не найдено, иначе - значение поля 'Название'</returns>
        public override string __mNameByCode(string pTableName, int pCode)
        {
            string vReturn = "null"; // Возвращаемое значение

            DataTable vDataTabl = __mSqlQuery("Select des" + pTableName + " From " + pTableName + " Where  cod" + pTableName + " = " + pCode.ToString());
            if (vDataTabl.Rows.Count > 0)
                vReturn = vDataTabl.Rows[0][0].ToString().Trim();

            return vReturn;
        }
        /// <summary>
        /// Получение названия справочника по названию и значению опции
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pOptionName">Название опции</param>
        /// <returns>Значение поля 'Название' </returns>
        public override string __mNameByOption(string pTableName, string pOptionName)
        {
            return "null";
        }
        /// <summary>
        /// Проверка существования названия
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pName">Значение поля названия</param>
        /// <param name="pClueSkip">Идентификатор исключаемой записи</param>
        /// <returns>[true] - Указанное название уже существует, иначе - [false]</returns>
        public override bool __mNameExists(string pTableName, string pValue, int pClueSkip)
        {
            bool vReturn = false; // Возвращаемое значение

            DataTable vDataTabl = __mSqlQuery("Select des" + pTableName + " as des" + pTableName + " From " + pTableName + " Where des" + pTableName + " = '" + pValue + "' and CLU != '" + pClueSkip.ToString() + "'");
            if (vDataTabl.Rows.Count > 0)
                vReturn = true;

            return vReturn;
        }

        #endregion Функции - Названия

        #region Функции - Учетные коды

        /// <summary>
        /// Получение значения поля учетного кода по идентификатору записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор записи</param>
        /// <returns>[<=0] - Запись не найдена, иначе - учетный код</returns>
        public override int __mCodeByClue(string pTableName, int pClue)
        {
            return Convert.ToInt32(__mSqlValue("Select cod" + pTableName + " From " + pTableName + " Where CLU = " + pClue.ToString()));
        }
        /// <summary>
        /// Проверка существования учетного кода исключая идентификатор записи указанный в 'pClueSkip'
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pCodeCheck">Проверяемый учетный код</param>
        /// <param name="pClueSkip">Идентификатор записи который нужно исключить из поиска</param>
        /// <returns>[true] - Запись найдена, иначе - [false]</returns>
        public override bool __mCodeExists(string pTableName, int pCodeCheck, int pClueSkip)
        {
            if (__mTableRowsCountWhere(pTableName, "cod" + pTableName + " = " + pCodeCheck.ToString() + " and CLU != " + pClueSkip.ToString()) > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Вычисление нового учетного кода
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pCodeNewCalculateType">Порядок расчета нового учетного кода</param>
        /// <param name="pValueMaximal">Минимальное значение кода</param>
        /// <param name="pValueMinimal">Максимальное значение кода</param>
        /// <returns>[0] - неудалось вычислить учетный код, иначе - новый учетный код</returns>
        public override int __mCodeNew(string pTableName, int pRecordSkip, CODESNEWTYPES pCodeNewCalculateType, int pValueMinimal = 1, int pValueMaximal = 9999999)
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
        public override int __mCodeNewGroup(string pTableName, int pClueSkip, CODESNEWTYPES pCodeNewCalculateType, int pValueMinimal, int pValueMaximal, ArrayList pFieldS)
        {
            return -1;
        }

        #endregion Функции - Учетные коды

        #region Функции - Позиция в документе

        /// <summary>
        /// Вычисление новой позиции в документе
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClueSkip">Идентификатор записи исключаемой из обработки</param>
        /// <param name="pWhere">Условие отбирающее документ</param>
        /// <returns></returns>
        public override int __mPositionNew(string pTableName, int pClueSkip, string pWhere)
        {
            return -1;
        }

        #endregion Функция - Позиция в документе

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Внутренние

        /// <summary>
        /// Указатель на соединение с источником данных
        /// </summary>
        private SQLiteConnection _fConnection = null;
        /// <summary>
        /// Указатель на открытую транзакцию
        /// </summary>
        private SQLiteTransaction _fTransaction = null;

        #endregion Внутренние

        #endregion ПОЛЯ    
    }
}
