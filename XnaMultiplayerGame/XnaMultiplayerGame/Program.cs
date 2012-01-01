using System;
using System.Net;
using XnaMultiplayerGame.Network;

namespace XnaMultiplayerGame
{
#if WINDOWS || XBOX
	static class Program
	{
		public static bool Debugging = false;
		public static bool Hosting = true;
		public static string Ip;

		public static Game1 Game { get; set; }
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			GetSettingsFromInput();

			Game = new Game1();
			Game.Run();
		}

		private static void GetSettingsFromInput()
		{
			Console.Write("Your name: ");
			string name = Console.ReadLine();
			LocalClient.Name = name;

			Console.Clear();

			Console.Write("Are you hosting? y/n: ");

			bool canBreak = false;
			ConsoleKeyInfo input;
			while (true)
			{
				input = Console.ReadKey(true);

				switch (input.Key)
				{
					case ConsoleKey.Y:
						{
							Hosting = true;

							canBreak = true;
							break;
						}
					case ConsoleKey.N:
						{
							Hosting = false;

							canBreak = true;
							break;
						}
					default:
						{
							canBreak = false;
							break;
						}
				}

				if (canBreak)
					break;
			}

			if (!Hosting)
			{
				Console.Clear();

				Console.Write("Server IP: ");

				IPAddress ip;
				while (!IPAddress.TryParse(Console.ReadLine(), out ip))
				{
					Console.WriteLine("Invalid ip address.");
					Console.ReadKey(true);
					Console.Clear();
					Console.Write("Server IP: ");
				}

				Ip = ip.ToString();
			}

			Console.Clear();
		}
	}
#endif
}