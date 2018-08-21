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

		public RTC_SelectBox_Form mtForm;
		
		public RTC_EngineConfig_Form()
		{
			InitializeComponent();
			LoadLists();

			mtForm = new RTC_SelectBox_Form(new ComponentForm[] {
				RTC_Core.vmdNoToolForm,
				RTC_Core.vmdPoolForm,
				RTC_Core.vmdGenForm,
				RTC_Core.vmdActForm,
			});
			mtForm.popoutAllowed = false;

			RTC_Core.gpForm.AnchorToPanel(pnGeneralParameters);
			RTC_Core.mdForm.AnchorToPanel(pnMemoryDomains);
			RTC_Core.ceForm.AnchorToPanel(pnCorruptionEngine);
			mtForm.AnchorToPanel(pnAdvancedMemoryTools);
		}


		private void LoadLists()
		{
			RTC_Core.LimiterListBindingSource.Clear();
			RTC_Core.ValueListBindingSource.Clear();

			string[] paths = System.IO.Directory.GetFiles(RTC_Core.listsDir);

			paths = paths.OrderBy(x => x).ToArray();

			List<string> hashes = RTC_Filtering.LoadListsFromPaths(paths);
			for (int i = 0; i < hashes.Count; i++)
			{
				string[] _paths = paths[i].Split('\\' , '.');
				RTC_Core.LimiterListBindingSource.Add(new { Text = _paths[_paths.Length - 2], Value = hashes[i] });
				RTC_Core.ValueListBindingSource.Add(new { Text = _paths[_paths.Length - 2], Value = hashes[i] });
			}
		}

		private void cbMemoryDomainTool_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
	}
}
