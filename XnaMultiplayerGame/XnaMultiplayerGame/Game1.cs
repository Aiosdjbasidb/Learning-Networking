using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XnaMultiplayerGame.Classes;
using XnaMultiplayerGame.Helpers;
using XnaMultiplayerGame.Managers;
using XnaMultiplayerGame.Network;

namespace XnaMultiplayerGame
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch sb;

		private Texture2D _backgroundWallTexture;
		private Texture2D _platformTexture;
		private Texture2D _vignette;

		private readonly Color _clearColor = new Color(50, 82, 95, 255);

		public BackgroundWall BackgroundWall;

		private const float DefaultScrollSpeed = 150f;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			Exiting += delegate
			           	{
			           		Listener.StopListening();
			           	};
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// Initialize classes
			sb = new SpriteBatch(GraphicsDevice);
			ResourceHelper.Initialize(Content);

			// Load content.
			_backgroundWallTexture = ResourceHelper.LoadTexture("Backgrounds\\Veins");
			_platformTexture = ResourceHelper.LoadTexture("Objects\\Platform");
			_vignette = ResourceHelper.LoadTexture("Gui\\Vignette");
			Platform.Texture = ResourceHelper.LoadTexture("Objects\\Platform");
			Player.Texture = ResourceHelper.LoadTexture("Objects\\Player");

			// TODO: Add your initialization logic here
			Helper.Initialize(this);

			_graphics.PreferredBackBufferWidth = 768;
			_graphics.PreferredBackBufferHeight = 768;
			_graphics.PreferMultiSampling = true;
			_graphics.SynchronizeWithVerticalRetrace = false;
			_graphics.ApplyChanges();
			IsFixedTimeStep = true;

			BackgroundWall = new BackgroundWall(_backgroundWallTexture, DefaultScrollSpeed);

			if (Program.Hosting)
			{
				PlatformWorld.Initialize(DefaultScrollSpeed);
				Server.Initialize();
				Listener.Initialize();
			}

			LocalClient.Initialize();

			base.Initialize();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			// TODO: Add your update logic here
			InputManager.InputManager.Update(Keyboard.GetState(), Mouse.GetState(), IsActive);
			TimeManager.Update(gameTime);
			BackgroundWall.Update();

			if (Program.Hosting)
			{
				Server.Update();
			}

			LocalClient.Update();
			PlatformWorld.Update();

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(_clearColor);

			// TODO: Add your drawing code here

			sb.Begin();
			BackgroundWall.Draw(sb);

			if (Program.Hosting)
			{
				PlatformWorld.Draw(sb);
			}

			LocalClient.Draw(sb);
			DrawVignette();
			sb.End();

			base.Draw(gameTime);
		}

		private void DrawVignette()
		{
			sb.Draw(_vignette, Helper.GetWindowRectangle(), Color.White);
		}
	}
}