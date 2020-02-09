using DungeonDelver.Core.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Toolbox.Graphics;
using Toolbox.Graphics.Animations;

namespace DungeonDelver.Core.Scenes
{
    public class TestScene : Scene
    {
        public Creature creature;

        public TestScene()
        {
            AnimatedSprite animation = Engine.Assets.GetAsset<AnimatedSprite>("player");
            creature = new Creature(animation);
            creature.X = 5;
            creature.Y = 5;
        }

        public override void Input(Keys key)
        {

        }

        public override void Render(SpriteBatch batch)
        {
            Sprite sprite = creature.Sprite;
            batch.Draw(sprite.Texture, new Vector2(creature.X * Game.SpriteWidth, creature.Y * Game.SpriteHeight), sprite.Bounds, Color.White);
        }

        public override void Update(float delta)
        {
            creature.Update(delta);
        }
    }
}
