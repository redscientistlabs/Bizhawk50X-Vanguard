using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace RTC
{
	public partial class RTC_VmdPool_Form : Form
	{
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

				foreach (BlastPipe bp in RTC_PipeEngine.AllBlastPipes)
					bp.Rasterize();

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
			if (lbLoadedVmdList.SelectedIndex == -1)
				return;

			string VmdName = lbLoadedVmdList.SelectedItem.ToString();
			MemoryInterface mi = RTC_MemoryDomains.VmdPool[VmdName];

			lbRealDomainValue.Text = (mi as VirtualMemoryDomain).PointerDomains[0];
			lbVmdSizeValue.Text = mi.Size.ToString() + "(0x" + mi.Size.ToString("X") + ")";
		}

		private void btnSaveVmd_Click(object sender, EventArgs e)
		{
			if (lbLoadedVmdList.SelectedIndex == -1)
				return;

			string VmdName = lbLoadedVmdList.SelectedItem.ToString();
			VirtualMemoryDomain VMD = (VirtualMemoryDomain)RTC_MemoryDomains.VmdPool[VmdName];

			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.DefaultExt = "xml";
			saveFileDialog1.Title = "Save VMD to File";
			saveFileDialog1.Filter = "XML VMD file|*.xml";
			saveFileDialog1.RestoreDirectory = true;

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				string Filename = saveFileDialog1.FileName;

				//creater stockpile.xml to temp folder from stockpile object
				using (FileStream FS = File.Open(Filename, FileMode.OpenOrCreate))
				{
					XmlSerializer xs = new XmlSerializer(typeof(VmdPrototype));
					xs.Serialize(FS, VMD.proto);
					FS.Close();
				}
			}
			else
				return;
		}

		private void btnLoadVmd_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.DefaultExt = "xml";
			ofd.Multiselect = true;
			ofd.Title = "Open VMD File";
			ofd.Filter = "VMD xml files|*.xml";
			ofd.RestoreDirectory = true;
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				//string Filename = ofd.FileName.ToString();
				foreach (string Filename in ofd.FileNames)
				{
					try
					{
						using (FileStream FS = File.Open(Filename, FileMode.OpenOrCreate))
						{
							XmlSerializer xs = new XmlSerializer(typeof(VmdPrototype));
							var proto = (VmdPrototype)xs.Deserialize(FS);
							FS.Close();

							RTC_MemoryDomains.AddVMD(proto);
						}
					}
					catch
					{
						MessageBox.Show($"The VMD xml file {Filename} could not be loaded.");
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

			string VmdName = lbLoadedVmdList.SelectedItem.ToString();

			RTC_MemoryDomains.RenameVMD(VmdName);

			RefreshVMDs();
		}
	}
}
