using System;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_GameProtection
	{
		static Timer t;
		public static int BackupInterval = 30;
		public static bool isRunning = false;

		public static void Start()
		{
			if (t == null)
			{
				t = new Timer();
				t.Tick += new EventHandler(Tick);
			}

			t.Interval = Convert.ToInt32(BackupInterval) * 1000;
			t.Start();

			isRunning = true;

			if (NetCoreImplementation.RemoteRTC.PeerCommandQueue.Count > 0)
				foreach (var item in NetCoreImplementation.RemoteRTC.PeerCommandQueue)
					if (item.Type == CommandType.REMOTE_BACKUPKEY_REQUEST)
						NetCoreImplementation.RemoteRTC.PeerCommandQueue.Remove(item);

		}

		public static void Stop()
		{
			t?.Stop();

			isRunning = false;
		}

		public static void Reset()
		{
			Stop();
			Start();
		}

		private static void Tick(object sender, EventArgs e)
		{
			NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_BACKUPKEY_REQUEST));
		}
	}
}
