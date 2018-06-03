using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace RTC
{
	public partial class RTC_BlastGenerator_Form : Form
	{
		private StashKey sk = null;


		public RTC_BlastGenerator_Form()
		{
			InitializeComponent();
		}

		private void RTC_BlastGeneratorForm_Load(object sender, EventArgs e)
		{
			//Set up the UI
			Controls.Remove(gbBlastByteGenerator);
			pnEngineMode.Controls.Add(gbBlastByteGenerator);
			gbBlastByteGenerator.Location = new Point(0, 0);

			Controls.Remove(gbBlastCheatGenerator);
			pnEngineMode.Controls.Add(gbBlastCheatGenerator);
			gbBlastCheatGenerator.Location = new Point(0, 0);

			Controls.Remove(gbBlastPipeGenerator);
			pnEngineMode.Controls.Add(gbBlastPipeGenerator);
			gbBlastPipeGenerator.Location = new Point(0, 0);

			cbBlastByteModes.SelectedIndex = 0;
			cbBlastCheatModes.SelectedIndex = 0;
			cbBlastPipeMode.SelectedIndex = 0;

			cbBlastUnitMode.SelectedIndex = 0;



			updownBlastByteValue.Hexadecimal = cbUseHex.Checked;
			updownBlastCheatValue.Hexadecimal = cbUseHex.Checked;
			updownBlastPipeTilt.Hexadecimal = cbUseHex.Checked;
			updownStepSize.Hexadecimal = cbUseHex.Checked;
		}

		public void LoadStashkey(StashKey _sk)
		{
			if (_sk == null)
				return;

			sk = (StashKey)_sk.Clone();

			this.Show();
		}

		private void btnLoadDomains_Click(object sender, EventArgs e)
		{
			RTC_Core.ecForm.RefreshDomainsAndKeepSelected();

			cbSelectedMemoryDomain.Items.Clear();
			cbSelectedMemoryDomain.Items.AddRange(RTC_MemoryDomains.MemoryInterfaces.Keys.ToArray());
			cbSelectedMemoryDomain.SelectedIndex = 0;

		}


		private void btnJustCorrupt_Click(object sender, EventArgs e)
		{

		}

		private void btnSendBlastLayerToEditor_Click(object sender, EventArgs e)
		{

		}

		private void btnLoadCorrupt_Click(object sender, EventArgs e)
		{

		}


		private int SafeStringToInt(string input)
		{
			if (input.ToUpper().Contains("0X"))
				return int.Parse(input.Substring(2), NumberStyles.HexNumber);
			else
				return Convert.ToInt32(input);
		}
		private void cbSelectedMemoryDomain_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(cbSelectedMemoryDomain.SelectedItem?.ToString()) || !RTC_MemoryDomains.MemoryInterfaces.ContainsKey(cbSelectedMemoryDomain.SelectedItem.ToString()))
			{
				cbSelectedMemoryDomain.Items.Clear();
				return;
			}

			MemoryInterface mi = RTC_MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];

			lbDomainSizeValue.Text = mi.Size.ToString();
			lbWordSizeValue.Text = $"{mi.WordSize * 8} bits";
			lbEndianTypeValue.Text = (mi.BigEndian ? "Big" : "Little");

			//currentDomainSize = Convert.ToInt64(mi.Size);
		}

		private void cbBlastByteModes_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void cbBlastUnitMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			gbBlastByteGenerator.Visible = false;
			gbBlastCheatGenerator.Visible = false;
			gbBlastPipeGenerator.Visible = false;

			switch (cbBlastUnitMode.SelectedItem.ToString())
			{
				case "BlastByte":
					gbBlastByteGenerator.Visible = true;
					break;
				case "BlastCheat":
					gbBlastCheatGenerator.Visible = true;
					break;
				case "BlastPipe":
					gbBlastPipeGenerator.Visible = true;
					break;

				default:
					break;
			}
		}

		private void cbUseHex_CheckedChanged(object sender, EventArgs e)
		{
			updownBlastByteValue.Hexadecimal = cbUseHex.Checked;
			updownBlastCheatValue.Hexadecimal = cbUseHex.Checked;
			updownBlastPipeTilt.Hexadecimal = cbUseHex.Checked;
			updownStepSize.Hexadecimal = cbUseHex.Checked;
		}

		private void label5_Click(object sender, EventArgs e)
		{

		}

		private void btnHelp_Click(object sender, EventArgs e)
		{

		}
	}
}
