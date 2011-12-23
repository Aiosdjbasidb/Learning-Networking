// -----------------------------------------------------------------------
// <copyright file="Chat.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleSidescroll
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Network;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public static class Chat
	{
		public static string Text { get; set; }
		public static bool Focused;

		private const int MaxChatRows = 20;

		private static List<ChatLine> _chatLines;

		private static int Width = 20;
		private static int Height = 24;
		private static int X = Console.BufferWidth - Width;
		private static int Y = 0;
		private static char OutlineCharX = '-';
		private static char OutlineCharY = '|';

		private static List<ChatLine> _toSendChatLines;

		private const string Title = "Server chat";

		public struct ChatLine
		{
			public string Text;
			public ConsoleColor Color;

			public ChatLine(string text, ConsoleColor color = ConsoleColor.Gray)
			{
				Text = text;
				Color = color;
			}
		}

		public static void Initialize()
		{
			_chatLines = new List<ChatLine>();
			_toSendChatLines = new List<ChatLine>();
			PrintChatLine(new ChatLine("Hello there stranger"));
			Focused = false;
		}

		public static void Update(TcpClient server)
		{
			if (Focused)
			{
				string buffer = "";

				foreach (ConsoleKeyInfo keyInfo in InputManager.PressedKeys)
				{
					switch (keyInfo.Key)
					{
						case ConsoleKey.Escape:
							{
								Focused = false;
								Text = string.Empty;
								break;
							}
						case ConsoleKey.Enter:
							{
								Focused = false;
								AddMessage(new ChatLine(Text, LocalClient.Player.Drawable.Color));
								Text = string.Empty;
								break;
							}
						case ConsoleKey.Backspace:
							{
								if (Text.Length - 1 > 0)
									Text = Text.Substring(0, Text.Length - 1);
								else
								{
									Text = string.Empty;
								}
								break;
							}
						default:
							{
								buffer += keyInfo.KeyChar;
								break;
							}
					}
				}

				Text += buffer;
			}
			else
			{
				foreach (ConsoleKeyInfo keyInfo in InputManager.PressedKeys)
				{
					switch (keyInfo.Key)
					{
						case ConsoleKey.Y:
							{
								Focused = true;
								break;
							}
					}
				}
			}

			SendBuffer(server);

			// An incoming message spotted.
			if (server.Available > 0)
			{
				string message;
				while(true)
				{
					message = NetHelper.ReceiveMessageFrom(server);

					if (message != string.Empty)
						break;
				}

				string[] messages = message.Split(Headers.SplitChar);
				
				int id = int.Parse(messages[1]);
				string chatMessage = messages[2];
				ConsoleColor chatColor = (ConsoleColor) int.Parse(messages[3]);

				PrintChatLine(new ChatLine(chatMessage, chatColor));
			}
		}

		public static void AddMessage(ChatLine line)
		{
			_toSendChatLines.Add(line);
		}

		public static void Draw()
		{
			ClearDrawArea();
			DrawWriteArea();
			DrawChatText();
		}

		private static void SendBuffer(TcpClient server)
		{
			foreach (ChatLine line in _toSendChatLines)
			{
				NetHelper.SendMessageTo(server,
				                        NetHelper.BuildRequestString((int) Headers.DataType.SendChatMessage, LocalClient.Id) +
				                        line.Text + Headers.SplitChar.ToString() +
				                        (int) line.Color);
			}
		}

		private static void PrintChatLine(ChatLine chatLine)
		{
			if (_chatLines.Count >= MaxChatRows)
			{
				// Delete first entry and move everything up, then add entry to last position.
				_chatLines.Remove(_chatLines[0]);
				for (int i = 1; i < _chatLines.Count; i++)
				{
					_chatLines[i - 1] = _chatLines[i];
				}

				_chatLines.Add(chatLine);
			}
			else
			{
				_chatLines.Add(chatLine);
			}
		}

		private static void ClearDrawArea()
		{
			for (int y = Y; y < Y + Height; y++)
			{
				for (int x = X; x < X + Width; x++)
				{
					if (y == Y && x == X + (Title.Length / 2))
					{
						DrawManager.DrawString(Title, new Vector2(x, y), ConsoleColor.DarkGray);
						x += Title.Length;
					}

					if(x == X || x == Width - 1)
						DrawManager.DrawString(OutlineCharY.ToString(), new Vector2(x, y), ConsoleColor.DarkGray);
					else if(y == Y || y == Height - 1)
						DrawManager.DrawString(OutlineCharX.ToString(), new Vector2(x, y), ConsoleColor.DarkGray);
				}
			}
		}

		private static void DrawWriteArea()
		{
			/*Console.SetCursorPosition(X, Height);
			Console.Write("> " + Text);*/

			for (int x = X; x < Width; x++)
			{
				DrawManager.DrawString(" ", new Vector2(x, Height), ConsoleColor.DarkGray);
			}

			DrawManager.DrawString("> " + Text, new Vector2(X, Height), ConsoleColor.DarkGray);
		}

		private static void DrawChatText()
		{
			foreach (ChatLine line in _chatLines)
			{

			}
		}
	}
}