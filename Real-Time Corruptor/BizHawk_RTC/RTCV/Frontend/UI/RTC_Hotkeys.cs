//Incomplete. Needs to be re-done as the library in use took exclusive control of the keys which was a big no-no

/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using RTC;
using RTC.Shortcuts;

namespace RTC
{
	public static class RTC_Hotkeys
	{
		private static HotkeyBinder _hotkeyBinder = new HotkeyBinder();
		public static Dictionary<string, Hotkey> name2HotkeyDico = new Dictionary<string, Hotkey>();

		#region HOTKEYDEFINITIONS

		public static Hotkey REMOTE_HOTKEY_MANUALBLAST			= null;
		public static Hotkey REMOTE_HOTKEY_AUTOCORRUPTTOGGLE	= null;
		public static Hotkey REMOTE_HOTKEY_ERRORDELAYDECREASE	= null;
		public static Hotkey REMOTE_HOTKEY_ERRORDELAYINCREASE	= null;
		public static Hotkey REMOTE_HOTKEY_INTENSITYDECREASE	= null;
		public static Hotkey REMOTE_HOTKEY_INTENSITYINCREASE	= null;
		public static Hotkey REMOTE_HOTKEY_GHLOADCORRUPT		= null;
		public static Hotkey REMOTE_HOTKEY_GHCORRUPT			= null;
		public static Hotkey REMOTE_HOTKEY_GHLOAD				= null;
		public static Hotkey REMOTE_HOTKEY_GHSAVE				= null;
		public static Hotkey REMOTE_HOTKEY_GHSTASHTOSTOCKPILE	= null;
		public static Hotkey REMOTE_HOTKEY_BLASTRAWSTASH		= null;
		public static Hotkey REMOTE_HOTKEY_SENDRAWSTASH			= null;
		public static Hotkey REMOTE_HOTKEY_BLASTLAYERTOGGLE		= null;
		public static Hotkey REMOTE_HOTKEY_BLASTLAYERREBLAST	= null;

		#endregion HOTKEYDEFINITIONS

		public static void InitializeHotkeySystem()
		{
			//AddPreDefinedHotkeysToList();
		}

		public static void AddPreDefinedHotkeysToList()
		{
		}

		/*
		public static void AssignAllHotkeyBindings()
		{
			_hotkeyBinder.Bind(REMOTE_HOTKEY_MANUALBLAST)?.To(()		=> NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_MANUALBLAST)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_AUTOCORRUPTTOGGLE)?.To(()	=> NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_AUTOCORRUPTTOGGLE)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_ERRORDELAYDECREASE)?.To(()	=> NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_ERRORDELAYDECREASE)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_INTENSITYDECREASE)?.To(()	=> NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_INTENSITYDECREASE)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_INTENSITYINCREASE)?.To(()	=> NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_INTENSITYINCREASE)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_GHLOADCORRUPT)?.To(()		=> NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHLOADCORRUPT)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_GHCORRUPT)?.To(()			=> NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHCORRUPT)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_GHLOAD)?.To(()				=> NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHLOAD)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_GHSAVE)?.To(()				=> NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHSAVE)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_GHSTASHTOSTOCKPILE)?.To(()	=> NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHSTASHTOSTOCKPILE)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_BLASTRAWSTASH)?.To(()		=> NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTRAWSTASH)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_SENDRAWSTASH)?.To(()		=> NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_SENDRAWSTASH)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_BLASTRAWSTASH)?.To(()		=> NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTRAWSTASH)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_BLASTLAYERTOGGLE)?.To(()	=> NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTLAYERTOGGLE)));
			_hotkeyBinder.Bind(REMOTE_HOTKEY_BLASTLAYERREBLAST)?.To(()	=> NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTLAYERREBLAST)));
		}	*/
/*
public static void AssignStaticHotkeyBinding(Hotkey key, string command)
{
	_hotkeyBinder.Bind(key).To(() => NetCoreImplementation.SendCommandToRTC(new RTC_Command((CommandType)Enum.Parse(typeof(CommandType),command))));
	name2HotkeyDico[command] = key;

	RTC_Params.SaveHotkeys();
}

public static string SaveHotkeys()
{
	string data = "";
	foreach (var hotkey in name2HotkeyDico)
	{
		data = data + $"{hotkey.Value},";
	}
	return data;
}

//The save works like this. Modifier,Key,Name, . There's always a trailing , so you have to subtract by 1 when iterating through. Increment by 3 every time
public static string LoadHotkeys(string data)
{
	string[] _data = data.Split(',');
	for (int i = 0; i < _data.Length-1; i = i + 3)
	{
		Modifiers modifier = (Modifiers)Enum.Parse(typeof(Modifiers), _data[i].ToString());
		Keys keys = (Keys)Enum.Parse(typeof(Keys), _data[i + 1].ToString());
		string command = _data[i + 2].ToString();

		Hotkey key = new Hotkey(modifier,keys,command);

		AssignStaticHotkeyBinding(key, _data[i + 2].ToString());
	}
	return data;
}
}
}
/*
switch (trigger)
	{
		default:
			return false;

		case "Manual Blast":

			break;

		case "Auto-Corrupt":
			NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_AUTOCORRUPTTOGGLE));
			break;

		case "Error Delay--":
			NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_ERRORDELAYDECREASE));
			break;

		case "Error Delay++":
			NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_ERRORDELAYINCREASE));
			break;

		case "Intensity--":
			NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_INTENSITYDECREASE));
			break;

		case "Intensity++":
			NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_INTENSITYINCREASE));
			break;

		case "GH Load and Corrupt":
			watch = System.Diagnostics.Stopwatch.StartNew();
			NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHLOADCORRUPT));
			break;

		case "GH Just Corrupt":
			watch = System.Diagnostics.Stopwatch.StartNew();
			NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHCORRUPT));
			break;

		case "GH Load":
			watch = System.Diagnostics.Stopwatch.StartNew();
			NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHLOAD));
			break;

		case "GH Save":
			watch = System.Diagnostics.Stopwatch.StartNew();
			NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHSAVE));
			break;

		case "Stash->Stockpile":
			NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHSTASHTOSTOCKPILE));
			break;

		case "Blast+RawStash":
			NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTRAWSTASH));
			break;

		case "Send Raw to Stash":
			NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_SENDRAWSTASH));
			break;

		case "BlastLayer Toggle":
			NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTLAYERTOGGLE));
			break;

		case "BlastLayer Re-Blast":
			NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTLAYERREBLAST));
			break;
	}
*/
