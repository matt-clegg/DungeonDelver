using DungeonDelver.Core.Util;
using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Graphics;

namespace DungeonDelver.Core.Data
{
    public static class FontLoader
    {
        public static void Load()
        {
            Dictionary<char, Sprite> glyphs = new Dictionary<char, Sprite>();

            char[] characters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

            foreach (char character in characters)
            {
                LoadSprite(character, glyphs);
            }

            Font font = new Font(glyphs);
            Engine.Assets.AddAsset("font_regular", font);
        }

        private static void LoadSprite(char character, Dictionary<char, Sprite> glyphs)
        {
            glyphs.Add(character, Engine.Assets.GetAsset<Sprite>(character.ToString()));
        }
    }
}
