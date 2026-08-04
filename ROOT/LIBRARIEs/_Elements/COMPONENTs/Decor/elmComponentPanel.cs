using nlApplication;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentPanel.cs
    /// </summary>
    /// <remarks>Класс-Компонент для отображении панели на форме</remarks>
 	/// <version>2026.01.16 09-23</version> // Дата-время последней корректировки
	/// <feature>Работает в Debug и в Release при наличии файла .pdb</feature>
    public class elmComponentPanel : UserControl
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструкторы
        /// </summary>
        public elmComponentPanel()
        {
            _mObjectAssembly();
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        {
            SuspendLayout();

            #region /// Настройка компонента

            BackColor = Color.Transparent;
            BorderStyle = BorderStyle.None;
            _fError = new appUnitError(_fClassFilePath_);
            TabStop = false;

            #endregion Настройка компонента

            ResumeLayout(false);

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected virtual void _mObjectPresentation()
        {
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region * Информация о файле

        /// <summary>
        /// Получение пути и имени файла класса
        /// </summary>
        protected string _mClassFilePath([CallerFilePath] string pFilePath = "")
        {
            return pFilePath;
        }
        /// <summary>
        /// Получение значения номера строки в текущей процедуре
        /// </summary>
        protected int _mClassLine(string pMessage = "", [CallerLineNumber] int pLine = 0)
        {
            return pLine;
        }
        /// <summary>
        /// Получение названия текущей процедуры
        /// </summary>
        protected string _mClassProcedure(string message, [CallerMemberName] string pMember = "")
        {
            return pMember;
        }

        #endregion Информация о файле

        #region - Поведение

        /// <summary>
        /// Выполняется при создании объекта
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _mObjectPresentation();
        }

        #endregion Поведение

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Скрытые

        /// <summary>
        /// Объект ошибки
        /// </summary>
        protected appUnitError _fError;

        #endregion Скрытые

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        #region - Скрытые

        /// <summary>
        /// Путь к файлу текущего класса
        /// </summary>
        protected string _fClassFilePath_
        {
            get { return _mClassFilePath(); }
        }
        /// <summary>
        /// Текущая процедура класса
        /// </summary>
        protected string _fClassProcedure_
        {
            get { return _mClassProcedure(""); }
        }
        /// <summary>
        /// Номер строки в файле класса
        /// </summary>
        protected int _fClassLine_
        {
            get { return _mClassLine(""); }
        }

        #endregion Скрытые

        #endregion = СВОЙСТВА
    }
}
