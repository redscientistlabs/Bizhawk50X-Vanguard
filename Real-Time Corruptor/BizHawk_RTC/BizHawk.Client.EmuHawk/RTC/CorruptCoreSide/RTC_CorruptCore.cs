﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_CorruptCore
	{
		public static Random RND = new Random();
		public static CorruptionEngine SelectedEngine = CorruptionEngine.NIGHTMARE;

		private static int customPrecision = 1;
		public static int CustomPrecision
		{
			get { return customPrecision; }
			set
			{
				customPrecision = value;
				CurrentPrecision = value;
			}
		}
		public static int CurrentPrecision = 1;
		public static int Intensity = 1;
		public static int ErrorDelay = 1;
		public static BlastRadius Radius = BlastRadius.SPREAD;
		public static bool AutoCorrupt = false;



		public static BlastUnit getBlastUnit(string _domain, long _address, int precision)
		{
			//Will generate a blast unit depending on which Corruption Engine is currently set.
			//Some engines like Distortion may not return an Unit depending on the current state on things.

			BlastUnit bu = null;

			switch (SelectedEngine)
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
		public static BlastLayer Blast(BlastLayer _layer, string[] _selectedDomains)
		{
			string Domain = null;
			long MaxAddress = -1;
			long RandomAddress = -1;
			BlastUnit bu;
			BlastLayer bl;

			try
			{
				if (_layer != null)
				{
					_layer.Apply(); //If the BlastLayer was provided, there's no need to generate a new one.

					return _layer;
				}
				else if (SelectedEngine == CorruptionEngine.BLASTGENERATORENGINE)
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

					if (_selectedDomains == null || _selectedDomains.Count() == 0)
						return null;

					// Capping intensity at engine-specific maximums

					int _Intensity = Intensity; //general RTC intensity

					if ((SelectedEngine == CorruptionEngine.HELLGENIE || SelectedEngine == CorruptionEngine.FREEZE || SelectedEngine == CorruptionEngine.PIPE) && _Intensity > RTC_StepActions.MaxInfiniteBlastUnits)
						_Intensity = RTC_StepActions.MaxInfiniteBlastUnits; //Capping for cheat max

					switch (Radius) //Algorithm branching
					{
						case BlastRadius.SPREAD: //Randomly spreads all corruption bytes to all selected domains

							for (int i = 0; i < _Intensity; i++)
							{
								Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];

								MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;
								RandomAddress = RND.RandomLong(MaxAddress - 1);

								bu = getBlastUnit(Domain, RandomAddress, RTC_CorruptCore.CurrentPrecision);
								if (bu != null)
									bl.Layer.Add(bu);
							}

							break;

						case BlastRadius.CHUNK: //Randomly spreads the corruption bytes in one randomly selected domain

							Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];

							MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;

							for (int i = 0; i < _Intensity; i++)
							{
								RandomAddress = RND.RandomLong(MaxAddress - 1);

								bu = getBlastUnit(Domain, RandomAddress, CurrentPrecision);
								if (bu != null)
									bl.Layer.Add(bu);
							}

							break;

						case BlastRadius.BURST: // 10 shots of 10% chunk

							for (int j = 0; j < 10; j++)
							{
								Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];

								MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;

								for (int i = 0; i < (int)((double)_Intensity / 10); i++)
								{
									RandomAddress = RND.RandomLong(MaxAddress - 1);

									bu = getBlastUnit(Domain, RandomAddress, CurrentPrecision);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;

						case BlastRadius.NORMALIZED: // Blasts based on the size of the largest selected domain. Intensity =  Intensity / (domainSize[largestdomain]/domainSize[currentdomain])

							//Find the smallest domain and base our normalization around it
							//Domains aren't IComparable so I used keys

							long[] domainSize = new long[_selectedDomains.Length];
							for (int i = 0; i < _selectedDomains.Length; i++)
							{
								Domain = _selectedDomains[i];
								domainSize[i] = RTC_MemoryDomains.GetInterface(Domain).Size;
							}
							//Sort the arrays
							Array.Sort(domainSize, _selectedDomains);

							for (int i = 0; i < _selectedDomains.Length; i++)
							{
								Domain = _selectedDomains[i];

								//Get the intensity divider. The size of the largest domain divided by the size of the current domain
								long normalized = ((domainSize[_selectedDomains.Length - 1] / (domainSize[i])));

								for (int j = 0; j < (_Intensity / normalized); j++)
								{
									MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;
									RandomAddress = RND.RandomLong(MaxAddress - 1);

									bu = getBlastUnit(Domain, RandomAddress, CurrentPrecision);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;

						case BlastRadius.PROPORTIONAL: //Blasts proportionally based on the total size of all selected domains

							long totalSize = _selectedDomains.Select(it => RTC_MemoryDomains.GetInterface(it).Size).Sum(); //Gets the total size of all selected domains

							long[] normalizedIntensity = new long[_selectedDomains.Length]; //matches the index of selectedDomains
							for (int i = 0; i < _selectedDomains.Length; i++)
							{   //calculates the proportionnal normalized Intensity based on total selected domains size
								double proportion = (double)RTC_MemoryDomains.GetInterface(_selectedDomains[i]).Size / (double)totalSize;
								normalizedIntensity[i] = Convert.ToInt64((double)_Intensity * proportion);
							}

							for (int i = 0; i < _selectedDomains.Length; i++)
							{
								Domain = _selectedDomains[i];

								for (int j = 0; j < normalizedIntensity[i]; j++)
								{
									MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;
									RandomAddress = RND.RandomLong(MaxAddress - 1);

									bu = getBlastUnit(Domain, RandomAddress, CurrentPrecision);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;

						case BlastRadius.EVEN: //Evenly distributes the blasts through all selected domains

							for (int i = 0; i < _selectedDomains.Length; i++)
							{
								Domain = _selectedDomains[i];

								for (int j = 0; j < (_Intensity / _selectedDomains.Length); j++)
								{
									MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;
									RandomAddress = RND.RandomLong(MaxAddress - 1);

									bu = getBlastUnit(Domain, RandomAddress, CurrentPrecision);
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
				(S.GET<RTC_Core_Form>().AutoCorrupt ? ">> STOP AUTOCORRUPT ?.\n\n" : "") +
				$"domain:{Domain?.ToString()} maxaddress:{MaxAddress.ToString()} randomaddress:{RandomAddress.ToString()} \n\n" +
				ex.ToString(), "Error", (S.GET<RTC_Core_Form>().AutoCorrupt ? MessageBoxButtons.YesNo : MessageBoxButtons.OK));

				if (dr == DialogResult.Yes || dr == DialogResult.OK)
					S.GET<RTC_Core_Form>().AutoCorrupt = false;

				return null;
			}
		}
		public static BlastTarget GetBlastTarget()
		{
			//Standalone version of BlastRadius SPREAD

			string Domain = null;
			long MaxAddress = -1;
			long RandomAddress = -1;

			string[] _selectedDomains = RTC_MemoryDomains.SelectedDomains;

			Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];

			MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;
			RandomAddress = RND.RandomLong(MaxAddress - 1);

			return new BlastTarget(Domain, RandomAddress);
		}

		public static string GetRandomKey()
		{
			//Generates unique string ids that are human-readable, unlike GUIDs
			string Key = RND.Next(1, 9999).ToString() + RND.Next(1, 9999).ToString() + RND.Next(1, 9999).ToString() + RND.Next(1, 9999).ToString();
			return Key;
		}


	}
}
