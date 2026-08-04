using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Класс-Компонент блок вкладок (с возможностью изменения цветов)</remarks>
    public class elmComponentPagesBlockColor : TabControl
    {
        #region = БИБЛИОТЕКИ

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowDC(IntPtr handle);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr ReleaseDC(IntPtr handle, IntPtr hDC);

        [DllImport("Gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hwnd, char[] className, int maxCount);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindow(IntPtr hwnd, int uCmd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindowVisible(IntPtr hwnd);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern int GetClientRect(IntPtr hwnd, ref RECT lpRect);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern int GetClientRect(IntPtr hwnd, [In, Out] ref Rectangle rect);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool UpdateWindow(IntPtr hwnd);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool InvalidateRect(IntPtr hwnd, ref Rectangle rect, bool bErase);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool ValidateRect(IntPtr hwnd, ref Rectangle rect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool GetWindowRect(IntPtr hWnd, [In, Out] ref Rectangle rect);

        #endregion = БИБЛИОТЕКИ

        #region = ДИЗАЙНЕРЫ

        /// <summary> 
        /// Конструктор без параметров
        /// </summary>
        public elmComponentPagesBlockColor()
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

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            bUpDown = false;

            this.ControlAdded += new ControlEventHandler(FlatTabControl_ControlAdded);
            this.ControlRemoved += new ControlEventHandler(FlatTabControl_ControlRemoved);
            this.SelectedIndexChanged += new EventHandler(_mSelectedIndexChanged);

            //__fImagesButtonsLeftRight.Images.Add(global::nlResourcesImages.Properties.Resources._ArrowLeftEnabled_b16C);
            //__fImagesButtonsLeftRight.Images.Add(global::nlResourcesImages.Properties.Resources._ArrowRightEnabled_b16C);
            //__fImagesButtonsLeftRight.Images.Add(global::nlResourcesImages.Properties.Resources._ArrowLeftDisabled_a16C);
            //__fImagesButtonsLeftRight.Images.Add(global::nlResourcesImages.Properties.Resources._ArrowRightDisabled_a16C);

            fWindowBackColor = elmApplication.__oInterface.__mColor(COLORS.DataBackNecessarily);

            #endregion Настройка компонентов

            ResumeLayout();

            return;
        }
        /// <summary> 
        /// Презентация объекта
        /// </summary>
        protected virtual void _mObjectPresentation()
        {
            FindUpDown();

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary> 
        /// Выполняется при после создания объекта
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _mObjectPresentation();
            return;
        }
        /// <summary> 
        /// Выполняется при перерисовке 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawControl(e.Graphics);

            return;
        }

        #endregion Поведение

        #region - Процедуры

        internal void DrawControl(Graphics pGraphics)
        {
            if (!Visible)
                return;

            Rectangle vTabControlArea = this.ClientRectangle;
            Rectangle vTabArea = this.DisplayRectangle;

            Brush vBrush = new SolidBrush(fWindowBackColor); //(SystemColors.Control); UPDATED
            pGraphics.FillRectangle(vBrush, vTabControlArea);
            vBrush.Dispose();
            int nDelta = SystemInformation.Border3DSize.Width; // Определение размера рамки

            Pen vPenBorder = new Pen(SystemColors.ControlDark);
            vTabArea.Inflate(nDelta, nDelta);
            pGraphics.DrawRectangle(vPenBorder, vTabArea);
            vPenBorder.Dispose();

            Region vRegion = pGraphics.Clip;
            Rectangle rreg;

            int nWidth = vTabArea.Width + nMargin;
            if (bUpDown)
            {
                ///* Исключение скрываемой вкладки из перерисовки
                if (IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rupdown = new Rectangle();
                    GetWindowRect(scUpDown.Handle, ref rupdown);
                    Rectangle rupdown2 = this.RectangleToClient(rupdown);

                    nWidth = rupdown2.X;
                }
            }

            rreg = new Rectangle(vTabArea.Left, vTabControlArea.Top, nWidth - nMargin, vTabControlArea.Height);

            pGraphics.SetClip(rreg);

            // draw tabs
            for (int i = 0; i < this.TabCount; i++)
                DrawTab(pGraphics, this.TabPages[i], i);

            pGraphics.Clip = vRegion;

            if (this.SelectedTab != null)
            {
                TabPage tabPage = this.SelectedTab;
                Color color = tabPage.BackColor;
                vPenBorder = new Pen(color);

                vTabArea.Offset(1, 1);
                vTabArea.Width -= 2;
                vTabArea.Height -= 2;

                pGraphics.DrawRectangle(vPenBorder, vTabArea);
                vTabArea.Width -= 1;
                vTabArea.Height -= 1;
                pGraphics.DrawRectangle(vPenBorder, vTabArea);

                vPenBorder.Dispose();
            }
        }

        internal void DrawTab(Graphics g, TabPage tabPage, int nIndex)
        {
            Rectangle recBounds = this.GetTabRect(nIndex);
            RectangleF tabTextArea = (RectangleF)this.GetTabRect(nIndex);

            bool bSelected = (this.SelectedIndex == nIndex);

            Point[] pt = new Point[7];
            if (this.Alignment == TabAlignment.Top)
            {
                pt[0] = new Point(recBounds.Left, recBounds.Bottom);
                pt[1] = new Point(recBounds.Left, recBounds.Top + 3);
                pt[2] = new Point(recBounds.Left + 3, recBounds.Top);
                pt[3] = new Point(recBounds.Right - 3, recBounds.Top);
                pt[4] = new Point(recBounds.Right, recBounds.Top + 3);
                pt[5] = new Point(recBounds.Right, recBounds.Bottom);
                pt[6] = new Point(recBounds.Left, recBounds.Bottom);
            }
            else
            {
                pt[0] = new Point(recBounds.Left, recBounds.Top);
                pt[1] = new Point(recBounds.Right, recBounds.Top);
                pt[2] = new Point(recBounds.Right, recBounds.Bottom - 3);
                pt[3] = new Point(recBounds.Right - 3, recBounds.Bottom);
                pt[4] = new Point(recBounds.Left + 3, recBounds.Bottom);
                pt[5] = new Point(recBounds.Left, recBounds.Bottom - 3);
                pt[6] = new Point(recBounds.Left, recBounds.Top);
            }

            //----------------------------
            // fill this tab with background color
            Brush br = new SolidBrush(tabPage.BackColor);
            g.FillPolygon(br, pt);
            br.Dispose();
            //----------------------------

            //----------------------------
            // draw border
            //g.DrawRectangle(SystemPens.ControlDark, recBounds);
            g.DrawPolygon(SystemPens.ControlDark, pt);

            if (bSelected)
            {
                //----------------------------
                // clear bottom lines
                Pen pen = new Pen(tabPage.BackColor);

                switch (this.Alignment)
                {
                    case TabAlignment.Top:
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Bottom, recBounds.Right - 1, recBounds.Bottom);
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Bottom + 1, recBounds.Right - 1, recBounds.Bottom + 1);
                        break;

                    case TabAlignment.Bottom:
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Top, recBounds.Right - 1, recBounds.Top);
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Top - 1, recBounds.Right - 1, recBounds.Top - 1);
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Top - 2, recBounds.Right - 1, recBounds.Top - 2);
                        break;
                }

                pen.Dispose();
                //----------------------------
            }
            //----------------------------

            //----------------------------
            // draw tab's icon
            if ((tabPage.ImageIndex >= 0) && (ImageList != null) && (ImageList.Images[tabPage.ImageIndex] != null))
            {
                int nLeftMargin = 8;
                int nRightMargin = 2;

                Image img = ImageList.Images[tabPage.ImageIndex];

                Rectangle rimage = new Rectangle(recBounds.X + nLeftMargin, recBounds.Y + 1, img.Width, img.Height);

                // adjust rectangles
                float nAdj = (float)(nLeftMargin + img.Width + nRightMargin);

                rimage.Y += (recBounds.Height - img.Height) / 2;
                tabTextArea.X += nAdj;
                tabTextArea.Width -= nAdj;

                // draw icon
                g.DrawImage(img, rimage);
            }
            //----------------------------

            //----------------------------
            // draw string
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            br = new SolidBrush(tabPage.ForeColor);

            g.DrawString(tabPage.Text, Font, br, tabTextArea, stringFormat);
            //----------------------------
        }
        /// <summary>* Рисование кнопок перебора вкладок
        /// </summary>
        /// <param name="pGraphics"></param>
        internal void DrawIcons(Graphics pGraphics)
        {
            if ((__fImagesButtonsLeftRight == null) || (__fImagesButtonsLeftRight.Images.Count != 4))
                return;
            Rectangle vTabControlArea = this.ClientRectangle;

            Rectangle vRectangle = new Rectangle();
            GetClientRect(scUpDown.Handle, ref vRectangle);

            Brush vBrush = new SolidBrush(__fWindowBackColor);
            pGraphics.FillRectangle(vBrush, vRectangle);
            vBrush.Dispose();

            Pen vBorder = new Pen(SystemColors.ControlDark);
            Rectangle vRectangleBorder = vRectangle;
            vRectangleBorder.Inflate(-1, -1);
            pGraphics.DrawRectangle(vBorder, vRectangleBorder);
            vBorder.Dispose();

            int nMiddle = (vRectangle.Width / 2);
            int nTop = (vRectangle.Height - 16) / 2;
            int nLeft = (nMiddle - 16) / 2;

            Rectangle vRectangleLeft = new Rectangle(nLeft, nTop, 16, 16);
            Rectangle vRectabgleRight = new Rectangle(nMiddle + nLeft, nTop, 16, 16);

            Image vImage = __fImagesButtonsLeftRight.Images[1];
            if (vImage != null)
            {
                if (this.TabCount > 0)
                {
                    Rectangle r3 = this.GetTabRect(0);
                    if (r3.Left < vTabControlArea.Left)
                        pGraphics.DrawImage(vImage, vRectangleLeft);
                    else
                    {
                        vImage = __fImagesButtonsLeftRight.Images[3];
                        if (vImage != null)
                            pGraphics.DrawImage(vImage, vRectangleLeft);
                    }
                }
            }

            vImage = __fImagesButtonsLeftRight.Images[0];
            if (vImage != null)
            {
                if (this.TabCount > 0)
                {
                    Rectangle r3 = this.GetTabRect(this.TabCount - 1);
                    if (r3.Right > (vTabControlArea.Width - vRectangle.Width))
                        pGraphics.DrawImage(vImage, vRectabgleRight);
                    else
                    {
                        vImage = __fImagesButtonsLeftRight.Images[2];
                        if (vImage != null)
                            pGraphics.DrawImage(vImage, vRectabgleRight);
                    }
                }
            }
        }

        private void FlatTabControl_ControlAdded(object sender, ControlEventArgs e)
        {
            FindUpDown();
            UpdateUpDown();

        }

        private void FlatTabControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            FindUpDown();
            UpdateUpDown();
        }
        /// <summary>
        /// Выполняется при смене активной вкладки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _mSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUpDown();
            if (__eSelectedIndexChange != null)
                __eSelectedIndexChange(this, e);	///* Перерисовка блока вкладок 
			Invalidate();
        }

        private void FindUpDown()
        {
            bool bFound = false;

            // find the UpDown control
            ///IntPtr pWnd = Win32.GetWindow(this.Handle, Win32.GW_CHILD);
            IntPtr pWnd = GetWindow(this.Handle, GW_CHILD);

            while (pWnd != IntPtr.Zero)
            {
                //----------------------------
                // Get the window class name
                char[] className = new char[33];

                ///int length = Win32.GetClassName(pWnd, className, 32);
                int length = GetClassName(pWnd, className, 32);

                string s = new string(className, 0, length);
                //----------------------------

                if (s == "msctls_updown32")
                {
                    bFound = true;

                    if (!bUpDown)
                    {
                        //----------------------------
                        // Subclass it
                        this.scUpDown = new SubClass(pWnd, true);
                        this.scUpDown.SubClassedWndProc += new SubClass.SubClassWndProcEventHandler(scUpDown_SubClassedWndProc);
                        //----------------------------

                        bUpDown = true;
                    }
                    break;
                }

                pWnd = GetWindow(pWnd, GW_HWNDNEXT);
            }

            if ((!bFound) && (bUpDown))
                bUpDown = false;
        }

        private void UpdateUpDown()
        {
            if (bUpDown)
            {
                if (IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rect = new Rectangle();
                    GetClientRect(scUpDown.Handle, ref rect);
                    InvalidateRect(scUpDown.Handle, ref rect, true);
                }
            }

            return;
        }
        /// <summary>
        /// scUpDown_SubClassedWndProc Event Handler
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private int scUpDown_SubClassedWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_PAINT:
                    {
                        IntPtr vIntPtr = GetWindowDC(scUpDown.Handle);
                        Graphics vGraphics = Graphics.FromHdc(vIntPtr);
                        DrawIcons(vGraphics);
                        vGraphics.Dispose();
                        ReleaseDC(scUpDown.Handle, vIntPtr);
                        m.Result = IntPtr.Zero;

                        //------------------------
                        // validate current rect
                        Rectangle rect = new Rectangle();

                        GetClientRect(scUpDown.Handle, ref rect);
                        ValidateRect(scUpDown.Handle, ref rect);
                        //------------------------
                    }
                    return 1;
            }

            return 0;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Атрибуты

        private SubClass scUpDown = null;
        private bool bUpDown; // true when the button UpDown is required
        /// <summary>
        /// * Массив изображений
        /// </summary>
        public ImageList __fImagesPages = new ImageList();
        /// <summary>
        /// * Массив изображений кнопок промотки вкладок
        /// </summary>
        public ImageList __fImagesButtonsLeftRight = new ImageList();
        private const int nMargin = 5;
        private Color fWindowBackColor = SystemColors.Control;

        #endregion Атрибуты

        #region - Константы

        public const int GW_HWNDFIRST = 0;
        public const int GW_HWNDLAST = 1;
        public const int GW_HWNDNEXT = 2;
        public const int GW_HWNDPREV = 3;
        public const int GW_OWNER = 4;
        public const int GW_CHILD = 5;

        public const int WM_NCCALCSIZE = 0x83;
        public const int WM_WINDOWPOSCHANGING = 0x46;
        public const int WM_PAINT = 0xF;
        public const int WM_CREATE = 0x1;
        public const int WM_NCCREATE = 0x81;
        public const int WM_NCPAINT = 0x85;
        public const int WM_PRINT = 0x317;
        public const int WM_DESTROY = 0x2;
        public const int WM_SHOWWINDOW = 0x18;
        public const int WM_SHARED_MENU = 0x1E2;
        public const int HC_ACTION = 0;
        public const int WH_CALLWNDPROC = 4;
        public const int GWL_WNDPROC = -4;

        #endregion Константы 

        #region - Структуры

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NCCALCSIZE_PARAMS
        {
            public RECT rgc;
            public WINDOWPOS wndpos;
        }

        #endregion Структуры

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        public new TabPageCollection TabPages
        {
            get
            {
                return base.TabPages;
            }
        }

        new public TabAlignment Alignment
        {
            get { return base.Alignment; }
            set
            {
                TabAlignment ta = value;
                if ((ta != TabAlignment.Top) && (ta != TabAlignment.Bottom))
                    ta = TabAlignment.Top;

                base.Alignment = ta;
            }
        }

        new public bool Multiline
        {
            get { return base.Multiline; }
            set { base.Multiline = false; }
        }

        /// <summary> 
        /// Цвет формы
        /// </summary>
        public Color __fWindowBackColor
        {
            get { return fWindowBackColor; }
            set
            {
                fWindowBackColor = value;
                IntPtr hDC = GetWindowDC(scUpDown.Handle);
                Graphics g = Graphics.FromHdc(hDC);
                DrawIcons(g);
                Invalidate();
            }
        }

        #endregion СВОЙСТВА

        /// <summary>
        /// Возникает при изменении данных
        /// </summary>
        public event EventHandler __eSelectedIndexChange;

    }
    /// <summary> 
    /// Класс 'SubClass'
    /// </summary>
    /// <remarks>Вспомогательный функционал</remarks>
    internal class SubClass : System.Windows.Forms.NativeWindow
    {
        public delegate int SubClassWndProcEventHandler(ref System.Windows.Forms.Message m);
        public event SubClassWndProcEventHandler SubClassedWndProc;
        private bool IsSubClassed = false;

        public SubClass(IntPtr Handle, bool _SubClass)
        {
            base.AssignHandle(Handle);
            this.IsSubClassed = _SubClass;
        }

        public bool SubClassed
        {
            get { return this.IsSubClassed; }
            set { this.IsSubClassed = value; }
        }

        protected override void WndProc(ref Message m)
        {
            if (this.IsSubClassed)
            {
                if (OnSubClassedWndProc(ref m) != 0)
                    return;
            }
            base.WndProc(ref m);
        }

        public void CallDefaultWndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        #region HiWord Message Cracker
        public int HiWord(int Number)
        {
            return ((Number >> 16) & 0xffff);
        }
        #endregion

        #region LoWord Message Cracker
        public int LoWord(int Number)
        {
            return (Number & 0xffff);
        }
        #endregion

        #region MakeLong Message Cracker
        public int MakeLong(int LoWord, int HiWord)
        {
            return (HiWord << 16) | (LoWord & 0xffff);
        }
        #endregion

        #region MakeLParam Message Cracker
        public IntPtr MakeLParam(int LoWord, int HiWord)
        {
            return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
        }
        #endregion

        private int OnSubClassedWndProc(ref Message m)
        {
            if (SubClassedWndProc != null)
            {
                return this.SubClassedWndProc(ref m);
            }

            return 0;
        }
    }
}
