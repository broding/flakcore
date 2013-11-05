using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Flakcore.Utils
{
    /// <summary>
    /// Much like Rectangle, but stored as two Vector2s
    /// </summary>
    public struct BoundingRectangle
    {
        public Vector2 Min;
        public Vector2 Max;

        public float Left { get { return this.Min.X; } }
        public float Right { get { return this.Max.X; } }
        public float Top { get { return this.Min.Y; } }
        public float Bottom { get { return this.Max.Y; } }

        public float X { get { return this.Min.X; } }
        public float Y { get { return this.Min.Y; } }

        public float Width { get { return this.Max.X - this.Min.X; } }
        public float Height { get { return this.Max.Y - this.Min.Y; } }

        private static BoundingRectangle mEmpty;
        private static BoundingRectangle mMinMax;

        static BoundingRectangle()
        {
            BoundingRectangle.mEmpty = new BoundingRectangle();
            BoundingRectangle.mMinMax = new BoundingRectangle(Vector2.One * float.MinValue, Vector2.One * float.MaxValue);
        }

        public Vector2 Center
        {
            get { return (this.Min + this.Max) / 2; }
        }

        public static BoundingRectangle Empty
        {
            get { return BoundingRectangle.mEmpty; }
        }


        public static BoundingRectangle MinMax
        {
            get { return BoundingRectangle.mMinMax; }
        }

        public bool IsZero
        {
            get
            {
                return
                    (this.Min.X == 0) &&
                    (this.Min.Y == 0) &&
                    (this.Max.X == 0) &&
                    (this.Max.Y == 0);
            }
        }

        public BoundingRectangle(float x, float y, float width, float height)
        {
            this.Min.X = x;
            this.Min.Y = y;
            this.Max.X = x + width;
            this.Max.Y = y + height;
        }

        public BoundingRectangle(Vector2 min, Vector2 max)
        {
            this.Min = min;
            this.Max = max;
        }

        public bool Contains(float x, float y)
        {
            return
                (this.Min.X <= x) &&
                (this.Min.Y <= y) &&
                (this.Max.X >= x) &&
                (this.Max.Y >= y);
        }

        public bool Contains(Vector2 vector)
        {
            return
                (this.Min.X <= vector.X) &&
                (this.Min.Y <= vector.Y) &&
                (this.Max.X >= vector.X) &&
                (this.Max.Y >= vector.Y);
        }

        public void Contains(ref Vector2 rect, out bool result)
        {
            result =
                (this.Min.X <= rect.X) &&
                (this.Min.Y <= rect.Y) &&
                (this.Max.X >= rect.X) &&
                (this.Max.Y >= rect.Y);
        }

        public bool Contains(BoundingRectangle rect)
        {
            return
                (this.Min.X <= rect.Min.X) &&
                (this.Min.Y <= rect.Min.Y) &&
                (this.Max.X >= rect.Max.X) &&
                (this.Max.Y >= rect.Max.Y);
        }

        public void Contains(ref BoundingRectangle rect, out bool result)
        {
            result =
                (this.Min.X <= rect.Min.X) &&
                (this.Min.Y <= rect.Min.Y) &&
                (this.Max.X >= rect.Max.X) &&
                (this.Max.Y >= rect.Max.Y);
        }

        public bool Intersects(BoundingRectangle rect)
        {
            return
                (this.Min.X < rect.Max.X) &&
                (this.Min.Y < rect.Max.Y) &&
                (this.Max.X > rect.Min.X) &&
                (this.Max.Y > rect.Min.Y);
        }

        public void Intersects(ref BoundingRectangle rect, out bool result)
        {
            result =
                (this.Min.X < rect.Max.X) &&
                (this.Min.Y < rect.Max.Y) &&
                (this.Max.X > rect.Min.X) &&
                (this.Max.Y > rect.Min.Y);
        }

        public static BoundingRectangle Intersect(BoundingRectangle rect1, BoundingRectangle rect2)
        {
            BoundingRectangle result;

            float num8 = rect1.Max.X;
            float num7 = rect2.Max.X;
            float num6 = rect1.Max.Y;
            float num5 = rect2.Max.Y;
            float num2 = (rect1.Min.X > rect2.Min.X) ? rect1.Min.X : rect2.Min.X;
            float num = (rect1.Min.Y > rect2.Min.Y) ? rect1.Min.Y : rect2.Min.Y;
            float num4 = (num8 < num7) ? num8 : num7;
            float num3 = (num6 < num5) ? num6 : num5;

            if ((num4 > num2) && (num3 > num))
            {
                result.Min.X = num2;
                result.Min.Y = num;
                result.Max.X = num4;
                result.Max.Y = num3;

                return result;
            }

            result.Min.X = 0;
            result.Min.Y = 0;
            result.Max.X = 0;
            result.Max.Y = 0;

            return result;
        }

        public static void Intersect(ref BoundingRectangle rect1, ref BoundingRectangle rect2, out BoundingRectangle result)
        {
            float num8 = rect1.Max.X;
            float num7 = rect2.Max.X;
            float num6 = rect1.Max.Y;
            float num5 = rect2.Max.Y;
            float num2 = (rect1.Min.X > rect2.Min.X) ? rect1.Min.X : rect2.Min.X;
            float num = (rect1.Min.Y > rect2.Min.Y) ? rect1.Min.Y : rect2.Min.Y;
            float num4 = (num8 < num7) ? num8 : num7;
            float num3 = (num6 < num5) ? num6 : num5;

            if ((num4 > num2) && (num3 > num))
            {
                result.Min.X = num2;
                result.Min.Y = num;
                result.Max.X = num4;
                result.Max.Y = num3;
            }

            result.Min.X = 0;
            result.Min.Y = 0;
            result.Max.X = 0;
            result.Max.Y = 0;
        }

        public static BoundingRectangle Union(BoundingRectangle rect1, BoundingRectangle rect2)
        {
            BoundingRectangle result;

            float num6 = rect1.Max.X;
            float num5 = rect2.Max.X;
            float num4 = rect1.Max.Y;
            float num3 = rect2.Max.Y;
            float num2 = (rect1.Min.X < rect2.Min.X) ? rect1.Min.X : rect2.Min.X;
            float num = (rect1.Min.Y < rect2.Min.Y) ? rect1.Min.Y : rect2.Min.Y;
            float num8 = (num6 > num5) ? num6 : num5;
            float num7 = (num4 > num3) ? num4 : num3;

            result.Min.X = num2;
            result.Min.Y = num;
            result.Max.X = num8;
            result.Max.Y = num7;

            return result;
        }

        public static void Union(ref BoundingRectangle rect1, ref BoundingRectangle rect2, out BoundingRectangle result)
        {
            float num6 = rect1.Max.X;
            float num5 = rect2.Max.X;
            float num4 = rect1.Max.Y;
            float num3 = rect2.Max.Y;
            float num2 = (rect1.Min.X < rect2.Min.X) ? rect1.Min.X : rect2.Min.X;
            float num = (rect1.Min.Y < rect2.Min.Y) ? rect1.Min.Y : rect2.Min.Y;
            float num8 = (num6 > num5) ? num6 : num5;
            float num7 = (num4 > num3) ? num4 : num3;

            result.Min.X = num2;
            result.Min.Y = num;
            result.Max.X = num8;
            result.Max.Y = num7;
        }

        public bool Equals(BoundingRectangle other)
        {
            return
                (this.Min.X == other.Min.X) &&
                (this.Min.Y == other.Min.Y) &&
                (this.Max.X == other.Max.X) &&
                (this.Max.Y == other.Max.Y);
        }

        public override int GetHashCode()
        {
            return this.Min.GetHashCode() + this.Max.GetHashCode();
        }

        public static bool operator ==(BoundingRectangle a, BoundingRectangle b)
        {
            return
                (a.Min.X == b.Min.X) &&
                (a.Min.Y == b.Min.Y) &&
                (a.Max.X == b.Max.X) &&
                (a.Max.Y == b.Max.Y);
        }

        public static bool operator !=(BoundingRectangle a, BoundingRectangle b)
        {
            return
                (a.Min.X != b.Min.X) ||
                (a.Min.Y != b.Min.Y) ||
                (a.Max.X != b.Max.X) ||
                (a.Max.Y != b.Max.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is BoundingRectangle)
            {
                return this == (BoundingRectangle)obj;
            }

            return false;
        }
    }
}