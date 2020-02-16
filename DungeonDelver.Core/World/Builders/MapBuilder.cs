using System.Collections.Generic;
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

        public Point2D GetRandomEdgeTile(Point2D direction)
        {
            int x = 0;
            int y = 0;

            switch (direction.X)
            {
                case -1: // West
                    y = Random.Next(Map.Height);
                    break;
                case 1: // East
                    y = Random.Next(Map.Height);
                    x = Map.Width - 1;
                    break;
            }

            switch (direction.Y){
                case -1: // North
                    x = Random.Next(Map.Width);
                    break;
                case 1: // South
                    x = Random.Next(Map.Width);
                    y = Map.Height - 1;
                    break;
            }
            
            return new Point2D(x, y);
        }

        public Point2D GetRandomPoint(bool onlyPassable = false)
        {
            int x = 0;
            int y = 0;

            do
            {
                x = Random.Next(Map.Width);
                y = Random.Next(Map.Height);

                if (!onlyPassable)
                {
                    break;
                }

            } while (Map.IsSolid(x, y));

            return new Point2D(x, y);
        }

#if DEBUG
        public void TakeSnapshot()
        {
            Map snapshot = Map.Clone();
        }
#endif
    }
}
