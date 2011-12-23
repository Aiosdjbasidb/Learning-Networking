// -----------------------------------------------------------------------
// <copyright file="NetHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Net.Sockets;

namespace XnaMultiplayerGame.Network
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public static class NetHelper
	{
		public const char SplitChar = ';';

		public static string[] ReceiveMessageFrom(TcpClient target)
		{
			byte[] bMessage = new byte[target.Available];

			target.GetStream().Read(bMessage, 0, target.Available);

			string message = Encoding.Unicode.GetString(bMessage);

			Console.WriteLine("Received message from " + target.Client.RemoteEndPoint + ": \"" + message + "\".");

			return message.Split(SplitChar);
		}

		public static void SendMessageTo(TcpClient target, string message)
		{
			byte[] bMessage = Encoding.Unicode.GetBytes(message);
			target.GetStream().Write(bMessage, 0, bMessage.Count());
			Console.WriteLine("Sent message to " + target.Client.RemoteEndPoint + ": \"" + message + "\".");
		}

		public static string BuildMessage(params float[] contents)
		{
			string buffer = string.Empty;
			foreach (int msg in contents)
			{
				buffer += msg.ToString() + SplitChar;
			}

			return buffer;
		}
	}
}