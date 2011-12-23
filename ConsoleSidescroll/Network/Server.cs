// -----------------------------------------------------------------------
// <copyright file="Server.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Net.Sockets;

namespace ConsoleSidescroll.Network
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
		//public static TcpClient ServerConnection { get; private set; }

		private const int MaxPlayers = 100;
 
		public static void Initialize()
		{
			/*ServerConnection = new TcpClient(Program.Ip, Program.Port);
			NetHelper.SendMessageTo(ServerConnection, "1");*/

			ConnectedClients = new Client[MaxPlayers];
		}

		public static void Update()
		{
			World.Update();

			int clientCount = ConnectedClients.Count(c => c != null);

			//Console.WriteLine("Server client count: " + clientCount);

			foreach (Client c in ConnectedClients)
			{
				if (c == null) continue;

				TcpClient tcpClient = c.TcpClient;

				if (!tcpClient.GetStream().DataAvailable) continue;

				string message = NetHelper.ReceiveMessageFrom(tcpClient);

				string[] messages = message.Split(Headers.SplitChar);
				int dataType = int.Parse(messages[0]);

				switch (dataType)
				{
					case (int)Headers.DataType.RequestData:
						{
							int requestType = int.Parse(messages[1]);

							switch (requestType)
							{
								case (int)Headers.RequestDataType.GetPlayerPosition:
									{
										int id = int.Parse(messages[2]);
										string posMessage = NetHelper.BuildRequestString(ConnectedClients[id].Player.Position.X, ConnectedClients[id].Player.Position.Y);

										NetHelper.SendMessageTo(tcpClient, posMessage);
										break;
									}
								case (int)Headers.RequestDataType.GetPlayersInfo: // Send players position and draw character in the form of: id:x:y:drawChar:drawColor:name.
									{
										string msg = string.Empty;
										foreach (Client _c in ConnectedClients)
										{
											if (_c == null) continue;

											msg += _c.Id + ":" + _c.Player.Position.X + ":" + _c.Player.Position.Y + ":" + (int)_c.Player.DrawChar + ":" + (int)_c.Player.Drawable.Color + ":" + _c.Player.Name + Headers.SplitChar;
										}

										NetHelper.SendMessageTo(tcpClient, msg);
										break;
									}
								case (int)Headers.RequestDataType.GetFreeSlot:
									{
										bool sendFull = true;
										for (int i = 0; i < ConnectedClients.Length; i++)
										{
											Client _c = ConnectedClients[i];
											if (_c == null)
											{
												// Send i to tcpClient.
												NetHelper.SendMessageTo(tcpClient,
																	 (int)Headers.DataType.RequestJoin + Headers.SplitChar.ToString() + (int)MathHelper.Clamp(i - 1, 0, int.MaxValue));
												sendFull = false;
												break;
											}
										}

										if (sendFull)
											NetHelper.SendMessageTo(tcpClient, (int)Headers.DataType.RequestJoin + ";-1");

										break;
									}
								case (int)Headers.RequestDataType.GetWorldTiles:
									{
										string msg = "";
										foreach (Drawable d in World.CharTiles)
										{
											msg += d.Position.X + ":" + d.Position.Y + ":" + (int)d.DrawChar + ":" + (int)d.Color + Headers.SplitChar;
										}

										NetHelper.SendMessageTo(tcpClient, msg);

										break;
									}
							}

							break;
						}
					case (int)Headers.DataType.SetPlayerPosition:
						{
							int id = int.Parse(messages[1]);
							int x = int.Parse(messages[2]);
							int y = int.Parse(messages[3]);
							int success = 1;

							// Collision check with console buffer size.
							if (x >= 0 && y >= 0 && x < Console.BufferWidth && y < Console.BufferHeight)
							{
								foreach (Client client in ConnectedClients)
								{
									if (client == null) continue;

									// Collision check with other players.
									if (client.Player.Position == new Vector2(x, y))
									{
										success = 0;
									}
								}

								// Collision check with world.
								if (!(x >= World.Width || y >= World.Height))
								{
									if (World.CharTiles[x + y * World.Width].DrawChar != ' ')
										success = 0;
								}
								else
								{
									success = 0;
								}
							}
							else
							{
								success = 0;
							}

							if (success == 1)
							{
								ConnectedClients[id].Player.Position = new Vector2(x, y);
								NetHelper.SendMessageTo(tcpClient, "1");
							}
							else
							{
								NetHelper.SendMessageTo(tcpClient, "0");
							}

							break;
						}
					case (int)Headers.DataType.SetPlayerChar:
						{
							int id = int.Parse(messages[1]);
							char charSet = (char)int.Parse(messages[2]);

							if (charSet != Headers.SplitChar)
							{
								ConnectedClients[id].Player.DrawChar = charSet;

								NetHelper.SendMessageTo(tcpClient, "1");
							}
							else
								NetHelper.SendMessageTo(tcpClient, "0");

							break;
						}
					case (int)Headers.DataType.SetPlayerColor:
						{
							int id = int.Parse(messages[1]);
							ConsoleColor color = (ConsoleColor)int.Parse(messages[2]);

							ConnectedClients[id].Player.Drawable.Color = color;

							NetHelper.SendMessageTo(tcpClient, "1");

							break;
						}
					case (int)Headers.DataType.SetPlayerName:
						{
							int id = int.Parse(messages[1]);
							string name = messages[2];

							ConnectedClients[id].Player.Name = name;

							NetHelper.SendMessageTo(tcpClient, "1");

							Console.WriteLine("Set name to: " + name);

							break;
						}
					case (int)Headers.DataType.SendChatMessage:
						{
							int id = int.Parse(messages[1]);
							string msg = messages[2];
							ConsoleColor color = (ConsoleColor) int.Parse(messages[3]);

							if (msg.Contains(Headers.SplitChar))
								continue;

							foreach (Client _c in ConnectedClients)
							{
								if (_c == null) continue;

								NetHelper.SendMessageTo(_c.TcpClient,
								                        NetHelper.BuildRequestString((int) Headers.DataType.SendChatMessage, _c.Id) + msg +
								                        Headers.SplitChar + (int) color);
							}

							break;
						}
					default:
						{
							Console.WriteLine("Unknown data type recieved: " + dataType + ".");
							break;
						}
				}
			}
		}

		public static int GetConnectedClientsCount()
		{
			return ConnectedClients.Count(c => c != null);
		}
	}
}