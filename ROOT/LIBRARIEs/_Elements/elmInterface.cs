using nlApplication;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmInterface.cs
    /// </summary>
    /// <remarks>Класс-настройки пользовательского интерфейса</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.14 00-00</version> // Дата-время последней корректировки
    public class elmInterface
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static elmInterface()
        {
            //crlFormAbout vForm = new crlFormAbout();
        }
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public elmInterface()
        {
            _mObjectAssembly();
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        public virtual void _mObjectAssembly()
        {
            __fColorData = Color.FromArgb(255, 0, 100, 255);
            __fColorDataBack = Color.FromArgb(255, 255, 240, 220);
            __fColorDataBackDisabled = Color.FromArgb(255, 220, 220, 255);
            __fColorDataBackNecessarily = Color.FromArgb(255, 255, 220, 220);
            __fColorFormActive = Color.FromArgb(255, 250, 220, 200);
            __fColorFormDeactive = Color.FromArgb(255, 210, 180, 160);
            __fColorText = Color.FromArgb(255, 0, 0, 160);
            __fColorTextButton = Color.FromArgb(255, 10, 10, 255);
            __fColorTextTitle = Color.FromArgb(255, 192, 0, 0);

            __fFontDataSize = 9;

            Form vForm = new Form();
            vForm.WindowState = FormWindowState.Normal;
            Rectangle vRectangleAll = vForm.RectangleToClient(vForm.Bounds);
            Rectangle vRectangleClient = vForm.ClientRectangle;
            __fFormBorderHeight = (vRectangleAll.Height - vRectangleClient.Height) / 2;
            __fFormBorderWidth = (vRectangleAll.Width - vRectangleClient.Width) / 2;
            __fFormHeaderHeight = SystemInformation.CaptionHeight;
            __fScrollBarHorizontalHeight = SystemInformation.HorizontalScrollBarHeight;
            __fScrollBarVerticalWidth = SystemInformation.VerticalScrollBarWidth;

            __fIntervalHorizontal = 5;
            __fIntervalVertical = 5;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Получение цвета
        /// </summary>
        /// <param name="pColor">Выбираемый цвет</param>
        /// <returns>{Color}</returns>
        public virtual Color __mColor(COLORS pColor)
        {
            Color vReturn = Color.Empty; // Возвращаемое значение

            switch (pColor)
            {
                case COLORS.Data:
                    vReturn = __fColorData;
                    break;
                case COLORS.DataBack:
                    vReturn = __fColorDataBack;
                    break;
                case COLORS.DataBackDisabled:
                    vReturn = __fColorDataBackDisabled;
                    break;
                case COLORS.DataBackNecessarily:
                    vReturn = __fColorDataBackNecessarily;
                    break;
                case COLORS.FormActive:
                    vReturn = __fColorFormActive;
                    break;
                case COLORS.FormDeactive:
                    vReturn = __fColorFormDeactive;
                    break;
                case COLORS.Text:
                    vReturn = __fColorText;
                    break;
                case COLORS.TextButton:
                    vReturn = __fColorTextButton;
                    break;
                case COLORS.TextTitle:
                    vReturn = __fColorTextTitle;
                    break;
            }

            return vReturn;
        }
        /// <summary>
        /// Получение шрифта
        /// </summary>
        /// <param name="pFont">Выбираемый шрифт</param>
        /// <returns>{Font}</returns>
        public virtual Font __mFont(FONTS pFont)
        {
            Font vReturn = new Font("Calibri", __fFontDataSize);

            switch (pFont)
            {
                case FONTS.Data:
                    vReturn = new Font("Courier", __fFontDataSize);
                    break;
                case FONTS.NodeNotEdit:
                    break;
                case FONTS.Text:
                    vReturn = new Font("Segoe UI", __fFontDataSize);
                    break;
                case FONTS.TextButton:
                    vReturn = new Font(vReturn, FontStyle.Underline);
                    break;
                case FONTS.TextTitle:
                    vReturn = new Font(vReturn, FontStyle.Bold);
                    break;
            }

            return vReturn;
        }
        /// <summary>
        /// Загрузка настроек из файла
        /// </summary>
        /// <returns>[true] - данные загружены, иначе - [false]</returns>
        public virtual bool __mLoad()
        {
            appFileIni vFileIni = new appFileIni(appApplication.__oPathes.__mFileTunes()); // Объект для работы с инициализационным файлом
            string vValueIni = "";
            vValueIni = vFileIni.__mValueReadWrite("0, 100, 255", "COLORs", "ColorData");
            __fColorData = Color.FromArgb(255, elmTypeColor.__mFromString2(vValueIni));
            vValueIni = vFileIni.__mValueReadWrite("255, 240, 220", "COLORs", "ColorDataBack");
            __fColorDataBack = Color.FromArgb(255, elmTypeColor.__mFromString2(vValueIni));
            vValueIni = vFileIni.__mValueReadWrite("220, 220, 255", "COLORs", "ColorDataBackDisabled");
            __fColorDataBackDisabled = Color.FromArgb(255, elmTypeColor.__mFromString2(vValueIni));
            vValueIni = vFileIni.__mValueReadWrite("255, 220, 220", "COLORs", "ColorDataBackNecessarily");
            __fColorDataBackNecessarily = Color.FromArgb(255, elmTypeColor.__mFromString2(vValueIni));
            vValueIni = vFileIni.__mValueReadWrite("250, 220, 200", "COLORs", "ColorFormActive");
            __fColorFormActive = Color.FromArgb(255, elmTypeColor.__mFromString2(vValueIni));
            vValueIni = vFileIni.__mValueReadWrite("210, 180, 160", "COLORs", "ColorFormDeactive");
            __fColorFormDeactive = Color.FromArgb(255, elmTypeColor.__mFromString2(vValueIni));
            vValueIni = vFileIni.__mValueReadWrite("0, 0, 160", "COLORs", "ColorText");
            __fColorText = Color.FromArgb(255, elmTypeColor.__mFromString2(vValueIni));
            vValueIni = vFileIni.__mValueReadWrite("10, 10, 255", "COLORs", "ColorTextButton");
            __fColorTextButton = Color.FromArgb(255, elmTypeColor.__mFromString2(vValueIni));
            vValueIni = vFileIni.__mValueReadWrite("192, 0, 0", "COLORs", "ColorTextTitle");
            __fColorTextTitle = Color.FromArgb(255, elmTypeColor.__mFromString2(vValueIni));

            return true;
        }
        /// <summary>
        /// Загрузка настроек в файл
        /// </summary>
        /// <returns>[true] - Данные загружены без ошибок, иначе - [false]</returns>
        public virtual bool __mSave()
        {
            appFileIni vFileIni = new appFileIni(appApplication.__oPathes.__mFileTunes()); // Объект для работы с инициализационным файлом
            vFileIni.__mValueWrite(elmTypeColor.__mToString2(__fColorData), "COLORs", "ColorData");
            vFileIni.__mValueWrite(elmTypeColor.__mToString2(__fColorDataBack), "COLORs", "ColorDataBack");
            vFileIni.__mValueWrite(elmTypeColor.__mToString2(__fColorDataBackDisabled), "COLORs", "ColorDataBackDisabled");
            vFileIni.__mValueWrite(elmTypeColor.__mToString2(__fColorDataBackNecessarily), "COLORs", "ColorDataBackNecessarily");
            vFileIni.__mValueWrite(elmTypeColor.__mToString2(__fColorFormActive), "COLORs", "ColorFormActive");
            vFileIni.__mValueWrite(elmTypeColor.__mToString2(__fColorFormDeactive), "COLORs", "ColorFormDeactive");
            vFileIni.__mValueWrite(elmTypeColor.__mToString2(__fColorText), "COLORs", "ColorText");
            vFileIni.__mValueWrite(elmTypeColor.__mToString2(__fColorTextButton), "COLORs", "ColorTextButton");
            vFileIni.__mValueWrite(elmTypeColor.__mToString2(__fColorTextTitle), "COLORs", "ColorTextTitle");

            return true;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        /// <summary>
        /// Цвет текста данных
        /// </summary>
        public Color __fColorData = Color.FromArgb(255, 0, 100, 255);
        /// <summary>
        /// Цвет фона данных
        /// </summary>
        public Color __fColorDataBack = Color.FromArgb(255, 255, 240, 220);
        /// <summary>
        /// Цвет фона не доступных данных
        /// </summary>
        public Color __fColorDataBackDisabled = Color.FromArgb(255, 220, 220, 255);
        /// <summary>
        /// Цвет фона данных которые должны быть заполнены обязательно
        /// </summary>
        public Color __fColorDataBackNecessarily = Color.FromArgb(255, 255, 220, 220);
        /// <summary>
        /// Цвет фона активной формы
        /// </summary>
        public Color __fColorFormActive = Color.FromArgb(255, 250, 220, 200);
        /// <summary>
        /// Цвет фона не активной формы
        /// </summary>
        public Color __fColorFormDeactive = Color.FromArgb(255, 210, 180, 160);
        /// <summary>
        /// Цвет надписи
        /// </summary>
        public Color __fColorText = Color.FromArgb(255, 0, 0, 160);
        /// <summary>
        /// Цвет текста-кнопки
        /// </summary>
        public Color __fColorTextButton = Color.FromArgb(255, 10, 10, 255);
        /// <summary>
        /// Цвет текста заголовка
        /// </summary>
        public Color __fColorTextTitle = Color.FromArgb(255, 192, 0, 0);

        /// <summary>
        /// Размер шрифта данных
        /// </summary>
        public int __fFontDataSize = 9;

        /// <summary>
        /// Высота рамки формы
        /// </summary>
        public int __fFormBorderHeight = 0;
        /// <summary>
        /// Ширина рамки формы
        /// </summary>
        public static int __fFormBorderWidth = 0;
        /// <summary>
        /// Высота заголовка формы
        /// </summary>
        public int __fFormHeaderHeight = 0; // static readonly
        /// <summary>
        /// Вертикальный интервал между визуальными компонентами
        /// </summary>
        public static int __fIntervalHorizontal = 5;
        /// <summary>
        /// Вертикальный интервал между визуальными компонентами
        /// </summary>
        public static int __fIntervalVertical = 5;
        /// <summary>
        /// Высота бегунка горизонтальной прорутки
        /// </summary>
        public int __fScrollBarHorizontalHeight = 0;
        /// <summary>
        /// Ширина бегунка вертикальной прокрутки
        /// </summary>
        public int __fScrollBarVerticalWidth = 0;

        #endregion ПОЛЯ
    }
}
