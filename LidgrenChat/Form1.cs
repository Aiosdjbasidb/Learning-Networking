using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lidgren.Network;

namespace LidgrenChat
{
	public partial class k : Form
	{
		public k()
		{
			InitializeComponent();
		}
		
		public void AddMessage(string name, string message)
		{
			mainChat.Items.Add(DateTime.Now.ToShortTimeString() + " - " + name + ": " + message);
		}

		private void startServerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Server.Started)
			{
				Server.Initialize();
				startServerToolStripMenuItem.Text = "Stop server";
			}
			else
			{
				Server.Stop();
				startServerToolStripMenuItem.Text = "Start server";
			}
		}

		private void ConnectButtonClick(object sender, EventArgs e)
		{
			if (LocalClient.NetClient == default(NetClient) || LocalClient.NetClient.ConnectionStatus != NetConnectionStatus.Connected)
			{
				LocalClient.Initialize(ipBox.Text, 5555, nameBox.Text);
				connectButton.Text = "Disconnect";

				writeBox.Enabled = true;
				sendButton.Enabled = true;
				nameBox.Enabled = false;
				ipBox.Enabled = false;

				Program.Form.AddMessage("Program", "Connected to " + ipBox.Text + ":" + 5555);
			}
			else
			{
				LocalClient.Disconnect();
				connectButton.Text = "Connect";

				writeBox.Enabled = false;
				sendButton.Enabled = false;
				nameBox.Enabled = true;
				ipBox.Enabled = true;

				Program.Form.AddMessage("Program", "Disconnected from server.");
			}
		}

		private void SendButtonClick(object sender, EventArgs e)
		{
			LocalClient.SendMessage(writeBox.Text);
			ClearWriteBox();
		}

		private void ClearWriteBox()
		{
			writeBox.Clear();
			writeBox.ClearUndo();
		}

		private void nameBox_TextChanged(object sender, EventArgs e)
		{
			LocalClient.Name = nameBox.Text;
		}

		private void writeBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
			{
				if (writeBox.Text != string.Empty)
				{
					LocalClient.SendMessage(writeBox.Text);
					ClearWriteBox();
				}
			}
		}
	}
}