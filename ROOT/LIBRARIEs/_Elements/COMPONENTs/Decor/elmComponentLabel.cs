using System;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentLabel.cs
    /// </summary>
    /// <remarks>Класс-Компонент для отображения текста на форме</remarks>
	/// <author>Lucasin V.</author> // Автор
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.14 09-19</version> // Дата-время последней корректировки
    public class elmComponentLabel : Label
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public elmComponentLabel()
        {
            _mObjectAssembly();
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        {
            SuspendLayout();

            #region /// Настройки компонента

            AutoSize = true;
            BackColor = Color.Transparent;
            ForeColor = elmApplication.__oInterface.__fColorText;
            fLabelType = LABELTYPES.Normal;

            #endregion Настройка компонента

            ResumeLayout();

            Type vType = this.GetType();
            Name = vType.Name;
            _fClassNameFull = vType.FullName + ".";

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение 

        /// <summary>
        /// Выполняется при отпускании правой кнопки мыши
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs pEvent)
        {
            if (fCaptionClickable == true & Enabled == true)
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

        #region - Внутренние

        /// <summary>
        /// Разрешение обрабатывать клики мыши
        /// </summary>
        private bool fCaptionClickable = true;
        /// <summary>
        /// Доступность компонента
        /// </summary>
        private bool fEnabled = true;
        /// <summary>
        /// Вид надписи
        /// </summary>
        private LABELTYPES fLabelType = LABELTYPES.Normal;
        /// <summary>
        /// Вид надписи определенный программой
        /// </summary>
        private LABELTYPES fLabelTypeSaved = LABELTYPES.Normal;
        /// <summary>
        /// Строка заголовка без перевода
        /// </summary>
        private string fTextWithOutTranslate = "";

        #endregion Внутренние

        #region - Служебные

        /// <summary>
        /// Полное имя класса
        /// </summary>
        protected string _fClassNameFull = "";

        #endregion Служебные

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
                Text = elmApplication.__oTunes.__mTranslate(fTextWithOutTranslate).Trim();
            }
        }
        /// <summary>
        /// Доступность компонента
        /// </summary>
        /// <remarks>Определяет доступность компонента и работу Lustar функционала</remarks>
        public bool __fEnabled_
        {
            get { return fEnabled; }
            set
            {
                fEnabled = value;
                if (value == false)
                {
                    fLabelTypeSaved = __fLabelType_;
                    __fLabelType_ = LABELTYPES.Normal;
                }
                else
                {
                    __fLabelType_ = fLabelTypeSaved;
                }
                fCaptionClickable = value;
            }
        }
        /// <summary>
        /// Вид надписи
        /// </summary>
        public LABELTYPES __fLabelType_
        {
            get { return fLabelType; }
            set
            {
                Cursor = Cursors.Default;
                fLabelType = value;
                switch (fLabelType)
                {
                    case LABELTYPES.Normal:
                        Font = elmApplication.__oInterface.__mFont(FONTS.Text);
                        ForeColor = elmApplication.__oInterface.__mColor(COLORS.Text);
                        //                        fLabelTypeOnLoad = LABELTYPES.Normal;
                        break;
                    case LABELTYPES.Button:
                        Font = elmApplication.__oInterface.__mFont(FONTS.TextButton);
                        ForeColor = elmApplication.__oInterface.__mColor(COLORS.TextButton);
                        //                        fLabelTypeOnLoad = LABELTYPES.Button;   
                        Cursor = Cursors.Hand;
                        break;
                    case LABELTYPES.Title:
                        Font = elmApplication.__oInterface.__mFont(FONTS.TextTitle);
                        ForeColor = elmApplication.__oInterface.__mColor(COLORS.TextTitle);
                        //                        fLabelTypeOnLoad = LABELTYPES.Title;
                        break;
                }
            }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при клике левой кнопки мыши по компоненту
        /// </summary>
        public event EventHandler __eClickLeft;
        /// <summary>
        /// Возникает при клике правой кнопки мыши по компоненту
        /// </summary>
        public event EventHandler __eClickRight;

        #endregion СОБЫТИЯ    
    }
}
