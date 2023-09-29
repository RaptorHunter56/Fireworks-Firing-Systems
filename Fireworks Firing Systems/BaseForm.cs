﻿using System;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
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

                foreach (var IgnitionPort in IgnitionPorts.Where(x => !x.Value.Item3 && !x.Value.Item2 && @object.fireworks.Keys.Contains(x.Key)))
                {
                    @object.fireworks.Remove(IgnitionPort.Key);
                }
                item.Tag = @object;
                UpdateIgnitionObject(@object, item);
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

        private void UpdateIgnitionObject(IgnitionObject @object, object sender)
        {
            TableLayoutPanel layoutPanel = @object.CreateTableLayoutPanel();

            layoutPanel.DragDrop += new DragEventHandler(control_DragDrop);
            layoutPanel.DragEnter += new DragEventHandler(ControlDragEnter);

            ((TableLayoutPanel)sender).Controls.Clear();
            ((TableLayoutPanel)sender).Controls.Add(@object.CreateTableLayoutPanel().Controls[0], 0, 0);
            ((TableLayoutPanel)sender).Controls.Add(@object.CreateTableLayoutPanel().Controls[1], 0, 1);
            ((TableLayoutPanel)sender).Controls.Add(@object.CreateTableLayoutPanel().Controls[2], 0, 2);

            ResizeButtons();
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
                    if (child == null)
                        return c;
                    else
                        return child;
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

    }
}
