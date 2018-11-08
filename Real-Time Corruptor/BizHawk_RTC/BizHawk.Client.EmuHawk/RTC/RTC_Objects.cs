using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Nintendo.N64;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace RTC
{
	[XmlInclude(typeof(StashKey))]
	[XmlInclude(typeof(BlastLayer))]
	[XmlInclude(typeof(BlastCheat))]
	[XmlInclude(typeof(BlastByte))]
	[XmlInclude(typeof(BlastPipe))]
	[XmlInclude(typeof(BlastVector))]
	[XmlInclude(typeof(BlastUnit))]
	[Serializable]
	public class Stockpile
	{
		public List<StashKey> StashKeys = new List<StashKey>();

		public string Name;
		public string Filename = null;
		public string ShortFilename;
		public string RtcVersion;

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
			return Name ?? String.Empty;
		}

		public void Save(bool isQuickSave = false)
		{
			Save(this, isQuickSave);
		}

		public static bool Save(Stockpile sks, bool isQuickSave = false)
		{
			string tempFilename = "";
			try
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
						sks.ShortFilename = sks.Filename.Substring(sks.Filename.LastIndexOf("\\") + 1,
							sks.Filename.Length - (sks.Filename.LastIndexOf("\\") + 1));
					}
					else
						return false;
				}
				else
				{
					sks.Filename = RTC_StockpileManager.currentStockpile.Filename;
					sks.ShortFilename = RTC_StockpileManager.currentStockpile.ShortFilename;
				}

				//Backuping bizhawk settings
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_EVENT_SAVEBIZHAWKCONFIG), true);

				//Watermarking RTC Version
				sks.RtcVersion = RTC_Core.RtcVersion;

				List<string> allRoms = new List<string>();

				//populating Allroms array
				foreach (StashKey key in sks.StashKeys)
					if (!allRoms.Contains(key.RomFilename))
					{
						allRoms.Add(key.RomFilename);

						if (key.RomFilename.ToUpper().Contains(".CUE"))
						{
							string cueFolder = RTC_Extensions.getLongDirectoryFromPath(key.RomFilename);
							string[] cueLines = File.ReadAllLines(key.RomFilename);
							List<string> binFiles = new List<string>();

							foreach (string line in cueLines)
								if (line.Contains("FILE") && line.Contains("BINARY"))
								{
									int startFilename = line.IndexOf('"') + 1;
									int endFilename = line.LastIndexOf('"');

									binFiles.Add(line.Substring(startFilename, endFilename - startFilename));
								}

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

				//clean temp2 folder
				foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP2"))
				{
					File.SetAttributes(file, FileAttributes.Normal);
					File.Delete(file);
				}

				//populating temp2 folder with roms
				for (int i = 0; i < allRoms.Count; i++)
				{
					string rom = allRoms[i];
					string romTempfilename = RTC_Core.rtcDir + "\\TEMP2\\" + (rom.Substring(rom.LastIndexOf("\\") + 1,
						                         rom.Length - (rom.LastIndexOf("\\") + 1)));

					if (!rom.Contains("\\"))
						rom = RTC_Core.rtcDir + "\\TEMP\\" + rom;

					if (File.Exists(romTempfilename))
					{
						File.SetAttributes(romTempfilename, FileAttributes.Normal);
						File.Delete(romTempfilename);
						File.Copy(rom, romTempfilename);
					}
					else
						File.Copy(rom, romTempfilename);
				}

				//clean temp folder
				EmptyFolder("TEMP");
				//foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP"))
				//    File.Delete(file);

				//sending back filtered files from temp2 folder to temp
				foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP2"))
					File.Move(file,
						RTC_Core.rtcDir + "\\TEMP\\" + (file.Substring(file.LastIndexOf("\\") + 1,
							file.Length - (file.LastIndexOf("\\") + 1))));

				//clean temp2 folder again
				EmptyFolder("TEMP2");
				//foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP2"))
				//    File.Delete(file);

				foreach (StashKey key in sks.StashKeys)
				{
					string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

					if (!File.Exists(RTC_Core.rtcDir + "\\TEMP\\" + statefilename))
						File.Copy(RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename,
							RTC_Core.rtcDir + "\\TEMP\\" + statefilename); // copy savestates to temp folder
				}

				if (File.Exists(RTC_Core.bizhawkDir + "\\config.ini"))
					File.Copy(RTC_Core.bizhawkDir + "\\config.ini", RTC_Core.rtcDir + "\\TEMP\\config.ini");

				foreach (StashKey sk in sks.StashKeys)
					sk.RomFilename = RTC_Extensions.getShortFilenameFromPath(sk.RomFilename);

				//creater stockpile.xml to temp folder from stockpile object
				using (FileStream fs = File.Open(RTC_Core.rtcDir + "\\TEMP\\stockpile.xml", FileMode.OpenOrCreate))
				{
					XmlSerializer xs = new XmlSerializer(typeof(Stockpile));
					xs.Serialize(fs, sks);
					fs.Close();
				}
				//7z the temp folder to destination filename
				//string[] stringargs = { "-c", sks.Filename, RTC_Core.rtcDir + "\\TEMP\\" };
				//FastZipProgram.Exec(stringargs);

				tempFilename = sks.Filename + ".temp";

				CompressionLevel comp = System.IO.Compression.CompressionLevel.Fastest;

				if (!RTC_Core.ghForm.cbCompressStockpiles.Checked)
					comp = System.IO.Compression.CompressionLevel.NoCompression;


				if (File.Exists(tempFilename))
				{
					if(MessageBox.Show("There is an existing temp stockpile.\nThis may be the result of a previously failed save.\nThat temp file is named " + tempFilename + " in case you want to back it up.\nDo you want to continue saving?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return false;
					}
					File.Delete(tempFilename);
				}


				System.IO.Compression.ZipFile.CreateFromDirectory(RTC_Core.rtcDir + "\\TEMP\\", tempFilename, comp,
					false);

				if (File.Exists(sks.Filename))
					File.Delete(sks.Filename);

				File.Move(tempFilename, sks.Filename);

				RTC_StockpileManager.currentStockpile = sks;

				RTC_StockpileManager.unsavedEdits = false;

				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong while saving your stockpile!\n");

				if (File.Exists(tempFilename) && File.Exists((sks.Filename)))
				{
					MessageBox.Show(
						"Your original file should be untouched. The temporary file we were trying to save to is named " +
						tempFilename + "\n It's possible that the save completed and it failed to overwrite the original file.\nIf this is the case, the .temp file should work if you rename it to .sks\n\n" + ex);
				}

				else if (File.Exists(tempFilename) && !File.Exists(sks.Filename))
				{
					MessageBox.Show(
						"Something went wrong when trying to rename the temp file" + tempFilename + " to " + sks.Filename + "!\n If it got this far, the temporary file should be intact and you can just remove .temp from the end yourself.\nMake sure nothing is locking the original file to prevent this.\n\n" + ex);
				}

				else if (!File.Exists(tempFilename) && File.Exists(sks.Filename))
				{
					MessageBox.Show(
						"Something went wrong when trying to write to the temp file " + tempFilename + ". Make sure nothing is locking that file. Your original stockpile should be intact.\n\n" + ex);
				}

				return false;
			}
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
					Filename = ofd.FileName.ToString();
				}
				else
					return false;
			}

			if (!File.Exists(Filename))
			{
				MessageBox.Show("The Stockpile file wasn't found");
				return false;
			}

			Guid token = RTC_NetCore.HugeOperationStart();

			Extract(Filename, "TEMP", "stockpile.xml");

			Stockpile sks;

			try
			{
				using (FileStream fs = File.Open(RTC_Core.rtcDir + "\\TEMP\\stockpile.xml", FileMode.OpenOrCreate))
				{
					XmlSerializer xs = new XmlSerializer(typeof(Stockpile));
					sks = (Stockpile)xs.Deserialize(fs);
					fs.Close();
				}
			}
			catch
			{
				MessageBox.Show("The Stockpile file could not be loaded");
				RTC_NetCore.HugeOperationEnd(token);
				return false;
			}

			RTC_StockpileManager.currentStockpile = sks;

			// repopulating savestates out of temp folder
			foreach (StashKey key in sks.StashKeys)
			{
				string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

				string systemFolder = RTC_Core.bizhawkDir + "\\" + key.SystemName;
				string systemStateFolder = RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\";

				if (!Directory.Exists(systemFolder))
					Directory.CreateDirectory(systemFolder);

				if (!Directory.Exists(systemStateFolder))
					Directory.CreateDirectory(systemStateFolder);

				if (!File.Exists(RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename))
					File.Copy(RTC_Core.rtcDir + "\\TEMP\\" + statefilename, RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename); // copy savestates to temp folder
			}

			foreach (StashKey t in sks.StashKeys)
			{
				t.RomFilename = RTC_Core.rtcDir + "\\TEMP\\" + t.RomFilename;
			}

			//fill list controls
			dgvStockpile.Rows.Clear();

			foreach (StashKey key in sks.StashKeys)
				dgvStockpile.Rows.Add(key, key.GameName, key.SystemName, key.SystemCore, key.Note);

			RTC_Core.ghForm.RefreshNoteIcons();
			RTC_Core.spForm.RefreshNoteIcons();

			sks.Filename = Filename;

			CheckCompatibility(sks);

			RTC_NetCore.HugeOperationEnd(token);

			return true;
		}

		public static void CheckCompatibility(Stockpile sks)
		{
			List<string> errorMessages = new List<string>();

			if (sks.RtcVersion != RTC_Core.RtcVersion)
			{
				if (sks.RtcVersion == null)
					errorMessages.Add("You have loaded a broken stockpile that didn't contain an RTC Version number\n. There is no reason to believe that these items will work.");
				else
					errorMessages.Add("You have loaded a stockpile created with RTC " + sks.RtcVersion + " using RTC " + RTC_Core.RtcVersion + "\n" + "Items might not appear identical to how they when they were created or it is possible that they don't work if BizHawk was upgraded.");
			}

			if (errorMessages.Count == 0)
				return;

			string message = "The loaded stockpile returned the following errors:\n\n";

			foreach (string line in errorMessages)
				message += $"•  {line} \n\n";

			MessageBox.Show(message, "Compatibility Checker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

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
				foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + $"\\{folder}"))
				{
					File.SetAttributes(file, FileAttributes.Normal);
					File.Delete(file);
				}

				foreach (string dir in Directory.GetDirectories(RTC_Core.rtcDir + $"\\{folder}"))
					RecursiveDelete(new DirectoryInfo(dir));
			}
			catch (System.IO.IOException ex)
			{
				MessageBox.Show("Unable to empty a temp folder! If your stockpile has any CD based games, close them before saving the stockpile! If this isn't the case, report this bug to the RTC developers.");
				throw new Exception(ex.ToString());
			}
		}

		public static void Extract(string filename, string folder, string masterFile)
		{
			try
			{
				EmptyFolder(folder);
				ZipFile.ExtractToDirectory(filename, RTC_Core.rtcDir + $"\\{folder}\\");

				if (!File.Exists(RTC_Core.rtcDir + $"\\{folder}\\{masterFile}"))
				{
					MessageBox.Show("The file could not be read properly");

					EmptyFolder(folder);
				}
			}
			catch
			{
				//If it errors out, empty the folder
				MessageBox.Show("The file could not be read properly");
				EmptyFolder(folder);
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

			if (File.Exists(RTC_Core.bizhawkDir + "\\import_config.ini"))
				File.Delete(RTC_Core.bizhawkDir + "\\import_config.ini");
			File.Copy(Filename, RTC_Core.bizhawkDir + "\\import_config.ini");

			if (File.Exists(RTC_Core.bizhawkDir + "\\stockpile_config.ini"))
				File.Delete(RTC_Core.bizhawkDir + "\\stockpile_config.ini");
			File.Copy(RTC_Core.bizhawkDir + "\\config.ini", RTC_Core.bizhawkDir + "\\stockpile_config.ini");

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_IMPORTKEYBINDS), true);

			Process.Start(RTC_Core.bizhawkDir + $"\\StockpileConfig{(RTC_Core.isStandalone ? "DETACHED" : "ATTACHED")}.bat");
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

			if (File.Exists(RTC_Core.bizhawkDir + "\\backup_config.ini"))
			{
				if (MessageBox.Show("Do you want to overwrite the previous Config Backup with the current Bizhawk Config?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					File.Delete(RTC_Core.bizhawkDir + "\\backup_config.ini");
					File.Copy((RTC_Core.bizhawkDir + "\\config.ini"), (RTC_Core.bizhawkDir + "\\backup_config.ini"));
				}
			}
			else
				File.Copy((RTC_Core.bizhawkDir + "\\config.ini"), (RTC_Core.bizhawkDir + "\\backup_config.ini"));

			Extract(Filename, "TEMP", "stockpile.xml");

			if (File.Exists(RTC_Core.bizhawkDir + "\\stockpile_config.ini"))
				File.Delete(RTC_Core.bizhawkDir + "\\stockpile_config.ini");
			File.Copy((RTC_Core.rtcDir + "\\TEMP\\config.ini"), (RTC_Core.bizhawkDir + "\\stockpile_config.ini"));

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_MERGECONFIG), true);

			Process.Start(RTC_Core.bizhawkDir + $"\\StockpileConfig{(RTC_Core.isStandalone ? "DETACHED" : "ATTACHED")}.bat");
		}

		public static void MergeBizhawkConfig_NET()
		{
			Config bc;
			Config sc;

			FileInfo fileBc = new FileInfo(RTC_Core.bizhawkDir + "\\backup_config.ini");
			FileInfo fileSc = new FileInfo(RTC_Core.bizhawkDir + "\\stockpile_config.ini");

			using (StreamReader reader = fileBc.OpenText())
			{
				JsonTextReader r = new JsonTextReader(reader);
				bc = (Config)ConfigService.Serializer.Deserialize(r, typeof(Config));
			}

			using (StreamReader reader = fileSc.OpenText())
			{
				JsonTextReader r = new JsonTextReader(reader);
				sc = (Config)ConfigService.Serializer.Deserialize(r, typeof(Config));
			}

			//bc = (JObject)JsonConvert.DeserializeObject(backupConfig);
			//sc = (JObject)JsonConvert.DeserializeObject(stockpileConfig);

			sc.HotkeyBindings = bc.HotkeyBindings;
			sc.AllTrollers = bc.AllTrollers;
			sc.AllTrollersAutoFire = bc.AllTrollersAutoFire;
			sc.AllTrollersAnalog = bc.AllTrollersAnalog;

			if (File.Exists(RTC_Core.bizhawkDir + "\\stockpile_config.ini"))
				File.Delete(RTC_Core.bizhawkDir + "\\stockpile_config.ini");

			try
			{
				using (StreamWriter writer = fileSc.CreateText())
				{
					JsonTextWriter w = new JsonTextWriter(writer) { Formatting = Formatting.Indented };
					ConfigService.Serializer.Serialize(w, sc);
				}
			}
			catch
			{
				/* Eat it */
			}
		}

		public static void ImportBizhawkKeybinds_NET()
		{
			Config bc;
			Config sc;

			FileInfo fileBc = new FileInfo(RTC_Core.bizhawkDir + "\\import_config.ini");
			FileInfo fileSc = new FileInfo(RTC_Core.bizhawkDir + "\\stockpile_config.ini");

			using (StreamReader reader = fileBc.OpenText())
			{
				JsonTextReader r = new JsonTextReader(reader);
				bc = (Config)ConfigService.Serializer.Deserialize(r, typeof(Config));
			}

			using (StreamReader reader = fileSc.OpenText())
			{
				JsonTextReader r = new JsonTextReader(reader);
				sc = (Config)ConfigService.Serializer.Deserialize(r, typeof(Config));
			}

			//bc = (JObject)JsonConvert.DeserializeObject(backupConfig);
			//sc = (JObject)JsonConvert.DeserializeObject(stockpileConfig);

			sc.HotkeyBindings = bc.HotkeyBindings;
			sc.AllTrollers = bc.AllTrollers;
			sc.AllTrollersAutoFire = bc.AllTrollersAutoFire;
			sc.AllTrollersAnalog = bc.AllTrollersAnalog;

			if (File.Exists(RTC_Core.bizhawkDir + "\\stockpile_config.ini"))
				File.Delete(RTC_Core.bizhawkDir + "\\stockpile_config.ini");

			try
			{
				using (StreamWriter writer = fileSc.CreateText())
				{
					JsonTextWriter w = new JsonTextWriter(writer) { Formatting = Formatting.Indented };
					ConfigService.Serializer.Serialize(w, sc);
				}
			}
			catch
			{
				/* Eat it */
			}
		}

		public static void RestoreBizhawkConfig()
		{
			Process.Start(RTC_Core.bizhawkDir + $"\\RestoreConfig{(RTC_Core.isStandalone ? "DETACHED" : "ATTACHED")}.bat");
		}

		public static void Import()
		{
			Import(null, false);
		}

		public static void Import(string filename, bool corruptCloud)
		{
			//clean temp folder
			EmptyFolder("TEMP3");
			//foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP3"))
			//    File.Delete(file);

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

			ZipFile.ExtractToDirectory(filename, RTC_Core.rtcDir + $"\\TEMP3\\");

			if (!File.Exists(RTC_Core.rtcDir + "\\TEMP3\\stockpile.xml"))
			{
				MessageBox.Show("The file could not be read properly");

				EmptyFolder("TEMP3");

				return;
			}

			//stockpile deserialization
			Stockpile sks;

			try
			{
				using (FileStream FS = File.Open(RTC_Core.rtcDir + "\\TEMP3\\stockpile.xml", FileMode.OpenOrCreate))
				{
					XmlSerializer xs = new XmlSerializer(typeof(Stockpile));
					sks = (Stockpile)xs.Deserialize(FS);
					FS.Close();
				}
			}
			catch
			{
				MessageBox.Show("The Stockpile file could not be loaded");
				return;
			}

			// repopulating savestates out of temp folder
			foreach (StashKey key in sks.StashKeys)
			{
				string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

				string systemFolder = RTC_Core.bizhawkDir + "\\" + key.SystemName;
				string systemStateFolder = RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\";

				if (!Directory.Exists(systemFolder))
					Directory.CreateDirectory(systemFolder);

				if (!Directory.Exists(systemStateFolder))
					Directory.CreateDirectory(systemStateFolder);

				if (!File.Exists(RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename))
					File.Copy(RTC_Core.rtcDir + "\\TEMP3\\" + statefilename, RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename); // copy savestates to temp folder
			}

			foreach (StashKey sk in sks.StashKeys)
			{
				sk.RomFilename = RTC_Core.rtcDir + "\\TEMP\\" + sk.RomFilename;
			}

			foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP3\\"))
				if (!file.Contains(".sk") && !file.Contains(".timejump.State"))
					try
					{
						File.Copy(file, RTC_Core.rtcDir + "\\TEMP\\" + file.Substring(file.LastIndexOf('\\'), file.Length - file.LastIndexOf('\\'))); // copy roms to temp
					}
					catch { }

			EmptyFolder("TEMP3");

			//fill list controls

			foreach (StashKey sk in sks.StashKeys)
			{
				DataGridViewRow dgvRow = RTC_Core.ghForm.dgvStockpile.Rows[RTC_Core.ghForm.dgvStockpile.Rows.Add()];
				dgvRow.Cells["Item"].Value = sk;
				dgvRow.Cells["GameName"].Value = sk.GameName;
				dgvRow.Cells["SystemName"].Value = sk.SystemName;
				dgvRow.Cells["SystemCore"].Value = sk.SystemCore;
			}

			RTC_Core.ghForm.RefreshNoteIcons();
			CheckCompatibility(sks);

			RTC_StockpileManager.StockpileChanged();
		}
	}

	[Serializable]
	public class StashKey : ICloneable
	{
		public string RomFilename;
		public byte[] RomData = null;

		public string StateShortFilename = null;
		public string StateFilename = null;
		public byte[] StateData = null;

		public string SystemName;
		public string SystemDeepName;
		public string SystemCore;
		public List<string> SelectedDomains = new List<string>();
		public string GameName;
		public string Note = null;
		public string SyncSettings = null;

		public string Key;
		public string ParentKey = null;
		public BlastLayer BlastLayer = null;

		public string Alias
		{
			get => alias ?? Key;
			set => alias = value;
		}

		private string alias;

		public StashKey(string key, string parentkey, BlastLayer blastlayer)
		{
			Key = key;
			ParentKey = parentkey;
			BlastLayer = blastlayer;

			RomFilename = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETOPENROMFILENAME), true);
			SystemName = RTC_Core.EmuFolderCheck((string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETSYSTEMNAME), true));
			SystemCore = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETSYSTEMCORE) { objectValue = SystemName }, true);
			GameName = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETGAMENAME), true);
			SyncSettings = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETSYNCSETTINGS), true);

			this.SelectedDomains.AddRange(RTC_MemoryDomains.SelectedDomains);
		}

		public StashKey()
		{
		}

		public object Clone()
		{
			object sk = ObjectCopier.Clone(this);
			((StashKey)sk).Key = RTC_Core.GetRandomKey();
			((StashKey)sk).Alias = null;
			return sk;
		}

		public static void SetCore(StashKey sk) => SetCore(sk.SystemName, sk.SystemCore);

		public static void SetCore(string systemName, string systemCore)
		{
			switch (systemName.ToUpper())
			{
				case "GAMEBOY":
					Global.Config.GB_AsSGB = systemCore == "sameboy";
					Global.Config.SGB_UseBsnes = false;
					Global.Config.GB_UseGBHawk = systemCore == "gbhawk";

					break;

				case "NES":
					Global.Config.NES_InQuickNES = systemCore == "quicknes";
					break;

				case "SNES":

					if (systemCore == "bsnes_SGB")
					{
						Global.Config.GB_AsSGB = true;
						Global.Config.SGB_UseBsnes = true;
					}
					else
						Global.Config.SNES_InSnes9x = systemCore == "snes9x";

					break;

				case "GBA":
					Global.Config.GBA_UsemGBA = systemCore == "mgba";
					break;

				case "N64":

					//Leaving this here for backwards compatability with 3.10
					//TODO: Remove this
					string[] coreParts = systemCore.Split('/');
					N64SyncSettings ss = (N64SyncSettings)Global.Config.GetCoreSyncSettings<N64>()
					?? new N64SyncSettings();
					ss.VideoPlugin = (PluginType)Enum.Parse(typeof(PluginType), coreParts[0], true);
					ss.Rsp = (N64SyncSettings.RspType)Enum.Parse(typeof(N64SyncSettings.RspType), coreParts[1], true);
					ss.Core = (N64SyncSettings.CoreType)Enum.Parse(typeof(N64SyncSettings.CoreType), coreParts[2], true);
					ss.DisableExpansionSlot = (coreParts[3] == "NoExp");
					N64VideoPluginconfig.PutSyncSettings(ss);

					break;
			}
		}

		public static string getCoreName_NET(string systemName)
		{
			try
			{

				SettingsAdapter settable = new SettingsAdapter(Global.Emulator);

				switch (systemName.ToUpper())
				{
					case "GAMEBOY":

						if (Global.Config.GB_AsSGB)
							return "sameboy";
						else if (Global.Config.GB_UseGBHawk)
							return "gbhawk";
						else
							return "gambatte";

					case "NES":
						return (Global.Config.NES_InQuickNES ? "quicknes" : "neshawk");

					case "SNES":

						if (RTC_MemoryDomains.MemoryInterfaces.ContainsKey("SGB WRAM"))
							return "bsnes_SGB";

						return (Global.Config.SNES_InSnes9x ? "snes9x" : "bsnes");

					case "GBA":
						return (Global.Config.GBA_UsemGBA ? "mgba" : "vba-next");

					case "N64":

						N64SyncSettings ss = (N64SyncSettings)Global.Config.GetCoreSyncSettings<N64>()
						                     ?? new N64SyncSettings();

						return $"{ss.VideoPlugin}/{ss.Rsp}/{ss.Core}/{(ss.DisableExpansionSlot ? "NoExp" : "Exp")}";
					default:
						break;
				}

				return systemName;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static string getSyncSettings_NET(string ss)
		{
			SettingsAdapter settable = new SettingsAdapter(Global.Emulator);
			if (settable.HasSyncSettings)
			{
				ss = ConfigService.SaveWithType(settable.GetSyncSettings());
				return ss;
			}
			return null;
		}

		public static void putSyncSettings_NET(string ss)
		{
			SettingsAdapter settable = new SettingsAdapter(Global.Emulator);
			if (settable.HasSyncSettings)
			{
				settable.PutSyncSettings(ConfigService.LoadWithType(ss));
			}
		}

		public override string ToString()
		{
			return Alias;
		}

		public bool Run()
		{
			RTC_StockpileManager.currentStashkey = this;
			return RTC_StockpileManager.ApplyStashkey(this);
		}

		public void RunOriginal()
		{
			RTC_StockpileManager.currentStashkey = this;
			RTC_StockpileManager.OriginalFromStashkey(this);
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
			return RTC_Core.bizhawkDir + "\\" + this.SystemName + "\\State\\" + this.GameName + "." + this.ParentKey + ".timejump.State"; // get savestate name
		}
	}

	[Serializable]
	public class SaveStateKeys
	{
		public StashKey[] StashKeys = new StashKey[41];
		public string[] Text = new string[41];
	}

	[Serializable]
	public class BlastTarget
	{
		public string domain = null;
		public long address = 0;

		public BlastTarget(string _domain, long _address)
		{
			domain = _domain;
			address = _address;
		}
	}

	[XmlInclude(typeof(BlastCheat))]
	[XmlInclude(typeof(BlastByte))]
	[XmlInclude(typeof(BlastPipe))]
	[XmlInclude(typeof(BlastVector))]
	[XmlInclude(typeof(BlastUnit))]
	[Serializable]
	public class BlastLayer : ICloneable
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
			return ObjectCopier.Clone(this);
		}

		public void Apply(bool ignoreMaximums = false)
		{

			//Console.WriteLine("Entering Blastlayer Apply");
			if (RTC_Core.isStandalone)
			{
				RTC_Core.SendCommandToBizhawk(new RTC.RTC_Command(CommandType.BLAST) { blastlayer = this });
				return;
			}

			/*
			if (this != RTC_StockpileManager.lastBlastLayerBackup &&
				RTC_Core.SelectedEngine != CorruptionEngine.HELLGENIE &&
				RTC_Core.SelectedEngine != CorruptionEngine.FREEZE &&
				RTC_Core.SelectedEngine != CorruptionEngine.PIPE)
				RTC_StockpileManager.lastBlastLayerBackup = GetBackup();
            */

			if (this != RTC_StockpileManager.lastBlastLayerBackup)
				RTC_StockpileManager.lastBlastLayerBackup = GetBackup();

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
						"The operation was cancelled\n\n" + bb.ToString());
				}
			}
			catch (Exception ex)
			{
				throw new Exception(
							"An error occurred in RTC while applying a BlastLayer to the game.\n\n" +
							"The operation was cancelled\n\n" +
							ex.ToString()
							);
			}
			finally
			{
				if (!ignoreMaximums)
				{
					RTC_HellgenieEngine.RemoveExcessCheats();
					RTC_PipeEngine.RemoveExcessPipes();
				}
				//Console.WriteLine("Exiting Blastlayer Apply");
			}
		}

		public BlastLayer GetBackup()
		{
			List<BlastUnit> BackupLayer = new List<BlastUnit>(); ;

			BackupLayer.AddRange(Layer.Select(it => it.GetBackup()));

			return new BlastLayer(BackupLayer);
		}

		public void Reroll()
		{
			foreach (BlastUnit bu in Layer)
				bu.Reroll();
		}

		public void Rasterize()
		{
			foreach (BlastUnit bu in Layer)
				bu.Rasterize();
		}
	}

	[Serializable]
	public abstract class BlastUnit 
	{
		public abstract bool Apply();

		public abstract BlastUnit GetBackup();

		public abstract void Reroll();

		public abstract void Rasterize();

		public abstract bool IsEnabled { get; set; }
		public abstract bool IsLocked { get; set; }
		public abstract bool BigEndian { get; set; }

		public abstract string Domain { get; set; }
		public abstract long Address { get; set; }
		public abstract string Note { get; set; }

	}

	[Serializable]
	public class BlastByte : BlastUnit
	{
		public override string Domain { get; set; }
		public override long Address { get; set; }
		public BlastByteType Type;
		public byte[] Value;
		public override bool IsEnabled { get; set; }
		public override bool IsLocked { get; set; } = false;
		public override bool BigEndian { get; set; }
		public override string Note { get; set; } = "";

		public BlastByte(string _domain, long _address, BlastByteType _type, byte[] _value, bool _bigEndian, bool _isEnabled, string _note = "")
		{
			Domain = _domain;
			Address = _address - (_address % _value.Length);

			Type = _type;
			Value = _value;
			IsEnabled = _isEnabled;
			BigEndian = _bigEndian;
			Note = _note;
		}

		public BlastByte()
		{
		}

		public override void Rasterize()
		{
			if (Domain.Contains("[V]"))
			{
				/*
                Tuple<string,long> mp = (RTC_MemoryDomains.VmdPool[Domain] as VirtualMemoryDomain)?.MemoryPointers[(int)Address];
                if (mp == null)
                    return;

                Domain = mp.Item1;
                Address = mp.Item2;
                */

				string _domain = (string)Domain.Clone();
				long _address = Address;

				Domain = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerDomains[(int)_address] ?? "ERROR";
				Address = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerAddresses[(int)_address] ?? -1;
			}
		}

		public override bool Apply()
		{
			if (!IsEnabled)
				return true;

			try
			{
				MemoryInterface mi = RTC_MemoryDomains.GetInterface(Domain);

				if (mi == null)
					return false;

				byte[] _Values = (byte[])Value.Clone();

				if ((Type == BlastByteType.VECTOR || Type == BlastByteType.ADD || Type == BlastByteType.SUBSTRACT) && BigEndian)
					_Values.FlipBytes();

				//When using ADD and SUBSTRACT we need to properly handle multi-byte values.
				//This means that it has to properly roll-over.
				//00 00 00 FF needs to become 00 00 01 00,  FF FF FF FF needs to become 00 00 00 00, etc
				//We assume that the user is going to be using SET and VECTOR more than ADD and SUBSTRACT so check them first
				switch (Type)
				{
					case (BlastByteType.SET):
					case (BlastByteType.VECTOR):
						break;

					case (BlastByteType.ADD):
						_Values = RTC_Extensions.AddValueToByteArray(mi.PeekBytes(Address, Address + _Values.Length), RTC_Extensions.GetDecimalValue(_Values, !(BigEndian)), BigEndian);
						break;

					case (BlastByteType.SUBSTRACT):
						_Values = RTC_Extensions.AddValueToByteArray(mi.PeekBytes(Address, Address + _Values.Length), RTC_Extensions.GetDecimalValue(_Values, !(BigEndian)) * -1, BigEndian);
						break;

					case (BlastByteType.NONE):
						return true;
				}

				//As add and substract are accounted for already, we no longer need to check the type here.
				for (int i = 0; i < _Values.Length; i++)
					mi.PokeByte(Address + i, _Values[i]);
			}
			catch (Exception ex)
			{
				throw new Exception("The BlastByte apply() function threw up. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
				ex.ToString());
			}

			return true;
		}

		public override BlastUnit GetBackup()
		{
			if (!IsEnabled)
				return null;

			try
			{
				MemoryInterface mi = RTC_MemoryDomains.GetInterface(Domain);
				

				if (mi == null || Type == BlastByteType.NONE)
					return null;

				byte[] _value = new byte[Value.Length];

				for (int i = 0; i < _value.Length; i++)
					_value[i] = mi.PeekByte(Address + i);

				return new BlastByte(Domain, Address, BlastByteType.SET, _value, BigEndian, true);
			}
			catch (Exception ex)
			{
				throw new Exception("The BlastByte GetBackup() function threw up. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
				ex.ToString());
			}
		}

		public override void Reroll()
		{
			if (Type == BlastByteType.SET)
			{
				long randomValue = 0;
				switch (Value.Length)
				{
					case (1):
						randomValue = RTC_Core.RND.RandomLong(RTC_NightmareEngine.MinValue8Bit, RTC_NightmareEngine.MaxValue8Bit);
						break;
					case (2):
						randomValue = RTC_Core.RND.RandomLong(RTC_NightmareEngine.MinValue16Bit, RTC_NightmareEngine.MaxValue16Bit);
						break;
					case (4):
						randomValue = RTC_Core.RND.RandomLong(RTC_NightmareEngine.MinValue32Bit, RTC_NightmareEngine.MaxValue32Bit);
						break;
				}
				Value = RTC_Extensions.GetByteArrayValue(Value.Length, randomValue, true);
			}
			else if (Type == BlastByteType.ADD || Type == BlastByteType.SUBSTRACT)
			{
				var result = RTC_Core.RND.Next(1, 3);
				switch (result)
				{
					case 1:
						Type = BlastByteType.ADD;
						break;

					case 2:
						Type = BlastByteType.SUBSTRACT;
						break;
				}
			}
			else if (Type == BlastByteType.VECTOR)
			{
				Value = RTC_VectorEngine.GetRandomConstant(RTC_VectorEngine.ValueList);
			}
		}

		public override string ToString()
		{
			string enabledString = "[ ] BlastByte -> ";
			if (IsEnabled)
				enabledString = "[x] BlastByte -> ";

			string cleanDomainName = Domain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"
			return (enabledString + cleanDomainName + "(" + Convert.ToInt32(Address).ToString() + ")." + Type.ToString() + "(" + RTC_Extensions.GetDecimalValue(Value, BigEndian).ToString() + ")");
		}
	}

	[Serializable]
	public class BlastVector : BlastUnit
	{
		public override string Domain { get; set; }
		public override long Address { get; set; }
		public override bool IsLocked { get; set; } = false;
		public override bool BigEndian { get; set; } = true;
		public override string Note { get; set; } = "";
		public BlastByteType Type;

		public byte[] Values;

		public override bool IsEnabled { get; set; }

		public BlastVector(string _domain, long _address, byte[] _values, bool _isEnabled, string _note = "")
		{
			Domain = _domain;
			Address = (_address - (_address % 4));
			Values = _values;
			IsEnabled = _isEnabled;
			Note = _note;
		}

		public BlastVector()
		{
		}

		public override void Rasterize()
		{
			if (Domain.Contains("[V]"))
			{
				/*
                Tuple<string,long> mp = (RTC_MemoryDomains.VmdPool[Domain] as VirtualMemoryDomain)?.MemoryPointers[(int)Address];
                if (mp == null)
                    return;

                Domain = mp.Item1;
                Address = mp.Item2;
                */

				string _domain = (string)Domain.Clone();
				long _address = Address;

				Domain = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerDomains[(int)_address] ?? "ERROR";
				Address = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerAddresses[(int)_address] ?? -1;
			}
		}

		public override bool Apply()
		{
			if (!IsEnabled)
				return true;

			try
			{
				MemoryInterface mi = RTC_MemoryDomains.GetInterface(Domain);

				if (mi == null)
					return true;


				for(int i = 0; i < 4; i++)
					mi.PokeByte(Address + i, Values[i]);
			}
			catch (Exception ex)
			{
				throw new Exception("The BlastVector apply() function threw up. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
				ex.ToString());
			}

			return true;
		}

		public override BlastUnit GetBackup()
		{
			if (!IsEnabled)
				return null;

			try
			{
				MemoryInterface mi = RTC_MemoryDomains.GetInterface(Domain);
				if (mi == null)
					return null;

				return new BlastVector(Domain, Address, mi.PeekBytes(Address, Address + 4), true);
			}
			catch (Exception ex)
			{
				throw new Exception("The BlastVector GetBackup() function threw up. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
				ex.ToString());
			}
		}

		public override void Reroll()
		{
			Values = RTC_VectorEngine.GetRandomConstant(RTC_VectorEngine.ValueList);
		}

		public override string ToString()
		{
			string EnabledString = "[ ] BlastVector -> ";
			if (IsEnabled)
				EnabledString = "[x] BlastVector -> ";

			string cleanDomainName = Domain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"
			return (EnabledString + cleanDomainName + "(" + Convert.ToUInt32(Address).ToString() + ")." + Type.ToString() + "(" + RTC_VectorEngine.ByteArrayToString(Values) + ")");
		}
	}

	[Serializable]
	public class BlastPipe : BlastUnit
	{
		public override string Domain { get; set; }
		public override long Address { get; set; }
		public override bool BigEndian { get; set; }
		public override bool IsLocked { get; set; } = false;
		public override string Note { get; set; } = "";
		public string PipeDomain;
		public long PipeAddress;
		public int PipeSize;
		public int TiltValue;

		public override bool IsEnabled { get; set; }

		public BlastPipe(string _domain, long _address, string _pipeDomain, long _pipeAddress, int _tiltValue, int _pipeSize, bool _bigEndian, bool _isEnabled, string _note = "")
		{
			Domain = _domain;
			Address = _address;
			PipeDomain = _pipeDomain;
			PipeAddress = _pipeAddress;
			PipeSize = _pipeSize;
			IsEnabled = _isEnabled;
			TiltValue = _tiltValue;
			BigEndian = _bigEndian;
			Note = _note;
		}

		public BlastPipe()
		{
			new object();
		}

		public void Execute()
		{
			try
			{
				MemoryInterface mi = RTC_MemoryDomains.GetInterface(Domain);
				MemoryInterface mi2 = RTC_MemoryDomains.GetInterface(PipeDomain);

				if (mi == null || mi2 == null)
					throw new Exception($"Memory Domain error, MD1 -> {mi?.ToString()}, md2 -> {mi2?.ToString()}");

				for (int i = 0; i < PipeSize; i++)
				{
					int currentValue = (int)mi.PeekByte(Address + i);

					int newValue = currentValue + TiltValue;

					if (newValue < 0)
						newValue = 0;
					else if (newValue > 255)
						newValue = 255;

					mi2.PokeByte(PipeAddress + i, (byte)newValue);
				}
			}
			catch (Exception ex)
			{
				RTC_PipeEngine.ClearPipes();
				throw new Exception("The BlastPipe apply() function threw up. Clearing the execution pool to prevent further issues. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
				ex.ToString());
			}
		}

		public override void Rasterize()
		{
			if (Domain.Contains("[V]"))
			{
				/*
                Tuple<string,long> mp = (RTC_MemoryDomains.VmdPool[Domain] as VirtualMemoryDomain)?.MemoryPointers[(int)Address];
                if (mp != null)
                {
                    Domain = mp.Item1;
                    Address = mp.Item2;
                }
                */

				string _domain = (string)Domain.Clone();
				long _address = Address;

				Domain = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerDomains[(int)_address] ?? "ERROR";
				Address = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerAddresses[(int)_address] ?? -1;
			}

			if (PipeDomain.Contains("[V]"))
			{
				/*
                Tuple<string, long> mp = (RTC_MemoryDomains.VmdPool[PipeDomain] as VirtualMemoryDomain)?.MemoryPointers[(int)PipeAddress];
                if (mp != null)
                {
                    PipeDomain = mp.Item1;
                    PipeAddress = mp.Item2;
                }
                */
				string _domain = (string)PipeDomain.Clone();
				long _address = PipeAddress;

				PipeDomain = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerDomains[(int)_address] ?? "ERROR";
				PipeAddress = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerAddresses[(int)_address] ?? -1;
			}
		}

		public override bool Apply()
		{
			if (!IsEnabled)
				return true;

			RTC_PipeEngine.AddUnit(this);

			return true;
		}

		public override BlastUnit GetBackup()
		{
			if (!IsEnabled)
				return null;

			try
			{
				MemoryInterface mi = RTC_MemoryDomains.GetInterface(PipeDomain);

				if (mi == null)
					return null;

				byte[] _value = new byte[PipeSize];

				for (int i = 0; i < _value.Length; i++)
					_value[i] = mi.PeekByte(PipeAddress + i);

				return new BlastByte(PipeDomain, PipeAddress, BlastByteType.SET, _value, BigEndian, true);
			}
			catch (Exception ex)
			{
				throw new Exception("The BlastPipe GetBackup() function threw up. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
				ex.ToString());
			}
		}

		public override void Reroll()
		{
			var pipeEnd = RTC_Core.GetBlastTarget();

			PipeDomain = pipeEnd.domain;
			PipeAddress = pipeEnd.address;
		}

		public override string ToString()
		{
			string EnabledString = "[ ] BlastPipe -> ";
			if (IsEnabled)
				EnabledString = "[x] BlastPipe -> ";

			string cleanDomainName = Domain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"
			string cleanDomainName2 = PipeDomain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"
			return (EnabledString + cleanDomainName + "(" + Convert.ToInt32(Address).ToString() + ")piped->" + cleanDomainName2 + "(" + Convert.ToInt32(PipeAddress).ToString() + "), tilt->" + TiltValue.ToString());
		}
	}

	[Serializable]
	public class BlastCheat : BlastUnit
	{
		public override string Domain { get; set; }
		public override long Address { get; set; }
		public override bool BigEndian { get; set; }
		public override bool IsLocked { get; set; } = false;
		public override string Note { get; set; } = "";
		public BizHawk.Client.Common.DisplayType DisplayType;

		public byte[] Value;
		public bool IsFreeze;

		public override bool IsEnabled { get; set; }

		public BlastCheat(string _domain, long _address, BizHawk.Client.Common.DisplayType _displayType, bool _bigEndian, byte[] _value, bool _isEnabled, bool _isFreeze, string _note = "")
		{
			Domain = _domain;

			//Address = _address - (_address % (int)settings.Size);
			Address = _address - (_address % _value.Length);

			DisplayType = _displayType;
			BigEndian = _bigEndian;

			Value = _value;
			IsEnabled = _isEnabled;
			IsFreeze = _isFreeze;

			Note = _note;
		}

		public BlastCheat()
		{
		}

		public override void Rasterize()
		{
			if (Domain.Contains("[V]"))
			{
				/*
                Tuple<string, long> mp = (RTC_MemoryDomains.VmdPool[Domain] as VirtualMemoryDomain)?.MemoryPointers[(int)Address];
                if (mp == null)
                    return;

                Domain = mp.Item1;
                Address = mp.Item2;
                */

				string _domain = (string)Domain.Clone();
				long _address = Address;

				Domain = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerDomains[(int)_address] ?? "ERROR";
				Address = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerAddresses[(int)_address] ?? -1;
			}
		}

		public override bool Apply()
		{
			try
			{
				if (!IsEnabled)
					return true;

				//This technically doesn't work properly with hybrid VMDs, but can't be fixed with the BlastCheat implementation in this branch. We stick with an MDP as we need to pass the domain along.
				string targetDomain = RTC_MemoryDomains.GetRealDomain(Domain, Address);
				long targetAddress = RTC_MemoryDomains.GetRealAddress(Domain, Address);

				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(targetDomain, targetAddress);

				var settings = new RamSearchEngine.Settings(RTC_MemoryDomains.MDRI.MemoryDomains);

				if (mdp == null)
					return true;


				string cheatName = "RTC Cheat|" + targetDomain + "|" + (targetAddress).ToString() + "|" + DisplayType.ToString() + "|" + BigEndian.ToString() + "|" + String.Join(",", Value.Select(it => it.ToString())) + "|" + IsEnabled.ToString() + "|" + IsFreeze.ToString();


				//VERY IMPORTANT MESSAGE FOR ENDIANESS
				//Endianess used to be borked in cheats for bizhawk 2.2
				//So we handle it ourselves and all cheats become little endian
				
				//I don't think this is true any more 6/4/2018.

				long _value = 0;
				if (IsFreeze)
				{
					byte[] freezeValue = new byte[Value.Length];

					MemoryDomainProxy targetMdp = RTC_MemoryDomains.GetProxy(targetDomain, targetAddress);

					for (int i = 0; i < Value.Length; i++)
					{
						freezeValue[i] = targetMdp.PeekByte(targetAddress + i);
					}

					_value = Convert.ToInt64(RTC_Extensions.GetDecimalValue(freezeValue, BigEndian));
				}
				else
				{
					_value = Convert.ToInt64(RTC_Extensions.GetDecimalValue(Value, BigEndian));
				}

				Watch somewatch = Watch.GenerateWatch(mdp.md, targetAddress, (WatchSize)Value.Length, DisplayType, BigEndian, cheatName, _value, 0, 0);
				Cheat ch = new Cheat(somewatch, unchecked((int)_value), null, true);
				Global.CheatList.Add(ch);
			}
			catch (Exception ex)
			{
				throw new Exception("The BlastCheat apply() function threw up. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
				ex.ToString());
			}

			return true;
		}

		public override BlastUnit GetBackup()
		{
			if (!IsEnabled)
				return null;

			try
			{
				//This technically doesn't work properly, but can't be fixed with the BlastCheat implementation in this branch
				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(Domain, Address);
				long targetAddress = RTC_MemoryDomains.GetRealAddress(Domain, Address);

				if (mdp == null)
					return null;

				byte[] _value = new byte[Value.Length];

				for (int i = 0; i < _value.Length; i++)
					_value[i] = mdp.PeekByte(targetAddress + i);

				return new BlastByte(Domain, Address, BlastByteType.SET, _value, BigEndian, true);
			}
			catch (Exception ex)
			{
				throw new Exception("The BlastCheat GetBackup() function threw up. \n" +
				                    "This is an RTC error, so you should probably send this to the RTC devs.\n" +
				                    "If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
				                    ex.ToString());
			}
		}

		public override void Reroll()
		{
			//No reason to re-roll freeze
			if (!IsFreeze)
			{
				long randomValue = 0;
				switch (Value.Length)
				{
					case (1):
						randomValue = RTC_Core.RND.RandomLong(RTC_HellgenieEngine.MinValue8Bit, RTC_HellgenieEngine.MaxValue8Bit);
						break;
					case (2):
						randomValue = RTC_Core.RND.RandomLong(RTC_HellgenieEngine.MinValue16Bit, RTC_HellgenieEngine.MaxValue16Bit);
						break;
					case (4):
						randomValue = RTC_Core.RND.RandomLong(RTC_HellgenieEngine.MinValue32Bit, RTC_HellgenieEngine.MaxValue32Bit);
						break;
				}
				Value = RTC_Extensions.GetByteArrayValue(Value.Length, randomValue, true);
			}
		}

		public override string ToString()
		{
			string EnabledString = $"[ ] BlastCheat{(IsFreeze ? ":Freeze" : "")} -> ";
			if (IsEnabled)
				EnabledString = $"[x] BlastCheat{(IsFreeze ? ":Freeze" : "")} -> ";

			string cleanDomainName = Domain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"

			//RTC_TODO: Rewrite the toString method for this
			return (EnabledString + cleanDomainName + "(" + Convert.ToInt32(Address).ToString() + ")" + (IsFreeze ? "" : ".Value(" + RTC_Extensions.GetDecimalValue(Value, BigEndian).ToString() + ")"));
		}
	}

	[Serializable]
	public class ActiveTableObject
	{
		public long[] data;

		public ActiveTableObject()
		{
		}

		public ActiveTableObject(long[] _data)
		{
			data = _data;
		}
	}


	[Serializable]
	public class BlastGeneratorProto
	{
		public string BlastType { get; }
		public string Domain { get; }
		public int Precision { get; }
		public long StepSize { get; }
		public long StartAddress { get; }
		public long EndAddress { get; }
		public long Param1 { get; }
		public long Param2 { get; }
		public string Mode { get; }
		public string Note { get; set; }
		public int Seed { get; }
		public BlastLayer bl;

		public BlastGeneratorProto()
		{
		}

		public BlastGeneratorProto(string _note, string _blastType, string _domain, string _mode, int _precision, long _stepSize, long _startAddress, long _endAddress, long _param1, long _param2, int _seed)
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
			Seed = _seed;
		}

		public BlastLayer GenerateBlastLayer()
		{
			switch (BlastType)
			{
				case "BlastByte":
					bl = RTC_BlastByteGenerator.GenerateLayer(Note, Domain, StepSize, StartAddress, EndAddress, Param1, Param2, Precision, Seed, (BGBlastByteModes)Enum.Parse(typeof(BGBlastByteModes), Mode, true));
					break;
				case "BlastCheat":
					bl = RTC_BlastCheatGenerator.GenerateLayer(Note, Domain, StepSize, StartAddress, EndAddress, Param1, Param2, Precision, Seed, (BGBlastCheatModes)Enum.Parse(typeof(BGBlastCheatModes), Mode, true));
					break;
				case "BlastPipe":
					bl = RTC_BlastPipeGenerator.GenerateLayer(Note, Domain, StepSize, StartAddress, EndAddress, Param1, Param2, Precision, Seed, (BGBlastPipeModes)Enum.Parse(typeof(BGBlastPipeModes), Mode, true));
					break;
				default:
					return null;
			}

			return bl;
		}

	}


	public class ProblematicProcess
	{
		public string Name;
		public string Message;
	}
}
