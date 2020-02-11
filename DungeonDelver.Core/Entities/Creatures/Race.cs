using DungeonDelver.Core.Data.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Graphics.Animations;

namespace DungeonDelver.Core.Entities.Creatures
{
    public class Race
    {
        public string Name { get; }
        public Animation Animation { get; }
        public int Speed { get; }

        public Race(string name, Animation animation, int speed)
        {
            Name = name;
            Animation = animation;
            Speed = speed;
        }
    }
}
