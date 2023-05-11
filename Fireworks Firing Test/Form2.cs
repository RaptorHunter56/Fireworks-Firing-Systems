using Fireworks_Firing_Systems;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fireworks_Firing_Test
{
    public partial class Form2 : Form
    {
        public List<Firework> Fireworks { get; set; }
        public Form2()
        {
            InitializeComponent();
            Fireworks = new List<Firework>();
            folderBrowserDialog1.SelectedPath = Properties.Settings.Default.FileDirectory ?? AppDomain.CurrentDomain.BaseDirectory;
            textBox1.Text = AppDomain.CurrentDomain.BaseDirectory;
            OpenFiles();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
            OpenFiles();
        }

        private void OpenFiles()
        {
            Properties.Settings.Default.FileDirectory = textBox1.Text;
            Properties.Settings.Default.Save();
            foreach (var file in Directory.EnumerateFiles(textBox1.Text, "*.firework"))
            {
                var t = File.ReadAllText(file);
                Fireworks.Add(new Firework(t));
            }
            listBox1.Items.Clear();
            listBox1.Items.AddRange(Fireworks.ToArray());
        }

        private void textBox1_Leave(object sender, EventArgs e) => OpenFiles();

        private void button2_Click(object sender, EventArgs e)
        {
            Fireworks.Add(new Firework(Int32.Parse(Interaction.InputBox("ID")), Interaction.InputBox("Name"), 0, Color.White, 0, 0));
            listBox1.Items.Clear();
            listBox1.Items.AddRange(Fireworks.ToArray());
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (Firework item in listBox1.Items)
            {
                File.WriteAllText($@"{textBox1.Text}\{item.ID}.firework", item.ToJson());
            }
            this.Close();
        }
    }
}
