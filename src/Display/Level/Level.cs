using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Flakcore;
using Flakcore.Display;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Flakcore.Physics;
using Flakcore.Display.Level;

namespace Flakcore.Display.Level
{
    public class Level : Node
    {
        internal const int BLOCK_WIDTH = 32;
        internal const int BLOCK_HEIGHT = 32;
        internal const int ROOM_WIDTH = 24;
        internal const int ROOM_HEIGHT = 16;
        internal const int LEVEL_WIDTH = 4;
        internal const int LEVEL_HEIGHT = 4;

        private static Random Random = new Random();

        private Dungeon Dungeon;

        private Architect Architect;
        private Builder Builder;
        private Furnisher Furnisher;

        public Level()
        {
            CollisionSolver.Level = this;
            Block.Graphic = Controller.Content.Load<Texture2D>("tilemap");
            Block.BorderGraphic = Controller.Content.Load<Texture2D>("level/borders_bunker");
            this.Collidable = true;

            this.InitializeBackground();
            this.Architect = new Architect();
            this.Builder = new Builder();
            this.Furnisher = new Furnisher();

            RoomType[,] plan = this.Architect.GenerateLevel();
            this.Dungeon = this.Builder.BuildLevel(plan);

            this.Furnisher.FurnishRooms(this.Dungeon.Rooms);

            this.AddChild(this.Dungeon);
        }

        private void InitializeBackground()
        {
            TiledSprite borderBackground = new TiledSprite(LEVEL_WIDTH * ROOM_WIDTH * BLOCK_WIDTH * 2, LEVEL_HEIGHT * ROOM_HEIGHT * BLOCK_HEIGHT * 2);
            borderBackground.LoadTexture("level/borderTile");
            borderBackground.Position = new Vector2(-Controller.LevelBorderSize.X, -Controller.LevelBorderSize.Y);

            this.AddChild(borderBackground);

            TiledSprite levelBackground = new TiledSprite(LEVEL_WIDTH * ROOM_WIDTH * BLOCK_WIDTH, LEVEL_HEIGHT * ROOM_HEIGHT * BLOCK_HEIGHT);
            levelBackground.LoadTexture("level/levelTile");

            this.AddChild(levelBackground);
        }

        public Vector2 StartPosition
        {
            get
            {
                return this.Dungeon.StartPosition;
            }
        }

        internal bool HasBlockCollisionGroup(string groupName)
        {
            return groupName == "ladderArea" || groupName == "tilemap";
        }

        internal void GetCollidedTiles(Node node, List<Node> collidedNodes)
        {
            foreach (Room room in this.Dungeon.Rooms)
            {
                room.GetCollidedBlocks(node, collidedNodes);
            }
        }
    }
}
