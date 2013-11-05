using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Flakcore.Physics;

namespace Flakcore.Display.Level
{
    internal class Furnisher
    {
        private List<Room> Rooms;

        public Furnisher()
        {
        }

        public void FurnishRooms(List<Room> rooms)
        {
            this.Rooms = rooms;

            this.PlaceBlockBorders();
        }

        private void PlaceBlockBorders()
        {
            Block[,] blockMap = this.GetBlockMap();

            for (int x = 0; x < Level.ROOM_WIDTH * Level.LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < Level.ROOM_HEIGHT * Level.LEVEL_HEIGHT; y++)
                {
                    if (blockMap[x, y] != null)
                        this.PlaceBorderOnBlock(blockMap, x, y);


                }
            }
        }

        private void PlaceBorderOnBlock(Block[,] blockMap, int x, int y)
        {
            Block block = blockMap[x, y];

            bool top = false;
            bool bottom = false;
            bool left = false;
            bool right = false;

            if (y > 0)
                top = blockMap[x, y - 1] != null && blockMap[x, y - 1].Type == BlockType.WALL;

            if(y < Level.LEVEL_HEIGHT * Level.ROOM_HEIGHT - 1)
                bottom = blockMap[x, y + 1] != null && blockMap[x, y + 1].Type == BlockType.WALL;

            if (x > 0)
                left = blockMap[x - 1, y] != null && blockMap[x - 1, y].Type == BlockType.WALL;

            if (x < Level.LEVEL_WIDTH * Level.ROOM_WIDTH - 1)
                right = blockMap[x + 1, y] != null && blockMap[x + 1, y].Type == BlockType.WALL;

            block.SetBorders(new Sides(top, bottom, left, right));

        }

        private Block[,] GetBlockMap()
        {
            Block[,] blockMap = new Block[Level.LEVEL_WIDTH * Level.ROOM_WIDTH, Level.LEVEL_HEIGHT * Level.ROOM_HEIGHT];

            foreach (Room room in this.Rooms)
            {
                Vector2 beginPosition = new Vector2(room.LevelPosition.X * Level.ROOM_WIDTH, room.LevelPosition.Y * Level.ROOM_HEIGHT);

                for (int x = 0; x < Level.ROOM_WIDTH; x++)
                {
                    for (int y = 0; y < Level.ROOM_HEIGHT; y++)
                    {
                        blockMap[(int)beginPosition.X + x, (int)beginPosition.Y + y] = room.Map[x, y];
                    }
                }

            }

            return blockMap;
        }
    }
}
