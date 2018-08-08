using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_CorruptionEngine_Form : ComponentForm
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

		private int defaultPrecision = -1;
		private bool updatingMinMax = false;



		public RTC_CorruptionEngine_Form()
		{
			InitializeComponent();

			this.undockedSizable = false;
		}

		private void RTC_CorruptionEngine_Form_Load(object sender, EventArgs e)
		{

			gbNightmareEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
			gbHellgenieEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
			gbDistortionEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
			gbFreezeEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
			gbPipeEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
			gbVectorEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
			gbBlastGeneratorEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);


			cbSelectedEngine.SelectedIndex = 0;
			//cbVectorLimiterList.SelectedIndex = 0;
			//cbVectorValueList.SelectedIndex = 0;
			cbBlastType.SelectedIndex = 0;
			cbCustomPrecision.SelectedIndex = 0;


		}



		private void nmMaxCheats_ValueChanged(object sender, EventArgs e)
		{
			if (Convert.ToInt32(nmMaxCheats.Value) != RTC_StepActions.MaxInfiniteBlastUnits)
				RTC_StepActions.SetMaxLifetimeBlastUnits(Convert.ToInt32(nmMaxCheats.Value));

			if (nmMaxCheats.Value != nmMaxFreezes.Value)
				nmMaxFreezes.Value = nmMaxCheats.Value;
		}


		private void cbClearCheatsOnRewind_CheckedChanged(object sender, EventArgs e)
		{
			if (cbClearFreezesOnRewind.Checked != cbClearCheatsOnRewind.Checked)
				cbClearFreezesOnRewind.Checked = cbClearCheatsOnRewind.Checked;

			RTC_StepActions.ClearStepActionsOnRewind(true);
		}

		private void nmDistortionDelay_ValueChanged(object sender, EventArgs e)
		{
			RTC_DistortionEngine.Delay = Convert.ToInt32(nmDistortionDelay.Value);
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_DISTORTION_DELAY) { objectValue = RTC_DistortionEngine.Delay });
		}

		private void btnResyncDistortionEngine_Click(object sender, EventArgs e)
		{
			RTC_DistortionEngine.Resync();
		}

		public void UpdateDefaultPrecision()
		{
			defaultPrecision = RTC_MemoryDomains.MemoryInterfaces[RTC_MemoryDomains.MainDomain].WordSize;
			lbCoreDefault.Text = $"Core default: { defaultPrecision * 8}-bit";

			if (RTC_Core.CustomPrecision == -1)
				RTC_Core.CurrentPrecision = defaultPrecision;

			updateMinMaxBoxes(defaultPrecision);
		}





		private void cbSelectedEngine_SelectedIndexChanged(object sender, EventArgs e)
		{
			gbNightmareEngine.Visible = false;
			gbHellgenieEngine.Visible = false;
			gbDistortionEngine.Visible = false;
			gbFreezeEngine.Visible = false;
			gbPipeEngine.Visible = false;
			gbVectorEngine.Visible = false;

			pnCustomPrecision.Visible = false;
			

			RTC_Core.coreForm.btnAutoCorrupt.Visible = true;
			RTC_Core.ghForm.pnIntensity.Visible = true;
			RTC_Core.ecForm.pnGeneralParameters.Visible = true;
			RTC_Core.ecForm.pnMemoryDomains.Visible = true;

			switch (cbSelectedEngine.SelectedItem.ToString())
			{
				case "Nightmare Engine":
					RTC_Core.SelectedEngine = CorruptionEngine.NIGHTMARE;
					gbNightmareEngine.Visible = true;
					pnCustomPrecision.Visible = true;
					break;

				case "Hellgenie Engine":
					RTC_Core.SelectedEngine = CorruptionEngine.HELLGENIE;
					gbHellgenieEngine.Visible = true;
					pnCustomPrecision.Visible = true;
					break;

				case "Distortion Engine":
					RTC_Core.SelectedEngine = CorruptionEngine.DISTORTION;
					gbDistortionEngine.Visible = true;
					pnCustomPrecision.Visible = true;
					break;

				case "Freeze Engine":
					RTC_Core.SelectedEngine = CorruptionEngine.FREEZE;
					gbFreezeEngine.Visible = true;
					pnCustomPrecision.Visible = true;
					break;

				case "Pipe Engine":
					RTC_Core.SelectedEngine = CorruptionEngine.PIPE;
					gbPipeEngine.Visible = true;
					pnCustomPrecision.Visible = true;
					break;

				case "Vector Engine":
					RTC_Core.SelectedEngine = CorruptionEngine.VECTOR;
					gbVectorEngine.Visible = true;
					break;

				case "Blast Generator":
					RTC_Core.SelectedEngine = CorruptionEngine.BLASTGENERATORENGINE;
					gbBlastGeneratorEngine.Visible = true;

					RTC_Core.coreForm.AutoCorrupt = false;
					RTC_Core.coreForm.btnAutoCorrupt.Visible = false;
					RTC_Core.ecForm.pnGeneralParameters.Visible = false;
					RTC_Core.ecForm.pnMemoryDomains.Visible = false;

					RTC_Core.ghForm.pnIntensity.Visible = false;
					break;

				default:
					break;

			}

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_ENGINE) { objectValue = RTC_Core.SelectedEngine });

			if (cbSelectedEngine.SelectedItem.ToString() == "Blast Generator")
			{
				RTC_Core.gpForm.labelBlastRadius.Visible = false;
				RTC_Core.gpForm.labelIntensity.Visible = false;
				RTC_Core.gpForm.labelIntensityTimes.Visible = false;
				RTC_Core.gpForm.labelErrorDelay.Visible = false;
				RTC_Core.gpForm.labelErrorDelaySteps.Visible = false;
				RTC_Core.gpForm.nmErrorDelay.Visible = false;
				RTC_Core.gpForm.nmIntensity.Visible = false;
				RTC_Core.gpForm.track_ErrorDelay.Visible = false;
				RTC_Core.gpForm.track_Intensity.Visible = false;
				RTC_Core.gpForm.cbBlastRadius.Visible = false;
			}
			else
			{
				RTC_Core.gpForm.labelBlastRadius.Visible = true;
				RTC_Core.gpForm.labelIntensity.Visible = true;
				RTC_Core.gpForm.labelIntensityTimes.Visible = true;
				RTC_Core.gpForm.labelErrorDelay.Visible = true;
				RTC_Core.gpForm.labelErrorDelaySteps.Visible = true;
				RTC_Core.gpForm.nmErrorDelay.Visible = true;
				RTC_Core.gpForm.nmIntensity.Visible = true;
				RTC_Core.gpForm.track_ErrorDelay.Visible = true;
				RTC_Core.gpForm.track_Intensity.Visible = true;
				RTC_Core.gpForm.cbBlastRadius.Visible = true;
			}

			cbSelectedEngine.BringToFront();
			pnCustomPrecision.BringToFront();

			RTC_StepActions.ClearStepBlastUnits();
		}

		private void cbClearFreezesOnRewind_CheckedChanged(object sender, EventArgs e)
		{
			if (cbClearFreezesOnRewind.Checked != cbClearCheatsOnRewind.Checked)
				cbClearCheatsOnRewind.Checked = cbClearFreezesOnRewind.Checked;

			RTC_Core.ClearStepActionsOnRewind = cbClearFreezesOnRewind.Checked;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_STEPACTIONS_CLEARREWIND) { objectValue = RTC_Core.ClearStepActionsOnRewind });
		}

		private void nmMaxFreezes_ValueChanged(object sender, EventArgs e)
		{
			if (Convert.ToInt32(nmMaxFreezes.Value) != RTC_StepActions.MaxInfiniteBlastUnits)
			{
				RTC_StepActions.SetMaxLifetimeBlastUnits(Convert.ToInt32(nmMaxFreezes.Value));
			}

			if (nmMaxCheats.Value != nmMaxFreezes.Value)
				nmMaxCheats.Value = nmMaxFreezes.Value;
		}

		private void nmMaxPipes_ValueChanged(object sender, EventArgs e)
		{
			RTC_StepActions.SetMaxLifetimeBlastUnits(Convert.ToInt32(nmMaxPipes.Value));
		}

		private void btnClearPipes_Click(object sender, EventArgs e)
		{
			RTC_StepActions.ClearStepBlastUnits();
		}

		private void cbLockPipes_CheckedChanged(object sender, EventArgs e)
		{
			RTC_StepActions.LockExecution = cbLockPipes.Checked;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_PIPE_LOCKPIPES) { objectValue = RTC_StepActions.LockExecution });
		}


		private void cbClearPipesOnRewind_CheckedChanged(object sender, EventArgs e)
		{
			RTC_StepActions.ClearStepActionsOnRewind(cbClearPipesOnRewind.Checked);
		}

		private void cbVectorLimiterList_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectedText = ((ComboBox)sender).SelectedItem.ToString();

			switch (selectedText)
			{
			}

		//	RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_VECTOR_LIMITER) { objectValue = RTC_VectorEngine.LimiterList });
		}

		private void cbVectorValueList_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectedText = (sender as ComboBox)?.SelectedItem.ToString();

			switch (selectedText)
			{
			}

			//RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_VECTOR_VALUES) { objectValue = RTC_VectorEngine.ValueList });
		}



		private void btnClearCheats_Click(object sender, EventArgs e)
		{
			RTC_StepActions.ClearStepBlastUnits();
		}

		private void cbUseCustomPrecision_CheckedChanged(object sender, EventArgs e)
		{
			if (cbUseCustomPrecision.Checked)
			{
				if (cbCustomPrecision.SelectedIndex == -1)
					cbCustomPrecision.SelectedIndex = 0;
			}
			else
			{
				cbCustomPrecision.SelectedIndex = -1;
				RTC_Core.CustomPrecision = -1;
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOMPRECISION) { objectValue = RTC_Core.CustomPrecision }, true);
			}
		}

		private void updateMinMaxBoxes(int precision)
		{
			updatingMinMax = true;
			switch (precision)
			{
				case 1:
					nmMinValueNightmare.Maximum = byte.MaxValue;
					nmMaxValueNightmare.Maximum = byte.MaxValue;

					nmMinValueHellgenie.Maximum = byte.MaxValue;
					nmMaxValueHellgenie.Maximum = byte.MaxValue;


					nmMinValueNightmare.Value = RTC_NightmareEngine.MinValue8Bit;
					nmMaxValueNightmare.Value = RTC_NightmareEngine.MaxValue8Bit;

					nmMinValueHellgenie.Value = RTC_HellgenieEngine.MinValue8Bit;
					nmMaxValueHellgenie.Value = RTC_HellgenieEngine.MaxValue8Bit;
					break;

				case 2:
					nmMinValueNightmare.Maximum = UInt16.MaxValue;
					nmMaxValueNightmare.Maximum = UInt16.MaxValue;

					nmMinValueHellgenie.Maximum = UInt16.MaxValue;
					nmMaxValueHellgenie.Maximum = UInt16.MaxValue;

					nmMinValueNightmare.Value = RTC_NightmareEngine.MinValue16Bit;
					nmMaxValueNightmare.Value = RTC_NightmareEngine.MaxValue16Bit;

					nmMinValueHellgenie.Value = RTC_HellgenieEngine.MinValue16Bit;
					nmMaxValueHellgenie.Value = RTC_HellgenieEngine.MaxValue16Bit;
					break;
				case 4:
					nmMinValueNightmare.Maximum = UInt32.MaxValue;
					nmMaxValueNightmare.Maximum = UInt32.MaxValue;

					nmMinValueHellgenie.Maximum = UInt32.MaxValue;
					nmMaxValueHellgenie.Maximum = UInt32.MaxValue;

					nmMinValueNightmare.Value = RTC_NightmareEngine.MinValue32Bit;
					nmMaxValueNightmare.Value = RTC_NightmareEngine.MaxValue32Bit;

					nmMinValueHellgenie.Value = RTC_HellgenieEngine.MinValue32Bit;
					nmMaxValueHellgenie.Value = RTC_HellgenieEngine.MaxValue32Bit;

					break;
			}
			updatingMinMax = false;
		}

		private void cbCustomPrecision_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Deselect the updown boxes so they commit if they're selected.
			//As you can use the scroll wheel over the combobox while the textbox is focused, this is required
			cbCustomPrecision.Focus();

			if (cbCustomPrecision.SelectedIndex != -1)
			{
				cbUseCustomPrecision.Checked = true;

				switch (cbCustomPrecision.SelectedIndex)
				{
					case 0:
						RTC_Core.CustomPrecision = 1;
						break;
					case 1:
						RTC_Core.CustomPrecision = 2;
						break;
					case 2:
						RTC_Core.CustomPrecision = 4;

						break;
				}
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOMPRECISION) { objectValue = RTC_Core.CustomPrecision }, true);
				updateMinMaxBoxes(RTC_Core.CustomPrecision);
			}
		}

		private void btnOpenBlastGenerator_Click(object sender, EventArgs e)
		{
			if (RTC_Core.bgForm != null)
				RTC_Core.bgForm.Close();
			RTC_Core.bgForm = new RTC_BlastGenerator_Form();
			RTC_Core.bgForm.LoadNoStashKey();
		}

		//TODO
		//Refactor this into a struct or something

		private void nmMinValueNightmare_ValueChanged(object sender, EventArgs e)
		{
			//We don't want to trigger this if it caps when stepping downwards
			if (updatingMinMax)
				return;

			long value = Convert.ToInt64(nmMinValueNightmare.Value);


			switch (RTC_Core.CurrentPrecision)
			{
				case 1:
					RTC_NightmareEngine.MinValue8Bit = value;
					break;
				case 2:
					RTC_NightmareEngine.MinValue16Bit = value;
					break;
				case 4:
					RTC_NightmareEngine.MinValue32Bit = value;
					break;
			}

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_NIGHTMARE_MINVALUE) { objectValue = new object[] { RTC_Core.CurrentPrecision, value } });
		}

		private void nmMaxValueNightmare_ValueChanged(object sender, EventArgs e)
		{
			//We don't want to trigger this if it caps when stepping downwards
			if (updatingMinMax)
				return;
			long value = Convert.ToInt64(nmMaxValueNightmare.Value);


			switch (RTC_Core.CurrentPrecision)
			{
				case 1:
					RTC_NightmareEngine.MaxValue8Bit = value;
					break;
				case 2:
					RTC_NightmareEngine.MaxValue16Bit = value;
					break;
				case 4:
					RTC_NightmareEngine.MaxValue32Bit = value;
					break;
			}

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_NIGHTMARE_MAXVALUE) { objectValue = new object[] { RTC_Core.CurrentPrecision, value } });
		}

		private void nmMinValueHellgenie_ValueChanged(object sender, EventArgs e)
		{
			//We don't want to trigger this if it caps when stepping downwards
			if (updatingMinMax)
				return;
			long value = Convert.ToInt64(nmMinValueHellgenie.Value);

			switch (RTC_Core.CurrentPrecision)
			{
				case 1:
					RTC_HellgenieEngine.MinValue8Bit = value;
					break;
				case 2:
					RTC_HellgenieEngine.MinValue16Bit = value;
					break;
				case 4:
					RTC_HellgenieEngine.MinValue32Bit = value;
					break;
			}

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_HELLGENIE_MINVALUE) { objectValue = new object[] { RTC_Core.CurrentPrecision, value } });
		}

		private void nmMaxValueHellgenie_ValueChanged(object sender, EventArgs e)
		{
			//We don't want to trigger this if it caps when stepping downwards
			if (updatingMinMax)
				return;

			long value = Convert.ToInt64(nmMaxValueHellgenie.Value);

			switch (RTC_Core.CurrentPrecision)
			{
				case 1:
					RTC_HellgenieEngine.MaxValue8Bit = value;
					break;
				case 2:
					RTC_HellgenieEngine.MaxValue16Bit = value;
					break;
				case 4:
					RTC_HellgenieEngine.MaxValue32Bit = value;
					break;
			}

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_HELLGENIE_MAXVALUE) { objectValue = new object[] { RTC_Core.CurrentPrecision, value } });
		}


		private void cbBlastType_SelectedIndexChanged(object sender, EventArgs e)
		{
			/*
			switch (cbBlastType.SelectedItem.ToString())
			{
				case "RANDOM":

					RTC_NightmareEngine.Algo = BlastByteAlgo.RANDOM;
					nmMinValueNightmare.Enabled = true;
					nmMaxValueNightmare.Enabled = true;
					break;

				case "RANDOMTILT":
					RTC_NightmareEngine.Algo = BlastByteAlgo.RANDOMTILT;
					nmMinValueNightmare.Enabled = true;
					nmMaxValueNightmare.Enabled = true;
					break;

				case "TILT":
					RTC_NightmareEngine.Algo = BlastByteAlgo.TILT;
					nmMinValueNightmare.Enabled = false;
					nmMaxValueNightmare.Enabled = false;
					break;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_NIGHTMARE_TYPE) { objectValue = RTC_NightmareEngine.Algo });
			}*/

		}

	}
}
