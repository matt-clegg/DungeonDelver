using DungeonDelver.Core.Ai;
using DungeonDelver.Core.Audio;
using DungeonDelver.Core.Data.Definitions;
using DungeonDelver.Core.Entities.Creatures;
using DungeonDelver.Core.Events;
using DungeonDelver.Core.Extensions;
using DungeonDelver.Core.Turns;
using DungeonDelver.Core.World;
using DungeonDelver.Core.World.Builders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Toolbox;
using Toolbox.Graphics;
using Toolbox.Graphics.Animations;
using Toolbox.Rng;

namespace DungeonDelver.Core.Scenes
{
    public class TestScene : Scene
    {
        private readonly IRandom _random;

        private readonly Player _player;
        private readonly Map _map;

        private readonly GameEventManager _eventManager = new GameEventManager();
        private readonly TurnManager<Creature> _turnManager;
        private TurnResult _turnResult;

        private readonly Sprite _tileSelection;
        private readonly Sprite _dot;
        private readonly AnimatedSprite _rain;

        private readonly FieldOfView _fov;

        private readonly SoundEffectManager _soundEffectManager = new SoundEffectManager();

        //private SoundEffect rainSound;

        //private SoundEffectInstance thunderInstance;

        private const bool HideFov = false;
        private const bool ShowRain = true;

        private readonly Dictionary<string, AnimatedSprite> _tileAnimations = new Dictionary<string, AnimatedSprite>();

        public TestScene()
        {
            _random = new DotNetRandom();

            MapBuilder builder = new ForestBuilder(32, 12, 0, _random.Next());
            builder.Build();
            _map = builder.Map;
            _fov = new FieldOfView(_map);

            Race playerRace = Engine.Assets.GetAsset<Race>("player");
            _player = new Player(playerRace);
            _map.Add(_player, 5, 5);

            Race crabRace = Engine.Assets.GetAsset<Race>("crab");
            for (int i = 0; i < 3; i++)
            {
                Creature crab = new Creature(crabRace);
                new MonsterAi(crab);
                _map.Add(crab, 10, 10);
            }

            _turnManager = new TurnManager<Creature>(_map.Creatures);

            _tileSelection = Engine.Assets.GetAsset<Sprite>("tile_selection");
            _dot = Engine.Assets.GetAsset<Sprite>("dot");

            _rain = Engine.Assets.GetAsset<Animation>("rain").NewAnimatedSprite();

            _soundEffectManager.LoadSoundEffect("rain_looped");
            _soundEffectManager.LoadSoundEffect("thunder_01");

            _soundEffectManager.Play("rain_looped", 0.4f, loop: true);

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

        public override void Input(Keys key)
        {
            _player.Input(key);
        }

        private MouseState lastState;

        public override void Update(float delta)
        {
            _soundEffectManager.Update();

            foreach (AnimatedSprite animation in _tileAnimations.Values)
            {
                animation.Update(delta);
            }

            //if(_random.NextDouble() < 0.001)
            //{
            //    brightness = (float)_random.NextDouble(maxBrightness - 0.2f, maxBrightness);

            //    //thunderInstance.Play();
            //    _soundEffectManager.Play("thunder_01", (float)_random.NextDouble(0.05, 0.25), (float)_random.NextDouble(-0.3, 0.3), (float)_random.NextDouble(-0.2, 0.1));
            //}

            //if(brightness > minBrightness)
            //{
            //    brightness *= 0.9f;

            //    if(_random.NextDouble() < 0.04)
            //    {
            //        brightness += (float)_random.NextDouble(0.1, 0.3);
            //    }

            //    if (brightness < minBrightness)
            //    {
            //        brightness = minBrightness;
            //    }
            //}

            MouseState state = Mouse.GetState();

            _rain.Update(delta);
            _map.Update(delta);

            if (!_eventManager.HasEventsToProcess())
            {
                _turnResult = _turnManager.Process();
                _eventManager.AddEvents(_turnResult.Events);

                if (_turnResult.MadeProgress)
                {
                    _map.ClearFov();
                    _fov.RefreshVisibility(new Point2D(_player.X, _player.Y), 8);
                }
            }

            _eventManager.Update(delta);

            lastState = state;
        }

        float brightness = 1f;

        float minBrightness = 0.3f;
        float maxBrightness = 2f;

        public override void Render(SpriteBatch batch)
        {
           
            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    if (_map.IsVisible(x, y) || HideFov)
                    {
                        Tile tile = _map.GetTile(x, y);
                        Sprite tileSprite = tile.Sprite;

                        if (!string.IsNullOrWhiteSpace(tile.Animation))
                        {
                            tileSprite = _tileAnimations[tile.Animation].Sprite;
                        }

                        batch.Draw(tileSprite.Texture, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), tileSprite.Bounds, LitColor(tile.Color.Blend(Color.SlateGray, 0.85f), Color.White));
                    }
                    else if (_map.IsVisited(x, y) && !HideFov)
                    {
                        Tile tile = _map.GetTile(x, y);
                        Sprite tileSprite = tile.Sprite;
                        if (tileSprite != null)
                        {
                            batch.Draw(tileSprite.Texture, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), tileSprite.Bounds, Color.FromNonPremultiplied(30, 30, 30, 255).Darken(1f - brightness));
                        }
                    }

                }
            }

            foreach (Creature creature in _map.Creatures)
            {
                if (!_map.IsVisible(creature.RenderX / Game.SpriteWidth, creature.RenderY / Game.SpriteHeight))
                {
                    continue;
                }

                Sprite sprite = creature.Sprite;
                batch.Draw(sprite.Texture, new Vector2(creature.RenderX, creature.RenderY), sprite.Bounds, LitColor(creature.Color, Color.White), 0f, sprite.Origin, 1f, creature.SpriteEffect, 0);
            }

            if (ShowRain)
            {
                for (int y = 0; y < Engine.GameHeight / Game.SpriteHeight; y++)
                {
                    for (int x = 0; x < Engine.GameWidth / Game.SpriteWidth; x++)
                    {
                        if (_map.IsVisible(x, y) || HideFov)
                        {
                            batch.Draw(_rain.Sprite.Texture, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), _rain.Sprite.Bounds, LitColor(Color.DarkSlateGray * 0.9f, Color.Black));
                        }
                        else if (_map.IsVisited(x, y) && !HideFov)
                        {
                            batch.Draw(_rain.Sprite.Texture, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), _rain.Sprite.Bounds, Color.FromNonPremultiplied(79, 79, 79, 255).Darken(1f - brightness) * 0.9f);
                        }
                    }
                }
            }

            MouseState state = Mouse.GetState();
            int tileX = ((state.X / Engine.GameScale) / Game.SpriteWidth) * Game.SpriteWidth;
            int tileY = ((state.Y / Engine.GameScale) / Game.SpriteHeight) * Game.SpriteHeight;

            batch.Draw(_tileSelection.Texture, new Vector2(tileX, tileY), _tileSelection.Bounds, Color.White);
        }

        private Color LitColor(Color color, Color flashColor)
        {
            Color newColor = color.Darken(1f - brightness);
            //if(brightness > minBrightness)
            //{
            //    float flashAmount = (brightness - minBrightness) / (maxBrightness - minBrightness);
            //    newColor = newColor.Blend(flashColor, 1f - flashAmount);
            //}

            return newColor;
        }

    }
}

