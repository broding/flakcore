using System;
using Microsoft.Xna.Framework.Audio;
using Flakcore;

namespace FlakCore
{
	public class Sound
	{
		public bool Loop 
		{
			get { return _instance.IsLooped; }
			set { _instance.IsLooped = value; }
		}

		private SoundEffectInstance _instance;
		private SoundEffect _effect;

		public Sound (String name)
		{
			_effect = Director.Content.Load<SoundEffect> (name);
			_instance = _effect.CreateInstance();
		}

		public bool Play()
		{
			return _effect.Play ();
		}

		public bool Play(float volume, float pitch, float pan)
		{
			return _effect.Play (volume, pitch, pan);
		}
	}
}

