using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Flakcore.Utils;

namespace Flakcore.Display.ParticleEngine.Modifiers
{
    public class LinearAlpha : IParticleModifier
    {
        public float FinalAlpha;

        private float Time;
        private Particle Target;

        public LinearAlpha()
        {
        }

        public LinearAlpha(float finalAlpha)
        {
            this.FinalAlpha = finalAlpha;
        }

        public void SetParticle(Particle particle)
        {
            this.Target = particle;
        }

        public void Apply()
        {
            this.Time = 0;
            this.Target.Alpha = this.Target.Emitter.Data.ReleaseAlpha;
        }

        public void Update(GameTime gameTime)
        {
            this.Time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            this.Target.Alpha = Easing.Linear(
                this.Time,
                this.Target.Emitter.Data.ReleaseAlpha,
                this.FinalAlpha - this.Target.Emitter.Data.ReleaseAlpha,
                this.Target.Emitter.Data.Lifetime
                );
        }

        public object Clone()
        {
            return new LinearAlpha(this.FinalAlpha);
        }
    }
}
