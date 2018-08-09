using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GongSolutions.Shell.Interop;

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
		private static List<BlastUnit> preProcess = new List<BlastUnit>();
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
		public static int MaxInfiniteBlastUnits = 50;
		/// <summary>
		/// Don't set this manually
		/// </summary>
		public static bool LockExecution;

		public static void ClearStepBlastUnits()
		{
			if (RTC_Core.isStandalone)
			{
				RTC_Command cmd = new RTC_Command(CommandType.REMOTE_SET_STEPACTIONS_CLEARALLBLASTUNITS);
				RTC_Core.SendCommandToBizhawk(cmd);
			}
			else
			{
				preProcess.Clear();
				buListCollection.Clear();
				queued.Clear();
				appliedLifetime.Clear();
				appliedInfinite.Clear();

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
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_STEPACTIONS_MAXLIFETIMEUNITS) { objectValue = value });
		}

		public static void ClearStepActionsOnRewind(bool value)
		{
			RTC_Core.ClearStepActionsOnRewind = value;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_STEPACTIONS_CLEARREWIND) { objectValue = value });
		}
		public static void SetLockExecution(bool value)
		{
			RTC_StepActions.LockExecution = value;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_STEPACTIONS_LOCKEXECUTION) { objectValue = value });
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
			bu.ExecuteFrameQueued = bu.ExecuteFrame + currentFrame;
			//We subtract 1 here as we want lifetime to be exclusive. 1 means 1 apply, not applies 0 > applies 1 > done
			bu.LastFrame = bu.ExecuteFrameQueued + bu.Lifetime - 1;

			List<BlastUnit> collection = buListCollection.FirstOrDefault(it => (it[0].ExecuteFrameQueued == bu.ExecuteFrameQueued) && (it[0].Lifetime == bu.Lifetime) && (it[0].Loop == bu.Loop));
			
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

			queued.Clear();

			buListCollection = buListCollection.OrderBy(it => it[0].ExecuteFrameQueued).ToList();


			foreach(List<BlastUnit> buList in buListCollection)
			{
				queued.AddLast(buList);
			}
			buListCollection.Clear();

			//There's data so have the execute loop actually do something
			nextFrame = (queued.First())[0].ExecuteFrameQueued;
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
				nextFrame = (queued.First())[0].ExecuteFrameQueued;
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
				if (buList[0].LastFrame == currentFrame)
					itemsToRemove.Add(buList);
			}

			bool needsRefilter = false;
			//Remove any temp units that have expired
			foreach (List<BlastUnit> buList in itemsToRemove)
			{
				//Remove it
				appliedLifetime.Remove(buList);

				//Add any that loop back to the starting pool.
				//Since we already have it filtered as a list, there's no reason to build a new list
				//We just update the ExecuteFrameQueued and LastFrame, then just add it back to buListCollection
				if(buList[0].Loop)
				{
					//We have to add 1 since currentFrame hasn't been incremented yet
					buList[0].ExecuteFrameQueued = buList[0].ExecuteFrame + currentFrame + 1; 
					buList[0].LastFrame = buList[0].ExecuteFrameQueued + buList[0].Lifetime + 1;
					buListCollection.Add(buList);
					needsRefilter = true;
				}
				
			}
			//We only call this if there's a loop layer for optimization purposes.
			if(needsRefilter)
				FilterBuListCollection();

			currentFrame++;
		}
	}
}