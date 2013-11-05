using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Display.Tilemap
{
    public class Tileset
    {
        public int FirstGid { get; private set; }
        public string Name { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Texture2D Texture { get; private set; }
        public string[] CollisionGroups { get; private set; }
        public Dictionary<string, string>[] Properties { get; private set; }


        public Tileset(int firstGid, string name, int width, int height, Texture2D graphic, string[] collisionGroups, Dictionary<string,string>[] properties)
        {
            this.FirstGid = firstGid;
            this.Name = name;
            this.Width = width;
            this.Height = height;
            this.Texture = graphic;
            this.CollisionGroups = collisionGroups;
            this.Properties = properties;
        }

        public Dictionary<string, string> GetPropertiesOfGid(int gid)
        {
            if (this.Properties[gid] != null)
                return this.Properties[gid];
            else
                return null;
        }
    }
}
