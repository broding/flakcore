using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flakcore.Display.Level.LevelObjects
{
    class Start : LevelObject
    {
        public Start()
            : base()
        {
            this.LoadTexture("start");
            this.Immovable = true;
        }
    }
}
