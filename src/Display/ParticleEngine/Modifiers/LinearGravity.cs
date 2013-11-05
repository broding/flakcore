using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Flakcore.Display.ParticleEngine.Modifiers
{
    public class LinearGravity : IParticleModifier
    {
        public Vector2 Gravity;

        private Particle Target;

        public LinearGravity()
        {

        }

        public LinearGravity(Vector2 gravity)
        {
            this.Gravity = gravity;
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
            this.Target.Velocity += this.Gravity;
        }

        public object Clone()
        {
            return new LinearGravity(this.Gravity);
        }
    }
}
