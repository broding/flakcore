using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Flakcore.Display
{
    public class Layer : Node
	{
		public Vector2 ScrollFactor;
        public RenderTarget2D RenderTarget { get; protected set; }
		public Effect Effect { get; set; }

        public Layer()
            : base()
        {
			ScrollFactor = Vector2.One;

			this.RenderTarget = new RenderTarget2D (
				Director.Graphics.GraphicsDevice,
				(int)Director.ScreenSize.X,
				(int)Director.ScreenSize.Y);

        }
    }
}
