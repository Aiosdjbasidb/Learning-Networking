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

		public enum DataType
		{
			/// <summary>
			/// Format: x:y
			/// </summary>
			Position,

			/// <summary>
			/// Format: x:y
			/// </summary>
			Velocity,

			/// <summary>
			/// Format: x:y:w:h;
			/// </summary>
			Platforms,

			/// <summary>
			/// Format: id:x:y:w:h
			/// </summary>
			NewPlayerInfo
		}

		public static void Initialize()
		{
			TcpClient = new TcpClient();

			if (Program.Hosting)
			{
				ConnectLocalHost();
			}
		}

		public static void ConnectLocalHost()
		{
			TcpClient.Connect("127.0.0.1", 5555);
			NetHelper.SendMessageTo(TcpClient, NetHelper.BuildMessage((int) Server.DataType.Join));
		}

		public static void Update()
		{
			float elapsed = TimeManager.Elapsed;

			if (TcpClient.Available > 0)
			{
				// Update network
				string[] messages = NetHelper.ReceiveMessageFrom(TcpClient);
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
							case (int)DataType.Platforms:
								{
									// Receive order is: x:y:w:h;
									int x = int.Parse(messages[1]);
									int y = int.Parse(messages[2]);
									int width = int.Parse(messages[3]);
									int height = int.Parse(messages[4]);

									break;
								}
							case (int)DataType.Position:
								{
									// Receive order is: x;y
									int x = int.Parse(messages[1]);
									int y = int.Parse(messages[2]);

									Player.Position = new Vector2(x, y);

									break;
								}
							case (int)DataType.Velocity:
								{
									// Receive order is: x;y
									int x = int.Parse(messages[1]);
									int y = int.Parse(messages[2]);

									Player.Velocity = new Vector2(x, y);

									break;
								}
						}
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
						Program.Game.Exit();
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

				Player.Update(Program.Game.World.Platforms.ToArray());
			}
		}

		public static void Draw(SpriteBatch sb)
		{
			if (Player != null)
			{
				Player.Draw(sb);
			}
		}
	}
}