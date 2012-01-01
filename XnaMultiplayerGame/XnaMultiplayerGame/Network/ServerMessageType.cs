// -----------------------------------------------------------------------
// <copyright file="ServerMessageType.cs" company="">
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
	public class ServerMessageType
	{
		public Headers.Server Type { get; private set; }

		public ServerMessageType(int type)
		{
			Type = (Headers.Server) type;
		}
	}
}