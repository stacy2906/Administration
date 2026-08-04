using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nlSystemTray
{
    /// <summary>
    /// Файл staComponentPopupMenu.cs
    /// </summary>
    /// <remarks>Класс всплывающего меню</remarks>
    public class staComponentPopupMenu : ContextMenuStrip
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public staComponentPopupMenu()
        {
            _mObjectAssembly();
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        {
            #region /// Описание компонента


            #endregion Описание компонента

            Type vType = this.GetType();
            Name = vType.Name;
            _fClassNameFull = vType.Namespace + "." + vType.Name + ".";

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected virtual void _mObjectPresentation()
        {
        }
        /// <summary>
        /// Выполняется при клике кнопки мыши по иконке в трее
        /// </summary>
        /// <param name="e"></param>


        #endregion Поведение

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary> 
        /// Полное название класса
        /// </summary>
        protected string _fClassNameFull = "";

        #endregion Атрибуты

        #endregion ПОЛЯ
    }
}
