﻿using BizHawk.Emulation.Common;

namespace BizHawk.Emulation.Cores.Calculators
{
	public partial class TI83 : ISettable<TI83.TI83Settings, object>
	{
		private TI83Settings _settings;

		public TI83Settings GetSettings()
		{
			return _settings.Clone();
		}

		public PutSettingsDirtyBits PutSettings(TI83Settings o)
		{
			_settings = o;
			return PutSettingsDirtyBits.None;
		}

		public object GetSyncSettings()
		{
			return null;
		}

		public PutSettingsDirtyBits PutSyncSettings(object o)
		{
			return PutSettingsDirtyBits.None;
		}

		public class TI83Settings
		{
			public uint BGColor { get; set; } = 0x889778;
			public uint ForeColor { get; set; } = 0x36412D;

			public TI83Settings Clone()
			{
				return (TI83Settings)MemberwiseClone();
			}
		}
	}
}
