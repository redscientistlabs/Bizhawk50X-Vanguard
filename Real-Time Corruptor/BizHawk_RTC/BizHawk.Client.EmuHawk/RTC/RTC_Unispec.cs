using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace RTC
{
	internal class RTC_Unispec
	{
		public static FullSpec RTCSpec;

		private void RegisterSpec()
		{

			PartialSpec specTemplate = new PartialSpec("RTCSpec");


			//Engine Settings
			specTemplate[Spec.CORE_SELECTEDENGINE.ToString()] = CorruptionEngine.NIGHTMARE;
			specTemplate[Spec.CORE_CLEARSTEPACTIONSONREWIND.ToString()] = false;

			specTemplate[Spec.CORE_CUSTOMPRECISION.ToString()] = 0;
			specTemplate[Spec.CORE_INTENSITY.ToString()] = 1;
			specTemplate[Spec.CORE_ERRORDELAY.ToString()] = 1;
			specTemplate[Spec.CORE_RADIUS.ToString()] = BlastRadius.SPREAD;


			specTemplate[Spec.STEP_MAXINFINITEBLASTUNITS.ToString()] = 50;
			specTemplate[Spec.STEP_LOCKEXECUTION.ToString()] = false;


			//RTC Status
			specTemplate[Spec.CORE_EXTRACTBLASTLAYER.ToString()] = false;
			specTemplate[Spec.CORE_LASTOPENROM.ToString()] = null;
			specTemplate[Spec.CORE_LASTLOADERROM.ToString()] = 0;
			specTemplate[Spec.CORE_AUTOCORRUPT.ToString()] = false;


			//RTC Settings
			specTemplate[Spec.CORE_BIZHAWKOSDDISABLED.ToString()] = true;
			specTemplate[Spec.HOOKS_SHOWCONSOLE.ToString()] = false;
			specTemplate[Spec.CORE_DONTCLEANSAVESTATESONQUIT.ToString()] = false;


			//Nightmare Config
			specTemplate[Spec.NIGHTMARE_MINVALUE8BIT.ToString()]  = 0;
			specTemplate[Spec.NIGHTMARE_MAXVALUE16BIT.ToString()] = 0;
			specTemplate[Spec.NIGHTMARE_MAXVALUE32BIT.ToString()] = 0;

			specTemplate[Spec.NIGHTMARE_MAXVALUE8BIT.ToString()]  = 0xFF;
			specTemplate[Spec.NIGHTMARE_MINVALUE16BIT.ToString()] = 0xFFFF;
			specTemplate[Spec.NIGHTMARE_MINVALUE32BIT.ToString()] = 0xFFFFFFFF;

			//Hellgenie Config
			specTemplate[Spec.HELLGENIE_MINVALUE8BIT.ToString()]  =	0;
			specTemplate[Spec.HELLGENIE_MINVALUE16BIT.ToString()] = 0;
			specTemplate[Spec.HELLGENIE_MINVALUE32BIT.ToString()] = 0;

			specTemplate[Spec.HELLGENIE_MAXVALUE8BIT.ToString()]  = 0xFF;
			specTemplate[Spec.HELLGENIE_MAXVALUE16BIT.ToString()] = 0xFFFF;
			specTemplate[Spec.HELLGENIE_MAXVALUE32BIT.ToString()] = 0xFFFFFFFF;


			//Custom Engine Config
			specTemplate[Spec.CUSTOM_DELAY.ToString()] = 0;
			specTemplate[Spec.CUSTOM_LIFETIME.ToString()] = 1;
			specTemplate[Spec.CUSTOM_LOOP.ToString()] = false;

			specTemplate[Spec.CUSTOM_TILTVALUE.ToString()] = 1;

			specTemplate[Spec.CUSTOM_LIMITERLISTHASH.ToString()] = null;
			specTemplate[Spec.CUSTOM_LIMITERTIME.ToString()] = ActionTime.NONE;
			specTemplate[Spec.CUSTOM_LIMITERINVERTED.ToString()] = false;

			specTemplate[Spec.CUSTOM_VALUELISTHASH.ToString()] = null;

			specTemplate[Spec.CUSTOM_MINVALUE8BIT.ToString()] = 0;
			specTemplate[Spec.CUSTOM_MINVALUE16BIT.ToString()]= 0xFF;
			specTemplate[Spec.CUSTOM_MINVALUE32BIT.ToString()] = 0;
			specTemplate[Spec.CUSTOM_MAXVALUE8BIT.ToString()] = 0xFFFF;
			specTemplate[Spec.CUSTOM_MAXVALUE16BIT.ToString()] = 0;
			specTemplate[Spec.CUSTOM_MAXVALUE32BIT.ToString()] = 0xFFFFFFFF;

			specTemplate[Spec.CUSTOM_VALUESOURCE.ToString()] = CustomValueSource.RANDOM;

			specTemplate[Spec.CUSTOM_SOURCE.ToString()] = BlastUnitSource.VALUE;

			specTemplate[Spec.CUSTOM_STOREADDRESS.ToString()] = CustomStoreAddress.RANDOM;
			specTemplate[Spec.CUSTOM_STORETIME.ToString()] = ActionTime.IMMEDIATE;
			specTemplate[Spec.CUSTOM_STORETYPE.ToString()] = StoreType.ONCE;


			specTemplate[Spec.FILTERING_HASH2LIMITERDICO.ToString()] = new Dictionary<string, HashSet<Byte[]>>();
			specTemplate[Spec.FILTERING_HASH2VALUEDICO.ToString()] = new Dictionary<string, List<Byte[]>>();

			specTemplate[Spec.VECTOR_LIMITERLISTHASH.ToString()] = null;
			specTemplate[Spec.VECTOR_VALUELISTHASH.ToString()] = null;

			//Was volatile keep an eye on
			specTemplate[Spec.STOCKPILE_CURRENTSAVESTATEKEY.ToString()] = null;
			specTemplate[Spec.STOCKPILE_CURRENTGAMESYSTEM.ToString()] = String.Empty;
			specTemplate[Spec.STOCKPILE_CURRENTGAMENAME.ToString()] = String.Empty;
			specTemplate[Spec.STOCKPILE_BACKUPEDSTATE.ToString()] = null;

			specTemplate[Spec.MEMORYDOMAINS_SELECTEDDOMAINS.ToString()] = new string[] { };
			specTemplate[Spec.MEMORYDOMAINS_LASTSELECTEDDOMAINS.ToString()] = new string[] { };



			RTCSpec = new FullSpec(specTemplate); //You have to feed a partial spec as a template


			RTCSpec.RegisterUpdateAction((ob, ea) => {

				PartialSpec partial = ea.partialSpec;
				//send partial update via netcore or whatever

			});


			if (RTC_StockpileManager.BackupedState != null)
				RTC_StockpileManager.BackupedState.Run();
			else
			{
				RTC_Core.AutoCorrupt = false;
			}
		}
	}

	public class FullSpec : BaseSpec
	{
		public event EventHandler<SpecUpdateEventArgs> SpecUpdated;
		public virtual void OnSpecUpdated(SpecUpdateEventArgs e) => SpecUpdated?.Invoke(this, e);

		private PartialSpec template = null;
		public string name = "UnnamedSpec";

		public new object this[string key]  //FullSpec is readonly, must update with partials
		{
			get
			{
				if (specDico.ContainsKey(key))
					return specDico[key];
				else
					return null;
			}
		}

		public FullSpec(PartialSpec partialSpec)
		{
			//Creating a FullSpec requires a template
			template = partialSpec;
			name = partialSpec.name;
			Update(template);
		}

		public void RegisterUpdateAction(Action<object, SpecUpdateEventArgs> registrant)
		{
			UnregisterUpdateAction();
			SpecUpdated += registrant.Invoke; //We trick the eventhandler in executing the registrant instead
		}

		public void UnregisterUpdateAction()
		{
			//finds any delegate referencing SpecUpdated and dereferences it

			FieldInfo eventFieldInfo = typeof(FullSpec).GetField("SpecUpdated", BindingFlags.NonPublic | BindingFlags.Instance);
			MulticastDelegate eventInstance = (MulticastDelegate)eventFieldInfo.GetValue(this);
			Delegate[] invocationList = eventInstance?.GetInvocationList() ?? new Delegate[] { };
			MethodInfo eventRemoveMethodInfo = typeof(FullSpec).GetEvent("SpecUpdated").GetRemoveMethod(true);
			foreach (Delegate eventHandler in invocationList)
				eventRemoveMethodInfo.Invoke(this, new object[] { eventHandler });
		}

		public new void Reset()
		{
			base.Reset();

			if (template != null)
				Update(template);
		}

		public void Update(PartialSpec _partialSpec, bool propagate = true)
		{
			if (name != _partialSpec.name)
				throw new Exception("Name mismatch between PartialSpec and FullSpec");

			for (int i = 0; i < _partialSpec.keys.Count; i++)
				base[_partialSpec.keys[i]] = _partialSpec.values[i];

			if (propagate)
				OnSpecUpdated(new SpecUpdateEventArgs() { partialSpec = _partialSpec });
		}

		public void Update(String key, Object value, bool propagate = true)
		{
			//Make a partial spec and pass it into Update(PartialSpec)
			PartialSpec spec = new PartialSpec(name);
			spec[key] = value;
			Update(spec, propagate);
		}

	}

	[Serializable]
	public class PartialSpec : BaseSpec
	{
		public string name;
		public PartialSpec(string _name)
		{
			name = _name;
		}
	}

	[Serializable]
	public abstract class BaseSpec : ISerializable
	{

		[IgnoreDataMember]
		internal Dictionary<string, object> specDico { get; set; } = new Dictionary<string, object>();

		public List<string> keys = new List<string>();
		public List<object> values = new List<object>();

		public object this[string key]
		{
			get
			{

				if (specDico.ContainsKey(key))
					return specDico[key];
				else
					return null;
			}

			set => specDico[key] = value;
		}

		public void Remove(string key)
		{
			if (specDico.ContainsKey(key))
				specDico.Remove(key);
		}

		public void Reset() => specDico.Clear();

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Keys", keys);
			info.AddValue("Values", values);
		}

		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			keys.Clear();
			keys.AddRange(specDico.Keys);

			values.Clear();
			values.AddRange(specDico.Values);

			//If for some ungodly reason the indexes were not to match, this slower code below should fix that.
			/*
            keys.Clear();
            values.Clear();
            foreach (var key in specDico.Keys)
            {
                keys.Add(key);
                values.Add(specDico[key]);
            }
            */
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			for (int i = 0; i < keys.Count; i++)
				specDico[keys[i]] = values[i];
		}

	}

	public class SpecUpdateEventArgs : EventArgs
	{
		public PartialSpec partialSpec = null;
	}
}
