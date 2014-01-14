using System;
using Microsoft.Xna.Framework;

namespace FlakCore
{
	public class Vector2Utils
	{
		public static Vector2 Perpendicular(Vector2 original)
		{
			return new Vector2 (-original.Y, original.X);
		}

		public static float DotProduct(Vector2 v1, Vector2 v2)
		{
			return v1.X * v2.X + v1.Y * v2.Y;
		}
	}
}

