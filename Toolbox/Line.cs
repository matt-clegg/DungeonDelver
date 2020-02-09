using System;
using System.Collections;
using System.Collections.Generic;

namespace Toolbox
{
    public class Line : IEnumerable<Point2D>
    {
        private readonly int _xa;
        private readonly int _ya;
        private readonly int _xb;
        private readonly int _yb;

        public Line(int xa, int ya, int xb, int yb)
        {
            _xa = xa;
            _ya = ya;
            _xb = xb;
            _yb = yb;
        }

        private IEnumerator<Point2D> CalculateLine()
        {
            int x = _xa;
            int y = _ya;

            int dx = Math.Abs(_xb - _xa);
            int dy = Math.Abs(_yb - _ya);

            int sx = _xa < _xb ? 1 : -1;
            int sy = _ya < _yb ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                yield return new Point2D(x, y);

                if (x == _xb && y == _yb)
                {
                    yield break;
                }

                int e2 = err * 2;
                if (e2 > -dx)
                {
                    err -= dy;
                    x += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y += sy;
                }
            }
        }

        public IEnumerator<Point2D> GetEnumerator()
        {
            return CalculateLine();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
