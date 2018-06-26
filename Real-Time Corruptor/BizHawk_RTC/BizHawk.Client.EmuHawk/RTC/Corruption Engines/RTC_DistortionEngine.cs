using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_DistortionEngine
	{
		public static int MaxAge = 50;
		public static int CurrentAge = 0;
		public static Queue<BlastUnit> AllDistortionBytes = new Queue<BlastUnit>();

		public static BlastUnit GetUnit()
		{
			return CurrentAge >= MaxAge ? AllDistortionBytes.Dequeue() : null;
		}

		public static void AddUnit(BlastUnit bu)
		{
			AllDistortionBytes.Enqueue(bu);
		}

		public static BlastUnit GenerateUnit(string domain, long address)
		{
			// Randomly selects a memory operation according to the selected algorithm

			try
			{
				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);
				BlastByteType Type = BlastByteType.SET;

				byte[] value = RTC_Core.CustomPrecision == -1 ? new byte[mdp.WordSize] : new byte[RTC_Core.CustomPrecision];

				for (int i = 0; i < value.Length; i++)
					value[i] = 1;

				long safeAddress = address - (address % value.Length);

				BlastByte bb = new BlastByte(domain, safeAddress, Type, value, mdp.BigEndian, true);
				return bb.GetBackup();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC Distortion Engine. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
								ex.ToString());
				return null;
			}
		}

		public static void Resync()
		{
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_DISTORTION_RESYNC));
		}
	}
}
