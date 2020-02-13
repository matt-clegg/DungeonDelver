using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonDelver.Core.World.Builders
{
    public class ForestBuilder : MapBuilder
    {
        public ForestBuilder(int width, int height, int depth) : base(width, height, depth)
        {
        }

        public override void Build()
        {
            for(int y = 0; y < Map.Width; y++)
            {
                for(int x = 0; x < Map.Width; x++)
                {
                    Tile tile = null;

                    if (Random.NextDouble() < 0.3)
                    {
                        if(Random.NextDouble() < 0.4)
                        {
                            tile = GetRandomTile("tree_dead_a", "tree_dead_b");
                        }
                        else
                        {
                            tile = GetRandomTile("tree_a", "tree_b");
                        }
                    }
                    else
                    {
                        tile = GetRandomTile("grass_a", "grass_b", "grass_c", "grass_d");
                    }

                    Map.SetTile(x, y, tile);

                    if(Random.NextDouble() < 0.03)
                    {
                        Map.SetTile(x, y, "mushrooms");
                    }

                }
            }
        }

    }
}
