using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static Fireworks_Firing_Systems.OrderType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using Button = System.Windows.Forms.Button;

namespace Fireworks_Firing_Systems
{
    public partial class BaseForm : Form
    {
        public System.IO.Ports.SerialPort _serialPort;
        public Dictionary<int, Tuple<Firework, bool, bool>> IgnitionPorts = new Dictionary<int, Tuple<Firework, bool, bool>>();

        public delegate void UpdateButton();
        public UpdateButton updateButton = delegate { };

        public BaseForm()
        {
            InitializeComponent();
            RefreshLabel();
            RefreshList();

            toolStripTextBox1.LostFocus += ToolStripTextBox1_LostFocus;
            UpdateOrderComboBox();
        }

        private void RefreshLabel()
        {
            label1.Text = "Serial Port: " + Properties.Settings.Default.SerialPort;
            label2.Text = "Baud Rate: " + Properties.Settings.Default.BaudRate.ToString();
        }
        private void RefreshList()
        {
            listView1.Items.Clear();
            foreach (var item in IgnitionPorts.Where(x => x.Value.Item2 && x.Value.Item3))
            {
                var temp = item.Value.Item1.CreateListViewItem();
                temp.Text = $"[{item.Key}] {temp.Text}";
                temp.Tag = item;
                temp.Group = listView1.Groups[(int)(((item.Key % 60) - 0.1) / 6)];
                listView1.Items.Add(temp);
            }
            foreach (TableLayoutPanel item in flowLayoutPanel1.Controls)
            {
                IgnitionObject @object = UpdateIgnitionObjectList((IgnitionObject)(item).Tag);

                foreach (var IgnitionPort in IgnitionPorts.Where(x => ((!x.Value.Item3 && !x.Value.Item2) || x.Value.Item3) && @object.fireworks.Keys.Contains(x.Key)))
                {
                    @object.fireworks.Remove(IgnitionPort.Key);
                }
                item.Tag = @object;
                if (!UpdateIgnitionObject(@object, item))
                    flowLayoutPanel1.Controls.Remove(item);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) => OpenForm(new DataBase(), "Data Base");
        private void serialPortToolStripMenuItem_Click(object sender, EventArgs e) => OpenForm(new SerialPort(), "Serial Port");
        private void orderSettingsToolStripMenuItem_Click(object sender, EventArgs e) => OpenForm(new Order(), "Order");

        private void button3_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = $"Opening Setup Settings";
            this.Enabled = false;
            using (var form = new Setup(IgnitionPorts))
            {
                if (form.ShowDialog() == DialogResult.OK)
                    IgnitionPorts = form.IgnitionPorts;
                RefreshList();
                updateButton();
            }
            this.Enabled = true;
            toolStripStatusLabel1.Text = $"Closed and Saved Setup Settings";
        }
        private void button4_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = $"Opening Grid Settings";
            var form = new Grid(this);
            updateButton = form.UpdateButton;
            form.unlinkForm = ClearForm;
            form.Show();
            toolStripStatusLabel1.Text = $"Closed and Saved Grid Settings";
        }
        public void ClearForm() => updateButton = delegate { };

        private void OpenForm(Form form, string name)
        {
            toolStripStatusLabel1.Text = $"Opening {name} Settings";
            this.Enabled = false;
            form.ShowDialog();
            RefreshLabel();
            this.Enabled = true;
            toolStripStatusLabel1.Text = $"Closed and Saved {name} Settings";
            UpdateOrderComboBox();
        }
        private void UpdateOrderComboBox()
        {
            comboBox1.Items.Add(new OrderRandom());
            foreach (var item in Properties.Settings.Default.Orders?.Cast<string>() ?? new List<string>())
            {
                comboBox1.Items.Add(OrderWeighted.FromJson(item));
            }
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e) => Connect_Disconnect();
        private void disConnectToolStripMenuItem_Click(object sender, EventArgs e) => Connect_Disconnect(false);
        private void BaseForm_FormClosing(object sender, FormClosingEventArgs e) => Connect_Disconnect(false);

        #region SerialPort
        private void Connect_Disconnect(bool connect = true)
        {
            tabControl1.Enabled = connect;
            toolStripMenuItem1.Enabled = serialPortToolStripMenuItem.Enabled = orderSettingsToolStripMenuItem.Enabled = groupBox1.Visible = !connect;

            try
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    _serialPort.Close();
                    toolStripStatusLabel1.Text = "Serial Port Close";
                    richTextBox1.Text += $"{DateTime.Now} ⏺ Closed\r\n";
                    menuStrip1.ContextMenuStrip = null;
                }
            }
            catch (Exception ex) { }
            if (connect)
            {
                _serialPort = new System.IO.Ports.SerialPort(Properties.Settings.Default.SerialPort, Properties.Settings.Default.BaudRate, Parity.None, 8, StopBits.One);
                _serialPort.Handshake = Handshake.None;
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                _serialPort.WriteTimeout = 500;
                try
                {
                    if (!(_serialPort.IsOpen))
                        _serialPort.Open();
                    toolStripStatusLabel1.Text = "Serial Port Open";
                    richTextBox1.Text += $"{DateTime.Now} ⏺ Opened [{Properties.Settings.Default.SerialPort} - {Properties.Settings.Default.BaudRate}]\r\n";
                    menuStrip1.ContextMenuStrip = contextMenuStrip1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening to serial port :: " + ex.Message, "Error!");
                    toolStripStatusLabel1.Text = "Error opening to serial port :: " + ex.Message;
                }
            }
        }
        private delegate void SetTextDeleg(string text);
        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(500);
            string data = _serialPort.ReadLine();
            this.BeginInvoke(new SetTextDeleg(si_DataReceived), new object[] { data });
        }
        private void si_DataReceived(string data) { richTextBox1.Text += $"{DateTime.Now} ⏩ {data.Trim()}\r\n"; }
        #endregion

        #region Fire
        public void Fire(int i)
        {
            IgnitionPorts[i] = new Tuple<Firework, bool, bool>(IgnitionPorts[i].Item1, false, IgnitionPorts[i].Item3);
            RefreshList();
            updateButton();
        }
        #endregion


        private void listView1_ItemDrag(object sender, ItemDragEventArgs e) => listView1.DoDragDrop(e.Item, DragDropEffects.All);
        private void ControlDragEnter(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Move;
        private void flowLayoutPanel1_DragDrop(object sender, DragEventArgs e)
        {
            var control = FindControlAtPoint(splitContainer1.Panel2, new Point(PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y));
            var controlAbove = FindControlAtPoint(splitContainer1.Panel2, new Point(PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y - 10));

            if (control.GetType() == flowLayoutPanel1.GetType())
            {
                var index = 0;
                IgnitionObject @object = UpdateIgnitionObjectList(new IgnitionObject());
                TableLayoutPanel layoutPanel = @object.CreateTableLayoutPanel();
                layoutPanel.ContextMenuStrip = contextMenuStrip2;


                foreach (var button in layoutPanel.Controls.OfType<FlowLayoutPanel>().First().Controls.OfType<Button>())
                {
                    button.ContextMenuStrip = contextMenuStrip2;
                }

                layoutPanel.DragDrop += new DragEventHandler(control_DragDrop);
                layoutPanel.DragEnter += new DragEventHandler(ControlDragEnter);

                flowLayoutPanel1.Controls.Add(layoutPanel);

                if (controlAbove.GetType() != flowLayoutPanel1.GetType())
                    flowLayoutPanel1.Controls.SetChildIndex(layoutPanel, flowLayoutPanel1.Controls.IndexOf(controlAbove) + 1);

                ResizeButtons();
            }
        }
        private void control_DragDrop(object sender, DragEventArgs e)
        {
            IgnitionObject @object = UpdateIgnitionObjectList((IgnitionObject)((TableLayoutPanel)sender).Tag);
            UpdateIgnitionObject(@object, sender);
        }

        private bool UpdateIgnitionObject(IgnitionObject @object, object sender)
        {
            try
            {
                TableLayoutPanel layoutPanel = @object.CreateTableLayoutPanel();

                layoutPanel.DragDrop += new DragEventHandler(control_DragDrop);
                layoutPanel.DragEnter += new DragEventHandler(ControlDragEnter);

                ((TableLayoutPanel)sender).Controls.Clear();
                ((TableLayoutPanel)sender).Controls.Add(layoutPanel.Controls[0], 0, 0);
                ((TableLayoutPanel)sender).Controls.Add(layoutPanel.Controls[0], 0, 1);
                ((TableLayoutPanel)sender).Controls.Add(layoutPanel.Controls[0], 0, 2);
                ((TableLayoutPanel)sender).ContextMenuStrip = contextMenuStrip2;

                foreach (var button in ((TableLayoutPanel)sender).Controls.OfType<FlowLayoutPanel>().First().Controls.OfType<Button>())
                {
                    button.ContextMenuStrip = contextMenuStrip2;
                }

                ResizeButtons();
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
        private IgnitionObject UpdateIgnitionObjectList(IgnitionObject @object)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                @object.fireworks.Add(((KeyValuePair<int, Tuple<Firework, bool, bool>>)item.Tag).Key, ((KeyValuePair<int, Tuple<Firework, bool, bool>>)item.Tag).Value.Item1);
                listView1.Items.Remove(item);
                IgnitionPorts[((KeyValuePair<int, Tuple<Firework, bool, bool>>)item.Tag).Key] = new Tuple<Firework, bool, bool>(
                    IgnitionPorts[((KeyValuePair<int, Tuple<Firework, bool, bool>>)item.Tag).Key].Item1,
                    IgnitionPorts[((KeyValuePair<int, Tuple<Firework, bool, bool>>)item.Tag).Key].Item2,
                    false);
            }
            return @object;
        }
        public static Control FindControlAtPoint(Control container, Point pos)
        {
            Control child;
            foreach (Control c in container.Controls)
            {
                var cPoint = c.FindForm().PointToClient(c.Parent.PointToScreen(c.Location));
                var posPoint = new Point(pos.X - cPoint.X, pos.Y - cPoint.Y);
                if (c.Visible && c.Bounds.Contains(posPoint))
                {
                    child = c.GetChildAtPoint(new Point(posPoint.X - c.Left, posPoint.Y - c.Top));
                    return (child == null) ? c : child;
                }
            }
            return null;
        }

        private void ResizeButtons()
        {
            foreach (TableLayoutPanel tableLayoutPanel in flowLayoutPanel1.Controls.OfType<TableLayoutPanel>())
            {
                tableLayoutPanel.MinimumSize = tableLayoutPanel.Size = new Size(flowLayoutPanel1.Size.Width - 38, 0);
                foreach (var button in tableLayoutPanel.Controls.OfType<FlowLayoutPanel>().First().Controls.OfType<Button>())
                {
                    button.MinimumSize = button.Size = new Size(flowLayoutPanel1.Size.Width - 38, button.Size.Height);
                }
            }
        }


        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if (((ContextMenuStrip)sender).SourceControl.GetType() == typeof(Button))
            {
                addDelayToolStripMenuItem.Visible = minusDelayToolStripMenuItem.Visible = toolStripTextBox1.Visible = toolStripSeparator1.Visible = false;
                deleateToolStripMenuItem.Text = "Remove";
            }
            else
            {
                addDelayToolStripMenuItem.Visible = minusDelayToolStripMenuItem.Visible = toolStripTextBox1.Visible = toolStripSeparator1.Visible = true;
                deleateToolStripMenuItem.Text = "Remove All";
                IgnitionObject tag = (IgnitionObject)((TableLayoutPanel)((ContextMenuStrip)sender).SourceControl).Tag;
                if (tag.ExtraDelay == null)
                {
                    addDelayToolStripMenuItem.Enabled = true;
                    minusDelayToolStripMenuItem.Enabled = toolStripTextBox1.Enabled = false;
                    toolStripTextBox1.Text = string.Empty;
                }
                else
                {
                    addDelayToolStripMenuItem.Enabled = false;
                    minusDelayToolStripMenuItem.Enabled = toolStripTextBox1.Enabled = true;
                    toolStripTextBox1.Text = tag.ExtraDelay.ToString();
                }
            }
        }
        private void deleateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.GetType() == typeof(Button))
            {
                KeyValuePair<int, Firework> tag = (KeyValuePair<int, Firework>)((Button)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl).Tag;
                IgnitionPorts[tag.Key] = new Tuple<Firework, bool, bool>(IgnitionPorts[tag.Key].Item1, IgnitionPorts[tag.Key].Item2, true);
            }
            else
            {
                IgnitionObject tag = (IgnitionObject)((TableLayoutPanel)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl).Tag;
                foreach (var item in tag.fireworks)
                {
                    IgnitionPorts[item.Key] = new Tuple<Firework, bool, bool>(IgnitionPorts[item.Key].Item1, IgnitionPorts[item.Key].Item2, true);
                }
                flowLayoutPanel1.Controls.Remove(((TableLayoutPanel)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl));
            }
            RefreshList();
            updateButton();
        }
        private void addDelayToolStripMenuItem_Click(object sender, EventArgs e) => ((IgnitionObject)((TableLayoutPanel)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl).Tag).ExtraDelay = 0;
        private void minusDelayToolStripMenuItem_Click(object sender, EventArgs e) => ((IgnitionObject)((TableLayoutPanel)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl).Tag).ExtraDelay = null;
        private void ToolStripTextBox1_LostFocus(object sender, EventArgs e)
        {
            ((IgnitionObject)((TableLayoutPanel)((ContextMenuStrip)((ToolStripTextBox)sender).Owner).SourceControl).Tag).ExtraDelay = Convert.ToDecimal(toolStripTextBox1.Text);
            RefreshList();
            updateButton();
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
                e.Handled = true;
            if ((e.KeyChar == '.') && (((ToolStripTextBox)sender).Text.IndexOf('.') > -1))
                e.Handled = true;
            if ((e.KeyChar == '-') && (((ToolStripTextBox)sender).Text.IndexOf('-') > -1))
                e.Handled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Dictionary<int, IgnitionObject> ignitionObjects = flowLayoutPanel1.Controls.Cast<TableLayoutPanel>().ToList().Select(x => (IgnitionObject)x.Tag).Select((s, i) => new { s, i }).ToDictionary(x => x.i, x => x.s);
            ((OrderType)comboBox1.SelectedItem).Order(ref ignitionObjects);
            foreach (var item in ignitionObjects)
            {
                flowLayoutPanel1.Controls.SetChildIndex((Control)flowLayoutPanel1.Controls.Cast<TableLayoutPanel>().ToList().Where(x => x.Tag == item.Value).First(), item.Key);
            }
        }
    }
}
