using DungeonDelver.Core.Audio;
using DungeonDelver.Core.Entities.Creatures;
using DungeonDelver.Core.Events;
using DungeonDelver.Core.Renderers;
using DungeonDelver.Core.Turns;
using DungeonDelver.Core.Util;
using DungeonDelver.Core.World;
using DungeonDelver.Core.World.Builders;
using Microsoft.Xna.Framework;
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

        private readonly MapRenderer _mapRenderer;
        private readonly CreatureRenderer _creatureRenderer;
        private readonly EffectsRenderer _effectsRenderer;

        private readonly Camera _camera;
        private readonly Player _player;
        private readonly Map _map;
        private readonly FieldOfView _fov;

        private readonly LightingManager _lightingManager = new LightingManager(Color.White);
        private readonly GameEventManager _eventManager = new GameEventManager();
        private readonly SoundEffectManager _soundEffectManager = new SoundEffectManager();

        private readonly TurnManager<Creature> _turnManager;
        private TurnResult _turnResult;

        private readonly Sprite _tileSelection;

        private readonly Dictionary<string, AnimatedSprite> _tileAnimations = new Dictionary<string, AnimatedSprite>();

        public TestScene()
        {
            _random = new DotNetRandom();

            MapBuilder builder = new ForestBuilder(64, 64, 0, _random.Next());
            builder.Build();
            _map = builder.Map;
            _fov = new FieldOfView(_map);

            Race playerRace = Engine.Assets.GetAsset<Race>("player");
            _player = new Player(playerRace);
            _map.Add(_player, 32, 32);

            for (int i = 0; i < 10; i++)
            {
                Creature frog = CreatureGenerator.NewCreature("frog");
                _map.Add(frog, _map.RandomEmptyPoint(_random));
            }

            for (int i = 0; i < 10; i++)
            {
                Creature rat = CreatureGenerator.NewCreature("rat");
                _map.Add(rat, _map.RandomEmptyPoint(_random));
            }

            _turnManager = new TurnManager<Creature>(_map.Creatures);

            _tileSelection = Engine.Assets.GetAsset<Sprite>("tile_selection");

            _soundEffectManager.LoadSoundEffect("rain_looped");
            _soundEffectManager.LoadSoundEffect("thunder_01");

            _soundEffectManager.Play("rain_looped", 0.4f, loop: true);

            _camera = new Camera();
            _camera.Zoom = 1f;

            int w = Engine.Width / 8;
            int h = Engine.Height / 8;

            _camera.Origin = new Vector2(Engine.Width / 2, Engine.Height / 2);
            _camera.X = _player.RenderX;
            _camera.Y = _player.RenderY;


            _mapRenderer = new MapRenderer(_map, _lightingManager, _camera);
            _creatureRenderer = new CreatureRenderer(_map.Creatures, _map, _lightingManager, _camera);
            _effectsRenderer = new EffectsRenderer(_map, _lightingManager, _camera);
        }

        public override void Input(Keys key)
        {
            _player.Input(key);
        }

        public override void Update(float delta)
        {
            _camera.Approach(new Vector2(_player.RenderX, _player.RenderY), 0.25f);

            _soundEffectManager.Update();
            _lightingManager.Update(delta);

            _mapRenderer.Update(delta);
            _effectsRenderer.Update(delta);

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
        }

        public override void Render()
        {
            _mapRenderer.Render();
            _creatureRenderer.Render();
            _effectsRenderer.Render();

            //MouseState state = Mouse.GetState();
            //int tileX = ((state.X / Engine.GameScale) / Game.SpriteWidth) * Game.SpriteWidth;
            //int tileY = ((state.Y / Engine.GameScale) / Game.SpriteHeight) * Game.SpriteHeight;

            //Draw.Sprite(_tileSelection, new Vector2(tileX, tileY), Color.White);
            //batch.Draw(_tileSelection.Texture, new Vector2(tileX, tileY), _tileSelection.Bounds, Color.White);
        }
    }
}

