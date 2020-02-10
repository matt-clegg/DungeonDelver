using DungeonDelver.Core.Entities.Creatures;
using DungeonDelver.Core.Turns;
using DungeonDelver.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Toolbox.Graphics;
using Toolbox.Graphics.Animations;

namespace DungeonDelver.Core.Scenes
{
    public class TestScene : Scene
    {
        private readonly Player _player;
        private readonly Map _map;

        private readonly TurnManager<Creature> _turnManager;

        public TestScene()
        {
            _map = new Map(32, 32, 0);

            AnimatedSprite animation = Engine.Assets.GetAsset<AnimatedSprite>("player");
            _player = new Player(animation, Energy.NormalSpeed);
            _map.Add(_player, 5, 5);

            _turnManager = new TurnManager<Creature>(_map.Creatures);
        }

        public override void Input(Keys key)
        {
            _player.Input(key);
        }

        public override void Update(float delta)
        {
            _map.Update(delta);
            _turnManager.Process();
        }

        public override void Render(SpriteBatch batch)
        {
            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    Tile tile = _map.GetTile(x, y);
                    Sprite tileSprite = tile.Sprite;
                    batch.Draw(tileSprite.Texture, new Vector2(x * Game.SpriteWidth, y * Game.SpriteHeight), tileSprite.Bounds, tile.Color);
                }
            }

            Sprite sprite = _player.Sprite;
            //batch.Draw(sprite.Texture, new Vector2(_player.X * Game.SpriteWidth, _player.Y * Game.SpriteHeight), sprite.Bounds, Color.White, 0, Vector2.Zero, _player.SpriteEffect, 0);
            batch.Draw(sprite.Texture, new Vector2(_player.X * Game.SpriteWidth, _player.Y * Game.SpriteHeight), sprite.Bounds, Color.White, 0f, sprite.Origin, 1f, _player.SpriteEffect, 0);
        }

    }
}
