using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Display.Tilemap;

namespace Flakcore.Physics
{
    public class CollisionSolver
    {
        public static Tilemap Tilemap;

		private QuadTree _quadTree; 
		private List<Collision> _collisions;

        public CollisionSolver()
        {
			_quadTree = new QuadTree (0, new Flakcore.Utils.BoundingRectangle(0,0,Director.ScreenSize.X, Director.ScreenSize.Y));
			_collisions = new List<Collision> ();
        }

		public void Reset()
		{
			_quadTree.clear();
			_collisions.Clear();
		}

		public void Resolve(GameTime gameTime)
		{
			List<Collision> contacts = new List<Collision> (_collisions.Count);

			foreach(Collision collision in _collisions)
			{
				// TODO fix quadtree!!
				if (_quadTree.isColliding (collision.Node1, collision.Node2) || true) 
				{
					if (collision.intersectionTest())
						contacts.Add (collision);
				}
			}

			contacts = contacts.OrderByDescending (o => o.Penetration).ToList();

			foreach(Collision collision in contacts)
			{
				collision.resolve (gameTime);
			}
		}

		public void AddCollision(Node node1, Node node2, Action<Node, Node> callback, Func<Node, Node, bool> checker)
		{
			_quadTree.insert (node1);
			_quadTree.insert (node2);

			if (!isAlreadyInCollisionList(node1, node2) && node1 != node2)
				_collisions.Add(new Collision(node1, node2, callback, checker));
		}

        private bool isAlreadyInCollisionList(Node node1, Node node2)
        {
            foreach(Collision collision in _collisions)
            {
                if ((collision.Node2 == node1 && collision.Node1 == node2) || (collision.Node1 == node1 && collision.Node2 == node2))
                        return true;
            }

            return false;
        }
    }
}
