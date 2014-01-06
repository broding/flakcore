using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Flakcore.Display;
using Flakcore;
using Flakcore.Utils;
using Flakcore.Physics;
using Microsoft.Xna.Framework.Audio;

namespace Display.Tilemap
{
    public class Tilemap : Node
    {
        public static int tileWidth { get; private set; }
        public static int tileHeight { get; private set; }

        private HashSet<string> CollisionGroups;

        public Dictionary<string, string> Properties;

        private List<TileLayer> Layers;
		private List<Tileset> Tilesets;

        public Tilemap()
        {
            this.Layers = new List<TileLayer>();
            this.Tilesets = new List<Tileset>();
            this.CollisionGroups = new HashSet<string>();
            this.Properties = new Dictionary<string, string>();

            CollisionSolver.Tilemap = this;
        }

        public void LoadMap(string path, int tileWidth, int tileHeight)
        {
            Tilemap.tileWidth = tileWidth;
            Tilemap.tileHeight = tileHeight;

            XDocument doc = XDocument.Load(path);

            Width = Convert.ToInt32(doc.Element("map").Attribute("width").Value);
            Height = Convert.ToInt32(doc.Element("map").Attribute("height").Value);

            // load all properties
            if (doc.Element("map").Element("properties") != null)
            {
                foreach (XElement propElement in doc.Element("map").Element("properties").Elements())
                {
                    this.Properties.Add(propElement.Attribute("name").Value, propElement.Attribute("value").Value);
                }
            }
            

            // load all tilesets
            foreach (XElement element in doc.Descendants("tileset"))
            {
                string assetName = element.Element("image").Attribute("source").Value;
                assetName = Path.GetFileNameWithoutExtension(assetName);

                // load all collisionGroups from this tileset ('collisionGroups' from different tiles)
                string[] tileCollisionGroups = new string[100];
                Dictionary<string, string>[] properties = new Dictionary<string, string>[100];


                foreach (XElement tile in element.Descendants("tile"))
                {
                    int index = (int)tile.Attribute("id");

                    if (tile.Descendants("property").First().Attribute("name").Value == "tags")
                    {
                       
                        string groupName = tile.Descendants("property").First().Attribute("value").Value;

                        tileCollisionGroups[index] = groupName;
                        this.CollisionGroups.Add(groupName);
                    }

                    foreach (XElement tileElements in tile.Element("properties").Elements())
                    {
                        properties[index] = new Dictionary<string, string>();
                        properties[index].Add(tileElements.Attribute("name").Value, tileElements.Attribute("value").Value);
                    }
                }

                Tileset tileset = new Tileset(Convert.ToInt32(element.Attribute("firstgid").Value), element.Attribute("name").Value, Convert.ToInt32(element.Element("image").Attribute("width").Value), Convert.ToInt32(element.Element("image").Attribute("height").Value), Director.Content.Load<Texture2D>(assetName), tileCollisionGroups, properties);

                Tilesets.Add(tileset);
            }

            // load all layers
            foreach (XElement element in doc.Descendants("layer"))
            {
                TileLayer layer = new TileLayer(element.Attribute("name").Value, Convert.ToInt32(element.Attribute("width").Value), Convert.ToInt32(element.Attribute("height").Value), this);

                int x = 0;
                int y = 0;

                foreach (XElement tile in element.Descendants("tile"))
                {
                    if (Convert.ToInt32(tile.Attribute("gid").Value) == 0)
                    {
                        x++;
                        if (x >= layer.Width)
                        {
                            y++;
                            x = 0;
                        }
                        continue;
                    }

                    layer.addTile(Convert.ToInt32(tile.Attribute("gid").Value), x, y, getCorrectTileset(Convert.ToInt32(tile.Attribute("gid").Value)));
                    x++;

                    // check if y needs to be incremented
                    if (x >= layer.Width)
                    {
                        y++;
                        x = 0;
                    }
                }


                Layers.Add(layer);
            }
        }

        private Tileset getCorrectTileset(int gid)
        {
            Tileset best = null;

            foreach (Tileset tileset in Tilesets)
            {
                if (best == null)
                    best = tileset;
                else
                {
                    if (gid >= tileset.FirstGid && tileset.FirstGid > best.FirstGid)
                        best = tileset;
                }
            }

            return best;
        }

        public override void DrawCall(SpriteBatch spriteBatch, DrawProperties worldProperties)
        {
            // loop through all layers to draw them
            foreach (TileLayer layer in Layers)
            {
                layer.DrawCall(spriteBatch, worldProperties);
            }
        }

        public override List<Node> GetAllChildren(List<Node> nodes)
        {
            foreach (TileLayer layer in Layers)
            {
                layer.GetAllChildren(nodes);
            }

            return nodes;
        }

        public TileLayer GetLayer(string name)
        {
            foreach (TileLayer layer in this.Layers)
            {
                if (layer.name == name)
                    return layer;
            }

            throw new Exception("Could not find layer with name");
        }

        public List<Tile> RemoveTiles(int gid)
        {
            List<Tile> tiles = new List<Tile>();

            foreach (TileLayer layer in this.Layers)
            {
                tiles.AddRange(layer.RemoveTiles(gid));
            }

            return tiles;
        }

        internal bool HasTileCollisionGroup(string groupName)
        {
            return this.CollisionGroups.Contains(groupName);
        }

        internal void GetCollidedTiles(Node node, List<Node> collidedNodes)
        {
            foreach (TileLayer layer in this.Layers)
            {
                layer.GetCollidedTiles(node, collidedNodes);
            }
        }
    }
}
