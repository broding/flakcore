using System;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Flakcore;

namespace FlakCore
{
	public class EffectUtils
	{
		public static Effect LoadEffect(String path)
		{
			BinaryReader Reader = new BinaryReader(File.Open(Director.ContentPath + path, FileMode.Open)); 
			return new Effect(Director.Graphics.GraphicsDevice, Reader.ReadBytes((int)Reader.BaseStream.Length)); 
		}
	}
}

