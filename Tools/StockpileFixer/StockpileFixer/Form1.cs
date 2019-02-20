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

                string data = File.ReadAllText(tempDir + "\\stockpile.json");
                /*
		        SKS,
		        SSK,
		        MP,
		        SESSION,
		        DEFAULTVALUE
                */

                data = data.Replace("\"StateLocation\": 0,", "\"StateLocation\": \"SKS\",");
                data = data.Replace("\"StateLocation\": 1,", "\"StateLocation\": \"SSK\",");
                data = data.Replace("\"StateLocation\": 2,", "\"StateLocation\": \"MP\",");
                data = data.Replace("\"StateLocation\": 3,", "\"StateLocation\": \"SESSION\",");
                data = data.Replace("\"StateLocation\": 4,", "\"StateLocation\": \"DEFAULTVALUE\",");


                data = data.Replace("\"LimiterTime\": 0,", "\"LimiterTime\": \"NONE\",");
                data = data.Replace("\"LimiterTime\": 1,", "\"LimiterTime\": \"GENERATE\",");
                data = data.Replace("\"LimiterTime\": 2,", "\"LimiterTime\": \"PREEXECUTE\",");
                data = data.Replace("\"LimiterTime\": 3,", "\"LimiterTime\": \"EXECUTE\",");

                data = data.Replace("\"StoreTime\": 0,", "\"StoreTime\": \"IMMEDIATE\",");
                data = data.Replace("\"StoreTime\": 1,", "\"StoreTime\": \"IMMEDIATE\",");
                data = data.Replace("\"StoreTime\": 2,", "\"StoreTime\": \"PREEXECUTE\",");

                data = data.Replace("\"StoreType\": 0,", "\"StoreType\": \"ONCE\",");
                data = data.Replace("\"StoreType\": 1,", "\"StoreType\": \"CONTINUOUS\",");

                //Fix my fixer goof
                data = data.Replace("\"StateLocation\":\"ONCE\",", "\"StoreType\": \"ONCE\",");
                data = data.Replace("\"StateLocation\": \"CONTINUOUS\",", "\"StoreType\": \"CONTINUOUS\",");
                data = data.Replace("\"StateLocation\":\"IMMEDIATE\",", "\"StoreTime\": \"IMMEDIATE\",");
                data = data.Replace("\"StateLocation\": \"PREEXECUTE\",", "\"StoreTime\": \"PREEXECUTE\",");

                data = data.Replace("\"StoreLimiterSource\": 0,", "\"StoreLimiterSource\": \"ADDRESS\",");
                data = data.Replace("\"StoreLimiterSource\": 1,", "\"StoreLimiterSource\": \"SOURCEADDRESS\",");
                data = data.Replace("\"StoreLimiterSource\": 2,", "\"StoreLimiterSource\": \"BOTH\",");


                data = data.Replace("\"BlastUnitSource\": 0,", "\"BlastUnitSource\": \"VALUE\",");
                data = data.Replace("\"BlastUnitSource\": 1,", "\"BlastUnitSource\": \"STORE\",");


                File.Delete(tempDir + "\\stockpile.json");
                File.WriteAllText(tempDir + "\\stockpile.json", data, Encoding.UTF8);

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

                string data = File.ReadAllText(tempDir + "\\keys.json");


                data = data.Replace("\"StateLocation\": 0,", "\"StateLocation\": \"SKS\",");
                data = data.Replace("\"StateLocation\": 1,", "\"StateLocation\": \"SSK\",");
                data = data.Replace("\"StateLocation\": 2,", "\"StateLocation\": \"MP\",");
                data = data.Replace("\"StateLocation\": 3,", "\"StateLocation\": \"SESSION\",");
                data = data.Replace("\"StateLocation\": 4,", "\"StateLocation\": \"DEFAULTVALUE\",");


                File.Delete(tempDir + "\\keys.json");
                File.WriteAllText(tempDir + "\\keys.json", data, Encoding.UTF8);

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
