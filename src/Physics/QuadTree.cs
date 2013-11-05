using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Flakcore.Display;
using Flakcore.Utils;

namespace Flakcore.Physics
{
    public class QuadTree
    {
        public const int MAX_OBJECTS = 4;
        public const int MAX_LEVELS  = 35;

        private int level;
        private List<Node> objects;
        private BoundingRectangle bounds;
        private QuadTree[] nodes;

        public QuadTree(int level, BoundingRectangle bounds)
        {
            this.level = level;
            objects = new List<Node>();
            this.bounds = bounds;
            this.nodes = new QuadTree[4];
        }

        public void clear()
        {
            objects.Clear();
            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null)
                {
                    nodes[i].clear();
                    nodes[i] = null;
                }
            }
        }

        private void split()
        {
            float subWidth = bounds.Width / 2;
            float subHeight = bounds.Height / 2;

            nodes[0] = new QuadTree(level + 1, new BoundingRectangle(bounds.X, bounds.Y, subWidth, subHeight));
            nodes[1] = new QuadTree(level + 1, new BoundingRectangle(bounds.X + subWidth, bounds.Y, subWidth, subHeight));
            nodes[2] = new QuadTree(level + 1, new BoundingRectangle(bounds.X + subWidth, bounds.Y + subHeight, subWidth, subHeight));
            nodes[3] = new QuadTree(level + 1, new BoundingRectangle(bounds.X, bounds.Y + subHeight, subWidth, subHeight));
        }

        private int getIndex(Node node)
        {
            BoundingRectangle nodeRect = node.GetBoundingBox();
            int index = -1;

            float verticleMidPoint = bounds.X + (bounds.Width / 2);
            float horizontalMidPoint = bounds.Y + (bounds.Height / 2);

            bool topQuadrant = nodeRect.Y < horizontalMidPoint && nodeRect.Y + nodeRect.Height < horizontalMidPoint;
            bool bottomQuadrant = nodeRect.Y > horizontalMidPoint;

            if (nodeRect.X < verticleMidPoint && nodeRect.X + nodeRect.Width < verticleMidPoint)
            {
                if (topQuadrant)
                    index = 0;
                else if (bottomQuadrant)
                    index = 3;
            }
            else if (nodeRect.X > verticleMidPoint)
            {
                if (topQuadrant)
                    index = 1;
                else if (bottomQuadrant)
                    index = 2;  
            }
            return index;
        }

        public void insert(Node node)
        {
            if (!node.HasCollisionGroups() || !node.Active || !node.Collidable)
                return;

            if(!node.GetBoundingBox().Intersects(this.bounds))
                return;

            if(nodes[0] != null)
            {
                int index = getIndex(node);
                if(index != -1)
                {
                    nodes[index].insert(node);
                    return;
                }
            }

            objects.Add(node);

            if(objects.Count > MAX_OBJECTS && level < MAX_LEVELS)
            {
                if(nodes[0] == null)
                    split();

                int i = 0;
                while(i < objects.Count)
                {
                    int index = getIndex(objects.ElementAt(i));
                    if (index != -1)
                    {
                        Node nodeToMove = objects.ElementAt(i);
                        nodes[index].insert(nodeToMove);
                        objects.RemoveAt(i);
                    }
                    else
                        i++;
                }
            }
        }

        public List<Node> retrieve(List<Node> returnObjects, Node node)
        {
            int index = getIndex(node);
            if (index != -1 && nodes[0] != null)
            {
                nodes[index].retrieve(returnObjects, node);
            }

            returnObjects.AddRange(objects);
            return returnObjects;
        }

        public List<BoundingRectangle> getAllQuads(List<BoundingRectangle> quads)
        {
            quads.Add(this.bounds);

            if (nodes[0] == null)
                return quads;
            else
            {
                foreach (QuadTree node in nodes)
                {
                    node.getAllQuads(quads);
                }

                return quads;
            }
        }

    }
}
