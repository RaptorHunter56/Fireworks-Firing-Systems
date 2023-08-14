using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

namespace Fireworks_Firing_Systems
{
    public partial class Grid : Form
    {
        public BaseForm BaseForm { get; }

        public Grid(BaseForm baseForm)
        {
            BaseForm = baseForm;
            InitializeComponent();
            for (int i = 1; i < 57; i++) // 1
            {
                AddButton(i);
            }
        }

        public void AddButton(int i)
        {
            Button button = new Button();
            PictureBox pictureBox = new PictureBox();
            Label label = new Label();

            if (BaseForm.IgnitionPorts.Where(x => x.Key == i).Count() > 0)
            {
                button.Enabled = true;
                if (BaseForm.IgnitionPorts.First(x => x.Key == i).Value.Item2)
                    pictureBox.Image = imageList1.Images[2];
                else
                    pictureBox.Image = imageList1.Images[1];
            }
            else
                button.Enabled = false;
            button.ImageIndex = 0;
            button.ImageList = imageList1;
            button.Name = "button" + i.ToString();
            button.Size = new Size(75, 75);
            button.TabIndex = i;
            button.UseVisualStyleBackColor = false;

            pictureBox.Location = new Point(0, 0);
            pictureBox.Name = "pictureBox" + i.ToString();
            pictureBox.Size = new Size(75, 75);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = i;
            pictureBox.TabStop = false;

            label.BackColor = Color.Transparent;
            label.Dock = DockStyle.Fill;
            label.FlatStyle = FlatStyle.Flat;
            label.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label.ForeColor = SystemColors.ButtonHighlight;
            label.ImageKey = "(none)";
            label.Location = new Point(0, 0);
            label.Name = "label" + i.ToString();
            label.Size = new Size(75, 75);
            label.TabIndex = i;
            label.Text = i.ToString();
            label.TextAlign = ContentAlignment.MiddleCenter;


            pictureBox.Controls.Add(label);
            button.Controls.Add(pictureBox);
            flowLayoutPanel1.Controls.Add(button);
        }
    }
}
