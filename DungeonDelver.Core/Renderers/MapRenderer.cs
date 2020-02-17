using DungeonDelver.Core.Data.Definitions;
using DungeonDelver.Core.Entities;
using DungeonDelver.Core.Extensions;
using DungeonDelver.Core.Util;
using DungeonDelver.Core.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Toolbox.Graphics;
using Toolbox.Graphics.Animations;

namespace DungeonDelver.Core.Renderers
{
    public class MapRenderer : Renderer
    {
        private readonly Map _map;
        private readonly LightingManager _lightingManager;

        private readonly Dictionary<string, AnimatedSprite> _tileAnimations = new Dictionary<string, AnimatedSprite>();

        public MapRenderer(Map map, LightingManager lightingManager, Camera camera) : base(camera)
        {
            _map = map;
            _lightingManager = lightingManager;

            // Find any tiles with animations.
            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    Tile tile = _map.GetTile(x, y);
                    if (!string.IsNullOrWhiteSpace(tile.Animation))
                    {
                        if (!_tileAnimations.ContainsKey(tile.Animation))
                        {
                            AnimatedSprite animation = Engine.Assets.GetAsset<Animation>(tile.Animation).NewAnimatedSprite();
                            _tileAnimations.Add(tile.Animation, animation);
                        }
                    }
                }
            }
        }

        public override void Update(float delta)
        {
            foreach (AnimatedSprite animation in _tileAnimations.Values)
            {
                animation.Update(delta);
            }
        }

        protected override void DoRender()
        {
            for (int x = Math.Max(0, BoundsX); x <= Math.Min(_map.Width - 1, BoundsWidth); x++)
            {
                for (int y = Math.Max(0, BoundsY); y <= Math.Min(_map.Height - 1, BoundsHeight); y++)
                {
                    if (_map.IsVisible(x, y) || Game.HideFov)
                    {
                        Tile tile = _map.GetTile(x, y);
                        Sprite tileSprite = tile.Sprite;

                        if (!string.IsNullOrWhiteSpace(tile.Animation))
                        {
                            tileSprite = _tileAnimations[tile.Animation].Sprite;
                        }

                        Color color = _lightingManager.GetLitColor(tile.Color.Blend(Color.SlateGray, 0.85f));
                     
                        Draw.Sprite(tileSprite, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), color);

                        Prop prop = _map.GetProp(x, y);
                        if (prop != null)
                        {
                            Draw.Sprite(prop.Sprite, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), _lightingManager.GetLitColor(prop.Color.Blend(Color.SlateGray, 0.85f)));
                        }
                    }
                    else if (_map.IsVisited(x, y) && !Game.HideFov)
                    {
                        Tile tile = _map.GetTile(x, y);
                        Sprite tileSprite = tile.Sprite;
                        Color color = Color.FromNonPremultiplied(50, 50, 50, 255).Darken(1f - _lightingManager.Brightness);

                        if (!string.IsNullOrWhiteSpace(tile.Animation))
                        {
                            tileSprite = _tileAnimations[tile.Animation].Sprite;
                        }

                        if (tileSprite != null)
                        {
                            Draw.Sprite(tileSprite, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), color);
                        }

                        Prop prop = _map.GetProp(x, y);
                        if (prop != null)
                        {
                            Draw.Sprite(prop.Sprite, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), color);
                        }
                    }

                }
            }

        }

    }
}
