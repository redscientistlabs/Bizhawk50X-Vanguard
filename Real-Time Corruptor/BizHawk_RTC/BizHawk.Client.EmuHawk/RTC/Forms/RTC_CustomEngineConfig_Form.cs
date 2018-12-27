using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_CustomEngineConfig_Form : Form, IAutoColorize
	{

		private bool updatingMinMax = false;

		public RTC_CustomEngineConfig_Form()
		{
			InitializeComponent();
		}


		private void RTC_CustomEngineConfig_Form_Load(object sender, EventArgs e)
		{


			cbValueList.DisplayMember = "Text";
			cbLimiterList.DisplayMember = "Text";

			cbValueList.ValueMember = "Value";
			cbLimiterList.ValueMember = "Value";

			//Do this here as if it's stuck into the designer, it keeps defaulting out
			cbValueList.DataSource = RTC_Core.ValueListBindingSource;
			cbLimiterList.DataSource = RTC_Core.LimiterListBindingSource;
	
			if (RTC_Core.ValueListBindingSource.Count > 0)
			{
				cbValueList_SelectedIndexChanged(cbValueList, null);
			}
			if (RTC_Core.LimiterListBindingSource.Count > 0)
			{
				cbLimiterList_SelectedIndexChanged(cbLimiterList, null);
			}
		}

		private void RTC_CustomEngineConfig_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
				e.Cancel = true;
				this.Hide();
				return;
			}
		}

		private void nmMaxInfinite_ValueChanged(object sender, EventArgs e)
		{
			//This is a netcore redundant method
			RTC_StepActions.SetMaxLifetimeBlastUnits(Convert.ToInt32(nmMaxInfinite.Value));
		}

		//I'm using if-else's rather than switch statements on purpose.
		//The switch statements required more lines and were harder to read.
		private void unitSource_CheckedChanged(object sender, EventArgs e)
		{
			if (rbUnitSourceStore.Checked)
				RTC_CustomEngine.Source = BlastUnitSource.STORE;

			else if (rbUnitSourceValue.Checked)
				RTC_CustomEngine.Source = BlastUnitSource.VALUE;

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOM_UNIT_SOURCE) { objectValue = RTC_CustomEngine.Source });
		}

		private void valueSource_CheckedChanged(object sender, EventArgs e)
		{
			if (rbRandom.Checked)
				RTC_CustomEngine.ValueSource = CustomValueSource.RANDOM;

			else if (rbValueList.Checked)
				RTC_CustomEngine.ValueSource = CustomValueSource.VALUELIST;

			else if (rbRange.Checked)
				RTC_CustomEngine.ValueSource = CustomValueSource.RANGE;

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOM_VALUE_SOURCE) { objectValue = RTC_CustomEngine.ValueSource});
		}


		private void storeTime_CheckedChanged(object sender, EventArgs e)
		{
			if (rbStoreImmediate.Checked)
				RTC_CustomEngine.StoreTime = ActionTime.IMMEDIATE;

			else if (rbStoreFirstExecute.Checked)
				RTC_CustomEngine.StoreTime = ActionTime.PREEXECUTE;

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOM_STORE_TIME) { objectValue = RTC_CustomEngine.StoreTime });
		}
		
		private void storeAddress_CheckedChanged(object sender, EventArgs e)
		{
			if (rbStoreRandom.Checked)
				RTC_CustomEngine.StoreAddress = CustomStoreAddress.RANDOM;

			else if (rbStoreSame.Checked)
				RTC_CustomEngine.StoreAddress = CustomStoreAddress.SAME;

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOM_STORE_ADDRESS) { objectValue = RTC_CustomEngine.StoreAddress });
		}


		private void storeType_CheckedChanged(object sender, EventArgs e)
		{
			if (rbStoreOnce.Checked)
				RTC_CustomEngine.StoreType = StoreType.ONCE;

			if (rbStoreStep.Checked)
				RTC_CustomEngine.StoreType = StoreType.CONTINUOUS;

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOM_STORE_TYPE) { objectValue = RTC_CustomEngine.StoreType });
		}


		private void nmMinValue_ValueChanged(object sender, EventArgs e)
		{
			//We don't want to trigger this if it caps when stepping downwards
			if (updatingMinMax)
				return;
			long value = Convert.ToInt64(nmMinValue.Value);

			switch (RTC_Core.CurrentPrecision)
			{
				case 1:
					RTC_CustomEngine.MinValue8Bit = value;
					break;
				case 2:
					RTC_CustomEngine.MinValue16Bit = value;
					break;
				case 4:
					RTC_CustomEngine.MinValue32Bit = value;
					break;
			}

			//We send the value and the precision and it determines which to update using the precision on the other side.
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOM_RANGE_MINVALUE) { objectValue = new object[] { RTC_Core.CurrentPrecision, value } });
		}
		private void nmMaxValue_ValueChanged(object sender, EventArgs e)
		{
			//We don't want to trigger this if it caps when stepping downwards
			if (updatingMinMax)
				return;
			long value = Convert.ToInt64(nmMaxValue.Value);


			switch (RTC_Core.CurrentPrecision)
			{
				case 1:
					RTC_CustomEngine.MaxValue8Bit = value;
					break;
				case 2:
					RTC_CustomEngine.MaxValue16Bit = value;
					break;
				case 4:
					RTC_CustomEngine.MaxValue32Bit = value;
					break;
			}

			//We send the value and the precision and it determines which to update using the precision on the other side.
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOM_RANGE_MAXVALUE) { objectValue = new object[] { RTC_Core.CurrentPrecision, value } });
		}


		private void cbLockUnits_CheckedChanged(object sender, EventArgs e)
		{
			//Netcore redundant method
			RTC_StepActions.SetLockExecution(cbLockUnits.Checked);
		}

		private void cbClearRewind_CheckedChanged(object sender, EventArgs e)
		{
			//Netcore redundant method
			RTC_StepActions.ClearStepActionsOnRewind(cbClearRewind.Checked);
		}

		private void cbLoopUnit_CheckedChanged(object sender, EventArgs e)
		{
			RTC_CustomEngine.Loop = cbLoopUnit.Checked;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOM_LOOP) { objectValue = RTC_CustomEngine.Loop });
		}

		private void cbValueList_SelectedIndexChanged(object sender, EventArgs e)
		{
			RTC_CustomEngine.ValueListHash = (string)cbValueList.SelectedValue;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOM_VALUELIST) { objectValue = RTC_CustomEngine.ValueListHash });
		}
		private void cbLimiterList_SelectedIndexChanged(object sender, EventArgs e)
		{
			RTC_CustomEngine.LimiterListHash = (string)cbLimiterList.SelectedValue;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOM_LIMITERLIST) { objectValue = RTC_CustomEngine.LimiterListHash });
		}
		private void limiterTime_CheckedChanged(object sender, EventArgs e)
		{
			if (rbLimiterNone.Checked)
				RTC_CustomEngine.LimiterTime = ActionTime.NONE;

			else if (rbLimiterGenerate.Checked)
				RTC_CustomEngine.LimiterTime = ActionTime.GENERATE;

			else if (rbLimiterFirstExecute.Checked)
				RTC_CustomEngine.LimiterTime = ActionTime.PREEXECUTE;

			else if (rbLimiterExecute.Checked)
				RTC_CustomEngine.LimiterTime = ActionTime.EXECUTE;

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOM_LIMITERTIME) { objectValue = RTC_CustomEngine.LimiterTime });
		}
		
		private void btnClearActive_Click(object sender, EventArgs e)
		{
			//Netcore redundant method
			RTC_StepActions.ClearStepBlastUnits();
		}

		private void nmLifetime_ValueChanged(object sender, EventArgs e)
		{
			RTC_CustomEngine.Lifetime = Convert.ToInt32(nmLifetime.Value);
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOM_LIFETIME) { objectValue = RTC_CustomEngine.Lifetime });
		}

		private void nmDelay_ValueChanged(object sender, EventArgs e)
		{
			RTC_CustomEngine.Delay = Convert.ToInt32(nmDelay.Value);
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOM_DELAY) { objectValue = RTC_CustomEngine.Delay });
		}

		private void nmTilt_ValueChanged(object sender, EventArgs e)
		{
			RTC_CustomEngine.TiltValue = (BigInteger)nmTilt.Value;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOM_TILT) { objectValue = RTC_CustomEngine.TiltValue });
		}

		public void UpdateMinMaxBoxes(int precision)
		{
			updatingMinMax = true;
			switch (precision)
			{
				case 1:
					nmMinValue.Maximum = byte.MaxValue;
					nmMaxValue.Maximum = byte.MaxValue;

					nmMinValue.Value = RTC_CustomEngine.MinValue8Bit;
					nmMaxValue.Value = RTC_CustomEngine.MaxValue8Bit;
					break;

				case 2:
					nmMinValue.Maximum = UInt16.MaxValue;
					nmMaxValue.Maximum = UInt16.MaxValue;
									   
					nmMinValue.Value = RTC_CustomEngine.MinValue16Bit;
					nmMaxValue.Value = RTC_CustomEngine.MaxValue16Bit;
					break;
				case 4:
					nmMinValue.Maximum = UInt32.MaxValue;
					nmMaxValue.Maximum = UInt32.MaxValue;

					nmMinValue.Value = RTC_CustomEngine.MinValue32Bit;
					nmMaxValue.Value = RTC_CustomEngine.MaxValue32Bit;

					break;
			}
			updatingMinMax = false;
		}

		private void cbLimiterInverted_CheckedChanged(object sender, EventArgs e)
		{
			RTC_CustomEngine.LimiterInverted = cbLimiterInverted.Checked;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_CUSTOM_LIMITERINVERTED) { objectValue = RTC_CustomEngine.LimiterInverted });

		}
	}
}
