using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Flakcore.Display
{
	public class State : IDisposable
    {
        public Color BackgroundColor { get; protected set; }
		public List<Layer> Layers { get; private set; }

        public State()
        {
			this.BackgroundColor = Color.DarkSlateGray;

			Layers = new List<Layer> ();
        }

        public virtual void Load()
        {
        }

		protected void AddLayer(Layer layer)
		{
			Layers.Add (layer);
		}

		protected void RemoveLayer(Layer layer)
		{
			Layers.Remove (layer);
		}

		public virtual void Update (GameTime gameTime)
		{
			foreach (Layer layer in Layers)
				layer.Update (gameTime);
		}

		public virtual void PostUpdate (GameTime gameTime)
		{
			foreach (Layer layer in Layers)
				layer.PostUpdate (gameTime);
		}

		public void Dispose()
		{
			foreach (Layer layer in Layers)
				layer.Dispose ();
		}
    }

    public enum StateTransition
    {
        IMMEDIATELY,
        FADE
    }
}
