using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BizHawk.Client.EmuHawk;
using BizHawk.Client.Common;
using System.IO;
using BizHawk.Emulation.Common;
using System.Xml.Serialization;
using System.Diagnostics;

namespace RTC
{
    public partial class RTC_GlitchHarvester_Form : Form
    {
        public string[] btnParentKeys = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
        public string[] btnAttachedRom = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };

        public bool DontLoadSelectedStash = false;
        public bool DontLoadSelectedStockpile = false;

		public Panel pnHideGlitchHarvester = new Panel();
		public Label lbConnectionStatus = new Label();
        public Button btnEmergencySaveStockpile = new Button();

        Dictionary<string, TextBox> StateBoxes = new Dictionary<string, TextBox>();
        
        public bool IsCorruptionApplied
        {
            get
            {
                return RTC_StockpileManager.isCorruptionApplied;
            }
            set
            {
                if (value)
                {
                    btnBlastToggle.BackColor = Color.FromArgb(224,128,128);
                    btnBlastToggle.ForeColor = Color.Black;
                    btnBlastToggle.Text = "BlastLayer : ON";

					RTC_Core.spForm.btnBlastToggle.BackColor = Color.FromArgb(224, 128, 128);
					RTC_Core.spForm.btnBlastToggle.ForeColor = Color.Black;
					RTC_Core.spForm.btnBlastToggle.Text = "BlastLayer : ON     (Attempts to uncorrupt/recorrupt in real-time)";
				}
                else
                {
					btnBlastToggle.BackColor = RTC_Core.coreForm.btnLogo.BackColor;
                    btnBlastToggle.ForeColor = Color.White;
                    btnBlastToggle.Text = "BlastLayer : OFF";

					RTC_Core.spForm.btnBlastToggle.BackColor = RTC_Core.coreForm.btnLogo.BackColor;
					RTC_Core.spForm.btnBlastToggle.ForeColor = Color.White;
					RTC_Core.spForm.btnBlastToggle.Text = "BlastLayer : OFF    (Attempts to uncorrupt/recorrupt in real-time)";
				}

				if (RTC_StockpileManager.isCorruptionApplied != value)
					RTC_StockpileManager.isCorruptionApplied = value;
            }
        }

		public void RefreshNoteIcons(DataGridView dgv)
		{
			if (dgv == null)
				return;

			foreach (DataGridViewRow dataRow in dgv.Rows)
			{
				StashKey sk = (StashKey)dataRow.Cells["Item"].Value;
                if (sk.Note == null)
                {
                    dataRow.Cells["Note"].Value = "";
                }
                else
                {
                    dataRow.Cells["Note"].Value = "📝";
                }

			}
		}

		public RTC_GlitchHarvester_Form()
        {
            InitializeComponent();

            //btnSavestate_Click(btnSavestate01, null); //Selects first button as default

            #region textbox states to dico
            StateBoxes.Add("01", tbSavestate01);
			StateBoxes.Add("02", tbSavestate02);
			StateBoxes.Add("03", tbSavestate03);
			StateBoxes.Add("04", tbSavestate04);
			StateBoxes.Add("05", tbSavestate05);
			StateBoxes.Add("06", tbSavestate06);
			StateBoxes.Add("07", tbSavestate07);
			StateBoxes.Add("08", tbSavestate08);
			StateBoxes.Add("09", tbSavestate09);
			StateBoxes.Add("10", tbSavestate10);
			StateBoxes.Add("11", tbSavestate11);
			StateBoxes.Add("12", tbSavestate12);
			StateBoxes.Add("13", tbSavestate13);
			StateBoxes.Add("14", tbSavestate14);
			StateBoxes.Add("15", tbSavestate15);
			StateBoxes.Add("16", tbSavestate16);
			StateBoxes.Add("17", tbSavestate17);
			StateBoxes.Add("18", tbSavestate18);
			StateBoxes.Add("19", tbSavestate19);
			StateBoxes.Add("20", tbSavestate20);
			StateBoxes.Add("21", tbSavestate21);
			StateBoxes.Add("22", tbSavestate22);
			StateBoxes.Add("23", tbSavestate23);
			StateBoxes.Add("24", tbSavestate24);
			StateBoxes.Add("25", tbSavestate25);
			StateBoxes.Add("26", tbSavestate26);
			StateBoxes.Add("27", tbSavestate27);
			StateBoxes.Add("28", tbSavestate28);
			StateBoxes.Add("29", tbSavestate29);
			StateBoxes.Add("30", tbSavestate30);
			StateBoxes.Add("31", tbSavestate31);
			StateBoxes.Add("32", tbSavestate32);
			StateBoxes.Add("33", tbSavestate33);
			StateBoxes.Add("34", tbSavestate34);
			StateBoxes.Add("35", tbSavestate35);
			StateBoxes.Add("36", tbSavestate36);
			StateBoxes.Add("37", tbSavestate37);
			StateBoxes.Add("38", tbSavestate38);
			StateBoxes.Add("39", tbSavestate39);
			StateBoxes.Add("40", tbSavestate40);
			#endregion

			pnHideGlitchHarvester.Location = new Point(0, 0);
			pnHideGlitchHarvester.Size = this.Size;
			pnHideGlitchHarvester.BackColor = Color.Black;
			pnHideGlitchHarvester.Controls.Add(lbConnectionStatus);

			lbConnectionStatus.Location = new Point(32, 32);
			lbConnectionStatus.Size = new Size(500, 32);
			lbConnectionStatus.ForeColor = Color.FromArgb(255, 192, 128);

            btnEmergencySaveStockpile.Text = "Emergency Save Stockpile";
            btnEmergencySaveStockpile.Location = new Point(32, 64);
            btnEmergencySaveStockpile.Size = new Size(200, 32);
            btnEmergencySaveStockpile.BackColor = Color.OrangeRed;
            btnEmergencySaveStockpile.Click += (s,e)=>{
                btnSaveStockpileAs_Click(s,e);
            };
            pnHideGlitchHarvester.Controls.Add(btnEmergencySaveStockpile);

            Controls.Add(pnHideGlitchHarvester);
			pnHideGlitchHarvester.Hide();

			cbRenderType.SelectedIndex = 0;
		}

		private void RTC_GH_Form_Load(object sender, EventArgs e)
        {
            /*
			foreach (Control ctrl in pnSavestateHolder.Controls)
				if (ctrl is Button)
					ctrl.Size = new Size(29, 25);
            */

			RefreshStashHistory();
            refreshSavestateTextboxes();


        }

		public void RefreshStashHistory(bool scrolldown = false)
		{
			DontLoadSelectedStash = true;
			var lastSelect = lbStashHistory.SelectedIndex;

			DontLoadSelectedStash = true;
			lbStashHistory.DataSource = null;

			DontLoadSelectedStash = true;
            //lbStashHistory.BeginUpdate();
			lbStashHistory.DataSource = RTC_StockpileManager.StashHistory;
            //lbStashHistory.EndUpdate();

			DontLoadSelectedStash = true;
			if(lastSelect < lbStashHistory.Items.Count)
				lbStashHistory.SelectedIndex = lastSelect;

			DontLoadSelectedStash = false;

		}

        public void btnSavestate_Click(object sender, EventArgs e)
        {
			try
			{
				(sender as Button).Visible = false;

				foreach (var item in pnSavestateHolder.Controls)
					if (item is Button)
						(item as Button).ForeColor = Color.FromArgb(192, 255, 192);


				Button clickedButton = (sender as Button);
				clickedButton.ForeColor = Color.OrangeRed;
				clickedButton.BringToFront();


				RTC_StockpileManager.currentSavestateKey = clickedButton.Text;
                StashKey psk = RTC_StockpileManager.getCurrentSavestateStashkey();


                if (psk != null && !File.Exists(psk.RomFilename))
                {
                    if (DialogResult.Yes == MessageBox.Show($"Can't find file {psk.RomFilename}\nGame name: {psk.GameName}\nSystem name: {psk.SystemName}\n\n Would you like to provide a new file for replacement?", "Error: File not found", MessageBoxButtons.YesNo))
                    {
                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.DefaultExt = "*";
                        ofd.Title = "Select Replacement File";
                        ofd.Filter = "Any file|*.*";
                        ofd.RestoreDirectory = true;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            string Filename = ofd.FileName.ToString();
                            string oldFilename = psk.RomFilename;
                            for (int i = 1; i < 41; i++)
                            {
                                string key = i.ToString().PadLeft(2, '0');

                                if (RTC_StockpileManager.SavestateStashkeyDico.ContainsKey(key))
                                {
                                    StashKey sk = RTC_StockpileManager.SavestateStashkeyDico[key];
                                    if (sk.RomFilename == oldFilename)
                                        sk.RomFilename = Filename;
                                }

                            }
                        }
                        else
                        {
                            clickedButton.ForeColor = Color.FromArgb(192, 255, 192);
                            RTC_StockpileManager.currentSavestateKey = null;
                            return;
                        }
                    }
                    else
                    {
                        clickedButton.ForeColor = Color.FromArgb(192, 255, 192);
                        RTC_StockpileManager.currentSavestateKey = null;
                        return;
                    }
                }




                RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_SAVESTATEBOX) { objectValue = RTC_StockpileManager.currentSavestateKey });

				if (cbSavestateLoadOnClick.Checked)
                {
                    btnSaveLoad.Text = "LOAD";
                    btnSaveLoad_Click(null, null);
                }
					//RTC_StockpileManager.LoadState(RTC_StockpileManager.getCurrentSavestateStashkey());

			}
			finally
			{
				(sender as Button).Visible = true;
			}
		}

        private void btnToggleSaveLoad_Click(object sender, EventArgs e)
        {
            if (btnSaveLoad.Text == "LOAD")
            {
                btnSaveLoad.Text = "SAVE";
                btnSaveLoad.ForeColor = Color.OrangeRed;
            }
            else
            {
                btnSaveLoad.Text = "LOAD";
                btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);
            }

        }

        public void btnSaveLoad_Click(object sender, EventArgs e)
        {
            if (btnSaveLoad.Text == "LOAD")
            {
				StashKey psk = RTC_StockpileManager.getCurrentSavestateStashkey();
				if (psk != null)
				{
					if(!File.Exists(psk.RomFilename))
						if(DialogResult.Yes == MessageBox.Show($"Can't find file {psk.RomFilename}\nGame name: {psk.GameName}\nSystem name: {psk.SystemName}\n\n Would you like to provide a new file for replacement?","Error: File not found", MessageBoxButtons.YesNo))
						{
							OpenFileDialog ofd = new OpenFileDialog();
							ofd.DefaultExt = "*";
							ofd.Title = "Select Replacement File";
							ofd.Filter = "Any file|*.*";
							ofd.RestoreDirectory = true;
							if (ofd.ShowDialog() == DialogResult.OK)
							{
								string Filename = ofd.FileName.ToString();
								string oldFilename = psk.RomFilename;
								for (int i = 1; i<41; i++)
								{
									string key = i.ToString().PadLeft(2, '0');

									if (RTC_StockpileManager.SavestateStashkeyDico.ContainsKey(key))
									{
										StashKey sk = RTC_StockpileManager.SavestateStashkeyDico[key];
										if (sk.RomFilename == oldFilename)
											sk.RomFilename = Filename;
									}

								}
							}
							else
								return;
						}

                    var token = RTC_NetCore.HugeOperationStart("LAZY");

                    RTC_StockpileManager.LoadState(psk);

                    RTC_NetCore.HugeOperationEnd(token);

                }
				else
					MessageBox.Show("Savestate box is empty");
            }
            else
            {
                if(RTC_StockpileManager.currentSavestateKey == null)
                {
                    MessageBox.Show("No Savestate Box is currently selected in the Glitch Harvester's Savestate Manager");
                    return;
                }


				StashKey sk = RTC_StockpileManager.SaveState(true);

                btnSaveLoad.Text = "LOAD";
                btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);

                
            }

        }

		public void btnCorrupt_Click(object sender, EventArgs e)
		{
			if (!btnCorrupt.Visible)
				return;

			try
			{
				btnCorrupt.Visible = false;

				StashKey psk = RTC_StockpileManager.getCurrentSavestateStashkey();


				if (btnCorrupt.Text.ToUpper() == "MERGE")
				{
                    List<StashKey> sks = new List<StashKey>();

					foreach (DataGridViewRow row in dgvStockpile.SelectedRows)
						sks.Add((StashKey)row.Cells[0].Value);

					RTC_StockpileManager.MergeStashkeys(sks);

                    RefreshStashHistory();

					//lbStashHistory.TopIndex = lbStashHistory.Items.Count - 1;

					return;

				}


                var token = RTC_NetCore.HugeOperationStart("LAZY");

                if (rbCorrupt.Checked)
                {

                    var RomFilename = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETOPENROMFILENAME), true);
                    if(RomFilename.Contains("|"))
                    {
                        MessageBox.Show($"The Glitch Harvester attempted to corrupt a game bound to the following file:\n{RomFilename}\n\nIt cannot be processed because the rom seems to be inside a Zip Archive\n(Bizhawk returned a filename with the chracter | in it)");
                        return;
                    }

                    if (RTC_Core.SelectedEngine == CorruptionEngine.EXTERNALROM)
                    {
                        if (sender == null)
                            RTC_StockpileManager.Corrupt();
                        else
                            RTC_RPC.CorruptPlugin();
                    }
                    else
                        RTC_StockpileManager.Corrupt();
                }
				else if (rbInject.Checked)
					RTC_StockpileManager.InjectFromStashkey(RTC_StockpileManager.currentStashkey);
				else if (rbOriginal.Checked)
					RTC_StockpileManager.OriginalFromStashkey(RTC_StockpileManager.currentStashkey);


                RTC_NetCore.HugeOperationEnd(token);


                RefreshStashHistory();

			}
			finally
			{
				btnCorrupt.Visible = true;
			}
		}

        private void RTC_GH_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                RTC_Core.coreForm.btnGlitchHarvester.Text = RTC_Core.coreForm.btnGlitchHarvester.Text.Replace("○ ", "");
                e.Cancel = true;
                this.Hide();
            }

        }

        private void lbStashHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
			try
			{
				lbStashHistory.Enabled = false;
                btnStashUP.Enabled = false;
                btnStashDOWN.Enabled = false;

                if (DontLoadSelectedStash || lbStashHistory.SelectedIndex == -1)
				{
					DontLoadSelectedStash = false;
					return;
				}

				dgvStockpile.ClearSelection();
				RTC_Core.spForm.dgvStockpile.ClearSelection();

				if (!rbCorrupt.Checked && !rbInject.Checked && !rbOriginal.Checked)
					rbCorrupt.Checked = true;

                if(btnCorrupt.Text == "Merge")
                {
                    rbCorrupt.Enabled = true;
                    rbInject.Enabled = true;
                    rbOriginal.Enabled = true;
                    btnRenameSelected.Visible = true;
                    btnRemoveSelectedStockpile.Text = "Remove Item";

                    if (rbCorrupt.Checked)
                        btnCorrupt.Text = "Blast/Send";
                    else if (rbInject.Checked)
                        btnCorrupt.Text = "Inject";
                    else if (rbOriginal.Checked)
                        btnCorrupt.Text = "Original";

                }


                RTC_StockpileManager.currentStashkey = RTC_StockpileManager.StashHistory[lbStashHistory.SelectedIndex];

				if (!cbLoadOnSelect.Checked && RTC_StockpileManager.loadBeforeOperation)
					return;

                var token = RTC_NetCore.HugeOperationStart("LAZY");

                if (rbCorrupt.Checked)
					RTC_StockpileManager.ApplyStashkey(RTC_StockpileManager.currentStashkey);
				if (rbInject.Checked)
					RTC_StockpileManager.InjectFromStashkey(RTC_StockpileManager.currentStashkey);
				else if (rbOriginal.Checked)
					RTC_StockpileManager.OriginalFromStashkey(RTC_StockpileManager.currentStashkey);

                RTC_NetCore.HugeOperationEnd(token);

            }
			finally
			{
				lbStashHistory.Enabled = true;
                btnStashUP.Enabled = true;
                btnStashDOWN.Enabled = true;
			}
		}

        private void rbInject_CheckedChanged(object sender, EventArgs e)
        {
            if (rbInject.Checked)
                btnCorrupt.Text = "Inject";
        }

        private void rbCorrupt_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCorrupt.Checked)
                btnCorrupt.Text = "Blast/Send";
        }

        private void rbOriginal_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOriginal.Checked)
                btnCorrupt.Text = "Original";
        }

		private void btnAddStashToStockpile_Click(object sender, EventArgs e) => AddStashToStockpile();
        public void AddStashToStockpile(bool askForName = true)
        {
            if (lbStashHistory.Items.Count == 0 || lbStashHistory.SelectedIndex == -1)
            {
                MessageBox.Show("Can't add the Stash to the Stockpile because none is selected in the Stash History");
                return;
            }

            RTC_Core.StopSound();
            string Name = "";
            string value = "";
            if (askForName)
            {
                if (RTC_Extensions.getInputBox("Harvester", "Enter the new Stash name:", ref value) == DialogResult.OK)
                {
                    Name = value.Trim();
                    RTC_Core.StartSound();
                }
                else
                {
                    RTC_Core.StartSound();
                    return;
                }
            }

            RTC_StockpileManager.currentStashkey = (StashKey)lbStashHistory.SelectedItem;

            if (Name != "")
				RTC_StockpileManager.currentStashkey.Alias = Name;
            else
				RTC_StockpileManager.currentStashkey.Alias = RTC_StockpileManager.currentStashkey.Key;


			var sk = RTC_StockpileManager.currentStashkey;

            sk.BlastLayer.Rasterize();

			var dataRow = dgvStockpile.Rows[dgvStockpile.Rows.Add()];
			dataRow.Cells["Item"].Value = sk;
			dataRow.Cells["GameName"].Value = sk.GameName;
			dataRow.Cells["SystemName"].Value = sk.SystemName;
			dataRow.Cells["SystemCore"].Value = sk.SystemCore;

			RefreshNoteIcons(dgvStockpile);

			RTC_StockpileManager.StashHistory.Remove(sk);

			RefreshStashHistory();

			DontLoadSelectedStash = true;
			lbStashHistory.ClearSelected();
			DontLoadSelectedStash = false;

			int nRowIndex = dgvStockpile.Rows.Count - 1;

			dgvStockpile.ClearSelection();
			dgvStockpile.Rows[nRowIndex].Selected = true;

			RTC_StockpileManager.StockpileChanged();

            RTC_StockpileManager.unsavedEdits = true;

        }


        private void btnClearStashHistory_Click(object sender, EventArgs e)
        {
			RTC_StockpileManager.StashHistory.Clear();
			RefreshStashHistory();
        }

        private void btnRemoveSelectedStockpile_Click(object sender, EventArgs e) => RemoveSelected();
		public void RemoveSelected(bool force = false)
		{
			RTC_Core.StopSound();

			if (dgvStockpile.SelectedRows.Count != 0 && (MessageBox.Show("Are you sure you want to remove the selected stockpile entries?", "Delete Stockpile Entry?", MessageBoxButtons.YesNo) == DialogResult.Yes))
					foreach (DataGridViewRow row in dgvStockpile.SelectedRows)
						dgvStockpile.Rows.Remove(row);

			RTC_StockpileManager.StockpileChanged();

            RTC_StockpileManager.unsavedEdits = true;

            RedrawActionUI();

            RTC_Core.StartSound();
		}

		private void btnClearStockpile_Click(object sender, EventArgs e) => ClearStockpile();
		public void ClearStockpile(bool force = false)
		{
			RTC_Core.StopSound();

			if (force || MessageBox.Show("Are you sure you want to clear the stockpile?", "Clearing stockpile", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				dgvStockpile.Rows.Clear();

				if (RTC_StockpileManager.currentStockpile != null)
				{
					RTC_StockpileManager.currentStockpile.Filename = null;
					RTC_StockpileManager.currentStockpile.ShortFilename = null;
				}

				RTC_Core.ghForm.btnSaveStockpile.Enabled = false;
				RTC_Core.ghForm.btnSaveStockpile.BackColor = Color.Gray;
				RTC_Core.ghForm.btnSaveStockpile.ForeColor = Color.DimGray;

				RTC_StockpileManager.StockpileChanged();

                RTC_StockpileManager.unsavedEdits = false;

                RedrawActionUI();

                RTC_Core.StartSound();

			}
		}

        private void btnLoadStockpile_Click(object sender, MouseEventArgs e)
        {
            RTC_Core.CheckForProblematicProcesses();

            Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

			ContextMenuStrip LoadMenuItems = new ContextMenuStrip();
			LoadMenuItems.Items.Add("Load Stockpile", null, new EventHandler((ob, ev) => {

				try
				{
					RTC_Core.StopSound();

					if (Stockpile.Load(dgvStockpile))
					{
						RTC_Core.ghForm.btnSaveStockpile.Enabled = true;
						RTC_Core.ghForm.btnSaveStockpile.BackColor = Color.Tomato;
						RTC_Core.ghForm.btnSaveStockpile.ForeColor = Color.Black;
					}

					RTC_Core.spForm.dgvStockpile.Rows.Clear();

					dgvStockpile.ClearSelection();
					RTC_StockpileManager.StockpileChanged();

                    RTC_StockpileManager.unsavedEdits = false;

                }
				finally
				{
					RTC_Core.StartSound();
				}
			}));

			LoadMenuItems.Items.Add("Load Bizhawk settings from Stockpile", null, new EventHandler((ob, ev) => {
				try
				{
					RTC_Core.StopSound();
					Stockpile.LoadBizhawkConfigFromStockpile();
				}
				finally
				{
					RTC_Core.StartSound();
				}
			}));

			LoadMenuItems.Items.Add("Restore Bizhawk config Backup", null, new EventHandler((ob, ev) =>
			{

				try
				{
					RTC_Core.StopSound();
					Stockpile.RestoreBizhawkConfig();
				}
				finally
				{
					RTC_Core.StartSound();
				}

			})).Enabled = (File.Exists(RTC_Core.bizhawkDir + "\\backup_config.ini"));

			LoadMenuItems.Show(this, locate);


        }

        private void btnSaveStockpileAs_Click(object sender, EventArgs e)
        {
            if (dgvStockpile.Rows.Count == 0)
            {
				RTC_Core.StopSound();
				MessageBox.Show("You cannot save the Stockpile because it is empty");
				RTC_Core.StartSound();
				return;
            }

			RTC_Core.StopSound();

            Stockpile sks = new Stockpile(dgvStockpile);
			if (Stockpile.Save(sks))
			{
				sendCurrentStockpileToTemp();
				RTC_Core.ghForm.btnSaveStockpile.Enabled = true;
				RTC_Core.ghForm.btnSaveStockpile.BackColor = Color.Tomato;
				RTC_Core.ghForm.btnSaveStockpile.ForeColor = Color.Black;
			}

			RTC_Core.StartSound();

        }

        private void btnSaveStockpile_Click(object sender, EventArgs e)
        {
			RTC_Core.StopSound();

			Stockpile sks = new Stockpile(dgvStockpile);
			if (Stockpile.Save(sks, true))
				sendCurrentStockpileToTemp();

			RTC_Core.StartSound();
		}

        public void btnBlastToggle_Click(object sender, EventArgs e)
        {
            //if (Global.Emulator is NullEmulator)
            //   return;

            if (RTC_StockpileManager.currentStashkey == null || RTC_StockpileManager.currentStashkey.BlastLayer.Layer.Count == 0)
            {
                IsCorruptionApplied = false;
                return;
            }

            if (!IsCorruptionApplied)
            {
                IsCorruptionApplied = true;
				RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.BLAST) { blastlayer = RTC_StockpileManager.currentStashkey.BlastLayer});
				//RTC_StockpileManager.currentStashkey.blastlayer.Apply();

            }
            else
            {
                IsCorruptionApplied = false;

				//RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_SET_HELLGENIE_CLEARALLCHEATS) {});
				RTC_HellgenieEngine.ClearCheats();
				RTC_PipeEngine.ClearPipes();

				RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_SET_RESTOREBLASTLAYERBACKUP) { });

			}

		}

        private void btnImportStockpile_Click(object sender, EventArgs e)
        {
			RTC_Core.StopSound();
            Stockpile.Import();
			RTC_Core.StartSound();

        }

        private void btnStashUP_Click(object sender, EventArgs e)
        {
            if(lbStashHistory.SelectedIndex == -1)
                return;

            if (lbStashHistory.SelectedIndex == 0)
                lbStashHistory.SelectedIndex = lbStashHistory.Items.Count - 1;
            else
                lbStashHistory.SelectedIndex--;
        }

        private void btnStashDOWN_Click(object sender, EventArgs e)
        {
                if (lbStashHistory.SelectedIndex == -1)
                    return;

                if (lbStashHistory.SelectedIndex == lbStashHistory.Items.Count - 1)
                    lbStashHistory.SelectedIndex = 0;
                else
                    lbStashHistory.SelectedIndex++;
        }

        private void btnStockpileUP_Click(object sender, EventArgs e)
        {
            if (dgvStockpile.SelectedRows.Count == 0)
                return;

			int CurrentSelectedIndex = dgvStockpile.SelectedRows[0].Index;

			if (CurrentSelectedIndex == 0)
			{
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[dgvStockpile.Rows.Count - 1].Selected = true;
			}
			else
			{
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[CurrentSelectedIndex - 1].Selected = true;
			}

			dgvStockpile_CellClick(dgvStockpile, null);

        }

        private void btnStockpileDOWN_Click(object sender, EventArgs e)
        {
			if (dgvStockpile.SelectedRows.Count == 0)
				return;

			int CurrentSelectedIndex = dgvStockpile.SelectedRows[0].Index;

			if (CurrentSelectedIndex == dgvStockpile.Rows.Count - 1)
			{
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[0].Selected = true;
			}
			else
			{
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[CurrentSelectedIndex + 1].Selected = true;
			}

			dgvStockpile_CellClick(dgvStockpile, null);

		}

        private void btnStockpileMoveSelectedUp_Click(object sender, EventArgs e)
        {
			if (dgvStockpile.SelectedRows.Count == 0)
				return;

			int count = dgvStockpile.Rows.Count;

			if (count < 2)
                return;

			int pos = dgvStockpile.SelectedRows[0].Index;
			var row = dgvStockpile.Rows[pos];


			dgvStockpile.Rows.RemoveAt(pos);


            if (pos == 0)
            {
				int newpos = dgvStockpile.Rows.Add(row);
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[newpos].Selected = true;
            }
            else
            {
				int newpos = pos - 1;
				dgvStockpile.Rows.Insert(newpos, row);
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[newpos].Selected = true;
			}

            RTC_StockpileManager.unsavedEdits = true;

            RTC_StockpileManager.StockpileChanged();

		}

        private void btnStockpileMoveSelectedDown_Click(object sender, EventArgs e)
        {
			if (dgvStockpile.SelectedRows.Count == 0)
				return;

			int count = dgvStockpile.Rows.Count;

			if (count < 2)
                return;

			int pos = dgvStockpile.SelectedRows[0].Index;
			var row = dgvStockpile.Rows[pos];

			dgvStockpile.Rows.RemoveAt(pos);

            if (pos == count - 1)
            {
				int newpos = 0;
				dgvStockpile.Rows.Insert(newpos, row);
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[newpos].Selected = true;
			}
            else
            {
				int newpos = pos + 1;
				dgvStockpile.Rows.Insert(newpos, row);
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[newpos].Selected = true;
			}

            RTC_StockpileManager.unsavedEdits = true;

            RTC_StockpileManager.StockpileChanged();

		}

        private void btnStopRender_Click(object sender, EventArgs e)
        {
            RTC_Render.StopRender();
        }

        private void nmIntensity_ValueChanged(object sender, EventArgs e)
        {
			int _fx = Convert.ToInt32(nmIntensity.Value);

			if (RTC_Core.ecForm.Intensity != _fx)
				RTC_Core.ecForm.Intensity = _fx;

		}

		private void track_Intensity_Scroll(object sender, EventArgs e)
        {
			double fx = Math.Floor(Math.Pow((track_Intensity.Value * 0.0005d), 2));
			int _fx = Convert.ToInt32(fx);

			if (_fx != RTC_Core.ecForm.Intensity)
				RTC_Core.ecForm.Intensity = _fx;

		}


        public void btnSendRaw_Click(object sender, EventArgs e)
        {
			try
			{
				btnSendRaw.Visible = false;

                var token = RTC_NetCore.HugeOperationStart("LAZY");

                StashKey sk = (StashKey)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETRAWBLASTLAYER), true);

				RTC_StockpileManager.currentStashkey = sk;
				RTC_StockpileManager.StashHistory.Add(RTC_StockpileManager.currentStashkey);

                RTC_NetCore.HugeOperationEnd(token);

                RefreshStashHistory();

				dgvStockpile.ClearSelection();

			}
			finally
			{
				btnSendRaw.Visible = true;
			}
        }

		private void btnRenameSelected_Click(object sender, EventArgs e)
		{
			if (!btnRenameSelected.Visible)
				return;

			RTC_Core.StopSound();

			if (dgvStockpile.SelectedRows.Count != 0)
			{

				string Name = "";
				string value = "";

				if (RTC_Extensions.getInputBox("Glitch Harvester", "Enter the new Stash name:", ref value) == DialogResult.OK)
				{
					Name = value.Trim();
				}
				else
				{
					RTC_Core.StartSound();
					return;
				}

				(dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey).Alias = Name;

				dgvStockpile.Refresh();
				//lbStockpile.RefreshItemsReal();


			}

			RTC_StockpileManager.StockpileChanged();

            RTC_StockpileManager.unsavedEdits = true;

            RTC_Core.StartSound();

		}



		private void cbAutoLoadState_CheckedChanged(object sender, EventArgs e)
		{
			RTC_StockpileManager.loadBeforeOperation = cbAutoLoadState.Checked;
		}

		private void cbStashCorrupted_CheckedChanged(object sender, EventArgs e)
		{
			RTC_StockpileManager.stashAfterOperation = cbStashCorrupted.Checked;
		}

		private void btnBackPanelPage_Click(object sender, EventArgs e)
		{
			if (pnSavestateHolder.Location.X != 0)
				pnSavestateHolder.Location = new Point(pnSavestateHolder.Location.X + 150, pnSavestateHolder.Location.Y);
		}

		private void btnForwardPanelPage_Click(object sender, EventArgs e)
		{
			if(pnSavestateHolder.Location.X != -450)
				pnSavestateHolder.Location = new Point(pnSavestateHolder.Location.X - 150, pnSavestateHolder.Location.Y);
		}

		private void dgvStockpile_MouseDown(object sender, MouseEventArgs e)
		{

			if (e.Button == MouseButtons.Right)
			{
				Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

				ContextMenuStrip columnsMenu = new ContextMenuStrip();
				(columnsMenu.Items.Add("Show Item Name", null, new EventHandler((ob,ev) => { dgvStockpile.Columns["Item"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["Item"].Visible;
				(columnsMenu.Items.Add("Show Game Name", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["GameName"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["GameName"].Visible;
				(columnsMenu.Items.Add("Show System Name", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["SystemName"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["SystemName"].Visible;
				(columnsMenu.Items.Add("Show System Core", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["SystemCore"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["SystemCore"].Visible;
				(columnsMenu.Items.Add("Show Note", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["Note"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["Note"].Visible;

                columnsMenu.Items.Add(new ToolStripSeparator());
				(columnsMenu.Items.Add("[Multiplayer] Send Selected Item as a Blast", null, new EventHandler((ob, ev) => { RTC_Core.Multiplayer?.SendBlastlayer(); })) as ToolStripMenuItem).Enabled = RTC_Core.Multiplayer != null && RTC_Core.Multiplayer.side != NetworkSide.DISCONNECTED;
				(columnsMenu.Items.Add("[Multiplayer] Send Selected Item as a Game State", null, new EventHandler((ob, ev) => { RTC_Core.Multiplayer?.SendStashkey(); })) as ToolStripMenuItem).Enabled = RTC_Core.Multiplayer != null && RTC_Core.Multiplayer.side != NetworkSide.DISCONNECTED;

                columnsMenu.Items.Add(new ToolStripSeparator());
				(columnsMenu.Items.Add("Open Selected Item in Blast Editor", null, new EventHandler((ob, ev) => {
                    if (RTC_Core.beForm != null)
                    {
                        var sk = (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);
                        RTC_Core.beForm.Close();
                        RTC_Core.beForm = new RTC_NewBlastEditor_Form();

						//If the blastlayer is big, prompt them before opening it. Let's go with 5k for now.
						if (sk.BlastLayer.Layer.Count > 5000 && (DialogResult.Yes == MessageBox.Show($"You're trying to open a blastlayer of size " + sk.BlastLayer.Layer.Count + ". This could take a while. Are you sure you want to continue?", "Opening a large BlastLayer", MessageBoxButtons.YesNo)));
							RTC_Core.beForm.LoadStashkey(sk);
                    }
                })) as ToolStripMenuItem).Enabled = (dgvStockpile.SelectedRows.Count == 1);

                columnsMenu.Items.Add(new ToolStripSeparator());
                (columnsMenu.Items.Add("Generate VMD from Selected Item", null, new EventHandler((ob, ev) => {
                    var sk = (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);
                    RTC_MemoryDomains.GenerateVmdFromStashkey(sk);
                })) as ToolStripMenuItem).Enabled = (dgvStockpile.SelectedRows.Count == 1);

                columnsMenu.Show(this, locate);
			}
		}

		private void btnRerollSelected_Click(object sender, EventArgs e)
		{

			if (lbStashHistory.SelectedIndex != -1)
			{
				RTC_StockpileManager.currentStashkey = (StashKey)RTC_StockpileManager.StashHistory[lbStashHistory.SelectedIndex].Clone();
			}
			else if (dgvStockpile.SelectedRows.Count != 0 && dgvStockpile.SelectedRows[0].Cells[0].Value != null)
			{
				RTC_StockpileManager.currentStashkey = (StashKey)(dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey).Clone();
                //RTC_StockpileManager.unsavedEdits = true;
            }
			else
				return;

			RTC_StockpileManager.currentStashkey.BlastLayer.Reroll();

            RTC_StockpileManager.AddCurrentStashkeyToStash();

			RTC_StockpileManager.ApplyStashkey(RTC_StockpileManager.currentStashkey);
		}

		private void btnSaveSavestateList_Click(object sender, EventArgs e)
		{
			try
			{

				SaveStateKeys ssk = new SaveStateKeys();

				for (int i = 1; i < 41; i++)
				{
					string key = i.ToString().PadLeft(2, '0');

					if (RTC_StockpileManager.SavestateStashkeyDico.ContainsKey(key))
					{
						ssk.StashKeys[i] = RTC_StockpileManager.SavestateStashkeyDico[key];
						ssk.Text[i] = StateBoxes[key].Text;
					}
					else
					{
						ssk.StashKeys[i] = null;
						ssk.Text[i] = null;
					}

				}

				string Filename;
				string ShortFilename;

				SaveFileDialog saveFileDialog1 = new SaveFileDialog();
				saveFileDialog1.DefaultExt = "ssk";
				saveFileDialog1.Title = "Savestate Keys File";
				saveFileDialog1.Filter = "SSK files|*.ssk";
				saveFileDialog1.RestoreDirectory = true;

				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
				{
					Filename = saveFileDialog1.FileName;
					ShortFilename = Filename.Substring(Filename.LastIndexOf("\\") + 1, Filename.Length - (Filename.LastIndexOf("\\") + 1));
				}
				else
					return;

				//clean temp4 folder
				foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP4"))
					File.Delete(file);


				for (int i = 1; i < 41; i++)
				{
					StashKey key = ssk.StashKeys[i];

					if (key == null)
						continue;

					string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

					if (!File.Exists(RTC_Core.rtcDir + "\\TEMP4\\" + statefilename))
						File.Copy(RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename, RTC_Core.rtcDir + "\\TEMP4\\" + statefilename); // copy savestates to temp folder

				}

				//creater stockpile.xml to temp folder from stockpile object

				using (FileStream FS = File.Open(RTC_Core.rtcDir + "\\TEMP4\\keys.xml", FileMode.OpenOrCreate))
				{
					XmlSerializer xs = new XmlSerializer(typeof(SaveStateKeys));

					xs.Serialize(FS, ssk);
					FS.Close();
				}


				//7z the temp folder to destination filename
				//string[] stringargs = { "-c", Filename, RTC_Core.rtcDir + "\\TEMP4\\" };
				//FastZipProgram.Exec(stringargs);

				string tempFilename = Filename + ".temp";

				System.IO.Compression.ZipFile.CreateFromDirectory(RTC_Core.rtcDir + "\\TEMP4\\", tempFilename, System.IO.Compression.CompressionLevel.Fastest, false);

				if (File.Exists(Filename))
					File.Delete(Filename);

				File.Move(tempFilename, Filename);
			}
			catch
			{
				MessageBox.Show("The Savestate Keys file could not be saved");
				return;
			}
        }

		private void btnLoadSavestateList_Click(object sender, EventArgs e)
		{
			string Filename;

				OpenFileDialog ofd = new OpenFileDialog();
				ofd.DefaultExt = "ssk";
				ofd.Title = "Open Savestate Keys File";
				ofd.Filter = "SSK files|*.ssk";
				ofd.RestoreDirectory = true;
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					Filename = ofd.FileName.ToString();
				}
				else
					return;

			if (!File.Exists(Filename))
			{
				MessageBox.Show("The Savestate Keys file wasn't found");
				return;
			}

			SaveStateKeys ssk;

            var token = RTC_NetCore.HugeOperationStart();

			try
			{
				
				Stockpile.Extract(Filename,"TEMP4","keys.xml");

				using(FileStream FS = File.Open(RTC_Core.rtcDir + "\\TEMP4\\keys.xml", FileMode.OpenOrCreate))
				{
					XmlSerializer xs = new XmlSerializer(typeof(SaveStateKeys));
					ssk = (SaveStateKeys)xs.Deserialize(FS);
					FS.Close();
				}
			}
			catch
			{
				MessageBox.Show("The Savestate Keys file could not be loaded");
				return;
			}

			// repopulating savestates out of temp4 folder
			for (int i = 1; i < 41; i++)
			{
				StashKey key = ssk.StashKeys[i];

				if (key == null)
					continue;

				string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name
                string newStatePath = RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename;
                string shortRomFilename = key.RomFilename.Substring(key.RomFilename.LastIndexOf("\\") + 1);


                if (!File.Exists(RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename))
					File.Copy(RTC_Core.rtcDir + "\\TEMP4\\" + statefilename, newStatePath); // copy savestates to temp folder

                //key.RomFilename = RTC_Core.rtcDir + "\\TEMP4\\" + shortRomFilename;
                key.StateFilename = newStatePath;

            }

            //clear the stockpile dico
            RTC_StockpileManager.SavestateStashkeyDico.Clear();


            //fill text/state controls/dico
            for (int i = 1; i < 41; i++)
			{
				string key = i.ToString().PadLeft(2, '0');

				if (key == null)
					continue;

				if (ssk.StashKeys[i] != null)
				{
					if (!RTC_StockpileManager.SavestateStashkeyDico.ContainsKey(key))
						RTC_StockpileManager.SavestateStashkeyDico.Add(key, ssk.StashKeys[i]);
					else
						RTC_StockpileManager.SavestateStashkeyDico[key] = ssk.StashKeys[i];
				}

				StateBoxes[key].Text = "";

				if (ssk.Text[i] != null)
					StateBoxes[key].Text = ssk.Text[i];

			}

            // We can switch cores on the fly now, no need for compatibility check
            //CheckCompatibility();

            refreshSavestateTextboxes();

            RTC_NetCore.HugeOperationEnd(token);
        }

        /*
         * //Not used since we switch cores on demand now
        public static void CheckCompatibility()
        {
            List<string> ErrorMessages = new List<string>();

            List<StashKey> sks = new List<StashKey>();

            for (int i = 1; i < 41; i++)
            {
                string key = i.ToString().PadLeft(2, '0');

                if (key == null)
                    continue;

                if (RTC_StockpileManager.SavestateStashkeyDico.ContainsKey(key))
                    sks.Add(RTC_StockpileManager.SavestateStashkeyDico[key]);
            }

            Dictionary<string, string> StashkeySystemNameToCurrentCoreDico = new Dictionary<string, string>();

            foreach (StashKey sk in sks)
            {
                string currentCore;
                string systemName = sk.SystemName;

                if (StashkeySystemNameToCurrentCoreDico.ContainsKey(systemName))
                    currentCore = StashkeySystemNameToCurrentCoreDico[systemName];
                else
                {
                    currentCore = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETSYSTEMCORE) { objectValue = systemName }, true);
                    StashkeySystemNameToCurrentCoreDico.Add(systemName, currentCore);
                }

                if (sk.SystemCore != currentCore)
                {
                    string errorMessage = $"Core mismatch for System [{sk.SystemName}]\n Current Bizhawk core -> {currentCore}\n SavestateList core -> {sk.SystemCore}";

                    if (!ErrorMessages.Contains(errorMessage))
                        ErrorMessages.Add(errorMessage);
                }

            }

            if (ErrorMessages.Count == 0)
                return;

            string message = "The loaded SavestateList returned the following errors:\n\n";

            foreach (string line in ErrorMessages)
                message += $"•  {line} \n\n";


            MessageBox.Show(message, "Compatibility Checker", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }
        */

        public void refreshSavestateTextboxes()
        {
            //fill text/state controls/dico
            for (int i = 1; i < 41; i++)
            {
                string key = i.ToString().PadLeft(2, '0');

                if (key == null)
                    continue;

                if(RTC_StockpileManager.SavestateStashkeyDico.ContainsKey(key))
                    StateBoxes[key].Visible = true;
                else
                    StateBoxes[key].Visible = false;

            }
        }

		private void lbStashHistory_MouseDown(object sender, MouseEventArgs e)
		{

			if (e.Button == MouseButtons.Right)
			{
				Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

				ContextMenuStrip columnsMenu = new ContextMenuStrip();
				(columnsMenu.Items.Add("[Multiplayer] Send Selected Item as a Blast", null, new EventHandler((ob, ev) => { RTC_Core.Multiplayer.SendBlastlayer(); })) as ToolStripMenuItem).Enabled = RTC_Core.Multiplayer.side != NetworkSide.DISCONNECTED;
				(columnsMenu.Items.Add("[Multiplayer] Send Selected Item as a Game State", null, new EventHandler((ob, ev) => { RTC_Core.Multiplayer.SendStashkey(); })) as ToolStripMenuItem).Enabled = RTC_Core.Multiplayer.side != NetworkSide.DISCONNECTED;
				columnsMenu.Show(this, locate);
			}
		}

		private void btnSaveLoad_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

				ContextMenuStrip columnsMenu = new ContextMenuStrip();
				(columnsMenu.Items.Add("[Multiplayer] Pull State from peer", null, new EventHandler((ob, ev) => {

					RTC_Core.multiForm.cbPullStateToGlitchHarvester.Checked = true;
					RTC_Core.Multiplayer.SendCommand(new RTC_Command(CommandType.PULLSTATE), false);

				})) as ToolStripMenuItem).Enabled = RTC_Core.Multiplayer != null && RTC_Core.Multiplayer.side != NetworkSide.DISCONNECTED;

                columnsMenu.Items.Add(new ToolStripSeparator());
                (columnsMenu.Items.Add("Open Selected Item in Blast Editor", null, new EventHandler((ob, ev) => {
                    if (RTC_Core.beForm != null)
                    {
                        var sk = RTC_StockpileManager.StashHistory[lbStashHistory.SelectedIndex];
                        RTC_Core.beForm.Close();
                        RTC_Core.beForm = new RTC_NewBlastEditor_Form();
                        RTC_Core.beForm.LoadStashkey(sk);
                    }
                })) as ToolStripMenuItem).Enabled = lbStashHistory.SelectedIndex != -1;

                columnsMenu.Items.Add(new ToolStripSeparator());
                (columnsMenu.Items.Add("Generate VMD from Selected Item", null, new EventHandler((ob, ev) => {
                    var sk = RTC_StockpileManager.StashHistory[lbStashHistory.SelectedIndex];
                    sk.BlastLayer.Rasterize();
                    RTC_MemoryDomains.GenerateVmdFromStashkey(sk);
                })) as ToolStripMenuItem).Enabled = lbStashHistory.SelectedIndex != -1;

                columnsMenu.Show(this, locate);
			}
		}

		private void sendCurrentStockpileToTemp()
		{
			foreach (DataGridViewRow dataRow in RTC_Core.ghForm.dgvStockpile.Rows)
			{
				StashKey sk = (StashKey)dataRow.Cells["Item"].Value;
				sk.RomFilename = RTC_Core.rtcDir + "\\TEMP\\" + RTC_Extensions.getShortFilenameFromPath(sk.RomFilename);
			}
		}

		private void dgvStockpile_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				dgvStockpile.Enabled = false;
                btnStockpileUP.Enabled = false;
                btnStockpileDOWN.Enabled = false;

                // Stockpile Note handling
                if (e != null)
				{
					var senderGrid = (DataGridView)sender;

					if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
						e.RowIndex >= 0)
					{
						StashKey sk = (StashKey)senderGrid.Rows[e.RowIndex].Cells["Item"].Value;
						if (RTC_NoteEditor_Form.currentlyOpenNoteForm == null)
						{
							RTC_NoteEditor_Form.currentlyOpenNoteForm = new RTC_NoteEditor_Form(sk, senderGrid);
						}
						else
						{
							if (RTC_NoteEditor_Form.currentlyOpenNoteForm.Visible)
								RTC_NoteEditor_Form.currentlyOpenNoteForm.Close();

							RTC_NoteEditor_Form.currentlyOpenNoteForm = new RTC_NoteEditor_Form(sk, senderGrid);
						}

						return;
					}
				}



				lbStashHistory.ClearSelected();
				RTC_Core.spForm.dgvStockpile.ClearSelection();

                RedrawActionUI();

                if (dgvStockpile.SelectedRows.Count == 0)
                    return;

                RTC_StockpileManager.currentStashkey = (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);

				if (!cbLoadOnSelect.Checked)
					return;



				// Merge Execution
				if (dgvStockpile.SelectedRows.Count > 1)
				{

                    List<StashKey> sks = new List<StashKey>();

					foreach (DataGridViewRow row in dgvStockpile.SelectedRows)
						sks.Add((StashKey)row.Cells[0].Value);

					RTC_StockpileManager.MergeStashkeys(sks);

                    return;
				}


                // One item Execution

                var token = RTC_NetCore.HugeOperationStart("LAZY");

                if (rbCorrupt.Checked)
					RTC_StockpileManager.ApplyStashkey(RTC_StockpileManager.currentStashkey);
				if (rbInject.Checked)
					RTC_StockpileManager.InjectFromStashkey(RTC_StockpileManager.currentStashkey);
				else if (rbOriginal.Checked)
					RTC_StockpileManager.OriginalFromStashkey(RTC_StockpileManager.currentStashkey);

                RTC_NetCore.HugeOperationEnd(token);

            }
			finally
			{
				dgvStockpile.Enabled = true;
                btnStockpileUP.Enabled = true;
                btnStockpileDOWN.Enabled = true;
			}
		}

        public void RedrawActionUI()
        {
            // Merge tool and ui change
            if (dgvStockpile.SelectedRows.Count > 1)
            {
                rbCorrupt.Checked = true;
                rbCorrupt.Enabled = false;
                rbInject.Enabled = false;
                rbOriginal.Enabled = false;
                btnCorrupt.Text = "Merge";
                btnRenameSelected.Visible = false;
                btnRemoveSelectedStockpile.Text = "Remove Items";

                rbCorrupt.Checked = true;
            }
            else
            {
                rbCorrupt.Enabled = true;
                rbInject.Enabled = true;
                rbOriginal.Enabled = true;
                btnRenameSelected.Visible = true;
                btnRemoveSelectedStockpile.Text = "Remove Item";

                if (rbCorrupt.Checked)
                    btnCorrupt.Text = "Blast/Send";
                else if (rbInject.Checked)
                    btnCorrupt.Text = "Inject";
                else if (rbOriginal.Checked)
                    btnCorrupt.Text = "Original";

            }
        }

		private void cbRenderType_SelectedIndexChanged(object sender, EventArgs e)
		{
			RTC_Render.setType(cbRenderType.SelectedItem.ToString());
		}

		private void btnOpenRenderFolder_Click(object sender, EventArgs e)
		{
			Process.Start(RTC_Core.rtcDir + "\\RENDEROUTPUT\\");
		}

		private void btnRender_Click(object sender, EventArgs e)
		{
			if(btnRender.Text == "Start Render")
			{
				RTC_Render.StartRender();
			}
			else
			{
				btnRender.Text = "Start Render";
				btnRender.ForeColor = Color.White;
				RTC_Render.StopRender();
			}
		}

		private void cbRenderAtLoad_CheckedChanged(object sender, EventArgs e)
		{
			RTC_StockpileManager.renderAtLoad = cbRenderAtLoad.Checked;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_RENDER_RENDERATLOAD) { objectValue = RTC_StockpileManager.renderAtLoad });
		}

        private void btnLoadSavestateList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

                ContextMenuStrip columnsMenu = new ContextMenuStrip();
                columnsMenu.Items.Add("Clear Savestate List", null, new EventHandler((ob, ev) => {

                    foreach (var item in pnSavestateHolder.Controls)
                    {
                        if (item is Button)
                            (item as Button).ForeColor = Color.FromArgb(192, 255, 192);

                        if (item is TextBox)
                            (item as TextBox).Text = "";
                    }


                    for (int i = 1; i < 41; i++)
                    {
                        string key = i.ToString().PadLeft(2, '0');

                        if (key == null)
                            continue;

                        if (RTC_StockpileManager.SavestateStashkeyDico.ContainsKey(key))
                            RTC_StockpileManager.SavestateStashkeyDico.Remove(key);

                    }

                    RTC_StockpileManager.currentSavestateKey = null;

                    refreshSavestateTextboxes();

                }));

                columnsMenu.Show(this, locate);
            }
        }

        private void btnCorrupt_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

                ContextMenuStrip columnsMenu = new ContextMenuStrip();
                columnsMenu.Items.Add("Blast + Send RAW To Stash", null, new EventHandler((ob, ev) => {
                    RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTRAWSTASH));
                }));
                columnsMenu.Show(this, locate);
            }
        }

        private void track_Intensity_MouseDown(object sender, MouseEventArgs e)
        {
            RTC_NetCore.HugeOperationStart("LAZY");
        }

        private void track_Intensity_MouseUp(object sender, MouseEventArgs e)
        {
            RTC_NetCore.HugeOperationEnd();

            track_Intensity_Scroll(sender, e);
        }
    }
}
