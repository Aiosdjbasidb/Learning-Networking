// -----------------------------------------------------------------------
// <copyright file="Vector2.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.Remoting.Messaging;

namespace ConsoleSidescroll
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: A vector that holds two integer values.
	/// </summary>
	public class Vector2
	{
		public int X { get; set; }
		public int Y { get; set; }

		/// <summary>
		/// Returns a Vector2 with values set to 0.
		/// </summary>
		public static Vector2 Zero
		{
			get { return new Vector2(); }
		}

		/// <summary>
		/// Returns a Vector2 with values set to 0.
		/// </summary>
		public static Vector2 Invalid
		{
			get { return new Vector2(0, 0); }
		}

		/// <summary>
		/// Initializes both values to 0.
		/// </summary>
		public Vector2()
		{
			X = 0;
			Y = 0;
		}

		/// <summary>
		/// Initializes both values to x.
		/// </summary>
		/// <param name="x"></param>
		public Vector2(int x)
		{
			X = x;
			Y = x;
		}

		/// <summary>
		/// Initializes values to given parameters.
		/// </summary>
		/// <param name="x">X value.</param>
		/// <param name="y">Y value.</param>
		public Vector2(int x, int y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Returns a Vector2 with the position of lerping from a to b with the given amount.
		/// </summary>
		/// <param name="a">The vector to lerp.</param>
		/// <param name="b">The target vector.</param>
		/// <param name="amount">The amount to lerp.</param>
		/// <returns><c>Vector2</c></returns>
		public static Vector2 Lerp(Vector2 a, Vector2 b, float amount)
		{
			var vector = new Vector2();
			vector.X = (int) MathHelper.Lerp(a.X, b.X, amount);
			vector.Y = (int) MathHelper.Lerp(a.Y, b.Y, amount);
			return vector;
		}

		/// <summary>
		/// Lerps this Vector2's position to the target position with the given amount.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="amount"></param>
		public void Lerp(Vector2 target, float amount)
		{
			var pos = Lerp(this, target, amount);

			X = pos.X;
			Y = pos.Y;
		}

		public static Vector2 operator +(Vector2 a, Vector2 b)
		{
			int x = a.X + b.X;
			int y = a.Y + b.Y;

			return new Vector2(x, y);
		}

		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			int x = a.X - b.X;
			int y = a.Y - b.Y;

			return new Vector2(x, y);
		}

		public static Vector2 operator *(Vector2 a, Vector2 b)
		{
			int x = a.X*b.X;
			int y = a.Y*b.Y;

			return new Vector2(x, y);
		}

		public static Vector2 operator /(Vector2 a, Vector2 b)
		{
			if (a.X <= 0 || a.Y <= 0)
				return new Vector2();

			int x = a.X/b.X;
			int y = a.Y/b.Y;

			return new Vector2(x, y);
		}

		public static bool operator ==(Vector2 a, Vector2 b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator !=(Vector2 a, Vector2 b)
		{
			return !(a == b);
		}

		public override string ToString()
		{
			return "{ " + X + ", " + Y + " }";
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
	}
}