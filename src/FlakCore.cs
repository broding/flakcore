using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Flakcore;

namespace FlakCore
{
	public class FlakCore : Game
	{
		Vector2 screenSize;
		Type state;

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Core core;

		public FlakCore (Vector2 screenSize, Type state)
		{
			this.screenSize = screenSize;
			this.state = state;

			graphics = new GraphicsDeviceManager (this);
			graphics.IsFullScreen = false;
			Content.RootDirectory = "Content";
		}

		protected override void Initialize ()
		{
			base.Initialize ();
		}

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (graphics.GraphicsDevice);
			core = new Core (screenSize, graphics, Content);
			core.SwitchState (state);
		}

		protected override void Update (GameTime gameTime)
		{
			core.Update (gameTime);
			base.Update (gameTime);
		}

		protected override void Draw (GameTime gameTime)
		{
			core.Draw (spriteBatch, gameTime);
			base.Draw (gameTime);
		}
	}
}

