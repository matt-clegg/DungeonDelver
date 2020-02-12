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
        private readonly float _bounceHeight;

        private readonly int _startX;
        private readonly int _startY;
        private readonly int _newX;
        private readonly int _newY;

        private Vector2 _start;
        private Vector2 _mid;
        private Vector2 _end;

        private float _count;

        public MoveEvent(Creature creature, int startX, int startY, int newX, int newY, float moveSpeed, float bounceHeight) : base(true)
        {
            _creature = creature;
            _moveSpeed = moveSpeed;
            _bounceHeight = bounceHeight;

            _startX = startX;
            _startY = startY;
            _newX = newX;
            _newY = newY;

            Id = creature.Id;
        }

        protected override void OnStart()
        {
            Console.WriteLine("starting new event to move to " + _newX + "," + _newY);
            _start = new Vector2(_startX * Game.SpriteWidth, _startY * Game.SpriteHeight);
            _end = new Vector2(_newX * Game.SpriteWidth, _newY * Game.SpriteHeight);
            _mid = _start + (_end - _start) / 2 + new Vector2(0, -(Game.SpriteHeight * _bounceHeight));
        }

        protected override bool Process(float delta)
        {
            if (_count < 1f)
            {
                _count += _moveSpeed * delta;
                if(_count > 1)
                {
                    _count = 1f;
                }

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
