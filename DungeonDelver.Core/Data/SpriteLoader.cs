using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using Toolbox.Assets;
using Toolbox.Data;
using Toolbox.Graphics;

namespace DungeonDelver.Core.Data
{
    public static class SpriteLoader
    {
        public static void Load(string path, AssetStore<string> assets)
        {
            foreach (PropertyBag artProp in PropertyBag.FromFile(path))
            {
                string sheetName = artProp.GetOrDefault("sheet", string.Empty);
                Spritesheet sheet = assets.GetAsset<Spritesheet>(sheetName);

                string name = artProp.Name;
                int x = artProp.GetOrDefault("x", 0);
                int y = artProp.GetOrDefault("y", 0);
                int width = artProp.GetOrDefault("width", -1);
                int height = artProp.GetOrDefault("height", -1);
                bool exact = artProp.GetOrDefault("exact", false);

                Vector2? origin = DataLoader.ParseVector(artProp.GetOrDefault("origin", null));

                Sprite sprite;

                if (!exact)
                {
                    sprite = sheet.CutSprite(x, y, width, height, name, origin);
                }
                else
                {
                    sprite = sheet.CutSpriteExact(x, y, width, height, name, origin);
                }
                assets.AddAsset(name, sprite);
                Console.WriteLine($"Loaded Sprite: {name}");
            }
        }
    }
}
