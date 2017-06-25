using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Media;

namespace RTC
{
    public partial class RTC_Launcher : Form
    {
        Timer t;
        SoundPlayer simpleSound = new SoundPlayer("crash.wav");

        public RTC_Launcher()
        {
            InitializeComponent();
        }

        private void RTC_Launcher_Load(object sender, EventArgs e)
        {
            cbDetection.SelectedIndex = 0;

            t = new Timer();
            t.Interval = 300;
            t.Tick += new EventHandler(CheckHeartbeat);
            t.Start();

            RTC_RPC.Start();
        }

        private void CheckHeartbeat(object sender, EventArgs e)
        {
            if ((pbTimeout.Value == pbTimeout.Maximum && !RTC_RPC.Heartbeat) || RTC_RPC.Freeze || !cbEnabled.Checked)
                return;

            if (!RTC_RPC.Heartbeat)
            {
                pbTimeout.PerformStep();

                if (pbTimeout.Value == pbTimeout.Maximum)
                {
                    //this.Focused = false;
                    btnKillAndRestart_Click(sender, e);
                    //this.Focused = false;
                }


            }
            else
            {
                pbTimeout.Value = 0;
                RTC_RPC.Heartbeat = false;
            }

            

        }

        private void btnKill_Click(object sender, EventArgs e)
        {
            simpleSound.Play();
            RTC_RPC.Heartbeat = false;
            pbTimeout.Value = pbTimeout.Maximum;
            Process.Start("KillSwitch.bat");
            RTC_RPC.Freeze = true;

        }

        private void btnKillAndRestart_Click(object sender, EventArgs e)
        {
            simpleSound.Play();
            RTC_RPC.Heartbeat = false;
            pbTimeout.Value = pbTimeout.Maximum;
            Process.Start("KillSwitchRestart.bat");
            RTC_RPC.Freeze = true;
        }

        private void btnKillResetAndRestart_Click(object sender, EventArgs e)
        {
            if(File.Exists("Restore.dat"))
                File.Delete("Restore.dat");

            if(sender != null)
                simpleSound.Play();

            RTC_RPC.Heartbeat = false;
            pbTimeout.Value = pbTimeout.Maximum;
            Process.Start("KillSwitchRestart.bat");
            RTC_RPC.Freeze = true;

        }

        private void cbDetection_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbDetection.SelectedItem.ToString())
            {
                case "VIOLENT":
                    pbTimeout.Maximum = 4;
                    lbThreshold.Text = "4 seconds";
                    break;
                case "HEAVY":
                    pbTimeout.Maximum = 6;
                    lbThreshold.Text = "6 seconds";
                    break;
                case "MILD":
                    pbTimeout.Maximum = 10;
                    lbThreshold.Text = "10 seconds";
                    break;
                case "SLOPPY":
                    pbTimeout.Maximum = 15;
                    lbThreshold.Text = "15 seconds";
                    break;
                case "COMATOSE":
                    pbTimeout.Maximum = 30;
                    lbThreshold.Text = "30 seconds";
                    break;
            }
        }

        private void cbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbEnabled.Checked)
                RTC_RPC.Freeze = true;
            else
                RTC_RPC.Freeze = false;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://docs.google.com/document/d/1Q3GhhsNBlmzgco8UpSHrViq_MFBg6p6S0ycl8rm_r0I/edit?usp=sharing");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.youtube.com/user/WelshGamer010263");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.redscientist.com");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.youtube.com/watch?v=sIELpn4-Umw");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.youtube.com/watch?v=rEWUpRBvjAM");
        }

    }
}
