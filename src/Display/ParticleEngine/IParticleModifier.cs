using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Flakcore.Display.ParticleEngine
{
    public interface IParticleModifier : ICloneable
    {
        void SetParticle(Particle particle);
        void Update(GameTime gameTime);
        void Apply();
    }
}
