using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace WindowsGlitchHarvester
{

    public static class WGH_Core
    {
		public static string WghVersion = "0.93b";

		public static Random RND = new Random();

        //Values
        public static bool isLoaded = false;

        public static CorruptionEngine selectedEngine = CorruptionEngine.NIGHTMARE;

        public static int Intensity = 100;
        public static int StartingAddress = 0;
        public static int BlastRange = 0;
        public static bool useBlastRange = false;
        public static string ProcessHookName = "";

        public static bool ExtractBlastLayer = false;
        public static string lastOpenTarget = null;

        //General Values
        public static string currentDir = System.IO.Directory.GetCurrentDirectory();
        public static string currentTargetType = "";
        public static string currentTargetName = "";
        public static string currentTargetFullName = "";
        public static string currentTargetId = "";

        public static bool writeCopyMode = false;
        public static bool AutoUncorrupt = true;

        //Forms
        public static WGH_MainForm ghForm;
        public static WGH_SelectMultipleForm smForm = null;
		public static WGH_HookProcessForm hpForm = null;
		public static WGH_AutoCorruptForm acForm = null;
        public static WGH_BlastEditorForm beForm = null;
        public static WGH_SavestateInfoForm ssForm = null;

        //object references
        public static MemoryInterface currentMemoryInterface = null;
        public static Stockpile currentStockpile = null;
        public static StashKey currentStashkey = null;
        public static BlastLayer lastBlastLayerBackup = null;

		public static int ErrorDelay = 100;

		public static void Start(WGH_MainForm _ghForm)
        {

            bool Expires = false;
            DateTime ExpiringDate = DateTime.Parse("2015-01-02");

            if (Expires && DateTime.Now > ExpiringDate)
            {
                MessageBox.Show("This version has expired");
                Application.Exit();
                return;
            }

            ghForm = _ghForm;
			acForm = new WGH_AutoCorruptForm();
			acForm.TopLevel = false;
			ghForm.pnBottom.Controls.Add(acForm);
			acForm.Anchor = AnchorStyles.Left;
			acForm.Location = new Point(240, 0);

			acForm.Show();
			
			acForm.BringToFront();
			acForm.Visible = false;

            if (!Directory.Exists(WGH_Core.currentDir + "\\TEMP\\"))
                Directory.CreateDirectory(WGH_Core.currentDir + "\\TEMP\\");

            if (!Directory.Exists(WGH_Core.currentDir + "\\PARAMS\\"))
                Directory.CreateDirectory(WGH_Core.currentDir + "\\PARAMS\\");


            if (File.Exists(currentDir + "\\PARAMS\\COLOR.TXT"))
			{
				string[] bytes = File.ReadAllText(currentDir + "\\PARAMS\\COLOR.TXT").Split(',');
				SetWGHColor(Color.FromArgb(Convert.ToByte(bytes[0]), Convert.ToByte(bytes[1]), Convert.ToByte(bytes[2])));
			}
			else
				SetWGHColor(Color.FromArgb(127, 120, 165));

		}

        public static long RandomLong(long max)
        {
            return RND.RandomLong(0, max);
        }


		public static BlastUnit getBlastUnit(string _target, long _address)
        {

            BlastUnit bu = null;

            switch (selectedEngine)
            {
                case CorruptionEngine.NIGHTMARE:
                    bu = WGH_NightmareEngine.GenerateUnit(_target, _address);
                    break;
                case CorruptionEngine.VECTOR:
                    bu = WGH_VectorEngine.GenerateUnit(_target, _address);
                    break;

                case CorruptionEngine.NONE:
                    return null;
            }

            return bu;
        }

        //Generates or queries a blast layer then applies it.
        public static BlastLayer Blast(BlastLayer _layer)
        {
            try
            {
                if (_layer != null)
                {
                    _layer.Apply(); //If the BlastLayer was provided, there's no need to generate a new one.

                    currentMemoryInterface.ApplyWorkingFile();

                    return _layer;
                }
                else
                {
                    BlastLayer bl = new BlastLayer();

                    string TargetType;
                    long StartingAddress;
                    long BlastRange;
                    long MaxAddress;
                    long RandomAdress = 0;
                    BlastUnit bu;

                    TargetType = currentTargetType;
                    StartingAddress = WGH_Core.StartingAddress;
                    BlastRange = WGH_Core.BlastRange;
                    MaxAddress = (long)(currentMemoryInterface.lastMemorySize ?? 0 );

                    if (!WGH_Core.useBlastRange)
                        BlastRange = MaxAddress - StartingAddress;
                    else if (StartingAddress + BlastRange > MaxAddress)
                        BlastRange = MaxAddress - StartingAddress;



                    for (int i = 0; i < Intensity; i++)
                    {
                        RandomAdress = StartingAddress + RandomLong(BlastRange -1);

                        bu = getBlastUnit(TargetType, RandomAdress);
                        if (bu != null)
                            bl.Layer.Add(bu);
                    }

                    bl.Apply();

                    currentMemoryInterface.ApplyWorkingFile();

                    if (bl.Layer.Count == 0)
                        return null;
                    else
                        return bl;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong in the WGH Core. \n\n" +
                ex.ToString());
                return null;
            }
        }

        public static BlastLayer Blast()
        {
            return Blast(null);
        }

        public static string GetRandomKey()
        {
            string Key = RND.Next(1, 9999).ToString() + RND.Next(1, 9999).ToString() + RND.Next(1, 9999).ToString() + RND.Next(1, 9999).ToString();
            return Key;
        }

        public static void LoadTarget()
        {

            if (WGH_Core.ghForm.rbTargetFile.Checked)
            {
                OpenFileDialog OpenFileDialog1;
                LoadTargetAgain:
                OpenFileDialog1 = new OpenFileDialog();

                OpenFileDialog1.Title = "Open File";
                OpenFileDialog1.Filter = "files|*.*";
                OpenFileDialog1.RestoreDirectory = true;
                if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if(OpenFileDialog1.FileName.ToString().Contains('^'))
                    {
                        MessageBox.Show("You can't use a file that contains the character ^ ");
                        goto LoadTargetAgain;
                    }

                    currentTargetId = "File|" + OpenFileDialog1.FileName.ToString();
                    currentTargetFullName = OpenFileDialog1.FileName.ToString();
                }
                else
                    return;

                if (currentMemoryInterface != null && (currentTargetType == "File" || currentTargetType == "MultipleFiles"))
                    WGH_Core.RestoreTarget();


                //Disable caching of the previously loaded file if it was enabled
                if (ghForm.btnEnableCaching.Text == "Disable caching on current target")
                    ghForm.btnEnableCaching.PerformClick();

                currentTargetType = "File";
                var fi = new FileInterface(currentTargetId);
                currentTargetName = fi.ShortFilename;

                currentMemoryInterface = fi;
                ghForm.lbTarget.Text = currentTargetId + "|MemorySize:" + fi.lastMemorySize.ToString();


            }
            else if (WGH_Core.ghForm.rbTargetMultipleFiles.Checked)
            {
                if(smForm != null)
                   smForm.Close();

                smForm = new WGH_SelectMultipleForm();

                if (smForm.ShowDialog() != DialogResult.OK)
                {
                    WGH_Core.currentMemoryInterface = null;
                    return;
                }

                currentTargetType = "MultipleFiles";
                var mfi = (MultipleFileInterface)WGH_Core.currentMemoryInterface;
                currentTargetName = mfi.ShortFilename;
                ghForm.lbTarget.Text = mfi.ShortFilename + "|MemorySize:" + mfi.lastMemorySize.ToString();
            }
            else if (WGH_Core.ghForm.rbTargetProcess.Checked)
            {
				if (hpForm != null)
					hpForm.Close();

				hpForm = new WGH_HookProcessForm();

				if (hpForm.ShowDialog() != DialogResult.OK)
				{
					WGH_Core.currentMemoryInterface = null;
					return;
				}

				currentTargetType = "Process";
				var mfi = (ProcessInterface)WGH_Core.currentMemoryInterface;
				currentTargetName = mfi.ProcessName;
				ghForm.lbTarget.Text = mfi.ProcessName + "|MemorySize:" + mfi.lastMemorySize.ToString();
			}
        }

        public static void RestoreTarget()
        {
            if (WGH_Core.AutoUncorrupt)
            {
                if (WGH_Core.lastBlastLayerBackup != null)
                    WGH_Core.lastBlastLayerBackup.Apply();
                else
                {
                    //CHECK CRC WITH BACKUP HERE AND SKIP BACKUP IF WORKING FILE = BACKUP FILE
                    WGH_Core.currentMemoryInterface.ResetWorkingFile();
                }
            }
            else
            {
                WGH_Core.currentMemoryInterface.ResetWorkingFile();
            }
        }


		/// <summary>
		/// Creates color with corrected brightness.
		/// </summary>
		/// <param name="color">Color to correct.</param>
		/// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
		/// Negative values produce darker colors.</param>
		/// <returns>
		/// Corrected <see cref="Color"/> structure.
		/// </returns>
		public static Color ChangeColorBrightness(Color color, float correctionFactor)
		{
			float red = (float)color.R;
			float green = (float)color.G;
			float blue = (float)color.B;

			if (correctionFactor < 0)
			{
				correctionFactor = 1 + correctionFactor;
				red *= correctionFactor;
				green *= correctionFactor;
				blue *= correctionFactor;
			}
			else
			{
				red = (255 - red) * correctionFactor + red;
				green = (255 - green) * correctionFactor + green;
				blue = (255 - blue) * correctionFactor + blue;
			}

			return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
		}

		private static List<Control> FindTag(Control.ControlCollection controls)
		{
			List<Control> allControls = new List<Control>();

			foreach (Control c in controls)
			{
				if (c.Tag != null)
					allControls.Add(c);

				if (c.HasChildren)
					allControls.AddRange(FindTag(c.Controls)); //Recursively check all children controls as well; ie groupboxes or tabpages
			}

			return allControls;
		}

		public static void SetWGHColor(Color color, Form form = null)
		{
			List<Control> allControls = new List<Control>();

			if (form == null)
			{
				if (ghForm != null)
				{
					allControls.AddRange(FindTag(ghForm.Controls));
					allControls.Add(ghForm);
				}

				
				if (acForm != null)
				{
					allControls.AddRange(FindTag(acForm.Controls));
					allControls.Add(acForm);
				}
				

			}
			else
				allControls.AddRange(FindTag(form.Controls));

			var lightColorControls = allControls.FindAll(it => (it.Tag as string).Contains("color:light"));
			var normalColorControls = allControls.FindAll(it => (it.Tag as string).Contains("color:normal"));
			var darkColorControls = allControls.FindAll(it => (it.Tag as string).Contains("color:dark"));
			var darkerColorControls = allControls.FindAll(it => (it.Tag as string).Contains("color:darker"));

			foreach (Control c in lightColorControls)
				c.BackColor = ChangeColorBrightness(color, 0.30f);

			foreach (Control c in normalColorControls)
				c.BackColor = color;

			//spForm.dgvStockpile.BackgroundColor = color;
			//ghForm.dgvStockpile.BackgroundColor = color;

			foreach (Control c in darkColorControls)
				c.BackColor = ChangeColorBrightness(color, -0.30f);

			foreach (Control c in darkerColorControls)
				c.BackColor = ChangeColorBrightness(color, -0.75f);

		}

		public static void SetAndSaveColorWGH()
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

			SetWGHColor(color);

			if (File.Exists(currentDir + "\\PARAMS\\COLOR.TXT"))
				File.Delete(currentDir + "\\PARAMS\\COLOR.TXT");

			File.WriteAllText(currentDir + "\\PARAMS\\COLOR.TXT", color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString());
		}

	}

	static class RandomExtensions
	{
		public static long RandomLong(this Random rnd)
		{
			byte[] buffer = new byte[8];
			rnd.NextBytes(buffer);
			return BitConverter.ToInt64(buffer, 0);
		}

		public static long RandomLong(this Random rnd, long min, long max)
		{
			EnsureMinLEQMax(ref min, ref max);
			long numbersInRange = unchecked(max - min + 1);
			if (numbersInRange < 0)
				throw new ArgumentException("Size of range between min and max must be less than or equal to Int64.MaxValue");

			long randomOffset = RandomLong(rnd);
			if (IsModuloBiased(randomOffset, numbersInRange))
				return RandomLong(rnd, min, max); // Try again
			else
				return min + PositiveModuloOrZero(randomOffset, numbersInRange);
		}

		static bool IsModuloBiased(long randomOffset, long numbersInRange)
		{
			long greatestCompleteRange = numbersInRange * (long.MaxValue / numbersInRange);
			return randomOffset > greatestCompleteRange;
		}

		static long PositiveModuloOrZero(long dividend, long divisor)
		{
			long mod;
			Math.DivRem(dividend, divisor, out mod);
			if (mod < 0)
				mod += divisor;
			return mod;
		}

		static void EnsureMinLEQMax(ref long min, ref long max)
		{
			if (min <= max)
				return;
			long temp = min;
			min = max;
			max = temp;
		}
	}
}
