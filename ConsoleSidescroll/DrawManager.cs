// -----------------------------------------------------------------------
// <copyright file="DrawManager.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using MS.Internal.Xml.XPath;

namespace ConsoleSidescroll
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public static class DrawManager
	{
		private static List<Drawable> _drawables;
		private static List<TextDrawable> _textDrawables;

		private static List<Drawable> _oldDrawables;
		private static List<TextDrawable> _oldTextDrawables; 

		private struct TextDrawable
		{
			public Vector2 Position;
			public string Text;
			public ConsoleColor Color;

			public TextDrawable(string text, Vector2 position, ConsoleColor color)
			{
				Text = text;
				Position = position;
				Color = color;
			}
		}

		public static void Initialize()
		{
			_drawables = new List<Drawable>();
			_textDrawables = new List<TextDrawable>();
			_oldDrawables = new List<Drawable>();
			_oldTextDrawables = new List<TextDrawable>();
		}

		public static void Draw(Drawable drawable)
		{
			_drawables.Add(drawable);
		}

		public static void Draw(Drawable drawable, ConsoleColor color)
		{
			var d = drawable;
			d.Color = color;

			_drawables.Add(d);
		}

		public static void DrawString(string text, Vector2 position, ConsoleColor color)
		{
			_textDrawables.Add(new TextDrawable(text, position, color));
		}

		public static void Paint()
		{
			//Clear();
			Console.Clear();

			foreach (var drawable in _drawables)
			{
				Console.ForegroundColor = drawable.Color;
				Console.SetCursorPosition(drawable.Position.X, drawable.Position.Y);
				Console.Write(drawable.DrawChar);
			}

			foreach (var text in _textDrawables)
			{
				Console.ForegroundColor = text.Color;
				Console.SetCursorPosition(text.Position.X, text.Position.Y);
				Console.Write(text.Text);
			}

			ClearBuffer();
		}

		private static void Clear()
		{
			foreach (var drawable in _oldDrawables)
			{
				Console.SetCursorPosition(drawable.Position.X, drawable.Position.Y);
				Console.Write(" ");
			}

			foreach (var textDrawable in _oldTextDrawables)
			{
				for (int x = textDrawable.Position.X; x < textDrawable.Text.Length; x++)
				{
					Console.SetCursorPosition(x, textDrawable.Position.Y);
					Console.Write(" ");
				}
			}
		}

		private static void ClearBuffer()
		{
			_oldDrawables = _drawables;
			_oldTextDrawables = _textDrawables;

			_drawables.Clear();
			_textDrawables.Clear();
		}
	}
}