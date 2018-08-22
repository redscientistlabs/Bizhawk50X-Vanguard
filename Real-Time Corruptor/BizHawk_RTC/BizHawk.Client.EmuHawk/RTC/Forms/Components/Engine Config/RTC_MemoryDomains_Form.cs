using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_MemoryDomains_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

		public RTC_MemoryDomains_Form()
		{
			InitializeComponent();
		}

		public void SetMemoryDomainsSelectedDomains(string[] _domains)
		{
			lbMemoryDomains_DontExecute_SelectedIndexChanged = true;

			for (int i = 0; i < lbMemoryDomains.Items.Count; i++)
				if (_domains.Contains(lbMemoryDomains.Items[i].ToString()))
					lbMemoryDomains.SetSelected(i, true);
				else
					lbMemoryDomains.SetSelected(i, false);

			lbMemoryDomains_DontExecute_SelectedIndexChanged = false;
			lbMemoryDomains_SelectedIndexChanged(null, null);
		}

		public void SetMemoryDomainsAllButSelectedDomains(string[] _blacklistedDomains)
		{
			lbMemoryDomains_DontExecute_SelectedIndexChanged = true;

			for (
				int i = 0; i < lbMemoryDomains.Items.Count; i++)
				if (_blacklistedDomains.Contains(lbMemoryDomains.Items[i].ToString()))
					lbMemoryDomains.SetSelected(i, false);
				else
					lbMemoryDomains.SetSelected(i, true);

			lbMemoryDomains_DontExecute_SelectedIndexChanged = false;
			lbMemoryDomains_SelectedIndexChanged(null, null);
		}

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			RefreshDomains();

			lbMemoryDomains_DontExecute_SelectedIndexChanged = true;

			for (int i = 0; i < lbMemoryDomains.Items.Count; i++)
				lbMemoryDomains.SetSelected(i, true);

			lbMemoryDomains_DontExecute_SelectedIndexChanged = false;

			lbMemoryDomains_SelectedIndexChanged(null, null);
		}

		private void btnAutoSelectDomains_Click(object sender, EventArgs e)
		{
			RefreshDomains();
			SetMemoryDomainsAllButSelectedDomains(RTC_MemoryDomains.GetBlacklistedDomains());
		}

		public void RefreshDomains()
		{
			RTC_MemoryDomains.RefreshDomains();

			lbMemoryDomains.Items.Clear();
			if (RTC_MemoryDomains.MemoryInterfaces != null)
				lbMemoryDomains.Items.AddRange(RTC_MemoryDomains.MemoryInterfaces.Keys.ToArray());

			if (RTC_MemoryDomains.VmdPool.Count > 0)
				lbMemoryDomains.Items.AddRange(RTC_MemoryDomains.VmdPool.Values.Select(it => it.ToString()).ToArray());
		}


		public void RefreshDomainsAndKeepSelected(string[] overrideDomains = null)
		{
			string[] copy = RTC_MemoryDomains.lastSelectedDomains;

			if (overrideDomains != null)
				copy = overrideDomains;

			RefreshDomains(); //refresh and reload domains

			RTC_MemoryDomains.UpdateSelectedDomains(copy);

			SetMemoryDomainsSelectedDomains(copy);
		}

		public bool lbMemoryDomains_DontExecute_SelectedIndexChanged = false;

		private void lbMemoryDomains_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lbMemoryDomains_DontExecute_SelectedIndexChanged)
				return;

			string[] selectedDomains = lbMemoryDomains.SelectedItems.Cast<string>().ToArray();

			RTC_MemoryDomains.UpdateSelectedDomains(selectedDomains, true);

			//RTC_Restore.SaveRestore();
		}

		private void btnRefreshDomains_Click(object sender, EventArgs e)
		{
			RefreshDomains();
		}

	}
}
