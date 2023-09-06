using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComboBox = System.Windows.Forms.ComboBox;

namespace Fireworks_Firing_Systems
{
    public partial class Setup : Form
    {
        public Dictionary<int, Tuple<Firework, bool>> IgnitionPorts = new Dictionary<int, Tuple<Firework, bool>>();
        public int buttonTag => (int)((Int32.Parse(button2.Tag.ToString()) * 6 - 5) + (60 * (numericUpDown1.Value - 1)));
        public Setup(Dictionary<int, Tuple<Firework, bool>> ignitionPorts)
        {
            IgnitionPorts = ignitionPorts;
            if (IgnitionPorts.Count > 0)
                IgnitionPorts[1] = new Tuple<Firework, bool>(IgnitionPorts[1].Item1, false);
            InitializeComponent();
        }

        private void Setup_Load(object sender, EventArgs e)
        {
            Directory.GetFiles((Properties.Settings.Default.DataBaseLocatrion.Length > 0) ? Properties.Settings.Default.DataBaseLocatrion : Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "*.Firework", SearchOption.AllDirectories).ToList()
                .ForEach(item => flowLayoutPanel1.Controls.OfType<TableLayoutPanel>().SelectMany(x => x.Controls.OfType<ComboBox>()).ToList()
                    .ForEach(x => x.Items.Add(Firework.FromJson(File.ReadAllText(item)))));
            UpdateSelectedItem(buttonTag);
        }

        private void Setup_FormClosing(object sender, FormClosingEventArgs e) => DialogResult = DialogResult.OK;

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int comboBoxTag = Int32.Parse(((ComboBox)sender).Tag.ToString()) - 1;
            if (((ComboBox)sender).SelectedIndex > 0)
            {
                IgnitionPorts[comboBoxTag + buttonTag] = new Tuple<Firework, bool>((Firework)((ComboBox)sender).SelectedItem, (IgnitionPorts.Count > comboBoxTag) ? IgnitionPorts[comboBoxTag + buttonTag].Item2 : true);
                flowLayoutPanel1.Controls.OfType<TableLayoutPanel>().SelectMany(x => x.Controls.OfType<Panel>()).First(x => x.Tag == ((ComboBox)sender).Tag).BackColor = IgnitionPorts[comboBoxTag + buttonTag].Item2 ? Color.LightGreen : Color.Red;
            }
            else
            {
                IgnitionPorts.Remove(comboBoxTag + buttonTag);
                flowLayoutPanel1.Controls.OfType<TableLayoutPanel>().SelectMany(x => x.Controls.OfType<Panel>()).First(x => x.Tag == ((ComboBox)sender).Tag).BackColor = SystemColors.ControlDark;
            }
        }
        private void comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int comboBoxTag = Int32.Parse(((ComboBox)sender).Tag.ToString()) - 1;
            if (((ComboBox)sender).SelectedIndex > 0)
            {
                IgnitionPorts[comboBoxTag + buttonTag] = new Tuple<Firework, bool>((Firework)((ComboBox)sender).SelectedItem, true);
                flowLayoutPanel1.Controls.OfType<TableLayoutPanel>().SelectMany(x => x.Controls.OfType<Panel>()).First(x => x.Tag == ((ComboBox)sender).Tag).BackColor = Color.LightGreen;
            }
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
        private void clearToolStripMenuItem_Click(object sender, EventArgs e) => flowLayoutPanel1.Controls.OfType<TableLayoutPanel>().SelectMany(x => x.Controls.OfType<ComboBox>()).First(x => x.Tag == ((dynamic)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl).Tag).SelectedIndex = 0;
        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IgnitionPorts = new Dictionary<int, Tuple<Firework, bool>>();
            flowLayoutPanel1.Controls.OfType<TableLayoutPanel>().SelectMany(x => x.Controls.OfType<ComboBox>()).ToList()
                .ForEach(x => x.SelectedIndex = 0);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) => UpdateButtons();

        private void UpdateButtons()
        {
            int id = buttonTag;
            button2.Text = $"{id} / {60 * numericUpDown1.Value}";
            button1.Enabled = (Int32.Parse(button2.Tag.ToString()) != 1);
            button3.Enabled = (Int32.Parse(button2.Tag.ToString()) != 10);
            UpdateText(id);
            UpdateSelectedItem(id);
        }

        private void UpdateText(int id) => flowLayoutPanel1.Controls.OfType<TableLayoutPanel>().SelectMany(x => x.Controls.OfType<Label>()).ToList()
            .ForEach(x => x.Text = (id + (Int32.Parse(x.Tag.ToString()) - 1)).ToString());
        private void UpdateSelectedItem(int id) => flowLayoutPanel1.Controls.OfType<TableLayoutPanel>().SelectMany(x => x.Controls.OfType<ComboBox>()).ToList()
            .ForEach(x => x.SelectedItem = IgnitionPorts.ContainsKey(id + (Int32.Parse(x.Tag.ToString()) - 1)) ? IgnitionPorts[id + (Int32.Parse(x.Tag.ToString()) - 1)].Item1 : "None");
    }
}
