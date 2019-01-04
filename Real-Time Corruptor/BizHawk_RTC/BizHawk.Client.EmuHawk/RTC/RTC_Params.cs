using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;

namespace RTC
{
	[Serializable]
	public static class RTC_Params
	{
		public static void LoadRTCColor()
		{
			if (RTC_Core.isStandalone || !RTC_Hooks.isRemoteRTC)
			{
				if (IsParamSet("COLOR"))
				{
					string[] bytes = ReadParam("COLOR").Split(',');
					RTC_Core.GeneralColor = Color.FromArgb(Convert.ToByte(bytes[0]), Convert.ToByte(bytes[1]), Convert.ToByte(bytes[2]));
				}
				else
					RTC_Core.GeneralColor = Color.FromArgb(110, 150, 193);

				RTC_Core.SetRTCColor(RTC_Core.GeneralColor);
			}
		}

		public static void SaveRTCColor(Color color)
		{
			SetParam("COLOR", color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString());
		}

		public static void LoadHotkeys()
		{
			if (IsParamSet("RTC_HOTKEYS"))
			{
				//		RTC_Hotkeys.LoadHotkeys(ReadParam("RTC_HOTKEYS"));
			}
		}

		public static void SaveHotkeys()
		{
			//	SetParam("RTC_HOTKEYS", RTC_Hotkeys.SaveHotkeys());
		}

		public static void LoadBizhawkWindowState()
		{
			if (IsParamSet("BIZHAWK_SIZE"))
			{
				string[] size = ReadParam("BIZHAWK_SIZE").Split(',');
				RTC_Hooks.BIZHAWK_GETSET_MAINFORMSIZE = new Size(Convert.ToInt32(size[0]), Convert.ToInt32(size[1]));
				string[] location = ReadParam("BIZHAWK_LOCATION").Split(',');
				RTC_Hooks.BIZHAWK_GETSET_MAINFORMLOCATION = new Point(Convert.ToInt32(location[0]), Convert.ToInt32(location[1]));
			}
		}

		public static void SaveBizhawkWindowState()
		{
			var size = RTC_Hooks.BIZHAWK_GETSET_MAINFORMSIZE;
			var location = RTC_Hooks.BIZHAWK_GETSET_MAINFORMLOCATION;

			SetParam("BIZHAWK_SIZE", $"{size.Width},{size.Height}");
			SetParam("BIZHAWK_LOCATION", $"{location.X},{location.Y}");
		}

		public static void AutoLoadVMDs()
		{
			string currentGame = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETGAMENAME), true);
			SetParam(currentGame.GetHashCode().ToString());
		}

		public static void SetParam(string paramName, string data = null)
		{
			if (data == null)
			{
				if (!IsParamSet(paramName))
					SetParam(paramName, "");
			}
			else
				File.WriteAllText(RTC_Core.paramsDir + "\\" + paramName, data);
		}

		public static void RemoveParam(string paramName)
		{
			if (IsParamSet(paramName))
				File.Delete(RTC_Core.paramsDir + "\\" + paramName);
		}

		public static string ReadParam(string paramName)
		{
			if (IsParamSet(paramName))
				return File.ReadAllText(RTC_Core.paramsDir + "\\" + paramName);

			return null;
		}

		public static bool IsParamSet(string paramName)
		{
			return File.Exists(RTC_Core.paramsDir + "\\" + paramName);
		}
	}

}