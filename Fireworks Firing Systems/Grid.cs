using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            if (BaseForm.IgnitionPorts.Where(x => x.Key == i).Count() > 0)
            {
                button.Enabled = true;
                if (BaseForm.IgnitionPorts.First(x => x.Key == i).Value.Item2)
                {
                    button.BackColor = Color.FromArgb(247, 232, 225); 
                    button.ImageIndex = 1;
                }
                else
                {

                    button.BackColor = Color.FromArgb(239, 232, 225);
                    button.ImageIndex = 2;
                }
            }
            else
            {
                button.Enabled = false;
                button.ImageIndex = 0;
                button.BackColor = Color.FromArgb(225, 225, 225);
            }
            button.ImageList = imageList1;
            button.Name = "button" + i.ToString();
            button.Size = new Size(75, 75);
            button.TabIndex = i;
            button.Text = i.ToString();
            button.TextImageRelation = TextImageRelation.TextAboveImage;
            button.UseVisualStyleBackColor = false;
            button.Click += button_Click;
            flowLayoutPanel1.Controls.Add(button);
        }

        private void button_Click(object sender, EventArgs e)
        {

        }
    }
}
