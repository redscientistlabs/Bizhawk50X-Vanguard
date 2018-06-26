using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_EngineConfig_Form : Form
	{
		public bool DontUpdateIntensity = false;

		public int Intensity
		{
			get
			{
				return RTC_Core.Intensity;
			}
			set
			{
				if (DontUpdateIntensity)
					return;

				RTC_Core.Intensity = value;
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_INTENSITY) { objectValue = RTC_Core.Intensity });

				DontUpdateIntensity = true;

				if (nmIntensity.Value != RTC_Core.Intensity)
					nmIntensity.Value = RTC_Core.Intensity;

				if (RTC_Core.ghForm.nmIntensity.Value != RTC_Core.Intensity)
					RTC_Core.ghForm.nmIntensity.Value = RTC_Core.Intensity;

				int _fx = Convert.ToInt32(Math.Sqrt(value) * 2000d);

				if (track_Intensity.Value != _fx)
					track_Intensity.Value = _fx;

				if (RTC_Core.ghForm.track_Intensity.Value != _fx)
					RTC_Core.ghForm.track_Intensity.Value = _fx;

				DontUpdateIntensity = false;
			}
		}

		public bool DontUpdateErrorDelay = false;

		public int ErrorDelay
		{
			get
			{
				return RTC_Core.ErrorDelay;
			}
			set
			{
				if (DontUpdateErrorDelay)
					return;

				RTC_Core.ErrorDelay = Convert.ToInt32(value);
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_ERRORDELAY) { objectValue = RTC_Core.ErrorDelay });

				DontUpdateErrorDelay = true;

				if (nmErrorDelay.Value != RTC_Core.ErrorDelay)
					nmErrorDelay.Value = RTC_Core.ErrorDelay;

				int _fx = Convert.ToInt32(Math.Sqrt(value) * 2000d);

				if (track_ErrorDelay.Value != _fx)
					track_ErrorDelay.Value = _fx;

				DontUpdateErrorDelay = false;
			}
		}

		public RTC_EngineConfig_Form()
		{
			InitializeComponent();
		}

		private void RTC_EC_Form_Load(object sender, EventArgs e)
		{
			Controls.Remove(gbNightmareEngine);
			pnCorruptionEngine.Controls.Add(gbNightmareEngine);
			gbNightmareEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);

			Controls.Remove(gbHellgenieEngine);
			pnCorruptionEngine.Controls.Add(gbHellgenieEngine);
			gbHellgenieEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);

			Controls.Remove(gbDistortionEngine);
			pnCorruptionEngine.Controls.Add(gbDistortionEngine);
			gbDistortionEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);

			Controls.Remove(gbFreezeEngine);
			pnCorruptionEngine.Controls.Add(gbFreezeEngine);
			gbFreezeEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);
			Controls.Remove(gbPipeEngine);
			pnCorruptionEngine.Controls.Add(gbPipeEngine);
			gbPipeEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);

			Controls.Remove(gbVectorEngine);
			pnCorruptionEngine.Controls.Add(gbVectorEngine);
			gbVectorEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);

			Controls.Remove(gbBlastGeneratorEngine);
			pnCorruptionEngine.Controls.Add(gbBlastGeneratorEngine);
			gbBlastGeneratorEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);

			cbSelectedEngine.SelectedIndex = 0;
			cbVectorLimiterList.SelectedIndex = 0;
			cbVectorValueList.SelectedIndex = 0;
			cbBlastRadius.SelectedIndex = 0;
			cbBlastType.SelectedIndex = 0;
			cbMemoryDomainTool.SelectedIndex = 0;
			cbCustomPrecision.SelectedIndex = 0;
		}

		public void setMemoryDomainsSelectedDomains(string[] _domains)
		{
			lbMemoryDomains_DontExecute_SelectedIndexChanged = true;

			for (int i = 0; i < lbMemoryDomains.Items.Count; i++)
				if (_domains.Contains(lbMemoryDomains.Items[i].ToString()))
					lbMemoryDomains.SetSelected(i, true);
				else
					lbMemoryDomains.SetSelected(i, false);

			lbMemoryDomains_DontExecute_SelectedIndexChanged = false;
			lbMemoryDomains_SelectedIndexChanged(null, null);
		}

		public void setMemoryDomainsAllButSelectedDomains(string[] _blacklistedDomains)
		{
			lbMemoryDomains_DontExecute_SelectedIndexChanged = true;

			for (
				int i = 0; i < lbMemoryDomains.Items.Count; i++)
				if (_blacklistedDomains.Contains(lbMemoryDomains.Items[i].ToString()))
					lbMemoryDomains.SetSelected(i, false);
				else
					lbMemoryDomains.SetSelected(i, true);

			lbMemoryDomains_DontExecute_SelectedIndexChanged = false;
			lbMemoryDomains_SelectedIndexChanged(null, null);
		}

		private void btnRefreshDomains_Click(object sender, EventArgs e)
		{
			RefreshDomains();

			//RTC_Restore.SaveRestore();
		}

		public void track_ErrorDelay_Scroll(object sender, EventArgs e)
		{
			double fx = Math.Ceiling(Math.Pow((track_ErrorDelay.Value * 0.0005d), 2));
			int _fx = Convert.ToInt32(fx);

			if (_fx != ErrorDelay)
				ErrorDelay = _fx;
		}

		public void nmErrorDelay_ValueChanged(object sender, EventArgs e)
		{
			int _fx = Convert.ToInt32(nmErrorDelay.Value);

			if (_fx != ErrorDelay)
				ErrorDelay = _fx;
		}

		public void track_Intensity_Scroll(object sender, EventArgs e)
		{
			double fx = Math.Floor(Math.Pow((track_Intensity.Value * 0.0005d), 2));
			int _fx = Convert.ToInt32(fx);

			if (_fx != Intensity)
				Intensity = _fx;
		}

		public void nmIntensity_ValueChanged(object sender, EventArgs e)
		{
			int _fx = Convert.ToInt32(nmIntensity.Value);

			if (Intensity != _fx)
				Intensity = _fx;
		}

		private void cbBlastRadius_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (cbBlastRadius.SelectedItem.ToString())
			{
				case "SPREAD":
					RTC_Core.Radius = BlastRadius.SPREAD;
					break;

				case "CHUNK":
					RTC_Core.Radius = BlastRadius.CHUNK;
					break;

				case "BURST":
					RTC_Core.Radius = BlastRadius.BURST;
					break;

				case "NORMALIZED":
					RTC_Core.Radius = BlastRadius.NORMALIZED;
					break;

				case "PROPORTIONAL":
					RTC_Core.Radius = BlastRadius.PROPORTIONAL;
					break;

				case "EVEN":
					RTC_Core.Radius = BlastRadius.EVEN;
					break;
			}

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_BLASTRADIUS) { objectValue = RTC_Core.Radius });
		}

		private void cbBlastType_SelectedIndexChanged(object sender, EventArgs e)
		{
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
			}

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_NIGHTMARE_TYPE) { objectValue = RTC_NightmareEngine.Algo });
		}

		private void nmMaxCheats_ValueChanged(object sender, EventArgs e)
		{
			if (Convert.ToInt32(nmMaxCheats.Value) != RTC_HellgenieEngine.MaxCheats)
			{
				RTC_HellgenieEngine.MaxCheats = Convert.ToInt32(nmMaxCheats.Value);
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_HELLGENIE_MAXCHEATS) { objectValue = RTC_HellgenieEngine.MaxCheats });
			}

			if (nmMaxCheats.Value != nmMaxFreezes.Value)
				nmMaxFreezes.Value = nmMaxCheats.Value;
		}

		public bool lbMemoryDomains_DontExecute_SelectedIndexChanged = false;

		private void lbMemoryDomains_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lbMemoryDomains_DontExecute_SelectedIndexChanged)
				return;

			string[] selectedDomains = lbMemoryDomains.SelectedItems.Cast<string>().ToArray();

			RTC_MemoryDomains.UpdateSelectedDomains(selectedDomains, true);

			//RTC_Restore.SaveRestore();
		}

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			RefreshDomains();

			lbMemoryDomains_DontExecute_SelectedIndexChanged = true;

			for (int i = 0; i < lbMemoryDomains.Items.Count; i++)
				lbMemoryDomains.SetSelected(i, true);

			lbMemoryDomains_DontExecute_SelectedIndexChanged = false;

			lbMemoryDomains_SelectedIndexChanged(null, null);
		}

		private void btnAutoSelectDomains_Click(object sender, EventArgs e)
		{
			RefreshDomains();
			setMemoryDomainsAllButSelectedDomains(RTC_MemoryDomains.GetBlacklistedDomains());
		}

		private void cbClearCheatsOnRewind_CheckedChanged(object sender, EventArgs e)
		{
			if (cbClearFreezesOnRewind.Checked != cbClearCheatsOnRewind.Checked)
				cbClearFreezesOnRewind.Checked = cbClearCheatsOnRewind.Checked;

			RTC_Core.ClearCheatsOnRewind = cbClearCheatsOnRewind.Checked;

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_HELLGENIE_CHEARCHEATSREWIND) { objectValue = RTC_Core.ClearCheatsOnRewind });
		}

		private void nmDistortionDelay_ValueChanged(object sender, EventArgs e)
		{
			RTC_DistortionEngine.MaxAge = Convert.ToInt32(nmDistortionDelay.Value);
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_DISTORTION_DELAY) { objectValue = RTC_DistortionEngine.MaxAge });
		}

		private void btnResyncDistortionEngine_Click(object sender, EventArgs e)
		{
			RTC_DistortionEngine.Resync();
		}


		public void RefreshDomains()
		{
			RTC_MemoryDomains.RefreshDomains();

			lbMemoryDomains.Items.Clear();
			lbMemoryDomains.Items.AddRange(RTC_MemoryDomains.MemoryInterfaces.Keys.ToArray());

			if (RTC_MemoryDomains.VmdPool.Count > 0)
				lbMemoryDomains.Items.AddRange(RTC_MemoryDomains.VmdPool.Values.Select(it => it.ToString()).ToArray());
		}

		public void RefreshDomainsAndKeepSelected(string[] overrideDomains = null)
		{
			string[] copy = RTC_MemoryDomains.lastSelectedDomains;

			if (overrideDomains != null)
				copy = overrideDomains;

			RefreshDomains(); //refresh and reload domains

			RTC_MemoryDomains.UpdateSelectedDomains(copy);

			setMemoryDomainsSelectedDomains(copy);
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
				labelBlastRadius.Visible = false;
				labelIntensity.Visible = false;
				labelIntensityTimes.Visible = false;
				labelErrorDelay.Visible = false;
				labelErrorDelaySteps.Visible = false;
				nmErrorDelay.Visible = false;
				nmIntensity.Visible = false;
				track_ErrorDelay.Visible = false;
				track_Intensity.Visible = false;
				cbBlastRadius.Visible = false;
			}
			else if (cbSelectedEngine.SelectedItem.ToString() == "Freeze Engine")
			{
				labelBlastRadius.Visible = true;
				labelIntensity.Visible = true;
				labelIntensityTimes.Visible = true;
				labelErrorDelay.Visible = true;
				labelErrorDelaySteps.Visible = true;
				nmErrorDelay.Visible = true;
				nmIntensity.Visible = true;
				track_ErrorDelay.Visible = true;
				track_Intensity.Visible = true;
				cbBlastRadius.Visible = true;
			}
			else
			{
				labelBlastRadius.Visible = true;
				labelIntensity.Visible = true;
				labelIntensityTimes.Visible = true;
				labelErrorDelay.Visible = true;
				labelErrorDelaySteps.Visible = true;
				nmErrorDelay.Visible = true;
				nmIntensity.Visible = true;
				track_ErrorDelay.Visible = true;
				track_Intensity.Visible = true;
				cbBlastRadius.Visible = true;
			}

			cbSelectedEngine.BringToFront();

			RTC_HellgenieEngine.ClearCheats();
			RTC_PipeEngine.ClearPipes();
		}

		private void cbClearFreezesOnRewind_CheckedChanged(object sender, EventArgs e)
		{
			if (RTC_Core.ecForm.cbClearFreezesOnRewind.Checked != RTC_Core.ecForm.cbClearCheatsOnRewind.Checked)
				RTC_Core.ecForm.cbClearCheatsOnRewind.Checked = RTC_Core.ecForm.cbClearFreezesOnRewind.Checked;

			RTC_Core.ClearCheatsOnRewind = RTC_Core.ecForm.cbClearFreezesOnRewind.Checked;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_HELLGENIE_CHEARCHEATSREWIND) { objectValue = RTC_Core.ClearCheatsOnRewind });
		}

		private void nmMaxFreezes_ValueChanged(object sender, EventArgs e)
		{
			if (Convert.ToInt32(RTC_Core.ecForm.nmMaxFreezes.Value) != RTC_HellgenieEngine.MaxCheats)
			{
				RTC_HellgenieEngine.MaxCheats = Convert.ToInt32(RTC_Core.ecForm.nmMaxFreezes.Value);
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_HELLGENIE_MAXCHEATS) { objectValue = RTC_HellgenieEngine.MaxCheats });
			}

			if (RTC_Core.ecForm.nmMaxCheats.Value != RTC_Core.ecForm.nmMaxFreezes.Value)
				RTC_Core.ecForm.nmMaxCheats.Value = RTC_Core.ecForm.nmMaxFreezes.Value;
		}

		private void nmMaxPipes_ValueChanged(object sender, EventArgs e)
		{
			RTC_PipeEngine.MaxPipes = Convert.ToInt32(nmMaxPipes.Value);
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_PIPE_MAXPIPES) { objectValue = RTC_PipeEngine.MaxPipes });
		}

		private void btnClearPipes_Click(object sender, EventArgs e)
		{
			RTC_PipeEngine.ClearPipes();
		}

		private void cbLockPipes_CheckedChanged(object sender, EventArgs e)
		{
			RTC_PipeEngine.LockPipes = cbLockPipes.Checked;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_PIPE_LOCKPIPES) { objectValue = RTC_PipeEngine.LockPipes });
		}

		private void cbProcessOnStep_CheckedChanged(object sender, EventArgs e)
		{
			RTC_PipeEngine.ProcessOnStep = cbProcessOnStep.Checked;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_PIPE_PROCESSONSTEP) { objectValue = RTC_PipeEngine.ProcessOnStep });
		}

		private void cbClearPipesOnRewind_CheckedChanged(object sender, EventArgs e)
		{
			RTC_Core.ClearPipesOnRewind = cbClearPipesOnRewind.Checked;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_PIPE_CLEARPIPESREWIND) { objectValue = RTC_Core.ClearPipesOnRewind });
		}

		private void cbVectorLimiterList_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectedText = (sender as ComboBox).SelectedItem.ToString();

			switch (selectedText)
			{
				case "Extended":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.extendedListOfConstants;
					lbVectorEngineLimiterText1.Text = "-65536.00 to +65536.00 in low res";
					lbVectorEngineLimiterText2.Text = "including tiny decimals";
					break;
				case "Extended+":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.listOfPositiveConstants;
					lbVectorEngineLimiterText1.Text = "0 to +65536 in low res";
					lbVectorEngineLimiterText2.Text = "including tiny decimals";
					break;
				case "Extended-":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.listOfNegativeConstants;
					lbVectorEngineLimiterText1.Text = "0 to -65536 in low res";
					lbVectorEngineLimiterText2.Text = "including tiny decimals";
					break;
				case "Whole":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.listOfWholeConstants;
					lbVectorEngineLimiterText1.Text = "-65536.00 to +65536.00 in low res";
					lbVectorEngineLimiterText2.Text = "integral numbers";
					break;
				case "Whole+":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.listOfWholePositiveConstants;
					lbVectorEngineLimiterText1.Text = "0 to +65536.00 in low res";
					lbVectorEngineLimiterText2.Text = "integral numbers";
					break;
				case "Tiny":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.listOfTinyConstants;
					lbVectorEngineLimiterText1.Text = "tiny decimals between";
					lbVectorEngineLimiterText2.Text = "-1.00 and +1.00";
					break;
				case "One":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.constantPositiveOne;
					lbVectorEngineLimiterText1.Text = "The number 1.00";
					lbVectorEngineLimiterText2.Text = "";
					break;
				case "One*":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.constantOne;
					lbVectorEngineLimiterText1.Text = "The numbers 1.00 and -1.00";
					lbVectorEngineLimiterText2.Text = "";
					break;
				case "Two":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.constantPositiveTwo;
					lbVectorEngineLimiterText1.Text = "The number 2.00";
					lbVectorEngineLimiterText2.Text = "";
					break;
				case "AnyFloat":
					RTC_VectorEngine.limiterList = null;
					lbVectorEngineLimiterText1.Text = "Any address is legal";
					lbVectorEngineLimiterText2.Text = "";
					break;
			}

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_VECTOR_LIMITER) { objectValue = RTC_VectorEngine.limiterList });
		}

		private void cbVectorValueList_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectedText = (sender as ComboBox).SelectedItem.ToString();

			switch (selectedText)
			{
				case "Extended":
					RTC_VectorEngine.valueList = RTC_VectorEngine.extendedListOfConstants;
					lbVectorEngineValueText1.Text = "-65536.00 to +65536.00 in low res";
					lbVectorEngineValueText2.Text = "including tiny decimals";
					break;
				case "Extended+":
					RTC_VectorEngine.valueList = RTC_VectorEngine.listOfPositiveConstants;
					lbVectorEngineValueText1.Text = "0 to +65536 in low res";
					lbVectorEngineValueText2.Text = "including tiny decimals";
					break;
				case "Extended-":
					RTC_VectorEngine.valueList = RTC_VectorEngine.listOfNegativeConstants;
					lbVectorEngineValueText1.Text = "0 to -65536 in low res";
					lbVectorEngineValueText2.Text = "including tiny decimals";
					break;
				case "Whole":
					RTC_VectorEngine.valueList = RTC_VectorEngine.listOfWholeConstants;
					lbVectorEngineValueText1.Text = "-65536.00 to +65536.00 in low res";
					lbVectorEngineValueText2.Text = "integral numbers";
					break;
				case "Whole+":
					RTC_VectorEngine.valueList = RTC_VectorEngine.listOfWholePositiveConstants;
					lbVectorEngineValueText1.Text = "0 to +65536.00 in low res";
					lbVectorEngineValueText2.Text = "integral numbers";
					break;
				case "Tiny":
					RTC_VectorEngine.valueList = RTC_VectorEngine.listOfTinyConstants;
					lbVectorEngineValueText1.Text = "tiny decimals between";
					lbVectorEngineValueText2.Text = "-1.00 and +1.00";
					break;
				case "One":
					RTC_VectorEngine.valueList = RTC_VectorEngine.constantPositiveOne;
					lbVectorEngineValueText1.Text = "The number 1.00";
					lbVectorEngineValueText2.Text = "";
					break;
				case "One*":
					RTC_VectorEngine.valueList = RTC_VectorEngine.constantOne;
					lbVectorEngineValueText1.Text = "The numbers 1.00 and -1.00";
					lbVectorEngineValueText2.Text = "";
					break;
				case "Two":
					RTC_VectorEngine.valueList = RTC_VectorEngine.constantPositiveTwo;
					lbVectorEngineValueText1.Text = "The number 2.00";
					lbVectorEngineValueText2.Text = "";
					break;
				case "AnyFloat":
					RTC_VectorEngine.valueList = null;
					lbVectorEngineValueText1.Text = "Randomly generated Float";
					lbVectorEngineValueText2.Text = "";
					break;
			}

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_VECTOR_VALUES) { objectValue = RTC_VectorEngine.valueList });
		}

		private void nmTiltPipeValue_ValueChanged(object sender, EventArgs e)
		{
			RTC_PipeEngine.tiltValue = Convert.ToInt32(nmTiltPipeValue.Value);
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_PIPE_TILTVALUE) { objectValue = RTC_PipeEngine.tiltValue });
		}

		private void cbGenerateChainedPipes_CheckedChanged(object sender, EventArgs e)
		{
			RTC_PipeEngine.ChainedPipes = cbGenerateChainedPipes.Checked;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_PIPE_CHAINEDPIPES) { objectValue = RTC_PipeEngine.ChainedPipes });
		}

		private void cbMemoryDomainTool_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!pnAdvancedTool.Controls.Contains(RTC_Core.vmdPoolForm))
			{
				RTC_Core.vmdPoolForm.TopLevel = false;
				pnAdvancedTool.Controls.Add(RTC_Core.vmdPoolForm);
			}
			if (RTC_Core.vmdPoolForm.Visible)
				RTC_Core.vmdPoolForm.Hide();

			if (!pnAdvancedTool.Controls.Contains(RTC_Core.vmdGenForm))
			{
				RTC_Core.vmdGenForm.TopLevel = false;
				pnAdvancedTool.Controls.Add(RTC_Core.vmdGenForm);
			}
			if (RTC_Core.vmdGenForm.Visible)
				RTC_Core.vmdGenForm.Hide();

			if (!pnAdvancedTool.Controls.Contains(RTC_Core.vmdActForm))
			{
				RTC_Core.vmdActForm.TopLevel = false;
				pnAdvancedTool.Controls.Add(RTC_Core.vmdActForm);
			}
			if (RTC_Core.vmdActForm.Visible)
				RTC_Core.vmdActForm.Hide();

			switch (cbMemoryDomainTool.SelectedItem.ToString())
			{
				case "No Tool Selected":
					break;
				case "Virtual Memory Domain Pool":
					RTC_Core.vmdPoolForm.Show();
					break;
				case "Virtual Memory Domain Generator":
					RTC_Core.vmdGenForm.Show();
					break;
				case "ActiveTable Generator":
					RTC_Core.vmdActForm.Show();
					break;
			}
		}

		Guid? ErrorDelayToken = null;
		Guid? IntensityToken = null;

		private void track_ErrorDelay_MouseDown(object sender, MouseEventArgs e)
		{
			ErrorDelayToken = RTC_NetCore.HugeOperationStart("LAZY");
		}

		private void track_ErrorDelay_MouseUp(object sender, MouseEventArgs e)
		{
			RTC_NetCore.HugeOperationEnd(ErrorDelayToken);

			track_ErrorDelay_Scroll(sender, e);
		}

		private void track_Intensity_MouseDown(object sender, MouseEventArgs e)
		{
			IntensityToken = RTC_NetCore.HugeOperationStart("LAZY");
		}

		private void track_Intensity_MouseUp(object sender, MouseEventArgs e)
		{
			RTC_NetCore.HugeOperationEnd(IntensityToken);

			track_Intensity_Scroll(sender, e);
		}

		private void btnClearCheats_Click(object sender, EventArgs e)
		{
			RTC_HellgenieEngine.ClearCheats();

			//RTC_Restore.SaveRestore();
		}

		private void nmMaxCheats_ValueChanged(object sender, KeyPressEventArgs e)
		{
		}

		private void nmMaxCheats_ValueChanged(object sender, KeyEventArgs e)
		{
		}

		private void nmMaxFreezes_ValueChanged(object sender, KeyPressEventArgs e)
		{
		}

		private void nmMaxFreezes_ValueChanged(object sender, KeyEventArgs e)
		{
		}

		private void nmDistortionDelay_ValueChanged(object sender, KeyPressEventArgs e)
		{
		}

		private void nmDistortionDelay_ValueChanged(object sender, KeyEventArgs e)
		{
		}

		private void nmTiltPipeValue_ValueChanged(object sender, KeyPressEventArgs e)
		{
		}

		private void nmTiltPipeValue_ValueChanged(object sender, KeyEventArgs e)
		{
		}

		private void nmMaxPipes_ValueChanged(object sender, KeyPressEventArgs e)
		{
		}

		private void nmMaxPipes_ValueChanged(object sender, KeyEventArgs e)
		{
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
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOMPRECISION) { objectValue = RTC_Core.CustomPrecision });
			}
		}

		private void cbCustomPrecision_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbCustomPrecision.SelectedIndex != -1)
			{
				cbUseCustomPrecision.Checked = true;

				switch (cbCustomPrecision.SelectedIndex)
				{
					case 0:
						RTC_Core.CustomPrecision = 1;

						nmMinValueNightmare.Maximum = byte.MaxValue;
						nmMinValueHellgenie.Maximum = byte.MaxValue;

						nmMaxValueNightmare.Maximum = byte.MaxValue;
						nmMaxValueHellgenie.Maximum = byte.MaxValue;

						nmMaxValueNightmare.Value = byte.MaxValue;
						nmMaxValueHellgenie.Value = byte.MaxValue;
						break;
					case 1:
						RTC_Core.CustomPrecision = 2;

						nmMinValueNightmare.Maximum = UInt16.MaxValue;
						nmMinValueHellgenie.Maximum = UInt16.MaxValue;

						nmMaxValueNightmare.Maximum = UInt16.MaxValue;
						nmMaxValueHellgenie.Maximum = UInt16.MaxValue;

						nmMaxValueNightmare.Value = UInt16.MaxValue;
						nmMaxValueHellgenie.Value = UInt16.MaxValue;
						break;
					case 2:
						RTC_Core.CustomPrecision = 4;

						nmMinValueNightmare.Maximum = UInt32.MaxValue;
						nmMinValueHellgenie.Maximum = UInt32.MaxValue;

						nmMaxValueNightmare.Maximum = UInt32.MaxValue;
						nmMaxValueHellgenie.Maximum = UInt32.MaxValue;

						nmMaxValueNightmare.Value = UInt32.MaxValue;
						nmMaxValueHellgenie.Value = UInt32.MaxValue;
						break;
				}

				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOMPRECISION) { objectValue = RTC_Core.CustomPrecision });
			}
		}

		private void btnOpenBlastGenerator_Click(object sender, EventArgs e)
		{
			if (RTC_Core.bgForm != null)
				RTC_Core.bgForm.Close();
			RTC_Core.bgForm = new RTC_BlastGenerator_Form();
			RTC_Core.bgForm.LoadNoStashKey();
		}

		private void nmMinValueNightmare_ValueChanged(object sender, EventArgs e)
		{
			RTC_NightmareEngine.MinValue = Convert.ToInt64(nmMinValueNightmare.Value);
		}

		private void nmMaxValueNightmare_ValueChanged(object sender, EventArgs e)
		{
			RTC_NightmareEngine.MaxValue = Convert.ToInt64(nmMaxValueNightmare.Value);
		}

		private void nmMinValueHellgenie_ValueChanged(object sender, EventArgs e)
		{
			RTC_HellgenieEngine.MinValue = Convert.ToInt64(nmMinValueHellgenie.Value);
		}

		private void nmMaxValueHellgenie_ValueChanged(object sender, EventArgs e)
		{
			RTC_HellgenieEngine.MaxValue = Convert.ToInt64(nmMaxValueHellgenie.Value);
		}
	}
}
