using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;

namespace RTC
{
	[Serializable]
	public class RTC_Params
	{
		List<object> objectList = new List<object>();
		List<Type> typeList = new List<Type>();

		//This is an array of pointers for setting/getting variables upon parameter synchronization accross processes
		//The index of refs must match between processes otherwise it will break
		//(EmuHawk.exe and StandaloneRTC.exe must match versions)

		Ref[] refs = {

			new Ref(() => RTC_StepActions.MaxInfiniteBlastUnits, x => { RTC_StepActions.MaxInfiniteBlastUnits   = (int)x; }),
			new Ref(() => RTC_StepActions.LockExecution, x => { RTC_StepActions.LockExecution = (bool)x; }),

			new Ref(() => RTC_EmuCore.lastOpenRom, x => { RTC_EmuCore.lastOpenRom = (string)x; }),
			new Ref(() => RTC_EmuCore.lastLoaderRom, x => { RTC_EmuCore.lastLoaderRom = (int)x; }),
			new Ref(() => RTC_EmuCore.currentGameSystem, x => { RTC_EmuCore.currentGameSystem = (string)x; }),
			new Ref(() => RTC_EmuCore.currentGameName, x => { RTC_EmuCore.currentGameName = (string)x; }),

			new Ref(() => RTC_EmuCore.OsdDisabled, x => { RTC_EmuCore.OsdDisabled = (bool)x; }),
			new Ref(() => RTC_Hooks.ShowConsole, x => { RTC_Hooks.ShowConsole = (bool)x; }),

			

			new Ref(() => RTC_Filtering.Hash2LimiterDico,   x => { RTC_Filtering.Hash2LimiterDico   = (SerializableDico<string, string[]>)x; }),
			new Ref(() => RTC_Filtering.Hash2ValueDico,     x => { RTC_Filtering.Hash2ValueDico     = (SerializableDico<string, string[]>)x; }),

			
		};

		public RTC_Params()
		{
			//Fills the Params object upon creation
			GetSetLiveParams(true);
		}

		public void Deploy()
		{
			//Has to be manually deployed after received

			GetSetLiveParams(false);

			if (RTC_StockpileManager.backupedState != null)
				RTC_StockpileManager.backupedState.Run();
			else
			{
				RTC_CorruptCore.AutoCorrupt = false;
			}
		}

		private void GetSetLiveParams(bool buildObject)
		{
			//Builds the params object or unwraps the params object from/back to all monitored variables

			for (int i = 0; i < refs.Length; i++)
				if (buildObject)
					objectList.Add(refs[i].Value);
				else
					refs[i].Value = objectList[i];
		}

		public static void LoadRTCColor()
		{
			if (NetCoreImplementation.isStandaloneUI || NetCoreImplementation.isAttached)
			{
				if (IsParamSet("COLOR"))
				{
					string[] bytes = ReadParam("COLOR").Split(',');
					RTC_UICore.generalColor = Color.FromArgb(Convert.ToByte(bytes[0]), Convert.ToByte(bytes[1]), Convert.ToByte(bytes[2]));
				}
				else
					RTC_UICore.generalColor = Color.FromArgb(110, 150, 193);

				RTC_UICore.SetRTCColor(RTC_UICore.generalColor);
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
			string currentGame = (string)NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETGAMENAME), true);
			SetParam((currentGame.GetHashCode().ToString()));
		}

		public static void SetParam(string paramName, string data = null)
		{
			if (data == null)
			{
				if (!IsParamSet(paramName))
					SetParam(paramName, "");
			}
			else
				File.WriteAllText(RTC_EmuCore.paramsDir + "\\" + paramName, data);
		}

		public static void RemoveParam(string paramName)
		{
			if (IsParamSet(paramName))
				File.Delete(RTC_EmuCore.paramsDir + "\\" + paramName);
		}

		public static string ReadParam(string paramName)
		{
			if (IsParamSet(paramName))
				return File.ReadAllText(RTC_EmuCore.paramsDir + "\\" + paramName);

			return null;
		}

		public static bool IsParamSet(string paramName)
		{
			return File.Exists(RTC_EmuCore.paramsDir + "\\" + paramName);
		}
	}

	[Serializable]
	internal class Ref //Serializable pointer object
	{
		private Func<object> getter;
		private Action<object> setter;

		public Ref(Func<object> getter, Action<object> setter)
		{
			this.getter = getter;
			this.setter = setter;
		}

		public object Value
		{
			get => getter();
			set => setter(value);
		}
	}
}
