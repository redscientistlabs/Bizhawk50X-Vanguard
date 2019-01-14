using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using CorruptCore;
using RTCV.NetCore;


namespace RTCV.CorruptCore
{
	public static class RTC_BlastTools
	{
		public static string LastBlastLayerSavePath { get; set; }

		public static bool SaveBlastLayerToFile(BlastLayer bl, string path = null)
		{
			string filename = path;

			if (bl.Layer.Count == 0)
			{
				MessageBox.Show("Can't save because the provided blastlayer is empty is empty");
				return false;
			}

			if (filename == null)
			{
				SaveFileDialog saveFileDialog1 = new SaveFileDialog
				{
					DefaultExt = "bl",
					Title = "Save BlastLayer File",
					Filter = "bl files|*.bl",
					RestoreDirectory = true
				};

				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
				{
					filename = saveFileDialog1.FileName;
				}
				else
					return false;
			}

			XmlSerializer xs = new XmlSerializer(typeof(BlastLayer));

			using (FileStream fs = new FileStream(filename, FileMode.Create))
			{
				xs.Serialize(fs, bl);
			}

			LastBlastLayerSavePath = filename;
			return true;
		}

		public static BlastLayer LoadBlastLayerFromFile(string filename = null)
		{
			BlastLayer bl = null;

			if (filename == null)
			{
				OpenFileDialog ofd = new OpenFileDialog
				{
					DefaultExt = "bl",
					Title = "Open BlastLayer File",
					Filter = "bl files|*.bl",
					RestoreDirectory = true
				};
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					filename = ofd.FileName.ToString();
				}
				else
					return null;
			}

			if (!File.Exists(filename))
			{
				MessageBox.Show("The BlastLayer file wasn't found");
				return null;
			}


			XmlSerializer xs = new XmlSerializer(typeof(BlastLayer));

			try
			{
				using (FileStream fs = new FileStream(filename, FileMode.Open))
				{
					bl = (BlastLayer)xs.Deserialize(fs);
					return bl;
				}
			}
			catch
			{
				MessageBox.Show("The BlastLayer file could not be loaded");
				return null;
			}
		}

		private static byte[] CapBlastUnit(byte[] input)
		{
			switch (input.Length)
			{
				case 1:
					if (RTC_Extensions.GetDecimalValue(input, false) > Byte.MaxValue)
						return getByteArray(1, 0xFF);
					break;
				case 2:
					if (RTC_Extensions.GetDecimalValue(input, false) > UInt16.MaxValue)
						return getByteArray(2, 0xFF);
					break;
				case 4:
					if (RTC_Extensions.GetDecimalValue(input, false) > UInt32.MaxValue)
						return getByteArray(2, 0xFF);
					break;
			}
			return input;
		}

		public static BlastUnit ConvertBlastUnit(this BlastUnit bu, Type destinationType)
		{
			//TODO
			/*
			try
			{
				if (bu is BlastByte bb)
				{
					if (destinationType == typeof(BlastByte))
					{
						return new BlastByte(bb.Domain, bb.Address, BlastUnitSource.SET, CapBlastUnit(bb.Value), bb.BigEndian, bb.IsEnabled, bb.Note);
					}
					if (destinationType == typeof(BlastCheat))
					{
						return new BlastCheat(bb.Domain, bb.Address, bb.BigEndian, CapBlastUnit(bb.Value), bb.IsEnabled, false, bb.Note);
					}
					else if (destinationType == typeof(BlastPipe))
					{
						//Pipe to 0
						return new BlastPipe(bb.Domain, bb.Address, bb.Domain, 0, 0, bb.Value.Length, bb.BigEndian, bb.IsEnabled, bb.Note);
					}
				}
				else if (bu is BlastCheat bc)
				{
					if (destinationType == typeof(BlastCheat))
					{
						return new BlastCheat(bc.Domain, bc.Address, bc.BigEndian, CapBlastUnit(bc.Value), bc.IsEnabled, false, bc.Note);
					}
					if (destinationType == typeof(BlastByte))
					{
						return new BlastByte(bc.Domain, bc.Address, BlastUnitSource.SET, CapBlastUnit(bc.Value), bc.BigEndian, bc.IsEnabled, bc.Note);
					}
					else if (destinationType == typeof(BlastPipe))
					{
						return new BlastPipe(bc.Domain, bc.Address, bc.Domain, 0, 0, bc.Value.Length, bc.BigEndian, bc.IsEnabled, bc.Note);
					}
				}
				else if (bu is BlastPipe bp)
				{
					if (destinationType == typeof(BlastPipe))
					{
						return new BlastPipe(bp.Domain, bp.Address, bp.PipeDomain, bp.PipeAddress, 0, bp.PipeSize, bp.BigEndian, bp.IsEnabled, bp.Note);
					}
					else if (destinationType == typeof(BlastByte))
					{
						return new BlastByte(bp.PipeDomain, bp.PipeAddress, BlastUnitSource.SET, getByteArray(bp.PipeSize, 0x0), bp.BigEndian, bp.IsEnabled, bp.Note);
					}
					else if (destinationType == typeof(BlastCheat))
					{
						return new BlastCheat(bp.PipeDomain, bp.PipeAddress, bp.BigEndian, getByteArray(bp.PipeSize, 0x0), bp.IsEnabled, false, bp.Note);
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			*/
			return null;
		}

		private static byte[] getByteArray(int size, byte value)
		{
			var temp = new byte[size];
			for (int i = 0; i < temp.Length; i++)
			{
				temp[i] = value;
			}
			return temp;
		}

		public static BlastLayer GetAppliedBackupLayer(BlastLayer bl, StashKey sk)
		{
			BlastLayer newBlastLayer = new BlastLayer();
			//So basically due to how netcore handles synced commands, we can't actually call sk.Run()
			//from within emuhawk or else it'll apply the blastlayer AFTER this code completes
			//So we manually apply the blastlayer
			sk.RunOriginal();
			sk.BlastLayer.Apply();

			newBlastLayer = bl.GetBackup();
			return newBlastLayer;
		}

		public static BlastLayer BakeBlastUnitsToSet(StashKey sk, BlastLayer inputLayer)
		{
			BlastLayer newLayer = RTC_BlastTools.GetAppliedBackupLayer(inputLayer, sk);
			return newLayer;
		}


		public static BlastLayer GetBlastLayerFromDiff(byte[] Original, byte[] Corrupt)
		{
			BlastLayer bl = new BlastLayer();

			string thisSystem = (string)RTC_Corruptcore.VanguardSpec[VSPEC.SYSTEM.ToString()];
			string romFilename = (string)RTC_Corruptcore.VanguardSpec[VSPEC.OPENROMFILENAME.ToString()];

			var rp = RTC_MemoryDomains.GetRomParts(thisSystem, romFilename);

			if (rp.Error != null)
			{
				MessageBox.Show(rp.Error);
				return null;
			}

			if (Original.Length != Corrupt.Length)
			{
				MessageBox.Show("ERROR, ROM SIZE MISMATCH");
				return null;
			}

			MemoryInterface mi = RTC_MemoryDomains.GetInterface(rp.PrimaryDomain);
			long maxaddress = mi.Size;

			for (int i = 0; i < Original.Length; i++)
			{
				if (Original[i] != Corrupt[i] && i >= rp.SkipBytes)
				{
					if (i - rp.SkipBytes >= maxaddress)
						bl.Layer.Add(new BlastUnit(new byte[] { Corrupt[i] }, rp.SecondDomain, (i - rp.SkipBytes) - maxaddress, 1, mi.BigEndian));
					else
						bl.Layer.Add(new BlastUnit(new byte[] { Corrupt[i] }, rp.PrimaryDomain, (i - rp.SkipBytes) - maxaddress, 1, mi.BigEndian));
				}
			}

			if (bl.Layer.Count == 0)
				return null;
			return bl;
		}

		public static List<BlastGeneratorProto> GenerateBlastLayersFromBlastGeneratorProtos(List<BlastGeneratorProto> blastLayers, StashKey sk)
		{
			//Load the game first for stuff like REPLACE_X_WITH_Y
			sk?.RunOriginal();
			foreach (BlastGeneratorProto bgp in blastLayers)
			{
				//Only generate if there's no BlastLayer.
				//A new proto is always generated if the cell is dirty which means no BlastLayer will exist
				//Otherwise, we just return the existing BlastLayer
				if (bgp.Bl == null)
				{
					Console.Write("BGP was dirty. Generating BlastLayer\n");
					bgp.Bl = bgp.GenerateBlastLayer();
				}
					
			}

			return blastLayers;
		}
	}

	/*
	private bool GenerateBlastLayer()
	{
		if (string.IsNullOrWhiteSpace(cbSelectedMemoryDomain.SelectedItem?.ToString()) || !RTC_MemoryDomains.MemoryInterfaces.ContainsKey(cbSelectedMemoryDomain.SelectedItem.ToString()))
		{
			cbSelectedMemoryDomain.Items.Clear();
			return false;
		}

		MemoryInterface mi = RTC_MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];
		BlastLayer bl = new BlastLayer();

		foreach (string line in tbCustomAddresses.Lines)
		{
			if (string.IsNullOrWhiteSpace(line))
				continue;

			string trimmedLine = line.Trim();

			bool remove = false;

			if (trimmedLine[0] == '-')
			{
				remove = true;
				trimmedLine = trimmedLine.Substring(1);
			}

			string[] lineParts = trimmedLine.Split('-');

			if (lineParts.Length > 1)
			{
				int start = SafeStringToInt(lineParts[0]);
				int end = SafeStringToInt(lineParts[1]);

				if (end >= currentDomainSize)
					end = Convert.ToInt32(currentDomainSize - 1);

				if (remove)
					proto.removeRanges.Add(new int[] { start, end });
				else
					proto.addRanges.Add(new int[] { start, end });
			}
			else
			{
				int address = SafeStringToInt(lineParts[0]);

				if (address < currentDomainSize)
				{
					if (remove)
						proto.removeSingles.Add(address);
					else
						proto.addSingles.Add(address);
				}
			}
		}

		if (proto.addRanges.Count == 0 && proto.addSingles.Count == 0)
		{
			//No add range was specified, use entire domain
			proto.addRanges.Add(new int[] { 0, (currentDomainSize > int.MaxValue ? int.MaxValue : Convert.ToInt32(currentDomainSize)) });
		}
	}*/
}
