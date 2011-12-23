// -----------------------------------------------------------------------
// <copyright file="InputManager.cs" company="">
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
	public static class InputManager
	{
		public static List<ConsoleKeyInfo> PressedKeys { get; private set; }

		public static bool ShiftPressed { get; private set; }
		public static bool CapsLockOn { get; private set; }

		public static void Initialize()
		{
			PressedKeys = new List<ConsoleKeyInfo>();
		}

		public static void Update()
		{
			var keyList = new List<ConsoleKeyInfo>();

			while (Console.KeyAvailable)
			{
				keyList.Add(Console.ReadKey(true));
			}

			PressedKeys = keyList;

			CapsLockOn = Console.CapsLock;
			ShiftPressed = PressedKeys.Any(info => info.Modifiers == ConsoleModifiers.Shift);
		}

		public static bool IsKeyDown(ConsoleKey key)
		{
			return PressedKeys.Any(info => info.Key == key);
		}

		public static string GetKeyChar(ConsoleKey key)
		{
			switch(key)
			{
				// Alphabet.
				case ConsoleKey.A:
					return ShiftPressed || CapsLockOn ? "A" : "a";
				case ConsoleKey.B:
					return ShiftPressed || CapsLockOn ? "B" : "b";
				case ConsoleKey.C:
					return ShiftPressed || CapsLockOn ? "C" : "c";
				case ConsoleKey.D:
					return ShiftPressed || CapsLockOn ? "D" : "d";
				case ConsoleKey.E:
					return ShiftPressed || CapsLockOn ? "E" : "e";
				case ConsoleKey.F:
					return ShiftPressed || CapsLockOn ? "F" : "f";
				case ConsoleKey.G:
					return ShiftPressed || CapsLockOn ? "G" : "g";
				case ConsoleKey.H:
					return ShiftPressed || CapsLockOn ? "H" : "h";
				case ConsoleKey.I:
					return ShiftPressed || CapsLockOn ? "I" : "i";
				case ConsoleKey.J:
					return ShiftPressed || CapsLockOn ? "J" : "j";
				case ConsoleKey.K:
					return ShiftPressed || CapsLockOn ? "K" : "k";
				case ConsoleKey.L:
					return ShiftPressed || CapsLockOn ? "L" : "l";
				case ConsoleKey.M:
					return ShiftPressed || CapsLockOn ? "M" : "m";
				case ConsoleKey.N:
					return ShiftPressed || CapsLockOn ? "N" : "n";
				case ConsoleKey.O:
					return ShiftPressed || CapsLockOn ? "O" : "o";
				case ConsoleKey.P:
					return ShiftPressed || CapsLockOn ? "P" : "p";
				case ConsoleKey.Q:
					return ShiftPressed || CapsLockOn ? "Q" : "q";
				case ConsoleKey.R:
					return ShiftPressed || CapsLockOn ? "R" : "r";
				case ConsoleKey.S:
					return ShiftPressed || CapsLockOn ? "S" : "s";
				case ConsoleKey.T:
					return ShiftPressed || CapsLockOn ? "T" : "t";
				case ConsoleKey.U:
					return ShiftPressed || CapsLockOn ? "U" : "u";
				case ConsoleKey.V:
					return ShiftPressed || CapsLockOn ? "V" : "v";
				case ConsoleKey.W:
					return ShiftPressed || CapsLockOn ? "W" : "w";
				case ConsoleKey.X:
					return ShiftPressed || CapsLockOn ? "X" : "x";
				case ConsoleKey.Y:
					return ShiftPressed || CapsLockOn ? "Y" : "y";
				case ConsoleKey.Z:
					return ShiftPressed || CapsLockOn ? "Z" : "z";

				// Numpad keys
				case ConsoleKey.NumPad0:
					return "0";
				case ConsoleKey.NumPad1:
					return "1";
				case ConsoleKey.NumPad2:
					return "2";
				case ConsoleKey.NumPad3:
					return "3";
				case ConsoleKey.NumPad4:
					return "4";
				case ConsoleKey.NumPad5:
					return "5";
				case ConsoleKey.NumPad6:
					return "6";
				case ConsoleKey.NumPad7:
					return "7";
				case ConsoleKey.NumPad8:
					return "8";
				case ConsoleKey.NumPad9:
					return "9";

				// Keys below F1-F8.
				case ConsoleKey.D0:
					return ShiftPressed ? "=" : "0";
				case ConsoleKey.D1:
					return ShiftPressed ? "!" : "1";
				case ConsoleKey.D2:
					return ShiftPressed ? "\"" : "2";
				case ConsoleKey.D3:
					return ShiftPressed ? "#" : "3";
				case ConsoleKey.D4:
					return ShiftPressed ? "¤" : "4";
				case ConsoleKey.D5:
					return ShiftPressed ? "%" : "5";
				case ConsoleKey.D6:
					return ShiftPressed ? "&" : "6";
				case ConsoleKey.D7:
					return ShiftPressed ? "/" : "7";
				case ConsoleKey.D8:
					return ShiftPressed ? "(" : "8";
				case ConsoleKey.D9:
					return ShiftPressed ? ")" : "9";

				// Special characters.
				case ConsoleKey.Spacebar:
					return " ";
				case ConsoleKey.Tab:
					return "\t";
				case ConsoleKey.Backspace:
					return "\b";
				case ConsoleKey.Enter:
					return "\n";
				case ConsoleKey.Multiply:
					return "*";
				case ConsoleKey.Add:
					return ShiftPressed ? "?" : "+";
				case ConsoleKey.Subtract:
					return "-";

				// Oem keys
				case ConsoleKey.OemComma:
					return ShiftPressed ? ";" : ",";
				case ConsoleKey.OemPeriod:
					return ShiftPressed ? ":" : ".";
				case ConsoleKey.OemMinus:
					return ShiftPressed ? "_" : "-";
				case ConsoleKey.OemPlus:
					return ShiftPressed ? "?" : "+";
				case ConsoleKey.Oem4:
					return ShiftPressed ? "`" : "´";
				case ConsoleKey.Oem5:
					return ShiftPressed ? "½" : "§";
				case ConsoleKey.Oem102:
					return ShiftPressed ? ">" : "<";
			}

			return key.ToString();
		}
	}
}