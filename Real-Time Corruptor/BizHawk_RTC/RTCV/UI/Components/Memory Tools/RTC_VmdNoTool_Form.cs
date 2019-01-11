using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using UI;
using static UI.UI_Extensions;


namespace UI
{
	public partial class RTC_VmdNoTool_Form : UI_Extensions.ComponentForm, UI_Extensions.IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

		public RTC_VmdNoTool_Form()
		{
			InitializeComponent();

			popoutAllowed = false;
		}

		
	}
}
