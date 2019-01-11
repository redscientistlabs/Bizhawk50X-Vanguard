using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using RTCV.CorruptCore;

namespace RTCV.UI
{
	public partial class RTC_VmdPool_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);
		
		public RTC_VmdPool_Form()
		{
			InitializeComponent();
		}

		private void btnUnloadVMD_Click(object sender, EventArgs e)
		{
			if (lbLoadedVmdList.SelectedIndex == -1)
				return;

			foreach (var item in lbLoadedVmdList.SelectedItems)
			{
				string VmdName = item.ToString();

				foreach (BlastUnit bu in RTC_StepActions.GetRawBlastLayer().Layer)
				{
					bu.RasterizeSourceAddress();
				}

				RTC_MemoryDomains.RemoveVMD(VmdName);
			}
			RefreshVMDs();
		}

		public void RefreshVMDs()
		{
			lbLoadedVmdList.Items.Clear();
			lbLoadedVmdList.Items.AddRange(RTC_MemoryDomains.VmdPool.Values.Select(it => it.ToString()).ToArray());

			lbRealDomainValue.Text = "#####";
			lbVmdSizeValue.Text = "#####";
		}

		private void RTC_VmdPool_Form_Load(object sender, EventArgs e)
		{
		}

		private void lbLoadedVmdList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lbLoadedVmdList.SelectedItem == null)
				return;

			string vmdName = lbLoadedVmdList.SelectedItem.ToString();
			MemoryInterface mi = RTC_MemoryDomains.VmdPool[vmdName];

			lbVmdSizeValue.Text = mi.Size.ToString() + " (0x" + mi.Size.ToString("X") + ")";

			if ((mi as VirtualMemoryDomain)?.PointerDomains.Distinct().Count() > 1)
				lbRealDomainValue.Text = "Hybrid";
			else
				lbRealDomainValue.Text = (mi as VirtualMemoryDomain)?.PointerDomains[0];
		}

		private void btnSaveVmd_Click(object sender, EventArgs e)
		{
			if (lbLoadedVmdList.SelectedIndex == -1)
				return;

			string vmdName = lbLoadedVmdList.SelectedItem.ToString();
			VirtualMemoryDomain vmd = (VirtualMemoryDomain)RTC_MemoryDomains.VmdPool[vmdName];

			SaveFileDialog saveFileDialog1 = new SaveFileDialog
			{
				DefaultExt = "xml",
				Title = "Save VMD to File",
				Filter = "XML VMD file|*.xml",
				RestoreDirectory = true
			};

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				string filename = saveFileDialog1.FileName;

				//creater stockpile.xml to temp folder from stockpile object
				using (FileStream fs = File.Open(filename, FileMode.OpenOrCreate))
				{
					XmlSerializer xs = new XmlSerializer(typeof(VmdPrototype));
					xs.Serialize(fs, vmd.Proto);
					fs.Close();
				}
			}
			else
				return;
		}

		private void btnLoadVmd_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog
			{
				DefaultExt = "xml",
				Multiselect = true,
				Title = "Open VMD File",
				Filter = "VMD xml files|*.xml",
				RestoreDirectory = true
			};
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				//string Filename = ofd.FileName.ToString();
				foreach (string filename in ofd.FileNames)
				{
					try
					{
						using (FileStream fs = File.Open(filename, FileMode.OpenOrCreate))
						{
							XmlSerializer xs = new XmlSerializer(typeof(VmdPrototype));
							VmdPrototype proto = (VmdPrototype)xs.Deserialize(fs);
							fs.Close();

							RTC_MemoryDomains.AddVMD(proto);
						}
					}
					catch
					{
						MessageBox.Show($"The VMD xml file {filename} could not be loaded.");
					}
				}

				RefreshVMDs();
			}
			else
				return;
		}

		private void btnRenameVMD_Click(object sender, EventArgs e)
		{
			if (lbLoadedVmdList.SelectedIndex == -1)
				return;

			string vmdName = lbLoadedVmdList.SelectedItem.ToString();

			RTC_MemoryDomains.RenameVMD(vmdName);

			RefreshVMDs();
		}

	}
}
