using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using CorruptCore;
using NetCore;
using Newtonsoft.Json;
using RTCV.CorruptCore;
using RTCV.NetCore;
using RTCV.UI;
using UI;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
{
	public static class RTC_UICore
	{
		//Engine Values
		public static BindingList<ComboBoxItem<string>> LimiterListBindingSource = new BindingList<ComboBoxItem<string>>();
		public static BindingList<ComboBoxItem<string>> ValueListBindingSource = new BindingList<ComboBoxItem<string>>();


		//Note Box Settings
		public static Point NoteBoxPosition;
		public static Size NoteBoxSize;

		public static bool FirstConnect = true;
		public static SoundPlayer[] LoadedSounds = null;

		//RTC Main Forms
		//public static Color generalColor = Color.FromArgb(60, 45, 70);
		public static Color GeneralColor = Color.LightSteelBlue;

		public static void Start(RTC_Standalone_Form standaloneForm = null)
		{
			RTC_Extensions.DirectoryRequired(paths: new string[] {
				RTC_Corruptcore.workingDir, RTC_Corruptcore.workingDir + "\\TEMP\\"
				, RTC_Corruptcore.workingDir + "\\SKS\\", RTC_Corruptcore.workingDir + "\\SSK\\"
				, RTC_Corruptcore.workingDir + "\\SESSION\\", RTC_Corruptcore.workingDir + "\\MEMORYDUMPS\\"
				, RTC_Corruptcore.workingDir + "\\MP\\", RTC_Corruptcore.assetsDir + "\\CRASHSOUNDS\\"
				, RTC_Corruptcore.rtcDir + "\\PARAMS\\", RTC_Corruptcore.rtcDir + "\\LISTS\\"
				, RTC_Corruptcore.rtcDir + "\\RENDEROUTPUT\\",
			});

			S.SET<RTC_Standalone_Form>(standaloneForm);

			Form dummy = new Form();
			IntPtr Handle = dummy.Handle;

			SyncObjectSingleton.SyncObject = dummy;

			UI_VanguardImplementation.StartServer();


			PartialSpec p = new PartialSpec("UISpec");

			p["SELECTEDDOMAINS"] = new string[]{};

			RTC_Corruptcore.UISpec = new FullSpec(p);
			RTC_Corruptcore.UISpec.SpecUpdated += (o, e) =>
			{
				PartialSpec partial = e.partialSpec;

				LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHUISPECUPDATE, partial, true);
			};

			RTC_Corruptcore.Start();

			//Loading RTC Params
			LoadRTCColor();
			S.GET<RTC_SettingsGeneral_Form>().cbDisableBizhawkOSD.Checked = !RTC_Params.IsParamSet("ENABLE_BIZHAWK_OSD");
			S.GET<RTC_SettingsGeneral_Form>().cbAllowCrossCoreCorruption.Checked = RTC_Params.IsParamSet("ALLOW_CROSS_CORE_CORRUPTION");
			S.GET<RTC_SettingsGeneral_Form>().cbDontCleanAtQuit.Checked = RTC_Params.IsParamSet("DONT_CLEAN_SAVESTATES_AT_QUIT");


			S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_ConnectionStatus_Form>());
			S.GET<RTC_Core_Form>().Show();

		}

		//All RTC forms
		public static Form[] AllRtcForms
		{
			get
			{
				//This fetches all singletons of interface IAutoColorized

				List<Form> all = new List<Form>();

				foreach (Type t in Assembly.GetAssembly(typeof(S)).GetTypes())
					if (typeof(IAutoColorize).IsAssignableFrom(t) && t != typeof(IAutoColorize))
						all.Add((Form)S.GET(Type.GetType(t.ToString())));

				return all.ToArray();
				return all.ToArray();

			}
		}

		public static volatile bool isClosing = false;
		public static void CloseAllRtcForms() //This allows every form to get closed to prevent RTC from hanging
		{
			if (isClosing)
				return;

			isClosing = true;
			if(NetCoreServer.loopbackConnector != null)
				NetCoreServer.StopLoopback();


			foreach (Form frm in RTC_UICore.AllRtcForms)
			{
				if (frm != null)
					frm.Close();
			}

			if (S.GET<RTC_Standalone_Form>() != null)
				S.GET<RTC_Standalone_Form>().Close();

			//Clean out the working folders
			if (!RTC_Corruptcore.DontCleanSavestatesOnQuit)
			{
				Stockpile.EmptyFolder(Path.DirectorySeparatorChar + "WORKING" + Path.DirectorySeparatorChar);
			}

			Application.Exit();
		}

		public static void SetRTCHexadecimal(bool useHex, Form form = null)
		{
			//Sets the interface to use Hex across the board

			List<Control> allControls = new List<Control>();

			if (form == null)
			{
				foreach (Form targetForm in RTC_UICore.AllRtcForms)
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
			List<Control> allControls = new List<Control>();

			if (form == null)
			{
				foreach (Form targetForm in RTC_UICore.AllRtcForms)
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



			S.GET<RTC_NewBlastEditor_Form>().dgvBlastEditor.BackgroundColor = color.ChangeColorBrightness(-0.30f);
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

			SaveRTCColor(color);
		}

		public static void LoadRTCColor()
		{
				if (RTC_Params.IsParamSet("COLOR"))
				{
					string[] bytes = RTC_Params.ReadParam("COLOR").Split(',');
					RTC_UICore.GeneralColor = Color.FromArgb(Convert.ToByte(bytes[0]), Convert.ToByte(bytes[1]), Convert.ToByte(bytes[2]));
				}
				else
					RTC_UICore.GeneralColor = Color.FromArgb(110, 150, 193);

				RTC_UICore.SetRTCColor(RTC_UICore.GeneralColor);
		}

		public static void SaveRTCColor(Color color)
		{
			RTC_Params.SetParam("COLOR", color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString());
		}


		public static void PlayCrashSound(bool forcePlay = false)
		{
			if (LoadedSounds != null && (forcePlay || S.GET<RTC_ConnectionStatus_Form>().btnStartEmuhawkDetached.Text == "Restart BizHawk"))
				LoadedSounds[RTC_Corruptcore.RND.Next(LoadedSounds.Length)].Play();
		}
	}
}
