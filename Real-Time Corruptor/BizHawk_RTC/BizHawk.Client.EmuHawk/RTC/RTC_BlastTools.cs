using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;

namespace RTC
{
	public static class RTC_BlastTools
	{
		public static bool SaveBlastLayerToFile(BlastLayer bl, bool isQuickSave = false)
		{
			string filename;

			if (bl.Layer.Count == 0)
			{
				MessageBox.Show("Can't save because the provided blastlayer is empty is empty");
				return false;
			}

			if (!isQuickSave)
			{
				SaveFileDialog saveFileDialog1 = new SaveFileDialog();
				saveFileDialog1.DefaultExt = "bl";
				saveFileDialog1.Title = "Save BlastLayer File";
				saveFileDialog1.Filter = "bl files|*.bl";
				saveFileDialog1.RestoreDirectory = true;

				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
				{
					filename = saveFileDialog1.FileName;
					RTC_Core.beForm.CurrentBlastLayerFile = saveFileDialog1.FileName;
				}
				else
					return false;
			}
			else
				filename = RTC_Core.beForm.CurrentBlastLayerFile;

			XmlSerializer xs = new XmlSerializer(typeof(BlastLayer));

			using (FileStream fs = new FileStream(filename, FileMode.Create))
			{
				xs.Serialize(fs, bl);
			}
			return true;
		}

		public static BlastLayer LoadBlastLayerFromFile(string filename = null)
		{
			BlastLayer bl = null;

			if (filename == null)
			{
				OpenFileDialog ofd = new OpenFileDialog();
				ofd.DefaultExt = "bl";
				ofd.Title = "Open BlastLayer File";
				ofd.Filter = "bl files|*.bl";
				ofd.RestoreDirectory = true;
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

			var token = RTC_NetCore.HugeOperationStart();

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
				RTC_NetCore.HugeOperationEnd(token);
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
			try
			{
				if (bu is BlastByte bb)
				{
					if (destinationType == typeof(BlastByte))
					{
						return new BlastByte(bb.Domain, bb.Address, BlastByteType.SET, CapBlastUnit(bb.Value), bb.BigEndian, bb.IsEnabled, bb.Note);
					}
					if (destinationType == typeof(BlastCheat))
					{
						return new BlastCheat(bb.Domain, bb.Address, BizHawk.Client.Common.DisplayType.Unsigned, bb.BigEndian, CapBlastUnit(bb.Value), bb.IsEnabled, false, bb.Note);
					}
					else if (destinationType == typeof(BlastPipe))
					{
						//Pipe to 0
						return new BlastPipe(bb.Domain, bb.Address, bb.Domain, 0, 0, bb.Value.Length, bb.BigEndian, bb.IsEnabled, bb.Note);
					}
					else if (destinationType == typeof(BlastVector))
					{
						MessageBox.Show("BlastVector is depricated.");
						return null;
					}
				}
				else if (bu is BlastCheat bc)
				{
					if (destinationType == typeof(BlastCheat))
					{
						return new BlastCheat(bc.Domain, bc.Address, BizHawk.Client.Common.DisplayType.Unsigned, bc.BigEndian, CapBlastUnit(bc.Value), bc.IsEnabled, false, bc.Note);
					}
					if (destinationType == typeof(BlastByte))
					{
						return new BlastByte(bc.Domain, bc.Address, BlastByteType.SET, CapBlastUnit(bc.Value), bc.BigEndian, bc.IsEnabled, bc.Note);
					}
					else if (destinationType == typeof(BlastPipe))
					{
						return new BlastPipe(bc.Domain, bc.Address, bc.Domain, 0, 0, bc.Value.Length, bc.BigEndian, bc.IsEnabled, bc.Note);
					}
					else if (destinationType == typeof(BlastVector))
					{
						MessageBox.Show("BlastVector is depricated.");
						return null;
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
						return new BlastByte(bp.PipeDomain, bp.PipeAddress, BlastByteType.SET, getByteArray(bp.PipeSize, 0x0), bp.BigEndian, bp.IsEnabled, bp.Note);
					}
					else if (destinationType == typeof(BlastCheat))
					{
						return new BlastCheat(bp.PipeDomain, bp.PipeAddress, BizHawk.Client.Common.DisplayType.Unsigned, bp.BigEndian, getByteArray(bp.PipeSize, 0x0), bp.IsEnabled, false, bp.Note);
					}
					else if (destinationType == typeof(BlastVector))
					{
						MessageBox.Show("BlastVector is depricated.");
						return null;
					}
				}
				else if (bu is BlastVector bv)
				{
					if (destinationType == typeof(BlastVector))
					{
						MessageBox.Show("BlastVector is depricated.");
					}
					else if (destinationType == typeof(BlastByte))
					{
						return new BlastByte(bv.Domain, bv.Address, BlastByteType.VECTOR, CapBlastUnit(bv.Values), true, bv.IsEnabled, bv.Note);
					}
					else if (destinationType == typeof(BlastCheat))
					{
						return new BlastCheat(bv.Domain, bv.Address, BizHawk.Client.Common.DisplayType.Unsigned, true, CapBlastUnit(bv.Values), bv.IsEnabled, false, bv.Note);
					}
					else if (destinationType == typeof(BlastPipe))
					{
						return new BlastPipe(bv.Domain, bv.Address, bv.Domain, 0, 0, bv.Values.Length, true, bv.IsEnabled, bv.Note);
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}

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

		public static BlastLayer GetBlastByteBackupLayer(BlastLayer bl, StashKey sk)
		{
			BlastLayer newBlastLayer = new BlastLayer();
			sk.Run();

			foreach (BlastUnit bu in bl.Layer)
			{
				if (bu is BlastByte)
				{
					BlastByte bb = bu as BlastByte;
					newBlastLayer.Layer.Add(bb.GetBackup());
				}
			}
			return newBlastLayer;
		}

		public static BlastLayer BakeBlastBytesToSet(StashKey sk, BlastLayer inputLayer)
		{
			try
			{
				//Bake them
				var token = RTC_NetCore.HugeOperationStart();
				BlastLayer newLayer = (BlastLayer)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETBLASTBYTEBACKUPLAYER)
				{
					blastlayer =  inputLayer,
					stashkey = sk
				}, true);
				RTC_NetCore.HugeOperationEnd(token);

				return newLayer;
			}
			catch (Exception ex)
			{
				throw;
			}
			return null;
		}


		public static BlastLayer GetBlastLayerFromDiff(byte[] Original, byte[] Corrupt)
		{
			BlastLayer bl = new BlastLayer();

			string thisSystem = Global.Game.System;
			string romFilename = GlobalWin.MainForm.CurrentlyOpenRom;

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

			RTC.MemoryInterface mi = RTC_MemoryDomains.GetInterface(rp.PrimaryDomain);
			long maxaddress = mi.Size;

			for (int i = 0; i < Original.Length; i++)
			{
				if (Original[i] != Corrupt[i] && i >= rp.SkipBytes)
				{
					if (i - rp.SkipBytes >= maxaddress)
						bl.Layer.Add(new BlastByte(rp.SecondDomain, (i - rp.SkipBytes) - maxaddress, BlastByteType.SET, new byte[] { Corrupt[i] }, mi.BigEndian, true));
					else
						bl.Layer.Add(new BlastByte(rp.PrimaryDomain, i - rp.SkipBytes, BlastByteType.SET, new byte[] { Corrupt[i] }, mi.BigEndian, true));
				}
			}

			if (bl.Layer.Count == 0)
				return null;
			else
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
				if (bgp.bl == null)
				{
					Console.Write("BGP was dirty. Generating BlastLayer\n");
					bgp.bl = bgp.GenerateBlastLayer();
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
