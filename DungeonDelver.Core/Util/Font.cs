using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonDelver.Core.Util
{
    public class Font
    {
        public Texture2D Texture { get; }
        public int CharWidth { get; }
        public int CharHeight { get; }

        public int Width => Texture.Width;
        public int Height => Texture.Height;

        private readonly Rectangle[] _sources;

        public int Characters => _sources.Length;

        public Font(Texture2D texture, int charWidth, int charHeight)
        {
            Texture = texture;
            CharWidth = charWidth;
            CharHeight = charHeight;

            int widthInChars = Width / CharWidth;
            int heightInChars = Height / CharHeight;

            _sources = new Rectangle[widthInChars * heightInChars];

            for (int i = 0; i < _sources.Length; i++)
            {
                int x = (i % widthInChars) * CharWidth;
                int y = (i / widthInChars) * CharHeight;

                _sources[i] = new Rectangle(x, y, CharWidth, CharHeight);
            }
        }

        public Rectangle GetBounds(char glyph)
        {
            return _sources[glyph];
        }
    }
}
