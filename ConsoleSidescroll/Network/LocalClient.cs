// -----------------------------------------------------------------------
// <copyright file="LocalClient.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace ConsoleSidescroll.Network
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
		public static Player Player { get; private set; }
		public static TcpClient TcpClient { get; private set; }
		public static NetworkStream Stream { get { return TcpClient.GetStream(); } }
		public static int Id { get; private set; }
		public static char DrawChar { get; set; }
		public static string PlayerName { get; set; }
		public static ConsoleColor DrawColor { get; set; }
		public static List<Player> ServerPlayers { get; set; }

		public static void Initialize(string ip, int port)
		{
			TcpClient = new TcpClient(ip, port);
			Player = new Player(new Vector2(0));
			Id = -1;

			NetHelper.SendMessageTo(TcpClient, "0");

			while (true)
			{
				string status;
				status = NetHelper.ReceiveMessageFrom(TcpClient);

				if (status == "OK")
					break;
			}

			NetHelper.SendMessageTo(TcpClient,
			                     NetHelper.BuildRequestString((int) Headers.DataType.RequestData,
			                                               (int) Headers.RequestDataType.GetFreeSlot));

			while (!Stream.DataAvailable)
			{
				Console.WriteLine("Waiting for ID from server...");

				Thread.Sleep(1000/2);
			}

			// Possibly have id.
			string[] message = NetHelper.ReceiveMessageFrom(TcpClient).Split(Headers.SplitChar);

			switch (Int32.Parse(message[0]))
			{
				case (int)Headers.DataType.RequestJoin:
					{
						Id = Int32.Parse(message[1]);
						Player.Id = Id;
						Console.WriteLine("Player id set to: " + Id);

						if (Id == -1)
						{
							Console.WriteLine("Server is full.");
							Environment.Exit(0);
						}
						break;
					}
				default:
					{
						Console.WriteLine("Received message was invalid.");
						break;
					}
			}

			ServerSetDrawChar(DrawChar);
			ServerSetColor(DrawColor);
			ServerSetPlayerName(PlayerName);

			World.CharTiles = ServerGetTiles();

			Console.ReadKey(true);
		}

		public static void Update()
		{
			Chat.Update(TcpClient);
			UpdateInput();

			// Request player X and Y position.
			Vector2 vector = Vector2.Zero;

			vector = ServerGetPosition();

			if (vector == Vector2.Invalid)
				return; // Don't set position.

			// Set player X and Y position.
			Player.Position = vector;

			ServerPlayers = ServerGetPlayers();
		}

		public static List<Player> ServerGetPlayers()
		{
			var pList = new List<Player>();

			NetHelper.SendMessageTo(TcpClient,
								 NetHelper.BuildRequestString((int)Headers.DataType.RequestData,
														   (int)Headers.RequestDataType.GetPlayersInfo));

			string message;
			while (true)
			{
				message = NetHelper.ReceiveMessageFrom(TcpClient);

				if (message != String.Empty)
					break;
			}

			string[] players = message.Split(Headers.SplitChar);

			foreach (string player in players)
			{
				if (player == String.Empty) continue;

				string[] info = player.Split(':');

				var p = new Player(new Vector2(Int32.Parse(info[1]), Int32.Parse(info[2])), (char)Int32.Parse(info[3]));
				p.Drawable.Color = (ConsoleColor) int.Parse(info[4]);
				p.Name = info[5];

				pList.Add(p);
			}

			return pList;
		}

		public static List<Drawable> ServerGetTiles()
		{
			var dList = new List<Drawable>();

			// Send message
			NetHelper.SendMessageTo(TcpClient,
			                     NetHelper.BuildRequestString((int) Headers.DataType.RequestData,
			                                               (int) Headers.RequestDataType.GetWorldTiles));

			// Receive message
			string message;
			while (true)
			{
				message = NetHelper.ReceiveMessageFrom(TcpClient);

				if (message != string.Empty)
					break;
			}

			// Parse message into drawable's and add to list. Server sends drawable's in the format of: x:y:charInteger:colorInteger;
			string[] drawables = message.Split(Headers.SplitChar);
			foreach (string drawable in drawables)
			{
				if (!drawable.Contains(':')) continue;

				string[] info = drawable.Split(':');

				int x, y, charInteger, colorInteger;
				x = int.Parse(info[0]);
				y = int.Parse(info[1]);
				charInteger = int.Parse(info[2]);
				colorInteger = int.Parse(info[3]);

				char ch = (char) charInteger;
				ConsoleColor color = (ConsoleColor) colorInteger;

				dList.Add(new Drawable(new Vector2(x, y), ch, color));
			}

			return dList;
		}

		private static void UpdateInput()
		{
			if (Chat.Focused) return;

			foreach (ConsoleKeyInfo keyInfo in InputManager.PressedKeys)
			{
				switch (keyInfo.Key)
				{
					case ConsoleKey.A:
						{
							ServerMoveTo(-1, 0);
							break;
						}
					case ConsoleKey.D:
						{
							ServerMoveTo(1, 0);
							break;
						}
					case ConsoleKey.W:
						{
							ServerMoveTo(0, -1);
							break;
						}
					case ConsoleKey.S:
						{
							ServerMoveTo(0, 1);
							break;
						}
				}
			}
		}

		private static Vector2 ServerGetPosition()
		{
			int x, y;

			NetHelper.SendMessageTo(TcpClient,
			                     NetHelper.BuildRequestString((int)Headers.DataType.RequestData, (int)Headers.RequestDataType.GetPlayerPosition, Id));

			string receivedMessage;

			while (true)
			{
				receivedMessage = NetHelper.ReceiveMessageFrom(TcpClient);

				if (receivedMessage != string.Empty)
					break;
			}

			string[] message = receivedMessage.Split(Headers.SplitChar);

			x = Int32.Parse(message[0]);
			y = Int32.Parse(message[1]);

			return new Vector2(x, y);
		}

		private static bool ServerSetPosition(Vector2 position)
		{
			int x = position.X;
			int y = position.Y;

			NetHelper.SendMessageTo(TcpClient, NetHelper.BuildRequestString((int) Headers.DataType.SetPlayerPosition, Id, x, y));

			string message;
			while (true)
			{
				message = NetHelper.ReceiveMessageFrom(TcpClient);

				if (message != string.Empty)
					break;
			}

			return message == "1";
		}

		private static bool ServerMoveTo(int x, int y)
		{
			return ServerSetPosition(Player.Position + new Vector2(x, y));
		}

		private static bool ServerSetDrawChar(char drawChar)
		{
			int charInt = drawChar;

			NetHelper.SendMessageTo(TcpClient, NetHelper.BuildRequestString((int)Headers.DataType.SetPlayerChar, Id, charInt));

			string message;
			while (true)
			{
				message = NetHelper.ReceiveMessageFrom(TcpClient);

				if (message != string.Empty)
					break;
			}

			return message == "1";
		}

		private static bool ServerSetColor(ConsoleColor color)
		{
			int colorInt = (int) color;

			NetHelper.SendMessageTo(TcpClient, NetHelper.BuildRequestString((int) Headers.DataType.SetPlayerColor, Id, colorInt));

			string message;
			while (true)
			{
				message = NetHelper.ReceiveMessageFrom(TcpClient);

				if (message != string.Empty)
					break;
			}

			return message == "1";
		}

		private static bool ServerSetPlayerName(string name)
		{
			NetHelper.SendMessageTo(TcpClient, NetHelper.BuildRequestString((int) Headers.DataType.SetPlayerName, Id) + name);

			string message;
			while (true)
			{
				message = NetHelper.ReceiveMessageFrom(TcpClient);

				if (message != string.Empty)
					break;
			}

			return message == "1";
		}
	}
}