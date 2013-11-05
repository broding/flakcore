using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flakcore.Physics
{
    public class Sides
    {
        public bool Top;
        public bool Left;
        public bool Bottom;
        public bool Right;

        public Sides()
        {
        }

        public Sides(bool top, bool bottom, bool left, bool right)
        {
            this.Top = top;
            this.Bottom = bottom;
            this.Left = left;
            this.Right = right;
        }

        public void SetAllTrue()
        {
            this.Top = true;
            this.Bottom = true;
            this.Left = true;
            this.Right = true;
        }

        public void SetAllFalse()
        {
            this.Top = false;
            this.Bottom = false;
            this.Left = false;
            this.Right = false;
        }

        public static bool operator ==(Sides sides1, Sides sides2)
        {
            return
                sides1.Left == sides2.Left &&
                sides1.Right == sides2.Right &&
                sides1.Top == sides2.Top &&
                sides1.Bottom == sides2.Bottom;
        }

        public static bool operator !=(Sides sides1, Sides sides2)
        {
            return
                sides1.Left != sides2.Left ||
                sides1.Right != sides2.Right ||
                sides1.Top != sides2.Top ||
                sides1.Bottom != sides2.Bottom;
        }

        public override bool Equals(object sides)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
