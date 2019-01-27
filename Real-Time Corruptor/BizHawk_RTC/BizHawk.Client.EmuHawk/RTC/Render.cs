using System;
using System.Data;
using System.IO;
using RTCV.CorruptCore;
using RTCV.NetCore;

namespace Vanguard
{
	public static class Render
	{

		public static void StartRender_NET()
		{
			if (RTCV.CorruptCore.Render.RenderType == RTCV.CorruptCore.Render.RENDERTYPE.NONE)
				return;

			string Key = "RENDER_" + (CorruptCore.GetRandomKey());

			switch (RTCV.CorruptCore.Render.RenderType)
			{
				case RTCV.CorruptCore.Render.RENDERTYPE.WAV:
					Hooks.BIZHAWK_STARTRECORDAV("wave", CorruptCore.rtcDir + Path.DirectorySeparatorChar + "RENDEROUTPUT" + Path.DirectorySeparatorChar + Key + ".wav", true);
					break;
				case RTCV.CorruptCore.Render.RENDERTYPE.AVI:
					try
					{
						Hooks.BIZHAWK_STARTRECORDAV("vfwavi", CorruptCore.rtcDir + Path.DirectorySeparatorChar + "RENDEROUTPUT" + Path.DirectorySeparatorChar + Key + ".avi", true);
					}
					catch (Exception ex)
					{
						throw new Exception("Rendering AVI Failed. \nIf you haven't already, you need to set a codec in bizhawk before starting an AVI Render. \nThe menu for config is at: File -> AVI/WAV -> Config.\n\n" + ex.ToString());
					}

					break;
				case RTCV.CorruptCore.Render.RENDERTYPE.MPEG:
					Hooks.BIZHAWK_STARTRECORDAV("ffmpeg", CorruptCore.rtcDir + Path.DirectorySeparatorChar + "RENDEROUTPUT" + Path.DirectorySeparatorChar + Key + ".mpg", true);
					break;
			}
		}

		public static void StopRender_NET()
		{
			Hooks.BIZHAWK_STOPRECORDAV();
		}
	}

}
