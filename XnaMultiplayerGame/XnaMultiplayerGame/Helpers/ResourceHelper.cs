using Microsoft.Xna.Framework.Media;

namespace XnaMultiplayerGame.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Audio;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;

	/// <summary>
	/// Makes content loading easier and faster.
	/// </summary>
	public static class ResourceHelper
	{
		/// <summary>
		/// The loaded images.
		/// </summary>
		private static Dictionary<string, Texture2D> _textures;

		/// <summary>
		/// The loaded sounds.
		/// </summary>
		private static Dictionary<string, SoundEffect> _soundEffects;

		/// <summary>
		/// The loaded fonts.
		/// </summary>
		private static Dictionary<string, SpriteFont> _fonts;

		/// <summary>
		/// The loaded songs.
		/// </summary>
		private static Dictionary<string, Song> _songs;

		/// <summary>
		/// The XNA game's ContentManager.
		/// </summary>
		public static ContentManager XnaContentManager { get; set; }

		/// <summary>
		/// Initializes the ResourceHelper.
		/// </summary>
		/// <param name="content">The XNA game's ContentManager.</param>
		public static void Initialize(ContentManager content)
		{
			_textures = new Dictionary<string, Texture2D>();
			_soundEffects = new Dictionary<string, SoundEffect>();
			_fonts = new Dictionary<string, SpriteFont>();
			_songs = new Dictionary<string, Song>();
			XnaContentManager = content;
		}

		/// <summary>
		/// Loads an image file if it isn't already loaded.
		/// </summary>
		/// <param name="path">The path to the image file.</param>
		/// <returns>The loaded Texture2D</returns>
		public static Texture2D LoadTexture(string path)
		{
			if (!_textures.ContainsKey(path))
			{
				_textures.Add(path, XnaContentManager.Load<Texture2D>(path));
			}

			return _textures[path];
		}

		/// <summary>
		/// Loads a sound file if it isn't already loaded.
		/// </summary>
		/// <param name="path">The path to the sound file.</param>
		/// <returns>The loaded SoundEffect.</returns>
		public static SoundEffect LoadSound(string path)
		{
			if (!_soundEffects.ContainsKey(path))
			{
				_soundEffects.Add(path, XnaContentManager.Load<SoundEffect>(path));
			}

			return _soundEffects[path];
		}

		/// <summary>
		/// Loads a fontName if it isn't already loaded.
		/// </summary>
		/// <param name="name">The name to the fontName.</param>
		/// <returns>The loaded SpriteFont.</returns>
		public static SpriteFont LoadFont(string name)
		{
			if (!_fonts.ContainsKey(name))
			{
				_fonts.Add(name, XnaContentManager.Load<SpriteFont>(name));
			}

			return _fonts[name];
		}

		public static Song LoadSong(string path)
		{
			if (!_songs.ContainsKey(path))
			{
				_songs.Add(path, XnaContentManager.Load<Song>(path));
			}

			return _songs[path];
		}

		public static void UnloadContent()
		{
			MediaPlayer.Stop();
		}
	}
}