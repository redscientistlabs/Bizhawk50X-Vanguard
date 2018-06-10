namespace RTC
{
	public static class RTC_BlastGeneratorEngine
	{
		public static BlastUnit GetUnit()
		{
			return null;
		}

		public static BlastLayer GetBlastLayer()
		{
			return RTC_Core.bgForm.GenerateBlastLayers();
		}
	}
}
