using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ChainChompCorruptor;
using System.IO.Compression;
using System.Media;
using System.Runtime.Serialization;

/*
 * Chain Chomp (c) by DaleJ

Chain Chomp is licensed under a
Creative Commons Attribution-NonCommercial 4.0 International License.

You should have received a copy of the license along with this
work. If not, see <http://creativecommons.org/licenses/by-nc/4.0/>. 
*/


namespace ChainChomp
{
    public partial class ChainChompApplication : Form
    {
        //hmm....
        SoundPlayer m64Chomp = new SoundPlayer(ChainChomp.Properties.Resources.chomp);

        //extra forms
        AboutChainChomp aboutDialog = new AboutChainChomp();

        //paths
        public static string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ChainChomp\";
        public static string tempPath = Path.GetTempPath() + @"chainChomp\";
        public static string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public static string appPath = Application.StartupPath;
        public static string defaultChainPath = appPath + @"\chains\";
        public static string factoryPresetsPath = appPath + @"\presets\";
        public static string outputPath = appDataPath + @"output";

        //emulator process
        System.Diagnostics.Process emulator;

        //working data
        public static ROMImage game;
        private ComboBoxItem selectedGame;
        private ComboBoxItem selectedEmu;

        //RTC Plugin Vars
        public static ChainChompApplication mainForm;
        public static string p = "";

        //construct
        public ChainChompApplication()
        {
            InitializeComponent();

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
                
            }

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            //load library
            ImageEmuLibrary.LoadSettings();
            PopulateComboBoxes();
            if (ImageEmuLibrary.images.Count > 0)
            {
                romImageComboBox.SelectedIndex = 0;
            }

            ChainEditor root = new ChainEditor(chainTab);
            root.removeChainButton.Enabled = false;
            rootTabPage.Controls.Add(root);
			root.ResizeRack(panel1.Height - 136);
       
        }

        // Temp File Methods
        public static void FlushTempDir() //clear & create temp dir for use
        {
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
            }

            Directory.CreateDirectory(tempPath);

        }

        public static DirectoryInfo ExtractToTemp(string path) //extract a zip archive to temp and get dirinfo
        {
            FlushTempDir();
			if (FileIO.ExtractZip(path, tempPath))
				return new DirectoryInfo(tempPath);

			return null;
        }

        public static void ZipFromTemp(string path)
        {
            FileIO.ZipFromDir(tempPath, path);
        }
        

        // Rom Handling Methods

        private static ROMImage LoadROM(string path)
        {
            ROMImage rom = new ROMImage(path);
            return rom;
        }

        private void LoadSelectedROM()
        {
            if (selectedGame == null)
            {
                game = null;
                return;
            }

            //load clean rom if not already loaded
            if (game == null)
            {
                game = LoadROM(selectedGame.Text);
            }
            else if(game.fileName != selectedGame.Text)
            {
                game = LoadROM(selectedGame.Text);
            }
        }

        // combo box stuff

        private void PopulateComboBoxes()
        {
            //save selection
            string imgSel = romImageComboBox.SelectedItem != null ? selectedGame.Text : null;

            //empty boxes
            romImageComboBox.Items.Clear();
            emuComboBox.Items.Clear();

            //fill
            emuComboBox.Items.Add(new ComboBoxItem("None"));
            ImageEmuLibrary.emus.ForEach(i => emuComboBox.Items.Add(new ComboBoxItem(i)));
            ImageEmuLibrary.images.ForEach(i => romImageComboBox.Items.Add(new ComboBoxItem(i[0], i[1])));
            emuComboBox.SelectedIndex = 0; //default to "None"

            //attempt to restore selection
            if (imgSel != null)
            {
                ComboBoxItem[] imgItems = new ComboBoxItem[romImageComboBox.Items.Count];
                romImageComboBox.Items.CopyTo(imgItems, 0);
                romImageComboBox.SelectedItem = imgItems.ToList().FirstOrDefault(i => i.Text == imgSel);

                UpdateEmuSelection();                
            }
        }

        private void UpdateEmuSelection()
        {
            ComboBoxItem[] emuItems = new ComboBoxItem[emuComboBox.Items.Count];
            emuComboBox.Items.CopyTo(emuItems, 0);
            emuComboBox.SelectedItem = emuItems.ToList().FirstOrDefault(i => (string)selectedGame.Value == i.Text);
            if (emuComboBox.SelectedItem == null)
            {
                emuComboBox.SelectedIndex = 0;
            }
        }

        // RUN

        public void runButton_Click(object sender, EventArgs e)
        {
            //the fun part!
            if (game != null && game.length > 0)
            {
                //get ready for another round
                try
                {
                    if (Directory.Exists(outputPath))
                    {
                        Directory.Delete(outputPath, true);
                        Directory.CreateDirectory(outputPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(outputPath);
                    }
                }
                catch (IOException error)
                {
                    MessageBox.Show(error.Message, "Error", MessageBoxButtons.OK);
                    return;
                }
               
                game.ClearCorruptions();

                //gather chains & prepare threads
                List<Chain> chains = new List<Chain>();
                //List<BackgroundWorker> threads = new List<BackgroundWorker>();

                for(int i = 0; i < chainTab.TabCount - 1; i++)
                {
                    ChainEditor ce = (ChainEditor)chainTab.TabPages[i].Controls[0];
                    if (ce.enableChainCheckBox.Checked)
                    {
                        chains.Add((chainTab.TabPages[i].Controls[0] as ChainEditor).chain);
                    }
                }

                //corrupt stuff
                List<ROMSample> corruptions = new List<ROMSample>();
                chains.ForEach(i => corruptions.Add(i.RunChain(game.GetSample(i.startOffset, i.endOffset))));
                game.CommitSampleRangeToROM(corruptions);

                //string p = string.Format(@"{0}\chainChomp{1}", outputPath, Path.GetExtension(game.fileName));



                if (game.Dump(p) && selectedEmu != null && selectedEmu.Text != "None")
                {
                    // stop/start emulator
                    if (emulator != null && !emulator.HasExited)
                    {
                        emulator.Kill();
                    }

					// Set-up process
					string fileName = selectedEmu.Text;
					string location = fileName.Substring(0, fileName.LastIndexOf("\\") - 1);
					var startInfo = new System.Diagnostics.ProcessStartInfo();
					startInfo.FileName = fileName;
					startInfo.Arguments = p;

                    //emulator = System.Diagnostics.Process.Start(startInfo);

                    emulator = System.Diagnostics.Process.Start("\"" + fileName + "\"", "\"" + p + "\"");
                }
            }
            else
            {
                MessageBox.Show("Invalid ROM Image selected. Chains were not run.", "Run Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        // QUIT

        private void ChainChompWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            ImageEmuLibrary.SaveSettings();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m64Chomp.Play();
            aboutDialog.ShowDialog(this);
        }


        // LIBRARY

        private void openLibrary_Click(object sender, EventArgs e)
        {
            LibraryWindow libWindow = new LibraryWindow();
            libWindow.ShowDialog(this);
            PopulateComboBoxes();
            selectedGame = (ComboBoxItem)romImageComboBox.SelectedItem;
            LoadSelectedROM();
        }

        public void setRom(string RomFile)
        {
            selectedGame = new ComboBoxItem(RomFile);
            //chainChompToolTip.SetToolTip(romImageComboBox, selectedGame.Text);
            UpdateEmuSelection();
            LoadSelectedROM();
        }

        public void setTarget(string RomFile)
        {
            p = RomFile;
        }

        private void romImageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            return;

            selectedGame = (ComboBoxItem)romImageComboBox.SelectedItem;
            chainChompToolTip.SetToolTip(romImageComboBox, selectedGame.Text);
            UpdateEmuSelection();
            LoadSelectedROM();

        }

        public void setEmu(string EmuFile)
        {
            selectedEmu = new ComboBoxItem(EmuFile);
        }

        private void emuComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            return;

            ComboBoxItem sel = (ComboBoxItem)emuComboBox.SelectedItem;
            if (sel != null && sel != emuComboBox.Items[0])
            {
                if (File.Exists(sel.Text))
                {
                    selectedEmu = sel;
                }
                else if(sel.Text != "")
                {
                    selectedEmu = (ComboBoxItem)emuComboBox.Items[0];
                    MessageBox.Show(string.Format("File {0} not found!\nChains will still be run but no emulator will start.", sel.Text));
                }
            }
            else if (sel == null || sel == emuComboBox.Items[0])
            {
                selectedEmu = (ComboBoxItem)emuComboBox.Items[0];
            }

            //update rom-emu association
            if (emuComboBox.SelectedIndex > -1 && selectedGame != null)
            {
                selectedGame.Value = selectedEmu.Text;
                string[] item = ImageEmuLibrary.images.FirstOrDefault(i => i[0] == selectedGame.Text);
                if (item != null)
                {
                    item[1] = selectedEmu.Text;
                }
            }
        }

        // NEW TAB

        private void chainTab_Selecting(object sender, TabControlCancelEventArgs e)
        {
            int index = chainTab.TabCount - 1;
            if (e.TabPageIndex == index)
            {
                chainTab.TabPages.Insert(index, "New Chain");
                chainTab.TabPages[index].Controls.Add(new ChainEditor(chainTab));
				(chainTab.TabPages[index].Controls[0] as ChainEditor).ResizeRack(panel1.Height - 136);
                chainTab.SelectedIndex = index;
            }
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://bitbucket.org/Lafolie/chainchomp/wiki/Home");
        }

		private void ChainChompApplication_Resize(object sender, EventArgs e)
		{

			foreach (TabPage t in chainTab.TabPages)
			{
				if (t.Controls.Count > 0) // ignore +Chain tab
				{
					(t.Controls[0] as ChainEditor).ResizeRack(panel1.Height - 136); // offset makes it fit properly (missing margin somewhere?)
				}
			}
		}

        private void ChainChompApplication_Load(object sender, EventArgs e)
        {
            mainForm = this;
            RTC.RTC_RPC.Plugin_Loaded(this);
        }
    }
}
