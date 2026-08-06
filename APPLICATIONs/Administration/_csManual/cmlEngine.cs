using nlApplication;
using nlSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace nlcsManual
{
    /// <summary>
    /// Файл cmlEngine.cs
    /// </summary>
    /// <remarks>Класс-движок документирования C# проекта</remarks>
    /// <adjustment></adjustment>
    /// <conception>Lucasin V.</conception>
    public class cmlEngine
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Главная процедура сканирования и генерации
        /// </summary>
        public int __mDo(string pPathDirectoryProject)
        {
            int vReturn = 0;
            string vFolderPathManual = "";
            ArrayList vClassList = new ArrayList();

            if (Directory.Exists(pPathDirectoryProject) == false)
            {
                appUnitError vError = new appUnitError();
                vError.__fErrorType_ = ERRORSTYPES.User;
                vError.__fProcedure_ = fClassNameFull + "__mDo";
                vError.__mMessageBuild("Путь '{0}' указан не верно", pPathDirectoryProject);
                cmlApplication.__oErrorsHandler.__mShow(vError);
                return -1;
            }

            // 1. Создание каталога # MANUAL
            vFolderPathManual = Path.Combine(pPathDirectoryProject, "# MANUAL");
            if (Directory.Exists(vFolderPathManual) == false)
            {
                Directory.CreateDirectory(vFolderPathManual);
            }

            // 2. Очистка старых файлов
            sstFileSystem oFile = new sstFileSystem();
            List<sstUnitFile> vFileOldS = oFile.__mFilesInDirectory(vFolderPathManual);
            foreach (sstUnitFile vFileUnit in vFileOldS)
            {
                string vPath = Path.Combine(vFileUnit.__fDirectory, vFileUnit.__fName);
                if (File.Exists(vPath))
                {
                    File.Delete(vPath);
                }
            }

            // 3. Сканирование .cs файлов и создание отдельных HTML страниц
            List<sstUnitFile> vAllCsFiles = oFile.__mFilesInDirectory(pPathDirectoryProject);
            foreach (sstUnitFile vFileUnit in vAllCsFiles)
            {
                if (vFileUnit.__fName.EndsWith(".cs") && !vFileUnit.__fDirectory.Contains("# MANUAL"))
                {
                    string vFullFilePath = Path.Combine(vFileUnit.__fDirectory, vFileUnit.__fName);
                    cmlManual vClassDoc = mProcessClassFile(vFullFilePath, vFolderPathManual);

                    if (vClassDoc != null && !string.IsNullOrEmpty(vClassDoc.__fClassName))
                    {
                        vClassList.Add(vClassDoc);
                    }
                }
            }

            // 4. Генерация главного файла index.html
            mGenerateIndexHtml(vFolderPathManual, vClassList);

            return vReturn;
        }

        #region Закрытые методы

        /// <summary>
        /// Анализирует отдельный .cs файл и записывает для него HTML
        /// </summary>
        private cmlManual mProcessClassFile(string pFilePath, string pOutputDir)
        {
            if (!File.Exists(pFilePath)) return null;

            string[] vLines = File.ReadAllLines(pFilePath);
            cmlManual vDoc = new cmlManual();
            string vCurrentSummary = "";

            foreach (string vLine in vLines)
            {
                string vTrimmed = vLine.Trim();

                if (vTrimmed.StartsWith("///"))
                {
                    string vClean = Regex.Replace(vTrimmed, @"///\s*", "");
                    if (vClean.Contains("<summary>") || vClean.Contains("<remarks>"))
                    {
                        vCurrentSummary += Regex.Replace(vClean, @"<[^>]+>", "").Trim() + " ";
                    }
                    continue;
                }

                Match vMatch = Regex.Match(vTrimmed, @"public\s+(sealed\s+|static\s+|abstract\s+)?class\s+(\w+)");
                if (vMatch.Success)
                {
                    vDoc.__fClassName = vMatch.Groups[2].Value;
                    vDoc.__fSummary = string.IsNullOrEmpty(vCurrentSummary) ? "Описание класса отсутствует." : vCurrentSummary.Trim();
                    vCurrentSummary = "";
                    continue;
                }

                if (!string.IsNullOrEmpty(vDoc.__fClassName))
                {
                    if ((vTrimmed.StartsWith("public ") || vTrimmed.StartsWith("protected ")) && vTrimmed.Contains("(") && vTrimmed.Contains(")"))
                    {
                        vDoc.__fMethods.Add(mLineColoring(vTrimmed.Replace("{", "").Trim()));
                    }
                    else if ((vTrimmed.StartsWith("public ") || vTrimmed.StartsWith("protected ")) && (vTrimmed.Contains(";") || vTrimmed.Contains("get")))
                    {
                        vDoc.__fFieldsAndProps.Add(mLineColoring(vTrimmed.Replace(";", "").Trim()));
                    }
                }
            }

            if (string.IsNullOrEmpty(vDoc.__fClassName)) return null;

            // Формирование стилизованного HTML документа для класса
            StringBuilder vSb = new StringBuilder();
            vSb.AppendLine("<!DOCTYPE html><html lang=\"ru\"><head><meta charset=\"UTF-8\">");
            vSb.AppendLine("<title>Класс " + vDoc.__fClassName + "</title>");
            vSb.AppendLine("<style>");
            vSb.AppendLine("body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 30px; background-color: #f4f6f9; color: #333; }");
            vSb.AppendLine(".container { max-width: 900px; margin: 0 auto; background: #fff; padding: 25px; border-radius: 8px; box-shadow: 0 4px 12px rgba(0,0,0,0.08); }");
            vSb.AppendLine(".back-btn { display: inline-block; margin-bottom: 15px; color: #0066FF; text-decoration: none; font-weight: bold; }");
            vSb.AppendLine("h1 { color: #1a252f; border-bottom: 2px solid #0066FF; padding-bottom: 8px; }");
            vSb.AppendLine("h2 { color: #2c3e50; margin-top: 25px; }");
            vSb.AppendLine(".summary-box { background: #eef5ff; border-left: 4px solid #0066FF; padding: 12px 15px; margin: 15px 0; border-radius: 0 4px 4px 0; }");
            vSb.AppendLine("ul { list-style: none; padding-left: 0; }");
            vSb.AppendLine("li { background: #f8f9fa; margin: 6px 0; padding: 10px 14px; border-radius: 4px; border: 1px solid #e9ecef; font-family: Consolas, monospace; }");
            vSb.AppendLine("</style></head><body>");

            vSb.AppendLine("<div class=\"container\">");
            vSb.AppendLine("<a href=\"index.html\" class=\"back-btn\">← Вернуться к списку классов (Index)</a>");
            vSb.AppendLine("<h1>Класс: " + vDoc.__fClassName + "</h1>");
            vSb.AppendLine("<div class=\"summary-box\"><strong>Описание:</strong> " + vDoc.__fSummary + "</div>");

            vSb.AppendLine("<h2>Поля и Свойства</h2>");
            if (vDoc.__fFieldsAndProps.Count > 0)
            {
                vSb.AppendLine("<ul>");
                foreach (string vField in vDoc.__fFieldsAndProps) vSb.AppendLine("<li>" + vField + "</li>");
                vSb.AppendLine("</ul>");
            }
            else vSb.AppendLine("<p><em>Публичные поля и свойства не найдены</em></p>");

            vSb.AppendLine("<h2>Методы</h2>");
            if (vDoc.__fMethods.Count > 0)
            {
                vSb.AppendLine("<ul>");
                foreach (string vMethod in vDoc.__fMethods) vSb.AppendLine("<li>" + vMethod + "</li>");
                vSb.AppendLine("</ul>");
            }
            else vSb.AppendLine("<p><em>Публичные методы не найдены</em></p>");

            vSb.AppendLine("</div></body></html>");

            File.WriteAllText(Path.Combine(pOutputDir, vDoc.__fHtmlFileName), vSb.ToString(), Encoding.UTF8);
            return vDoc;
        }

        /// <summary>
        /// Генерация главного файла index.html
        /// </summary>
        private void mGenerateIndexHtml(string pOutputDir, ArrayList pClassList)
        {
            StringBuilder vSb = new StringBuilder();
            vSb.AppendLine("<!DOCTYPE html><html lang=\"ru\"><head><meta charset=\"UTF-8\">");
            vSb.AppendLine("<title>Индекс Документации Проекта</title>");
            vSb.AppendLine("<style>");
            vSb.AppendLine("body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 30px; background-color: #e9ecef; color: #333; }");
            vSb.AppendLine(".container { max-width: 950px; margin: 0 auto; background: #fff; padding: 30px; border-radius: 10px; box-shadow: 0 4px 15px rgba(0,0,0,0.1); }");
            vSb.AppendLine("h1 { color: #0066FF; margin-top: 0; }");
            vSb.AppendLine(".class-card { background: #f8f9fa; border-left: 5px solid #0066FF; margin: 12px 0; padding: 15px; border-radius: 0 6px 6px 0; }");
            vSb.AppendLine(".class-card h3 { margin: 0 0 6px 0; }");
            vSb.AppendLine(".class-card a { color: #0066FF; text-decoration: none; font-size: 18px; }");
            vSb.AppendLine(".class-card a:hover { text-decoration: underline; }");
            vSb.AppendLine(".notes-section { margin-top: 40px; padding-top: 20px; border-top: 2px solid #dee2e6; }");
            vSb.AppendLine("textarea { width: 100%; height: 90px; margin: 10px 0; padding: 10px; border: 1px solid #ced4da; border-radius: 5px; box-sizing: border-box; }");
            vSb.AppendLine("button { padding: 10px 20px; background: #0066FF; color: white; border: none; border-radius: 5px; cursor: pointer; font-weight: bold; }");
            vSb.AppendLine("button:hover { background: #0052cc; }");
            vSb.AppendLine(".note-item { background: #fff3cd; border-left: 4px solid #ffc107; padding: 10px; margin-top: 8px; border-radius: 4px; }");
            vSb.AppendLine("</style></head><body>");

            vSb.AppendLine("<div class=\"container\">");
            vSb.AppendLine("<h1>Документация Проекта</h1>");
            vSb.AppendLine("<p>Всего задокументировано классов: <strong>" + pClassList.Count + "</strong></p>");

            foreach (cmlManual vClass in pClassList)
            {
                vSb.AppendLine("<div class=\"class-card\">");
                vSb.AppendLine("<h3><a href=\"" + vClass.__fHtmlFileName + "\">Класс " + vClass.__fClassName + "</a></h3>");
                vSb.AppendLine("<p>" + vClass.__fSummary + "</p>");
                vSb.AppendLine("</div>");
            }

            // Интерактивный блок заметок
            vSb.AppendLine("<div class=\"notes-section\">");
            vSb.AppendLine("<h2>Заметки и Комментарии к проекту</h2>");
            vSb.AppendLine("<div id=\"notesContainer\"></div>");
            vSb.AppendLine("<textarea id=\"noteInput\" placeholder=\"Введите ваши замечания...\"></textarea>");
            vSb.AppendLine("<button onclick=\"addNote()\">Сохранить заметку</button>");
            vSb.AppendLine("</div>");

            vSb.AppendLine("<script>");
            vSb.AppendLine("function addNote() {");
            vSb.AppendLine("    var text = document.getElementById('noteInput').value;");
            vSb.AppendLine("    if(text.trim() !== '') {");
            vSb.AppendLine("        var div = document.createElement('div');");
            vSb.AppendLine("        div.className = 'note-item';");
            vSb.AppendLine("        div.innerHTML = '<strong>' + new Date().toLocaleString() + ':</strong> ' + text;");
            vSb.AppendLine("        document.getElementById('notesContainer').appendChild(div);");
            vSb.AppendLine("        document.getElementById('noteInput').value = '';");
            vSb.AppendLine("    }");
            vSb.AppendLine("}");
            vSb.AppendLine("</script>");

            vSb.AppendLine("</div></body></html>");

            File.WriteAllText(Path.Combine(pOutputDir, "index.html"), vSb.ToString(), Encoding.UTF8);
        }

        private string mKeyWord(string pKeyWord)
        {
            switch (pKeyWord.ToLower())
            {
                case "public":
                case "private":
                case "internal":
                case "protected":
                    return "<font color=\"#0066FF\"><b>" + pKeyWord + "</b></font>";
                case "class":
                case "void":
                case "string":
                case "int":
                case "bool":
                case "override":
                case "virtual":
                case "static":
                    return "<font color=\"#0066FF\"><b><i>" + pKeyWord + "</i></b></font>";
                default:
                    return pKeyWord;
            }
        }

        private string mLineColoring(string pLine)
        {
            string vReturn = "";
            foreach (string vWord in appTypeString.__mWordsList(pLine.Trim(), ' '))
            {
                vReturn += mKeyWord(vWord) + " ";
            }
            return vReturn;
        }
        /// <summary>
        /// Протоколоирование недоработок документируемого кода 
        /// </summary>
        /// <param name="pMessage">Протоколированое сообщение</param>
        private void mProtocol(string pFileName, int pFileNumber, string pFileContent, string pErrorCharacter = "")
        {
            string vMessage = pFileName + " " + pFileNumber.ToString() + " " + pFileContent + " " + pErrorCharacter;
            appApplication.__oProtocols.__mCreate(PROTOCOLSTYPES.ApplicationError, "");
            appApplication.__oProtocols.__mRecord(PROTOCOLRECORDSTYPES.Message, vMessage);
        }

        #endregion

        #endregion

        #endregion

        #region = ПОЛЯ

        private string fClassNameFull = "nlcsManual.cmlEngine.";

        #endregion
    }

    /// <summary>
    /// Вспомогательный класс-модель для хранения метаданных класса
    /// </summary>
    public class cmlManual
    {
        public string __fClassName = "";
        public string __fSummary = "";
        public string __fHtmlFileName => __fClassName + ".html";

        public ArrayList __fMethods = new ArrayList();
        public ArrayList __fFieldsAndProps = new ArrayList();
    }
}