﻿using DungeonDelver.Core.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Toolbox.Graphics;

namespace DungeonDelver.Core.Util
{
    public static class Draw
    {
        private const float DefaultThickness = 1f;

        private static Rectangle _rect;

        public static Renderer Renderer { get; internal set; }
        public static SpriteBatch SpriteBatch { get; private set; }

        public static Texture2D Pixel { get; set; }

        public static int SpriteDraws = 0;

        public static void UseDebugPixelTexture(GraphicsDevice graphicsDevice)
        {
            Pixel = new Texture2D(graphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
        }

        public static void Sprite(Sprite sprite, Vector2 position, Color color, SpriteEffects spriteEffect = SpriteEffects.None)
        {
            SpriteBatch.Draw(sprite.Texture, position, sprite.Bounds, color, 0, Vector2.Zero, 1f, spriteEffect, 0);
            SpriteDraws++;
        }

        internal static void Initialize(GraphicsDevice graphicsDevice)
        {
            SpriteBatch = new SpriteBatch(graphicsDevice);
            UseDebugPixelTexture(graphicsDevice);
        }
    }
}
