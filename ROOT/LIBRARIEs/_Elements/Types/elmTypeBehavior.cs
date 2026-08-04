using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization; 
using System.Runtime.InteropServices; 
using System.Text;
using System.Windows.Forms;

namespace nlElements
{
    #region = ПЕРЕЧИСЛЕНИЯ

    /// <summary>
    /// Значения, которые можно добавлять/удалять из свойства <see cref="elmTypeBehavior.Flags" />, связанные с тем, что происходит, когда текстовое поле теряет фокус
    /// </summary>
    [Flags]
    public enum LostFocusFlag
    {
        /// <summary>
        /// Когда текстовое поле теряет фокус, добавьте к значению до <see cref="MaxWholeDigits" /> нулей слева от десятичной точки.
        /// </summary>
        PadWithZerosBeforeDecimal = 0x00000100,
        /// <summary>
        /// Когда текстовое поле теряет фокус, добавьте к значению до <see cref="MaxDecimalPlaces" /> нулей справа от десятичной точки
        /// </summary>
        PadWithZerosAfterDecimal = 0x00000200,
        /// <summary> 
        /// При использовании в сочетании с <see cref="PadWithZerosBeforeDecimal" /> или <see cref="PadWithZerosAfterDecimal" />, отступы выполняются только в том случае, если текстовое поле не пустое
        /// </summary>
        DontPadWithZerosIfEmpty = 0x00000400,
        /// <summary> 
        /// Незначащие нули удаляются из числового значения слева от десятичной точки, за исключением случаев, когда само число равно 0
        /// </summary>
        RemoveExtraLeadingZeros = 0x00000800,
        /// <summary> 
        /// Комбинация всех вышеперечисленных флагов; используется внутри программы
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)] Max = 0x00000F00,
        /// <summary> 
        /// Если свойство Text задано, вызывается обработчик LostFocus
        /// </summary>
        CallHandlerWhenTextPropertyIsSet = 0x00001000,
        /// <summary> 
        /// Если текст изменяется, вызывается обработчик LostFocus
        /// </summary>
        CallHandlerWhenTextChanges = 0x00002000
    };
    /// <summary>
    /// Значения, которые можно добавлять/удалять из свойства <see cref="elmTypeBehavior.Flags" />, связанные с проверкой текстового поля
    /// </summary>
    [Flags]
    public enum ValidatingFlag
    {
        /// <summary> 
        /// Если значение недопустимо, издайте звуковой сигнал
        /// </summary>
        Beep_IfInvalid = 0x00000001,
        /// <summary> 
        /// Если значение пустое, издать звуковой сигнал
        /// </summary>
        Beep_IfEmpty = 0x00000002,
        /// <summary> 
        /// Если значение пустое или недопустимое, издать звуковой сигнал
        /// </summary>
        Beep = Beep_IfInvalid | Beep_IfEmpty,
        /// <summary> 
        /// Если значение недопустимо, измените его на допустимое
        /// </summary>
        SetValid_IfInvalid = 0x00000004,
        /// <summary> 
        /// Если значение пустое, измените его на допустимое
        /// </summary>
        SetValid_IfEmpty = 0x00000008,
        /// <summary> 
        /// Если значение пустое или недопустимое, измените его на допустимое
        /// </summary>
        SetValid = SetValid_IfInvalid | SetValid_IfEmpty,
        /// <summary> 
        /// Если значение недопустимо, отобразить окно с сообщением об ошибке
        /// </summary>
        ShowMessage_IfInvalid = 0x00000010,
        /// <summary> 
        /// Если значение пустое, отобразить окно с сообщением об ошибке
        /// </summary>
        ShowMessage_IfEmpty = 0x00000020,
        /// <summary> 
        /// Если значение пустое или недопустимое, отобразить окно с сообщением об ошибке
        /// </summary>
        ShowMessage = ShowMessage_IfInvalid | ShowMessage_IfEmpty,
        /// <summary> 
        /// Если значение недопустимо, рядом с ним отобразится мигающий значок
        /// </summary>
        ShowIcon_IfInvalid = 0x00000040,
        /// <summary> 
        /// Если значение пустое, рядом с ним отобразится мигающий значок
        /// </summary>
        ShowIcon_IfEmpty = 0x00000080,
        /// <summary>
        /// Если значение пустое или недопустимое, рядом с ним отображается мигающий значок
        /// </summary>
        ShowIcon = ShowIcon_IfInvalid | ShowIcon_IfEmpty,
        /// <summary> 
        /// Комбинация всех флагов IfInvalid (указанных выше); используется внутри программы
        /// </summary>
		Max_IfInvalid = Beep_IfInvalid | SetValid_IfInvalid | ShowMessage_IfInvalid | ShowIcon_IfInvalid,
        /// <summary> 
        /// Комбинация всех флагов IfEmpty (указанных выше); используется внутри программы
        /// </summary>
		Max_IfEmpty = Beep_IfEmpty | SetValid_IfEmpty | ShowMessage_IfEmpty | ShowIcon_IfEmpty,
        /// <summary> 
        /// Комбинация всех флагов; используется внутри программы
        /// </summary>
		Max = Max_IfInvalid + Max_IfEmpty
    };

    #endregion ПЕРЕЧИСЛЕНИЯ

    /// <summary>
    /// Файл elmTypeBehavior.cs
    /// </summary>
    /// <remarks>Класс-базис для всех классов поведения в этом пространстве имен. Он предназначен для представления объекта поведения, который может быть связан с объектом TextBoxBase.</remarks>
    public abstract class elmTypeBehavior : IDisposable
	{
        /// <summary>  
        ///   If TRACE_AMS (and TRACE) are defined for the compiler, a message line is sent to the tracer. </summary>
        /// <param name="message">
        ///   The message line to trace. </param>
        /// <remarks>
        ///   This method is used to help diagnose problems.  It's called at the beginning of all 
        ///   event handlers (the ones that begin with Handle) to trace the program's execution. </remarks>
        [Conditional("TRACE_AMS")]
        public void TraceLine(string message)
        {
            Trace.WriteLine(message);
        }
        
		#region = БИБЛИОТЕКИ

        /// <summary>
        /// Издает звуковой сигнал
        /// </summary>
        /// <param name="mbi">Тип звука, который следует издавать в зависимости от ситуации</param>
        [DllImport("user32.dll")]
        protected static extern bool MessageBeep(MessageBoxIcon mbi);

        #endregion БИБЛИОТЕКИ

        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Инициализирует новый экземпляр класса Behavior, связывая его с объектом, производным от TextBoxBase
        /// </summary>
        /// <param name="textBox">Объект TextBoxBase, который необходимо связать с этим поведением. Он не должен быть равен null</param>
        /// <param name="addEventHandlers">Если значение равно true, обработчики событий текстового поля привязаны к соответствующим методам этого объекта поведения</param>
        /// <exception cref="ArgumentNullException">Текстовое поле равно [null]</exception>
        /// <remarks>
        /// Этот конструктор является «защищенным», поскольку данный класс предназначен только для использования в качестве основы для других функций
        /// </remarks>
        protected elmTypeBehavior(TextBoxBase textBox, bool addEventHandlers)
        {
            if (textBox == null)
                throw new ArgumentNullException("textBox");

            m_textBox = textBox;
            m_selection = new elmTypeSelection(m_textBox);
            m_selection.TextChanging += new EventHandler(HandleTextChangingBySelection);

            if (addEventHandlers)
                AddEventHandlers();
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса Behavior, копируя его из другого объекта Behavior
        /// </summary>
        /// <param name="behavior">Объект Behavior, который необходимо скопировать (а затем удалить). Он не должен быть пустым [null]</param>
        /// <exception cref="ArgumentNullException">Поведение отсутствует</exception>
        /// <remarks>
        /// Этот конструктор защищен, поскольку данный класс предназначен только для использования в качестве основы для других типов поведения. После копирования объекта behavior.TextBox вызывается метод Dispose для параметра behavior
		/// </remarks>
        protected elmTypeBehavior(elmTypeBehavior behavior)
        {
            if (behavior == null)
                throw new ArgumentNullException("behavior");

            TextBox = behavior.TextBox;
            m_flags = behavior.m_flags;

            behavior.Dispose();
        }
        /// <summary>
        /// Disposes of the object by dettaching the textBox event handlers from their corresponding virtual 
        /// methods of the Behavior class and setting the Textbox to null 
        /// </summary>
        public virtual void Dispose()
        {
            RemoveEventHandlers();
            m_textBox = null;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Обрабатывает изменения, внесенные в свойство DataBindings элемента управления.
        /// </summary>
        /// <param name="sender">Объект, отправивший событие.</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод является виртуальным, поэтому его можно переопределить за водные занятия, чтобы завершить их субботними новостями. 
        /// Здесь вчера был ли объект Binding, добавленный в коллекцию DataBindings. 
        /// с событием Parse можно было связаться с методами <see cref="HandleBindingFormat" /> и
        /// <см. cref="HandleBindingParse" />
        /// </remarks>
        protected virtual void HandleBindingChanges(object sender, CollectionChangeEventArgs e)
        {
            if (e.Action == CollectionChangeAction.Add)
            {
                Binding binding = (Binding)e.Element;
                binding.Format += new ConvertEventHandler(HandleBindingFormat);
                binding.Parse += new ConvertEventHandler(HandleBindingParse);
            }
        }
        /// <summary>
        /// Handles when the value of the object bound to this control needs to be formatted to be placed on the control
        /// </summary>
        /// <param name="sender">The object who sent the event</param>
        /// <param name="e">The event data</param>
        /// <remarks>
        /// This method is virtual so that it can be overriden by derived classes to accomodate their own behavior.
        /// Here it does nothing
        /// </remarks>
        protected virtual void HandleBindingFormat(object sender, ConvertEventArgs e)
        {
        }
        /// <summary>
        /// Обрабатывает ситуацию, когда текст элемента управления анализируется и преобразуется в тип, ожидаемый объектом, к которому он привязан.
        /// </summary>
        /// <param name="sender">Объект, отправивший событие.</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод является виртуальным, поэтому его можно переопределить производными классами для учета их собственного поведения.
        /// Здесь проверяется, пуст ли текст элемента управления, чтобы установить его значение равным DBNull.Value
        /// </remarks>
        protected virtual void HandleBindingParse(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() == "")
                e.Value = DBNull.Value;
        }
        /// <summary>
        /// Обрабатывает нажатия клавиш внутри текстового поля
        /// </summary>
        /// <param name="sender">Объект, отправивший событие</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод является виртуальным, поэтому его можно переопределить производными классами для учета их собственного поведения. 
        /// Здесь он просто устанавливает e.Handled в false, чтобы могло произойти нажатие клавиши.		
        /// </remarks>
        protected virtual void HandleKeyDown(object sender, KeyEventArgs e)
        {
            TraceLine("Behavior.HandleKeyDown " + e.KeyCode);

            e.Handled = false;
        }
        /// <summary>
        /// Обрабатывает нажатия клавиш внутри текстового поля
        /// </summary>
        /// <param name="sender">Объект, отправивший событие</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод является виртуальным, поэтому его можно переопределить производными классами для учета их собственного поведения. 
        /// Здесь он просто устанавливает e.Handled в false, чтобы могло произойти нажатие клавиши
        /// </remarks>
        protected virtual void HandleKeyPress(object sender, KeyPressEventArgs e)
        {
            TraceLine("Behavior.HandleKeyPress " + e.KeyChar);

            e.Handled = false;
        }
        /// <summary>
        /// Обрабатывает ситуацию, когда элемент управления теряет фокус
        /// </summary>
        /// <param name="sender">Объект, отправивший событие.</param>
        /// <param name="e">The event data</param>
        /// <remarks>
        /// Этот метод является виртуальным, поэтому его можно переопределить в производных классах для учета их собственного поведения. Здесь же он ничего не делает
        /// </remarks>
        protected virtual void HandleLostFocus(object sender, EventArgs e)
        {
            TraceLine("Behavior.HandleLostFocus");
        }
        /// <summary>
        /// Обрабатывает изменения текста в текстовом поле
        /// </summary>
        /// <param name="sender">Объект, отправивший событие</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод является виртуальным, поэтому его можно переопределить производными классами для учета их собственного поведения.
        /// Здесь вызывается <see cref="UpdateText" /> (если внутренний флаг <see cref="m_noTextChanged" /> не равен <c>true</c>)
        /// чтобы гарантировать корректность текста.
        /// </remarks>
        protected virtual void HandleTextChanged(object sender, EventArgs e)
        {
            TraceLine("Behavior.HandleTextChanged " + m_noTextChanged);

            if (!m_noTextChanged)
                UpdateText();

            m_noTextChanged = false;
        }
        /// <summary>
        /// Обрабатывает изменение текста в результате непосредственного манипулирования выделенным фрагментом
        /// </summary>
        /// <remarks>
        /// Этот метод устанавливает флаг m_noTextChanged в значение true, чтобы метод UpdateText не вызывался без необходимости внутри метода HandleTextChanged
        /// </remarks>
        private void HandleTextChangingBySelection(object sender, EventArgs e)
        {
            m_noTextChanged = true;
        }
        /// <summary>
        /// Обрабатывает ситуацию, когда элемент управления, находящийся в процессе проверки, теряет фокус
        /// </summary>
        /// <param name="sender">Объект, отправивший событие</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод вызывает <see cref="Validate" /> для определения допустимости значения текстового поля, и
        /// возвращаемое значение используется для установки <see cref="CancelEventArgs.Cancel">e.Cancel</see>.
        /// Хотя это и не ожидается, этот метод может быть переопределен производными классами для учета их собственного поведения. 
		/// </remarks>
        protected virtual void HandleValidating(object sender, CancelEventArgs e)
        {
            TraceLine("Behavior.HandleValidating");

            e.Cancel = !Validate();
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Прикрепляет несколько обработчиков событий текстового поля к соответствующим виртуальным методам класса Behavior
        /// </summary>
        /// <remarks>
        /// Для изменения поведения текстового поля могут потребоваться следующие события: KeyDown, KeyPress, TextChanged, Validating и LostFocus.
        /// Этот метод связывает эти события с виртуальными методами: HandleKeyDown, HandleKeyPress, HandleTextChanged, HandleValidating и HandleLostFocus.
        /// Производные классы поведения могут переопределять любой из этих методов для удовлетворения своих собственных требований
        /// </remarks>
        protected virtual void AddEventHandlers()
        {
            m_textBox.KeyDown += new KeyEventHandler(HandleKeyDown);
            m_textBox.KeyPress += new KeyPressEventHandler(HandleKeyPress);
            m_textBox.TextChanged += new EventHandler(HandleTextChanged);
            m_textBox.Validating += new CancelEventHandler(HandleValidating);
            m_textBox.LostFocus += new EventHandler(HandleLostFocus);
            m_textBox.DataBindings.CollectionChanged += new CollectionChangeEventHandler(HandleBindingChanges);
        }
        /// <summary>
        /// Извлекает текст из текстового поля в корректном виде
        /// </summary>
        /// <returns>
        /// Если текст в текстовом поле допустим, он возвращается; в противном случае возвращается допустимая версия текста
        /// </returns>
        /// <remarks>
        /// Этот метод предназначен для переопределения производными классами Behavior. Здесь он просто возвращает текст из текстового поля
        /// </remarks>
        protected virtual string GetValidText()
        {
            return m_textBox.Text;
        }
        /// <summary>
        /// Проверяет, установлено ли значение флага (включено ли оно).
        /// </summary>
        /// <param name="flag">Флаг для проверки</param>
        /// <returns>
        /// Если флаг установлен, возвращаемое значение равно true; в противном случае — false
        /// </returns>
        public bool HasFlag(int flag)
        {
            return (m_flags & flag) != 0;
        }
        /// <summary>
        /// Проверяет, содержит ли текстовое поле допустимое значение
        /// </summary>
        /// <returns>
        /// Если значение допустимо, возвращаемое значение равно true; в противном случае — false
        /// </returns>
        /// <remarks>
        /// Этот метод вызывается классом <see cref="Validate" /> для проверки допустимости. Здесь он просто возвращает true,
        /// но он предназначен для переопределения классами поведения, где либо допустимый диапазон значений не контролируется
        /// по мере ввода пользователем (например, NumericBehavior, TimeBehavior), либо значение не считается
        /// допустимым, пока пользователь не введет все необходимые символы (например, DateBehavior, TimeBehavior)		/// </remarks>
        public virtual bool __mIsValid()
        {
            return true;
        }
        /// <summary>
        /// Добавляет или удаляет флаги из поведения
        /// </summary>
        /// <param name="flags">Биты, которые нужно включить (объединить ИЛИ) или выключить во внутреннем параметре флагов</param>
        /// <param name="addOrRemove">If true the flags are added, otherwise they're removed</param>
        /// <remarks>
        /// Этот метод — удобный способ изменить свойство <see cref="Flags" /> без перезаписи его значения. Если изменяется внутреннее значение флагов, автоматически вызывается метод <see cref="UpdateText" />
        /// </remarks>
        public void ModifyFlags(int flags, bool addOrRemove)
        {
            if (addOrRemove)
                Flags = m_flags | flags;
            else
                Flags = m_flags & ~flags;
        }
        /// <summary>
        /// Отсоединяет несколько обработчиков событий текстового поля от соответствующих виртуальных методов класса Behavior.
        /// </summary>
        /// <remarks>
        /// Этот метод делает противоположное тому, что делает <see cref="AddEventHandlers" />, и позволяет связать объект Behavior с
        /// текстовым полем и впоследствии заменить его другим объектом Behavior
        /// </remarks>
        protected virtual void RemoveEventHandlers()
        {
            if (m_textBox == null)
                return;

            m_textBox.KeyDown -= new KeyEventHandler(HandleKeyDown);
            m_textBox.KeyPress -= new KeyPressEventHandler(HandleKeyPress);
            m_textBox.TextChanged -= new EventHandler(HandleTextChanged);
            m_textBox.Validating -= new CancelEventHandler(HandleValidating);
            m_textBox.LostFocus -= new EventHandler(HandleLostFocus);
            m_textBox.DataBindings.CollectionChanged -= new CollectionChangeEventHandler(HandleBindingChanges);
        }
        /// <summary>
        /// Рядом с текстовым полем отображается мигающий значок с сообщением об ошибке
        /// </summary>
        /// <param name="message">Сообщение, которое отображается при наведении курсора на значок</param>
        /// <remarks>
        ///Хотя это и не ожидается, данный метод может быть переопределен производными классами
        /// </remarks>
        public virtual void ShowErrorIcon(string message)
        {
            if (m_errorProvider == null)
            {
                if (message == "")
                    return;
                m_errorProvider = new ErrorProvider();
            }
            m_errorProvider.SetError(m_textBox, message);
        }
        /// <summary>
        /// Отображает окно с сообщением об ошибке
        /// </summary>
        /// <param name="message">Сообщение, которое нужно показать</param>
        /// <remarks>
        /// Хотя это и не ожидается, данный метод может быть переопределен производными классами
        /// </remarks>
        public virtual void ShowErrorMessageBox(string message)
        {
            MessageBox.Show(m_textBox, message, ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        /// <summary>
        /// Преобразует заданный текст в число с плавающей запятой типа double
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это текст в формате double или 0, если преобразование невозможно.
        /// </returns>
        /// <remarks>
        /// Этот метод удобен для производных классов поведения, которым необходимо преобразовать строку в тип double, не опасаясь возникновения исключения System.FormatException
        /// </remarks>
        /// <seealso cref="ToInt" />	
        protected double ToDouble(String text)
        {
            try
            {
                return Convert.ToDouble(text);
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// Преобразует заданный текст в целое число
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это текст в виде целого числа или 0, если преобразование невозможно
        /// </returns>
        /// <remarks>
        /// Этот метод удобен для производных классов поведения, которым необходимо преобразовывать строку в целое число, не опасаясь возникновения исключения System.FormatException
        /// </remarks>
        protected int ToInt(String text)
        {
            try
            {
                // Make it work like "atoi" -- ignore any trailing non-digit characters
                for (int i = 0, length = text.Length; i < length; i++)
                {
                    if (!Char.IsDigit(text[i]))
                        return Convert.ToInt32(text.Substring(0, i));
                }

                return Convert.ToInt32(text);
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// Проверяет, является ли текст в текстовом поле допустимым, и если нет, обновляет его допустимым значением
        /// </summary>
        /// <returns>
        /// Если текст в текстовом поле обновляется (поскольку он был недействительным), возвращаемое значение равно true; в противном случае — false
        /// </returns>
        /// <remarks>
        /// Этот метод используется производными классами для обеспечения корректности текста в текстовом поле
        /// </remarks>
        public virtual bool UpdateText()
        {
            string validText = GetValidText();
            if (validText != m_textBox.Text)
            {
                m_textBox.Text = validText;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Проверяет, является ли значение текстового поля допустимым, и если нет, то выполняет действия в соответствии с поведением, заданным параметром <see cref="Flags" />.
        /// </summary>
        /// <returns>
        /// Если проверка прошла успешно, возвращаемое значение равно true; в противном случае — false
        /// </returns>
        /// <remarks>
        /// Этот метод автоматически вызывается событием <see cref="Control.Validating" /> текстового поля, если его свойство
        /// <see cref="Control.CausesValidation" /> установлено в значение true.
        /// Он делегирует вызов переопределяемой версии метода Validate.
        /// </remarks>
        public bool Validate()
        {
            return Validate(Flags, false);
        }
        /// <summary>
        /// Проверяет, является ли значение в текстовом поле допустимым, и если нет, продолжает выполнение в соответствии с заданным набором флагов
        /// </summary>
        /// <param name="flags">
        /// Комбинация из нуля или более значений <see cref="ValidatingFlag" />, сложенных (объединенных оператором ИЛИ).
        /// Это определяет, следует ли проверять значение на пустоту, недопустимость или ни то, ни другое, и какое действие следует предпринять</param>
        /// <param name="setFocusIfNotValid">Если значение истинно и проверка не пройдена (на основе параметра flags), фокус устанавливается на текстовое поле</param>
        /// <returns>
        /// Если проверка прошла успешно, возвращаемое значение равно true; в противном случае — false
        /// </returns>
        /// <remarks>
        /// Этот метод косвенно вызывается событием <see cref="Control.Validating" /> текстового поля, если его свойство
        /// <see cref="Control.CausesValidation" /> установлено в значение true.
        /// Хотя это и не ожидается, этот метод может быть переопределен для обеспечения дополнительной проверки в производных классах.
        /// </remarks>
        public virtual bool Validate(int flags, bool setFocusIfNotValid)
        {
            ShowErrorIcon("");  // clear the icon if it's being shown

            // Check if any of the flags are set
            if ((flags & (int)ValidatingFlag.Max) == 0)
                return true;

            // If we care about the value being empty, check and take the proper action
            if ((flags & (int)ValidatingFlag.Max_IfEmpty) != 0 && m_textBox.Text == "")
            {
                if ((flags & (int)ValidatingFlag.Beep_IfEmpty) != 0)
                    MessageBeep(MessageBoxIcon.Exclamation);

                if ((flags & (int)ValidatingFlag.SetValid_IfEmpty) != 0)
                {
                    UpdateText();
                    return true;
                }

                if ((flags & (int)ValidatingFlag.ShowIcon_IfEmpty) != 0)
                    ShowErrorIcon(ErrorMessage);

                if ((flags & (int)ValidatingFlag.ShowMessage_IfEmpty) != 0)
                    ShowErrorMessageBox(ErrorMessage);

                if (setFocusIfNotValid)
                    m_textBox.Focus();

                return false;
            }

            // If we care about the value being invalid, check and take the proper action
            if ((flags & (int)ValidatingFlag.Max_IfInvalid) != 0 && m_textBox.Text != "" && !__mIsValid())
            {
                if ((flags & (int)ValidatingFlag.Beep_IfInvalid) != 0)
                    MessageBeep(MessageBoxIcon.Exclamation);

                if ((flags & (int)ValidatingFlag.SetValid_IfInvalid) != 0)
                {
                    UpdateText();
                    return true;
                }

                if ((flags & (int)ValidatingFlag.ShowIcon_IfInvalid) != 0)
                    ShowErrorIcon(ErrorMessage);

                if ((flags & (int)ValidatingFlag.ShowMessage_IfInvalid) != 0)
                    ShowErrorMessageBox(ErrorMessage);

                if (setFocusIfNotValid)
                    m_textBox.Focus();

                return false;
            }

            return true;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        /// <summary> 
        /// Объект TextBox, связанный с этим поведением
        /// </summary>
        protected TextBoxBase m_textBox;
        /// <summary> 
        /// Для этого поведения были включены соответствующие флаги
        /// </summary>
        protected int m_flags;
        /// <summary> 
        /// Если значение равно true, это означает, что метод HandleTextChanged должен вести себя так, как если бы текст не изменился, и не вызывать метод <see cref="UpdateText" />
        /// </summary>
        protected bool m_noTextChanged;
        /// <summary> 
        /// Вспомогательный объект, используемый для управления выделением объекта TextBox
        /// </summary>
        protected elmTypeSelection m_selection;
        /// <summary> 
        /// Объект, используемый для отображения мигающего значка (с сообщением об ошибке) рядом с элементом управления
        /// </summary>		
        protected ErrorProvider m_errorProvider;
        /// <summary> 
        /// Заголовок, используемый для всех окон с сообщениями об ошибках
        /// </summary>		
        private static string 	m_errorCaption;

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Получает или задает заголовок, используемый для всех окон с сообщениями об ошибках
        /// </summary>
        /// <remarks>
        /// Это свойство можно использовать для изменения заголовка по умолчанию (<see cref="Application.ProductName" />), используемого для всех окон сообщений об ошибках, отображаемых с помощью метода <see cref="ShowErrorMessageBox" />
        /// </remarks>
        public static string ErrorCaption
        {
            get
            {
                if (m_errorCaption == null)
                    return Application.ProductName;
                return m_errorCaption;
            }
            set
            {
                m_errorCaption = value;
            }
        }
        /// <summary>
        /// Получает сообщение об ошибке, используемое для уведомления пользователя о необходимости ввести допустимое значение
        /// </summary>
        /// <remarks>
        /// Это свойство используется классом <see cref="Validate" /> для получения сообщения, которое будет отображаться в диалоговом окне или на значке, если проверка не удалась, в зависимости от флагов, установленных пользователем.
        /// Здесь отображается только общее сообщение об ошибке, но предполагается, что оно будет переопределено
        /// классами поведения, в которых либо допустимый диапазон значений не контролируется по мере ввода пользователем (например, NumericBehavior, TimeBehavior), либо значение не считается
        /// действительным, пока пользователь не введет все необходимые символы (например, DateBehavior, TimeBehavior)
        /// </remarks>
        public virtual string ErrorMessage
        {
            get
            {
                return "Please specify a valid value.";
            }
        }
        /// <summary>
        /// Получает или задает флаги, связанные с этим объектом поведения
        /// </summary>
        /// <remarks>
        /// Это свойство служит для удобства производных классов поведения, которые могут использовать его для хранения бинарных атрибутов (флагов) внутри отдельных битов. Если это свойство изменяется, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />
        /// </remarks>
        public virtual int Flags
        {
            get
            {
                return m_flags;
            }
            set
            {
                if (m_flags == value)
                    return;

                m_flags = value;
                UpdateText();
            }
        }
        /// <summary>
        /// Получает или задает объект TextBoxBase, связанный с этим объектом Behavior (который не может быть равен null)
        /// </summary>
        /// <remarks>
        /// Перед заменой объекта TextBoxBase его обработчики событий отсоединяются от этого объекта поведения. Затем они прикрепляются к новому объекту
        /// </remarks>
        public TextBoxBase TextBox
        {
            get
            {
                return m_textBox;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                RemoveEventHandlers();

                m_textBox = value;
                m_selection = new elmTypeSelection(m_textBox);
                m_selection.TextChanging += new EventHandler(HandleTextChangingBySelection);

                AddEventHandlers();
            }
        }

        #endregion СВОЙСТВА
	}

    /// <summary>
    /// Класс поведения, который предотвращает ввод одного или нескольких символов
    /// </summary>
    public class AlphanumericBehavior : elmTypeBehavior
	{
        #region = ПОЛЯ

        /// <summary>
        /// Список недопустимых символов
        /// </summary>
        private char[] fCharsInvalid = { '%', '\'', '*', '"', '+', '?', '>', '<', ':', '\\' };

        #endregion ПОЛЯ

        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Инициализирует новый экземпляр класса AlphanumericBehavior, копируя его из другого объекта AlphanumericBehavior.
        /// </summary>
        /// <param name="behavior">Объект AlphanumericBehavior, который необходимо скопировать (а затем удалить). Он не должен быть равен null</param>
        /// <remarks>
        /// После копирования объекта behavior.TextBox вызывается метод Dispose для параметра behavior
        /// </remarks>
        public AlphanumericBehavior(AlphanumericBehavior behavior) : base(behavior)
        {
            fCharsInvalid = behavior.fCharsInvalid;
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса AlphanumericBehavior, связывая его с объектом, производным от TextBoxBase
        /// </summary>
        /// <param name="textBox">Объект TextBoxBase, который необходимо связать с этим поведением. Он не должен быть равен null</param>
        /// <exception cref="ArgumentNullException">текстовое поле пустое</exception>
        /// <remarks>
        /// Этот конструктор устанавливает недопустимые символы в значения %, ', *, ", +, ?, >, &lt;, : и \
        /// </remarks>
        public AlphanumericBehavior(TextBoxBase textBox) : base(textBox, true)
        {
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса AlphanumericBehavior, связывая его с объектом, производным от TextBoxBase, и устанавливая для него недопустимые символы
        /// </summary>
        /// <param name="textBox">Объект TextBoxBase, который необходимо связать с этим поведением. Он не должен быть равен [null]</param>
        /// <param name="invalidChars">Набор символов, который не должен быть разрешен</param>
        public AlphanumericBehavior(TextBoxBase textBox, char[] invalidChars) : base(textBox, true)
        {
            fCharsInvalid = invalidChars;
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса AlphanumericBehavior, связывая его с объектом, производным от TextBoxBase, и устанавливая для него недопустимые символы
		/// </summary>
        /// <param name="textBox">Объект TextBoxBase, который необходимо связать с этим поведением. Он не должен быть равен null.</param>
        /// <param name="invalidChars">Набор запрещенных символов, объединенных в строку</param>
        public AlphanumericBehavior(TextBoxBase textBox, string invalidChars) : base(textBox, true)
        {
            fCharsInvalid = invalidChars.ToCharArray();
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Обрабатывает нажатия клавиш внутри текстового поля
        /// </summary>
        /// <param name="sender">Объект, отправивший событие.</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод переопределен в классе Behavior и обрабатывает событие KeyPress текстового поля.
        /// </remarks>
        protected override void HandleKeyPress(object sender, KeyPressEventArgs e)
        {
            TraceLine("AlphanumericBehavior.HandleKeyPress " + e.KeyChar);

            // Check to see if it's read only
            if (m_textBox.ReadOnly || fCharsInvalid == null)
                return;

            char c = e.KeyChar;
            e.Handled = true;

            // Check if the character is invalid				
            if (Array.IndexOf(fCharsInvalid, c) >= 0)
            {
                MessageBeep(MessageBoxIcon.Exclamation);
                return;
            }

            // If the number of characters is already at Max, overwrite
            string text = m_textBox.Text;
            if (text.Length == m_textBox.MaxLength && m_textBox.MaxLength > 0 && !Char.IsControl(c))
            {
                int start, end;
                m_selection.Get(out start, out end);

                if (start < m_textBox.MaxLength)
                    m_selection.SetAndReplace(start, start + 1, c.ToString());

                return;
            }

            base.HandleKeyPress(sender, e);
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Извлекает текст из текстового поля в корректном виде
        /// </summary>
        /// <returns>
        /// Если текст в текстовом поле допустим, он возвращается; в противном случае возвращается допустимая версия текста
        /// </returns>
        protected override string GetValidText()
        {
            string text = m_textBox.Text;

            // Check if there are any invalid characters and if so, remove them
            if (fCharsInvalid != null && text.IndexOfAny(fCharsInvalid) >= 0)
            {
                // There are invalid characters -- remove them
                foreach (char c in fCharsInvalid)
                {
                    if (text.IndexOf(c) >= 0)
                        text = text.Replace(c.ToString(), "");
                }
            }

            // Check the max length
            if (text.Length > m_textBox.MaxLength)
                text = text.Remove(m_textBox.MaxLength, text.Length - m_textBox.MaxLength);

            return text;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = СВОЙСТВА

        /// <summary>
        /// Получает или задает массив недопустимыми символов
        /// </summary>
        public char[] __fInvalidChars_
        {
            get
            {
                return fCharsInvalid;
            }
            set
            {
                if (fCharsInvalid == value)
                    return;

                fCharsInvalid = value;
                UpdateText();
            }
        }

        #endregion СВОЙСТВА
	}

    /// <summary>
    /// Класс поведения, ограничивающий ввод на основе маски, содержащей один или несколько специальных символов
    /// </summary>
    /// <remarks>
    /// Этот класс полезен для значений со строгим форматом, таких как номера телефонов, номера социального страхования или почтовые индексы
    /// </remarks>
    public class MaskedBehavior : elmTypeBehavior
	{
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Инициализирует новый экземпляр класса MaskedBehavior, копируя его из другого объекта MaskedBehavior.
        /// </summary>
        /// <param name="behavior">Объект MaskedBehavior, который необходимо скопировать (а затем удалить). Он не должен быть равен null.</param>
        /// <exception cref="ArgumentNullException">поведение равно нулю</exception>
        /// <remarks>
        /// После копирования объекта behavior.TextBox вызывается метод Dispose для параметра behavior
        /// </remarks>
        public MaskedBehavior(MaskedBehavior behavior) : base(behavior)
        {
            fMask = behavior.fMask;
            fSymbolS = behavior.fSymbolS;
        }
        /// <summary>
        /// Initializes a new instance of the MaskedBehavior class by associating it with a TextBoxBase derived object
        /// </summary>
        /// <param name="textBox">The TextBoxBase object to associate with this behavior It must not be null</param>
        /// <remarks>
        /// This constructor sets the mask to an empty string, so that anything is allowed</remarks>
        public MaskedBehavior(TextBoxBase textBox) : this(textBox, "")
        {
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса MaskedBehavior, связывая его с объектом, производным от TextBoxBase, и устанавливая его маску.
        /// </summary>
        /// <param name="textBox">Объект TextBoxBase, который необходимо связать с этим поведением. Он не должен быть равен null.</param>
        /// <param name="mask">Строка-маска, используемая для проверки и/или форматирования символов, введенных пользователем
        /// По умолчанию символ '#' используется в качестве заполнителя для цифры в маске</param>
        /// <example>
        /// MaskedBehavior behavior = new MaskedBehavior(txtPhoneNumber, "###-####");
        /// </example>
        public MaskedBehavior(TextBoxBase textBox, string mask) : base(textBox, true)
        {
            fMask = mask;

            // Add the default numeric symbol
            fSymbolS.Add(new Symbol('#', new Symbol.ValidatorMethod(Char.IsDigit)));
        }

        #endregion ДИЗАЙНЕРЫ

        #region = КЛАССЫ

        /// <summary>
        /// Представляет собой символ, который может быть добавлен к маске, а затем интерпретирован классом <see cref="MaskedBehavior" />
        /// для проверки ввода пользователя и, возможно, форматирования его во что-либо другое
        /// </summary>
        public class Symbol
        {
            #region = ДИЗАЙНЕРЫ

            /// <summary>
            /// Инициализирует новый экземпляр класса Symbol, связывая его с символом
            /// </summary>
            /// <param name="symbol">Символ, который представлен этим объектом в строке маски</param>
            /// <remarks>
            /// Символ, который представлен этим объектом в строке маски
            /// </remarks>
            public Symbol(char symbol) : this(symbol, null, null)
            {
            }
            /// <summary>
            /// Инициализирует новый экземпляр класса Symbol, связывая его с символом и методом валидатора
			/// </summary>
            /// <param name="symbol">Символ, который представлен этим объектом в строке маски.</param>
            /// <param name="validator">Вызывается метод для проверки соответствия введенного пользователем символа символу данного объекта.</param>
            /// <remarks>
            /// Этот конструктор устанавливает метод форматирования в значение null, что означает, что введенный пользователем символ не форматируется.
			/// </remarks>
            /// <seealso cref="MaskedBehavior" />
            public Symbol(char symbol, ValidatorMethod validator) : this(symbol, validator, null)
            {
            }
            /// <summary>
            /// Инициализирует новый экземпляр класса Symbol, связывая его с символом и методом валидатора.
			/// </summary>
            /// <param name="symbol">Символ, который представлен этим объектом в строке маски.</param>
            /// <param name="validator">Вызывается метод для проверки соответствия введенного пользователем символа символу данного объекта.</param>
            /// <param name="formatter">Этот метод вызывается для форматирования введенного пользователем символа на другой символ, если это необходимо</param>
            public Symbol(char symbol, ValidatorMethod validator, FormatterMethod formatter)
            {
                m_symbol = symbol;
                Validator = validator;
                Formatter = formatter;
            }

            #endregion ДИЗАЙНЕРЫ

            #region = МЕТОДЫ

            /// <summary>
            /// Позволяет преобразовывать/приводить объект Symbol в его символьное представление
            /// </summary>
            /// <example>
            /// MaskedBehavior.Symbol s = new MaskedBehavior.Symbol('#');
            ///   char c = s; 
            /// </example>
            public static implicit operator char(Symbol s)
            {
                return s.Char;
            }
            /// <summary>
            /// Форматирует введенный пользователем символ, заменяя его другим символом
            /// </summary>
            /// <param name="c">Символ, введенный пользователем, который будет отформатирован</param>
            /// <returns>
            /// Преобразованный символ в строку. Это позволяет производным классам при необходимости проявлять большую гибкость в форматировании
            /// </returns>
            /// <remarks>
            /// Этот метод может быть переопределен производными классами для реализации пользовательской логики форматирования. Если с этим объектом не связан метод форматирования, символ возвращается без изменений
            /// </remarks> 
            public virtual string Format(char c)
            {
                if (Formatter != null)
                    return Formatter(c).ToString();
                return c.ToString();
            }
            /// <summary>
            /// Проверяет, соответствует ли введенный пользователем символ этому объекту
            /// </summary>
            /// <param name="c">Символ, введенный пользователем, который необходимо проверить</param>
            /// <returns>
            /// Если введенный пользователем символ является допустимым представлением символа, возвращается значение true; в противном случае — false
            /// </returns>
            /// <remarks>
            /// Этот метод может быть переопределен производными классами для предоставления пользовательской логики проверки
            /// </remarks> 
            public virtual bool Validate(char c)
            {
                if (Validator != null)
                {
                    foreach (ValidatorMethod validator in Validator.GetInvocationList())
                    {
                        if (!validator(c))
                            return false;
                    }
                }
                return true;
            }

            #endregion МЕТОДЫ

            #region = ПОЛЯ

            // The symbol's character
            private char m_symbol;

            /// <summary>
            /// Описание метода, используемого для проверки соответствия введенного пользователем символа символу данного объекта
            /// </summary>
            public delegate bool ValidatorMethod(char c);
            /// <summary>
            /// Описание метода, используемого для преобразования введенного пользователем символа в другой символ, если это необходимо
            /// </summary>
            public delegate char FormatterMethod(char c);

            #endregion ПОЛЯ

            #region = СВОЙСТВА

            /// <summary>
            /// Получает или задает символ для данного обозначения
            /// </summary>
            public char Char
            {
                get
                {
                    return m_symbol;
                }
                set
                {
                    m_symbol = value;
                }
            }

            #endregion СВОЙСТВА

            #region = СОБЫТИЯ

            /// <summary>
            /// Событие используется для проверки соответствия введенного пользователем символа символу данного объекта
            /// </summary>
            public event ValidatorMethod Validator;
            /// <summary>
            /// Событие используется для форматирования введенного пользователем символа на другой символ, если это необходимо
            /// </summary>
            public event FormatterMethod Formatter;

            #endregion СОБЫТИЯ
        }

        #endregion КЛАССЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Обрабатывает нажатия клавиш внутри текстового поля
        /// </summary>
        /// <param name="sender">Объект, отправивший событие.</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод переопределен в классе Behavior и обрабатывает событие KeyDown текстового поля
        /// </remarks>
        protected override void HandleKeyDown(object sender, KeyEventArgs e)
        {
            TraceLine("MaskedBehavior.HandleKeyDown " + e.KeyCode);

            if (e.KeyCode == Keys.Delete)
            {
                // If deleting make sure it's the last character or that
                // the selection goes all the way to the end of the text

                int start, end;
                m_selection.Get(out start, out end);

                string text = m_textBox.Text;
                int length = text.Length;

                if (end != length)
                {
                    if (!(end == start && end == length - 1))
                        e.Handled = true;
                }
            }
        }
        /// <summary>
        /// Обрабатывает нажатия клавиш внутри текстового поля
        /// </summary>
        /// <param name="sender">Объект, отправивший событие.</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод переопределен в классе Behavior и обрабатывает событие KeyPress текстового поля.
        /// </remarks>
        protected override void HandleKeyPress(object sender, KeyPressEventArgs e)
        {
            TraceLine("MaskedBehavior.HandleKeyPress " + e.KeyChar);

            // Check to see if it's read only
            if (m_textBox.ReadOnly)
                return;

            char c = e.KeyChar;
            e.Handled = true;

            // If the mask is empty, allow anything
            int maskLength = fMask.Length;
            if (maskLength == 0)
            {
                base.HandleKeyPress(sender, e);
                return;
            }

            int start, end;
            m_selection.Get(out start, out end);

            // Check that we haven't gone past the mask's length
            if (start >= maskLength && c != (short)Keys.Back)
                return;

            string text = m_textBox.Text;
            int length = text.Length;

            // Check for a non-printable character (such as Ctrl+C)
            if (Char.IsControl(c))
            {
                if (c == (short)Keys.Back && start != length)
                {
                    SendKeys.Send("{LEFT}");  // move the cursor left
                    return;
                }

                // Allow backspace only if the cursor is all the way to the right
                base.HandleKeyPress(sender, e);
                return;
            }

            char cMask = fMask[start];

            // Check if the mask's character matches with any of the symbols in the array.
            foreach (Symbol symbol in fSymbolS)
            {
                if (cMask == (char)symbol)
                {
                    if (symbol.Validate(c))
                    {
                        end = (end == length ? end : (start + 1));
                        m_selection.SetAndReplace(start, end, symbol.Format(c));
                    }
                    return;
                }
            }

            // Check if it's the same character as the mask.
            if (cMask == c)
            {
                end = (end == length ? end : (start + 1));
                m_selection.SetAndReplace(start, end, c.ToString());
                return;
            }

            // Concatenate all the mask symbols
            StringBuilder concatenatedSymbols = new StringBuilder();
            foreach (Symbol symbol in fSymbolS)
                concatenatedSymbols.Append((char)symbol);

            char[] symbolChars = concatenatedSymbols.ToString().ToCharArray();

            // If it's a valid character, find the next symbol on the mask and add any non-mask characters in between.
            foreach (Symbol symbol in fSymbolS)
            {
                // See if the character is valid for any other symbols
                if (!symbol.Validate(c))
                    continue;

                string maskPortion = fMask.Substring(start);
                int maskPos = maskPortion.IndexOfAny(symbolChars);

                // Enter the character if there isn't another symbol before it
                if (maskPos >= 0 && maskPortion[maskPos] == (char)symbol)
                {
                    m_selection.SetAndReplace(start, start + maskPos, maskPortion.Substring(0, maskPos));
                    HandleKeyPress(sender, e);
                    return;
                }
            }
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Извлекает текст из текстового поля в корректном виде
        /// </summary>
        /// <returns>
        /// Если текст в текстовом поле допустим, он возвращается; в противном случае возвращается допустимая версия текста
        /// </returns>
        protected override string GetValidText()
        {
            string text = m_textBox.Text;
            int maskLength = fMask.Length;

            // If the mask is empty, allow anything
            if (maskLength == 0)
                return text;

            StringBuilder validText = new StringBuilder();
            int symbolCount = fSymbolS.Count;

            // Accomodate the text to the mask as much as possible
            for (int iPos = 0, iMaskPos = 0, length = text.Length; iPos < length; iPos++, iMaskPos++)
            {
                char c = text[iPos];
                char cMask = (iMaskPos < maskLength ? fMask[iMaskPos] : (char)0);

                // If we've reached the end of the mask, break
                if (cMask == 0)
                    break;

                int iSymbol = 0;

                // Match the character to any of the symbols
                for (; iSymbol < symbolCount; iSymbol++)
                {
                    Symbol symbol = (Symbol)fSymbolS[iSymbol];

                    // Find the symbol that applies for the given character
                    if (!symbol.Validate(c))
                        continue;

                    // Try to add matching characters in the mask until a different symbol is reached
                    for (; iMaskPos < maskLength; iMaskPos++)
                    {
                        cMask = fMask[iMaskPos];
                        if (cMask == (char)symbol)
                        {
                            validText.Append(symbol.Format(c));
                            break;
                        }
                        else
                        {
                            int iSymbol2 = 0;
                            for (; iSymbol2 < symbolCount; iSymbol2++)
                            {
                                Symbol symbol2 = (Symbol)fSymbolS[iSymbol2];
                                if (cMask == (char)symbol2)
                                {
                                    validText.Append(symbol.Format(c));
                                    break;
                                }
                            }

                            if (iSymbol2 < symbolCount)
                                break;

                            validText.Append(cMask);
                        }
                    }

                    break;
                }

                // If the character was not matched to a symbol, stop
                if (iSymbol == symbolCount)
                {
                    if (c == cMask)
                    {
                        // Match the character to any of the symbols
                        for (iSymbol = 0; iSymbol < symbolCount; iSymbol++)
                        {
                            Symbol symbol = (Symbol)fSymbolS[iSymbol];
                            if (cMask == (char)symbol)
                                break;
                        }

                        if (iSymbol == symbolCount)
                        {
                            validText.Append(c);
                            continue;
                        }
                    }

                    break;
                }
            }

            return validText.ToString();
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        /// <summary>
        /// Маска вводимых данных
        /// </summary>
        private string fMask;

		private ArrayList fSymbolS = new ArrayList();

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Получает или устанавливает маску
        /// </summary>
        /// <remarks>
        /// Эта строка используется для проверки и/или форматирования символов, введенных пользователем.
        /// По умолчанию символ '#' настроен как заполнитель цифры в маске.
        /// Таким образом, каждый символ '#' в маске представляет собой цифру, а любые другие символы между символами
        /// '#' автоматически заполняются по мере ввода цифр пользователем
		/// </remarks>
        public string __fMask_
        {
            get
            {
                return fMask;
            }
            set
            {
                if (fMask == value)
                    return;

                fMask = value;
                UpdateText();
            }
        }
        /// <summary>
        /// Извлекает значение из текстового поля без каких-либо нечисловых символов
        /// </summary>
        public string NumericText
        {
            get
            {
                string text = m_textBox.Text;
                StringBuilder numericText = new StringBuilder();

                foreach (char c in text)
                {
                    if (Char.IsDigit(c))
                        numericText.Append(c);
                }

                return numericText.ToString();
            }
        }
        /// <summary>
        /// Получает список ArrayList объектов Symbol
        /// </summary>
        /// <remarks>
        /// Изначально этот массив будет содержать одну запись: запись для символа <c>#</c>, который представляет собой заполнитель цифры в маске.
        /// Однако в массив можно легко добавить больше объектов Symbol, чтобы сделать маску более мощной
        /// </remarks>
        /// <example>
        /// MaskedBehavior behavior = new MaskedBehavior(txtSerialNumber, "^#^-^##-###");
        /// Добавляем символ ^, чтобы разрешить только буквы и преобразовывать их в верхний регистр.
        /// MaskedBehavior.Symbol.ValidatorMethod validator = new MaskedBehavior.Symbol.ValidatorMethod(Char.IsLetter);
        /// MaskedBehavior.Symbol.FormatterMethod formatter = new MaskedBehavior.Symbol.FormatterMethod(Char.ToUpper)));
        /// behavior.Symbols.Add(new MaskedBehavior.Symbol('^', validator, formatter));
        /// </example>
        public ArrayList __fSymbols_
        {
            get
            {
                return fSymbolS;
            }
        }

        #endregion СВОЙСТВА
	}

    /// <summary>
    /// Класс поведения, обрабатывающий числовой ввод
    /// </summary>
    /// <remarks>
    /// Это базовый класс для других классов, работающих с числовыми данными.
    /// Он гарантирует, что пользователь вводит допустимое число, и предоставляет такие функции, как автоматическое форматирование.
    /// Он также позволяет точно контролировать внешний вид числа, например, количество цифр слева и справа от десятичной точки, а также может ли оно быть отрицательным или нет
	/// </remarks>
    public class NumericBehavior : elmTypeBehavior
	{
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Инициализирует новый экземпляр класса NumericBehavior, копируя его из другого объекта NumericBehavior
        /// </summary>
        /// <param name="behavior">Объект NumericBehavior, который необходимо скопировать (а затем удалить). Он не должен быть равен null.</param>
        /// <exception cref="ArgumentNullException">Поведение равно нулю</exception>
        /// <remarks>
        /// После копирования объекта behavior.TextBox вызывается метод Dispose для параметра behavior
        /// </remarks>
        public NumericBehavior(NumericBehavior behavior) : base(behavior)
        {
            fSymbolsIntCount = behavior.fSymbolsIntCount;
            fSymbolsFractionalCount = behavior.fSymbolsFractionalCount;
            fSymbolsInGroupCount = behavior.fSymbolsInGroupCount;
            fSymbolNegative = behavior.fSymbolNegative;
            fSymbolSeparator = behavior.fSymbolSeparator;
            fSymbolGroup = behavior.fSymbolGroup;
            fSymbolCurrency = behavior.fSymbolCurrency;
            fValueMinimum = behavior.fValueMinimum;
            fValueMaximum = behavior.fValueMaximum;
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса NumericBehavior, связывая его с объектом, производным от TextBoxBase
        /// </summary>
        /// <param name="textBox">Объект TextBoxBase, который необходимо связать с этим поведением. Он не должен быть равен null</param>
        /// <exception cref="ArgumentNullException">Текстовое поле имеет значение null</exception>
        /// <remarks>
        /// Этот конструктор задает 
        /// <see cref="__fSymbolsIntCount_" /> = 9, 
        /// <see cref="__fSymbolsFractionalCount_" /> = 4, 
        /// <see cref="__fSymbolsInGroupCount_" /> = 0, 
        /// <see cref="Prefix" /> = "", 
        /// <see cref="AllowNegative" /> = true, 
        /// А остальные свойства определяются системой пользователя</remarks>
        public NumericBehavior(TextBoxBase textBox) : base(textBox, true)
        {
            _mAdjustDecimalAndGroupSeparators();
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса NumericBehavior, связывая его с объектом, производным от TextBoxBase.
        /// и устанавливая максимальное количество цифр, разрешенных слева и справа от десятичной точки
        /// </summary>
        /// <param name="textBox">Объект TextBoxBase, который необходимо связать с этим поведением. Он не должен быть равен null</param>
        /// <param name="maxWholeDigits">Максимальное количество цифр, допустимое слева от десятичной точки. Если оно меньше 1, устанавливается равным 1</param>
        /// <param name="maxDecimalPlaces">Максимальное количество цифр, допустимое после десятичной точки. Если значение меньше 0, оно устанавливается равным 0</param>
        /// <exception cref="ArgumentNullException">Текстовое поле имеет значение null</exception>
        /// <remarks>
        /// Этот конструктор задает
        /// <see cref="__fSymbolsInGroupCount_" /> = 0, 
        /// <see cref="Prefix" /> = "", 
        /// <see cref="AllowNegative" /> = true, 
        /// а остальные свойства — в соответствии с системой пользователя
        /// </remarks>
        public NumericBehavior(TextBoxBase textBox, int maxWholeDigits, int maxDecimalPlaces) : this(textBox)
        {
            fSymbolsIntCount = maxWholeDigits;
            fSymbolsFractionalCount = maxDecimalPlaces;

            if (fSymbolsIntCount < 1)
                fSymbolsIntCount = 1;
            if (fSymbolsFractionalCount < 0)
                fSymbolsFractionalCount = 0;
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса NumericBehavior, связывая его с объектом, производным от TextBoxBase,
        /// и присваивая его атрибуты из строки маски.
        /// </summary>
        /// <param name="textBox">Объект TextBoxBase, который необходимо связать с этим поведением. Он не должен быть равен null</param>
        /// <param name="mask">Строка, используемая для установки нескольких свойств объекта. Дополнительную информацию см. в файле <see cref="__fMask_" /></param>
        /// <remarks>
        /// Этот конструктор устанавливает <see cref="AllowNegative" /> = true
        /// и остальные свойства, используя маску        
		/// </remarks>
        public NumericBehavior(TextBoxBase textBox, string mask) : base(textBox, true)
        {
            __fMask_ = mask;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Изменяет значение текстового поля таким образом, чтобы оно находилось в пределах допустимого диапазона
        /// </summary>
        protected void _mAdjustWithinRange()
        {
            // Проверьте, находится ли значение уже в пределах допустимого диапазона
            if (__mIsValid())
                return;

            // Если поле пустое, задайте допустимое число
            if (m_textBox.Text == "")
                m_textBox.Text = " ";
            else
                UpdateText();

            // Сделайте так, чтобы оно находилось в пределах указанного диапазона
            double value = ToDouble(__fValueToString_);
            if (value < fValueMinimum)
            {
                m_textBox.Text = fValueMinimum.ToString();
            }
            else
            {
                if (value > fValueMaximum)
                { 
                    m_textBox.Text = fValueMaximum.ToString(); 
                }
            }
        }
        /// <summary>
        /// Обрабатывает ситуацию, когда текст элемента управления анализируется и преобразуется в тип, ожидаемый объектом, к которому он привязан
        /// </summary>
        /// <param name="sender">Объект, отправивший событие</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод проверяет, пуст ли текст элемента управления, и если да, то устанавливает значение равным DBNull.Value;
        /// в противном случае преобразует его в простое числовое значение (без префикса)
        /// </remarks>
        protected override void HandleBindingParse(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() == "")
                e.Value = DBNull.Value;
            else
                e.Value = _mGetNumericText(e.Value.ToString(), false);
        }
        /// <summary>
        /// Обрабатывает нажатия клавиш внутри текстового поля
        /// </summary>
        /// <param name="sender">The object who sent the event</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод переопределен в классе Behavior и обрабатывает событие KeyDown текстового поля.
        /// </remarks>
        protected override void HandleKeyDown(object sender, KeyEventArgs e)
        {
            TraceLine("NumericBehavior.HandleKeyDown " + e.KeyCode);

            if (e.KeyCode == Keys.Delete)
            {
                int start, end;
                m_selection.Get(out start, out end);

                string text = m_textBox.Text;
                int length = text.Length;

                // If deleting the prefix, don't allow it if there's a number after it.
                int prefixLength = fSymbolCurrency.Length;
                if (start < prefixLength && length > prefixLength)
                {
                    if (end != length)
                        e.Handled = true;
                    return;
                }

                m_textChangedByKeystroke = true;

                // If deleting a group separator (comma), move the cursor to the right
                if (start < length && text[start] == fSymbolGroup && start == end)
                    SendKeys.SendWait("{RIGHT}");

                m_previousSeparatorCount = GetGroupSeparatorCount(text);

                // If everything on the right was deleted, put the selection on the right
                if (end == length)
                    SendKeys.Send("{RIGHT}");
            }
        }
        /// <summary>
        /// Обрабатывает нажатия клавиш внутри текстового поля
        /// </summary>
        /// <param name="sender">Объект, отправивший событие</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод переопределен в классе Behavior и обрабатывает событие KeyPress текстового поля.
		/// </remarks>
        protected override void HandleKeyPress(object sender, KeyPressEventArgs e)
        {
            TraceLine("NumericBehavior.HandleKeyPress " + e.KeyChar);

            // Check to see if it's read only
            if (m_textBox.ReadOnly)
                return;

            char c = e.KeyChar;
            e.Handled = true;
            m_textChangedByKeystroke = true;

            int start, end;
            m_selection.Get(out start, out end);

            string text = m_textBox.Text;
            m_previousSeparatorCount = -1;

            string numericText = NumericText;
            int decimalPos = text.IndexOf(fSymbolSeparator);
            int numericDecimalPos = numericText.IndexOf(fSymbolSeparator);
            int length = text.Length;
            int numericLen = numericText.Length;
            int prefixLength = fSymbolCurrency.Length;
            int separatorCount = GetGroupSeparatorCount(text);

            // Check if we're in the prefix's location
            if (start < prefixLength && !Char.IsControl(c))
            {
                char cPrefix = fSymbolCurrency[start];

                // Check if it's the same character as the prefix.
                if (cPrefix == c)
                {
                    if (length > start)
                    {
                        end = (end == length ? end : (start + 1));
                        m_selection.SetAndReplace(start, end, c.ToString());
                    }
                    else
                        base.HandleKeyPress(sender, e);
                }
                // If it's a part of the number, enter the prefix
                else if (Char.IsDigit(c) || c == fSymbolNegative || c == fSymbolSeparator)
                {
                    end = (end == length ? end : prefixLength);
                    m_selection.SetAndReplace(start, end, fSymbolCurrency.Substring(start));
                    HandleKeyPress(sender, e);
                }

                return;
            }

            // Check if it's a negative sign
            if (c == fSymbolNegative && __fNegative_)
            {
                // If it's at the beginning, determine if it should overwritten
                if (start == prefixLength)
                {
                    if (numericText != "" && numericText[0] == fSymbolNegative)
                    {
                        end = (end == length ? end : (start + 1));
                        m_selection.SetAndReplace(start, end, fSymbolNegative.ToString());
                        return;
                    }
                }
                // If we're not at the beginning, toggle the sign
                else
                {
                    if (numericText[0] == fSymbolNegative)
                    {
                        m_selection.SetAndReplace(prefixLength, prefixLength + 1, "");
                        m_selection.Set(start - 1, end - 1);
                    }
                    else
                    {
                        m_selection.SetAndReplace(prefixLength, prefixLength, fSymbolNegative.ToString());
                        m_selection.Set(start + 1, end + 1);
                    }

                    return;
                }
            }

            // Check if it's a decimal point (only one is allowed).
            else if (c == fSymbolSeparator && fSymbolsFractionalCount > 0)
            {
                if (decimalPos >= 0)
                {
                    // Check if we're replacing the decimal point
                    if (decimalPos >= start && decimalPos < end)
                        m_previousSeparatorCount = separatorCount;
                    else
                    {   // Otherwise, put the caret on it
                        m_selection.Set(decimalPos + 1, decimalPos + 1);
                        return;
                    }
                }
                else
                    m_previousSeparatorCount = separatorCount;
            }

            // Check if it's a digit
            else if (Char.IsDigit(c))
            {
                // Check if we're on the right of the decimal point.
                if (decimalPos >= 0 && decimalPos < start)
                {
                    if (numericText.Substring(numericDecimalPos + 1).Length == fSymbolsFractionalCount)
                    {
                        if (start <= decimalPos + fSymbolsFractionalCount)
                        {
                            end = (end == length ? end : (start + 1));
                            m_selection.SetAndReplace(start, end, c.ToString());
                        }
                        return;
                    }
                }

                // We're on the left side of the decimal point
                else
                {
                    bool isNegative = (numericText.Length != 0 && numericText[0] == fSymbolNegative);

                    // Make sure we can still enter digits.
                    if (start == fSymbolsIntCount + separatorCount + prefixLength + (isNegative ? 1 : 0))
                    {
                        if (AddDecimalAfterMaxWholeDigits && fSymbolsFractionalCount > 0)
                        {
                            end = (end == length ? end : (start + 2));
                            m_selection.SetAndReplace(start, end, fSymbolSeparator.ToString() + c);
                        }

                        return;
                    }

                    if (numericText.Substring(0, numericDecimalPos >= 0 ? numericDecimalPos : numericLen).Length == fSymbolsIntCount + (isNegative ? 1 : 0))
                    {
                        if (text[start] == fSymbolGroup)
                            start++;

                        end = (end == length ? end : (start + 1));
                        m_selection.SetAndReplace(start, end, c.ToString());
                        return;
                    }

                    m_previousSeparatorCount = separatorCount;
                }
            }

            // Check if it's a non-printable character, such as Backspace or Ctrl+C
            else if (Char.IsControl(c))
                m_previousSeparatorCount = separatorCount;
            else
                return;

            base.HandleKeyPress(sender, e);
        }
        /// <summary>
        /// Обрабатывает ситуацию, когда элемент управления теряет фокус
        /// </summary>
        /// <param name="sender">Объект, отправивший событие</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод переопределен в классе Behavior и
        /// обрабатывает событие LostFocus текстового поля.
        /// Здесь он проверяет значение на соответствие допустимому диапазону и добавляет недостающие нули
        /// </remarks>
        protected override void HandleLostFocus(object sender, EventArgs e)
        {
            TraceLine("NumericBehavior.HandleLostFocus");

            if (!HasFlag((int)LostFocusFlag.Max))
                return;

            string originalText = _mGetNumericText(m_textBox.Text, true);
            string text = originalText;
            int length = text.Length;

            // If desired, remove any extra leading zeros but always leave one in front of the decimal point
            if (HasFlag((int)LostFocusFlag.RemoveExtraLeadingZeros) && length > 0)
            {
                bool isNegative = (text[0] == fSymbolNegative);
                if (isNegative)
                    text = text.Substring(1);
                text = text.TrimStart('0');
                if (text == "" || text[0] == fSymbolSeparator)
                    text = '0' + text;
                if (isNegative)
                    text = fSymbolNegative + text;
            }
            // Check if the value is empty and we don't want to touch it
            else if (length == 0 && HasFlag((int)LostFocusFlag.DontPadWithZerosIfEmpty))
                return;

            int decimalPos = text.IndexOf('.');
            int maxDecimalPlaces = fSymbolsFractionalCount;
            int maxWholeDigits = fSymbolsIntCount;

            // Check if we need to pad the number with zeros after the decimal point
            if (HasFlag((int)LostFocusFlag.PadWithZerosAfterDecimal) && maxDecimalPlaces > 0)
            {
                if (decimalPos < 0)
                {
                    if (length == 0 || text == "-")
                    {
                        text = "0";
                        length = 1;
                    }
                    text += '.';
                    decimalPos = length++;
                }

                text = InsertZeros(text, -1, maxDecimalPlaces - (length - decimalPos - 1));
            }

            // Check if we need to pad the number with zeros before the decimal point
            if (HasFlag((int)LostFocusFlag.PadWithZerosBeforeDecimal) && maxWholeDigits > 0)
            {
                if (decimalPos < 0)
                    decimalPos = length;

                if (length > 0 && text[0] == '-')
                    decimalPos--;

                text = InsertZeros(text, (length > 0 ? (text[0] == '-' ? 1 : 0) : -1), maxWholeDigits - decimalPos);
            }

            if (text != originalText)
            {
                if (decimalPos >= 0 && fSymbolSeparator != '.')
                    text = text.Replace('.', fSymbolSeparator);

                // remember the current selection 
                using (elmTypeSelection.Saver savedSelection = new elmTypeSelection.Saver(m_textBox))
                {
                    m_textBox.Text = text;
                }
            }
        }
        /// <summary>
        /// Обрабатывает изменения текста в текстовом поле
        /// </summary>
        /// <param name="sender">Объект, отправивший событие.</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод переопределен в классе Behavior и
        /// обрабатывает событие TextChanged текстового поля.
        /// Здесь он используется для корректировки выделения, если были добавлены или удалены новые разделители.        /// </remarks>
        protected override void HandleTextChanged(object sender, EventArgs e)
        {
            TraceLine("NumericBehavior.HandleTextChanged");

            elmTypeSelection.Saver savedSelection = new elmTypeSelection.Saver(m_textBox);  // save the selection before the text changes
            bool textChangedByKeystroke = m_textChangedByKeystroke;
            base.HandleTextChanged(sender, e);

            // Check if the user has changed the number enough to cause
            // one or more separators to be added/removed, in which case
            // the selection may need to be adjusted.
            if (m_previousSeparatorCount >= 0)
            {
                using (savedSelection)
                {
                    int newSeparatorCount = GetGroupSeparatorCount(m_textBox.Text);
                    if (m_previousSeparatorCount != newSeparatorCount && savedSelection.Start > fSymbolCurrency.Length)
                        savedSelection.MoveBy(newSeparatorCount - m_previousSeparatorCount);
                }
            }

            // If the text wasn't changed by a keystroke and the UseLostFocusFlagsWhenTextPropertyIsSet flag is set,
            // call the LostFocus handler to adjust the value according to whatever LostFocus flags are set.
            if (HasFlag((int)LostFocusFlag.CallHandlerWhenTextChanges) ||
               (!textChangedByKeystroke && HasFlag((int)LostFocusFlag.CallHandlerWhenTextPropertyIsSet)))
                HandleLostFocus(sender, e);

            m_textChangedByKeystroke = false;
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// При необходимости корректирует десятичные и групповые разделители, чтобы они не совпадали.
        /// </summary>
        /// <remarks>
        /// Если десятичный разделитель и разделитель групп совпадают, они изменяются.
        /// Это предотвращает потенциальные проблемы при вводе значения пользователем.        
        /// </remarks>	
        protected void _mAdjustDecimalAndGroupSeparators()
        {
            if (fSymbolSeparator == fSymbolGroup)
                fSymbolGroup = (fSymbolSeparator == ',' ? '.' : ',');
        }
        /// <summary>
        /// Копирует строку, вставляя в неё нули
        /// </summary>
        /// <param name="text">Текст для копирования с вставленными нулями.</param>
        /// <param name="startIndex">Позиция, отсчитываемая от нуля, куда следует вставлять нули. Если она меньше 0, нули добавляются.</param>
        /// <param name="count">Количество нулей для вставки</param>
        /// <returns>
        /// Возвращаемое значение представляет собой копию текста с вставленными нулями
        /// </returns>
        protected string InsertZeros(string text, int startIndex, int count)
        {
            if (startIndex < 0 && count > 0)
                startIndex = text.Length;

            StringBuilder result = new StringBuilder(text);
            for (int iZero = 0; iZero < count; iZero++)
                result.Insert(startIndex, '0');

            return result.ToString();
        }
        /// <summary>
        /// Проверяет, находится ли числовое значение текстового поля в допустимом диапазоне.
        /// </summary>
        /// <returns>
        /// Если значение находится в допустимом диапазоне, возвращается значение true; в противном случае — false
        /// </returns>
        public override bool __mIsValid()
        {
            double value = ToDouble(__fValueToString_);
            return (value >= fValueMinimum && value <= fValueMaximum);
        }
        /// <summary>
        /// Копирует строку, удаляя из неё все нечисловые символы
        /// </summary>
        /// <param name="text">Текст для анализа и копирования</param>
        /// <param name="realNumeric">
        /// Если true, значение возвращается в виде действительного числа
        /// (с точкой в ​​качестве десятичной точки и знаком минус для отрицательного знака);
        /// в противном случае, оно возвращается с использованием ожидаемых символов
        /// </param>
        /// <returns>
        /// Возвращаемое значение представляет собой копию исходного текста, содержащую только числовые символы
        /// </returns>
        protected string _mGetNumericText(string text, bool realNumeric)
        {
            StringBuilder numericText = new StringBuilder();
            bool isNegative = false;
            bool hasDecimalPoint = false;

            foreach (char c in text)
            {
                if (Char.IsDigit(c))
                    numericText.Append(c);
                else if (c == fSymbolNegative)
                    isNegative = true;
                else if (c == fSymbolSeparator && !hasDecimalPoint)
                {
                    hasDecimalPoint = true;
                    numericText.Append(realNumeric ? '.' : fSymbolSeparator);
                }
            }

            // Add the negative sign to the front of the number.
            if (isNegative)
                numericText.Insert(0, realNumeric ? '-' : fSymbolNegative);

            return numericText.ToString();
        }
        /// <summary>
        /// Возвращает количество символов-разделителей групп в заданном тексте
		/// </summary>
        private int GetGroupSeparatorCount(string text)
        {
            int count = 0;
            foreach (char c in text)
            {
                if (c == fSymbolGroup)
                    count++;
            }
            return count;
        }
        /// <summary>
        /// Принимает фрагмент текста, содержащий числовое значение, и вставляет разделители групп в нужных местах</summary>
        /// <param name="text">Текст для анализа</param>
        /// <returns>
        /// Возвращаемое значение представляет собой копию исходного текста с вставленными разделителями групп
        /// </returns>
        protected string GetSeparatedText(string text)
        {
            string numericText = _mGetNumericText(text, false);
            string separatedText = numericText;

            // Retrieve the number without the decimal point
            int decimalPos = numericText.IndexOf(fSymbolSeparator);
            if (decimalPos >= 0)
                separatedText = separatedText.Substring(0, decimalPos);

            if (fSymbolsInGroupCount > 0)
            {
                int length = separatedText.Length;
                bool isNegative = (separatedText != "" && separatedText[0] == fSymbolNegative);

                // Loop in reverse and stick the separator every m_digitsInGroup digits.
                for (int iPos = length - (fSymbolsInGroupCount + 1); iPos >= (isNegative ? 1 : 0); iPos -= fSymbolsInGroupCount)
                    separatedText = separatedText.Substring(0, iPos + 1) + fSymbolGroup + separatedText.Substring(iPos + 1);
            }

            // Prepend the prefix, if the number is not empty.
            if (separatedText != "" || decimalPos >= 0)
            {
                separatedText = fSymbolCurrency + separatedText;

                if (decimalPos >= 0)
                    separatedText += numericText.Substring(decimalPos);
            }

            return separatedText;
        }
        /// <summary>
        /// Извлекает текст из текстового поля в корректном виде
        /// </summary>
        /// <returns>
        /// Если текст в текстовом поле допустим, он возвращается; в противном случае возвращается допустимая версия текста
        /// </returns>
        protected override string GetValidText()
        {
            string text = m_textBox.Text;
            StringBuilder newText = new StringBuilder();
            bool isNegative = false;
            int prefixLength = fSymbolCurrency.Length;

            // Remove any invalid characters from the number
            for (int iPos = 0, decimalPos = -1, newLength = 0, length = text.Length; iPos < length; iPos++)
            {
                char c = text[iPos];

                // Check for a negative sign
                if (c == fSymbolNegative && __fNegative_)
                    isNegative = true;

                // Check for a digit
                else if (Char.IsDigit(c))
                {
                    // Make sure it doesn't go beyond the limits
                    if (decimalPos < 0 && newLength == fSymbolsIntCount)
                        continue;

                    if (decimalPos >= 0 && newLength > decimalPos + fSymbolsFractionalCount)
                        break;

                    newText.Append(c);
                    newLength++;
                }

                // Check for a decimal point
                else if (c == fSymbolSeparator && decimalPos < 0)
                {
                    if (fSymbolsFractionalCount == 0)
                        break;

                    newText.Append(c);
                    decimalPos = newLength;
                    newLength++;
                }
            }

            // Insert the negative sign if it's there
            if (isNegative)
                newText.Insert(0, fSymbolNegative);

            return GetSeparatedText(newText.ToString());
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПЕРЕЧИСЛЕНИЯ

        /// <summary>
        /// Внутренние значения, которые добавляются/удаляются из свойства <see cref="elmTypeBehavior.Flags" /> другими свойствами этого класса
        /// </summary>
        [Flags]
        protected enum Flag
        {
            /// <summary> 
            /// Значение не может быть отрицательным; пользователю запрещено вводить отрицательный знак
            /// </summary>
            CannotBeNegative = 0x00010000,
            /// <summary> 
            /// Если пользователь вводит цифру после того, как было введено значение <see cref="__fSymbolsIntCount_" />, то вставляется значение <see cref="__fSymbolSeparator_" />, а затем вводится цифра
            /// </summary>
            AddDecimalAfterMaxWholeDigits = 0x00020000
        };

        #endregion ПЕРЕЧИСЛЕНИЯ

        #region = ПОЛЯ

        private int fSymbolsIntCount = 9;
        private int fSymbolsFractionalCount = 4;
        private int fSymbolsInGroupCount = 0;
        private char fSymbolNegative = NumberFormatInfo.CurrentInfo.NegativeSign[0];
        private char fSymbolSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];
        private char fSymbolGroup = NumberFormatInfo.CurrentInfo.NumberGroupSeparator[0];
        private string fSymbolCurrency = "";
        private double fValueMinimum = Double.MinValue;
        private double fValueMaximum = Double.MaxValue;

        private int m_previousSeparatorCount = -1;
        private bool m_textChangedByKeystroke = false;

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Получает или задает значение, указывающее, будет ли автоматически вставляться десятичная точка, если пользователь вводит цифру после того, как было введено максимальное количество целых цифр
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство установлено в значение false, что означает, что если значение <see cref="__fSymbolsIntCount_" /> было введено,
        /// и значение <see cref="__fSymbolSeparator_" /> не вставляется автоматически, пользователю приходится делать это вручную.
        /// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />.		
        /// </remarks>
        public bool AddDecimalAfterMaxWholeDigits
        {
            get
            {
                return HasFlag((int)Flag.AddDecimalAfterMaxWholeDigits);
            }
            set
            {
                ModifyFlags((int)Flag.AddDecimalAfterMaxWholeDigits, value);
            }
        }
        /// <summary>
        /// Получает или задает символ, используемый в качестве десятичной точки.
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство устанавливается на основе системных настроек пользователя.
        /// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />
        /// </remarks>
        public char __fSymbolSeparator_
        {
            get
            {
                return fSymbolSeparator;
            }
            set
            {
                if (fSymbolSeparator == value)
                    return;

                fSymbolSeparator = value;
                _mAdjustDecimalAndGroupSeparators();
                UpdateText();
            }
        }
        /// <summary>
        /// Получает или задает количество цифр, которые нужно разместить в каждой группе слева от десятичной точки
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство установлено на 0. Его можно установить на 3, чтобы сгруппировать тысячи с помощью разделителя групп <see cref="__fSymbolGroup_"></see>.
        /// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />.
        /// </remarks>
        public int __fSymbolsInGroupCount_
        {
            get
            {
                return fSymbolsInGroupCount;
            }
            set
            {
                if (fSymbolsInGroupCount == value)
                    return;

                fSymbolsInGroupCount = value;
                if (fSymbolsInGroupCount < 0)
                    fSymbolsInGroupCount = 0;

                UpdateText();
            }
        }
        /// <summary>
        /// Получает сообщение об ошибке, уведомляющее пользователя о необходимости ввести допустимое числовое значение в пределах допустимого диапазона
        /// </summary>
        public override string ErrorMessage
        {
            get
            {
                if (fValueMinimum > double.MinValue && fValueMaximum < double.MaxValue)
                    return "Please specify a numeric value between " + fValueMinimum.ToString() + " and " + fValueMaximum.ToString() + ".";
                else if (fValueMinimum > double.MinValue)
                    return "Please specify a numeric value greater than or equal to " + fValueMinimum.ToString() + ".";
                else if (fValueMaximum < double.MinValue)
                    return "Please specify a numeric value less than or equal to " + fValueMaximum.ToString() + ".";
                return "Please specify a valid numeric value.";
            }
        }
        /// <summary>
        /// Получает или задает символ, используемый в качестве разделителя групп
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство устанавливается на основе системных настроек пользователя.
        /// В США это обычно запятая, используемая для разделения тысяч.
        /// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />. 
        /// </remarks>
        public char __fSymbolGroup_
        {
            get
            {
                return fSymbolGroup;
            }
            set
            {
                if (fSymbolGroup == value)
                    return;

                fSymbolGroup = value;
                _mAdjustDecimalAndGroupSeparators();
                UpdateText();
            }
        }
        /// <summary>
        /// Получает или задает значение маски, представляющее свойства данного объекта
        /// </summary>
        /// <remarks>
        /// </remarks>
        public string __fMask_
        {
            get
            {
                StringBuilder mask = new StringBuilder();

                for (int iDigit = 0; iDigit < fSymbolsIntCount; iDigit++)
                    mask.Append('0');

                if (fSymbolsFractionalCount > 0)
                    mask.Append(fSymbolSeparator);

                for (int iDigit = 0; iDigit < fSymbolsFractionalCount; iDigit++)
                    mask.Append('0');

                mask = new StringBuilder(GetSeparatedText(mask.ToString()));

                for (int iPos = 0, length = mask.Length; iPos < length; iPos++)
                {
                    if (mask[iPos] == '0')
                        mask[iPos] = '#';
                }

                return mask.ToString();
            }
            set
            {
                int decimalPos = -1;
                int length = value.Length;

                fSymbolsIntCount = 0;
                fSymbolsFractionalCount = 0;
                fSymbolsInGroupCount = 0;
                m_flags = (m_flags & (int)~Flag.CannotBeNegative);  // allow it to be negative
                fSymbolCurrency = "";

                for (int iPos = length - 1; iPos >= 0; iPos--)
                {
                    char c = value[iPos];
                    if (c == '#')
                    {
                        if (decimalPos >= 0)
                            fSymbolsIntCount++;
                        else
                            fSymbolsFractionalCount++;
                    }
                    else if ((c == '.' || c == fSymbolSeparator) && decimalPos < 0)
                    {
                        decimalPos = iPos;
                        fSymbolSeparator = c;
                    }
                    else if (c == ',' || c == fSymbolGroup)
                    {
                        if (fSymbolsInGroupCount == 0)
                        {
                            fSymbolsInGroupCount = (((decimalPos >= 0) ? decimalPos : length) - iPos) - 1;
                            fSymbolGroup = c;
                        }
                    }
                    else
                    {
                        fSymbolCurrency = value.Substring(0, iPos + 1);
                        break;
                    }
                }

                if (decimalPos < 0)
                {
                    fSymbolsIntCount = fSymbolsFractionalCount;
                    fSymbolsFractionalCount = 0;
                }

                Debug.Assert(fSymbolsIntCount > 0); // must have at least one digit on left side of decimal point

                _mAdjustDecimalAndGroupSeparators();
                UpdateText();
            }
        }
        /// <summary>
        /// Получает или задает максимальное количество цифр, допустимых слева от десятичной точки
        /// </summary>
        /// <remarks>
        /// Если этому свойству присвоено число меньше 1, оно устанавливается условием 1.
        /// Вот как это работает, как это работает, как это работает метод <see cref="elmTypeBehavior.UpdateText" />.
        /// </remarks>
        public int __fSymbolsIntCount_
        {
            get
            {
                return fSymbolsIntCount;
            }
            set
            {
                if (fSymbolsIntCount == value)
                    return;

                fSymbolsIntCount = value;
                if (fSymbolsIntCount < 1)
                    fSymbolsIntCount = 1;

                UpdateText();
            }
        }
        /// <summary>
        /// Получает или задает максимально допустимое количество цифр после десятичной точки
        /// </summary>
        /// <remarks>
        /// Если этому свойству присвоено число меньше 0, оно устанавливается равным 0.
        /// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />
		/// </remarks>
        public int __fSymbolsFractionalCount_
        {
            get
            {
                return fSymbolsFractionalCount;
            }
            set
            {
                if (fSymbolsFractionalCount == value)
                    return;

                fSymbolsFractionalCount = value;
                if (fSymbolsFractionalCount < 0)
                    fSymbolsFractionalCount = 0;

                UpdateText();
            }
        }
        /// <summary>
        /// Получает или задает значение, разрешающее отрицательные значения
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство установлено в значение true, что означает, что допускаются отрицательные значения.
        /// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />
		/// </remarks>
        public bool __fNegative_
        {
            get
            {
                return !HasFlag((int)Flag.CannotBeNegative);
            }
            set
            {
                ModifyFlags((int)Flag.CannotBeNegative, !value);
            }
        }
        /// <summary>
        /// Получает или задает символ, используемый для отрицательного знака
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство устанавливается на основе системных настроек пользователя, но, скорее всего, это будет знак минус.
        /// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />.
		/// </remarks>
        public char __fSymbolNegative_
        {
            get
            {
                return fSymbolNegative;
            }
            set
            {
                if (fSymbolNegative == value)
                    return;

                fSymbolNegative = value;
                UpdateText();
            }
        }
        /// <summary>
        /// Извлекает значение из текстового поля без каких-либо нечисловых символов
        /// </summary>
        public string NumericText
        {
            get
            {
                return _mGetNumericText(m_textBox.Text, false);
            }
        }
        /// <summary>
        /// Получает или задает текст для автоматической вставки перед числом, например, символ валюты.
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство установлено в пустую строку.
        /// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />
        /// </remarks>
        public String __fSymbolCurrency_
        {
            get
            {
                return fSymbolCurrency;
            }
            set
            {
                if (fSymbolCurrency == value)
                    return;

                fSymbolCurrency = value;
                UpdateText();
            }
        }
        /// <summary>
        /// Получает или задает максимально допустимое значение
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство установлено на <see cref="Double.MaxValue" />, однако диапазон значений
        /// проверяется только при потере фокуса элементом управления, если установлен один из флагов <see cref="ValidatingFlag" />		/// </remarks>	
        public double __fValueMaximum_
        {
            get
            {
                return fValueMaximum;
            }
            set
            {
                fValueMaximum = value;
            }
        }
        /// <summary>
        /// Получает или задает минимально допустимое значение
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство установлено на <see cref="Double.MinValue" />, однако диапазон значений
        /// проверяется только при потере фокуса элементом управления, если установлен один из флагов <see cref="ValidatingFlag" />		/// </remarks>	
        public double __fValueMinimum_
        {
            get
            {
                return fValueMinimum;
            }
            set
            {
                fValueMinimum = value;
            }
        }
        /// <summary>
        /// Извлекает значение текстового поля без каких-либо нечисловых символов,
        /// с точкой в ​​качестве десятичной точки и знаком минус для отрицательного знака
        /// </summary>
        public string __fValueToString_
        {
            get
            {
                return _mGetNumericText(m_textBox.Text, true);
            }
        }

        #endregion СВОЙСТВА
	}

    /// <summary>
    /// Класс поведения, который позволяет вводить только целочисленные значения
    /// </summary>
    /// <remarks>
    /// Это всего лишь класс <see cref="NumericBehavior" />, который поддерживает значение <see cref="MaxDecimalPlaces" /> всегда равным 0
    /// </remarks>
    public class IntegerBehavior : NumericBehavior
	{
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Initializes a new instance of the IntegerBehavior class by copying it from another IntegerBehavior object
        /// </summary>
        /// <param name="behavior">The IntegerBehavior object to copied (and then disposed of). It must not be null</param>
        public IntegerBehavior(IntegerBehavior behavior) : base(behavior)
        {
            SetDefaultRange();
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса IntegerBehavior, связывая его с объектом, производным от TextBoxBase.
        /// </summary>
        /// <param name="textBox">Объект TextBoxBase, который необходимо связать с этим поведением. Он не должен быть равен null.</param>
        public IntegerBehavior(TextBoxBase textBox) : base(textBox, 9, 0)
        {
            SetDefaultRange();
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса IntegerBehavior, связывая его с объектом, производным от TextBoxBase.
        /// </summary>
        /// <param name="textBox">Объект TextBoxBase, который необходимо связать с этим поведением. Он не должен быть равен null</param>
        /// <param name="maxWholeDigits">Максимально допустимое количество цифр слева от десятичной точки</param>
        public IntegerBehavior(TextBoxBase textBox, int maxWholeDigits) : base(textBox, maxWholeDigits, 0)
        {
            SetDefaultRange();
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Изменяет значения по умолчанию для минимального и максимального значений на 32-битные целочисленные диапазоны.
        /// </summary>
        private void SetDefaultRange()
        {
            __fValueMinimum_ = Int32.MinValue;
            __fValueMaximum_ = Int32.MaxValue;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = СВОЙСТВА

        /// <summary>
        /// Получает или задает значение маски, представляющее свойства данного объекта
        /// </summary>
        /// <remarks>
        /// Это свойство ведет себя аналогично <see cref="NumericBehavior.__fMask_" />, за исключением того, что
        /// <see cref="NumericBehavior.__fSymbolsFractionalCount_" /> поддерживается со значением 0
        /// </remarks>
        public new string __fMask_
        {
            get
            {
                return base.__fMask_;
            }
            set
            {
                base.__fMask_ = value;
                if (base.__fSymbolsFractionalCount_ > 0)
                    base.__fSymbolsFractionalCount_ = 0;
            }
        }
        /// <summary>
        /// Получает максимально допустимое количество цифр после десятичной точки, которое всегда равно 0
        /// </summary>
        public new int MaxDecimalPlaces
        {
            get
            {
                return base.__fSymbolsFractionalCount_;
            }
        }

        #endregion СВОЙСТВА
	}

    /// <summary>
    /// Класс поведения полезен для ввода денежных значений
    /// </summary>
    /// <remarks>
    /// Это просто класс <see cref="NumericBehavior" />, настроенный для придания значению денежного вида.
    /// Он устанавливает <see cref="NumericBehavior.Prefix" /> в знак валюты, указанный в системе пользователя (например, '$').
    /// Он также разделяет тысячи символом, указанным в системе (например, запятой).
    /// Он устанавливает <see cref="NumericBehavior.__fSymbolsFractionalCount_" /> в значение, указанное в системе — обычно два.	
	/// </remarks>
    public class CurrencyBehavior : NumericBehavior
	{
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Инициализирует новый экземпляр класса CurrencyBehavior, копируя его из
        /// другого объекта CurrencyBehavior
        /// </summary>
        /// <param name="behavior">Объект CurrencyBehavior, который необходимо скопировать (а затем удалить). Он не должен быть равен null.</param>
        /// <remarks>
        /// После копирования объекта behavior.TextBox вызывается метод Dispose для параметра behavior
        /// </remarks>
        public CurrencyBehavior(CurrencyBehavior behavior) : base(behavior)
        {
        }
        /// <summary>
        /// Initializes a new instance of the CurrencyBehavior class by associating it with a TextBoxBase derived object
        /// </summary>
        /// <param name="textBox">The TextBoxBase object to associate with this behavior. It must not be null</param>
        /// <remarks>
        /// This constructor sets <see cref="NumericBehavior.__fSymbolsIntCount_" /> = 9, <see cref="NumericBehavior.AllowNegative" /> = true, 
        /// and the rest of the properties according to user's system. If the system has the 
        /// currency symbol configured to be placed in front of the value, then it is assigned to the <see cref="NumericBehavior.Prefix" />.
        /// Also, the number is automatically padded with zeros after the <see cref="NumericBehavior.__fSymbolSeparator_" /> when the textbox loses focus
        /// </remarks>
        public CurrencyBehavior(TextBoxBase textBox) : base(textBox)
        {
            m_flags |= ((int)LostFocusFlag.RemoveExtraLeadingZeros |
                        (int)LostFocusFlag.PadWithZerosAfterDecimal |
                        (int)LostFocusFlag.DontPadWithZerosIfEmpty |
                        (int)LostFocusFlag.CallHandlerWhenTextPropertyIsSet);

            // Get the system's current settings
            __fSymbolsInGroupCount_ = NumberFormatInfo.CurrentInfo.CurrencyGroupSizes[0];
            __fSymbolsFractionalCount_ = NumberFormatInfo.CurrentInfo.CurrencyDecimalDigits;
            __fSymbolSeparator_ = NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator[0];
            __fSymbolGroup_ = NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator[0];

            // Determine how the currency symbol should be shown for the prefix
            switch (NumberFormatInfo.CurrentInfo.CurrencyPositivePattern)
            {
                case 0:     // Prefix, no separation
                    __fSymbolCurrency_ = NumberFormatInfo.CurrentInfo.CurrencySymbol;
                    break;
                case 2:     // Prefix, one space separation
                    __fSymbolCurrency_ = NumberFormatInfo.CurrentInfo.CurrencySymbol + ' ';
                    break;

                    // The rest are suffixes, so no prefix
            }

            _mAdjustDecimalAndGroupSeparators();
        }

        #endregion ДИЗАЙНЕРЫ
	}

    /// <summary>
    /// Класс поведения, обрабатывающий ввод значений дат в формате mm/dd/yyyy или dd/mm/yyyy
    /// </summary>
    /// <remarks>
    /// Это поведение предназначено для того, чтобы пользователь мог быстро и точно вводить дату.
    /// По мере ввода цифр, косые черты автоматически заполняются. Пользователь может удалять только символы справа от введенного значения.
    /// Это помогает сохранить правильное форматирование значения.
    /// Пользователь также может использовать клавиши со стрелками вверх/вниз для увеличения/уменьшения месяца, дня или года в зависимости от положения курсора
	/// </remarks>
    public class DateBehavior : elmTypeBehavior
	{
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Initializes a new instance of the DateBehavior class by copying it from another DateBehavior object
        /// </summary>
        /// <param name="behavior">The DateBehavior object to copied (and then disposed of). It must not be null</param>
        /// <remarks>
        /// After the behavior.TextBox object is copied, Dispose is called on the behavior parameter
        /// </remarks>
        public DateBehavior(DateBehavior behavior) : base(behavior)
        {
            fValueMin = behavior.fValueMin;
            fValueMax = behavior.fValueMax;
            fSeparator = behavior.fSeparator;
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса DateBehavior, связывая его с объектом, производным от TextBoxBase.
        /// </summary>
        /// <param name="textBox">Объект TextBoxBase, который необходимо связать с этим поведением. Он не должен быть равен null</param>
        /// <remarks>
        /// Этот конструктор определяет разделитель (<see cref="__fSeparator_" />) и формат даты (мм/дд/гггг или дд/мм/гггг) на основе данных системы пользователя
        /// </remarks>
        public DateBehavior(TextBoxBase textBox) : this(textBox, true)
        {
        }
        /// <summary>
        /// Initializes a new instance of the DateBehavior class by associating it with a TextBoxBase derived object
        /// </summary>
        /// <param name="textBox">The TextBoxBase object to associate with this behavior. It must not be null</param>
        /// <param name="addEventHandlers">If true, the textBox's event handlers are tied to the corresponding methods on this behavior object</param>
        /// <remarks>
        /// This constructor determines the <see cref="__fSeparator_" /> and date format (mm/dd/yyyy or dd/mm/yyyy) from the user's system. 
        /// It is meant to be used internally by the DateTime behavior class. </remarks>
        internal DateBehavior(TextBoxBase textBox, bool addEventHandlers) : base(textBox, addEventHandlers)
        {
            // Get the system's date separator
            fSeparator = DateTimeFormatInfo.CurrentInfo.DateSeparator[0];

            // Determine if the day should go before the month
            string shortDate = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
            for (int iPos = 0; iPos < shortDate.Length; iPos++)
            {
                char c = Char.ToUpper(shortDate[iPos]);
                if (c == 'M')   // see if the month is first
                    break;
                if (c == 'D')   // see if the day is first, and then set the flag
                {
                    m_flags |= (int)FLAG.DayBeforeMonth;
                    break;
                }
            }
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Обрабатывает нажатия клавиш внутри текстового поля.
        /// </summary>
        /// <param name="sender">Объект, отправивший событие.</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод переопределен в классе Behavior и обрабатывает событие KeyDown текстового поля
        /// </remarks>
        protected override void HandleKeyDown(object sender, KeyEventArgs e)
        {
            TraceLine("DateBehavior.HandleKeyDown " + e.KeyCode);

            // Check to see if it's read only
            if (m_textBox.ReadOnly)
                return;

            e.Handled = true;

            switch (e.KeyCode)
            {
                case Keys.Delete:
                    {
                        // If deleting make sure it's the last character or that
                        // the selection goes all the way to the end of the text

                        int start, end;
                        m_selection.Get(out start, out end);

                        string text = m_textBox.Text;
                        int length = text.Length;

                        if (end != length)
                        {
                            if (!(end == start && end == length - 1))
                                return;
                        }

                        m_noTextChanged = true;
                        break;
                    }

                case Keys.Up:
                    {
                        // If pressing the UP arrow, increment the corresponding value.

                        int start, end;
                        m_selection.Get(out start, out end);

                        if (start >= GetYearStartPosition() && start <= GetYearStartPosition() + 4)
                        {
                            int year = Year;
                            if (year >= fValueMin.Year && year < fValueMax.Year)
                                Year = ++year;
                        }

                        else if (start >= GetMonthStartPosition() && start <= GetMonthStartPosition() + 2)
                        {
                            int month = __fMonth_;
                            if (month >= GetMinMonth() && month < GetMaxMonth())
                                __fMonth_ = ++month;
                        }

                        else if (start >= GetDayStartPosition() && start <= GetDayStartPosition() + 2)
                        {
                            int day = Day;
                            if (day >= GetMinDay() && day < GetMaxDay())
                                Day = ++day;
                        }

                        return;
                    }

                case Keys.Down:
                    {
                        // If pressing the DOWN arrow, decrement the corresponding value.

                        int start, end;
                        m_selection.Get(out start, out end);

                        if (start >= GetYearStartPosition() && start <= GetYearStartPosition() + 4)
                        {
                            int year = Year;
                            if (year > fValueMin.Year)
                                Year = --year;
                        }

                        else if (start >= GetMonthStartPosition() && start <= GetMonthStartPosition() + 2)
                        {
                            int month = __fMonth_;
                            if (month > GetMinMonth())
                                __fMonth_ = --month;
                        }

                        else if (start >= GetDayStartPosition() && start <= GetDayStartPosition() + 2)
                        {
                            int day = Day;
                            if (day > GetMinDay())
                                Day = --day;
                        }

                        return;
                    }
            }

            base.HandleKeyDown(sender, e);
        }
        /// <summary>
        /// Вызывает либо метод <see cref="HandleKeyPress" />, либо метод <see cref="HandleKeyDown" />.
        /// </summary>
        /// <param name="sender">Объект, отправивший событие.</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод должен вызываться классом <see cref="DateTime designedBehavior" />
        /// поскольку он не имеет публичного доступа к методам HandleKeyPress или HandleKeyDown.
        /// Тип EventArgs определяет, какой метод будет вызван
        /// </remarks>
        internal void HandleKeyEvent(object sender, EventArgs e)
        {
            if (e is KeyEventArgs)
                HandleKeyDown(sender, (KeyEventArgs)e);
            else if (e is KeyPressEventArgs)
                HandleKeyPress(sender, (KeyPressEventArgs)e);
        }
        /// <summary>
        /// Обрабатывает нажатия клавиш внутри текстового поля
        /// </summary>
        /// <param name="sender">Объект, отправивший событие.</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод переопределен в классе Behavior и обрабатывает событие KeyPress текстового поля
        /// </remarks>
        protected override void HandleKeyPress(object sender, KeyPressEventArgs e)
        {
            TraceLine("DateBehavior.HandleKeyPress " + e.KeyChar);

            // Check to see if it's read only
            if (m_textBox.ReadOnly)
                return;

            char c = e.KeyChar;
            e.Handled = true;
            m_noTextChanged = true;

            int start, end;
            m_selection.Get(out start, out end);

            string text = m_textBox.Text;
            int length = text.Length;

            // Check for a non-printable character (such as Ctrl+C)
            if (Char.IsControl(c))
            {
                if (c == (short)Keys.Back && start != length)
                {
                    SendKeys.Send("{LEFT}");  // move the cursor left
                    return;
                }

                // Allow backspace only if the cursor is all the way to the right
                base.HandleKeyPress(sender, e);
                return;
            }

            // Add the digit depending on its location
            switch (start)
            {
                case 0:     // FIRST DIGIT
                    {
                        if (__fShowDayBeforeMonth_)
                        {
                            if (IsValidDayDigit(c, 0))
                            {
                                if (length > start)
                                {
                                    m_selection.SetAndReplace(start, start + 1, c.ToString());

                                    if (length > start + 1)
                                    {
                                        if (!IsValidDay(Day))
                                        {
                                            m_selection.SetAndReplace(start + 1, start + 2, GetMinDayDigit(1).ToString());
                                            m_selection.Set(start + 1, start + 2);
                                        }
                                    }
                                }
                                else
                                    base.HandleKeyPress(sender, e);
                            }
                            // Check if we can insert the digit with a leading zero
                            else if (length == start && GetMinDayDigit(0) == '0' && IsValidDayDigit(c, 1))
                                m_selection.SetAndReplace(start, start + 2, "0" + c);
                        }
                        else
                        {
                            if (IsValidMonthDigit(c, 0))
                            {
                                if (length > start)
                                {
                                    m_selection.SetAndReplace(start, start + 1, c.ToString());

                                    if (length > start + 1)
                                    {
                                        if (!IsValidMonth(__fMonth_))
                                        {
                                            m_selection.SetAndReplace(start + 1, start + 2, GetMinMonthDigit(1).ToString());
                                            m_selection.Set(start + 1, start + 2);
                                        }
                                    }
                                    AdjustMaxDay();
                                }
                                else
                                    base.HandleKeyPress(sender, e);
                            }
                            // Check if we can insert the digit with a leading zero
                            else if (length == start && GetMinMonthDigit(0) == '0' && IsValidMonthDigit(c, 1))
                                m_selection.SetAndReplace(start, start + 2, "0" + c);
                        }
                        break;
                    }
                case 1:     // SECOND DIGIT
                    {
                        if (__fShowDayBeforeMonth_)
                        {
                            if (IsValidDayDigit(c, 1))
                            {
                                if (length > start)
                                    m_selection.SetAndReplace(start, start + 1, c.ToString());
                                else
                                    base.HandleKeyPress(sender, e);
                            }
                            // Check if it's a slash and the first digit (preceded by a zero) is a valid month
                            else if (c == fSeparator && length == start && GetMinDayDigit(0) == '0' && IsValidDay(ToInt("0" + text[0])))
                                m_selection.SetAndReplace(0, start, "0" + text[0] + c);
                        }
                        else
                        {
                            if (IsValidMonthDigit(c, 1))
                            {
                                if (length > start)
                                {
                                    m_selection.SetAndReplace(start, start + 1, c.ToString());

                                    if (Day > 0 && AdjustMaxDay())
                                        m_selection.Set(GetDayStartPosition(), GetDayStartPosition() + 2);
                                }
                                else
                                    base.HandleKeyPress(sender, e);
                            }
                            // Check if it's a slash and the first digit (preceded by a zero) is a valid month
                            else if (c == fSeparator && length == start && GetMinMonthDigit(0) == '0' && IsValidMonth(ToInt("0" + text[0])))
                                m_selection.SetAndReplace(0, start, "0" + text[0] + c);
                        }
                        break;
                    }
                case 2:     // FIRST SLASH
                    {
                        int slash = 0;
                        if (c == fSeparator)
                            slash = 1;
                        else
                        {
                            if (__fShowDayBeforeMonth_)
                                slash = (IsValidMonthDigit(c, 0) || (length == start && GetMinMonthDigit(0) == '0' && IsValidMonthDigit(c, 1))) ? 2 : 0;
                            else
                                slash = (IsValidDayDigit(c, 0) || (length == start && GetMinDayDigit(0) == '0' && IsValidDayDigit(c, 1))) ? 2 : 0;
                        }

                        // If we need the slash, enter it
                        if (slash != 0)
                            m_selection.SetAndReplace(start, start + 1, fSeparator.ToString());

                        // If the slash is to be preceded by a valid digit, "type" it in.
                        if (slash == 2)
                            SendKeys.Send(c.ToString());
                        break;
                    }
                case 3:     // THIRD DIGIT
                    {
                        if (__fShowDayBeforeMonth_)
                        {
                            if (IsValidMonthDigit(c, 0))
                            {
                                if (length > start)
                                {
                                    m_selection.SetAndReplace(start, start + 1, c.ToString());

                                    if (length > start + 1)
                                    {
                                        if (!IsValidMonth(__fMonth_))
                                        {
                                            m_selection.SetAndReplace(start + 1, start + 2, GetMinMonthDigit(1).ToString());
                                            m_selection.Set(start + 1, start + 2);
                                        }
                                    }
                                }
                                else
                                    base.HandleKeyPress(sender, e);

                                AdjustMaxDay();
                            }
                            // Check if we can insert the digit with a leading zero
                            else if (length == start && GetMinMonthDigit(0) == '0' && IsValidMonthDigit(c, 1))
                            {
                                m_selection.SetAndReplace(start, start + 2, "0" + c);
                                AdjustMaxDay();
                            }
                        }
                        else
                        {
                            if (IsValidDayDigit(c, 0))
                            {
                                if (length > start)
                                {
                                    m_selection.SetAndReplace(start, start + 1, c.ToString());

                                    if (length > start + 1)
                                    {
                                        if (!IsValidDay(Day))
                                        {
                                            m_selection.SetAndReplace(start + 1, start + 2, GetMinDayDigit(1).ToString());
                                            m_selection.Set(start + 1, start + 2);
                                        }
                                    }
                                }
                                else
                                    base.HandleKeyPress(sender, e);
                            }
                            // Check if we can insert the digit with a leading zero
                            else if (length == start && GetMinDayDigit(0) == '0' && IsValidDayDigit(c, 1))
                                m_selection.SetAndReplace(start, start + 2, "0" + c);
                        }
                        break;
                    }
                case 4:     // FOURTH DIGIT
                    {
                        if (__fShowDayBeforeMonth_)
                        {
                            if (IsValidMonthDigit(c, 1))
                            {
                                if (length > start)
                                {
                                    m_selection.SetAndReplace(start, start + 1, c.ToString());

                                    if (Day > 0 && AdjustMaxDay())
                                        m_selection.Set(GetDayStartPosition(), GetDayStartPosition() + 2);
                                }
                                else
                                {
                                    base.HandleKeyPress(sender, e);
                                    AdjustMaxDay();
                                }
                            }
                            // Check if it's a slash and the first digit (preceded by a zero) is a valid month
                            else if (c == fSeparator && length == start && GetMinMonthDigit(0) == '0' && IsValidMonth(ToInt("0" + text[3])))
                                m_selection.SetAndReplace(3, start, "0" + text[3] + c);
                        }
                        else
                        {
                            if (IsValidDayDigit(c, 1))
                            {
                                if (length > start)
                                    m_selection.SetAndReplace(start, start + 1, c.ToString());
                                else
                                    base.HandleKeyPress(sender, e);
                            }
                            // Check if it's a slash and the first digit (preceded by a zero) is a valid month
                            else if (c == fSeparator && length == start && GetMinDayDigit(0) == '0' && IsValidDay(ToInt("0" + text[3])))
                                m_selection.SetAndReplace(3, start, "0" + text[3] + c);
                        }
                        break;
                    }
                case 5:     // SECOND SLASH	(year's first digit)
                    {
                        int slash = 0;
                        if (c == fSeparator)
                            slash = 1;
                        else
                            slash = (IsValidYearDigit(c, 0) ? 2 : 0);

                        // If we need the slash, enter it
                        if (slash != 0)
                            m_selection.SetAndReplace(start, start + 1, fSeparator.ToString());

                        // If the slash is to be preceded by a valid digit, "type" it in.
                        if (slash == 2)
                            SendKeys.Send(c.ToString());
                        break;
                    }
                case 6:     // YEAR (all 4 digits)
                case 7:
                case 8:
                case 9:
                    {
                        if (IsValidYearDigit(c, start - GetYearStartPosition()))
                        {
                            if (length > start)
                            {
                                m_selection.SetAndReplace(start, start + 1, c.ToString());

                                for (; start + 1 < length && start < 9; start++)
                                {
                                    if (!IsValidYearDigit(text[start + 1], start - (GetYearStartPosition() - 1)))
                                    {
                                        m_selection.Set(start + 1, 10);
                                        StringBuilder portion = new StringBuilder();
                                        for (int iPos = start + 1; iPos < length && iPos < 10; iPos++)
                                            portion.Append(GetMinYearDigit(iPos - GetYearStartPosition(), false));

                                        m_selection.Replace(portion.ToString());
                                        m_selection.Set(start + 1, 10);
                                        break;
                                    }
                                }
                            }
                            else
                                base.HandleKeyPress(sender, e);

                            if (IsValidYear(Year))
                            {
                                AdjustMaxDay();         // adjust the day first
                                AdjustMaxMonthAndDay(); // then adjust the month and the day, if necessary
                            }
                        }
                        break;
                    }
            }
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Корректирует день (до максимального значения), если значение недействительно
        /// </summary>
        /// <returns>
        /// Если дата корректируется, возвращаемое значение равно true; в противном случае — false
        /// </returns>
        protected bool AdjustMaxDay()
        {
            int day = Day;
            if (day != 0 && !IsValidDay(day))
            {
                Day = GetMaxDay();
                return true;
            }

            return false;   // nothing had to be adjusted
        }
        /// <summary>
        /// Если значение недействительно, корректирует месяц (до минимального значения); если значение недействительно, корректирует день (до максимального значения)
        /// </summary>
        /// <returns>
        /// Если месяц и/или день изменяются, возвращаемое значение равно true; в противном случае — false
        /// </returns>
        protected bool AdjustMaxMonthAndDay()
        {
            int month = __fMonth_;
            if (month != 0 && !IsValidMonth(month))
            {
                __fMonth_ = GetMinMonth();  // this adjusts the day automatically
                return true;
            }

            return AdjustMaxDay();
        }
        /// <summary>
        /// Получает или задает день недели в текстовом поле
        /// </summary>
        /// <remarks>
        /// Если день в текстовом поле недопустим, это свойство вернет 0.
        /// Это свойство должно быть установлено с днем, который находится в допустимом диапазоне
        /// </remarks>
        public int Day
        {
            get
            {
                string text = m_textBox.Text;

                int startPos = GetDayStartPosition();
                int slash = text.IndexOf(fSeparator);

                if (startPos != 0 && slash > 0)
                    startPos = slash + 1;

                if (text.Length >= startPos + 2)
                    return ToInt(text.Substring(startPos, 2));
                return 0;
            }
            set
            {
                // Verify it's in range
                if (!IsValidDay(value))
                    throw new ArgumentOutOfRangeException();

                using (elmTypeSelection.Saver savedSelection = new elmTypeSelection.Saver(m_textBox))   // remember the current selection
                {
                    if (Day > 0)        // see if there's already a day
                        m_selection.Set(GetDayStartPosition(), GetDayStartPosition() + 3);

                    m_selection.Replace(TwoDigits(value) + fSeparator);    // set the day
                }
            }
        }
        /// <summary>
        /// Извлекает начальную позицию дня из текстового поля
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это начальная позиция дня
        /// </returns>
        /// <remarks>
        /// Это зависит от того, указан ли день до или после месяца
        /// </remarks>
        protected int GetDayStartPosition()
        {
            return __fShowDayBeforeMonth_ ? 0 : 3;
        }
        /// <summary>
        /// Преобразует значения года, месяца и дня в строку в соответствии с заданным форматом (мм/дд/гггг или дд/мм/гггг)
        /// </summary>
        /// <param name="year">Годовое значение</param>
        /// <param name="month">Значение за месяц</param>
        /// <param name="day">Дневная стоимость</param>
        /// <returns>
        /// Возвращаемое значение представляет собой отформатированное значение даты.
        /// </returns>
        public string GetFormattedDate(int year, int month, int day)
        {
            if (__fShowDayBeforeMonth_)
                return String.Format("{0,2:00}{1}{2,2:00}{3}{4,4:0000}", day, fSeparator, month, fSeparator, year);
            return String.Format("{0,2:00}{1}{2,2:00}{3}{4,4:0000}", month, fSeparator, day, fSeparator, year);
        }
        /// <summary>
        /// Извлекает максимальное значение за день на основе месяца, года и допустимого диапазона
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это максимальное значение за день
        /// </returns>
        protected int GetMaxDay()
        {
            int year = GetValidYear();
            int month = GetValidMonth();

            if (year == fValueMax.Year && month == fValueMax.Month)
                return fValueMax.Day;

            return GetMaxDayOfMonth(month, year);
        }  /// <summary>
           /// Извлекает максимальное количество цифр, которое может принимать значение дня, в одной из двух позиций символа
           /// </summary>
           /// <param name="position">Положение цифры дня недели (0 или 1)</param>
           /// <returns>
           /// Возвращаемое значение — это максимально допустимое количество цифр
           /// </returns>
        protected char GetMaxDayDigit(int position)
        {
            Debug.Assert(position >= 0 && position <= 1);

            int month = GetValidMonth();
            int year = GetValidYear();
            int maxDay = fValueMax.Day;
            int maxMonth = fValueMax.Month;
            int maxYear = fValueMax.Year;

            // First digit
            if (position == 0)
            {
                // If the year and month are at the max, then use the first digit of the max day
                if (year == maxYear && month == maxMonth)
                    return TwoDigits(maxDay)[0];
                return TwoDigits(GetMaxDayOfMonth(month, year))[0];
            }

            // Second digit
            string text = m_textBox.Text;
            char firstDigit = (text.Length > GetDayStartPosition()) ? text[GetDayStartPosition()] : '0';
            Debug.Assert(firstDigit != 0);  // must have a valid first digit at this point

            // If the year and month are at the max, then use the second digit of the max day
            if (year == maxYear && month == maxMonth && TwoDigits(maxDay)[0] == firstDigit)
                return TwoDigits(maxDay)[1];

            if (firstDigit == '0' ||
                firstDigit == '1' ||
                (firstDigit == '2' && month != 2) ||
                (month == 2 && !IsValidYear(Year)))
                return '9';
            return TwoDigits(GetMaxDayOfMonth(month, year))[1];
        }
        /// <summary>
        /// Извлекает максимальное количество дней за заданный месяц и год
        /// </summary>
        /// <param name="month">Месяц (1 - 12)</param>
        /// <param name="year">Год (1900 - 9999)</param>
        /// <returns>
        /// Возвращаемое значение — максимальный день (1–31)
        /// </returns>
        protected static int GetMaxDayOfMonth(int month, int year)
        {
            Debug.Assert(month >= 1 && month <= 12);

            switch (month)
            {
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;

                case 2:
                    return DateTime.IsLeapYear(year) ? 29 : 28;
            }
            return 31;
        }
        /// <summary>
        /// Извлекает максимальное значение за месяц на основе года и допустимого диапазона
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это максимальное значение за месяц
        /// </returns>
        protected int GetMaxMonth()
        {
            if (GetValidYear() == fValueMax.Year)
                return fValueMax.Month;

            return 12;
        }
        /// <summary>
        /// Извлекает максимальное количество цифр, которое может принимать значение месяца, в одной из двух позиций символов
        /// </summary>
        /// <param name="position">
        /// Положение цифры месяца (0 или 1
        /// </param>
        /// <returns>
        /// Возвращаемое значение — это максимально допустимое количество цифр
        /// </returns>
        protected char GetMaxMonthDigit(int position)
        {
            Debug.Assert(position >= 0 && position <= 1);

            int year = GetValidYear();
            int maxMonth = fValueMax.Month;
            int maxYear = fValueMax.Year;

            // First digit
            if (position == 0)
            {
                // If the year is at the max, then use the first digit of the max month
                if (year == maxYear)
                    return TwoDigits(maxMonth)[0];

                // Otherwise, it's always '1'
                return '1';
            }

            // Second digit
            string text = m_textBox.Text;
            char firstDigit = (text.Length > GetMonthStartPosition()) ? text[GetMonthStartPosition()] : '0';
            Debug.Assert(firstDigit != 0);  // must have a valid first digit at this point

            // If the year is at the max, then check if the first digits match
            if (year == maxYear && (IsValidYear(Year) || maxYear == fValueMin.Year))
            {
                // If the first digits match, then use the second digit of the max month
                if (TwoDigits(maxMonth)[0] == firstDigit)
                    return TwoDigits(maxMonth)[1];

                // Assuming the logic for the first digit is correct, then it must be '0'
                Debug.Assert(firstDigit == '0');
                return '9';
            }

            // Use the first digit to determine the second digit's max
            return (firstDigit == '1' ? '2' : '9');
        }
        /// <summary>
        /// Извлекает максимальное количество цифр, которое может принимать значение года, в одной из четырех позиций символов.
        /// </summary>
        /// <param name="position">The position of the digit of the day (0 to 3)</param>
        /// <returns>
        /// Возвращаемое значение — это максимально допустимое количество цифр
        /// </returns>
        protected char GetMaxYearDigit(int position)
        {
            Debug.Assert(position >= 0 && position <= 3);

            string yearStr = "" + Year;
            string maxYear = "" + fValueMax.Year;

            if (position == 0 || ToInt(maxYear.Substring(0, position)) <= ToInt(yearStr.Substring(0, position)))
                return maxYear[position];
            return '9';
        }
        /// <summary>
        /// Определяет минимальное значение за день на основе месяца, года и допустимого диапазона
        /// </summary>
        /// <returns>
        /// Возвращаемая сумма — это минимальное значение за день
        /// </returns>
        protected int GetMinDay()
        {
            int year = GetValidYear();
            int month = GetValidMonth();

            if (year == fValueMin.Year && month == fValueMin.Month)
                return fValueMin.Day;

            return 1;
        }
        /// <summary>
        /// Извлекает минимальную цифру, которую может принимать значение дня, в одной из двух позиций символа
        /// </summary>
        /// <param name="position">Положение цифры дня недели (0 или 1)</param>
        /// <returns>
        /// Возвращаемое значение — это минимальное количество знаков после запятой, которое оно может содержать
        /// </returns>
        protected char GetMinDayDigit(int position)
        {
            Debug.Assert(position >= 0 && position <= 1);

            int month = GetValidMonth();
            int year = GetValidYear();
            int minDay = fValueMin.Day;
            int minMonth = fValueMin.Month;
            int minYear = fValueMin.Year;

            // First digit
            if (position == 0)
            {
                // If the year and month are at the min, then use the first digit of the min day
                if (year == minYear && month == minMonth)
                    return TwoDigits(minDay)[0];
                return '0';
            }

            // Second digit
            string text = m_textBox.Text;
            char firstDigit = (text.Length > GetDayStartPosition()) ? text[GetDayStartPosition()] : '0';
            if (firstDigit == 0)  // must have a valid first digit at this point
                return '1';

            // If the year and month are at the max, then use the first second of the max day
            if (year == minYear && month == minMonth && TwoDigits(minDay)[0] == firstDigit)
                return TwoDigits(minDay)[1];

            // Use the first digit to determine the second digit's min
            return (firstDigit == '0' ? '1' : '0');
        }
        /// <summary>
        /// Извлекает минимальное значение за месяц на основе года и допустимого диапазона
        /// </summary>
        /// <returns>
        /// Возвратная стоимость — это минимальное значение за месяц
        /// </returns>
        protected int GetMinMonth()
        {
            if (GetValidYear() == fValueMin.Year)
                return fValueMin.Month;
            return 1;
        }
        /// <summary>
        /// Retrieves the minimum digit that a month value can take, at one of its two character positions
        /// </summary>
        /// <param name="position">Положение цифры месяца (0 или 1)</param>
        /// <returns>
        /// Позиция цифры месяца (0 или 1). Возвращаемое значение — это минимально возможная цифра
        /// </returns>
        protected char GetMinMonthDigit(int position)
        {
            Debug.Assert(position >= 0 && position <= 1);

            int year = GetValidYear();
            int minMonth = fValueMin.Month;
            int minYear = fValueMin.Year;

            // First digit
            if (position == 0)
            {
                // If the year is at the min, then use the first digit of the min month
                if (year == minYear)
                    return TwoDigits(minMonth)[0];

                // Otherwise, it's always '0'
                return '0';
            }

            // Second digit
            string text = m_textBox.Text;
            char firstDigit = (text.Length > GetMonthStartPosition()) ? text[GetMonthStartPosition()] : '0';
            if (firstDigit == 0)
                return '1';

            // If the year is at the max, then check if the first digits match
            if (year == minYear && (IsValidYear(Year) || minYear == fValueMax.Year))
            {
                // If the first digits match, then use the second digit of the max month
                if (TwoDigits(minMonth)[0] == firstDigit)
                    return TwoDigits(minMonth)[1];

                return '0';
            }

            // Use the first digit to determine the second digit's min
            return (firstDigit == '1' ? '0' : '1');
        }
        /// <summary>
        /// Извлекает минимальную цифру, которую может принимать значение года, в одной из четырех позиций символов
        /// </summary>
        /// <param name="position">Положение цифры дня недели (от 0 до 3)</param>
        /// <param name="validYear">Если это так, используется действующий год, если текущий год не соответствует действительности</param>
        /// <returns>
        /// Возвращаемое значение — это минимальное количество знаков после запятой, которое оно может содержать.
        /// </returns>
        protected char GetMinYearDigit(int position, bool validYear)
        {
            Debug.Assert(position >= 0 && position <= 3);

            int year = Year;
            if (validYear && !IsValidYear(year))
                year = GetValidYear();

            string yearStr = "" + year;
            string minYear = "" + fValueMin.Year;

            if (position == 0 || ToInt(minYear.Substring(0, position)) >= ToInt(yearStr.Substring(0, position)))
                return minYear[position];
            return '0';
        }
        /// <summary>
        /// Получает начальную позицию месяца в текстовом поле
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это начальная позиция месяца
        /// </returns>
        /// <remarks>
        /// Это зависит от того, указан ли месяц до или после дня недели
        /// </remarks>
        protected int GetMonthStartPosition()
        {
            return __fShowDayBeforeMonth_ ? 3 : 0;
        }
        /// <summary>
        /// Извлекает день из текстового поля как допустимое значение
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это допустимое значение на текущий день (с 1 по 31)
        /// </returns>
        /// <remarks>
        /// Метод проверяет значение дня в текстовом поле.
        /// Если значение находится в допустимом диапазоне, он возвращает его.
        /// Если значение меньше минимально допустимого, возвращается минимальное значение.
        /// Если значение больше максимально допустимого, возвращается максимальное значение
        /// </remarks>
        protected int GetValidDay()
        {
            int day = Day;

            // It it's outside the range, fix it
            if (day < GetMinDay())
                day = GetMinDay();
            else if (day > GetMaxDay())
                day = GetMaxDay();

            return day;
        }
        /// <summary>
        /// Извлекает месяц из текстового поля как допустимое значение
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это допустимое значение за месяц (1–12)
        /// </returns>
        /// <remarks>
        /// Метод проверяет значение месяца в текстовом поле.
        /// Если оно находится в допустимом диапазоне, возвращает его.
        /// Если оно меньше минимально допустимого значения, возвращается минимальное значение.
        /// Если оно больше максимально допустимого значения, возвращается максимальное значение
        /// </remarks>
        protected int GetValidMonth()
        {
            int month = __fMonth_;

            // It it's outside the range, fix it
            if (month < GetMinMonth())
                month = GetMinMonth();
            else if (month > GetMaxMonth())
                month = GetMaxMonth();

            return month;
        }
        /// <summary>
        /// Извлекает год из текстового поля как допустимое значение
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это допустимое значение за год
        /// </returns>
        /// <remarks>
        /// Метод проверяет значение года в текстовом поле.
        /// Если оно находится в допустимом диапазоне, возвращает его.
        /// Если оно меньше минимально допустимого значения, возвращается минимальное значение.
        /// Если оно больше максимально допустимого значения, возвращается максимальное значение
        /// </remarks>
        protected int GetValidYear()
        {
            int year = Year;
            if (year < fValueMin.Year)
            {
                year = DateTime.Today.Year;
                if (year < fValueMin.Year)
                    year = fValueMin.Year;
            }
            if (year > fValueMax.Year)
                year = fValueMax.Year;

            return year;
        }
        /// <summary>
        /// Извлекает нулевой индекс года из текстового поля
        /// </summary>
        /// <returns>
        /// Доходность представляет собой исходное положение в году
        /// </returns>
        /// <remarks>
        /// Это всегда 6
        /// </remarks>
        protected int GetYearStartPosition()
        {
            return 6;
        }
        /// <summary>
        /// Проверяет, является ли дата в текстовом поле допустимой и находится ли она в пределах разрешенного диапазона
        /// </summary>
        /// <returns>
        /// Если значение допустимо и находится в пределах допустимого диапазона, возвращается значение true; в противном случае — false
        /// </returns>
        public override bool __mIsValid()
        {
            try
            {
                return IsWithinRange(new DateTime(Year, __fMonth_, Day));
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Проверяет, действителен ли день — находится ли он в допустимом диапазоне
        /// </summary>
        /// <param name="day">День проверки</param>
        /// <returns>
        /// Если дата попадает в допустимый диапазон, возвращается значение true; в противном случае — false
        /// </returns>
        protected bool IsValidDay(int day)
        {
            try
            {
                return IsWithinRange(new DateTime(GetValidYear(), GetValidMonth(), day));
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Проверяет, является ли месяц действительным — находится ли он в допустимом диапазоне
        /// </summary>
        /// <param name="month">Месяц для проверки</param>
        /// <returns>
        /// Если месяц попадает в допустимый диапазон, возвращается значение true; в противном случае — false
        /// </returns>
        protected bool IsValidMonth(int month)
        {
            int year = GetValidYear();
            int day = GetValidDay();
            try
            {
                return IsWithinRange(new DateTime(year, month, day));
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Проверяет, является ли цифра допустимой на текущий день в одной из двух позиций символа
        /// </summary>
        /// <param name="c">Цифра для проверки</param>
        /// <param name="position">Положение цифры дня недели (0 или 1)</param>
        /// <returns>
        /// Если цифра действительна в данный день (в указанной позиции), возвращаемое значение равно true; в противном случае — false
        /// </returns>
        protected bool IsValidDayDigit(char c, int position)
        {
            return (c >= GetMinDayDigit(position) && c <= GetMaxDayDigit(position));
        }
        /// <summary>
        /// Проверяет, является ли цифра допустимой для месяца в одной из двух позиций символов
        /// </summary>
        /// <param name="c">Цифра для проверки</param>
        /// <param name="position">Положение цифры месяца (0 или 1)</param>
        /// <returns>
        /// Если цифра действительна для месяца (в указанной позиции), возвращаемое значение равно true; в противном случае — false
        /// </returns>
        protected bool IsValidMonthDigit(char c, int position)
        {
            return (c >= GetMinMonthDigit(position) && c <= GetMaxMonthDigit(position));
        }
        /// <summary>
        /// Извлекает текст из текстового поля в корректном виде
        /// </summary>
        /// <returns>
        /// Если текст в текстовом поле допустим, он возвращается; в противном случае возвращается допустимая версия текста
        /// </returns>
        protected override string GetValidText()
        {
            string text = m_textBox.Text;

            if (text == "")
                return text;

            if (__mIsValid())
                return GetFormattedDate(Year, __fMonth_, Day);

            // If the date is empty, try using today
            if (Year == 0 && __fMonth_ == 0 && Day == 0)
                __fValue_ = DateTime.Today;

            int year = GetValidYear();
            int month = GetValidMonth();
            int day = GetValidDay();

            if (!IsWithinRange(new DateTime(year, month, day)))
                month = GetMinMonth();

            if (!IsWithinRange(new DateTime(year, month, day)))
                day = GetMaxDay();

            return GetFormattedDate(year, month, day);
        }
        /// <summary>
        /// Извлекает текст из текстового поля в корректном виде
        /// </summary>
        /// <returns>
        /// Это всего лишь внутренняя версия класса <see cref="GetValidText" />, предназначенная для доступа к ней со стороны класса <see cref="DateTimeBehavior" />, которому она необходима.
        /// </returns>
        internal string GetValidTextForDateTime()
        {
            return GetValidText();
        }
        /// <summary>
        /// Проверяет, действителен ли год — находится ли он в допустимом диапазоне
        /// </summary>
        /// <param name="year">Год для проверки</param>
        /// <returns>
        /// Если год находится в допустимом диапазоне, возвращаемое значение равно true; в противном случае — false
        /// </returns>
        protected bool IsValidYear(int year)
        {
            return (year >= fValueMin.Year && year <= fValueMax.Year);
        }
        /// <summary>
        /// Проверяет, является ли цифра допустимой для года в одной из четырех позиций символов
        /// </summary>
        /// <param name="c">Цифра для проверки</param>
        /// <param name="position">Положение цифры дня недели (от 0 до 3)</param>
        /// <returns>
        /// Если цифра действительна для данного года (в указанной позиции), возвращаемое значение равно true; в противном случае — false
        /// </returns>
        protected bool IsValidYearDigit(char c, int position)
        {
            return (c >= GetMinYearDigit(position, false) && c <= GetMaxYearDigit(position));
        }
        /// <summary>
        /// Проверяет, находится ли значение даты в допустимом диапазоне
        /// </summary>
        /// <param name="dt">Значение даты, которое нужно проверить</param>
        /// <returns>
        /// Если значение находится в допустимом диапазоне, возвращается значение true; в противном случае — false
        /// </returns>
        /// <remarks>
        /// Проверяется только дата; время игнорируется
        /// </remarks>
        public bool IsWithinRange(DateTime dt)
        {
            DateTime date = new DateTime(dt.Year, dt.Month, dt.Day);
            return (date >= fValueMin && date <= fValueMax);
        }
        /// <summary>
        /// Получает или задает месяц в текстовом поле
        /// </summary>
        /// <remarks>
        /// Если месяц в текстовом поле указан неверно, это свойство вернет 0.
        /// Это свойство должно быть установлено с указанием месяца, попадающего в допустимый диапазон
        /// </remarks>
        public int __fMonth_
        {
            get
            {
                string text = m_textBox.Text;

                int startPos = GetMonthStartPosition();
                int slash = text.IndexOf(fSeparator);

                if (startPos != 0 && slash > 0)
                    startPos = slash + 1;

                if (text.Length >= startPos + 2)
                    return ToInt(text.Substring(startPos, 2));
                return 0;
            }
            set
            {
                using (elmTypeSelection.Saver savedSelection = new elmTypeSelection.Saver(m_textBox))   // remember the current selection
                {
                    if (__fMonth_ > 0)      // see if there's already a month
                        m_selection.Set(GetMonthStartPosition(), GetMonthStartPosition() + 3);

                    m_selection.Replace(TwoDigits(value) + fSeparator);    // set the month

                    AdjustMaxDay(); // adjust the day if it's out of range

                    // Verify it's in range
                    if (!IsValidMonth(value))
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        /// <summary>
        /// Устанавливает месяц, день и год в текстовом поле.
        /// </summary>
        /// <param name="year">Год для начала работы</param>
        /// <param name="month">Месяц для начала работы</param>
        /// <param name="day">День начала</param>
        /// <remarks>
        /// Это удобный метод для установки каждого значения по отдельности с помощью одного метода.
        /// Объект <see cref="DateTime" /> создается с использованием этих параметров, поэтому они должны быть допустимыми
        /// </remarks>
        public void SetDate(int year, int month, int day)
        {
            __fValue_ = new DateTime(year, month, day);
        }
        /// <summary>
        /// Преобразует целое число в двухзначную строку (00–99)
        /// </summary>
        /// <param name="value">Число для конвертации</param>
        /// <returns>
        /// Возвращаемое значение представляет собой отформатированную строку
        /// </returns>
        /// <remarks>
        /// Это удобный способ форматирования двузначных значений, таких как месяц и день
        /// </remarks>
        protected static string TwoDigits(int value)
        {
            return String.Format("{0,2:00}", value);
        }
        /// <summary>
        /// Gets or sets the year on the textbox
        /// </summary>
        /// <remarks>
        /// If the year is not valid on the textbox, this property will return 0.
        /// This property must be set with a year that falls within the allowed range
        /// </remarks>
        public int Year
        {
            get
            {
                string text = m_textBox.Text;
                int length = text.Length;

                int slash = text.LastIndexOf(fSeparator);
                if (slash > 0 && slash < length - 1)
                    return ToInt(text.Substring(slash + 1, Math.Min(4, length - slash - 1)));
                return 0;
            }
            set
            {
                // Verify it's in range
                if (!IsValidYear(value))
                    throw new ArgumentOutOfRangeException();

                using (elmTypeSelection.Saver savedSelection = new elmTypeSelection.Saver(m_textBox))   // remember the current selection
                {
                    if (Year > 0)       // see if there's already a year
                        m_selection.Set(GetYearStartPosition(), GetYearStartPosition() + 4);

                    m_selection.Replace(String.Format("{0,4:0000}", value));    // set the year

                    AdjustMaxMonthAndDay(); // adjust the month and/or day if they're out of range
                }
            }
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПЕРЕЧИСЛЕНИЯ

        /// <summary>
        /// Внутренние значения, которые добавляются/удаляются из свойства <see cref="elmTypeBehavior.Flags" /> другими свойствами этого класса
        /// </summary>
        [Flags]
        protected enum FLAG
        {
            /// <summary> 
            /// День недели отображается перед месяцем
            /// </summary>
            DayBeforeMonth = 0x00010000,
        };

        #endregion ПЕРЕЧИСЛЕНИЯ

        #region = ПОЛЯ

        /// <summary>
        /// Максимально допустимое значение 
        /// </summary>
        private DateTime fValueMax = new DateTime(9998, 12, 31);
        /// <summary>
        /// Минимально допустимое значение
        /// </summary>
        private DateTime fValueMin = new DateTime(1900, 1, 1);
        /// <summary>
        /// Символ, используемый для разделения значений месяца, дня и года в дате
        /// </summary>
        private char fSeparator = '/';

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Получает сообщение об ошибке, используемое для уведомления пользователя о необходимости ввести допустимое значение даты в пределах разрешенного диапазона
        /// </summary>
        public override string ErrorMessage
        {
            get
            {
                return "Please specify a date between " + GetFormattedDate(fValueMin.Year, fValueMin.Month, fValueMin.Day) + " and " + GetFormattedDate(fValueMax.Year, fValueMax.Month, fValueMax.Day) + ".";
            }
        }
        /// <summary>
        /// Получает или устанавливает максимально допустимое значение
        /// </summary>
        public DateTime __fValueMax_
        {
            get
            {
                return fValueMax;
            }
            set
            {
                fValueMax = value;
                UpdateText();
            }
        }
        /// <summary>
        /// Получает или устанавливает минимально допустимое значение
        /// </summary>
        public DateTime __fValueMin_
        {
            get
            {
                return fValueMin;
            }
            set
            {
                if (value < new DateTime(1900, 1, 1))
                    throw new ArgumentOutOfRangeException("RangeMin", value, "Minimum value may not be older than January 1, 1900");

                fValueMin = value;
                UpdateText();
            }
        }
        /// <summary>
        /// Получает или устанавливает символ, используемый для разделения значений месяца, дня и года в дате
        /// </summary>
        public char __fSeparator_
        {
            get
            {
                return fSeparator;
            }
            set
            {
                if (fSeparator == value)
                    return;

                Debug.Assert(value != 0);
                Debug.Assert(!Char.IsDigit(value));

                fSeparator = value;
                UpdateText();
            }
        }
        /// <summary>
        /// Определяет, следует ли отображать день недели перед месяцем или после него
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство устанавливается в соответствии с настройками системы пользователя.
        /// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />.        /// </remarks>
        public bool __fShowDayBeforeMonth_
        {
            get
            {
                return HasFlag((int)FLAG.DayBeforeMonth);
            }
            set
            {
                ModifyFlags((int)FLAG.DayBeforeMonth, value);
            }
        }
        /// <summary>
        /// Получает или задает месяц, день и год в текстовом поле с помощью объекта <see cref="DateTime" />
        /// </summary>
        /// <remarks>
        /// Это свойство получает и устанавливает значение <see cref="DateTime" />, заключенное в <c>объект</c>.
        /// Это обеспечивает гибкость, так что если текстовое поле не содержит допустимой даты, возвращается <c>null</c>,
        /// вместо того, чтобы беспокоиться о возникновении исключения
        /// </remarks>        
        /// <example>
        ///   object obj = txtDate.Behavior.Value;
        ///   
        ///   if (obj != null)
        ///   {
        ///     DateTime dtm = (DateTime)obj;
        ///     ...
        ///   } 
        /// </example>
        public object __fValue_
        {
            get
            {
                try
                {
                    return new DateTime(Year, __fMonth_, Day);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                DateTime dt = (DateTime)value;
                m_textBox.Text = GetFormattedDate(dt.Year, dt.Month, dt.Day);
            }
        }

        #endregion СВОЙСТВА
	}

    /// <summary>
    /// Класс поведения, обрабатывающий ввод значений времени
    /// </summary>
    /// <remarks>
    /// Эта функция поддерживает значения времени в 12- или 24-часовом формате, с секундами или без них.
    /// Она разработана для того, чтобы пользователь мог быстро и точно ввести значение времени.
    /// По мере ввода цифр двоеточия автоматически заполняются. Пользователь может удалять только символы справа от введенного значения. Это помогает сохранить правильное форматирование значения.
    /// Пользователь также может использовать клавиши со стрелками вверх/вниз для увеличения/уменьшения часа, минуты, секунды или AM/PM,
    /// в зависимости от положения курсора
    /// </remarks>
    public class TimeBehavior : elmTypeBehavior
	{
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Инициализирует новый экземпляр класса TimeBehavior, копируя его из другого объекта TimeBehavior.
        /// </summary>
        /// <param name="behavior">Объект TimeBehavior, который необходимо скопировать (а затем удалить). Он не должен быть равен null</param>
        /// <remarks>
        /// После копирования объекта behavior.TextBox вызывается метод Dispose для параметра behavior
        /// </remarks>
        public TimeBehavior(TimeBehavior behavior) : base(behavior)
        {
            m_rangeMin = behavior.m_rangeMin;
            m_rangeMax = behavior.m_rangeMax;
            m_separator = behavior.m_separator;
            m_am = behavior.m_am;
            m_pm = behavior.m_pm;
            m_ampmLength = behavior.m_ampmLength;
            m_hourStart = behavior.m_hourStart;
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса TimeBehavior, связывая его с объектом, производным от TextBoxBase
        /// </summary>
        /// <param name="textBox">Объект TextBoxBase, который необходимо связать с этим поведением. Он не должен быть равен null</param>
        /// <remarks>
        /// Этот конструктор определяет формат <see cref="Separator" /> и время из системы пользователя
        /// </remarks>
        public TimeBehavior(TextBoxBase textBox) : base(textBox, true)
        {
            // Get the system's time separator
            m_separator = DateTimeFormatInfo.CurrentInfo.TimeSeparator[0];

            // Determine if it's in 24-hour format
            string shortTime = DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
            if (shortTime.IndexOf('H') >= 0)
                m_flags |= (int)Flag.TwentyFourHourFormat;

            // Get the AM and PM symbols
            m_am = DateTimeFormatInfo.CurrentInfo.AMDesignator;
            m_pm = DateTimeFormatInfo.CurrentInfo.PMDesignator;
            m_ampmLength = m_am.Length;

            // Verify the lengths are the same; otherwise use the default
            if (m_ampmLength == 0 || m_ampmLength != m_pm.Length)
            {
                m_am = "AM";
                m_pm = "PM";
                m_ampmLength = 2;
            }
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Обрабатывает нажатия клавиш внутри текстового поля
        /// </summary>
        /// <param name="sender">Объект, отправивший событие</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод переопределен в классе Behavior и обрабатывает событие KeyDown текстового поля
        /// </remarks>
        protected override void HandleKeyDown(object sender, KeyEventArgs e)
        {
            TraceLine("TimeBehavior.HandleKeyDown " + e.KeyCode);

            // Check to see if it's read only
            if (m_textBox.ReadOnly)
                return;

            e.Handled = true;

            switch (e.KeyCode)
            {
                case Keys.Delete:
                    {
                        // If deleting make sure it's the last character or that
                        // the selection goes all the way to the end of the text

                        int start, end;
                        m_selection.Get(out start, out end);

                        string text = m_textBox.Text;
                        int length = text.Length;

                        if (end != length)
                        {
                            if (!(end == start && end == length - 1))
                                return;
                        }

                        m_noTextChanged = true;
                        break;
                    }

                case Keys.Up:
                    {
                        // If pressing the UP arrow, increment the corresponding value.

                        int start, end;
                        m_selection.Get(out start, out end);

                        if (start >= GetHourStartPosition() && start <= GetHourStartPosition() + 2)
                        {
                            int hour = Hour;
                            if (hour >= GetMinHour(false))
                            {
                                // Handle moving up through the noon hour
                                string ampm = AMPM;
                                if (IsValidAMPM(ampm))
                                {
                                    if (hour == 11)
                                    {
                                        if (ampm == m_pm)  // stop at midnight
                                            return;
                                        SetAMPM(false);
                                    }
                                    else if (hour == 12)
                                        hour = 0;
                                }

                                if (hour < GetMaxHour(false))
                                    Hour = ++hour;
                            }
                        }
                        else if (start >= GetMinuteStartPosition() && start <= GetMinuteStartPosition() + 2)
                        {
                            int minute = Minute;
                            if (minute >= GetMinMinute() && minute < GetMaxMinute())
                                Minute = ++minute;
                        }
                        else if (start >= GetAMPMStartPosition() && start <= GetAMPMStartPosition() + m_ampmLength)
                        {
                            string ampm = AMPM;
                            SetAMPM(!IsValidAMPM(ampm) || ampm == m_pm);
                        }
                        else if (start >= GetSecondStartPosition() && start <= GetSecondStartPosition() + 2)
                        {
                            int second = Second;
                            if (second >= GetMinSecond() && second < GetMaxSecond())
                                Second = ++second;
                        }

                        return;
                    }

                case Keys.Down:
                    {
                        // If pressing the DOWN arrow, decrement the corresponding value.

                        int start, end;
                        m_selection.Get(out start, out end);

                        if (start >= GetHourStartPosition() && start <= GetHourStartPosition() + 2)
                        {
                            int hour = Hour;
                            if (hour <= GetMaxHour(false))
                            {
                                // Handle moving up through the noon hour
                                string ampm = AMPM;
                                if (IsValidAMPM(ampm))
                                {
                                    if (hour == 12)
                                    {
                                        if (ampm == m_am)   // stop at midnight
                                            return;
                                        SetAMPM(true);
                                    }
                                    else if (hour == 1)
                                        hour = 13;
                                }

                                if (hour > GetMinHour(false))
                                    Hour = --hour;
                            }
                        }
                        else if (start >= GetMinuteStartPosition() && start <= GetMinuteStartPosition() + 2)
                        {
                            int minute = Minute;
                            if (minute > GetMinMinute() && minute <= GetMaxMinute())
                                Minute = --minute;
                        }
                        else if (start >= GetAMPMStartPosition() && start <= GetAMPMStartPosition() + m_ampmLength)
                        {
                            string ampm = AMPM;
                            SetAMPM(!IsValidAMPM(ampm) || ampm == m_pm);
                        }
                        else if (start >= GetSecondStartPosition() && start <= GetSecondStartPosition() + 2)
                        {
                            int second = Second;
                            if (second > GetMinSecond() && second <= GetMaxSecond())
                                Second = --second;
                        }
                        return;
                    }
            }

            base.HandleKeyDown(sender, e);
        }
        /// <summary>
        /// Обрабатывает нажатия клавиш внутри текстового поля.
        /// </summary>
        /// <param name="sender">Объект, отправивший событие</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>
        /// Этот метод переопределен в классе Behavior и обрабатывает событие KeyPress текстового поля
        /// </remarks>
        protected override void HandleKeyPress(object sender, KeyPressEventArgs e)
        {
            TraceLine("TimeBehavior.HandleKeyPress " + e.KeyChar);

            // Check to see if it's read only
            if (m_textBox.ReadOnly)
                return;

            char c = e.KeyChar;
            e.Handled = true;
            m_noTextChanged = true;

            int start, end;
            m_selection.Get(out start, out end);

            string text = m_textBox.Text;
            int length = text.Length;

            // Check for a non-printable character (such as Ctrl+C)
            if (Char.IsControl(c))
            {
                if (c == (short)Keys.Back && start != length)
                {
                    SendKeys.Send("{LEFT}");  // move the cursor left
                    return;
                }

                // Allow backspace only if the cursor is all the way to the right
                base.HandleKeyPress(sender, e);
                return;
            }

            // Add the digit depending on its location
            if (start == m_hourStart)       // FIRST DIGIT
            {
                if (IsValidHourDigit(c, 0))
                {
                    if (length > start)
                    {
                        m_selection.SetAndReplace(start, start + 1, c.ToString());

                        if (length > start + 1)
                        {
                            // If the second digit is no longer valid, correct and select it
                            if (!IsValidHour(Hour, false))
                            {
                                m_selection.SetAndReplace(start + 1, start + 2, GetMinHourDigit(1).ToString());
                                m_selection.Set(start + 1, start + 2);
                            }
                        }
                    }
                    else
                        base.HandleKeyPress(sender, e);
                }
                else if (length == start && IsValidHourDigit(c, 1))
                    m_selection.SetAndReplace(start, start + 2, "0" + c);
                else
                    ChangeAMPM(c);  // allow changing AM/PM (if it's being shown) by pressing A or P
            }

            else if (start == m_hourStart + 1)  // SECOND DIGIT
            {
                if (IsValidHourDigit(c, 1))
                {
                    if (length > start)
                        m_selection.SetAndReplace(start, start + 1, c.ToString());
                    else
                        base.HandleKeyPress(sender, e);
                }
                else if (c == m_separator && length == start && IsValidHour(ToInt("0" + text[m_hourStart]), false))
                    m_selection.SetAndReplace(m_hourStart, start, "0" + text[m_hourStart] + c);
                else
                    ChangeAMPM(c);  // allow changing AM/PM (if it's being shown) by pressing A or P
            }

            else if (start == m_hourStart + 2)  // FIRST COLON
            {
                int colon = 0;
                if (c == m_separator)
                    colon = 1;
                else
                    colon = (IsValidMinuteDigit(c, 0) ? 2 : 0);

                // If we need the colon, enter it
                if (colon != 0)
                    m_selection.SetAndReplace(start, start + 1, m_separator.ToString());

                // If the colon is to be preceded by a valid digit, "type" it in.
                if (colon == 2)
                    SendKeys.Send(c.ToString());
                else
                    ChangeAMPM(c);  // allow changing AM/PM (if it's being shown) by pressing A or P
            }

            else if (start == m_hourStart + 3)  // THIRD DIGIT
            {
                if (IsValidMinuteDigit(c, 0))
                {
                    if (length > start)
                    {
                        m_selection.SetAndReplace(start, start + 1, c.ToString());

                        if (length > start + 1)
                        {
                            if (!IsValidMinute(Minute))
                            {
                                m_selection.SetAndReplace(start + 1, start + 2, GetMinMinuteDigit(1).ToString());
                                m_selection.Set(start + 1, start + 2);
                            }
                        }
                    }
                    else
                        base.HandleKeyPress(sender, e);
                }
                else
                    ChangeAMPM(c);  // allow changing AM/PM (if it's being shown) by pressing A or P
            }

            else if (start == m_hourStart + 4)  // FOURTH DIGIT
            {
                if (IsValidMinuteDigit(c, 1))
                {
                    if (length > start)
                        m_selection.SetAndReplace(start, start + 1, c.ToString());
                    else
                        base.HandleKeyPress(sender, e);

                    // Show the AM/PM symbol if we're not showing seconds
                    if (!ShowSeconds)
                        ShowAMPM();
                }
                else
                    ChangeAMPM(c);  // allow changing AM/PM (if it's being shown) by pressing A or P
            }

            else if (start == m_hourStart + 5)  // SECOND COLON	OR FIRST SPACE (seconds' first digit or AM/PM)
            {
                if (ShowSeconds)
                {
                    int colon = 0;
                    if (c == m_separator)
                        colon = 1;
                    else
                        colon = (IsValidSecondDigit(c, 0) ? 2 : 0);

                    // If we need the slash, enter it
                    if (colon != 0)
                    {
                        int replace = (start < length && text[start] != ' ') ? 1 : 0;
                        m_selection.SetAndReplace(start, start + replace, m_separator.ToString());
                    }

                    // If the colon is to be preceded by a valid digit, "type" it in.
                    if (colon == 2)
                        SendKeys.Send(c.ToString());
                }
                else if (!Show24HourFormat)
                {
                    if (c == ' ')
                        m_selection.SetAndReplace(start, start + 1, c.ToString());
                    ShowAMPM();
                }

                ChangeAMPM(c);  // allow changing AM/PM (if it's being shown) by pressing A or P
            }

            else if (start == m_hourStart + 6)  // FIFTH DIGIT - first digit of seconds or AM/PM
            {
                if (ShowSeconds)
                {
                    if (IsValidSecondDigit(c, 0))
                    {
                        if (length > start)
                        {
                            int replace = (start < length && text[start] != ' ') ? 1 : 0;
                            m_selection.SetAndReplace(start, start + replace, c.ToString());
                        }
                        else
                            base.HandleKeyPress(sender, e);
                    }
                }

                ChangeAMPM(c);  // allow changing AM/PM (if it's being shown) by pressing A or P
            }

            else if (start == m_hourStart + 7)  // SIXTH DIGIT - second digit of seconds or AM/PM
            {
                if (ShowSeconds)
                {
                    if (IsValidSecondDigit(c, 1))
                    {
                        if (length > start)
                        {
                            int replace = (start < length && text[start] != ' ') ? 1 : 0;
                            m_selection.SetAndReplace(start, start + replace, c.ToString());
                        }
                        else
                            base.HandleKeyPress(sender, e);

                        // Show the AM/PM symbol if we're not in 24-hour format
                        ShowAMPM();
                    }
                }

                ChangeAMPM(c);  // allow changing AM/PM (if it's being shown) by pressing A or P
            }

            else if (start == m_hourStart + 8)  // FIRST SPACE (with seconds showing)
            {
                if (ShowSeconds && !Show24HourFormat)
                {
                    if (c == ' ')
                    {
                        m_selection.SetAndReplace(start, start + 1, c.ToString());
                        ShowAMPM();
                    }
                }

                ChangeAMPM(c);  // allow changing AM/PM (if it's being shown) by pressing A or P
            }

            else        // AM/PM
                ChangeAMPM(c);
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Изменяет значение текстового поля таким образом, чтобы оно находилось в пределах допустимого диапазона
        /// </summary>
        protected void AdjustWithinRange()
        {
            // Check if it's already within the range
            if (__mIsValid())
                return;

            // If it's empty, set it to the current time
            if (m_textBox.Text == "")
                m_textBox.Text = " ";
            else
                UpdateText();

            // Make it fall within the range
            DateTime date = (DateTime)Value;
            if (date < m_rangeMin)
                Value = m_rangeMin;
            else if (date > m_rangeMax)
                Value = m_rangeMax;
        }
        /// <summary>
        /// Добавляет символ AM/PM в текстовое поле.
        /// </summary>
        /// <remarks>
        /// Если символ AM/PM недопустим или не отображается в текстовом поле, это свойство вернет пустую строку
        /// </remarks>
        public string AMPM
        {
            get
            {
                string text = m_textBox.Text;
                int position = GetAMPMPosition(text);
                if (position > 0)
                    return text.Substring(position);
                return "";
            }
        }
        /// <summary>
        /// Изменяет символ AM/PM в зависимости от символа, введенного пользователем
        /// </summary>
        /// <param name="c">Символ, введенный пользователем, например, «a» или «p»</param>
        /// <returns>
        /// Если символ AM/PM изменен, возвращаемое значение равно true; в противном случае — false
        /// </returns>
        protected bool ChangeAMPM(char c)
        {
            if (Show24HourFormat)
                return false;

            string text = m_textBox.Text;
            int length = text.Length;

            int position = GetAMPMPosition(text);
            if (position == 0)
                return false;

            int start, end;
            m_selection.Get(out start, out end);

            char cUpper = Char.ToUpper(c);

            switch (cUpper)
            {
                case 'A':
                case 'P':
                    SetAMPM(cUpper == 'A');

                    if (cUpper == Char.ToUpper(m_am[0]) || cUpper == Char.ToUpper(m_pm[0]))
                    {
                        // Move the cursor right, if we're in front of the AM/PM symbols
                        if (start == position)
                            SendKeys.Send("{RIGHT}");

                        // Move the cursor right twice, if we're in front of the space in front of the AM/PM symbols
                        if (start + 1 == position)
                        {
                            SendKeys.Send("{RIGHT}");
                            SendKeys.Send("{RIGHT}");
                        }
                    }
                    return true;

                default:
                    // Handle entries after the first character of the AM/PM symbol -- allow the user to enter each character
                    if (start > position)
                    {
                        // Check if we're adding a character of the AM/PM symbol (after the first one)
                        if ((length == start && !IsValidAMPM(AMPM)) || (length == end && end != start))
                        {
                            string ampmToUse = Char.ToUpper(text[position]) == Char.ToUpper(m_am[0]) ? m_am : m_pm;
                            if (cUpper == Char.ToUpper(ampmToUse[start - position]))
                            {
                                m_selection.Replace(ampmToUse.Substring(start - position)); // set the rest of the AM/PM
                                m_selection.Set(start, start);  // Reset the selection so that the cursor can be moved
                                return ChangeAMPM(c); // move the cursor (below)
                            }
                        }

                        // Check if the AM/PM symbol is OK and we just need to move over one
                        if (length > start && end == start && cUpper == Char.ToUpper(text[start]))
                        {
                            SendKeys.Send("{RIGHT}");
                            return true;
                        }
                    }
                    break;
            }

            return false;
        }
        /// <summary>
        /// Преобразует час в 12-часовом формате и его обозначение AM/PM в 24-часовой эквивалент
        /// </summary>
        /// <param name="hour">Значение часа для перевода в 12-часовой формат (от 1 до 12)</param>
        /// <param name="ampm">Символ AM/PM обозначает, находится ли час в диапазоне от 0 до 11 или от 12 до 23</param>
        /// <returns>
        /// Возвращаемое значение — это час, преобразованный в 24-часовой формат (от 0 до 23)
        /// </returns>
        protected int ConvertTo24Hour(int hour, string ampm)
        {
            if (ampm == m_pm && hour >= 1 && hour <= 11)
                hour += 12;
            else if (ampm == m_am && hour == 12)
                hour = 0;
            return hour;
        }
        /// <summary>
        /// Преобразует час в 24-часовом формате в его 12-часовой эквивалент.
        /// </summary>
        /// <param name="hour">Значение часа для перевода в 24-часовой формат (от 0 до 23)</param>
        /// <param name="ampm">Возвращаемый символ AM/PM используется для обозначения того, находится ли час в диапазоне от 0 до 11 или от 12 до 23</param>
        /// <returns>
        /// Возвращаемое значение — это час, преобразованный в 12-часовой формат (от 1 до 12).
        /// </returns>
        protected int ConvertToAMPMHour(int hour, out string ampm)
        {
            ampm = m_am;

            if (hour >= 12)
            {
                hour -= 12;
                ampm = m_pm;
            }
            if (hour == 0)
                hour = 12;

            return hour;
        }
        /// <summary>
        /// Получает нулевую позицию значения AM/PM внутри текстового поля
        /// </summary>
        /// <returns>Возвращаемое значение — это начальная позиция AM/PM</returns>
        /// <remarks>
        /// Это зависит от того, отображаются секунды или нет
        /// </remarks>
        protected int GetAMPMStartPosition()
        {
            return m_hourStart + (ShowSeconds ? 9 : 6);
        }
        /// <summary>
        /// Получает символы, используемые для обозначения AM и PM.
        /// </summary>
        /// <param name="am">Символ, используемый для обозначения AM</param>
        /// <param name="pm">Символ, используемый для обозначения PM</param>
        public void GetAMPMSymbols(out string am, out string pm)
        {
            am = m_am;
            pm = m_pm;
        }
        /// <summary>
        /// Преобразует значения часа, минуты, секунды и AM/PM в строку в соответствии с заданным форматом
        /// </summary>
        /// <param name="hour">Стоимость часа</param>
        /// <param name="minute">минутное значение</param>
        /// <param name="second">второе значение</param>
        /// <param name="ampm">The AM/PM value, which may be empty if the hour is in 24-hour format</param>
        /// <returns>Возвращаемое значение представляет собой отформатированное значение времени</returns>
        public string GetFormattedTime(int hour, int minute, int second, string ampm)
        {
            if (Show24HourFormat)
            {
                // Handle switching from AM/PM to 24-hour format
                if (IsValidAMPM(ampm))
                    hour = ConvertTo24Hour(hour, ampm);
            }
            else
            {
                // Handle switching from 24-hour format to AM/PM
                if (!IsValidAMPM(ampm))
                    hour = ConvertToAMPMHour(hour, out ampm);
            }

            if (ShowSeconds)
            {
                if (Show24HourFormat)
                    return String.Format("{0,2:00}{1}{2,2:00}{3}{4,2:00}", hour, m_separator, minute, m_separator, second);
                return String.Format("{0,2:00}{1}{2,2:00}{3}{4,2:00} {5}", hour, m_separator, minute, m_separator, second, ampm);
            }

            if (Show24HourFormat)
                return String.Format("{0,2:00}{1}{2,2:00}", hour, m_separator, minute);
            return String.Format("{0,2:00}{1}{2,2:00} {3}", hour, m_separator, minute, ampm);
        }
        /// <summary>
        /// Получает начальную позицию часа в текстовом поле
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это начальная позиция часа
        /// </returns>
        protected int GetHourStartPosition()
        {
            return m_hourStart;
        }
        /// <summary>
        /// Извлекает максимальное значение за час
        /// </summary>
        /// <param name="force24HourFormat">Если это так, то максимальное значение равно 23, независимо от свойства <see cref="Show24HourFormat" />;
        /// в противном случае оно определяется свойством <see cref="Show24HourFormat" /></param>
        /// <returns>Возвращаемое значение — это максимальное значение за час (23 или 12)</returns>
        /// <remarks>
        /// Примечание: это значение не основано на <see cref="RangeMax" />
        /// </remarks>
        protected int GetMaxHour(bool force24HourFormat)
        {
            return (force24HourFormat || Show24HourFormat) ? 23 : 12;
        }
        /// <summary>
        /// Извлекает максимальное количество цифр, которое может принимать значение часа, в одной из двух позиций символа
        /// </summary>
        /// <param name="position">Положение цифры часа (0 или 1)</param>
        /// <returns>Возвращаемое значение — это максимально допустимое количество цифр</returns>
        protected char GetMaxHourDigit(int position)
        {
            Debug.Assert(position >= 0 && position <= 1);

            // First digit
            if (position == 0)
                return Show24HourFormat ? '2' : '1';

            // Second digit
            string text = m_textBox.Text;
            char firstDigit = (text.Length > GetHourStartPosition()) ? text[GetHourStartPosition()] : '0';
            Debug.Assert(firstDigit != 0);  // must have a valid first digit at this point

            // Use the first digit to determine the second digit's max
            if (firstDigit == '2')
                return '3';
            if (firstDigit == '1' && !Show24HourFormat)
                return '2';
            return '9';
        }
        /// <summary>
        /// Извлекает максимальное значение за минуту: 59
        /// </summary>
        /// <returns>Возвращаемое значение всегда равно 59.</returns>
        /// <remarks>Примечание: это значение не основано на <see cref="RangeMax" /></remarks>
        protected int GetMaxMinute()
        {
            return 59;
        }
        /// <summary>
        /// Извлекает максимальное количество цифр, которое может принимать значение минуты, в одной из двух позиций символа.
        /// </summary>
        /// <param name="position">Положение цифры минуты (0 или 1)</param>
        /// <returns>
        /// Возвращаемое значение — это максимально допустимое количество цифр
        /// </returns>
        protected char GetMaxMinuteDigit(int position)
        {
            Debug.Assert(position >= 0 && position <= 1);
            return (position == 0 ? '5' : '9');
        }
        /// <summary>
        /// Извлекает максимальное значение для второго значения: 59
        /// </summary>
        /// <returns>Возвращаемое значение всегда равно 59.</returns>
        /// <remarks>Примечание: это значение не основано на <see cref="RangeMax" /></remarks>
        protected int GetMaxSecond()
        {
            return 59;
        }
        /// <summary>
        /// Извлекает максимальное количество цифр, которое может принимать значение «второго» символа, в одной из двух его позиций
        /// </summary>
        /// <param name="position">Положение второй цифры (0 или 1)</param>
        /// <returns>
        /// Возвращаемое значение — это максимально допустимое количество цифр
        /// </returns>
        protected char GetMaxSecondDigit(int position)
        {
            Debug.Assert(position >= 0 && position <= 1);
            return (position == 0 ? '5' : '9');
        }
        /// <summary>
        /// Извлекает минимальное значение за час
        /// </summary>
        /// <param name="force24HourFormat">Если это так, то минимальное значение равно 0, независимо от свойства <see cref="Show24HourFormat" />;
        /// в противном случае оно основано на свойстве <see cref="Show24HourFormat" /></param>
        /// <returns>Возвращаемое значение — это минимальное значение за час (0 или 1)</returns>
        /// <remarks>Примечание: это значение не основано на <see cref="RangeMin" /></remarks>
        protected int GetMinHour(bool force24HourFormat)
        {
            return (force24HourFormat || Show24HourFormat) ? 0 : 1;
        }
        /// <summary>
        /// Извлекает минимальное количество цифр, которое может принимать значение часа, в одной из двух позиций символа
        /// </summary>
        /// <param name="position">Положение цифры часа (0 или 1)</param>
        /// <returns>Возвращаемое значение — это минимальное количество знаков после запятой, которое оно может содержать</returns>
        protected char GetMinHourDigit(int position)
        {
            Debug.Assert(position >= 0 && position <= 1);

            // First digit
            if (position == 0)
                return '0';

            // Second digit
            string text = m_textBox.Text;
            char firstDigit = (text.Length > GetHourStartPosition()) ? text[GetHourStartPosition()] : '0';
            Debug.Assert(firstDigit != 0);  // must have a valid first digit at this point

            // If the first digit is a 0 and we're not in 24-hour format, don't allow 0
            if (firstDigit == '0' && !Show24HourFormat)
                return '1';

            // For all other cases it's always 0
            return '0';
        }
        /// <summary>
        /// Получает минимальное значение за минуту: 0
        /// </summary>
        /// <returns>Возвращаемое значение всегда равно 0.</returns>
        /// <remarks>Примечание: это значение не основано на <see cref="RangeMin" /></remarks>
        protected int GetMinMinute()
        {
            return 0;
        }
        /// <summary>
        /// Извлекает минимальную цифру, которую может принимать значение минуты, в одной из двух позиций символа
        /// </summary>
        /// <param name="position">Положение цифры минуты (0 или 1)</param>
        /// <returns>
        /// Возвращаемое значение — это минимальное количество знаков после запятой, которое оно может содержать
        /// </returns>
        protected char GetMinMinuteDigit(int position)
        {
            Debug.Assert(position >= 0 && position <= 1);
            return '0';
        }
        /// <summary>
        /// Извлекает минимальное значение за секунду: 0
        /// </summary>
        /// <returns>Возвращаемое значение всегда равно 0.</returns>
        /// <remarks>Примечание: это значение не основано на <see cref="RangeMin" /></remarks>
        protected int GetMinSecond()
        {
            return 0;
        }
        /// <summary>
        /// Извлекает минимальную цифру, которую может принимать «второе» значение, в одной из двух его позиций символов
        /// </summary>
        /// <param name="position">The position of the digit of the second (0 or 1)</param>
        /// <returns>
        /// Возвращаемое значение — это минимальное количество знаков после запятой, которое оно может содержать
        /// </returns>
        protected char GetMinSecondDigit(int position)
        {
            Debug.Assert(position >= 0 && position <= 1);
            return '0';
        }
        /// <summary>
        /// Получает начальную позицию минуты в текстовом поле
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это начальная позиция минуты
        /// </returns>
        protected int GetMinuteStartPosition()
        {
            return m_hourStart + 3;
        }
        /// <summary>
        /// Получает нулевую позицию второго элемента внутри текстового поля
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это начальная позиция второго элемента
        /// </returns>
        protected int GetSecondStartPosition()
        {
            return m_hourStart + 6;
        }
        /// <summary>
        /// Извлекает символ AM/PM из текстового поля как допустимое значение
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это допустимое значение для символа AM/PM
        /// </returns>
        /// <remarks>
        /// Метод проверяет значение символа AM/PM в текстовом поле.
        /// Если это допустимый символ AM/PM, он возвращает его; в противном случае возвращает символ AM
        /// </remarks>
        protected string GetValidAMPM()
        {
            string ampm = AMPM;
            if (!IsValidAMPM(ampm))
                return m_am;

            return ampm;
        }
        /// <summary>
        /// Извлекает время из текстового поля как допустимое значение
        /// </summary>
        /// <param name="force24HourFormat">Если значение истинно, проверка выполняется на основе 24-часового формата, независимо от свойства <see cref="Show24HourFormat" />;
        /// в противном случае проверка выполняется на основе свойства <see cref="Show24HourFormat" /></param>
        /// <returns>
        /// Возвращаемое значение — это допустимое значение на данный час.
        /// </returns>
        /// <remarks>
        /// Метод проверяет значение часа в текстовом поле.
        /// Если это допустимое значение часа, он возвращает его.
        /// Если значение меньше, чем должно быть, возвращается минимальное значение.
        /// Если значение больше, чем должно быть, возвращается максимальное значение        
        /// </remarks>
        protected int GetValidHour(bool force24HourFormat)
        {
            int hour = Hour;

            // It it's outside the range, fix it
            if (hour < GetMinHour(force24HourFormat))
                hour = GetMinHour(force24HourFormat);
            else if (hour > GetMaxHour(force24HourFormat))
                hour = GetMaxHour(force24HourFormat);

            return hour;
        }
        /// <summary>
        /// Извлекает значение минут из текстового поля как допустимое значение.
        /// </summary>
        /// <returns>
        /// Возвращаемое значение — это допустимое значение на минуту.
        /// </returns>
        /// <remarks>
        /// Метод проверяет значение минут в текстовом поле.
        /// Если значение минут допустимо, он возвращает его.
        /// Если значение меньше, чем должно быть, возвращается минимальное значение.
        /// Если значение больше, чем должно быть, возвращается максимальное значение
        /// </remarks>
        protected int GetValidMinute()
        {
            int minute = Minute;

            // It it's outside the range, fix it
            if (minute < GetMinMinute())
                minute = GetMinMinute();
            else if (minute > GetMaxMinute())
                minute = GetMaxMinute();

            return minute;
        }
        /// <summary>
        /// Извлекает второе значение из текстового поля как допустимое
        /// </summary>
        /// <returns>
        /// Возвращаемое значение является допустимым значением для второго значения
        /// </returns>
        /// <remarks>
        /// Метод проверяет значение секунды в текстовом поле.
        /// Если значение секунды допустимо, метод возвращает его.
        /// Если значение меньше, чем должно быть, возвращается минимальное значение.
        /// Если значение больше, чем должно быть, возвращается максимальное значение
        /// </remarks>
        protected int GetValidSecond()
        {
            int second = Second;
            if (second < GetMinSecond())
                second = GetMinSecond();
            else if (second > GetMaxSecond())
                second = GetMaxSecond();

            return second;
        }
        /// <summary>
        /// Извлекает текст из текстового поля в корректном виде
        /// </summary>
        /// <returns>
        /// Если текст в текстовом поле допустим, он возвращается; в противном случае возвращается допустимая версия текста
        /// </returns>
        protected override string GetValidText()
        {
            string text = m_textBox.Text;

            // If it's empty or has a valid time, return it
            if (text == "")
                return text;

            if (IsValid(false))
                return GetFormattedTime(Hour, Minute, Second, AMPM);

            // If the hour, minute, and second are invalid, set it to the current time
            if (Hour < 0 && Minute < 0 && Second < 0)
            {
                DateTime dt = DateTime.Now;
                return GetFormattedTime(dt.Hour, dt.Minute, dt.Second, "");
            }

            // Otherwise retrieve the validated time
            return GetFormattedTime(GetValidHour(true), GetValidMinute(), GetValidSecond(), AMPM);
        }
        /// <summary>
        /// Получает или задает час в текстовом поле
        /// </summary>
        /// <remarks>
        /// Если время в текстовом поле указано неверно, это свойство вернет -1. Для установки этого свойства необходимо указать допустимое время — от 0 до 23
        /// </remarks>
        public int Hour
        {
            get
            {
                string text = m_textBox.Text;

                int startPos = GetHourStartPosition();

                // If there's already a separator, extract the value in front of it
                int sepPos = text.IndexOf(m_separator);
                if (sepPos > 0)
                {
                    startPos = sepPos - 2;
                    if (startPos < 0)
                        startPos = 0;
                }

                if (text.Length >= startPos + 1)
                    return ToInt(text.Substring(startPos, 2).Trim());

                return -1;
            }
            set
            {
                if (!IsValidHour(value, false))
                    throw new ArgumentOutOfRangeException();

                using (elmTypeSelection.Saver savedSelection = new elmTypeSelection.Saver(m_textBox))   // remember the current selection
                {
                    if (Hour >= 0)      // see if there's already an hour
                        m_selection.Set(GetHourStartPosition(), GetHourStartPosition() + 3);

                    // Convert it to AM/PM hour if necessary
                    string ampm = "";
                    if (!Show24HourFormat && value > 12)
                        value = ConvertToAMPMHour(value, out ampm);

                    m_selection.Replace(TwoDigits(value) + m_separator);    // set the hour

                    // Change the AM/PM if it's present
                    if (ampm != "" && IsValidAMPM(AMPM))
                        SetAMPM(ampm == m_am);
                }
            }
        }
        /// <summary>
        /// Проверяет, является ли время в текстовом поле допустимым и находится ли оно в пределах разрешенного диапазона
        /// </summary>
        /// <returns>
        /// Если значение допустимо и находится в пределах допустимого диапазона, возвращается значение true; в противном случае — false
        /// </returns>
        public override bool __mIsValid()
        {
            return IsValid(true);
        }
        /// <summary>
        /// Проверяет, является ли время в текстовом поле допустимым и, при необходимости, находится ли оно в допустимом диапазоне
        /// </summary>
        /// <param name="checkRangeAlso">Если это так, проверяется также, попадает ли время в допустимый диапазон</param>
        /// <returns>
        /// Если значение допустимо, возвращаемое значение равно true; в противном случае — false
        /// </returns>
        public bool IsValid(bool checkRangeAlso)
        {
            // Check that we have a valid hour and minute
            int hour = Hour;
            int minute = Minute;
            if (hour < 0 || minute < 0)
                return false;

            // Check that the seconds are valid if being shown
            int second = Second;
            bool showingSeconds = ShowSeconds;
            if (showingSeconds != (second >= 0))
                return false;

            // Check the AM/PM portion
            string ampm = AMPM;
            bool force24HourFormat = Show24HourFormat;
            if ((force24HourFormat && ampm != "") ||
                (!force24HourFormat && (ampm != m_am && ampm != m_pm)))
                return false;

            if (!force24HourFormat && ampm == m_pm)
            {
                hour += 12;
                if (hour == 24)
                    hour = 0;
            }
            if (!showingSeconds)
                second = m_rangeMin.Second; // avoids possible problem when checking range below

            // Check the range if desired
            if (checkRangeAlso)
                return IsWithinRange(new DateTime(1900, 1, 1, hour, minute, second));
            return true;
        }
        /// <summary>
        /// Проверяет, является ли строка допустимым символом AM или PM
        /// </summary>
        /// <param name="ampm">Значение для проверки</param>
        /// <returns>
        /// Если значение является допустимым символом AM или PM, возвращаемое значение равно true; в противном случае оно равно false
        /// </returns>
        protected bool IsValidAMPM(string ampm)
        {
            return (ampm == m_am || ampm == m_pm);
        }
        /// <summary>
        /// Checks if a value represents a valid hour
        /// </summary>
        /// <param name="hour">Значение для проверки</param>
        /// <param name="force24HourFormat">Если это так, диапазон основан на 24-часовом формате, независимо от свойства <see cref="Show24HourFormat" />;
        /// в противном случае он основан на свойстве <see cref="Show24HourFormat" /></param>
        /// <returns>
        /// Если значение является допустимым значением часа, возвращается значение true; в противном случае — false
        /// </returns>
        protected bool IsValidHour(int hour, bool force24HourFormat)
        {
            return (hour >= GetMinHour(force24HourFormat) && hour <= GetMaxHour(force24HourFormat));
        }
        /// <summary>
        /// Проверяет, является ли цифра допустимой для часа в одной из двух позиций символа
        /// </summary>
        /// <param name="c">Цифра для проверки</param>
        /// <param name="position">Положение цифры часа (0 или 1)</param>
        /// <returns>
        /// Если цифра действительна для данного часа (в указанной позиции), возвращаемое значение равно true; в противном случае — false.
        /// </returns>
        protected bool IsValidHourDigit(char c, int position)
        {
            return (c >= GetMinHourDigit(position) && c <= GetMaxHourDigit(position));
        }
        /// <summary>
        /// Проверяет, соответствует ли значение допустимой минуте
        /// </summary>
        /// <param name="minute">Значение для проверки</param>
        /// <returns>
        /// Если значение представляет собой допустимую минуту, возвращаемое значение равно true; в противном случае — false
        /// </returns>
        protected bool IsValidMinute(int minute)
        {
            return (minute >= GetMinMinute() && minute <= GetMaxMinute());
        }
        /// <summary>
        /// Проверяет, является ли цифра допустимой для данной минуты в одной из двух позиций символа
        /// </summary>
        /// <param name="c">The digit to check</param>
        /// <param name="position">Положение цифры минуты (0 или 1)</param>
        /// <returns>
        /// Если цифра действительна для данной минуты (в указанной позиции), возвращаемое значение равно true; в противном случае — false
        /// </returns>
        protected bool IsValidMinuteDigit(char c, int position)
        {
            return (c >= GetMinMinuteDigit(position) && c <= GetMaxMinuteDigit(position));
        }
        /// <summary>
        /// Проверяет, представляет ли значение допустимую секунду
        /// </summary>
        /// <param name="second">Значение для проверки</param>
        /// <returns>
        /// Если значение является допустимым вторым значением, возвращаемое значение равно true; в противном случае оно равно false
        /// </returns>
        protected bool IsValidSecond(int second)
        {
            return (second >= GetMinSecond() && second <= GetMaxSecond());
        }
        /// <summary>
        /// Проверяет, является ли цифра допустимой для "второй" позиции в одном из двух символов
        /// </summary>
        /// <param name="c">Цифра для проверки</param>
        /// <param name="position">Положение второй цифры (0 или 1)</param>
        /// <returns>
        /// Если цифра действительна для второй позиции (в данной позиции), возвращаемое значение равно true; в противном случае — false
        /// </returns>
        protected bool IsValidSecondDigit(char c, int position)
        {
            return (c >= GetMinSecondDigit(position) && c <= GetMaxSecondDigit(position));
        }
        /// <summary>
        /// Проверяет, находится ли значение времени в допустимом диапазоне
        /// </summary>
        /// <param name="dt">Значение времени, которое необходимо проверить</param>
        /// <returns>
        /// Если значение находится в допустимом диапазоне, возвращается значение true; в противном случае — false
        /// </returns>
        /// <remarks>
        /// Проверяется только временная часть; дата игнорируется
        /// </remarks>
        public bool IsWithinRange(DateTime dt)
        {
            DateTime time = new DateTime(1900, 1, 1, dt.Hour, dt.Minute, dt.Second);
            return (time >= m_rangeMin && time <= m_rangeMax);
        }
        /// <summary>
        /// Получает или задает значение в минутах в текстовом поле
        /// </summary>
        /// <remarks>
        /// Если значение в текстовом поле недопустимо, это свойство вернет -1.
        /// Это свойство должно быть установлено с допустимым значением в минутах — от 0 до 59.
        /// </remarks>
        public int Minute
        {
            get
            {
                string text = m_textBox.Text;
                int startPos = text.IndexOf(m_separator, m_hourStart) + 1;

                if (startPos > 0 && text.Length >= startPos + 2)
                    return ToInt(text.Substring(startPos, 2));

                return -1;
            }
            set
            {
                if (!IsValidMinute(value))
                    throw new ArgumentOutOfRangeException();

                using (elmTypeSelection.Saver savedSelection = new elmTypeSelection.Saver(m_textBox))   // remember the current selection
                {
                    if (Minute >= 0)        // see if there's already a minute
                        m_selection.Set(GetMinuteStartPosition(), GetMinuteStartPosition() + 2 + (ShowSeconds ? 1 : 0));

                    string text = TwoDigits(value);
                    if (ShowSeconds)
                        text += m_separator;

                    m_selection.Replace(text);  // set the minute

                    // Append the AM/PM if no seconds come after and it's not in 24-hour format
                    if (!ShowSeconds)
                        ShowAMPM();
                }
            }
        }
        /// <summary>
        /// Получает или задает максимально допустимое значение
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство установлено в DateTime(1900, 1, 1, 23, 59, 59),
        /// однако диапазон проверяется только тогда, когда элемент управления теряет фокус, если установлен один из флагов <see cref="ValidatingFlag" />
        /// </remarks>	
        public DateTime RangeMax
        {
            get
            {
                return m_rangeMax;
            }
            set
            {
                m_rangeMax = new DateTime(1900, 1, 1, value.Hour, value.Minute, value.Second);
            }
        }
        /// <summary>
        /// Получает или задает минимально допустимое значение
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство установлено в DateTime(1900, 1, 1, 0, 0, 0),
        /// однако диапазон проверяется только тогда, когда элемент управления теряет фокус, если установлен один из флагов <see cref="ValidatingFlag" />
        /// </remarks>	
        public DateTime RangeMin
        {
            get
            {
                return m_rangeMin;
            }
            set
            {
                m_rangeMin = new DateTime(1900, 1, 1, value.Hour, value.Minute, value.Second);
            }
        }
        /// <summary>
        /// Получает или задает значение второго элемента в текстовом поле
        /// </summary>
        /// <remarks>
        /// Если второе значение в текстовом поле недопустимо, это свойство вернет -1.
        /// Это свойство должно быть установлено с допустимым значением второго значения — от 0 до 59.
        /// </remarks>
        public int Second
        {
            get
            {
                string text = m_textBox.Text;
                int startPos = text.IndexOf(m_separator, m_hourStart);
                if (startPos > 0)
                {
                    startPos = text.IndexOf(m_separator, startPos + 1) + 1;
                    if (startPos == 0)
                        return -1;
                }

                if (text.Length >= startPos + 2 && Char.IsDigit(text[startPos]) && Char.IsDigit(text[startPos + 1]))
                    return ToInt(text.Substring(startPos, 2));

                return -1;
            }
            set
            {
                if (!IsValidSecond(value))
                    throw new ArgumentOutOfRangeException();

                if (!ShowSeconds)
                    return;

                using (elmTypeSelection.Saver savedSelection = new elmTypeSelection.Saver(m_textBox))   // remember the current selection
                {
                    if (Second >= 0)        // see if there's already a second
                        m_selection.Set(GetSecondStartPosition(), GetSecondStartPosition() + 2);

                    m_selection.Replace(TwoDigits(value));  // set the second

                    // Append the AM/PM if it's not in 24-hour format
                    ShowAMPM();
                }
            }
        }
        /// <summary>
        /// Получает или задает символ, используемый для разделения значений часа, минуты и секунды времени
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство устанавливается в соответствии с настройками системы пользователя. Если это свойство изменяется, автоматически вызывается метод `<see cref="elmTypeBehavior.UpdateText" />`
        /// </remarks>
        public char Separator
        {
            get
            {
                return m_separator;
            }
            set
            {
                if (m_separator == value)
                    return;

                Debug.Assert(value != 0);
                Debug.Assert(!Char.IsDigit(value));

                m_separator = value;
                UpdateText();
            }
        }
        /// <summary>
        /// Определяет, следует ли отображать час в 24-часовом формате.
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство устанавливается в соответствии с системой пользователя.
        /// Если установлен 12-часовой формат, также отображаются символы AM/PM; в противном случае они не отображаются.
        /// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />        /// </remarks>
        public bool Show24HourFormat
        {
            get
            {
                return HasFlag((int)Flag.TwentyFourHourFormat);
            }
            set
            {
                ModifyFlags((int)Flag.TwentyFourHourFormat, value);
            }
        }
        /// <summary>
        /// Определяет, следует ли отображать секунды (после минут)
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство установлено в значение false, поэтому секунды не отображаются.
        /// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />        /// </remarks>
        public bool ShowSeconds
        {
            get
            {
                return HasFlag((int)Flag.WithSeconds);
            }
            set
            {
                ModifyFlags((int)Flag.WithSeconds, value);
            }
        }
        /// <summary>
        /// Устанавливает символ AM или PM, если формат времени не 24-часовой
        /// </summary>
        /// <param name="am">Если значение истинно, устанавливается символ AM; в противном случае устанавливается символ PM</param>
        /// <seealso cref="AMPM" />
        public void SetAMPM(bool am)
        {
            if (Show24HourFormat)
                return;

            using (elmTypeSelection.Saver savedSelection = new elmTypeSelection.Saver(m_textBox))   // remember the current selection
            {
                m_selection.Set(GetAMPMStartPosition() - 1, GetAMPMStartPosition() + m_ampmLength);
                m_selection.Replace(" " + (am ? m_am : m_pm));  // set the AM/PM
            }
        }
        /// <summary>
        /// Задает символы, используемые для обозначения AM и PM
        /// </summary>
        /// <param name="am">Символ, используемый для обозначения AM</param>
        /// <param name="pm">Символ, используемый для обозначения PM</param>
        /// <remarks>
        /// По умолчанию символы устанавливаются в соответствии с системой пользователя.
        /// Этот метод позволяет изменять их, однако они должны быть одинаковой длины.
        /// Если символы изменены, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />
        /// </remarks>
        public void SetAMPMSymbols(string am, string pm)
        {
            if (m_am == am && m_pm == pm)
                return;

            // Make sure they're the same length
            if (am.Length != pm.Length)
                throw new ArgumentException("The length of the AM and PM symbols must be identical.");

            m_am = am;
            m_pm = pm;

            if (m_am == "")
                m_am = "AM";
            if (m_pm == "")
                m_pm = "PM";

            m_ampmLength = m_am.Length;
            UpdateText();
        }
        /// <summary>
        /// Устанавливает часы и минуты в текстовом поле
        /// </summary>
        /// <param name="hour">Время установки, от 0 до 23</param>
        /// <param name="minute">Минута до начала, от 0 до 59</param>
        /// <remarks>
        /// Это удобный метод для установки каждого значения по отдельности с помощью одного метода.
        /// Объект <see cref="DateTime" /> создается с использованием этих параметров, поэтому они должны быть допустимыми.
        /// Если в текстовом поле отображается секунда, она устанавливается равной 0
        /// </remarks>
        public void SetTime(int hour, int minute)
        {
            SetTime(hour, minute, 0);
        }
        /// <summary>
        /// Устанавливает часы, минуты и секунды в текстовом поле.
        /// </summary>
        /// <param name="hour">Время установки, от 0 до 23</param>
        /// <param name="minute">Минута до начала, от 0 до 59</param>
        /// <param name="second">Минута до начала отсчета, от 0 до 59. Секунда до начала отсчета, от 0 до 59.</param>
        /// <remarks>
        /// Это удобный способ установить каждое значение по отдельности с помощью одного метода.
        /// Объект <see cref="DateTime" /> создается с использованием этих параметров, поэтому они должны быть допустимыми.
        /// </remarks>
        public void SetTime(int hour, int minute, int second)
        {
            Value = new DateTime(1900, 1, 1, hour, minute, second);
        }
        /// <summary>
        /// Определяет позицию символа AM/PM в заданном тексте, начиная с нуля
        /// </summary>
        /// <param name="text">Текст для анализа и определения положения символа AM/PM.</param>
        /// <returns>
        /// Возвращаемое значение — это нулевая позиция символа AM/PM
        /// </returns>
        private int GetAMPMPosition(string text)
        {
            int position = text.IndexOf(' ' + m_am);
            return ((position < 0) ? text.IndexOf(' ' + m_pm) : position) + 1;
        }
        /// <summary>
        /// Отображает символ AM, если время отображается не в 24-часовом формате и еще не отображается
        /// </summary>
        protected void ShowAMPM()
        {
            if (!Show24HourFormat && !IsValidAMPM(AMPM))
                SetAMPM(true);
        }
        /// <summary>
        /// Преобразует целое число в двухзначную строку (00–99).
        /// </summary>
        /// <param name="value">Число для конвертации</param>
        /// <returns>Возвращаемое значение представляет собой отформатированную строку</returns>
        /// <remarks>
        /// Это удобный способ форматирования двузначных значений, таких как час и минута
        /// </remarks>
        protected static string TwoDigits(int value)
        {
            return String.Format("{0,2:00}", value);
        }
        /// <summary>
        /// Получает или задает час, минуту и ​​секунду в текстовом поле с помощью объекта <see cref="DateTime" />
        /// </summary>
        /// <remarks>
        /// Это свойство получает и устанавливает значение <see cref="DateTime" />, заключенное в блок <c>object</c>.
        /// Это обеспечивает гибкость, так что если текстовое поле не содержит допустимого времени, возвращается <c>null</c>,
        /// вместо того, чтобы беспокоиться о возникновении исключения.
        /// </remarks>
        /// <example>
        ///   object obj = txtTime.Behavior.Value;
        ///   if (obj != null)
        ///   {
        ///     DateTime dtm = (DateTime)obj;
        ///     ...
        ///   } 
        /// </example>
        public object Value
        {
            get
            {
                try
                {
                    if (Show24HourFormat)
                        return new DateTime(1900, 1, 1, Hour, Minute, GetValidSecond());
                    return new DateTime(1900, 1, 1, ConvertTo24Hour(Hour, AMPM), Minute, GetValidSecond());
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                DateTime dt = (DateTime)value;
                m_textBox.Text = GetFormattedTime(dt.Hour, dt.Minute, dt.Second, "");
            }
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПЕРЕЧИСЛЕНИЕ

        /// <summary>
        /// Внутренние значения, которые добавляются/удаляются из свойства <see cref="elmTypeBehavior.Flags" /> другими свойствами этого класса
        /// </summary>
        [Flags]
        protected enum Flag
        {
            /// <summary> 
            /// Время отображается в 24-часовом формате (от 00 до 23)
            /// </summary>
            TwentyFourHourFormat = 0x00020000,

            /// <summary>
            /// Также отображаются секунды
            /// </summary>
            WithSeconds = 0x00040000,
        };

        #endregion ПЕРЕЧИСЛЕНИЕ

        #region = ПОЛЯ

        private DateTime m_rangeMin = new DateTime(1900, 1, 1, 0, 0, 0);
        private DateTime m_rangeMax = new DateTime(1900, 1, 1, 23, 59, 59);
        private char m_separator = ':';
        private string m_am = "AM";
        private string m_pm = "PM";
        private int m_ampmLength = 2;
        /// <summary>
        /// Начальная нулевая отметка времени в текстовом поле
        /// </summary>
        /// <remarks>
        ///По умолчанию это значение равно 0, однако его можно изменить, чтобы разрешить добавление другого значения перед временем, например, даты
        /// </remarks>
        protected int m_hourStart = 0;

        #endregion ПОЛЯ

        /// <summary>
        /// Получает сообщение об ошибке, используемое для уведомления пользователя о необходимости ввести допустимое значение времени в пределах разрешенного диапазона
        /// </summary>
        public override string ErrorMessage
		{
			get
			{
				return "Please specify a time between " + GetFormattedTime(m_rangeMin.Hour, m_rangeMin.Minute, m_rangeMin.Second, "") + " and " + GetFormattedTime(m_rangeMax.Hour, m_rangeMax.Minute, m_rangeMax.Second, "") + ".";
			}
		}
	}

    /// <summary>
    /// Класс поведения, обработка вводимых значений даты и времени</summary>
    /// <remarks>
    /// Это поведение предназначено для того, чтобы пользователь мог быстро и точно вводить дату и время.
    /// По мере ввода цифр разделители заполняются автоматически. Пользователь может удалять только символы справа от введенного значения.
    /// Это помогает сохранить правильное форматирование значения.
    /// Пользователь также может использовать клавиши со стрелками вверх/вниз для увеличения/уменьшения месяца, дня, года, часа, минуты или секунды,
    /// в зависимости от положения курсора
    /// </remarks>
    public class DateTimeBehavior : TimeBehavior
	{
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Инициализирует новый экземпляр класса DateTimeBehavior, копируя его из другого объекта DateTimeBehavior
        /// </summary>
        /// <param name="behavior">Объект DateTimeBehavior, который необходимо скопировать (а затем удалить). Он не должен быть равен null</param>
        /// <remarks>
        /// После копирования объекта behavior.TextBox вызывается метод Dispose для параметра behavior
        /// </remarks>
        public DateTimeBehavior(DateTimeBehavior behavior) : base(behavior)
        {
            m_dateBehavior = new DateBehavior(m_textBox, false);  // does not add the event handlers
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса DateTimeBehavior, связывая его с объектом, производным от TextBoxBase
        /// </summary>
        /// <param name="textBox">Объект TextBoxBase, который необходимо связать с этим поведением. Он не должен быть равен null</param>
        /// <remarks>
        /// Этот конструктор получает многие свойства из системы пользователя
        /// </remarks>
        public DateTimeBehavior(TextBoxBase textBox) : base(textBox)
        {
            m_dateBehavior = new DateBehavior(textBox, false);  // does not add the event handlers
            m_flags |= m_dateBehavior.Flags;
            m_hourStart = 11;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Получает или задает день недели в текстовом поле
        /// </summary>
        /// <remarks>
        /// Если день не отображается или недействителен в текстовом поле, это свойство вернет 0.
        /// Это свойство должно быть установлено с днем, который находится в допустимом диапазоне
        /// </remarks>
        public int Day
        {
            get
            {
                if (HasFlag((int)Flag.TimeOnly))
                    return 0;
                return m_dateBehavior.Day;
            }
            set
            {
                if (!HasFlag((int)Flag.TimeOnly))
                    m_dateBehavior.Day = value;
            }
        }
        /// <summary>
        /// Проверяет, является ли дата и/или время в текстовом поле допустимыми и находятся ли они в пределах разрешенного диапазона
        /// </summary>
        /// <returns>Если значение допустимо и находится в пределах допустимого диапазона, возвращается значение true; в противном случае — false.</returns>
        public override bool __mIsValid()
        {
            if (HasFlag((int)Flag.DateOnly))
                return m_dateBehavior.__mIsValid();
            if (HasFlag((int)Flag.TimeOnly))
                return base.__mIsValid();
            return (m_dateBehavior.__mIsValid() && base.__mIsValid());
        }
        /// <summary>
        /// Проверяет, находится ли значение даты и времени в допустимом диапазоне.
        /// </summary>
        /// <param name="dt">Значение даты и времени для проверки</param>
        /// <returns>Если значение находится в допустимом диапазоне, возвращается значение true; в противном случае — false</returns>
        public new bool IsWithinRange(DateTime dt)
        {
            if (HasFlag((int)Flag.DateOnly))
                return m_dateBehavior.IsWithinRange(dt);
            if (HasFlag((int)Flag.TimeOnly))
                return base.IsWithinRange(dt);
            return (m_dateBehavior.IsWithinRange(dt) && base.IsWithinRange(dt));
        }
        /// <summary>
        /// Извлекает текст из текстового поля в корректном виде
        /// </summary>
        /// <returns>
        /// Если текст в текстовом поле допустим, он возвращается; в противном случае возвращается допустимая версия текста
        /// </returns>
        protected override string GetValidText()
        {
            // Check if we're showing the date only
            string date = m_dateBehavior.GetValidTextForDateTime();
            if (HasFlag((int)Flag.DateOnly))
                return date;

            // Check if we're showing the time only
            string time = base.GetValidText();
            if (HasFlag((int)Flag.TimeOnly))
                return time;

            string space = (date != "" && time != "" ? " " : "");
            return date + space + time;
        }
        /// <summary>
        /// Устанавливает месяц, день и год в текстовом поле
        /// </summary>
        /// <param name="year">Год для начала работы</param>
        /// <param name="month">Месяц для начала работы</param>
        /// <param name="day">День начала</param>
        /// <remarks>
        /// Это удобный метод для установки каждого значения по отдельности с помощью одного метода.
        /// Объект <see cref="DateTime" /> создается с использованием этих параметров, поэтому они должны быть допустимыми
        /// </remarks>
        public void SetDate(int year, int month, int day)
        {
            if (HasFlag((int)Flag.DateOnly) || !HasFlag((int)Flag.TimeOnly))
                m_dateBehavior.SetDate(year, month, day);
        }
        /// <summary>
        /// Устанавливает месяц, день, год, час, минуту и ​​секунду в текстовом поле
        /// </summary>
        /// <param name="year">Год для начала работы</param>
        /// <param name="month">Месяц для начала работы</param>
        /// <param name="day">День начала</param>
        /// <param name="hour">Время установки, от 0 до 23</param>
        /// <param name="minute">Минута до начала, от 0 до 59</param>
        /// <remarks>
        /// Это удобный метод для установки каждого значения по отдельности с помощью одного метода.
        /// Объект <see cref="DateTime" /> создается с использованием этих параметров, поэтому они должны быть допустимыми.
        /// Если в текстовом поле отображается секунда, она устанавливается равной 0
        /// </remarks>
        public void SetDateTime(int year, int month, int day, int hour, int minute)
        {
            SetDateTime(year, month, day, hour, minute, 0);
        }
        /// <summary>
        /// Устанавливает месяц, день, год, час, минуту и ​​секунду в текстовом поле.
        /// </summary>
        /// <param name="year">Год для начала работы</param>
        /// <param name="month">Месяц для начала работы</param>
        /// <param name="day">День начала</param>
        /// <param name="hour">Время установки, от 0 до 23</param>
        /// <param name="minute">Минута до начала, от 0 до 59</param>
        /// <param name="second">Минута до начала, от 0 до 59</param>
        /// <remarks>
        /// Это удобный метод для установки каждого значения по отдельности с помощью одного метода.
        /// Объект <see cref="DateTime" /> создается с использованием этих параметров, поэтому они должны быть допустимыми
        /// </remarks>
        public void SetDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            if (HasFlag((int)Flag.DateOnly))
                m_dateBehavior.SetDate(year, month, day);
            else if (HasFlag((int)Flag.TimeOnly))
                SetTime(hour, minute, second);
            else
            {
                Debug.Assert(m_dateBehavior.IsWithinRange(new DateTime(year, month, day)));
                m_textBox.Text = m_dateBehavior.GetFormattedDate(year, month, day) + ' ' + GetFormattedTime(hour, minute, second, "");
            }
        }
        /// <summary>
        /// Устанавливает часы и минуты в текстовом поле
        /// </summary>
        /// <param name="hour">Время установки, от 0 до 23</param>
        /// <param name="minute">Минута до начала, от 0 до 59</param>
        /// <remarks>
        /// Это удобный метод для установки каждого значения по отдельности с помощью одного метода.
        /// Объект <see cref="DateTime" /> создается с использованием этих параметров, поэтому они должны быть допустимыми.
        /// Если в текстовом поле отображается секунда, она устанавливается равной 0.        /// </remarks>
        public new void SetTime(int hour, int minute)
        {
            SetTime(hour, minute, 0);
        }
        /// <summary>
        /// Устанавливает часы, минуты и секунды в текстовом поле
        /// </summary>
        /// <param name="hour">Время установки, от 0 до 23</param>
        /// <param name="minute">Минута до начала, от 0 до 59</param>
        /// <param name="second">Второй, который нужно установить, в диапазоне от 0 до 59.</param>
        /// <remarks>
        /// Это удобный метод для установки каждого значения по отдельности с помощью одного метода.
        /// Объект <see cref="DateTime" /> создается с использованием этих параметров, поэтому они должны быть допустимыми
        /// </remarks>
        public new void SetTime(int hour, int minute, int second)
        {
            if (!HasFlag((int)Flag.DateOnly) && HasFlag((int)Flag.TimeOnly))
                base.SetTime(hour, minute, second);
        }

        #endregion Процедуры

        #region - Поведение

        /// <summary>
        /// Обрабатывает нажатия клавиш внутри текстового поля
        /// </summary>
        /// <param name="sender">Объект, отправивший событие</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>Этот метод переопределен в классе Behavior и обрабатывает событие KeyDown текстового поля</remarks>
        protected override void HandleKeyDown(object sender, KeyEventArgs e)
        {
            TraceLine("DateTimeBehavior.HandleKeyDown " + e.KeyCode);

            // Check if we're showing the time only
            if (HasFlag((int)Flag.TimeOnly))
            {
                base.HandleKeyDown(sender, e);
                return;
            }

            if (e.KeyCode != Keys.Delete)
                m_dateBehavior.HandleKeyEvent(sender, e);

            if ((e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Delete) && !HasFlag((int)Flag.DateOnly))
                base.HandleKeyDown(sender, e);
        }
        /// <summary>
        /// Обрабатывает нажатия клавиш внутри текстового поля.
        /// </summary>
        /// <param name="sender">Объект, отправивший событие.</param>
        /// <param name="e">Данные о событии</param>
        /// <remarks>Этот метод переопределен в классе Behavior и обрабатывает событие KeyPress текстового поля</remarks>
        protected override void HandleKeyPress(object sender, KeyPressEventArgs e)
        {
            TraceLine("DateTimeBehavior.HandleKeyPress " + e.KeyChar);

            // Check to see if it's read only
            if (m_textBox.ReadOnly)
                return;

            m_noTextChanged = true;

            // Check if we're showing the date or the time only
            if (HasFlag((int)Flag.DateOnly))
            {
                m_dateBehavior.HandleKeyEvent(sender, e);
                return;
            }
            if (HasFlag((int)Flag.TimeOnly))
            {
                base.HandleKeyPress(sender, e);
                return;
            }

            char c = e.KeyChar;
            e.Handled = true;

            int start, end;
            m_selection.Get(out start, out end);

            string text = m_textBox.Text;
            int length = text.Length;

            if (start >= 0 && start <= 9)
            {
                m_dateBehavior.HandleKeyEvent(sender, e);
                ChangeAMPM(c);  // allow changing AM/PM (if it's being shown) by pressing A or P
            }
            else if (start == 10)
            {
                m_dateBehavior.HandleKeyEvent(sender, e);

                int space = 0;
                if (c == ' ')
                    space = 1;
                else
                    space = (base.IsValidHourDigit(c, 0) || (base.IsValidHourDigit(c, 1) && length <= 11) ? 2 : 0);

                // If we need the space, enter it
                if (space != 0)
                    m_selection.SetAndReplace(start, start + 1, " ");

                // If the space is to be preceded by a valid digit, "type" it in.
                if (space == 2)
                    SendKeys.Send(c.ToString());
                else
                    base.ChangeAMPM(c); // allow changing AM/PM (if it's being shown) by pressing A or P
            }
            else
                base.HandleKeyPress(sender, e);
        }

        #endregion Поведение

        #endregion МЕТОДЫ

        #region = ПЕРЕЧИСЛЕНИЯ

        /// <summary>
        /// Внутренние значения, которые добавляются/удаляются из свойства <see cref="elmTypeBehavior.Flags" /> другими свойствами этого класса
        /// </summary>
        [Flags]
        protected new enum Flag
        {
            /// <summary> 
            /// Приводит к тому, что этот объект ведёт себя аналогично объекту типа Date, где отображается только часть, содержащая дату
            /// </summary>
            DateOnly = 0x00100000,
            /// <summary>
            /// Приводит к тому, что этот объект ведёт себя подобно объекту Time, отображая только временную часть
            /// </summary>
            TimeOnly = 0x00200000,
            /// <summary>
            /// День недели отображается перед месяцем
            /// </summary>
            DayBeforeMonth = 0x00010000,
            /// <summary>
            /// Время отображается в 24-часовом формате (от 00 до 23)
            /// </summary>
            TwentyFourHourFormat = 0x00020000,
            /// <summary> 
            /// Также отображаются секунды
            /// </summary>
            WithSeconds = 0x00040000
        };

        #endregion ПЕРЕЧИСЛЕНИЯ

        #region = ПОЛЯ

        private DateBehavior m_dateBehavior;

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Получает или задает символ, используемый для разделения значений месяца, дня и года в дате
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство устанавливается в соответствии с системой пользователя.
        /// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />
        /// </remarks>
        public char DateSeparator
        {
            get
            {
                return m_dateBehavior.__fSeparator_;
            }
            set
            {
                m_dateBehavior.__fSeparator_ = value;
            }
        }
        /// <summary>
        /// Получает сообщение об ошибке, уведомляющее пользователя о необходимости ввести допустимые значения даты и времени в пределах разрешенного диапазона
        /// </summary>
        public override string ErrorMessage
        {
            get
            {
                // Get the message depending on what we're showing
                if (HasFlag((int)Flag.DateOnly))
                    return m_dateBehavior.ErrorMessage;
                else if (HasFlag((int)Flag.TimeOnly))
                    return base.ErrorMessage;
                else
                {
                    string minDateTime =
                        m_dateBehavior.GetFormattedDate(m_dateBehavior.__fValueMin_.Year, m_dateBehavior.__fValueMin_.Month, m_dateBehavior.__fValueMin_.Day) + ' ' +
                        base.GetFormattedTime(base.RangeMin.Hour, base.RangeMin.Minute, base.RangeMin.Second, "");
                    string maxDateTime =
                        m_dateBehavior.GetFormattedDate(m_dateBehavior.__fValueMax_.Year, m_dateBehavior.__fValueMax_.Month, m_dateBehavior.__fValueMax_.Day) + ' ' +
                        base.GetFormattedTime(base.RangeMax.Hour, base.RangeMax.Minute, base.RangeMax.Second, "");

                    return "Please specify a date and time between " + minDateTime + " and " + maxDateTime + '.';
                }
            }
        }
        /// <summary>
        /// Получает или задает флаги, связанные с этим объектом
        /// </summary>
        /// <remarks>
        /// Это свойство ведет себя аналогично свойству в базовом классе <see cref="elmType Behavior.Flags"></see>, 
        /// но переопределено для корректной установки начальной позиции часа, если        
        /// <see cref="Flag.DateOnly" /> or <see cref="Flag.TimeOnly" /> flags are turned on/off
        /// </remarks>
        public override int Flags
        {
            get
            {
                return m_flags;
            }
            set
            {
                if (m_flags == value)
                    return;

                m_flags = value;
                m_hourStart = ((value & (int)Flag.TimeOnly) != 0) ? 0 : 11;

                m_dateBehavior.Flags = value;  // should call UpdateText
            }
        }
        /// <summary>
        /// Получает или задает месяц
        /// </summary>
        public int __fMonth_
        {
            get
            {
                if (HasFlag((int)Flag.TimeOnly))
                    return 0;
                return m_dateBehavior.__fMonth_;
            }
            set
            {
                if (!HasFlag((int)Flag.TimeOnly))
                    m_dateBehavior.__fMonth_ = value;
            }
        }
        /// <summary>
        /// Получает или задает максимально допустимое значение
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство установлено на <see cref="DateTime.MaxValue" />.
        /// Диапазон активно проверяется по мере ввода пользователем даты, но время проверяется только
        /// когда элемент управления теряет фокус, если установлен один из флагов <see cref="ValidatingFlag" />
        /// </remarks>	
        public new DateTime RangeMax
        {
            get
            {
                if (HasFlag((int)Flag.DateOnly))
                    return m_dateBehavior.__fValueMax_;
                if (HasFlag((int)Flag.TimeOnly))
                    return base.RangeMax;

                DateTime rangeMax = base.RangeMax;
                return new DateTime(m_dateBehavior.__fValueMax_.Year, m_dateBehavior.__fValueMax_.Month, m_dateBehavior.__fValueMax_.Day, rangeMax.Hour, rangeMax.Minute, rangeMax.Second);
            }
            set
            {
                base.RangeMax = value;
                if (HasFlag((int)Flag.DateOnly) || !HasFlag((int)Flag.TimeOnly))
                    m_dateBehavior.__fValueMax_ = value;        // updates the control
            }
        }
        /// <summary>
        /// Получает или задает минимально допустимое значение
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство установлено в DateTime(1900, 1, 1, 0, 0, 0).
        /// Диапазон активно проверяется по мере ввода пользователем даты, но время проверяется только
        /// когда элемент управления теряет фокус, если установлен один из флагов <see cref="ValidatingFlag" />
        /// </remarks>	
        public new DateTime RangeMin
        {
            get
            {
                if (HasFlag((int)Flag.DateOnly))
                    return m_dateBehavior.__fValueMin_;
                if (HasFlag((int)Flag.TimeOnly))
                    return base.RangeMin;

                DateTime rangeMin = base.RangeMin;
                return new DateTime(m_dateBehavior.__fValueMin_.Year, m_dateBehavior.__fValueMin_.Month, m_dateBehavior.__fValueMin_.Day, rangeMin.Hour, rangeMin.Minute, rangeMin.Second);
            }
            set
            {
                base.RangeMin = value;
                if (HasFlag((int)Flag.DateOnly) || !HasFlag((int)Flag.TimeOnly))
                    m_dateBehavior.__fValueMin_ = value;        // updates the control
            }
        }
        /// <summary>
        /// Получает символ, используемый для разделения значений даты и времени
        /// </summary>
        /// <remarks>
        /// Если отображается только дата, это свойство извлекает значение из файла <see cref="DateSeparator" />
        /// Если отображается только время, это свойство извлекает значение из файла <see cref="TimeSeparator" />
        /// Если отображаются и дата, и время, это свойство извлекает пробел
        /// </remarks>
        private new char Separator
        {
            get
            {
                if (HasFlag((int)Flag.DateOnly))
                    return m_dateBehavior.__fSeparator_;
                if (HasFlag((int)Flag.TimeOnly))
                    return base.Separator;
                return ' ';
            }
        }
        /// <summary>
        /// Определяет, следует ли отображать день недели перед месяцем или после него
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство устанавливается в соответствии с системой пользователя
        /// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" />
        /// </remarks>
        public bool ShowDayBeforeMonth
        {
            get
            {
                return HasFlag((int)Flag.DayBeforeMonth);
            }
            set
            {
                if (!HasFlag((int)Flag.TimeOnly))
                    ModifyFlags((int)Flag.DayBeforeMonth, value);
            }
        }
        /// <summary>
        /// Получает или задает символ, используемый для разделения значений часа, минуты и секунды времени
        /// </summary>
        /// <remarks>
        /// По умолчанию это свойство устанавливается в соответствии с системой пользователя.
        /// Если это свойство изменено, автоматически вызывается метод <see cref="elmTypeBehavior.UpdateText" /> 
        /// </remarks>
        public char TimeSeparator
        {
            get
            {
                return base.Separator;
            }
            set
            {
                base.Separator = value;
            }
        }
        /// <summary>
        /// Получает или задает месяц, день, год, час, минуту и ​​секунду в текстовом поле, используя объект <see cref="DateTime" />
        /// </summary>
        /// <remarks>
        /// Это свойство получает и устанавливает значение <see cref="DateTime" />, заключенное в <c>объект</c>.
        /// Это обеспечивает гибкость: если текстовое поле не содержит допустимых даты и времени, возвращается <c>null</c>,
        /// вместо того, чтобы беспокоиться о возникновении исключения. </remarks>        
        /// <example>
        ///   object obj = txtDateTime.Behavior.Value;
        ///   if (obj != null)
        ///   {
        ///     DateTime dtm = (DateTime)obj;
        ///     ...
        ///   } 
        /// </example>
        public new object Value
        {
            get
            {
                try
                {
                    if (HasFlag((int)Flag.DateOnly))
                        return m_dateBehavior.__fValue_;
                    if (HasFlag((int)Flag.TimeOnly))
                        return base.Value;
                    return new DateTime(Year, __fMonth_, Day, Hour, Minute, Second);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                DateTime dt = (DateTime)value;
                SetDateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
            }
        }
        /// <summary>
        /// Получает или задает год в текстовом поле
        /// </summary>
        /// <remarks>
        /// Если год не отображается или недействителен в текстовом поле, это свойство вернет 0.
        /// Это свойство должно быть установлено с годом, который находится в допустимом диапазоне
        /// </remarks>
        public int Year
        {
            get
            {
                if (HasFlag((int)Flag.TimeOnly))
                    return 0;
                return m_dateBehavior.Year;
            }
            set
            {
                if (!HasFlag((int)Flag.TimeOnly))
                    m_dateBehavior.Year = value;
            }
        }

        #endregion СВОЙСТВА
    }
}
