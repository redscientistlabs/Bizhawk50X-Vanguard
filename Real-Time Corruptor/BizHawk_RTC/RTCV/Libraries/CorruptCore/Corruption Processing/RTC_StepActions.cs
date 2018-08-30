using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GongSolutions.Shell.Interop;
using RTCV.NetCore;

namespace RTC
{
	///Rather than handling everything individually, we have a system here that works on collections of Blastunits
	///In most usage, you're probably only going to have a small number of different lifetime/start time mixtures
	///Rather than operating on every unit individually, we place everything into collections of Blastunits and then operate on them
	///We have four lists and a linked list.
	///preProcess contains all the blastunits as they're queued up
	///buListCollection is the collection of all blast unit lists once they've been filtered into groups of shared StartFrame and Lifetime
	///queuedLifetime is a linked list that contains a sorted version of all the blastunit lists with a limited lifetime that have let to be applied
	///appliedLifetime and appliedInfinite are the two collections where we store what we want to actually be applied
	public static class RTC_StepActions
	{
		private static List<List<BlastUnit>> buListCollection = new List<List<BlastUnit>>();

		private static LinkedList<List<BlastUnit>> queued = new LinkedList<List<BlastUnit>>();
		private static List<List<BlastUnit>> appliedLifetime = new List<List<BlastUnit>>();
		private static List<List<BlastUnit>> appliedInfinite = new List<List<BlastUnit>>();


		public static List<BlastUnit> StoreDataPool = new List<BlastUnit>();

		private static int currentFrame = 0;
		private static int nextFrame = -1;

		private static bool isRunning = false;

		/// <summary>
		/// Don't set this manually
		/// </summary>
		public static int MaxInfiniteBlastUnits
		{
			get { return (int)RTC_CorruptCore.spec["StepActions_MaxInfiniteBlastUnits"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "StepActions_MaxInfiniteBlastUnits", value)); }
		}
		/// <summary>
		/// Don't set this manually
		/// </summary>
		public static bool LockExecution
		{
			get { return (bool)RTC_CorruptCore.spec["StepActions_LockExecution"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "StepActions_LockExecution", value)); }
		}

		public static bool ClearStepActionsOnRewind
		{
			get { return (bool)RTC_CorruptCore.spec["StepActions_ClearStepActionsOnRewind"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "StepActions_ClearStepActionsOnRewind", value)); }
		}

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("CorruptCore");

			partial["StepActions_MaxInfiniteBlastUnits"] = 50;
			partial["StepActions_LockExecution"] = false;
			partial["StepActions_ClearStepActionsOnRewind"] = false;


			return partial;
		}

		public static void ClearStepBlastUnits()
		{
			if (NetCoreImplementation.isStandaloneUI)
			{
				RTC_Command cmd = new RTC_Command(CommandType.REMOTE_SET_STEPACTIONS_CLEARALLBLASTUNITS);
				NetCoreImplementation.SendCommandToBizhawk(cmd);
			}
			else
			{
				//Clean out the working data to prevent memory leaks
				foreach (List<BlastUnit> buList in appliedLifetime)
				{
					foreach (BlastUnit bu in buList)
						bu.Working = null;
				}
				foreach (List<BlastUnit> buList in appliedInfinite)
				{
					foreach (BlastUnit bu in buList)
						bu.Working = null;
				}

				buListCollection = new List<List<BlastUnit>>();
				queued = new LinkedList<List<BlastUnit>>();
				appliedLifetime = new List<List<BlastUnit>>();
				appliedInfinite = new List<List<BlastUnit>>();
				StoreDataPool = new List<BlastUnit>();

				nextFrame = -1;
				currentFrame = 0;
				isRunning = false;
			}
		}

		public static void RemoveExcessInfiniteStepUnits()
		{
			if (LockExecution == true)
				return;

			while (appliedInfinite.Count > RTC_StepActions.MaxInfiniteBlastUnits)
				appliedInfinite.Remove(appliedInfinite[0]);
		}

		public static void SetMaxLifetimeBlastUnits(int value)
		{
			RTC_StepActions.MaxInfiniteBlastUnits = value;
		}


		public static void SetLockExecution(bool value)
		{
			RTC_StepActions.LockExecution = value;
		}

		public static BlastLayer GetRawBlastLayer()
		{
			BlastLayer bl = new BlastLayer();
			foreach (List<BlastUnit> buList in buListCollection)
			{
				foreach (BlastUnit bu in buList)
				{
					bl.Layer.Add(bu);
				}
				
			}

			return bl;
		}

		public static void AddBlastUnit(BlastUnit bu)
		{
			bu.Working.ExecuteFrameQueued = bu.ExecuteFrame + currentFrame;
			//We subtract 1 here as we want lifetime to be exclusive. 1 means 1 apply, not applies 0 > applies 1 > done
			bu.Working.LastFrame = bu.Working.ExecuteFrameQueued + bu.Lifetime - 1;

			List<BlastUnit> collection = buListCollection.FirstOrDefault(it => (it[0].Working.ExecuteFrameQueued == bu.Working.ExecuteFrameQueued) && (it[0].Lifetime == bu.Lifetime) && (it[0].Loop == bu.Loop));
			
			if (collection == null)
			{
				collection = new List<BlastUnit>();
				collection.Add(bu);
				buListCollection.Add(collection);
			}
			else
			{
				if (collection.Contains(bu))
					return;
				collection.Add(bu);
			}
		}

		//TODO OPTIMIZE THIS TO INSERT RATHER THAN REBUILD
		public static void FilterBuListCollection()
		{
			foreach (List<BlastUnit> buList in queued)
				buListCollection.Add(buList);

			queued = new LinkedList<List<BlastUnit>>();

			buListCollection = buListCollection.OrderBy(it => it[0].Working.ExecuteFrameQueued).ToList();


			foreach(List<BlastUnit> buList in buListCollection)
			{
				queued.AddLast(buList);
			}
			//Nuke the list 
			buListCollection = new List<List<BlastUnit>>();

			//There's data so have the execute loop actually do something
			nextFrame = (queued.First())[0].Working.ExecuteFrameQueued;
			isRunning = true;			
		}

		private static void GetStoreBackups()
		{
			foreach (var bu in StoreDataPool)
			{
				bu.StoreBackup();
			}
		}

		private static void CheckApply()
		{ 
			//We need to do this twice because the while loop is vital on the nextFrame being set from the very beginning.
			if (queued.Count == 0)
				return;
			//This will only occur if the queue has something in it due to the check above
			while (currentFrame >= nextFrame)
			{
				List<BlastUnit> buList = queued.First();

				bool dontApply = false;
				//This is our EnteringExecution
				foreach (BlastUnit bu in buList)
				{
					//If it returns false, that means the layer shouldn't apply
					//This is primarily for if a limiter returns false 
					//If this happens, we need to remove it from the pool and then return out
					if (!bu.EnteringExecution())
					{
						queued.RemoveFirst();
						dontApply = true;
						break;
					}
				}

				if (!dontApply)
				{
					//Add it to the infinite pool
					if (buList[0].Lifetime == 0)
					{
						appliedInfinite.Add(buList);
						queued.RemoveFirst();
					}
					//Add it to the Lifetime pool
					else
					{
						appliedLifetime.Add(buList);
						queued.RemoveFirst();
					}
				}

				//Check if the queue is empty
				if (queued.Count == 0)
					return;
				//It's not empty so set the next frame
				nextFrame = (queued.First())[0].Working.ExecuteFrameQueued;
			}
		}
		public static void Execute()
		{
			if (isRunning == false)
				return;

			//Queue everything up
			CheckApply();

			GetStoreBackups();

			//Execute all infinite lifetime units
			foreach (List<BlastUnit> buList in appliedInfinite)
				foreach (BlastUnit bu in buList)
					bu.Execute();

			//Execute all temp units
			List<List<BlastUnit>> itemsToRemove = new List<List<BlastUnit>>();
			foreach (List<BlastUnit> buList in appliedLifetime)
			{
				foreach (BlastUnit bu in buList)
					bu.Execute();
				if (buList[0].Working.LastFrame == currentFrame)
					itemsToRemove.Add(buList);
			}

			bool needsRefilter = false;
			//Remove any temp units that have expired
			foreach (List<BlastUnit> buList in itemsToRemove)
			{
				//Remove it
				appliedLifetime.Remove(buList);

				//Since we already have it filtered as a list, there's no reason to build a new list
				//We just update the ExecuteFrameQueued and LastFrame, then just add it back to buListCollection
				if (buList[0].Loop)
				{
					//Clean out the working data for all units
					foreach (BlastUnit bu in buList)
						bu.Working = new BlastUnitWorkingData();

					//We have to add 1 since currentFrame hasn't been incremented yet
					buList[0].Working.ExecuteFrameQueued = buList[0].ExecuteFrame + currentFrame + 1;
					buList[0].Working.LastFrame = buList[0].Working.ExecuteFrameQueued + buList[0].Lifetime + 1;
					buListCollection.Add(buList);
					needsRefilter = true;
				}
				//If we're not looping, set the working data to null instead of instantiating a new one
				else
				{
					foreach (BlastUnit bu in buList)
					{
						bu.Working = null;
						//Remove it from the store pool
						if (bu.Source == BlastUnitSource.STORE)
							StoreDataPool.Remove(bu);
					}
				}
			}
			//We only call this if there's a loop layer for optimization purposes.
			if(needsRefilter)
				FilterBuListCollection();

			currentFrame++;
		}
	}
}