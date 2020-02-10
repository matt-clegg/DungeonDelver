using DungeonDelver.Core.Actions;
using DungeonDelver.Core.Entities.Creatures;

namespace DungeonDelver.Core.Ai
{
    public abstract class CreatureAi
    {
        protected Creature Creature { get; }

        protected CreatureAi(Creature creature)
        {
            Creature = creature;
            Creature.Ai = this;
        }

        public abstract BaseAction DecideNextAction();
    }
}
