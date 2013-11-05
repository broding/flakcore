using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flakcore.Display.Level.LevelObjects
{
    class LevelObject : Sprite
    {
        public LevelObject()
        {
            this.Collidable = true;
        }
    }
}
