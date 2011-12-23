// -----------------------------------------------------------------------
// <copyright file="ChatServer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Chat
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Net;
	using System.Net.Sockets;
	using System.Threading;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class ChatServer
	{
		public TcpListener Listener { get; private set; }
		public bool Listening { get; set; }
		public List<ChatClient> Clients { get; private set; }

		private List<string> _messages; 

		public ChatServer(int port)
		{
			_messages = new List<string>();
			Clients = new List<ChatClient>();

			Listener = new TcpListener(IPAddress.Any, port);
			Listener.Start();
			Listening = true;
		}

		public void StartListening()
		{
			new Thread(delegate()
			           	{
			           		while (Listening)
			           		{
			           			Clients.Add(new ChatClient(Listener.AcceptTcpClient()));
			           		}
			           	}).Start();
		}

		public void Receive()
		{
			_messages.Clear();

			var clients = Clients.ToArray();

			foreach (ChatClient client in clients)
			{
				var stream = client.Client.GetStream();

				if (!stream.DataAvailable) continue;

				var bMsg = new byte[client.Client.Available];

				stream.Read(bMsg, 0, client.Client.Available);

				var msg = Encoding.ASCII.GetString(bMsg);

				_messages.Add(msg);
				Console.WriteLine("Added message: \"" + msg + "\".");
			}
		}

		public void Send()
		{
			var clients = Clients.ToArray();

			foreach (ChatClient client in clients)
			{
				foreach (string msg in _messages)
				{
					var bMsg = Encoding.ASCII.GetBytes(msg);

					client.Client.GetStream().Write(bMsg, 0, bMsg.Count());
				}
			}
		}
	}
}