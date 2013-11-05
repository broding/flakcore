using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Flakcore.Utils;

namespace Flakcore.Display.Activities
{
    public class ScaleTo : Activity
    {
        private Vector2 StartScale;
        private Vector2 TargetScale;
        private Vector2 Delta;

        public ScaleTo(Node node, int duration, Vector2 scale)
            : base(node, duration)
        {
            this.StartScale = this.Node.Scale;
            this.TargetScale = scale;
            this.Delta = this.TargetScale - this.Node.Scale;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Node.Scale.X = Easing.Linear(this.Time, this.StartScale.X, this.Delta.X, this.Duration);
            this.Node.Scale.Y = Easing.Linear(this.Time, this.StartScale.Y, this.Delta.Y, this.Duration);
        }

        protected override void Done()
        {
            this.Node.Scale = this.TargetScale;

            base.Done();
        }
    }
}