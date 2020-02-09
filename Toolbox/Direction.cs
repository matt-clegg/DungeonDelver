using System.Collections.Generic;
using System.Linq;

namespace Toolbox
{
    public static class Direction
    {
        public static readonly Point2D North = new Point2D(0, -1);
        public static readonly Point2D South = new Point2D(0, 1);
        public static readonly Point2D East = new Point2D(1, 0);
        public static readonly Point2D West = new Point2D(-1, 0);

        public static readonly Point2D NorthEast = new Point2D(1, -1);
        public static readonly Point2D NorthWest = new Point2D(-1, -1);
        public static readonly Point2D SouthEast = new Point2D(1, 1);
        public static readonly Point2D SouthWest = new Point2D(-1, 1);

        public static readonly Point2D[] Vertical =
        {
          North,
          South
        };

        public static readonly Point2D[] Horizontal =
        {
          East,
          West
        };

        public static readonly Point2D[] DiagonalDirections =
        {
          NorthEast,
          NorthWest,
          SouthEast,
          SouthWest
        };

        public static readonly Point2D[] CardinalDirections =
        {
          North,
          South,
          East,
          West
        };

        public static readonly Point2D[] AllDirections =
        {
          North,
          South,
          East,
          West,
          NorthEast,
          NorthWest,
          SouthEast,
          SouthWest
        };

        public static IEnumerable<Point2D> GetNeighbours(Point2D point) => GetNeighbours(point.X, point.Y);

        public static IEnumerable<Point2D> GetNeighbours(int x, int y) => GetNeighbours(x, y, AllDirections);

        public static IEnumerable<Point2D> GetNeighbours(int x, int y, IEnumerable<Point2D> neighbours) => GetNeighbours(new Point2D(x, y), neighbours);

        public static IEnumerable<Point2D> GetNeighbours(Point2D point, IEnumerable<Point2D> neighbours)
        {
            return neighbours.Select(neighbour => neighbour + point);
        }
    }
}
