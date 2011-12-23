using System.Windows.Forms;
using XnaMultiplayerGame;

namespace XnaMultiplayerGame.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Audio;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.GamerServices;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;
	using Microsoft.Xna.Framework.Media;

	/// <summary>
	/// Helper class for various things making development easier.
	/// </summary>
	public static class Helper
	{
		/// <summary>
		/// The XNA game that owns this class.
		/// </summary>
		private static Game1 game;

		/// <summary>
		/// Gets an initialized Random class.
		/// </summary>
		public static Random Rand { get; private set; }

		/// <summary>
		/// Gets the time multiplier.
		/// </summary>
		public static float TimeMultiplier { get; private set; }

		/// <summary>
		/// Gets a white 1x1 Texture2D.
		/// </summary>
		public static Texture2D WhitePixel { get; private set; }

		/// <summary>
		/// Initializes the helper class.
		/// </summary>
		/// <param name="gameClass">The XNA class that owns this class.</param>
		public static void Initialize(Game1 gameClass)
		{
			game = gameClass;
			Rand = new Random();
			TimeMultiplier = 1f;

			WhitePixel = new Texture2D(gameClass.GraphicsDevice, 1, 1);

			Color[] c = new Color[1];
			c[0] = Color.White;

			WhitePixel.SetData<Color>(c);
		}

		/// <summary>
		/// Returns the center of the window.
		/// </summary>
		/// <returns>The center of the window.</returns>
		public static Vector2 GetWindowCenter()
		{
			return new Vector2(game.GraphicsDevice.Viewport.Width * 0.5f, game.GraphicsDevice.Viewport.Height * 0.5f);
		}

		/// <summary>
		/// Returns the center of the window minus half of the size.
		/// </summary>
		/// <param name="size">The size</param>
		/// <returns>The center of the window for the given Vector.</returns>
		public static Vector2 GetWindowCenter(Vector2 size)
		{
			return GetWindowCenter() - (size / 2f);
		}

		/// <summary>
		/// Returns the game's viewport size.
		/// </summary>
		/// <returns>The game's viewport size.</returns>
		public static Vector2 GetWindowSize()
		{
			return new Vector2(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
		}

		/// <summary>
		/// Adds the given value to the current TimeMultiplier.
		/// </summary>
		/// <param name="add">Value to add.</param>
		public static void ModifyTimeMultiplier(float add)
		{
			SetTimeMultiplier(TimeMultiplier + add);
		}

		/// <summary>
		/// Sets the TimeMultiplier to the given value.
		/// </summary>
		/// <param name="value">The value to set the TimeMultiplier to.</param>
		public static void SetTimeMultiplier(float value)
		{
			TimeMultiplier = value;

			TimeMultiplier = MathHelper.Clamp(TimeMultiplier, 0.005f, 1f);
		}

		/// <summary>
		/// Creates a random color and then returns it.
		/// </summary>
		/// <returns>A random color.</returns>
		public static Color GetRandomColor()
		{
			Color c;
			float r, g, b;

			r = (float)Rand.NextDouble() * 255;
			g = (float)Rand.NextDouble() * 255;
			b = (float)Rand.NextDouble() * 255;

			c = new Color((byte)r, (byte)g, (byte)b, 255);

			return c;
		}

		/// <summary>
		/// Converts a Vector2 to a degree angle.
		/// </summary>
		/// <param name="vector">The Vector2 to convert.</param>
		/// <returns>Returns a float that represents the given Vector2.</returns>
		public static float GetDegreesFromVector2(Vector2 vector)
		{
			float returnFloat = (float)Math.Atan2(vector.Y, vector.X);
			returnFloat = MathHelper.ToDegrees(returnFloat);

			return returnFloat;
		}

		/// <summary>
		/// Converts a degree angle to a Vector2.
		/// </summary>
		/// <param name="degree">The degree angle to convert.</param>
		/// <returns>Returns a Vector2 that represents the given degree angle.</returns>
		public static Vector2 GetVector2FromDegrees(float degree)
		{
			Vector2 returnVector;

			returnVector.X = (float)Math.Sin(degree);
			returnVector.Y = (float)Math.Cos(degree);

			return returnVector;
		}

		/// <summary>
		/// Decides whether a's X value is bigger than b's X value.
		/// </summary>
		/// <param name="a">The Vector2 to be checked</param>
		/// <param name="b">The Vector2 to be compared against.</param>
		/// <returns>Returns true if a's X value is bigger than b's Y value.</returns>
		public static bool VectorXIsBigger(Vector2 a, Vector2 b)
		{
			if (a.X > b.X)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Decides whether a's Y value is bigger than b's Y value.
		/// </summary>
		/// <param name="a">The Vector2 to be checked.</param>
		/// <param name="b">The Vector2 to be compared against.</param>
		/// <returns>Returns true if a's Y value is bigger than b's Y value.</returns>
		public static bool VectorYIsBigger(Vector2 a, Vector2 b)
		{
			if (a.Y > b.Y)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Decides whether a's X and Y values are bigger than b's X and Y values.
		/// </summary>
		/// <param name="a">The Vector2 to be checked.</param>
		/// <param name="b">The Vector2 to be compared against.</param>
		/// <returns>Returns true if a's X and Y values are bigger than b's X and Y values.</returns>
		public static bool VectorIsBigger(Vector2 a, Vector2 b)
		{
			if (VectorXIsBigger(a, b) && VectorYIsBigger(a, b))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns a rectangle with the size set to the size of the window.
		/// </summary>
		/// <returns>Returns a rectangle with the size of the window.</returns>
		public static Rectangle GetWindowRectangle()
		{
			int width = (int)GetWindowSize().X;
			int height = (int)GetWindowSize().Y;

			return new Rectangle(0, 0, width, height);
		}

		/// <summary>
		/// Draws an outlining around the given rectangle to the given spritebatch in the given color.
		/// </summary>
		/// <param name="sb">The SpriteBatch to draw to.</param>
		/// <param name="rectangle">The rectangle to outline.</param>
		/// <param name="color">The color to draw in.</param>
		/// <param name="thickness">The thickness in pixels.</param>
		public static void DrawRectangleOutline(SpriteBatch sb, Rectangle rectangle, Color color, int thickness)
		{
			sb.Draw(WhitePixel, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width + thickness, thickness), color); // Top horizonal.
			sb.Draw(WhitePixel, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width + thickness, thickness), color); // Bottom horizontal.
			sb.Draw(WhitePixel, new Rectangle(rectangle.Left, rectangle.Top, thickness, rectangle.Height + thickness), color); // Left vertical.
			sb.Draw(WhitePixel, new Rectangle(rectangle.Right, rectangle.Top, thickness, rectangle.Height + thickness), color); // Right vertical.
		}

		public static Vector2 GetRandomVector(int minX, int minY, int maxX, int maxY)
		{
			return new Vector2(Rand.Next(minX, maxX), Rand.Next(minY, maxY));
		}

		public static Vector2 GetRandomVector(int maxX, int maxY)
		{
			return GetRandomVector(0, 0, maxX, maxY);
		}

		public static Vector2 GetRandomVector()
		{
			return GetRandomVector((int)GetWindowSize().X, (int)GetWindowSize().Y);
		}

		public static string ReverseString(string s)
		{
			string temp = string.Empty;

			for (int i = s.Length - 1; i >= 0; i--)
			{
				temp += s[i].ToString();
			}

			return temp;
		}
	}
}