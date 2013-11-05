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
        public Action<Layer, GameTime> PostEffectAction;

        public Layer()
            : base()
        {
            this.RenderTarget = new RenderTarget2D(
                Controller.Graphics.GraphicsDevice,
                (int)Controller.ScreenSize.X,
                (int)Controller.ScreenSize.Y,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                1,
                RenderTargetUsage.DiscardContents
                );

        }
    }
}
