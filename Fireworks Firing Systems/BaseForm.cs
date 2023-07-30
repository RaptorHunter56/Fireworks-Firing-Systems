using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Fireworks_Firing_Systems
{
    public partial class BaseForm : Form
    {
        public System.IO.Ports.SerialPort _serialPort;

        public BaseForm()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) => new DataBase().ShowDialog();
        private void serialPortToolStripMenuItem_Click(object sender, EventArgs e) => new SerialPort().ShowDialog();

        private void orderSettingsToolStripMenuItem_Click(object sender, EventArgs e) => new Order().ShowDialog();

        private void button1_Click(object sender, EventArgs e) => Connect_Disconnect();
        private void disConnectToolStripMenuItem_Click(object sender, EventArgs e) => Connect_Disconnect(false);

        private void Connect_Disconnect(bool connect = true)
        {
            tabControl1.Enabled = connect;
            menuStrip1.Enabled = button1.Visible = !connect;

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening to serial port :: " + ex.Message, "Error!");
                toolStripStatusLabel1.Text = "Error opening to serial port :: " + ex.Message;
            }
        }
        #region SerialPort
        private delegate void SetTextDeleg(string text);
        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(500);
            string data = _serialPort.ReadLine();
            this.BeginInvoke(new SetTextDeleg(si_DataReceived), new object[] { data });
        }
        private void si_DataReceived(string data) { richTextBox1.Text += $"{DateTime.Now} ⏩ {data.Trim()}\r\n"; }
        #endregion

    }
}
