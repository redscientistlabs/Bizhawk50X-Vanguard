using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using CorruptCore;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
	public static class RTC_Params
	{
		//TODO remove this is not used


		public static void LoadHotkeys()
		{
			if (NetCore.Params.IsParamSet("RTC_HOTKEYS"))
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
			string currentGame = (string)RTC_Corruptcore.VanguardSpec[VSPEC.GAMENAME.ToString()];
			NetCore.Params.SetParam(currentGame.GetHashCode().ToString());
		}

	}

}