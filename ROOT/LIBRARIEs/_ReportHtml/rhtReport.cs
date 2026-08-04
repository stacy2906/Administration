using nlApplication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace nlReportHtml
{
    /// <summary>
    /// Файл rhtReport.cs
    /// </summary>
    /// <remarks>Класс отчета</remarks>
    public class rhtReport
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public rhtReport()
        {
            __mObjectAssembly();
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Объект

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected void __mObjectAssembly()
        {
            Type vType = this.GetType();
            fClassNameFull = vType.Namespace + "." + vType.Name + ".";

            return;
        }

        #endregion Объект

        #region - Процедуры

        #region Отчет

        /// <summary>
        /// Создание нового отчета
        /// </summary>
        public void __mCreate()
        {
            __fFilePath = appApplication.__oPathes.__mFileTemp("htm");
            fTimeCreate = DateTime.Now;
            _fRowsList.Clear();
        }
        /// <summary>
        /// Создание файла отчета
        /// </summary>
        public void __mFile()
        {
            appFileText vFileText = new appFileText(); // Объект для работы с текстовыми файлами
            string vSpan = ""; // Содержание команды для объединения ячеек
            string vBody = ""; // Содержание файла отчета
            /// Удаление файла, если он уже существует
            if (File.Exists(__fFilePath) == true)
                File.Delete(__fFilePath);

            #region /// Создание заголовка отчета

            vBody = "<HTML>\n";
            vBody = vBody + "<HEAD>\n";
            vBody = vBody + "<META HTTP-EQUIV=\"Expires\" CONTENT=\"" + fTimeCreate.DayOfWeek.ToString().Substring(0, 3) + " ," + fTimeCreate.Day.ToString() + " " + fTimeCreate.Month.ToString() + " " + fTimeCreate.Year.ToString() + " " + fTimeCreate.Hour.ToString() + ":" + fTimeCreate.Minute.ToString() + "GMT\">\n";
            vBody = vBody + "<META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; CHARSET=Windows-1251\">\n";
            vBody = vBody + "<META HTTP-EQUIV=\"Content-Language\" CONTENT=\"ru\">\n";
            if (__fTitle.Length > 0)
            {
                vBody = vBody + "<TITLE>" + __fTitle.Trim() + "</TITLE>\n";
            }
            vBody = vBody + "<link rel = \"stylesheet\" href = \"reports.css\">";

            //if (_User.Length > 0)
            //{
            //    vFileText._mWriteToEnd(_fFilePath, "<META NAME=\"user\" CONTENT=\"" + _User.Trim() + "\">");
            //}
            vBody = vBody + "<META NAME=\"copyright\" CONTENT=\"&copy; " + appApplication.__fTradeMark_ + '-' + fTimeCreate.Year.ToString() + ' ' + appApplication.__fTradeMark_ + "\">\n";
            vBody = vBody + "</HEAD>\n";

            //.ADD_LINE('<BODY '+IIF(VARTYPE(.REPORT(3))='C', 'TOPMARGIN="'+.REPORT(3)+'" ', '')+IIF(VARTYPE(.REPORT(4))='C', 'LEFTMARGIN="'+.REPORT(4)+'" ', '')+IIF(VARTYPE(.REPORT(5))='C', 'BOTTOMMARGIN="'+.REPORT(5)+'" ', '')+IIF(VARTYPE(.REPORT(6))='C', 'RIGHTMARGIN="'+.REPORT(6)+'" ', '')+IIF(VARTYPE(.REPORT(7))='C', 'MARGINHEIGHT="'+.REPORT(7)+'" ', '')+IIF(VARTYPE(.REPORT(8))='C', 'MARGINWIDTH="'+.REPORT(8)+'" ', '')+IIF(VARTYPE(.REPORT(9))='C', 'SCROLL="'+.REPORT(9)+'" ', '')+IIF(VARTYPE(.REPORT(10))='C', 'BGPROPERTIES="'+.REPORT(10)+'" ', '')+IIF(VARTYPE(.REPORT(11))='C', 'BACKGROUND="'+.REPORT(11)+'" ', '')+IIF(VARTYPE(.REPORT(12))='C', 'BGCOLOR="'+.REPORT(12)+'" ', '')+IIF(VARTYPE(.REPORT(13))='C', 'TEXT="'+.REPORT(13)+'" ', '')+IIF(VARTYPE(.REPORT(19))='C', 'LINK="'+.REPORT(19)+'" ', '')+IIF(VARTYPE(.REPORT(20))='C', 'ALINK="'+.REPORT(20)+'" ', '')+IIF(VARTYPE(.REPORT(21))='C', 'VLINK="'+.REPORT(21)+'" ', '')+'>')
            vBody = vBody + "<BODY text=\"#" + __fForeColor + "\" bgcolor=\"" + __fBackColor + "\">\n";

            vBody = vBody + "<TABLE Cols=\"" + __fColumnsCountInReport.ToString() + "\">\n";

            #endregion Создание заголовка отчета

            #region // Заполнение отчета данными

            foreach (rhtUnitRow vRow in _fRowsList)
            {
                if (vRow.__fHide == true)
                    continue;

                vBody = vBody + "<TR";
                /// Для ячейки определен класс стиля
                if (vRow.__fClasses.Length > 0)
                {
                    vBody = vBody + " class=\"" + vRow.__fClasses + "\">\n";
                }
                /// Для ячейки не определен класс стиля
                else
                    vBody = vBody + ">\n";


                foreach (rhtUnitCell vCell in vRow._fCellsList)
                {
                    // Возвращаемый результат
                    string vReturn = "&nbsp;";

                    if (vCell.__fLine == true)
                    {
                        vBody = vBody + "<TD colspan=\"" + __fColumnsCountInReport + "\">" + vCell.__fValue + "</TD>\n";
                        goto BeginNewRow;
                    }
                    else
                    {
                        if (vCell.__fBorderColor.Length == 0)
                            vCell.__fBorderColor = vRow.__fBorderColor;

                        if (vCell.__fImage.Length > 0)
                        {
                            #region Вывод изображения

                            vReturn = "<TD";

                            if (vCell.__fAlign.Length != 0)
                                vReturn = vReturn + " align=\"" + vCell.__fAlign + "\"";

                            if (vCell.__fSpanColumn.Length != 0)
                            {
                                switch (vCell.__fSpanColumn)
                                {
                                    case "max":
                                        vSpan = " colspan=\"" + __fColumnsCountInReport.ToString() + "\"";
                                        break;
                                    case "end":
                                        vSpan = " colspan=\"" + (__fColumnsCountInReport - fCurrentRow._fCellsList.Count).ToString() + "\"";
                                        break;
                                    default:
                                        vSpan = " colspan=\"" + vCell.__fSpanColumn + "\"";
                                        break;
                                }
                                vReturn = vReturn + vSpan;
                            }
                            else
                            {
                                if (vRow.__fSpanColumn.Length != 0)
                                {
                                    switch (vRow.__fSpanColumn)
                                    {
                                        case "max":
                                            vSpan = " colspan=\"" + __fColumnsCountInReport.ToString() + "\"";
                                            break;
                                        case "end":
                                            vSpan = " colspan=\"" + (__fColumnsCountInReport - fCurrentRow._fCellsList.Count).ToString() + "\"";
                                            break;
                                        default:
                                            vSpan = " colspan=\"" + vRow.__fSpanColumn + "\"";
                                            break;
                                    }
                                    vReturn = vReturn + vSpan;
                                }
                            }
                            vSpan = "";
                            if (vCell.__fSpanRow.Length != 0)
                            {
                                switch (vCell.__fSpanRow)
                                {
                                    case "max":
                                        vSpan = " rowspan=\"" + __fColumnsCountInReport.ToString() + "\"";
                                        break;
                                    case "end":
                                        vSpan = " rowspan=\"" + (__fColumnsCountInReport - fCurrentRow._fCellsList.Count).ToString() + "\"";
                                        break;
                                    default:
                                        vSpan = " rowspan=\"" + vCell.__fSpanColumn + "\"";
                                        break;
                                }
                                vReturn = vReturn + vSpan;
                            }
                            else
                            {
                                if (vRow.__fSpanRow.Length != 0)
                                {
                                    switch (vRow.__fSpanColumn)
                                    {
                                        case "max":
                                            vSpan = " rowspan=\"" + __fColumnsCountInReport.ToString() + "\"";
                                            break;
                                        case "end":
                                            vSpan = " rowspan=\"" + (__fColumnsCountInReport - fCurrentRow._fCellsList.Count).ToString() + "\"";
                                            break;
                                        default:
                                            vSpan = " rowspan=\"" + vRow.__fSpanColumn + "\"";
                                            break;
                                    }
                                    vReturn = vReturn + vSpan;
                                }
                            }

                            vReturn = vReturn + "><img src=\"file:" + vCell.__fImage + "\"";

                            if (vCell.__fHeight.Length != 0)
                                vReturn = vReturn + " height=" + vCell.__fHeight;

                            if (vCell.__fWidth.Length != 0)
                                vReturn = vReturn + " width=" + vCell.__fWidth;
                            vReturn = vReturn + "></TD>\n";
                            vBody = vBody + vReturn;

                            #endregion Вывод изображения
                        }
                        else
                        {
                            #region /// Вывод данных

                            // Возвращаемый результат
                            if (vCell.__fValue == null)
                                vReturn = "";
                            else
                                if (!(vCell.__fZero == false & vCell.__fValue.ToString() == "0"))

                                if (vCell.__fLinkFile.Trim().Length != 0)
                                    vReturn = "<a href=\"" + vCell.__fLinkFile.Trim() + "\">" + vCell.__fValue.ToString() + "</a>";
                                else
                                    vReturn = vCell.__fValue.ToString();
                            // Тэг ячейки
                            string vAttributeStart = "<TD";
                            /// Для ячейки определен класс стиля
                            if (vCell.__fClasses.Length > 0)
                            {
                                vAttributeStart = vAttributeStart + " class=\"" + vCell.__fClasses + "\"";
                            }
                            // Тэг шрифта
                            string vAttributeFont = "<FONT";
                            // 
                            string vrBord = "";

                            if (vCell.__fColorBack.Length != 0)
                                vAttributeStart = vAttributeStart + " bgcolor=\"#" + vCell.__fColorBack + "\"";
                            else
                                if (vRow.__fColorBack.Length != 0)
                                vAttributeStart = vAttributeStart + " bgcolor=\"#" + vRow.__fColorBack + "\"";

                            if (vCell.__fAlign.Trim().Length != 0)
                                if (vCell.__fAlign.Trim().Length > 1)
                                    vAttributeStart = vAttributeStart + " align=\"" + vCell.__fAlign + "\"";
                                else
                                    switch (vCell.__fAlign.Trim())
                                    {
                                        case "l":
                                            vAttributeStart = vAttributeStart + " align=\"left\"";
                                            break;
                                        case "c":
                                            vAttributeStart = vAttributeStart + " align=\"center\"";
                                            break;
                                        case "r":
                                            vAttributeStart = vAttributeStart + " align=\"right\"";
                                            break;
                                        case "j":
                                            vAttributeStart = vAttributeStart + " align=\"justify\"";
                                            break;
                                    }
                            else
                                if (vRow.__fAlign.Length != 0)
                                if (vCell.__fAlign.Trim().Length > 1)
                                    vAttributeStart = vAttributeStart + " align=\"" + vCell.__fAlign + "\"";
                                else
                                    switch (vCell.__fAlign.Trim())
                                    {
                                        case "l":
                                            vAttributeStart = vAttributeStart + " align=\"left\"";
                                            break;
                                        case "c":
                                            vAttributeStart = vAttributeStart + " align=\"center\"";
                                            break;
                                        case "r":
                                            vAttributeStart = vAttributeStart + " align=\"right\"";
                                            break;
                                        case "j":
                                            vAttributeStart = vAttributeStart + " align=\"justify\"";
                                            break;
                                    }
                            if (vCell.__fValign.Length != 0)
                                vAttributeStart = vAttributeStart + " valign=\"" + vCell.__fValign + "\"";
                            else
                                if (vRow.__fValign.Length != 0)
                                vAttributeStart = vAttributeStart + " valign=\"" + vRow.__fValign + "\"";

                            if (vCell.__fSpanColumn.Length != 0)
                            {
                                switch (vCell.__fSpanColumn)
                                {
                                    case "max":
                                        vSpan = " colspan=\"" + __fColumnsCountInReport.ToString() + "\"";
                                        break;
                                    case "end":
                                        vSpan = " colspan=\"" + (__fColumnsCountInReport - fCurrentRow._fCellsList.Count).ToString() + "\"";
                                        break;
                                    default:
                                        vSpan = " colspan=\"" + vCell.__fSpanColumn + "\"";
                                        break;
                                }
                                vAttributeStart = vAttributeStart + vSpan;
                            }
                            else
                            {
                                if (vRow.__fSpanColumn.Length != 0)
                                {
                                    switch (vRow.__fSpanColumn)
                                    {
                                        case "max":
                                            vSpan = " colspan=\"" + __fColumnsCountInReport.ToString() + "\"";
                                            break;
                                        case "end":
                                            vSpan = " colspan=\"" + (__fColumnsCountInReport - fCurrentRow._fCellsList.Count).ToString() + "\"";
                                            break;
                                        default:
                                            vSpan = " colspan=\"" + vRow.__fSpanColumn + "\"";
                                            break;
                                    }
                                    vAttributeStart = vAttributeStart + vSpan;
                                }
                            }
                            if (vCell.__fSpanRow.Length != 0)
                            {
                                vSpan = " rowspan=\"" + vCell.__fSpanRow + "\"";
                                vAttributeStart = vAttributeStart + vSpan;
                            }
                            else
                            {
                                if (vRow.__fSpanRow.Length != 0)
                                {
                                    vSpan = " rowspan=\"" + vRow.__fSpanRow + "\"";
                                    vAttributeStart = vAttributeStart + vSpan;
                                }
                            }

                            if (vCell.__fHeight.Length != 0)
                                vAttributeStart = vAttributeStart + " height=\"" + vCell.__fHeight + "\"";
                            else
                                if (vRow.__fHeight.Length != 0)
                                vAttributeStart = vAttributeStart + " height=\"" + vRow.__fHeight + "\"";

                            if (vCell.__fWidth.Length != 0)
                                vAttributeStart = vAttributeStart + " width=\"" + vCell.__fWidth + "\"";
                            else
                                if (vRow.__fWidth.Length != 0)
                                vAttributeStart = vAttributeStart + " width=\"" + vRow.__fWidth + "\"";

                            if (vCell.__fBorder.Length == 0 & vRow.__fBorder.Length != 0)
                            {
                                vCell.__fBorder = vRow.__fBorder;
                                vCell.__fBorderColor = vRow.__fBorderColor;
                            }
                            /// Рисование рамки ячейки
                            for (int vAmount = 0; vAmount < vCell.__fBorder.ToCharArray().Length; vAmount++)
                            {
                                string vString = vCell.__fBorder.Substring(vAmount, 1);
                                switch (vString)
                                {
                                    case "l":
                                        vrBord = vrBord + " border-left-style: solid; border-left-width: " + vCell.__fBorderSize + "px; border-collapse: collapse;";
                                        break;
                                    case "t":
                                        vrBord = vrBord + " border-top-style: solid; border-top-width: " + vCell.__fBorderSize + "px; border-collapse: collapse;";
                                        break;
                                    case "r":
                                        vrBord = vrBord + " border-right-style: solid; border-right-width: " + vCell.__fBorderSize + "px; border-collapse: collapse;";
                                        break;
                                    case "b":
                                        vrBord = vrBord + " border-bottom-style: solid; border-bottom-width: " + vCell.__fBorderSize + "px; border-collapse: collapse;";
                                        break;
                                }
                            }
                            if (vrBord.Trim().Length > 5)
                                vAttributeStart = vAttributeStart + " style=\"" + vrBord + "\" bordercolor=\"#" + vCell.__fBorderColor + "\"";

                            if (vCell.__fFontBold == true)
                                vReturn = "<B>" + vReturn + "</B>";
                            if (vCell.__fFontItalic == true)
                                vReturn = "<I>" + vReturn + "</I>";
                            if (vCell.__fFontUnderline == true)
                                vReturn = "<U>" + vReturn + "</U>";

                            if (vCell.__fFontName.Length != 0)
                                vAttributeFont = vAttributeFont + " face=\"" + vCell.__fFontName + "\"";
                            else
                                if (vRow.__fFontName.Length != 0)
                                vAttributeFont = vAttributeFont + " face=\"" + vRow.__fFontName + "\"";

                            if (vCell.__fFontSize.Length != 0)
                                vAttributeFont = vAttributeFont + " size=\"" + vCell.__fFontSize + "\"";
                            else
                                if (vRow.__fFontSize.Length != 0)
                                vAttributeFont = vAttributeFont + " size=\"" + vRow.__fFontSize + "\"";
                            if (vCell.__fColorText.Length != 0)
                                vAttributeFont = vAttributeFont + " color=\"#" + vCell.__fColorText + "\"";
                            else
                                if (vRow.__fColorText.Length != 0)
                                vAttributeFont = vAttributeFont + " color=\"#" + vRow.__fColorText + "\"";
                            if (vAttributeFont.Length > 5)
                            {
                                vReturn = vAttributeFont + ">" + vReturn + "</FONT>";
                            }

                            #endregion * Вывод данных

                            vReturn = vAttributeStart + ">" + vReturn + "</TD>\n";
                            vBody = vBody + vReturn;
                        }
                    }
                }

            BeginNewRow:

                vBody = vBody + "</TR>\n";
            }

            #endregion Заполнение отчета данными

            #region - Закрытие файла

            vBody = vBody + "</TABLE>\n";
            vBody = vBody + "</BODY>\n";
            vBody = vBody + "</HTML>";

            #endregion - Закрытие файла

            vFileText.__mWriteToEnd(__fFilePath, vBody);

            /// Вызов формы для предварительного просмотра
            if (__fAtOnceExcel == false)
            {
                /// Активация предыдущей формы если она была указана в отчете
                if (__fFormParent != null)
                {
                    //crlForm vForm = (crlForm)Activator.CreateInstance(_fFormParent);
                    //(vForm as crlFormFilter).ShowDialog();
                }
            }
            /// Отображение отчета в Excel
            else
            {
                string vExcelFileName = Path.Combine(appApplication.__oPathes.__fDirectoryTemp_, appApplication.__fPrefix_ + "_" + __fFileExcelName + ".xlsx");
                if (File.Exists(vExcelFileName) == true)
                {
                    // Список Excel процессов.
                    Process[] vaPrcsList = Process.GetProcessesByName("EXCEL");

                    foreach (Process voPrcs in vaPrcsList)
                    {
                        if (voPrcs.MainWindowTitle == appApplication.__fPrefix_ + "_" + __fFileExcelName + " - Excel")
                        {
                            voPrcs.Kill();
                            //do { appApplication.__oEventsHandler.__mPause(5); } while (voPrcs.HasExited == false); 123456
                        }
                    }
                }
                File.Delete(vExcelFileName);
                Excel.Application voExcl;
                voExcl = new Excel.Application();
                Excel.Workbook voWorkBook = voExcl.Workbooks.Open(__fFilePath);
                voWorkBook.SaveAs(vExcelFileName, Excel.XlFileFormat.xlOpenXMLWorkbook, Missing.Value, Missing.Value, false, false, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlUserResolution, true, Missing.Value, Missing.Value, Missing.Value);
                voWorkBook.Saved = true;
                voExcl.Visible = true;
            }
        }

        #endregion Отчет

        #region Строки и ячейки

        /// <summary>
        /// Создание ячейки в текущей строке отчета
        /// </summary>
        /// <param name="pValue">Значение ячейки</param>
        /// <param name="pAttribute">Названия классов в CSS файле или атрибуты тэгов</param>
        /// <returns>Номер ячейки в строке</returns>
        public int __mCell(object pValue, params string[] pAttribute)
        {
            if (pValue.ToString().Trim() == "ПЕРЕЧИСЛЕНИЯ")
            {
                pValue = 0;
            }
            rhtUnitCell vCell = new rhtUnitCell();
            vCell.__fValue = pValue;

            foreach (string vParameter in pAttribute)
            {
                string vParameterName = vParameter; // Название параметра
                string vParameterValue = ""; // Значение параметра
                /// Определение ключа и значения
                appTypeString.__mParameterParse(ref vParameterName, ref vParameterValue);
                if (vParameterName != "CL")
                    vParameterValue = vParameterValue.ToLower();
                switch (vParameterName.Trim())
                {
                    case "A":
                        vCell.__fAlign = vParameterValue;
                        break;
                    case "B":
                        vCell.__fBorder = vParameterValue;
                        break;
                    case "BC":
                        vCell.__fBorderColor = vParameterValue;
                        break;
                    case "BS":
                        vCell.__fBorderSize = vParameterValue;
                        break;
                    case "CL":
                        vCell.__fClasses = vParameterValue;
                        break;
                    case "CB":
                        vCell.__fColorBack = vParameterValue;
                        break;
                    case "CT":
                        vCell.__fColorText = vParameterValue;
                        break;
                    case "FB":
                        vCell.__fFontBold = true;
                        break;
                    case "FI":
                        vCell.__fFontItalic = true;
                        break;
                    case "FU":
                        vCell.__fFontUnderline = true;
                        break;
                    case "FN":
                        vCell.__fFontName = vParameterValue;
                        break;
                    case "FS":
                        vCell.__fFontSize = vParameterValue;
                        break;
                    case "H":
                        vCell.__fHeight = vParameterValue;
                        break;
                    case "IM":
                        vCell.__fImage = vParameterValue;
                        break;
                    case "L":
                        vCell.__fLinkFile = vParameterValue;
                        break;
                    case "SC":
                        vCell.__fSpanColumn = vParameterValue;
                        break;
                    case "SR":
                        vCell.__fSpanRow = vParameterValue;
                        break;
                    case "VA":
                        vCell.__fValign = vParameterValue;
                        break;
                    case "W":
                        vCell.__fWidth = vParameterValue;
                        break;
                    case "Z":
                        vCell.__fZero = true;
                        break;
                }
            }
            /// Запись ячейки в строку
            fCurrentRow._fCellsList.Add(vCell);
            /// Подсчет максимального количества ячеек в расчете
            if (__fColumnsCountInReport < fCurrentRow._fCellsList.Count)
                __fColumnsCountInReport = fCurrentRow._fCellsList.Count;

            return fCurrentRow._fCellsList.Count - 1;
        }
        /// <summary>
        /// Изменение значения в не текущей ячейке
        /// </summary>
        /// <param name="pValue">Новое значение</param>
        /// <param name="pRowNumber">Номер строки</param>
        /// <param name="pCellNumber">Номер ячейки в строке</param>
        public void __mCellChangeValue(object pValue, int pRowNumber, int pCellNumber)
        {
            _fRowsList[pRowNumber]._fCellsList[pCellNumber].__fValue = pValue;

            return;
        }
        /// <summary>
        /// Рисование сплошной линии
        /// </summary>
        /// <param name="pAttribute"></param>
        /// <returns>Указатель на созданную запись</returns>
        public int __mLine(params string[] pAttribute)
        {
            string vCommand = "<HR ";
            foreach (string vString in pAttribute)
            {
                string vParameterName = vString; // Название параметра
                string vParameterValue = ""; // Значение параметра
                /// Определение ключа и значения
                appTypeString.__mParameterParse(ref vParameterName, ref vParameterValue);
                switch (vParameterName.Trim())
                {
                    case "A":
                        vCommand += "align = ";
                        switch (vParameterValue.Trim())
                        {
                            case "left":
                                vCommand += "\"left\" ";
                                break;
                            case "l":
                                vCommand += "\"left\" ";
                                break;
                            case "center":
                                vCommand += "\"center\" ";
                                break;
                            case "c":
                                vCommand += "\"center\" ";
                                break;
                            case "right":
                                vCommand += "\"right\" ";
                                break;
                            case "r":
                                vCommand += "\"right\" ";
                                break;
                        }
                        break;
                    case "C":
                        vCommand += "color = \"#" + vParameterValue.Trim() + "\" ";
                        break;
                    case "S":
                        vCommand += "size = \"" + vParameterValue.Trim() + "\" ";
                        break;
                    case "W":
                        vCommand += "width = \"" + vParameterValue.Trim() + "\" ";
                        break;
                    case "noshade":
                        vCommand += "noshade ";
                        break;
                }
            }
            vCommand += ">";
            int vReturn = __mRow();
            for (int vAmount = 0; vAmount < __fColumnsCountInReport; vAmount++)
            {
                rhtUnitCell vCell = new rhtUnitCell();
                vCell.__fValue = vCommand;
                vCell.__fLine = true;
                fCurrentRow.__mAdd(vCell);
            }

            return vReturn;
        }
        /// <summary>
        /// Создание новой строки в отчете
        /// </summary>
        /// <param name="pAttribute">Названия классов в CSS файле или атрибуты тэгов</param>
        /// <returns>Указатель на созданную запись</returns>
        public int __mRow(params string[] pAttribute)
        {
            rhtUnitRow vRow = new rhtUnitRow();
            foreach (string vAttribut in pAttribute)
            {
                string vParameterName = vAttribut; // Название параметра
                string vParameterValue = ""; // Значение параметра
                /// Определение ключа и значения
                appTypeString.__mParameterParse(ref vParameterName, ref vParameterValue);
                /// Заполнение атрибутов записи полученными данными
                switch (vParameterName)
                {
                    case "A":
                        vRow.__fAlign = vParameterValue;
                        break;
                    case "B":
                        vRow.__fBorder = vParameterValue;
                        break;
                    case "BC":
                        vRow.__fBorderColor = vParameterValue;
                        break;
                    case "BS":
                        vRow.__fBorderSize = vParameterValue;
                        break;
                    case "CL":
                        vRow.__fClasses = vParameterValue;
                        break;
                    case "CB":
                        vRow.__fColorBack = vParameterValue;
                        break;
                    case "CT":
                        vRow.__fColorText = vParameterValue;
                        break;
                    case "FB":
                        vRow.__fFontBold = true;
                        break;
                    case "FI":
                        vRow.__fFontItalic = true;
                        break;
                    case "FU":
                        vRow.__fFontUnderline = true;
                        break;
                    case "FN":
                        vRow.__fFontName = vParameterValue;
                        break;
                    case "FS":
                        vRow.__fFontSize = vParameterValue;
                        break;
                    case "SC":
                        vRow.__fSpanColumn = vParameterValue;
                        break;
                    case "SR":
                        vRow.__fSpanRow = vParameterValue;
                        break;
                    case "W":
                        vRow.__fWidth = vParameterValue;
                        break;
                }
            }
            fCurrentRow = vRow;
            _fRowsList.Add(fCurrentRow);

            return _fRowsList.Count - 1;
        }
        /// <summary>
        /// Создание новой пустой строки в отчете
        /// </summary>
        /// <returns>Указатель на созданную запись</returns>
        public int __mRowEmpty()
        {
            int vReturn = __mRow();
            for (int vAmount = 0; vAmount < __fColumnsCountInReport; vAmount++)
            {
                rhtUnitCell vCell = new rhtUnitCell();
                vCell.__fValue = "&nbsp;";
                fCurrentRow.__mAdd(vCell);
            }

            return vReturn;
        }
        /// <summary>
        /// Создание строки из повторяющихся пробелов
        /// </summary>
        /// <param name="pCount">Количество пробелов</param>
        public string __mSpace(int pCount)
        {
            return string.Concat(Enumerable.Repeat("&nbsp;", pCount));
        }
        /// <summary>
        /// Отображение времени создания отчета
        /// </summary>
        /// <param name="pAttributesGroup">Названия классов в CSS файле или атрибуты тэгов</param>
        /// <returns>Указатель на созданную запись</returns>
        public int __mTime(params string[] pAttributesGroup)
        {
            int vRowNumber = __mRow();
            int vCellNumber = __mCell(appApplication.__oTunes.__mTranslate("Время создания отчета") + ": " + fTimeCreate.ToString(), pAttributesGroup);
            _fRowsList[vRowNumber]._fCellsList[vCellNumber].__fSpanColumn = __fColumnsCountInReport.ToString();

            return vRowNumber;
        }
        /// <summary>
        /// Отображение пользователя создавшего отчет
        /// </summary>
        /// <param name="pAttributesGroup">Названия классов в CSS файле или атрибуты тэгов</param>
        /// <returns>Указатель на созданную запись</returns>
        public int __mUser(string pUser, params string[] pAttributesGroup)
        {
            int vRowNumber = __mRow();
            int vCellNumber = __mCell(appApplication.__oTunes.__mTranslate("Оператор создавший отчет") + ": "
                + pUser
                , pAttributesGroup);
            _fRowsList[vRowNumber]._fCellsList[vCellNumber].__fSpanColumn = __fColumnsCountInReport.ToString();

            return vRowNumber;
        }

        #endregion Строки и ячейки

        #region Надстройки

        /// <summary>
        /// Добавление строки в отчет из DataRow
        /// </summary>
        /// <param name="pDataRow">Запись из таблицы</param>
        /// <param name="pAttributesGroup">Аттрибуты ячеек</param>
        /// <returns></returns>
        public int __mRowFromDataRow(DataRow pDataRow, params string[] pAttributesGroup)
        {
            int vReturn = __mRow();

            for (int vAmount = 0; vAmount < pDataRow.Table.Columns.Count; vAmount++)
            {
                __mCell(pDataRow[vAmount].ToString(), pAttributesGroup);
            }

            return vReturn;
        }
        /// <summary>
        /// Добавление строк в отчет из DataTable
        /// </summary>
        /// <param name="pDataTable">Таблица с данными</param>
        /// <param name="pAttributesGroup">Аттрибуты ячеек</param>
        public void __mRowFromDataTable(DataTable pDataTable, params string[] pAttributesGroup)
        {
            foreach (DataRow vDataRow in pDataTable.Rows)
            {
                __mRowFromDataRow(vDataRow, pAttributesGroup);
            }

            return;
        }

        #endregion Надстройки

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Аргументы

        /// <summary>
        /// Отображение отчета сразу в MS Excel
        /// </summary>
        public bool __fAtOnceExcel = false;
        /// <summary>
        /// Основной цвет фона отчета
        /// </summary>
        public string __fBackColor = "";
        /// <summary>
        /// Максимальное количество колонок в строках отчета
        /// </summary>
        public int __fColumnsCountInReport = 0;
        /// <summary>
        /// Имя для сохранения отчета в формате Excel
        /// </summary>
        public string __fFileExcelName = "Excel";
        /// <summary>
        /// Путь и имя файла отчета
        /// </summary>
        public string __fFilePath = "";
        /// <summary>
        /// Основной цвет текста отчета
        /// </summary>
        public string __fForeColor = "";
        /// <summary>
        /// Тип родительской формы
        /// </summary>
        public Type __fFormParent;
        /// <summary>
        /// Заголовок отчета
        /// </summary>
        public string __fTitle = "";

        #endregion Аргументы

        #region - Внутренние

        /// <summary>
        /// Список строк отчета
        /// </summary>
        internal List<rhtUnitRow> _fRowsList = new List<rhtUnitRow>();

        #endregion Внутренние

        #region - Служебные

        /// <summary>
        /// Полное название класса
        /// </summary>
        private string fClassNameFull = "";
        /// <summary>
        /// Текущая строка отчета в которую добавляются ячейки
        /// </summary>
        private rhtUnitRow fCurrentRow;
        /// <summary>
        /// Время создания отчета
        /// </summary>
        private DateTime fTimeCreate = DateTime.Now;

        #endregion Служебные

        #endregion ПОЛЯ
    }
}

