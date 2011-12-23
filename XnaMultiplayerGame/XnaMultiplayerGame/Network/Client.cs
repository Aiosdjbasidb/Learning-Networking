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
			Player.Update(Program.Game.World.Platforms.ToArray());
		}
	}
}