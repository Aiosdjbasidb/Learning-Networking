// -----------------------------------------------------------------------
// <copyright file="Client.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Lidgren.Network;
using Microsoft.Xna.Framework;
using XnaMultiplayerGame.Classes;
using XnaMultiplayerGame.Helpers;

namespace XnaMultiplayerGame.Network
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// Server side client.
	/// </summary>
	public class Client
	{
		public NetConnection Connection { get; private set; }
		public string Name { get; private set; }
		public int Id { get; private set; }
		public Player Player { get; private set; }

		private Player _oldPlayer;

		#region Static methods
		public static Client FromConnection(NetConnection netConnection, List<Client> clients)
		{
			foreach (Client c in clients)
				if (netConnection.RemoteEndpoint.ToString() == c.Connection.RemoteEndpoint.ToString()) // ToString returns a string containing the IP and port the remote client is connected from.
					return c;

			return null;
		}
		#endregion

		public Client(NetConnection connection, string name, int id)
		{
			Connection = connection;
			Name = name;
			Id = id;

			Player = new Player(new Vector2(100, 100), Player.TextureSize, Helper.GetRandomColor());

			SendPlayerInformation();
		}

		public void SendPlayerInformation()
		{
			var msg = Server.NetServer.CreateMessage();

			msg.Write((int)Headers.Client.NewPlayerInfo);
			msg.Write(Id);
			msg.Write(Player.Position.X);
			msg.Write(Player.Position.Y);
			msg.Write(Player.BoundingBox.Width);
			msg.Write(Player.BoundingBox.Height);
			msg.Write(Player.DrawColor.R);
			msg.Write(Player.DrawColor.G);
			msg.Write(Player.DrawColor.B);
			msg.Write(Player.DrawColor.A);

			Connection.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, 0);
		}

		public void SendPlayerPositionVelocity()
		{
			var msg = Server.NetServer.CreateMessage();

			msg.Write((int) Headers.Client.PlayerInfo);
			msg.Write(Player.Position.X);
			msg.Write(Player.Position.Y);
			msg.Write(Player.Velocity.X);
			msg.Write(Player.Velocity.Y);

			Connection.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, 0);
		}

		public void Update()
		{
			_oldPlayer = Player;
			Player.UpdatePhysics(PlatformWorld.Platforms.ToArray());
		}
	}
}