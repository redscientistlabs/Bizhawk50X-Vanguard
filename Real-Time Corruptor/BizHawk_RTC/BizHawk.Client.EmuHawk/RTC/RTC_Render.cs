using BizHawk.Client.EmuHawk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_RENDER_START), true);
		}

		public static void StartRender_NET()
		{
			isRendering = true;
			if (lastType == RENDERTYPE.NONE)
				return;

			string Key = "RENDER_" + (RTC_Core.GetRandomKey());

			if (lastType == RENDERTYPE.WAV)
				GlobalWin.MainForm.RecordAvBase("wave", RTC_Core.rtcDir + "\\RENDEROUTPUT\\" + Key + ".wav", true);
			else if (lastType == RENDERTYPE.AVI)
			{
				try
				{
					GlobalWin.MainForm.RecordAvBase("vfwavi", RTC_Core.rtcDir + "\\RENDEROUTPUT\\" + Key + ".avi", true);
				}
				catch (Exception ex)
				{
					throw new Exception("Rendering AVI Failed. \nIf you haven't already, you need to set a codec in bizhawk before starting an AVI Render. \nThe menu for config is at: File -> AVI/WAV -> Config.\n\n" + ex.ToString());
				}
			}
			else if (lastType == RENDERTYPE.MPEG)
				GlobalWin.MainForm.RecordAvBase("ffmpeg", RTC_Core.rtcDir + "\\RENDEROUTPUT\\" + Key + ".mpg", true);

			RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_RENDER_STARTED));
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

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_RENDER_SETTYPE) { objectValue = lastType });
		}

		public static void StopRender()
		{
			isRendering = false;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_RENDER_STOP));
		}
		public static void StopRender_NET()
		{
			isRendering = false;
			GlobalWin.MainForm.StopAv();
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
