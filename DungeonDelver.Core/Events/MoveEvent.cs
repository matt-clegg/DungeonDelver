using DungeonDelver.Core.Entities.Creatures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonDelver.Core.Events
{
    public class MoveEvent : GameEvent
    {
        private readonly Creature _creature;
        private readonly float _moveSpeed;

        private readonly Vector2 _start;
        private readonly Vector2 _mid;
        private readonly Vector2 _end;

        private float _count;

        public MoveEvent(Creature creature, int newX, int newY, float moveSpeed)
        {
            _creature = creature;
            _moveSpeed = moveSpeed;

            _start = new Vector2(creature.X * Game.SpriteWidth, creature.Y * Game.SpriteHeight);
            _end = new Vector2(newX * Game.SpriteWidth, newY * Game.SpriteHeight);
            _mid = _start + (_end - _start) / 2 + new Vector2(0, -(Game.SpriteHeight / 2));
        }

        protected override bool Process(float delta)
        {
            if (_count < 1f)
            {
                _count += _moveSpeed * delta;

                Vector2 m1 = Vector2.Lerp(_start, _mid, _count);
                Vector2 m2 = Vector2.Lerp(_mid, _end, _count);
                Vector2 position = Vector2.Lerp(m1, m2, _count);

                _creature.RenderX = (int)position.X;
                _creature.RenderY = (int)position.Y;
            }

            return _count >= 1f;
        }
    }
}
