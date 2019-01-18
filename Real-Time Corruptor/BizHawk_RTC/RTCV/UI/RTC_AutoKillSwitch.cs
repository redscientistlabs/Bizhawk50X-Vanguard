using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetCore;
using RTCV.CorruptCore;
using RTCV.UI;
using UI;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
{

	public static class RTC_AutoKillSwitch
	{
		public static int MaxMissedPulses = 15;

		public static bool Enabled
		{
			get
			{
				if (BoopMonitoringTimer == null)
					return false;
				return BoopMonitoringTimer.Enabled;
			}
			set
			{
				if (value)
					Start();
				else
					Stop();

			}
		}

		private static volatile int pulseCount = MaxMissedPulses;
		private static System.Timers.Timer BoopMonitoringTimer = null;

		public static SoundPlayer[] LoadedSounds = null;

		public static void PlayCrashSound(bool forcePlay = false)
		{
			if (LoadedSounds != null && (forcePlay || S.GET<RTC_ConnectionStatus_Form>()
				.btnStartEmuhawkDetached.Text == "Restart BizHawk"))
				LoadedSounds[RTC_Corruptcore.RND.Next(LoadedSounds.Length)]
					.Play();
		}

		public static void Pulse()
		{
			pulseCount = MaxMissedPulses;
		}

		public static void KillEmulator(string str)
		{
			PlayCrashSound(true);
			switch (str)
			{
				case "KILL":
					Process.Start("KILLDETACHEDRTC.bat");
					break;
				case "KILL + RESTART":
					Process.Start("RESTARTDETACHEDRTC.bat");
					break;
				case "RESTART + RESET":
					Process.Start("RESETDETACHEDRTC.bat");
					break;
			}
		}

		private static void Start()
		{
			pulseCount = MaxMissedPulses;
			BoopMonitoringTimer?.Stop();
			BoopMonitoringTimer = new System.Timers.Timer();
			BoopMonitoringTimer.Interval = 500;
			BoopMonitoringTimer.Elapsed += BoopMonitoringTimer_Tick;
			BoopMonitoringTimer.Start();
		}

		private static void Stop()
		{
			BoopMonitoringTimer?.Stop();
		}

		private static void BoopMonitoringTimer_Tick(object sender, EventArgs e)
		{
			if (!Enabled || UI_VanguardImplementation.connector.netConn.status != NetCore.NetworkStatus.CONNECTED)
				return;

			pulseCount--;

			if(pulseCount < MaxMissedPulses - 1)
				SyncObjectSingleton.FormExecute((o, ea) =>
				{
					S.GET<RTC_Core_Form>().pbAutoKillSwitchTimeout.PerformStep();
				});
			else
				SyncObjectSingleton.FormExecute((o, ea) =>
				{
					S.GET<RTC_Core_Form>().pbAutoKillSwitchTimeout.Value = 0;
				});

			if (pulseCount == 0)
			{
				KillEmulator("KILL + RESTART");
			}
		}
	}
}
