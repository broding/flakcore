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
        public string Name { get; set; }
        public RenderTarget2D RenderTarget { get; protected set; }
		public Effect Effect { get; set; }

        public Layer()
            : base()
        {
            this.RenderTarget = new RenderTarget2D(
                Director.Graphics.GraphicsDevice,
                (int)Director.ScreenSize.X,
                (int)Director.ScreenSize.Y,
                false,
                SurfaceFormat.Vector4,
                DepthFormat.None,
                1,
                RenderTargetUsage.DiscardContents
                );

        }
    }
}
