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
	public partial class RTC_GeneralParameters_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

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

				if (S.GET<RTC_GlitchHarvester_Form>().nmIntensity.Value != RTC_Core.Intensity)
					S.GET<RTC_GlitchHarvester_Form>().nmIntensity.Value = RTC_Core.Intensity;

				int fx = Convert.ToInt32(Math.Sqrt(value) * 2000d);

				if (track_Intensity.Value != fx)
					track_Intensity.Value = fx;

				if (S.GET<RTC_GlitchHarvester_Form>().track_Intensity.Value != fx)
					S.GET<RTC_GlitchHarvester_Form>().track_Intensity.Value = fx;

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

		public RTC_GeneralParameters_Form()
		{
			InitializeComponent();
		}

		private void RTC_GeneralParameters_Form_Load(object sender, EventArgs e)
		{
			cbBlastRadius.SelectedIndex = 0;
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


		Guid? errorDelayToken = null;
		Guid? intensityToken = null;

		private void track_ErrorDelay_MouseDown(object sender, MouseEventArgs e)
		{
			errorDelayToken = RTC_NetCore.HugeOperationStart("LAZY");
		}

		private void track_ErrorDelay_MouseUp(object sender, MouseEventArgs e)
		{
			RTC_NetCore.HugeOperationEnd(errorDelayToken);

			track_ErrorDelay_Scroll(sender, e);
		}

		private void track_Intensity_MouseDown(object sender, MouseEventArgs e)
		{
			intensityToken = RTC_NetCore.HugeOperationStart("LAZY");
		}

		private void track_Intensity_MouseUp(object sender, MouseEventArgs e)
		{
			RTC_NetCore.HugeOperationEnd(intensityToken);

			track_Intensity_Scroll(sender, e);
		}

		private void RTC_GeneralParameters_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
				e.Cancel = true;
				this.RestoreToPreviousPanel();
				return;
			}
		}

	}
}
