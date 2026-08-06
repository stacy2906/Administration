//using nlApplication;
//using nlSystem;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using System.Text.RegularExpressions;

//namespace nlcsManual
//{
//    /// <summary>
//    /// Файл cmlManual.cs
//    /// </summary>
//    public class cmlManual
//    {
//        public string __fClassName = "";
//        public string __fSummary = "";
//        public string __fAuthor = "";
//        public string __fVersion = "";
//        public string __fHtmlFileName => __fClassName + ".html";

//        public ArrayList __fMethods = new ArrayList();
//        public ArrayList __fFieldsAndProps = new ArrayList();

//        /// <summary>
//        /// Парсинг одного C# файла и создание его HTML-страницы (Принимает 2 аргумента)
//        /// </summary>
//        /// <param name="pFilePath">Полный путь к .cs файлу</param>
//        /// <param name="pOutputDir">Путь к папке # MANUAL</param>
//        public bool __mManualing(string pFilePath, string pOutputDir)
//        {
//            if (File.Exists(pFilePath) == false)
//                return false;

//            string[] vLines = File.ReadAllLines(pFilePath);
//            string vCurrentSummary = "";

//            foreach (string vLine in vLines)
//            {
//                string vTrimmed = vLine.Trim();

//                if (vTrimmed.StartsWith("///"))
//                {
//                    string vClean = Regex.Replace(vTrimmed, @"///\s*", "");
//                    if (vClean.Contains("<summary>") || vClean.Contains("<remarks>"))
//                    {
//                        vCurrentSummary += Regex.Replace(vClean, @"<[^>]+>", "").Trim() + " ";
//                    }
//                    else if (vClean.Contains("<author>"))
//                    {
//                        __fAuthor = Regex.Replace(vClean, @"<[^>]+>", "").Split(new[] { "//" }, StringSplitOptions.None)[0].Trim();
//                    }
//                    else if (vClean.Contains("<version>"))
//                    {
//                        __fVersion = Regex.Replace(vClean, @"<[^>]+>", "").Split(new[] { "//" }, StringSplitOptions.None)[0].Trim();
//                    }
//                    continue;
//                }

//                Match vMatch = Regex.Match(vTrimmed, @"public\s+(sealed\s+|static\s+|abstract\s+)?class\s+(\w+)");
//                if (vMatch.Success)
//                {
//                    __fClassName = vMatch.Groups[2].Value;
//                    __fSummary = string.IsNullOrEmpty(vCurrentSummary) ? "Описание отсутствует" : vCurrentSummary.Trim();
//                    vCurrentSummary = "";
//                    continue;
//                }

//                if (!string.IsNullOrEmpty(__fClassName))
//                {
//                    if ((vTrimmed.StartsWith("public ") || vTrimmed.StartsWith("protected ")) && vTrimmed.Contains("(") && vTrimmed.Contains(")"))
//                    {
//                        __fMethods.Add(vTrimmed.Replace("{", "").Trim());
//                    }
//                    else if ((vTrimmed.StartsWith("public ") || vTrimmed.StartsWith("protected ")) && (vTrimmed.Contains(";") || vTrimmed.Contains("get")))
//                    {
//                        __fFieldsAndProps.Add(vTrimmed.Replace(";", "").Trim());
//                    }
//                }
//            }

//            if (string.IsNullOrEmpty(__fClassName))
//                return false;

//            StringBuilder vSb = new StringBuilder();
//            vSb.AppendLine("<!DOCTYPE html><html lang=\"ru\"><head><meta charset=\"UTF-8\">");
//            vSb.AppendLine("<title>Класс " + __fClassName + "</title>");
//            vSb.AppendLine("<style>");
//            vSb.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; background: #f8f9fa; }");
//            vSb.AppendLine(".card { background: #fff; padding: 20px; border-radius: 6px; box-shadow: 0 0 8px rgba(0,0,0,0.1); }");
//            vSb.AppendLine("code { font-family: Consolas, monospace; color: #d63384; }");
//            vSb.AppendLine("ul { list-style: none; padding-left: 0; }");
//            vSb.AppendLine("li { background: #f1f3f5; margin: 4px 0; padding: 6px 10px; border-left: 3px solid #0066FF; }");
//            vSb.AppendLine("</style></head><body>");
//            vSb.AppendLine("<div class=\"card\">");
//            vSb.AppendLine("<p><a href=\"index.html\">← Вернуться к списку классов</a></p>");
//            vSb.AppendLine("<h1>Класс: " + __fClassName + "</h1>");
//            vSb.AppendLine("<p><strong>Описание:</strong> " + __fSummary + "</p>");
//            if (!string.IsNullOrEmpty(__fAuthor)) vSb.AppendLine("<p><strong>Автор:</strong> " + __fAuthor + "</p>");
//            if (!string.IsNullOrEmpty(__fVersion)) vSb.AppendLine("<p><strong>Версия:</strong> " + __fVersion + "</p>");

//            vSb.AppendLine("<h2>Поля и Свойства</h2><ul>");
//            foreach (string vField in __fFieldsAndProps) vSb.AppendLine("<li><code>" + vField + "</code></li>");
//            vSb.AppendLine("</ul>");

//            vSb.AppendLine("<h2>Методы</h2><ul>");
//            foreach (string vMethod in __fMethods) vSb.AppendLine("<li><code>" + vMethod + "</code></li>");
//            vSb.AppendLine("</ul></div></body></html>");

//            File.WriteAllText(Path.Combine(pOutputDir, __fHtmlFileName), vSb.ToString(), Encoding.UTF8);
//            return true;
//        }

//        /// <summary>
//        /// Генерация главного файла index.html
//        /// </summary>
//        /// <param name="pOutputDir">Папка размещения # MANUAL</param>
//        /// <param name="pClassList">Список сгенерированных классов</param>
//        public bool __mManualing2(string pOutputDir, ArrayList pClassList)
//        {
//            StringBuilder vSb = new StringBuilder();
//            vSb.AppendLine("<!DOCTYPE html><html lang=\"ru\"><head><meta charset=\"UTF-8\">");
//            vSb.AppendLine("<title>Индекс Документации</title>");
//            vSb.AppendLine("<style>");
//            vSb.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; background: #e9ecef; }");
//            vSb.AppendLine(".container { background: #fff; padding: 20px; border-radius: 8px; }");
//            vSb.AppendLine(".item { padding: 10px; border-bottom: 1px solid #dee2e6; }");
//            vSb.AppendLine("textarea { width: 100%; height: 80px; margin-top: 10px; }");
//            vSb.AppendLine("button { padding: 8px 16px; background: #0066FF; color: white; border: none; border-radius: 4px; cursor: pointer; }");
//            vSb.AppendLine(".note { background: #fff3cd; border-left: 4px solid #ffc107; padding: 8px; margin-top: 5px; }");
//            vSb.AppendLine("</style></head><body>");
//            vSb.AppendLine("<div class=\"container\">");
//            vSb.AppendLine("<h1>Список классов проекта</h1>");

//            foreach (cmlManual vClass in pClassList)
//            {
//                vSb.AppendLine("<div class=\"item\">");
//                vSb.AppendLine("<h3><a href=\"" + vClass.__fHtmlFileName + "\">Класс " + vClass.__fClassName + "</a></h3>");
//                vSb.AppendLine("<p>" + vClass.__fSummary + "</p>");
//                vSb.AppendLine("</div>");
//            }

//            vSb.AppendLine("<hr>");
//            vSb.AppendLine("<h2>Заметки к проекту</h2>");
//            vSb.AppendLine("<div id=\"userNotes\"></div>");
//            vSb.AppendLine("<textarea id=\"noteInput\" placeholder=\"Введите комментарий...\"></textarea><br>");
//            vSb.AppendLine("<button onclick=\"addNote()\">Добавить текст</button>");

//            vSb.AppendLine("<script>");
//            vSb.AppendLine("function addNote() {");
//            vSb.AppendLine("    var text = document.getElementById('noteInput').value;");
//            vSb.AppendLine("    if(text.trim() !== '') {");
//            vSb.AppendLine("        var div = document.createElement('div');");
//            vSb.AppendLine("        div.className = 'note';");
//            vSb.AppendLine("        div.innerHTML = '<strong>' + new Date().toLocaleTimeString() + ':</strong> ' + text;");
//            vSb.AppendLine("        document.getElementById('userNotes').appendChild(div);");
//            vSb.AppendLine("        document.getElementById('noteInput').value = '';");
//            vSb.AppendLine("    }");
//            vSb.AppendLine("}");
//            vSb.AppendLine("</script>");

//            vSb.AppendLine("</div></body></html>");

//            File.WriteAllText(Path.Combine(pOutputDir, "index.html"), vSb.ToString(), Encoding.UTF8);
//            return true;
//        }
//    }
//}