using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Graphics;
using Toolbox.Graphics.Animations;

namespace DungeonDelver.Core.Entities
{
    public class Creature : Entity
    {
        private readonly AnimatedSprite _animation;

        public override Sprite Sprite => _animation.Sprite;

        public Creature(AnimatedSprite animation) : base(null)
        {
            _animation = animation;
        }

        public override void Update(float delta)
        {
            Console.WriteLine(delta);
            _animation.Update(delta);
        }
    }
}
