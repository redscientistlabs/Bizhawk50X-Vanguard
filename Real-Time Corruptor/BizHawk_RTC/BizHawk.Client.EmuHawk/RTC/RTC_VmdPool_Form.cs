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

            lbRealDomainValue.Text = (mi as VirtualMemoryDomain).PointerDomains[0];
            lbVmdSizeValue.Text = mi.Size.ToString();
        }

        private void btnSaveVmd_Click(object sender, EventArgs e)
        {
            if (lbLoadedVmdList.SelectedIndex == -1)
                return;

            string VmdName = lbLoadedVmdList.SelectedItem.ToString();
            VirtualMemoryDomain VMD = (VirtualMemoryDomain)RTC_MemoryDomains.VmdPool[VmdName];

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.DefaultExt = "xml";
            saveFileDialog1.Title = "Save VMD to File";
            saveFileDialog1.Filter = "XML VMD file|*.xml";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string Filename = saveFileDialog1.FileName;

                FileStream FS;
                XmlSerializer xs = new XmlSerializer(typeof(VmdPrototype));

                //creater stockpile.xml to temp folder from stockpile object
                FS = File.Open(Filename, FileMode.OpenOrCreate);
                xs.Serialize(FS, VMD.proto);
                FS.Close();

            }
            else
                return;
        }

        private void btnLoadVmd_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "xml";
            ofd.Multiselect = true;
            ofd.Title = "Open VMD File";
            ofd.Filter = "VMD xml files|*.xml";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //string Filename = ofd.FileName.ToString();
                foreach (string Filename in ofd.FileNames)
                {
                    VirtualMemoryDomain VMD;
                    try
                    {

                        FileStream FS;
                        XmlSerializer xs = new XmlSerializer(typeof(VmdPrototype));
                        FS = File.Open(Filename, FileMode.OpenOrCreate);
                        var proto = (VmdPrototype)xs.Deserialize(FS);
                        FS.Close();
                        VMD = proto.Generate();

                        string VmdName = VMD.ToString();
                        if (RTC_MemoryDomains.VmdPool.ContainsKey(VmdName))
                            RTC_MemoryDomains.VmdPool.Remove(VmdName);

                        RTC_MemoryDomains.VmdPool[VmdName] = VMD;
                        if (RTC_Core.isStandalone)
                        {
                            RTC_RPC.SendToKillSwitch("FREEZE");
                            RTC_NetCore.HugeOperationStart();
                            RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_VMD_ADD) { objectValue = VMD.proto }, true);
                            RTC_RPC.SendToKillSwitch("UNFREEZE");
                            RTC_NetCore.HugeOperationEnd();
                        }

                        RefreshVMDs();
                        RTC_Core.coreForm.RefreshDomainsAndKeepSelected();
                    }
                    catch
                    {
                        MessageBox.Show($"The VMD xml file {Filename} could not be loaded.");
                    }
                }
            }
            else
                return;
        }
    }
}
