using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Test
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);
        private const int VK_LSHIFT = 0xA0;
        private const int VK_Z = 0x5A;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Magenta;
            this.TransparencyKey = Color.Magenta;
            this.ClientSize = new Size(700, 700);
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.DoubleBuffered = true;
            this.ShowInTaskbar = true;

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        /// <summary>
        /// Returns parameters with extended window styles, in this case
        /// to make window not clickable and to prevent receiving focus from user
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                // Window will have alpha channel and transparency
                const int WS_EX_LAYERED = 0x00080000;

                // Window won't receive clicks and focus from user
                const int WS_EX_TRANSPARENT = 0x00000020;

                // Window won't be active if user clicks on it
                const int WS_EX_NOACTIVATE = 0x08000000;

                CreateParams createParamsObject = base.CreateParams;
                createParamsObject.ExStyle |= WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_NOACTIVATE;
                return createParamsObject;
            }
        }

        /// <summary>
        /// Changes window state based on hotkey state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object? sender, EventArgs e)
        {
            short keyStateLeftShift = GetAsyncKeyState(VK_LSHIFT);
            short keyStateZ = GetAsyncKeyState(VK_Z);

            if ((keyStateLeftShift & 0x8000) != 0 && (keyStateZ & 0x8000) != 0)
            {
                Application.Exit();
            }

            if ((keyStateLeftShift & 0x8000) != 0)
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Minimized;
                }
                else if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
            }
        }

        /// <summary>
        /// Draws cirlce in the center of window
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            int windowCenterX = this.ClientSize.Width / 2;
            int windowCenterY = this.ClientSize.Height / 2;

            //Change code in this fragment if needed
            int drawedEllipseHeight = 200;
            int drawedEllipseWidth = 585;
            float topScale = 2.15f;
            float bottomScale = 2.72f;
            float pixelsToDrawLine = 20;
            float pixelsToDrawSpaceBetweenLines = 1;

            //Draws not filled ellipse
            PointF[] points = new PointF[100];
            for (int i = 0; i < 100; i++)
            {
                double angle = i * Math.PI * 2 / 100;
                float x = (float)(Math.Cos(angle) * drawedEllipseWidth / 2);
                float y = (float)(Math.Sin(angle) * drawedEllipseHeight / 2);

                float t = (y < 0) ? topScale : bottomScale;
                y *= t;

                points[i] = new PointF(windowCenterX + x, windowCenterY - 12 + y);
            }

            //Second numeric parameter in Pen() is line width
            Pen redPen = new Pen(Color.Red, 2);
            redPen.DashStyle = DashStyle.Custom;
            redPen.DashPattern = new float[] { pixelsToDrawLine, pixelsToDrawSpaceBetweenLines };

            graphics.DrawPolygon(redPen, points);

            //Draws little filled ellipse in the center of screen
            /**int filledEllipseHeight = 10;
            int filledEllipseWidth = 10;
            int filledEllipseCenterX = windowCenterX - (filledEllipseWidth / 2);
            int filledEllipseCenterY = windowCenterY - (filledEllipseHeight / 2);
            graphics.FillEllipse(Brushes.Red, filledEllipseCenterX, filledEllipseCenterY - 12, filledEllipseWidth, filledEllipseHeight);**/
        }

        /// <summary>
        /// Redraws the window when its size in changed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
    }
}
