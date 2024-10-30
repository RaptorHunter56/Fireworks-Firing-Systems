using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace Fireworks_Firing_Systems
{
    public partial class SatelliteSettings : Form
    {
        public BaseForm BaseForm { get; }

        public SatelliteSettings(BaseForm baseForm)
        {
            InitializeComponent();
            BaseForm = baseForm;
            BaseForm._serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_SatelliteDataReceived);
            CallSatellites("Satellite Check In");
        }
        private void SatelliteSettings_FormClosing(object sender, FormClosingEventArgs e) => BaseForm._serialPort.DataReceived -= new SerialDataReceivedEventHandler(sp_SatelliteDataReceived);

        private void button1_Click(object sender, EventArgs e) => CallSatellites("Satellite Check In");
        public void CallSatellites(string text)
        {
            richTextBox1.Text += $"{DateTime.Now} ⏪ {text}\r\n";
            treeView1.Nodes.Clear();
            BaseForm.SendText(text);
        }

        private delegate void SetTextDeleg(string text);
        void sp_SatelliteDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(100);
            string data = BaseForm._serialPort.ReadLine();
            this.BeginInvoke(new SetTextDeleg(si_DataReceived), new object[] { data });
        }
        private void si_DataReceived(string data)
        {
            richTextBox1.Text += $"{DateTime.Now} ⏩ {data.Trim()}\r\n";
            Regex CheckIn = new Regex(@"\[ID\: [0-9]+\]");
            switch (data.Trim())
            {
                case var someVal when CheckIn.IsMatch(someVal):
                    richTextBox1.Text += $"{new string(' ', $"{DateTime.Now} ".Length)}🔽 Checking In...\r\n";
                    var match = CheckIn.Match(someVal);
                    treeView1.Nodes.Add(match.Groups[0].Value);
                    CallSatellites($"[{match.Groups[0].Value}] Get All Settings");
                    break;
                default:
                    richTextBox1.Text += $"{new string(' ', $"{DateTime.Now} ".Length)}🔽 Unknown command received...\r\n";
                    break;
            }
        }
    }
}
