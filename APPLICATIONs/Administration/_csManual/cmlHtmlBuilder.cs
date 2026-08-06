using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nlcsManual
{
    /// <summary>
    /// Файл cmlHtmlBuilder.cs
    /// </summary>
    /// <remarks>Класс построения HTML-документации: формирует страницу отдельного типа (класс/интерфейс/
    /// структура/перечисление) и главную страницу 'index.html' со списком типов, группировкой по
    /// пространствам имён и функцией поиска/фильтрации. Оформление согласовано между обоими шаблонами</remarks>
    /// <conception>Lucasin V.</conception>
    public class cmlHtmlBuilder
    {
        #region = ПОЛЯ

        /// <summary>Общая таблица стилей CSS, используемая всеми страницами документации</summary>
        private const string cStyle = @"
:root{
  --bg:#0f1115; --panel:#161923; --panel2:#1c2030; --border:#2a2f42;
  --text:#e6e8ef; --muted:#9aa1b5; --accent:#6ea8fe; --accent2:#8b7bff;
  --ok:#57c785; --warn:#e8b84c; --code:#0b0d13;
}
*{box-sizing:border-box}
body{margin:0;background:var(--bg);color:var(--text);font-family:Segoe UI,Calibri,Arial,sans-serif;font-size:14px;line-height:1.55}
a{color:var(--accent);text-decoration:none}
a:hover{text-decoration:underline}
header.top{position:sticky;top:0;background:var(--panel);border-bottom:1px solid var(--border);padding:12px 20px;display:flex;align-items:center;gap:14px;z-index:10}
header.top .title{font-weight:600;font-size:16px}
header.top .sub{color:var(--muted);font-size:12px}
header.top .spacer{flex:1}
.container{max-width:1100px;margin:0 auto;padding:24px 20px 60px}
.badge{display:inline-block;padding:2px 8px;border-radius:10px;font-size:11px;font-weight:600;border:1px solid var(--border)}
.badge.public{color:var(--ok);border-color:var(--ok)}
.badge.private{color:var(--muted)}
.badge.protected{color:var(--warn);border-color:var(--warn)}
.badge.internal{color:var(--accent);border-color:var(--accent)}
.badge.kind{color:var(--accent2);border-color:var(--accent2);margin-left:6px}
h1{font-size:22px;margin:0 0 4px}
h2{font-size:17px;border-bottom:1px solid var(--border);padding-bottom:6px;margin-top:36px}
h3.member{font-size:14px;background:var(--panel2);border:1px solid var(--border);border-left:3px solid var(--accent);padding:8px 12px;border-radius:4px;margin:14px 0 6px;font-family:Consolas,monospace}
.panel{background:var(--panel);border:1px solid var(--border);border-radius:8px;padding:16px 20px;margin-bottom:16px}
.namespace{color:var(--muted);font-family:Consolas,monospace;font-size:13px}
.summary{margin:6px 0}
.meta-row{display:flex;gap:18px;flex-wrap:wrap;color:var(--muted);font-size:12px;margin:8px 0 0}
.meta-row b{color:var(--text)}
table{border-collapse:collapse;width:100%;margin:6px 0}
table th,table td{text-align:left;padding:6px 10px;border-bottom:1px solid var(--border);vertical-align:top}
table th{color:var(--muted);font-weight:600;font-size:12px;text-transform:uppercase;letter-spacing:.03em}
code, .code{font-family:Consolas,monospace;background:var(--code);border:1px solid var(--border);border-radius:4px;padding:1px 6px}
pre.example{background:var(--code);border:1px solid var(--border);border-radius:6px;padding:12px;overflow:auto;font-family:Consolas,monospace}
.sig{font-family:Consolas,monospace;color:var(--text)}
.type-list{list-style:none;padding:0;margin:0}
.type-list li{padding:6px 0;border-bottom:1px solid var(--border)}
.type-list li:last-child{border-bottom:none}
.type-list .path{color:var(--muted);font-size:11px}
.nsgroup{margin-bottom:22px}
.nsgroup > .nsname{font-family:Consolas,monospace;color:var(--accent2);font-size:13px;margin-bottom:6px;display:block}
input#search{width:100%;padding:10px 14px;border-radius:8px;border:1px solid var(--border);background:var(--panel2);color:var(--text);font-size:14px;margin-bottom:6px}
.stat{color:var(--muted);font-size:12px;margin-bottom:18px}
textarea#notes{width:100%;min-height:110px;background:var(--panel2);border:1px solid var(--border);border-radius:8px;color:var(--text);padding:10px;font-family:Segoe UI,Arial,sans-serif;font-size:13px}
.notes-hint{color:var(--muted);font-size:11px;margin-top:4px}
.empty{color:var(--muted);font-style:italic}
footer{color:var(--muted);font-size:11px;text-align:center;padding:30px 0 10px}
";

        #endregion ПОЛЯ

        #region = МЕТОДЫ

        #region - Функции

        /// <summary>
        /// Построение HTML-страницы документации для одного типа
        /// </summary>
        /// <param name="pType">Описание документируемого типа</param>
        /// <param name="pAllTypeS">Полный список типов проекта (для построения перекрёстных ссылок на базовый класс/интерфейсы)</param>
        /// <returns>Готовый HTML-документ страницы типа</returns>
        public string __mBuildTypePage(cmlUnitType pType, List<cmlUnitType> pAllTypeS)
        {
            StringBuilder vHtml = new StringBuilder();

            vHtml.Append("<!DOCTYPE html><html lang=\"ru\"><head><meta charset=\"utf-8\">");
            vHtml.Append("<title>" + mEsc(pType.__fName) + " - Документация проекта</title>");
            vHtml.Append("<style>" + cStyle + "</style></head><body>");

            vHtml.Append("<header class=\"top\">");
            vHtml.Append("<div><div class=\"title\">" + mEsc(pType.__fName) + "</div><div class=\"sub\">" + mEsc(pType.__fNamespace) + "</div></div>");
            vHtml.Append("<div class=\"spacer\"></div>");
            vHtml.Append("<a href=\"index.html\">&larr; К списку классов</a>");
            vHtml.Append("</header>");

            vHtml.Append("<div class=\"container\">");

            /// Заголовок и краткое описание
            vHtml.Append("<div class=\"panel\">");
            vHtml.Append("<span class=\"badge " + pType.__fAccess.Split(' ')[0] + "\">" + mEsc(pType.__fAccess) + "</span>");
            foreach (string vMod in pType.__fModifiers)
                vHtml.Append(" <span class=\"badge kind\">" + mEsc(vMod) + "</span>");
            vHtml.Append(" <span class=\"badge kind\">" + pType.__fKind + "</span>");
            vHtml.Append("<h1 style=\"margin-top:10px\">" + mEsc(pType.__fName) + "</h1>");
            vHtml.Append("<div class=\"namespace\">namespace " + mEsc(pType.__fNamespace) + "</div>");

            vHtml.Append("<p class=\"summary\">" + (pType.__fSummary.Length > 0 ? mEsc(pType.__fSummary) : "<span class='empty'>Описание отсутствует</span>") + "</p>");
            if (pType.__fRemarks.Length > 0)
                vHtml.Append("<p class=\"summary\">" + mEsc(pType.__fRemarks) + "</p>");

            vHtml.Append("<div class=\"meta-row\">");
            if (pType.__fBaseClass.Length > 0)
                vHtml.Append("<div><b>Базовый класс:</b> " + mLinkToType(pType.__fBaseClass, pAllTypeS) + "</div>");
            if (pType.__fInterfaceS.Count > 0)
                vHtml.Append("<div><b>Интерфейсы:</b> " + string.Join(", ", pType.__fInterfaceS.Select(i => mLinkToType(i, pAllTypeS))) + "</div>");
            if (pType.__fAuthor.Length > 0)
                vHtml.Append("<div><b>Автор:</b> " + mEsc(pType.__fAuthor) + "</div>");
            if (pType.__fVersion.Length > 0)
                vHtml.Append("<div><b>Версия:</b> " + mEsc(pType.__fVersion) + "</div>");
            vHtml.Append("<div><b>Файл:</b> <span class=\"code\">" + mEsc(pType.__fFilePathRelative) + "</span></div>");
            vHtml.Append("</div>");

            /// Список типов-потомков (кто ещё в проекте наследует данный тип)
            List<cmlUnitType> vDescendantS = pAllTypeS.Where(t => t.__fBaseClass == pType.__fName).ToList();
            if (vDescendantS.Count > 0)
            {
                vHtml.Append("<div class=\"meta-row\"><div><b>Наследники в проекте:</b> " +
                    string.Join(", ", vDescendantS.Select(d => "<a href=\"" + d.__fHtmlFileName + "\">" + mEsc(d.__fName) + "</a>")) + "</div></div>");
            }

            if (pType.__fExample.Length > 0)
                vHtml.Append("<pre class=\"example\">" + mEsc(pType.__fExample) + "</pre>");

            vHtml.Append("</div>"); // panel

            if (pType.__fConstructorS.Count > 0)
                mAppendMemberSection(vHtml, "Конструкторы", pType.__fConstructorS, pType.__fName, false);

            if (pType.__fPropertyS.Count > 0)
                mAppendMemberSection(vHtml, "Свойства", pType.__fPropertyS, "", false);

            if (pType.__fMethodS.Count > 0)
                mAppendMemberSection(vHtml, "Методы", pType.__fMethodS, "", true);

            if (pType.__fFieldS.Count > 0)
                mAppendMemberSection(vHtml, "Поля", pType.__fFieldS, "", false);

            if (pType.__fEventS.Count > 0)
                mAppendMemberSection(vHtml, "События", pType.__fEventS, "", false);

            vHtml.Append("<footer>Сгенерировано движком документирования 'cmlEngine' &middot; " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "</footer>");
            vHtml.Append("</div></body></html>");

            return vHtml.ToString();
        }

        /// <summary>
        /// Построение главной страницы документации 'index.html' со списком всех типов проекта
        /// </summary>
        /// <param name="pTypeS">Список всех документируемых типов проекта</param>
        /// <param name="pProjectName">Название документируемого проекта (заголовок страницы)</param>
        /// <param name="pProtocolCount">Количество протокольных сообщений (недоработок документирования), выявленных при разборе</param>
        /// <returns>Готовый HTML-документ главной страницы</returns>
        public string __mBuildIndexPage(List<cmlUnitType> pTypeS, string pProjectName, int pProtocolCount)
        {
            StringBuilder vHtml = new StringBuilder();

            vHtml.Append("<!DOCTYPE html><html lang=\"ru\"><head><meta charset=\"utf-8\">");
            vHtml.Append("<title>" + mEsc(pProjectName) + " - Документация</title>");
            vHtml.Append("<style>" + cStyle + "</style></head><body>");

            vHtml.Append("<header class=\"top\"><div class=\"title\">" + mEsc(pProjectName) + " &mdash; Документация проекта</div></header>");
            vHtml.Append("<div class=\"container\">");

            /// Блок редактируемого произвольного текста (сохраняется в localStorage браузера)
            vHtml.Append("<div class=\"panel\">");
            vHtml.Append("<h2 style=\"margin-top:0;border:none\">Описание проекта</h2>");
            vHtml.Append("<textarea id=\"notes\" placeholder=\"Введите произвольное описание проекта, заметки для команды и т.п. Текст сохраняется локально в браузере.\"></textarea>");
            vHtml.Append("<div class=\"notes-hint\">Текст сохраняется автоматически в этом браузере (localStorage) и не пересоздаётся при повторной генерации документации.</div>");
            vHtml.Append("</div>");

            int vNamespaceCount = pTypeS.Select(t => t.__fNamespace).Distinct().Count();
            vHtml.Append("<div class=\"stat\">Всего типов: <b style=\"color:var(--text)\">" + pTypeS.Count + "</b> &middot; пространств имён: <b style=\"color:var(--text)\">" +
                vNamespaceCount + "</b>" + (pProtocolCount > 0 ? " &middot; <span style=\"color:var(--warn)\">незадокументированных членов: " + pProtocolCount + "</span> (см. Protocols.txt)" : "") + "</div>");

            vHtml.Append("<input id=\"search\" type=\"text\" placeholder=\"Поиск по названию класса, пространству имён или описанию...\" oninput=\"filterClasses()\">");

            /// Группировка по пространству имён
            var vGroupS = pTypeS.OrderBy(t => t.__fNamespace).ThenBy(t => t.__fName)
                .GroupBy(t => t.__fNamespace);

            foreach (var vGroup in vGroupS)
            {
                vHtml.Append("<div class=\"nsgroup\" data-ns=\"" + mEsc(vGroup.Key.ToLower()) + "\">");
                vHtml.Append("<span class=\"nsname\">" + mEsc(vGroup.Key) + " (" + vGroup.Count() + ")</span>");
                vHtml.Append("<ul class=\"type-list\">");
                foreach (cmlUnitType vType in vGroup)
                {
                    string vSearchBlob = (vType.__fName + " " + vType.__fNamespace + " " + vType.__fSummary).ToLower();
                    vHtml.Append("<li class=\"class-item\" data-search=\"" + mEsc(vSearchBlob) + "\">");
                    vHtml.Append("<span class=\"badge kind\">" + vType.__fKind + "</span> ");
                    vHtml.Append("<a href=\"" + vType.__fHtmlFileName + "\">" + mEsc(vType.__fName) + "</a>");
                    if (vType.__fSummary.Length > 0)
                        vHtml.Append(" &mdash; " + mEsc(vType.__fSummary));
                    vHtml.Append("<div class=\"path\">" + mEsc(vType.__fFilePathRelative) + "</div>");
                    vHtml.Append("</li>");
                }
                vHtml.Append("</ul></div>");
            }

            vHtml.Append("<p id=\"noResults\" class=\"empty\" style=\"display:none\">Ничего не найдено.</p>");

            vHtml.Append("<footer>Сгенерировано движком документирования 'cmlEngine' &middot; " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "</footer>");
            vHtml.Append("</div>");

            /// Скрипт поиска/фильтрации и сохранения заметок
            vHtml.Append(@"<script>
function filterClasses(){
  var q = document.getElementById('search').value.toLowerCase().trim();
  var groups = document.querySelectorAll('.nsgroup');
  var anyVisible = false;
  groups.forEach(function(g){
    var items = g.querySelectorAll('.class-item');
    var groupHasVisible = false;
    items.forEach(function(it){
      var match = q === '' || it.getAttribute('data-search').indexOf(q) !== -1;
      it.style.display = match ? '' : 'none';
      if (match) groupHasVisible = true;
    });
    g.style.display = groupHasVisible ? '' : 'none';
    if (groupHasVisible) anyVisible = true;
  });
  document.getElementById('noResults').style.display = anyVisible ? 'none' : '';
}
(function(){
  var notes = document.getElementById('notes');
  var saved = localStorage.getItem('cmlManualNotes');
  if (saved) notes.value = saved;
  notes.addEventListener('input', function(){
    localStorage.setItem('cmlManualNotes', notes.value);
  });
})();
</script>");

            vHtml.Append("</body></html>");
            return vHtml.ToString();
        }

        #endregion Функции

        #region - Функции закрытые

        /// <summary>
        /// Добавление раздела членов типа (методы/свойства/поля/конструкторы/события) на страницу
        /// </summary>
        private void mAppendMemberSection(StringBuilder pHtml, string pTitle, List<cmlUnitMember> pMemberS, string pCtorName, bool pShowReturns)
        {
            pHtml.Append("<h2>" + pTitle + "</h2>");
            foreach (cmlUnitMember vMember in pMemberS.OrderByDescending(m => m.__fAccess == "public").ThenBy(m => m.__fName))
            {
                string vSignature = mBuildSignature(vMember, pCtorName);
                pHtml.Append("<h3 class=\"member\">" + mEsc(vSignature) + "</h3>");

                pHtml.Append("<span class=\"badge " + vMember.__fAccess.Split(' ')[0] + "\">" + mEsc(vMember.__fAccess) + "</span>");
                foreach (string vMod in vMember.__fModifiers)
                    pHtml.Append(" <span class=\"badge kind\">" + mEsc(vMod) + "</span>");

                if (vMember.__fSummary.Length > 0)
                    pHtml.Append("<p class=\"summary\">" + mEsc(vMember.__fSummary) + "</p>");
                else
                    pHtml.Append("<p class=\"summary empty\">Описание отсутствует</p>");

                if (vMember.__fRemarks.Length > 0)
                    pHtml.Append("<p class=\"summary\">" + mEsc(vMember.__fRemarks) + "</p>");

                if (vMember.__fParamS.Count > 0)
                {
                    pHtml.Append("<table><tr><th>Параметр</th><th>Тип</th><th>По умолчанию</th><th>Описание</th></tr>");
                    foreach (cmlUnitParam vParam in vMember.__fParamS)
                    {
                        pHtml.Append("<tr><td><code>" + mEsc(vParam.__fName) + "</code></td><td><code>" + mEsc(vParam.__fType) + "</code></td>" +
                            "<td>" + (vParam.__fDefault.Length > 0 ? "<code>" + mEsc(vParam.__fDefault) + "</code>" : "-") + "</td>" +
                            "<td>" + (vParam.__fDescription.Length > 0 ? mEsc(vParam.__fDescription) : "<span class='empty'>-</span>") + "</td></tr>");
                    }
                    pHtml.Append("</table>");
                }

                if (pShowReturns && vMember.__fType != "void" && vMember.__fType.Length > 0)
                {
                    pHtml.Append("<div class=\"meta-row\"><div><b>Возвращает (" + mEsc(vMember.__fType) + "):</b> " +
                        (vMember.__fReturns.Length > 0 ? mEsc(vMember.__fReturns) : "<span class='empty'>не описано</span>") + "</div></div>");
                }

                if (vMember.__fExample.Length > 0)
                    pHtml.Append("<pre class=\"example\">" + mEsc(vMember.__fExample) + "</pre>");
            }
        }

        /// <summary>
        /// Построение текстовой сигнатуры члена типа для заголовка блока документации
        /// </summary>
        private string mBuildSignature(cmlUnitMember pMember, string pCtorName)
        {
            string vParams = string.Join(", ", pMember.__fParamS.Select(p =>
                p.__fType + " " + p.__fName + (p.__fDefault.Length > 0 ? " = " + p.__fDefault : "")));

            if (pMember.__fKind == MEMBERKINDS.Constructor)
                return pMember.__fName + "(" + vParams + ")";
            if (pMember.__fKind == MEMBERKINDS.Method)
                return (pMember.__fType.Length > 0 ? pMember.__fType + " " : "") + pMember.__fName + "(" + vParams + ")";
            if (pMember.__fKind == MEMBERKINDS.Property)
                return pMember.__fType + " " + pMember.__fName + " { get; set; }";
            return pMember.__fType + " " + pMember.__fName;
        }

        /// <summary>
        /// Построение ссылки на страницу типа проекта по имени, если такой тип задокументирован; иначе - обычный текст
        /// </summary>
        private string mLinkToType(string pTypeName, List<cmlUnitType> pAllTypeS)
        {
            cmlUnitType vFound = pAllTypeS.FirstOrDefault(t => t.__fName == pTypeName);
            return vFound != null
                ? "<a href=\"" + vFound.__fHtmlFileName + "\">" + mEsc(pTypeName) + "</a>"
                : mEsc(pTypeName);
        }

        /// <summary>
        /// Экранирование текста для безопасной вставки в HTML
        /// </summary>
        private string mEsc(string pText)
        {
            if (string.IsNullOrEmpty(pText)) return "";
            return pText.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
        }

        #endregion Функции закрытые

        #endregion МЕТОДЫ
    }
}
