using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System.Windows.Forms;
using BizHawk.Client.EmuHawk;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace RTC
{

	public static class RTC_BlastGeneratorEngine
	{
		public static BlastUnit GetUnit()
		{
			return null;
		}

		public static BlastLayer GetBlastLayer()
		{
			return RTC_Core.bgForm.GenerateBlastLayers();
		}
	}
}
