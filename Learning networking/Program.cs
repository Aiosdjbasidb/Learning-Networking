using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Chat
{
	class Program
	{
		private static TcpClient _meTcpClient;
		private static ChatClient _me;
		private static string _name;

		static void Main(string[] args)
		{
			while (true)
			{
				Console.Write("Your alias: ");
				_name = Console.ReadLine();

				if (string.IsNullOrEmpty(_name))
				{
					Console.WriteLine("Your name can not be empty.");
				}
				else
				{
					break;
				}
			}

			var choice = -1;

			while (true)
			{
				Console.Write("Are you a (1)client or a (2)server? ");

				var input = Console.ReadLine();

				if (!int.TryParse(input, out choice))
				{
					Console.WriteLine("Invalid choice, try again.");

					continue;
				}

				break;
			}

			switch (choice)
			{
				case 1: // Is a client
					{
						while (true)
						{
							string ip;

							Console.Write("IP: ");
							ip = Console.ReadLine();

							try
							{
								_meTcpClient = new TcpClient(ip, 5555);
								_me = new ChatClient(_meTcpClient, _name);

								break;
							}
							catch (Exception e)
							{
								Console.WriteLine(e.Message);
							}
						}

						new Thread(delegate()
						           	{
										while (true)
										{
											_me.ReadMessage();
										}
						           	}).Start();

						while (true)
						{
							Console.Write("Say: ");
							var msg = _me.Name + "\n" + Console.ReadLine();

							var bMsg = Encoding.ASCII.GetBytes(msg);

							_meTcpClient.GetStream().Write(bMsg, 0, bMsg.Count());
						}

						break;
					}
				case 2: // Is a server
					{
						var server = new ChatServer(5555);
						server.StartListening();

						while (server.Listening)
						{
							server.Receive();
							server.Send();
						}

						break;
					}
			}
		}
	}
}