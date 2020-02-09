using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Toolbox.Graphics
{
    public class Spritesheet : GameTexture
    {
        public Spritesheet(string name, Texture2D texture) : base(name, texture)
        {
        }

        public Sprite CutSprite(int x, int y, int width, int height, string name, Vector2? origin = null)
        {
            // TODO: INDEX TILES ON 8x8 GRID!!!
            var bounds = new Rectangle(x * 8, y * 8, width, height);
            return new Sprite(name, Texture, bounds, origin);
        }
    }
}
