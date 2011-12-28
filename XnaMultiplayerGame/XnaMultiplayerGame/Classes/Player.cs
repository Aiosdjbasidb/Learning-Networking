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
		public static TcpClient RemoteClient { get; set; }

		public static Texture2D Texture;

		public static Vector2 TextureSize
		{
			get { return new Vector2(Texture.Width, Texture.Height); }
		}

		public static Vector2 Gravity
		{
			get { return new Vector2(0, 1500); }
		}

		public Color DrawColor { get; set; }

		public static bool Noclipping { get; set; }

		public Vector2 Position
		{
			get { return new Vector2(BoundingBox.X, BoundingBox.Y); }
			set { BoundingBox = new Rectangle((int) value.X, (int) value.Y, BoundingBox.Width, BoundingBox.Height); }
		}

		public Vector2 Velocity { get; set; }
		public Rectangle BoundingBox { get; set; }

		public Player(Vector2 position, Vector2 size, Color drawColor, TcpClient remoteClient)
		{
			BoundingBox = new Rectangle((int) position.X, (int) position.Y, (int) size.X, (int) size.Y);
			DrawColor = drawColor;
			RemoteClient = remoteClient;
		}

		public void UpdatePhysics(Platform[] platforms)
		{
			float elapsed = TimeManager.Elapsed;

			if (!Noclipping)
			{
				Velocity += Gravity*elapsed;
				Position += Velocity*elapsed;
			}
			else
			{
				Position = InputManager.InputManager.MousePosition;
				Velocity = Vector2.Zero;
			}

			// Update collisions
			foreach (Platform p in platforms)
			{
				if (BoundingBox.Intersects(p.BoundingBox))
				{
					Velocity = new Vector2(Velocity.X, 0);
					Position = new Vector2(Position.X, p.Position.Y - Texture.Height);

					break;
				}
			}

			Position = Vector2.Clamp(Position, new Vector2(0, 0),
			                         Helper.GetWindowSize() - new Vector2(BoundingBox.Width, BoundingBox.Height));
		}

		public void Draw(SpriteBatch sb)
		{
			sb.Draw(Texture, BoundingBox, DrawColor);
		}

		public void Move(float x, float y)
		{
			Position = new Vector2(Position.X + x, Position.Y + y);
		}
	}
}