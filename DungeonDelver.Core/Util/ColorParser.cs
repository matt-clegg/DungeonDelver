using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace DungeonDelver.Core.Util
{
    public static class ColorParser
    {
        private static readonly Dictionary<string, Color> _colors = new Dictionary<string, Color>
        {
            { "red", Color.Red },
            { "white", Color.White },
            { "black", Color.Black },
            { "brown", Color.Brown },
            { "gray", Color.Gray },
            { "orange", Color.Orange },
        };

        public static Color FromName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!_colors.TryGetValue(name, out Color color))
            {
                throw new InvalidOperationException($"Unknown color: {color}");
            }

            return color;
        }
    }
}

