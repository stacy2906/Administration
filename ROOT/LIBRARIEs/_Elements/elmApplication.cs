using nlData;
using System;
using System.ComponentModel;

namespace nlElements
{
    #region = ПЕРЕЧИСЛЕНИЯ

    /// <summary>
    /// Категории цветов используемых приложением
    /// </summary>
    public enum COLORS
    {
        /// <summary>
        /// Цвет текста данных
        /// </summary>
        Data,
        /// <summary>
        /// Цвет фона текста данных
        /// </summary>
        DataBack,
        /// <summary>
        /// Фон не доступных данных
        /// </summary>
        DataBackDisabled,
        /// <summary>
        /// Фон обязательно заполняемых данных
        /// </summary>
        DataBackNecessarily,
        /// <summary>
        /// Цвет формы
        /// </summary>
        FormActive,
        /// <summary>
        /// Цвет не активной формы
        /// </summary>
        FormDeactive,
        /// <summary>
        /// Подсветка записи помеченной для обновления менее X суток назад
        /// </summary>
        GridChange,
        /// <summary>
        /// Цвет обычного текста
        /// </summary>
        Text,
        /// <summary>
        /// Цвет текста - ссылки
        /// </summary>
        TextButton,
        /// <summary>
        /// Цвет текста - заголовка
        /// </summary>
        TextTitle
    }
    /// <summary>
    /// Вид данных отображаемых в ComboBox
    /// </summary>
    public enum COMBOTYPES
    {
        /// <summary>
        /// Логические значения
        /// </summary>
        Bool,
        /// <summary>
        /// Значения загружаются из источника данных и вводяться в ручную
        /// </summary>
        Items
    }
    /// <summary>
    /// Виды объектов правки данных в колонках 'DataGridView'
    /// </summary>
    public enum DATAGRIDCOLUMNTYPE
    {
        /// <summary>
        /// Поле ввода текста
        /// </summary>
        DataGridViewTextBoxColumn,
        /// <summary>
        /// Поле ввода логического значения
        /// </summary>
        DataGridViewCheckBoxColumn,
        /// <summary>
        /// Поле ввода - кнопка
        /// </summary>
        DataGridViewButtonColumn
    }
    /// <summary>
    /// Вид отображения датывремени
    /// </summary>
    public enum DATETIMETYPES
    {
        /// <summary>
        /// Дата
        /// </summary>
        Date,
        /// <summary>
        /// Дата и время
        /// </summary>
        DateTime
    }
    /// <summary>
    /// Категории шрифтов используемых приложеннием
    /// </summary>
    public enum FONTS
    {
        /// <summary>
        /// Шрифт текста данных
        /// </summary>
        Data,
        /// <summary>
        /// Не редактируемый узел дерева
        /// </summary>
        NodeNotEdit,
        /// <summary>
        /// Шрифт текста
        /// </summary>
        Text,
        /// <summary>
        /// Шрифт текста - ссылки
        /// </summary>
        TextButton,
        /// <summary>
        /// Шрифт текста - заголовка
        /// </summary>
        TextTitle
    }
    /// <summary>
    /// Обязательность заполнения поля ввода
    /// </summary>
    public enum FILLTYPES
    {
        /// <summary>
        /// Можно заполнять или не заполнять
        /// </summary>
        None,
        /// <summary>
        /// Данные должны быть введены обязательно
        /// </summary>
        Necessarily
    }
    public enum GRIDCELLTYPE
    {
        /// <summary>
        /// Обычная ячейка
        /// </summary>
        Normal,
        /// <summary>
        /// Цифровая ячейка с двумя знаками после запятой
        /// </summary>
        NumericFractionalTwo,
        /// <summary>
        /// Цифровая ячейка с тремя знаками после запятой
        /// </summary>
        NumericFractionalThree,
    }
    /// <summary>
    /// Вид контролов для изменения данных
    /// </summary>
    public enum CONTROLsOPENEDTYPES
    {
        /// <summary>
        /// Форма для изменения документа
        /// </summary>
        AreaToEditDocument,
        /// <summary>
        /// Форма для изменения записи
        /// </summary>
        AreaToEditRecord,
        /// <summary>
        /// Форма для изменения записи группы
        /// </summary>
        AreaToEditRecordGroup,
        /// <summary>
        /// Форма для изменения документа
        /// </summary>
        FormDocument,
        /// <summary>
        /// Форма для подписи документа
        /// </summary>
        FormDocumentSignature,
        /// <summary>
        /// Форма для правки табличных данных
        /// </summary>
        FormGrid,
        /// <summary>
        /// Форма для правки связывания данных
        /// </summary>
        FormLink,
        /// <summary>
        /// Форма для изменения документа
        /// </summary>
        FormPages,
        /// <summary>
        /// Форма для изменения записи
        /// </summary>
        FormRecord,
        /// <summary>
        /// Форма для изменения строки документа
        /// </summary>

        FormRecordContent,
        /// <summary>
        /// Форма для правки древовидных данных
        /// </summary>
        FormTree
    }
    /// <summary>
    /// Режим использования формы
    /// </summary>
    public enum FORMMODE
    {
        /// <summary>
        /// Форма предназначена для создания данных
        /// </summary>
        ForCreate,
        /// <summary>
        /// Форма предназначена для правки данных
        /// </summary>
        ForEdit,
        /// <summary>
        /// Режим формы не определен
        /// </summary>
        None
    }
    /// <summary>
    /// Вид формы для выбора значений для 'crlInputForm'
    /// </summary>
    public enum FORMSELECTEDTYPES
    {
        /// <summary>
        /// Форма для работы с табличными данными
        /// </summary>
        FormGrid,
        /// <summary>
        /// Форма для работы с папочными данными
        /// </summary>
        FormGridDirectory,
        /// <summary>
        /// Форма для работы с древовидными данными
        /// </summary>
        FormTree
    }
    /// <summary>
    /// Виды программируемых форм
    /// </summary>
    public enum FORMTYPE
    {
        FormFilter,
        FormGrid,
        FormGridAccess,
        FormRecord,
        FormLink,
        FormTree
    }
    /// <summary>
    /// Виды текста на форме
    /// </summary>
    public enum LABELTYPES
    {
        /// <summary>
        /// Надпись - ссылка
        /// </summary>
        Button,
        /// <summary>
        /// Обычный текст
        /// </summary>
        Normal,
        /// <summary>
        /// Заголовок
        /// </summary>
        Title
    }
    /// <summary>
    /// Виды путей
    /// </summary>
    public enum PATHTYPES
    {
        /// <summary>
        /// Файл
        /// </summary>
        File,
        /// <summary>
        /// Папка
        /// </summary>
        Directory,
        /// <summary>
        /// не определено
        /// </summary>
        None
    }
    /// <summary>
    /// Виды привязки компонентов
    /// </summary>
    public enum SCALETYPEs
    {
        /// <summary>
        /// Привязать к форме
        /// </summary>
        Anchor,
        /// <summary>
        /// Фиксированный размер
        /// </summary>
        Fixed
    }
    /// <summary>
    /// Режим работы панели статуса
    /// </summary>
    public enum STATUSPANELTYPEs
    {
        /// <summary>
        /// Отображение текста м процента выполнения задач 
        /// </summary>
        TextAndPercent,
        /// <summary>
        /// Отображение текста и движения прогресса по таймеру
        /// </summary>
        TextAndTimer,
        /// <summary>
        /// Отображение текста по таймеру
        /// </summary>
        TextByTimer,
        /// <summary>
        /// Отображение текста до выполнения метода '__mClear'
        /// </summary>
        Text
    }
    /// <summary>
    /// Привязка размещения текста
    /// </summary>
    public enum TEXTPOSITION
    {
        Center,
        Left,
        None,
        Right,
        Sliding
    }

    #endregion ПЕРЕЧИСЛЕНИЯ

    /// <summary>
    /// Файл elmApplication.cs
    /// </summary>
    /// <remarks>Класс-проекта '_Elements'</remarks>
	/// <conception>Lucasin V.</conception>
	/// <version>2025.12.25 10-40</version>
    public class elmApplication : datApplication
    {
        #region = ПОЛЯ

        #region - Объекты

        /// <summary>
        /// Объект для работы с настройками интерфейса
        /// </summary>
        public static elmInterface __oInterface = new elmInterface();
        /// <summary>
        /// Объект для обработки ошибок приложения
        /// </summary>
        public new static elmErrorsHandler __oErrorsHandler = new elmErrorsHandler();
        /// <summary>
        /// Объект для отображения сообщений пользователю
        /// </summary>
        public new static elmMessages __oMessages = new elmMessages();
        /// <summary>
        /// Объект для работы с путями приложения
        /// </summary>
        public new static elmPathes __oPathes = new elmPathes();

        #endregion Объекты

        #endregion ПОЛЯ
    }
}
