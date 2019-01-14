using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
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
			SetMemoryDomainsAllButSelectedDomains((string[])RTC_Corruptcore.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS.ToString()]);
		}

		public void RefreshDomains()
		{
			lbMemoryDomains.Items.Clear();
			if (RTC_MemoryDomains.MemoryInterfaces != null)
				lbMemoryDomains.Items.AddRange(RTC_MemoryDomains.MemoryInterfaces.Keys.ToArray());

			if (RTC_MemoryDomains.VmdPool.Count > 0)
				lbMemoryDomains.Items.AddRange(RTC_MemoryDomains.VmdPool.Values.Select(it => it.ToString()).ToArray());
		}

		public void RefreshDomainsAndKeepSelected(string[] overrideDomains = null)
		{
			var temp = (string[])RTC_Corruptcore.UISpec["SELECTEDDOMAINS"];
			var oldDomain = lbMemoryDomains.Items;


			if (overrideDomains != null)
				temp = overrideDomains;

			RefreshDomains(); //refresh and reload domains

			if (temp.Length != 0)
			{
				RTC_Corruptcore.UISpec.Update("SELECTEDDOMAINS", temp);
				SetMemoryDomainsSelectedDomains(temp);
			}
			else
			{
				SetMemoryDomainsAllButSelectedDomains((string[])RTC_Corruptcore.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS.ToString()]);
			}
		}

		public bool lbMemoryDomains_DontExecute_SelectedIndexChanged = false;

		private void lbMemoryDomains_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lbMemoryDomains_DontExecute_SelectedIndexChanged)
				return;

			RTC_Corruptcore.UISpec.Update("SELECTEDDOMAINS", lbMemoryDomains.SelectedItems.Cast<string>().ToArray());


			//RTC_Restore.SaveRestore();
		}

		private void btnRefreshDomains_Click(object sender, EventArgs e)
		{
			RefreshDomains();
			RTC_Corruptcore.UISpec.Update("SELECTEDDOMAINS", lbMemoryDomains.SelectedItems.Cast<string>().ToArray());
		}

	}
}
