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
/*
BlastLayer
    Load from File (.bl)
    Save to File (.bl)
    Import BlastLayer (.bl)
    Export to CSV
*/

namespace RTC
{
	public static class RTC_BlastLayerTools
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

	}
}
