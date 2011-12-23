// -----------------------------------------------------------------------
// <copyright file="Player.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Forms;

namespace ConsoleSidescroll
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
		public Drawable Drawable { get; private set; }
		public Vector2 Position
		{
			get { return Drawable.Position; }
			set { Drawable.Position = value; }
		}
		public char DrawChar
		{
			get { return Drawable.DrawChar; }
			set { Drawable.DrawChar = value; }
		}
		public int Id { get; set; }
		public string Name { get; set; }

		public Player(Vector2 position, char drawChar = '$')
		{
			Drawable = new Drawable(position, drawChar, ConsoleColor.White);
		}

		public void Draw()
		{
			DrawManager.Draw(Drawable);
		}

		public void Draw(ConsoleColor color)
		{
			DrawManager.Draw(Drawable, color);
		}

		public void Move(int x, int y)
		{
			Drawable.Position += new Vector2(x, y);
		}

		public void Move(Vector2 add)
		{
			Move(add.X, add.Y);
		}
	}
}