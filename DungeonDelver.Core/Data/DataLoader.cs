using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DungeonDelver.Core.Data
{
    public static class DataLoader
    {
        private const float AnimationFrameDuration = 0.4f;

        public static void Load()
        {
            SpritesheetLoader.Load("Content/Data/spritesheets.txt", Engine.Assets, Engine.Instance.Content);
            SpriteLoader.Load("Content/Data/sprites.txt", Engine.Assets);
            AnimationLoader.Load("Content/Data/animations.txt", Engine.Assets);
            TileLoader.Load("Content/Data/tiles.txt", Engine.Assets);
            RaceLoader.Load("Content/Data/races.txt", Engine.Assets);

            AudioLoader.Load(Engine.Assets, Engine.Instance.Content);
        }

        public static Vector2? ParseVector(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            string[] split = input.Trim().Split(' ', ',');

            if (split.Length != 2)
            {
                throw new InvalidOperationException($"Invalid vector string: {input}");
            }

            if (!int.TryParse(split[0], out int x))
            {
                throw new InvalidOperationException($"Invalid vector string: {input}");
            }

            if (!int.TryParse(split[1], out int y))
            {
                throw new InvalidOperationException($"Invalid vector string: {input}");
            }

            return new Vector2(x, y);
        }

        public static Texture2D LoadTexture(string path)
        {
            return Engine.Instance.Content.Load<Texture2D>(path);
        }
    }
}
