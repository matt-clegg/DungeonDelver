﻿using DungeonDelver.Core.Entities;
using DungeonDelver.Core.Entities.Creatures;
using DungeonDelver.Core.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using Toolbox;
using Toolbox.Rng;

namespace DungeonDelver.Core.World
{
    public class Map : IWeightedGraph<Point2D>
    {
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }

        private readonly byte[,] _tiles;
        private readonly Prop[,] _props;

        private readonly List<Creature> _creatures = new List<Creature>();
        public IReadOnlyList<Creature> Creatures => _creatures;

        private readonly HashSet<Point2D> _visible = new HashSet<Point2D>();
        private readonly HashSet<Point2D> _visited = new HashSet<Point2D>();

        public int Area => Width * Height;

        public Map(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
            _tiles = new byte[width, height];
            _props = new Prop[width, height];
        }

        public void Update(float delta)
        {
            foreach (Creature creature in _creatures.Where( c => !c.ShouldRemove))
            {
                creature.Update(delta);
            }

            _creatures.RemoveAll(c => c.ShouldRemove);

            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    _props[x, y]?.Update(delta);
                }
            }
        }

        public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

        public bool IsSolid(int x, int y) => GetTile(x, y).IsSolid;

        public void SetVisible(int x, int y, bool isVisible)
        {
            Point2D pos = new Point2D(x, y);

            if (isVisible)
            {
                _visible.Add(pos);
                _visited.Add(pos);
            }
            else
            {
                _visible.Remove(pos);
            }
        }

        public bool IsVisible(int x, int y)
        {
            return _visible.Contains(new Point2D(x, y));
        }

        public bool IsVisited(int x, int y)
        {
            return _visited.Contains(new Point2D(x, y));
        }

        public void ClearFov()
        {
            _visible.Clear();
        }

        public Tile GetTile(int x, int y)
        {
            return Tile.GetTile(_tiles[x, y]);
        }

        public void SetTile(int x, int y, string name)
        {
            Tile tile = Engine.Assets.GetAsset<Tile>(name);
            SetTile(x, y, tile);
        }

        public void SetTile(int x, int y, Tile tile)
        {
            _tiles[x, y] = tile.Id;
        }

        public void SetProp(int x, int y, Prop prop)
        {
            _props[x, y] = prop;
        }

        public Prop GetProp(int x, int y)
        {
            return _props[x, y];
        }

        public void Add(Creature creature, Point2D point) => Add(creature, point.X, point.Y);

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

        public Point2D RandomEmptyPoint(IRandom random)
        {
            int x;
            int y;

            do
            {
                x = random.Next(Width);
                y = random.Next(Height);
            } while (IsSolid(x, y) || GetCreature(x, y) != null);

            return new Point2D(x, y);
        }

        public float Cost(Point2D a, Point2D b)
        {
            return 1f;
        }

        public IEnumerable<Point2D> GetNeighbors(Point2D origin)
        {
            foreach(Point2D neighbor in Direction.GetNeighbours(origin, Direction.AllDirections))
            {
                if(InBounds(neighbor.X, neighbor.Y) && !GetTile(neighbor.X, neighbor.Y).IsSolid)
                {
                    yield return neighbor;
                }
            }
        }

#if DEBUG
        public Map Clone()
        {
            Map map = new Map(Width, Height, Depth);
            Array.Copy(_tiles, map._tiles, _tiles.Length);
            return map;
        }
#endif

    }
}
