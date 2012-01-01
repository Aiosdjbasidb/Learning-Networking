// -----------------------------------------------------------------------
// <copyright file="LocalClient.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
	public static class LocalClient
	{
		private const int MoveSpeed = 300;

		public static NetClient NetClient { get; private set; }
		public static Player Player { get; set; }
		public static int Id { get; set; }
		public static string Name { get; set; }

		private static List<Player> _serverPlayers;

		private static float _sendInterval = .1f;
		private static float _elapsed = 0f;

		public static void Initialize()
		{
			NetClient =
				new NetClient(new NetPeerConfiguration("XnaMultiplayerGame")
				              	{
				              		AcceptIncomingConnections = false
				              	});

			if (Program.Hosting)
			{
				ConnectLocalHost();
			}
			else
			{
				Connect(Program.Ip);
			}

			_serverPlayers = new List<Player>();
		}

		public static void ConnectLocalHost()
		{
			Connect("127.0.0.1");
		}

		public static void Connect(string ip)
		{
			NetClient.Start();

			var hailMessage = NetClient.CreateMessage();
			hailMessage.Write(Name);

			NetClient.Connect(ip, 5555, hailMessage);

			Console.WriteLine(NetClient.ConnectionStatus);
		}

		public static void Disconnect()
		{
			NetClient.Disconnect("Client disconnecting.");

			Console.WriteLine(NetClient.ConnectionStatus);
		}

		public static void Update()
		{
			float elapsed = TimeManager.Elapsed;

			_elapsed += TimeManager.ActualElapsed;

			if (_elapsed >= _sendInterval)
			{
				_elapsed = 0f;

				GetServerPlayers();
				GetServerPlatforms();
			}

			// Receive network data.
			NetIncomingMessage msg;
			while ((msg = NetClient.ReadMessage()) != null)
			{
				switch (msg.MessageType)
				{
					case NetIncomingMessageType.Data:
						{
							var type = new ClientMessageType(msg.ReadInt32());

							switch (type.Type)
							{
								case Headers.Client.NewPlayerInfo:
									{
										int id = msg.ReadInt32();
										float x = msg.ReadFloat();
										float y = msg.ReadFloat();
										int w = msg.ReadInt32();
										int h = msg.ReadInt32();
										Color drawColor = new Color(msg.ReadByte(), msg.ReadByte(), msg.ReadByte(), msg.ReadByte());

										Id = id;
										Player = new Player(new Vector2(x, y), new Vector2(w, h), drawColor);

										break;
									}
								case Headers.Client.PlayerInfo:
									{
										float x = msg.ReadFloat();
										float y = msg.ReadFloat();
										float velX = msg.ReadFloat();
										float velY = msg.ReadFloat();

										Player.Position = new Vector2(x, y);
										Player.Velocity = new Vector2(velX, velY);

										break;
									}
								case Headers.Client.ServerPlayers:
									{
										var plrs = new List<Player>();

										while (msg.Position != msg.LengthBits)
										{
											float x = msg.ReadFloat();
											float y = msg.ReadFloat();
											float velX = msg.ReadFloat();
											float velY = msg.ReadFloat();
											Color drawColor = new Color(msg.ReadByte(), msg.ReadByte(), msg.ReadByte(), msg.ReadByte());

											plrs.Add(new Player(new Vector2(x, y), Player.PlayerSize, drawColor));
										}

										_serverPlayers = plrs;

										break;
									}
								case Headers.Client.ServerPlatforms:
									{
										var platforms = new List<Platform>();

										while (msg.Position != msg.LengthBits)
										{
											int x = msg.ReadInt32();
											int y = msg.ReadInt32();
											int width = msg.ReadInt32();
											int height = msg.ReadInt32();

											platforms.Add(new Platform(new Rectangle(x, y, width, height)));
										}

										PlatformWorld.Platforms = platforms;

										break;
									}
							}

							break;
						}
					case NetIncomingMessageType.StatusChanged:
						{
							var status = (NetConnectionStatus) msg.ReadByte();

							if (Program.Debugging)
								Console.WriteLine("Status changed to: " + status);

							if (status == NetConnectionStatus.Disconnected)
							{
								Console.WriteLine("Server shutdown");
								Environment.Exit(0);
							}

							break;
						}
					default:
						{
							if (Program.Debugging)
								Console.WriteLine("Unhandled " + msg.MessageType + ". Contained string: \"" + msg.ReadString() + "\".");

							break;
						}
				}
			}

			// Update player
			if (Player != null)
			{
				// Update input
				if (InputManager.InputManager.KeyPressed(Keys.A))
				{
					Player.Move(-MoveSpeed*elapsed, 0);
				}
				if (InputManager.InputManager.KeyPressed(Keys.D))
				{
					Player.Move(MoveSpeed*elapsed, 0);
				}
				if (InputManager.InputManager.KeyPressed(Keys.W))
				{
					Player.Velocity = new Vector2(Player.Velocity.X, Player.Velocity.Y - (4000 * elapsed));
				}
				if (InputManager.InputManager.KeyJustPressed(Keys.Space))
				{
					Player.Velocity = new Vector2(Player.Velocity.X, -600);
				}

				ServerSetPosition(Player.Position);
				ServerSetVelocity(Player.Velocity);

				Player.UpdatePhysics(PlatformWorld.Platforms.ToArray());
			}

			// Update other "local" players.
			foreach (Player p in _serverPlayers)
			{
				if(p.DrawColor != Player.DrawColor)
					p.UpdatePhysics(PlatformWorld.Platforms.ToArray());
			}
		}

		public static void Draw(SpriteBatch sb)
		{
			foreach (Player p in _serverPlayers)
			{
				if (p.DrawColor != Player.DrawColor)
					p.Draw(sb);
			}

			if (Player != null)
			{
				Player.Draw(sb);
			}
		}

		private static void ServerSetPosition(Vector2 position)
		{
			var msg = NetClient.CreateMessage();

			msg.Write((int)Headers.Server.SetPosition);
			msg.Write(Player.Position.X);
			msg.Write(Player.Position.Y);

			NetClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
		}

		private static void ServerSetVelocity(Vector2 velocity)
		{
			var msg = NetClient.CreateMessage();

			msg.Write((int) Headers.Server.SetVelocity);
			msg.Write(Player.Velocity.X);
			msg.Write(Player.Velocity.Y);

			NetClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, 0);
		}

		private static void GetServerPlayers()
		{
			var msg = NetClient.CreateMessage();

			msg.Write((int) Headers.Server.GetPlayers);
			msg.Write(Id);

			NetClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, 1);
		}

		private static void GetServerPlatforms()
		{
			var msg = NetClient.CreateMessage();

			msg.Write((int) Headers.Server.GetPlatforms);
			msg.Write(Id);

			NetClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, 2);
		}
	}
}