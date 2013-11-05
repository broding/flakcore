using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Flakcore.Utils;

namespace Flakcore.Display.Activities
{
    public class AlphaTo : Activity
    {
        private float StartAlpha;
        private float TargetAlpha;
        private float Delta;

        public AlphaTo(Node node, int duration, float alpha)
            : base(node, duration)
        {
            this.StartAlpha = this.Node.Alpha;
            this.TargetAlpha = alpha;
            this.Delta = this.TargetAlpha - this.Node.Alpha;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Node.Alpha = Easing.Linear(this.Time, this.StartAlpha, this.Delta, this.Duration);
        }

        protected override void Done()
        {
            this.Node.Alpha = this.TargetAlpha;

            base.Done();
        }
    }
}