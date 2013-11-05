using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Flakcore.Display.Level.LevelObjects;

namespace Flakcore.Display.Level
{
    class Dungeon : Node
    {
        public List<Room> Rooms { get; private set; }
        public List<LevelObject> LevelObjects { get; private set; }

        public Dungeon()
        {
            this.Collidable = true;
            this.Rooms = new List<Room>(Level.LEVEL_WIDTH * Level.LEVEL_HEIGHT);
            this.LevelObjects = new List<LevelObject>();
            this.UpdateChildren = false;
        }

        public Block GetBlock(int x, int y)
        {
            Vector2 roomPosition = new Vector2((float)Math.Floor((float)x / Level.ROOM_WIDTH), (float)Math.Floor((float)y / Level.ROOM_HEIGHT));
            Room room = this.GetRoom(roomPosition);

            if (room != null)
                return room.GetBlock(new Vector2(x % Level.ROOM_WIDTH, y % Level.ROOM_HEIGHT));
            else
                return null;
        }

        public Block GetBlock(Vector2 position)
        {
            return this.GetBlock((int)position.X, (int)position.Y);
        }

        public Room GetRoom(Vector2 position)
        {
            foreach (Room room in this.Rooms)
            {
                if (room.LevelPosition == position)
                    return room;
            }

            return null;
        }

        public Block GetBlock(BlockType blockType)
        {
            foreach (Room room in this.Rooms)
            {
                foreach (Block block in room.Blocks)
                {
                    if (block.Type == blockType)
                    {
                        return block;
                    }
                }
            }

            return null;
        }

        public Vector2 StartPosition
        {
            get
            {
                foreach (LevelObject levelObject in this.LevelObjects)
                {
                    if (levelObject is Start)
                        return levelObject.Position;
                }

                throw new Exception("Could not find start position");
            }
        }

        internal void AddRoom(Room room, Vector2 position)
        {
            this.Rooms.Add(room);
            room.LevelPosition = position;
            room.Position = new Vector2(position.X * (Level.ROOM_WIDTH * Level.BLOCK_WIDTH), position.Y * (Level.ROOM_HEIGHT * Level.BLOCK_HEIGHT));
            this.AddChild(room);
        }

        internal void AddObject(LevelObject ladder, Vector2 position)
        {
            this.LevelObjects.Add(ladder);
            ladder.Position = new Vector2(Level.BLOCK_WIDTH * position.X, Level.BLOCK_HEIGHT * position.Y);
            this.AddChild(ladder);

        }
    }
}
