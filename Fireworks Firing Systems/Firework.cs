using Fireworks_Firing_Systems.Properties;
using System.Drawing;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Resources;

namespace Fireworks_Firing_Systems
{
    public class Firework
    {
        public int ID { get; set; }
        public string FireworkName { get; set; }
        public FireworkType Type { get; set; }
        public Color Colour { get; set; }
        public int Delay { get; set; }
        public int Length { get; set; }

        /// var temp = new Firework(File.ReadAllText("Fireworks\\0.json"));
        /// temp.Colour = Color.Blue;
        /// File.WriteAllTextAsync("Fireworks\\1.json", temp.ToJson());
        //public Firework() { }
        public Firework(int ID_, string Name_, FireworkType Type_, Color Colour_, int Delay_, int Length_)
        {
            ID = ID_;
            FireworkName = Name_;
            Type = Type_;
            Colour = Colour_;
            Delay = Delay_;
            Length = Length_;
        }

        public override string ToString() => $"[{ID.ToString()}] {FireworkName} : {Type} ({Colour}) [{Delay} - {Length}]";
        public string ToJson() => Newtonsoft.Json.JsonConvert.SerializeObject(this);
        public Firework FromJson(string Json) => Newtonsoft.Json.JsonConvert.DeserializeObject<Firework>(Json);
        public string GetIcon() => $"Images\\icons8_Firework_{Type}_100px.png";

        public Panel GetPanel() => (new FireworkDisplay(this)).GetPanel();
    }

    /// <summary>
    /// https://www.fireworkshawkesbay.co.nz/types-of-fireworks#
    /// </summary>
    public enum FireworkType
    {
        Sparkler,
        Fountain,
        Smoke_Ball,
        Missile,
        Mine,
        Cake,
        Roman_Candle
    }

    public class FireworkDisplay
    {
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private PictureBox pictureBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label2;
        private Label label3;
        private Panel panel2;
        private Button button1;
        private Label label4;

        public Firework firework;

        public FireworkDisplay(Firework firework)
        {
            this.firework = firework;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.label1 = new Label();
            this.pictureBox1 = new PictureBox();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.label2 = new Label();
            this.label3 = new Label();
            this.panel2 = new Panel();
            this.button1 = new Button();
            this.label4 = new Label();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.MaximumSize = new Size(935, 49);
            this.panel1.MinimumSize = new Size(680, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(935, 49);
            this.panel1.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 45F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.button1, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 4, 0);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.MaximumSize = new Size(933, 47);
            this.tableLayoutPanel1.MinimumSize = new Size(678, 47);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new Size(933, 47);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = DockStyle.Left;
            this.label1.Font = new Font("Verdana", 7F, FontStyle.Regular, GraphicsUnit.Point);
            this.label1.ForeColor = SystemColors.ControlDark;
            this.label1.ImageAlign = ContentAlignment.BottomCenter;
            this.label1.Location = new Point(776, 0);
            this.label1.Name = firework.Delay.ToString();
            this.label1.Size = new Size(46, 47);
            this.label1.TabIndex = 5;
            this.label1.Text = firework.Delay.ToString();
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            switch (firework.Type)
            {
                case FireworkType.Sparkler:
                    this.pictureBox1.Image = global::Fireworks_Firing_Systems.Properties.Resources.icons8_Firework_Sparkler_100px;
                    break;
                case FireworkType.Fountain:
                    this.pictureBox1.Image = global::Fireworks_Firing_Systems.Properties.Resources.icons8_Firework_Fountain_100px;
                    break;
                case FireworkType.Smoke_Ball:
                    this.pictureBox1.Image = global::Fireworks_Firing_Systems.Properties.Resources.icons8_Firework_Smoke_Ball_100px;
                    break;
                case FireworkType.Missile:
                    this.pictureBox1.Image = global::Fireworks_Firing_Systems.Properties.Resources.icons8_Firework_Missile_100px;
                    break;
                case FireworkType.Mine:
                    this.pictureBox1.Image = global::Fireworks_Firing_Systems.Properties.Resources.icons8_Firework_Mine_100px;
                    break;
                case FireworkType.Cake:
                    this.pictureBox1.Image = global::Fireworks_Firing_Systems.Properties.Resources.icons8_Firework_Cake_100px;
                    break;
                case FireworkType.Roman_Candle:
                    this.pictureBox1.Image = global::Fireworks_Firing_Systems.Properties.Resources.icons8_Firework_Roman_Candle_100px;
                    break;
                default:
                    break;
            }
            this.pictureBox1.Location = new Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(41, 41);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Location = new Point(50, 13);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new Size(675, 21);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = DockStyle.Left;
            this.label2.ForeColor = SystemColors.Control;
            this.label2.Location = new Point(3, 0);
            this.label2.Name = firework.FireworkName;
            this.label2.Size = new Size(61, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = firework.FireworkName;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = DockStyle.Left;
            this.label3.Font = new Font("Verdana", 7F, FontStyle.Regular, GraphicsUnit.Point);
            this.label3.ForeColor = SystemColors.ControlDark;
            this.label3.ImageAlign = ContentAlignment.BottomCenter;
            this.label3.Location = new Point(70, 0);
            this.label3.Name = firework.ID.ToString();
            this.label3.Size = new Size(46, 18);
            this.label3.TabIndex = 3;
            this.label3.Text = firework.ID.ToString();
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Anchor = AnchorStyles.None;
            this.panel2.BackColor = firework.Colour;
            this.panel2.Location = new Point(740, 13);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(20, 20);
            this.panel2.TabIndex = 2;
            IntPtr handle = Form1.CreateRoundRectRgn(0, 0, panel2.Width, panel2.Height, 20, 20);
            this.panel2.Region = System.Drawing.Region.FromHrgn(handle);
            Form1.DeleteObject(handle);
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Font = new Font("Verdana", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            this.button1.ForeColor = Color.FromArgb(200, 200, 201);
            this.button1.Image = Properties.Resources.icons8_Settings_32px;
            this.button1.Location = new Point(896, 3);
            this.button1.Name = "button1";
            this.button1.Size = new Size(34, 40);
            this.button1.TabIndex = 4;
            this.button1.TextImageRelation = TextImageRelation.TextBeforeImage;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = DockStyle.Left;
            this.label4.Font = new Font("Verdana", 7F, FontStyle.Regular, GraphicsUnit.Point);
            this.label4.ForeColor = SystemColors.ControlDark;
            this.label4.ImageAlign = ContentAlignment.BottomCenter;
            this.label4.Location = new Point(836, 0);
            this.label4.Name = firework.Length.ToString();
            this.label4.Size = new Size(46, 47);
            this.label4.TabIndex = 6;
            this.label4.Text = firework.Length.ToString();
            this.label4.TextAlign = ContentAlignment.MiddleLeft;

            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
        }
        public Panel GetPanel() => panel1;
    }
}
