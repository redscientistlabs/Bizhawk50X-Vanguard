using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTCV.CorruptCore;
using RTCV.NetCore;

namespace CorruptCore
{
	public static class RTC_Render_CorruptCore
	{


		public static bool IsRendering
		{
			get => (bool)RTC_Corruptcore.CorruptCoreSpec[RTCSPEC.RENDER_ISRENDERING.ToString()];
			set => RTC_Corruptcore.CorruptCoreSpec.Update(RTCSPEC.RENDER_ISRENDERING.ToString(), value);
		}

		public static RENDERTYPE RenderType
		{
			get => (RENDERTYPE)RTC_Corruptcore.CorruptCoreSpec[RTCSPEC.RENDER_RENDERTYPE.ToString()];
			set => RTC_Corruptcore.CorruptCoreSpec.Update(RTCSPEC.RENDER_RENDERTYPE.ToString(), value);
		}

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");
			partial[RTCSPEC.RENDER_ISRENDERING.ToString()] = false;
			partial[RTCSPEC.RENDER_RENDERTYPE.ToString()] = RENDERTYPE.NONE;
			return partial;
		}

		public static void setType(string _type)
		{
			switch (_type)
			{
				case "NONE":
					RenderType = RENDERTYPE.NONE;
					break;
				case "WAV":
					RenderType = RENDERTYPE.WAV;
					break;
				case "AVI":
					RenderType = RENDERTYPE.AVI;
					break;
				case "MPEG":
					RenderType = RENDERTYPE.MPEG;
					break;
			}
		}


		public static void StartRender()
		{
			if (IsRendering)
				StopRender();

			IsRendering = true; LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_RENDER_START, true);
		}

		public static void StopRender()
		{
			IsRendering = true;
			LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_RENDER_STOP, true);
		}

		public enum RENDERTYPE
		{
			NONE,
			WAV,
			AVI,
			MPEG,
			LAST,
		}
	}
}
