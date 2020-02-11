using DungeonDelver.Core.Actions;
using DungeonDelver.Core.Ai;
using DungeonDelver.Core.Turns;
using DungeonDelver.Core.World;
using Toolbox.Graphics;
using Toolbox.Graphics.Animations;

namespace DungeonDelver.Core.Entities.Creatures
{
    public class Creature : Entity, ITurnable
    {
        public int RenderX { get; set; }
        public int RenderY { get; set; }

        private readonly Energy _energy = new Energy();
        public int Speed { get; }

        public CreatureAi Ai { get; set; }

        public Race Race { get; }

        private readonly AnimatedSprite _animation;
        public override Sprite Sprite => _animation.Sprite;

        public Creature(Race race) : base(null, race.Color)
        {
            Race = race;
            _animation = race.Animation.NewAnimatedSprite();
            Speed = race.Speed;
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

        public void FinishTurn()
        {
            if (ShouldRemove)
            {
                return;
            }

            _energy.Spend();
        }

        public BaseAction GetAction()
        {
            return OnGetAction();
        }

        protected virtual BaseAction OnGetAction()
        {
            return Ai.DecideNextAction();
        }

        public virtual bool IsTurning()
        {
            return false;
        }

    }
}
