using nlApplication;

namespace nlData
{ 
    #region = ПЕРЕЧИСЛЕНИЯ

    /// <summary>
    /// Порядок расчета нового учетного кода
    /// </summary>
    public enum CODESNEWTYPES
    {
        /// <summary>
        /// Поиском пропущенных кодов
        /// </summary>
        Skiped,
        /// <summary>
        /// Приращением к максимальному коду
        /// </summary>
        Next,
        /// <summary>
        /// Приращение к макисмальному коду с пересчетом по порядку
        /// </summary>
        NextRecalculation
    }
    /// <summary>
    /// Виды источников данных
    /// </summary>
    public enum DATASOURCETYPES
    {
        /// <summary>
        /// Вид источника данных не определен
        /// </summary>
        None,
        /// <summary>
        /// Ms Exel файл
        /// </summary>
        MsExcel,
        /// <summary>
        /// MsSql сервер
        /// </summary>
        MsSql,
        /// <summary>
        /// Oracle
        /// </summary>
        Oracle,
        /// <summary>
        /// PostgreSql
        /// </summary>
        PostgreSql,
        /// <summary>
        /// Sqlite
        /// </summary>
        Sqlite
    }
    /// <summary>
    /// Типы данных
    /// </summary>
    public enum COLUMNSTYPES
    {
        Bigint,
        Binary,
        Bit,
        Char,
        Date,
        Datetime,
        Datetime2,
        Datetimeoffset,
        Decimal,
        Float,
        Geography,
        Geomentry,
        Hierarchyid,
        Image,
        Int,
        Money,
        Nchar,
        Ntext,
        Numeric,
        Nvarchar,
        Real,
        Rowversion, // Уникальный идентификатор (в пределах базы данных). Увеличивается. Не основан на дате и времени
        Smalldatetime,
        Smallint,
        Smallmoney,
        Sql_variant,
        Text,
        Time,
        Timestamp, // Уникальный идентификатор (в пределах базы данных). Увеличивается. Не основан на дате и времени
        Tinyint,
        Uniqueidentifier, // 16-байтовый идентификатор GID
        Varbinary,
        Varchar,
        Varcharmax,
        Xml
    }
    /// <summary>
    /// Вид удаления записи
    /// </summary>
    public enum DELETETYPES
    {
        /// <summary>
        /// Удаление
        /// </summary>
        Delete,
        /// <summary>
        /// Пометка на удаление
        /// </summary>
        Mark
    }
    /// <summary>
    /// Вид хранения данных типа даты в источнике данных
    /// </summary>
    public enum DATETIMESTORE
    {
        /// <summary>
        /// Дата-время
        /// </summary>
        DateTime,
        /// <summary>
        /// BigInt - тики
        /// </summary>
        Ticks
    }
    /// <summary>
    /// Информация о поле таблицы источника данных
    /// </summary>
    public enum FIELDINFO
    {
        /// <summary>
        /// Описание
        /// </summary>
        Description,
        /// <summary>
        /// Разрешение пустых данных
        /// </summary>
        Null,
        /// <summary>
        /// Точность
        /// </summary>
        Precision,
        /// <summary>
        /// Масштаб
        /// </summary>
        Scale,
        /// <summary>
        /// Тип поля
        /// </summary>
        Type
    }
    /// <summary>
    /// Вид триггера для проверки записи
    /// </summary>
    public enum TRIGGERTYPEFORCHANGERECORD
    {
        /// <summary>
        /// Вставка данных
        /// </summary>
        Insert,
        /// <summary>
        /// Обновление данных
        /// </summary>
        Update
    }

    #endregion ПЕРЕЧИСЛЕНИЯ

    /// <summary>
    /// Файл datApplication.cs
    /// </summary>
    /// <remarks>Класс приложения</remarks>
   	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 14-22</version> // Дата-время последней корректировки
    public class datApplication : appApplication
    {
        #region = ПОЛЯ

        #region - Объекты

        /// <summary>
        /// Объект для работы с источником данных
        /// </summary>
        public static datData __oData = new datData();
        /// <summary>
        /// Объект для работы с путями приложения
        /// </summary>
        public new static datPathes __oPathes = new datPathes();

        #endregion Объекты

        #endregion ПОЛЯ
    }
}
