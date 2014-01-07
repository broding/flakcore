using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Flakcore.Display
{
    public class State : Node
    {
        public Color BackgroundColor { get; protected set; }

		private List<Layer> _layers;

        public State()
        {
			this.BackgroundColor = Color.DarkSlateGray;

			_layers = new List<Layer> ();
        }

        public virtual void Load()
        {
        }

		protected void AddLayer(Layer layer)
		{
			_layers.Add (layer);
		}

		protected void RemoveLayer(Layer layer)
		{
			_layers.Remove (layer);
		}

        public override List<Node> GetAllChildren(List<Node> nodes)
        {
            if (Children.Count == 0)
                return nodes;
            else
            {
                foreach (Node child in Children)
                {
                    child.GetAllChildren(nodes);
                }

                return nodes;
            }
        }

		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);

			foreach (Layer layer in _layers)
				layer.Update (gameTime);
		}

		public override void PostUpdate (GameTime gameTime)
		{
			base.PostUpdate (gameTime);

			foreach (Layer layer in _layers)
				layer.PostUpdate (gameTime);
		}

		public override void DrawCall (SpriteBatch spriteBatch, DrawProperties worldProperties)
		{
			spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.LinearClamp, null, null, null, Director.CurrentDrawCamera.GetTransformMatrix());
			base.DrawCall (spriteBatch, worldProperties);
			spriteBatch.End();

			foreach (Layer layer in _layers)
			{
				if (layer.Parent != null)
					continue;

				Director.Graphics.GraphicsDevice.SetRenderTarget(layer.RenderTarget);
				Director.Graphics.GraphicsDevice.Clear(Color.Transparent);

				spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.LinearClamp, null, null, layer.Effect, Director.CurrentDrawCamera.GetTransformMatrix());
				layer.DrawCall(spriteBatch);
				spriteBatch.End();
			}

			Director.Graphics.GraphicsDevice.SetRenderTarget(null);

			foreach (Layer layer in _layers)
			{
				if (layer.Parent != null)
					continue;

				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

				spriteBatch.Draw(layer.RenderTarget, Vector2.Zero, Color.White);
				spriteBatch.End();
			}
		}
    }

    public enum StateTransition
    {
        IMMEDIATELY,
        FADE
    }
}
