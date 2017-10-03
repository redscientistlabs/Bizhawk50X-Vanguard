using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using BizHawk.Client.EmuHawk;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace RTC
{
    public static class RTC_FreezeEngine
    {
        
        public static BlastCheat GenerateUnit(string _domain, long _address)
        {
            try
            {
                /*
                BizHawk.Client.Common.DisplayType _displaytype = BizHawk.Client.Common.DisplayType.Unsigned;
                bool _bigEndian = false;
                int _value = 0;
                long Address = 0;
				MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(RTC_MemoryDomains.MainDomain, _address);
                */

                MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(_domain, _address);
                BizHawk.Client.Common.DisplayType _displaytype = BizHawk.Client.Common.DisplayType.Unsigned;

                int _value = mdp.PeekByte(_address);

                return new BlastCheat(_domain, _address, _displaytype, mdp.BigEndian, _value, true, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong in the RTC Freeze Engine. \n" +
                                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
                                ex.ToString());
                return null;
            }
        }


    }
}
