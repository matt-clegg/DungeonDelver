using DungeonDelver.Core.Ai;
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

        private SoundEffect rainSound;

        private SoundEffectInstance thunderInstance;

        public TestScene()
        {
            _random = new DotNetRandom();

            MapBuilder builder = new ForestBuilder(32, 32, 0);
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

            SoundEffect thunder = Engine.Instance.Content.Load<SoundEffect>("Audio/thunder_01");
            thunderInstance = thunder.CreateInstance();
            thunderInstance.Volume = 0.5f;
            

            rainSound = Engine.Instance.Content.Load<SoundEffect>("Audio/rain");
            SoundEffectInstance instance  =rainSound.CreateInstance();
            instance.Volume = 0.5f;
            instance.Play();

        }

        public override void Input(Keys key)
        {
            _player.Input(key);
        }

        private MouseState lastState;

        public override void Update(float delta)
        {
            if(_random.NextDouble() < 0.01)
            {
                brightness = 2f;

                thunderInstance.Play();
            }

            if(brightness > minBrightness)
            {
                brightness *= 0.9f;

                if(_random.NextDouble() < 0.04)
                {
                    brightness += 0.3f;
                }

                if (brightness < minBrightness)
                {
                    brightness = minBrightness;
                }
            }

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

        float brightness = 0.3f;

        float minBrightness = 0.3f;

        public override void Render(SpriteBatch batch)
        {
           
            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    if (_map.IsVisible(x, y))
                    {
                        Tile tile = _map.GetTile(x, y);
                        Sprite tileSprite = tile.Sprite;
                        batch.Draw(tileSprite.Texture, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), tileSprite.Bounds, tile.Color.Blend(Color.SlateGray, 0.9f).Darken(1f - brightness));
                    }
                    else if (_map.IsVisited(x, y))
                    {
                        Tile tile = _map.GetTile(x, y);
                        Sprite tileSprite = tile.Sprite;
                        batch.Draw(tileSprite.Texture, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), tileSprite.Bounds, Color.FromNonPremultiplied(30, 30, 30, 255).Darken(1f - brightness));
                    }

                }
            }

            foreach (Creature creature in _map.Creatures)
            {
                //if(!_map.IsVisible(creature.RenderX / Game.SpriteWidth, creature.RenderY / Game.SpriteHeight))
                //{
                //    continue;
                //}

                Sprite sprite = creature.Sprite;
                batch.Draw(sprite.Texture, new Vector2(creature.RenderX, creature.RenderY), sprite.Bounds, creature.Color.Darken(1f - brightness), 0f, sprite.Origin, 1f, creature.SpriteEffect, 0);
            }

            for (int y = 0; y < Engine.GameHeight / Game.SpriteHeight; y++)
            {
                for (int x = 0; x < Engine.GameWidth / Game.SpriteWidth; x++)
                {
                    if (_map.IsVisible(x, y))
                    {
                        batch.Draw(_rain.Sprite.Texture, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), _rain.Sprite.Bounds, Color.DarkSlateGray.Darken(1f - brightness) * 0.9f);
                    }
                    else if (_map.IsVisited(x, y))
                    {
                        batch.Draw(_rain.Sprite.Texture, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), _rain.Sprite.Bounds, Color.FromNonPremultiplied(79, 79, 79, 255).Darken(1f - brightness) * 0.9f);
                    }
                }
            }


            MouseState state = Mouse.GetState();
            int tileX = ((state.X / Engine.GameScale) / Game.SpriteWidth) * Game.SpriteWidth;
            int tileY = ((state.Y / Engine.GameScale) / Game.SpriteHeight) * Game.SpriteHeight;

            batch.Draw(_tileSelection.Texture, new Vector2(tileX, tileY), _tileSelection.Bounds, Color.White);

        }

    }
}

