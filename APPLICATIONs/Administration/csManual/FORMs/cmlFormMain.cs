using nlApplication;
using nlcsManual;
using nlElements;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace naCsManual
{
    /// <summary>
    /// Файл cmlFormMain.cs
    /// </summary>
    /// <remarks>Класс - Главная форма приложения 'CsManual'</remarks>
    public class cmlFormMain : elmForm
    {
        #region = МЕТОДЫ

        #region - Объект

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Размещение компонентов

            Controls.Add(_cBlockFormMain);
            _cBlockFormMain.Controls.Add(_cBlockInput);
            _cBlockFormMain.Controls.SetChildIndex(_cBlockInput, 0);
            _cBlockFormMain.Controls.Add(_cToolbar);
            _cBlockFormMain.Controls.SetChildIndex(_cToolbar, 1);
            _cToolbar.Items.Add(_cButtonRun);
            _cToolbar.Items.Add(_cButtonHelp);
            _cBlockInput.__mInputAdd(_cFolderScan);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            __mCaptionBuilding("Документирование CS проектов");

            ShowInTaskbar = true;

            // _cButtonRun
            {
                _cButtonRun.Image = global::nlResourcesImages.Properties.Resources._Sign_Tick_g32;
                _cButtonRun.Click += _cButtonRun_Click;
            }
            // _cButtonHelp
            {
                _cButtonHelp.Image = global::nlResourcesImages.Properties.Resources._Sign_Question_b32;
                _cButtonHelp.Click += _cButtonHelp_Click;
            }
            // _cBlockInput
            {
                _cBlockInput.Dock = DockStyle.Fill;
            }
            // _cFolderScan
            {
                _cFolderScan.__fCaption_ = "Путь к проекту";
                _cFolderScan.__fPathType_ = PATHTYPES.Directory;
                _cFolderScan.__fMarkVisible_ = false;
            }

            #endregion Настройка компонентов

            ResumeLayout();

            Type vType = this.GetType();
            Name = vType.Name;
            _fClassNameFull = vType.FullName + ".";

            return;
        }

        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();
            _cFolderScan.__mItemsLoadFromFile("scan");
            _cFolderScan.__fSymbolsCount_ = -1;

          
            if (string.IsNullOrEmpty(_cFolderScan.__fValue_?.ToString()))
            {
                _cFolderScan.__fValue_ = Application.StartupPath; // Или укажите ваш рабочий путь
            }
        }

        #endregion Объект

        #region - Поведение

        /// <summary>
        /// Выполняется при выборе кнопки 'Помощь'
        /// </summary>
        private void _cButtonHelp_Click(object sender, EventArgs e)
        {
            __mHelp();
            return;
        }

        /// <summary>
        /// Выполняется при выборе кнопки 'Выполнить'
        /// </summary>
        /// <summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cButtonRun_Click(object sender, EventArgs e)
        {
            string vProjectPath = "";

            // 1. Пытаемся достать путь из внутреннего поля ввода компонента _cFolderScan
            foreach (Control vControl in _cFolderScan.Controls)
            {
                if (vControl is TextBox || vControl is ComboBox)
                {
                    vProjectPath = vControl.Text.Trim();
                    if (!string.IsNullOrEmpty(vProjectPath))
                        break;
                }
            }

            // 2. Если пользователь ничего не ввел/не выбрал — открываем диалог выбора папки
            if (string.IsNullOrEmpty(vProjectPath) || !Directory.Exists(vProjectPath))
            {
                using (FolderBrowserDialog vFolderDialog = new FolderBrowserDialog())
                {
                    vFolderDialog.Description = "Выберите папку с исходным кодом C# (.cs):";
                    if (vFolderDialog.ShowDialog() == DialogResult.OK)
                    {
                        vProjectPath = vFolderDialog.SelectedPath;
                        _cFolderScan.Text = vProjectPath; // Записываем выбор обратно в элемент
                    }
                    else
                    {
                        // Пользователь отменил выбор
                        return;
                    }
                }
            }

            // 3. Запуск генерации документации
            if (!string.IsNullOrEmpty(vProjectPath) && Directory.Exists(vProjectPath))
            {
                // Сохраняем путь в историю
                _cFolderScan.__mItemsSaveToFile("scan");

                // Запускаем ваш движок
                cmlEngine vDocumating = new cmlEngine();
                vDocumating.__mDo(vProjectPath);

                // Открываем сгенерированный index.html
                string vIndexPath = Path.Combine(vProjectPath, "# MANUAL", "index.html");
                if (File.Exists(vIndexPath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = vIndexPath,
                        UseShellExecute = true
                    });
                }
            }
            else
            {
                appUnitError vError = new appUnitError();
                vError.__mMessageBuild("Указанная папка не существует.");
                vError.__fErrorType_ = ERRORSTYPES.User;
                vError.__fProcedure_ = _fClassNameFull + "_cButtonRun_Click(object, EventArgs)";
                cmlApplication.__oErrorsHandler.__mShow(vError);
            }

            return;
        }
        #endregion Поведение

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        protected elmComponentToolbar _cToolbar = new elmComponentToolbar();
        protected elmComponentToolbarButton _cButtonRun = new elmComponentToolbarButton();
        protected elmComponentToolbarButton _cButtonHelp = new elmComponentToolbarButton();
        protected elmBlockFormMain _cBlockFormMain = new elmBlockFormMain();
        protected elmBlockInputs _cBlockInput = new elmBlockInputs();
        protected elmInputPath _cFolderScan = new elmInputPath();

        #endregion Компоненты

        #region - Служебные

        protected string _fClassNameFull = "";

        #endregion Служебные

        #endregion ПОЛЯ
    }
}