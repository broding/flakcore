using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Flakcore.Display.Level.LevelObjects
{
    class Ladder : LevelObject
    {
        private int LadderHeight;

        public Ladder(int height) : base()
        {
            this.LadderHeight = height;

            this.LoadTexture("ladder");
            this.AddCollisionGroup("ladder");
            this.Height = this.LadderHeight * Level.BLOCK_HEIGHT;
            this.Immovable = true;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, WorldProperties worldProperties)
        {
            for (int y = 0; y < this.LadderHeight; y++)
            {
                base.Draw(spriteBatch, worldProperties);
                worldProperties.Position.Y += this.Texture.Height;
            }
        }
    }
}
