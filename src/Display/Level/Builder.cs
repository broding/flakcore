using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Flakcore.Display.Level.LevelObjects;

namespace Flakcore.Display.Level
{
    class Builder
    {
        private Dungeon Dungeon;

        public Builder()
        {

        }

        internal Dungeon BuildLevel(RoomType[,] plan)
        {
            this.Dungeon = new Dungeon();

            for (int x = 0; x < Level.LEVEL_WIDTH; x++)
                for (int y = 0; y < Level.LEVEL_HEIGHT; y++)
                {
                    Room room = plan[x, y].CreateRoom();
                    this.Dungeon.AddRoom(room, new Vector2(x, y));
                }

            this.BuildLadders();
            this.BuildStart();
            this.BuildExit();

            return this.Dungeon;
        }

        private void BuildLadders()
        {
            foreach (Room room in this.Dungeon.Rooms)
            {
                List<Block> ladderBlocks = room.GetLadderBlocks();

                foreach (Block block in ladderBlocks)
                {
                    this.BuildLadder(block);
                }
            }
        }

        private void BuildLadder(Block block)
        {
            Vector2 beginPosition = block.LevelPosition;
            beginPosition.Y -= 1;
            Vector2 position = beginPosition;

            while (block == null || block.Type != BlockType.WALL)
            {
                position.Y++;
                block = this.Dungeon.GetBlock(position);
            }

            Ladder ladder = new Ladder((int)position.Y - (int)beginPosition.Y);
            this.Dungeon.AddObject(ladder, new Vector2(beginPosition.X, beginPosition.Y));
        }

        private void BuildStart()
        {
            Start start = new Start();
            Vector2 levelPosition = this.Dungeon.GetBlock(BlockType.START).LevelPosition;

            this.Dungeon.AddObject(start, new Vector2(levelPosition.X - 2, levelPosition.Y - 2));
        }

        private void BuildExit()
        {
            End end = new End();
            Vector2 levelPosition = this.Dungeon.GetBlock(BlockType.END).LevelPosition;

            this.Dungeon.AddObject(end, new Vector2(levelPosition.X - 2, levelPosition.Y - 2));
        }
    }
}
