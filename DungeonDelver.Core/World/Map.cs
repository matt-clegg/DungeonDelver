using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonDelver.Core.World
{
    public class Map
    {
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }

        public byte[,] _tiles;

        public Map(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;
    }
}
