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
    public partial class DataBase : Form
    {
        public DataBase()
        {
            InitializeComponent();
            foreach (FireworkType type in (FireworkType[])Enum.GetValues(typeof(FireworkType))) { comboBox1.Items.Add(type); }
            folderBrowserDialog1.SelectedPath = textBox3.Text = (Properties.Settings.Default.DataBaseLocatrion.Length > 0) ? Properties.Settings.Default.DataBaseLocatrion : System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            bindingSource1.Add(new Firework(1, "Test", FireworkType.Missile, Color.Blue, 0, 0));
            bindingSource1.Add(new Firework(2, "Test", FireworkType.Missile, Color.DarkRed, 0, 0));
            Firework.DataGridViewSetup(dataGridView1);
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSize = true;
            dataGridView1.DataSource = bindingSource1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            textBox2.Text = colorDialog1.Color.ToString();
            panel1.BackColor = colorDialog1.Color;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox3.Text = Properties.Settings.Default.DataBaseLocatrion = folderBrowserDialog1.SelectedPath;
            Properties.Settings.Default.Save();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].Tag == "Color")
            {
                colorDialog1.ShowDialog();
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = colorDialog1.Color;
                e.Cancel = true;
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["🎨"].Style.BackColor = (Color)row.Cells["Colour"].Value;
            }
        }
    }
}
