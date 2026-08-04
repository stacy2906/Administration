using nlElements;
using System.Collections;
using System.ComponentModel;

namespace nlElements
{
    /// <summary>
    ///   TextBox class which supports the <see cref="MaskedBehavior">Masked</see> behavior. </summary>	
    [Description("TextBox control which supports the Masked behavior.")]
    public class elmComponentMask : elmTextBox
    {
        /// <summary>
        ///   Initializes a new instance of the MaskedTextBox class by assigning its Behavior field
        ///   to an instance of <see cref="MaskedBehavior" />. </summary>
        public elmComponentMask()
        {
            _fBehavior = new MaskedBehavior(this);
        }

        /// <summary>
        ///   Initializes a new instance of the MaskedTextBox class by assigning its Behavior field
        ///   to an instance of <see cref="MaskedBehavior" /> and setting its mask. </summary>
        /// <param name="mask">
        ///   The mask string to use for validating and/or formatting the characters entered by the user. 
        ///   By default, the <c>#</c> symbol is configured to represent a digit placeholder on the mask. </param>
        public elmComponentMask(string mask)
        {
            _fBehavior = new MaskedBehavior(this, mask);
        }

        /// <summary>
        ///   Initializes a new instance of the MaskedTextBox class by explicitly assigning its Behavior field. </summary>
        /// <param name="behavior">
        ///   The <see cref="MaskedBehavior" /> object to associate the textbox with. </param>
        public elmComponentMask(MaskedBehavior behavior) : base(behavior)
        {
        }

        /// <summary>
        ///   Gets the Behavior object associated with this class. </summary>
        [Browsable(false)]
        public MaskedBehavior Behavior
        {
            get
            {
                return (MaskedBehavior)_fBehavior;
            }
        }

        /// <summary>
        /// Получает или задает маску — строку, используемую для проверки и/или форматирования символов, введенных пользователем
        /// </summary>
        /// <remarks>
        ///   This property delegates to <see cref="MaskedBehavior.__fMask_">MaskedBehavior.Mask</see>. </remarks>
        public string Mask
        {
            get
            {
                return Behavior.__fMask_;
            }
            set
            {
                Behavior.__fMask_ = value;
            }
        }

        /// <summary>
        /// Получает список ArrayList объектов Symbol
        /// </summary>
        /// <remarks>
        ///   This property delegates to <see cref="MaskedBehavior.__fSymbols_">MaskedBehavior.Symbols</see>. </remarks>
        /// <seealso cref="Mask" />
        /// <seealso cref="MaskedBehavior.Symbol" />
        [Browsable(false)]
        public ArrayList Symbols
        {
            get
            {
                return Behavior.__fSymbols_;
            }
        }

        /// <summary>
        /// Извлекает значение из текстового поля без каких-либо нечисловых символов
        /// </summary>
        /// <remarks>
        ///   This property delegates to <see cref="MaskedBehavior.NumericText">MaskedBehavior.NumericText</see>. </remarks>
        [Browsable(false)]
        public string NumericText
        {
            get
            {
                return Behavior.NumericText;
            }
        }
    }

}
