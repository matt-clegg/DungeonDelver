using System;
using System.Collections.Generic;
using System.Text;
using Toolbox;
using Toolbox.Rng;

namespace DungeonDelver.Core.World.Builders
{
    public abstract class MapBuilder
    {
        protected IRandom Random { get; private set; }

        public Map Map { get; protected set; }
        public Point2D StartingPoint { get; protected set; }

#if DEBUG
        public List<Map> History { get; private set; }
#endif
        public MapBuilder(int width, int height, int depth, int seed = 1337)
        {
            Random = new DotNetRandom(seed);
            Map = new Map(width, height, depth);

#if DEBUG
            History = new List<Map>();
#endif
        }

        public abstract void Build();

        protected Tile GetRandomTile(params string[] tiles)
        {
            string tile = tiles[Random.Next(tiles.Length)];
            return Engine.Assets.GetAsset<Tile>(tile);
        }

#if DEBUG
        public void TakeSnapshot()
        {
            Map snapshot = Map.Clone();
        }
#endif
    }
}
