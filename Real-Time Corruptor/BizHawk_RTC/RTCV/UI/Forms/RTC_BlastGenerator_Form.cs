using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CorruptCore;
using RTCV.CorruptCore;
using RTCV.NetCore;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
{
	// 0  dgvBlastProtoReference
	// 1  dgvRowDirty
	// 2  dgvEnabled
	// 3  dgvNoteText
	// 4  dgvDomain
	// 5  dgvPrecision
	// 6  dgvType
	// 7  dgvMode
	// 8  dgvStepSize
	// 9  dgvStartAddress
	// 10 dgvEndAddress
	// 11 dgvParam1
	// 12 dgvParam2
	// 13 dgvNoteButton

	//TYPE = BLASTUNITTYPE
	//MODE = GENERATIONMODE

	public partial class RTC_BlastGenerator_Form : Form, IAutoColorize
	{
		private enum BlastGeneratorColumn
		{
			dgvBlastProtoReference,
			DgvRowDirty,
			DgvEnabled,
			DgvNoteText,
			DgvDomain,
			DgvPrecision,
			DgvType,
			DgvMode,
			DgvStepSize,
			DgvStartAddress,
			DgvEndAddress,
			DgvParam1,
			DgvParam2,
			DgvNoteButton
		}

		public static BlastLayer CurrentBlastLayer = null;
		private bool openedFromBlastEditor = false;
		private StashKey sk = null;
		private ContextMenuStrip cms = new ContextMenuStrip();
		private bool initialized = false;

		private static Dictionary<string, MemoryInterface> domainToMiDico = new Dictionary<string, MemoryInterface>();
		private string[] domains;

		public RTC_BlastGenerator_Form()
		{
			InitializeComponent();
		}

		private void RTC_BlastGeneratorForm_Load(object sender, EventArgs e)
		{
			dgvBlastGenerator.MouseClick += dgvBlastGenerator_MouseClick;
			dgvBlastGenerator.CellValueChanged += dgvBlastGenerator_CellValueChanged;
			dgvBlastGenerator.CellClick += dgvBlastGenerator_CellClick;
			RTC_UICore.SetRTCColor(RTC_UICore.GeneralColor, this);
			domains = RTC_MemoryDomains.MemoryInterfaces?.Keys?.Concat(RTC_MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();
		}

		public void LoadNoStashKey()
		{
			RefreshDomains();
			AddDefaultRow();
			PopulateModeCombobox(dgvBlastGenerator.Rows[0]);
			openedFromBlastEditor = false;
			btnSendTo.Text = "Send to Stash";
			initialized = true;

			this.Show();
		}

		public void LoadStashkey(StashKey _sk)
		{
			if (_sk == null)
				return;

			sk = (StashKey)_sk.Clone();
			sk.BlastLayer = new BlastLayer();

			RefreshDomains();
			AddDefaultRow();
			PopulateModeCombobox(dgvBlastGenerator.Rows[0]);
			openedFromBlastEditor = true;
			btnSendTo.Text = "Send to Blast Editor";
			initialized = true;

			this.Show();
		}

		private void AddDefaultRow()
		{
			try
			{
				//Add an empty row and populate with default values
				dgvBlastGenerator.Rows.Add();
				int lastrow = dgvBlastGenerator.RowCount - 1;
				//Set up the DGV based on the current state of Bizhawk
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvRowDirty"]).Value = true;
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvEnabled"]).Value = true;
				((DataGridViewComboBoxCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvPrecision"]).Value = ((DataGridViewComboBoxCell)dgvBlastGenerator.Rows[0].Cells["dgvPrecision"]).Items[0];
				((DataGridViewComboBoxCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvType"]).Value = ((DataGridViewComboBoxCell)dgvBlastGenerator.Rows[0].Cells["dgvType"]).Items[0];


				//We need to make the rows type decimal as the NumericUpDown is formatted as string by default (due to the potential for commas)
				dgvBlastGenerator.Rows[lastrow].Cells["dgvStartAddress"].ValueType = typeof(System.Decimal);
				dgvBlastGenerator.Rows[lastrow].Cells["dgvEndAddress"].ValueType = typeof(System.Decimal);
				dgvBlastGenerator.Rows[lastrow].Cells["dgvParam1"].ValueType = typeof(System.Decimal);
				dgvBlastGenerator.Rows[lastrow].Cells["dgvParam2"].ValueType = typeof(System.Decimal);


				//These can't be null or else things go bad when trying to save and load them from a file. Include an M as they NEED to be decimal.
				((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvStartAddress"]).Value = 0M;
				((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvEndAddress"]).Value = 1M;
				((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvParam1"]).Value = 0M;
				((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvParam2"]).Value = 0M;

				PopulateDomainCombobox(dgvBlastGenerator.Rows[lastrow]);
				PopulateModeCombobox(dgvBlastGenerator.Rows[lastrow]);
				// (dgvBlastGenerator.Rows[lastrow].Cells["dgvMode"] as DataGridViewComboBoxCell).Value = (dgvBlastGenerator.Rows[0].Cells["dgvMode"] as DataGridViewComboBoxCell).Items[0];


				//For some reason, setting the minimum on the DGV to 1 doesn't change the fact it inserts with a count of 0
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvStepSize"]).Value = 1;
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					"An error occurred in RTC while adding a new row.\n\n" +
					"Your session is probably broken\n\n\n" +
					ex.ToString());
			}
		}

		private bool PopulateDomainCombobox(DataGridViewRow row)
		{
			try
			{
				if (row.Cells["dgvDomain"] is DataGridViewComboBoxCell)
				{
					DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)row.Cells["dgvDomain"];
					object currentValue = cell.Value;

					cell.Value = null;
					cell.Items.Clear();

					foreach (string domain in domains)
					{
						cell.Items.Add(domain);
					}


					if (currentValue != null && cell.Items.Contains(currentValue))
						cell.Value = currentValue;
					else if (cell.Items.Count > 0)
						cell.Value = cell.Items[0];
					else
						cell.Value = null;

				}

				UpdateAddressRange(row);

				return true;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private static void UpdateAddressRange(DataGridViewRow row)
		{
			try
			{
				if (row.Cells["dgvDomain"].Value == null)
					return;

				long size = domainToMiDico[row.Cells["dgvDomain"].Value.ToString()].Size;

				((DataGridViewNumericUpDownCell)row.Cells["dgvStartAddress"]).Maximum = size;
				((DataGridViewNumericUpDownCell)row.Cells["dgvEndAddress"]).Maximum = size;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private static void PopulateModeCombobox(DataGridViewRow row)
		{

			if (row.Cells["dgvMode"] is DataGridViewComboBoxCell cell)
			{
				cell.Value = null;
				cell.Items.Clear();


				switch (row.Cells["dgvType"].Value.ToString())
				{
					case "BlastByte":
						foreach (BGBlastByteModes type in Enum.GetValues(typeof(BGBlastByteModes)))
						{
							cell.Items.Add(type.ToString());
						}
						break;
					case "BlastCheat":
						foreach (BGBlastCheatModes type in Enum.GetValues(typeof(BGBlastCheatModes)))
						{
							cell.Items.Add(type.ToString());
						}
						break;
					case "BlastPipe":
						foreach (BGBlastPipeModes type in Enum.GetValues(typeof(BGBlastPipeModes)))
						{
							cell.Items.Add(type.ToString());
						}
						break;
				}
				cell.Value = cell.Items[0];
			}
		}

		private void btnJustCorrupt_Click(object sender, EventArgs e)
		{
			BlastLayer bl = GenerateBlastLayers();
			(bl?.Clone() as BlastLayer)?.Apply();
		}

		private void btnLoadCorrupt_Click(object sender, EventArgs e)
		{
			if (sk == null)
			{
				StashKey psk = RTC_StockpileManager.GetCurrentSavestateStashkey();
				if (psk == null)
				{
					MessageBox.Show("Could not perform the CORRUPT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
					return;
				}
				sk = (StashKey)psk.Clone();
			}

			StashKey newSk = (StashKey)sk.Clone();

			BlastLayer bl = GenerateBlastLayers(true);
			if (bl == null)
				return;
			newSk.BlastLayer = bl;

			newSk.Run();
		}

		private void btnSendTo_Click(object sender, EventArgs e)
		{
			if (sk == null)
			{
				StashKey psk = RTC_StockpileManager.GetCurrentSavestateStashkey();
				if (psk == null)
				{
					MessageBox.Show("Could not perform the CORRUPT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
					return;
				}
				sk = (StashKey)psk.Clone();
			}

			StashKey newSk = (StashKey)sk.Clone();
			BlastLayer bl = GenerateBlastLayers(true);
			if (bl == null)
				return;
			newSk.BlastLayer = bl;

			if (openedFromBlastEditor)
			{
				if (S.GET<RTC_BlastEditor_Form>() != null)
				{
					//TODO
					//	S.GET<RTC_BlastEditor_Form>().ImportBlastLayer(newSk.BlastLayer);
				}
			}
			else
			{
				RTC_StockpileManager.StashHistory.Add(newSk);
				S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
				S.GET<RTC_GlitchHarvester_Form>().dgvStockpile.ClearSelection();
				S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.ClearSelected();

				S.GET<RTC_GlitchHarvester_Form>().DontLoadSelectedStash = true;
				S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.SelectedIndex = S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.Items.Count - 1;
				RTC_StockpileManager.CurrentStashkey = RTC_StockpileManager.StashHistory[S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.SelectedIndex];
			}
		}


		private void dgvBlastGenerator_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (!initialized || dgvBlastGenerator == null)
				return;

			if (e.ColumnIndex != (int)BlastGeneratorColumn.DgvRowDirty)
				dgvBlastGenerator.Rows[e.RowIndex].Cells["dgvRowDirty"].Value = true;

			if ((BlastGeneratorColumn)e.ColumnIndex == BlastGeneratorColumn.DgvType)
			{
				PopulateModeCombobox(dgvBlastGenerator.Rows[e.RowIndex]);
			}
			if ((BlastGeneratorColumn)e.ColumnIndex == BlastGeneratorColumn.DgvDomain)
			{
				UpdateAddressRange(dgvBlastGenerator.Rows[e.RowIndex]);
			}
		}

		private void dgvBlastGenerator_MouseClick(object sender, MouseEventArgs e)
		{
			int currentMouseOverColumn = dgvBlastGenerator.HitTest(e.X, e.Y).ColumnIndex;
			int currentMouseOverRow = dgvBlastGenerator.HitTest(e.X, e.Y).RowIndex;

			if (e.Button == MouseButtons.Left)
			{
				if (currentMouseOverRow == -1)
				{
					dgvBlastGenerator.EndEdit();
					dgvBlastGenerator.ClearSelection();
				}
			}
		}

		public BlastLayer GenerateBlastLayers(bool useStashkey = false)
		{
			try
			{
				BlastLayer bl = new BlastLayer();

				if (useStashkey)
				{
					//If opened from engine config, use the GH state
					if (!openedFromBlastEditor)
					{
						StashKey psk = RTC_StockpileManager.GetCurrentSavestateStashkey();
						if (psk == null)
						{
							MessageBox.Show(
								"The Blast Generator could not perform the CORRUPT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
							return null;
						}

						sk = (StashKey)psk.Clone();
					}
				}

				List<BlastGeneratorProto> protoList = new List<BlastGeneratorProto>();

				foreach (DataGridViewRow row in dgvBlastGenerator.Rows)
				{
					BlastGeneratorProto proto;
					if ((bool)row.Cells["dgvRowDirty"].Value == true)
					{
						proto = CreateProtoFromRow(row);
						row.Cells["dgvBlastProtoReference"].Value = proto;
					}
					else
					{
						proto = (BlastGeneratorProto)row.Cells["dgvBlastProtoReference"].Value;
					}

					protoList.Add(proto);
				}


				List<BlastGeneratorProto> returnList = new List<BlastGeneratorProto>();



				returnList = (List<BlastGeneratorProto>)LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.BLASTGENERATOR_BLAST, new object[] {protoList, sk}, true);

				if (returnList == null) return null;

				//The return list is in the same order as the original list so we can go by index here
				for (int i = 0; i < dgvBlastGenerator.RowCount; i++)
				{
					dgvBlastGenerator.Rows[i].Cells["dgvBlastProtoReference"].Value = returnList[i];
					dgvBlastGenerator.Rows[i].Cells["dgvRowDirty"].Value = false;

					bl.Layer.AddRange(returnList[i].Bl.Layer);
				}

				return bl;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong when generating the blastlayers. \n" +
				                "Are you sure your input is correct and you have the correct core loaded?\n\n" +
				                ex.ToString());
				return null;
			}
			finally
			{
			}
		}

		//We don't always know how the parameters are going to be used
		//Because of this, we create the prototype using the raw value and flip the bytes as needed inside the generator
		//An example would be that the param could be an address, or a value
		//If we flipped to big endian here, that'd goof the handling of addresses
		//As such, we always create the prototype with the raw value.
		private BlastGeneratorProto CreateProtoFromRow(DataGridViewRow row)
		{
			try
			{
				string note;
				if (cbUnitsShareNote.Checked)
				{
					var value = row.Cells["dgvNoteText"].Value;
					if (value != null)
						note = value.ToString();
					else
						note = String.Empty;
				}
				else
					note = String.Empty;


				string domain = row.Cells["dgvDomain"].Value.ToString();
				string type = row.Cells["dgvType"].Value.ToString();
				string mode = row.Cells["dgvMode"].Value.ToString();
				int precision = GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString());
				int stepSize = Convert.ToInt32(row.Cells["dgvStepSize"].Value);
				long startAddress = Convert.ToInt64(row.Cells["dgvStartAddress"].Value);
				long endAddress = Convert.ToInt64(row.Cells["dgvEndAddress"].Value);
				long param1 = Convert.ToInt64(row.Cells["dgvParam1"].Value);
				long param2 = Convert.ToInt64(row.Cells["dgvParam2"].Value);

				return new BlastGeneratorProto(note, type, domain, mode, precision, stepSize, startAddress, endAddress, param1, param2);
			}catch(Exception ex)
			{
				throw;
			}
		}

		private string GetPrecisionNameFromSize(int precision)
		{
			switch (precision)
			{
				case 1:
					return "8-bit";

				case 2:
					return "16-bit";

				case 4:
					return "32-bit";

				default:
					return null;
			}
		}

		private int GetPrecisionSizeFromName(string precision)
		{
			switch (precision)
			{
				case "8-bit":
					return 1;

				case "16-bit":
					return 2;

				case "32-bit":
					return 4;

				default:
					return -1;
			}
		}

		private void btnAddRow_Click(object sender, EventArgs e)
		{
			AddDefaultRow();
		}

		private void btnNudgeStartAddressUp_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvStartAddress", updownNudgeStartAddress.Value);
		}

		private void btnNudgeStartAddressDown_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvStartAddress", updownNudgeStartAddress.Value, true);
		}

		private void btnNudgeEndAddressUp_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvEndAddress", updownNudgeEndAddress.Value);
		}

		private void btnNudgeEndAddressDown_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvEndAddress", updownNudgeEndAddress.Value, true);
		}

		private void btnNudgeParam1Up_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvParam1", updownNudgeParam1.Value);
		}

		private void btnNudgeParam1Down_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvParam1", updownNudgeParam1.Value, true);
		}

		private void btnNudgeParam2Up_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvParam2", updownNudgeParam2.Value);
		}

		private void btnNudgeParam2Down_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvParam2", updownNudgeParam2.Value, true);
		}

		private void NudgeParams(string column, decimal amount, bool shiftDown = false)
		{
			if (shiftDown)
				foreach (DataGridViewRow selected in dgvBlastGenerator.SelectedRows)
				{
					if ((Convert.ToDecimal(selected.Cells[column].Value) - amount) >= 0)

						selected.Cells[column].Value = Convert.ToDecimal(selected.Cells[column].Value) - amount;
					else
						selected.Cells[column].Value = 0;
				}
			else
			{
				foreach (DataGridViewRow selected in dgvBlastGenerator.SelectedRows)
				{
					decimal max = ((DataGridViewNumericUpDownCell)selected.Cells[column]).Maximum;

					if ((Convert.ToDecimal(selected.Cells[column].Value) - amount) <= max)
						selected.Cells[column].Value = Convert.ToDecimal(selected.Cells[column].Value) + amount;
					else
						selected.Cells[column].Value = max;
				}
			}
		}

		private void btnHideSidebar_Click(object sender, EventArgs e)
		{
			if (btnHideSidebar.Text == "▶")
			{
				panelSidebar.Visible = false;
				btnHideSidebar.Text = "◀";
			}
			else
			{
				panelSidebar.Visible = true;
				btnHideSidebar.Text = "▶";
			}
		}

		private void RefreshDomains()
		{
			try
			{
				S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected();
				domainToMiDico.Clear();
				domains = RTC_MemoryDomains.MemoryInterfaces.Keys.Concat(RTC_MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();
				foreach (string domain in domains)
				{
					domainToMiDico.Add(domain, RTC_MemoryDomains.GetInterface(domain));
				}

				foreach (DataGridViewRow row in dgvBlastGenerator.Rows)
					PopulateDomainCombobox(row);
			}
			catch (Exception ex)
			{
				throw new Exception(
							"An error occurred in RTC while refreshing the domains\n" +
							"Are you sure you don't have an invalid domain selected?\n" +
							"Make sure any VMDs are loaded and you have the correct core loaded in Bizhawk\n" +
							ex.ToString()
							);
			}
		}

		private void btnRefreshDomains_Click(object sender, EventArgs e)
		{
			RefreshDomains();
		}

		private void SaveDataGridView(DataGridView dgv)
		{
			DataTable dt = new DataTable();
			foreach (DataGridViewColumn column in dgv.Columns)
			{
				dt.Columns.Add();
			}

			object[] cellValues = new object[dgv.Columns.Count];
			foreach (DataGridViewRow row in dgv.Rows)
			{
				for (int i = 0; i < row.Cells.Count; i++)
				{
					cellValues[i] = row.Cells[i].Value;
				}
				dt.Rows.Add(cellValues);
			}

			DataSet ds = new DataSet();
			ds.Tables.Add(dt);
			SaveFileDialog sfd = new SaveFileDialog
			{
				Filter = "bg|*.bg"
			};
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				try
				{
					ds.Tables[0].WriteXml(sfd.FileName);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
		}

		private bool importDataGridView(DataGridView dgv)
		{
			return loadDataGridView(dgv, true);
		}

		private bool loadDataGridView(DataGridView dgv, bool import = false)
		{
			DataSet ds = new DataSet();
			OpenFileDialog ofd = new OpenFileDialog
			{
				Filter = "bg|*.bg"
			};
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				try
				{
					DataTable dt = new DataTable();
					ds.Tables.Add(dt);
					foreach (DataGridViewColumn column in dgv.Columns)
						dt.Columns.Add();
					ds.Tables[0].ReadXml(ofd.FileName);
					if (!import)
					{
						foreach (DataGridViewRow row in dgv.Rows)
							dgv.Rows.Remove(row);
					}

					foreach (DataRow row in ds.Tables[0].Rows)
					{
						dgv.Rows.Add();

						int lastrow = dgvBlastGenerator.RowCount - 1;

						//We need to populate the comboboxes or else things go bad
						//To do this, we load the type first so we can populate the modes, then add the domain to the domain combobox.
						//If it's invalid, they'll know on generation. If they load the correct core and press refresh, it'll maintain its selection
						dgv[(int)BlastGeneratorColumn.DgvType, lastrow].Value = row.ItemArray[(int)BlastGeneratorColumn.DgvType];

						PopulateModeCombobox(dgv.Rows[lastrow]);
						(dgv.Rows[lastrow].Cells["dgvDomain"] as DataGridViewComboBoxCell)?.Items.Add(row.ItemArray[(int)BlastGeneratorColumn.DgvDomain]);

						for (int i = 0; i < dgv.Rows[lastrow].Cells.Count; i++)
						{
							dgv.Rows[lastrow].Cells[i].Value = row.ItemArray[i];
						}
						//Override these two
						dgv[(int)BlastGeneratorColumn.dgvBlastProtoReference, lastrow].Value = null;
						dgv[(int)BlastGeneratorColumn.DgvRowDirty, lastrow].Value = true;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
					return false;
				}
			}
			return true;
		}

		private void loadFromFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			loadDataGridView(dgvBlastGenerator);
		}

		private void saveAsToFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveDataGridView(dgvBlastGenerator);
		}

		private void importBlastlayerblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			importDataGridView(dgvBlastGenerator);
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			System.Diagnostics.ProcessStartInfo sInfo = new System.Diagnostics.ProcessStartInfo("https://corrupt.wiki/corruptors/rtc-real-time-corruptor/blast-generator.html");
			System.Diagnostics.Process.Start(sInfo);
		}
		
		private void dgvBlastGenerator_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			// Note handling
			if (e != null)
			{
				DataGridView senderGrid = (DataGridView)sender;

				if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
					e.RowIndex >= 0)
				{
					DataGridViewCell cell = dgvBlastGenerator.Rows[e.RowIndex].Cells["dgvNoteText"];
					string note = cell.Value == null ? "" : cell.Value.ToString();

					/*
					if (RTC_NoteEditor_Form.CurrentlyOpenNoteForm == null)
					{
						RTC_NoteEditor_Form.CurrentlyOpenNoteForm = new RTC_NoteEditor_Form(note, "BlastGenerator", cell);
					}
					else
					{
						if (RTC_NoteEditor_Form.CurrentlyOpenNoteForm.Visible)
							RTC_NoteEditor_Form.CurrentlyOpenNoteForm.Close();

						RTC_NoteEditor_Form.CurrentlyOpenNoteForm = new RTC_NoteEditor_Form(note, "BlastGenerator", cell);
					}
					*/
					return;
				}
			}
		}

		public void RefreshNoteIcons()
		{
			foreach (DataGridViewRow row in dgvBlastGenerator.Rows)
			{
				DataGridViewCell textCell = row.Cells["dgvNoteText"];
				DataGridViewCell buttonCell = row.Cells["dgvNoteButton"];

				buttonCell.Value = string.IsNullOrWhiteSpace(textCell.Value?.ToString()) ? string.Empty : "📝";
				
			}
		}
	}
}
