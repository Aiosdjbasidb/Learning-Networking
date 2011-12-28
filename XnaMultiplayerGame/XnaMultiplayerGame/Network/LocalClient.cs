// -----------------------------------------------------------------------
// <copyright file="LocalClient.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Net.Sockets;
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

		public static TcpClient TcpClient { get; private set; }
		public static Player Player { get; set; }
		public static int Id;

		private static List<Platform> _platforms;

		public enum DataType
		{
			/// <summary>
			/// Format: id:x:y:w:h
			/// </summary>
			NewPlayerInfo,

			/// <summary>
			/// Format: plrX:plrY:plrW:plrH:velX:velY
			/// </summary>
			UpdateInfo
		}

		public static void Initialize()
		{
			TcpClient = new TcpClient();
			_platforms = new List<Platform>();

			if (Program.Hosting)
			{
				ConnectLocalHost();
			}
		}

		public static void ConnectLocalHost()
		{
			TcpClient.Connect("127.0.0.1", 5555);
			NetHelper.SendMessageTo(TcpClient, NetHelper.BuildMessage((int) Server.RequestType.Join));
		}

		public static void Update()
		{
			float elapsed = TimeManager.Elapsed;

			while (TcpClient.Available > 0)
			{
				// Update network
				string fullMessage = NetHelper.ReceiveStringMessageFrom(TcpClient);
				string[] messages = fullMessage.Split(NetHelper.SplitChar);
				int type;
				if (int.TryParse(messages[0], out type))
				{
					try
					{
						switch (type)
						{
							case (int)DataType.NewPlayerInfo:
								{
									// Receive order is: id;x;y;w;h;r;g;b
									int id = int.Parse(messages[1]);
									int x = int.Parse(messages[2]);
									int y = int.Parse(messages[3]);
									int width = int.Parse(messages[4]);
									int height = int.Parse(messages[5]);
									Color color = new Color(byte.Parse(messages[6]), byte.Parse(messages[7]), byte.Parse(messages[8]), 255);
									Player = new Player(new Vector2(x, y), new Vector2(width, height), color, TcpClient);

									break;
								}
							case (int)DataType.UpdateInfo:
								{
									// Receive for player order is: plrX:plrY:plrW:plrH:velX:velY;
									// Receive for platform order is: x:y;
									string plrInfoString = fullMessage.Split(';')[1];
									string mapInfoString = fullMessage.Split(';')[2];
									string[] plrInfo = plrInfoString.Split(':');
									string[] mapInfo = mapInfoString.Split(':');

									float plrX = float.Parse(plrInfo[0]);
									float plrY = float.Parse(plrInfo[1]);
									float plrWidth = float.Parse(plrInfo[2]);
									float plrHeight = float.Parse(plrInfo[3]);
									float velX = float.Parse(plrInfo[4]);
									float velY = float.Parse(plrInfo[5]);

									Player.Position = new Vector2(plrX, plrY);
									Player.BoundingBox = new Rectangle(Player.BoundingBox.X, Player.BoundingBox.Y, (int) plrWidth, (int) plrHeight);
									Player.Velocity = new Vector2(velX, velY);

									if (!Program.Hosting)
									{
										for (int i = 0; i < mapInfo.Length; i++)
										{
											if (mapInfo.Length < 2) break;
											if (mapInfo[i] == string.Empty) continue;

											float x = float.Parse(mapInfo[i]);
											float y = float.Parse(mapInfo[++i]);

											PlatformWorld.SpawnPlatform(new Vector2(x, y));
										}
									}

									break;
								}
						}
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);

						if(!Program.Debugging)
							Program.Game.Exit();
						else
							throw;
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
				if (InputManager.InputManager.KeyJustPressed(Keys.W))
				{
					Player.Velocity = new Vector2(Player.Velocity.X, -600);
				}

				Player.UpdatePhysics(PlatformWorld.Platforms.ToArray());
			}
		}

		public static void Draw(SpriteBatch sb)
		{
			DrawWorld(sb);

			if (Player != null)
			{
				Player.Draw(sb);
			}
		}

		private static void DrawWorld(SpriteBatch sb)
		{
			foreach (Platform p in _platforms)
			{
				p.Draw(sb);
			}
		}
	}
}