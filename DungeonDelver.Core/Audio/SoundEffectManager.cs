using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Toolbox.Rng;

namespace DungeonDelver.Core.Audio
{
    public class SoundEffectManager
    {
        private readonly IRandom _random = new DotNetRandom();

        private readonly Dictionary<string, SoundEffect> _soundEffects = new Dictionary<string, SoundEffect>();
        private readonly List<SoundEffectInstance> _soundsPlaying = new List<SoundEffectInstance>();

        public void LoadSoundEffect(string name)
        {
            _soundEffects.Add(name, Engine.Assets.GetAsset<SoundEffect>(name));
        }

        public void Play(string name, float volume = 1f, float pan = 0f, float pitch = 0, bool loop = false)
        {
            SoundEffectInstance instance = _soundEffects[name].CreateInstance();
            instance.Volume = volume;
            instance.Pan = pan;
            instance.Pitch = pitch;

            _soundsPlaying.Add(instance);
            instance.Play();
        }

        public void Update()
        {
            foreach (SoundEffectInstance instance in _soundsPlaying)
            {
                if (instance.State == SoundState.Stopped)
                {
                    instance.Dispose();
                }
            }

            _soundsPlaying.RemoveAll(i => i.State == SoundState.Stopped);
        }
    }
}
