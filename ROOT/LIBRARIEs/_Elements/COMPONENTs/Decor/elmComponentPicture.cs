using System;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentPicture.cs
    /// </summary>
    /// <remarks>Класс-Компонент для отображении картинки на форме</remarks>
 	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 09-21</version> // Дата-время последней корректировки
    public class elmComponentPicture : PictureBox
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public elmComponentPicture()
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

            #endregion Настройка компонента

            ResumeLayout();

            return;
        }

        #endregion ДИЗАЙНЕРЫ
    }
}
