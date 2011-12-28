using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaMultiplayerGame.Classes;

namespace XnaMultiplayerGame.Network
{
	public class Client
	{
		public TcpClient TcpClient { get; private set; }
		public Player Player { get; private set; }

		public Client(TcpClient connection, Player player)
		{
			TcpClient = connection;
			Player = player;
		}

		public void Update()
		{
			Player.UpdatePhysics(PlatformWorld.Platforms.ToArray());
		}

		public void SendUpdateData()
		{
			Console.WriteLine("Sending update to client");
			string message = (int) LocalClient.DataType.UpdateInfo + ";" + Player.Position.X + ":" +
			                 Player.Position.Y + ":" + 
			                 Player.BoundingBox.Width + ":" + Player.BoundingBox.Height + ":" + Player.Velocity.X + ":" + Player.Velocity.Y + ";";

			foreach (Platform p in PlatformWorld.Platforms)
			{
				//Console.WriteLine(PlatformWorld.Platforms.Count);
				message += p.Position.X + ":" + p.Position.Y + ":";
			}

			NetHelper.SendMessageTo(TcpClient, message);
		}
	}
}