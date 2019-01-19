using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using CorruptCore;
using Newtonsoft.Json;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
	public static class RTC_Corruptcore
	{
		//General RTC Values
		public static string RtcVersion = "3.35a";

		public static Random RND = new Random();
		public static bool Attached = false;

		public static List<ProblematicProcess> ProblematicProcesses;

		public static System.Windows.Forms.Timer KillswitchTimer = new System.Windows.Forms.Timer();

		//Directories
		public static string bizhawkDir = Directory.GetCurrentDirectory();

		public static string rtcDir = bizhawkDir + Path.DirectorySeparatorChar + "RTC" + Path.DirectorySeparatorChar;
		public static string workingDir = rtcDir + Path.DirectorySeparatorChar + "WORKING" + Path.DirectorySeparatorChar;
		public static string assetsDir = rtcDir + Path.DirectorySeparatorChar + "ASSETS" + Path.DirectorySeparatorChar;
		public static string paramsDir = rtcDir + Path.DirectorySeparatorChar + "PARAMS" + Path.DirectorySeparatorChar;
		public static string listsDir = rtcDir + Path.DirectorySeparatorChar + "LISTS" + Path.DirectorySeparatorChar;


		public static bool AllowCrossCoreCorruption
		{
			get => (bool)CorruptCoreSpec[RTCSPEC.CORE_ALLOWCROSSCORECORRUPTION.ToString()];
			set => CorruptCoreSpec.Update(RTCSPEC.CORE_ALLOWCROSSCORECORRUPTION.ToString(), value);
		}

		public static CorruptionEngine SelectedEngine
		{
			get => (CorruptionEngine)CorruptCoreSpec[RTCSPEC.CORE_SELECTEDENGINE.ToString()];
			set => CorruptCoreSpec.Update(RTCSPEC.CORE_SELECTEDENGINE.ToString(), value);
		}

		public static int CurrentPrecision
		{
			get => (int)CorruptCoreSpec[RTCSPEC.CORE_CURRENTPRECISION.ToString()];
			set => CorruptCoreSpec.Update(RTCSPEC.CORE_CURRENTPRECISION.ToString(), value);
		}

		public static int Intensity
		{
			get => (int)CorruptCoreSpec[RTCSPEC.CORE_INTENSITY.ToString()];
			set => CorruptCoreSpec.Update(RTCSPEC.CORE_INTENSITY.ToString(), value);
		}

		public static int ErrorDelay
		{
			get => (int)CorruptCoreSpec[RTCSPEC.CORE_ERRORDELAY.ToString()];
			set => CorruptCoreSpec.Update(RTCSPEC.CORE_ERRORDELAY.ToString(), value);
		}

		public static BlastRadius Radius
		{
			get => (BlastRadius)CorruptCoreSpec[RTCSPEC.CORE_RADIUS.ToString()];
			set => CorruptCoreSpec.Update(RTCSPEC.CORE_RADIUS.ToString(), value);
		}

		public static bool AutoCorrupt
		{
			get => (bool)CorruptCoreSpec[RTCSPEC.CORE_AUTOCORRUPT.ToString()];
			set => CorruptCoreSpec.Update(RTCSPEC.CORE_AUTOCORRUPT.ToString(), value);
		}

		public static bool DontCleanSavestatesOnQuit
		{
			get => (bool)CorruptCoreSpec[RTCSPEC.CORE_DONTCLEANSAVESTATESONQUIT.ToString()];
			set => CorruptCoreSpec.Update(RTCSPEC.CORE_DONTCLEANSAVESTATESONQUIT.ToString(), value);
		}

		public static bool ShowConsole
		{
			get => (bool)CorruptCoreSpec[RTCSPEC.CORE_SHOWCONSOLE.ToString()];
			set => CorruptCoreSpec.Update(RTCSPEC.CORE_SHOWCONSOLE.ToString(), value);
		}

		public static bool RerollAddress
		{
			get => (bool)CorruptCoreSpec[RTCSPEC.CORE_REROLLADDRESS.ToString()];
			set => CorruptCoreSpec.Update(RTCSPEC.CORE_REROLLADDRESS.ToString(), value);
		}

		public static bool RerollSourceAddress
		{
			get => (bool)CorruptCoreSpec[RTCSPEC.CORE_REROLLSOURCEADDRESS.ToString()];
			set => CorruptCoreSpec.Update(RTCSPEC.CORE_REROLLSOURCEADDRESS.ToString(), value);
		}

		public static bool ExtractBlastlayer
		{
			get => (bool)CorruptCoreSpec[RTCSPEC.CORE_EXTRACTBLASTLAYER.ToString()];
			set => CorruptCoreSpec.Update(RTCSPEC.CORE_EXTRACTBLASTLAYER.ToString(), value);
		}

		public static bool BizhawkOsdDisabled
		{
			get => (bool)CorruptCoreSpec[RTCSPEC.CORE_BIZHAWKOSDDISABLED.ToString()];
			set => CorruptCoreSpec.Update(RTCSPEC.CORE_BIZHAWKOSDDISABLED.ToString(), value);
		}

		public static volatile FullSpec CorruptCoreSpec;
		public static volatile FullSpec VanguardSpec;
		public static volatile FullSpec UISpec;
		public static bool IsStandaloneUI;
		public static bool IsEmulatorSide;


		public static void StartUISide()
		{
			RegisterCorruptcoreSpec();
			IsStandaloneUI = true;
		}
		public static void StartEmuSide()
		{
			if (!Attached)
			{
				KillswitchTimer.Interval = 250;
				KillswitchTimer.Tick += KillswitchTimer_Tick;
				KillswitchTimer.Start();
			}
			IsEmulatorSide = true;
		}

		private static void KillswitchTimer_Tick(object sender, EventArgs e)
		{
			LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.KILLSWITCH_PULSE);
		}

		/**
		* Register the spec on the rtc side
		*/
		public static void RegisterCorruptcoreSpec()
		{
			PartialSpec rtcSpecTemplate = new PartialSpec("RTCSpec");

			//Engine Settings
			rtcSpecTemplate.Insert(RTC_Corruptcore.getDefaultPartial());
			rtcSpecTemplate.Insert(RTC_NightmareEngine.getDefaultPartial());
			rtcSpecTemplate.Insert(RTC_HellgenieEngine.getDefaultPartial());
			rtcSpecTemplate.Insert(RTC_DistortionEngine.getDefaultPartial());

			//Custom Engine Config with Nightmare Engine
			RTC_CustomEngine.InitTemplate_NightmareEngine(rtcSpecTemplate);

			rtcSpecTemplate.Insert(RTC_StepActions.getDefaultPartial());
			rtcSpecTemplate.Insert(RTC_Filtering.getDefaultPartial());
			rtcSpecTemplate.Insert(RTC_VectorEngine.getDefaultPartial());
			rtcSpecTemplate.Insert(RTC_MemoryDomains.getDefaultPartial());
			rtcSpecTemplate.Insert(RTC_StockpileManager_EmuSide.getDefaultPartial());
			rtcSpecTemplate.Insert(RTC_Render_CorruptCore.getDefaultPartial());


			CorruptCoreSpec = new FullSpec(rtcSpecTemplate, !RTC_Corruptcore.Attached); //You have to feed a partial spec as a template


			CorruptCoreSpec.SpecUpdated += (o, e) =>
			{
				PartialSpec partial = e.partialSpec;
				if(IsStandaloneUI)
					LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHCORRUPTCORESPECUPDATE, partial, true);
				else
					LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_PUSHCORRUPTCORESPECUPDATE, partial, true);
			};

			/*
			if (RTC_StockpileManager.BackupedState != null)
				RTC_StockpileManager.BackupedState.Run();
			else
				CorruptCoreSpec.Update(RTCSPEC.CORE_AUTOCORRUPT.ToString(), false);
				*/
		}

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");


			partial[RTCSPEC.CORE_ALLOWCROSSCORECORRUPTION.ToString()] = CorruptionEngine.NIGHTMARE;
			partial[RTCSPEC.CORE_SELECTEDENGINE.ToString()] = CorruptionEngine.NIGHTMARE;

			partial[RTCSPEC.CORE_CURRENTPRECISION.ToString()] = 1;
			partial[RTCSPEC.CORE_INTENSITY.ToString()] = 1;
			partial[RTCSPEC.CORE_ERRORDELAY.ToString()] = 1;
			partial[RTCSPEC.CORE_RADIUS.ToString()] = BlastRadius.SPREAD;

			partial[RTCSPEC.CORE_EXTRACTBLASTLAYER.ToString()] = false;
			partial[RTCSPEC.CORE_AUTOCORRUPT.ToString()] = false;

			partial[RTCSPEC.CORE_BIZHAWKOSDDISABLED.ToString()] = true;
			partial[RTCSPEC.CORE_DONTCLEANSAVESTATESONQUIT.ToString()] = false;
			partial[RTCSPEC.CORE_SHOWCONSOLE.ToString()] = false;


			if (RTC_Params.IsParamSet("REROLL_ADDRESS"))
				partial[RTCSPEC.CORE_REROLLADDRESS.ToString()] = (RTC_Params.ReadParam("REROLL_ADDRESS") == "true");
			else
				partial[RTCSPEC.CORE_REROLLADDRESS.ToString()] = false;

			if (RTC_Params.IsParamSet("REROLL_SOURCEADDRESS"))
				partial[RTCSPEC.CORE_REROLLSOURCEADDRESS.ToString()] = (RTC_Params.ReadParam("REROLL_SOURCEADDRESS") == "true");
			else
				partial[RTCSPEC.CORE_REROLLSOURCEADDRESS.ToString()] = false;

			return partial;
		}


		public static void DownloadProblematicProcesses()
		{
			//Windows does the big dumb: part 11
			WebRequest.DefaultWebProxy = null;

			string LocalPath = RTC_Corruptcore.paramsDir + "\\BADPROCESSES";
			string json = "";
			try
			{
				if (File.Exists(LocalPath))
				{
					DateTime lastModified = File.GetLastWriteTime(LocalPath);
					if (lastModified.Date == DateTime.Today)
						return;
				}
				using (HttpClient client = new HttpClient())
				{
					client.Timeout = TimeSpan.FromMilliseconds(5000);
					//Using .Result makes it synchronous
					json = client.GetStringAsync("http://redscientist.com/software/rtc/ProblematicProcesses.json").Result;
				}
				File.WriteAllText(LocalPath, json);
			}
			catch (Exception ex)
			{
				if (ex is WebException)
				{
					//Couldn't download the new one so just fall back to the old one if it's there
					Console.WriteLine(ex.ToString());
					if (File.Exists(LocalPath))
					{
						try
						{
							json = File.ReadAllText(LocalPath);
						}
						catch (Exception _ex)
						{
							Console.WriteLine("Couldn't read BADPROCESSES\n\n" + _ex.ToString());
							return;
						}
					}
					else
						return;
				}
				else
				{
					Console.WriteLine(ex.ToString());
				}
			}

			try
			{
				ProblematicProcesses = JsonConvert.DeserializeObject<List<ProblematicProcess>>(json);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				if (File.Exists(LocalPath))
					File.Delete(LocalPath);
			}
		}

		//Checks if any problematic processes are found
		public static bool Warned = false;
		public static void CheckForProblematicProcesses()
		{
			Console.WriteLine(DateTime.Now + "Entering CheckForProblematicProcesses");
			if (Warned || ProblematicProcesses == null)
				return;

			try
			{
				var processes = Process.GetProcesses().Select(it => $"{it.ProcessName.ToUpper()}").OrderBy(x => x)
					.ToArray();

				//Warn based on loaded processes
				foreach (var item in ProblematicProcesses)
				{
					if (processes.Contains(item.Name))
					{
						MessageBox.Show(item.Message, "Incompatible Program Detected!");
						Warned = true;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return;
			}
			finally
			{
				Console.WriteLine(DateTime.Now + "Exiting CheckForProblematicProcesses");
			}
		}


		public static BlastUnit GetBlastUnit(string _domain, long _address, int precision)
		{
			//Will generate a blast unit depending on which Corruption Engine is currently set.
			//Some engines like Distortion may not return an Unit depending on the current state on things.

			BlastUnit bu = null;

			switch (RTC_Corruptcore.SelectedEngine)
			{
				case CorruptionEngine.NIGHTMARE:
					bu = RTC_NightmareEngine.GenerateUnit(_domain, _address, precision);
					break;
				case CorruptionEngine.HELLGENIE:
					bu = RTC_HellgenieEngine.GenerateUnit(_domain, _address, precision);
					break;
				case CorruptionEngine.DISTORTION:
					bu = RTC_DistortionEngine.GenerateUnit(_domain, _address, precision);
					break;
				case CorruptionEngine.FREEZE:
					bu = RTC_FreezeEngine.GenerateUnit(_domain, _address, precision);
					break;
				case CorruptionEngine.PIPE:
					bu = RTC_PipeEngine.GenerateUnit(_domain, _address, precision);
					break;
				case CorruptionEngine.VECTOR:
					bu = RTC_VectorEngine.GenerateUnit(_domain, _address);
					break;
				case CorruptionEngine.CUSTOM:
					bu = RTC_CustomEngine.GenerateUnit(_domain, _address, precision);
					break;
				case CorruptionEngine.NONE:
					return null;
			}

			return bu;
		}

		//Generates or applies a blast layer using one of the multiple BlastRadius algorithms

		public static BlastLayer GenerateBlastLayer(string[] selectedDomains)
		{
			string Domain = null;
			long MaxAddress = -1;
			long RandomAddress = -1;
			BlastUnit bu;
			BlastLayer bl;

			try
			{
				if (RTC_Corruptcore.SelectedEngine == CorruptionEngine.BLASTGENERATORENGINE)
				{
					//It will query a BlastLayer generated by the Blast Generator
					bl = RTC_BlastGeneratorEngine.GetBlastLayer();
					if (bl == null)
						//We return an empty blastlayer so when it goes to apply it, it doesn't find a null blastlayer and try and apply to the domains which aren't enabled resulting in an exception
						return new BlastLayer();
					else
						return bl;
				}
				else
				{
					bl = new BlastLayer();

					if (selectedDomains == null || selectedDomains.Count() == 0)
						return null;

					// Capping intensity at engine-specific maximums

					int _Intensity = RTC_Corruptcore.Intensity; //general RTC intensity

					if ((RTC_Corruptcore.SelectedEngine == CorruptionEngine.HELLGENIE ||
							RTC_Corruptcore.SelectedEngine == CorruptionEngine.FREEZE ||
							RTC_Corruptcore.SelectedEngine == CorruptionEngine.PIPE) &&
						_Intensity > RTC_StepActions.MaxInfiniteBlastUnits)
						_Intensity = RTC_StepActions.MaxInfiniteBlastUnits; //Capping for cheat max

					switch (RTC_Corruptcore.Radius) //Algorithm branching
					{
						case BlastRadius.SPREAD: //Randomly spreads all corruption bytes to all selected domains

							for (int i = 0; i < _Intensity; i++)
							{
								Domain = selectedDomains[RTC_Corruptcore.RND.Next(selectedDomains.Length)];

								MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;
								RandomAddress = RTC_Corruptcore.RND.RandomLong(MaxAddress - 1);

								bu = GetBlastUnit(Domain, RandomAddress, RTC_Corruptcore.CurrentPrecision);
								if (bu != null)
									bl.Layer.Add(bu);
							}

							break;

						case BlastRadius.CHUNK: //Randomly spreads the corruption bytes in one randomly selected domain

							Domain = selectedDomains[RTC_Corruptcore.RND.Next(selectedDomains.Length)];

							MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;

							for (int i = 0; i < _Intensity; i++)
							{
								RandomAddress = RTC_Corruptcore.RND.RandomLong(MaxAddress - 1);

								bu = GetBlastUnit(Domain, RandomAddress, RTC_Corruptcore.CurrentPrecision);
								if (bu != null)
									bl.Layer.Add(bu);
							}

							break;

						case BlastRadius.BURST: // 10 shots of 10% chunk

							for (int j = 0; j < 10; j++)
							{
								Domain = selectedDomains[RTC_Corruptcore.RND.Next(selectedDomains.Length)];

								MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;

								for (int i = 0; i < (int)((double)_Intensity / 10); i++)
								{
									RandomAddress = RTC_Corruptcore.RND.RandomLong(MaxAddress - 1);

									bu = GetBlastUnit(Domain, RandomAddress, RTC_Corruptcore.CurrentPrecision);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;

						case BlastRadius.NORMALIZED: // Blasts based on the size of the largest selected domain. Intensity =  Intensity / (domainSize[largestdomain]/domainSize[currentdomain])

							//Find the smallest domain and base our normalization around it
							//Domains aren't IComparable so I used keys

							long[] domainSize = new long[selectedDomains.Length];
							for (int i = 0; i < selectedDomains.Length; i++)
							{
								Domain = selectedDomains[i];
								domainSize[i] = RTC_MemoryDomains.GetInterface(Domain).Size;
							}
							//Sort the arrays
							Array.Sort(domainSize, selectedDomains);

							for (int i = 0; i < selectedDomains.Length; i++)
							{
								Domain = selectedDomains[i];

								//Get the intensity divider. The size of the largest domain divided by the size of the current domain
								long normalized = ((domainSize[selectedDomains.Length - 1] / (domainSize[i])));

								for (int j = 0; j < (_Intensity / normalized); j++)
								{
									MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;
									RandomAddress = RTC_Corruptcore.RND.RandomLong(MaxAddress - 1);

									bu = GetBlastUnit(Domain, RandomAddress, RTC_Corruptcore.CurrentPrecision);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;

						case BlastRadius.PROPORTIONAL: //Blasts proportionally based on the total size of all selected domains

							long totalSize = selectedDomains.Select(it => RTC_MemoryDomains.GetInterface(it).Size).Sum(); //Gets the total size of all selected domains

							long[] normalizedIntensity = new long[selectedDomains.Length]; //matches the index of selectedDomains
							for (int i = 0; i < selectedDomains.Length; i++)
							{   //calculates the proportionnal normalized Intensity based on total selected domains size
								double proportion = (double)RTC_MemoryDomains.GetInterface(selectedDomains[i]).Size / (double)totalSize;
								normalizedIntensity[i] = Convert.ToInt64((double)_Intensity * proportion);
							}

							for (int i = 0; i < selectedDomains.Length; i++)
							{
								Domain = selectedDomains[i];

								for (int j = 0; j < normalizedIntensity[i]; j++)
								{
									MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;
									RandomAddress = RTC_Corruptcore.RND.RandomLong(MaxAddress - 1);

									bu = GetBlastUnit(Domain, RandomAddress, RTC_Corruptcore.CurrentPrecision);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;

						case BlastRadius.EVEN: //Evenly distributes the blasts through all selected domains

							for (int i = 0; i < selectedDomains.Length; i++)
							{
								Domain = selectedDomains[i];

								for (int j = 0; j < (_Intensity / selectedDomains.Length); j++)
								{
									MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;
									RandomAddress = RTC_Corruptcore.RND.RandomLong(MaxAddress - 1);

									bu = GetBlastUnit(Domain, RandomAddress, RTC_Corruptcore.CurrentPrecision);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;

						case BlastRadius.NONE: //Shouldn't ever happen but handled anyway
							return null;
					}

					if (bl.Layer.Count == 0)
						return null;
					else
						return bl;
				}
			}
			catch (Exception ex)
			{
				string additionalInfo = "";

				if (RTC_MemoryDomains.GetInterface(Domain) == null)
				{
					additionalInfo = "Unable to get an interface to the selected memory domain! Try clicking the Auto-Select Domains button to refresh the domains!\n\n";
				}

				DialogResult dr = MessageBox.Show("Something went wrong in the RTC Core. \n" +
					additionalInfo +
					"This is an RTC error, so you should probably send this to the RTC devs.\n\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
					(RTC_Corruptcore.AutoCorrupt ? ">> STOP AUTOCORRUPT ?.\n\n" : "") +
					$"domain:{Domain?.ToString()} maxaddress:{MaxAddress.ToString()} randomaddress:{RandomAddress.ToString()} \n\n" +
					ex.ToString(), "Error", (RTC_Corruptcore.AutoCorrupt ? MessageBoxButtons.YesNo : MessageBoxButtons.OK));

				if (dr == DialogResult.Yes || dr == DialogResult.OK)
				{
					RTC_Corruptcore.AutoCorrupt = false;
					LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.ERROR_DISABLE_AUTOCORRUPT);
				}

				return null;
			}
		}

		public static BlastTarget GetBlastTarget()
		{
			//Standalone version of BlastRadius SPREAD

			string Domain = null;
			long MaxAddress = -1;
			long RandomAddress = -1;

			string[] _selectedDomains = (string[])RTC_Corruptcore.UISpec["SELECTEDDOMAINS"];

			Domain = _selectedDomains[RTC_Corruptcore.RND.Next(_selectedDomains.Length)];

			MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;
			RandomAddress = RTC_Corruptcore.RND.RandomLong(MaxAddress - 1);

			return new BlastTarget(Domain, RandomAddress);
		}

		public static string GetRandomKey()
		{
			//Generates unique string ids that are human-readable, unlike GUIDs
			string Key = RTC_Corruptcore.RND.Next(1, 9999).ToString() + RTC_Corruptcore.RND.Next(1, 9999).ToString() + RTC_Corruptcore.RND.Next(1, 9999).ToString() + RTC_Corruptcore.RND.Next(1, 9999).ToString();
			return Key;
		}


		public static void ASyncGenerateAndBlast()
		{
			BlastLayer bl = RTC_Corruptcore.GenerateBlastLayer((string[])RTC_Corruptcore.UISpec["SELECTEDDOMAINS"]);
			if (bl != null)
				bl.Apply(false);
		}

		/*
		public static void ApplyBlastLayer(BlastLayer bl)
		{
			if(bl.Layer != null)
				bl.Apply();
		}*/
	}
}
