using nlApplication;
using nlData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentCombo.cs
    /// </summary>
    /// <remarks>Класс-Компонент для ввода данных выбором из выпадающего списка</remarks>
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.14 08-42</version> // Дата-время последней корректировки
    /// <example>
    /// 
    /// </example>
    public class elmComponentCombo : ComboBox
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public elmComponentCombo()
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

            if (fComboType == COMBOTYPES.Bool)
                Size = new System.Drawing.Size(50, 21);
            BackColor = elmApplication.__oInterface.__mColor(COLORS.DataBack);
            ItemHeight = 20;
            DropDownStyle = ComboBoxStyle.DropDownList;
            DropDownHeight = 180;
            DropDownWidth = 200;
            Font = elmApplication.__oInterface.__mFont(FONTS.Data);
            ForeColor = elmApplication.__oInterface.__mColor(COLORS.Data);
            __fFillType_ = FILLTYPES.None;

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

        #region - Закрытые

        /// <summary>
        /// Получение пути и имени файла класса
        /// </summary>
        private string mFilePath([CallerFilePath] string filePath = "")
        {
            return filePath;
        }
        /// <summary>
        /// Получение значения номера строки в текущей процедуре
        /// </summary>
        private int mLine(string message = "", [CallerLineNumber] int line = 0)
        {
            return line;
        }
        /// <summary>
        /// Получение названия текущей процедуры
        /// </summary>
        private string mProcedure(string message, [CallerMemberName] string member = "")
        {
            return member;
        }
        /// <summary>
        /// Вычисление и установка размера объекта по существующим данным
        /// </summary>
        private void mWidthCalculate()
        {
            int vWidth = 20; // Устанавливаемая ширина объекта
            int vWidthFont = 0; // Ширина установленного шрифта

            if (__fSymbolsCount_ < 0)
                return;

            if (fScaleType == SCALETYPEs.Fixed)
            {
                for (int vAmount = 0; vAmount < fItemS.Count; vAmount++)
                {
                    int vSymbolsCount = fItemS[vAmount].__fValue_.ToString().Length;
                    if (vSymbolsCount <= 3)
                        vSymbolsCount = vSymbolsCount + 1;
                    vWidthFont = Convert.ToInt32(elmTypeFont.__mMeasureText(vSymbolsCount, this.Font).Width);
                    if (vWidthFont > vWidth)
                        vWidth = vWidthFont + SystemInformation.VerticalScrollBarWidth + elmInterface.__fIntervalHorizontal;
                }
                Width = vWidth + 10;
            }
            else
            {
                Width = Parent.Width - elmInterface.__fIntervalHorizontal * 2;
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            }

            return;
        }

        #endregion Закрытые

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
        /// <summary>
        /// Выполняется при потери фокуса элементом управления
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (__eChangedByUserAfter != null)
                __eChangedByUserAfter(this, e);

            return;
        }
        /// <summary>
        /// Выполняется перед закрытием выпадающего списка элемента управдения
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);
            if (__eChangedByUserAfter != null)
                __eChangedByUserAfter(this, e);

            return;
        }
        /// <summary>
        /// Выполняется перед открытием выпадающего списка элемента управдения
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDropDown(EventArgs e)
        {
            if (__eChangedByUserBefore != null)
                __eChangedByUserBefore(this, e);
            base.OnDropDown(e);
        }
        /// <summary>
        /// Выполняется при нажатии клавиши на клавиатуре, когда элемент управления находиться в фокусе
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (__eKeyDown != null)
                __eKeyDown(this, e);

            base.OnKeyDown(e);

            fKeyPressNow = true;

            return;
        }
        /// <summary>
        /// Выполняется при отпускании клавиши на клавиатуре, когда элемент управления находиться в фокусе
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            fKeyPressNow = false;

            return;
        }
        /// <summary>
        /// Выполняется при изменении данных в элементе управления
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            if (fKeyPressNow == false)
            {
                if (__eChangedByProgram != null)
                    __eChangedByProgram(this, e);
            }
            else
            {
                if (__eChangedByUserAfter != null)
                    __eChangedByUserAfter(this, e);
            }
            if (__eChanged != null)
                __eChanged(this, e);

            base.OnTextChanged(e);

            return;
        }
        /// <summary>
        /// Выполняется при проверке ввода данных в элемент управления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnValidating(CancelEventArgs e)
        {
            if (__fFillType_ == FILLTYPES.Necessarily)
            {
                if (Text.Length == 0)
                {
                    (FindForm() as elmForm).__mBaloonMessage(this, elmApplication.__oTunes.__mTranslate("Поле должно быть обязательно заполненным"));
                    e.Cancel = true;
                }
            }

            base.OnValidating(e);

            return;
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Загрузка или обновление отображаемых данных
        /// </summary>
        public void __mDataRefresh()
        {
            DataSource = null;
            Items.Clear(); // Очистка отображаемых данных
            Refresh();
            DataSource = fItemS; // Назначение списка, как источника данных
            DisplayMember = "__fValue_"; // Cтолбец для отображения
            ValueMember = "__fClue_"; // Cтолбец с идентификатором записи
            Refresh();
            mWidthCalculate(); // Расчет размера компонента

            if (__eChangedByProgram != null)
                __eChangedByProgram(this, new EventArgs());
            if (__eChanged != null)
                __eChanged(this, new EventArgs());

            Refresh();

            return;
        }
        /// <summary>
        /// Добавление значения в конец списка значений элемента управления
        /// </summary>
        /// <param name="pValue">Добавляемое значение</param>
        /// <returns>Название переводиться на язык интерфейса. Идентификатор добавляемой записи, положительный - из таблицы, отрицательный - назначаемый компонентом</returns>
        public int __mItemAdd(string pValue)
        {
            int vReturn = --fAmountClue; // Возвращаемое значение

            appUnitItem vItem = new appUnitItem();
            vItem.__fClue_ = vReturn;
            vItem.__fValue_ = elmApplication.__oTunes.__mTranslate(pValue);
            fItemS.Add(vItem);

            return vReturn;
        }
        /// <summary>
        /// Добавление списка значений в конец списка значений элемента управления
        /// </summary>
        /// <param name="pValueS">Список значений в порядке определения индексов</param>
        /// <returns>[true] - Значение добавлено, иначе - [false]</returns>
        public bool __mItemsAdd(ArrayList pValueS)
        {
            bool vReturn = false; // Возвращаемое значение

            foreach (string vValue in pValueS)
            {
                __mItemAdd(vValue);
                vReturn = true;
            }

            return vReturn;
        }
        /// <summary>
        /// Добавление списка значений в конец списка значений элемента управления
        /// </summary>
        /// <param name="pValueS">Список значений в порядке определения индексов</param>
        /// <returns>[true] - Значение добавлено, иначе - [false]</returns>
        public bool __mItemsAdd(params string[] pValueS)
        {
            bool vReturn = false; // Возвращаемое значение

            foreach (string vValue in pValueS)
            {
                __mItemAdd(vValue);
                vReturn = true;
            }

            return vReturn;
        }
        /// <summary>
        /// Добавление списка новых значений из списка слов разделенных запятой
        /// </summary>
        /// <param name="pValueS">Список значений в порядке определения индексов</param>
        /// <returns>[true] - Значение добавлено, иначе - [false]</returns>
        public bool __mItemsAddFromList(string pValueS)
        {
            bool vReturn = false; // Возвращаемое значение
            foreach (string vValue in appTypeString.__mWordsList(pValueS, ','))
            {
                __mItemAdd(vValue);
                vReturn = true;
            }

            return vReturn;
        }
        /// <summary>
        /// Очистка всех данных и подготовка к вводу новых данных
        /// </summary>
        public void __mItemsClear()
        {
            DataSource = null;
            Items.Clear();
            fItemS.Clear();
            fAmountClue = 0;
            this.Sorted = false;

            return;
        }
        /// <summary>
        /// Загрузка данных из сущности данных
        /// </summary>
        /// <param name="pWhereExpression">Выражение выбора получаемых данных</param>
        /// <param name="pOrderExpression">Выражение сортировки получаемых данных</param>
        /// <returns>[true] - данные загружены, иначе - [false]</returns>
        public bool __mItemsEssenceLoad(string pWhereExpression, string pOrderExpression)
        {
            bool vReturn = false; // Возвращаемое значение

            if (__oEssence != null)
            {
                __mItemsLoadFromDataTable(__oEssence.__mCombo(pWhereExpression, pOrderExpression));
                vReturn = true;
            }

            return vReturn;
        }
        /// <summary>
        /// Получение списка названий загруженных данных
        /// </summary>
        /// <returns>[ArrayList]</returns>
        public ArrayList __mItemsList()
        {
            ArrayList vArrayList = new ArrayList();

            foreach (var vItem in fItemS)
            {
                vArrayList.Add(vItem.__fValue_.ToString());
            }

            return vArrayList;
        }
        /// <summary>
        /// Загрузка данных из {DataTable}, со столбцами clu(идентификатор) и des(название)
        /// </summary>
        /// <param name="pDataTable">таблица</param>
        /// <returns>[true] - данные загружены, иначе - [false]</returns>
        public bool __mItemsLoadFromDataTable(DataTable pDataTable)
        {
            bool vReturn = false; // Возвращаемое значение

            if (pDataTable != null)
            {
                foreach (DataRow vDataRow in pDataTable.Rows)
                {
                    appUnitItem vItem = new appUnitItem();
                    vItem.__fClue_ = (int)vDataRow["clu"];
                    vItem.__fValue_ = (string)vDataRow["dsi"];
                    fItemS.Add(vItem);
                }

                vReturn = true;
            }

            return vReturn;
        }
        /// <summary>
        /// Получение индекса значения по идентификатору значения
        /// </summary>
        /// <param name="pClue"></param>
        /// <returns></returns>
        public int __mGetIndexByClue(int pClue)
        {
            int vReturn = 0;

            foreach (var vItem in fItemS)
            {
                if (vItem.__fClue_ == pClue)
                    break;
                vReturn++;
            }

            return vReturn;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Закрытые

        /// <summary>
        /// Счетчик внутренних идентификаторов
        /// </summary>
        private int fAmountClue = 0;
        /// <summary>
        /// Вид заполнения выпадающего списка данными
        /// </summary>
        private COMBOTYPES fComboType = COMBOTYPES.Items;
        /// <summary>
        /// Вид ввода данных
        /// </summary>
        private FILLTYPES fFillType = FILLTYPES.None;
        /// <summary>
        /// Список отображаемых данных
        /// </summary>
        private List<appUnitItem> fItemS = new List<appUnitItem>();
        /// <summary>
        /// Состояние - нажата клавиша клавиатуры 
        /// </summary>
        private bool fKeyPressNow = false;
        /// <summary>
        /// Вид привязки компонента
        /// </summary>
        private SCALETYPEs fScaleType = SCALETYPEs.Fixed;
        /// <summary>
        /// Количество отображаемых символов
        /// </summary>
        private int fSymbolCount = 10;

        #endregion Закрытые 

        #region - Объекты

        /// <summary>
        /// Сущность данных
        /// </summary>
        public datUnitEssence __oEssence;

        #endregion Объекты

        #endregion ПОЛЯ    

        #region = СВОЙСТВА

        #region - Скрытые

        /// <summary>
        /// Путь к файлу текущего класса
        /// </summary>
        protected string _fCurrentFilePath_
        {
            get { return mFilePath(); }
        }
        /// <summary>
        /// Текущая процедура класса
        /// </summary>
        protected string _fCurrentProcedure_
        {
            get { return mProcedure(""); }
        }
        /// <summary>
        /// Номер строки в файле класса
        /// </summary>
        protected int _fCurrentLine_
        {
            get { return mLine(""); }
        }

        #endregion Скрытые

        /// <summary>
        /// Вид заполнения выпадающего списка данными
        /// </summary>
        public COMBOTYPES __fComboType_
        {
            get { return fComboType; }
            set
            {
                __mItemsClear(); /// Очистка и подготовка компонента к вводу данных

                fComboType = value;
                if (fComboType == COMBOTYPES.Bool)
                {
                    appUnitItem vItem = new appUnitItem();
                    vItem.__fClue_ = 0;
                    vItem.__fValue_ = elmApplication.__oTunes.__mTranslate("Нет");
                    fItemS.Add(vItem);
                    vItem = new appUnitItem();
                    vItem.__fClue_ = 1;
                    vItem.__fValue_ = elmApplication.__oTunes.__mTranslate("Да");
                    fItemS.Add(vItem);
                    __mDataRefresh();
                }
            }
        }
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
        /// <summary>
        /// Значение
        /// </summary>
        public object __fValue_
        {
            get
            {
                object vReturn = null; // Возвращаемое значение

                switch (__fComboType_)
                {
                    case COMBOTYPES.Bool: /// Логическое значение
                        vReturn = SelectedIndex;
                        break;
                    default:
                        //if (SelectedIndex - 1 < fItemS.Count & fItemS.Count > 0)
                        if (SelectedIndex >= 0 & fItemS.Count > 0)
                            vReturn = fItemS[SelectedIndex].__fClue_;
                        break;
                }

                return vReturn;
            }
            set
            {
                switch (__fComboType_)
                {
                    case COMBOTYPES.Bool: /// Логическое значение
                        SelectedIndex = Convert.ToInt32(value);
                        break;
                    default:
                        if (Items.Count > __mGetIndexByClue(Convert.ToInt32(value)))
                        {
                            SelectedIndex = __mGetIndexByClue(Convert.ToInt32(value));
                            Refresh();
                        }
                        break;
                }
                if (__eChanged != null)
                    __eChanged(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Логическое значение
        /// </summary>
        public bool? __fValueToBool
        {
            get
            {
                if (__fComboType_ == COMBOTYPES.Bool)
                    return Convert.ToBoolean(Items[SelectedIndex]);
                else
                    return null;
            }
        }
        /// <summary>
        /// Отображаемое значение компонента
        /// </summary>
        public string __fValueToString_
        {
            get
            {
                if (SelectedIndex >= 0)
                    return fItemS[SelectedIndex].__fValue_.ToString();
                else
                    return "";
            }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при изменении данных
        /// </summary>
        public event EventHandler __eChanged;
        /// <summary>
        /// Возникает при изменении данных пользователем
        /// </summary>
        public event EventHandler __eChangedByUserAfter;
        /// <summary>
        /// Возникает перед изменением данных пользователем
        /// </summary>
        public event EventHandler __eChangedByUserBefore;
        /// <summary>
        /// Возникает при изменении данных программой
        /// </summary>
        public event EventHandler __eChangedByProgram;
        /// <summary>
        /// Возникает при нажатии клавиши
        /// </summary>
        public event EventHandler __eKeyDown;

        #endregion СОБЫТИЯ
    }
}
