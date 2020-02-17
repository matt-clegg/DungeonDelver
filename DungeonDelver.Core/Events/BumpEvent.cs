using DungeonDelver.Core.Entities.Creatures;
using Microsoft.Xna.Framework;

namespace DungeonDelver.Core.Events
{
    public class BumpEvent : GameEvent
    {
        private readonly Creature _creature;
        private Vector2 _start;
        private Vector2 _mid;
        private Vector2 _end;

        private readonly float _moveSpeed;
        private float _count;

        public BumpEvent(int dx, int dy, float moveSpeed, Creature creature) : base(true)
        {
            _creature = creature;
            _moveSpeed = moveSpeed;

            _start = new Vector2(creature.RenderX, creature.RenderY);
            _end = new Vector2(creature.RenderX, creature.RenderY);
            _mid = new Vector2(creature.RenderX + (dx * Game.SpriteWidth * 0.6f), creature.RenderY + (dy * Game.SpriteHeight * 0.6f));

            // Ensure the camera does not track the player when we are bumping
            if (_creature is Player player)
            {
                player.UnlockCamera = true;
            }
        }

        protected override bool Process(float delta)
        {
            if (_count < 1f)
            {
                _count += _moveSpeed * delta;
                if (_count > 1)
                {
                    _count = 1f;
                }

                Vector2 m1 = Vector2.Lerp(_start, _mid, _count);
                Vector2 m2 = Vector2.Lerp(_mid, _end, _count);
                Vector2 groundPosition = Vector2.Lerp(m1, m2, _count);

                _creature.RenderX = groundPosition.X;
                _creature.RenderY = groundPosition.Y;
            }

            bool result = _count >= 1f;
            if (result && _creature is Player player)
            {
                player.UnlockCamera = false;
            }

            return result;
        }
    }
}
