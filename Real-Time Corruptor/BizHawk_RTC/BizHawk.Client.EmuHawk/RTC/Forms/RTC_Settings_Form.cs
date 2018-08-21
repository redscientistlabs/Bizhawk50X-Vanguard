using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_Settings_Form : Form
	{

		RTC_ListBox_Form lbForm;

		public RTC_Settings_Form()
		{
			InitializeComponent();

			lbForm = new RTC_ListBox_Form(new ComponentForm[]{
				RTC_Core.sgForm,
				RTC_Core.sncForm,
				RTC_Core.saForm,
			});
			lbForm.popoutAllowed = false;

			lbForm.AnchorToPanel(pnListBoxForm);
		}


		private void btnRtcFactoryClean_Click(object sender, EventArgs e)
		{
			Process.Start($"FactoryClean{(RTC_Core.isStandalone ? "DETACHED" : "ATTACHED")}.bat");
		}

		private void RTC_Settings_Form_Load(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			RTC_Core.coreForm.ShowPanelForm(RTC_Core.coreForm.previousForm, false);
		}





		private void button2_Click(object sender, EventArgs e)
		{
			Form form = new RTC_Test_Form();
			form.Show();
		}

	}
}
