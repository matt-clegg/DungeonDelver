using System;

namespace Toolbox
{
    public struct Point2D : IEquatable<Point2D>
    {
        public static readonly Point2D Zero = new Point2D(0, 0);

        public int X { get; }
        public int Y { get; }

        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public double Magnitude => Math.Sqrt(X * X + Y * Y);

        public static Point2D operator +(Point2D a, Point2D b) => new Point2D(a.X + b.X, a.Y + b.Y);
        public static Point2D operator -(Point2D a, Point2D b) => new Point2D(a.X - b.X, a.Y - b.Y);
        public static Point2D operator -(Point2D a) => new Point2D(-a.X, -a.Y);
        public static bool operator ==(Point2D a, Point2D b) => Equals(a, b);
        public static bool operator !=(Point2D a, Point2D b) => !Equals(a, b);

        private static bool Equals(Point2D a, Point2D b) => a.Equals(b);

        public bool Equals(Point2D other) => X == other.X && Y == other.Y;

        public override string ToString() => $"{X}, {Y}";

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }
            return obj is Point2D other && Equals(this, other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 32 + X.GetHashCode();
                hash = hash * 32 + Y.GetHashCode();
                return hash;
            }
        }
    }
}
