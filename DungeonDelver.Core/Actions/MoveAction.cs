using DungeonDelver.Core.Entities.Creatures;
using DungeonDelver.Core.Turns;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonDelver.Core.Actions
{
    public class MoveAction : BaseAction
    {
        private readonly int _x;
        private readonly int _y;

        public MoveAction(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public override ActionResult Perform(ITurnable turnable)
        {
            Creature creature = turnable as Creature;

            if (_x != 0)
            {
                creature.SpriteEffect = _x > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            }

            int newX = creature.X + _x;
            int newY = creature.Y + _y;


            if (creature.Map.InBounds(newX, newY) && creature.Map.GetTile(newX, newY).IsSolid)
            {
                return Succeed();
            }

            Creature other = creature.Map.GetCreature(newX, newY);

            if(other != null)
            {
                return Succeed();
            }

            creature.X += _x;
            creature.Y += _y;


            return Succeed();
        }
    }
}
