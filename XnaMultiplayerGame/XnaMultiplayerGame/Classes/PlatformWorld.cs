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
	public class PlatformWorld
	{
		public List<Platform> Platforms { get; private set; }
		public float MoveSpeed { get; set; }

		private float _elapsed;
		private float _maxElapsed = 1f; // In seconds

		public PlatformWorld(float moveSpeed)
		{
			Platforms = new List<Platform>();
			MoveSpeed = -Math.Abs(moveSpeed);
		}

		public void SpawnPlatform()
		{
			Vector2 pos = GetSpawnPosition();
			Platforms.Add(new Platform(new Rectangle((int) pos.X, (int) pos.Y, Platform.Texture.Width, Platform.Texture.Height)));
		}

		public void Update()
		{
			float elapsed = TimeManager.Elapsed;

			_elapsed += elapsed;
			if (_elapsed >= _maxElapsed)
			{
				_elapsed = 0f;
				SpawnPlatform();
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

		public void Draw(SpriteBatch sb)
		{
			foreach (Platform p in Platforms)
			{
				p.Draw(sb);
			}
		}

		private Vector2 GetSpawnPosition()
		{
			return new Vector2(Helper.Rand.Next(10, (int)Helper.GetWindowSize().X - (Platform.Texture.Width + 10)), Helper.GetWindowSize().Y);
		}
	}
}