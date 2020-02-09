﻿using DungeonDelver.Core.Scenes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DungeonDelver.Core
{
    public class Game
    {
        public const int SpriteWidth = 16;
        public const int SpriteHeight = 24;

        public Scene Scene { get; set; }

        public Game()
        {
            Scene = new TestScene();
        }

        public void Input(Keys key)
        {
            Scene?.Input(key);
        }

        public void Update(float delta)
        {
            Scene?.Update(delta);
        }

        public void Render(SpriteBatch batch)
        {
            Scene?.Render(batch);
        }
    }
}
