using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Toolbox.Graphics
{
    public class Sprite : GameTexture
    {
        public Rectangle Bounds { get; }
        public Vector2 Origin { get; }

        public Sprite(string name, Texture2D texture, Rectangle bounds, Vector2? origin = null) : base(name, texture)
        {
            Bounds = bounds;
            Origin = origin ?? Vector2.Zero;
        }

        public override int Width => Bounds.Width;
        public override int Height => Bounds.Height;
    }
}
