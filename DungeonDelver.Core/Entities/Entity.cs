using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Graphics;

namespace DungeonDelver.Core.Entities
{
    public abstract class Entity
    {
        public int X { get; set; }
        public int Y { get; set; }

        public virtual Sprite Sprite { get; }

        protected Entity(Sprite sprite)
        {
            Sprite = sprite;
        }

        public abstract void Update(float delta);
    }
}
