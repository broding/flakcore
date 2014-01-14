using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FlakCore.Physics
{
	public class ConvexShape : ICloneable
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

		public List<Vector2> axis
		{
			get 
			{
				List<Vector2> axis = new List<Vector2>(Points.Count);

				for(int i = 0; i < Points.Count; i++)
				{
					Vector2 p1 = Points [i];
					Vector2 p2 = Points[i + 1 == Points.Count ? 0 : i + 1];
					Vector2 edge = p2 - p1;
					Vector2 normalizedEdge = new Vector2 (edge.X, edge.Y);
					normalizedEdge.Normalize ();
					Vector2 normal = Vector2Utils.Perpendicular (normalizedEdge);
					axis.Add (normal);
				}

				return axis;
			}
		}

		public static ConvexShape operator *(ConvexShape shape, Matrix matrix)
		{
			for (int i = 0; i < shape.Points.Count; i++) 
			{
				shape.Points[i] = Vector2.Transform (shape.Points[i], matrix);
			}

			return shape;
		}

		public object Clone()
		{
			ConvexShape shape = new ConvexShape ();

			foreach (Vector2 point in Points) 
			{
				shape.Points.Add (new Vector2 (point.X, point.Y));
			}

			return shape;
		}

	}
}

