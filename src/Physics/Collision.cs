using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Flakcore.Utils;
using Display.Tilemap;
using FlakCore.Physics;
using FlakCore;

namespace Flakcore.Physics
{
    class Collision
    {
        public Node Node1 { get; private set; }
        public Node Node2 { get; private set; }
        public Action<Node, Node> Callback { get; private set; }
        public Func<Node, Node, bool> Checker { get; private set; }

		public float Penetration { get; private set; }
		private Vector2 _normal;

        public Collision(Node node1, Node node2, Action<Node, Node> callback, Func<Node, Node, bool> checker)
        {
            this.Node1 = node1;
            this.Node2 = node2;
            this.Callback = callback;
            this.Checker = checker;
        }

		/**
		 * 	Return true if there is a collision
		 */
		public bool intersectionTest()
		{
			if(this.Checker != null)
				if(!this.Checker(this.Node1, this.Node2))
					return false;

			Vector2 intersectionDepth = RectangleExtensions.GetIntersectionDepth (Node1.GetBoundingBox (), Node2.GetBoundingBox ());

			if (intersectionDepth.LengthSquared () != 0) 
			{
				return boxAndBoxTest ();
			} 
			else 
			{
				return false;
			}
		}

        public void resolve(GameTime gameTime)
        {
			if(this.Callback != null)
				this.Callback(Node1, Node2);

			//System.Console.WriteLine (_penetration);

			/*
			if(entity1Collision->trigger && entity2Collision->trigger)
				return;

			if(entity2Collision->solid && entity1Collision->solid)
				return;
			*/

			// TODO add triggers, check if both nodes are triggers, if not, do code here

			Vector2 delta = Node1.Position - Node2.Position;

			if(Vector2Utils.DotProduct(_normal, delta) > 0)
				_normal *= -1.0f;

			if(!Node1.Immovable && Node2.Immovable)
				Node1.Position += _normal * (Penetration);

			if(!Node2.Immovable && Node1.Immovable)
				Node2.Position += _normal * (Penetration);

			if(!Node1.Immovable && !Node2.Immovable)
			{
				Node1.Position -= _normal * (Penetration / 2);
				Node2.Position += _normal * (Penetration / 2);
			}

			float x1 = Vector2Utils.DotProduct(_normal, Node1.Velocity);
			Vector2 v1x = _normal * x1;
			Vector2 v1y = Node1.Velocity - v1x;

			float x2 = Vector2Utils.DotProduct(_normal, Node2.Velocity);
			Vector2 v2x = _normal * x2;
			Vector2 v2y = Node2.Velocity - v2x;

			// 1 is mass
			float node1mass = 1;
			float node2mass = 1;

			float massFormula1 = (node1mass - node2mass) / (node1mass + node2mass);
			float massFormula2 = (node2mass - node1mass) / (node2mass + node1mass);

			if(!Node1.Immovable)
				Node1.Velocity = v1x * massFormula1 + v2x * massFormula2 + v1y;

			if(!Node2.Immovable)
				Node2.Velocity = v1x * massFormula2 + v2x * massFormula1 + v2y;
        }

		private bool boxAndBoxTest()
		{
			ConvexShape shape1 = Node1.TransformedConvexShape;
			ConvexShape shape2 = Node2.TransformedConvexShape;

			List<Vector2> axis1 = shape1.axis;
			List<Vector2> axis2 = shape2.axis;

			float penetration = Int16.MaxValue;
			Vector2 normal = Vector2.Zero;

			for(int i = 0; i < shape1.Points.Count; i++)
			{
				Vector2 axis = axis1[i];

				Projection projection1 = projectOnAxis(shape1, axis);
				Projection projection2 = projectOnAxis(shape2, axis);

				if(!projection1.IsOverlapping(projection2))
					return false;
				else
				{
					float newPenetration = projection1.GetOverlap(projection2);
					if(newPenetration < penetration)
					{
						penetration = newPenetration;
						normal = axis;
					}
				}
			}

			for(int i = 0; i < shape2.Points.Count; i++)
			{
				Vector2 axis = axis2[i];

				Projection projection1 = projectOnAxis(shape1, axis);
				Projection projection2 = projectOnAxis(shape2, axis);

				if(!projection1.IsOverlapping(projection2))
					return false;
				else
				{
					float newPenetration = projection1.GetOverlap(projection2);
					if(newPenetration < penetration)
					{
						penetration = newPenetration;
						normal = axis;
					}
				}
			}

			Penetration = penetration;
			_normal = normal;

			return true;
		}

		private Projection projectOnAxis(ConvexShape shape, Vector2 axis)
		{
			float min = Vector2Utils.DotProduct(axis, shape.Points[0]);
			float max = min;

			for(int i = 1; i < shape.Points.Count; i++)
			{
				float p = Vector2Utils.DotProduct(axis, shape.Points[i]);

				if(p < min)
					min = p;
				else if(p > max)
					max = p;
			}

			return new Projection(min, max);
		}
    }
}
