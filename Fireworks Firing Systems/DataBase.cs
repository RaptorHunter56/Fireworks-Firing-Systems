using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            RefreshDataGrid();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            bindingSource1.Add(new Firework((int)numericUpDown1.Value, textBox1.Text, (FireworkType)comboBox1.SelectedItem, panel1.BackColor, numericUpDown2.Value, numericUpDown3.Value));
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
                row.Cells["🎨"].Style.BackColor = row.Cells["🎨"].Style.SelectionBackColor = (Color)row.Cells["Colour"].Value;
            }
        }
        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var folder = (Properties.Settings.Default.DataBaseLocatrion.Length > 0) ? Properties.Settings.Default.DataBaseLocatrion : System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var path = $"{folder}\\{e.Row.Cells[0].Value}.Firework";
            File.Delete(path);
        }

        private void DataBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Firework item in bindingSource1)
            {
                var folder = (Properties.Settings.Default.DataBaseLocatrion.Length > 0) ? Properties.Settings.Default.DataBaseLocatrion : System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var path = $"{folder}\\{item.ID}.Firework";
                File.WriteAllText(path, item.ToJson());
            }
        }
        private void DataBase_Load(object sender, EventArgs e) => RefreshDataGrid();

        private void RefreshDataGrid()
        {
            var folder = (Properties.Settings.Default.DataBaseLocatrion.Length > 0) ? Properties.Settings.Default.DataBaseLocatrion : System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            bindingSource1.Clear();
            foreach (var item in Directory.GetFiles(folder, "*.Firework", SearchOption.AllDirectories))
            {
                bindingSource1.Add(Firework.FromJson(File.ReadAllText(item)));
            }
        }
    }
}
