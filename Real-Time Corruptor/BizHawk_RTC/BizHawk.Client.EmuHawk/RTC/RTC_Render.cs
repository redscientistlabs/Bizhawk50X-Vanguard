using System;

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
			NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_RENDER_START), true);
		}

		public static void StartRender_NET()
		{
			isRendering = true;
			if (lastType == RENDERTYPE.NONE)
				return;

			string Key = "RENDER_" + (RTC_CorruptCore.GetRandomKey());

			if (lastType == RENDERTYPE.WAV)
				RTC_Hooks.BIZHAWK_STARTRECORDAV("wave", RTC_EmuCore.rtcDir + "\\RENDEROUTPUT\\" + Key + ".wav", true);
			else if (lastType == RENDERTYPE.AVI)
			{
				try
				{
					RTC_Hooks.BIZHAWK_STARTRECORDAV("vfwavi", RTC_EmuCore.rtcDir + "\\RENDEROUTPUT\\" + Key + ".avi", true);
				}
				catch (Exception ex)
				{
					throw new Exception("Rendering AVI Failed. \nIf you haven't already, you need to set a codec in bizhawk before starting an AVI Render. \nThe menu for config is at: File -> AVI/WAV -> Config.\n\n" + ex.ToString());
				}
			}
			else if (lastType == RENDERTYPE.MPEG)
				RTC_Hooks.BIZHAWK_STARTRECORDAV("ffmpeg", RTC_EmuCore.rtcDir + "\\RENDEROUTPUT\\" + Key + ".mpg", true);

			NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_RENDER_STARTED));
		}

		public static void setType(string _type)
		{
			if (_type == "NONE")
				lastType = RENDERTYPE.NONE;
			else if (_type == "WAV")
				lastType = RENDERTYPE.WAV;
			else if (_type == "AVI")
				lastType = RENDERTYPE.AVI;
			else if (_type == "MPEG")
				lastType = RENDERTYPE.MPEG;

			NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_RENDER_SETTYPE) { objectValue = lastType });
		}

		public static void StopRender()
		{
			isRendering = false;
			NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_RENDER_STOP));
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
