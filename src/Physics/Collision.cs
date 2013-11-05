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

        private static Node DirtyNode1;
        private static Node DirtyNode2;

        private Vector2 node1VelocityDiff;
        private Vector2 node2VelocityDiff;

        private Vector2 node1PositionDiff;
        private Vector2 node2PositionDiff;

        public Collision(Node node1, Node node2, Action<Node, Node> callback, Func<Node, Node, bool> checker)
        {
            if (DirtyNode1 == null || DirtyNode2 == null)
            {
                DirtyNode1 = new Node();
                DirtyNode2 = new Node();
            }

            this.Node1 = node1;
            this.Node2 = node2;
            this.Callback = callback;
            this.Checker = checker;

            this.node1PositionDiff = Vector2.Zero;
            this.node2PositionDiff = Vector2.Zero;

            this.node1VelocityDiff = Vector2.Zero;
            this.node2VelocityDiff = Vector2.Zero;
        }

        public void resolve(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.IsCollision(deltaTime))
            {
                if(this.Checker != null)
                    if(!this.Checker(this.Node1, this.Node2))
                        return;

                this.Node1.Clone(DirtyNode1);
                this.Node2.Clone(DirtyNode2);

                DirtyNode1.Position.X += DirtyNode1.Velocity.X * deltaTime;
                DirtyNode2.Position.X += DirtyNode2.Velocity.X * deltaTime;

                DirtyNode1.RoundPosition();
                DirtyNode2.RoundPosition();

                float intersectionDepth = RectangleExtensions.GetIntersectionDepth(DirtyNode1.GetBoundingBox(), DirtyNode2.GetBoundingBox()).X;
                if (intersectionDepth != 0)
                    overlapX(intersectionDepth);

                this.Node1.Clone(DirtyNode1);
                this.Node2.Clone(DirtyNode2);

                DirtyNode1.Position.Y += DirtyNode1.Velocity.Y * deltaTime;
                DirtyNode2.Position.Y += DirtyNode2.Velocity.Y * deltaTime;

                DirtyNode1.RoundPosition();
                DirtyNode2.RoundPosition();

                BoundingRectangle box1 = DirtyNode1.GetBoundingBox();
                BoundingRectangle box2 = DirtyNode2.GetBoundingBox();
                intersectionDepth = RectangleExtensions.GetIntersectionDepth(DirtyNode1.GetBoundingBox(), DirtyNode2.GetBoundingBox()).Y;
                if (intersectionDepth != 0)
                    overlapY(intersectionDepth);

                this.CorrectNodes(deltaTime);

                this.Node1.RoundPosition();
                this.Node2.RoundPosition();

                if(this.Callback != null)
                    this.Callback(Node1, Node2);
            }
        }

        private bool IsCollision(float deltaTime)
        {
            this.Node1.Clone(DirtyNode1);
            this.Node2.Clone(DirtyNode2);

            DirtyNode1.Position += DirtyNode1.Velocity * deltaTime;
            DirtyNode2.Position += DirtyNode2.Velocity * deltaTime;

            DirtyNode1.RoundPosition();
            DirtyNode2.RoundPosition();

            BoundingRectangle box1 = DirtyNode1.GetBoundingBox();
            BoundingRectangle box2 = DirtyNode2.GetBoundingBox();

            return DirtyNode1.GetBoundingBox().Intersects(DirtyNode2.GetBoundingBox());
        }

        private void CorrectNodes(float deltaTime)
        {
            if (node1PositionDiff.X != 0)
            {
                this.Node1.Position.X += this.Node1.Velocity.X * deltaTime;
                this.Node1.Position.X += node1PositionDiff.X;
                this.Node1.Velocity.X += node1VelocityDiff.X;
            }

            if (node1PositionDiff.Y != 0)
            {
                this.Node1.Position.Y += this.Node1.Velocity.Y * deltaTime;
                this.Node1.Position.Y += node1PositionDiff.Y;
                this.Node1.Velocity.Y += node1VelocityDiff.Y;
            }

            if (node2PositionDiff.X != 0)
            {
                this.Node2.Position.X += this.Node2.Velocity.X * deltaTime;
                this.Node2.Position.X += node2PositionDiff.X;
                this.Node2.Velocity.X += node2VelocityDiff.X;
            }

            if (node2PositionDiff.Y != 0)
            {
                this.Node2.Position.Y += this.Node2.Velocity.Y * deltaTime;
                this.Node2.Position.Y += node2PositionDiff.Y;
                this.Node2.Velocity.Y += node2VelocityDiff.Y;
            }
        }

        private void overlapX(float overlap)
        {
            if (overlap == 0)
                return;

            if (Node1.Velocity.X != Node2.Velocity.X)
            {
                if (Node1.Velocity.X > Node2.Velocity.X && Node1.CollidableSides.Right && Node2.CollidableSides.Left)
                {
                    Node1.Touching.Right = true;
                    Node2.Touching.Left = true;

                    separateX(overlap);
                }
                else if (Node1.Velocity.X < Node2.Velocity.X && Node1.CollidableSides.Left && Node2.CollidableSides.Right)
                {
                    Node1.Touching.Left = true;
                    Node2.Touching.Right = true;

                    separateX(overlap);
                }
            }
        }

        private void overlapY(float overlap)
        {
            if (overlap == 0)
                return;

            if (Node1.Velocity.Y != Node2.Velocity.Y)
            {
                if (Node1.Velocity.Y > Node2.Velocity.Y &&  Node1.CollidableSides.Bottom && Node2.CollidableSides.Top)
                {
                    Node1.Touching.Bottom = true;
                    Node2.Touching.Top = true;

                    separateY(overlap);
                }
                else if (Node1.Velocity.Y < Node2.Velocity.Y && Node1.CollidableSides.Top && Node2.CollidableSides.Bottom)
                {
                    Node1.Touching.Top = true;
                    Node2.Touching.Bottom = true;

                    separateY(overlap);
                }
            }
        }

        private void separateY(float overlap)
        {
            if (!Node1.Immovable && !Node2.Immovable)
            {
            }
            else if (!Node1.Immovable)
            {
                this.node1PositionDiff.Y += overlap;
                this.node1VelocityDiff.Y -= Node1.Velocity.Y;
            }
            else if (!Node2.Immovable)
            {
                this.node2PositionDiff.Y += overlap;
                this.node2VelocityDiff.Y -= Node2.Velocity.Y;
            }
        }

        private void separateX(float overlap)
        {
            if (!Node1.Immovable && !Node2.Immovable)
            {
            }
            else if (!Node1.Immovable)
            {
                this.node1PositionDiff.X += overlap;
                this.node1VelocityDiff.X -= Node1.Velocity.X;
            }
            else if (!Node2.Immovable)
            {
                this.node2PositionDiff.X += overlap;
                this.node2VelocityDiff.X -= Node2.Velocity.X;
            }
        }
    }
}
