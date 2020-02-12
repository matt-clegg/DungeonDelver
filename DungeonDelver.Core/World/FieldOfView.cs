using System;
using System.Collections.Generic;
using Toolbox;

namespace DungeonDelver.Core.World
{
    public class FieldOfView
    {
        private readonly Map _map;

        public FieldOfView(Map map)
        {
            _map = map;
        }

        public void RefreshVisibility(Point2D point, int radius)
        {
            for (int octant = 0; octant < 8; octant++)
            {
                RefreshOctant(point, octant, radius);
            }

            _map.SetVisible(point.X, point.Y, true);
        }

        private void RefreshOctant(Point2D point, int octant, int radius)
        {
            ShadowLine line = new ShadowLine();
            bool fullShadow = false;
            int radiusSquared = radius * radius;

            for (int row = 1; row <= radius; row++)
            {
                int rowSquared = row * row;

                Point2D pos = point + TransformOctant(row, 0, octant);
                if (!_map.InBounds(pos.X, pos.Y))
                {
                    break;
                }

                for (int col = 0; col <= row; col++)
                {
                    Point2D colPos = point + TransformOctant(row, col, octant);

                    if(!_map.InBounds(colPos.X, colPos.Y))
                    {
                        break;
                    }

                    if(rowSquared + col * col > radiusSquared)
                    {
                        break;
                    }

                    if (fullShadow)
                    {
                        _map.SetVisible(colPos.X, colPos.Y, false);
                    }
                    else
                    {
                        Shadow projection = ProjectTile(row, col);

                        bool visible = !line.IsInShadow(projection);
                        _map.SetVisible(colPos.X, colPos.Y, visible);

                        if (visible && _map.IsSolid(colPos.X, colPos.Y))
                        {
                            line.Add(projection);
                            fullShadow = line.IsFullShadow;
                        }
                    }
                }
            }
        }

        private Point2D TransformOctant(int row, int col, int octant)
        {
            switch (octant)
            {
                case 0: return new Point2D(col, -row);
                case 1: return new Point2D(row, -col);
                case 2: return new Point2D(row, col);
                case 3: return new Point2D(col, row);
                case 4: return new Point2D(-col, row);
                case 5: return new Point2D(-row, col);
                case 6: return new Point2D(-row, -col);
                case 7: return new Point2D(-col, -row);
                default: throw new ArgumentException(nameof(octant), $"Invalid octant: {octant}");
            }
        }

        private Shadow ProjectTile(int row, int col)
        {
            float topLeft = col / (row + 2f);
            float bottomRight = (col + 1f) / (row + 1f);
            return new Shadow(topLeft, bottomRight);
        }

        private class ShadowLine
        {
            private readonly List<Shadow> _shadows = new List<Shadow>();

            public bool IsFullShadow => _shadows.Count == 1 && _shadows[0].Start == 0 && _shadows[0].End == 1;

            public void Add(Shadow shadow)
            {
                int index = 0;

                for (; index < _shadows.Count; index++)
                {
                    if (_shadows[index].Start > +shadow.Start)
                    {
                        break;
                    }
                }

                Shadow overlappingPrevious = null;
                if (index > 0 && _shadows[index - 1].End > shadow.Start)
                {
                    overlappingPrevious = _shadows[index - 1];
                }

                Shadow overlappingNext = null;
                if (index < _shadows.Count && _shadows[index].Start < shadow.End)
                {
                    overlappingNext = _shadows[index];
                }

                if (overlappingNext != null)
                {
                    if (overlappingPrevious != null)
                    {
                        overlappingPrevious.End = overlappingNext.End;
                        _shadows.RemoveAt(index);
                    }
                    else
                    {
                        overlappingNext.Start = shadow.Start;
                    }
                }
                else
                {
                    if (overlappingPrevious != null)
                    {
                        overlappingPrevious.End = shadow.End;
                    }
                    else
                    {
                        _shadows.Insert(index, shadow);
                    }
                }


            }

            public bool IsInShadow(Shadow projection)
            {
                foreach (Shadow shadow in _shadows)
                {
                    if (shadow.Contains(projection))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private class Shadow
        {
            public float Start { get; set; }
            public float End { get; set; }

            public Shadow(float start, float end)
            {
                Start = start;
                End = end;
            }

            public bool Contains(Shadow other)
            {
                return Start <= other.Start && End >= other.End;
            }
        }

    }
}
