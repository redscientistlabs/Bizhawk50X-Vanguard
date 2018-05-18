using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Emulation.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO.Compression;
using RTC;

namespace RTC
{
	public static class RTC_BlastTools
	{

		public static bool SaveBlastLayerToFile(BlastLayer bl)
		{
			string filename;

			if (bl.Layer.Count == 0)
			{
				MessageBox.Show("Can't save because the provided blastlayer is empty is empty");
				return false;
			}

			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.DefaultExt = "bl";
			saveFileDialog1.Title = "Save BlastLayer File";
			saveFileDialog1.Filter = "bl files|*.bl";
			saveFileDialog1.RestoreDirectory = true;

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				filename = saveFileDialog1.FileName;
			}
			else
				return false;

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


		public static BlastUnit ConvertBlastUnit(this BlastUnit bu, Type destinationType)
		{
			if(bu is BlastByte)
			{
				BlastByte bb = bu as BlastByte;
				if (destinationType == typeof(BlastByte))
				{
					return bb;
				}
				if (destinationType == typeof(BlastCheat))
				{
					return new BlastCheat(bb.Domain, bb.Address, BizHawk.Client.Common.DisplayType.Unsigned, bb.BigEndian, bb.Value, bb.IsEnabled, false);
				}
				else if (destinationType == typeof(BlastPipe))
				{
					//Pipe to 0
					return new BlastPipe(bb.Domain, bb.Address, bb.Domain, 0, 0, bb.Value.Length, bb.BigEndian, bb.IsEnabled);
				}
				else if (destinationType == typeof(BlastVector))
				{
					MessageBox.Show("BlastVector is depricated.");
					return null;
				}
			}
			else if (bu is BlastCheat)
			{
				BlastCheat bc = bu as BlastCheat;
				if (destinationType == typeof(BlastCheat))
				{
					return bc;
				}
				if (destinationType == typeof(BlastByte))
				{
					return new BlastByte(bc.Domain, bc.Address, BlastByteType.SET, bc.Value, bc.BigEndian, bc.IsEnabled);
				}
				else if (destinationType == typeof(BlastPipe))
				{
					return new BlastPipe(bc.Domain, bc.Address, bc.Domain, 0, 0, bc.Value.Length, bc.BigEndian, bc.IsEnabled);
				}
				else if (destinationType == typeof(BlastVector))
				{
					MessageBox.Show("BlastVector is depricated.");
					return null;
				}
			}
			else if (bu is BlastPipe)
			{
				BlastPipe bp = bu as BlastPipe;
				if (destinationType == typeof(BlastPipe))
				{
					return bp;
				}
				else if (destinationType == typeof(BlastByte))
				{
					return new BlastByte(bp.PipeDomain, bp.PipeAddress, BlastByteType.SET, getZeroedByteArray(bp.PipeSize), bp.BigEndian, bp.IsEnabled);
				}
				else if (destinationType == typeof(BlastCheat))
				{
					return new BlastCheat(bp.PipeDomain, bp.PipeAddress, BizHawk.Client.Common.DisplayType.Unsigned, bp.BigEndian, getZeroedByteArray(bp.PipeSize), bp.IsEnabled, false);
				}
				else if (destinationType == typeof(BlastVector))
				{
					MessageBox.Show("BlastVector is depricated.");
					return null;
				}
			}
			else if (bu is BlastVector)
			{
				BlastVector bv = bu as BlastVector;
				if (destinationType == typeof(BlastVector))
				{
					return bv;
				}
				else if (destinationType == typeof(BlastByte))
				{
					//BlastVector is depricated. If converting into a blastbyte, convert it to a regular blastbyte not a blastbyte in vector mode. It can't be re-rolled but it's cleaner
					//True to the endianess as BlastVector was always big endian
					return new BlastByte(bv.Domain, bv.Address, BlastByteType.SET, bv.Values, true, bv.IsEnabled);
				}
				else if (destinationType == typeof(BlastCheat))
				{
					return new BlastCheat(bv.Domain, bv.Address, BizHawk.Client.Common.DisplayType.Unsigned, true, bv.Values, bv.IsEnabled, false);
				}
				else if (destinationType == typeof(BlastPipe))
				{
					return new BlastPipe(bv.Domain, bv.Address, bv.Domain, 0, 0, bv.Values.Length, true, bv.IsEnabled);
				}
			}

			return null;
		}
		
		private static byte[] getZeroedByteArray(int size)
		{
			var temp = new byte[size];
			for (int i = 0; i < temp.Length; i++)
			{
				temp[i] = 0x0;
			}
			return temp;
		}
	}
}
