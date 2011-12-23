// -----------------------------------------------------------------------
// <copyright file="Client.cs" company="">
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
	public class Client
	{
		public Player Player { get; private set; }
		public TcpClient TcpClient { get; private set; }
		public int Id { get; private set; }
		public bool IsServer { get; private set; }

		public Client(TcpClient tcpClient, int id, bool isServer)
		{
			var r = new Random();

			Player = new Player(new Vector2(r.Next(1, 10), r.Next(1, 10)));
			TcpClient = tcpClient;
		}
	}
}