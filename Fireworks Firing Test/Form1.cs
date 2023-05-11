using Fireworks_Firing_Systems;
using System.Collections;

namespace Fireworks_Firing_Test
{
    public partial class Form1 : Form
    {
        List<Firework> Fierworks = new List<Firework>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e) => listBox2.Items.Remove(listBox2.SelectedItem);

        private void button2_Click(object sender, EventArgs e) => listBox2.Items.Add(listBox1.SelectedItem);
        private void button4_Click(object sender, EventArgs e) => checkedListBox1.Items.AddRange(listBox2.Items);

        private void button3_Click(object sender, EventArgs e)
        {
            listBox2.Items.Cast<Firework>().ToList().Select((x, i) => new KeyValuePair<int, Firework>(i, x));
        }

        private void timer1_Tick(object sender, EventArgs e) => progressBar1.Value = (progressBar1.Value == 100) ? 0 : progressBar1.Value + 1;

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }
    }
}