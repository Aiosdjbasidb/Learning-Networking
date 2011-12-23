// -----------------------------------------------------------------------
// <copyright file="Drawable.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ConsoleSidescroll
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class Drawable
	{
		public Vector2 Position { get; set; }
		public char DrawChar { get; set; }
		public ConsoleColor Color { get; set; }

		public Drawable(Vector2 position, char drawChar, ConsoleColor color)
		{
			Position = position;
			DrawChar = drawChar;
			Color = color;
		}
	}
}