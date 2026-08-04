using System;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Класс-Компонент разделителя областей</remarks>
    public class elmComponentSplitter : SplitContainer
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public elmComponentSplitter()
        {
            _mObjectAssembly();
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        {
            SuspendLayout();

            #region /// Настройка компонентов

            BackColor = Color.Transparent;
            BorderStyle = BorderStyle.Fixed3D;
            Dock = DockStyle.Fill;
            Orientation = Orientation.Vertical;
            TabStop = false;

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected virtual void _mObjectPresentation()
        {
            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется после создания объекта
        /// </summary>
        protected override void OnCreateControl()
        {
            _mObjectPresentation();
            base.OnCreateControl();

            return;
        }

        #endregion Поведение

        #endregion МЕТОДЫ
    }
}
