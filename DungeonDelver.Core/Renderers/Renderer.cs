using DungeonDelver.Core.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonDelver.Core.Renderers
{
    public abstract class Renderer
    {
        public BlendState BlendState { get; set; }
        public SamplerState SamplerState { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public RasterizerState RasterizerState { get; set; }
        public SpriteSortMode SortMode { get; set; }

        public Effect Effect { get; set; }
        public Camera Camera { get; set; }

        public Matrix? Matrix { get; set; }

        public Renderer(Camera camera)
        {
            BlendState = BlendState.AlphaBlend;
            SamplerState = SamplerState.PointClamp;
            DepthStencilState = DepthStencilState.None;
            RasterizerState = RasterizerState.CullNone;
            SortMode = SpriteSortMode.Immediate;
            Camera = camera;
        }

        protected abstract void DoRender();

        public virtual void Update(float delta) { }

        public void Render()
        {
            Draw.SpriteBatch.Begin(SortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Matrix ?? Camera.Matrix * Engine.ScreenMatrix);
            DoRender();
            Draw.SpriteBatch.End();
        }

        protected bool InCameraBounds(int x, int y)
        {
            int tileX = (int)((Camera.X - Engine.Width / 2) / Game.SpriteWidth);
            int tileY = (int)((Camera.Y - Engine.Height / 2) / Game.SpriteHeight);
            int width = tileX + (Camera.Viewport.Width / Game.SpriteWidth);
            int height = tileY + (Camera.Viewport.Height / Game.SpriteHeight);
            return x >= tileX && y >= tileY && x <= width && y <= height;
        }
    }
}
