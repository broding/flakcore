using System;
using Microsoft.Xna.Framework;
using Flakcore.Display;
using Flakcore;
using Microsoft.Xna.Framework.Graphics;

namespace FlakCore
{
	public class Renderer
	{
		public Renderer ()
		{
		}

		public void Draw(State state, SpriteBatch spriteBatch)
		{
			foreach (Layer layer in state.Layers)
			{
				if (layer.Parent != null)
					continue;

				Director.Graphics.GraphicsDevice.SetRenderTarget(layer.RenderTarget);
				Director.Graphics.GraphicsDevice.Clear(Color.Transparent);

				spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.LinearClamp, null, null, layer.Effect, Director.Camera.GetTransformMatrix(layer.ScrollFactor));
				layer.DrawCall(spriteBatch);
				spriteBatch.End();
			}

			Director.Graphics.GraphicsDevice.SetRenderTarget(null);

			foreach (Layer layer in state.Layers)
			{
				if (layer.Parent != null)
					continue;

				spriteBatch.Begin ();

				spriteBatch.Draw(layer.RenderTarget, Vector2.Zero, Color.White);
				spriteBatch.End();
			}
		}
	}
}

