using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

namespace RTC
{
	public partial class RTC_SettingsAestethics_Form : ComponentForm
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

		public RTC_SettingsAestethics_Form()
		{
			InitializeComponent();

			this.undockedSizable = false;
		}
		
	}
}
