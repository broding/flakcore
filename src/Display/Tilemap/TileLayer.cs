using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Flakcore;

namespace Display.Tilemap
{
    public class TileLayer : Sprite
    {
        public string name;
        public List<Tile> Tiles;

        private Tilemap _tilemap;

        private Tile[,] _map;

        public Tile[,] map
        {
            get
            {
                return _map;
            }
        }

        public TileLayer(string name, int width, int height, Tilemap tilemap)
        {
            this.name = name;
            this.Width = width;
            this.Height = height;
            this._tilemap = tilemap;

            _map = new Tile[width, height];
            Tiles = new List<Tile>();
        }

        public void addTile(int gid, int x, int y, Tileset tileset)
        {
            gid--; // remove 1 because the Tiled editor start counting at 1, instead of 0

            string[] collisionGroups = new string[10];
            if(tileset.CollisionGroups[gid] != null)
                collisionGroups = tileset.CollisionGroups[gid].Split(' ');

            int sourceY = ((gid * Tilemap.tileWidth) / (tileset.Width)) * Tilemap.tileHeight;
            Rectangle sourceRect = new Rectangle(gid * Tilemap.tileWidth - ((sourceY / Tilemap.tileHeight) * tileset.Width), sourceY, Tilemap.tileWidth, Tilemap.tileHeight);

            map[x, y] = new Tile(x, y, gid, sourceRect, tileset, collisionGroups);
            Tiles.Add(map[x, y]);
            this.AddChild(map[x, y]);
        }

        public override List<Node> GetAllChildren(List<Node> nodes)
        {
            foreach (Tile tile in Tiles)
                nodes.Add(tile);

            return nodes;
        }

        internal void GetCollidedTiles(Node node, List<Node> collidedNodes)
        {
            int xMin = (int)Math.Floor(node.Position.X / Tilemap.tileWidth);
            int xMax = (int)Math.Ceiling((node.Position.X + node.Width) / Tilemap.tileWidth);
            int yMin = (int)Math.Floor(node.Position.Y / Tilemap.tileHeight);
            int yMax = (int)Math.Ceiling((node.Position.Y + node.Height) / Tilemap.tileHeight);

            xMin = Math.Max(0, xMin - 1);
            xMax = Math.Min(Width, xMax + 1);
            yMin = Math.Max(0, yMin - 1);
            yMax = Math.Min(Height, yMax + 1);

            for (var x = xMin; x < xMax; x++)
            {
                for (var y = yMin; y < yMax; y++)
                {
                    if(this._map[x,y] != null)
                        collidedNodes.Add(this._map[x, y]);
                }
            }
        }

        internal List<Tile> RemoveTiles(int gid)
        {
            List<Tile> tiles = new List<Tile>();

            foreach (Tile tile in this.Tiles.ToList())
            {
                if (tile.Gid == gid)
                {
                    tiles.Add(tile);
                    this.RemoveChild(tile);
                    this.Tiles.Remove(tile);
                    this._map[(int)Math.Floor(tile.Position.X / Tilemap.tileWidth), (int)Math.Floor(tile.Position.Y / Tilemap.tileHeight)] = null;
                }
            }

            return tiles;
        }
    }
}
