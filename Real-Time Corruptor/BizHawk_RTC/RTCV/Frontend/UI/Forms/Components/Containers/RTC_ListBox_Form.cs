using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.ComponentModel;
using RTCV.CorruptCore;

namespace RTC
{
	public partial class RTC_ListBox_Form : ComponentForm
	{
		ComponentForm[] childForms;

		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

		public RTC_ListBox_Form(ComponentForm[] _childForms)
		{
			InitializeComponent();

			this.undockedSizable = false;

			childForms = _childForms;

			lbComponentForms.DisplayMember = "text";
			lbComponentForms.ValueMember = "value";

			foreach (var item in childForms)
				lbComponentForms.Items.Add(new { text = item.Text, value = item });
			

			
		}

		private void lbComponentForms_SelectedIndexChanged(object sender, EventArgs e)
		{
			((lbComponentForms.SelectedItem as dynamic).value as ComponentForm)?.AnchorToPanel(pnTargetComponentForm);
		}

		private void RTC_ListBox_Form_Load(object sender, EventArgs e)
		{
			lbComponentForms.SelectedIndex = 0;
		}
	}
}
