using System.Windows.Forms;
using XnaMultiplayerGame.EventArgs;

namespace InputManager
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Input;

	/// <summary>
	/// Used for managing input.
	/// </summary>
	public static class InputManager
	{
		/// <summary>
		/// The current KeyboardState.
		/// </summary>
		private static KeyboardState CurrentKeyboardState { get; set; }

		/// <summary>
		/// The last frame's KeyboardState.
		/// </summary>
		private static KeyboardState OldKeyboardState { get; set; }

		/// <summary>
		/// The current MouseState.
		/// </summary>
		private static MouseState CurrentMouseState { get; set; }

		/// <summary>
		/// The last frame's MouseState.
		/// </summary>
		private static MouseState OldMouseState { get; set; }

		/// <summary>
		/// Tells whether the window is in focus or not.
		/// </summary>
		private static bool _active;

		/// <summary>
		/// Gets the mouse position.
		/// </summary>
		public static Vector2 MousePosition { get; private set; }

		/// <summary>
		/// Gets the last frame's mouse position.
		/// </summary>
		public static Vector2 OldMousePosition { get; private set; }

		/// <summary>
		/// Updates the input information.
		/// </summary>
		/// <param name="ks">The KeyboardState.</param>
		/// <param name="mt">The MouseState.</param>
		/// <param name="windowActive">If false, no input will be accepted in the program.</param>
		public static void Update(KeyboardState ks, MouseState mt, bool windowActive)
		{
			OldKeyboardState = CurrentKeyboardState;
			OldMouseState = CurrentMouseState;
			OldMousePosition = MousePosition;

			CurrentKeyboardState = ks;
			CurrentMouseState = mt;
			MousePosition = new Vector2(mt.X, mt.Y);

			_active = windowActive;

			DoEvents();
		}

		/// <summary>
		/// Used for determining if a keyType is being pressed.
		/// </summary>
		/// <param name="key">Key to check if pressed.</param>
		/// <returns>Returns true if keyType is down.</returns>
		public static bool KeyPressed(Keys key)
		{
			if (_active && CurrentKeyboardState.IsKeyDown(key))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used for determining if a keyType was just pressed.
		/// </summary>
		/// <param name="key">Key to check if pressed.</param>
		/// <returns>Returns true if keyType is down this frame but not last.</returns>
		public static bool KeyJustPressed(Keys key)
		{
			if (_active && CurrentKeyboardState.IsKeyDown(key) && OldKeyboardState.IsKeyUp(key))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used for determining if a keyType was just released.
		/// </summary>
		/// <param name="key">Key to check if released.</param>
		/// <returns>Returns true if keyType is up and last frame is down.</returns>
		public static bool KeyJustReleased(Keys key)
		{
			if (_active && OldKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used for determining if all keys in an array are being pressed.
		/// </summary>
		/// <param name="keys">Keys to check if pressed.</param>
		/// <returns>Returns true if all keys are down.</returns>
		public static bool AreKeysDown(params Keys[] keys)
		{
			if (_active)
			{
				foreach (Keys key in keys)
				{
					if (!KeyPressed(key))
					{
						return false;
					}
				}
			}
			else
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Used for determining if either of the keys in an array are being pressed.
		/// </summary>
		/// <param name="keys">Keys to check if pressed.</param>
		/// <returns>Returns true if either keyType is being pressed.</returns>
		public static bool IsEitherKeyDown(params Keys[] keys)
		{
			if (_active)
			{
				foreach (Keys key in keys)
				{
					if (KeyPressed(key))
					{
						return true;
					}
				}
			}
			else
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Used for determining if the left mouse button is being pressed.
		/// </summary>
		/// <returns>Returns true if the left mouse button is being pressed.</returns>
		public static bool LMBPressed()
		{
			if (_active && CurrentMouseState.LeftButton == ButtonState.Pressed)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used for determining if the left mouse button was just pressed.
		/// </summary>
		/// <returns>Returns true if the left mouse buttion is pressed this frame, but not the last frame.</returns>
		public static bool LMBJustPressed()
		{
			if (_active && CurrentMouseState.LeftButton == ButtonState.Pressed && OldMouseState.LeftButton == ButtonState.Released)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used for determining if the left mouse button was just released.
		/// </summary>
		/// <returns>Returns true if the left mouse button is released, but pressed in the last frame.</returns>
		public static bool LMBJustReleased()
		{
			if (_active && CurrentMouseState.LeftButton == ButtonState.Released && OldMouseState.LeftButton == ButtonState.Pressed)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used for determining if the right mouse button is being pressed.
		/// </summary>
		/// <returns>Return true if the right mouse button is being pressed.</returns>
		public static bool RMBPressed()
		{
			if (_active && CurrentMouseState.RightButton == ButtonState.Pressed)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used for determining if the right mouse button was just pressed.
		/// </summary>
		/// <returns>Returns true if the right mouse button is pressed, but not in the last frame.</returns>
		public static bool RMBJustPressed()
		{
			if (_active && CurrentMouseState.RightButton == ButtonState.Pressed && OldMouseState.RightButton == ButtonState.Released)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used for determining if the right mouse button was just released.
		/// </summary>
		/// <returns>Returns true if the right mouse button is released, but not in the last frame.</returns>
		public static bool RMBJustReleased()
		{
			if (_active && CurrentMouseState.RightButton == ButtonState.Released && OldMouseState.RightButton == ButtonState.Pressed)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used for determining if the middle mouse button is being pressed.
		/// </summary>
		/// <returns>Returns true if the middle mouse button is being pressed.</returns>
		public static bool MMBPressed()
		{
			if (_active && CurrentMouseState.MiddleButton == ButtonState.Pressed)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used for determining if the middle mouse button was just pressed.
		/// </summary>
		/// <returns>Returns true if the middle mouse buttion is pressed, but not in the last frame.</returns>
		public static bool MMBJustPressed()
		{
			if (_active && CurrentMouseState.MiddleButton == ButtonState.Pressed && OldMouseState.MiddleButton == ButtonState.Released)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used for determining if the middle mouse button was just released.
		/// </summary>
		/// <returns>Returns true if the middle mouse button was pressed last frame, but not this one.</returns>
		public static bool MMBJustReleased()
		{
			if (_active && CurrentMouseState.MiddleButton == ButtonState.Released && OldMouseState.MiddleButton == ButtonState.Pressed)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used for determining if the scroll has been scrolled upwards.
		/// </summary>
		/// <returns>Returns true if the scroll has been scrolled upwards.</returns>
		public static bool ScrollIncreased()
		{
			if (CurrentMouseState.ScrollWheelValue > OldMouseState.ScrollWheelValue)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used for determining if the scrool has been scrolled downwards.
		/// </summary>
		/// <returns>Returns true if the scroll has been scrolled downwards.</returns>
		public static bool ScrollDecreased()
		{
			if (CurrentMouseState.ScrollWheelValue < OldMouseState.ScrollWheelValue)
			{
				return true;
			}

			return false;
		}

		public static Keys[] GetPressedKeys()
		{
			return CurrentKeyboardState.GetPressedKeys();
		}

		public static event MouseEventHandler MouseDown;
		public static event MouseEventHandler MouseUp;
		public static event MouseEventHandler MouseMoved;
		public static event XnaKeyEventArgs.XnaKeyEventHandler KeyDown;

		private static void OnMouseDown(MouseEventArgs args)
		{
			if (MouseDown != null)
			{
				MouseDown(null, args);
			}
		}

		private static void OnMouseUp(MouseEventArgs args)
		{
			if (MouseUp != null)
			{
				MouseUp(null, args);
			}
		}

		private static void OnMouseMoved(MouseEventArgs args)
		{
			if (MouseMoved != null)
			{
				MouseMoved(null, args);
			}
		}

		private static void OnKeyPressed(XnaKeyEventArgs args)
		{
			if (KeyDown != null)
			{
				KeyDown(null, args);
			}
		}

		private static void DoEvents()
		{
			if (LMBJustPressed())
			{
				OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, (int) MousePosition.X, (int) MousePosition.Y, 0));
			}
			else if (RMBJustPressed())
			{
				OnMouseDown(new MouseEventArgs(MouseButtons.Right, 1, (int)MousePosition.X, (int)MousePosition.Y, 0));
			}
			else if (MMBJustPressed())
			{
				OnMouseDown(new MouseEventArgs(MouseButtons.Middle, 1, (int)MousePosition.X, (int)MousePosition.Y, 0));
			}
			else if (LMBJustReleased())
			{
				OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, (int) MousePosition.X, (int) MousePosition.Y, 0));
			}
			else if (RMBJustReleased())
			{
				OnMouseUp(new MouseEventArgs(MouseButtons.Right, 1, (int) MousePosition.X, (int) MousePosition.Y, 0));
			}
			else if (MMBJustReleased())
			{
				OnMouseUp(new MouseEventArgs(MouseButtons.Middle, 1, (int) MousePosition.X, (int) MousePosition.Y, 0));
			}

			if (MousePosition != OldMousePosition)
			{
				OnMouseMoved(new MouseEventArgs(MouseButtons.None, 0, (int) MousePosition.X, (int) MousePosition.Y, 0));
			}

			foreach (Keys key in GetPressedKeys())
			{
				if (KeyJustPressed(key))
				{
					OnKeyPressed(new XnaKeyEventArgs(key));
				}
			}
		}

		public static bool IsCapsOn()
		{
			// If neither shift is pressed, return the state of caps lock.
			if (!KeyPressed(Keys.LeftShift) && !KeyPressed(Keys.RightShift))
			{
				return Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock);
			}

			// If shift is pressed, return the opposite state of caps lock.
			return !Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock);
		}

		public static MouseButtons GetHeldMouseButton()
		{
			if (LMBPressed())
				return MouseButtons.Left;
			if (RMBPressed())
				return MouseButtons.Right;
			if (MMBPressed())
				return MouseButtons.Middle;

			return MouseButtons.None;
		}
	}
}