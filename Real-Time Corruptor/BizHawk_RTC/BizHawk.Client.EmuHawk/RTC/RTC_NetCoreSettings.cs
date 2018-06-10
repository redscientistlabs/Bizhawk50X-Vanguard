using System.Media;

namespace RTC
{
	public static class RTC_NetCoreSettings
	{
		public static SoundPlayer[] loadedSounds = null;

		public static void changeNetCoreSettings(string setting)
		{
			if (RTC_Core.isStandalone && RTC_Core.sForm.cbNetCoreCommandTimeout.SelectedItem.ToString() == setting)
				return;

			switch (setting)
			{
				case "STANDARD":
					ReturnWatch.maxtries = 0;
					RTC_NetCore.DefaultKeepAliveCounter = 5;
					RTC_NetCore.DefaultNetworkStreamTimeout = 2000;
					RTC_NetCore.DefaultMaxRetries = 666;

					if (RTC_Core.isStandalone)
						RTC_Core.coreForm.pbAutoKillSwitchTimeout.Maximum = 13;

					break;
				case "LAZY":
					ReturnWatch.maxtries = 0;
					RTC_NetCore.DefaultKeepAliveCounter = 15;
					RTC_NetCore.DefaultNetworkStreamTimeout = 6000;
					RTC_NetCore.DefaultMaxRetries = 6666;

					if (RTC_Core.isStandalone)
						RTC_Core.coreForm.pbAutoKillSwitchTimeout.Maximum = 20;

					break;

				case "DISABLED":

					ReturnWatch.maxtries = 0;
					RTC_NetCore.DefaultKeepAliveCounter = int.MaxValue;
					RTC_NetCore.DefaultNetworkStreamTimeout = int.MaxValue;
					RTC_NetCore.DefaultMaxRetries = int.MaxValue;

					if (RTC_Core.isStandalone)
						RTC_Core.coreForm.pbAutoKillSwitchTimeout.Maximum = int.MaxValue;

					break;
			}

			RTC_NetCore.KeepAliveCounter = RTC_NetCore.DefaultKeepAliveCounter;
		}

		public static void PlayCrashSound(bool forcePlay = false)
		{
			if (loadedSounds != null && (forcePlay || RTC_Core.csForm.btnStartEmuhawkDetached.Text == "Restart BizHawk"))
				loadedSounds[RTC_Core.RND.Next(loadedSounds.Length)].Play();
		}
	}
}
