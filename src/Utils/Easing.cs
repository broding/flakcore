using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flakcore.Utils
{
    class Easing
    {
        public static float Linear(float time, float startValue, float change, float duration)
        {
            return change * time / duration + startValue;
        }
    }
}