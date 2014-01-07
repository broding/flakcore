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
		public float Rotation;
		public float Zoom;

        public Rectangle BoundingBox;

        private Matrix transformMatrix;

        public Node followNode { get; set; }

        public Camera(int x, int y, int width, int height)
        {
            Position = Vector2.Zero;
			Zoom = 1;
            Rotation = 0;
            Viewport = new Viewport(x, y, width, height);
            this.BoundingBox = this.Viewport.Bounds;
        }

        public void resetViewport(int x, int y, int width, int height)
        {
            Viewport = new Viewport(x, y, width, height);
        }

        public Matrix GetTransformMatrix()
        {
            transformMatrix =
               Matrix.CreateTranslation(new Vector3((int)-Position.X, (int)-Position.Y, 0)) *
               Matrix.CreateRotationZ(Rotation / 1000) *
               Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
               Matrix.CreateTranslation(new Vector3(Viewport.Width * 0.5f,
                   Viewport.Height * 0.5f, 0));

            return transformMatrix;
        }

        public Vector2 TransformPosition(Vector2 position)
        {
            return position - new Vector2(this.Position.X - Director.ScreenSize.X / 2, this.Position.Y - Director.ScreenSize.Y / 2); ;
        }

        public void update(GameTime gameTime)
        {
            if (followNode != null)
            {
                Position.Y = followNode.Position.Y + followNode.Height / 2;
                Position.X = followNode.Position.X + followNode.Width / 2;
            }

            Position.X = Math.Max(Director.ScreenSize.X / this.Zoom / 2, Position.X);
            Position.Y = Math.Max(Director.ScreenSize.Y / this.Zoom / 2, Position.Y);

            this.BoundingBox.X = (int)Position.X - (int)Director.ScreenSize.X / 2;
            this.BoundingBox.Y = (int)Position.Y - (int)Director.ScreenSize.Y / 2;
        }
    }
}
