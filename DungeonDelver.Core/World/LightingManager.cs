using DungeonDelver.Core.Extensions;
using Microsoft.Xna.Framework;
using Toolbox.Rng;

namespace DungeonDelver.Core.World
{
    public class LightingManager
    {
        private readonly IRandom _random;

        private readonly Color _lightningFlashColor;

        private readonly float _minBrightness = 0.4f;
        private readonly float _maxBrightness = 2f;

        public float Brightness { get; private set; }

        public LightingManager(Color lightningFlashColor)
        {
            _lightningFlashColor = lightningFlashColor;
            _random = new DotNetRandom();
            Brightness = _minBrightness;
        }

        public void Update(float delta)
        {
            if (_random.NextDouble() < 0.001)
            {
                Brightness = (float)_random.NextDouble(_maxBrightness - 0.2f, _maxBrightness);

                //thunderInstance.Play();
                //_soundEffectManager.Play("thunder_01", (float)_random.NextDouble(0.05, 0.25), (float)_random.NextDouble(-0.3, 0.3), (float)_random.NextDouble(-0.2, 0.1));
            }

            if (Brightness > _minBrightness)
            {
                Brightness *= 0.9f;

                if (_random.NextDouble() < 0.04)
                {
                    Brightness += (float)_random.NextDouble(0.1, 0.3);
                }

                if (Brightness < _minBrightness)
                {
                    Brightness = _minBrightness;
                }
            }
        }

        public Color GetLitColor(Color color, Color? flashColor = null)
        {
            Color newColor = color.Darken(1f - Brightness);
            if (Brightness > _minBrightness)
            {
                float flashAmount = (Brightness - _minBrightness) / (_maxBrightness - _minBrightness);
                newColor = newColor.Blend(flashColor ?? _lightningFlashColor, 1f - flashAmount);
            }

            return newColor;
        }
    }
}
