using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Ceras;
using Newtonsoft.Json;
using RTC.Legacy;

namespace RTC
{
	internal class RTC_Unispec
	{
		public static bool PushRTCSpec()
		{
			return (bool)(RTC_NetcoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_PUSHRTCSPEC) { objectValue = RTC_Corruptcore.RTCSpec.GetPartialSpec() }, true) ?? false);;
		}
		public static bool PushEmuSpec()
		{
			return (bool)(RTC_NetcoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_PUSHEMUSPEC) { objectValue = RTC_EmuCore.EmuSpec.GetPartialSpec() }, true) ?? false);
		}
	}

}
