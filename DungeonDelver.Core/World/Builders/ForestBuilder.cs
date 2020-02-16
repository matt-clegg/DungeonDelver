using DungeonDelver.Core.Pathfinding;
using System;
using System.Collections.Generic;
using Toolbox;

namespace DungeonDelver.Core.World.Builders
{
    public class ForestBuilder : MapBuilder
    {

        public ForestBuilder(int width, int height, int depth, int seed) : base(width, height, depth, seed)
        {
        }

        public override void Build()
        {
            for (int y = 0; y < Map.Height; y++)
            {
                for (int x = 0; x < Map.Width; x++)
                {
                    Tile tile = null;

                    if (Random.NextDouble() < 0.3)
                    {
                        if (Random.NextDouble() < 0.4)
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

                    if (Random.NextDouble() < 0.03)
                    {
                        Map.SetTile(x, y, "mushrooms");
                    }

                    if (Random.NextDouble() < 0.1)
                    {
                        Map.SetTile(x, y, "water");
                    }
                }
            }

            int pathDistance = (int)(Map.Width * 0.6f);
            int distance = 0;

            Point2D start;
            Point2D end;

            do
            {
                start = GetRandomEdgeTile(Direction.East);
                end = GetRandomEdgeTile(Direction.West);
                distance = Distance(start, end);
            } while (distance < pathDistance);

            Map.SetTile(start.X, start.Y, "stones_a");
            Map.SetTile(end.X, end.Y, "stones_a");

            DrawPath(start, end, 1, 0.6f);
        }

        private void DrawPath(Point2D start, Point2D end, int thickness = 1, float density = 0.7f)
        {
            List<Point2D> points = AStar<Point2D>.FindPath(Map, start, end, Heuristics.ManhattanDistance, true, true);

            Console.WriteLine("points", points);
            if (points != null)
            {
                foreach (Point2D point in points)
                {
                    int x = point.X;
                    int y = point.Y;
                    if (thickness == 1)
                    {
                        if (Map.InBounds(x, y) && Random.NextDouble() < density)
                        {
                            Map.SetTile(x, y, GetRandomTile("stones_a", "stones_b", "stones_c"));
                        }
                    }
                    else
                    {
                        for (int xa = 0; xa < thickness; xa++)
                        {
                            for (int ya = 0; ya < thickness; ya++)
                            {
                                if (Map.InBounds(x + xa, y + ya) && Random.NextDouble() < density)
                                {
                                    Map.SetTile(x + xa, y + ya, GetRandomTile("stones_a", "stones_b", "stones_c"));
                                }
                            }
                        }
                    }

                }
            }
        }

        private int Distance(Point2D a, Point2D b)
        {
            return (int)Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

    }
}
