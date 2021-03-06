﻿using DungeonDelver.Core.Actions;
using DungeonDelver.Core.Entities.Creatures;
using System;

namespace DungeonDelver.Core.Ai
{
    public class MonsterAi : CreatureAi
    {
        static Random random = new Random();

        public MonsterAi(Creature creature) : base(creature)
        {
        }

        public override BaseAction DecideNextAction()
        {
            int x = random.Next(3) - 1;
            int y = random.Next(3) - 1;

            return new MoveAction(x, y);
        }

    }
}
