using System;
using System.IO;
using System.Xml;

namespace nlApplication
{
    /// <summary>
    /// Файл appFileXml.cs
    /// </summary>
    /// <remarks>Класс для работы с XML файлами</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 14-02</version> // Дата-время последней корректировки
    public class appFileXml
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Запись протокола в файл
        /// </summary>
        public virtual void __mToFile()
        {
            /// Если приложение будет работать за полночь, чтобы писать в новый файл
            string vFilePath = appApplication.__oPathes.__fFileProtocol_;
            DateTime vDateTime = DateTime.Now;

            XmlDocument vXmlDocument = new XmlDocument(); // Объект для работы с XML документами
            if (File.Exists(vFilePath) == false)
            {
                XmlDeclaration vXmlDeclaration = vXmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement vXmlElementRoot = vXmlDocument.DocumentElement;
                vXmlDocument.InsertBefore(vXmlDeclaration, vXmlElementRoot);
                XmlElement vXmlElementProtocol = vXmlDocument.CreateElement(string.Empty, "Protocol", string.Empty);
                vXmlDocument.AppendChild(vXmlElementProtocol);

                vXmlDocument.Save(vFilePath);
            }
            vXmlDocument.Load(vFilePath); /// Открытие файла
            long __ProtocolKey = DateTime.Now.Ticks; // Сохранение ключа протокола
            string vProtocolKey = __ProtocolKey.ToString(); // Строчный идентификатор записи в протоколе

            XmlNode vXmlNodeRecord = vXmlDocument.CreateElement("Protocol"); // Создание записи протокола
            vXmlDocument.DocumentElement.AppendChild(vXmlNodeRecord);

            // Создание аттрибутов
            XmlAttribute vXmlAttributeKey = vXmlDocument.CreateAttribute("cluPcl"); // Аттрибут "Key"
            vXmlAttributeKey.Value = vProtocolKey;
            vXmlNodeRecord.Attributes.Append(vXmlAttributeKey);

            XmlAttribute vXmlAttributeDateTime = vXmlDocument.CreateAttribute("dtmPcl");// Аттрибут "DateTime"
            vXmlAttributeDateTime.Value = DateTime.Now.ToString();
            vXmlNodeRecord.Attributes.Append(vXmlAttributeDateTime);

            XmlAttribute vXmlAttributeApplicationName = vXmlDocument.CreateAttribute("dsiApl");// Аттрибут "Приложение"
            vXmlAttributeApplicationName.Value = appApplication.__fProcessName_;
            vXmlNodeRecord.Attributes.Append(vXmlAttributeApplicationName);

            XmlAttribute vXmlAttributeApplicationDescription = vXmlDocument.CreateAttribute("dpnApl");// Аттрибут "Краткое описание приложения"
            vXmlAttributeApplicationDescription.Value = appApplication.__fDescription_;
            vXmlNodeRecord.Attributes.Append(vXmlAttributeApplicationDescription);

            XmlAttribute vXmlAttributeHostName = vXmlDocument.CreateAttribute("dsiHst");// Аттрибут "Хост"
            vXmlAttributeHostName.Value = Environment.MachineName;
            vXmlNodeRecord.Attributes.Append(vXmlAttributeHostName);

            XmlAttribute vXmlAttributeHostLogin = vXmlDocument.CreateAttribute("Lgn");// Аттрибут "Логин хоста"
            vXmlAttributeHostLogin.Value = Environment.UserName;
            vXmlNodeRecord.Attributes.Append(vXmlAttributeHostLogin);

            XmlAttribute vXmlAttributeType = vXmlDocument.CreateAttribute("lnkPclTyp");// Вид протокола
            //vXmlAttributeType.Value = fProtocolTypeClue.ToString();
            vXmlNodeRecord.Attributes.Append(vXmlAttributeType);

            XmlAttribute vXmlAttributePrintScreen = vXmlDocument.CreateAttribute("FilPrnScr");// Путь и имя файла PrintScreen
            //vXmlAttributePrintScreen.Value = __fFilePathPrintScreen;
            vXmlNodeRecord.Attributes.Append(vXmlAttributePrintScreen);

            XmlAttribute vXmlAttributeProcedure = vXmlDocument.CreateAttribute("Prc");// Процедура
            //vXmlAttributeProcedure.Value = __fProcedure_;
            vXmlNodeRecord.Attributes.Append(vXmlAttributeProcedure);

            // Создание полей
            XmlNode vXmlNodeFieldLnkPclRrdTyp = vXmlDocument.CreateElement("lnkPclRrdTyp");
            //vXmlNodeFieldLnkPclRrdTyp.InnerText = fProtocolRecordTypeKey.ToString();
            vXmlNodeRecord.AppendChild(vXmlNodeFieldLnkPclRrdTyp);

            XmlNode vXmlNodeFieldMsg = vXmlDocument.CreateElement("Msg");
            //vXmlNodeFieldMsg.InnerText = fProtocolRecordMessage;
            vXmlNodeRecord.AppendChild(vXmlNodeFieldMsg);

            XmlNode vXmlNodeFieldDtmPcl = vXmlDocument.CreateElement("Sec");
            //vXmlNodeFieldDtmPcl.InnerText = fProtocolRecordTick.ToString();
            vXmlNodeRecord.AppendChild(vXmlNodeFieldDtmPcl);
            /// Сохранение документа
            vXmlDocument.Save(vFilePath);
        }
        /// <summary>
        /// Преобразование Xml в структуированную строку
        /// </summary>
        /// <param name="xmlDoc">Xml документ</param>
        public static string __mXmlToString(XmlDocument xmlDoc)
        {
            System.Text.StringBuilder vStringBuilder = new System.Text.StringBuilder("");
            System.IO.StringWriter vStringVriter = new System.IO.StringWriter(vStringBuilder);
            xmlDoc.Save(vStringVriter);
            return vStringVriter.ToString();
        }
        public static object __mParseResult(XmlDocument pXmlDocument, string pXmlNodeName)
        {
            string vReturn = ""; // Возвращаемое значение

            File.Delete("temp.xml");
            pXmlDocument.Save("temp.xml");

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("temp.xml");
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {
                foreach (XmlElement xnode in xRoot)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("name");

                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        if (childnode.Name == pXmlNodeName)
                            vReturn = childnode.InnerText;

                    }
                }
            }
            /// Удаление временного файла
            if (File.Exists("temp.xml") == true)
            {
                File.Delete("temp.xml");
            }

            return vReturn;
        }
        /// <summary>
        /// Преобразование содержания Xml в структуированную строку
        /// </summary>
        /// <param name="xmlDoc">Xml документ</param>
        public object __mParseResult(string pXmlDocument, string pXmlNodeName)
        {
            string vReturn = ""; // Возвращаемое значение

            File.Delete("temp.xml");
            appFileText vFileText = new appFileText();
            vFileText.__mWriteToEnd("temp.xml", pXmlDocument);

            // pXmlDocument.Save("temp.xml");

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("temp.xml");
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {
                foreach (XmlElement xnode in xRoot)
                {
                    //XmlNode attr = xnode.Attributes.GetNamedItem("root");
                    if (xnode.Name == pXmlNodeName)
                    {
                        vReturn = xnode.InnerText;
                        return vReturn;
                    }
                    else
                    {
                        foreach (XmlNode childnode in xnode.ChildNodes)
                        {
                            if (childnode.Name == pXmlNodeName)
                                vReturn = childnode.InnerText;

                        }
                    }
                }
            }
            /// Удаление временного файла
            if (File.Exists("temp.xml") == true)
            {
                File.Delete("temp.xml");
            }

            return vReturn;
        }

        #endregion Процедуры

        #endregion = МЕТОДЫ
    }
}
