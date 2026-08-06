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
    /// <conception>Lucasin V.</conception>
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
                vLineS = File.ReadAllLines(pFilePath, Encoding.UTF8);
            }
            catch (Exception vException)
            {
                vReturn.__fProtocolS.Add("Не удалось прочитать файл: " + vException.Message);
                return vReturn;
            }

            string vNamespace = "";
            int vBraceDepth = 0;
            int vTypeBraceDepth = -1;
            cmlUnitType vCurrentType = null;
            List<string> vPendingDoc = new List<string>();

            for (int i = 0; i < vLineS.Length; i++)
            {
                string vLine = vLineS[i];
                string vTrim = vLine.Trim();

                /// 1. Накопление строк XML-документации, предшествующих объявлению
                if (vTrim.StartsWith("///"))
                {
                    vPendingDoc.Add(vTrim.Substring(3).Trim());
                    continue;
                }

                /// 2. Пустые строки и обычные комментарии не прерывают накопленный блок документации,
                /// но строки кода (кроме region/атрибутов) - сбрасывают его, если он не был использован
                bool vIsStructural = vTrim.Length == 0
                    || vTrim.StartsWith("//")
                    || vTrim.StartsWith("#region")
                    || vTrim.StartsWith("#endregion")
                    || vTrim.StartsWith("[");

                /// 3. Пространство имён
                Match vMatchNamespace = rNamespace.Match(vLine);
                if (vMatchNamespace.Success)
                {
                    vNamespace = vMatchNamespace.Groups[1].Value;
                }

                /// 4. Объявление типа (класс/интерфейс/структура/перечисление)
                Match vMatchType = rType.Match(vLine);
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
                    mTryParseMember(vLine, i + 1, vCurrentType, vPendingDoc, vReturn.__fProtocolS);
                    vPendingDoc.Clear();
                }
                else if (!vIsStructural && vTrim.Length > 0)
                {
                    /// Строка кода, не являющаяся членом верхнего уровня (тело метода, вложенный блок и т.п.) - документация не относится к ней
                    vPendingDoc.Clear();
                }

                /// 6. Обновление глубины вложенности фигурных скобок (после обработки строки)
                foreach (char vChar in vLine)
                {
                    if (vChar == '{') vBraceDepth++;
                    else if (vChar == '}')
                    {
                        vBraceDepth--;
                        if (vCurrentType != null && vBraceDepth == vTypeBraceDepth)
                        {
                            vCurrentType = null;
                            vTypeBraceDepth = -1;
                        }
                    }
                }
            }

            return vReturn;
        }

        #endregion Процедуры

        #region - Функции закрытые

        /// <summary>
        /// Попытка разобрать строку как объявление члена типа (конструктор/метод/свойство/поле)
        /// </summary>
        private void mTryParseMember(string pLine, int pLineNumber, cmlUnitType pType, List<string> pPendingDoc, List<string> pProtocolS)
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
                mFillModifiersAndAccess(vCtorMatch.Groups["mods"].Value, vMember.__fModifiers, ref vMember.__fAccess);
                mParseParams(vCtorMatch.Groups["params"].Value, vMember.__fParamS);

                string vAuthorUnused, vVersionUnused;
                Dictionary<string, string> vParamDescS;
                mApplyDoc(pPendingDoc, out vMember.__fSummary, out vMember.__fRemarks, out vAuthorUnused, out vVersionUnused, out vMember.__fExample, out vParamDescS);
                mApplyParamDescriptions(vMember.__fParamS, vParamDescS);

                pType.__fConstructorS.Add(vMember);
                return;
            }

            /// 2. Поле (в т.ч. по соглашению проекта - имена вида '__fИмя' или '_fИмя')
            Match vFieldMatch = rField.Match(pLine);
            if (vFieldMatch.Success && !vTrim.Contains("("))
            {
                cmlUnitMember vMember = new cmlUnitMember();
                vMember.__fKind = MEMBERKINDS.Field;
                vMember.__fType = vFieldMatch.Groups["type"].Value.Trim();
                vMember.__fName = vFieldMatch.Groups["name"].Value.Trim();
                vMember.__fLineNumber = pLineNumber;
                mFillModifiersAndAccess(vFieldMatch.Groups["mods"].Value, vMember.__fModifiers, ref vMember.__fAccess);

                string vFieldAuthorUnused, vFieldVersionUnused;
                Dictionary<string, string> vFieldParamDescSUnused;
                mApplyDoc(pPendingDoc, out vMember.__fSummary, out vMember.__fRemarks, out vFieldAuthorUnused, out vFieldVersionUnused, out vMember.__fExample, out vFieldParamDescSUnused);

                pType.__fFieldS.Add(vMember);
                return;
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
                mFillModifiersAndAccess(vMemberMatch.Groups["mods"].Value, vMember.__fModifiers, ref vMember.__fAccess);

                if (vIsMethod)
                    mParseParams(vMemberMatch.Groups["params"].Value, vMember.__fParamS);

                string vMemberAuthorUnused, vMemberVersionUnused;
                Dictionary<string, string> vParamDescS;
                mApplyDoc(pPendingDoc, out vMember.__fSummary, out vMember.__fRemarks, out vMemberAuthorUnused, out vMemberVersionUnused, out vMember.__fExample, out vParamDescS);
                string vReturnsDesc = mExtractTag(pPendingDoc, "returns");
                vMember.__fReturns = vReturnsDesc;
                if (vIsMethod) mApplyParamDescriptions(vMember.__fParamS, vParamDescS);

                /// Проверка на недокументированный публичный член - протоколируется как недоработка
                if (vMember.__fAccess == "public" && string.IsNullOrWhiteSpace(vMember.__fSummary))
                    pProtocolS.Add(string.Format("Строка {0}: публичный член '{1}.{2}' не имеет описания <summary>",
                        pLineNumber, pType.__fName, vMember.__fName));

                if (vIsMethod) pType.__fMethodS.Add(vMember);
                else pType.__fPropertyS.Add(vMember);
                return;
            }
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
        /// Разбор накопленного блока строк XML-документации ('///') на составляющие теги
        /// </summary>
        private void mApplyDoc(List<string> pDocLineS, out string pSummary, out string pRemarks,
            out string pAuthor, out string pVersion, out string pExample, out Dictionary<string, string> pParamDescS)
        {
            string vJoined = string.Join(" ", pDocLineS);

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
            string vJoined = string.Join(" ", pDocLineS);
            Match vMatch = Regex.Match(vJoined, "<" + pTagName + @"[^>]*>(.*?)</" + pTagName + ">", RegexOptions.Singleline);
            return vMatch.Success ? vMatch.Groups[1].Value.Trim() : "";
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
