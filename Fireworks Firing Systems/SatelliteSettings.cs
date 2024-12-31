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
            BaseForm.portAddition += si_DataReceived;
            CallSatellites("Satellite Check In");
        }
        private void SatelliteSettings_FormClosing(object sender, FormClosingEventArgs e) => BaseForm.portAddition -= si_DataReceived;

        private void button1_Click(object sender, EventArgs e) => CallSatellites("Satellite Check In");
        public void CallSatellites(string text)
        {
            richTextBox1.Text += $"{DateTime.Now} ⏪ {text}\r\n";
            treeView1.Nodes.Clear();
            BaseForm.SendText(text);
        }

        public void si_DataReceived(string data)
        {
            richTextBox1.Text += $"{DateTime.Now} ⏩ {data.Trim()}\r\n";
            Regex CheckIn = new Regex(@"\[ID\:[0-9]+\]");
            Regex SettingsIn = new Regex(@"\[ID\:([0-9]+)\|([a-zA-Z]+):([0-9]+)\]");
            switch (data.Trim())
            {
                case var someVal when CheckIn.IsMatch(someVal):
                    richTextBox1.Text += $"{new string(' ', $"{DateTime.Now} ".Length)}🔽 Checking In...\r\n";
                    var CheckInmatch = CheckIn.Match(someVal);
                    treeView1.Nodes.Add(CheckInmatch.Groups[0].Value);
                    CallSatellites($"{CheckInmatch.Groups[0].Value} Get All Settings");
                    break;
                case var someVal when SettingsIn.IsMatch(someVal):
                    var SettingsInmatch = SettingsIn.Match(someVal);
                    TreeNode parentNode = treeView1.Nodes.Cast<TreeNode>().FirstOrDefault(node => node.Text == $"[ID: {SettingsInmatch.Groups[1].Value}]");
                    if (parentNode != null)
                        parentNode.Nodes.Add($"{SettingsInmatch.Groups[2].Value} : {SettingsInmatch.Groups[3].Value}");
                    else
                        richTextBox1.Text += $"{new string(' ', $"{DateTime.Now} ".Length)}🔽 Parent node not found for ID: {SettingsInmatch.Groups[1].Value}\r\n";
                    break;
                default:
                    richTextBox1.Text += $"{new string(' ', $"{DateTime.Now} ".Length)}🔽 Unknown command received...\r\n";
                    break;
            }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e) { richTextBox1.SelectionStart = richTextBox1.Text.Length; richTextBox1.ScrollToCaret(); }

    }
}
