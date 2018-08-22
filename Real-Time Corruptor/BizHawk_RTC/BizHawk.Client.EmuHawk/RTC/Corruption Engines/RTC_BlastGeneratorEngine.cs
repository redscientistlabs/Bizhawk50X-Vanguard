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
			return S.GET<RTC_BlastGenerator_Form>().GenerateBlastLayers();
		}
	}
}
