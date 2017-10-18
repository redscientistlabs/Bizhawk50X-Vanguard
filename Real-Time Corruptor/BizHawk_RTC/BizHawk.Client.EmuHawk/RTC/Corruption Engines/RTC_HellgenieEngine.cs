using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTC
{
    public static class RTC_HellgenieEngine
    {
        public static int MaxCheats = 50;


        public static BlastCheat GenerateUnit(string _domain, long _address)
        {
            try
            {
				MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(_domain, _address);
				BizHawk.Client.Common.DisplayType _displaytype = BizHawk.Client.Common.DisplayType.Unsigned;

                byte[] _value;
                if (RTC_Core.CustomPrecision == -1)
                    _value = new byte[mdp.WordSize];
                else
                    _value = new byte[RTC_Core.CustomPrecision];

                long safeAddress = _address - (_address % _value.Length);

                for (int i = 0; i < _value.Length; i++)
                    _value[i] = (byte)RTC_Core.RND.Next(255);


                return new BlastCheat(_domain, safeAddress, _displaytype, mdp.BigEndian, _value, true, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong in the RTC Hellgenie Engine. \n" +
                                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
                                ex.ToString());
                return null;
            }
        }

        public static void RemoveExcessCheats()
        {

			RTC_Command cmd = new RTC_Command(CommandType.REMOTE_SET_HELLGENIE_REMOVEEXCESSCHEATS);
			RTC_Core.SendCommandToBizhawk(cmd);

            
        }

        public static void ClearCheats(bool sync = false)
        {
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_HELLGENIE_CLEARALLCHEATS) , sync);
		}
    }
}
