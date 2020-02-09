using Microsoft.Xna.Framework.Graphics;

namespace Toolbox.Graphics
{
    public abstract class GameTexture
    {
        public string Name { get; }
        public Texture2D Texture { get; }

        public virtual int Width => Texture.Width;
        public virtual int Height => Texture.Height;

        public GameTexture(string name, Texture2D texture)
        {
            Name = name;
            Texture = texture;
        }
    }
}
