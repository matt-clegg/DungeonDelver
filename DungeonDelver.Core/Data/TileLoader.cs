using DungeonDelver.Core.Util;
using DungeonDelver.Core.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Assets;
using Toolbox.Data;
using Toolbox.Graphics;

namespace DungeonDelver.Core.Data
{
    public static class TileLoader
    {
        private static byte _nextId = 0;

        public static void Load(string path, AssetStore<string> assets)
        {
            foreach (PropertyBag tileProp in PropertyBag.FromFile(path))
            {
                string name = tileProp.Name;
                bool isSolid = tileProp.GetOrDefault("solid", false);
                bool isTransparent = tileProp.GetOrDefault("transparent",true);
                byte id = _nextId++;

                string spriteName = tileProp.GetOrDefault("sprite", name);
                Sprite sprite = assets.GetAsset<Sprite>(spriteName);
                Color color = ColorParser.ParseColor(tileProp["color"].Value.ToLower().Trim());

                Tile tile = new Tile(id, sprite, color, isSolid, isTransparent);
                assets.AddAsset(name, tile);
            }
        }
    }
}
