using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizHawk.Client.EmuHawk;
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

		private static int currentFrame = 0;
		private static int nextFrame = -1;

		private static bool isRunning = false;

		public static int MaxLifetimeBlastUnits = 50;

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
			while (appliedInfinite.Count > RTC_HellgenieEngine.MaxCheats)
				appliedInfinite.Remove(appliedInfinite[0]);
		}

		public static void SetMaxLifetimeBlastUnits(int value)
		{
			RTC_StepActions.MaxLifetimeBlastUnits = value;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_STEPACTIONS_MAXLIFETIMEUNITS) { objectValue = value });
		}

		public static void ClearStepActionsOnRewind(bool value)
		{
				RTC_Core.ClearStepActionsOnRewind = value;
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_STEPACTIONS_CLEARREWIND) { objectValue = value });
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
			bu.ApplyFrameQueued = bu.ApplyFrame + currentFrame;
			bu.LastFrame = bu.ApplyFrameQueued + bu.Lifetime;

			List<BlastUnit> collection = buListCollection.FirstOrDefault(it => (it[0].ApplyFrameQueued == bu.ApplyFrameQueued) && (it[0].Lifetime == bu.Lifetime));
			
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

			buListCollection = buListCollection.OrderBy(it => it[0].ApplyFrameQueued).ToList();


			foreach(List<BlastUnit> buList in buListCollection)
			{
				queued.AddLast(buList);
			}

			//There's data so have the execute loop actually do something
			isRunning = true;			
		}


		private static void CheckApply()
		{
			//If there's nothing to dequeue, return
			if (currentFrame < nextFrame)
				return;

			//This will only occur if the queue has something in it due to the check above
			while (currentFrame >= nextFrame)
			{
				//Make sure there's something actually in the queue.
				//We do this here rather than above so we break out of the while loop once the queue has been emptied
				if (queued.Count == 0)
					return;

				nextFrame = (queued.First())[0].ApplyFrameQueued;

				List<BlastUnit> buList = queued.First();
				//Add it to the infinite pool
				if (buList[0].Lifetime == -1)
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

				//Call EnteringExecution so any BlastUnits that need to do something before they start executing can
				//This could be optimized by only running it if the BlastUnits are of a type that needs this run.
				foreach(BlastUnit bu in buList)
					bu.EnteringExecution();

			}
		}
		public static void Execute()
		{
			if (isRunning == false)
				return;

			//Queue everything up
			CheckApply();

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
			//Remove any temp units that have expired
			foreach (List<BlastUnit> buList in itemsToRemove)
			{
				//Clean up
				buList[0].LastFrame = -1;
				//Remove it
				appliedLifetime.Remove(buList);
			}
			currentFrame++;
		}
	}
}