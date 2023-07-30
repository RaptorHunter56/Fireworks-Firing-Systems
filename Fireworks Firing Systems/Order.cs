using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static Fireworks_Firing_Systems.OrderType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using TrackBar = System.Windows.Forms.TrackBar;

namespace Fireworks_Firing_Systems
{
    public partial class Order : Form
    {
        public Order()
        {
            InitializeComponent();

            treeView1.Nodes.Add("Order - Random", "Order - Random");
            foreach (var item in Properties.Settings.Default.Orders?.Cast<string>() ?? new List<string>())
            {
                treeView1.Nodes.Add(OrderWeighted.FromJson(item).ToTreeNode());
                treeView1.Nodes[treeView1.Nodes.Count - 1].Tag = "Order - Weighted";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Add(new OrderWeighted().ToTreeNode());
            treeView1.Nodes[treeView1.Nodes.Count - 1].Tag = "Order - Weighted";
            treeView1.SelectedNode = treeView1.Nodes[treeView1.Nodes.Count - 1];
            ToggleEnabled(treeView1.SelectedNode);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag?.ToString() == "Order - Weighted")
            {
                treeView1.SelectedNode.Text = $"Order - {((textBox1.Text.Length > 0) ? textBox1.Text : "Weighted")}";
                foreach (var item in groupBox2.Controls.OfType<TrackBar>())
                {
                    treeView1.SelectedNode.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text.Contains((string)item.Tag)).Text = $"{item.Tag} - {item.Value}";
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Remove(treeView1.SelectedNode);
            treeView1.SelectedNode = treeView1.Nodes[treeView1.Nodes.Count - 1];
            ToggleEnabled(treeView1.SelectedNode);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ToggleEnabled(e.Node);
            if (e.Node.Tag?.ToString() == "Order - Weighted")
            {
                textBox1.Text = e.Node.Text.Split('-')[1].Trim();
                foreach (var item in groupBox2.Controls.OfType<TrackBar>())
                {
                    item.Value = int.Parse(e.Node.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text.Contains((string)item.Tag)).Text.Split('-')[1].Trim());
                }
            }
        }

        private void Order_FormClosing(object sender, FormClosingEventArgs e)
        {
            var orders = new StringCollection();
            orders.AddRange(treeView1.Nodes.OfType<TreeNode>().Where(x => x.Tag == "Order - Weighted").Select(x => OrderWeighted.FromTreeNode(x).ToJson()).ToArray());
            Properties.Settings.Default.Orders = orders;
            Properties.Settings.Default.Save();
        }

        private void ToggleEnabled(TreeNode node) => button3.Enabled = groupBox2.Enabled = (node.Tag?.ToString() == "Order - Weighted");
        private void trackBar_ValueChanged(object sender, EventArgs e) => toolTip1.SetToolTip((TrackBar)sender, ((TrackBar)sender).Value.ToString());
    }

    public abstract class OrderType
    {
        private static Random rng = new Random();
        public string Name;
        public abstract void Order(ref Dictionary<int, IgnitionObject> ignitionObjects);
        public abstract override string ToString();

        public class OrderRandom : OrderType
        {
            public new string Name = "Order - Random";
            public override void Order(ref Dictionary<int, IgnitionObject> ignitionObjects)
            {
                ignitionObjects = ignitionObjects.Values.OrderBy(a => rng.Next()).ToList().Select((s, i) => new { s, i }).ToDictionary(x => x.i, x => x.s);
            }
            public override string ToString() => Name;
        }

        public class OrderWeighted : OrderType
        {
            public Dictionary<FireworkType, int> Weighted = new Dictionary<FireworkType, int>
            {
                {FireworkType.Cake, 1 },
                {FireworkType.Fountain, 1 },
                {FireworkType.Mine, 1 },
                {FireworkType.Missile, 1 },
                {FireworkType.Roman_Candle, 1 },
                {FireworkType.Smoke_Ball, 1 },
                {FireworkType.Sparkler, 1 }
            };
            public int NullWeight => Weighted.Values.Sum() / Weighted.Count;
            public new string Name = "Order - Weighted";
            public override void Order(ref Dictionary<int, IgnitionObject> ignitionObjects) => ignitionObjects = ignitionObjects.Values.SelectMany(x => Enumerable.Repeat(x, x.fireworks.Count() == 0 ? NullWeight : x.fireworks.Select(y => y.Type).Distinct().Select(y => Weighted[y]).DefaultIfEmpty(0).Sum() / x.fireworks.Select(y => y.Type).Distinct().Count())).OrderBy(a => rng.Next()).ToList().Distinct().Select((s, i) => new { s, i }).ToDictionary(x => x.i, x => x.s);
            public override string ToString() => Name;
            public string ToJson() => Newtonsoft.Json.JsonConvert.SerializeObject(this);
            internal TreeNode ToTreeNode() => new TreeNode(Name, Weighted.Select(x => new TreeNode($"{x.Key} - {x.Value}")).ToArray());
            public static OrderWeighted FromJson(string json) => Newtonsoft.Json.JsonConvert.DeserializeObject<OrderWeighted>(json);
            internal static OrderWeighted FromTreeNode(TreeNode node) => new OrderWeighted() { Weighted = new OrderWeighted().Weighted.Select(x => new KeyValuePair<FireworkType, int>(x.Key, int.Parse(node.Nodes.OfType<TreeNode>().FirstOrDefault(y => y.Text.Contains(x.Key.ToString())).Text.Split('-')[1].Trim()))).ToDictionary(x => x.Key, x => x.Value), Name = node.Text };
        }
    }
}
