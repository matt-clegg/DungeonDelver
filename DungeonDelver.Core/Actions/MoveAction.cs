using DungeonDelver.Core.Entities.Creatures;
using DungeonDelver.Core.Events;
using DungeonDelver.Core.Turns;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DungeonDelver.Core.Actions
{
    public class MoveAction : BaseAction
    {
        public int X { get; }
        public int Y { get; }

        public MoveAction(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override ActionResult Perform(ITurnable turnable, TurnResult result)
        {
            Creature creature = turnable as Creature;

            if (X != 0)
            {
                creature.SpriteEffect = X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            }

            int newX = creature.X + X;
            int newY = creature.Y + Y;

            if (!creature.Map.InBounds(newX, newY) || creature.Map.GetTile(newX, newY).IsSolid)
            {
                return Succeed();
            }

            Creature other = creature.Map.GetCreature(newX, newY);

            if (other != null)
            {
                return Succeed();
            }

            result.AddEvent(new MoveEvent(creature, creature.X, creature.Y, newX, newY, 4f, 0.3f));
            creature.X += X;
            creature.Y += Y;

            return Succeed();
        }
    }
}

