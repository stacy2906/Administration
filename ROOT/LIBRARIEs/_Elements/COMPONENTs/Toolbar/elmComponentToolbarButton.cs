using System;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentToolbarButton.cs
    /// </summary>
    /// <remarks>Класс-компонент кнопки панели управления</remarks>
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 09-55</version> // Дата-время последней корректировки
    public class elmComponentToolbarButton : ToolStripButton
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструкторы
        /// </summary>
        public elmComponentToolbarButton()
        {
            _mObjectAssembly();
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        {
            // SuspendLayout(); - Отсутствует

            #region Настройка компонента

            DisplayStyle = ToolStripItemDisplayStyle.Image;

            #endregion Настройка компонента

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется при отпускании правой кнопки мыши
        /// </summary>
        /// <param name="pEvent"></param>
        protected override void OnMouseUp(MouseEventArgs pEvent)
        {
            if (pEvent.Button == MouseButtons.Left)
            {
                if (__eClickLeft != null)
                    __eClickLeft(this, new EventArgs());
            }
            if (pEvent.Button == MouseButtons.Right)
            {
                if (__eClickRight != null)
                    __eClickRight(this, new EventArgs());
            }

            base.OnMouseUp(pEvent);
        }

        #endregion Поведение

        #region - Процедуры

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

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Закрытые 

        /// <summary>
        /// Текст без перевода
        /// </summary>
        private string fTextWithOutTranslate = "";
        /// <summary>
        /// Полное имя класса
        /// </summary>
        protected string _fClassNameFull = "";

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
                fTextWithOutTranslate = value;
                Text = elmApplication.__oTunes.__mTranslate(fTextWithOutTranslate);
            }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при выборе правой кнопкой мыши
        /// </summary>
        public event EventHandler __eClickRight;
        /// <summary>
        /// Возникает при выборе левой кнопкой мыши
        /// </summary>
        public event EventHandler __eClickLeft;

        #endregion СОБЫТИЯ

    }
}
