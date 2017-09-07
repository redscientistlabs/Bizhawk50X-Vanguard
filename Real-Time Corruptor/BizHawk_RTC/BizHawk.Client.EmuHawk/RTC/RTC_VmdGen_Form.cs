using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTC
{
    public partial class RTC_VmdGen_Form : Form
    {
        public RTC_VmdGen_Form()
        {
            InitializeComponent();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            RTC_Core.coreForm.RefreshDomainsAndKeepSelected();

            cbSelectedEngine.Items.Clear();
            cbSelectedEngine.Items.AddRange(RTC_MemoryDomains.MemoryInterfaces.Keys.Where(it => !it.Contains("[V]")).ToArray());

            cbSelectedEngine.SelectedIndex = 0;
        }

        private void cbSelectedEngine_SelectedIndexChanged(object sender, EventArgs e)
        {



            if (string.IsNullOrWhiteSpace(cbSelectedEngine.SelectedItem?.ToString()) || !RTC_MemoryDomains.MemoryInterfaces.ContainsKey(cbSelectedEngine.SelectedItem.ToString()))
            {
                cbSelectedEngine.Items.Clear();
                return;
            }

            MemoryInterface mi = RTC_MemoryDomains.MemoryInterfaces[cbSelectedEngine.SelectedItem.ToString()];

            lbDomainSizeValue.Text = mi.Size.ToString();
            lbWordSizeValue.Text = $"{mi.WordSize*8} bits";
            lbEndianTypeValue.Text = (mi.BigEndian ? "Big" : "Little");

            nmStartingAddress.Value = 0;
            nmStartingAddress.Maximum = mi.Size - 2;

            nmRangeSize.Value = 1;
            nmRangeSize.Maximum = mi.Size;
            nmRangeSize.Value = mi.Size;
        }

        private void btnGenerateVMD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbSelectedEngine.SelectedItem?.ToString()) || !RTC_MemoryDomains.MemoryInterfaces.ContainsKey(cbSelectedEngine.SelectedItem.ToString()))
            {
                cbSelectedEngine.Items.Clear();
                return;
            }

            MemoryInterface mi = RTC_MemoryDomains.MemoryInterfaces[cbSelectedEngine.SelectedItem.ToString()];
            VirtualMemoryDomain VMD = new VirtualMemoryDomain();

            List<decimal> Blacklist = new List<decimal>();
            List<decimal> Added = new List<decimal>();

            foreach(string line in tbCustomAddresses.Lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string trimmedLine = line.Trim();

                bool remove = false;

                if (trimmedLine[0] == '-')
                {
                    remove = true;
                    trimmedLine = trimmedLine.Substring(1);
                }

                string[] lineParts = trimmedLine.Split('-');

                if(lineParts.Length > 1)
                {
                    decimal start = Convert.ToDecimal(lineParts[0]);
                    decimal end = Convert.ToDecimal(lineParts[1]);

                    if (start >= end)
                        continue;

                    for(decimal i = start; i<end+1; i++)
                    {
                        if (remove)
                        {
                            if (!Blacklist.Contains(i))
                                Blacklist.Add(i);
                        }
                        else
                        {
                            if (!Added.Contains(i))
                                Added.Add(i);
                        }
                    }
                }
                else
                {
                    decimal value = Convert.ToDecimal(lineParts[0]);

                    if (remove)
                    {
                        if (!Blacklist.Contains(value))
                            Blacklist.Add(value);
                    }
                    else
                    {
                        if (!Added.Contains(value))
                            Added.Add(value);
                    }
                }
            }


            if (string.IsNullOrWhiteSpace(tbVmdName.Text))
                tbVmdName.Text = RTC_Core.GetRandomKey().Substring(0, 5);

            VMD.name = tbVmdName.Text;
            VMD.BigEndian = mi.BigEndian;
            VMD.WordSize = mi.WordSize;


            for (decimal i = nmStartingAddress.Value; i < nmStartingAddress.Value+nmRangeSize.Value; i++)
            {
                if(!Blacklist.Contains(i) && (!cbUsePointerSpacer.Checked || i % nmPointerSpacer.Value == 0))
                    VMD.MemoryPointers.Add(new MemoryPointer(cbSelectedEngine.SelectedItem.ToString(), Convert.ToInt64(i)));
            }

            foreach(decimal i in Added)
            {
                if(i < mi.Size)
                    VMD.MemoryPointers.Add(new MemoryPointer(cbSelectedEngine.SelectedItem.ToString(), Convert.ToInt64(i)));
            }

            if(VMD.MemoryPointers.Count == 0)
            {
                MessageBox.Show("The resulting VMD had no pointers so the operation got cancelled.");
                return;
            }

            if (RTC_MemoryDomains.VmdPool.ContainsKey(VMD.ToString()))
            {
                MessageBox.Show("There is already a VMD with this name in the VMD Pool");
                return;
            }

            RTC_MemoryDomains.VmdPool[VMD.ToString()] = VMD;
            if (RTC_Core.isStandalone)
            {
                RTC_RPC.SendToKillSwitch("FREEZE");
                RTC_NetCore.HugeOperationStart();
                RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_VMD_ADD) { objectValue = VMD });
                RTC_RPC.SendToKillSwitch("UNFREEZE");
                RTC_NetCore.HugeOperationEnd();
            }

            RTC_Core.coreForm.RefreshDomainsAndKeepSelected();

            tbVmdName.Text = "";
            cbSelectedEngine.SelectedIndex = -1;
            cbSelectedEngine.Items.Clear();

            nmStartingAddress.Value = 0;
            nmStartingAddress.Maximum = int.MaxValue;
            nmRangeSize.Maximum = int.MaxValue;
            nmRangeSize.Value = int.MaxValue;

            nmPointerSpacer.Value = 1;
            cbUsePointerSpacer.Checked = false;

            tbCustomAddresses.Text = "";

            lbDomainSizeValue.Text = "######";
            lbEndianTypeValue.Text = "######";
            lbWordSizeValue.Text = "######";


            //send to vmd pool menu
            RTC_Core.vmdPoolForm.RefreshVMDs();
            RTC_Core.coreForm.cbMemoryDomainTool.SelectedIndex = 1;
        }
    }
}
