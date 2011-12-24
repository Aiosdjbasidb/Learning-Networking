// -----------------------------------------------------------------------
// <copyright file="Server.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Net.Sockets;
using Microsoft.Xna.Framework;
using XnaMultiplayerGame.Classes;
using XnaMultiplayerGame.Helpers;

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

		public enum DataType
		{
			Join,
			Disconnect
		}

		public static void Initialize()
		{
			ConnectedClients = new Client[100];
		}

		public static void Update()
		{
			AcceptPendingConnections();
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
					if (int.TryParse(messages[0], out type))
					{
						switch (type)
						{
							case (int) DataType.Join:
								{
									int id = GetFreeSlotId();

									ConnectedClients[id] = new Client(connection,
									                                  new Player(Vector2.Zero, Player.Size, Helper.GetRandomColor(), connection));
									int x = (int)ConnectedClients[id].Player.Position.X;
									int y = (int)ConnectedClients[id].Player.Position.Y;
									Color c = ConnectedClients[id].Player.DrawColor;

									NetHelper.SendMessageTo(connection,
									                        NetHelper.BuildMessage((float) LocalClient.DataType.NewPlayerInfo, id, x, y, (int)Player.Size.X,
									                                               (int)Player.Size.Y, c.R, c.G, c.B));

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