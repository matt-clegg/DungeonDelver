using DungeonDelver.Core.Actions;
using DungeonDelver.Core.Util;
using Microsoft.Xna.Framework.Input;

namespace DungeonDelver.Core.Entities.Creatures
{
    public class Player : Creature
    {
        private BaseAction _nextAction;

        public Player(Race race) : base(race)
        {
        }

        public void Input(Keys key)
        {
            if (Controls.North.IsPressed(key)) SetNextAction(new MoveAction(0, -1));
            else if (Controls.South.IsPressed(key)) SetNextAction(new MoveAction(0, 1));
            else if (Controls.East.IsPressed(key)) SetNextAction(new MoveAction(1, 0));
            else if (Controls.West.IsPressed(key)) SetNextAction(new MoveAction(-1, 0));
        }

        private void SetNextAction(BaseAction action)
        {
            _nextAction = action;
        }

        public override bool IsWaitingForInput()
        {
            return _nextAction == null;
        }

        protected override BaseAction OnGetAction()
        {
            BaseAction action = _nextAction;
            _nextAction = null;
            return action;
        }
    }
}
