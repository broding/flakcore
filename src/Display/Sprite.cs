using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Flakcore.Utils;
using FlakCore.Physics;

namespace Flakcore.Display
{
    public class Sprite : Node
    {
        public Texture2D Texture { get; protected set; }
		public Texture2D BumpTexture { get; protected set; }
        public Facing Facing;
        public Color Color;
        public Rectangle SourceRectangle;
        public SpriteEffects SpriteEffects;

        private List<Animation> animations;
        private bool animating;
        private double animationTimer;
        private int currentFrame;
        private Animation currentAnimation;

        public Sprite() : base()
        {
            animations = new List<Animation>();
            Facing = Facing.Right;
            Color = Color.White;
            SpriteEffects = new SpriteEffects();
        }

        public void LoadTexture(string assetName)
        {
            this.LoadTexture(Director.Content.Load<Texture2D>(assetName));
        }

        public void LoadTexture(Texture2D texture)
        {
            this.LoadTexture(texture, texture.Width, texture.Height);
        }

		public void LoadBumpTexture(Texture2D texture)
		{
			this.BumpTexture = texture;
		}

        public virtual void LoadTexture(Texture2D texture, int width, int height)
        {
            this.Texture = texture;
            this.Width = width;
            this.Height = height;
            this.animating = false;
            this.SourceRectangle = new Rectangle(0, 0, width, height);

			this.ConvexShape = new ConvexShape();
			this.ConvexShape.Points.Add (new Vector2 (0, 0));
			this.ConvexShape.Points.Add (new Vector2 (width, 0));
			this.ConvexShape.Points.Add (new Vector2 (width, height));
			this.ConvexShape.Points.Add (new Vector2 (0, height));
        }

        public void AddAnimation(string name, int[] frames, float frameRate)
        {
            animations.Add(new Animation(name, frames, frameRate));
        }

        public void PlayAnimation(string name)
        {
            // check if the wanted animated is already running
            if (currentAnimation.name == name)
                return;

            foreach (Animation animation in animations)
            {
                if (animation.name == name)
                {
                    currentAnimation = animation;
                    animating = true;
                    animationTimer = 0;
                    currentFrame = 0;

                    return;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // if animating, then update all animation stuff
            if (animating)
            {
                animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > currentAnimation.frameRate)
                {
                    if (currentFrame == currentAnimation.frames.Length-1)
                        currentFrame = 0;
                    else
                        currentFrame++;

                    animationTimer = 0;
                }
            }
        }

        public override void DrawCall(SpriteBatch spriteBatch, DrawProperties worldProperties)
        {
			worldProperties.Position += this.Position;
            worldProperties.Alpha = Math.Min(this.Alpha, worldProperties.Alpha);

            if (!Visable)
                return;

            this.SpriteEffects = SpriteEffects.None;

            if(Facing == Facing.Left)
                this.SpriteEffects = SpriteEffects.FlipHorizontally;

            this.Draw(spriteBatch, worldProperties);

			worldProperties.Position -= this.Position;

            base.DrawCall(spriteBatch, worldProperties);

        }

        public virtual void Draw(SpriteBatch spriteBatch, DrawProperties worldProperties)
        {
            if (this.Texture == null)
                return;

            if (animating)
                spriteBatch.Draw(Texture,
                    worldProperties.Position,
                    new Rectangle(currentAnimation.frames[currentFrame] * Width, 0, Width, Height),
                    this.Color * worldProperties.Alpha,
                    this.Rotation,
                    this.Origin,
                    this.Scale,
                    this.SpriteEffects,
                    Node.GetDrawDepth(worldProperties.Depth));
            else
                spriteBatch.Draw(Texture,
                    worldProperties.Position,
                    this.SourceRectangle,
                    this.Color * worldProperties.Alpha,
                    this.Rotation,
                    this.Origin,
                    this.Scale,
                    this.SpriteEffects,
                    Node.GetDrawDepth(worldProperties.Depth));
        }

        public static Sprite CreateRectangle(Vector2 size, Color color)
        {
            Texture2D rectangle = new Texture2D(Director.Graphics.GraphicsDevice, 1, 1);
            rectangle.SetData(new[] { color });

            Sprite sprite = new Sprite();
            sprite.LoadTexture(rectangle);
            sprite.Width = (int)size.X;
            sprite.Height = (int)size.Y;
            sprite.SourceRectangle.Width = (int)size.X;
            sprite.SourceRectangle.Height = (int)size.Y;
			sprite.ConvexShape = new ConvexShape();
			sprite.ConvexShape.Points.Add (new Vector2 (0, 0));
			sprite.ConvexShape.Points.Add (new Vector2 (size.X, 0));
			sprite.ConvexShape.Points.Add (new Vector2 (size.X, size.Y));
			sprite.ConvexShape.Points.Add (new Vector2 (0, size.Y));

            return sprite;
        }
    }

    public enum Facing
    {
        Left,
        Right
    }
}
