using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Flakcore
{
    public class FontController
    {
        private Dictionary<string, SpriteFont> Fonts;

        public FontController()
        {
            this.Fonts = new Dictionary<string, SpriteFont>(30);
            this.LoadFonts();
        }

        private void LoadFonts()
        {
             // load normal rooms
            DirectoryInfo dir = new DirectoryInfo(Controller.Content.RootDirectory + "/fonts/");
            FileInfo[] files = dir.GetFiles("*.xnb", SearchOption.AllDirectories);

            foreach (FileInfo file in files)
            {
                string fontName = Path.GetFileNameWithoutExtension(file.Name);
                this.Fonts.Add(fontName, Controller.Content.Load<SpriteFont>(@"fonts/" + fontName));
            }
        }

        public SpriteFont GetFont(string name)
        {
            foreach (KeyValuePair<string, SpriteFont> font in this.Fonts)
            {
                if (font.Key == name)
                    return font.Value;
            }

            throw new Exception("Could not find font");
        }
    }
}
