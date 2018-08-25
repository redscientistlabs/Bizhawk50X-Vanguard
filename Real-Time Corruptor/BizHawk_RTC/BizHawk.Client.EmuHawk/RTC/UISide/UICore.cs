using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_UICore
	{

		public static void SetRTCHexadecimal(bool useHex, Form form = null)
		{
			//Sets the interface to use Hex across the board

			List<Control> allControls = new List<Control>();

			if (form == null)
			{
				foreach (Form targetForm in RTC_Core.allRtcForms)
				{
					if (targetForm != null)
					{
						allControls.AddRange(targetForm.Controls.getControlsWithTag());
						allControls.Add(targetForm);
					}
				}
			}
			else
			{
				allControls.AddRange(form.Controls.getControlsWithTag());
				allControls.Add(form);
			}

			var hexadecimal = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("hex"));

			foreach (NumericUpDownHexFix updown in hexadecimal)
				updown.Hexadecimal = true;

			foreach (DataGridView dgv in hexadecimal)
				foreach (DataGridViewColumn column in dgv.Columns)
				{
					if (column.CellType.Name == "DataGridViewNumericUpDownCell")
					{
						DataGridViewNumericUpDownColumn _column = column as DataGridViewNumericUpDownColumn;
						_column.Hexadecimal = useHex;
					}
				}
		}


		public static void SetRTCColor(Color color, Form form = null)
		{
			//Recolors all the RTC Forms using the general skin color

			List<Control> allControls = new List<Control>();

			if (form == null)
			{
				foreach (Form targetForm in RTC_Core.allRtcForms)
				{
					if (targetForm != null)
					{
						allControls.AddRange(targetForm.Controls.getControlsWithTag());
						allControls.Add(targetForm);
					}
				}
			}
			else
			{
				allControls.AddRange(form.Controls.getControlsWithTag());
				allControls.Add(form);
			}

			var lightColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:light"));
			var normalColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:normal"));
			var darkColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:dark"));
			var darkerColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:darker"));
			var darkererColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:darkerer"));

			foreach (Control c in lightColorControls)
				if (c is Label)
					c.ForeColor = color.ChangeColorBrightness(0.30f);
				else
					c.BackColor = color.ChangeColorBrightness(0.30f);

			foreach (Control c in normalColorControls)
				if (c is Label)
					c.ForeColor = color;
				else
					c.BackColor = color;

			S.GET<RTC_StockpilePlayer_Form>().dgvStockpile.BackgroundColor = color;
			S.GET<RTC_GlitchHarvester_Form>().dgvStockpile.BackgroundColor = color;


			//TODO
			//beForm.dgvBlastLayer.BackgroundColor = color;

			S.GET<RTC_BlastGenerator_Form>().dgvBlastGenerator.BackgroundColor = color;

			foreach (Control c in darkColorControls)
				if (c is Label)
					c.ForeColor = color.ChangeColorBrightness(-0.30f);
				else
					c.BackColor = color.ChangeColorBrightness(-0.30f);

			foreach (Control c in darkerColorControls)
				if (c is Label)
					c.ForeColor = color.ChangeColorBrightness(-0.75f);
				else
					c.BackColor = color.ChangeColorBrightness(-0.75f);

			foreach (Control c in darkererColorControls)
				if (c is Label)
					c.ForeColor = color.ChangeColorBrightness(-0.825f);
				else
					c.BackColor = color.ChangeColorBrightness(-0.825f);
		}

		public static void SelectRTCColor()
		{
			// Show the color dialog.
			Color color;
			ColorDialog cd = new ColorDialog();
			DialogResult result = cd.ShowDialog();
			// See if user pressed ok.
			if (result == DialogResult.OK)
			{
				// Set form background to the selected color.
				color = cd.Color;
			}
			else
				return;

			SetRTCColor(color);

			RTC_Params.SaveRTCColor(color);
		}


		public static void SetEngineByName(string name)
		{
			//Selects an engine from a given string name

			for (int i = 0; i < S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.Items.Count; i++)
				if (S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.Items[i].ToString() == name)
				{
					S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.SelectedIndex = i;
					break;
				}
		}


		public static void LoadRom(string RomFile, bool sync = false)
		{
			// Safe method for loading a Rom file from any process

			NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_LOADROM)
			{
				romFilename = RomFile
			}, sync);
		}

	}
}
