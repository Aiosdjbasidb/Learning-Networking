// -----------------------------------------------------------------------
// <copyright file="Listener.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace XnaMultiplayerGame.Network
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public static class Listener
	{
		public static List<TcpClient> PendingConnections { get; private set; }
		private static TcpListener _listener;

		public static bool Listening { get; set; }

		private static Thread _listenThread;

		public static void Initialize()
		{
			PendingConnections = new List<TcpClient>();

			_listener = new TcpListener(IPAddress.Any, 5555);

			_listener.Start();

			StartListening();
		}

		public static void StopListening()
		{
			Console.WriteLine("Stopping listener...");
			_listenThread.Abort();
			Listening = false;
			Console.WriteLine("Stopped listener.");
		}

		private static void StartListening()
		{
			Listening = true;

			_listenThread = new Thread(delegate()
			                           	{
			                           		Console.WriteLine("Starting listen thread...");
			                           		while (Listening)
			                           		{
			                           			TcpClient client = _listener.AcceptTcpClient();

			                           			Console.WriteLine(client.Client.RemoteEndPoint + " is connecting...");

			                           			PendingConnections.Add(client);
			                           		}
			                           	});

			_listenThread.Start();
		}
	}
}