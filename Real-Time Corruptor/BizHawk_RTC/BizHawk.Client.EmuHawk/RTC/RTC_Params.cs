using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTC
{
	[Serializable()]
	public class RTC_Params
	{
		List<object> objectList = new List<object>();
		List<Type> typeList = new List<Type>();

		
        //This is an array of pointers for setting/getting variables upon parameter synchronization accross processes
        //The index of refs must match between processes otherwise it will break 
        //(EmuHawk.exe and StandaloneRTC.exe must match versions)

		Ref[] refs = {

			new Ref(() => RTC_Core.SelectedEngine, x => { RTC_Core.SelectedEngine = (CorruptionEngine)x; }),
			new Ref(() => RTC_Core.Intensity, x => { RTC_Core.Intensity = (int)x; }),
			new Ref(() => RTC_Core.ErrorDelay, x => { RTC_Core.ErrorDelay = (int)x; }),
			new Ref(() => RTC_Core.Radius, x => { RTC_Core.Radius = (BlastRadius)x; }),
			
			new Ref(() => RTC_Core.ClearCheatsOnRewind, x => { RTC_Core.ClearCheatsOnRewind = (bool)x; }),
			new Ref(() => RTC_Core.ClearPipesOnRewind, x => { RTC_Core.ClearPipesOnRewind = (bool)x; }),
			new Ref(() => RTC_Core.ExtractBlastLayer, x => { RTC_Core.ExtractBlastLayer = (bool)x; }),
			new Ref(() => RTC_Core.lastOpenRom, x => { RTC_Core.lastOpenRom = (string)x; }),
			new Ref(() => RTC_Core.lastLoaderRom, x => { RTC_Core.lastLoaderRom = (int)x; }),
			new Ref(() => RTC_Core.AutoCorrupt, x => { RTC_Core.AutoCorrupt = (bool)x; }),

            new Ref(() => RTC_NightmareEngine.Algo, x => { RTC_NightmareEngine.Algo = (BlastByteAlgo)x; }),
			new Ref(() => RTC_HellgenieEngine.MaxCheats, x => { RTC_HellgenieEngine.MaxCheats = (int)x; }),
			new Ref(() => RTC_DistortionEngine.MaxAge, x => { RTC_DistortionEngine.MaxAge = (int)x; }),
			new Ref(() => RTC_DistortionEngine.CurrentAge, x => { RTC_DistortionEngine.CurrentAge = (int)x; }),
			new Ref(() => RTC_PipeEngine.MaxPipes, x => { RTC_PipeEngine.MaxPipes = (int)x; }),
			new Ref(() => RTC_PipeEngine.LockPipes, x => { RTC_PipeEngine.LockPipes = (bool)x; }),
			new Ref(() => RTC_PipeEngine.ProcessOnStep, x => { RTC_PipeEngine.ProcessOnStep = (bool)x; }),
            new Ref(() => RTC_PipeEngine.ChainedPipes, x => { RTC_PipeEngine.ChainedPipes = (bool)x; }),
            new Ref(() => RTC_VectorEngine.limiterList, x => { RTC_VectorEngine.limiterList = (string[])x; }),
			new Ref(() => RTC_VectorEngine.valueList, x => { RTC_VectorEngine.valueList = (string[])x; }),

			new Ref(() => RTC_StockpileManager.currentGameSystem, x => { RTC_StockpileManager.currentGameSystem = (string)x; }),
			new Ref(() => RTC_StockpileManager.currentGameName, x => { RTC_StockpileManager.currentGameName = (string)x; }),
			new Ref(() => RTC_StockpileManager.backupedState, x => { RTC_StockpileManager.backupedState = (StashKey)x; }),

			new Ref(() => RTC_MemoryDomains.SelectedDomains, x => { RTC_MemoryDomains.SelectedDomains = (string[])x; }),
			new Ref(() => RTC_MemoryDomains.lastSelectedDomains, x => { RTC_MemoryDomains.lastSelectedDomains = (string[])x; }),
		};


		public RTC_Params()
		{
            //Fills the Params object upon creation
			GetSetParams(true);
		}

		public void Deploy()
		{
            //Has to be manually deployed after received

			GetSetParams(false);

			if (RTC_StockpileManager.backupedState != null)
				RTC_StockpileManager.backupedState.Run();
			else
			{
				RTC_Core.AutoCorrupt = false;
			}
		}


		private void GetSetParams(bool buildObject)
		{
            //Builds the params object  or unwraps the params object from/back to all monitored variables

            for ( int i = 0; i < refs.Length; i++)
				if(buildObject)
					objectList.Add(refs[i].Value);
				else
					refs[i].Value = objectList[i];

		}
		
	}

    
	[Serializable()]
	class Ref //Serializable pointer object 
    {
		private Func<object> getter;
		private Action<object> setter;
		public Ref(Func<object> getter, Action<object> setter)
		{
			this.getter = getter;
			this.setter = setter;
		}
		public object Value
		{
			get { return getter(); }
			set { setter(value); }
		}
	}
}
