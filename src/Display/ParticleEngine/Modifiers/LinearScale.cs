using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Flakcore.Utils;

namespace Flakcore.Display.ParticleEngine.Modifiers
{
    public class LinearScale : IParticleModifier
    {
        public Vector2 FinalScale;

        private Particle Target;
        private float Time;

        public LinearScale()
        {
        }

        public LinearScale(Vector2 finalScale)
        {
            this.FinalScale = finalScale;
        }

        public void SetParticle(Particle particle)
        {
            this.Target = particle;
        }

        public void Apply()
        {
            this.Time = 0;
            this.Target.Scale = this.Target.Emitter.Data.ReleaseScale;
        }

        public void Update(GameTime gameTime)
        {
            this.Time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            this.Target.Scale.X = Easing.Linear(
                this.Time,
                this.Target.Emitter.Data.ReleaseScale.X,
                this.FinalScale.X - this.Target.Emitter.Data.ReleaseScale.X,
                this.Target.Emitter.Data.Lifetime
                );

            this.Target.Scale.Y = Easing.Linear(
                this.Time,
                this.Target.Emitter.Data.ReleaseScale.Y,
                this.FinalScale.Y - this.Target.Emitter.Data.ReleaseScale.Y,
                this.Target.Emitter.Data.Lifetime
                );
        }

        public object Clone()
        {
            return new LinearScale(this.FinalScale);
        }
    }
}
