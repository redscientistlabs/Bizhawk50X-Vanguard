using System;
using System.Data;
using System.IO;
using RTCV.NetCore;

namespace RTC
{
	public static class RTC_Render
	{
		public static RENDERTYPE lastType = RENDERTYPE.NONE;

		public static bool isRendering = false;

		public static void StartRender()
		{
			if (isRendering)
				StopRender();

			isRendering = true;
			StartRender_NET();
		}

		public static void StartRender_NET()
		{
			isRendering = true;
			if (lastType == RENDERTYPE.NONE)
				return;

			string Key = "RENDER_" + (RTC_Corruptcore.GetRandomKey());

			switch (lastType)
			{
				case RENDERTYPE.WAV:
					RTC_Hooks.BIZHAWK_STARTRECORDAV("wave", RTC_Corruptcore.rtcDir + Path.DirectorySeparatorChar + "RENDEROUTPUT" + Path.DirectorySeparatorChar + Key + ".wav", true);
					break;
				case RENDERTYPE.AVI:
					try
					{
						RTC_Hooks.BIZHAWK_STARTRECORDAV("vfwavi", RTC_Corruptcore.rtcDir + Path.DirectorySeparatorChar + "RENDEROUTPUT" + Path.DirectorySeparatorChar + Key + ".avi", true);
					}
					catch (Exception ex)
					{
						throw new Exception("Rendering AVI Failed. \nIf you haven't already, you need to set a codec in bizhawk before starting an AVI Render. \nThe menu for config is at: File -> AVI/WAV -> Config.\n\n" + ex.ToString());
					}

					break;
				case RENDERTYPE.MPEG:
					RTC_Hooks.BIZHAWK_STARTRECORDAV("ffmpeg", RTC_Corruptcore.rtcDir + Path.DirectorySeparatorChar + "RENDEROUTPUT" + Path.DirectorySeparatorChar + Key + ".mpg", true);
					break;
			}

			LocalNetCoreRouter.Route("CORRUPTCORE", "REMOTE_RENDER_STARTED", true);
		}

		public static void setType(string _type)
		{
			switch (_type)
			{
				case "NONE":
					lastType = RENDERTYPE.NONE;
					break;
				case "WAV":
					lastType = RENDERTYPE.WAV;
					break;
				case "AVI":
					lastType = RENDERTYPE.AVI;
					break;
				case "MPEG":
					lastType = RENDERTYPE.MPEG;
					break;
			}

			LocalNetCoreRouter.Route("CORRUPTCORE", "REMOTE_RENDER_SETTYPE", lastType, true);
		}

		public static void StopRender()
		{
			isRendering = false;
			LocalNetCoreRouter.Route("CORRUPTCORE", "REMOTE_RENDER_STOP", true);
		}

		public static void StopRender_NET()
		{
			isRendering = false;
			RTC_Hooks.BIZHAWK_STOPRECORDAV();
		}
	}

	public enum RENDERTYPE
	{
		NONE,
		WAV,
		AVI,
		MPEG,
		LAST,
	}
}
