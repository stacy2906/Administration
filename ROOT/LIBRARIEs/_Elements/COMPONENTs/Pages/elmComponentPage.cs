using System;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentPage.cs
    /// </summary>
    /// <remarks>Класс-Компонент вкладки</remarks>
    public class elmComponentPage : TabPage
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public elmComponentPage()
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


            #endregion Настройка компонента

            ResumeLayout();

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется при активизации вкладки
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
        }

        #endregion Поведение

        #endregion МЕТОДЫ   

        #region = ПОЛЯ

        #region - Скрытые

        /// <summary>
        /// Полное название класса
        /// </summary>
        protected string _fClassNameFull = "";

        #endregion Скрытые

        #region - Закрытые

        /// <summary>
        /// Строка заголовка без перевода
        /// </summary>
        private string fTextWithOutTranslate = "";

        #endregion Закрытые

        #endregion ПОЛЯ    

        #region = СВОЙСТВА

        /// <summary>
        /// Текст заголовка
        /// </summary>
        /// <remarks>Отображаемый текст переводиться на язык интерфейса. Возвращается не переведенный текст</remarks>
        public string __fCaption_
        {
            get { return fTextWithOutTranslate; }
            set
            {
                fTextWithOutTranslate = value.Trim();
                Text = elmApplication.__oTunes.__mTranslate(fTextWithOutTranslate);
            }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при активации вкладки
        /// </summary>
        public event EventHandler __eActivate;

        #endregion СОБЫТИЯ

    }
}
