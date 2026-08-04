using nlApplication;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmInput.cs
    /// </summary>
    /// <remarks>Класс-Поле ввода</remarks>
  	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 14-09</version> // Дата-время последней корректировки
    public class elmInput : elmComponentSplitter
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

            Panel1.Controls.Add(_cMark);
            Panel1.Controls.Add(_cLabelCaption);
            Panel2.Controls.Add(_cLabelValue);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            BorderStyle = BorderStyle.None;
            IsSplitterFixed = true;
            FixedPanel = FixedPanel.Panel1;
            Orientation = Orientation.Vertical;
            Size = new Size(300, 25);
            SplitterDistance = 200;
            TabStop = false;

            // _cMark
            {
                _cMark.Location = new Point(elmInterface.__fIntervalHorizontal, elmInterface.__fIntervalVertical);
                _cMark.TabStop = false;
            }
            // _cLabelCaption
            {
                _cLabelCaption.Location = new Point(_cMark.Left
                    + _cMark.Width
                    + elmInterface.__fIntervalHorizontal
                    , elmInterface.__fIntervalVertical);
                _cLabelCaption.__fCaption_ = "НАДПИСЬ";
                _cLabelCaption.__eClickLeft += mCaption_ClickLeft;
            }
            // _cLabelValue
            {
                _cLabelValue.Location = new Point(0, elmInterface.__fIntervalVertical);
                _cLabelValue.Font = elmApplication.__oInterface.__mFont(FONTS.Data);
                _cLabelValue.ForeColor = elmApplication.__oInterface.__mColor(COLORS.Data);
                _cLabelValue.__fCaption_ = "нет данных";
                _cLabelValue.Visible = false;
            }

            #endregion Настройка компонентов

            ResumeLayout();

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();

            if (Parent != null)
                Width = Parent.ClientSize.Width - elmInterface.__fIntervalHorizontal * 2;
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
        /// Выполняется при клике левой кнопки мыши по 'Надписи-заголовку'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mCaption_ClickLeft(object sender, EventArgs e)
        {
            if (__eLabelCaption_ClickLeft != null)
                __eLabelCaption_ClickLeft(this, e);
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Перевод фокуса на поле ввода
        /// </summary>
        public virtual void __mInputFocus()
        {
            foreach (var vControl in Panel2.Controls)
            {
                switch (vControl.GetType().ToString())
                {
                    case "nlElements.elmComponentString":
                        (vControl as nlElements.elmComponentString).Focus();
                        break;
                }
            }
            Focus();
        }
        /// <summary>
        /// Сборка выражения с параметрами и перевод выражения на язык интерфейса 
        /// </summary>
        /// <param name="pString">Текст</param>
        /// <param name="pParameters">Список дополнительных парамметров</param>
        public void __mCaptionBuilding(string pString, params object[] pParameters)
        {
            fTextWithOutTranslate = String.Format(pString, pParameters);
            _cLabelCaption.Text = elmApplication.__oTunes.__mTranslate(pString, pParameters);
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Название поля таблицы
        /// </summary>
        public string __fFieldName = "";
        /// <summary>
        /// Разрешает / Запрещает очищать поле ввода при клике 
        /// </summary>
        public bool __fInputClearOnClickLeft = true;
        /// <summary>
        /// Псевдоним таблицы в запросе данных
        /// </summary>
        public string __fTableAlias = "";
        /// <summary>
        /// Индекс настройки
        /// </summary>
        /// <remarks>Используется в форме 'Изменение настроек приложения'</remarks>
        public int __fTuneIndex = -1;

        #endregion Атрибуты 

        #region - Закрытые

        /// <summary>
        /// Строка заголовка без перевода
        /// </summary>
        private string fTextWithOutTranslate = "";
        /// <summary>
        /// Подсказка
        /// </summary>
        private string fPromptCaption = "";
        /// <summary>
        /// Доступность контрола
        /// </summary>
        private bool fEnabled = true;
        /// <summary>
        /// Обязательность заполнения
        /// </summary>
        private FILLTYPES fFillType = FILLTYPES.None;
        /// <summary>
        /// Заголовок формы для поиска без перевода
        /// </summary>
        private string fCaptionNotTranslate = "";
        /// <summary>
        /// Запрет загрузки данных из файла
        /// </summary>
        private bool fNotLoad = false;

        #endregion Закрытые

        #region - Компоненты

        /// <summary>
        /// Компонент - включатель построения фильтра
        /// </summary>
        protected elmComponentMark _cMark = new elmComponentMark();
        /// <summary>
        /// Надпись - заголовок
        /// </summary>
        protected elmComponentLabel _cLabelCaption = new elmComponentLabel();
        /// <summary>
        /// Надпись - значение, отображаемое только для чтения
        /// </summary>
        protected elmComponentLabel _cLabelValue = new elmComponentLabel();

        #endregion Компоненты

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

        /// <summary>
        /// Текст заголовка
        /// </summary>
        /// <remarks>Отображаемый текст переводиться на язык интерфейса. Возвращается не переведенный текст</remarks>
        public string __fCaption_
        {
            get { return fCaptionNotTranslate; }
            set
            {
                fCaptionNotTranslate = value;
                _cLabelCaption.__fCaption_ = elmApplication.__oTunes.__mTranslate(value);
            }
        }
        /// <summary>
        /// Доступность включателя для построения фильтра
        /// </summary>
        public bool __fMarkEnabled_
        {
            get { return _cMark.Enabled; }
            set { _cMark.Enabled = value; }
        }
        /// <summary>
        /// Статус включатель для построения фильтра
        /// </summary>
        public bool __fMarkStatus_
        {
            get { return _cMark.Checked; }
            set { _cMark.Checked = value; }
        }
        /// <summary>
        /// Видимость включателя для построения фильтра
        /// </summary>
        public bool __fMarkVisible_
        {
            get { return _cMark.Visible; }
            set
            {
                _cMark.Visible = value;
                if (value == true)
                    _cLabelCaption.Location = new Point(_cMark.Left + _cMark.Width, elmInterface.__fIntervalHorizontal);
                else
                    _cLabelCaption.Location = new Point(0, elmInterface.__fIntervalHorizontal);
            }
        }
        /// <summary>
        /// Доступность контрола
        /// </summary>
        public virtual bool __fEnabled_
        {
            get { return fEnabled; }
            set
            {
                fEnabled = value;
                _cLabelValue.Visible = !fEnabled;
            }
        }
        /// <summary>
        /// Обязательность заполнения
        /// </summary>
        public virtual FILLTYPES __fFillType_
        {
            get { return fFillType; }
            set { fFillType = value; }
        }
        /// <summary>
        /// Условие фильтра для указанного поля
        /// </summary>
        public virtual string __fFilterExpression_ { get; }
        /// <summary>
        /// Выражение фильтра для указанного поля для отображения пользователю
        /// </summary>
        public virtual string __fFilterMessage_ { get; }
        /// <summary>
        /// Запрет загрузки фильтра из файла
        /// </summary>
        /// <remarks>Загружаются данные по умолчанию, определенные программой</remarks>
        public virtual bool __fFilterNotLoad_
        {
            get { return fNotLoad; }
            set { fNotLoad = value; }
        }
        /// <summary>
        /// Вид надписи-заголовка
        /// </summary>
        public virtual LABELTYPES __fLabelCaptionType_
        {
            get { return _cLabelCaption.__fLabelType_; }
            set { _cLabelCaption.__fLabelType_ = value; }
        }
        /// <summary>
        /// Всплывающая подсказка для заголовка контрола
        /// </summary>
        public virtual string __fPromptCaption_
        {
            get { return fPromptCaption; }
            set
            {
                fPromptCaption = elmApplication.__oTunes.__mTranslate(value);

                ToolTip vToolTip = new ToolTip();
                vToolTip.ToolTipIcon = ToolTipIcon.Info;
                vToolTip.IsBalloon = true;
                vToolTip.ShowAlways = true;

                vToolTip.SetToolTip(_cLabelCaption, value);
            }
        }
        /// <summary>
        /// Значение контрола
        /// </summary>
        public virtual object __fValue_
        {
            get { return null; }
            set { }
        }
        /// <summary>
        /// Название значения контрола
        /// </summary>
        public virtual string __fValueToText_
        {
            get { return _cLabelValue.Text; }
            set { _cLabelValue.Text = value.Trim(); }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при изменении данных в поле ввода
        /// </summary>
        public event EventHandler __eLabelCaption_ClickLeft;

        #endregion СОБЫТИЯ

    }
}
