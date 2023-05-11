namespace Fireworks_Firing_Test
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            listBox1 = new ListBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            toolStripMenuItem1 = new ToolStripMenuItem();
            listBox2 = new ListBox();
            button1 = new Button();
            button2 = new Button();
            groupBox1 = new GroupBox();
            button4 = new Button();
            groupBox2 = new GroupBox();
            richTextBox1 = new RichTextBox();
            button3 = new Button();
            textBox1 = new TextBox();
            timer1 = new System.Windows.Forms.Timer(components);
            propertyGrid1 = new PropertyGrid();
            progressBar1 = new ProgressBar();
            checkedListBox1 = new CheckedListBox();
            contextMenuStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.ContextMenuStrip = contextMenuStrip1;
            listBox1.Dock = DockStyle.Top;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 20;
            listBox1.Location = new Point(3, 23);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(341, 104);
            listBox1.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1 });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(211, 56);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(210, 24);
            toolStripMenuItem1.Text = "Open";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // listBox2
            // 
            listBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 20;
            listBox2.Location = new Point(6, 168);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(335, 424);
            listBox2.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(6, 133);
            button1.Name = "button1";
            button1.Size = new Size(39, 29);
            button1.TabIndex = 2;
            button1.Text = "<<";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(51, 133);
            button2.Name = "button2";
            button2.Size = new Size(39, 29);
            button2.TabIndex = 3;
            button2.Text = ">>";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            groupBox1.Controls.Add(button4);
            groupBox1.Controls.Add(listBox1);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(listBox2);
            groupBox1.Controls.Add(button1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(347, 602);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "List";
            // 
            // button4
            // 
            button4.Location = new Point(302, 133);
            button4.Name = "button4";
            button4.Size = new Size(39, 29);
            button4.TabIndex = 4;
            button4.Text = ">>";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(richTextBox1);
            groupBox2.Controls.Add(button3);
            groupBox2.Controls.Add(textBox1);
            groupBox2.Location = new Point(365, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(536, 162);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "Remote Conection";
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox1.Enabled = false;
            richTextBox1.Location = new Point(6, 56);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(524, 100);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button3.Location = new Point(436, 21);
            button3.Name = "button3";
            button3.Size = new Size(94, 29);
            button3.TabIndex = 1;
            button3.Text = "Send";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(6, 23);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(424, 27);
            textBox1.TabIndex = 0;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // propertyGrid1
            // 
            propertyGrid1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            propertyGrid1.Location = new Point(371, 352);
            propertyGrid1.Name = "propertyGrid1";
            propertyGrid1.SelectedObject = timer1;
            propertyGrid1.Size = new Size(524, 227);
            propertyGrid1.TabIndex = 6;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Location = new Point(371, 585);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(530, 29);
            progressBar1.Step = 1;
            progressBar1.TabIndex = 7;
            // 
            // checkedListBox1
            // 
            checkedListBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            checkedListBox1.ColumnWidth = 150;
            checkedListBox1.Enabled = false;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new Point(371, 180);
            checkedListBox1.MultiColumn = true;
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.SelectionMode = SelectionMode.None;
            checkedListBox1.Size = new Size(524, 202);
            checkedListBox1.TabIndex = 8;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(913, 626);
            Controls.Add(checkedListBox1);
            Controls.Add(progressBar1);
            Controls.Add(propertyGrid1);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            contextMenuStrip1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBox1;
        private ListBox listBox2;
        private Button button1;
        private Button button2;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button button3;
        private TextBox textBox1;
        private RichTextBox richTextBox1;
        private System.Windows.Forms.Timer timer1;
        private PropertyGrid propertyGrid1;
        private ProgressBar progressBar1;
        private Button button4;
        private CheckedListBox checkedListBox1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
    }
}