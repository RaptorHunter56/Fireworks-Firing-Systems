using System.Drawing;
using System.Runtime.InteropServices;

namespace Fireworks_Firing_Systems
{
    public partial class Form1 : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipes, int nHeightEllipes);
        [DllImport("Gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public static class FormColor
        {
            public static Color Backgrond = System.Drawing.SystemColors.WindowFrame;
            public static Color RaisedTab = System.Drawing.SystemColors.WindowFrame;
        }

        private void button1_Click(object sender, EventArgs e) => this.Close();
        private void button2_Click(object sender, EventArgs e) => this.WindowState = (this.WindowState == FormWindowState.Normal) ? FormWindowState.Maximized : FormWindowState.Normal;
        private void button3_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;
        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public Form1()
        {
            InitializeComponent();
            tabControl1.DrawItem += new DrawItemEventHandler(OnDrawItem);
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            BntClick(BntSequencer);

            this.panel7.Controls.Add(new Firework(100, "Test", FireworkType.Missile, Color.Green, 10, 11).GetPanel());
            this.panel7.Controls.Add(new Firework(101, "Test2", FireworkType.Cake, Color.Blue, 10, 11).GetPanel());
        }

        #region Side Bars
        const int _ = 5;

        Rectangle TopBars { get { return new Rectangle(0, 0, this.ClientSize.Width, _); } }
        Rectangle LeftBars { get { return new Rectangle(0, 0, _, this.ClientSize.Height); } }
        Rectangle BottomBars { get { return new Rectangle(0, this.ClientSize.Height - _, this.ClientSize.Width, _); } }
        Rectangle RightBars { get { return new Rectangle(this.ClientSize.Width - _, 0, _, this.ClientSize.Height); } }

        Rectangle TopSmallBars { get { return new Rectangle(0, 0, panel1.ClientSize.Width + 3, _); } }
        Rectangle BottomSmallBars { get { return new Rectangle(0, this.ClientSize.Height - _, panel1.ClientSize.Width + 3, _); } }

        Rectangle TopLeftBars { get { return new Rectangle(0, 0, _, _); } }
        Rectangle TopRightBars { get { return new Rectangle(this.ClientSize.Width - _, 0, _, _); } }
        Rectangle BottomLeftBars { get { return new Rectangle(0, this.ClientSize.Height - _, _, _); } }
        Rectangle BottomRightBars { get { return new Rectangle(this.ClientSize.Width - _, this.ClientSize.Height - _, _, _); } }

        protected override void OnPaint(PaintEventArgs e) // you can safely omit this method if you want
        {
            //System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(52)))), ((int)(((byte)(51)))))
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(57, 52, 51)), LeftBars);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(57, 52, 51)), TopSmallBars);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(57, 52, 51)), BottomSmallBars);
        }
        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x84) // WM_NCHITTEST
            {
                var cursor = this.PointToClient(Cursor.Position);

                if (TopLeftBars.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRightBars.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeftBars.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRightBars.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;

                else if (TopBars.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                else if (LeftBars.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (RightBars.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (BottomBars.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
            }
        }
        #endregion

        private void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.FromArgb(42, 39, 39), 10);
            g.DrawRectangle(p, this.tabSetings.Bounds);
        }

        private void BntClick(Button button)
        {
            panel2.Height = button.Height;
            panel2.Top = button.Top;
            panel2.Left = button.Left;
            button.BackColor = Color.FromArgb(42, 39, 39);
            if (button != BntSequencer) BntSequencer.BackColor = Color.FromArgb(57, 52, 51);
            if (button != BntDatabase) BntDatabase.BackColor = Color.FromArgb(57, 52, 51);
            if (button != BntSettings) BntSettings.BackColor = Color.FromArgb(57, 52, 51);
            switch (button.Name)
            {
                case "BntSequencer":
                    tabControl1.SelectedTab = tabControl1.TabPages[0];
                    break;
                case "BntDatabase":
                    tabControl1.SelectedTab = tabControl1.TabPages[1];
                    break;
                case "BntSettings":
                    tabControl1.SelectedTab = tabControl1.TabPages[2];
                    break;
                default:
                    break;
            }
        }
        private void BntSequencer_Click(object sender, EventArgs e)
        {
            BntClick((Button)sender);
        }

        private void BntDatabase_Click(object sender, EventArgs e)
        {
            BntClick((Button)sender);
        }

        private void BntSettings_Click(object sender, EventArgs e)
        {
            BntClick((Button)sender);
        }
    }
}