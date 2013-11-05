using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Flakcore;

namespace Flakcore.Display.Level
{
    class Room : Node
    {
        public RoomType RoomType;
        public Vector2 LevelPosition;
        public Block[,] Map { get; private set; }

        public List<Block> Blocks { get; private set; }

        public Room(RoomType roomType)
        {
            this.RoomType = roomType;
            this.Blocks = new List<Block>();
            this.Map = new Block[Level.ROOM_WIDTH, Level.ROOM_HEIGHT];
            this.Width = Level.BLOCK_WIDTH;
            this.Height = Level.BLOCK_HEIGHT;
            this.Collidable = false;
        }

        public void AddBlock(int x, int y, BlockType type)
        {
            Block block = new Block(type, this);
            block.Position = new Vector2(x, y);
            this.Blocks.Add(block);
            this.AddChild(block);
            this.Map[x / Level.BLOCK_WIDTH, y / Level.BLOCK_HEIGHT] = block;
        }

        public List<Block> GetLadderBlocks()
        {
            List<Block> ladderBlocks = new List<Block>(this.Blocks.Count / 2);

            foreach (Block block in this.Blocks)
            {
                if (block.Type == BlockType.LADDER)
                    ladderBlocks.Add(block);
            }

            return ladderBlocks;
        }

        internal void GetCollidedBlocks(Node node, List<Node> collidedNodes)
        {
            Vector2 nodeRoomPosition = node.Position - this.Position;

            int xMin = (int)Math.Floor(nodeRoomPosition.X / Level.BLOCK_WIDTH);
            int xMax = (int)Math.Ceiling((nodeRoomPosition.X + node.Width) / Level.BLOCK_WIDTH);
            int yMin = (int)Math.Floor(nodeRoomPosition.Y / Level.BLOCK_HEIGHT);
            int yMax = (int)Math.Ceiling((nodeRoomPosition.Y + node.Height) / Level.BLOCK_HEIGHT);

            xMin = Math.Max(0, xMin - 1);
            xMax = Math.Min(Width, xMax + 1);
            yMin = Math.Max(0, yMin - 1);
            yMax = Math.Min(Height, yMax + 1);

            xMax = Math.Min(xMax, Level.ROOM_WIDTH);
            yMax = Math.Min(yMax, Level.ROOM_HEIGHT);

            for (var x = xMin; x < xMax; x++)
            {
                for (var y = yMin; y < yMax; y++)
                {
                    Block block = this.Map[x, y];
                    if (block != null)
                        collidedNodes.Add(block);
                }
            }
        }

        internal Block GetBlock(Vector2 position)
        {
            position.X = position.X * Level.BLOCK_WIDTH;
            position.Y = position.Y * Level.BLOCK_HEIGHT;

            foreach (Block block in this.Blocks)
            {
                if (block.Position == position)
                    return block;
            }

            return null;
        }
    }
}
