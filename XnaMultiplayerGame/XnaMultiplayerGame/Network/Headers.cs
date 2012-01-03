// -----------------------------------------------------------------------
// <copyright file="Headers.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace XnaMultiplayerGame.Network
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class Headers
	{
		public enum Client
		{
			/// <summary>
			/// id, x, y, w, h, r, g, b, a
			/// </summary>
			NewPlayerInfo,

			/// <summary>
			/// x, y, velX, velY
			/// </summary>
			PlayerInfo,

			/// <summary>
			/// x, y, velX, velY, r, g, b, a, moveDirX, moveDirY
			/// </summary>
			ServerPlayers,

			/// <summary>
			/// x, y, w, h
			/// </summary>
			ServerPlatforms
		}

		public enum Server
		{
			/// <summary>
			/// x, y
			/// </summary>
			SetMoveDirection,

			/// <summary>
			/// x, y
			/// </summary>
			SetVelocity,

			/// <summary>
			/// id (send data to the client with the given id)
			/// </summary>
			GetPlayers,

			/// <summary>
			/// id (send data to the client with the given id)
			/// </summary>
			GetPlatforms
		}
	}
}