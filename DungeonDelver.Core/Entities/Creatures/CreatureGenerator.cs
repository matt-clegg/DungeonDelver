using DungeonDelver.Core.Ai;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonDelver.Core.Entities.Creatures
{
    public static class CreatureGenerator
    {
        public static Creature NewCreature(string name)
        {
            Race race = Engine.Assets.GetAsset<Race>(name);
            Creature creature = new Creature(race);
            new MonsterAi(creature);
            return creature;
        }
    }
}
