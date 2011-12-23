// -----------------------------------------------------------------------
// <copyright file="Listener.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading;

namespace ConsoleSidescroll.Network
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Net;
	using System.Net.Sockets;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public static class Listener
	{
		public static int Port { get { return 5555; } }
		public static bool Listening { get; set; }

		private static TcpListener _listener;
		private static Thread _listenThread;

		public static void Initialize()
		{
			_listener = new TcpListener(IPAddress.Any, Port);
			_listener.Start();
			Listening = true;

			StartListening();
		}

		public static void StopListening()
		{
			Listening = false;
			_listenThread.Abort();
		}

		private static void StartListening()
		{
			_listenThread = new Thread(delegate()
			                           	{
			                           		while (Listening)
											{
												TcpClient connection = _listener.AcceptTcpClient();

												string message;
												while (true)
												{
													message = NetHelper.ReceiveMessageFrom(connection);

													if (message != string.Empty)
														break;
												}

												if (message.Contains("User-Agent")) // Is a browser, send back a message.
												{
													string send = "<body bgcolor=\"#808080\"><div style=\"margin-left:auto; margin-right:auto;\"><h1 style=\"text-align:center;\">Nope.</h1></div></body>";

													connection.GetStream().Write(Encoding.Unicode.GetBytes(send), 0, Encoding.ASCII.GetBytes(send).Length);

													connection.GetStream().Close();
													connection.Close();
													continue;
												}

												if (message == "1") // Is server, don't add to client list.
													continue;
												else
												{
													NetHelper.SendMessageTo(connection, "OK");
												}

												if (Server.ConnectedClients == null) continue;

			                           			int slot = 0;
												for (int i = 0; i < Server.ConnectedClients.Length; i++)
												{
													Client c = Server.ConnectedClients[i];
													if (c == null)
													{
														slot = i;
														break;
													}
												}

												Server.ConnectedClients[slot] = new Client(connection, slot, slot == 0);

			                           			Console.WriteLine("Client connected from " + connection.Client.RemoteEndPoint + ".");
												Console.WriteLine("Total clients connected: " + Server.GetConnectedClientsCount() + "/" +
												                  Server.ConnectedClients.Length);
											}
			                           	});

			_listenThread.Start();
		}
	}
}