using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Toolbox.Assets;
using Toolbox.Data;
using Toolbox.Graphics;

namespace DungeonDelver.Core.Data
{
    public static class SpritesheetLoader
    {
        public static void Load(string path, AssetStore<string> assets, ContentManager content)
        {
            foreach (PropertyBag sheetProp in PropertyBag.FromFile(path))
            {
                string name = sheetProp.Name;
                string sheetPath = sheetProp.GetOrDefault("path", "Content");

                Texture2D texture = content.Load<Texture2D>(Path.Combine(sheetPath, name));
                Spritesheet spritesheet = new Spritesheet(name, texture);
                assets.AddAsset(name, spritesheet);
            }
        }
    }
}
