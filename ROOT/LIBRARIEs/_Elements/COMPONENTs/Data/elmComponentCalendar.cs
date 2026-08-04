using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace nlElements
{
    public class elmComponentCalendar : MonthCalendar
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public elmComponentCalendar()
        {
            _mObjectAssembly();
        }
        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected virtual void _mObjectAssembly()
        { 
            SuspendLayout();

            host = new ToolStripControlHost(this)
            {
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            vDropDown = new ToolStripDropDown { Padding = Padding.Empty };
            vDropDown.Items.Add(host);
            vDropDown.AutoClose = true;
            
            ResumeLayout();

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
        /// Выполняется после создания объекта
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _mObjectPresentation();

            return;
        }

        #endregion Поведение

        #region - Процедуры

        protected override void OnDateChanged(DateRangeEventArgs drevent)
        {
            base.OnDateChanged(drevent);
            vDropDown.Visible = false;
            fValue = SelectionStart;
        }
        public void _mShowCalendar(int pCoordinateX, int pCoordinateY)
        {
            if (vDropDown.Visible == false)
                vDropDown.Visible = true;

            Point location = new Point(pCoordinateX, pCoordinateY);

            vDropDown.Show(location);
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        public ToolStripDropDown vDropDown = new ToolStripDropDown { Padding = Padding.Empty };
        ToolStripControlHost host;
        private DateTime fValue = new DateTime();

        #endregion Компоненты

        #endregion ПОЛЯ

        public DateTime __fValue_
        {
            get { return fValue; }
        }

        #region = СОБЫТИЯ
        #endregion СОБЫТИЯ
    }
}
