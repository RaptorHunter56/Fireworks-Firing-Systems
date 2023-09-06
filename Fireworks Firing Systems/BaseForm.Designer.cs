namespace Fireworks_Firing_Systems
{
    partial class BaseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            ListViewGroup listViewGroup1 = new ListViewGroup("1-6", HorizontalAlignment.Left);
            ListViewGroup listViewGroup2 = new ListViewGroup("7-12", HorizontalAlignment.Left);
            ListViewGroup listViewGroup3 = new ListViewGroup("13-18", HorizontalAlignment.Left);
            ListViewGroup listViewGroup4 = new ListViewGroup("19-24", HorizontalAlignment.Left);
            ListViewGroup listViewGroup5 = new ListViewGroup("25-30", HorizontalAlignment.Left);
            ListViewGroup listViewGroup6 = new ListViewGroup("31-36", HorizontalAlignment.Left);
            ListViewGroup listViewGroup7 = new ListViewGroup("37-42", HorizontalAlignment.Left);
            ListViewGroup listViewGroup8 = new ListViewGroup("43-48", HorizontalAlignment.Left);
            ListViewGroup listViewGroup9 = new ListViewGroup("49-54", HorizontalAlignment.Left);
            ListViewGroup listViewGroup10 = new ListViewGroup("55-60", HorizontalAlignment.Left);
            ListViewItem listViewItem1 = new ListViewItem(new string[] { "Name", "Color", "Fierwok" }, -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseForm));
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            serialPortToolStripMenuItem = new ToolStripMenuItem();
            orderSettingsToolStripMenuItem = new ToolStripMenuItem();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            splitContainer1 = new SplitContainer();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            button4 = new Button();
            button3 = new Button();
            tabPage2 = new TabPage();
            button2 = new Button();
            textBox1 = new TextBox();
            richTextBox1 = new RichTextBox();
            groupBox1 = new GroupBox();
            label2 = new Label();
            label1 = new Label();
            button1 = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            disConnectToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 577);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(962, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(0, 17);
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, serialPortToolStripMenuItem, orderSettingsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(962, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(70, 20);
            toolStripMenuItem1.Text = "Data Base";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // serialPortToolStripMenuItem
            // 
            serialPortToolStripMenuItem.Name = "serialPortToolStripMenuItem";
            serialPortToolStripMenuItem.Size = new Size(72, 20);
            serialPortToolStripMenuItem.Text = "Serial Port";
            serialPortToolStripMenuItem.Click += serialPortToolStripMenuItem_Click;
            // 
            // orderSettingsToolStripMenuItem
            // 
            orderSettingsToolStripMenuItem.Name = "orderSettingsToolStripMenuItem";
            orderSettingsToolStripMenuItem.Size = new Size(94, 20);
            orderSettingsToolStripMenuItem.Text = "Order Settings";
            orderSettingsToolStripMenuItem.Click += orderSettingsToolStripMenuItem_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Enabled = false;
            tabControl1.Location = new Point(0, 24);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(962, 553);
            tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(splitContainer1);
            tabPage1.Controls.Add(button4);
            tabPage1.Controls.Add(button3);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(954, 525);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Setup";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.BorderStyle = BorderStyle.FixedSingle;
            splitContainer1.Location = new Point(89, 6);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listView1);
            splitContainer1.Size = new Size(857, 516);
            splitContainer1.SplitterDistance = 236;
            splitContainer1.TabIndex = 3;
            // 
            // listView1
            // 
            listView1.BackColor = SystemColors.ControlLightLight;
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            listView1.Dock = DockStyle.Fill;
            listView1.FullRowSelect = true;
            listViewGroup1.Header = "1-6";
            listViewGroup1.Name = "1-6";
            listViewGroup2.Header = "7-12";
            listViewGroup2.Name = "7-12";
            listViewGroup3.Header = "13-18";
            listViewGroup3.Name = "13-18";
            listViewGroup4.Header = "19-24";
            listViewGroup4.Name = "19-24";
            listViewGroup5.Header = "25-30";
            listViewGroup5.Name = "25-30";
            listViewGroup6.Header = "31-36";
            listViewGroup6.Name = "31-36";
            listViewGroup7.Header = "37-42";
            listViewGroup7.Name = "37-42";
            listViewGroup8.Header = "43-48";
            listViewGroup8.Name = "43-48";
            listViewGroup9.Header = "49-54";
            listViewGroup9.Name = "49-54";
            listViewGroup10.Header = "55-60";
            listViewGroup10.Name = "55-60";
            listView1.Groups.AddRange(new ListViewGroup[] { listViewGroup1, listViewGroup2, listViewGroup3, listViewGroup4, listViewGroup5, listViewGroup6, listViewGroup7, listViewGroup8, listViewGroup9, listViewGroup10 });
            listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listViewItem1.Group = listViewGroup1;
            listView1.Items.AddRange(new ListViewItem[] { listViewItem1 });
            listView1.Location = new Point(0, 0);
            listView1.Name = "listView1";
            listView1.Size = new Size(234, 514);
            listView1.Sorting = SortOrder.Ascending;
            listView1.TabIndex = 2;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Tile;
            // 
            // button4
            // 
            button4.Location = new Point(8, 35);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 1;
            button4.Text = "Grid";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.Location = new Point(8, 64);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 0;
            button3.Text = "Set Up";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(button2);
            tabPage2.Controls.Add(textBox1);
            tabPage2.Controls.Add(richTextBox1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(954, 525);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Console";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button2.Location = new Point(848, 499);
            button2.Name = "button2";
            button2.Size = new Size(98, 23);
            button2.TabIndex = 21;
            button2.Text = "Send";
            button2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(8, 499);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(834, 23);
            textBox1.TabIndex = 20;
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox1.BackColor = SystemColors.WindowFrame;
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.ForeColor = SystemColors.Window;
            richTextBox1.Location = new Point(8, 6);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            richTextBox1.Size = new Size(938, 487);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // groupBox1
            // 
            groupBox1.BackColor = SystemColors.ControlLightLight;
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(button1);
            groupBox1.Location = new Point(410, 220);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(135, 85);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 34);
            label2.Name = "label2";
            label2.Size = new Size(66, 15);
            label2.TabIndex = 6;
            label2.Text = "Baud Rate: ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 19);
            label1.Name = "label1";
            label1.Size = new Size(66, 15);
            label1.TabIndex = 5;
            label1.Text = "Serial Port: ";
            // 
            // button1
            // 
            button1.Location = new Point(29, 52);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 4;
            button1.Text = "Connect";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { disConnectToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(134, 26);
            // 
            // disConnectToolStripMenuItem
            // 
            disConnectToolStripMenuItem.Name = "disConnectToolStripMenuItem";
            disConnectToolStripMenuItem.Size = new Size(133, 22);
            disConnectToolStripMenuItem.Text = "Disconnect";
            disConnectToolStripMenuItem.Click += disConnectToolStripMenuItem_Click;
            // 
            // BaseForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(962, 599);
            Controls.Add(groupBox1);
            Controls.Add(tabControl1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "BaseForm";
            Text = "BaseForm";
            FormClosing += BaseForm_FormClosing;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem serialPortToolStripMenuItem;
        private ToolStripMenuItem orderSettingsToolStripMenuItem;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button button1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem disConnectToolStripMenuItem;
        private RichTextBox richTextBox1;
        private Button button2;
        private TextBox textBox1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Button button3;
        private Button button4;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private SplitContainer splitContainer1;
    }
}