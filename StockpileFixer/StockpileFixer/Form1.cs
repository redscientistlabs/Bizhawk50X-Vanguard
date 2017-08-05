using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StockpileFixer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*";
            ofd.Title = "Select Broken Stockpile File";
            ofd.Filter = "Broken Stockpile File|*.sks";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {

                string Filename = ofd.FileName;
                string currentDir = Directory.GetCurrentDirectory();
                string tempDir = currentDir + "\\FIX_TEMP\\";

                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);

                Directory.CreateDirectory(tempDir);

                ZipFile.ExtractToDirectory(Filename, tempDir);

                string stockpileXml = File.ReadAllText(tempDir + "\\stockpile.xml");

                stockpileXml = stockpileXml.Replace("address", "Address");
                stockpileXml = stockpileXml.Replace("value", "Value");
                stockpileXml = stockpileXml.Replace("displayType", "DisplayType");
                stockpileXml = stockpileXml.Replace("bigEndian", "BigEndian");
                stockpileXml = stockpileXml.Replace("pipeAdress", "PipeAddress");
                stockpileXml = stockpileXml.Replace("pipeAddress", "PipeAddress");
                stockpileXml = stockpileXml.Replace("pipeDomain", "PipeDomain");
                stockpileXml = stockpileXml.Replace("blastlayer", "BlastLayer");
                stockpileXml = stockpileXml.Replace("stateShortFilename", "StateShortFilename");
                stockpileXml = stockpileXml.Replace("stateFilename", "StateFilename");
                stockpileXml = stockpileXml.Replace("stashkeys", "StashKeys");
                stockpileXml = stockpileXml.Replace("Stashkeys", "StashKeys");
                stockpileXml = stockpileXml.Replace("stateData", "StateData");

                File.Delete(tempDir + "\\stockpile.xml");
                File.WriteAllText(tempDir + "\\stockpile.xml", stockpileXml, Encoding.UTF8);

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = "*";
                sfd.Title = "Save Fixed Stockpile File";
                sfd.Filter = "Fixed Stockpile File|*.sks";
                sfd.RestoreDirectory = true;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ZipFile.CreateFromDirectory(tempDir, sfd.FileName);

                    if (Directory.Exists(tempDir))
                        Directory.Delete(tempDir, true);

                    MessageBox.Show("SKS FILE FIXED");
                    Application.Exit();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*";
            ofd.Title = "Select Broken Savestatelist File";
            ofd.Filter = "Broken Savestatelist File|*.ssk";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {

                string Filename = ofd.FileName;
                string currentDir = Directory.GetCurrentDirectory();
                string tempDir = currentDir + "\\FIX_TEMP\\";

                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);

                Directory.CreateDirectory(tempDir);

                ZipFile.ExtractToDirectory(Filename, tempDir);

                string stockpileXml = File.ReadAllText(tempDir + "\\keys.xml");

                stockpileXml = stockpileXml.Replace("address", "Address");
                stockpileXml = stockpileXml.Replace("value", "Value");
                stockpileXml = stockpileXml.Replace("displayType", "DisplayType");
                stockpileXml = stockpileXml.Replace("bigEndian", "BigEndian");
                stockpileXml = stockpileXml.Replace("pipeAdress", "PipeAddress");
                stockpileXml = stockpileXml.Replace("pipeAddress", "PipeAddress");
                stockpileXml = stockpileXml.Replace("pipeDomain", "PipeDomain");
                stockpileXml = stockpileXml.Replace("blastlayer", "BlastLayer");
                stockpileXml = stockpileXml.Replace("stateShortFilename", "StateShortFilename");
                stockpileXml = stockpileXml.Replace("stateFilename", "StateFilename");
                stockpileXml = stockpileXml.Replace("stashkeys", "StashKeys");
                stockpileXml = stockpileXml.Replace("Stashkeys", "StashKeys");
                stockpileXml = stockpileXml.Replace("stateData", "StateData");

                File.Delete(tempDir + "\\keys.xml");
                File.WriteAllText(tempDir + "\\keys.xml", stockpileXml, Encoding.UTF8);

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = "*";
                sfd.Title = "Save Fixed Savestatelist File";
                sfd.Filter = "Fixed Savestatelist File|*.ssk";
                sfd.RestoreDirectory = true;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ZipFile.CreateFromDirectory(tempDir, sfd.FileName);

                    if (Directory.Exists(tempDir))
                        Directory.Delete(tempDir, true);

                    MessageBox.Show("SSK FILE FIXED");

                }
            }
        }
    }
}
