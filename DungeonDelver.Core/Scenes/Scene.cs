using Microsoft.Xna.Framework.Input;

namespace DungeonDelver.Core.Scenes
{
    public abstract class Scene
    {
        public abstract void Input(Keys key);
        public abstract void Update(float delta);
        public abstract void Render();
    }
}
