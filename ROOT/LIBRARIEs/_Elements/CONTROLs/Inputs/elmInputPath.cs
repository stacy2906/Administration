using nlApplication;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmInputPath.cs
    /// </summary>
    /// <remarks>Класс-поле ввода пути к файлу или папке</remarks>
    /* Пример использования
                _cInputPath.Location = new System.Drawing.Point(10, 40);
                _cInputPath.__fCaption_ = "Путь к проекту";
                _cInputPath.__fPathType_ = PATHTYPES.Directory;
                _cInputPath.__fValue_ = "C:\\Projects\\Sample";
    */
    public class elmInputPath : elmInput
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Размещение компонентов

            Panel2.Controls.Add(_cInput);
            Panel2.Controls.Add(_cButtonBrowse);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            // _cInput
            {
                _cInput.Location = new Point(0, 0);
                _cInput.__eChangedByUserAfter += mEventInputChangedByUser;
            }
            // _cButtonBrowse
            {
                _cButtonBrowse.Text = "...";
                _cButtonBrowse.Width = 28;
                _cButtonBrowse.Height = 20;
                _cButtonBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                _cButtonBrowse.TabStop = false;
                _cButtonBrowse.Click += mEventButtonBrowseClick;
            }

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();

            _mLayoutInputAndButton();

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется при выборе кнопки 'Обзор'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mEventButtonBrowseClick(object sender, EventArgs e)
        {
            string vPathCurrent = _cInput.Text.Trim(); // Текущее значение пути

            /// 1.Y Если выбирается файл - открывается диалог выбора файла
            if (__fPathType_ == PATHTYPES.File)
            {
                using (OpenFileDialog vDialog = new OpenFileDialog())
                {
                    vDialog.CheckFileExists = false;
                    vDialog.Multiselect = false;

                    if (vPathCurrent.Length > 0 && File.Exists(vPathCurrent))
                    {
                        vDialog.InitialDirectory = Path.GetDirectoryName(vPathCurrent);
                        vDialog.FileName = Path.GetFileName(vPathCurrent);
                    }
                    else if (vPathCurrent.Length > 0 && Directory.Exists(vPathCurrent))
                    {
                        vDialog.InitialDirectory = vPathCurrent;
                    }

                    if (vDialog.ShowDialog() == DialogResult.OK)
                        __fValue_ = vDialog.FileName;
                }
            }
            /// 1.N Иначе открывается диалог выбора папки
            else
            {
                using (FolderBrowserDialog vDialog = new FolderBrowserDialog())
                {
                    if (vPathCurrent.Length > 0 && Directory.Exists(vPathCurrent))
                        vDialog.SelectedPath = vPathCurrent;

                    if (vDialog.ShowDialog() == DialogResult.OK)
                        __fValue_ = vDialog.SelectedPath;
                }
            }
            /// Включение использования фильтра
            __fMarkStatus_ = true;

            return;
        }
        /// <summary>
        /// Выполняется при изменении данных пользователем
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mEventInputChangedByUser(object sender, EventArgs e)
        {
            /// Включение использования фильтра
            __fMarkStatus_ = true;

            return;
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Размещение поля ввода и кнопки обзора относительно ширины панели
        /// </summary>
        private void _mLayoutInputAndButton()
        {
            if (Panel2 == null)
                return;

            int vButtonWidth = _cButtonBrowse.Width; // Ширина кнопки обзора
            int vGap = elmInterface.__fIntervalHorizontal; // Отступ между полем ввода и кнопкой

            _cButtonBrowse.Location = new Point(Math.Max(0, Panel2.ClientSize.Width - vButtonWidth), 0);

            _cInput.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            _cInput.Width = Math.Max(0, Panel2.ClientSize.Width - vButtonWidth - vGap);

            return;
        }
        /// <summary>
        /// Загрузка пути из файла настроек
        /// </summary>
        /// <param name="pFileName">Название раздела настройки (имя ключевого параметра)</param>
        public override void __mItemsLoadFromFile(string pFileName)
        {
            appFileIni vFileIni = new appFileIni(elmApplication.__oPathes.__mFileTunes()); // Объект для работы с инициализационным файлом
            string vPath = vFileIni.__mValueRead(pFileName.Trim().ToUpper(), "Path"); // Прочитанное значение пути

            if (vPath.Trim().Length > 0)
                __fValue_ = vPath;

            return;
        }
        /// <summary>
        /// Сохранение пути в файл настроек
        /// </summary>
        /// <param name="pFileName">Название раздела настройки (имя ключевого параметра)</param>
        public override void __mItemsSaveToFile(string pFileName)
        {
            appFileIni vFileIni = new appFileIni(elmApplication.__oPathes.__mFileTunes()); // Объект для работы с инициализационным файлом
            vFileIni.__mValueWrite(_cInput.Text.Trim(), pFileName.Trim().ToUpper(), "Path");

            return;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Поле ввода пути
        /// </summary>
        /// <remarks>
        /// Используется конструктор с явным списком запрещенных символов, поскольку список по умолчанию
        /// (см. <see cref="AlphanumericBehavior"/>) запрещает ':' и '\', что делает невозможным ввод пути Windows
        /// </remarks>
        protected elmComponentString _cInput = new elmComponentString(new char[] { '%', '\'', '*', '"', '+', '?', '>', '<' });
        /// <summary>
        /// Кнопка обзора пути
        /// </summary>
        protected Button _cButtonBrowse = new Button();

        #endregion Компоненты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Доступность контрола
        /// </summary>
        public override bool __fEnabled_
        {
            get { return base.__fEnabled_; }
            set
            {
                base.__fEnabled_ = value;
                _cInput.Visible = value;
                _cButtonBrowse.Visible = value;
                if (value == false)
                {
                    if (_cInput.Text.Trim().Length > 0)
                        _cLabelValue.Text = _cInput.Text.Trim();
                    else
                        _cLabelValue.Text = elmApplication.__oTunes.__mTranslate("нет данных");
                }
            }
        }
        /// <summary>
        /// Количество отображаемых символов данных
        /// </summary>
        public override int __fSymbolsCount_
        {
            get { return _cInput.__fSymbolsCount_; }
            set
            {
                _cInput.__fSymbolsCount_ = value;
                _mLayoutInputAndButton();
            }
        }
        /// <summary>
        /// Значение поля ввода (путь к файлу или папке)
        /// </summary>
        public override object __fValue_
        {
            get { return _cInput.Text; }
            set
            {
                _cInput.Text = value == null ? "" : value.ToString().Trim();
                _cLabelValue.Text = _cInput.Text; // Запись значения по умолчанию
            }
        }

        #endregion СВОЙСТВА
    }
}