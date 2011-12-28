// -----------------------------------------------------------------------
// <copyright file="Server.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Net.Sockets;
using Microsoft.Xna.Framework;
using XnaMultiplayerGame.Classes;
using XnaMultiplayerGame.Helpers;
using XnaMultiplayerGame.Managers;

namespace XnaMultiplayerGame.Network
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public static class Server
	{
		public static Client[] ConnectedClients { get; private set; }

		private const float NetworkSendUpdateDelay = .1f; // Wait 10 ms before sending updates again.
		private static float _networkDelayTime;

		public enum RequestType
		{
			Join,
			Disconnect,
			GetPlatforms,
			GetPlayers
		}

		public static void Initialize()
		{
			ConnectedClients = new Client[100];
		}

		public static void Update()
		{
			AcceptPendingConnections();

			foreach (Client c in ConnectedClients)
			{
				if (c == null) continue;

				c.Update();
			}

			_networkDelayTime += TimeManager.ActualElapsed;

			if (_networkDelayTime >= NetworkSendUpdateDelay)
			{
				Console.WriteLine("Sending updates to clients");
				_networkDelayTime = 0f;
				SendServerData();
			}
		}

		private static void SendServerData()
		{
			foreach (Client client in ConnectedClients)
			{
				if (client == null) continue;

				client.SendUpdateData();

				/*while (client.TcpClient.Available > 0)
				{
					string fullMessage = NetHelper.ReceiveStringMessageFrom(client.TcpClient);
					string[] messages = fullMessage.Split(NetHelper.SplitChar);

					int type;
					if (Int32.TryParse(messages[0], out type))
					{
						switch (type)
						{
							case (int)RequestType.GetPlatforms:
								{
									string platformBuffer = "";

									foreach (Platform p in PlatformWorld.Platforms)
									{
										platformBuffer += p.Position.X + ":" + p.Position.Y + ":" + p.BoundingBox.Width + ":" + p.BoundingBox.Height + ";";
									}

									NetHelper.SendMessageTo(client.TcpClient,
																NetHelper.BuildMessage((int)LocalClient.DataType.Platforms) + platformBuffer);

									break;
								}
						}
					}
				}*/
			}
		}

		private static void AcceptPendingConnections()
		{
			var pending = Listener.PendingConnections.ToArray();

			foreach (var connection in pending)
			{
				if (connection.Available > 0)
				{
					string[] messages = NetHelper.ReceiveMessageFrom(connection);
					int type;
					if (Int32.TryParse(messages[0], out type))
					{
						switch (type)
						{
							case (int) RequestType.Join:
								{
									int id = GetFreeSlotId();

									ConnectedClients[id] = new Client(connection,
									                                  new Player(Vector2.Zero, Player.TextureSize, Helper.GetRandomColor(), connection));
									int x = (int)ConnectedClients[id].Player.Position.X;
									int y = (int)ConnectedClients[id].Player.Position.Y;
									Color c = ConnectedClients[id].Player.DrawColor;

									NetHelper.SendMessageTo(connection,
									                        NetHelper.BuildMessage((float) LocalClient.DataType.NewPlayerInfo, id, x, y, (int)Player.TextureSize.X,
									                                               (int)Player.TextureSize.Y, c.R, c.G, c.B));

									Listener.PendingConnections.Remove(connection);
									break;
								}
						}
					}
				}
			}
		}

		private static int GetFreeSlotId()
		{
			return ConnectedClients.Count(c => c != null);
		}
	}
}