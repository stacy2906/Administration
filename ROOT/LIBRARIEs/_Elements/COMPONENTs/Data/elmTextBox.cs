using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmTextBox.cs
	/// </summary>
    /// <remarks>
    /// Класс-базис для компонентов TextBox в папке 'Data'
    /// </remarks>
    public abstract class elmTextBox : TextBox
	{
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        protected elmTextBox()
        {
            _mObjectAssembly();
        }
        /// <summary>
        /// Конструктор с параметром 'Поведение'</summary>
        /// <param name="behavior">Поведение</param>
        protected elmTextBox(elmTypeBehavior behavior)
        {
            _fBehavior = behavior;
            _mObjectAssembly();
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        {
            SuspendLayout();

            #region /// Настройка компонента

            __fFillType_ = FILLTYPES.None;
            BackColor = elmApplication.__oInterface.__mColor(COLORS.DataBack);
            Font = elmApplication.__oInterface.__mFont(FONTS.Data);
            ForeColor = elmApplication.__oInterface.__mColor(COLORS.Data);

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

        #region - Поведение

        /// <summary>
        /// Выполняется при первом создании элемента управления
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _mObjectPresentation();

            return;
        }

        #endregion Поведение
        
        #region - Процедуры

        /// <summary>
        /// Проверка, содержит ли текстовое поле допустимое значение
        /// </summary>
        /// <returns>[true] - Значение допустимо, иначе - [false]</returns>
        public bool __mIsValid()
        {
            return _fBehavior.__mIsValid();
        }
        /// <summary>
        /// Добавление или удаление флагов из собственного поведения
        /// </summary>
        /// <param name="pFlags">Биты, которые нужно включить (объединить ИЛИ) или выключить во внутреннем параметре флагов</param>
        /// <param name="pOperationType">[true] - Значение истинно (флаги добавляются), иначе — [false] (флаги удаляются)</param>
        public void __mModifyFlags(int pFlags, bool pOperationType)
        {
            _fBehavior.ModifyFlags(pFlags, pOperationType);
        }
        /// <summary>
        /// Проверка, является ли текст в текстовом поле допустимым, и если нет, обновляет его допустимым значением
        /// </summary>
        /// <returns>[true] - Текст исправлен, иначе [false]</returns>
        public bool __mUpdateText()
        {
            return _fBehavior.UpdateText();
        }
        /// <summary>
        /// Проверка, является ли значение текстового поля допустимым, и если нет, то выполняет действия в соответствии с поведением, заданным параметром <see cref="__fFlags_" />
        /// </summary>
        /// <returns>[true] - Проверка выполнена успешно, иначе - [false]</returns>
        public bool __mValidate()
        {
            return _fBehavior.Validate();
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Закрытые

        /// <summary>
        /// Обязательность заполнения
        /// </summary>
        private FILLTYPES fFillType = FILLTYPES.None;
        /// <summary>
        /// Количество отображаемых символов
        /// </summary>
        private int fSymbolCount = 10;

        #endregion Закрытые

        #region - Скрытые

        /// <summary> 
        /// Объект Behavior, связанный с этим текстовым полем.
        /// </summary>
        protected elmTypeBehavior _fBehavior = null;    // must be initialized by derived classes

        #endregion Скрытые

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        ///  Обязательность заполнения
        /// </summary>
        public FILLTYPES __fFillType_
        {
            get { return fFillType; }
            set
            {
                fFillType = value;
                if (fFillType == FILLTYPES.None)
                    BackColor = elmApplication.__oInterface.__mColor(COLORS.DataBack);
                else
                    BackColor = elmApplication.__oInterface.__mColor(COLORS.DataBackNecessarily);
            }
        }
        /// <summary>
        ///   Gets or sets the flags associated with self's Behavior. </summary>
        /// <remarks>
        ///   This property delegates to <see cref="elmTypeBehavior.Flags">Behavior.Flags</see>. </remarks>
        /// <seealso cref="__mModifyFlags" />
        [Category("Behavior")]
        [Description("The flags (on/off attributes) associated with the Behavior.")]
        public int __fFlags_
        {
            get
            {
                return _fBehavior.Flags;
            }
            set
            {
                _fBehavior.Flags = value;
            }
        }
        /// <summary>
        /// Количество отображаемых символов
        /// </summary>
        public virtual int __fSymbolsCount_
        {
            get { return fSymbolCount; }
            set
            {
                fSymbolCount = value;
                /// Указано количество символов
                if (fSymbolCount > 0)
                {
                    Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    MaxLength = fSymbolCount;
                    if (fSymbolCount > 3)
                        Width = Convert.ToInt32(elmTypeFont.__mMeasureText(fSymbolCount, elmApplication.__oInterface.__mFont(FONTS.Data)).Width);
                    else
                        Width = 10 + Convert.ToInt32(elmTypeFont.__mMeasureText(fSymbolCount, elmApplication.__oInterface.__mFont(FONTS.Data)).Width);
                }
                /// Количество символов не указано
                else
                {
                    MaxLength = 32767;
                    Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                    if (Parent != null)
                    {
                        Width = Parent.Width
                            - Left
                            - elmInterface.__fFormBorderWidth * 2;
                    }
                }

                fSymbolCount = value;
            }
        }

        #endregion СВОЙСТВА

        ///// <summary>
        /////   Show an error message box. </summary>
        ///// <param name="message">
        /////   The message to show. </param>
        ///// <remarks>
        /////   This property delegates to <see cref="elmTypeBehavior.ShowErrorMessageBox">Behavior.ShowErrorMessageBox</see>. </remarks>
        ///// <seealso cref="ShowErrorIcon" />
        ///// <seealso cref="ErrorMessage" />
        //public void ShowErrorMessageBox(string message)
        //{
        //	m_behavior.ShowErrorMessageBox(message);
        //}

        ///// <summary>
        /////   Show a blinking icon next to the textbox with an error message. </summary>
        ///// <param name="message">
        /////   The message to show when the cursor is placed over the icon. </param>
        ///// <remarks>
        /////   This property delegates to <see cref="elmTypeBehavior.ShowErrorIcon">Behavior.ShowErrorIcon</see>. </remarks>
        ///// <seealso cref="ShowErrorMessageBox" />
        ///// <seealso cref="ErrorMessage" />
        //public void ShowErrorIcon(string message)
        //{
        //	m_behavior.ShowErrorIcon(message);
        //}

        ///// <summary>
        /////   Gets the error message used to notify the user to enter a valid value. </summary>
        ///// <remarks>
        /////   This property delegates to <see cref="elmTypeBehavior.ErrorMessage">Behavior.ErrorMessage</see>. </remarks>
        ///// <seealso cref="Validate" />
        ///// <seealso cref="IsValid" />
        //[Browsable(false)]
        //public string ErrorMessage
        //{
        //	get
        //	{
        //		return m_behavior.ErrorMessage;
        //	}
        //}
    }
}
