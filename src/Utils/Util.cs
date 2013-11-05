using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;

namespace Flakcore.Utils
{
    public class Util
    {
        public static int RandomPositiveNegative()
        {
            int number = new Random().Next(0, 2);

            return number == 1 ? 1 : -1;
        }

        public static int FacingToVelocity(Facing facing)
        {
            return facing == Facing.Left ? -1 : 1;
        }

        public static bool Visible(Microsoft.Xna.Framework.Vector2 position, int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}
