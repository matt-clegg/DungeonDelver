using System;
using System.Collections.Generic;
using System.Text;
using DungeonDelver.Core.Entities.Creatures;
using DungeonDelver.Core.Util;
using DungeonDelver.Core.World;
using Microsoft.Xna.Framework;
using Toolbox.Graphics;

namespace DungeonDelver.Core.Renderers
{
    public class CreatureRenderer : Renderer
    {
        private readonly IReadOnlyList<Creature> _creatures;
        private readonly Map _map;
        private readonly LightingManager _lightingManager;

        public CreatureRenderer(IReadOnlyList<Creature> creatures, Map map, LightingManager lightingManager, Camera camera) : base(camera)
        {
            _creatures = creatures;
            _map = map;
            _lightingManager = lightingManager;
        }

        protected override void DoRender()
        {
            foreach (Creature creature in _creatures)
            {
                if(!InCameraBounds(creature.X, creature.Y))
                {
                    continue;
                }

                if (!Game.HideFov && !_map.IsVisible(creature.X, creature.Y))
                {
                    continue;
                }

                Sprite sprite = creature.Sprite;
                Draw.Sprite(sprite, new Vector2(creature.RenderX, creature.RenderY - creature.RenderZ), _lightingManager.GetLitColor(creature.Color), creature.SpriteEffect);
                //batch.Draw(sprite.Texture, new Vector2(creature.RenderX, creature.RenderY), sprite.Bounds, LitColor(creature.Color, Color.White), 0f, sprite.Origin, 1f, creature.SpriteEffect, 0);
            }
        }

        private bool InCameraBounds(int x, int y)
        {
            return x >= BoundsX && y >= BoundsY && x <= BoundsWidth && y <= BoundsHeight;
        }
    }
}
