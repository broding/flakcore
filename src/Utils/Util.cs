using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;

namespace Flakcore.Utils
{
    public class Util
    {
		private static Random _random = new Random ();

		public static Random Random
		{
			get { return _random; }
		}

		public static int RandomSeed {
			set { 
				RandomSeed = value;
				_random = new Random (value);
			}
		}

        public static int RandomPositiveNegative()
        {
            int number = _random.Next(0, 2);

            return number == 1 ? 1 : -1;
        }

        public static int FacingToVelocity(Facing facing)
        {
            return facing == Facing.Left ? -1 : 1;
        } 
    }
}
