using System;
using System.Data;
using System.IO;
using CorruptCore;
using RTCV.CorruptCore;
using RTCV.NetCore;

namespace RTC
{
	public static class RTC_Render
	{

		public static void StartRender_NET()
		{
			if (RTC_Render_CorruptCore.RenderType == RTC_Render_CorruptCore.RENDERTYPE.NONE)
				return;

			string Key = "RENDER_" + (RTC_Corruptcore.GetRandomKey());

			switch (RTC_Render_CorruptCore.RenderType)
			{
				case RTC_Render_CorruptCore.RENDERTYPE.WAV:
					RTC_Hooks.BIZHAWK_STARTRECORDAV("wave", RTC_Corruptcore.rtcDir + Path.DirectorySeparatorChar + "RENDEROUTPUT" + Path.DirectorySeparatorChar + Key + ".wav", true);
					break;
				case RTC_Render_CorruptCore.RENDERTYPE.AVI:
					try
					{
						RTC_Hooks.BIZHAWK_STARTRECORDAV("vfwavi", RTC_Corruptcore.rtcDir + Path.DirectorySeparatorChar + "RENDEROUTPUT" + Path.DirectorySeparatorChar + Key + ".avi", true);
					}
					catch (Exception ex)
					{
						throw new Exception("Rendering AVI Failed. \nIf you haven't already, you need to set a codec in bizhawk before starting an AVI Render. \nThe menu for config is at: File -> AVI/WAV -> Config.\n\n" + ex.ToString());
					}

					break;
				case RTC_Render_CorruptCore.RENDERTYPE.MPEG:
					RTC_Hooks.BIZHAWK_STARTRECORDAV("ffmpeg", RTC_Corruptcore.rtcDir + Path.DirectorySeparatorChar + "RENDEROUTPUT" + Path.DirectorySeparatorChar + Key + ".mpg", true);
					break;
			}
		}

		public static void StopRender_NET()
		{
			RTC_Hooks.BIZHAWK_STOPRECORDAV();
		}
	}

}
