﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using System.Numerics;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Data;
using Ceras;
using RTCV.CorruptCore;
using RTCV.NetCore;
using Exception = System.Exception;

namespace RTCV.CorruptCore
{
	[XmlInclude(typeof(StashKey))]
	[XmlInclude(typeof(BlastLayer))]
	[XmlInclude(typeof(BlastUnit))]
	[Serializable]
	[Ceras.MemberConfig(TargetMember.All)]
	public class Stockpile
	{
		public List<StashKey> StashKeys = new List<StashKey>();

		public string Name;
		public string Filename;
		public string ShortFilename;
		public string RtcVersion;
		public bool MissingLimiter;

		public Stockpile(DataGridView dgvStockpile)
		{
			foreach (DataGridViewRow row in dgvStockpile.Rows)
				StashKeys.Add((StashKey)row.Cells[0].Value);
		}

		public Stockpile()
		{
		}

		public override string ToString()
		{
			return Name ?? string.Empty;
		}

		public void Save(bool isQuickSave = false)
		{
			Save(this, isQuickSave);
		}

		public static bool Save(Stockpile sks, bool isQuickSave = false, bool compress = true)
		{
			if (sks.StashKeys.Count == 0)
			{
				MessageBox.Show("Can't save because the Current Stockpile is empty");
				return false;
			}

			if (!isQuickSave)
			{
				SaveFileDialog saveFileDialog1 = new SaveFileDialog
				{
					DefaultExt = "sks",
					Title = "Save Stockpile File",
					Filter = "SKS files|*.sks",
					RestoreDirectory = true
				};

				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
				{
					sks.Filename = saveFileDialog1.FileName;
					//sks.ShortFilename = sks.Filename.Substring(sks.Filename.LastIndexOf(Path.DirectorySeparatorChar) + 1, sks.Filename.Length - (sks.Filename.LastIndexOf(Path.DirectorySeparatorChar) + 1));
					sks.ShortFilename = Path.GetFileName(sks.Filename);
				}
				else
					return false;
			}
			else
			{
				sks.Filename = RTC_StockpileManager_UISide.CurrentStockpile.Filename;
				sks.ShortFilename = RTC_StockpileManager_UISide.CurrentStockpile.ShortFilename;
			}

			//Backup bizhawk settings
			LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_EVENT_SAVEBIZHAWKCONFIG, true);

			//Watermarking RTC Version
			sks.RtcVersion = RTC_CorruptCore.RtcVersion;

			List<string> allRoms = new List<string>();

			//populating Allroms array
			foreach (StashKey key in sks.StashKeys)
				if (!allRoms.Contains(key.RomFilename))
				{
					allRoms.Add(key.RomFilename);

					//If it's a cue file, find the bins and fix the cue to be relative
					if (key.RomFilename.ToUpper().Contains(".CUE"))
					{
						string cueFolder = RTC_Extensions.getLongDirectoryFromPath(key.RomFilename);
						string[] cueLines = File.ReadAllLines(key.RomFilename);
						List<string> binFiles = new List<string>();

						string[] fixedCue = new string[cueLines.Length];
						for (int i = 0; i < cueLines.Length; i++)
						{
							if (cueLines[i].Contains("FILE") && cueLines[i].Contains("BINARY"))
							{
								int startFilename;
								int endFilename = cueLines[i].LastIndexOf('"');

								//If it's an absolute path, convert it to a relative path then fix the cue as well
								if (cueLines[i].Contains(':'))
								{
									startFilename = cueLines[i].LastIndexOfAny(new char[] { '\\', '/' }) + 1;
									fixedCue[i] = "FILE \"" + cueLines[i].Substring(startFilename, endFilename - startFilename) + "\" BINARY";
								}
								else
								{
									//Just copy the old cue into the new one
									startFilename = cueLines[i].IndexOf('"') + 1;
									fixedCue[i] = cueLines[i];
								}

								binFiles.Add(cueLines[i].Substring(startFilename, endFilename - startFilename));
							}
							else
								fixedCue[i] = cueLines[i];

						}
						//Write our new cue
						File.WriteAllLines(key.RomFilename, fixedCue);

						allRoms.AddRange(binFiles.Select(it => cueFolder + it));
					}

					if (key.RomFilename.ToUpper().Contains(".CCD"))
					{
						List<string> binFiles = new List<string>();

						if (File.Exists(RTC_Extensions.removeFileExtension(key.RomFilename) + ".sub"))
							binFiles.Add(RTC_Extensions.removeFileExtension(key.RomFilename) + ".sub");

						if (File.Exists(RTC_Extensions.removeFileExtension(key.RomFilename) + ".img"))
							binFiles.Add(RTC_Extensions.removeFileExtension(key.RomFilename) + ".img");

						allRoms.AddRange(binFiles);
					}
				}

			//clean temp folder
			EmptyFolder(Path.DirectorySeparatorChar + "WORKING\\TEMP");

			//populating temp folder with roms
			foreach (string str in allRoms)
			{
				string rom = str;
				string romTempfilename = RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP" + Path.DirectorySeparatorChar + Path.GetFileName(rom);
				if (!rom.Contains(Path.DirectorySeparatorChar))
					rom = RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar + rom;

				//If the file already exists, overwrite it.
				if (File.Exists(romTempfilename))
				{
					//Whack the attributes in case a rom is readonly 
					File.SetAttributes(romTempfilename, FileAttributes.Normal);
					File.Delete(romTempfilename);
					File.Copy(rom, romTempfilename);
				}
				else
					File.Copy(rom, romTempfilename);
			}

			//Copy all the savestates
			foreach (StashKey key in sks.StashKeys)
			{
				// get savestate name
				string stateFilename = key.GameName + "." + key.ParentKey + ".timejump.State";
				File.Copy(RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + key.StateLocation + Path.DirectorySeparatorChar + stateFilename, RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP" + Path.DirectorySeparatorChar + stateFilename, true); // copy savestates to temp folder
			}

			//If there's a config, snag it
			if (File.Exists(RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "config.ini"))
				File.Copy(RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "config.ini", RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP\\config.ini");


			//Get all the limiter lists
			List<string[]> limiterLists = RTC_Filtering.GetAllLimiterListsFromStockpile(sks);

			//Write them to a file
			for (int i = 0; i < limiterLists?.Count; i++)
			{
				File.WriteAllLines(RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP" + Path.DirectorySeparatorChar + i + ".limiter", limiterLists[i]);
			}

			//Update stashkey info 
			foreach (StashKey sk in sks.StashKeys)
			{
				sk.RomShortFilename = RTC_Extensions.getShortFilenameFromPath(sk.RomFilename);
				sk.RomFilename = RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar + sk.RomShortFilename;
				sk.StateLocation = StashKeySavestateLocation.SKS;
			}
			//Create stockpile.xml to temp folder from stockpile object
			using (FileStream fs = File.Open(RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP\\stockpile.json", FileMode.OpenOrCreate))
			{
				JsonHelper.Serialize(sks, fs, Formatting.Indented);
				fs.Close();
			}

			string tempFilename = sks.Filename + ".temp";
			//If there's already a temp file from a previous failed save, delete it
			try
			{
				if (File.Exists(tempFilename))
					File.Delete(tempFilename);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return false;
			}

			CompressionLevel comp = CompressionLevel.Fastest;
			if (!compress)
				comp = CompressionLevel.NoCompression;
			//Create the file into temp
			ZipFile.CreateFromDirectory(RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP" + Path.DirectorySeparatorChar, tempFilename, comp, false);

			//Remove the old stockpile
			try
			{
				if (File.Exists(sks.Filename))
					File.Delete(sks.Filename);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return false;
			}

			//Move us to the destination
			File.Move(tempFilename, sks.Filename);

			//Move all the files from temp into SKS
			EmptyFolder(Path.DirectorySeparatorChar + "WORKING\\SKS");
			foreach (string file in Directory.GetFiles(RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP"))
				try
				{
					File.Move(file, RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar +
						Path.GetFileName(file));
				}
				catch (Exception e)
				{
					MessageBox.Show("Unable to move " + Path.GetFileName(file) +
						"to SKS. Your stockpile should be saved.\n" +
						"If you're seeing this error, that means the file is probably in use. If it is, everything should technically be fine assuming it's the same file.\n" +
						"If the file you're seeing here has changed since the stockpile was last saved (rom edited manually), you should probably reload your stockpile from the file.");
				}

			RTC_StockpileManager_UISide.CurrentStockpile = sks;
			return true;
		}

		public static bool Load(DataGridView dgvStockpile, string Filename = null)
		{
			if (Filename == null)
			{
				OpenFileDialog ofd = new OpenFileDialog
				{
					DefaultExt = "sks",
					Title = "Open Stockpile File",
					Filter = "SKS files|*.sks",
					RestoreDirectory = true
				};
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					Filename = ofd.FileName;
				}
				else
					return false;
			}

			if (!File.Exists(Filename))
			{
				MessageBox.Show("The Stockpile file wasn't found");
				return false;
			}


			//Extract the stockpile
			Extract(Filename, Path.DirectorySeparatorChar + "WORKING\\SKS", "stockpile.json");

			Stockpile sks;
			//Read in the stockpile
			try
			{
				using (FileStream fs = File.Open(RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "SKS\\stockpile.json", FileMode.OpenOrCreate))
				{
					sks = JsonHelper.Deserialize<Stockpile>(fs);
					fs.Close();
				}

			}
			catch (Exception e)
			{
				MessageBox.Show("The Stockpile file could not be loaded" + e);
				return false;
			}

			//Load the limiter lists into the dictionary
			RTC_Filtering.LoadListsFromPaths(Directory.GetFiles(RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar, "*.limiter"));

			//Update the current stockpile to this one
			RTC_StockpileManager_UISide.CurrentStockpile = sks;

			//Set up the correct 
			foreach (StashKey t in sks.StashKeys)
			{
				t.RomFilename = RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar + t.RomShortFilename;
				t.StateLocation = StashKeySavestateLocation.SKS;
			}

			//fill list controls
			dgvStockpile.Rows.Clear();

			//Todo - Refactor this to get it out of the object
			//Populate the dgv
			foreach (StashKey key in sks.StashKeys)
				dgvStockpile?.Rows.Add(key, key.GameName, key.SystemName, key.SystemCore, key.Note);

			//Update the filename in case they renamed it
			sks.Filename = Filename;

			//Check version compatibility
			CheckCompatibility(sks);

			//If there's a limiter missing, pop a message
			if (sks.MissingLimiter)
				MessageBox.Show(
					"This stockpile is missing a limiter list used by some blastunits.\n" +
					"Some corruptions probably won't work properly.\n" +
					"If the limiter list is found next time you save, it'll automatically be packed in.");
			return true;
		}

		/// <summary>
		/// Checks a stockpile for compatibility with the current version of the RTC
		/// </summary>
		/// <param name="sks"></param>
		public static void CheckCompatibility(Stockpile sks)
		{
			List<string> errorMessages = new List<string>();

			if (sks.RtcVersion != RTC_CorruptCore.RtcVersion)
			{
				if (sks.RtcVersion == null)
					errorMessages.Add("You have loaded a broken stockpile that didn't contain an RTC Version number\n. There is no reason to believe that these items will work.");
				else
					errorMessages.Add("You have loaded a stockpile created with RTC " + sks.RtcVersion + " using RTC " + RTC_CorruptCore.RtcVersion + "\n" + "Items might not appear identical to how they when they were created or it is possible that they don't work if BizHawk was upgraded.");
			}

			if (errorMessages.Count == 0)
				return;

			string message = "The loaded stockpile returned the following errors:\n\n";

			foreach (string line in errorMessages)
				message += $"•  {line} \n\n";

			MessageBox.Show(message, "Compatibility Checker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		/// <summary>
		/// Recursively deletes all files and folders within a directory
		/// </summary>
		/// <param name="baseDir"></param>
		public static void RecursiveDelete(DirectoryInfo baseDir)
		{
			if (!baseDir.Exists)
				return;

			foreach (DirectoryInfo dir in baseDir.EnumerateDirectories())
			{
				RecursiveDelete(dir);
			}
			baseDir.Delete(true);
		}

		public static void EmptyFolder(string folder)
		{
			try
			{
				foreach (string file in Directory.GetFiles(RTC_CorruptCore.rtcDir + Path.DirectorySeparatorChar + $"{folder}"))
				{
					File.SetAttributes(file, FileAttributes.Normal);
					File.Delete(file);
				}

				foreach (string dir in Directory.GetDirectories(RTC_CorruptCore.rtcDir + Path.DirectorySeparatorChar + $"{folder}"))
					RecursiveDelete(new DirectoryInfo(dir));
			}
			catch (Exception ex)
			{
				MessageBox.Show("Unable to empty a temp folder! If your stockpile has any CD based games, close them before saving the stockpile! If this isn't the case, report this bug to the RTC developers.");
				throw new Exception(ex.ToString());
			}
		}

		/// <summary>
		/// Extracts a stockpile into a folder and ensures a master file exists
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="folder"></param>
		/// <param name="masterFile"></param>
		/// <returns></returns>
		public static bool Extract(string filename, string folder, string masterFile)
		{
			try
			{
				EmptyFolder(folder);
				ZipFile.ExtractToDirectory(filename, RTC_CorruptCore.rtcDir + Path.DirectorySeparatorChar + $"{folder}" + Path.DirectorySeparatorChar);

				if (!File.Exists(RTC_CorruptCore.rtcDir + Path.DirectorySeparatorChar + $"{folder}\\{masterFile}"))
				{
					MessageBox.Show("The file could not be read properly");

					EmptyFolder(folder);
				}

				return true;
			}
			catch (Exception e)
			{
				//If it errors out, empty the folder
				MessageBox.Show("The file could not be read properly" + e);
				EmptyFolder(folder);
				return false;
			}
		}


		public static void LoadBizhawkKeyBindsFromIni(string Filename = null)
		{
			if (Filename == null)
			{
				OpenFileDialog ofd = new OpenFileDialog
				{
					DefaultExt = "ini",
					Title = "Open ini File",
					Filter = "Bizhawk config file|*.ini",
					RestoreDirectory = true
				};
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					Filename = ofd.FileName.ToString();
				}
				else
					return;
			}

			if (File.Exists(RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "import_config.ini"))
				File.Delete(RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "import_config.ini");
			File.Copy(Filename, RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "import_config.ini");

			if (File.Exists(RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "stockpile_config.ini"))
				File.Delete(RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "stockpile_config.ini");
			File.Copy(RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "config.ini", RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "stockpile_config.ini");


			LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_IMPORTKEYBINDS);
			Process.Start(RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + $"StockpileConfigDETACHED.bat");

		}

		public static void LoadBizhawkConfigFromStockpile(string Filename = null)
		{
			if (Filename == null)
			{
				OpenFileDialog ofd = new OpenFileDialog
				{
					DefaultExt = "sks",
					Title = "Open Stockpile File",
					Filter = "SKS files|*.sks",
					RestoreDirectory = true
				};
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					Filename = ofd.FileName.ToString();
				}
				else
					return;
			}

			if (File.Exists(RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "backup_config.ini"))
			{
				if (MessageBox.Show("Do you want to overwrite the previous Config Backup with the current Bizhawk Config?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					File.Delete(RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "backup_config.ini");
					File.Copy((RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "config.ini"), (RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "backup_config.ini"));
				}
			}
			else
				File.Copy((RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "config.ini"), (RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "backup_config.ini"));

			Extract(Filename, Path.DirectorySeparatorChar + "WORKING\\TEMP", "stockpile.json");

			if (File.Exists(RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "stockpile_config.ini"))
				File.Delete(RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "stockpile_config.ini");
			File.Copy((RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "SKS\\config.ini"), (RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "stockpile_config.ini"));

			LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_MERGECONFIG);


			Process.Start(RTC_CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + $"StockpileConfigDETACHED.bat");

		}

		public static void RestoreBizhawkConfig()
		{
			LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_RESTOREBIZHAWKCONFIG);
		}


		public static void Import(string filename, DataGridView dgvStockpile)
		{
			//clean temp folder
			EmptyFolder(Path.DirectorySeparatorChar + "WORKING\\TEMP");

			if (filename == null)
			{
				OpenFileDialog openFileDialog1 = new OpenFileDialog
				{
					DefaultExt = "sks",
					Title = "Open Stockpile File",
					Filter = "SKS files|*.sks",
					RestoreDirectory = true
				};
				if (openFileDialog1.ShowDialog() == DialogResult.OK)
				{
					filename = openFileDialog1.FileName.ToString();
				}
				else
					return;
			}

			if (!File.Exists(filename))
			{
				MessageBox.Show("The Stockpile file wasn't found");
				return;
			}

			System.IO.Compression.ZipFile.ExtractToDirectory(filename, RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + $"TEMP" + Path.DirectorySeparatorChar);

			if (!File.Exists(RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP\\stockpile.json"))
			{
				MessageBox.Show("The file could not be read properly");

				EmptyFolder(Path.DirectorySeparatorChar + "WORKING\\TEMP");

				return;
			}

			//stockpile deserialization
			Stockpile sks;

			try
			{
				using (FileStream fs = File.Open(RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP\\stockpile.json", FileMode.OpenOrCreate))
				{
					JsonSerializer js = new JsonSerializer();
					sks = JsonHelper.Deserialize<Stockpile>(fs);
					fs.Close();
				}
			}
			catch
			{
				MessageBox.Show("The Stockpile file could not be loaded");
				return;
			}

			foreach (StashKey sk in sks.StashKeys)
			{
				sk.RomShortFilename = RTC_Extensions.getShortFilenameFromPath(sk.RomFilename);
				sk.RomFilename = RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar + sk.RomShortFilename;
				sk.StateLocation = StashKeySavestateLocation.SKS;
			}

			foreach (string file in Directory.GetFiles(RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP" + Path.DirectorySeparatorChar))
			{
				if (!file.Contains(".sk"))
				{
					try
					{
						string dest = RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar + Path.GetFileName(file);

						if (!File.Exists(dest))
						{
							File.Copy(file, dest); // copy roms/stockpile/whatever to sks folder
						}

					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString());
					}
				}

			}
			EmptyFolder(Path.DirectorySeparatorChar + "WORKING\\TEMP");

			//fill list controls
			//Todo - Get this out of the object
			foreach (StashKey sk in sks.StashKeys)
			{
				DataGridViewRow dgvRow = dgvStockpile.Rows[dgvStockpile.Rows.Add()];
				dgvRow.Cells["Item"].Value = sk;
				dgvRow.Cells["GameName"].Value = sk.GameName;
				dgvRow.Cells["SystemName"].Value = sk.SystemName;
				dgvRow.Cells["SystemCore"].Value = sk.SystemCore;
			}

			CheckCompatibility(sks);

			RTC_StockpileManager_UISide.StockpileChanged();
		}
	}

	[Serializable]
	[Ceras.MemberConfig(TargetMember.All)]
	public class StashKey : ICloneable, INote
	{
		public string RomFilename { get; set; }
		public string RomShortFilename { get; set; }
		public byte[] RomData { get; set; }

		public string StateShortFilename { get; set; }
		public string StateFilename { get; set; }
		public byte[] StateData { get; set; }
		public StashKeySavestateLocation StateLocation { get; set; } = StashKeySavestateLocation.SESSION;

		public string SystemName { get; set; }
		public string SystemDeepName { get; set; }
		public string SystemCore { get; set; }
		public List<string> SelectedDomains { get; set; } = new List<string>();
		public string GameName { get; set; }
		public string SyncSettings { get; set; }
		public string Note { get; set; }


		public string Key { get; set; }
		public string ParentKey { get; set; }
		public BlastLayer BlastLayer { get; set; }

		private string alias;
		public string Alias
		{
			get => alias ?? Key;
			set => alias = value;
		}


		public StashKey(string key, string parentkey, BlastLayer blastlayer)
		{
			Key = key;
			ParentKey = parentkey;
			BlastLayer = blastlayer;

			RomFilename = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME.ToString()];
			SystemName = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.SYSTEM.ToString()];
			SystemCore = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.SYSTEMCORE.ToString()];
			GameName = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.GAMENAME.ToString()];
			SyncSettings = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.SYNCSETTINGS.ToString()];

			this.SelectedDomains.AddRange((string[])RTCV.NetCore.AllSpec.UISpec["SELECTEDDOMAINS"]);
		}

		public StashKey()
		{
		}

		public object Clone()
		{
			object sk = ObjectCopierCeras.Clone(this);
			((StashKey)sk).Key = RTC_CorruptCore.GetRandomKey();
			((StashKey)sk).Alias = null;
			return sk;
		}

		public static void SetCore(StashKey sk) => SetCore(sk.SystemName, sk.SystemCore);
		public static void SetCore(string systemName, string systemCore)
		{
			LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_KEY_SETSYSTEMCORE, new object[] { systemName, systemCore }, true);
		}

		public override string ToString()
		{
			return Alias;
		}

		public bool Run()
		{
			RTC_StockpileManager_UISide.CurrentStashkey = this;
			return RTC_StockpileManager_UISide.ApplyStashkey(this);
		}

		public void RunOriginal()
		{
			RTC_StockpileManager_UISide.CurrentStashkey = this;
			RTC_StockpileManager_UISide.OriginalFromStashkey(this);
		}

		public byte[] EmbedState()
		{
			if (StateFilename == null)
				return null;

			if (this.StateData != null)
				return this.StateData;

			byte[] stateData = File.ReadAllBytes(StateFilename);
			this.StateData = stateData;

			return stateData;
		}

		public bool DeployState()
		{
			if (StateShortFilename == null || this.StateData == null)
				return false;

			string deployedStatePath = GetSavestateFullPath();

			if (File.Exists(deployedStatePath))
				return true;

			File.WriteAllBytes(deployedStatePath, this.StateData);

			return true;
		}

		public string GetSavestateFullPath()
		{
			return RTC_CorruptCore.workingDir + Path.DirectorySeparatorChar + this.StateLocation.ToString() + Path.DirectorySeparatorChar + this.GameName + "." + this.ParentKey + ".timejump.State"; // get savestate name
		}

	}

	[Serializable]
	[Ceras.MemberConfig(TargetMember.All)]
	public class SaveStateKeys
	{
		public StashKey[] StashKeys = new StashKey[41];
		public string[] Text = new string[41];
	}

	[Serializable]
	[Ceras.MemberConfig(TargetMember.All)]
	public class BlastTarget
	{
		public string Domain = null;
		public long Address = 0;

		public BlastTarget(string _domain, long _address)
		{
			Domain = _domain;
			Address = _address;
		}
	}

	[XmlInclude(typeof(BlastUnit))]
	[Ceras.MemberConfig(TargetMember.All)]
	[Serializable]
	public class BlastLayer : ICloneable, INote
	{
		public List<BlastUnit> Layer;

		public BlastLayer()
		{
			Layer = new List<BlastUnit>();
		}

		public BlastLayer(List<BlastUnit> _layer)
		{
			Layer = _layer;
		}

		public object Clone()
		{
			return ObjectCopierCeras.Clone(this);
		}

		public void Apply(bool storeUncorruptBackup, bool ignoreMaximums = false)
		{
			if (storeUncorruptBackup && this != RTC_StockpileManager_EmuSide.UnCorruptBL)
				RTC_StockpileManager_EmuSide.UnCorruptBL = GetBackup();

			bool success;

			try
			{
				foreach (BlastUnit bb in Layer)
				{
					if (bb == null) //BlastCheat getBackup() always returns null so they can happen and they are valid
						success = true;
					else
						success = bb.Apply();

					if (!success)
						throw new Exception(
						"One of the BlastUnits in the BlastLayer failed to Apply().\n\n" +
						"The operation was cancelled");
				}
				RTC_StepActions.FilterBuListCollection();
			}
			catch (Exception ex)
			{
				throw new Exception(
							"An error occurred in RTC while applying a BlastLayer to the game.\n\n" +
							"The operation was cancelled\n\n" +
							ex
							);
			}
			finally
			{
				if (!ignoreMaximums)
				{
					RTC_StepActions.RemoveExcessInfiniteStepUnits();
				}
			}
		}

		public BlastLayer GetBakedLayer()
		{
			List<BlastUnit> BackupLayer = new List<BlastUnit>();

			BackupLayer.AddRange(Layer.Select(it => it.GetBakedUnit()));

			return new BlastLayer(BackupLayer);
		}

		public BlastLayer GetBackup()
		{
			List<BlastUnit> BackupLayer = new List<BlastUnit>();

			BackupLayer.AddRange(Layer.Select(it => it.GetBackup()));

			return new BlastLayer(BackupLayer);
		}

		public void Reroll()
		{
			foreach (BlastUnit bu in Layer)
				bu.Reroll();
		}

		public void RasterizeVMDs()
		{
			foreach (BlastUnit bu in Layer)
				bu.RasterizeVMDs();
		}

		private string shared = "[DIFFERENT]";

		[JsonIgnore]
		public string Note
		{
			get
			{
				if (Layer.All(x => x.Note == Layer.First().Note))
				{
					return Layer.FirstOrDefault()?.Note;
				}
				return shared;
			}
			set
			{
				if (value == shared)
				{
					return;
				}
				foreach (BlastUnit bu in Layer)
					bu.Note = value;
			}
		}
	}

	/// <summary>
	/// Working data for BlastUnits.
	/// Not serialized
	/// </summary>
	public class BlastUnitWorkingData
	{
		//We Calculate a LastFrame at the beginning of execute
		[XmlIgnore, NonSerialized]
		public int LastFrame = -1;
		//We calculate ExecuteFrameQueued which is the ExecuteFrame + the currentframe that was calculated at the time of it entering the execution pool
		[XmlIgnore, NonSerialized]
		public int ExecuteFrameQueued = 0;

		//We use ApplyValue so we don't need to keep re-calculating the tiled value every execute if we don't have to.
		[XmlIgnore, NonSerialized]
		public byte[] ApplyValue = null;

		//The data that has been backed up. This is a list of bytes so if they start backing up at IMMEDIATE, they can have historical backups
		[XmlIgnore, NonSerialized]
		public Queue<byte[]> StoreData = new Queue<byte[]>();
	}

	[Serializable]
	[Ceras.MemberConfig(TargetMember.All)]
	public class BlastUnit : INote
	{

		public object Clone()
		{
			return ObjectCopierCeras.Clone(this);
		}

		[Category("Settings")]
		[Description("Whether or not the BlastUnit will apply if the stashkey is run")]
		[DisplayName("Enabled")]
		public bool IsEnabled { get; set; } = true;

		[Category("Settings")]
		[Description("Whether or not this unit will be affected by batch operations (disable 50, invert, etc)")]
		[DisplayName("Locked")]
		public bool IsLocked { get; set; } = false;

		[Category("Data")]
		[Description("Whether or not the unit's values need to be flipped due to endianess")]
		[DisplayName("Big Endian")]
		public bool BigEndian { get; set; }

		[Category("Data")]
		[Description("The domain this unit will target")]
		[DisplayName("Domain")]
		public string Domain { get; set; }

		[Category("Data")]
		[Description("The address this unit will target")]
		[DisplayName("Address")]
		public long Address { get; set; }


		private int precision;

		[Category("Data")]
		[Description("The precision of this unit")]
		[DisplayName("Precision")]
		public int Precision
		{
			get
			{
				return precision;
			}
			set
			{
				//Cache the old precision
				int oldPrecision = precision;
				//Update the precision
				precision = value;

				//If the user is changing the precision and already has a Value set, we need to update that array
				if (Value != null && oldPrecision != precision && Value.Length != precision)
				{
					//If the precision is being set to 0, force it back to 1 and fill in an empty value
					if (precision < 1)
					{
						Value = new byte[1];
						precision = 1;
					}

					else
					{
						//If there was no precision set, set it to 1
						if (oldPrecision == 0)
							oldPrecision = 1;

						byte[] temp = new byte[precision];
						//If the new unit is larger, copy it over left padded
						if (precision > oldPrecision)
						{
							Value.CopyTo(temp, precision - oldPrecision);
						}
						//If the new unit is smaller, truncate it (first X bytes cut off)
						else
						{
							int j = 0;
							for (int i = oldPrecision - precision; i < oldPrecision; i++)
							{
								temp[j] = Value[i];
								j++;
							}
						}
						Value = temp;
					}
				}
			}
		}

		[Category("Source")]
		[Description("The source for the value for this unit for STORE mode")]
		[DisplayName("Source")]
		public BlastUnitSource Source { get; set; }

		[Category("Store")]
		[Description("The time when the store will take place")]
		[DisplayName("Store Time")]
		public StoreTime StoreTime { get; set; }

		[Category("Store")]
		[Description("The type of store that when the store will take place")]
		[DisplayName("Store Type")]
		public StoreType StoreType { get; set; }


		[JsonIgnore]
		[Category("Value")]
		[Description("The value used for the BlastUnit in VALUE mode")]
		[DisplayName("Value")]
		public byte[] Value { get; set; }


		[Category("Value")]
		[Description("Gets and sets Value[] through a string. Used for Textboxes")]
		[DisplayName("ValueString")]
		public string ValueString
		{
			get
			{
				if (Value == null)
					return String.Empty;
				return BitConverter.ToString(this.Value).Replace("-", string.Empty);
			}
			set
			{
				var temp = RTC_Extensions.StringToByteArrayPadLeft(value, this.Precision);
				if (temp != null)
					this.Value = temp;
			}
		}

		[Category("Store")]
		[Description("The domain used for the STORE operation")]
		[DisplayName("Source Domain")]
		public string SourceDomain { get; set; }

		[Category("Store")]
		[Description("The address used for the STORE operation")]
		[DisplayName("Source Address")]
		public long SourceAddress { get; set; }


		[Category("Modifiers")]
		[Description("How much to tilt the value before poking memory")]
		[DisplayName("Tilt Value")]
		public BigInteger TiltValue { get; set; }


		public int ExecuteFrame { get; set; }
		public int Lifetime { get; set; }
		public bool Loop { get; set; } = false;


		[Category("Limiter")]
		[Description("When to apply the limiter list")]
		[DisplayName("Limiter List")]
		public LimiterTime LimiterTime { get; set; }

		[Category("Limiter")]
		[Description("The hash of the Limiter List in use")]
		[DisplayName("Limiter List Hash")]
		public string LimiterListHash { get; set; }

		[Category("Limiter")]
		[Description("Invert the limiter so the unit only applies if the value doesn't match the limiter")]
		[DisplayName("Invert Limiter")]
		public bool InvertLimiter { get; set; }

		[Category("Misc")]
		[Description("Note associated with this unit")]
		public string Note { get; set; }



		//Don't serialize this
		[XmlIgnore, NonSerialized, JsonIgnore, Ceras.Ignore]
		public BlastUnitWorkingData Working;



		/// <summary>
		/// Creates a Blastunit that utilizes a backup. 
		/// </summary>
		/// <param name="storeType">The type of store</param>
		/// <param name="storeTime">The time of the store</param>
		/// <param name="domain">The domain of the blastunit</param>
		/// <param name="address">The address of the blastunit</param>
		/// <param name="bigEndian">If the Blastunit is being applied to a big endian system. Results in the bytes being flipped before apply</param>
		/// <param name="applyFrame">The frame on which the BlastUnit will start executing</param>
		/// <param name="lifetime">How many frames the BlastUnit will execute for. 0 for infinite</param>
		/// <param name="note"></param>
		/// <param name="isEnabled"></param>
		/// <param name="isLocked"></param>
		public BlastUnit(StoreType storeType, StoreTime storeTime,
			string domain, long address, string sourceDomain, long sourceAddress, int precision, bool bigEndian, int executeFrame = 0, int lifetime = 1,
			string note = null, bool isEnabled = true, bool isLocked = false)
		{
			Source = BlastUnitSource.STORE;
			StoreTime = storeTime;
			StoreType = storeType;

			Domain = domain;
			Address = address;
			SourceDomain = sourceDomain;
			SourceAddress = sourceAddress;
			Precision = precision;
			BigEndian = bigEndian;
			ExecuteFrame = executeFrame;
			Lifetime = lifetime;
			Note = note;
			IsEnabled = isEnabled;
			IsLocked = isLocked;
		}

		/// <summary>
		/// Creates a BlastUnit that uses a byte array value as the value
		/// </summary>
		/// <param name="value">The value of the BlastUnit</param>
		/// <param name="domain">The domain the blastunit lies in</param>
		/// <param name="address"></param>
		/// <param name="bigEndian"></param>
		/// <param name="executeFrame"></param>
		/// <param name="lifetime"></param>
		/// <param name="note"></param>
		/// <param name="isEnabled"></param>
		/// <param name="isLocked"></param>
		public BlastUnit(byte[] value,
			string domain, long address, int precision, bool bigEndian, int executeFrame = 0, int lifetime = 1,
			string note = null, bool isEnabled = true, bool isLocked = false)
		{
			Source = BlastUnitSource.VALUE;
			//Precision has to be set before value
			Precision = precision;
			Value = value;

			Domain = domain;
			Address = address;
			ExecuteFrame = executeFrame;
			Lifetime = lifetime;
			Note = note;
			IsEnabled = isEnabled;
			IsLocked = isLocked;
		}

		public BlastUnit()
		{
		}

		/// <summary>
		/// Rasterizes VMDs to their underlying domain
		/// </summary>
		public void RasterizeVMDs()
		{
			//Todo - Change this to a more unique marker than [V]?
			if (Domain.Contains("[V]"))
			{
				string domain = (string)Domain.Clone();
				long address = Address;

				Domain = (RTC_MemoryDomains.VmdPool[domain] as VirtualMemoryDomain)?.PointerDomains[(int)address] ?? "ERROR";
				Address = (RTC_MemoryDomains.VmdPool[domain] as VirtualMemoryDomain)?.PointerAddresses[(int)address] ?? -1;
			}
			if (SourceDomain?.Contains("[V]") ?? false)
			{
				string sourceDomain = (string)SourceDomain.Clone();
				long sourceAddress = SourceAddress;

				SourceDomain = (RTC_MemoryDomains.VmdPool[sourceDomain] as VirtualMemoryDomain)?.PointerDomains[(int)sourceAddress] ?? "ERROR";
				SourceAddress = (RTC_MemoryDomains.VmdPool[sourceDomain] as VirtualMemoryDomain)?.PointerAddresses[(int)sourceAddress] ?? -1;
			}

		}
		/// <summary>
		/// Adds a blastunit to the execution pool
		/// </summary>
		/// <returns></returns>
		public bool Apply()
		{
			if (!IsEnabled)
				return true;
			//Create our working data object
			this.Working = new BlastUnitWorkingData();

			//We need to grab the value to freeze
			if (Source == BlastUnitSource.STORE && StoreTime == StoreTime.IMMEDIATE)
			{
				//If it's one time, store the backup. Otherwise add it to the backup pool 
				if (StoreType == StoreType.ONCE)
					StoreBackup();
				else
				{
					RTC_StepActions.StoreDataPool.Add(this);
				}
			}
			//Add it to the execution pool
			RTC_StepActions.AddBlastUnit(this);
			return true;
		}

		/// <summary>
		/// Executes (applies) a blastunit. This shouldn't be called manually.
		/// If you want to execute a blastunit, add it to the execution pool using Apply()
		/// </summary>
		public void Execute()
		{
			if (!IsEnabled)
				return;

			try
			{
				//Get our memory interface
				MemoryInterface mi = RTC_MemoryDomains.GetInterface(Domain);
				if (mi == null)
					return;

				//Limiter handling
				if (LimiterListHash != null && LimiterTime == LimiterTime.EXECUTE)
				{
					if (InvertLimiter)
					{
						/*
						//If it's store, we need to use the sourceaddress and sourcedomain
						if (Source == BlastUnitSource.STORE && RTC_Filtering.LimiterPeekBytes(SourceAddress,
								SourceAddress + Precision, SourceDomain, LimiterListHash, mi))
							return;
						//If it's VALUE, we need to use the address and domain
						*/
						if (RTC_Filtering.LimiterPeekBytes(Address,
							Address + Precision, Domain, LimiterListHash, mi))
							return;
					}
					else
					{
						/*
						//If it's store, we need to use the sourceaddress and sourcedomain
						if (Source == BlastUnitSource.STORE && !RTC_Filtering.LimiterPeekBytes(SourceAddress,
								SourceAddress + Precision, SourceDomain, LimiterListHash, mi))
							return;
						//If it's VALUE, we need to use the address and domain
						*/
						if (!RTC_Filtering.LimiterPeekBytes(Address,
							Address + Precision, Domain, LimiterListHash, mi))
							return;
					}
				}


				switch (Source)
				{
					case (BlastUnitSource.STORE):
						{
							//If there's no stored data, return out.
							if (Working.StoreData.Count == 0)
								return;

							//Apply the value we have stored
							Working.ApplyValue = Working.StoreData.First();

							//Remove it from the store pool if it's a continuous backup
							if (StoreType == StoreType.CONTINUOUS)
								Working.StoreData.Dequeue();

							//All the data is already handled by GetStoreBackup, so we can just poke
							for (int i = 0; i < Precision; i++)
							{
								mi.PokeByte(Address + i, Working.ApplyValue[i]);
							}
						}
						break;
					case (BlastUnitSource.VALUE):
						{
							//We only calculate it once for Value and then store it in ApplyValue.
							//If the length has changed (blast editor) we gotta recalc it
							if (Working.ApplyValue == null)
							{
								//Calculate the actual value to apply
								Working.ApplyValue = RTC_Extensions.AddValueToByteArrayUnchecked(Value, TiltValue, this.BigEndian);

								//Flip it if it's big endian
								if (this.BigEndian)
									Working.ApplyValue.FlipBytes();
							}
							//Poke the memory
							for (int i = 0; i < Precision; i++)
							{
								mi.PokeByte(Address + i, Working.ApplyValue[i]);
							}

							break;
						}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("The BlastUnit apply() function threw up. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n", ex);
			}

			return;
		}

		/// <summary>
		/// Adds a backup to the end of the StoreData queue
		/// </summary>
		public void StoreBackup()
		{
			//Snag our memory interface
			MemoryDomainProxy mi = RTC_MemoryDomains.GetProxy(SourceDomain, SourceAddress);

			if (mi == null)
				throw new Exception(
					$"Memory Domain error. Mi was null. If you know how to reproduce this, let the devs know");

			//Get the value
			Byte[] value = new byte[Precision];
			for (int i = 0; i < Precision; i++)
			{
				value[i] = mi.PeekByte(SourceAddress + i);
			}
			//Calculate the final value after adding the tilt value
			value = RTC_Extensions.AddValueToByteArrayUnchecked(value, TiltValue, mi.BigEndian);

			//Enqueue it
			Working.StoreData.Enqueue(value);
		}

		/// <summary>
		/// Returns a unit baked to VALUE with a lifetime of 1
		/// </summary>
		/// <returns></returns>
		public BlastUnit GetBakedUnit()
		{
			if (!IsEnabled)
				return null;

			try
			{
				//Grab our mi
				MemoryInterface mi = RTC_MemoryDomains.GetInterface(Domain);
				if (mi == null)
					return null;

				//Grab the value
				byte[] _value = new byte[Precision];
				for (int i = 0; i < Precision; i++)
				{
					_value[i] = mi.PeekByte(Address + i);
				}
				//Return a new unit
				return new BlastUnit(_value, Domain, Address, Precision, BigEndian, 0, 1, Note, IsEnabled, IsLocked);

			}
			catch (Exception ex)
			{
				throw new Exception("The BlastUnit GetBakedUnit() function threw up. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
				ex);
			}
		}


		public BlastUnit GetBackup()
		{
			//TODO
			//There's a todo here but I didn't leave a note please help someone tell me why there's a todo here oh god I'm the only one working on this code 
			return GetBakedUnit();
		}

		/// <summary>
		/// Rerolls a blastunit and generates new values based on various params 
		/// </summary>
		public void Reroll()
		{
			//Todo - Value List reroll. Currently always generates a random value
			if (Source == BlastUnitSource.VALUE)
			{
				//Generate a random value based on our precision. 
				//We use a BigInteger as we support arbitrary length, but we do use built in methods for 8,16,32 bit for performance reasons
				BigInteger randomValue;
				switch (Precision)
				{
					case (1):
						randomValue = RTC_CorruptCore.RND.RandomLong(RTC_NightmareEngine.MinValue8Bit, RTC_NightmareEngine.MaxValue8Bit);
						break;
					case (2):
						randomValue = RTC_CorruptCore.RND.RandomLong(RTC_NightmareEngine.MinValue16Bit, RTC_NightmareEngine.MaxValue16Bit);
						break;
					case (4):
						randomValue = RTC_CorruptCore.RND.RandomLong(RTC_NightmareEngine.MinValue32Bit, RTC_NightmareEngine.MaxValue32Bit);
						break;
					//No limits if out of normal range
					default:
						byte[] _randomValue = new byte[Precision];
						RTC_CorruptCore.RND.NextBytes(_randomValue);
						randomValue = new BigInteger(_randomValue);
						break;
				}

				byte[] temp = new byte[Precision];
				//We use this as it properly handles the length for us
				byte[] outArr = RTC_Extensions.AddValueToByteArrayUnchecked(temp, randomValue, false);

				//Todo - Evaluate if this reverse is required
				//Reverse the bytes to big endian
				Array.Reverse(outArr);
				Value = outArr;
			}
			else if (Source == BlastUnitSource.STORE)
			{
				//Todo - Allow rerolling address and domain separately
				if (RTC_CorruptCore.RerollSourceAddress)
				{
					var newSource = RTC_CorruptCore.GetBlastTarget();

					SourceAddress = newSource.Address;
					SourceDomain = newSource.Domain;
				}
				if (RTC_CorruptCore.RerollAddress)
				{
					var newSource = RTC_CorruptCore.GetBlastTarget();

					Address = newSource.Address;
					Domain = newSource.Domain;
				}
			}
		}

		public override string ToString()
		{
			string enabledString = "[ ] BlastByte -> ";
			if (IsEnabled)
				enabledString = "[x] BlastByte -> ";

			string cleanDomainName = Domain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"
			return (enabledString + cleanDomainName + "(" + Convert.ToInt32(Address).ToString() + ")." + Source.ToString() + "(" + RTC_Extensions.GetDecimalValue(Value, BigEndian).ToString() + ")");
		}

		/// <summary>
		/// Called when a unit is moved from the queue into the execution pool
		/// </summary>
		/// <returns></returns>
		public bool EnteringExecution()
		{
			//Snag our MI
			MemoryInterface mi = RTC_MemoryDomains.GetInterface(Domain);
			if (mi == null)
				return false;

			//If it's a store unit, store the backup
			if (Source == BlastUnitSource.STORE && StoreTime == StoreTime.PREEXECUTE)
			{
				//One off store vs execution pool
				if (StoreType == StoreType.ONCE)
					StoreBackup();
				else
					RTC_StepActions.StoreDataPool.Add(this);

			}
			//Limiter handling. Normal operation is to not do anything if it doesn't match the limiter. Inverted is to only continue if it doesn't match
			if (LimiterTime == LimiterTime.PREEXECUTE)
			{
				if (InvertLimiter)
				{
					if (RTC_Filtering.LimiterPeekBytes(Address,
						Address + Precision, Domain, LimiterListHash, mi))
						return false;
				}
				else
				{ 
					if (!RTC_Filtering.LimiterPeekBytes(Address,
						Address + Precision, Domain, LimiterListHash, mi))
						return false;
				}
			}

			return true;
		}
	}

	[Serializable]
	public class ActiveTableObject
	{
		public long[] Data { get; set; }

		public ActiveTableObject()
		{
		}

		public ActiveTableObject(long[] data)
		{
			Data = data;
		}
	}

	[Serializable]
	public class BlastGeneratorProto : INote
	{
		public string BlastType;
		public string Domain;
		public int Precision;
		public long StepSize;
		public long StartAddress;
		public long EndAddress;
		public long Param1;
		public long Param2;
		public string Mode;
		public BlastLayer Bl;

		public string Note { get; set; }

		public BlastGeneratorProto()
		{
		}

		public BlastGeneratorProto(string _note, string _blastType, string _domain, string _mode, int _precision, long _stepSize, long _startAddress, long _endAddress, long _param1, long _param2)
		{
			Note = _note;
			BlastType = _blastType;
			Domain = _domain;
			Precision = _precision;
			StartAddress = _startAddress;
			EndAddress = _endAddress;
			Param1 = _param1;
			Param2 = _param2;
			Mode = _mode;
			StepSize = _stepSize;
		}

		public BlastLayer GenerateBlastLayer()
		{
			switch (BlastType)
			{
				case "BlastByte":
					RTC_BlastByteGenerator bbGenerator = new RTC_BlastByteGenerator();
					Bl = bbGenerator.GenerateLayer(Note, Domain, StepSize, StartAddress, EndAddress, Param1, Param2, Precision, (BGBlastByteModes)Enum.Parse(typeof(BGBlastByteModes), Mode, true));
					break;
				case "BlastCheat":
					RTC_BlastCheatGenerator bcGenerator = new RTC_BlastCheatGenerator();
					Bl = bcGenerator.GenerateLayer(Note, Domain, StepSize, StartAddress, EndAddress, Param1, Param2, Precision, (BGBlastCheatModes)Enum.Parse(typeof(BGBlastCheatModes), Mode, true));
					break;
				case "BlastPipe":
					RTC_BlastPipeGenerator bpGenerator = new RTC_BlastPipeGenerator();
					Bl = bpGenerator.GenerateLayer(Note, Domain, StepSize, StartAddress, EndAddress, Param1, Param2, Precision, (BGBlastPipeModes)Enum.Parse(typeof(BGBlastPipeModes), Mode, true));
					break;
				default:
					return null;
			}

			return Bl;
		}

	}

	public class ProblematicProcess
	{
		public string Name { get; set; }
		public string Message { get; set; }
	}

	public interface INote
	{
		string Note { get; set; }
	}

	/// <summary>
	/// A generic object for combobox purposes.
	/// Has a name and a value of type T for storing any object.
	/// </summary>
	/// <typeparam name="T">The type of object you want the comboxbox value to be</typeparam>
	[Serializable]
	public class ComboBoxItem<T>
	{
		public string Name { get; set; }
		public T Value { get; set; }

		public ComboBoxItem(String name, T value)
		{
			Name = name;
			Value = value;
		}
		public ComboBoxItem()
		{
		}
	}
}