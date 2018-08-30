using System.Media;

namespace RTC
{
	public static class RTC_NetCoreSettings
	{
		public static SoundPlayer[] LoadedSounds = null;

		public static void ChangeNetCoreSettings(string setting)
		{
			if (NetCoreImplementation.isStandaloneUI && S.GET<RTC_SettingsNetCore_Form>().cbNetCoreCommandTimeout.SelectedItem.ToString() == setting)
				return;

			switch (setting)
			{
				case "STANDARD":
					ReturnWatch.maxtries = 0;
					RTC_NetCore.DefaultKeepAliveCounter = 7;
					RTC_NetCore.DefaultNetworkStreamTimeout = 3000;
					RTC_NetCore.DefaultMaxRetries = 666;

					if (NetCoreImplementation.isStandaloneUI)
						S.GET<RTC_Core_Form>().pbAutoKillSwitchTimeout.Maximum = 13;

					break;
				case "LAZY":
					ReturnWatch.maxtries = 0;
					RTC_NetCore.DefaultKeepAliveCounter = 15;
					RTC_NetCore.DefaultNetworkStreamTimeout = 6000;
					RTC_NetCore.DefaultMaxRetries = 2000;

					if (NetCoreImplementation.isStandaloneUI)
						S.GET<RTC_Core_Form>().pbAutoKillSwitchTimeout.Maximum = 20;

					break;

				case "DISABLED":

					ReturnWatch.maxtries = 0;
					RTC_NetCore.DefaultKeepAliveCounter = int.MaxValue;
					RTC_NetCore.DefaultNetworkStreamTimeout = int.MaxValue;
					RTC_NetCore.DefaultMaxRetries = int.MaxValue;

					if (NetCoreImplementation.isStandaloneUI)
						S.GET<RTC_Core_Form>().pbAutoKillSwitchTimeout.Maximum = int.MaxValue;

					break;
				default:
					break;
			}

			RTC_NetCore.KeepAliveCounter = RTC_NetCore.DefaultKeepAliveCounter;
		}

		public static void PlayCrashSound(bool forcePlay = false)
		{
			if (LoadedSounds != null && (forcePlay || S.GET<RTC_ConnectionStatus_Form>().btnStartEmuhawkDetached.Text == "Restart BizHawk"))
				LoadedSounds[RTC_CorruptCore.RND.Next(LoadedSounds.Length)].Play();
		}
	}
}
