using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Fireworks_Firing_Systems
{
    public partial class SerialPort : Form
    {
        public System.IO.Ports.SerialPort _serialPort;

        public SerialPort()
        {
            InitializeComponent();
            foreach (string type in Port) { comboBox1.Items.Add(type); }
            comboBox1.SelectedItem = (Properties.Settings.Default.SerialPort.Length > 0) ? Properties.Settings.Default.SerialPort : "COM4";
            foreach (int type in BaudRate) { comboBox2.Items.Add(type); }
            comboBox2.SelectedItem = (Properties.Settings.Default.BaudRate > 0) ? Properties.Settings.Default.BaudRate : 9600;

            UpdateSerialList();
        }

        private void UpdateSerialList()
        {
            treeView1.Nodes.Clear();
            var instances = new ManagementClass("Win32_SerialPort").GetInstances();
            foreach (ManagementObject port in instances)
            {
                treeView1.Nodes.Add($"{port["deviceid"]}:");
                treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add($"{port["name"]}");
            }
            treeView1.ExpandAll();
        }

        public static string[] Port = new[] { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10" };
        public static int[] BaudRate = new[] { 75, 110, 134, 150, 300, 600, 1200, 1800, 2400, 4800, 7200, 9600, 14400, 19200, 38400, 57600, 115200, 128000 };


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    _serialPort.Close();
                    toolStripStatusLabel1.Text = "Serial Port Close";
                    richTextBox1.Text += $"{DateTime.Now} ⏺ Closed\r\n";
                }
            }
            catch (Exception ex) { }
            _serialPort = new System.IO.Ports.SerialPort((string)comboBox1.SelectedItem, (int)comboBox2.SelectedItem, Parity.None, 8, StopBits.One);
            Properties.Settings.Default.SerialPort = (string)comboBox1.SelectedItem;
            Properties.Settings.Default.BaudRate = (int)comboBox2.SelectedItem;
            Properties.Settings.Default.Save();
            _serialPort.Handshake = Handshake.None;
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            _serialPort.WriteTimeout = 500;
            try
            {
                if (!(_serialPort.IsOpen))
                    _serialPort.Open();
                toolStripStatusLabel1.Text = "Serial Port Open";
                richTextBox1.Text += $"{DateTime.Now} ⏺ Opened [{comboBox1.Text} - {comboBox2.Text}]\r\n";
                button2.Enabled = button3.Enabled = textBox1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening to serial port :: " + ex.Message, "Error!");
                toolStripStatusLabel1.Text = "Error opening to serial port :: " + ex.Message;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                _serialPort.Write("Test\r\n");
                toolStripStatusLabel1.Text = "Test Sent";
                richTextBox1.Text += $"{DateTime.Now} ⏪ Test\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error writing to serial port :: " + ex.Message, "Error!");
                toolStripStatusLabel1.Text = "Error writing to serial port :: " + ex.Message;
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

        private void SerialPort_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    _serialPort.Close();
                    toolStripStatusLabel1.Text = "Serial Port Close";
                    richTextBox1.Text += $"{DateTime.Now} ⏺ Closed\r\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error closeing serial port :: " + ex.Message, "Error!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                _serialPort.Write($"{textBox1.Text}\r\n");
                toolStripStatusLabel1.Text = $"'{textBox1.Text}' Sent";
                richTextBox1.Text += $"{DateTime.Now} ⏪ {textBox1.Text}\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error writing to serial port :: " + ex.Message, "Error!");
                toolStripStatusLabel1.Text = "Error writing to serial port :: " + ex.Message;
            }
        }

        private void regreshToolStripMenuItem_Click(object sender, EventArgs e) => UpdateSerialList();
    }
}
