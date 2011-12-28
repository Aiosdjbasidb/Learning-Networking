// -----------------------------------------------------------------------
// <copyright file="Server.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading;

namespace LidgrenChat
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Lidgren.Network;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public static class Server
	{
		public static NetServer NetServer { get; set; }
		public static bool Started { get; private set; }

		public static void Initialize()
		{
			var config = new NetPeerConfiguration("LidgrenChat") {Port = 5555};
			NetServer = new NetServer(config);

			NetServer.Start();

			StartThread();
			Started = true;
		}

		public static void Stop()
		{
			NetServer.Shutdown("Server shutting down.");

			Started = false;
		}

		public static void StartThread()
		{
			new Thread(delegate()
			           	{
			           		while (true)
			           		{
			           			HandleIncomingMessages();

			           			Thread.Sleep(10);
			           		}
			           	}).Start();
		}

		private static void HandleIncomingMessages()
		{
			NetIncomingMessage incoming;
			while ((incoming = NetServer.ReadMessage()) != null)
			{
				//Console.WriteLine("Incoming message...");

				switch (incoming.MessageType)
				{
					case NetIncomingMessageType.StatusChanged:
						{
							var status = (NetConnectionStatus)incoming.ReadByte();

							if (status == NetConnectionStatus.Connected)
							{
								Broadcast("Server", "Client connected!", null);
							}

							break;
						}
					case NetIncomingMessageType.Data:
						{
							string name = incoming.ReadString();
							string msg = incoming.ReadString();

							Broadcast(name, msg, incoming.SenderConnection);

							break;
						}
					default:
						{
							Console.WriteLine("Unhandled type: \"" + incoming.MessageType + "\".");
							Console.WriteLine(("Contained string: " + incoming.ReadString() + "\n"));
							break;
						}
				}
			}
		}

		private static void Broadcast(string name, string message, NetConnection sender)
		{
			var connections = NetServer.Connections;

			if(sender != null) // Can set to null to send message to one that invoked broadcast method aswell.
				connections.Remove(sender);

			if (connections.Count > 0)
			{
				NetOutgoingMessage msg = NetServer.CreateMessage();
				msg.Write(name);
				msg.Write(message);
				NetServer.SendMessage(msg, connections, NetDeliveryMethod.ReliableOrdered, 0);

				Console.WriteLine("Broadcasted message from " + name + ", containing the following message: " + message);
			}
			else
			{
				Console.WriteLine("No connections to send message to");
			}
		}
	}
}