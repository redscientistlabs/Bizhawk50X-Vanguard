using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
	public static class RTC_Params
	{

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

		public static void AutoLoadVMDs()
		{
			string currentGame = (string)LocalNetCoreRouter.Route("VANGUARD", "REMOTE_KEY_GETGAMENAME", true);
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
				File.WriteAllText(RTC_Corruptcore.paramsDir + Path.DirectorySeparatorChar + paramName, data);
		}

		public static void RemoveParam(string paramName)
		{
			if (IsParamSet(paramName))
				File.Delete(RTC_Corruptcore.paramsDir + Path.DirectorySeparatorChar + paramName);
		}

		public static string ReadParam(string paramName)
		{
			if (IsParamSet(paramName))
				return File.ReadAllText(RTC_Corruptcore.paramsDir + Path.DirectorySeparatorChar + paramName);

			return null;
		}

		public static bool IsParamSet(string paramName)
		{
			return File.Exists(RTC_Corruptcore.paramsDir + Path.DirectorySeparatorChar + paramName);
		}
	}

}