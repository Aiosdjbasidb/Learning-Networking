// -----------------------------------------------------------------------
// <copyright file="ClientMessageType.cs" company="">
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
	public class ClientMessageType
	{
		public Headers.Client Type { get; private set; }

		public ClientMessageType(int type)
		{
			Type = (Headers.Client) type;
		}
	}
}