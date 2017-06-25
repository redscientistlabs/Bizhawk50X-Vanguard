using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace WindowsGlitchHarvester
{
    public partial class WGH_HookProcessForm : Form
    {
        OpenFileDialog openFileDialog1 = new OpenFileDialog();

        public WGH_HookProcessForm()
        {
            InitializeComponent();
        }

        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    lbProcesses.Items.Add(file);
                }
            }
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            //thx http://stackoverflow.com/questions/11624298/how-to-use-openfiledialog-to-select-a-folder-how-to-reuse-rc-file-from-mfc-in

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            List<String> files = DirSearch(fbd.SelectedPath);

            lbProcesses.Items.AddRange(files.ToArray());

        }

        private List<String> DirSearch(string sDir)
        {
            List<String> files = new List<String>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(DirSearch(d));
                }
            }
            catch (System.Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }

            return files;
        }

		private void WGH_HookToProcess_Load(object sender, EventArgs e)
		{
			lbProcesses.Items.AddRange(Process.GetProcesses().Select(it => $"{it.ProcessName}:{it.Id}").OrderBy(x => x).ToArray());

		}

        private void InitializeOpenFileDialog()
        {
            //thanks: http://stackoverflow.com/questions/1311578/opening-multiple-files-openfiledialog-c


            // Set the file dialog to filter for graphics files.
            this.openFileDialog1.Filter =
                "All files (*.*)|*.*";

            //  Allow the user to select multiple images.
            this.openFileDialog1.Multiselect = true;
            //                   ^  ^  ^  ^  ^  ^  ^

            this.openFileDialog1.Title = "Add File(s)";
        }

        private void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            if (lbProcesses.SelectedIndex != -1)
                lbProcesses.Items.RemoveAt(lbProcesses.SelectedIndex);
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            lbProcesses.Items.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            lbProcesses.Items.Clear();
            this.Close();
        }

		private void btnSendList_Click(object sender, EventArgs e)
		{

			if (lbProcesses.SelectedIndex == -1)
			{
				MessageBox.Show("There's no process selected");
				return;
			}

			var pi = new ProcessInterface(lbProcesses.SelectedItem.ToString());

			if (pi.Hooked)
			{
				WGH_Core.currentMemoryInterface = pi;
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			else
			{
				this.Close();
			}

            
        }

        private void WGH_SelectMultiple_FormClosing(object sender, FormClosingEventArgs e)
        {
            lbProcesses.Items.Clear();
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog1;
            OpenFileDialog1 = new OpenFileDialog();

            OpenFileDialog1.DefaultExt = "txt";
            OpenFileDialog1.Title = "Open File list";
            OpenFileDialog1.Filter = "TXT files|*.txt";
            OpenFileDialog1.RestoreDirectory = true;
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] files = File.ReadAllLines(OpenFileDialog1.FileName);
                    lbProcesses.Items.Clear();
                    lbProcesses.Items.AddRange(files);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Something went wrong while loading file list \n\n" + ex.ToString());
                }
            }
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Title = "Save File list";
            saveFileDialog1.Filter = "TXT files|*.txt";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<string> allLines = new List<string>();

                    foreach (var item in lbProcesses.Items)
                        allLines.Add(item.ToString());

                    File.WriteAllLines(saveFileDialog1.FileName, allLines.ToArray());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong while saving file list \n\n" + ex.ToString());
                }
            }
        }


    }
}
