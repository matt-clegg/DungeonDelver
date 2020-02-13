using Microsoft.Xna.Framework;

namespace DungeonDelver.Core.Extensions
{
    public static class ColorExtensions
    {
        public static Color Darken(this Color color, float amount)
        {
            if (amount >= 1)
            {
                return Color.Black;
            }

            if(amount < 0)
            {
                return color;
            }

            byte r = (byte)(color.R * (1 - amount));
            byte g = (byte)(color.G * (1 - amount));
            byte b = (byte)(color.B * (1 - amount));

            return Color.FromNonPremultiplied(r, g, b, 255);
        }

        public static Color Blend(this Color color, Color blendWith, float amount)
        {
            byte r = (byte)((color.R * amount) + blendWith.R * (1 - amount));
            byte g = (byte)((color.G * amount) + blendWith.G * (1 - amount));
            byte b = (byte)((color.B * amount) + blendWith.B * (1 - amount));

            return Color.FromNonPremultiplied(r, g, b, 255);
        }
    }
}
