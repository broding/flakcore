using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Flakcore.Display
{
    public class Label : Sprite
    {
        public string Text;
        public SpriteFont SpriteFont;
        public HorizontalAlign HorizontalAlign;
        public VerticalAlign VerticalAlign;

        private Vector2 TextSize;

        public Label() : this("", Controller.Content.Load<SpriteFont>("DefaultFont"))
        { 
        }

        public Label(string text) : this(text, Controller.Content.Load<SpriteFont>("DefaultFont"))
        {
        }

        public Label(string text, SpriteFont spriteFont)
        {
            this.Text = text;
            this.SpriteFont = spriteFont;
            this.HorizontalAlign = HorizontalAlign.LEFT;
            this.VerticalAlign = VerticalAlign.TOP;

            this.TextSize = this.SpriteFont.MeasureString(text);
            this.Width = (int)this.TextSize.X;
            this.Height = (int)this.TextSize.Y;
        }


        public override void Draw(SpriteBatch spriteBatch, WorldProperties worldProperties)
        {
            if (this.Text == "")
                return;

            this.TextSize = this.SpriteFont.MeasureString(this.Text);

            worldProperties.Position.X *= this.ScrollFactor.X;
            worldProperties.Position.Y *= this.ScrollFactor.Y;
            worldProperties.Position.X += this.Width / 2 - this.TextSize.X / 2;
            worldProperties.Position.Y += this.Height / 2 - this.TextSize.Y / 2;

            spriteBatch.DrawString(
                this.SpriteFont,
                this.Text,
                worldProperties.Position,
                this.Color * worldProperties.Alpha,
                this.Rotation,
                this.Origin,
                this.Scale,
                this.SpriteEffects,
                Node.GetDrawDepth(this.WorldDepth));
        }
    }

    public enum HorizontalAlign
    {
        LEFT,
        CENTER,
        RIGHT
    }

    public enum VerticalAlign
    {
        TOP,
        MIDDLE,
        BOTTOM
    }
}
