using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Flakcore.Display.ParticleEngine.EmitterData;

namespace Flakcore.Display.ParticleEngine
{
    public class BasicEmitter : Node
    {
        public BasicEmitterData Data { get; private set; }

        private List<Particle> Particles;
        private List<Particle> DeadParticles;
        private int ReleaseTimer;
        private bool Started;

        public BasicEmitter(BasicEmitterData data)
        {
            this.Collidable = false;
            this.Particles = new List<Particle>();
            this.DeadParticles = new List<Particle>();
            this.Data = data;
            this.InitParticles();
            this.Started = false;
        }

        private void InitParticles()
        {
            int amount = this.Data.TotalParticles;

            for (int i = 0; i < amount; i++)
            {
                Particle particle = new Particle(this.KillParticle, this);
                this.DeadParticles.Add(particle);
                this.AddChild(particle);
            }
        }

        public override void DrawCall(SpriteBatch spriteBatch, WorldProperties worldProperties)
        {
            // we need to pass a zero vector to the draw call, because the particle engine always is at 0,0
            worldProperties.Position = Vector2.Zero;
            base.DrawCall(spriteBatch, worldProperties);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!this.Started)
                return;

            this.ReleaseTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this.ReleaseTimer > this.Data.ReleaseSpeed)
            {
                this.ReleaseTimer = 0;
                this.Emit(this.Data.ReleaseQuantity);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            base.PostUpdate(gameTime);

            if (!this.Started)
                return;
        }

        private void Emit(int quanitity)
        {
            if (this.DeadParticles.Count < quanitity)
                quanitity = this.DeadParticles.Count;

            for (int i = 0; i < quanitity; i++)
            {
                Particle particle = this.DeadParticles[0];
                particle.Fire(this.Position);
                this.Particles.Add(particle);
                this.DeadParticles.RemoveAt(0);
            }
        }

        internal void KillParticle(Particle particle)
        {
            this.Particles.Remove(particle);
            particle.Deactivate();
            this.DeadParticles.Add(particle);
        }

        internal void Start()
        {
            this.Started = true;
        }
    
        internal void Stop()
        {
            this.Started = false;
        }

        internal void Explode()
        {
            this.Emit(this.Data.ReleaseQuantity);
        }
    }
}
