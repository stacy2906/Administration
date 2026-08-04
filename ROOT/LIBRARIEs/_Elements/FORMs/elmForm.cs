//using Microsoft.Office.Interop.Excel;
using nlApplication;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmForm.cs
    /// </summary>
    /// <remarks>Класс-Форма 'Базовая'</remarks>
    public class elmForm : Form
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public elmForm()
        {
            _mObjectAssembly();
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        {
            SuspendLayout();

            #region /// Размещение компонентов

            Controls.Add(__cPanelStatus);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 261);
            KeyPreview = true; /// Разрешение обработки горячих клавиш вложенных контролов 
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "Базовая форма";
            __oFileIni.__fFilePath = elmApplication.__oPathes.__mFileFormTunes();
            __cPanelStatus.__fPercent_ = 0;
            SizeGripStyle = SizeGripStyle.Show;
            _fError = new appUnitError();

            #endregion Настройка компонентов

            ResumeLayout();

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected virtual void _mObjectPresentation()
        {
            /// Вызов метода загрузки настроек формы
            _mTunesLoad();

            #region /// Определение позиций и размеров 

            #endregion Определение позиций и размеров

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Закрытые

        /// <summary>
        /// Получение имени класса
        /// </summary>
        private string mClassName()
        {
            Type vType = this.GetType();
            return vType.Name;
        }
        /// <summary>
        /// Получение пути и имени файла класса
        /// </summary>
        private string mFilePath([CallerFilePath] string filePath = "")
        {
            return filePath;
        }
        /// <summary>
        /// Получение значения номера строки в текущей процедуре
        /// </summary>
        private int mLine(string message = "", [CallerLineNumber] int line = 0)
        {
            return line;
        }
        /// <summary>
        /// Получение названия текущей процедуры
        /// </summary>
        private string mProcedure(string message = "", [CallerMemberName] string member = "")
        {
            return member;
        }

        #endregion Закрытые

        #region - Поведение

        /// <summary>
        /// Выполняется при активации формы
        /// </summary>
        /// <param name="e"></param>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            BackColor = elmApplication.__oInterface.__mColor(COLORS.FormActive);
        }
        /// <summary>
        /// Выполняется при дезактивации формы
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            BackColor = elmApplication.__oInterface.__mColor(COLORS.FormDeactive);
        }
        /// <summary>
        /// Выполняется перед отображением формы на экране
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _mObjectPresentation();
        }
        /// <summary>
        /// Выполняется перед закрытием формы
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            _mTunesSave();
            base.OnClosing(e);
        }
        /// <summary>
        /// Выполняется при нажатии на клавиши клавиатуры
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (__fKeyEscapeLock == false)
                    Close();
            }
            base.OnKeyDown(e);
        }
        /// <summary>
        /// Выполняется при получении окном сообщения
        /// </summary>
        /// <param name="msg"></param>
        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == WM_SYSCOMMAND && msg.WParam.ToInt32() == SC_CLOSE)
                __fClosedByXButtonOrAltF4_ = true;

            base.WndProc(ref msg);
        }

        #endregion Поведение

        #region - Настройки

        /// <summary>
        /// Загрузка настроек текущей формы из файла
        /// </summary>
        protected virtual void _mTunesLoad()
        {
            _mTunesLoad(__fClassName_);
        }
        /// <summary>
        /// Загрузка настроек указанной формы
        /// </summary>
        /// <param name="pFormName">'Name' формы</param>
        protected virtual void _mTunesLoad(string pFormName)
        {
            bool vTuneExists = true; // Настройки формы взяты мз настроечного файла
            Rectangle vRectangle = Screen.PrimaryScreen.Bounds;
            string vString = __oFileIni.__mValueRead(pFormName.ToUpper(), "Top");
            if (StartPosition != FormStartPosition.CenterParent & StartPosition != FormStartPosition.CenterScreen)
            {
                try
                {
                    if (vString.Length == 0)
                    {
                        vTuneExists = false;
                    } // Данных в файле нет
                    Top = Convert.ToInt32(vString);
                    if (Top < 0)
                        Top = 0;
                    if (Top > vRectangle.Size.Height)
                        Top = 0;
                }
                catch
                {
                    Top = 0;
                }
                vString = __oFileIni.__mValueRead(pFormName.ToUpper(), "Left");
                try
                {
                    Left = Convert.ToInt32(vString);
                    if (Left < 0)
                        Left = 0;
                    if (Left > vRectangle.Size.Width)
                        Left = 0;
                }
                catch
                {
                    Left = 0;
                }
            }
            if (FormBorderStyle == FormBorderStyle.Sizable)
            {
                vString = __oFileIni.__mValueRead(pFormName.ToUpper(), "Height");
                try
                {
                    Height = Convert.ToInt32(vString);
                    if (Height < MinimumSize.Height)
                        Height = MinimumSize.Height;
                }
                catch
                {
                    if (Height < MinimumSize.Height)
                        Height = MinimumSize.Height;
                }
                vString = __oFileIni.__mValueRead(pFormName.ToUpper(), "Width");
                try
                {
                    Width = Convert.ToInt32(vString);
                    if (Width < MinimumSize.Width)
                        Width = MinimumSize.Width;
                }
                catch
                {
                    if (Width < MinimumSize.Width)
                        Width = MinimumSize.Width;
                }
            }
            vString = __oFileIni.__mValueRead(pFormName.ToUpper(), "WindowState");
            try
            {
                switch (vString)
                {
                    case "Maximized":
                        WindowState = FormWindowState.Maximized;
                        break;
                    case "Minimized":
                        WindowState = FormWindowState.Minimized;
                        break;
                    case "Normal":
                        WindowState = FormWindowState.Normal;
                        break;
                }
            }
            catch { }
            if (vTuneExists == false)
            {
                WindowState = FormWindowState.Maximized;
            }
            {
                string vStringMinimumHeight = __oFileIni.__mValueRead(pFormName.ToUpper(), "MinimumHeight");
                int vIntMinimumHeight = 0;
                if (!(vStringMinimumHeight.Length == 0 | vStringMinimumHeight == "0"))
                    vIntMinimumHeight = Convert.ToInt32(vStringMinimumHeight);
                string vStringMinimumWidth = __oFileIni.__mValueRead(pFormName.ToUpper(), "MinimumWidth");
                int vIntMinimumWidth = 0;
                if (!(vStringMinimumWidth.Length == 0 | vStringMinimumWidth == "0"))
                    vIntMinimumWidth = Convert.ToInt32(vStringMinimumWidth);

                MinimumSize = new Size(vIntMinimumWidth, vIntMinimumHeight);
            } /// Сохранение минимальных размеров формы
            if (vTuneExists == false)
            {
                WindowState = FormWindowState.Maximized;
            }

        }
        /// <summary>
        /// Сохранение настроек текущей формы в файл
        /// </summary>
        protected virtual void _mTunesSave()
        {
            _mTunesSave(__fClassName_);
        }
        /// <summary>
        /// Сохранение настроек указанной формы
        /// </summary>
        /// <param name="pFormName">'Name' формы</param>
        protected virtual void _mTunesSave(string pFormName)
        {
            if (WindowState == FormWindowState.Normal) // Сохраняются размеры нормальной формы
            {
                __oFileIni.__mValueWrite(Top.ToString(), pFormName.ToUpper(), "Top");
                __oFileIni.__mValueWrite(Left.ToString(), pFormName.ToUpper(), "Left");
                __oFileIni.__mValueWrite(Height.ToString(), pFormName.ToUpper(), "Height");
                __oFileIni.__mValueWrite(Width.ToString(), pFormName.ToUpper(), "Width");
            }

            __oFileIni.__mValueWrite(WindowState.ToString(), pFormName.ToUpper(), "WindowState");

            __oFileIni.__mValueWrite(MinimumSize.Height.ToString(), pFormName.ToUpper(), "MinimumHeight");
            __oFileIni.__mValueWrite(MinimumSize.Width.ToString(), pFormName.ToUpper(), "MinimumWidth");
        }

        #endregion Настройки

        /// <summary>
        /// Отображение облака сообщения
        /// </summary>
        /// <param name="pTitle">Заголовок облака</param>
        /// <param name="pMessage">Сообщение</param>
        public void __mBaloonMessage(Control pObject, string pMessage)
        {
            ToolTip vToolTip = new ToolTip();
            vToolTip.IsBalloon = true;
            vToolTip.ToolTipTitle = elmApplication.__oTunes.__mTranslate("Ошибка ввода");
            vToolTip.ToolTipIcon = ToolTipIcon.None;
            vToolTip.UseFading = true;
            vToolTip.Show(string.Empty, pObject, 2000); // Для правильного позиционирования облака сообщения
            vToolTip.Show(pMessage, pObject, pObject.Width, pObject.Height);
        }
        /// <summary>
        /// Сборка выражения с параметрами и перевод выражения на язык интерфейса 
        /// </summary>
        /// <param name="pString">Текст</param>
        /// <param name="pParameters">Список дополнительных парамметров</param>
        public void __mCaptionBuilding(string pString, params object[] pParameters)
        {
            fTextWithOutTranslate = String.Format(pString, pParameters);
            Text = elmApplication.__oTunes.__mTranslate(pString, pParameters);
        }
        /// <summary>
        /// Вызов топика помощи связанного с формой
        /// </summary>
        public void __mHelp()
        {
            if (_fHelpFile.Length == 0)
                elmApplication.__oEventsHandler.__mHelp(_fHelpTopic);
            else
                elmApplication.__oEventsHandler.__mHelp(_fHelpFile, _fHelpTopic);
        }

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Блокировка клавиши 'Escape'
        /// </summary>
        public bool __fKeyEscapeLock = false;
        /// <summary>
        /// Список прав формы
        /// </summary>
        public ArrayList __fRightsList = new ArrayList();

        #endregion Атрибуты

        #region - Скрытые

        /// <summary>
        /// Название права формы
        /// </summary>
        //protected string __fFormRightName = "";
        /// <summary>
        /// Имя файла помощи в котором находиться топик помощи
        /// </summary>
        protected string _fHelpFile = "";
        /// <summary>
        /// Название топика помощи связанного с формой
        /// </summary>
        protected string _fHelpTopic = "";

        #endregion Скрытые

        #region - Компоненты

        /// <summary>
        /// Компонент для отображения состояния формы
        /// </summary>
        /// <remarks>public - для доступности формы из elmArea...</remarks>
        public elmPanelStatus __cPanelStatus = new elmPanelStatus();

        #endregion Компоненты

        #region - Константы

        private const int SC_CLOSE = 0xF060;
        private const int WM_SYSCOMMAND = 0x0112;

        #endregion Константы

        #region - Объекты

        /// <summary>
        /// Объект для работы с инициализационными файлами
        /// </summary>
        public appFileIni __oFileIni = new appFileIni();
        /// <summary>
        /// Объект ошибки
        /// </summary>
        protected appUnitError _fError;

        #endregion Объекты

        #region - Служебные

        /// <summary>
        /// Строка заголовка без перевода
        /// </summary>
        private string fTextWithOutTranslate = "";
        /// <summary>
        /// Отображение кнопки [X] - Закрыть окно
        /// </summary>
        //private bool fButtonCloseWindowVisible = true;

        #endregion Служебные

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        #region - Скрытые

        /// <summary>
        /// Название класса
        /// </summary>
        public string __fClassName_
        {
            get { return mClassName().Trim(); }
        }
        /// <summary>
        /// Путь к файлу текущего класса
        /// </summary>
        protected string _fClassFilePath_
        {
            get { return mFilePath(); }
        }
        /// <summary>
        /// Текущая процедура класса
        /// </summary>
        protected string _fClassProcedure_
        {
            get { return mProcedure(""); }
        }
        /// <summary>
        /// Номер строки в файле класса
        /// </summary>
        protected int _fClassLine_
        {
            get { return mLine(""); }
        }

        #endregion Скрытые

        /// <summary>
        /// Переводит и записывает выражение в заголовок формы
        /// </summary>
        /// <remarks>Возвращает переведенный текст заголовока формы</remarks>
        public string __fCaption_
        {
            get { return Text; }
            set 
            {
                fTextWithOutTranslate = value.Trim();
                Text = elmApplication.__oTunes.__mTranslate(fTextWithOutTranslate); 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool __fClosedByXButtonOrAltF4_ { get; private set; }

        #endregion СВОЙСТВА
    }
}
