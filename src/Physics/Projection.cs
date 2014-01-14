using System;

namespace FlakCore
{
	public class Projection
	{
		private readonly float Min;
		private readonly float Max;

		public Projection (float min, float max)
		{
			Min = min;
			Max = max;
		}

		public bool IsOverlapping(Projection other)
		{
			return !(Min > other.Max || other.Min > Max);
		}

		public float GetOverlap(Projection other)
		{
			if (IsOverlapping(other))
				return Math.Min(Max, other.Max) - Math.Max(Min, other.Min);

			return 0;
		}
	}
}

