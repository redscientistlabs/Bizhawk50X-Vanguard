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
using static RTC.RTC_Unispec;

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
			RTC_Unispec.RTCSpec.Update(RTCSPEC.STEP_MAXINFINITEBLASTUNITS.ToString(), Convert.ToInt32(nmMaxInfinite.Value));
		}

		//I'm using if-else's rather than switch statements on purpose.
		//The switch statements required more lines and were harder to read.
		private void unitSource_CheckedChanged(object sender, EventArgs e)
		{

			if (rbUnitSourceStore.Checked)
				RTCSpec.Update(RTCSPEC.CUSTOM_SOURCE.ToString(), BlastUnitSource.STORE);

			else if (rbUnitSourceValue.Checked)
				RTCSpec.Update(RTCSPEC.CUSTOM_SOURCE.ToString(), BlastUnitSource.VALUE);
		}

		private void valueSource_CheckedChanged(object sender, EventArgs e)
		{
			RTCSpec.Update(RTCSPEC.CUSTOM_VALUESOURCE.ToString(), CustomValueSource.RANDOM);
			if (rbRandom.Checked)
				RTCSpec.Update(RTCSPEC.CUSTOM_VALUESOURCE.ToString(), CustomValueSource.RANDOM);

			else if (rbValueList.Checked)
				RTCSpec.Update(RTCSPEC.CUSTOM_VALUESOURCE.ToString(), CustomValueSource.VALUELIST);

			else if (rbRange.Checked)
				RTCSpec.Update(RTCSPEC.CUSTOM_VALUESOURCE.ToString(), CustomValueSource.RANGE);
		}


		private void storeTime_CheckedChanged(object sender, EventArgs e)
		{
			if (rbStoreImmediate.Checked)
				RTCSpec.Update(RTCSPEC.CUSTOM_STORETIME.ToString(), ActionTime.IMMEDIATE);

			else if (rbStoreFirstExecute.Checked)
				RTCSpec.Update(RTCSPEC.CUSTOM_STORETIME.ToString(), ActionTime.PREEXECUTE);
		}
		
		private void storeAddress_CheckedChanged(object sender, EventArgs e)
		{
			if (rbStoreRandom.Checked)
				RTCSpec.Update(RTCSPEC.CUSTOM_STORETIME.ToString(), CustomStoreAddress.RANDOM);

			else if (rbStoreSame.Checked)
				RTCSpec.Update(RTCSPEC.CUSTOM_STORETIME.ToString(), CustomStoreAddress.SAME);
		}


		private void storeType_CheckedChanged(object sender, EventArgs e)
		{
			if (rbStoreOnce.Checked)
				RTCSpec.Update(RTCSPEC.CUSTOM_STORETYPE.ToString(), StoreType.ONCE);

			if (rbStoreStep.Checked)
				RTCSpec.Update(RTCSPEC.CUSTOM_STORETYPE.ToString(), StoreType.CONTINUOUS);

		}


		private void nmMinValue_ValueChanged(object sender, EventArgs e)
		{
			//We don't want to trigger this if it caps when stepping downwards
			if (updatingMinMax)
				return;
			long value = Convert.ToInt64(nmMinValue.Value);

			switch (RTCSpec[RTCSPEC.CORE_CURRENTPRECISION.ToString()])
			{
				case 1:
					RTCSpec.Update(RTCSPEC.CUSTOM_MINVALUE8BIT.ToString(), value);
					break;
				case 2:
					RTCSpec.Update(RTCSPEC.CUSTOM_MINVALUE16BIT.ToString(), value);
					break;
				case 4:
					RTCSpec.Update(RTCSPEC.CUSTOM_MINVALUE32BIT.ToString(), value);
					break;
			}
		}

		private void nmMaxValue_ValueChanged(object sender, EventArgs e)
		{
			//We don't want to trigger this if it caps when stepping downwards
			if (updatingMinMax)
				return;
			long value = Convert.ToInt64(nmMaxValue.Value);


			switch (RTCSpec[RTCSPEC.CORE_CURRENTPRECISION.ToString()])
			{
				case 1:
					RTCSpec.Update(RTCSPEC.CUSTOM_MAXVALUE8BIT.ToString(), value);
					break;
				case 2:
					RTCSpec.Update(RTCSPEC.CUSTOM_MAXVALUE16BIT.ToString(), value);
					break;
				case 4:
					RTCSpec.Update(RTCSPEC.CUSTOM_MAXVALUE32BIT.ToString(), value);
					break;
			}
		}


		private void cbLockUnits_CheckedChanged(object sender, EventArgs e)
		{
			RTC_Unispec.RTCSpec.Update(RTCSPEC.STEP_LOCKEXECUTION.ToString(), cbLockUnits.Checked);
		}

		private void cbClearRewind_CheckedChanged(object sender, EventArgs e)
		{
			RTCSpec.Update(RTCSPEC.CORE_CLEARSTEPACTIONSONREWIND.ToString(), cbClearRewind.Checked);
		}

		private void cbLoopUnit_CheckedChanged(object sender, EventArgs e)
		{
			RTCSpec.Update(RTCSPEC.CUSTOM_LOOP.ToString(), cbLoopUnit.Checked);
		}

		private void cbValueList_SelectedIndexChanged(object sender, EventArgs e)
		{
			RTCSpec.Update(RTCSPEC.CUSTOM_VALUELISTHASH.ToString(), (string)cbValueList.SelectedValue);
		}
		private void cbLimiterList_SelectedIndexChanged(object sender, EventArgs e)
		{
			RTCSpec.Update(RTCSPEC.CUSTOM_LIMITERLISTHASH.ToString(), (string)cbLimiterList.SelectedValue);
		}
		private void limiterTime_CheckedChanged(object sender, EventArgs e)
		{
			if (rbLimiterNone.Checked)
				RTCSpec.Update(RTCSPEC.CUSTOM_LIMITERTIME.ToString(), ActionTime.NONE);

			else if (rbLimiterGenerate.Checked)
				RTCSpec.Update(RTCSPEC.CUSTOM_LIMITERTIME.ToString(), ActionTime.IMMEDIATE);

			else if (rbLimiterFirstExecute.Checked)
				RTCSpec.Update(RTCSPEC.CUSTOM_LIMITERTIME.ToString(), ActionTime.PREEXECUTE);

			else if (rbLimiterExecute.Checked)
				RTCSpec.Update(RTCSPEC.CUSTOM_LIMITERTIME.ToString(), ActionTime.EXECUTE);
		}
		
		private void btnClearActive_Click(object sender, EventArgs e)
		{
			//Netcore redundant method
			RTC_StepActions.ClearStepBlastUnits();
		}

		private void nmLifetime_ValueChanged(object sender, EventArgs e)
		{
			RTC_Unispec.RTCSpec.Update(RTCSPEC.CUSTOM_LIFETIME.ToString(), Convert.ToInt32(nmLifetime.Value));
		}

		private void nmDelay_ValueChanged(object sender, EventArgs e)
		{
			RTC_Unispec.RTCSpec.Update(RTCSPEC.CUSTOM_DELAY.ToString(), Convert.ToInt32(nmDelay.Value));
		}

		private void nmTilt_ValueChanged(object sender, EventArgs e)
		{
			RTC_Unispec.RTCSpec.Update(RTCSPEC.CUSTOM_TILTVALUE.ToString(), (BigInteger)nmTilt.Value);
		}

		public void UpdateMinMaxBoxes(int precision)
		{
			updatingMinMax = true;
			switch (precision)
			{
				case 1:
					nmMinValue.Maximum = byte.MaxValue;
					nmMaxValue.Maximum = byte.MaxValue;

					nmMinValue.Value = (long)RTC_Unispec.RTCSpec[RTCSPEC.CUSTOM_MINVALUE8BIT.ToString()];
					nmMaxValue.Value = (long)RTC_Unispec.RTCSpec[RTCSPEC.CUSTOM_MAXVALUE8BIT.ToString()];
					break;

				case 2:
					nmMinValue.Maximum = UInt16.MaxValue;
					nmMaxValue.Maximum = UInt16.MaxValue;
									   
					nmMinValue.Value = (long)RTC_Unispec.RTCSpec[RTCSPEC.CUSTOM_MINVALUE16BIT.ToString()];
					nmMaxValue.Value = (long)RTC_Unispec.RTCSpec[RTCSPEC.CUSTOM_MAXVALUE16BIT.ToString()];
					break;
				case 4:
					nmMinValue.Maximum = UInt32.MaxValue;
					nmMaxValue.Maximum = UInt32.MaxValue;

					nmMinValue.Value = (long)RTC_Unispec.RTCSpec[RTCSPEC.CUSTOM_MINVALUE32BIT.ToString()];
					nmMaxValue.Value = (long)RTC_Unispec.RTCSpec[RTCSPEC.CUSTOM_MAXVALUE32BIT.ToString()];

					break;
			}
			updatingMinMax = false;
		}

		private void cbLimiterInverted_CheckedChanged(object sender, EventArgs e)
		{
			RTC_Unispec.RTCSpec.Update(RTCSPEC.CUSTOM_LIMITERINVERTED.ToString(), cbLimiterInverted.Checked);
		}
	}
}
