using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonDelver.Core.Scenes
{
    public abstract class Scene
    {
        public abstract void Input(Keys key);
        public abstract void Update(float delta);
        public abstract void Render(SpriteBatch batch);
    }
}
