using System.Reflection.Metadata.Ecma335;

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
        private Panel zzpanel8;
        private TableLayoutPanel zztableLayoutPanel7;
        private Label zzlabel11;
        private PictureBox zzpictureBox6;
        private FlowLayoutPanel zzflowLayoutPanel6;
        private Label zzlabel12;
        private Label zzlabel13;
        private Panel zzpanel11;
        private Button zzbutton4;
        private Label zzlabel14;

        public Firework firework;

        public FireworkDisplay(Firework firework)
        {
            this.firework = firework;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.zzpanel8 = new Panel();
            this.zztableLayoutPanel7 = new TableLayoutPanel();
            this.zzlabel11 = new Label();
            this.zzpictureBox6 = new PictureBox();
            this.zzflowLayoutPanel6 = new FlowLayoutPanel();
            this.zzlabel12 = new Label();
            this.zzlabel13 = new Label();
            this.zzpanel11 = new Panel();
            this.zzbutton4 = new Button();
            this.zzlabel14 = new Label();
            this.zzpanel8.SuspendLayout();
            this.zztableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zzpictureBox6)).BeginInit();
            this.zzflowLayoutPanel6.SuspendLayout();
            // 
            // zzpanel8
            // 
            this.zzpanel8.AutoSize = true;
            this.zzpanel8.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.zzpanel8.BorderStyle = BorderStyle.FixedSingle;
            this.zzpanel8.Controls.Add(this.zztableLayoutPanel7);
            this.zzpanel8.Dock = DockStyle.Top;
            this.zzpanel8.Location = new Point(0, 0);
            this.zzpanel8.MaximumSize = new Size(935, 49);
            this.zzpanel8.MinimumSize = new Size(680, 49);
            this.zzpanel8.Name = "zzpanel8";
            this.zzpanel8.Size = new Size(935, 49);
            this.zzpanel8.TabIndex = 3;
            // 
            // zztableLayoutPanel7
            // 
            this.zztableLayoutPanel7.AutoSize = true;
            this.zztableLayoutPanel7.ColumnCount = 6;
            this.zztableLayoutPanel7.ColumnStyles.Add(new ColumnStyle());
            this.zztableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.zztableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 45F));
            this.zztableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            this.zztableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            this.zztableLayoutPanel7.ColumnStyles.Add(new ColumnStyle());
            this.zztableLayoutPanel7.Controls.Add(this.zzlabel11, 3, 0);
            this.zztableLayoutPanel7.Controls.Add(this.zzpictureBox6, 0, 0);
            this.zztableLayoutPanel7.Controls.Add(this.zzflowLayoutPanel6, 1, 0);
            this.zztableLayoutPanel7.Controls.Add(this.zzpanel11, 2, 0);
            this.zztableLayoutPanel7.Controls.Add(this.zzbutton4, 5, 0);
            this.zztableLayoutPanel7.Controls.Add(this.zzlabel14, 4, 0);
            this.zztableLayoutPanel7.Dock = DockStyle.Fill;
            this.zztableLayoutPanel7.Location = new Point(0, 0);
            this.zztableLayoutPanel7.MaximumSize = new Size(933, 47);
            this.zztableLayoutPanel7.MinimumSize = new Size(678, 47);
            this.zztableLayoutPanel7.Name = "zztableLayoutPanel7";
            this.zztableLayoutPanel7.RowCount = 1;
            this.zztableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.zztableLayoutPanel7.Size = new Size(933, 47);
            this.zztableLayoutPanel7.TabIndex = 2;
            // 
            // zzlabel11
            // 
            this.zzlabel11.AutoSize = true;
            this.zzlabel11.Dock = DockStyle.Left;
            this.zzlabel11.Font = new Font("Verdana", 7F, FontStyle.Regular, GraphicsUnit.Point);
            this.zzlabel11.ForeColor = SystemColors.ControlDark;
            this.zzlabel11.ImageAlign = ContentAlignment.BottomCenter;
            this.zzlabel11.Location = new Point(776, 0);
            this.zzlabel11.Name = firework.Delay.ToString();
            this.zzlabel11.Size = new Size(46, 47);
            this.zzlabel11.TabIndex = 5;
            this.zzlabel11.Text = "label11";
            this.zzlabel11.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // zzpictureBox6
            // 
            this.zzpictureBox6.Image = (Image)resources.GetObject("zzpictureBox6.Image");
            this.zzpictureBox6.Location = new Point(3, 3);
            this.zzpictureBox6.Name = "zzpictureBox6";
            this.zzpictureBox6.Size = new Size(41, 41);
            this.zzpictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
            this.zzpictureBox6.TabIndex = 0;
            this.zzpictureBox6.TabStop = false;
            // 
            // zzflowLayoutPanel6
            // 
            this.zzflowLayoutPanel6.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.zzflowLayoutPanel6.Controls.Add(this.zzlabel12);
            this.zzflowLayoutPanel6.Controls.Add(this.zzlabel13);
            this.zzflowLayoutPanel6.Location = new Point(50, 13);
            this.zzflowLayoutPanel6.Name = "zzflowLayoutPanel6";
            this.zzflowLayoutPanel6.Size = new Size(675, 21);
            this.zzflowLayoutPanel6.TabIndex = 1;
            // 
            // zzlabel12
            // 
            this.zzlabel12.AutoSize = true;
            this.zzlabel12.Dock = DockStyle.Left;
            this.zzlabel12.ForeColor = SystemColors.Control;
            this.zzlabel12.Location = new Point(3, 0);
            this.zzlabel12.Name = firework.FireworkName;
            this.zzlabel12.Size = new Size(61, 18);
            this.zzlabel12.TabIndex = 2;
            this.zzlabel12.Text = "label12";
            // 
            // zzlabel13
            // 
            this.zzlabel13.AutoSize = true;
            this.zzlabel13.Dock = DockStyle.Left;
            this.zzlabel13.Font = new Font("Verdana", 7F, FontStyle.Regular, GraphicsUnit.Point);
            this.zzlabel13.ForeColor = SystemColors.ControlDark;
            this.zzlabel13.ImageAlign = ContentAlignment.BottomCenter;
            this.zzlabel13.Location = new Point(70, 0);
            this.zzlabel13.Name = firework.ID.ToString();
            this.zzlabel13.Size = new Size(46, 18);
            this.zzlabel13.TabIndex = 3;
            this.zzlabel13.Text = "label13";
            this.zzlabel13.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // zzpanel11
            // 
            this.zzpanel11.Anchor = AnchorStyles.None;
            this.zzpanel11.BackColor = firework.Colour;
            this.zzpanel11.Location = new Point(740, 13);
            this.zzpanel11.Name = "zzpanel11";
            this.zzpanel11.Size = new Size(20, 20);
            this.zzpanel11.TabIndex = 2;
            // 
            // zzbutton4
            // 
            this.zzbutton4.FlatAppearance.BorderSize = 0;
            this.zzbutton4.FlatStyle = FlatStyle.Flat;
            this.zzbutton4.Font = new Font("Verdana", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            this.zzbutton4.ForeColor = Color.FromArgb(200, 200, 201);
            this.zzbutton4.Image = Properties.Resources.icons8_Settings_32px;
            this.zzbutton4.Location = new Point(896, 3);
            this.zzbutton4.Name = "zzbutton4";
            this.zzbutton4.Size = new Size(34, 40);
            this.zzbutton4.TabIndex = 4;
            this.zzbutton4.TextImageRelation = TextImageRelation.TextBeforeImage;
            this.zzbutton4.UseVisualStyleBackColor = true;
            // 
            // zzlabel14
            // 
            this.zzlabel14.AutoSize = true;
            this.zzlabel14.Dock = DockStyle.Left;
            this.zzlabel14.Font = new Font("Verdana", 7F, FontStyle.Regular, GraphicsUnit.Point);
            this.zzlabel14.ForeColor = SystemColors.ControlDark;
            this.zzlabel14.ImageAlign = ContentAlignment.BottomCenter;
            this.zzlabel14.Location = new Point(836, 0);
            this.zzlabel14.Name = firework.Length.ToString();
            this.zzlabel14.Size = new Size(46, 47);
            this.zzlabel14.TabIndex = 6;
            this.zzlabel14.Text = "label14";
            this.zzlabel14.TextAlign = ContentAlignment.MiddleLeft;

            this.zzpanel8.ResumeLayout(false);
            this.zzpanel8.PerformLayout();
            this.zztableLayoutPanel7.ResumeLayout(false);
            this.zztableLayoutPanel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zzpictureBox6)).EndInit();
            this.zzflowLayoutPanel6.ResumeLayout(false);
            this.zzflowLayoutPanel6.PerformLayout();
        }
        public Panel GetPanel() => zzpanel8;
    }
}
