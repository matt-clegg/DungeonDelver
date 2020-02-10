using DungeonDelver.Core.World;
using Microsoft.Xna.Framework.Graphics;
using Toolbox.Graphics;

namespace DungeonDelver.Core.Entities
{
    public abstract class Entity
    {
        private static int _nextId = 0;

        public int Id { get; }

        public int X { get; set; }
        public int Y { get; set; }

        public SpriteEffects SpriteEffect { get; set; }
        public virtual Sprite Sprite { get; }
        public bool ShouldRemove { get; protected set; }
        public Map Map { get; private set; }

        protected Entity(Sprite sprite)
        {
            Id = _nextId++;
            Sprite = sprite;
            ShouldRemove = false;
        }

        public abstract void Update(float delta);

        public virtual void Initialise(Map map, int x, int y)
        {
            Map = map;
            X = x;
            Y = y;
        }
    }
}
