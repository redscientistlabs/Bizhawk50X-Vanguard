using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace RTC_Launcher
{
    public partial class MainForm : Form
    {
        public static string launcherDir = Directory.GetCurrentDirectory();
        public static string webRessourceDomain = "http://redscientist.com/software";
        public static DownloadForm dForm = null;

        public static int devCounter = 0;

        public Button[] buttons;

        public MainForm()
        {
            InitializeComponent();

            buttons = new Button[]
            {
            btnBatchfile01,
            btnBatchfile02,
            btnBatchfile03,
            btnBatchfile04,
            btnBatchfile05,
            btnBatchfile06,
            btnBatchfile07,
            btnBatchfile08,
            btnBatchfile09,
            btnBatchfile10,
            };

            if (!Directory.Exists(launcherDir + "\\VERSIONS\\"))
                Directory.CreateDirectory(launcherDir + "\\VERSIONS\\");

            if (!Directory.Exists(launcherDir + "\\PACKAGES\\"))
                Directory.CreateDirectory(launcherDir + "\\PACKAGES\\");

            if(File.Exists(launcherDir + "\\PACKAGES\\dev.txt"))
                webRessourceDomain = "http://cc.r5x.cc";

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshInstalledVersions();

            try
            {
                var versionFile = GetFileViaHttp($"{webRessourceDomain}/rtc/releases/version.php");
                string str = Encoding.UTF8.GetString(versionFile);
                List<string> onlineVersions = new List<string>(str.Split('|'));

                lbOnlineVersions.Items.Clear();
                lbOnlineVersions.Items.AddRange(onlineVersions.OrderByDescending(x => x).Select(it => it.Replace(".zip","")).ToArray());

                var motdFile = GetFileViaHttp($"{webRessourceDomain}/rtc/releases/MOTD.txt");
                string motd = Encoding.UTF8.GetString(motdFile);

                lbMOTD.Text = motd;

            }
            catch
            {
                lbMOTD.Text = "Couldn't load the RTC MOTD from Redscientist.com";
            }

            lbMOTD.Visible = true;

            SetRTCColor(Color.FromArgb(120,180,155));

        }

        public void SetRTCColor(Color color, Form form = null)
        {
            //Recolors all the RTC Forms using the general skin color

            List<Control> allControls = new List<Control>();

            if (form == null)
            {

                allControls.AddRange(this.Controls.getControlsWithTag());
                allControls.Add(this);


            }
            else
                allControls.AddRange(form.Controls.getControlsWithTag());

            var lightColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:light"));
            var normalColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:normal"));
            var darkColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:dark"));
            var darkerColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:darker"));

            foreach (Control c in lightColorControls)
                c.BackColor = color.ChangeColorBrightness(0.30f);

            foreach (Control c in normalColorControls)
                c.BackColor = color;

            //spForm.dgvStockpile.BackgroundColor = color;
            //ghForm.dgvStockpile.BackgroundColor = color;

            foreach (Control c in darkColorControls)
                c.BackColor = color.ChangeColorBrightness(-0.30f);

            foreach (Control c in darkerColorControls)
                c.BackColor = color.ChangeColorBrightness(-0.75f);

        }


        public void RefreshInstalledVersions()
        {
            lbVersions.Items.Clear();
            List<string> versions = new List<string>(Directory.GetDirectories(launcherDir + "\\VERSIONS\\"));
            lbVersions.Items.AddRange(versions.OrderByDescending(x => x).Select(it => getFilenameFromFullFilename(it)).ToArray<object>());
        }

        public byte[] GetFileViaHttp(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadData(url);
            }
        }

        public string getFilenameFromFullFilename(string fullFilename)
        {
            return fullFilename.Substring(fullFilename.LastIndexOf('\\') + 1);
        }

        public void DisplayVersion(string version)
        {
            List<string> batchFiles = new List<string>(Directory.GetFiles(launcherDir + "\\VERSIONS\\" + version));
            List<string> batchFileNames = new List<string>(batchFiles.Select(it => getFilenameFromFullFilename(it).Replace(".bat", "")));

            bool isDefaultStartPresent = false;

            if(batchFileNames.Contains("START"))
            {
                batchFileNames.Remove("START");
                isDefaultStartPresent = true;
            }

            btnStart.Visible = isDefaultStartPresent;

            foreach (Button btn in buttons)
                btn.Visible = false;

            for(int i = 0; i < batchFileNames.Count; i++)
            {
                buttons[i].Visible = true;
                buttons[i].Text = batchFileNames[i];
            }

            lbSelectedVersion.Text = version;
            lbSelectedVersion.Visible = true;
            pnVersionBatchFiles.Visible = true;

            new object();
        }

        private void lbVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbSelectedVersion.Visible = false;
            pnVersionBatchFiles.Visible = false;

            if (lbVersions.SelectedIndex == -1)
                return;

            DisplayVersion(lbVersions.SelectedItem.ToString());
        }

        private void btnBatchfile_Click(object sender, EventArgs e)
        {
            Button currentButton = (Button)sender;

            string version = lbVersions.SelectedItem.ToString();

            string fullPath = launcherDir + "\\VERSIONS\\" + version + "\\" + currentButton.Text + ".bat";
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Path.GetFileName(fullPath);
            psi.WorkingDirectory = Path.GetDirectoryName(fullPath);
            Process.Start(psi);
        }

        private void lbOnlineVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbOnlineVersions.SelectedIndex == -1)
                return;

            btnDownloadVersion.Visible = true;

        }

        private void btnDownloadVersion_Click(object sender, EventArgs e)
        {
            if (lbOnlineVersions.SelectedIndex == -1)
                return;

            string version = lbOnlineVersions.SelectedItem.ToString();

            if (Directory.Exists((launcherDir + "\\VERSIONS\\" + version)))
            {
                if(MessageBox.Show($"The version {version} is already installed.\nThis will DELETE version {version} and redownload it.\n\nWould you like to continue?","WARNING",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Directory.Delete(launcherDir + "\\VERSIONS\\" + version, true);
                }
                else
                {
                    return;
                }
            }

            if (File.Exists(launcherDir + "\\PACKAGES\\" + version + ".zip"))
                File.Delete(launcherDir + "\\PACKAGES\\" + version + ".zip");

            dForm = new DownloadForm();
           
            dForm.TopLevel = false;
            dForm.Location = new Point(0, 0);
            Controls.Add(dForm);

            dForm.Show();
            dForm.Focus();
            dForm.BringToFront();

            WebClient webClient = new WebClient();

            webClient.DownloadProgressChanged += (ov, ev) =>
            {
                dForm.progressBar.Value = ev.ProgressPercentage;
            };

            webClient.DownloadFileCompleted += (ov, ev) =>
            {

                InvokeUI(() => { DownloadComplete(); });
            };


            webClient.DownloadFileAsync(new Uri($"{webRessourceDomain}/rtc/releases/" + version + ".zip"), launcherDir + "\\PACKAGES\\" + version + ".zip");

        }

        private void InvokeUI(Action a)
        {
            this.BeginInvoke(new MethodInvoker(a));
        }

        public void DownloadComplete()
        {


            string version = lbOnlineVersions.SelectedItem.ToString();

            Directory.CreateDirectory(launcherDir + "\\VERSIONS\\" + version);
            System.IO.Compression.ZipFile.ExtractToDirectory(launcherDir + "\\PACKAGES\\" + version + ".zip", launcherDir + "\\VERSIONS\\" + version);

            lbVersions.SelectedIndex = -1;

            RefreshInstalledVersions();

            lbOnlineVersions.SelectedIndex = -1;
            btnDownloadVersion.Visible = false;

            dForm.Close();
            dForm = null;

            

        }

        public void DeleteSelected()
        {
            if (lbVersions.SelectedIndex == -1)
                return;

            string version = lbVersions.SelectedItem.ToString();

            if (File.Exists(launcherDir + "\\PACKAGES\\" + version + ".zip"))
                File.Delete(launcherDir + "\\PACKAGES\\" + version + ".zip");

            if (Directory.Exists((launcherDir + "\\VERSIONS\\" + version)))
                    Directory.Delete(launcherDir + "\\VERSIONS\\" + version, true);

            lbVersions.SelectedIndex = -1;
            RefreshInstalledVersions();
    
        }

        private void lbVersions_MouseDown(object sender, MouseEventArgs e)
        {
            if (lbVersions.SelectedIndex == -1)
                return;

            if (e.Button == MouseButtons.Right)
            {
                Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

                ContextMenuStrip columnsMenu = new ContextMenuStrip();
                columnsMenu.Items.Add("Delete", null, new EventHandler((ob, ev) => { DeleteSelected(); }));
                columnsMenu.Show(this, locate);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            devCounter++;

            if (devCounter >= 10)
            {
                if (MessageBox.Show((File.Exists(launcherDir + "\\PACKAGES\\dev.txt") ? "Do you want to stay connected to the Dev Server?" : "Do you want to connect to the Dev Server?"), "Dev mode activation", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    File.WriteAllText(launcherDir + "\\PACKAGES\\dev.txt", "DEV MODE ACTIVATED");
                    Application.Restart();
                }
                else
                {
                    if (File.Exists(launcherDir + "\\PACKAGES\\dev.txt"))
                        File.Delete(launcherDir + "\\PACKAGES\\dev.txt");

                    Application.Restart();
                }
            }
        }

        private void btnOnlineGuide_Click(object sender, EventArgs e)
        {
            Process.Start("https://corrupt.wiki/");
        }
    }
}
