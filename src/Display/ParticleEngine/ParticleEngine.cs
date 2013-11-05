using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Flakcore.Display.ParticleEngine.EmitterData;

namespace Flakcore.Display.ParticleEngine
{
    public class ParticleEngine : Node
    {
        private ParticleEffect Effect;
        private BasicEmitter[] Emitters;
        private bool Started;
        private float EmitTime;
        private float EmitTimer;

        public ParticleEngine(ParticleEffect Effect)
        {
            this.Effect = Effect;
            this.SetupEmitters();
            this.Started = false;
        }

        private void SetupEmitters()
        {
            // For each emitterData in our effect, get the Emitter and add it to our variable
            this.Emitters = new BasicEmitter[this.Effect.EmitterData.Length];

            for (int i = 0; i < this.Effect.EmitterData.Length; i++)
            {
                this.Emitters[i] = this.Effect.EmitterData[i].SetupEmitter();
                this.AddChild(this.Emitters[i]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!this.Started)
                return;

            this.EmitTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this.EmitTime != 0 && this.EmitTime < this.EmitTimer)
            {
                this.EmitTimer = 0;
                this.Stop();
            }

            this.UpdateEmitterPositions();
        }

        public void Start()
        {
            this.Started = true;
            this.UpdateEmitterPositions();
            this.StartEmitters();
        }

        public void Start(float time)
        {
            this.EmitTime = time;
            this.Started = true;
            this.UpdateEmitterPositions();
            this.StartEmitters();
        }

        public void Stop()
        {
            this.Started = false;
            this.StopEmitters();
        }

        public void Explode()
        {
            this.UpdateEmitterPositions();
            this.ExplodeEmitters();
        }

        private void StartEmitters()
        {
            for (int i = 0; i < this.Emitters.Length; i++)
                this.Emitters[i].Start();
        }

        private void StopEmitters()
        {
            for (int i = 0; i < this.Emitters.Length; i++)
                this.Emitters[i].Stop();
        }

        private void ExplodeEmitters()
        {
            for (int i = 0; i < this.Emitters.Length; i++)
                this.Emitters[i].Explode();
        }

        private void UpdateEmitterPositions()
        {
            for (int i = 0; i < this.Emitters.Length; i++)
                this.Emitters[i].Position = this.Position;
                
        }
    }

}
