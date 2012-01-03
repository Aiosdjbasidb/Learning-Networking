// -----------------------------------------------------------------------
// <copyright file="Server.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Lidgren.Network;
using Microsoft.Xna.Framework;
using XnaMultiplayerGame.Classes;
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
		public static NetServer NetServer { get; private set; }
		public static List<Client> Clients { get; private set; }

		public static void Initialize()
		{
			NetServer = new NetServer(new NetPeerConfiguration("XnaMultiplayerGame")
			                          	{
			                          		Port = 5555
			                          	});

			NetServer.RegisterReceivedCallback(MessageReceived);

			NetServer.Start();

			Clients = new List<Client>();
		}

		public static void Stop()
		{
			NetServer.Shutdown("Server shutting down");
		}

		public static void Update()
		{
			if (NetServer.Status == NetPeerStatus.NotRunning) return;

			float elapsed = TimeManager.ActualElapsed;

			UpdateClients();
		}

		private static void UpdateClients()
		{
			foreach (var c in Clients)
			{
				c.Update();
			}
		}

		private static void MessageReceived(object peer)
		{
			NetIncomingMessage msg;
			while ((msg = NetServer.ReadMessage()) != null)
			{
				switch (msg.MessageType)
				{
					case NetIncomingMessageType.Data:
						{
							var type = new ServerMessageType(msg.ReadInt32());

							switch (type.Type)
							{
								case Headers.Server.SetMoveDirection:
									{
										float x = msg.ReadFloat();
										float y = msg.ReadFloat();

										var client = Client.FromConnection(msg.SenderConnection, Clients);

										client.Player.MoveDirection = new Vector2(x, y);

										break;
									}
								case Headers.Server.SetVelocity:
									{
										float x = msg.ReadFloat();
										float y = msg.ReadFloat();

										var client = Client.FromConnection(msg.SenderConnection, Clients);

										client.Player.Velocity = new Vector2(x, y);

										break;
									}
								case Headers.Server.GetPlayers: // Player information is ordered like the following: x, y, velX, velY, r, g, b, a
									{
										int id = msg.ReadInt32();

										NetOutgoingMessage message = NetServer.CreateMessage();

										message.Write((int)Headers.Client.ServerPlayers);

										foreach (Client c in Clients)
										{
											message.Write(c.Player.Position.X);
											message.Write(c.Player.Position.Y);
											message.Write(c.Player.Velocity.X);
											message.Write(c.Player.Velocity.Y);
											message.Write(c.Player.DrawColor.R);
											message.Write(c.Player.DrawColor.G);
											message.Write(c.Player.DrawColor.B);
											message.Write(c.Player.DrawColor.A);
											message.Write(c.Player.MoveDirection.X);
											message.Write(c.Player.MoveDirection.Y);
										}

										Clients[id].Connection.SendMessage(message, NetDeliveryMethod.ReliableOrdered, 1);

										break;
									}
								case Headers.Server.GetPlatforms:
									{
										int id = msg.ReadInt32();

										NetOutgoingMessage message = NetServer.CreateMessage();

										message.Write((int)Headers.Client.ServerPlatforms);

										foreach (Platform p in PlatformWorld.Platforms)
										{
											message.Write(p.BoundingBox.X);
											message.Write(p.BoundingBox.Y);
											message.Write(p.BoundingBox.Width);
											message.Write(p.BoundingBox.Height);
										}

										Clients[id].Connection.SendMessage(message, NetDeliveryMethod.ReliableOrdered, 2);

										break;
									}
							}

							break;
						}
					case NetIncomingMessageType.StatusChanged:
						{
							var status = (NetConnectionStatus)msg.ReadByte();

							switch (status)
							{
								case NetConnectionStatus.Connected:
									{
										string name = msg.SenderConnection.RemoteHailMessage.ReadString();
										int id = FindAvailableId();

										Console.WriteLine("(" + msg.SenderConnection.RemoteEndpoint + ") " + name + " connected.");
										Console.WriteLine("Giving id: " + id + ".");

										Clients.Add(new Client(msg.SenderConnection, name, id));

										break;
									}
								case NetConnectionStatus.Disconnected:
									{
										var client = Client.FromConnection(msg.SenderConnection, Clients);

										//if (client == null) break; // If the client isn't in the list of connected clients, ignore it.

										Clients.Remove(client);

										Console.WriteLine("(" + client.Connection.RemoteEndpoint + ") " + client.Name + " disconnected.");

										break;
									}
							}

							break;
						}
					default:
						{
							Console.WriteLine(msg.ReadString());
							break;
						}
				}
			}
		}

		private static int FindAvailableId()
		{
			return Clients.Count();
		}
	}
}