using Toolbox.Graphics.Animations;

namespace DungeonDelver.Core.Data.Definitions
{
    public class Animation
    {
        private readonly float _frameDuration;
        private readonly AnimationFrame[] _frames;

        public Animation(float frameDuration, AnimationFrame[] frames)
        {
            _frameDuration = frameDuration;
            _frames = frames;
        }

        public AnimatedSprite NewAnimatedSprite()
        {
            return new AnimatedSprite(_frameDuration, _frames);
        }
    }
}
