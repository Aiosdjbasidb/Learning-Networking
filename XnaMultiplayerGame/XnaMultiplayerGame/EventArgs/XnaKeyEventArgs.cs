// -----------------------------------------------------------------------
// <copyright file="XnaKeyEventArgs.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Xna.Framework.Input;

namespace XnaMultiplayerGame.EventArgs
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class XnaKeyEventArgs : EventArgs
	{
		public delegate void XnaKeyEventHandler(object sender, XnaKeyEventArgs args);

		public Keys Key { get; private set; }

		public XnaKeyEventArgs(Keys key)
		{
			Key = key;
		}
	}
}