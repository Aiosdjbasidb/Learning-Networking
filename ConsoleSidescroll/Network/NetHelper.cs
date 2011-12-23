// -----------------------------------------------------------------------
// <copyright file="Helpe.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleSidescroll.Network
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
		public static string BuildRequestString(params int[] enumNumbers)
		{
			string returnValue = "";

			foreach (var val in enumNumbers)
			{
				returnValue += val.ToString() + Headers.SplitChar;
			}

			return returnValue;
		}

		public static void SendMessageTo(TcpClient target, string message)
		{
			if (!target.Connected) return;

			byte[] bMessage = Encoding.Unicode.GetBytes(message);
			target.GetStream().Write(bMessage, 0, bMessage.Count());

			/*if (Program.Hosting)
			Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + "] Sent \"" +
			                  message + "\" to " + target.Client.RemoteEndPoint + ".");
			/*else
			{
				TextWriter w = new StreamWriter("clientLog.txt", true);
				w.WriteLine("[" + DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + "] Sent message: \"" + message + "\".");
				w.Close();
			}*/
		}

		public static string ReceiveMessageFrom(TcpClient client)
		{
			if (!client.Connected) return string.Empty;
			if (!client.GetStream().DataAvailable) return string.Empty;

			List<byte> fullByteMessage = new List<byte>();

			do
			{
				byte[] bMessage = new byte[client.Available];
				client.GetStream().Read(bMessage, 0, client.Available);

				foreach (byte b in bMessage)
				{
					fullByteMessage.Add(b);
				}
			} 
			while (client.Available > 0);

			string message = Encoding.Unicode.GetString(fullByteMessage.ToArray());

			if (Program.Hosting)
			Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() +
			                  "] Received message: \"" + message + "\".");
			/*else
			{
				TextWriter w = new StreamWriter("clientLog.txt", true);
				w.WriteLine("[" + DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + "] Received message: \"" + message + "\".");
				w.Close();
			}*/

			return message;
		}
	}
}