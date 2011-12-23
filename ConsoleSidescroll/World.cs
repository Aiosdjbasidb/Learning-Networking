// -----------------------------------------------------------------------
// <copyright file="World.cs" company="">
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
	public static class World
	{
		public static List<Drawable> CharTiles { get; set; }

		public static int Width { get; private set; }
		public static int Height { get; private set; }

		public static void Initialize()
		{
			CharTiles = new List<Drawable>();

			Width = 20;
			Height = 20;

			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
					{
						CharTiles.Add(new Drawable(new Vector2(x, y), '#', ConsoleColor.DarkGray));
					}
					else
					{
						CharTiles.Add(new Drawable(new Vector2(x, y), ' ', ConsoleColor.DarkGray));
					}
				}
			}
		}

		public static void Update()
		{
			
		}
	}
}