// -----------------------------------------------------------------------
// <copyright file="Client.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Net.Sockets;

namespace Chat
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class ChatClient
	{
		public TcpClient Client { get; private set; }
		public string Name { get; private set; }

		public ChatClient(TcpClient socket, string name)
		{
			Client = socket;
			Name = name;
		}

		public ChatClient(TcpClient socket)
		{
			Client = socket;
			Name = "Unnamed";
		}

		public void ReadMessage()
		{
			var stream = Client.GetStream();

			if (stream.DataAvailable)
			{
				var bMsg = new byte[Client.Available];

				stream.Read(bMsg, 0, Client.Available);

				var msg = Encoding.ASCII.GetString(bMsg);

				var name = msg.Split('\n')[0];
				var message = msg.Split('\n')[1];

				if (name != Name)
				{
					Console.WriteLine("\n" + name + ": " + message);
				}
			}
		}
	}
}