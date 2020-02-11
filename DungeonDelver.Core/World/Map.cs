using DungeonDelver.Core.Entities.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonDelver.Core.World
{
    public class Map
    {
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }

        public byte[,] _tiles;

        private readonly List<Creature> _creatures = new List<Creature>();
        public IReadOnlyList<Creature> Creatures => _creatures;

        public Map(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
            _tiles = new byte[width, height];

            Tile wall = Engine.Assets.GetAsset<Tile>("wall");
            Tile floor = Engine.Assets.GetAsset<Tile>("floor");

            Random random = new Random();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SetTile(x, y, random.NextDouble() < 0.3 ? wall : floor);
                }
            }
        }

        public void Update(float delta)
        {
            foreach (Creature creature in _creatures.Where( c => !c.ShouldRemove))
            {
                creature.Update(delta);
            }

            _creatures.RemoveAll(c => c.ShouldRemove);
        }

        public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

        public Tile GetTile(int x, int y)
        {
            return Tile.GetTile(_tiles[x, y]);
        }

        public void SetTile(int x, int y, Tile tile)
        {
            _tiles[x, y] = tile.Id;
        }

        public void Add(Creature creature, int x, int y)
        {
            _creatures.Add(creature);
            creature.RenderX = x * Game.SpriteWidth;
            creature.RenderY = y * Game.SpriteHeight;
            creature.Initialise(this, x, y);
        }

        public Creature GetCreature(int x, int y)
        {
            if(InBounds(x, y))
            {
                foreach (Creature creature in Creatures)
                {
                    if(creature.X == x && creature.Y == y)
                    {
                        return creature;
                    }
                }
            }

            return null;
        }
    }
}
