using System;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmUnitTreeNode.cs
    /// </summary>
    /// <remarks>Класс узла 'TreeView'</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 00-00</version> // Дата-время последней корректировки
    public class elmUnitTreeNode : TreeNode
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public elmUnitTreeNode()
        {
            _mObjectAssembly();
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        {
            // Отсутствует SuspendLayout

            #region /// Настройка компонента

            NodeFont = elmApplication.__oInterface.__mFont(FONTS.NodeNotEdit);
            ForeColor = elmApplication.__oInterface.__mColor(COLORS.Text);

            #endregion Настройка компонента

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int __fClue = 0;
        /// <summary>
        /// Идентификатор родительской записи
        /// </summary>
        public int __fClueParent = 0;
        /// <summary>
        /// Описание
        /// </summary>
        public string __fDescription = "";
        /// <summary>
        /// Узел является папкой, иначе - значением
        /// </summary>
        public bool __fNodeFolder = false;
        /// <summary>
        /// Название формы для открытия узла при исползовании его в качестве меню
        /// </summary>
        public string __fFormCall = "";
        /// <summary>
        /// Служебная папка
        /// </summary>
        public bool __fNodeService = false;
        /// <summary>
        /// Сортировка
        /// </summary>
        public int __fSort = 0;

        #endregion Атрибуты

        #region - Закрытые

        /// <summary>
        /// Текст заголовка узла
        /// </summary>
        private string fCaption = "";

        #endregion Закрытые

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Текст заголовка узла
        /// </summary>
        public string __fCaption_
        {
            get { return fCaption; }
            set
            {
                fCaption = value;
                Text = elmApplication.__oTunes.__mTranslate(fCaption);
            }
        }

        #endregion СВОЙСТВА
    }
}
