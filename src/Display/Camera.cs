using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Flakcore.Utils;

namespace Flakcore.Display
{
    public class Camera
    {
        public Viewport Viewport { get; private set; }
		public Vector2 Position;
		public Vector2 Origin;
		public float Rotation;
		public float Zoom;

        public Rectangle BoundingBox;

        public Node followNode { get; set; }

        public Camera(int x, int y, int width, int height)
        {
            Position = Vector2.Zero;
			Zoom = 1;
            Rotation = 0;
            Viewport = new Viewport(x, y, width, height);
			Origin = new Vector2 (Viewport.Width / 2, Viewport.Height / 2);
            BoundingBox = this.Viewport.Bounds;
        }

        public void resetViewport(int x, int y, int width, int height)
        {
            Viewport = new Viewport(x, y, width, height);
        }

        public Matrix GetTransformMatrix()
		{
			return GetTransformMatrix (Vector2.One);
		}

		public Matrix GetTransformMatrix(Vector2 scrollFactor)
		{
			return Matrix.CreateTranslation(new Vector3(-Position * scrollFactor, 0.0f)) *
				// The next line has a catch. See note below.
				Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
					Matrix.CreateRotationZ(Rotation) *
					Matrix.CreateScale(Zoom, Zoom, 1) *
					Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
		}

        public Vector2 TransformPosition(Vector2 position)
        {
            return position - new Vector2(this.Position.X - Director.ScreenSize.X / 2, this.Position.Y - Director.ScreenSize.Y / 2); ;
        }

        public void update(GameTime gameTime)
        {
            if (followNode != null)
            {
                Position.Y = followNode.Position.Y + followNode.Height / 2 - Origin.Y;
                Position.X = followNode.Position.X + followNode.Width / 2 - Origin.X;
            }

            this.BoundingBox.X = (int)Position.X - (int)Director.ScreenSize.X / 2;
            this.BoundingBox.Y = (int)Position.Y - (int)Director.ScreenSize.Y / 2;
        }
    }
}
