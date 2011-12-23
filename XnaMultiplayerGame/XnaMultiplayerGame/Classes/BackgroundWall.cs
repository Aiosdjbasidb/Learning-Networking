// -----------------------------------------------------------------------
// <copyright file="BackgroundWall.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaMultiplayerGame.Helpers;
using XnaMultiplayerGame.Managers;

namespace XnaMultiplayerGame.Classes
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class BackgroundWall
	{
		public Texture2D Texture;

		private Vector2[] _positions = new Vector2[2];

		private float _targetSpeed;

		/// <summary>
		/// Speed in pixels per second.
		/// </summary>
		public float Speed { get; private set; }

		private const float LerpAmount = 2;

		private Rectangle _drawRectangle;

		public BackgroundWall(Texture2D texture, float scrollSpeed)
		{
			Texture = texture;
			_positions[0] = Vector2.Zero;
			_positions[1] = new Vector2(0, texture.Height);
			_targetSpeed = -Math.Abs(scrollSpeed);
			Speed = 0f;

			_drawRectangle = new Rectangle(0, 0, (int)Helper.GetWindowSize().X, texture.Height);
		}

		public void Update()
		{
			float elapsed = TimeManager.Elapsed;
			Speed = MathHelper.Lerp(Speed, _targetSpeed, LerpAmount * elapsed);

			for (int i = 0; i < _positions.Length; i++)
			{
				Vector2 other; // The other texture's position.
				switch(i)
				{
					case 0:
						other = _positions[1];
						break;
					case 1:
						other = _positions[0];
						break;
					default:
						other = _positions[0];
						break;
				}

				_positions[i].Y += elapsed*Speed;

				if (_positions[i].Y + Texture.Height < 0)
					_positions[i].Y = other.Y + Texture.Height - 1;
			}
		}

		public void Draw(SpriteBatch sb)
		{
			for (int i = 0; i < 2; i++)
			{
				_drawRectangle.X = (int)_positions[i].X;
				_drawRectangle.Y = (int)_positions[i].Y;
				sb.Draw(Texture, _drawRectangle, Color.White);
			}
		}
	}
}