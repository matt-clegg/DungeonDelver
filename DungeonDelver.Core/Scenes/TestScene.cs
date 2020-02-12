using DungeonDelver.Core.Actions;
using DungeonDelver.Core.Ai;
using DungeonDelver.Core.Entities.Creatures;
using DungeonDelver.Core.Events;
using DungeonDelver.Core.Pathfinding;
using DungeonDelver.Core.Turns;
using DungeonDelver.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Toolbox;
using Toolbox.Graphics;
using Toolbox.Graphics.Animations;

namespace DungeonDelver.Core.Scenes
{
    public class TestScene : Scene
    {
        private readonly Player _player;
        private readonly Map _map;

        private readonly GameEventManager _eventManager = new GameEventManager();
        private readonly TurnManager<Creature> _turnManager;
        private TurnResult _turnResult;

        private readonly Sprite _tileSelection;
        private readonly Sprite _dot;

        private readonly FieldOfView _fov;

        public TestScene()
        {
            _map = new Map(32, 32, 0);
            _fov = new FieldOfView(_map);

            Race playerRace = Engine.Assets.GetAsset<Race>("player");
            _player = new Player(playerRace);
            _map.Add(_player, 5, 5);

            //Race crabRace = Engine.Assets.GetAsset<Race>("crab");
            //for (int i = 0; i < 3; i++)
            //{
            //    Creature crab = new Creature(crabRace);
            //    new MonsterAi(crab);
            //    _map.Add(crab, 10, 10);
            //}

            _turnManager = new TurnManager<Creature>(_map.Creatures);

            _tileSelection = Engine.Assets.GetAsset<Sprite>("tile_selection");
            _dot = Engine.Assets.GetAsset<Sprite>("dot");
        }

        public override void Input(Keys key)
        {
            _player.Input(key);
        }

        private MouseState lastState;

        public override void Update(float delta)
        {

            MouseState state = Mouse.GetState();
            
            //if (state.LeftButton == ButtonState.Released && lastState.LeftButton == ButtonState.Pressed)
            //{
            //    int tileX = state.X / Engine.GameScale / Game.SpriteWidth;
            //    int tileY = state.Y / Engine.GameScale / Game.SpriteHeight;

            //    List<Point2D> points = AStar<Point2D>.FindPath(_map, new Point2D(_player.X, _player.Y), new Point2D(tileX, tileY), Heuristics.ManhattanDistance, false, true);
            //    int lastX = _player.X;
            //    int lastY = _player.Y;
            //    List<BaseAction> actions = new List<BaseAction>();
            //    foreach (Point2D point in points){
            //        int dx = point.X - lastX;
            //        int dy = point.Y - lastY;
            //        actions.Add(new MoveAction(dx, dy));
            //    }

            //    //_player.QueueActions(actions);
            //}

            _map.Update(delta);

            if (!_eventManager.HasEventsToProcess())
            {
                _turnResult = _turnManager.Process();
                _eventManager.AddEvents(_turnResult.Events);

                if (_turnResult.MadeProgress)
                {
                    System.Console.WriteLine("made progress");
                    _map.ClearFov();
                    _fov.RefreshVisibility(new Point2D(_player.X, _player.Y), 8);
                }
            }

            _eventManager.Update(delta);

            lastState = state;
        }

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
                        batch.Draw(tileSprite.Texture, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), tileSprite.Bounds, tile.Color);
                    }
                    else if (_map.IsVisited(x, y))
                    {
                        Tile tile = _map.GetTile(x, y);
                        Sprite tileSprite = tile.Sprite;
                        batch.Draw(tileSprite.Texture, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), tileSprite.Bounds, Color.FromNonPremultiplied(30, 30, 30, 255));
                    }

                }
            }

            foreach (Creature creature in _map.Creatures)
            {
                Sprite sprite = creature.Sprite;
                batch.Draw(sprite.Texture, new Vector2(creature.RenderX, creature.RenderY), sprite.Bounds, creature.Color, 0f, sprite.Origin, 1f, creature.SpriteEffect, 0);
            }

            MouseState state = Mouse.GetState();
            int tileX = ((state.X / Engine.GameScale) / Game.SpriteWidth) * Game.SpriteWidth;
            int tileY = ((state.Y / Engine.GameScale) / Game.SpriteHeight) * Game.SpriteHeight;

            batch.Draw(_tileSelection.Texture, new Vector2(tileX, tileY), _tileSelection.Bounds, Color.White);
        }

    }
}

