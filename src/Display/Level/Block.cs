using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Flakcore.Physics;

namespace Flakcore.Display.Level
{
    class Block : Sprite
    {
        public static Texture2D Graphic { get; set; }
        public static Texture2D BorderGraphic { get; set; }

        public BlockType Type { get; private set; }
        public Room Room { get; private set; }

        private static Rectangle BorderSourceRectangle;

        private Sides Borders;

        public Block(BlockType type, Room room)
        {
            this.LoadTexture(Block.Graphic, Level.BLOCK_WIDTH, Level.BLOCK_HEIGHT);
            this.Type = type;
            this.Collidable = false;
            this.Immovable = true;
            this.Borders = new Sides();
            this.SetupBlock();
            this.Room = room;
            Block.BorderSourceRectangle = new Rectangle(0, 0, Block.BorderGraphic.Width, Block.BorderGraphic.Height);
        }

        private void SetupBlock()
        {
            switch (this.Type)
            {
                case BlockType.WALL:
                    this.AddCollisionGroup("tilemap");
                    this.SourceRectangle = new Rectangle(32, 0, 32, 32);
                    this.Depth = 0.2f;
                    break;
                case BlockType.LADDERAREA:
                case BlockType.LADDER:
                    this.AddCollisionGroup("ladderArea");
                    this.SourceRectangle = new Rectangle(64, 0, 32, 32);
                    this.CollidableSides.SetAllFalse();
                    this.CollidableSides.Top = true;
                    break;
            }
        }

        internal void SetBorders(Sides borders)
        {
            this.Borders = borders;
        }

        public override void Draw(SpriteBatch spriteBatch, WorldProperties worldProperties)
        {
            base.Draw(spriteBatch, worldProperties);

            this.DrawBorders(spriteBatch, worldProperties.Position);
        }

        private void DrawBorders(SpriteBatch spriteBatch, Vector2 position)
        {
            Vector2 drawPosition = position;

            if (!this.Borders.Top && this.Type == BlockType.WALL)
            {
                drawPosition.X -= 4;
                drawPosition.Y -= 10;

                spriteBatch.Draw(Block.BorderGraphic,
                    drawPosition,
                    Block.BorderSourceRectangle,
                    Color.White * this.Alpha,
                    0,
                    Vector2.Zero,
                    Vector2.One,
                    this.SpriteEffects,
                    this.WorldDepth
                    );

                drawPosition = position;
            }

            if (!this.Borders.Bottom && this.Type == BlockType.WALL)
            {
                drawPosition.X += Level.BLOCK_WIDTH + 6;
                drawPosition.Y += Level.BLOCK_HEIGHT + 10;

                spriteBatch.Draw(Block.BorderGraphic,
                    drawPosition,
                    Block.BorderSourceRectangle,
                    Color.White * this.Alpha,
                    (float)Math.PI,
                    Vector2.Zero,
                    Vector2.One,
                    this.SpriteEffects,
                    this.WorldDepth
                    );

                drawPosition = position;
            }



            if (!this.Borders.Right && this.Type == BlockType.WALL)
            {
                drawPosition.X += Level.BLOCK_WIDTH + 10;
                drawPosition.Y -= 6;

                spriteBatch.Draw(Block.BorderGraphic,
                    drawPosition,
                    Block.BorderSourceRectangle,
                    Color.White * this.Alpha,
                    (float)Math.PI / 2,
                    Vector2.Zero,
                    Vector2.One,
                    this.SpriteEffects,
                    this.WorldDepth
                    );

                drawPosition = position;
            }

        }

        public Vector2 RoomPosition
        {
            get
            {
                return new Vector2((float)Math.Floor((float)this.Position.X / Level.BLOCK_WIDTH), (float)Math.Floor((float)this.Position.Y / Level.BLOCK_HEIGHT));
            }
        }

        public Vector2 LevelPosition
        {
            get
            {
                Vector2 roomPosition = this.RoomPosition;
                return new Vector2(this.Room.LevelPosition.X * Level.ROOM_WIDTH + roomPosition.X, this.Room.LevelPosition.Y * Level.ROOM_HEIGHT + roomPosition.Y);
            }
        }

        public static BlockType GetBlockTypeFromString(string type)
        {
            switch (type)
            {
                case "0":
                    return BlockType.WALL;
                case "1":
                    return BlockType.LADDERAREA;
                case "L":
                    return BlockType.LADDER;
                case "R":
                    return BlockType.REWARD;
                case "S":
                    return BlockType.START;
                case "E":
                    return BlockType.END;
            }

            return BlockType.EMPTY;
        }
    }

    public enum BlockType
    {
        EMPTY,
        WALL,
        LADDERAREA,
        LADDER,
        START,
        END,
        REWARD

    }
}
