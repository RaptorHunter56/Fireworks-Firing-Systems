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
    public partial class Setup : Form
    {
        public List<Firework> fireworks = new List<Firework>();
        public Dictionary<int, Tuple<Firework, bool>> IgnitionPorts = new Dictionary<int, Tuple<Firework, bool>>();
        public Setup()
        {
            InitializeComponent();
            #region comboBox.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            comboBox7.SelectedIndex = 0;
            comboBox8.SelectedIndex = 0;
            #endregion
        }

        private void Setup_Load(object sender, EventArgs e)
        {
            var folder = (Properties.Settings.Default.DataBaseLocatrion.Length > 0) ? Properties.Settings.Default.DataBaseLocatrion : System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            foreach (var item in Directory.GetFiles(folder, "*.Firework", SearchOption.AllDirectories))
            {
                fireworks.Add(Firework.FromJson(File.ReadAllText(item)));
                #region comboBox.Items.Add(fireworks[^1]);
                comboBox2.Items.Add(fireworks[^1]);
                comboBox3.Items.Add(fireworks[^1]);
                comboBox4.Items.Add(fireworks[^1]);
                comboBox5.Items.Add(fireworks[^1]);
                comboBox6.Items.Add(fireworks[^1]);
                comboBox7.Items.Add(fireworks[^1]);
                comboBox8.Items.Add(fireworks[^1]);
                #endregion
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.OfType<TableLayoutPanel>().SelectMany(x => x.Controls.OfType<Panel>()).First(x => x.Tag == ((ComboBox)sender).Tag).BackColor = ((ComboBox)sender).SelectedIndex <= 0 ? SystemColors.ControlDark : Color.LightGreen;
            if (((ComboBox)sender).SelectedIndex > 0)
                IgnitionPorts[Int32.Parse(((ComboBox)sender).Tag.ToString()) + (Int32.Parse(button2.Tag.ToString()) * 7 - 7)] = new Tuple<Firework, bool>((Firework)((ComboBox)sender).SelectedItem, true);
            else
                IgnitionPorts.Remove(Int32.Parse(((ComboBox)sender).Tag.ToString()) + (Int32.Parse(button2.Tag.ToString()) * 7 - 7));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2.Tag = Int32.Parse(button2.Tag.ToString()) + 1;
            UpdateButtons();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Tag = Int32.Parse(button2.Tag.ToString()) - 1;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            string id = button2.Tag.ToString();
            button2.Text = $"{Int32.Parse(id) * 7 - 6} / 56";
            button1.Enabled = (Int32.Parse(id) != 1);
            button3.Enabled = (Int32.Parse(id) != 8);
            #region label.Text = (Int32.Parse(button2.Tag.ToString()) * 7 - n).ToString();
            label2.Text = (Int32.Parse(id) * 7 - 6).ToString();
            label3.Text = (Int32.Parse(id) * 7 - 5).ToString();
            label4.Text = (Int32.Parse(id) * 7 - 4).ToString();
            label5.Text = (Int32.Parse(id) * 7 - 3).ToString();
            label6.Text = (Int32.Parse(id) * 7 - 2).ToString();
            label7.Text = (Int32.Parse(id) * 7 - 1).ToString();
            label8.Text = (Int32.Parse(id) * 7).ToString();
            #endregion
            #region if (IgnitionPorts.ContainsKey(Int32.Parse(id) * 7 - n)) comboBox.SelectedItem = IgnitionPorts[Int32.Parse(id) * 7 - n].Item1;
            if (IgnitionPorts.ContainsKey(Int32.Parse(id) * 7 - 6)) comboBox2.SelectedItem = IgnitionPorts[Int32.Parse(id) * 7 - 6].Item1;
            else comboBox2.SelectedItem = "None";
            if (IgnitionPorts.ContainsKey(Int32.Parse(id) * 7 - 5)) comboBox3.SelectedItem = IgnitionPorts[Int32.Parse(id) * 7 - 5].Item1;
            else comboBox3.SelectedItem = "None";
            if (IgnitionPorts.ContainsKey(Int32.Parse(id) * 7 - 4)) comboBox4.SelectedItem = IgnitionPorts[Int32.Parse(id) * 7 - 4].Item1;
            else comboBox4.SelectedItem = "None";
            if (IgnitionPorts.ContainsKey(Int32.Parse(id) * 7 - 3)) comboBox5.SelectedItem = IgnitionPorts[Int32.Parse(id) * 7 - 3].Item1;
            else comboBox5.SelectedItem = "None";
            if (IgnitionPorts.ContainsKey(Int32.Parse(id) * 7 - 2)) comboBox6.SelectedItem = IgnitionPorts[Int32.Parse(id) * 7 - 2].Item1;
            else comboBox6.SelectedItem = "None";
            if (IgnitionPorts.ContainsKey(Int32.Parse(id) * 7 - 1)) comboBox7.SelectedItem = IgnitionPorts[Int32.Parse(id) * 7 - 1].Item1;
            else comboBox7.SelectedItem = "None";
            if (IgnitionPorts.ContainsKey(Int32.Parse(id) * 7)) comboBox8.SelectedItem = IgnitionPorts[Int32.Parse(id) * 7]?.Item1;
            else comboBox8.SelectedItem = "None";
            #endregion
        }
    }
}
