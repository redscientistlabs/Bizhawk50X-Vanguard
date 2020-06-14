﻿using System;
using System.Drawing;
using System.Windows.Forms;
using BizHawk.Emulation.Common;

namespace BizHawk.Client.EmuHawk
{
	public partial class VirtualPadAnalogButton : UserControl, IVirtualPadControl
	{
		private string _displayName = "";
		private int _maxValue, _minValue;
		private bool _programmaticallyChangingValue;
		private bool _readonly;

		private bool _isSet = false;
		private bool IsSet
		{
			get => _isSet;
			set
			{
				_isSet = value;
				ValueLabel.ForeColor = DisplayNameLabel.ForeColor = _isSet ? SystemColors.HotTrack : SystemColors.WindowText;
			}
		}

		public VirtualPadAnalogButton()
		{
			InitializeComponent();
		}

		public void UpdateValues()
		{
			if (AnalogTrackBar.Value != (int)GlobalWin.InputManager.StickyXorAdapter.AxisValue(Name))
			{
				RefreshWidgets();
			}
		}

		public void Clear()
		{
			GlobalWin.InputManager.StickyXorAdapter.Unset(Name);
			IsSet = false;
		}

		public void Set(IController controller)
		{
			var newVal = (int)controller.AxisValue(Name);
			var changed = AnalogTrackBar.Value != newVal;
			if (changed)
			{
				CurrentValue = newVal;
			}
		}

		public bool ReadOnly
		{
			get => _readonly;
			set
			{
				if (_readonly != value)
				{
					AnalogTrackBar.Enabled =
						DisplayNameLabel.Enabled =
						ValueLabel.Enabled =
						!value;

					_readonly = value;
				
					Refresh();
				}
			}
		}

		public void Bump(int? x)
		{
			if (x.HasValue)
			{
				CurrentValue += x.Value;
			}
		}

		private void VirtualPadAnalogButton_Load(object sender, EventArgs e)
		{
			DisplayNameLabel.Text = DisplayName;
			ValueLabel.Text = AnalogTrackBar.Value.ToString();
			
		}

		public string DisplayName
		{
			get => _displayName;
			set
			{
				_displayName = value ?? "";
				if (DisplayNameLabel != null)
				{
					DisplayNameLabel.Text = _displayName;
				}
			}
		}

		public int MaxValue
		{
			get => _maxValue;

			set
			{
				_maxValue = value;
				if (AnalogTrackBar != null)
				{
					AnalogTrackBar.Maximum = _maxValue;
					UpdateTickFrequency();
				}
			}
		}

		public int MinValue
		{
			get => _minValue;

			set
			{
				_minValue = value;
				if (AnalogTrackBar != null)
				{
					AnalogTrackBar.Minimum = _minValue;
					UpdateTickFrequency();
				}
			}
		}

		public Orientation Orientation
		{
			get => AnalogTrackBar.Orientation;

			set
			{
				AnalogTrackBar.Orientation = value;
				if (value == Orientation.Horizontal)
				{
					AnalogTrackBar.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
					AnalogTrackBar.Size = new Size(Size.Width - 15, Size.Height - 15);
				}
				else if (value == Orientation.Vertical)
				{
					AnalogTrackBar.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
					AnalogTrackBar.Size = new Size(Size.Width - 15, Size.Height - 30);
					ValueLabel.Top = Size.Height / 2;
				}
			}
		}

		private void UpdateTickFrequency()
		{
			if (AnalogTrackBar == null)
			{
				return;
			}

			// try to base it on the width, lets make a tick every 10 pixels at the minimum
			int canDoTicks = AnalogTrackBar.Width / 10;
			if (canDoTicks < 2)
			{
				canDoTicks = 2;
			}

			int range = _maxValue - _minValue + 1;
			if (range < canDoTicks)
			{
				canDoTicks = range;
			}

			if (canDoTicks <= 0)
			{
				canDoTicks = 1;
			}

			AnalogTrackBar.TickFrequency = range / canDoTicks;
		}

		public int CurrentValue
		{
			get => AnalogTrackBar.Value;
			set
			{
				int val;
				if (value > AnalogTrackBar.Maximum)
				{
					val = AnalogTrackBar.Maximum;
				}
				else if (value < AnalogTrackBar.Minimum)
				{
					val = AnalogTrackBar.Minimum;
				}
				else
				{
					val = value;
				}

				IsSet = true;

				_programmaticallyChangingValue = true;
				AnalogTrackBar.Value = val;
				ValueLabel.Text = AnalogTrackBar.Value.ToString();
				_programmaticallyChangingValue = false;
			}
		}

		private void AnalogTrackBar_ValueChanged(object sender, EventArgs e)
		{
			if (!_programmaticallyChangingValue)
			{
				CurrentValue = AnalogTrackBar.Value;
				GlobalWin.InputManager.StickyXorAdapter.SetAxis(Name, AnalogTrackBar.Value);
			}
		}

		private void RefreshWidgets()
		{
			if (!_isSet)
			{
				_programmaticallyChangingValue = true;
				AnalogTrackBar.Value = (int)GlobalWin.InputManager.StickyXorAdapter.AxisValue(Name);
				ValueLabel.Text = AnalogTrackBar.Value.ToString();
				_programmaticallyChangingValue = false;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			RefreshWidgets();
			base.OnPaint(e);
		}
	}
}