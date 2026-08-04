using System;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentToolbarButtonMenu.cs
    /// </summary>
    /// <remarks>Класс-Компонент кнопки панели управления с выпадающим меню</remarks>
    public class elmComponentToolbarButtonMenu : ToolStripDropDownButton
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public elmComponentToolbarButtonMenu()
        {
            _mObjectAssembly();
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        {
            // Отсутсвует:
            // SuspendLayout(); 

            #region /// Настройка компонентов

            DisplayStyle = ToolStripItemDisplayStyle.Image;
            ShowDropDownArrow = false;

            #endregion Настройка компонента

            // Отсутсвует:
            // ResumeLayout(); 

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется при отпускании кнопки мыши
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
