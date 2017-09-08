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

        int currentDomainSize = 0;

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

            cbSelectedMemoryDomain.Items.Clear();
            cbSelectedMemoryDomain.Items.AddRange(RTC_MemoryDomains.MemoryInterfaces.Keys.Where(it => !it.Contains("[V]")).ToArray());

            cbSelectedMemoryDomain.SelectedIndex = 0;
        }

        private void cbSelectedEngine_SelectedIndexChanged(object sender, EventArgs e)
        {



            if (string.IsNullOrWhiteSpace(cbSelectedMemoryDomain.SelectedItem?.ToString()) || !RTC_MemoryDomains.MemoryInterfaces.ContainsKey(cbSelectedMemoryDomain.SelectedItem.ToString()))
            {
                cbSelectedMemoryDomain.Items.Clear();
                return;
            }

            MemoryInterface mi = RTC_MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];

            lbDomainSizeValue.Text = mi.Size.ToString();
            lbWordSizeValue.Text = $"{mi.WordSize*8} bits";
            lbEndianTypeValue.Text = (mi.BigEndian ? "Big" : "Little");

            currentDomainSize = Convert.ToInt32(mi.Size);
        }

        public bool isAddressInRanges(int Address, List<int> Singles, List<int[]> Ranges)
        {
            if (Singles.Contains(Address))
                return true;
        
            foreach(int[] range in Ranges)
            {
                int start = range[0];
                int end = range[1];

                if(Address >= start && Address < end)
                    return true;
            }

            return false;
        }

        private void btnGenerateVMD_Click(object sender, EventArgs e) => GenerateVMD();
        private bool GenerateVMD()
        {
            if (string.IsNullOrWhiteSpace(cbSelectedMemoryDomain.SelectedItem?.ToString()) || !RTC_MemoryDomains.MemoryInterfaces.ContainsKey(cbSelectedMemoryDomain.SelectedItem.ToString()))
            {
                cbSelectedMemoryDomain.Items.Clear();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(tbVmdName.Text) && RTC_MemoryDomains.VmdPool.ContainsKey($"[V]{tbVmdName.Text}"))
            {
                MessageBox.Show("There is already a VMD with this name in the VMD Pool");
                return false;
            }

            string Domain = cbSelectedMemoryDomain.SelectedItem.ToString();
            MemoryInterface mi = RTC_MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];
            VirtualMemoryDomain VMD = new VirtualMemoryDomain();

            List<int> addSingles = new List<int>();
            List<int> removeSingles = new List<int>();

            List<int[]> addRanges = new List<int[]>();
            List<int[]> removeRanges = new List<int[]>();

            foreach (string line in tbCustomAddresses.Lines)
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

                if (lineParts.Length > 1)
                {
                    int start = Convert.ToInt32(lineParts[0]);
                    int end = Convert.ToInt32(lineParts[1]);

                    if (remove)
                        removeRanges.Add(new int[] { start, end });
                    else
                        addRanges.Add(new int[] { start, end });
                }
                else
                {
                    int address = Convert.ToInt32(lineParts[0]);

                    if (remove)
                        removeSingles.Add(address);
                    else
                        addSingles.Add(address);
                }


            }

            if(addRanges.Count == 0 && addSingles.Count == 0)
            {
                //No add range was specified, use entire domain
                int start = 0;
                int end = currentDomainSize;

                for (int i = start; i < end; i++)
                {
                    if (!isAddressInRanges(i, removeSingles, removeRanges))
                        if (!cbUsePointerSpacer.Checked || i % nmPointerSpacer.Value == 0)
                            VMD.MemoryPointers.Add(new MemoryPointer(Domain, i));
                }
            }
            else
            {
                int addressCount = 0;

                int estimatedSize = 0;
                estimatedSize += addSingles.Count;
                foreach (int[] range in addRanges)
                    estimatedSize += (range[1] - range[0]);


                foreach (int[] range in addRanges)
                {
                    int start = range[0];
                    int end = range[1];

                    for (int i = start; i < end; i++)
                    {
                        if (!isAddressInRanges(i, removeSingles, removeRanges))
                            if (!cbUsePointerSpacer.Checked || addressCount % nmPointerSpacer.Value == 0)
                                VMD.MemoryPointers.Add(new MemoryPointer(Domain, i));
                        addressCount++;
                    }
                }

                foreach (int single in addSingles)
                {
                    VMD.MemoryPointers.Add(new MemoryPointer(Domain, single));
                    addressCount++;
                }

            }



            if (string.IsNullOrWhiteSpace(tbVmdName.Text))
                tbVmdName.Text = RTC_Core.GetRandomKey().Substring(0, 5);

            VMD.name = tbVmdName.Text;
            VMD.BigEndian = mi.BigEndian;
            VMD.WordSize = mi.WordSize;


            if(VMD.MemoryPointers.Count == 0)
            {
                MessageBox.Show("The resulting VMD had no pointers so the operation got cancelled.");
                return false;
            }

            RTC_MemoryDomains.VmdPool[VMD.ToString()] = VMD;
            if (RTC_Core.isStandalone)
            {
                RTC_RPC.SendToKillSwitch("FREEZE");
                RTC_NetCore.HugeOperationStart();
                RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_VMD_ADD) { objectValue = VMD.ToData() }, true);
                RTC_RPC.SendToKillSwitch("UNFREEZE");
                RTC_NetCore.HugeOperationEnd();
            }

            RTC_Core.coreForm.RefreshDomainsAndKeepSelected();

            tbVmdName.Text = "";
            cbSelectedMemoryDomain.SelectedIndex = -1;
            cbSelectedMemoryDomain.Items.Clear();

            currentDomainSize = 0;

            nmPointerSpacer.Value = 1;
            cbUsePointerSpacer.Checked = false;

            tbCustomAddresses.Text = "";

            lbDomainSizeValue.Text = "######";
            lbEndianTypeValue.Text = "######";
            lbWordSizeValue.Text = "######";

            //send to vmd pool menu
            RTC_Core.vmdPoolForm.RefreshVMDs();
            RTC_Core.coreForm.cbMemoryDomainTool.SelectedIndex = 1;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return true;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
@"VMD Generator instructions help and examples
-----------------------------------------------
Adding an address range:
50-100
Adding a single address:
55

Removing an address range:
-60-110
Removing a single address:
-66

> If no initial range is specified,
the removals will be done on the entire range.

> Ranges are exclusive, meaning that the last
address is excluded from the range.

> Single added addresses will bypass removal ranges

> Single addresses aren't affected by the
pointer spacer parameter");
        }
    }
}
