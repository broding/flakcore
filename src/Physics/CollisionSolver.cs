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

        private List<Collision> Collisions;
        private QuadTree QuadTree;
		private QuadTree _quadTree; 

        public CollisionSolver(QuadTree quadTree)
        {
            this.Collisions = new List<Collision>();
            this.QuadTree = quadTree;
			_quadTree = new QuadTree (0, Director.ScreenSize);
        }

		public void Reset()
		{
			_quadTree.clear ();
		}

		public void addCollision(Node node1, Node node2, Action<Node, Node> callback, Func<Node, Node, bool> checker)
		{
			QuadTree.insert (node1);
			QuadTree.insert (node2);
		}

        public void addCollision(Node node, string collideGroup, Action<Node, Node> callback, Func<Node, Node, bool> checker)
        {
            List<Node> collidedNodes = new List<Node>();

			QuadTree.retrieve(collidedNodes, node);

            foreach (Node collideNode in collidedNodes)
            {
                if (collideNode.IsMemberOfCollisionGroup(collideGroup))
                    this.addCollision(node, collideNode, callback, checker);
            }   
        }

        private void GetCollidedTiles(Node node, List<Node> collidedNodes)
        {
            CollisionSolver.Tilemap.GetCollidedTiles(node, collidedNodes);
        }

        public void resolveCollisions(GameTime gameTime)
        {
            foreach (Collision collision in Collisions)
            {
                collision.resolve(gameTime);
            }

            Collisions.Clear();
        }

        private void addCollision(Node node1, Node node2, Action<Node, Node> callback, Func<Node, Node, bool> checker)
        {
            if (!isAlreadyInCollisionList(node1, node2) && node1 != node2)
                Collisions.Add(new Collision(node1, node2, callback, checker));
        }

        private bool isAlreadyInCollisionList(Node node1, Node node2)
        {
            foreach(Collision collision in Collisions)
            {
                if ((collision.Node2 == node1 && collision.Node1 == node2) || (collision.Node1 == node1 && collision.Node2 == node2))
                        return true;
            }

            return false;
        }
    }
}
