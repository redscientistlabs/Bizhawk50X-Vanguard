using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
