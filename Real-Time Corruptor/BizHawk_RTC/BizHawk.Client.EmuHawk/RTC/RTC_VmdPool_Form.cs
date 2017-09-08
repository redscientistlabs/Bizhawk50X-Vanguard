using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace RTC
{
    public partial class RTC_VmdPool_Form : Form
    {
        public RTC_VmdPool_Form()
        {
            InitializeComponent();
        }

        private void btnUnloadVMD_Click(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
                return;

            string VmdName = lbLoadedVmdList.SelectedItem.ToString();

            if(RTC_MemoryDomains.VmdPool.ContainsKey(VmdName))
            {
                RTC_MemoryDomains.VmdPool.Remove(VmdName);
                RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_VMD_REMOVE) { objectValue = VmdName });
            }

            RefreshVMDs();
            RTC_Core.coreForm.RefreshDomainsAndKeepSelected();
        }

        public void RefreshVMDs()
        {
            lbLoadedVmdList.Items.Clear();
            lbLoadedVmdList.Items.AddRange(RTC_MemoryDomains.VmdPool.Values.Select(it => it.ToString()).ToArray());

            lbRealDomainValue.Text = "#####";
            lbVmdSizeValue.Text = "#####";

        }

        private void RTC_VmdPool_Form_Load(object sender, EventArgs e)
        {

        }

        private void lbLoadedVmdList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
                return;

            string VmdName = lbLoadedVmdList.SelectedItem.ToString();
            MemoryInterface mi = RTC_MemoryDomains.VmdPool[VmdName];

            lbRealDomainValue.Text = (mi as VirtualMemoryDomain).MemoryPointers[0].Domain;
            lbVmdSizeValue.Text = mi.Size.ToString();
        }

        private void btnSaveVmd_Click(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
                return;

            string VmdName = lbLoadedVmdList.SelectedItem.ToString();
            VirtualMemoryDomain VMD = (VirtualMemoryDomain)RTC_MemoryDomains.VmdPool[VmdName];

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.DefaultExt = "vmd";
            saveFileDialog1.Title = "Save VMD to File";
            saveFileDialog1.Filter = "Binary VMD file|*.vmd|XML VMD file|*.xml";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string Filename = saveFileDialog1.FileName;
                if (Filename.ToUpper().EndsWith(".VMD"))
                {
                    File.WriteAllBytes(Filename, VMD.ToData());
                }
                else
                {
                    FileStream FS;
                    XmlSerializer xs = new XmlSerializer(typeof(VirtualMemoryDomain));

                    //creater stockpile.xml to temp folder from stockpile object
                    FS = File.Open(Filename, FileMode.OpenOrCreate);
                    xs.Serialize(FS, VMD);
                    FS.Close();
                }
            }
            else
                return;
        }

        private void btnLoadVmd_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "vmd";
            ofd.Title = "Open VMD File";
            ofd.Filter = "VMD files|*.vmd;*.xml";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string Filename = ofd.FileName.ToString();

                
                VirtualMemoryDomain VMD;
                byte[] binaryData = null;

                try
                {
                    if (Filename.ToUpper().EndsWith(".VMD"))
                    {
                        binaryData = File.ReadAllBytes(Filename);
                        VMD = VirtualMemoryDomain.FromData(binaryData);
                    }
                    else
                    {
                        FileStream FS;
                        XmlSerializer xs = new XmlSerializer(typeof(VirtualMemoryDomain));
                        FS = File.Open(Filename, FileMode.OpenOrCreate);
                        VMD = (VirtualMemoryDomain)xs.Deserialize(FS);
                        FS.Close();
                    }

                    string VmdName = VMD.ToString();
                    if (RTC_MemoryDomains.VmdPool.ContainsKey(VmdName))
                        RTC_MemoryDomains.VmdPool.Remove(VmdName);

                    RTC_MemoryDomains.VmdPool[VmdName] = VMD;
                    if (RTC_Core.isStandalone)
                    {
                        RTC_RPC.SendToKillSwitch("FREEZE");
                        RTC_NetCore.HugeOperationStart();
                        RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_VMD_ADD) { objectValue = (binaryData != null ? binaryData : VMD.ToData()) },true);
                        RTC_RPC.SendToKillSwitch("UNFREEZE");
                        RTC_NetCore.HugeOperationEnd();
                    }

                    RefreshVMDs();
                    RTC_Core.coreForm.RefreshDomainsAndKeepSelected();
                }
                catch
                {
                    MessageBox.Show("The VMD xml file could not be loaded");
                    return;
                }
            }
            else
                return;
        }
    }
}
