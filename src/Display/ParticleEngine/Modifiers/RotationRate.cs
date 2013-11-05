using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Flakcore.Display.ParticleEngine.Modifiers
{
    public class RotationRate : IParticleModifier
    {
        private Particle Target;
        private float RotationPerSecond;

        public RotationRate(float rotationPerSecond)
        {
            this.RotationPerSecond = rotationPerSecond;
        }

        public void SetParticle(Particle particle)
        {
            this.Target = particle;
        }

        public void Apply()
        {
        }

        public void Update(GameTime gameTime)
        {
            this.Target.Rotation += RotationPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public object Clone()
        {
            return new RotationRate(this.RotationPerSecond);
        }
    }
}
