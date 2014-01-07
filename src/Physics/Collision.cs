using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Flakcore.Utils;
using Display.Tilemap;

namespace Flakcore.Physics
{
    class Collision
    {
        public Node Node1 { get; private set; }
        public Node Node2 { get; private set; }
        public Action<Node, Node> Callback { get; private set; }
        public Func<Node, Node, bool> Checker { get; private set; }

        public Collision(Node node1, Node node2, Action<Node, Node> callback, Func<Node, Node, bool> checker)
        {

            this.Node1 = node1;
            this.Node2 = node2;
            this.Callback = callback;
            this.Checker = checker;
        }

        public void resolve(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

			if(this.Checker != null)
				if(!this.Checker(this.Node1, this.Node2))
					return;

			Vector2 intersectionDepth = RectangleExtensions.GetIntersectionDepth (Node1.GetBoundingBox (), Node2.GetBoundingBox ());

			if (intersectionDepth.LengthSquared() != 0) 
			{
				if (Math.Abs(intersectionDepth.X) < Math.Abs(intersectionDepth.Y)) 
				{
					separateX (intersectionDepth.X);
				} 
				else 
				{
					separateY (intersectionDepth.Y);
				}
			}

            this.Node1.RoundPosition();
            this.Node2.RoundPosition();

            if(this.Callback != null)
                this.Callback(Node1, Node2);
        }
       
        private void separateY(float overlap)
        {
			//System.Console.WriteLine (overlap);
            if (!Node1.Immovable && !Node2.Immovable)
            {
				float oldVelocity = Node1.Velocity.Y;

				this.Node1.Position.Y += overlap;
				this.Node1.Velocity.Y = Node2.Velocity.Y / 20000;

				// divide by two, simulates mass??
				this.Node2.Position.Y -= overlap;
				this.Node2.Velocity.Y = oldVelocity / 100000;
            }
            else if (!Node1.Immovable)
            {
                this.Node1.Position.Y -= overlap;
				this.Node1.Velocity.Y = 0;
            }
            else if (!Node2.Immovable)
            {
				this.Node2.Position.Y -= overlap;
				this.Node2.Velocity.Y = 0;
            }
        }

        private void separateX(float overlap)
        {
            if (!Node1.Immovable && !Node2.Immovable)
            {
				float oldVelocity = Node1.Velocity.X;

				this.Node1.Position.X += overlap;
				this.Node1.Velocity.X = 0;

				// divide by two, simulates mass??
				this.Node2.Position.X -= overlap;
				this.Node2.Velocity.X = 0;
            }
            else if (!Node1.Immovable)
            {
                this.Node1.Position.X -= overlap;
				this.Node1.Velocity.X = 0;
            }
            else if (!Node2.Immovable)
            {
                this.Node2.Position.X -= overlap;
				this.Node2.Velocity.X = 0;
            }
        }
    }
}
