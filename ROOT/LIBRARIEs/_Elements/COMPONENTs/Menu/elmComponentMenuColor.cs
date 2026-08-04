using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentMenuColor.cs
    /// </summary>
    /// <remarks>Класс-Компонент меню формы (с возможностью изменения цветов)</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 09-28</version> // Дата-время последней корректировки
    public class elmComponentMenuColor : ProfessionalColorTable
    {
        #region = МЕТОДЫ

        #region - Процедуры

        #region /// Меню верхнего уровня

        /// <summary>
        /// Пункт меню под курсором
        /// </summary>
        public override Color ButtonSelectedHighlight
        {
            get { return Color.Green; }
        }
        /// <summary>
        /// Получает верхний цвет градиента, используемого при выборе меню верхнего уровня
        /// </summary>
        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.Yellow; }
        }
        /// <summary>
        /// Получает нижний цвет градиента, используемого при выборе меню верхнего уровня
        /// </summary>
        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.Blue; }
        }
        /// <summary>
        /// Рамка окружающая все меню
        /// </summary>
        public override Color MenuBorder
        {
            get { return Color.Red; } //
        }

        #endregion Меню верхнего уровня

        #endregion Процедуры

        #endregion МЕТОДЫ

        /// <summary>
        /// Получает начальный цвет градиента, используемого в MenuStrip
        /// </summary>
        public override Color MenuStripGradientBegin
        {
            get { return Color.Red; }
        }
        /// <summary>
        /// Получает конечный цвет градиента, используемого в MenuStrip
        /// </summary>
        public override Color MenuStripGradientEnd
        {
            get { return Color.Red; }
        }
        /// <summary>
        /// Получает средний цвет градиента, используемого при нажатии на элемент ToolStripMenuItem верхнего уровня
        /// </summary>
        public override Color MenuItemPressedGradientMiddle
        {
            get { return Color.Red; }
        }
        /// <summary>
        /// Получает начальный цвет градиента, используемого при выборе ToolStripMenuItem
        /// </summary>
        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.Red; }
        }
        /// <summary>
        /// Получает конечный цвет градиента, используемого при выборе ToolStripMenuItem
        /// </summary>
        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.Red; }
        }
        public override Color MenuItemSelected
        {
            get { return Color.Red; }
        }
        /// <summary>
        /// Цвет фона не выбранного пункта меню
        /// </summary>
        public override Color ToolStripDropDownBackground
        {
            get { return elmApplication.__oInterface.__mColor(COLORS.FormActive); }
        }
        public override Color ImageMarginGradientBegin
        {
            get { return elmApplication.__oInterface.__mColor(COLORS.FormActive); }
        }
        public override Color ImageMarginGradientEnd
        {
            get { return elmApplication.__oInterface.__mColor(COLORS.FormActive); }
        }
        public override Color ImageMarginGradientMiddle
        {
            get { return elmApplication.__oInterface.__mColor(COLORS.FormActive); }
        }
        public override Color MenuItemBorder
        {
            get { return Color.DarkRed; }
        }
        //--
        public override Color ButtonCheckedGradientBegin
        {
            get { return Color.Red; }
        }
        public override Color ButtonCheckedGradientEnd
        {
            get { return Color.Red; }
        }
        public override Color ButtonCheckedGradientMiddle
        {
            get { return Color.Red; }
        }
        public override Color ButtonCheckedHighlight
        {
            get { return Color.Red; }
        }
        public override Color ButtonCheckedHighlightBorder
        {
            get { return Color.Red; }
        }
        public override Color ButtonPressedBorder
        {
            get { return Color.Red; }
        }
        public override Color ButtonPressedGradientBegin
        {
            get { return Color.Red; }
        }
        public override Color ButtonPressedGradientEnd
        {
            get { return Color.Red; }
        }
        public override Color ButtonPressedGradientMiddle
        {
            get { return Color.Red; }
        }
        public override Color ButtonPressedHighlight
        {
            get { return Color.Red; }
        }
        public override Color ButtonPressedHighlightBorder
        {
            get { return Color.Red; }
        }
        public override Color ButtonSelectedBorder
        {
            get { return Color.Red; }
        }
        public override Color ButtonSelectedGradientBegin
        {
            get { return Color.Red; }
        }
        public override Color ButtonSelectedGradientEnd
        {
            get { return Color.Red; }
        }
        public override Color ButtonSelectedGradientMiddle
        {
            get { return Color.White; }
        }
        public override Color ButtonSelectedHighlightBorder
        {
            get { return Color.Red; }
        }
        public override Color CheckBackground
        {
            get { return Color.Red; }
        }
        public override Color CheckPressedBackground
        {
            get { return Color.Yellow; }
        }
        public override Color CheckSelectedBackground
        {
            get { return Color.Red; }
        }
        public override Color GripDark
        {
            get { return Color.Red; }
        }
        public override Color GripLight
        {
            get { return Color.Red; }
        }
        public override Color ImageMarginRevealedGradientBegin
        {
            get { return Color.Red; }
        }
        public override Color ImageMarginRevealedGradientEnd
        {
            get { return Color.Red; }
        }
        public override Color ImageMarginRevealedGradientMiddle
        {
            get { return Color.Red; }
        }
        public override Color OverflowButtonGradientBegin
        {
            get { return Color.Red; }
        }
        public override Color OverflowButtonGradientEnd
        {
            get { return Color.Red; }
        }
        public override Color OverflowButtonGradientMiddle
        {
            get { return Color.Red; }
        }
        public override Color RaftingContainerGradientBegin => base.RaftingContainerGradientBegin;
        public override Color RaftingContainerGradientEnd => base.RaftingContainerGradientEnd;
        public override Color SeparatorDark
        {
            get { return Color.Red; }
        }
        //
        public override Color SeparatorLight
        {
            get { return Color.YellowGreen; }
        }
        public override Color StatusStripGradientBegin
        {
            get { return Color.White; }
        }
        public override Color StatusStripGradientEnd
        {
            get { return Color.White; }
        }
        public override Color ToolStripBorder
        {
            get { return Color.White; }
        }
        public override Color ToolStripContentPanelGradientBegin
        {
            get { return Color.White; }
        }
        public override Color ToolStripContentPanelGradientEnd
        {
            get { return Color.White; }
        }
        public override Color ToolStripGradientBegin
        {
            get { return Color.White; }
        }
        public override Color ToolStripGradientEnd
        {
            get { return Color.White; }
        }
        public override Color ToolStripGradientMiddle
        {
            get { return Color.White; }
        }
        public override Color ToolStripPanelGradientBegin
        {
            get { return Color.White; }
        }
        public override Color ToolStripPanelGradientEnd
        {
            get { return Color.White; }
        }
    }
}
