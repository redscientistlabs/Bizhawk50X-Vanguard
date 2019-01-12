using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
{
	public partial class RTC_VmdGen_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

		long currentDomainSize = 0;

		public RTC_VmdGen_Form()
		{
			InitializeComponent();
		}

		private void numericUpDown2_ValueChanged(object sender, EventArgs e)
		{
		}

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected();

			cbSelectedMemoryDomain.Items.Clear();
			cbSelectedMemoryDomain.Items.AddRange(RTC_MemoryDomains.MemoryInterfaces.Keys.Where(it => !it.Contains("[V]")).ToArray());

			cbSelectedMemoryDomain.SelectedIndex = 0;
		}

		private void cbSelectedMemoryDomain_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(cbSelectedMemoryDomain.SelectedItem?.ToString()) || !RTC_MemoryDomains.MemoryInterfaces.ContainsKey(cbSelectedMemoryDomain.SelectedItem.ToString()))
			{
				cbSelectedMemoryDomain.Items.Clear();
				return;
			}

			MemoryInterface mi = RTC_MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];

			lbDomainSizeValue.Text = "0x" + mi.Size.ToString("X");
			lbWordSizeValue.Text = $"{mi.WordSize * 8} bits";
			lbEndianTypeValue.Text = (mi.BigEndian ? "Big" : "Little");

			currentDomainSize = Convert.ToInt64(mi.Size);
		}

		public long SafeStringToLong(string input)
		{
			if (input.ToUpper().Contains("0X"))
				return long.Parse(input.Substring(2), NumberStyles.HexNumber);
			else
				return long.Parse(input, NumberStyles.HexNumber);
		}

		private void btnGenerateVMD_Click(object sender, EventArgs e) => GenerateVMD();

		private bool GenerateVMD()
		{
			//Verify they want to continue if the domain is larger than 32MB
			if (currentDomainSize > 0x2000000)
			{
				DialogResult result = MessageBox.Show("The domain you have selected is larger than 32MB\n The domain size is " + (currentDomainSize / 1024) + "MB.\n Are you sure you want to continue?", "Large Domain Detected", MessageBoxButtons.YesNo);
				if (result == DialogResult.No)
					return false;
			}

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

			MemoryInterface mi = RTC_MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];
			VirtualMemoryDomain VMD = new VirtualMemoryDomain();
			VmdPrototype proto = new VmdPrototype();

			proto.GenDomain = cbSelectedMemoryDomain.SelectedItem.ToString();

			if (string.IsNullOrWhiteSpace(tbVmdName.Text))
				proto.VmdName = RTC_Corruptcore.GetRandomKey();
			else
				proto.VmdName = tbVmdName.Text;

			proto.BigEndian = mi.BigEndian;
			proto.WordSize = mi.WordSize;
			proto.Padding = 0;

			if (cbUsePointerSpacer.Checked && nmPointerSpacer.Value > 1)
				proto.PointerSpacer = Convert.ToInt64(nmPointerSpacer.Value);
			else
				proto.PointerSpacer = 1;

			if (cbUsePadding.Checked && nmPadding.Value > 0)
			{
				proto.Padding = Convert.ToInt64(nmPadding.Value);
			}

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
					long start = SafeStringToLong(lineParts[0]);
					long end = SafeStringToLong(lineParts[1]);

					if (end >= currentDomainSize)
						end = Convert.ToInt64(currentDomainSize - 1);

					if (remove)
						proto.RemoveRanges.Add(new long[] {start, end});
					else
						proto.AddRanges.Add(new long[] { start, end });
				}
				else
				{
					long address = SafeStringToLong(lineParts[0]);

					if (address < currentDomainSize)
					{
						if (remove)
							proto.RemoveSingles.Add(address);
						else
							proto.AddSingles.Add(address);
					}

				}
			}

			if (proto.AddRanges.Count == 0 && proto.AddSingles.Count == 0)
			{
				//No add range was specified, use entire domain
				proto.AddRanges.Add(new long[] { 0, (currentDomainSize > long.MaxValue ? long.MaxValue : Convert.ToInt64(currentDomainSize)) });
			}

			VMD = proto.Generate();

			if (VMD.PointerAddresses.Count == 0)
			{
				MessageBox.Show("The resulting VMD had no pointers so the operation got cancelled.");
				return false;
			}

			RTC_MemoryDomains.AddVMD(VMD);

			tbVmdName.Text = "";
			cbSelectedMemoryDomain.SelectedIndex = -1;
			cbSelectedMemoryDomain.Items.Clear();

			currentDomainSize = 0;

			nmPointerSpacer.Value = 2;
			cbUsePointerSpacer.Checked = false;

			tbCustomAddresses.Text = "";

			lbDomainSizeValue.Text = "######";
			lbEndianTypeValue.Text = "######";
			lbWordSizeValue.Text = "######";

			//send to vmd pool menu
			S.GET<RTC_VmdPool_Form>().RefreshVMDs();

			//Selects back the VMD Pool menu
			foreach(var item in S.GET<RTC_EngineConfig_Form>().mtForm.cbSelectBox.Items)
				if(((dynamic)item).value is RTC_VmdPool_Form)
				{
					S.GET<RTC_EngineConfig_Form>().mtForm.cbSelectBox.SelectedItem = item;
					break;
				}


			return true;
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			MessageBox.Show(
@"VMD Generator instructions help and examples
-----------------------------------------------
Adding an address range:
5F-FF
Adding a single address:
5F

Removing an address range:
-6D-110
Removing a single address:
-6D

> If no initial range is specified,
the removals will be done on the entire range.

> Ranges are exclusive, meaning that the last
address is excluded from the range.

> Single added addresses will bypass removal ranges

> Single addresses aren't affected by the
pointer spacer parameter");
		}

		private void RTC_VmdGen_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
				e.Cancel = true;
				this.RestoreToPreviousPanel();
				return;
			}
		}

		private void RTC_VmdGen_Form_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && (sender as ComponentForm).FormBorderStyle == FormBorderStyle.None)
			{
				Point locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);
				ContextMenuStrip columnsMenu = new ContextMenuStrip();
				columnsMenu.Items.Add("Detach to window", null, new EventHandler((ob, ev) =>
				{
					(sender as ComponentForm).SwitchToWindow();
				}));
				columnsMenu.Show(this, locate);
			}
		}

	}
}
