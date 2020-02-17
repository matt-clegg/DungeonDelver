using System.Collections.Generic;
using Toolbox.Graphics;

namespace DungeonDelver.Core.Util
{
    public class Font
    {
        private readonly Dictionary<char, Sprite> _glyphs;

        public Font(Dictionary<char, Sprite> glyphs)
        {
            _glyphs = glyphs;
        }

        public Sprite GetSprite(char character)
        {
            if(_glyphs.TryGetValue(character, out Sprite sprite))
            {
                return sprite;
            }
            return null;
        }
    }
}
