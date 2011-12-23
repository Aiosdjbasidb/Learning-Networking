using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ConsoleSidescroll.Network;

namespace ConsoleSidescroll
{
	static class Program
	{
		public static bool Hosting { get; private set; }
		public static string Ip { get; private set; }
		public static int Port { get; private set; }

		private static void Main(string[] args)
		{
			Initialize();

			RunMenu();
			RunGame();
		}

		private static void Update()
		{
			InputManager.Update();
			LocalClient.Update();
		}

		private static void Draw()
		{
			foreach (Player p in LocalClient.ServerPlayers)
			{
				p.Draw();

				int x = (int)MathHelper.Clamp(p.Position.X - (p.Name.Length / 2), 0, (Console.BufferWidth - 1) - p.Name.Length);
				int y = (int)MathHelper.Clamp(p.Position.Y - 1, 0, (Console.BufferWidth - 1));

				DrawManager.DrawString(p.Name, new Vector2(x, y), p.Drawable.Color);
			}

			foreach (Drawable d in World.CharTiles)
			{
				if (d.DrawChar != ' ')
				{
					Console.SetCursorPosition(d.Position.X, d.Position.Y);
					Console.ForegroundColor = d.Color;
					Console.Write(d.DrawChar);
				}
			}

			Chat.Draw();

			DrawManager.Paint();
		}

		static void Initialize()
		{
			DrawManager.Initialize();
			World.Initialize();

			Console.CursorVisible = false;
		}

		static void RunMenu()
		{
			string hostingSelection = string.Empty;
			string ipSelection = string.Empty;
			int hostingSelectionInt = 0;
			char drawChar = '$';
			ConsoleColor drawColor = ConsoleColor.White;
			string playerName = string.Empty;
			bool shouldBreak = false;

			while (!shouldBreak)
			{
				while (true)
				{
					Console.Clear();
					Console.Write("Are you (1)joining or (2)hosting a server? ");
					hostingSelection = Console.ReadKey(true).KeyChar.ToString();

					if (int.TryParse(hostingSelection, out hostingSelectionInt))
					{
						if (hostingSelectionInt >= 1 && hostingSelectionInt <= 2)
							break;
					}

					Console.WriteLine("\n\nInput \"" + hostingSelection + "\" is invalid. Press any key to try again.");
					Console.ReadKey(true);
				}
				Console.Clear();

				if (hostingSelectionInt != 2)
				{
					// Parse ip
					while (true)
					{
						IPAddress address;

						Console.Clear();
						Console.Write("The hoster's IP: ");

						Console.CursorVisible = true;
						ipSelection = Console.ReadLine();
						Console.CursorVisible = false;

						if (IPAddress.TryParse(ipSelection, out address))
						{
							break;
						}

						Console.WriteLine("\nInput \"" + ipSelection + "\" is not a valid IP address. Press and key to try again.");
						Console.ReadKey(true);
					}

					// Parse draw character
					while (true)
					{
						Console.Clear();
						Console.Write("The character you want to be drawn as: ");

						Console.CursorVisible = true;
						drawChar = Console.ReadLine()[0];
						Console.CursorVisible = false;

						if (drawChar != Headers.SplitChar && drawChar != ' ')
						{
							break;
						}

						Console.WriteLine("\nInput \"" + drawChar + "\" is not an allowed character. Press any key to try again.");
						Console.ReadKey(true);
					}

					// Parse draw color
					while (true)
					{
						Console.Clear();
						Console.WriteLine("What color do you want to be drawn in?\n");

						for (int i = 0; i < 16; i++)
						{
							Console.WriteLine(i + ": " + ((ConsoleColor) i));
						}
						Console.Write("\n> ");

						string input = Console.ReadLine();

						int colorInt;

						if (int.TryParse(input, out colorInt))
						{
							if (colorInt >= 0 && colorInt < 16)
							{
								drawColor = (ConsoleColor) colorInt;

								break;
							}
						}
					}

					// Parse player name
					while (true)
					{
						Console.Clear();
						Console.Write("Your name: ");

						string input = Console.ReadLine();

						if (input.Contains(Headers.SplitChar))
						{
							Console.WriteLine("\nInput \"" + input + "\" is not a valid name. Name can not contain \";\".");
							continue;
						}

						playerName = input;
						break;
					}
				}

				// Verify that settings are ok to the user.
				Console.Clear();

				string hostingAction = hostingSelectionInt == 1
				                       	? "joining dear comrades at " + ipSelection + ":" + Listener.Port + ".\n\nPeople will know you as " + playerName + ", the " + drawColor.ToString().ToLower() + " " +
				                       	  drawChar
				                       	: "hosting a server";

				Console.Write("You are " + hostingAction + ".\n");

				if (hostingSelectionInt == 2)
					Console.Write("The port you are hosting on is " + Listener.Port + ".\n");

				Console.Write("\nConfirm settings (n = start from beginning)? y/n: ");

				while (true)
				{
					var input = Console.ReadKey(true).KeyChar.ToString();

					switch (input.ToLower())
					{
						case "y":
							shouldBreak = true;
							break;
						case "n":
							shouldBreak = false;
							break;
					}

					break;
				}
			}

			// All settings are validated and are safe to be used.
			Ip = ipSelection != "" ? ipSelection : "127.0.0.1";
			Hosting = hostingSelectionInt != 1;
			Port = Listener.Port;

			// Set client info.
			if (!Hosting)
			{
				LocalClient.DrawChar = drawChar;
				LocalClient.DrawColor = drawColor;
				LocalClient.PlayerName = playerName;
			}

			Console.Title = Hosting ? "Server" : "Client";

			Console.Clear();
		}

		private static void RunGame()
		{
			if (!Hosting)
			{
				InputManager.Initialize();
				Chat.Initialize();
				LocalClient.Initialize(Ip, Port);

				Console.Clear();

				while (true)
				{
					Update();
					Draw();
				}
			}
			else
			{
				// Start a server and maintain it.
				Listener.Initialize();
				Server.Initialize();

				Console.Clear();
				Console.WriteLine("Started server.\n");

				while (true)
				{
					Server.Update();
				}
			}
		}
	}
}