using DungeonDelver.Core.Data.Definitions;
using DungeonDelver.Core.Entities.Creatures;
using DungeonDelver.Core.Turns;
using DungeonDelver.Core.Util;
using Microsoft.Xna.Framework;
using Toolbox.Assets;
using Toolbox.Data;

namespace DungeonDelver.Core.Data
{
    public static class RaceLoader
    {
        public static void Load(string path, AssetStore<string> assets)
        {
            foreach (PropertyBag raceProp in PropertyBag.FromFile(path))
            {
                string name = raceProp.Name;
                int speed = raceProp.GetOrDefault("speed", Energy.NormalSpeed);

                Animation animation = assets.GetAsset<Animation>(raceProp["animation"].Value);

                Color color = ColorParser.FromName(raceProp["color"].Value);

                Race race = new Race(name, animation, color, speed);
                assets.AddAsset(name, race);
            }
        }
    }
}
