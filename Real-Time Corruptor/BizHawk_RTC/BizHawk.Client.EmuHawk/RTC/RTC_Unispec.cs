using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Ceras;

namespace RTC
{
	internal class RTC_Unispec
	{
		public volatile static FullSpec RTCSpec;
		public volatile static FullSpec EmuSpec;

		/**
		 * Register the spec on the rtc side
		 */
		public static void RegisterRTCSpec()
		{
			PartialSpec rtcSpecTemplate = new PartialSpec("RTCSpec");
			//Engine Settings
			rtcSpecTemplate[RTCSPEC.CORE_SELECTEDENGINE.ToString()] = CorruptionEngine.NIGHTMARE;
			rtcSpecTemplate[RTCSPEC.CORE_CLEARSTEPACTIONSONREWIND.ToString()] = false;

			rtcSpecTemplate[RTCSPEC.CORE_CUSTOMPRECISION.ToString()] = 0;
			rtcSpecTemplate[RTCSPEC.CORE_CURRENTPRECISION.ToString()] = 1;
			rtcSpecTemplate[RTCSPEC.CORE_INTENSITY.ToString()] = 1;
			rtcSpecTemplate[RTCSPEC.CORE_ERRORDELAY.ToString()] = 1;
			rtcSpecTemplate[RTCSPEC.CORE_RADIUS.ToString()] = BlastRadius.SPREAD;


			rtcSpecTemplate[RTCSPEC.STEP_MAXINFINITEBLASTUNITS.ToString()] = 50;
			rtcSpecTemplate[RTCSPEC.STEP_LOCKEXECUTION.ToString()] = false;


			//RTC Status
			rtcSpecTemplate[RTCSPEC.CORE_EXTRACTBLASTLAYER.ToString()] = false;
			rtcSpecTemplate[RTCSPEC.CORE_AUTOCORRUPT.ToString()] = false;


			//RTC Settings
			rtcSpecTemplate[RTCSPEC.CORE_BIZHAWKOSDDISABLED.ToString()] = true;
			rtcSpecTemplate[RTCSPEC.HOOKS_SHOWCONSOLE.ToString()] = false;
			rtcSpecTemplate[RTCSPEC.CORE_DONTCLEANSAVESTATESONQUIT.ToString()] = false;


			//Nightmare Config
			rtcSpecTemplate[RTCSPEC.NIGHTMARE_TYPE.ToString()] = NightmareAlgo.RANDOM;

			rtcSpecTemplate[RTCSPEC.NIGHTMARE_MINVALUE8BIT.ToString()]  = 0L;
			rtcSpecTemplate[RTCSPEC.NIGHTMARE_MINVALUE16BIT.ToString()] = 0L;
			rtcSpecTemplate[RTCSPEC.NIGHTMARE_MINVALUE32BIT.ToString()] = 0L;

			rtcSpecTemplate[RTCSPEC.NIGHTMARE_MAXVALUE8BIT.ToString()]  = 0xFFL;
			rtcSpecTemplate[RTCSPEC.NIGHTMARE_MAXVALUE16BIT.ToString()] = 0xFFFFL;
			rtcSpecTemplate[RTCSPEC.NIGHTMARE_MAXVALUE32BIT.ToString()] = 0xFFFFFFFFL;


			rtcSpecTemplate[RTCSPEC.DISTORTION_DELAY.ToString()] = 50;

			//Hellgenie Config
			rtcSpecTemplate[RTCSPEC.HELLGENIE_MINVALUE8BIT.ToString()]  = 0L;
			rtcSpecTemplate[RTCSPEC.HELLGENIE_MINVALUE16BIT.ToString()] = 0L;
			rtcSpecTemplate[RTCSPEC.HELLGENIE_MINVALUE32BIT.ToString()] = 0L;

			rtcSpecTemplate[RTCSPEC.HELLGENIE_MAXVALUE8BIT.ToString()]  = 0xFFL;
			rtcSpecTemplate[RTCSPEC.HELLGENIE_MAXVALUE16BIT.ToString()] = 0xFFFFL;
			rtcSpecTemplate[RTCSPEC.HELLGENIE_MAXVALUE32BIT.ToString()] = 0xFFFFFFFFL;


			//Custom Engine Config
			RTC_CustomEngine.Initialize(rtcSpecTemplate);


			rtcSpecTemplate[RTCSPEC.FILTERING_HASH2LIMITERDICO.ToString()] = new Dictionary<string, HashSet<Byte[]>>();
			rtcSpecTemplate[RTCSPEC.FILTERING_HASH2VALUEDICO.ToString()] = new Dictionary<string, List<Byte[]>>();

			rtcSpecTemplate[RTCSPEC.VECTOR_LIMITERLISTHASH.ToString()] = null;
			rtcSpecTemplate[RTCSPEC.VECTOR_VALUELISTHASH.ToString()] = null;

			//Was volatile keep an eye on
			rtcSpecTemplate[RTCSPEC.STOCKPILE_CURRENTSAVESTATEKEY.ToString()] = null;
			rtcSpecTemplate[RTCSPEC.STOCKPILE_BACKUPEDSTATE.ToString()] = null;

			rtcSpecTemplate[RTCSPEC.MEMORYDOMAINS_SELECTEDDOMAINS.ToString()] = new string[] { };
			rtcSpecTemplate[RTCSPEC.MEMORYDOMAINS_LASTSELECTEDDOMAINS.ToString()] = new string[] { };



			RTCSpec = new FullSpec(rtcSpecTemplate); //You have to feed a partial spec as a template

			
			RTCSpec.RegisterUpdateAction((ob, ea) => {

				PartialSpec partial = ea.partialSpec;

				//Only send the update if we're connected
				if(RTC_Core.RemoteRTC_SupposedToBeConnected)
					RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_PUSHRTCSPECUPDATE) { objectValue = partial }, true);
			});

			if (RTC_Unispec.RTCSpec[RTCSPEC.STOCKPILE_BACKUPEDSTATE.ToString()] != null)
				((StashKey)RTC_Unispec.RTCSpec[RTCSPEC.STOCKPILE_BACKUPEDSTATE.ToString()]).Run();
			else
				RTCSpec.Update(RTCSPEC.CORE_AUTOCORRUPT.ToString(), false);
		}

		public static void RegisterEmuhawkSpec()
		{
			PartialSpec emuSpecTemplate = new PartialSpec("EmuSpec");
			emuSpecTemplate[EMUSPEC.CORE_LASTOPENROM.ToString()] = null;
			emuSpecTemplate[EMUSPEC.CORE_LASTLOADERROM.ToString()] = 0;

			//Was previously volatile?
			emuSpecTemplate[EMUSPEC.STOCKPILE_CURRENTGAMESYSTEM.ToString()] = String.Empty;
			emuSpecTemplate[EMUSPEC.STOCKPILE_CURRENTGAMENAME.ToString()] = String.Empty;

			EmuSpec = new FullSpec(emuSpecTemplate); //You have to feed a partial spec as a template

			EmuSpec.RegisterUpdateAction((ob, ea) => {
				PartialSpec partial = ea.partialSpec;

				//Only send the update if we're connected
				if (RTC_Core.RemoteRTC_SupposedToBeConnected)
					RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_PUSHEMUSPECUPDATE) { objectValue = partial }, true);
			});
		}

		public static bool PushRTCSpec()
		{
			return (bool)(RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_PUSHRTCSPEC) { objectValue = RTCSpec.GetPartialSpec() }, true) ?? false);;
		}
		public static bool PushEmuSpec()
		{
			return (bool)(RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_PUSHEMUSPEC) { objectValue = EmuSpec.GetPartialSpec() }, true) ?? false);
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
				return null;
			}
		}

		public FullSpec(PartialSpec partialSpec)
		{
			//Creating a FullSpec requires a template
			template = partialSpec;
			name = partialSpec.Name;
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
			if (name != _partialSpec.Name)
				throw new Exception("Name mismatch between PartialSpec and FullSpec");

			//For initial
			foreach (var key in _partialSpec.specDico.Keys)
				base[key] = _partialSpec.specDico[key];

			if (propagate)
				OnSpecUpdated(new SpecUpdateEventArgs() { partialSpec = _partialSpec });
		}

		public void Update(String key, Object value, bool propagate = true)
		{
			//Make a partial spec and pass it into Update(PartialSpec)
			if(RTC_Hooks.isRemoteRTC && name == "RTCSpec" && key != RTCSPEC.CORE_AUTOCORRUPT.ToString())
				throw new Exception("Tried updating the RTCSpec from Emuhawk");

			if (RTC_Core.isStandalone && name == "EmuSpec")
				throw new Exception("Tried updating the EmuSpec from StandaloneRTC");

			PartialSpec spec = new PartialSpec(name);
			spec[key] = value;
			Update(spec, propagate);
		}

		public PartialSpec GetPartialSpec()
		{
			PartialSpec p = new PartialSpec(name);
			foreach (var key in specDico.Keys)
			{
				p[key] = base[key];
			}

			return p;
		}
		public void FullUpdate()
		{
			Update(GetPartialSpec());
		}

	}

	[Serializable]
	[Ceras.MemberConfig(TargetMember.All)]
	public class PartialSpec : BaseSpec
	{
		public string Name;
		public PartialSpec(string _name)
		{
			Name = _name;
		}

		public PartialSpec()
		{
		}
		protected PartialSpec(SerializationInfo info, StreamingContext context)
		{
			Name = info.GetString("Name");
		}
	}

	[Serializable]
	[Ceras.MemberConfig(TargetMember.All)]
	public abstract class BaseSpec
	{
		internal Dictionary<string, object> specDico { get; set; } = new Dictionary<string, object>();

        public object this[string key]
        {
            get
            {
                if (specDico.ContainsKey(key))
                    return specDico[key];
                return null;
            }
            set
            {
                if (value == null && !(this is PartialSpec))    // Partials can have null values
                {                                               // A null value means a key removal in the Full Spec
                    if (specDico.ContainsKey(key))
                        specDico.Remove(key);
                }
                else
                    specDico[key] = value;
            }
        }


		public void Reset() => specDico.Clear();

	}

	public class SpecUpdateEventArgs : EventArgs
	{
		public PartialSpec partialSpec = null;
	}
}
