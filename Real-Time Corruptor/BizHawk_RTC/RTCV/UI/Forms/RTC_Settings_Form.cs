using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public partial class RTC_Settings_Form : Form, IAutoColorize
	{

		RTC_ListBox_Form lbForm;

		public RTC_Settings_Form()
		{
			InitializeComponent();

			lbForm = new RTC_ListBox_Form(new ComponentForm[]{
				S.GET<RTC_SettingsGeneral_Form>(),
				S.GET<RTC_SettingsNetCore_Form>(),
			 	S.GET<RTC_SettingsAbout_Form>(),
			})
			{
				popoutAllowed = false
			};

			lbForm.AnchorToPanel(pnListBoxForm);
		}


		private void btnRtcFactoryClean_Click(object sender, EventArgs e)
		{
			Process p = new Process();
			p.StartInfo.FileName = "FactoryCleanDETACHED.bat";
			p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.WorkingDirectory = CorruptCore.CorruptCore.rtcDir;
			p.Start();
		}

		private void RTC_Settings_Form_Load(object sender, EventArgs e)
		{

		}

		private void btnCloseSettings_Click(object sender, EventArgs e)
		{
			S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_Core_Form>().previousForm, false);
		}

		private void btnToggleConsole_Click(object sender, EventArgs e)
		{
			LogConsole.ToggleConsole();
		}

		private void btnDebugInfo_Click(object sender, EventArgs e)
		{
			S.GET<RTCV.NetCore.DebugInfo_Form>().ShowDialog();
		}
	}
}
