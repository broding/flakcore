using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Flakcore.Display.ParticleEngine.EmitterData
{
    public class BasicEmitterData
    {
        public string Name = "smoke1";
        public string TextureName;
        public int TotalParticles = 100;

        public int Lifetime = 500;

        public int ReleaseQuantity = 1;
        public int ReleaseSpeed = 10;
        public int ReleaseSpeedVariation = 0;

        public Vector2 ReleaseVelocity = Vector2.One * 10;
        public Vector2 ReleaseVelocityVariantion = Vector2.One * 20;

        public Color ReleaseColor = Color.White;
        public Color ReleaseColorVariation = Color.Black;

        public float ReleaseAlpha = 0.2f;
        public float ReleaseAlphaVariation = 0;

        public float ReleaseRotation = 0;
        public float ReleaseRotationVariation = 1;

        public Vector2 ReleaseScale = new Vector2(0.3f, 0.3f);
        public Vector2 ReleaseScaleVariation = new Vector2(0.05f, 0.05f);

        [ContentSerializer(Optional = true)]
        public object[] Modifiers;

        public Texture2D BaseTexture { get; private set; }

        public BasicEmitter SetupEmitter()
        {
            this.BaseTexture = Controller.Content.Load<Texture2D>(this.TextureName);
            return new BasicEmitter(this);
        }
    }
}
