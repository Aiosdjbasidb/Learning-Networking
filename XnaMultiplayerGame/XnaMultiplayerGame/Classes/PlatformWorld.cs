// -----------------------------------------------------------------------
// <copyright file="PlatformWorld.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaMultiplayerGame.Helpers;
using XnaMultiplayerGame.Managers;

namespace XnaMultiplayerGame.Classes
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public static class PlatformWorld
	{
		public static List<Platform> Platforms { get; set; }
		public static float MoveSpeed { get; set; }

		private static float _elapsed;
		private static float _maxElapsed = 1f; // Delay in seconds before spawning another platform.

		public static void Initialize(float moveSpeed)
		{
			Platforms = new List<Platform>();
			MoveSpeed = -Math.Abs(moveSpeed);
		}

		public static void SpawnPlatform()
		{
			Vector2 pos = GetSpawnPosition();
			SpawnPlatform(pos);
		}

		public static void SpawnPlatform(Vector2 position)
		{
			Platforms.Add(new Platform(new Rectangle((int)position.X, (int)position.Y, Platform.Texture.Width, Platform.Texture.Height)));
		}

		public static void Update()
		{
			float elapsed = TimeManager.Elapsed;

			if (Platforms == null) return; // Don't proceed if we haven't gotten any platforms from the server yet. (implies we're the client, and is only possible to be null if we're the client)

			if (Program.Hosting) // Only spawn platforms if we're the server, else we might spawn platforms only to have them removed when receiving platforms from server.
			{
				_elapsed += elapsed;
				if (_elapsed >= _maxElapsed)
				{
					_elapsed = 0f;
					SpawnPlatform();
				}
			}

			// Update positions and remove from list if they're outside the top of the viewable area.
			var pList = Platforms.ToArray();

			foreach (Platform p in pList)
			{
				p.Position = new Vector2(p.Position.X, p.Position.Y + (Program.Game.BackgroundWall.Speed * elapsed));

				if (p.Position.Y + Platform.Texture.Height < 0)
				{
					Platforms.Remove(p);
				}
			}
		}

		public static void Draw(SpriteBatch sb)
		{
			foreach (Platform p in Platforms)
			{
				p.Draw(sb);
			}
		}

		/// <summary>
		/// Do not confuse with player spawn position! This is the spawn position of a new platform.
		/// </summary>
		/// <returns></returns>
		private static Vector2 GetSpawnPosition()
		{
			return new Vector2(Helper.Rand.Next(10, (int)Helper.GetWindowSize().X - (Platform.Texture.Width + 10)), Helper.GetWindowSize().Y);
		}
	}
}