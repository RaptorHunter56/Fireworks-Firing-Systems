using Fireworks_Firing_Systems.Properties;
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
    public partial class GeneralSettings : Form
    {
        private MySettings settings;

        public GeneralSettings(BaseForm baseForm)
        {
            InitializeComponent();
            settings = new MySettings();
            propertyGrid1.SelectedObject = settings;
            timer1.Interval = settings.RangeDelay;
            timer1.Start();
        }

        private void GeneralSettings_FormClosing(object sender, FormClosingEventArgs e) => settings.Save();

        private void timer1_Tick(object sender, EventArgs e)
        {
            statusStrip1.BackColor = statusStrip1.BackColor == SystemColors.Control ? SystemColors.ControlDark : SystemColors.Control;
            timer1.Interval = settings.RangeDelay;
        }
    }
    public class MySettings
    {
        [Description("Indicates whether the range delay is enabled at the start.")]
        public bool RangeDelayStart
        {
            get { return Properties.Settings.Default.RangeDelayStart; }
            set { Properties.Settings.Default.RangeDelayStart = value; }
        }

        [Description("Delay in milliseconds. (Hover for seconds)")]
        public int RangeDelay
        {
            get { return Properties.Settings.Default.RangeDelay; }
            set { Properties.Settings.Default.RangeDelay = value; }
        }

        public void Save()
        {
            Properties.Settings.Default.Save();
        }
    }
}
