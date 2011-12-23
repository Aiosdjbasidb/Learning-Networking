﻿// -----------------------------------------------------------------------
// <copyright file="TimeManager.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Xna.Framework;

namespace XnaMultiplayerGame.Managers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public static class TimeManager
	{
		public static float Elapsed { get; private set; }

		private static float _timeMultiplier = 1f;
		public static float TimeMultiplier
		{
			get { return _timeMultiplier; }
			set { _timeMultiplier = value; }
		}

		public static void Update(GameTime gt)
		{
			Elapsed = (float) gt.ElapsedGameTime.TotalSeconds*_timeMultiplier;
		}
	}
}