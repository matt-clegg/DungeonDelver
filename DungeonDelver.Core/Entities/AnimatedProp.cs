using DungeonDelver.Core.Data.Definitions;
using Microsoft.Xna.Framework;
using Toolbox.Graphics;
using Toolbox.Graphics.Animations;

namespace DungeonDelver.Core.Entities
{
    public class AnimatedProp : Prop
    {
        private readonly AnimatedSprite _animation;

        public override Sprite Sprite => _animation.Sprite;

        public AnimatedProp(string animation, Color? color = null) : base(null, color)
        {
            _animation = Engine.Assets.GetAsset<Animation>(animation).NewAnimatedSprite();
        }

        public override void Update(float delta)
        {
            _animation.Update(delta);
        }
    }
}
