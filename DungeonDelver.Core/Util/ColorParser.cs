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
            { "dark_green", Color.FromNonPremultiplied(0, 55, 38, 255) },
            { "green", Color.FromNonPremultiplied(89, 133, 36, 255) },
        };

        public static Color ParseColor(string input)
        {
            if (input.StartsWith("#"))
            {
                int hex = Convert.ToInt32(input.Substring(1).Trim(), 16);

                int r = (hex & 0xFF0000) >> 16;
                int g = (hex & 0xFF00) >>8;
                int b = hex & 0xFF;

                return Color.FromNonPremultiplied(r, g, b, 255);
            }
            else if (input.Contains(","))
            {
                string[] split = input.Split(',');

                if(split.Length != 3)
                {
                    throw new InvalidOperationException($"Invalid RGB input: {input}");
                }

                if(!int.TryParse(split[0].Trim(), out int r) ||
                   !int.TryParse(split[1].Trim(), out int g) ||
                   !int.TryParse(split[2].Trim(), out int b))
                {
                    throw new InvalidOperationException($"Invalid RGB value in input: {input}");
                }

                return Color.FromNonPremultiplied(r, g, b, 255);
            }
            else
            {
                return FromName(input);
            }
        }

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

