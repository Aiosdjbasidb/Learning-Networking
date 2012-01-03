// -----------------------------------------------------------------------
// <copyright file="Player.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
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
	public class Player
	{
		private const int MoveSpeed = 300;
		private const int BoostValue = 3000;

		public static Texture2D Texture;

		public static Vector2 TextureSize
		{
			get { return new Vector2(Texture.Width, Texture.Height); }
		}

		/// <summary>
		/// Use when you haven't gotten a size from the server. Make sure this is the same size as the textures size (not TextureSize), as the texture can be modified by the user.
		/// </summary>
		public static Vector2 PlayerSize
		{
			get { return new Vector2(32, 64); }
		}

		public static Vector2 Gravity
		{
			get { return new Vector2(0, 1500); }
		}

		public Color DrawColor { get; set; }

		public Vector2 Position
		{
			get { return new Vector2(BoundingBox.X, BoundingBox.Y); }
			set { BoundingBox = new Rectangle((int) value.X, (int) value.Y, BoundingBox.Width, BoundingBox.Height); }
		}

		public Vector2 Velocity { get; set; }
		public Vector2 MoveDirection { get; set; }
		public Rectangle BoundingBox { get; set; }

		public Player(Vector2 position, Vector2 size, Color drawColor)
		{
			BoundingBox = new Rectangle((int) position.X, (int) position.Y, (int) size.X, (int) size.Y);
			DrawColor = drawColor;
		}

		public void UpdatePhysics(IEnumerable<Platform> platforms)
		{
			float elapsed = TimeManager.Elapsed;

			if (MoveDirection.Y <= -1)
			{
				Velocity = new Vector2(Velocity.X, Velocity.Y - (BoostValue * elapsed));
			}

			var oldPosition = new Vector2(Position.X, Position.Y);

			Velocity += Gravity * elapsed;
			Position += MoveDirection * (MoveSpeed * elapsed);
			Position += Velocity*elapsed;

			// Update collisions
			foreach (Platform p in platforms)
			{
				if (BoundingBox.Intersects(p.BoundingBox))
				{
					Position = oldPosition;

					if (Position.Y + BoundingBox.Height > p.Position.Y + Platform.Texture.Height)
					{
						if(Position.Y >= p.BoundingBox.Bottom)
							Velocity = new Vector2(0, 0);
						else
							Velocity = new Vector2(0, PlatformWorld.MoveSpeed);
					}
					else
					{
						Velocity = new Vector2(Velocity.X, PlatformWorld.MoveSpeed);
						Position = new Vector2(Position.X, p.Position.Y - Texture.Height);
					}

					break;
				}
			}

			if (Position.X + PlayerSize.X > Helper.GetWindowSize().X || Position.X < 0)
				Velocity = new Vector2(0, Velocity.Y);
			if(Position.Y + PlayerSize.Y > Helper.GetWindowSize().Y || Position.Y < 0)
				Velocity = new Vector2(Velocity.X, 0);

			Position = Vector2.Clamp(Position, new Vector2(0, 0),
			                         Helper.GetWindowSize() - new Vector2(BoundingBox.Width, BoundingBox.Height));
		}

		public void Draw(SpriteBatch sb)
		{
			sb.Draw(Texture, BoundingBox, DrawColor);
		}

		public void SetMoveDirection(float x, float y)
		{
			MoveDirection = new Vector2(x, y);
		}
	}
}