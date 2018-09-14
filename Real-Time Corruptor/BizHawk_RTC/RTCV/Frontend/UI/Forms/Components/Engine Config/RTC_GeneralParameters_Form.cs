using RTCV.CorruptCore;
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
				return RTC_CorruptCore.Intensity;
			}
			set
			{
				if (DontUpdateIntensity)
					return;

				RTC_CorruptCore.Intensity = value;

				DontUpdateIntensity = true;

				if (nmIntensity.Value != RTC_CorruptCore.Intensity)
					nmIntensity.Value = RTC_CorruptCore.Intensity;

				if (S.GET<RTC_GlitchHarvester_Form>().nmIntensity.Value != RTC_CorruptCore.Intensity)
					S.GET<RTC_GlitchHarvester_Form>().nmIntensity.Value = RTC_CorruptCore.Intensity;

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
				return RTC_CorruptCore.ErrorDelay;
			}
			set
			{
				if (DontUpdateErrorDelay)
					return;

				RTC_CorruptCore.ErrorDelay = Convert.ToInt32(value);

				DontUpdateErrorDelay = true;

				if (nmErrorDelay.Value != RTC_CorruptCore.ErrorDelay)
					nmErrorDelay.Value = RTC_CorruptCore.ErrorDelay;

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
					RTC_CorruptCore.Radius = BlastRadius.SPREAD;
					break;

				case "CHUNK":
					RTC_CorruptCore.Radius = BlastRadius.CHUNK;
					break;

				case "BURST":
					RTC_CorruptCore.Radius = BlastRadius.BURST;
					break;

				case "NORMALIZED":
					RTC_CorruptCore.Radius = BlastRadius.NORMALIZED;
					break;

				case "PROPORTIONAL":
					RTC_CorruptCore.Radius = BlastRadius.PROPORTIONAL;
					break;

				case "EVEN":
					RTC_CorruptCore.Radius = BlastRadius.EVEN;
					break;
			}

		}


		Guid? errorDelayToken = null;
		Guid? intensityToken = null;

		private void track_ErrorDelay_MouseDown(object sender, MouseEventArgs e)
		{

		}

		private void track_ErrorDelay_MouseUp(object sender, MouseEventArgs e)
		{

			track_ErrorDelay_Scroll(sender, e);
		}

		private void track_Intensity_MouseDown(object sender, MouseEventArgs e)
		{

		}

		private void track_Intensity_MouseUp(object sender, MouseEventArgs e)
		{

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
