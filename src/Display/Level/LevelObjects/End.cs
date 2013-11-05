using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flakcore.Display.Level.LevelObjects
{
    class End : LevelObject
    {
        public End()
            : base()
        {
            this.LoadTexture("start");
            this.AddCollisionGroup("end");
            this.Immovable = true;
        }
    }
}
