using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace nlcsManual
{
    /// <summary>
    /// Файл cmlSourceParser.cs
    /// </summary>
    /// <remarks>Класс разбора исходного файла *.cs: извлекает пространство имён, объявления типов
    /// (классы/интерфейсы/структуры/перечисления), их членов (конструкторы/методы/свойства/поля/события)
    /// и сопутствующие XML-комментарии документирования. Разбор построен на построчном сканировании с
    /// учётом глубины фигурных скобок, без использования компилятора Roslyn, чтобы оставаться совместимым
    /// с целевым фреймворком проекта ('appFileIni.cs' и соседние классы библиотеки)</remarks>

    public class cmlSourceParser
    {
        #region = ПОЛЯ

        #region - Регулярные выражения

        private static readonly Regex rNamespace = new Regex(@"^\s*namespace\s+([A-Za-z0-9_.]+)", RegexOptions.Compiled);

        private static readonly Regex rType = new Regex(
            @"^\s*(?<mods>(?:(?:public|private|protected|internal|static|sealed|abstract|partial)\s+)+)" +
            @"(?<kind>class|interface|struct|enum)\s+" +
            @"(?<name>[A-Za-z_][A-Za-z0-9_]*)\s*(?<generic><[^>]*>)?" +
            @"(?:\s*:\s*(?<bases>[^{]+))?\s*\{?\s*$", RegexOptions.Compiled);

        private static readonly Regex rMember = new Regex(
            @"^\s*(?<mods>(?:(?:public|private|protected|internal|static|virtual|override|abstract|sealed|readonly|async|new|extern)\s+)+)" +
            @"(?<type>[A-Za-z_][A-Za-z0-9_<>\[\],. ?]*?)\s+" +
            @"(?<name>[A-Za-z_][A-Za-z0-9_]*)\s*" +
            @"(?<paren>\((?<params>[^)]*)\))?\s*" +
            @"(?<body>\{|=>|;)?\s*$", RegexOptions.Compiled);

        private static readonly Regex rField = new Regex(
            @"^\s*(?<mods>(?:(?:public|private|protected|internal|static|readonly|const)\s+)+)" +
            @"(?<type>[A-Za-z_][A-Za-z0-9_<>\[\],. ?]*?)\s+" +
            @"(?<name>__?f[A-Za-z0-9_]*|[A-Za-z_][A-Za-z0-9_]*)\s*" +
            @"(=.*)?;\s*$", RegexOptions.Compiled);

        #endregion Регулярные выражения

        #endregion ПОЛЯ

        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Разбор одного файла исходного кода *.cs
        /// </summary>
        /// <param name="pFilePath">Полный путь разбираемого файла</param>
        /// <returns>Результат разбора: список найденных типов и протокольные сообщения</returns>
        public cmlUnitParseResult __mParseFile(string pFilePath)
        {
            cmlUnitParseResult vReturn = new cmlUnitParseResult();
            vReturn.__fFilePath = pFilePath;

            string[] vLineS;
            try
            {
                vLineS = mReadAllLinesAutoEncoding(pFilePath);
            }
            catch (Exception vException)
            {
                vReturn.__fProtocolS.Add("Не удалось прочитать файл: " + vException.Message);
                return vReturn;
            }

            string vNamespace = "";
            int vBraceDepth = 0;
            int vTypeBraceDepth = -1;
            bool vInBlockComment = false; // Состояние '/* ... */' на границе строк (может открыться на одной строке, закрыться на другой)
            bool vInVerbatimString = false; // Состояние '@"..."' на границе строк (см. <fixed> у 'mScanLine' - раньше не отслеживалось между строками вообще)
            cmlUnitType vCurrentType = null;
            List<string> vPendingDoc = new List<string>();
            cmlUnitMember vCurrentMember = null; // Член, чьё тело сейчас разбирается (для захвата внутренних '///'-пометок хода выполнения)
            int vMemberBraceDepth = -1;
            cmlUnitBodyNote vPendingBodyNote = null; // Последняя пометка хода выполнения, ещё не "закрытая" строкой кода
            bool vPendingBodyNoteConsumed = false; // [true] - после этой пометки уже встретилась строка кода - следующий '///' должен начать НОВУЮ пометку, а не продолжать эту (раньше для этого проверялась 'vPendingBodyNote.__fCode.Length == 0' - убрано вместе с самим кодом, см. <fixed> ниже)
            List<string> vRegionStack = new List<string>(); // Путь вложенных '#region' на текущей строке (для группировки членов на странице документации)

            for (int i = 0; i < vLineS.Length; i++)
            {
                string vLine = vLineS[i];
                string vTrim = vLine.Trim();

                /// 1. Накопление строк XML-документации, предшествующих объявлению
                if (vTrim.StartsWith("///"))
                {
                    string vDocText = vTrim.Substring(3).Trim();

                    /// 1.Y Внутри тела текущего члена (глубже уровня его объявления) - это не XML-документация
                    /// следующего члена, а построчная пометка хода выполнения (например '1.T ...') - сохраняется
                    /// в самом члене, а не в общем накопителе 'vPendingDoc'
                    if (vCurrentMember != null && vBraceDepth > vMemberBraceDepth && vDocText.Length > 0)
                    {

                        bool vIsNewStepMarker = Regex.IsMatch(vDocText, @"^\d+(\.[A-Za-zА-Яа-яЁё])?\.?\s");
                        if (vPendingBodyNote != null && vPendingBodyNoteConsumed == false && vIsNewStepMarker == false)
                        {
                            vPendingBodyNote.__fNote += " " + vDocText;
                        }
                        else
                        {
                            vPendingBodyNote = new cmlUnitBodyNote { __fNote = vDocText };
                            vPendingBodyNoteConsumed = false;
                            vCurrentMember.__fBodyNoteS.Add(vPendingBodyNote);
                        }
                    }
                    else
                        vPendingDoc.Add(vDocText);

                    continue;
                }

                /// 2. Пустые строки, обычные комментарии и одиночная открывающая скобка (когда '{' у метода
                /// стоит на своей отдельной строке - частый в проекте стиль форматирования) не прерывают
                /// накопленный блок документации и не считаются "новым членом", но обычные строки кода
                /// (кроме region/атрибутов) - сбрасывают его, если он не был использован.
                bool vIsStructural = vTrim.Length == 0
                    || vTrim.StartsWith("//")
                    || vTrim.StartsWith("#region")
                    || vTrim.StartsWith("#endregion")
                    || vTrim.StartsWith("[")
                    || vTrim == "{";

                /// 2.R Учёт вложенности '#region'/'#endregion'
                if (vTrim.StartsWith("#region"))
                {
                    string vRegionName = vTrim.Substring("#region".Length).Trim();
                    vRegionName = Regex.Replace(vRegionName, @"^[=\-*]\s*", "");
                    vRegionStack.Add(vRegionName);
                }
                else if (vTrim.StartsWith("#endregion"))
                {
                    if (vRegionStack.Count > 0)
                        vRegionStack.RemoveAt(vRegionStack.Count - 1);
                }

                /// 3. Пространство имён
                Match vMatchNamespace = rNamespace.Match(vLine);
                if (vMatchNamespace.Success)
                {
                    vNamespace = vMatchNamespace.Groups[1].Value;
                }


                string vLogicalLine = vLine;
                List<string> vLogicalConsumedLineS = new List<string> { vLine };
                if (vIsStructural == false && vTrim.Length > 0)
                {
                    bool vTempInBlockComment = vInBlockComment;
                    bool vTempInVerbatimString = vInVerbatimString;
                    int vParenDepth;
                    bool vEndsOnContinuation;
                    mScanLine(vLine, ref vTempInBlockComment, ref vTempInVerbatimString, out int vUnusedBraceDelta1, out vParenDepth, out vEndsOnContinuation);

                    int vLookaheadIndex = i;
                    while ((vParenDepth > 0 || vEndsOnContinuation == true) && vLookaheadIndex + 1 < vLineS.Length)
                    {
                        vLookaheadIndex++;
                        string vNextLine = vLineS[vLookaheadIndex];
                        vLogicalLine = vLogicalLine + " " + vNextLine.Trim();
                        vLogicalConsumedLineS.Add(vNextLine);

                        int vNextParenDelta;
                        mScanLine(vNextLine, ref vTempInBlockComment, ref vTempInVerbatimString, out int vUnusedBraceDelta2, out vNextParenDelta, out vEndsOnContinuation);
                        vParenDepth += vNextParenDelta;
                    }
                }

                /// 4. Объявление типа (класс/интерфейс/структура/перечисление)
                Match vMatchType = rType.Match(vLogicalLine);
                if (vMatchType.Success && vCurrentType == null)
                {
                    cmlUnitType vType = new cmlUnitType();
                    vType.__fNamespace = vNamespace;
                    vType.__fName = vMatchType.Groups["name"].Value;
                    vType.__fFilePath = pFilePath;

                    string vKind = vMatchType.Groups["kind"].Value;
                    vType.__fKind = vKind == "class" ? TYPEKINDS.Class
                                   : vKind == "interface" ? TYPEKINDS.Interface
                                   : vKind == "struct" ? TYPEKINDS.Struct
                                   : TYPEKINDS.Enum;

                    mFillModifiersAndAccess(vMatchType.Groups["mods"].Value, vType.__fModifiers, ref vType.__fAccess);

                    if (vMatchType.Groups["bases"].Success)
                    {
                        foreach (string vBase in vMatchType.Groups["bases"].Value.Split(','))
                        {
                            string vBaseName = vBase.Trim();
                            if (vBaseName.Length == 0) continue;
                            /// Эвристика: интерфейсы по соглашению именуются 'IИмя'
                            if (vBaseName.Length > 1 && vBaseName[0] == 'I' && char.IsUpper(vBaseName[1]))
                                vType.__fInterfaceS.Add(vBaseName);
                            else
                                vType.__fBaseClass = vBaseName;
                        }
                    }

                    Dictionary<string, string> vTypeParamDescSUnused;
                    mApplyDoc(vPendingDoc, out vType.__fSummary, out vType.__fRemarks, out vType.__fAuthor,
                        out vType.__fVersion, out vType.__fExample, out vTypeParamDescSUnused);
                    vType.__fFixed = mExtractTag(vPendingDoc, "fixed");

                    vCurrentType = vType;
                    vTypeBraceDepth = vBraceDepth;
                    vPendingDoc.Clear();
                    vReturn.__fTypeS.Add(vType);

                    /// Проверка полноты документирования типа
                    if (string.IsNullOrWhiteSpace(vType.__fSummary))
                        vReturn.__fProtocolS.Add(string.Format("Строка {0}: тип '{1}' не имеет описания <summary>", i + 1, vType.__fName));
                }
                /// 5. Члены типа (только на уровень ниже открывающей скобки самого типа)
                else if (vCurrentType != null && vBraceDepth == vTypeBraceDepth + 1 && !vIsStructural)
                {
                    vCurrentMember = mTryParseMember(vLogicalLine, i + 1, vCurrentType, vPendingDoc, vReturn.__fProtocolS, vRegionStack);
                    vMemberBraceDepth = vBraceDepth; // Тело члена начнётся на следующем уровне вложенности
                    vPendingBodyNote = null; // Новый член - пометка предыдущего к нему не относится
                    vPendingBodyNoteConsumed = false;
                    vPendingDoc.Clear();
                }
                else if (!vIsStructural && vTrim.Length > 0)
                {
                    /// Строка кода, не являющаяся членом верхнего уровня (тело метода, вложенный блок и т.п.) - документация не относится к ней
                    vPendingDoc.Clear();

                   
                    if (vPendingBodyNote != null)
                        vPendingBodyNoteConsumed = true;
                }


                foreach (string vConsumedLine in vLogicalConsumedLineS)
                {
                    mScanLine(vConsumedLine, ref vInBlockComment, ref vInVerbatimString, out int vBraceDelta, out int vUnusedParenDelta, out bool vUnusedEndsOnContinuation);
                    vBraceDepth += vBraceDelta;

                    if (vCurrentType != null && vBraceDepth <= vTypeBraceDepth && vConsumedLine.Contains("}"))
                    {
                        vCurrentType = null;
                        vTypeBraceDepth = -1;
                    }
                }

                /// Курсор основного цикла продвигается сразу до последней "поглощённой" в логическую
                /// строку физической строки - иначе строки-продолжения были бы разобраны ещё раз заново
                i += vLogicalConsumedLineS.Count - 1;
            }

            return vReturn;
        }

        /// <summary>
        /// Разбор одной физической строки с учётом строковых/символьных литералов и комментариев
        /// (однострочных '//' и блочных '/* ... */', в т.ч. переходящих через границу строк) - считает
        /// только символы РЕАЛЬНОГО кода, а не содержимое литералов, чтобы буквальные '{'/'}'/'('/')'
        /// внутри строковых констант (например регэксп-паттернов) не искажали разбор структуры файла
        /// </summary>
        /// <param name="pLine">Разбираемая физическая строка</param>
        /// <param name="pInBlockComment">Состояние "внутри блочного комментария" на входе - обновляется на выходе</param>
        /// <param name="pInVerbatimString">Состояние "внутри многострочного '@\"...\"'" на входе - обновляется на выходе</param>
        /// <param name="pBraceDelta">Чистое изменение глубины фигурных скобок ('{' минус '}'), только для символов реального кода</param>
        /// <param name="pParenDelta">Чистое изменение глубины круглых скобок ('(' минус ')'), только для символов реального кода</param>
        /// <param name="pEndsOnContinuation">[true] - строка (без учёта хвостовых литералов/комментариев) заканчивается конкатенирующим '+' - вероятное продолжение объявления на следующей строке</param>
      
        private void mScanLine(string pLine, ref bool pInBlockComment, ref bool pInVerbatimString, out int pBraceDelta, out int pParenDelta, out bool pEndsOnContinuation)
        {
            pBraceDelta = 0;
            pParenDelta = 0;
            pEndsOnContinuation = false;

            bool vInVerbatimString = pInVerbatimString; // Продолжение многострочного '@"..."' с предыдущей строки
            bool vInString = vInVerbatimString;
            bool vInChar = false;
            char vLastCodeChar = '\0';

            for (int j = 0; j < pLine.Length; j++)
            {
                char vChar = pLine[j];
                char vNext = j + 1 < pLine.Length ? pLine[j + 1] : '\0';

                if (pInBlockComment == true)
                {
                    if (vChar == '*' && vNext == '/') { pInBlockComment = false; j++; }
                    continue;
                }
                if (vInString == true)
                {
                    if (vInVerbatimString == true)
                    {
                        if (vChar == '"' && vNext == '"') { j++; continue; } // '""' - экранированная кавычка внутри '@"..."'
                        if (vChar == '"') { vInString = false; vInVerbatimString = false; }
                    }
                    else
                    {
                        if (vChar == '\\') { j++; continue; } // Экранированный символ - следующий символ не анализируется
                        if (vChar == '"') vInString = false;
                    }
                    continue;
                }
                if (vInChar == true)
                {
                    if (vChar == '\\') { j++; continue; }
                    if (vChar == '\'') vInChar = false;
                    continue;
                }

                if (vChar == '/' && vNext == '/') break; // Однострочный комментарий - остаток строки не код
                if (vChar == '/' && vNext == '*') { pInBlockComment = true; j++; continue; }
                if (vChar == '@' && vNext == '"') { vInString = true; vInVerbatimString = true; j++; vLastCodeChar = '\0'; continue; }
                if (vChar == '"') { vInString = true; vInVerbatimString = false; vLastCodeChar = '\0'; continue; }
                if (vChar == '\'') { vInChar = true; vLastCodeChar = '\0'; continue; }

                if (vChar == '{') pBraceDelta++;
                else if (vChar == '}') pBraceDelta--;
                else if (vChar == '(') pParenDelta++;
                else if (vChar == ')') pParenDelta--;

                if (char.IsWhiteSpace(vChar) == false)
                    vLastCodeChar = vChar;
            }

            pInVerbatimString = vInVerbatimString; // Сохраняем на случай, если многострочный '@"..."' не закрылся и на этой строке
            pEndsOnContinuation = vLastCodeChar == '+' || vLastCodeChar == ',';
        }

        /// <summary>
        /// Чтение файла с автоматическим определением кодировки (UTF-8 с BOM, либо Windows-1251
        /// без BOM) - часть исходных файлов проекта исторически сохранена в Windows-1251, и попытка
        /// прочитать их как UTF-8 приводила к искажённым символам ('кракозябрам') в готовой документации
        /// </summary>
        /// <param name="pFilePath">Путь читаемого файла</param>
        /// <returns>Массив строк файла в исходной кодировке</returns>
        private string[] mReadAllLinesAutoEncoding(string pFilePath)
        {
            byte[] vBytes = File.ReadAllBytes(pFilePath);

            /// 1 Файл начинается с BOM UTF-8 - однозначно UTF-8
            if (vBytes.Length >= 3 && vBytes[0] == 0xEF && vBytes[1] == 0xBB && vBytes[2] == 0xBF)
                return Encoding.UTF8.GetString(vBytes).Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            /// 2 BOM отсутствует - проверка, является ли содержимое корректным UTF-8 без искажений
            string vAsUtf8 = Encoding.UTF8.GetString(vBytes);
            bool vHasReplacementChar = vAsUtf8.IndexOf('\uFFFD') >= 0;

            /// 2 При наличии символов замены ('\uFFFD') содержимое не является валидным UTF-8 -
            /// файл читается как Windows-1251 (исторический формат части файлов проекта)
            string vResult = vHasReplacementChar ? Encoding.GetEncoding(1251).GetString(vBytes) : vAsUtf8;

            return vResult.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        #endregion Процедуры

        #region - Функции закрытые

        /// <summary>
        /// Попытка разобрать строку как объявление члена типа (конструктор/метод/свойство/поле)
        /// </summary>
        /// <param name="pLine">Разбираемая строка исходного файла</param>
        /// <param name="pLineNumber">Номер строки в исходном файле (для протокольных сообщений)</param>
        /// <param name="pType">Тип, которому принадлежит разбираемый член</param>
        /// <param name="pPendingDoc">Накопленный блок строк XML-документации, предшествующий члену</param>
        /// <param name="pProtocolS">Список протокольных сообщений, пополняемый при обнаружении недоработок документирования</param>
        /// <param name="pRegionPath">Путь вложенных '#region' на текущей строке (см. 'cmlUnitMember.__fRegionPath')</param>
        /// <returns>Разобранный член типа, [null] - строка не распознана как объявление члена</returns>
        private cmlUnitMember mTryParseMember(string pLine, int pLineNumber, cmlUnitType pType, List<string> pPendingDoc, List<string> pProtocolS, List<string> pRegionPath)
        {
            string vTrim = pLine.Trim();

            /// 1. Конструктор: имя совпадает с именем типа
            Regex vCtorRegex = new Regex(@"^\s*(?<mods>(?:(?:public|private|protected|internal|static)\s+)+)" +
                Regex.Escape(pType.__fName) + @"\s*\((?<params>[^)]*)\)");
            Match vCtorMatch = vCtorRegex.Match(pLine);
            if (vCtorMatch.Success)
            {
                cmlUnitMember vMember = new cmlUnitMember();
                vMember.__fKind = MEMBERKINDS.Constructor;
                vMember.__fName = pType.__fName;
                vMember.__fType = "";
                vMember.__fLineNumber = pLineNumber;
                vMember.__fRegionPath = new List<string>(pRegionPath);
                mFillModifiersAndAccess(vCtorMatch.Groups["mods"].Value, vMember.__fModifiers, ref vMember.__fAccess);
                mParseParams(vCtorMatch.Groups["params"].Value, vMember.__fParamS);

                string vAuthorUnused, vVersionUnused;
                Dictionary<string, string> vParamDescS;
                mApplyDoc(pPendingDoc, out vMember.__fSummary, out vMember.__fRemarks, out vAuthorUnused, out vVersionUnused, out vMember.__fExample, out vParamDescS);
                vMember.__fFixed = mExtractTag(pPendingDoc, "fixed");
                vMember.__fExceptionS = mExtractExceptions(pPendingDoc);
                mApplyParamDescriptions(vMember.__fParamS, vParamDescS);

                pType.__fConstructorS.Add(vMember);
                return vMember;
            }

            /// 2. Поле (в т.ч. по соглашению проекта - имена вида '__fИмя' или '_fИмя')
            Match vFieldMatch = rField.Match(pLine);


            if (vFieldMatch.Success)
            {
                cmlUnitMember vMember = new cmlUnitMember();
                vMember.__fKind = MEMBERKINDS.Field;
                vMember.__fType = vFieldMatch.Groups["type"].Value.Trim();
                vMember.__fName = vFieldMatch.Groups["name"].Value.Trim();
                vMember.__fLineNumber = pLineNumber;
                vMember.__fRegionPath = new List<string>(pRegionPath);
                mFillModifiersAndAccess(vFieldMatch.Groups["mods"].Value, vMember.__fModifiers, ref vMember.__fAccess);

                string vFieldAuthorUnused, vFieldVersionUnused;
                Dictionary<string, string> vFieldParamDescSUnused;
                mApplyDoc(pPendingDoc, out vMember.__fSummary, out vMember.__fRemarks, out vFieldAuthorUnused, out vFieldVersionUnused, out vMember.__fExample, out vFieldParamDescSUnused);
                vMember.__fFixed = mExtractTag(pPendingDoc, "fixed");
                vMember.__fExceptionS = mExtractExceptions(pPendingDoc);

                pType.__fFieldS.Add(vMember);
                return vMember;
            }

            /// 3. Метод или свойство: общий шаблон 'модификаторы Тип Имя(...)' либо 'модификаторы Тип Имя'
            Match vMemberMatch = rMember.Match(pLine);
            if (vMemberMatch.Success)
            {
                bool vIsMethod = vMemberMatch.Groups["paren"].Success;
                cmlUnitMember vMember = new cmlUnitMember();
                vMember.__fKind = vIsMethod ? MEMBERKINDS.Method : MEMBERKINDS.Property;
                vMember.__fType = vMemberMatch.Groups["type"].Value.Trim();
                vMember.__fName = vMemberMatch.Groups["name"].Value.Trim();
                vMember.__fLineNumber = pLineNumber;
                vMember.__fRegionPath = new List<string>(pRegionPath);
                mFillModifiersAndAccess(vMemberMatch.Groups["mods"].Value, vMember.__fModifiers, ref vMember.__fAccess);

                if (vIsMethod)
                    mParseParams(vMemberMatch.Groups["params"].Value, vMember.__fParamS);

                string vMemberAuthorUnused, vMemberVersionUnused;
                Dictionary<string, string> vParamDescS;
                mApplyDoc(pPendingDoc, out vMember.__fSummary, out vMember.__fRemarks, out vMemberAuthorUnused, out vMemberVersionUnused, out vMember.__fExample, out vParamDescS);
                string vReturnsDesc = mExtractTag(pPendingDoc, "returns");
                vMember.__fReturns = vReturnsDesc;
                vMember.__fFixed = mExtractTag(pPendingDoc, "fixed");
                vMember.__fExceptionS = mExtractExceptions(pPendingDoc);
                if (vIsMethod) mApplyParamDescriptions(vMember.__fParamS, vParamDescS);

                /// Проверка на недокументированный публичный член - протоколируется как недоработка
                if (vMember.__fAccess == "public" && string.IsNullOrWhiteSpace(vMember.__fSummary))
                    pProtocolS.Add(string.Format("Строка {0}: публичный член '{1}.{2}' не имеет описания <summary>",
                        pLineNumber, pType.__fName, vMember.__fName));

                if (vIsMethod) pType.__fMethodS.Add(vMember);
                else pType.__fPropertyS.Add(vMember);
                return vMember;
            }

            return null;
        }

        /// <summary>
        /// Разбор строки-модификаторов на список модификаторов и модификатор доступа
        /// </summary>
        private void mFillModifiersAndAccess(string pModsRaw, List<string> pModifiers, ref string pAccess)
        {
            string[] vAccessWordS = { "public", "private", "protected", "internal" };
            List<string> vWordS = new List<string>(pModsRaw.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            /// 'protected internal' обрабатывается как единый модификатор доступа
            if (vWordS.Contains("protected") && vWordS.Contains("internal"))
            {
                pAccess = "protected internal";
                vWordS.Remove("protected");
                vWordS.Remove("internal");
            }
            else
            {
                foreach (string vAccessWord in vAccessWordS)
                {
                    if (vWordS.Contains(vAccessWord))
                    {
                        pAccess = vAccessWord;
                        vWordS.Remove(vAccessWord);
                        break;
                    }
                }
            }
            pModifiers.AddRange(vWordS);
        }

        /// <summary>
        /// Разбор списка параметров метода/конструктора из строки объявления
        /// </summary>
        private void mParseParams(string pParamsRaw, List<cmlUnitParam> pParamS)
        {
            if (string.IsNullOrWhiteSpace(pParamsRaw)) return;

            foreach (string vParamRaw in mSplitTopLevel(pParamsRaw, ','))
            {
                string vParam = vParamRaw.Trim();
                if (vParam.Length == 0) continue;

                cmlUnitParam vParamUnit = new cmlUnitParam();

                string vDefault = "";
                int vEqIndex = vParam.IndexOf('=');
                if (vEqIndex >= 0)
                {
                    vDefault = vParam.Substring(vEqIndex + 1).Trim();
                    vParam = vParam.Substring(0, vEqIndex).Trim();
                }
                vParamUnit.__fDefault = vDefault;

                /// Удаление модификаторов параметра ('ref', 'out', 'params', '[CallerLineNumber]' и т.п.)
                vParam = Regex.Replace(vParam, @"\[[^\]]*\]", "").Trim();
                vParam = Regex.Replace(vParam, @"^(ref|out|params|this)\s+", "");

                int vLastSpace = vParam.LastIndexOf(' ');
                if (vLastSpace > 0)
                {
                    vParamUnit.__fType = vParam.Substring(0, vLastSpace).Trim();
                    vParamUnit.__fName = vParam.Substring(vLastSpace + 1).Trim();
                }
                else
                {
                    vParamUnit.__fName = vParam;
                }

                pParamS.Add(vParamUnit);
            }
        }

        /// <summary>
        /// Разделение строки по разделителю верхнего уровня без учёта вложенных '&lt;...&gt;' (обобщённые типы)
        /// </summary>
        private List<string> mSplitTopLevel(string pExpression, char pSeparator)
        {
            List<string> vReturn = new List<string>();
            int vDepth = 0;
            int vStart = 0;
            for (int i = 0; i < pExpression.Length; i++)
            {
                char vChar = pExpression[i];
                if (vChar == '<') vDepth++;
                else if (vChar == '>') vDepth--;
                else if (vChar == pSeparator && vDepth == 0)
                {
                    vReturn.Add(pExpression.Substring(vStart, i - vStart));
                    vStart = i + 1;
                }
            }
            vReturn.Add(pExpression.Substring(vStart));
            return vReturn;
        }


        /// <summary>
        /// Склейка накопленных строк документации в одну строку, с сохранением абзацев (пустая ///-строка
        /// внутри блока - разделитель абзацев, помечается символом '\u2029')
        /// </summary>
        private string mJoinDocLines(List<string> pDocLineS)
        {
            StringBuilder vBuilder = new StringBuilder();
            bool vLastWasBlank = true; // Подавляет пустые строки в самом начале блока
            foreach (string vLine in pDocLineS)
            {
                if (vLine.Length == 0)
                {
                    if (vLastWasBlank == false)
                        vBuilder.Append('\u2029');
                    vLastWasBlank = true;
                }
                else
                {
                    if (vBuilder.Length > 0 && vLastWasBlank == false)
                        vBuilder.Append(' ');
                    vBuilder.Append(vLine);
                    vLastWasBlank = false;
                }
            }
            return vBuilder.ToString().Trim('\u2029');
        }

        /// <summary>
        /// Разбор накопленного блока строк XML-документации ('///') на составляющие теги
        /// </summary>
        private void mApplyDoc(List<string> pDocLineS, out string pSummary, out string pRemarks,
            out string pAuthor, out string pVersion, out string pExample, out Dictionary<string, string> pParamDescS)
        {
            string vJoined = mJoinDocLines(pDocLineS);

            pSummary = mExtractTag(pDocLineS, "summary");
            pRemarks = mExtractTag(pDocLineS, "remarks");
            pAuthor = mExtractTag(pDocLineS, "author");
            if (string.IsNullOrEmpty(pAuthor)) pAuthor = mExtractTag(pDocLineS, "conception");
            pVersion = mExtractTag(pDocLineS, "version");
            pExample = mExtractTag(pDocLineS, "example");

            pParamDescS = new Dictionary<string, string>();
            foreach (Match vMatch in Regex.Matches(vJoined, @"<param\s+name=""([^""]+)""\s*>(.*?)</param>", RegexOptions.Singleline))
            {
                string vName = vMatch.Groups[1].Value.Trim();
                if (!pParamDescS.ContainsKey(vName))
                    pParamDescS[vName] = vMatch.Groups[2].Value.Trim();
            }
        }

        /// <summary>
        /// Извлечение содержимого одиночного XML-тега из накопленного блока документации
        /// </summary>
        private string mExtractTag(List<string> pDocLineS, string pTagName)
        {
            string vJoined = mJoinDocLines(pDocLineS);
            Match vMatch = Regex.Match(vJoined, "<" + pTagName + @"[^>]*>(.*?)</" + pTagName + ">", RegexOptions.Singleline);
            return vMatch.Success ? vMatch.Groups[1].Value.Trim() : "";
        }


        /// <summary>
        /// Извлечение ВСЕХ тегов &lt;exception&gt; из накопленного блока документации (у одного члена их
        /// может быть несколько - на каждый вид исключения свой тег)
        /// </summary>
        private List<KeyValuePair<string, string>> mExtractExceptions(List<string> pDocLineS)
        {
            List<KeyValuePair<string, string>> vReturn = new List<KeyValuePair<string, string>>();
            string vJoined = mJoinDocLines(pDocLineS);
            foreach (Match vMatch in Regex.Matches(vJoined, @"<exception\s+cref=""([^""]+)""\s*>(.*?)</exception>", RegexOptions.Singleline))
            {
                string vType = vMatch.Groups[1].Value.Trim().TrimStart('T', ':'); // 'cref="T:System.ArgumentException"' или просто 'System.ArgumentException'
                vReturn.Add(new KeyValuePair<string, string>(vType, vMatch.Groups[2].Value.Trim()));
            }
            return vReturn;
        }

        /// <summary>
        /// Сопоставление описаний параметров (из тегов &lt;param&gt;) со списком разобранных параметров метода
        /// </summary>
        private void mApplyParamDescriptions(List<cmlUnitParam> pParamS, Dictionary<string, string> pParamDescS)
        {
            foreach (cmlUnitParam vParam in pParamS)
            {
                if (pParamDescS.ContainsKey(vParam.__fName))
                    vParam.__fDescription = pParamDescS[vParam.__fName];
            }
        }

        #endregion Функции закрытые

        #endregion МЕТОДЫ
    }
}