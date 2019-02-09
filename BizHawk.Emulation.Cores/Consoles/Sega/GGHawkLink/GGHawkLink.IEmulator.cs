﻿using BizHawk.Common.NumberExtensions;
using BizHawk.Emulation.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using BizHawk.Emulation.Cores.Sega.MasterSystem;

namespace BizHawk.Emulation.Cores.Sega.GGHawkLink
{
	public partial class GGHawkLink : IEmulator, IVideoProvider, ISoundProvider
	{
		public IEmulatorServiceProvider ServiceProvider { get; }

		public ControllerDefinition ControllerDefinition => _controllerDeck.Definition;

		public bool FrameAdvance(IController controller, bool render, bool rendersound)
		{
			//Console.WriteLine("-----------------------FRAME-----------------------");
			if (_tracer.Enabled)
			{
				L.Cpu.TraceCallback = s => _tracer.Put(s);
			}
			else
			{
				L.Cpu.TraceCallback = null;
			}

			_frame++;

			if (controller.IsPressed("Power"))
			{
				HardReset();
			}

			bool cablediscosignalNew = controller.IsPressed("Toggle Cable");
			if (cablediscosignalNew && !_cablediscosignal)
			{
				_cableconnected ^= true;
				Console.WriteLine("Cable connect status to {0}", _cableconnected);
				LinkConnected = _cableconnected;
			}

			_cablediscosignal = cablediscosignalNew;

			_islag = true;

			GetControllerState(controller);

			do_frame(controller, render, rendersound);

			_islag = L._isLag;

			if (_islag)
			{
				_lagcount++;
			}

			return true;
		}

		public void do_frame(IController controller, bool render, bool rendersound)
		{
			/*
			L.do_controller_check();
			R.do_controller_check();
			
			// advance one full frame
			for (int i = 0; i < 70224; i++)
			{
				L.do_single_step();
				R.do_single_step();

				// the signal to shift out a bit is when serial_clock = 1
				if (((L.serialport.serial_clock == 1) || (L.serialport.serial_clock == 2)) && !do_r_next)
				{
					if (LinkConnected)
					{
						L.serialport.send_external_bit((byte)(L.serialport.serial_data & 0x80));

						if ((R.serialport.clk_rate == -1) && R.serialport.serial_start)
						{
							R.serialport.serial_clock = L.serialport.serial_clock;
							R.serialport.send_external_bit((byte)(R.serialport.serial_data & 0x80));
							R.serialport.coming_in = L.serialport.going_out;
						}

						L.serialport.coming_in = R.serialport.going_out;
					}
				}
				else if ((R.serialport.serial_clock == 1) || (R.serialport.serial_clock == 2))
				{
					do_r_next = false;

					if (LinkConnected)
					{
						R.serialport.send_external_bit((byte)(R.serialport.serial_data & 0x80));

						if ((L.serialport.clk_rate == -1) && L.serialport.serial_start)
						{
							L.serialport.serial_clock = R.serialport.serial_clock;
							L.serialport.send_external_bit((byte)(L.serialport.serial_data & 0x80));
							L.serialport.coming_in = R.serialport.going_out;
						}

						R.serialport.coming_in = L.serialport.going_out;
					}

					if (R.serialport.serial_clock == 2) { do_r_next = true; }
				}
				else
				{
					do_r_next = false;
				}

				// if we hit a frame boundary, update video
				if (L.vblank_rise)
				{
					buff_L = L.GetVideoBuffer();
					L.vblank_rise = false;
					FillVideoBuffer();
				}
				if (R.vblank_rise)
				{
					buff_R = R.GetVideoBuffer();
					R.vblank_rise = false;
					FillVideoBuffer();
				}
			}	
			*/
			L.FrameAdvance(controller, render, rendersound);
			R.FrameAdvance(controller, render, rendersound);

			buff_L = L.Vdp.GetVideoBuffer();
			buff_R = R.Vdp.GetVideoBuffer();

			FillVideoBuffer();

		}

		public void GetControllerState(IController controller)
		{
			InputCallbacks.Call();
			//L.controller_state = _controllerDeck.ReadPort1(controller);
			//R.controller_state = _controllerDeck.ReadPort2(controller);
		}

		public int Frame => _frame;

		public string SystemId => "DGB";

		public bool DeterministicEmulation { get; set; }

		public void ResetCounters()
		{
			_frame = 0;
			_lagcount = 0;
			_islag = false;
		}

		public CoreComm CoreComm { get; }

		public void Dispose()
		{
			L.Dispose();
			R.Dispose();
		}

		#region Video provider

		public int _frameHz = 60;

		public int[] _vidbuffer = new int[160 * 2 * 144];
		public int[] buff_L = new int[160 * 144];
		public int[] buff_R = new int[160 * 144];

		public int[] GetVideoBuffer()
		{
			return _vidbuffer;		
		}

		public void FillVideoBuffer()
		{
			// combine the 2 video buffers from the instances
			for (int i = 0; i < 144; i++)
			{
				for (int j = 0; j < 160; j++)
				{
					_vidbuffer[i * 320 + j] = buff_L[i * 160 + j];
					_vidbuffer[i * 320 + j + 160] = buff_R[i * 160 + j];
				}
			}
		}

		public int VirtualWidth => 160 * 2;
		public int VirtualHeight => 144;
		public int BufferWidth => 160 * 2;
		public int BufferHeight => 144;
		public int BackgroundColor => unchecked((int)0xFF000000);
		public int VsyncNumerator => _frameHz;
		public int VsyncDenominator => 1;

		public static readonly uint[] color_palette_BW = { 0xFFFFFFFF , 0xFFAAAAAA, 0xFF555555, 0xFF000000 };
		public static readonly uint[] color_palette_Gr = { 0xFFA4C505, 0xFF88A905, 0xFF1D551D, 0xFF052505 };

		public uint[] color_palette = new uint[4];

		#endregion

		#region audio

		public bool CanProvideAsync => false;

		public void SetSyncMode(SyncSoundMode mode)
		{
			if (mode != SyncSoundMode.Sync)
			{
				throw new InvalidOperationException("Only Sync mode is supported_");
			}
		}

		public SyncSoundMode SyncMode => SyncSoundMode.Sync;

		public void GetSamplesSync(out short[] samples, out int nsamp)
		{
			short[] temp_samp_L = new short[735 * 2];
			short[] temp_samp_R = new short[735 * 2];

			int nsamp_L = 735;
			int nsamp_R = 735;

			L.PSG.GetSamples(temp_samp_L);
			R.PSG.GetSamples(temp_samp_R);

			if (linkSettings.AudioSet == GGLinkSettings.AudioSrc.Left)
			{
				samples = temp_samp_L;
				nsamp = nsamp_L;
			}
			else if (linkSettings.AudioSet == GGLinkSettings.AudioSrc.Right)
			{
				samples = temp_samp_R;
				nsamp = nsamp_R;
			}
			else
			{
				samples = new short[0];
				nsamp = 0;
			}
		}

		public void GetSamplesAsync(short[] samples)
		{
			throw new NotSupportedException("Async is not available");
		}

		public void DiscardSamples()
		{
			L.PSG.DiscardSamples();
			R.PSG.DiscardSamples();
		}

		private void GetSamples(short[] samples)
		{

		}

		public void DisposeSound()
		{

		}

		#endregion
	}
}
