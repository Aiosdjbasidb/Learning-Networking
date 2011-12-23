// -----------------------------------------------------------------------
// <copyright file="Headers.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ConsoleSidescroll.Network
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public static class Headers
	{
		public const char SplitChar = ';';

		public enum DataType
		{
			/// <summary>
			/// Format: id;x;y.
			/// </summary>
			SetPlayerPosition,

			/// <summary>
			/// Format: id;charInt.
			/// </summary>
			SetPlayerChar,

			/// <summary>
			/// Format: id;colorInt.
			/// </summary>
			SetPlayerColor,

			/// <summary>
			/// Format: id;nameString.
			/// </summary>
			SetPlayerName,

			/// <summary>
			/// Returns: id
			/// </summary>
			RequestJoin,

			/// <summary>
			/// Format: requestDataType
			/// </summary>
			RequestData,

			/// <summary>
			/// Format: id;message;color
			/// </summary>
			SendChatMessage
		}

		public enum RequestDataType
		{
			/// <summary>
			/// Format: id
			/// Returns the X and Y position of the player.
			/// </summary>
			GetPlayerPosition,

			/// <summary>
			/// Returns info for all players. each player is in format of: id:x:y:drawChar.
			/// </summary>
			GetPlayersInfo,

			/// <summary>
			/// Returns an integer that represents a free id on the server. Returns -1 if the server is full.
			/// </summary>
			GetFreeSlot,

			/// <summary>
			/// Returns the world sprites, in the format of: x:y:drawCharInt:colorInt;
			/// </summary>
			GetWorldTiles
		}
	}
}