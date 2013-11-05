using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Flakcore.Utils;

namespace Flakcore.Display.Activities
{
    public class MoveTo : Activity
    {
        private Vector2 StartPosition;
        private Vector2 TargetPosition;
        private Vector2 Delta;

        public MoveTo(Node node, int duration, Vector2 position)
            : base(node, duration)
        {
            this.StartPosition = this.Node.Position;
            this.TargetPosition = position;
            this.Delta = this.TargetPosition - this.Node.Position;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Node.Position.X = Easing.Linear(this.Time, this.StartPosition.X, this.Delta.X, this.Duration);
            this.Node.Position.Y = Easing.Linear(this.Time, this.StartPosition.Y, this.Delta.Y, this.Duration);
        }

        protected override void Done()
        {
            this.Node.Position = this.TargetPosition;

            base.Done();
        }
    }
}