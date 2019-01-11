using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using CorruptCore;
using UI;
using static UI.UI_Extensions.S;

namespace UI
{
	public partial class RTC_EngineConfig_Form : Form, UI_Extensions.IAutoColorize
	{
		public RTC_SelectBox_Form mtForm;
		
		public RTC_EngineConfig_Form()
		{
			InitializeComponent();
			LoadLists();

			mtForm = new RTC_SelectBox_Form(new UI_Extensions.ComponentForm[] {
				GET<RTC_VmdNoTool_Form>(),
				GET<RTC_VmdPool_Form>(),
				GET<RTC_VmdGen_Form>(),
				GET<RTC_VmdAct_Form>(),
				GET<RTC_ListGen_Form>(),
			})
			{
				popoutAllowed = false
			};

			GET<RTC_GeneralParameters_Form>().AnchorToPanel(pnGeneralParameters);
			GET<RTC_MemoryDomains_Form>().AnchorToPanel(pnMemoryDomains);
			GET<RTC_CorruptionEngine_Form>().AnchorToPanel(pnCorruptionEngine);
			mtForm.AnchorToPanel(pnAdvancedMemoryTools);
		}


		public void LoadLists()
		{
			RTC_UICore.LimiterListBindingSource.Clear();
			RTC_UICore.ValueListBindingSource.Clear();

			string[] paths = System.IO.Directory.GetFiles(RTC_Corruptcore.listsDir);

			paths = paths.OrderBy(x => x).ToArray();

			List<string> hashes = RTC_Filtering.LoadListsFromPaths(paths);
			for (int i = 0; i < hashes.Count; i++)
			{
				string[] _paths = paths[i].Split('\\' , '.');
				RegisterListInUI(_paths[_paths.Length - 2], hashes[i]);
			}
		}

		public void RegisterListInUI(string name, string hash)
		{
			RTC_UICore.LimiterListBindingSource.Add(new ComboBoxItem<String>(name, hash));
			RTC_UICore.ValueListBindingSource.Add((new ComboBoxItem<String>(name, hash)));
		}
	}
}
