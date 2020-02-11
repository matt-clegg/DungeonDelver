using DungeonDelver.Core.Data.Definitions;
using Microsoft.Xna.Framework;

namespace DungeonDelver.Core.Entities.Creatures
{
    public class Race
    {
        public string Name { get; }
        public Animation Animation { get; }
        public Color Color { get; }
        public int Speed { get; }

        public Race(string name, Animation animation, Color color, int speed)
        {
            Name = name;
            Animation = animation;
            Color = color;
            Speed = speed;
        }
    }
}
