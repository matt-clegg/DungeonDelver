﻿using Microsoft.Xna.Framework;
using System;
using Toolbox.Graphics;

namespace DungeonDelver.Core.World
{
    public class Tile
    {
        private static readonly Tile[] _tiles = new Tile[byte.MaxValue];

        public byte Id { get; }
        public Sprite Sprite { get; }
        public Color Color { get; }
        public bool IsSolid { get; }
        public bool IsTransparent { get; }
        public string Animation { get; }

        public Tile(byte id, Sprite sprite, Color color, bool isSolid, bool isTransparent, string animation)
        {
            Id = id;
            Sprite = sprite;
            Color = color;
            IsSolid = isSolid;
            IsTransparent = isTransparent;
            Animation = animation;

            if (id < 0 || id >= _tiles.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(id), $"Invalid tile ID: {id}");
            }

            if (_tiles[id] != null)
            {
                throw new ArgumentException(nameof(id), $"Duplicate tile ID: {id}");
            }

            _tiles[id] = this;
        }

        public static Tile GetTile(int id)
        {
            return _tiles[id];
        }
    }
}
