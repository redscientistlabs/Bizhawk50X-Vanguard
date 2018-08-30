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
	public partial class RTC_EngineConfig_Form : Form, IAutoColorize
	{

		public RTC_SelectBox_Form mtForm;
		
		public RTC_EngineConfig_Form()
		{
			InitializeComponent();
			LoadLists();

			mtForm = new RTC_SelectBox_Form(new ComponentForm[] {
				S.GET<RTC_VmdNoTool_Form>(),
				S.GET<RTC_VmdPool_Form>(),
				S.GET<RTC_VmdGen_Form>(),
				S.GET<RTC_VmdAct_Form>(),
			});
			mtForm.popoutAllowed = false;

			S.GET<RTC_GeneralParameters_Form>().AnchorToPanel(pnGeneralParameters);
			S.GET<RTC_MemoryDomains_Form>().AnchorToPanel(pnMemoryDomains);
			S.GET<RTC_CorruptionEngine_Form>().AnchorToPanel(pnCorruptionEngine);
			mtForm.AnchorToPanel(pnAdvancedMemoryTools);
		}


		private void LoadLists()
		{
			RTC_EmuCore.LimiterListBindingSource.Clear();
			RTC_EmuCore.ValueListBindingSource.Clear();

			string[] paths = System.IO.Directory.GetFiles(RTC_EmuCore.listsDir);

			paths = paths.OrderBy(x => x).ToArray();

			List<string> hashes = RTC_Filtering.LoadListsFromPaths(paths);
			for (int i = 0; i < hashes.Count; i++)
			{
				string[] _paths = paths[i].Split('\\' , '.');
				RTC_EmuCore.LimiterListBindingSource.Add(new { Text = _paths[_paths.Length - 2], Value = hashes[i] });
				RTC_EmuCore.ValueListBindingSource.Add(new { Text = _paths[_paths.Length - 2], Value = hashes[i] });
			}
		}

		private void cbMemoryDomainTool_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
	}
}
