// -----------------------------------------------------------------------
// <copyright file="Platform.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaMultiplayerGame.Helpers;

namespace XnaMultiplayerGame.Classes
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class Platform
	{
		public static Texture2D Texture;

		public Rectangle BoundingBox { get; private set; }

		public Vector2 Position
		{
			get { return new Vector2(BoundingBox.X, BoundingBox.Y); }
			set { BoundingBox = new Rectangle((int) value.X, (int) value.Y, BoundingBox.Width, BoundingBox.Height); }
		}

		public Platform(Rectangle positionSize)
		{
			BoundingBox = positionSize;
		}

		public void Draw(SpriteBatch sb)
		{
			sb.Draw(Texture, BoundingBox, Color.White);
		}
	}
}