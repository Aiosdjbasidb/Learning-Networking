using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LidgrenChat
{
	static class Program
	{
		public static k Form { get; private set; }
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Form = new k();

			Application.Run(Form);
		}
	}
}