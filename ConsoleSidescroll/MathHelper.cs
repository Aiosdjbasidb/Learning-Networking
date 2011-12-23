// -----------------------------------------------------------------------
// <copyright file="MathHelper.cs" company="">
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
	public static class MathHelper
	{
		public static float Clamp(float value, float min, float max)
		{
			if (value < min) return min;
			if (value > max) return max;
			return value;
		}

		public static float Lerp(float a, float b, float amount)
		{
			return (a + ((b - a) * amount));
		}
	}
}