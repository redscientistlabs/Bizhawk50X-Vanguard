using System;
using System.Data;
using System.IO;
using RTCV.CorruptCore;
using RTCV.NetCore;

namespace RTCV.BizhawkVanguard
{
	public static class BizhawkRender
	{

		public static void StartRender_NET()
		{
			if (Render.RenderType == Render.RENDERTYPE.NONE)
				return;

			string Key = "RENDER_" + (RtcCore.GetRandomKey());

			switch (Render.RenderType)
			{
				case Render.RENDERTYPE.WAV:
					Hooks.BIZHAWK_STARTRECORDAV("wave", Path.Combine(RtcCore.RtcDir,"RENDEROUTPUT", Key + ".wav"), true);
					break;
				case Render.RENDERTYPE.AVI:
					try
					{
						Hooks.BIZHAWK_STARTRECORDAV("vfwavi", Path.Combine(RtcCore.RtcDir, "RENDEROUTPUT", Key + ".avi"), true);
					}
					catch (Exception ex)
					{
						throw new Exception("Rendering AVI Failed. \nIf you haven't already, you need to set a codec in bizhawk before starting an AVI Render. \nThe menu for config is at: File -> AVI/WAV -> Config.\n\n" + ex.ToString());
					}

					break;
				case Render.RENDERTYPE.MPEG:
					Hooks.BIZHAWK_STARTRECORDAV("ffmpeg", Path.Combine(RtcCore.RtcDir, "RENDEROUTPUT", Key + ".mpg"), true);
					break;
			}
		}

		public static void StopRender_NET()
		{
			Hooks.BIZHAWK_STOPRECORDAV();
		}
	}

}
