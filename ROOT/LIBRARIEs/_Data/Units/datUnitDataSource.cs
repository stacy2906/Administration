using nlApplication;
using nlReportHtml;
using nlSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;

namespace nlData
{
    /// <summary>
    /// Файл datUnitDataSource.cs
    /// </summary>
    /// <remarks>Класс-единица 'Источник данных'</remarks>
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 14-16</version> // Дата-время последней корректировки
    public class datUnitDataSource
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public datUnitDataSource()
        {
            _mObjectAssembly();
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Объект

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        {
            _fError = new appUnitError(_fClassFilePath_);

            return;
        }

        #endregion Объект

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

        #region Sql операции

        /// <summary>
        /// Отправка команды источнику данных
        /// </summary>
        /// <param name="pCommand">Команда отправляемая источнику данных</param>
        /// <returns>Количество обработанных командой записей</returns>
        public virtual int __mSqlCommand(string pCommand)
        {
            return -1;
        }
        /// <summary>
        /// Подсчет количества записей в таблице удовлетворяющих условию
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pExpressionWhere">Условия проверки</param>
        /// <returns></returns>
        public virtual int __mSqlCount(string pTableName, string pExpressionWhere)
        {
            return -1;
        }
        /// <summary>
        /// Отправка запроса источнику данных
        /// </summary>
        /// <param name="pQuery">Условие запроса</param>
        /// <returns>{DataTable} - с данными удовлетворяющими условию запроса</returns>
        public virtual DataTable __mSqlQuery(string pQuery)
        {
            return null;
        }
        /// <summary>
        /// Выполнение хранимой процедуры источника данных
        /// </summary>
        /// <param name="pStoredProcedure">Название хранимой процедуры</param>
        /// <param name="pParameters">Параметры хранимой процедуры</param>
        /// <returns>[DataTable]</returns>
        public virtual DataTable __mSqlStoredProcedures(string pStoredProcedure, params object[] pParameters)
        {
            return null;
        }
        /// <summary>
        /// Получение значения поля удовлетворяющего команде
        /// </summary>
        /// <param name="pCommand">Команда для получения значения поля</param>
        /// <returns>{object} - значение поля</returns>
        public virtual object __mSqlValue(string pCommand)
        {
            return null;
        }
        /// <summary>
        /// Получение значения поля по имени таблицы, имени поля и условию
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldName">Название поля</param>
        /// <param name="pExpressionWhere">Условие поиска записи</param>
        /// <returns>[null] - если значение не найдено, иначе - {object} - значение поля</returns>
        public virtual object __mSqlValue(string pTableName, string pFieldName, string pExpressionWhere)
        {
            return null;
        }
        /// <summary>
        /// Выполнение скрипта
        /// </summary>
        /// <param name="pFilePath">Путь и имя файла скрипта</param>
        /// <param name="pWaitForFinish">Ожидать завершения выполнения</param>
        /// <returns>[true] - скрипт выполнен, иначе - [false]</returns>
        public virtual bool __mSqlSriptFileRun(string pFilePath, bool pWaitForFinish = true)
        {
            return false;
        }

        #endregion Sql операции

        #region База данных

        /// <summary>
        /// Создание резервной копии базы данных
        /// </summary>
        /// <returns>Путь созданного файла копии базы данных</returns>
        public virtual string __mDatabaseBackUp()
        {
            return "";
        }
        /// <summary>
        /// Сравнение структуры таблиц в базе данных с моделью приложения
        /// </summary>
        /// <returns>[true] - структуры одинаковы, иначе - [false]</returns>
        public virtual bool __mDatabaseCompareWithModel()
        {
            return false;
        }
        /// <summary>
        /// Создание базы данных
        /// </summary>
        /// <returns>[true] - база данных создана, иначе - [false]</returns>
        public virtual bool __mDatabaseCreate()
        {
            return false;
        }
        /// <summary>
        /// Создание скрипта создания базы данных
        /// </summary>
        /// <param name="pFileScriptPath">Путь к создаваемому файлу</param>
        /// <param name="pFileDatabasePath">Путь к создаваемой базе данных</param>
        /// <param name="pFileStream">База данных должна работать с File Stream</param>
        /// <param name="pDatabaseCollate">Название сортировки базы данных</param>
        /// <param name="pProductionVersion">Номер производственной версии</param>
        /// <returns>[true] - Файл скрипта создан, иначе - [false]</returns>
        public virtual bool __mDatabaseCreateScriptForCreateDatabase(string pFileScriptPath, string pFileDatabasePath, bool pFileStream = false, string pDatabaseCollate = "", string pProductionVersion = "")
        {
            return false;
        }
        /// <summary>
        /// Восстановление базы данных из копии
        /// </summary>
        /// <param name="pFileName">Путь и имя файла копии</param>
        /// <returns>[true] - Файл копии базы данных создан, иначе - [false]</returns>
        public virtual bool __mDatabaseRestore(string pFileName)
        {
            return false;
        }
        /// <summary>
        /// Печать структуры базы данных в источнике
        /// </summary>
        public string __mDatabaseStructurePrint()
        {
            rhtReport vReport = new rhtReport();
            vReport.__mCreate();
            vReport.__fTitle = datApplication.__oTunes.__mTranslate("Структура базы данных '{0}'", __fDatabaseName);
            vReport.__fColumnsCountInReport = 9;

            /// Отображение заголовка
            vReport.__mRow();
            vReport.__mCell(vReport.__fTitle, "CL=Caption", "SC=" + vReport.__fColumnsCountInReport.ToString());
            vReport.__mRowEmpty();
            vReport.__mTime("CL=TimeUser");
            //vReport.__mUser(__fUserAlias, "CL=TimeUser");
            vReport.__mRowEmpty();

            /// Отображение данных
            /// Перебор таблиц
            foreach (string vDataRowTable in __mTablesList())
            {
                vReport.__mRowEmpty();
                vReport.__mRow();
                vReport.__mCell(vDataRowTable.Trim() + " - " + __mTableDescription(vDataRowTable.Trim()).Trim(), "SC=Max");
                /// Построение заголовка таблицы
                vReport.__mRow();
                vReport.__mCell(datApplication.__oTunes.__mTranslate("№ п/п"), "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Название"), "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Тип"), "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Точность"), "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Масштаб"), "CL=HeaderCell");
                vReport.__mCell("Null", "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Описание"), "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Значение"), "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Сортировка"), "CL=HeaderCell");

                /// Перебор полей
                foreach (DataRow vDataRowField in __mTableColumnS(vDataRowTable.Trim()).Rows)
                {
                    vReport.__mRow();
                    vReport.__mCell(vDataRowField["Ord"].ToString(), "CL=DataCell", "A=center");
                    vReport.__mCell(vDataRowField["dsiFld"].ToString(), "CL=DataCell");
                    vReport.__mCell(vDataRowField["Typ"].ToString(), "CL=DataCell");
                    vReport.__mCell(vDataRowField["Pre"].ToString(), "CL=DataCell");
                    vReport.__mCell(vDataRowField["Sca"].ToString(), "CL=DataCell");
                    vReport.__mCell(vDataRowField["Nul"].ToString(), "CL=DataCell");
                    vReport.__mCell(vDataRowField["Dcr"].ToString(), "CL=DataCell");
                    vReport.__mCell(vDataRowField["Dft"].ToString(), "CL=DataCell");
                    vReport.__mCell(vDataRowField["CLL"].ToString(), "CL=DataCell");
                }
                /// Отображение индексов
                {
                    DataTable vDataTableKeys = __mSqlQuery("Select * From INFORMATION_SCHEMA.KEY_COLUMN_USAGE Where table_name = '" + vDataRowTable + "' Order By CONSTRAINT_NAME Desc");
                    vReport.__mRow();
                    vReport.__mCell(datApplication.__oTunes.__mTranslate("Ключи"), "CB=#FFFFEE");
                    vReport.__mCell(datApplication.__oTunes.__mTranslate("Поля"), "CB=#FFFFEE");
                    vReport.__mCell(datApplication.__oTunes.__mTranslate("Связанная таблица"), "CB=#FFFFEE");
                    vReport.__mCell(datApplication.__oTunes.__mTranslate("Первичное поле"), "CB=#FFFFEE");
                    foreach (DataRow vDataRowKeys in vDataTableKeys.Rows)
                    {
                        vReport.__mRow();
                        vReport.__mCell(vDataRowKeys["CONSTRAINT_NAME"].ToString(), "CL=DataCell", "A=center");
                        vReport.__mCell(vDataRowKeys["COlUMN_NAME"].ToString(), "CL=DataCell", "A=center");
                        if (vDataRowKeys["CONSTRAINT_NAME"].ToString().Substring(0, 2) != "PK")
                        {
                            vReport.__mCell(vDataRowKeys["COlUMN_NAME"].ToString().PadRight(3, ' ').Substring(3), "CL=DataCell", "A=center");
                            vReport.__mCell("CLU" + vDataRowKeys["COlUMN_NAME"].ToString().PadRight(3, ' ').Substring(3), "CL=DataCell", "A=center");
                        }
                        else
                        {
                            vReport.__mCell("", "CL=DataCell", "A=center");
                            vReport.__mCell("", "CL=DataCell", "A=center");
                        }
                    }
                }
            }

            vReport.__fAtOnceExcel = true;
            vReport.__fFileExcelName = "StructureDatabase";
            vReport.__mFile();

            return vReport.__fFileExcelName;
        }
        /// <summary>
        /// Удаление базы данных
        /// </summary>
        /// <param name="pDatabaseName">Имя базы данных</param>
        /// <returns></returns>
        public virtual bool __mDatabaseDrop(string pDatabaseName)
        {
            return false;
        }
        /// <summary>
        /// Сжатие базы данных
        /// </summary>
        /// <returns>[true] - Сжатие базы данных, иначе - [false]</returns>
        public virtual bool __mDatabaseShrink()
        {
            return false;
        }
        /// <summary>
        /// Проверка существования базы данных
        /// </summary>
        /// <param name="pDatabaseName">Имя базы данных</param>
        /// <returns>[true] - база данных существует, иначе - [false]</returns>
        public virtual bool __mDatabaseExists(string pDatabaseName)
        {
            return false;
        }
        /// <summary>
        /// Получение списка баз данных
        /// </summary>
        /// <returns></returns>
        public virtual ArrayList __mDatabasesList()
        {
            return null;
        }

        #endregion База данных

        #region Блокировки

        /// <summary>
        /// Закрытие блокировок текущего пользователя
        /// </summary>
        /// <param name="pUserClue">Идентификатор пользователя</param>
        public virtual void __mLockClear(int pUserClue = -1)
        {
            return;
        }
        /// <summary>
        /// Снятие блокировки
        /// </summary>
        /// <param name="pLockClue">Идентификатор блокировки</param>
        /// <returns>[true] - блокировка снята, иначе - [false]</returns>
        public virtual bool __mLockOff(int pLockClue)
        {
            return false;
        }
        /// <summary>
        /// Выполнение блокировки таблицы или записи в таблице 
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pLockClue">Идентификатор записи</param>
        /// <remarks>Если 'pRecord' = 0, то блокируется вся таблица</remarks>
        /// <returns>Идентификатор заблокированной записи, [0] - запись не удалось заблокировать, [-1] - Блокировки отключены</returns>
        public virtual int __mLockOn(string pTableName, int pLockClue)
        {
            return -1;
        }
        /// <summary>
        /// Исправление в таблице блокировок идентификатора записи заблокированной записи равного 0 на фактический идентификатор 
        /// </summary>
        /// <param name="pLockClue">Идентификатор записи в таблице блокировок</param>
        /// <param name="pLinkRid">Идентификатор заблокированной записи</param>
        /// <returns>[true] - данные исправлены, иначе [false]</returns>
        public virtual bool __mLockLnkRidChange(int pLockClue, int pLinkRid)
        {
            return false;
        }

        #region Выражения

        /// <summary>
        /// Создание выражения 'Like' с поиском на вхождение строки с использованием транслита
        /// </summary>
        /// <param name="pFieldName">Название поля для которого строиться выражение</param>
        /// <param name="pText">Текст условия на одной из раскладок клавиатуры</param>
        public virtual string __mExpressionLikeEntryTranslit(string pFieldName, string pText)
        {
            return "";
        }
        /// <summary>
        /// Создание выражения 'Like' с поиском сначало строки с использованием транслита
        /// </summary>
        /// <param name="pFieldName">Название поля для которого строиться выражение</param>
        /// <param name="pText">Текст условия на одной из раскладок клавиатуры</param>
        public virtual string __mExpressionLikeStartTranslit(string pFieldName, string pText)
        {
            return "";
        }

        #endregion Выражения

        #endregion Блокировки

        #region Источник данных

        /// <summary>
        /// Получение списка баз данных в источнике данных
        /// </summary>
        /// <returns>{ArrayList} - Список баз данных</returns>
        public virtual ArrayList __mDataSourceDatabasesList()
        {
            return null;
        }
        /// <summary>
        /// Получение списка доступных серверов
        /// </summary>
        /// <returns>{DataTable} - Таблица со списком доступных серверов</returns>
        public virtual DataTable __mDataSourceServersList()
        {
            return null;
        }

        #endregion Источник данных

        #region Модель

        /// <summary>
        /// Построение модели источника данных моделям таблиц
        /// </summary>
        public virtual void __mModelBuild()
        {
            return;
        }
        /// <summary>
        /// Создание отчета сравнения модели с базой данных 
        /// </summary>
        public string _mModelCompareWithDatabase()
        {
            __mModelBuild();
            rhtReport vReport = new rhtReport();
            vReport.__mCreate();
            vReport.__fTitle = datApplication.__oTunes.__mTranslate("Сравнение модели с базой данных '{0}'", __fDatabaseName);
            vReport.__fColumnsCountInReport = 10;

            /// Отображение заголовка
            vReport.__mRow();
            vReport.__mCell(vReport.__fTitle, "CL=Caption", "SC=" + vReport.__fColumnsCountInReport.ToString());
            vReport.__mRowEmpty();
            vReport.__mTime("CL=TimeUser");
            vReport.__mUser(datApplication.__oData.__mUserAlias(), "CL=TimeUser");
            vReport.__mRowEmpty();

            /// Отображение данных
            /// Перебор таблиц
            foreach (datUnitModelTable vModelTable in __fModelTableS)
            {
                vReport.__mRowEmpty();
                vReport.__mRow();
                vReport.__mCell(vModelTable.__fName + " " + vModelTable.__fDescription, "SC=Max=Max");
                /// Построение заголовка таблицы
                vReport.__mRow();
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Название"), "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Тип"), "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Тип") + " DB", "CL=HeaderCellSelected");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Точность"), "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Масштаб"), "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Точность") + " DB", "CL=HeaderCellSelected");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Масштаб") + " DB", "CL=HeaderCellSelected");
                vReport.__mCell("Null", "CL=HeaderCell");
                vReport.__mCell("Null DB", "CL=HeaderCellSelected");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Описание"), "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Описание DB"), "CL=HeaderCellSelected");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Статус"), "CL=HeaderCell");

                ArrayList vFieldSInModel = new ArrayList();
                /// Перебор полей
                foreach (datUnitModelField vModelField in vModelTable.__fFieldS)
                {
                    /// Если поле отсутствует в базе данных
                    if (__mTableColumnInfo(vModelTable.__fName, vModelField.__fName, FIELDINFO.Type) == null)
                    {
                        int vRow = vReport.__mRow();
                        vReport.__mCell(vModelField.__fName, "CL=DataCellRed"); // Название
                        vReport.__mCell(vModelField.__fDataType.ToString().ToLower(), "CL=DataCellRed"); // Тип
                        vReport.__mCell(__mTableColumnInfo(vModelTable.__fName, vModelField.__fName, FIELDINFO.Type), "CL=DataCellRed"); // Тип DB
                        vReport.__mCell(vModelField.__fSize, "CL=DataCellRed"); // Точность
                        vReport.__mCell(vModelField.__fSizeDecimal, "CL=DataCellRed"); // Масштаб
                        vReport.__mCell(__mTableColumnInfo(vModelTable.__fName, vModelField.__fName, FIELDINFO.Precision), "CL=DataCellRed"); // Точность DB
                        vReport.__mCell(__mTableColumnInfo(vModelTable.__fName, vModelField.__fName, FIELDINFO.Scale), "CL=DataCellRed"); // Масштаб DB
                        vReport.__mCell(vModelField.__fIsNull.ToString().ToLower(), "CL=DataCellRed"); // Null
                        object vIsNullDB = __mTableColumnInfo(vModelTable.__fName, vModelField.__fName, FIELDINFO.Null); // Null DB
                        if (vIsNullDB == null)
                            vReport.__mCell("", "CL=DataCellRed");
                        else
                            vReport.__mCell(vIsNullDB.ToString().ToLower() == "false" ? "no" : "yes", "CL=DataCellRed");

                        vReport.__mCell(vModelField.__fDescription, "CL=DataCellRed");
                        vReport.__mCell(__mTableColumnInfo(vModelTable.__fName, vModelField.__fName, FIELDINFO.Description), "CL=DataCellRed");
                        vReport.__mCell(datApplication.__oTunes.__mTranslate("Добавить"), "CL=HeaderCell");
                    }
                    else
                    {
                        bool vChanged = true; // Необходимость исправления
                        vReport.__mRow();
                        vReport.__mCell(vModelField.__fName, "CL=DataCell"); // Название
                        vReport.__mCell(vModelField.__fDataType.ToString().ToLower(), "CL=DataCell"); // Тип
                        vReport.__mCell(__mTableColumnInfo(vModelTable.__fName, vModelField.__fName, FIELDINFO.Type), "CL=DataCell"); // Тип DB
                        vReport.__mCell(vModelField.__fSize, "CL=DataCell"); //Точность
                        vReport.__mCell(vModelField.__fSizeDecimal, "CL=DataCell"); // Масштаб
                        vReport.__mCell(__mTableColumnInfo(vModelTable.__fName, vModelField.__fName, FIELDINFO.Precision), "CL=DataCell"); // Точность DB
                        vReport.__mCell(__mTableColumnInfo(vModelTable.__fName, vModelField.__fName, FIELDINFO.Scale), "CL=DataCell"); // Масштаб DB
                        //vReport._mCell(_mTableFieldInfo(vModelTable._fName, vModelField._fName, FIELDINFO.Null), "CL=DataCellRed"); 
                        string vIsNull = vModelField.__fIsNull.ToString().ToLower(); // Null DB
                        if (vIsNull == "false")
                            vReport.__mCell("no", "CL=DataCell"); // Null 
                        else
                            vReport.__mCell("yes", "CL=DataCell"); // Null 
                        vReport.__mCell(__mTableColumnInfo(vModelTable.__fName, vModelField.__fName, FIELDINFO.Null).ToString().ToLower(), "CL=DataCell"); // 
                        vReport.__mCell(__mTableColumnInfo(vModelTable.__fName, vModelField.__fName, FIELDINFO.Description), "CL=DataCell"); // Описание
                        vReport.__mCell(vModelField.__fDescription, "CL=DataCell"); // Описание DB

                        if (vModelField.__fDataType.ToString().ToLower() != __mTableColumnInfo(vModelTable.__fName, vModelField.__fName, FIELDINFO.Type).ToString().ToLower()) /// Проверка типа данных
                            vChanged = vChanged & false;
                        if (vModelField.__fSize != Convert.ToInt32(__mTableColumnInfo(vModelTable.__fName, vModelField.__fName, FIELDINFO.Precision))) /// Проверка точности
                            vChanged = vChanged & false;
                        if (vModelField.__fSizeDecimal != Convert.ToInt32(__mTableColumnInfo(vModelTable.__fName, vModelField.__fName, FIELDINFO.Scale))) /// Проверка масштаба
                            vChanged = vChanged & false;
                        if (vModelField.__fIsNull != Convert.ToBoolean(__mTableColumnInfo(vModelTable.__fName, vModelField.__fName, FIELDINFO.Null) == null ? "True" : "False")) /// Проверка null
                            vChanged = vChanged & false;

                        if (vChanged == true)
                            vReport.__mCell("", "CL=HeaderCell");
                        else
                            vReport.__mCell(datApplication.__oTunes.__mTranslate("Исправить"), "CL=HeaderCell");

                        vFieldSInModel.Add(vModelField.__fName);
                    }
                }
                /// Вывод полей отсутствующих в модели
                //vReport._mRow();
                //vReport._mCell(datApplication._oTunes._mTranslate("Удаляемые поля"), "CL=DataCellRed");
                DataTable vDataTableFields = __mTableColumnS(vModelTable.__fName);
                foreach (DataRow vDataRowField in vDataTableFields.Rows)
                {
                    if (appTypeString.__mWordInArrayList(vDataRowField["dsiFld"].ToString(), vFieldSInModel) == false)
                    {
                        vReport.__mRow();
                        vReport.__mCell(vDataRowField["dsiFld"].ToString(), "CL=DataCellRed"); // Название
                        vReport.__mCell("", "CL=DataCellRed"); // Тип 
                        vReport.__mCell(__mTableColumnInfo(vModelTable.__fName, vDataRowField["dsiFld"].ToString(), FIELDINFO.Type), "CL=DataCellRed"); // Тип DB
                        vReport.__mCell("", "CL=DataCellRed"); // Точность
                        vReport.__mCell("", "CL=DataCellRed"); // Масштаб
                        vReport.__mCell(__mTableColumnInfo(vModelTable.__fName, vDataRowField["dsiFld"].ToString(), FIELDINFO.Precision), "CL=DataCellRed"); // Точность DB
                        vReport.__mCell(__mTableColumnInfo(vModelTable.__fName, vDataRowField["dsiFld"].ToString(), FIELDINFO.Scale), "CL=DataCellRed"); // Масштаб DB
                        vReport.__mCell("", "CL=DataCellRed"); // Null
                        object vIsNullDB = __mTableColumnInfo(vModelTable.__fName, vDataRowField["dsiFld"].ToString(), FIELDINFO.Null); // Null DB
                        if (vIsNullDB == null)
                            vReport.__mCell("", "CL=DataCellRed");
                        else
                            vReport.__mCell(vIsNullDB.ToString().ToLower() == "false" ? "no" : "yes", "CL=DataCellRed");
                        vReport.__mCell("", "CL=DataCellRed"); // Описание
                        vReport.__mCell(__mTableColumnInfo(vModelTable.__fName, vDataRowField["dsiFld"].ToString(), FIELDINFO.Description), "CL=DataCellRed"); // Описание DB
                        vReport.__mCell(datApplication.__oTunes.__mTranslate("Удалить"), "CL=HeaderCell");
                    }
                }
            }

            vReport.__fAtOnceExcel = true;
            vReport.__fFileExcelName = "StructureModel";
            vReport.__mFile();

            return vReport.__fFileExcelName;
        }
        /// <summary>
        /// Получение типа данных поля для текущего типа источника данных
        /// </summary>
        /// <param name="pModelField">Значение перечисления типа данных полей</param>
        /// <returns>Название типа данных</returns>
        public object __mTableFieldInfo(datUnitModelField pModelField)
        {
            return null;
        }
        /// <summary>
        /// Печать модели структуры базы данных
        /// </summary>
        /// <returns>Путь и имя файла отчета</returns>
        public string __mModelStructurePrint()
        {
            __mModelBuild();
            rhtReport vReport = new rhtReport();
            vReport.__mCreate();
            vReport.__fTitle = datApplication.__oTunes.__mTranslate("Модель базы данных '{0}'", __fDatabaseName);
            vReport.__fColumnsCountInReport = 7;

            /// Отображение заголовка
            vReport.__mRow();
            vReport.__mCell(vReport.__fTitle, "CL=Caption", "SC=" + vReport.__fColumnsCountInReport.ToString());
            vReport.__mRowEmpty();
            vReport.__mTime("CL=TimeUser");
            vReport.__mUser(__fUserAlias, "CL=TimeUser");
            vReport.__mRowEmpty();

            /// Отображение данных
            /// Перебор таблиц
            foreach (datUnitModelTable vModelTable in __fModelTableS)
            {
                vReport.__mRowEmpty();
                vReport.__mRow();
                vReport.__mCell(vModelTable.__fName + " " + vModelTable.__fDescription, "SC=Max");
                /// Построение заголовка таблицы
                vReport.__mRow();
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Название"), "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Тип"), "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Точность"), "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Масштаб"), "CL=HeaderCell");
                vReport.__mCell("Null", "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Описание"), "CL=HeaderCell");
                vReport.__mCell(datApplication.__oTunes.__mTranslate("Значение"), "CL=HeaderCell");

                /// Перебор полей
                foreach (datUnitModelField vModelField in vModelTable.__fFieldS)
                {
                    vReport.__mRow();
                    vReport.__mCell(vModelField.__fName, "CL=DataCell");
                    vReport.__mCell(vModelField.__fDataType.ToString().ToLower(), "CL=DataCell");
                    vReport.__mCell(vModelField.__fSize, "CL=DataCell");
                    vReport.__mCell(vModelField.__fSizeDecimal, "CL=DataCell");
                    vReport.__mCell(vModelField.__fIsNull, "CL=DataCell");
                    vReport.__mCell(vModelField.__fDescription, "CL=DataCell");
                    vReport.__mCell(vModelField.__fDefaultValue, "CL=DataCell");
                }
            }

            vReport.__fAtOnceExcel = true;
            vReport.__fFileExcelName = "StructureModel";
            vReport.__mFile();

            return vReport.__fFileExcelName;
        }
        /// <summary>
        /// Подключение таблицы к списку источника данных
        /// </summary>
        /// <param name="pModelTable">Объект модели таблицы</param>
        public void _mModelTableAdd(datUnitModelTable pModelTable)
        {
            bool vTableWasAdded = false; // Признак того, что таблица уже в списке
            foreach (datUnitModelTable vTable in __fModelTableS)
            {
                if (vTable.__fName == pModelTable.__fName)
                {
                    vTable.__fDataSourceAlias = __fAlias;
                    vTableWasAdded = true;
                }
            }
            if (vTableWasAdded == false)
                __fModelTableS.Add(pModelTable);

            return;
        }
        /// <summary>
        /// Подключение таблицы к списку источника данных
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns></returns>
        public void _mModelTableRemove(string pTableName)
        {
            foreach (datUnitModelTable vTable in __fModelTableS)
            {
                if (vTable.__fName == pTableName)
                    __fModelTableS.Remove(vTable);
            }

            return;
        }
        /// <summary>
        /// Получение типа данных поля для текущего типа источника данных
        /// </summary>
        /// <param name="pType">Значение перечисления типа данных полей</param>
        /// <returns></returns>
        public virtual string __mModelTableFieldType(datUnitModelField pModelField)
        {
            return "";
        }

        #endregion Модель

        #region Подключение

        /// <summary>
        /// Построение строки подключения к источнику данных
        /// </summary>
        /// <param name="pLogin">Использование логина с паролем</param>
        /// <returns>[true] - строка построена, иначе - [false]</returns>
        protected virtual bool __mConnectionLineBuild(bool pLogin)
        {
            return false;
        }
        /// <summary>
        /// Разрыв соединения с источником данных
        /// </summary>
        protected virtual bool __mConnectionOff()
        {
            return false;
        }
        /// <summary>
        /// Установка соединения с источником данных
        /// </summary>
        /// <returns>[true] - соединение установлено, иначе - [false]</returns>
        protected virtual bool __mConnectionOn()
        {
            return false;
        }

        /// <summary>
        /// Чтение информации из ключевого файла
        /// </summary>
        /// <returns></returns>
        protected virtual bool __mConnectionInfoRead()
        {
            return false;
        }
        /// <summary>
        /// Запись информации в ключевой файл
        /// </summary>
        /// <returns></returns>
        protected virtual bool __mConnectionInfoWrite(string pServer, string pDatabase, string pLogin, string pPassword)
        {
            return false;
        }

        #endregion Подключение

        #region Пользователи

        /// <summary>
        /// Получение права пользователя 
        /// </summary>
        /// <param name="pRight">Право</param>
        /// <param name="pClueUser">Пользователь</param>
        /// <returns>[true] - доступ разрешен, иначе - [false]</returns>
        public virtual bool __mUserAccess(int pRight, int pClueUser)
        {
            return false;
        }

        #endregion Пользователи

        #region Программирование

        /// ВЫНЕСТИ В ПОЛЯ ПЕРЕМЕННЫЕ ОПРЕДЕЛЯЮЩИЕ РАБОТУ С ИМЕНАМИ ТАБЛИЦ !!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// + Добавить в отчетах метод обработки выбора кнопки 'Выполнить'
        /// + Добавить создание класса Period
        /// + Не работет .bak в отчетах
        /// Ф формы фильтра должны добавляться классы период

        /// <summary>
        /// Создание файлов классов контролов приложения
        /// </summary>
        /// <param name="pNameSpace">Название пространства имен</param>
        /// <param name="pPrefix">Префикс приложения</param>
        /// <param name="pDirectoryPath">Папка для размещения результата</param>
        public void __mProgramingInputControls(string pNameSpace, string pPrefix, string pDirectoryPath)
        {
            appFileIni vFileIni = new appFileIni(); // Объект для работы с инициализационными файлами
            vFileIni.__fFilePath = Path.Combine(datApplication.__oPathes.__fDirectoryStart, "project.bld");
            appFileText vFiletext = new appFileText(); // Объект для работы текстовыми файлами
            ArrayList vTableS = __mTablesList();
            /// Перебор таблиц источника данных
            foreach (string vTableName in vTableS)
            {
                foreach (DataRow vDataRowField in __mTableColumnS(vTableName.Trim()).Rows)
                {
                    string vPeriodWord = "";
                    //string vFieldName = vDataRowField["dsiFld"].ToString().Trim(); // Название поля

                    string vFieldName = vDataRowField["dsiFld"].ToString().Trim(); // Название поля
                    string vFieldDescription = vDataRowField["Dcr"].ToString().Trim().Contains(":") == false ? vDataRowField["Dcr"].ToString().Trim() : vDataRowField["Dcr"].ToString().Trim().Substring(vDataRowField["Dcr"].ToString().IndexOf(':') + 1).Trim(); ; // Описание поля
                    string vFieldType = vDataRowField["Typ"].ToString().Trim(); // Тип поля
                    string vFieldPrecision = vDataRowField["Pre"].ToString().Trim(); // Размер поля
                    string vFieldScale = vDataRowField["Sca"].ToString().Trim(); // Размер поля

                    vPeriodWord = "";

                    /// Пропускаемые поля
                    string vFieldsSkip = "CHG, CLU, GID"; // Список пропускаемых полей
                    if (appTypeString.__mWordInList(vFieldName, vFieldsSkip, ',') >= 0)
                        continue;

                    Directory.CreateDirectory(Path.Combine(pDirectoryPath, vTableName));
                    string vFileName = Path.Combine(pDirectoryPath, vTableName, pPrefix + "Input" + vTableName + vFieldName + ".cs"); // Имя создаваемого файла 
                    /// Если файл существует, то создается его копия
                    if (File.Exists(vFileName) == true)
                        vFileName = Path.Combine(pDirectoryPath, vTableName, pPrefix + "Input" + vTableName + vFieldName + ".bak");

                    string vClassParentName = ""; // Имя родительского класса
                    bool vPeriod = false;
                    string vFieldCodeName = ""; // Название поля кода
                    bool vFileNotCreate = false; // Метка о запрете создания файл, по причине не завершенности структуры базы данных, или по другой причине
                    string vEssenceName = ""; // Название сущности
                    string vFile = vFileIni.__mValueReadWrite("", "INPUTFORMSEARCH", "FormSearch"); // Список таблиц для которых компоненты должны быть построены на основе davInputFormSearch

                    #region /// Определение типов родительских классов

                    if (vFieldName.StartsWith("cod"))
                    {
                        vClassParentName = "davInputNumberInt";
                        vFieldCodeName = vFieldName;
                    }
                    if (vFieldName.StartsWith("cgz"))
                    {
                        vClassParentName = "davInputNumberInt";
                        vFieldCodeName = vFieldName;
                    }
                    if (vFieldName.StartsWith("dsi"))
                    {
                        vClassParentName = "davInputChar";
                    }
                    if (vFieldName.StartsWith("dtm") & vPeriod == false) // Создание класса davInputDateTime
                    {
                        vClassParentName = "davInputDateTime";
                        vPeriod = true;
                    }
                Period: // Создание класса davInputDateTimePeriod
                    if (vFieldName.StartsWith("dtm") & vPeriod == false)
                    {
                        vClassParentName = "davInputDateTimePeriod";
                        vFileName = Path.Combine(Path.GetDirectoryName(vFileName), Path.GetFileNameWithoutExtension(vFileName) + "Period.cs");
                        if (File.Exists(vFileName) == true)
                            vFileName = Path.Combine(Path.GetDirectoryName(vFileName), Path.GetFileNameWithoutExtension(vFileName) + "Period.bak");
                        vPeriodWord = "Period";
                    }
                    if (vFieldName.StartsWith("lnk"))
                    {
                        if (appTypeString.__mWordInList(vFieldName.Substring(3), vFile, ',') == -1)
                            vClassParentName = "davInputComboList";
                        else
                            vClassParentName = "davInputFormSearch";
                    }
                    if (vFieldName.StartsWith("LCK"))
                    {
                        vClassParentName = "davInputComboBool";
                    }
                    if (vFieldName.StartsWith("mrk"))
                    {
                        vClassParentName = "davInputComboBool";
                    }
                    if (vFieldName.StartsWith("opt"))
                    {
                        vClassParentName = "davInputRadio";
                    }


                    if (vClassParentName.Length == 0 & vFieldType == "bit")
                    {
                        vClassParentName = "davInputComboBool";
                    }
                    if (vClassParentName.Length == 0 & vFieldType == "char")
                    {
                        vClassParentName = "davInputChar";
                    }
                    if (vClassParentName.Length == 0 & vFieldType == "varchar")
                    {
                        vClassParentName = "davInputChar";
                    }
                    if (vClassParentName.Length == 0 & vFieldType == "int")
                    {
                        vClassParentName = "davInputNumberInt";
                    }
                    if (vClassParentName.Length == 0 & vFieldType == "numeric")
                    {
                        vClassParentName = "davInputNumberDecimal";
                    }

                    #endregion Определение типов родительских классов

                    string vFileBody = "using nlApplication;" + CRLF; // Тело файла 
                    vFileBody += "using nlElements;" + CRLF;
                    vFileBody += "using System;" + CRLF;
                    vFileBody += "using nlDataCollection;" + CRLF;
                    //                    vFileBody += "using System;" + CRLF;
                    //                    vFileBody += "using System.Data;" + CRLF;
                    vFileBody += CRLF + "namespace " + pNameSpace + CRLF;
                    vFileBody += "{" + CRLF; // Начало пространства имен
                    vFileBody += "\t/// <summary>" + CRLF;
                    vFileBody += "\t/// Класс '" + pPrefix + "Input" + vTableName + vFieldName + "'" + CRLF;
                    vFileBody += "\t/// </summary>" + CRLF;
                    vFileBody += "\t/// <remarks>Поле ввода - " + vFieldDescription + "</remarks>" + CRLF;
                    vFileBody += "\tpublic class " + pPrefix + "Input" + vTableName + vFieldName + vPeriodWord + " : " + vClassParentName + CRLF;
                    vFileBody += "\t{" + CRLF;
                    vFileBody += "\t\t#region = МЕТОДЫ" + CRLF + CRLF;
                    vFileBody += "\t\t#region - Поведение" + CRLF + CRLF;
                    vFileBody += "\t\t#region Объект" + CRLF + CRLF;

                    vFileBody += "\t\t/// <summary>" + CRLF;
                    vFileBody += "\t\t/// Сборка объекта" + CRLF;
                    vFileBody += "\t\t/// </summary>" + CRLF;
                    vFileBody += "\t\tprotected override void _mObjectAssembly()" + CRLF;
                    vFileBody += "\t\t{" + CRLF;
                    vFileBody += "\t\t\tSuspendLayout();" + CRLF + CRLF;
                    vFileBody += "\t\t\tbase._mObjectAssembly();" + CRLF + CRLF;
                    vFileBody += "\t\t\t#region /// Настройка компонента" + CRLF + CRLF;

                    vFileBody += "\t\t\t__fCaption_ = \"" + vFieldDescription + "\";" + CRLF;
                    vFileBody += "\t\t\t__fFieldName = \"" + vFieldName + "\";" + CRLF;

                    /// Особые параметры для каждого типа контрола
                    switch (vClassParentName)
                    {
                        case "davInputChar":
                            vFileBody += "\t\t\t__fSymbolsCount_ = " + vFieldPrecision + ";" + CRLF + CRLF;
                            break;
                        case "davInputComboList":
                            vEssenceName = __mEssenseNameSeaching(vTableS, vFieldName); // Название сущности
                            if (vEssenceName.Length == 7)
                                vFileNotCreate = true;
                            vFileBody += "\t\t\t__oEssence_ = new " + pPrefix + vEssenceName + "();" + CRLF + CRLF;
                            break;
                        case "davInputFormSearch":
                            vEssenceName = __mEssenseNameSeaching(vTableS, vFieldName); // Название сущности
                            if (vEssenceName.Length == 7)
                                vFileNotCreate = true;
                            vFileBody += "\t\t\t__oEssence_ = new " + pPrefix + vEssenceName + "();" + CRLF;
                            vFileBody += "\t\t\t__oFormSelect = typeof(" + pPrefix + "FormGrid" + vFieldName + ");" + CRLF;
                            vFileBody += "\t\t\t__fFieldsCharList.Add(dsi" + vFieldName + ");" + CRLF;
                            vFileBody += "\t\t\t__fFieldCode = cod" + vFieldName + ";" + CRLF;
                            vFileBody += "\t\t\t__fFormSearchCaption_ = davApplication.__oTunes.__mTranslate(\"Поиск клиента\");" + CRLF + CRLF;
                            vFileBody += "\t\t\t#region Сетка / Определение колонок" + CRLF + CRLF;
                            vFileBody += "\t\t\t__mColumnAdd(davApplication.__oTunes.__mTranslate(\"Ключ\")" + CRLF;
                            vFileBody += "\t\t\t\t\t, \"clu\"" + vTableName + CRLF;
                            vFileBody += "\t\t\t\t\t, true" + CRLF;
                            vFileBody += "\t\t\t\t\t, false" + CRLF;
                            vFileBody += "\t\t\t\t\t, \"DataGridViewTextBoxColumn\");" + CRLF;
                            vFileBody += "\t\t\t__mColumnAdd(davApplication.__oTunes.__mTranslate(\"Код\")" + CRLF;
                            vFileBody += "\t\t\t\t\t, \"cod\"" + vTableName + CRLF;
                            vFileBody += "\t\t\t\t\t, true" + CRLF;
                            vFileBody += "\t\t\t\t\t, true" + CRLF;
                            vFileBody += "\t\t\t\t\t, \"DataGridViewTextBoxColumn\");" + CRLF;
                            vFileBody += "\t\t\t__mColumnAdd(davApplication.__oTunes.__mTranslate(\"Штрих -код\")" + CRLF;
                            vFileBody += "\t\t\t\t\t, \"dsi\"" + vTableName + CRLF;
                            vFileBody += "\t\t\t\t\t, true" + CRLF;
                            vFileBody += "\t\t\t\t\t, true" + CRLF;
                            vFileBody += "\t\t\t\t\t, \"DataGridViewTextBoxColumn\");" + CRLF + CRLF;
                            vFileBody += "\t\t\t#endregion Сетка / Определение колонок" + CRLF;


                            break;
                        case "davInputNumberDecimal":
                            vFileBody += "\t\t\t__fPrecision_ = " + vFieldPrecision + ";" + CRLF;
                            vFileBody += "\t\t\t__fScaleOnFocus_ = " + vFieldScale + ";" + CRLF;
                            vFileBody += "\t\t\t__fScaleOnLostFocus_ = " + vFieldScale + ";" + CRLF + CRLF;
                            break;
                        case "davInputNumberInt":
                            vFileBody += "\t\t__fValueMaximum_ = " + vFileIni.__mValueReadWrite("7", "PRECISIONs", vFieldName) + ";" + CRLF + CRLF;
                            break;
                    }

                    vFileBody += CRLF + "\t\t\t#endregion Настройка компонента" + CRLF + CRLF;
                    vFileBody += "\t\t\tResumeLayout(false);" + CRLF + CRLF;
                    //vFileBody += "\t\t\tType vType = this.GetType();" + CRLF;
                    //vFileBody += "\t\t\tName = vType.Name;" + CRLF;
                    //vFileBody += "\t\t\t_fClassNameFull = vType.FullName + \".\";" + CRLF + CRLF;
                    vFileBody += "\t\t\treturn;" + CRLF;
                    vFileBody += "\t\t}" + CRLF + CRLF;
                    /// Код для контролов вызывающих данные из справочников
                    /// vFieldName.StartsWith("cod") & (
                    if (vFieldCodeName.Length > 0 & (vClassParentName == "davInputComboList" | vClassParentName == "davInputFormSearch"))
                    {
                        vFileBody += "\t\t/// <summary>" + CRLF;
                        vFileBody += "\t\t/// Выполняется при первом отображении объекта" + CRLF;
                        vFileBody += "\t\t/// </summary>" + CRLF;
                        vFileBody += "\t\tprotected override void _mObjectPresetation()" + CRLF;
                        vFileBody += "\t\t{" + CRLF;
                        vFileBody += "\t\t\tbase._mObjectPresetation();" + CRLF + CRLF;
                        vFileBody += "\t\t\tstring vWhere = \"LCK = 0\";" + CRLF;
                        vFileBody += "\t\t\tstring vOrder = \"cod" + vFieldCodeName.Substring(3) + "\";" + CRLF;
                        vFileBody += "\t\t\t__mItemsEssenceLoad(vWhere, vOrder);" + CRLF;
                        vFileBody += "\t\t\t__mDataRefresh();" + CRLF;
                        vFileBody += "\t\t}" + CRLF + CRLF;
                    }
                    vFileBody += "\t\t#endregion Объект" + CRLF + CRLF;
                    vFileBody += "\t\t#endregion Поведение" + CRLF + CRLF;
                    vFileBody += "\t\t#endregion МЕТОДЫ" + CRLF;
                    vFileBody += "\t}" + CRLF;
                    vFileBody += "}" + CRLF; // Конец пространства имен
                    /// Вывод в файл
                    if (vFileNotCreate == false)
                    {
                        if (File.Exists(vFileName) == true) // Удаление одноименного файла, если он существует
                            File.Delete(vFileName);
                        vFiletext.__mWriteToEnd(vFileName, vFileBody); // Создание нового файла
                    }
                    /// Создание файла класса периода дат
                    if (vFieldName.StartsWith("dtm") & vPeriod == true)
                    {
                        vPeriod = false;
                        goto Period;
                    }
                }
            }

            datApplication.__oMessages.__mShow(MESSAGESTYPES.Warning, "Поля ввода сформированы!");

            return;
        }
        /// <summary>
        /// Создание файлов классов моделей таблиц
        /// </summary>
        /// <param name="pNameSpace">Название пространства имен</param>
        /// <param name="pPrefix">Префикс приложения</param>
        /// <param name="pDirectoryPath">Папка для размещения результата</param>
        public void __mProgrammingModelTable(string pNameSpace, string pPrefix, string pDirectoryPath)
        {
            string vCommandForModelBuild = ""; // Команда для построения модели данных
            appFileText vFiletext = new appFileText(); // Объект для работы текстовыми файлами
            /// Перебор таблиц источника данных
            foreach (string vTableName in __mTablesList())
            {
                #region Заполнение команды для построения модели данных

                vCommandForModelBuild += "\t\t\t" + pPrefix + "ModelTable" + vTableName + " vModel" + vTableName + "= new " + pPrefix + "ModelTable" + vTableName + "();" + CRLF;
                vCommandForModelBuild += "\t\t\tvModel" + vTableName + ".__mModelTableBuilding();" + CRLF;
                vCommandForModelBuild += "\t\t\t__fModelTableS.Add(vModel" + vTableName + ");" + CRLF + CRLF;

                #endregion

                string vFileName = Path.Combine(pDirectoryPath, pPrefix + "ModelTable" + vTableName + ".cs"); // Имя создаваемого файла класса модели
                /// Если файл существует, то создается его копия
                if (File.Exists(vFileName) == true)
                    vFileName = Path.Combine(pDirectoryPath, pPrefix + "ModelTable" + vTableName + ".bak");
                string vFileBody = "using nlDataCollection;" + CRLF;
                vFileBody += "using System;" + CRLF + CRLF;
                vFileBody += "namespace " + pNameSpace + CRLF;
                vFileBody += "{" + CRLF;
                { // Пространство имен
                    vFileBody += "\t/// <summary>" + CRLF;
                    vFileBody += "\t/// Класс '" + pPrefix + "ModelTable" + vTableName + "'" + CRLF;
                    vFileBody += "\t/// </summary>" + CRLF;
                    vFileBody += "\t/// <remarks> Сущность - " + __mTableDescription(vTableName) + "</remarks>" + CRLF;
                    vFileBody += "\tpublic class " + pPrefix + "ModelTable" + vTableName + " : datUnitModelTable" + CRLF;
                    vFileBody += "\t{" + CRLF;
                    vFileBody += "\t\t#region = МЕТОДЫ" + CRLF + CRLF;
                    vFileBody += "\t\t#region - Поведение" + CRLF + CRLF;
                    { // = МЕТОДЫ / - Поведение / Объекты
                        vFileBody += "\t\t/// <summary>" + CRLF;
                        vFileBody += "\t\t/// Сборка объекта" + CRLF;
                        vFileBody += "\t\t/// </summary>" + CRLF;
                        vFileBody += "\t\tprotected override void _mObjectAssembly()" + CRLF;
                        vFileBody += "\t\t{" + CRLF;
                        { // _mLoad()
                            vFileBody += "\t\t\tbase._mObjectAssembly();" + CRLF + CRLF;
                            //vFileBody += "\t\t\tType vType = this.GetType();" + CRLF;
                            //vFileBody += "\t\t\t_fClassNameFull = vType.FullName + \".\";" + CRLF;
                            vFileBody += "\t\t\t__fDescription = " + pPrefix + "Application.__oTunes.__mTranslate(\"" + __mTableDescription(vTableName) + "\");" + CRLF;
                            vFileBody += "\t\t\t__oEssence = new " + pPrefix + "Essence" + vTableName + "(); // Сущность таблицы" + CRLF;
                            vFileBody += "\t\t\t__fName = \"" + vTableName + "\";" + CRLF + CRLF;
                            vFileBody += "\t\t\treturn;" + CRLF;
                        } // _mLoad 
                        vFileBody += "\t\t}" + CRLF + CRLF;
                    } // = МЕТОДЫ / - Поведение / Объекты 
                    vFileBody += "\t\t#endregion Поведение" + CRLF + CRLF;
                    vFileBody += "\t\t#region - Процедуры" + CRLF + CRLF;
                    /// __mModelBuild()
                    {
                        vFileBody += "\t\t/// <summary>" + CRLF;
                        vFileBody += "\t\t/// Построение модели таблицы" + CRLF;
                        vFileBody += "\t\t/// </summary>" + CRLF;
                        vFileBody += "\t\t/// <returns>[true] - модель создана, иначе - [false]</returns>" + CRLF;
                        vFileBody += "\t\tpublic override void __mModelTableBuilding()" + CRLF;
                        vFileBody += "\t\t{" + CRLF;
                        vFileBody += "\t\t\tdatUnitModelField vField = new datUnitModelField(); // Модель поля таблицы" + CRLF;
                        foreach (DataRow vDataRowField in __mTableColumnS(vTableName.Trim()).Rows)
                        {
                            vFileBody += CRLF + "\t\t\t/// " + vDataRowField["dsiFld"].ToString() + " - " + vDataRowField["Dcr"].ToString() + CRLF;
                            vFileBody += "\t\t\t{" + CRLF;

                            vFileBody += "\t\t\t\tvField = new datUnitModelField();" + CRLF; /// Создание объекта поля таблицы
                            if (vDataRowField["dsiFld"].ToString().Trim().StartsWith("CLU") == true) /// Проверка - является ли поле идентификатором
                            {
                                vFileBody += "\t\t\t\tvField.__fAutoIncrement = true;" + CRLF; /// Автоприращение
                                vFileBody += "\t\t\t\tvField.__fIsClue = true;" + CRLF; /// Primary key
                            }
                            else
                            {
                                vFileBody += "\t\t\t\tvField.__fAutoIncrement = false;" + CRLF;
                                vFileBody += "\t\t\t\tvField.__fIsClue = false;" + CRLF;
                            }
                            vFileBody += "\t\t\t\tvField.__fIsNull = false;" + CRLF;
                            vFileBody += "\t\t\t\tvField.__fDataType = COLUMNSTYPES." + appTypeString.__mWordPersonal(vDataRowField["Typ"].ToString()) + ";" + CRLF; /// Назначение типа данных
                            vFileBody += "\t\t\t\tvField.__fName = \"" + vDataRowField["dsiFld"].ToString() + "\";" + CRLF; // Название поля
                            vFileBody += "\t\t\t\tvField.__fDescription = \"" + vDataRowField["Dcr"].ToString() + "\";" + CRLF; // Описание поля

                            /// Опеределение размерности полей не возвращаемых сервером
                            if (vDataRowField["dsiFld"].ToString().StartsWith("mrk") | vDataRowField["dsiFld"].ToString().StartsWith("opt"))
                                vFileBody += "\t\t\t\tvField.__fSize = 1;" + CRLF; /// Размер поля
                            else
                                if (vDataRowField["dsiFld"].ToString().StartsWith("dtm"))
                                vFileBody += "\t\t\t\tvField.__fSize = 8;" + CRLF; /// Размер поля
                            else
                                    if (vDataRowField["dsiFld"].ToString() == "CHG")
                                vFileBody += "\t\t\t\tvField.__fSize = 8;" + CRLF; /// Размер поля
                            else
                                        if (vDataRowField["dsiFld"].ToString() == "GID")
                                vFileBody += "\t\t\t\tvField.__fSize = 16;" + CRLF; /// Размер поля
                            else
                                            if (vDataRowField["dsiFld"].ToString() == "LCK")
                                vFileBody += "\t\t\t\tvField.__fSize = 1;" + CRLF; /// Размер поля
                            else
                                vFileBody += "\t\t\t\tvField.__fSize = " + vDataRowField["Pre"].ToString() + ";" + CRLF; /// Размер поля

                            vFileBody += "\t\t\t\tvField.__fSizeDecimal = " + vDataRowField["Sca"].ToString() + ";" + CRLF; /// Размер дробной части поля

                            vFileBody += "\t\t\t\t__fFieldS.Add(vField);" + CRLF; /// Добавление пооля в таблицу
                            vFileBody += "\t\t\t}" + CRLF;
                        }
                        vFileBody += CRLF + "\t\t\treturn;" + CRLF;
                        vFileBody += "\t\t}" + CRLF + CRLF;
                    }
                    vFileBody += "\t\t#endregion Процедуры" + CRLF + CRLF;
                    vFileBody += "\t\t#endregion МЕТОДЫ" + CRLF;
                    vFileBody += "\t}" + CRLF;
                } // Пространство имен
                vFileBody += "}" + CRLF;
                if (File.Exists(vFileName) == true) /// Удаление одноименного файла, если он существует
                    File.Delete(vFileName);
                vFiletext.__mWriteToEnd(vFileName, vFileBody); /// Создание нового файла
            }
            datApplication.__oMessages.__mShow(MESSAGESTYPES.Warning, "Модели таблиц сформированы!");

            /// Сохранение команды для построения модели данных в файл 
            appFileText vFileText = new appFileText();
            if (File.Exists(Path.Combine(pDirectoryPath, "CommandModelBuild")) == true)
                File.Delete(Path.Combine(pDirectoryPath, "CommandModelBuild"));
            vFiletext.__mWriteToEnd(Path.Combine(pDirectoryPath, "CommandModelBuild"), vCommandForModelBuild);
            datApplication.__oMessages.__mShow(MESSAGESTYPES.Warning, "Команда для построения модели данных сохранена в файл\n" + Path.Combine(pDirectoryPath, "CommandModelBuild"));

            return;
        }
        /// <summary>
        /// Создание файлов классов сущностей данных
        /// </summary>
        /// <param name="pNameSpace">Название пространства имен</param>
        /// <param name="pPrefix">Префикс приложения</param>
        /// <param name="pDirectoryPath">Папка для размещения результата</param>
        public void __mProgrammingTableEssence(string pNameSpace, string pPrefix, string pDirectoryPath)
        {
            appFileText vFiletext = new appFileText(); /// Объект для работы текстовыми файлами
                                                       /// Перебор таблиц источника данных
            foreach (string vTableName in __mTablesList())
            {
                string vFileName = Path.Combine(pDirectoryPath, pPrefix + "Essences" + vTableName + ".cs"); // Имя создаваемого файла 
                /// Если файл существует, то создается его копия
                if (File.Exists(vFileName) == true)
                    vFileName = Path.Combine(pDirectoryPath, pPrefix + "Essences" + vTableName + ".bak");
                string vFileBody = "using nlApplication;" + CRLF; // Тело файла 
                string vFileBodyTriggerChecks = ""; // Часть файла. Триггер, проверка заполнения и целостности данных

                vFileBody += "using nlDataCollection;" + CRLF;
                vFileBody += "using System;" + CRLF;
                vFileBody += "using System.Data;" + CRLF;
                vFileBody += CRLF + "namespace " + pNameSpace + CRLF;
                vFileBody += "{" + CRLF;
                { // Пространство имен
                    vFileBody += "\t/// <summary>" + CRLF;
                    vFileBody += "\t/// Класс '" + pPrefix + "Essence" + vTableName + "'" + CRLF;
                    vFileBody += "\t/// </summary>" + CRLF;
                    vFileBody += "\t/// <remarks>Сущность - " + __mTableDescription(vTableName) + "</remarks>" + CRLF;
                    vFileBody += "\tpublic class " + pPrefix + "Essence" + vTableName + " : datUnitEssence" + CRLF;
                    vFileBody += "\t{" + CRLF;
                    { // Класс
                        vFileBody += "\t\t#region = ДИЗАЙНЕРЫ" + CRLF + CRLF;
                        vFileBody += "\t\t/// Конструктор" + CRLF;
                        vFileBody += "\t\t/// </summary>" + CRLF;
                        vFileBody += "\t\t/// <remarks>Источник данных береться используемый по умолчанию</remarks>" + CRLF;
                        vFileBody += "\t\t/// <remarks>Вид удаления данных - пометка</remarks>" + CRLF;
                        vFileBody += "\t\tpublic " + pPrefix + "Essence" + vTableName + "()" + CRLF;
                        vFileBody += "\t\t{" + CRLF;
                        vFileBody += "\t\t\t__fDataSourceAlias = datApplication.__oData.__mDataSourceGet().__fAlias;" + CRLF;
                        vFileBody += "\t\t\t__fDeleteType = DELETETYPES.Mark;" + CRLF + CRLF;

                        vFileBody += "\t\t\t_mObjectAssembly();" + CRLF;
                        vFileBody += "\t\t}" + CRLF;
                        vFileBody += "\t\t/// <summary>" + CRLF;
                        vFileBody += "\t\t/// Конструктор" + CRLF;
                        vFileBody += "\t\t/// </summary>" + CRLF;
                        vFileBody += "\t\t/// <param name=\"pDeleteType\">Порядок удаления записей из таблиц базы данных</param>" + CRLF;
                        vFileBody += "\t\t/// <remarks>Источник данных береться используемый по умолчанию</remarks>" + CRLF;
                        vFileBody += "\t\tpublic " + pPrefix + "Essence" + vTableName + "(DELETETYPES pDeleteType)" + CRLF;
                        vFileBody += "\t\t{" + CRLF;
                        vFileBody += "\t\t\t__fDataSourceAlias = datApplication.__oData.__mDataSourceGet().__fAlias;" + CRLF;
                        vFileBody += "\t\t\t__fDeleteType = pDeleteType;" + CRLF + CRLF; ;

                        vFileBody += "\t\t\t_mObjectAssembly();" + CRLF;
                        vFileBody += "\t\t}" + CRLF;
                        vFileBody += "\t\t/// <summary>" + CRLF;
                        vFileBody += "\t\t/// Конструктор" + CRLF;
                        vFileBody += "\t\t/// </summary>" + CRLF;
                        vFileBody += "\t\t/// <param name=\"pDataSourceAlias\">Псевдоним источника данных</param>" + CRLF;
                        vFileBody += "\t\t/// <param name=\"pDeleteType\">Порядок удаления записей из таблиц базы данных</param>" + CRLF;
                        vFileBody += "\t\t/// <summary>" + CRLF;
                        vFileBody += "\t\tpublic " + pPrefix + "Essence" + vTableName + "(string pDataSourceAlias, DELETETYPES pDeleteType) : base(pDataSourceAlias, pDeleteType)" + CRLF;
                        vFileBody += "\t\t{" + CRLF;
                        vFileBody += "\t\t}" + CRLF + CRLF;
                        vFileBody += "\t\t#endregion ДИЗАЙНЕРЫ" + CRLF;

                        vFileBody += CRLF + "\t\t#region = МЕТОДЫ" + CRLF;
                        vFileBody += CRLF + "\t\t#region - Поведение" + CRLF;
                        /// = МЕТОДЫ / - Поведение
                        {
                            vFileBody += CRLF + "\t\t///<summary>" + CRLF;
                            vFileBody += "\t\t/// Сборка объекта" + CRLF;
                            vFileBody += "\t\t///</summary>" + CRLF;
                            vFileBody += "\t\tprotected override void _mObjectAssembly()" + CRLF;
                            vFileBody += "\t\t{" + CRLF;
                            /// Создание метода _mObjectAssembly()
                            {
                                vFileBody += "\t\t\tbase._mObjectAssembly();" + CRLF;
                                //vFileBody += "\t\t\tType vType = this.GetType();" + CRLF;
                                //vFileBody += "\t\t\t_fClassNameFull = vType.FullName + \".\";" + CRLF;
                                vFileBody += "\t\t\t__fTableDescription = " + pPrefix + "Application.__oTunes.__mTranslate(\"" + __mTableDescription(vTableName) + "\");" + CRLF;
                                vFileBody += "\t\t\t__fTableName = \"" + vTableName + "\";" + CRLF;

                                vFileBody += "\t\t\t__fTablePrefix = \"" + appTypeString.__mSymbolsOnlyUpperCase(vTableName) + "\";" + CRLF;

                                if (appTypeString.__mWordInList(vTableName.ToUpper(), new string[] { "ARR", "EXN", "INE" }) > 0) /// Для локументов новый учетный код расчитывается приращением, для справочников поиском пропущенных кодов
                                    vFileBody += "\t\t\t__fCodeNewCalculateType = CODESNEWTYPES.Next;" + CRLF;
                                else
                                    vFileBody += "\t\t\t__fCodeNewCalculateType = CODESNEWTYPES.Skiped;" + CRLF;
                                vFileBody += "\t\t\t__fLockUsed = true;" + CRLF + CRLF;
                                vFileBody += "\t\t\treturn;" + CRLF;
                            }
                            vFileBody += "\t\t}" + CRLF;
                        } // = МЕТОДЫ / - Поведение
                        vFileBody += CRLF + "\t\t#endregion Поведение" + CRLF;
                        vFileBody += CRLF + "\t\t#region - Процедуры" + CRLF;
                        /// = МЕТОДЫ / - Процедуры
                        {
                            vFileBody += CRLF + "\t\t/// <summary>" + CRLF;
                            /// Создание метода 'Получение пустой записи для указанной таблицы'
                            vFileBody += "\t\t/// Получение пустой записи для указанной таблицы" + CRLF;
                            vFileBody += "\t\t/// </summary>" + CRLF;
                            vFileBody += "\t\t/// <param name=\"pDataTable\">{DataTable}</param>" + CRLF;
                            vFileBody += "\t\t/// <returns>{DataRow} заполненная значениями по умолчанию</returns>" + CRLF;
                            vFileBody += "\t\tpublic override DataRow __mRecordNew(DataTable pDataTable)" + CRLF;
                            vFileBody += "\t\t{" + CRLF;
                            /// _mRecordNew(DataTable)
                            {
                                vFileBody += "\t\t\tDateTime vDateTime = DateTime.Now; // Текущее время" + CRLF;
                                vFileBody += "\t\t\tDateTime vDateTimeEmpty = new DateTime(1900, 1, 1, 0, 0, 0);" + CRLF;
                                vFileBody += "\t\t\tDataRow vDataRow = pDataTable.NewRow(); // Объект строки" + CRLF + CRLF;

                                foreach (DataRow vDataRowField in __mTableColumnS(vTableName.Trim()).Rows)
                                {
                                    vFileBody += "\t\t\tvDataRow[\"" + vDataRowField["dsiFld"] + "\"] = ";
                                    switch (vDataRowField["Typ"].ToString())
                                    {
                                        case "bit":
                                            vFileBody += "0;" + CRLF;
                                            break;
                                        case "char":
                                            vFileBody += "\"\";" + CRLF;
                                            break;
                                        case "datetime":
                                            vFileBody += "vDateTime;" + CRLF;
                                            break;
                                        case "int":
                                            vFileBody += "0;" + CRLF;
                                            break;
                                        case "numeric":
                                            vFileBody += "0;" + CRLF;
                                            break;
                                        case "smallint":
                                            vFileBody += "0;" + CRLF;
                                            break;
                                        case "tinyint":
                                            vFileBody += "0;" + CRLF;
                                            break;
                                        case "uniqueidentifier":
                                            vFileBody += "Guid.NewGuid();" + CRLF;
                                            break;
                                        case "varchar":
                                            vFileBody += "\"\";" + CRLF;
                                            break;
                                    }
                                }
                                vFileBody += CRLF + "\t\t\treturn vDataRow;" + CRLF;
                                vFileBody += "\t\t}" + CRLF;
                            } // _mRecordNew(DataTable)
                        }  // = МЕТОДЫ / - Процедуры
                        vFileBody += CRLF + "\t\t#endregion Процедуры" + CRLF;
                        vFileBody += CRLF + "\t\t#region - Триггеры" + CRLF;
                        { /// = МЕТОДЫ / - Триггеры 
                            vFileBody += CRLF + "\t\t/// <summary>" + CRLF;
                            vFileBody += "\t\t/// Проверка заполнения полей" + CRLF;
                            vFileBody += "\t\t/// </summary>" + CRLF;
                            vFileBody += "\t\t/// <param name=\"pDataRow\"></param>" + CRLF;
                            vFileBody += "\t\t/// <param name=\"pTriggerType\"></param>" + CRLF;
                            vFileBody += "\t\t/// <returns>[true] - данные введены без ошибок, иначе - [false]</returns>" + CRLF;
                            /// Создание метода 'Проверка заполнения полей'
                            vFileBody += "\t\tpublic override bool __mCheckRecordFieldsFill(ref DataRow pDataRow, TRIGGERTYPEFORCHANGERECORD pTriggerType)" + CRLF;
                            vFileBody += "\t\t{" + CRLF;
                            {
                                vFileBody += "\t\t\tbool vReturn = true; // Возвращаемое значение" + CRLF;
                                vFileBody += "\t\t\t_fTriggerErrorsDescriptions.Clear(); // Сброс списка ошибок" + CRLF + CRLF;
                                /// Перебор полей в таблице и чтение значений в переменные
                                foreach (DataRow vDataRowField in __mTableColumnS(vTableName.Trim()).Rows)
                                {
                                    string vColumnName = vDataRowField["dsiFld"].ToString().Trim(); // Название колонки 
                                    string[] vSkipedColumns = { "GID" }; // Пропускаемые колонки
                                    string vColumnDesciption = vDataRowField["dcr"].ToString(); // Описание колонки
                                    string vColumnType = vDataRowField["Typ"].ToString().Trim(); // Тип данных колонки

                                    if (appTypeString.__mWordInList(vDataRowField["dsiFld"].ToString(), new string[] { "CHG", "GID", "LCK" }) > 0) /// Обработка списка пропускаемых полей
                                        continue;

                                    vFileBody += "\t\t\t";
                                    switch (vDataRowField["Typ"].ToString())
                                    { /// Формирование переменных со значениями полей
                                        case "bigint":
                                            vFileBody += "long v" + vDataRowField["dsiFld"].ToString() + " = Convert.ToInt64(pDataRow[\"" + vDataRowField["desFld"].ToString() + "\"]); // " + vDataRowField["dcr"].ToString() + CRLF;
                                            break;
                                        case "bit":
                                            vFileBody += "bool v" + vDataRowField["dsiFld"].ToString() + " = Convert.ToBoolean(pDataRow[\"" + vDataRowField["desFld"].ToString() + "\"]); // " + vDataRowField["dcr"].ToString() + CRLF;
                                            break;
                                        case "char":
                                            vFileBody += "string v" + vDataRowField["dsiFld"].ToString() + " = Convert.ToString(pDataRow[\"" + vDataRowField["desFld"].ToString() + "\"]); // " + vDataRowField["dcr"].ToString() + CRLF;
                                            break;
                                        case "datetime":
                                            vFileBody += "DateTime v" + vDataRowField["dsiFld"].ToString() + " = Convert.ToDateTime(pDataRow[\"" + vDataRowField["desFld"].ToString() + "\"]); // " + vDataRowField["dcr"].ToString() + CRLF;
                                            break;
                                        case "numeric":
                                            vFileBody += "decimal v" + vDataRowField["dsiFld"].ToString() + " = Convert.ToDecimal(pDataRow[\"" + vDataRowField["desFld"].ToString() + "\"]); // " + vDataRowField["dcr"].ToString() + CRLF;
                                            break;
                                        case "tinyint":
                                            vFileBody += "int v" + vDataRowField["dsiFld"].ToString() + " = Convert.ToInt32(pDataRow[\"" + vDataRowField["desFld"].ToString() + "\"]); // " + vDataRowField["dcr"].ToString() + CRLF;
                                            break;
                                        case "varchar":
                                            vFileBody += "string v" + vDataRowField["dsiFld"].ToString() + " = Convert.ToString(pDataRow[\"" + vDataRowField["desFld"].ToString() + "\"]); // " + vDataRowField["dcr"].ToString() + CRLF;
                                            break;
                                        default:
                                            vFileBody += vDataRowField["Typ"].ToString() + " v" + vDataRowField["dsiFld"].ToString() + "= Convert.ToInt32(pDataRow[\"" + vDataRowField["desFld"].ToString() + "\"]); // " + vDataRowField["dcr"].ToString() + CRLF;
                                            break;
                                    }  // Формирование переменных со значениями полей
                                    switch (vColumnName.Substring(0, 3))
                                    {  /// Обработка типа -- и типа данных полей
                                        case "CLU": // Идентификатор
                                            break;
                                        case "cod": // Учетный код
                                            vFileBodyTriggerChecks += CRLF + "\t\t\t#region '" + vColumnName + "' - " + vColumnDesciption + CRLF + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t/// Если исправляется запись с кодом [0]" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\tif (vCLU == 0 & v" + vColumnName + " == 0 & pTriggerType == TRIGGERTYPEFORCHANGERECORD.Update)" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\t_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate(\"Запись с кодом '0' не корректируется\"))";
                                            vFileBodyTriggerChecks += "\t\t\telse";
                                            vFileBodyTriggerChecks += "\t\t\t{";


                                            vFileBodyTriggerChecks += "\t\t\t\t/// Если код не указан, выполняется его расчет" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\tif (v" + vColumnName + " <= 0)" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\t{" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\t\tpDataRow[\"" + vColumnName + "\" ] = datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mCodeNew(__fTableName, vCLU, __fCodeNewCalculateType, 1);" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\t}" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\t/// Если код указан, выполняется его проверка на использование с другим идентификатором, если такая запись обнаружена формируется сообщение сообщение об ошибке: 'Код уже используется'" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\telse" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\t{" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\t\tif (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mTableRowsCountWhere(__fTableName, \"" + vColumnName + " = \" + v" + vColumnName + ".ToString() + \" and CLU != \" + vCLU.ToString()+ \" and CLU != 0\") > 0)" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\t\t{" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\t\t\t_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate(\"Учетный код уже используется\"));" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\t\t}" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\t}" + CRLF;

                                            vFileBodyTriggerChecks += "\t\t\t}";

                                            vFileBodyTriggerChecks += CRLF + "\t\t\t#endregion 'cod" + vColumnName + "' - " + vColumnDesciption + CRLF;
                                            break;
                                        case "dsi": // Название
                                            vFileBodyTriggerChecks += CRLF + "\t\t\t#region '" + vColumnName + "' - " + vColumnDesciption + CRLF + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t/// Если название не указано, формируется сообщение об ошибке: 'Название не указано'" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\tif (v" + vColumnName + ".Length == 0)" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t{" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\t_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate(\"Название не указано\"));" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t}" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t/// Если название указано проверяется его дублирование, если дублирование обнаружено, формируется сообщение об ошибке: 'Название уже используется'" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\telse" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t{" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\tif (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mNameExists(__fTableName, v" + vColumnName + ".Trim(), vCLU) == true)" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\t\t_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate(\"Название уже используется\"));" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t}" + CRLF;
                                            vFileBodyTriggerChecks += CRLF + "\t\t\t#endregion '" + vColumnName + "' - " + vColumnDesciption + CRLF;
                                            break;
                                        case "lnk": // Ссылки
                                            string vTableLinked = vColumnName.Substring(3); // Название связанной таблицы
                                            vFileBodyTriggerChecks += CRLF + "\t\t\t#region '" + vColumnName + "' - " + vColumnDesciption + CRLF + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t/// Если ссылка меньше нуля формируется сообщение об ошибке: 'Ссылка: ... указана ошибочно'" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\tif(v" + vColumnName + " < 0)" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t{" + CRLF;
                                            if (appTypeString.__mWordInArrayList(vTableName, __fTablesListAllowedZero) == true)
                                                vFileBodyTriggerChecks += "\t\t\t\tif(v" + vColumnName + " < 0)" + CRLF;
                                            else
                                                vFileBodyTriggerChecks += "\t\t\t\tif(v" + vColumnName + " <= 0)" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\t\t_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate(\"" + vColumnDesciption + " указана ошибочно\"));" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t}" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t/// Если ссылка больше нуля проверяется ее наличие в связанной таблице, если ссылка не обнаружена формируется сообщение оь ошибке: 'Ссылка: ... указана не верно'" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\telse" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t{" + CRLF;
                                            if (appTypeString.__mWordInArrayList(vTableName, __fTablesListAllowedZero) == true)
                                                vFileBodyTriggerChecks += "\t\t\tif(v" + vColumnName + " > 0)" + CRLF;
                                            else
                                                vFileBodyTriggerChecks += "\t\t\tif(v" + vColumnName + " >= 0)" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\tif (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mTableRowsCountWhere(\"" + vTableLinked + "\", \"CLU = \" + v" + vColumnName + ".ToString()) <= 0)" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t\t\t_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate(\"" + vColumnDesciption + " указана не верно\"));" + CRLF;
                                            vFileBodyTriggerChecks += "\t\t\t}" + CRLF;

                                            vFileBodyTriggerChecks += CRLF + "\t\t\t#endregion '" + vColumnName + "' - " + vColumnDesciption + CRLF;
                                            break;
                                        case "mrk": // Ссылки
                                            break;
                                        default: // Остальные
                                            break;
                                    }  // Обработка типа -- и типа данных полей
                                }  // Перебор полей в таблице и чтение значений в переменные
                                vFileBody += vFileBodyTriggerChecks;
                                /// Проверка наличия ошибок и возвращение результата
                                vFileBody += CRLF + "\t\t\tif (_fTriggerErrorsDescriptions.Count > 0)" + CRLF;
                                vFileBody += "\t\t\t\tvReturn = false;" + CRLF + CRLF;
                                vFileBody += "\t\t\treturn vReturn;" + CRLF;
                            } // __mCheckRecordFieldsFill(ref DataRow, TRIGGERTYPEFORCHANGERECORD)
                            vFileBody += "\t\t}" + CRLF + CRLF;
                            vFileBody += "\t\t#endregion Триггеры" + CRLF + CRLF;
                            vFileBody += "\t\t#endregion МЕТОДЫ" + CRLF;
                        } // = МЕТОДЫ / - Триггеры
                        vFileBody += "\t}" + CRLF;
                    } // Класс
                    vFileBody += "}" + CRLF;
                } // Пространство имен
                if (File.Exists(vFileName) == true) /// Удаление одноименного файла, если он существует
                    File.Delete(vFileName);
                vFiletext.__mWriteToEnd(vFileName, vFileBody); /// Создание нового файла
            }
            datApplication.__oMessages.__mShow(MESSAGESTYPES.Warning, "Сущности таблиц сформированы!");

            return;
        }
        /// <summary>
        /// Создание форм справочников
        /// </summary>
        /// <param name="pNameSpace">Название пространства имен</param>
        /// <param name="pPrefix">Префикс приложения</param>
        /// <param name="pDirectoryPath">Папка для размещения результата</param>
        /// <param name="pExcludeTableS">Список исключаемых таблиц</param>
        public void __mProgrammingFormsReferences(string pNameSpace, string pPrefix, string pDirectoryPath, params string[] pExcludeTableS)
        {
            appFileText vFileText = new appFileText(); /// Объект для работы текстовыми файлами
                                                       /// Перебор таблиц источника данных
            foreach (string vTableName in __mTablesList())
            {
                /// Проверка на наличие таблицы в списке исключенных
                if (appTypeString.__mWordInList(vTableName, pExcludeTableS) >= 0)
                    continue;

                #region Создание формы для правки справочников

                string vFileName = Path.Combine(pDirectoryPath, pPrefix + "FormGrid" + vTableName + ".cs"); // Имя создаваемого файла для правки данных
                /// Если файл существует, то создается его копия
                if (File.Exists(vFileName) == true)
                    vFileName = Path.Combine(pDirectoryPath, pPrefix + "FormGrid" + vTableName + ".bak");
                string vFileBody = "using nlApplication;" + CRLF; // Тело файла
                vFileBody += "using nlElements;" + CRLF;
                vFileBody += CRLF + "namespace " + pNameSpace + CRLF;
                vFileBody += "{" + CRLF;
                { /// Пространство имен
                    vFileBody += "\t///<summary>" + CRLF;
                    vFileBody += "\t/// Класс '" + pPrefix + "FormGrid" + vTableName + "'" + CRLF;
                    vFileBody += "\t///</summary>" + CRLF;
                    vFileBody += "\t///<remarks>Форма для правки сущности '" + __mTableDescription(vTableName) + "'</remarks>" + CRLF;
                    vFileBody += "\tpublic class " + pPrefix + "FormGrid" + vTableName + " : davFormGrid" + CRLF;
                    vFileBody += "\t{" + CRLF;
                    { /// Тело класса
                        vFileBody += "\t\t#region = МЕТОДЫ" + CRLF + CRLF;
                        vFileBody += "\t\t#region - Поведение" + CRLF + CRLF;
                        { /// = МЕТОДЫ / - Поведение / Объект
                            vFileBody += "\t\t/// <summary>" + CRLF;
                            vFileBody += "\t\t/// Сборка объекта" + CRLF;
                            vFileBody += "\t\t/// </summary>" + CRLF;
                            vFileBody += "\t\tprotected override void _mObjectAssembly()" + CRLF;
                            vFileBody += "\t\t{" + CRLF;
                            { // _mLoad()
                                vFileBody += "\t\t\tSuspendLayout();" + CRLF + CRLF;
                                vFileBody += "\t\t\tbase._mObjectAssembly();" + CRLF + CRLF;
                                vFileBody += "\t\t\t__mCaptionBuilding(\"" + __mTableDescription(vTableName) + "\");" + CRLF;
                                vFileBody += "\t\t\t__cAreaGrid.__oEssence_ = new " + pPrefix + "Essence" + vTableName + "();" + CRLF;
                                vFileBody += "\t\t\t__cAreaGrid.__oFormFilter = typeof(" + pPrefix + "FormFilter" + vTableName + ");" + CRLF;
                                vFileBody += "\t\t\t__cAreaGrid.__oFormOpened = typeof(" + pPrefix + "FormRecord" + vTableName + ");" + CRLF;
                                vFileBody += "\t\t\t__cAreaGrid.__fFormOpenedType = CONTROLsOPENEDTYPES.FormRecord;" + CRLF;
                                vFileBody += CRLF + "\t\t\t#region Сетка / Определение колонок" + CRLF + CRLF;
                                { /// Сетка / Определение колонок
                                    foreach (DataRow vDataRowField in __mTableColumnS(vTableName.Trim()).Rows)
                                    { // Перебор колонок в таблице

                                        string vColumnName = vDataRowField["dsiFld"].ToString().Trim(); // Название колонки 
                                        string vColumnDesciption = vDataRowField["Dcr"].ToString().Trim().Contains(":") == false ? vDataRowField["Dcr"].ToString().Trim() : vDataRowField["Dcr"].ToString().Trim().Substring(vDataRowField["Dcr"].ToString().IndexOf(':') + 1).Trim(); ; // Описание поля

                                        /// Пропускаемые поля
                                        string vFieldsSkip = "CHG, CLU, GID"; // Список пропускаемых полей
                                        if (appTypeString.__mWordInList(vColumnName, vFieldsSkip, ',') >= 0)
                                            continue;

                                        vFileBody += "\t\t\t__cAreaGrid.__mColumnAdd(davApplication.__oTunes.__mTranslate(\"" + vColumnDesciption + "\")" + CRLF;

                                        ///// Определение заголовков колонок 
                                        //if (vColumnName.ToLower().StartsWith("CLU") == true)
                                        //    vFileBody += "\t\t\t__cAreaGrid.__mColumnAdd(davApplication.__oTunes.__mTranslate(\"Ключ\")" + CRLF; /// - Ключ
                                        //else
                                        //    if (vColumnName.ToLower().StartsWith("cod") == true)
                                        //    vFileBody += "\t\t\t__cAreaGrid.__mColumnAdd(davApplication.__oTunes.__mTranslate(\"Код\")" + CRLF; /// - Код
                                        //else
                                        //        if (vColumnName.ToLower().StartsWith("dsi") == true)
                                        //    vFileBody += "\t\t\t__cAreaGrid.__mColumnAdd(davApplication.__oTunes.__mTranslate(\"Название\")" + CRLF; /// Название
                                        //else
                                        //            if (vColumnName.ToLower().IndexOf("sht") > 0)
                                        //    vFileBody += "\t\t\t__cAreaGrid.__mColumnAdd(davApplication.__oTunes.__mTranslate(\"Сокращение\")" + CRLF; /// Сокращение
                                        //else
                                        //                if (vColumnName == "CHG")
                                        //    vFileBody += "\t\t\t__cAreaGrid.__mColumnAdd(davApplication.__oTunes.__mTranslate(\"Корректировка\")" + CRLF; /// Корректировкка
                                        //else
                                        //                    if (vColumnName == "LCK")
                                        //    vFileBody += "\t\t\t__cAreaGrid.__mColumnAdd(davApplication.__oTunes.__mTranslate(\"Блокировка\")" + CRLF; /// Блокировка
                                        //else
                                        //                        if (vColumnName.ToLower().StartsWith("lnk") == true | vColumnName.ToLower().StartsWith("mrk") == true)
                                        //    vFileBody += "\t\t\t__cAreaGrid.__mColumnAdd(davApplication.__oTunes.__mTranslate(\"" + appTypeString.__mWordNumber(vColumnDesciption, 1, ':').Trim() + "\")" + CRLF; /// Ссылка
                                        //else
                                        //    vFileBody += "\t\t\t__cAreaGrid.__mColumnAdd(davApplication.__oTunes.__mTranslate(\"" + vColumnDesciption + "\")" + CRLF; // Заголовок колонки

                                        /// Определение названия колонки
                                        vFileBody += "\t\t\t\t\t, \"" + vColumnName + "\"" + CRLF;
                                        /// Определение видимости колонки
                                        vFileBody += "\t\t\t\t\t, true" + CRLF;
                                        if (vColumnName.Trim().ToLower().StartsWith("CLU") == true)
                                            vFileBody += "\t\t\t\t\t, false" + CRLF; // Видимость колонки
                                        else
                                            vFileBody += "\t\t\t\t\t, true" + CRLF;  // Видимость колонки
                                        /// Определение типа колонки
                                        if (vColumnName.Trim().ToLower() != "lck")
                                            vFileBody += "\t\t\t\t\t, \"DataGridViewTextBoxColumn\");" + CRLF;
                                        else
                                            vFileBody += "\t\t\t\t\t, \"DataGridViewCheckBoxColumn\");" + CRLF;
                                    } // Перебок колонок в таблице
                                    vFileBody += CRLF + "\t\t\t__cAreaGrid.__mGridBuild();" + CRLF + CRLF;
                                } // Сетка / Определение колонок
                                vFileBody += "\t\t\t#endregion Сетка / Определение колонок" + CRLF + CRLF;
                                vFileBody += "\t\t\tResumeLayout(false);" + CRLF + CRLF;
                                vFileBody += "\t\t\treturn;" + CRLF;
                            } // _mLoad()
                            vFileBody += "\t\t}" + CRLF + CRLF;
                        } // МЕТОДЫ / Действия / Объект
                        vFileBody += "\t\t#endregion Поведение" + CRLF + CRLF;
                        vFileBody += "\t\t#endregion МЕТОДЫ" + CRLF;
                        vFileBody += "\t}" + CRLF;
                    } // Тело класса
                } // Пространство имен
                vFileBody += "}" + CRLF;

                if (File.Exists(vFileName) == true) /// Удаление одноименного файла, если он существует
                    File.Delete(vFileName);
                vFileText.__mWriteToEnd(vFileName, vFileBody); /// Создание нового файла

                #endregion Создание формы для правки справочников

                #region Создание формы для построения фильтра

                vFileName = Path.Combine(pDirectoryPath, pPrefix + "FormFilter" + vTableName + ".cs"); // Имя создаваемого файла для построения фильтра
                /// Если файл существует, то создается его копия
                if (File.Exists(vFileName) == true)
                    vFileName = Path.Combine(pDirectoryPath, pPrefix + "FormFilter" + vTableName + ".bak");
                vFileBody = "using nlApplication;" + CRLF; // Тело файла
                vFileBody += "using nlElements;" + CRLF;
                vFileBody += CRLF + "namespace " + pNameSpace + CRLF;
                vFileBody += "{" + CRLF;
                { // Пространство имен
                    vFileBody += "\t///<summary>" + CRLF;
                    vFileBody += "\t/// Класс '" + pPrefix + "FormFilter" + vTableName + "'" + CRLF;
                    vFileBody += "\t///</summary>" + CRLF;
                    vFileBody += "\t///<remarks>Форма для построения фильтра '" + __mTableDescription(vTableName) + "'</remarks>" + CRLF;
                    vFileBody += "\tpublic class " + pPrefix + "FormFilter" + vTableName + " : elmFormFilter" + CRLF;
                    vFileBody += "\t{" + CRLF;
                    {  // Тело класса
                        string vBodyAttachComponents = CRLF + "\t\t\t#region /// Размещение компонентов" + CRLF + CRLF; // Размещение компонентов
                        string vBodyTuningComponents = CRLF + "\t\t\t#region /// Настройка компонентов" + CRLF + CRLF; // Настройка компонентов
                        string vBodyDeclareComponents = ""; // Объявление компонентов

                        foreach (DataRow vDataRowField in __mTableColumnS(vTableName.Trim()).Rows)
                        { // Перебор колонок в таблице
                            string vColumnName = vDataRowField["dsiFld"].ToString().Trim(); // Название колонки 
                            string vColumnDesciption = vDataRowField["Dcr"].ToString().Trim().Contains(":") == false ? vDataRowField["Dcr"].ToString().Trim() : vDataRowField["Dcr"].ToString().Trim().Substring(vDataRowField["Dcr"].ToString().IndexOf(':') + 1).Trim(); ; // Описание поля

                            /// Пропускаемые поля
                            string vFieldsSkip = "CHG, CLU, GID"; // Список пропускаемых полей
                            if (appTypeString.__mWordInList(vColumnName, vFieldsSkip, ',') >= 0)
                                continue;

                            string vColumnType = vDataRowField["Typ"].ToString().Trim(); // Тип данных колонки

                            string vClassName = pPrefix + "Input" + vTableName + vColumnName;
                            switch (vColumnName.Substring(0, 3))
                            {  /// Обработка типа и типа данных полей
                                case "CLU":

                                    break;
                                default:

                                    vBodyAttachComponents += "\t\t\t__cAreaFilter.__mInputAdd(_c" + vClassName.Substring(3) + ");" + CRLF;

                                    vBodyDeclareComponents += "\t\t///<summary>" + CRLF;
                                    vBodyDeclareComponents += "\t\t/// Поле ввода '" + vColumnDesciption + "'" + CRLF;
                                    vBodyDeclareComponents += "\t\t///</summary>" + CRLF;
                                    vBodyDeclareComponents += "\t\tprotected " + vClassName + " _cInput" + vTableName + vColumnName + " = new " + vClassName + "();" + CRLF;

                                    break;
                                    //case "cod":
                                    //    vBodyAttachComponents += "\t\t\t__cAreaFilter.__mInputAdd(_c" + vColumnName + ");" + CRLF;
                                    //    //
                                    //    vBodyTuningComponents += "\t\t\t // _c" + vColumnName + CRLF;
                                    //    vBodyTuningComponents += "\t\t\t{" + CRLF;
                                    //    {
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fCaption_ = \"" + vColumnDesciption + "\";" + CRLF;
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fFieldName = \"" + vColumnName + "\";" + CRLF;
                                    //    }
                                    //    vBodyTuningComponents += "\t\t\t}" + CRLF;
                                    //    //
                                    //    vBodyDeclareComponents += "\t\t///<summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t/// Поле ввода '" + vColumnDesciption + "'" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t///</summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\tprotected " + vClassName + " _c" + vColumnName + " = new " + vClassName + "();" + CRLF;
                                    //    break;
                                    //case "dsi":
                                    //    vBodyAttachComponents += "\t\t\t__cAreaFilter.__mInputAdd(_c" + vColumnName + ");" + CRLF;
                                    //    //
                                    //    vBodyTuningComponents += "\t\t\t // _c" + vColumnName + CRLF;
                                    //    vBodyTuningComponents += "\t\t\t{" + CRLF;
                                    //    {
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fCaption_ = \"" + vColumnDesciption + "\";" + CRLF;
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fFieldName = \"" + vColumnName + "\";" + CRLF;
                                    //    }
                                    //    vBodyTuningComponents += "\t\t\t}" + CRLF;
                                    //    //
                                    //    vBodyDeclareComponents += "\t\t///<summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t/// Поле ввода '" + vColumnDesciption + "'" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t///</summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\tprotected " + vClassName + " _c" + vColumnName + " = new " + vClassName + "();" + CRLF;
                                    //    break;
                                    //case "dtm":
                                    //    vBodyAttachComponents += "\t\t\t__cAreaFilter.__mInputAdd(_c" + vColumnName + ");" + CRLF;
                                    //    //
                                    //    vBodyTuningComponents += "\t\t\t // _c" + vColumnName + CRLF;
                                    //    vBodyTuningComponents += "\t\t\t{" + CRLF;
                                    //    {
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fCaption_ = \"" + vColumnDesciption + "\";" + CRLF;
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fFieldName = \"" + vColumnName + "\";" + CRLF;
                                    //    }
                                    //    vBodyTuningComponents += "\t\t\t}" + CRLF;
                                    //    //
                                    //    vBodyDeclareComponents += "\t\t///<summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t/// Поле ввода '" + vColumnDesciption + "'" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t///</summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\tprotected davInputDateTimePeriod _c" + vColumnName + " = new davInputDateTimePeriod();" + CRLF;
                                    //    break;
                                    //case "mrk":
                                    //    vBodyAttachComponents += "\t\t\t__cAreaFilter.__mInputAdd(_c" + vColumnName + ");" + CRLF;
                                    //    //
                                    //    vBodyTuningComponents += "\t\t\t // _c" + vColumnName + CRLF;
                                    //    vBodyTuningComponents += "\t\t\t{" + CRLF;
                                    //    {
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fCaption_ = \"" + vColumnDesciption + "\";" + CRLF;
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fFieldName = \"" + vColumnName + "\";" + CRLF;
                                    //    }
                                    //    vBodyTuningComponents += "\t\t\t}" + CRLF;
                                    //    //
                                    //    vBodyDeclareComponents += "\t\t///<summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t/// Поле ввода '" + vColumnDesciption + "'" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t///</summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\tprotected " + vClassName + " _c" + vColumnName + " = new " + vClassName + "();" + CRLF;
                                    //    break;
                                    //case "opt":
                                    //    break;
                                    //case "CHG":
                                    //    break;
                                    //case "GID":
                                    //    break;
                                    //case "LCK":
                                    //    vBodyAttachComponents += "\t\t\t__cAreaFilter.__mInputAdd(_c" + vColumnName + ");" + CRLF;
                                    //    //
                                    //    vBodyTuningComponents += "\t\t\t// _c" + vColumnName + CRLF;
                                    //    vBodyTuningComponents += "\t\t\t{" + CRLF;
                                    //    {
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fCaption_ = \"" + vColumnDesciption + "\"; " + CRLF;
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fFieldName = \"" + vColumnName + "\";" + CRLF;
                                    //    }
                                    //    vBodyTuningComponents += "\t\t\t}" + CRLF;
                                    //    //
                                    //    vBodyDeclareComponents += "\t\t///<summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t/// Поле ввода '" + vColumnDesciption + "'" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t///</summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\tprotected " + vClassName + " _c" + vColumnName + " = new " + vClassName + "();" + CRLF;
                                    //    break;
                                    //default:
                                    //    vBodyAttachComponents += "\t\t\t__cAreaFilter.__mInputAdd(_c" + vColumnName + ");" + CRLF;
                                    //    //
                                    //    vBodyTuningComponents += "\t\t\t // _c" + vColumnName + CRLF;
                                    //    vBodyTuningComponents += "\t\t\t{" + CRLF;
                                    //    {
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fCaption_ = \"" + vColumnDesciption + "\";" + CRLF;
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fFieldName = \"" + vColumnName + "\";" + CRLF;
                                    //    }
                                    //    vBodyTuningComponents += "\t\t\t}" + CRLF;
                                    //    //
                                    //    vBodyDeclareComponents += "\t\t///<summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t/// Поле ввода '" + vColumnDesciption + "'" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t///</summary>" + CRLF;
                                    //    switch (vColumnType)
                                    //    {
                                    //        case "bool":
                                    //            vBodyDeclareComponents += "\t\tprotected davInputComboBool _c" + vColumnName + " = new davInputComboBool();" + CRLF;
                                    //            break;
                                    //        case "int":
                                    //            vBodyDeclareComponents += "\t\tprotected davInputNumberInt _c" + vColumnName + " = new davInputNumberInt();" + CRLF;
                                    //            break;
                                    //        case "numeric":
                                    //            vBodyDeclareComponents += "\t\tprotected davInputNumberDecimal _c" + vColumnName + " = new davInputNumberDecimal();" + CRLF;
                                    //            break;
                                    //        default:
                                    //            vBodyDeclareComponents += "\t\tprotected davInputChar _c" + vColumnName + " = new davInputChar();" + CRLF;
                                    //            break;
                                    //    }
                                    //    break;
                            }  /// Обработка типа и типа данных полей
                        } // Перебор колонок в таблице

                        vBodyAttachComponents += CRLF + "\t\t\t#endregion Размещение компонентов" + CRLF;
                        vBodyTuningComponents += CRLF + "\t\t\t#endregion Настройка компонентов" + CRLF;

                        vFileBody += "\t\t#region = МЕТОДЫ" + CRLF + CRLF;
                        vFileBody += "\t\t#region - Поведение" + CRLF + CRLF;
                        { // МЕТОДЫ / Действия / Объект
                            vFileBody += "\t\t/// <summary>" + CRLF;
                            vFileBody += "\t\t/// Сборка объекта" + CRLF;
                            vFileBody += "\t\t/// </summary>" + CRLF;
                            vFileBody += "\t\tprotected override void _mObjectAssembly()" + CRLF;
                            vFileBody += "\t\t{" + CRLF;
                            {
                                vFileBody += "\t\t\tSuspendLayout();" + CRLF + CRLF;
                                vFileBody += "\t\t\tbase._mObjectAssembly();" + CRLF;
                                vFileBody += vBodyAttachComponents;
                                vFileBody += vBodyTuningComponents;
                                vFileBody += CRLF + "\t\t\tResumeLayout(false);" + CRLF + CRLF;
                                vFileBody += "\t\t\treturn;" + CRLF;
                            }
                            vFileBody += "\t\t}" + CRLF + CRLF;
                        } // МЕТОДЫ / Действия / Объект
                        vFileBody += "\t\t#endregion Поведение" + CRLF;
                        vFileBody += CRLF + "\t\t#endregion МЕТОДЫ" + CRLF;
                        vFileBody += CRLF + "\t\t#region = ПОЛЯ" + CRLF;
                        vFileBody += CRLF + "\t\t#region - Компоненты" + CRLF + CRLF;
                        vFileBody += vBodyDeclareComponents + CRLF;
                        vFileBody += "\t\t#endregion Компоненты" + CRLF + CRLF;
                        vFileBody += "\t\t#endregion ПОЛЯ" + CRLF;
                    } // Тело класса
                    vFileBody += "\t}" + CRLF;
                } // Пространство имен
                vFileBody += "}" + CRLF;

                if (File.Exists(vFileName) == true) /// Удаление одноименного файла, если он существует
                    File.Delete(vFileName);
                vFileText.__mWriteToEnd(vFileName, vFileBody); /// Создание нового файла

                #endregion Создание формы для построения фильтра

                #region Создание формы для изменения записи

                vFileName = Path.Combine(pDirectoryPath, pPrefix + "FormRecord" + vTableName + ".cs"); // Имя создаваемого файла для построения фильтра
                /// Если файл существует, то создается его копия
                if (File.Exists(vFileName) == true)
                    vFileName = Path.Combine(pDirectoryPath, pPrefix + "FormRecord" + vTableName + ".bak");
                vFileBody = "using nlApplication;" + CRLF; // Тело файла
                vFileBody += "using nlElements;" + CRLF;
                vFileBody += CRLF + "namespace " + pNameSpace + CRLF;
                vFileBody += "{" + CRLF;
                { /// Пространиство имен
                    vFileBody += "\t///<summary>" + CRLF;
                    vFileBody += "\t/// Класс '" + pPrefix + "FormRecord" + vTableName + "'" + CRLF;
                    vFileBody += "\t///</summary>" + CRLF;
                    vFileBody += "\t///<remarks>Форма для изменения записи '" + __mTableDescription(vTableName) + "'</remarks>" + CRLF;
                    vFileBody += "\tpublic class " + pPrefix + "FormRecord" + vTableName + " : davFormRecord" + CRLF;
                    vFileBody += "\t{" + CRLF;
                    { /// Тело класса
                        string vBodyAttachComponents = CRLF + "\t\t\t#region /// Размещение компонентов" + CRLF + CRLF; // Размещение компонентов
                        string vBodyTuningComponents = CRLF + "\t\t\t#region /// Настройка компонентов" + CRLF + CRLF; // Настройка компонентов
                        vBodyTuningComponents += "\t\t\t__cAreaRecord.__oEssence = new " + pPrefix + "Essence" + vTableName + "();" + CRLF + CRLF;
                        string vBodyDeclareComponents = ""; // Объявление компонентов

                        foreach (DataRow vDataRowField in __mTableColumnS(vTableName.Trim()).Rows)
                        { // Перебор колонок в таблице
                            string vColumnName = vDataRowField["dsiFld"].ToString().Trim(); // Название колонки 
                            string vColumnDesciption = vDataRowField["Dcr"].ToString().Trim().Contains(":") == false ? vDataRowField["Dcr"].ToString().Trim() : vDataRowField["Dcr"].ToString().Trim().Substring(vDataRowField["Dcr"].ToString().IndexOf(':') + 1).Trim(); ; // Описание поля
                            string vColumnType = vDataRowField["Typ"].ToString().Trim(); // Тип данных колонки

                            /// Пропускаемые поля
                            string vFieldsSkip = "CHG, CLU, GID"; // Список пропускаемых полей
                            if (appTypeString.__mWordInList(vColumnName, vFieldsSkip, ',') >= 0)
                                continue;

                            string vClassName = pPrefix + "Input" + vTableName + vColumnName;

                            switch (vColumnName.Substring(0, 3))
                            {  /// Обработка типа и типа данных полей
                                case "CLU":
                                    break;
                                default:

                                    vBodyAttachComponents += "\t\t\t__cAreaRecord.__mInputAdd(_c" + vClassName.Substring(3) + ");" + CRLF;

                                    vBodyDeclareComponents += "\t\t///<summary>" + CRLF;
                                    vBodyDeclareComponents += "\t\t/// Поле ввода '" + vColumnDesciption + "'" + CRLF;
                                    vBodyDeclareComponents += "\t\t///</summary>" + CRLF;
                                    vBodyDeclareComponents += "\t\tprotected " + vClassName + " _cInput" + vTableName + vColumnName + " = new " + vClassName + "();" + CRLF;

                                    break;
                                    //case "cod":
                                    //    vBodyAttachComponents += "\t\t\t__cAreaRecord.__mInputAdd(_c" + vColumnName + ");" + CRLF;
                                    //    //
                                    //    vBodyTuningComponents += "\t\t\t// _c" + vColumnName + CRLF;
                                    //    vBodyTuningComponents += "\t\t\t{" + CRLF;
                                    //    {
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fCaption_ = \"" + vColumnDesciption + "\";" + CRLF;
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fFieldName = \"" + vColumnName + "\";" + CRLF;
                                    //    }
                                    //    vBodyTuningComponents += "\t\t\t}" + CRLF;
                                    //    //
                                    //    vBodyDeclareComponents += "\t\t///<summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t/// Поле ввода '" + vColumnDesciption + "'" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t///</summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\tprotected davInputNumberInt _c" + vColumnName + " = new davInputNumberInt();" + CRLF;
                                    //    break;
                                    //case "dsi":
                                    //    vBodyAttachComponents += "\t\t\t__cAreaRecord.__mInputAdd(_c" + vColumnName + ");" + CRLF;
                                    //    //
                                    //    vBodyTuningComponents += "\t\t\t// _c" + vColumnName + CRLF;
                                    //    vBodyTuningComponents += "\t\t\t{" + CRLF;
                                    //    {
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fCaption_ = \"" + vColumnDesciption + "\";" + CRLF;
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fFieldName = \"" + vColumnName + "\";" + CRLF;
                                    //    }
                                    //    vBodyTuningComponents += "\t\t\t}" + CRLF;
                                    //    //
                                    //    vBodyDeclareComponents += "\t\t///<summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t/// Поле ввода '" + vColumnDesciption + "'" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t///</summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\tprotected davInputChar _c" + vColumnName + " = new davInputChar();" + CRLF;
                                    //    break;
                                    //case "dtm":
                                    //    vBodyAttachComponents += "\t\t\t__cAreaRecord.__mInputAdd(_c" + vColumnName + ");" + CRLF;
                                    //    //
                                    //    vBodyTuningComponents += "\t\t\t// _c" + vColumnName + CRLF;
                                    //    vBodyTuningComponents += "\t\t\t{" + CRLF;
                                    //    {
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fCaption_ = \"" + vColumnDesciption + "\";" + CRLF;
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fFieldName = \"" + vColumnName + "\";" + CRLF;
                                    //    }
                                    //    vBodyTuningComponents += "\t\t\t}" + CRLF;
                                    //    //
                                    //    vBodyDeclareComponents += "\t\t///<summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t/// Поле ввода '" + vColumnDesciption + "'" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t///</summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\tprotected davInputDateTimePeriod _c" + vColumnName + " = new davInputDateTimePeriod();" + CRLF;
                                    //    break;
                                    //case "mrk":
                                    //    vBodyAttachComponents += "\t\t\t__cAreaRecord.__mInputAdd(_c" + vColumnName + ");" + CRLF;
                                    //    //
                                    //    vBodyTuningComponents += "\t\t\t// _c" + vColumnName + CRLF;
                                    //    vBodyTuningComponents += "\t\t\t{" + CRLF;
                                    //    {
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fCaption_ = \"" + vColumnDesciption + "\";" + CRLF;
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fFieldName = \"" + vColumnName + "\";" + CRLF;
                                    //    }
                                    //    vBodyTuningComponents += "\t\t\t}" + CRLF;
                                    //    //
                                    //    vBodyDeclareComponents += "\t\t///<summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t/// Поле ввода '" + vColumnDesciption + "'" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t///</summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\tprotected davInputComboBool _c" + vColumnName + " = new davInputComboBool();" + CRLF;
                                    //    break;
                                    //case "opt":
                                    //    break;
                                    //case "CHG":
                                    //    break;
                                    //case "GID":
                                    //    break;
                                    //case "LCK":
                                    //    vBodyAttachComponents += "\t\t\t__cAreaRecord.__mInputAdd(_c" + vColumnName + ");" + CRLF;
                                    //    //
                                    //    vBodyTuningComponents += "\t\t\t// _c" + vColumnName + CRLF;
                                    //    vBodyTuningComponents += "\t\t\t{" + CRLF;
                                    //    {
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fCaption_ = \"" + vColumnDesciption + "\";" + CRLF;
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fFieldName = \"" + vColumnName + "\";" + CRLF;
                                    //    }
                                    //    vBodyTuningComponents += "\t\t\t}" + CRLF;
                                    //    //
                                    //    vBodyDeclareComponents += "\t\t///<summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t/// Поле ввода '" + vColumnDesciption + "'" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t///</summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\tprotected davInputComboBool _c" + vColumnName + " = new davInputComboBool();" + CRLF;
                                    //    break;
                                    //default:
                                    //    vBodyAttachComponents += "\t\t\t__cAreaRecord.__mInputAdd(_c" + vColumnName + ");" + CRLF;
                                    //    //
                                    //    vBodyTuningComponents += "\t\t\t// _c" + vColumnName + CRLF;
                                    //    vBodyTuningComponents += "\t\t\t{" + CRLF;
                                    //    {
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fCaption_ = \"" + vColumnDesciption + "\";" + CRLF;
                                    //        vBodyTuningComponents += "\t\t\t\t_c" + vColumnName + ".__fFieldName = \"" + vColumnName + "\";" + CRLF;
                                    //    }
                                    //    vBodyTuningComponents += "\t\t\t}" + CRLF;
                                    //    //
                                    //    vBodyDeclareComponents += "\t\t///<summary>" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t/// Поле ввода '" + vColumnDesciption + "'" + CRLF;
                                    //    vBodyDeclareComponents += "\t\t///</summary>" + CRLF;
                                    //    switch (vColumnType)
                                    //    {
                                    //        case "bool":
                                    //            vBodyDeclareComponents += "\t\tprotected davInputComboBool _c" + vColumnName + " = new davInputComboBool();" + CRLF;
                                    //            break;
                                    //        case "int":
                                    //            vBodyDeclareComponents += "\t\tprotected davInputNumberInt _c" + vColumnName + " = new davInputNumberInt();" + CRLF;
                                    //            break;
                                    //        case "numeric":
                                    //            vBodyDeclareComponents += "\t\tprotected davInputNumberDecimal _c" + vColumnName + " = new davInputNumberDecimal();" + CRLF;
                                    //            break;
                                    //        default:
                                    //            vBodyDeclareComponents += "\t\tprotected davInputChar _c" + vColumnName + " = new davInputChar();" + CRLF;
                                    //            break;
                                    //    }
                                    //    break;
                            }  /// Обработка типа и типа данных полей
                        } // Перебор колонок в таблице

                        vBodyAttachComponents += CRLF + "\t\t\t#endregion Размещение компонентов" + CRLF;
                        vBodyTuningComponents += CRLF + "\t\t\t#endregion Настройка компонентов" + CRLF;

                        vFileBody += "\t\t#region = МЕТОДЫ" + CRLF + CRLF;
                        vFileBody += "\t\t#region - Поведение" + CRLF + CRLF;
                        { // МЕТОДЫ / Действия / Объект
                            vFileBody += "\t\t/// <summary>" + CRLF;
                            vFileBody += "\t\t/// Загрузка формы" + CRLF;
                            vFileBody += "\t\t/// </summary>" + CRLF;
                            vFileBody += "\t\tprotected override void _mObjectAssembly()" + CRLF;
                            vFileBody += "\t\t{" + CRLF;
                            { // _mLoad()
                                vFileBody += "\t\t\tbase._mObjectAssembly();" + CRLF;
                                vFileBody += CRLF + "\t\t\tSuspendLayout();" + CRLF;
                                vFileBody += CRLF + "\t\t\t__mCaptionBuilding(\"" + __mTableDescription(vTableName) + "\");" + CRLF;
                                vFileBody += vBodyAttachComponents;
                                vFileBody += vBodyTuningComponents;
                                vFileBody += CRLF + "\t\t\tResumeLayout(false);" + CRLF + CRLF;
                                vFileBody += "\t\t\treturn;" + CRLF;
                            } // _mLoad()
                            vFileBody += "\t\t}" + CRLF + CRLF;
                        } // МЕТОДЫ / Действия / Объект
                        vFileBody += "\t\t#endregion Поведение" + CRLF + CRLF;
                        vFileBody += "\t\t#endregion МЕТОДЫ" + CRLF;
                        vFileBody += CRLF + "\t\t#region = ПОЛЯ" + CRLF;
                        vFileBody += CRLF + "\t\t#region - Компоненты" + CRLF + CRLF;
                        vFileBody += vBodyDeclareComponents + CRLF;
                        vFileBody += "\t\t#endregion Компоненты" + CRLF;
                        vFileBody += CRLF + "\t\t#endregion ПОЛЯ" + CRLF;
                    } // Тело класса
                    vFileBody += "\t}" + CRLF;
                } // Пространство имен
                vFileBody += "}" + CRLF;

                if (File.Exists(vFileName) == true) /// Удаление одноименного файла, если он существует
                    File.Delete(vFileName);
                vFileText.__mWriteToEnd(vFileName, vFileBody); /// Создание нового файла

                #endregion Создание формы для изменения записи
            }

            datApplication.__oMessages.__mShow(MESSAGESTYPES.Warning, "Формы справочников сформированы!");

            return;
        }
        /// <summary>
        /// Создание форм отчетов
        /// </summary>
        /// <param name="pNameSpace">Название пространства имен</param>
        /// <param name="pPrefix">Префикс приложения</param>
        /// <param name="pDirectoryPath">Папка для размещения результата</param>
        public void __mProgrammingFormsReports(string pNameSpace, string pPrefix, string pDirectoryPath)
        {
            sstFileSystem vFileSystem = new sstFileSystem(); // Объект для работы с файловой системой
            appFileIni vFileIni = new appFileIni(); // Объект для работы с инициализационными файлами
            appFileText vFileText = new appFileText(); // Объект для работы текстовыми файлами

            List<sstUnitFile> vFileS = vFileSystem.__mFilesInDirectory(datApplication.__oPathes.__fDirectoryStart);
            foreach (sstUnitFile vFile in vFileS)
            {
                if (vFile.__fName.StartsWith("Form") == false)
                    continue;

                vFileIni.__fFilePath = Path.Combine(datApplication.__oPathes.__fDirectoryStart, vFile.__fName); // Читаемый файл

                string vFilePath = Path.Combine(pDirectoryPath, pPrefix + vFileIni.__mValueReadWrite("", "FORM", "Name")) + ".cs"; // Путь создаваемого файла для правки данных
                string vFileName = Path.GetFileNameWithoutExtension(vFilePath);
                string vFormCaption = vFileIni.__mValueReadWrite("", "FORM", "Caption"); // Заголовок создаваемой формы
                /// Если файл существует, то создается его копия
                if (File.Exists(vFilePath) == true)
                    vFilePath = Path.ChangeExtension(vFilePath, ".bak");

                string vFileBody = "using nlApplication;" + CRLF; // Тело файла
                vFileBody += "using nlElements;" + CRLF;
                vFileBody += "using System;" + CRLF;
                vFileBody += "using System.Windows.Forms;" + CRLF;
                vFileBody += CRLF + "namespace " + pNameSpace + CRLF;
                vFileBody += "{" + CRLF;
                {
                    vFileBody += "\t///<summary>" + CRLF;
                    vFileBody += "\t/// Класс '" + vFileName + "'" + CRLF;
                    vFileBody += "\t///</summary>" + CRLF;
                    vFileBody += "\t///<remarks>Форма отчета '" + vFormCaption + "'</remarks>" + CRLF;
                    vFileBody += "\tpublic class " + vFileName + " : davFormReport" + CRLF;
                    vFileBody += "\t{" + CRLF;
                    vFileBody += "\t\t#region = МЕТОДЫ" + CRLF + CRLF;

                    vFileBody += "\t\t#region - Поведение" + CRLF + CRLF;

                    vFileBody += "\t\t#region Объект" + CRLF + CRLF;

                    vFileBody += "\t\t/// <summary>" + CRLF;
                    vFileBody += "\t\t/// Сборка объекта" + CRLF;
                    vFileBody += "\t\t/// </summary>" + CRLF;
                    vFileBody += "\t\tprotected override void _mObjectAssembly()" + CRLF;
                    vFileBody += "\t\t{" + CRLF;
                    vFileBody += "\t\t\tSuspendLayout();" + CRLF + CRLF;
                    vFileBody += "\t\t\tbase._mObjectAssembly();" + CRLF + CRLF;
                    vFileBody += "\t\t\t#region /// Размещение компонентов" + CRLF + CRLF;

                    ArrayList vControls = vFileIni.__mParametersList("CONTROLs");
                    foreach (string vString in vControls)
                    {
                        string vControl = vString;
                        vFileBody += "\t\t\t__cAreaReport.__mInputAdd(_c" + vControl.Substring(3) + ");" + CRLF;
                    }

                    vFileBody += CRLF + "\t\t\t#endregion Размещение компонентов" + CRLF + CRLF;
                    vFileBody += "\t\t\t#region /// Настройка компонентов" + CRLF + CRLF;
                    vFileBody += "\t\t\t__fCaption_ = \"" + vFormCaption + "\";" + CRLF;
                    vFileBody += "\t\t\t_fHelpTopic = \"\";" + CRLF + CRLF;
                    vFileBody += "\t\t\t// __cAreaReport" + CRLF;
                    vFileBody += "\t\t\t{" + CRLF;
                    vFileBody += "\t\t\t\t__cAreaReport.Dock = DockStyle.Fill;" + CRLF;
                    vFileBody += "\t\t\t\t__cAreaReport.__eButtonRun_Click += mAreaReportButtonRunClick;" + CRLF;
                    vFileBody += "\t\t\t}" + CRLF;
                    vFileBody += CRLF + "\t\t\t#endregion Настройка компонентов" + CRLF + CRLF;
                    vFileBody += "\t\t\tResumeLayout(false);" + CRLF + CRLF;
                    //vFileBody += "\t\t\tType vType = this.GetType();" + CRLF;
                    //vFileBody += "\t\t\tName = vType.Name;" + CRLF;
                    //vFileBody += "\t\t\t_fClassNameFull = vType.FullName + \".\";" + CRLF + CRLF;
                    vFileBody += "\t\t\treturn;" + CRLF;
                    vFileBody += "\t\t}" + CRLF + CRLF;
                    vFileBody += "\t\t#endregion Объект" + CRLF + CRLF;

                    vFileBody += "\t\t/// <summary>" + CRLF;
                    vFileBody += "\t\t/// Выполняется при выборе кнопки 'Выполнить'" + CRLF;
                    vFileBody += "\t\t/// </summary>" + CRLF;
                    vFileBody += "\t\tprivate void mAreaReportButtonRunClick(object sender, EventArgs e)" + CRLF;
                    vFileBody += "\t\t{" + CRLF;
                    vFileBody += "\t\t\t/// Построение фильтра запроса" + CRLF;
                    vFileBody += "\t\t\tstring vReturn = __cAreaReport.__mFilterBuild();" + CRLF;
                    vFileBody += "\t\t}" + CRLF + CRLF;

                    vFileBody += "\t\t#endregion Поведение" + CRLF + CRLF;
                    vFileBody += "\t\t#endregion МЕТОДЫ" + CRLF + CRLF;
                    vFileBody += "\t\t#region = ПОЛЯ" + CRLF + CRLF;
                    vFileBody += "\t\t#region - Компоненты" + CRLF + CRLF;
                    foreach (string vString in vControls)
                    {
                        string vControl = vString;
                        vFileBody += "\t\tprotected " + vControl + " _c" + vControl.Substring(3) + " = new " + vControl + "();" + CRLF;
                    }
                    vFileBody += CRLF + "\t\t#endregion Комопненты" + CRLF + CRLF;
                    vFileBody += "\t\t#endregion ПОЛЯ" + CRLF;
                    vFileBody += "\t}" + CRLF;
                    vFileBody += "}" + CRLF;

                    /// Вывод в файл
                    if (File.Exists(vFilePath) == true) // Удаление одноименного файла, если он существует
                        File.Delete(vFilePath);
                    vFileText.__mWriteToEnd(vFilePath, vFileBody); // Создание нового файла

                }
            }

            datApplication.__oMessages.__mShow(MESSAGESTYPES.Warning, "Формы отчетов сформированы!");

            return;
        }
        /// <summary>
        /// Формирование имени файла сущности для поля
        /// </summary>
        /// <param name="pTableS"></param>
        /// <param name="pFieldName"></param>
        /// <returns></returns>
        private string __mEssenseNameSeaching(ArrayList pTableS, string pFieldName)
        {
            string vReturn = "Essence"; // Возвращаемое значение

            if (pFieldName.ToLower().StartsWith("lnk") == true)
            {
                pFieldName = pFieldName.Substring(3);
                /// Если имя поля соответствует названию таблицы, возвращаем имя сущности
                if (appTypeString.__mWordInArrayList(pFieldName, pTableS) == true)
                {
                    vReturn += pFieldName;
                }
                else
                {
                    if (pFieldName.Length > 3)
                    {
                        pFieldName = appTypeString.__mSymbolsFromLeft(pFieldName, pFieldName.Length - 3);
                        if (appTypeString.__mWordInArrayList(pFieldName, pTableS) == true)
                        {
                            vReturn += pFieldName;
                        }
                    }
                }
            }

            return vReturn;
        }

        #endregion Программирование

        #region Таблицы

        /// <summary>
        /// Получение пустого курсора со структурой таблицы из базы данных
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns>? Таблица с одной пустой записью</returns>
        public virtual DataTable __mTableEmpty(string pTableName)
        {
            return __mSqlQuery("Select * From " + pTableName + " Where CLU < 0");
        }
        /// <summary>
        /// Проверка существования таблицы в базе данных источника данных
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns>[true] - таблица существует. иначе - [false]</returns>
        public bool __mTableExists(string pTableName)
        {
            bool vReturn = false; // Возвращаемое значение
            ArrayList vDataTable = __mTablesList();

            foreach (string vDataRow in vDataTable)
            {
                if (vDataRow.Trim().ToUpper() == pTableName.Trim().ToUpper())
                {
                    vReturn = true;
                    break;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Получение списка таблиц в базе данных
        /// </summary>
        /// <returns>Список таблиц в базе данных</returns>
        public virtual ArrayList __mTablesList()
        {
            return null; /// Возвращаемое значение
        }
        /// <summary>
        /// Получение описание таблицы в базе данных
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns>Описание таблицы</returns>
        public virtual string __mTableDescription(string pTableName)
        {
            return "";
        }
        /// <summary>
        /// Очистка таблицы со сбросом идентификатора в 0
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns>[true] - таблица очищена, иначе - [false]</returns>
        public virtual bool __mTableTruncate(string pTableName)
        {
            return false;
        }

        #endregion Таблицы

        #region Таблицы - Строки

        /// <summary>
        /// Получение записи из таблицы указанной идентификатором
        /// </summary>
        /// <param name="pClue">Идентификатор записи</param>
        /// <returns>[DataTable]</returns>
        public virtual DataTable __mTableRow(string pTableName, int pClue)
        {
            return __mSqlQuery("Select * From " + pTableName + " Where CLU = " + pClue.ToString());
        }
        /// <summary>
        /// Получение записи из таблицы указанной идентификатором
        /// </summary>
        /// <param name="pGuid">Уникальный идентификатор записи</param>
        /// <returns>[DataTable]</returns>
        public virtual DataTable __mTableRow(string pTableName, Guid pGuid)
        {
            return __mSqlQuery("Select * From " + pTableName + " Where GID = '" + pGuid.ToString() + "'");
        }
        /// <summary>
        /// Установка текущего времени в качестве последнего времени изменения записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор записи</param>
        /// <param name="pDataSourceAlias">Псевдоним источника данных в котором будут выполняться операции</param>
        public virtual bool __mTableRowChangeTimeNow(string pTableName, int pClue)
        {
            bool vReturn = false;
            if (__mSqlCommand("Update " + pTableName + " Set CHG = GetDate() Where CLU = " + pClue.ToString()) > 0)
                vReturn = true;

            return vReturn;
        }
        /// <summary>
        /// Подсчет количества дублирующихся записей
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldS">Список полей</param>
        /// <returns>{DataTable} - Таблица с названием поля и количеством повторений</returns>
        public virtual DataTable __mTableRowsCountDouble(string pTableName, params string[] pFieldS)
        {
            return null;
        }
        /// <summary>
        /// Подсчет количества записей удовлетворяющих условию
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pExpressionWhere">Условие для подсчета записей</param>
        /// <returns>Количество подсчитанных записей</returns>
        public virtual int __mTableRowsCountWhere(string pTableName, string pExpressionWhere)
        {
            return -1;
        }

        #endregion Таблицы - Строки

        #region Таблицы - Поля

        /// <summary>
        /// Проверка существования поля в таблице источника данных
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldName">Название поля</param>
        /// <returns>[true] - Поле существует, иначе - [false]</returns>
        public virtual bool __mTableColumnExists(string pTableName, string pFieldName)
        {
            return false;
        }
        /// <summary>
        /// Получение списка полей в таблице
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns>{DataTable} - заполненная списком полей указанной таблицы</returns>
        public virtual DataTable __mTableColumnS(string pTableName)
        {
            return null;
        }
        /// <summary>
        /// Получение информации о поле таблицы 
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pField">Название поля</param>
        /// <param name="pFieldInfo">Вид операции</param>
        /// <returns>Запрашиваемое значение, иначе [null]</returns>
        public virtual object __mTableColumnInfo(string pTableName, string pFieldName, FIELDINFO pFieldInfo)
        {
            return null;
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
        /// <param name="pDefaultValue">Значение по умолчанию</param>
        /// <returns>[true] - Колонка добавлена, иначе - [false]</returns>
        public virtual bool __mTableColumnAdd(string pTableName, string pColumnName, COLUMNSTYPES pDataType, bool IsNull, int pColumnScale = 0, int pColumnPrecision = 0, string pDefaultValue = "0")
        {
            return false;
        }
        /// <summary>
        /// Удаление колонки из таблицы
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pColumnName">Название колонки</param>
        /// <returns>[true] - Колонка добавлена, иначе - [false]</returns>
        public virtual bool __mTableColumnDrop(string pTableName, string pColumnName)
        {
            return false;
        }

        #endregion Таблицы - Поля

        #region Транзакции

        /// <summary>
        /// Закрытие транзакции
        /// </summary>
        /// <param name="pCommit">Условие закрытия транзакции. [true] - [Commit], [false] - [RollBack]</param>
        /// <returns>[true] - Транзакция закрыта, иначе - [false]</returns>
        public virtual bool __mTransactionOff(bool pCommit)
        {
            return false;
        }
        /// <summary>
        /// Открытие транзакции
        /// </summary>
        /// <returns>[true] - Транзация создана, иначе - [false]</returns>
        public virtual bool __mTransactionOn()
        {
            return false;
        }
        /// <summary>
        /// Закрытие объединяющей транзакции
        /// </summary>
        /// <param name="pCommit">Условие закрытия транзакции. [true] - [Commit], [false] - [RollBack]</param>
        /// <returns>[true] - Транзакция закрыта, иначе - [false]</returns>
        public virtual bool __mTransactionUnionOff(bool pCommit)
        {
            return false;
        }
        /// <summary>
        /// Создание объединяющей транзакцией
        /// </summary>
        /// <returns>[true] - Транзакция создана, иначе - [false]</returns>
        public virtual bool __mTransactionUnionOn()
        {
            return false;
        }

        #endregion Транзакции

        #region Функции - Ключи

        /// <summary>
        /// Получение Ключа по учетному коду
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pCode">Учетный код</param>
        /// <returns></returns>
        public virtual int __mClueByCode(string pTableName, int pCode)
        {
            return -1;
        }
        /// <summary>
        /// Получение ключа записи по значению поля названия
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldName">Значение поля названия</param>
        /// <returns></returns>
        public virtual int __mClueByName(string pTableName, string pFieldName)
        {
            return -1;
        }
        /// <summary>
        /// Получение идентификатора записи по названию поля опции
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pFieldOptionName">Имя поля опции</param>
        /// <returns>Значение поля идентификатор записи</returns>
        public virtual int __mClueByOption(string pTableName, string pFieldOptionName)
        {
            return -1;
        }
        /// <summary>
        /// Проверка существования идентификатора в таблице
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентияикатоор записи</param>
        /// <returns></returns>
        public virtual bool __mClueExists(string pTableName, int pClue)
        {
            return false;
        }
        /// <summary>
        /// Получение идентификатора последеней вставленной записи в таблицу
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <returns>[<=0] - Запись не найдена, иначе - идентификатор</returns>
        public virtual int __mClueLastInserted(string pTableName)
        {
            return 0;
        }

        #endregion Функции - Идентификаторы

        #region Функции - Названия

        /// <summary>
        /// Получение названия по идентификатору записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор искомой строки</param>
        /// <returns>Значение поля 'Название'</returns>
        public virtual string __mNameByClue(string pTableName, int pClue)
        {
            return "null";
        }
        /// <summary>
        /// Получение названия по идентификатору записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор искомой строки</param>
        /// <returns>[""] - значение не найдено, иначе - значение поля 'Название'</returns>
        public virtual string __mNameByCode(string pTableName, int pClue)
        {
            return "null";
        }
        /// <summary>
        /// Получение названия справочника по названию и значению опции
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pOptionName">Название опции</param>
        /// <returns>Значение поля 'Название' </returns>
        public virtual string __mNameByOption(string pTableName, string pOptionName)
        {
            return "null";
        }
        /// <summary>
        /// Проверка существования названия
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pValue">Значение поля названия</param>
        /// <param name="pClueSkip">Идентификатор исключаемой записи</param>
        /// <returns>[true] - Указанное название уже существует, иначе - [false]</returns>
        public virtual bool __mNameExists(string pTableName, string pValue, int pClueSkip)
        {
            return false;
        }
        /// <summary>
        /// Проверка существования названия
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pValue">Значение поля названия</param>
        /// <param name="pExpressionWhere">Условие проверки названия</param>
        /// <returns>[true] - Указанное название уже существует, иначе - [false]</returns>
        public virtual bool __mNameExists(string pTableName, string pValue, string pExpressionWhere)
        {
            return false;
        }

        #endregion Функции - Названия

        #region Функции - Учетные коды

        /// <summary>
        /// Получение значения поля учетного кода по идентификатору записи
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pClue">Идентификатор записи</param>
        /// <returns>Значение поля учетного кода</returns>
        public virtual int __mCodeByClue(string pTableName, int pClue)
        {
            return -1;
        }
        /// <summary>
        /// Проверка существования учетного кода исключая идентификатор записи указанный в 'pClueSkip'
        /// </summary>
        /// <param name="pTableName">Название таблицы</param>
        /// <param name="pCodeCheck">Проверяемый учетный код</param>
        /// <param name="pClueSkip">Идентификатор записи который нужно исключить из поиска</param>
        /// <returns>[true] - Дублирующийся учетный код найден, иначе - [false]</returns>
        public virtual bool __mCodeExists(string pTableName, int pCodeCheck, int pClueSkip)
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
        public virtual int __mCodeNew(string pTableName, int pClueSkip, CODESNEWTYPES pCodeNewCalculateType, int pValueMinimal = 1, int pValueMaximal = 999999)
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
        public virtual int __mCodeNewGroup(string pTableName, int pClueSkip, CODESNEWTYPES pCodeNewCalculateType, int pValueMinimal, int pValueMaximal, ArrayList pFieldS)
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
        public virtual int __mPositionNew(string pTableName, int pClueSkip, string pWhere)
        {
            return -1;
        }

        #endregion Функция - Позиция в документе

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Псевдоним источника данных
        /// </summary>
        public string __fAlias = "";
        /// <summary>
        /// Строка подключения к источнику данных
        /// </summary>
        public string __fConnectionLine = "";
        /// <summary>
        /// Вид хранения даты
        /// </summary>
        public DATETIMESTORE __fDateTimeStore = DATETIMESTORE.DateTime;
        /// <summary>
        /// Вид источника данных
        /// </summary>
        public DATASOURCETYPES __fDataSourceType;
        /// <summary>
        /// Таблицы источника данных
        /// </summary>
        public List<datUnitModelTable> __fModelTableS = new List<datUnitModelTable>();
        /// <summary>
        /// Список исправлений сделанных в базе данных при сверке с моделю базы данных
        /// </summary>
        public ArrayList __fStructureChanges = new ArrayList();
        /// <summary>
        /// Список названий полей-ссылок в которых допускается нулевое значение
        /// </summary>
        public ArrayList __fTablesListAllowedZero = new ArrayList();

        #region Пользователь

        /// <summary>
        /// Псевдоним пользователя
        /// </summary>
        public string __fUserAlias = "";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int __fUserClue = -1;
        /// <summary>
        /// Учетный код пользователя
        /// </summary>
        public int __fUserCode = -1;
        /// <summary>
        /// Идентификатор роли пользователя
        /// </summary>
        public int __fUserRoleClue = -1;
        /// <summary>
        /// Название роли пользователя
        /// </summary>
        public string __fUserRoleName = "";
        /// <summary>
        /// Пользователь - Администратор
        /// </summary>
        public bool __fUserAdministrator = false;
        /// <summary>
        /// Пользователь - Разработчик
        /// </summary>
        public bool __fUserDesign = false;
        /// <summary>
        /// Пользователь - Кладовщик
        /// </summary>
        public bool __fUserStorage = false;
        /// <summary>
        /// Если пользователь кладовщик, указывается отдел в котором он кладовщик
        /// </summary>
        public int __fUserDepartment = -1;

        #endregion Пользователь

        #region Сервер

        /// <summary>
        /// Название базы данных с расширением (если есть)
        /// </summary>
        public string __fDatabaseName = "";
        /// <summary>
        /// Полный путь к базе данных 
        /// </summary>
        public string __fDatabasePath = "";
        /// <summary>
        /// Работа в режиме не разрывного соедиения 
        /// </summary>
        public bool __fOnLine = false;
        /// <summary>
        /// Название сервера
        /// </summary>
        public string __fServer = "";
        /// <summary>
        /// Имя входа на сервер
        /// </summary>
        public string __fServerLogin = "";
        /// <summary>
        /// Пароль входа на сервер
        /// </summary>
        public string __fServerPassword = "";

        #endregion Сервер

        #endregion Атрибуты

        #region - Скрытые

        /// <summary>
        /// Объект ошибки
        /// </summary>
        protected appUnitError _fError;

        #endregion Скрытые

        #region - Константы

        /// <summary>
        /// Первод каретки
        /// </summary>
        private string CRLF = "\r\n";

        #endregion Константы

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
}
