using System.Collections.Generic;

namespace nlcsManual
{
    /// <summary>
    /// Файл cmlModels.cs
    /// </summary>
    /// <remarks>Модели данных, используемые движком документирования ('cmlEngine') для описания
    /// разобранной структуры C# проекта: типы, члены типов, параметры и XML-комментарии</remarks>


    #region = ПАРАМЕТР ЧЛЕНА ТИПА

    /// <summary>
    /// Класс-единица описания параметра метода
    /// </summary>
    public class cmlUnitParam
    {
        /// <summary>Тип параметра (например 'string', 'int', 'List&lt;admEssenceUsr&gt;')</summary>
        public string __fType = "";
        /// <summary>Имя параметра</summary>
        public string __fName = "";
        /// <summary>Значение по умолчанию, если указано (например '""', 'null', '0')</summary>
        public string __fDefault = "";
        /// <summary>Описание параметра, извлечённое из тега &lt;param name="..."&gt;</summary>
        public string __fDescription = "";
    }

    #endregion ПАРАМЕТР ЧЛЕНА ТИПА

    #region = ЧЛЕН ТИПА (метод / свойство / поле / конструктор)

    /// <summary>
    /// Перечисление видов членов типа, документируемых движком
    /// </summary>
    public enum MEMBERKINDS
    {
        /// <summary>Конструктор</summary>
        Constructor,
        /// <summary>Метод (процедура или функция)</summary>
        Method,
        /// <summary>Свойство</summary>
        Property,
        /// <summary>Поле</summary>
        Field,
        /// <summary>Событие</summary>
        Event
    }

    /// <summary>
    /// Класс-единица построчной пометки хода выполнения ('///'-комментарий внутри тела члена, например
    /// '1.T ...') вместе со строкой кода, к которой она относится - без кода сама пометка мало что
    /// объясняет читателю документации (видно только "что" по замыслу, но не "как" реализовано)
    /// </summary>
    public class cmlUnitBodyNote
    {
        /// <summary>Текст пометки (без ведущих '///')</summary>
        public string __fNote = "";
        /// <summary>Строка кода, следующая сразу за пометкой в исходном файле - пусто, если пометка стоит последней перед закрытием тела</summary>
        public string __fCode = "";
    }

    /// <summary>
    /// Класс-единица описания члена типа (метода, свойства, поля, конструктора или события)
    /// </summary>
    public class cmlUnitMember
    {
        /// <summary>Вид члена типа</summary>
        public MEMBERKINDS __fKind = MEMBERKINDS.Method;
        /// <summary>Модификатор доступа ('public', 'private', 'protected', 'internal', 'protected internal')</summary>
        public string __fAccess = "private";
        /// <summary>Дополнительные модификаторы ('static', 'virtual', 'override', 'abstract', 'sealed', 'readonly', 'async')</summary>
        public List<string> __fModifiers = new List<string>();
        /// <summary>Возвращаемый / объявленный тип (для метода - тип результата, для свойства/поля - тип значения)</summary>
        public string __fType = "";
        /// <summary>Имя члена</summary>
        public string __fName = "";
        /// <summary>Список параметров (для методов и конструкторов)</summary>
        public List<cmlUnitParam> __fParamS = new List<cmlUnitParam>();
        /// <summary>Краткое описание члена (тег &lt;summary&gt;)</summary>
        public string __fSummary = "";
        /// <summary>Развёрнутое замечание (тег &lt;remarks&gt;)</summary>
        public string __fRemarks = "";
        /// <summary>Описание возвращаемого значения (тег &lt;returns&gt;)</summary>
        public string __fReturns = "";
        /// <summary>Пример использования (тег &lt;example&gt;)</summary>
        public string __fExample = "";
      
        /// <summary>Пояснение причины исправления (собственный тег проекта &lt;fixed&gt;)</summary>
        public string __fFixed = "";
        /// <summary>Исключения, которые может выбросить член (теги &lt;exception cref="..."&gt;описание&lt;/exception&gt;,
        /// может быть несколько на один член) - каждая строка: "Тип исключения" -&gt; описание</summary>
        public List<KeyValuePair<string, string>> __fExceptionS = new List<KeyValuePair<string, string>>();
        /// <summary>Номер строки исходного файла, с которой начинается объявление члена</summary>
        public int __fLineNumber = 0;
        /// <summary>Признак принадлежности члена интерфейсу (реализация метода интерфейса)</summary>
        public bool __fIsGetOnlyProperty = false;
        /// <summary>Построчные пояснительные комментарии ('///'), обнаруженные внутри тела члена (например
        /// нумерованные пометки хода выполнения вида '1.T ...'/'2.Y ...' - устоявшееся в проекте соглашение),
        /// каждая - вместе со строкой кода, которую она поясняет</summary>
        public List<cmlUnitBodyNote> __fBodyNoteS = new List<cmlUnitBodyNote>();
        /// <summary>Путь вложенных '#region' в исходном файле, внутри которых объявлен член (например
        /// ['МЕТОДЫ', 'Процедуры'] для метода, лежащего внутри '#region = МЕТОДЫ' -&gt; '#region - Процедуры').
        /// Пусто, если член объявлен вне какого-либо '#region'</summary>
        public List<string> __fRegionPath = new List<string>();

        /// <summary>Путь '#region', в виде готовой для показа строки ('МЕТОДЫ &rarr; Процедуры'), либо
        /// пустая строка, если член объявлен вне какого-либо '#region'</summary>
        public string __mRegionLabel()
        {
            return string.Join(" ; ", __fRegionPath);
        }
    }

    #endregion ЧЛЕН ТИПА

    #region = ТИП (класс / интерфейс / структура / перечисление)

    /// <summary>
    /// Перечисление видов документируемых типов
    /// </summary>
    public enum TYPEKINDS
    {
        /// <summary>Класс</summary>
        Class,
        /// <summary>Интерфейс</summary>
        Interface,
        /// <summary>Структура</summary>
        Struct,
        /// <summary>Перечисление</summary>
        Enum
    }

    /// <summary>
    /// Класс-единица описания документируемого типа (класса, интерфейса, структуры или перечисления)
    /// </summary>
    public class cmlUnitType
    {
        /// <summary>Вид типа</summary>
        public TYPEKINDS __fKind = TYPEKINDS.Class;
        /// <summary>Пространство имён, в котором объявлен тип</summary>
        public string __fNamespace = "";
        /// <summary>Имя типа</summary>
        public string __fName = "";
        /// <summary>Модификатор доступа типа</summary>
        public string __fAccess = "public";
        /// <summary>Дополнительные модификаторы типа ('static', 'sealed', 'abstract', 'partial')</summary>
        public List<string> __fModifiers = new List<string>();
        /// <summary>Имя базового класса, если есть (первый элемент списка наследования, не являющийся интерфейсом)</summary>
        public string __fBaseClass = "";
        /// <summary>Список реализуемых интерфейсов (по эвристике - идентификаторы, начинающиеся на 'I' + заглавная буква)</summary>
        public List<string> __fInterfaceS = new List<string>();
        /// <summary>Краткое описание типа (тег &lt;summary&gt;)</summary>
        public string __fSummary = "";
        /// <summary>Развёрнутое замечание (тег &lt;remarks&gt;)</summary>
        public string __fRemarks = "";
        /// <summary>Автор класса (тег &lt;author&gt; либо &lt;conception&gt;)</summary>
        public string __fAuthor = "";
        /// <summary>Версия / дата последней корректировки (тег &lt;version&gt;)</summary>
        public string __fVersion = "";
        /// <summary>Пример использования типа (тег &lt;example&gt;)</summary>
        public string __fExample = "";
        /// <summary>Пояснение причины исправления на уровне типа (собственный тег проекта &lt;fixed&gt;)</summary>
        public string __fFixed = "";
        /// <summary>Полный путь файла, в котором объявлен тип</summary>
        public string __fFilePath = "";
        /// <summary>Путь файла относительно корня документируемого проекта</summary>
        public string __fFilePathRelative = "";
        /// <summary>Название сгенерированного HTML файла (например 'nlAdministration.admEssenceUsr.html')</summary>
        public string __fHtmlFileName = "";
        /// <summary>Список конструкторов</summary>
        public List<cmlUnitMember> __fConstructorS = new List<cmlUnitMember>();
        /// <summary>Список методов</summary>
        public List<cmlUnitMember> __fMethodS = new List<cmlUnitMember>();
        /// <summary>Список свойств</summary>
        public List<cmlUnitMember> __fPropertyS = new List<cmlUnitMember>();
        /// <summary>Список полей</summary>
        public List<cmlUnitMember> __fFieldS = new List<cmlUnitMember>();
        /// <summary>Список событий</summary>
        public List<cmlUnitMember> __fEventS = new List<cmlUnitMember>();

        /// <summary>Полное имя типа вида 'Пространство.Имя'</summary>
        public string __mFullName()
        {
            return __fNamespace.Length > 0 ? __fNamespace + "." + __fName : __fName;
        }
    }

    #endregion ТИП

    #region = РЕЗУЛЬТАТ РАЗБОРА ФАЙЛА

    /// <summary>
    /// Класс-единица результата разбора одного файла *.cs, включая обнаруженные несоответствия документирования
    /// </summary>
    public class cmlUnitParseResult
    {
        /// <summary>Полный путь разобранного файла</summary>
        public string __fFilePath = "";
        /// <summary>Список типов, обнаруженных в файле</summary>
        public List<cmlUnitType> __fTypeS = new List<cmlUnitType>();
        /// <summary>Список протокольных сообщений (недоработки документирования), выявленных при разборе файла</summary>
        public List<string> __fProtocolS = new List<string>();
    }

    #endregion РЕЗУЛЬТАТ РАЗБОРА ФАЙЛА
}