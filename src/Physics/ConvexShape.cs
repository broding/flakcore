using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FlakCore.Physics
{
	public class ConvexShape
	{
		public const int MAX_POINTS = 15;

		public readonly List<Vector2> Points;

		public ConvexShape ()
		{
			Points = new List<Vector2> (MAX_POINTS);
		}

		public void setPoint(int index, Vector2 position)
		{
			Points [index] = position;
		}

		public static ConvexShape operator *(ConvexShape shape, Matrix matrix)
		{
			for (int i = 0; i < shape.Points.Count; i++) 
			{
				shape.Points[i] = Vector2.Transform (shape.Points[i], matrix);
			}

			return shape;
		}
	}
}

