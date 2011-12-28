using System;
using XnaMultiplayerGame.Network;

namespace XnaMultiplayerGame
{
#if WINDOWS || XBOX
	static class Program
	{
		public static bool Debugging = true;
		public static bool Hosting = true;

		public static Game1 Game { get; set; }
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			Game = new Game1();

			Game.Run();
		}
	}
#endif
}