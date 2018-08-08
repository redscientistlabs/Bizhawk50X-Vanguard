using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_EngineConfig_Form : Form
	{
		
		public RTC_EngineConfig_Form()
		{
			InitializeComponent();
		}

		private void RTC_EC_Form_Load(object sender, EventArgs e)
		{

			RTC_Core.gpForm.AnchorToPanel(pnGeneralParameters);
			RTC_Core.mdForm.AnchorToPanel(pnMemoryDomains);
			RTC_Core.ceForm.AnchorToPanel(pnCorruptionEngine);

			cbMemoryDomainTool.SelectedIndex = 0;
			LoadLists();

		}

		private void LoadLists()
		{
			RTC_Core.filterListsComboSource.Clear();

			string[] paths = System.IO.Directory.GetFiles(RTC_Core.listsDir);

			List<MD5> md5s = RTC_Filtering.LoadListsFromPaths(paths);
			for (int i = 0; i < md5s.Count; i++)
			{
				string[] _paths = paths[i].Split('.');
				RTC_Core.filterListsComboSource.Add(new { Text = _paths[_paths.Length-1], Value = md5s[i] });

			}
		}
		
		private void cbMemoryDomainTool_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComponentForm component = null;

			switch (cbMemoryDomainTool.SelectedItem.ToString())
			{
				case "Virtual Memory Domain Pool":
					component = RTC_Core.vmdPoolForm;
					break;
				case "Virtual Memory Domain Generator":
					component = RTC_Core.vmdGenForm;
					break;
				case "ActiveTable Generator":
					component = RTC_Core.vmdActForm;
					break;


				case "No Tool Selected":
				default:
					component = RTC_Core.vmdNoToolForm;
					break;
			}

			component?.AnchorToPanel(pnAdvancedTool);

		}


	}
}
