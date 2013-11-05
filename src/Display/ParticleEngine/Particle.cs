using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Flakcore.Utils;

namespace Flakcore.Display.ParticleEngine
{
    public class Particle : Sprite
    {
        public BasicEmitter Emitter { get; private set; }

        private Action<Particle> KillCallBack;
        private int Lifetime;
        private LinkedList<IParticleModifier> Modifiers;

        private static Random random;

        public Particle(Action<Particle> killCallBack, BasicEmitter emitter) : base()
        {
            Particle.random = new Random();
            this.KillCallBack = killCallBack;
            this.Emitter = emitter;
            this.Lifetime = 0;
            this.Modifiers = new LinkedList<IParticleModifier>();
            this.Deactivate();
            this.Origin = new Vector2(this.Emitter.Data.BaseTexture.Width / 2, this.Emitter.Data.BaseTexture.Height / 2);
            this.OffScreenAction = OffScreenAction.NONE;
            this.SourceRectangle = new Rectangle(0, 0, this.Emitter.Data.BaseTexture.Width, this.Emitter.Data.BaseTexture.Height);

            this.InitializeModifiers();
        }

        private void InitializeModifiers()
        {
            foreach (IParticleModifier modifier in this.Emitter.Data.Modifiers)
            {
                IParticleModifier addedModifier = this.Modifiers.AddLast((IParticleModifier)modifier.Clone()).Value;
                addedModifier.SetParticle(this);
            }
        }

        private void InitializeEffect()
        {
            this.Lifetime = 0;
            this.Velocity = new Vector2(
                (this.Emitter.Data.ReleaseVelocity.X + Particle.GetVariantion(this.Emitter.Data.ReleaseVelocityVariantion.X) * Util.RandomPositiveNegative()) * (float)Math.Cos(random.NextDouble() * (Math.PI * 2)),
                (this.Emitter.Data.ReleaseVelocity.Y + Particle.GetVariantion(this.Emitter.Data.ReleaseVelocityVariantion.Y) * Util.RandomPositiveNegative()) * (float)Math.Sin(random.NextDouble() * (Math.PI * 2)));
            this.Scale = this.Emitter.Data.ReleaseScale + Particle.GetVector2Variantion(this.Emitter.Data.ReleaseScaleVariation) * Util.RandomPositiveNegative();
            //this.Color = Particle.GetColorVariation(this.Emitter.Data.ReleaseColor, this.Emitter.Data.ReleaseColorVariation);
            this.Rotation = this.Emitter.Data.ReleaseRotation + Particle.GetVariantion(this.Emitter.Data.ReleaseRotationVariation) * Util.RandomPositiveNegative();
            this.LoadTexture(this.Emitter.Data.BaseTexture);
            this.Alpha = this.Emitter.Data.ReleaseAlpha;

            foreach (IParticleModifier modifier in this.Modifiers)
            {
                modifier.Apply();
            }
        }

        public void Fire(Vector2 position)
        {
            this.Activate();
            this.Position = position;
            this.InitializeEffect();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!this.Active)
                return;

            this.Lifetime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this.Lifetime > this.Emitter.Data.Lifetime)
            {
                this.Lifetime = 0;
                this.KillCallBack(this);
                return;
            }

            foreach (IParticleModifier modifier in this.Modifiers)
                modifier.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, WorldProperties worldProperties)
        {
            worldProperties.Position.X *= this.ScrollFactor.X;
            worldProperties.Position.Y *= this.ScrollFactor.Y;

            spriteBatch.Draw(
                this.Emitter.Data.BaseTexture,
                this.Position,
                this.SourceRectangle,
                this.Color * worldProperties.Alpha,
                this.Rotation,
                this.Origin,
                this.Scale,
                this.SpriteEffects,
                Node.GetDrawDepth(this.Depth));
        }

        private static float GetVariantion(float variation)
        {
            return (float)random.NextDouble() * variation;
        }

        private static Vector2 GetVector2Variantion(Vector2 variation)
        {
            return new Vector2(
                (float)random.NextDouble() * variation.X,
                (float)random.NextDouble() * variation.Y
            );
        }

        private static Color GetColorVariation(Color normal, Color variation)
        {
            Vector4 normalVector = normal.ToVector4();
            Vector4 variationVector = variation.ToVector4();

            return new Color(
                normalVector.W + variationVector.W * Util.RandomPositiveNegative(),
                normalVector.X + variationVector.X * Util.RandomPositiveNegative(),
                normalVector.Y + variationVector.Y * Util.RandomPositiveNegative(),
                normalVector.Z + variationVector.Y * Util.RandomPositiveNegative()
                );
        }
    }
}
