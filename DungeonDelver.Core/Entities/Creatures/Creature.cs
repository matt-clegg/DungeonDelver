﻿using DungeonDelver.Core.Actions;
using DungeonDelver.Core.Ai;
using DungeonDelver.Core.Turns;
using DungeonDelver.Core.World;
using Toolbox.Graphics;
using Toolbox.Graphics.Animations;

namespace DungeonDelver.Core.Entities.Creatures
{
    public class Creature : Entity, ITurnable
    {
        public float RenderX { get; set; }
        public float RenderY { get; set; }
        public float RenderZ { get; set; }

        private readonly Energy _energy = new Energy();
        public int Speed { get; }

        public CreatureAi Ai { get; set; }

        public Race Race { get; }

        private readonly AnimatedSprite _animation;
        public override Sprite Sprite => _animation.Sprite;

        public int MaxHealth { get; protected set; }
        public int Health { get; protected set; }

        public Creature(Race race) : base(null, race.Color)
        {
            Race = race;
            _animation = race.Animation.NewAnimatedSprite();
            Speed = race.Speed;
            MaxHealth = Health = race.Health;
        }

        public override void Update(float delta)
        {
            _animation.Update(delta);
        }

        public bool CanTakeTurn()
        {
            return !ShouldRemove && _energy.CanTakeTurn;
        }

        public virtual bool IsWaitingForInput()
        {
            return false;
        }

        public bool GainEnergy()
        {
            return _energy.Gain(Speed);
        }

        public virtual bool FinishTurn()
        {
            if (ShouldRemove)
            {
                return true;
            }

            _energy.Spend();
            return true;
        }

        public BaseAction GetAction()
        {
            return OnGetAction();
        }

        protected virtual BaseAction OnGetAction()
        {
            return Ai.DecideNextAction();
        }

    }
}
