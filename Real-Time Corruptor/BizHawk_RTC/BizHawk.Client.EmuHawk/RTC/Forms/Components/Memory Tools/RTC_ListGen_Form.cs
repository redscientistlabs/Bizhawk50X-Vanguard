using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_ListGen_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

		long currentDomainSize = 0;

		public RTC_ListGen_Form()
		{
			InitializeComponent();
		}

		public long SafeStringToLong(string input)
		{
			if (input.ToUpper().Contains("0X"))
				return long.Parse(input.Substring(2), NumberStyles.HexNumber);
			else
				return long.Parse(input, NumberStyles.HexNumber);
		}

		private void btnGenerateList_Click(object sender, EventArgs e) => GenerateList();

		private bool GenerateList()
		{
			List<String> newList = new List<string>();
			foreach (string line in tbListValues.Lines)
			{
				if (string.IsNullOrWhiteSpace(line))
					continue;

				string trimmedLine = line.Trim();

				string[] lineParts = trimmedLine.Split('-');

				if (lineParts.Length > 1)
				{
					long start = SafeStringToLong(lineParts[0]);
					long end = SafeStringToLong(lineParts[1]);

					for (long i = start; i < end; i++)
					{
						newList.Add(i.ToString("X"));
					}
				}
				else
				{
					newList.Add(lineParts[0]);
				}
			}

			String filename = tbListName.Text.MakeSafeFilename('-');
			//Handle saving the list to a file
			if (cbSaveFile.Checked)
			{
				if (!String.IsNullOrWhiteSpace(filename))
				{
					File.WriteAllLines(RTC_Core.rtcDir + "//LISTS//" + filename + ".txt", newList);
				}
				else
				{
					MessageBox.Show("Filename is empty. Unable to save your list to a file");
				}
			}

			//If there's no name just generate one
			if (String.IsNullOrWhiteSpace(filename))
				filename = RTC_Core.GetRandomKey();

			//Register the list and update netcore


			List<Byte[]> byteList = new List<byte[]>();
			foreach (string t in newList)
			{
				byte[] bytes = RTC_Extensions.StringToByteArray(t);
				byteList.Add(bytes);
			}
			string hash = RTC_Filtering.RegisterList(byteList, true);

			//Register the list in the ui
			S.GET<RTC_EngineConfig_Form>().RegisterList(filename, hash);


			return true;
		}

		private void RTC_ListGen_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
				e.Cancel = true;
				this.RestoreToPreviousPanel();
				return;
			}
		}

		private void RTC_ListGen_Form_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && (sender as ComponentForm).FormBorderStyle == FormBorderStyle.None)
			{
				Point locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);
				ContextMenuStrip columnsMenu = new ContextMenuStrip();
				columnsMenu.Items.Add("Detach to window", null, new EventHandler((ob, ev) =>
				{
					(sender as ComponentForm).SwitchToWindow();
				}));
				columnsMenu.Show(this, locate);
			}
		}

		private void btnRefreshListsFromFile_Click(object sender, EventArgs e)
		{
			S.GET<RTC_EngineConfig_Form>().LoadLists();
		}
	}
}
