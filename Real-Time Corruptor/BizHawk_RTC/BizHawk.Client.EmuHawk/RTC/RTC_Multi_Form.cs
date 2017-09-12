using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_Multi_Form : Form
	{
		
		System.Windows.Forms.Timer streamTimer = null;


		public int GameOfSwapCounter
		{
			get
			{
				return _GameOfSwapCounter;
			}
			set
			{
				_GameOfSwapCounter = value;

				if (_GameOfSwapCounter >= 0)
					UpdateRedBar(_GameOfSwapCounter * 4);
				else
					UpdateRedBar(0);
			}
		}
		int _GameOfSwapCounter = 0;
		bool GameOfSwapHost = false;
		public System.Windows.Forms.Timer GameOfSwapTimer = null;

		int fps = 30;

		public RTC_Multi_Form()
		{
			InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.AutoSize = false;
        }

		private void RTC_Multi_Form_Load(object sender, EventArgs e)
		{
			//assing event handlers here

			RTC_Core.Multiplayer = new RTC_NetCore();

			RTC_Core.Multiplayer.ClientConnecting += Multiplayer_ClientConnecting;
			RTC_Core.Multiplayer.ClientConnected += Multiplayer_ClientConnected;
			RTC_Core.Multiplayer.ClientDisconnected += Multiplayer_ClientDisconnected;
			RTC_Core.Multiplayer.ClientConnectionLost += Multiplayer_ClientConnectionLost;
			RTC_Core.Multiplayer.ClientReconnecting += Multiplayer_ClientReconnecting;

			RTC_Core.Multiplayer.ServerStarted += Multiplayer_ServerStarted;
			RTC_Core.Multiplayer.ServerConnected += Multiplayer_ServerConnected;
			RTC_Core.Multiplayer.ServerDisconnected += Multiplayer_ServerDisconnected;
			RTC_Core.Multiplayer.ServerConnectionLost += Multiplayer_ServerConnectionLost;

			cbStreamFps.SelectedIndex = 2;

			string[] cheekyHeadlines =
			{
				"Yes, it really works™",
				"Yes, it really works™",
				"Yes, it really works™",
				"Yes, it really works™",
				"Yes, it really works™",
				"Yes, it really works™",
				"Yes, it really works™",
				"Yes, it really works™",
				"Yes, it really works™",
				"Yes, it really works™",
				"Yes, it really works™",
				"Yes, it really works™",
				"Yes, it really works™",
				"Yes, it really works™",
				"Yes, it really works™",
				"Yes, it really works™",
				"Some sort of netplay",
			};

			lbCheekyHeadline.Text = cheekyHeadlines[RTC_Core.RND.Next(cheekyHeadlines.Length)];

		}

		private void Multiplayer_ServerConnectionLost(object sender, EventArgs e)
		{
			lbServerStatus.Text = "Server Status : friend has dropped";
		}

		private void Multiplayer_ServerDisconnected(object sender, EventArgs e)
		{
			tbClientAdress.Visible = true;
			btnStartClient.Visible = true;
			lbClientStatus.Visible = true;
			lbClient.Visible = true;

			lbServerStatus.Text = "Server Status : pre-heated";

			btnStartServer.Text = "Start Server";
		}

		private void Multiplayer_ServerConnected(object sender, EventArgs e)
		{
			lbServerStatus.Text = "Server Status : Someone said hi";
		}

		private void Multiplayer_ServerStarted(object sender, EventArgs e)
		{
			tbClientAdress.Visible = false;
			btnStartClient.Visible = false;
			lbClientStatus.Visible = false;
			lbClient.Visible = false;

			btnStartServer.Text = "Stop Server";

			if(!lbServerStatus.Text.Contains("dropped"))
				lbServerStatus.Text = "Server Status : the thing started or something";
		}

		private void Multiplayer_ClientReconnecting(object sender, EventArgs e)
		{
			btnStartClient.Text = $"Reconnecting";
		}

		private void Multiplayer_ClientConnectionLost(object sender, EventArgs e)
		{
			lbClientStatus.Text = "Client Status : very sad (connection lost)";
		}

		private void Multiplayer_ClientDisconnected(object sender, EventArgs e)
		{
			btnStartServer.Visible = true;
			lbServerStatus.Visible = true;
			lbServer.Visible = true;

			tbClientAdress.Enabled = true;
			lbClientStatus.Text = "Client Status : sad";

			btnStartClient.Text = "Connect to server";
		}

		private void Multiplayer_ClientConnected(object sender, EventArgs e)
		{
			btnStartClient.Text = "Disconnect";
			lbClientStatus.Text = "Client Status : actually looks kind of connected";
		}

		private void Multiplayer_ClientConnecting(object sender, EventArgs e)
		{
			btnStartServer.Visible = false;
			lbServerStatus.Visible = false;
			lbServer.Visible = false;

			btnStartClient.Text = "Connecting";
			tbClientAdress.Enabled = false;
			lbClientStatus.Text = "Client Status : Trying to spot the server from afar";
		}


		private void btnStartClient_Click(object sender, EventArgs e)
		{
			if(btnStartClient.Text == "Disconnect" || btnStartClient.Text == "Reconnecting" || btnStartClient.Text == "Connecting")
			{
				RTC_Core.Multiplayer.StopNetworking();
				return;
			}

			RTC_Core.Multiplayer.StartNetworking(NetworkSide.CLIENT);

		}








		private void btnStartServer_Click(object sender, EventArgs e)
		{
			if(btnStartServer.Text == "Stop Server")
			{
				RTC_Core.Multiplayer.StopNetworking();
				return;
			}

			RTC_Core.Multiplayer.StartNetworking(NetworkSide.SERVER);

		}


		private void btnPushBlastToServer_Click(object sender, EventArgs e)
		{
			RTC_Core.Multiplayer.SendBlastlayer();
		}


		private void tbShowIp_TextChanged(object sender, EventArgs e)
		{
			tbShowIp.Text = new WebClient().DownloadString("http://icanhazip.com");
		}

		private void btnPullGameFromServer_Click(object sender, EventArgs e)
		{
			RTC_Core.Multiplayer.SendCommand(new RTC_Command(CommandType.PULLROM), false);
		}

		private void btnPushGameToServer_Click(object sender, EventArgs e)
		{
			RTC_Core.Multiplayer.SendCommand(new RTC_Command(CommandType.PULLROM), true);
		}

		private void btnPullStateFromServer_Click(object sender, EventArgs e)
		{


			RTC_Core.Multiplayer.SendCommand(new RTC_Command(CommandType.PULLSTATE), false);
		}

		private void btnPushStateToServer_Click(object sender, EventArgs e)
		{

			RTC_Core.Multiplayer.SendCommand(new RTC_Command(CommandType.PULLSTATE), true);
		}

		private void tbServerPort_TextChanged(object sender, EventArgs e)
		{
			if (RTC_Core.Multiplayer.side == NetworkSide.DISCONNECTED)
				RTC_Core.Multiplayer.port = Convert.ToInt32(tbServerPort.Text);
			else
				tbServerPort.Text = RTC_Core.Multiplayer.port.ToString();
		}

		private void btnSwapGameState_Click(object sender, EventArgs e)
		{
			RTC_Core.Multiplayer.SwapGameState();
		}



		private void btnPushScreenToPear_Click(object sender, EventArgs e)
		{
			RTC_Core.Multiplayer.SendCommand(new RTC_Command(CommandType.PULLSCREEN) , true);
		}

		private void btnPullScreenToPear_Click(object sender, EventArgs e)
		{
			RTC_Core.Multiplayer.SendCommand(new RTC_Command(CommandType.PULLSCREEN), false);
		}

		public void cbStreamScreenToPeer_CheckedChanged(object sender, EventArgs e)
		{
			

			if(cbStreamScreenToPeer.Checked)
			{
				streamTimer = new System.Windows.Forms.Timer();
				streamTimer.Interval = (1000 / fps);
				streamTimer.Tick += StreamTimer_Tick;
				streamTimer.Start();
			}
			else
			{
				streamTimer.Stop();
				streamTimer = null;
			}
		}

		private void StreamTimer_Tick(object sender, EventArgs e)
		{
			if(GlobalWin.MainForm.Visible)
			{

				RTC_Command cmdBack = new RTC_Command(CommandType.PUSHSCREEN);

				Bitmap bmp = MainForm.MakeScreenshotImage().ToSysdrawingBitmap();

				if(cbCompressStream.Checked)
				cmdBack.screen = SaveJPG100(bmp, 60);
				else
				cmdBack.screen = bmp;
				RTC_Core.Multiplayer.PeerCommandQueue.AddLast(cmdBack);

			}
		}


		public static void SaveJPG100(Bitmap bmp, Stream stream)
		{
			EncoderParameters encoderParameters = new EncoderParameters(1);
			encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
			bmp.Save(stream, GetEncoder(ImageFormat.Jpeg), encoderParameters);
		}

		public static Image SaveJPG100(Bitmap bmp, long quality)
		{
			MemoryStream stream = new MemoryStream();
			EncoderParameters encoderParameters = new EncoderParameters(1);
			encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
			bmp.Save(stream, GetEncoder(ImageFormat.Jpeg), encoderParameters);

			return Image.FromStream(stream);
		}

		public static ImageCodecInfo GetEncoder(ImageFormat format)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

			foreach (ImageCodecInfo codec in codecs)
			{
				if (codec.FormatID == format.Guid)
				{
					return codec;
				}
			}

			return null;
		}


		public void SetStreamingFPS(int _fps)
		{
			fps = _fps;

			if(streamTimer != null)
			{
				streamTimer.Interval = (1000 / fps);
			}
		}

		private void rbStream30fps_CheckedChanged(object sender, EventArgs e)
		{
			SetStreamingFPS(30);
		}

		private void rbStream20fps_CheckedChanged(object sender, EventArgs e)
		{
			SetStreamingFPS(20);
		}

		private void rbStream10fps_CheckedChanged(object sender, EventArgs e)
		{
			SetStreamingFPS(10);
		}

		private void btnClearNetworkCache_Click(object sender, EventArgs e)
		{
			RTC_Core.Multiplayer.ClearNetowrkCache();
		}

		private void btnGameOfSwap_Click(object sender, EventArgs e)
		{
			if(btnGameOfSwap.ForeColor == Color.Red)
			{
				StopGameOfSwap();
				return;
			}

			if (RTC_Core.Multiplayer.side == NetworkSide.DISCONNECTED)
				return;

			StartGameOfSwap(true);

			
		}

		public void StopGameOfSwap(bool fromStopCommand = false)
		{
			if(GameOfSwapTimer != null)
			{
				GameOfSwapTimer.Stop();
				GameOfSwapTimer = null;
			}

			btnGameOfSwap.ForeColor = Color.White;
			GameOfSwapCounter = -1;

			if (!fromStopCommand)
				RTC_Core.Multiplayer.SendCommand(new RTC_Command(CommandType.GAMEOFSWAPSTOP), false);
		}

		public void StartGameOfSwap(bool isGameHost)
		{
			GameOfSwapHost = isGameHost;
			GameOfSwapTimer = new System.Windows.Forms.Timer();
			GameOfSwapTimer.Interval = 200;
			GameOfSwapTimer.Tick += GameOfSwapTimer_Tick;
			GameOfSwapTimer.Start();

			if (isGameHost)
				RTC_Core.Multiplayer.SendCommand(new RTC_Command(CommandType.GAMEOFSWAPSTART), false);

			GameOfSwapCounter = 64;

			if (!cbStreamScreenToPeer.Checked)
				cbStreamScreenToPeer.Checked = true;

			btnGameOfSwap.ForeColor = Color.Red;

		}

		private void GameOfSwapTimer_Tick(object sender, EventArgs e)
		{
			GameOfSwapCounter--;

			if (GameOfSwapCounter == 0 && GameOfSwapHost)
				RTC_Core.Multiplayer.SwapGameState();

		}

		private void btnPopoutPeerGameScreen_Click(object sender, EventArgs e)
		{
			RTC_Core.multipeerpopoutForm.Show();

			pbPeerScreen.Visible = false;
			pnPeerRedBar.Visible = false;
			btnPopoutPeerGameScreen.Visible = false;

		}

		public void UpdateRedBar(int sizeX)
		{
				pnPeerRedBar.Size = new Size(sizeX, 3);
				RTC_Core.multipeerpopoutForm.pnPeerRedBar.Size = new Size(Convert.ToInt32((Convert.ToDouble(sizeX) / 256f) * Convert.ToDouble(RTC_Core.multipeerpopoutForm.pnPlacer.Size.Width)), 5);
		}

		private void RTC_Multi_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
				e.Cancel = true;
				this.Hide();
			}
		}

		private void btnPushStashkeyToServer_Click(object sender, EventArgs e)
		{
			RTC_Core.Multiplayer.SendStashkey();
		}

		private void btnRequestStream_Click(object sender, EventArgs e)
		{
			RTC_Core.Multiplayer.SendCommand(new RTC_Command(CommandType.REQUESTSTREAM), false);
		}

		private void btnStartServer_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				Point locate = new Point((sender as Control).Location.X + (sender as Control).Parent.Location.X + e.Location.X, (sender as Control).Location.Y + (sender as Control).Parent.Location.Y + e.Location.Y);

				ContextMenuStrip columnsMenu = new ContextMenuStrip();
				(columnsMenu.Items.Add("BRUTAL DISCONNECT", null, new EventHandler((ob, ev) => { RTC_Core.Multiplayer.StopNetworking(true, false); })) as ToolStripMenuItem).Enabled = RTC_Core.Multiplayer.side != NetworkSide.DISCONNECTED;
				columnsMenu.Show(this, locate);
			}
		}

		private void btnSplitscreen_Click(object sender, EventArgs e)
		{
			if (btnSplitscreen.ForeColor == Color.Red)
			{
				RTC_Core.multipeerpopoutForm.SetSplitscreen(false);
				btnSplitscreen.ForeColor = Color.White;
				return;
			}

			RTC_Core.multipeerpopoutForm.SetSplitscreen(true);

			if (btnPopoutPeerGameScreen.Visible)
				btnPopoutPeerGameScreen_Click(null, null);

			RTC_Core.Multiplayer.SendCommand(new RTC_Command(CommandType.REQUESTSTREAM), false);
			cbStreamScreenToPeer.Checked = true;
			btnSplitscreen.ForeColor = Color.Red;

		}

		private void btnBlastBoard_Click(object sender, EventArgs e)
		{
			RTC_Core.sbForm.Show();
		}

		private void tbClientAdress_TextChanged(object sender, EventArgs e)
		{
			RTC_Core.Multiplayer.address = tbClientAdress.Text;
		}

		private void cbStreamFps_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetStreamingFPS(Convert.ToInt32(cbStreamFps.SelectedItem.ToString().Split('f')[0]));
		}
	}

	[Serializable()]
	public class RTC_Command
	{
		public CommandType Type;
		public CommandType ReturnedFrom;
		public bool Priority = false;
		public Guid? requestGuid = null;
		public object objectValue = null;

		public BlastLayer blastlayer = null;
		public bool isReplay = false;
		public byte[] romData = null;
		public string romFilename = null;
		public StashKey stashkey = null;

		public Image screen = null;

		public RTC_Command(CommandType _Type)
		{
			Type = _Type;
		}
	}

}
