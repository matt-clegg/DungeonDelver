using DungeonDelver.Core.Data.Definitions;
using DungeonDelver.Core.Extensions;
using DungeonDelver.Core.Util;
using DungeonDelver.Core.World;
using Microsoft.Xna.Framework;
using Toolbox.Graphics.Animations;

namespace DungeonDelver.Core.Renderers
{
    public class EffectsRenderer : Renderer
    {
        private readonly Map _map;
        private readonly LightingManager _lightingManager;

        private readonly AnimatedSprite _rain;

        public EffectsRenderer(Map map, LightingManager lightingManager, Camera camera) : base(camera)
        {
            _map = map;
            _lightingManager = lightingManager;

            _rain = Engine.Assets.GetAsset<Animation>("rain").NewAnimatedSprite();
        }

        public override void Update(float delta)
        {
            _rain.Update(delta);
        }

        protected override void DoRender()
        {
            if (!Game.ShowRain)
            {
                return;
            }

            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    if(!InCameraBounds(x, y))
                    {
                        continue;
                    }

                    if (_map.IsVisible(x, y) || Game.HideFov)
                    {
                        Draw.Sprite(_rain.Sprite, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), _lightingManager.GetLitColor(Color.DarkSlateGray * 0.9f, Color.Black));
                    }
                    else if (_map.IsVisited(x, y) && !Game.HideFov)
                    {
                        Draw.Sprite(_rain.Sprite, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), Color.FromNonPremultiplied(79, 79, 79, 255).Darken(1f - _lightingManager.Brightness) * 0.7f);
                    }
                }
            }
        }
    }
}
