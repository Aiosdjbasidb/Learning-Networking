// -----------------------------------------------------------------------
// <copyright file="LocalClient.cs" company="">
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
	public static class LocalClient
	{
		public static NetClient NetClient { get; private set; }

		public static string Ip { get; private set; }
		public static int Port { get; private set; }
		public static string Name { get; set; }

		public static void Initialize(string ip, int port, string name)
		{
			Ip = ip;
			Port = port;
			Name = name;

			Disconnect();

			NetClient = new NetClient(new NetPeerConfiguration("LidgrenChat") {ConnectionTimeout = 5f});
			NetClient.Start();

			var hail = NetClient.CreateMessage(name);

			NetClient.Connect(Ip, Port, hail);

			NetClient.RegisterReceivedCallback(MessageReceived);
		}

		public static void SendMessage(string message)
		{
			NetOutgoingMessage msg = NetClient.CreateMessage();
			msg.Write(Name);
			msg.Write(message);

			NetClient.SendMessage(msg, NetClient.Connections, NetDeliveryMethod.ReliableOrdered, 0);

			Program.Form.AddMessage(Name, message);
		}

		private static void MessageReceived(object peer)
		{
			NetIncomingMessage incoming;
			while ((incoming = NetClient.ReadMessage()) != null)
			{
				incoming.Decrypt(new NetXtea("SuperSecretKey"));

				switch (incoming.MessageType)
				{
					case NetIncomingMessageType.Data:
						{
							string name = incoming.ReadString();
							string msg = incoming.ReadString();

							Program.Form.AddMessage(name, msg);

							break;
						}
				}
			}
		}

		public static void Disconnect()
		{
			if (NetClient == default(NetClient)) return;
			if (NetClient.ConnectionStatus != NetConnectionStatus.Disconnected)
			{
				NetClient.Disconnect("Client disconnecting.");
			}
		}
	}
}