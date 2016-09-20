using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Management;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fanBOOST___METRO_UI
{
    public partial class Form1 : MetroForm
    {

        public Form1()
        {
            InitializeComponent();

            this.MaximizeBox = false;
            this.notifyIcon1.Text = "fanBOOSTv2.00";
            try
            {
                this.fanc = new FanController(getModel("Vendor"), getModel("Name"));
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message, "fanBOOST", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            // CHECK THE STATUS OF DLL
            string status = fanc.getStatus();
            if (status != "OK")
            {
                MetroMessageBox.Show(this, status, "WinRing0 error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            updateForm();
        }

        public void MinimizeToTray()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                this.ShowInTaskbar = false;
                notifyIcon1.ShowBalloonTip(2000);
            }
        }

        public void MaximizeFromTray()
        {
            this.ShowInTaskbar = true;
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }


        private FanController fanc;
        private Icon ico;
        private void Form1_Load(object sender, EventArgs e)
        {
            ico = notifyIcon1.Icon;
            notifyIcon1.Visible = false;

            Timer MyTimer = new Timer();
            MyTimer.Interval = (4 * 1000);
            MyTimer.Tick += new EventHandler(MyTimer_Tick);
            MyTimer.Start();
        }
        
        private string getModel(string type)
        {
            string data= "unknown";
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystemProduct");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (type == "Name")
                        data = (string)queryObj["Name"];
                    else if (type == "Vendor")
                        data = (string)queryObj["Vendor"];
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }

            return data;
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void metroContextMenu1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.MaximizeFromTray();
        }

        private void ajustesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MaximizeFromTray();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.MinimizeToTray();
        }

        private void autoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fanc.setFanSpeed(FanController.SPEED.AUTO);
            this.notifyIcon1.BalloonTipText = "spedd changed";
            notifyIcon1.ShowBalloonTip(1500);

        }

        private void bOOST1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fanc.setFanSpeed(FanController.SPEED.BOOST_1);
            this.notifyIcon1.BalloonTipText = "spedd changed";
            notifyIcon1.ShowBalloonTip(1500);
        }

        private void bOOST2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fanc.setFanSpeed(FanController.SPEED.BOOST_2);
            this.notifyIcon1.BalloonTipText = "spedd changed";
            notifyIcon1.ShowBalloonTip(1500);
        }

        private void bOOST3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fanc.setFanSpeed(FanController.SPEED.BOOST_3);
            this.notifyIcon1.BalloonTipText = "spedd changed";
            notifyIcon1.ShowBalloonTip(1500);
        }
        
        private void metroLabel1_Click(object sender, EventArgs e)
        {

        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            fanc.setFanSpeed(FanController.SPEED.AUTO);
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            fanc.setFanSpeed(FanController.SPEED.BOOST_1);
        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            fanc.setFanSpeed(FanController.SPEED.BOOST_2);
        }

        private void metroTile4_Click(object sender, EventArgs e)
        {
            fanc.setFanSpeed(FanController.SPEED.BOOST_3);
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                updateForm();
        }

        private void updateForm()
        {
            metroLabel2.Text = fanc.getFanSpeed().ToString();
        }

        private void metroLabel3_Click(object sender, EventArgs e)
        {

        }
    }
}
