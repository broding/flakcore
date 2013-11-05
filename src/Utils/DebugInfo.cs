using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Flakcore.Utils
{
    class DebugInfo
    {
        private static Dictionary<string, string> infoLines;

        public static void DrawLine(SpriteBatch batch, Texture2D blank, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color,angle, Vector2.Zero, new Vector2(length, width),SpriteEffects.None, 0);
        }

        public static void AddDebugItem(string name, string value)
        {
            if (infoLines == null)
                infoLines = new Dictionary<string, string>();

            if(!infoLines.ContainsKey(name))
                infoLines.Add(name, value);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (infoLines == null)
                return;

            int i = 0;
            foreach (KeyValuePair<string, string> line in infoLines)
            {
                spriteBatch.DrawString(Controller.FontController.GetFont("DefaultFont"), line.Key + ":", new Vector2(3, 16 * i), Color.White);
                spriteBatch.DrawString(Controller.FontController.GetFont("DefaultFont"), line.Value, new Vector2(180, 16 * i), Color.White);
                i++;
            }

            infoLines.Clear();
        }
    }
}
