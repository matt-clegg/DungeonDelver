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

        protected int BoundsX => (int)((Camera.X - Engine.Width / 2) / Game.SpriteWidth);
        protected int BoundsY => (int)((Camera.Y - Engine.Height / 2) / Game.SpriteHeight);
        protected int BoundsWidth => BoundsX + (Camera.Viewport.Width / Game.SpriteWidth);
        protected int BoundsHeight => BoundsY + (Camera.Viewport.Height / Game.SpriteHeight);

        public Renderer(Camera camera)
        {
            BlendState = BlendState.AlphaBlend;
            SamplerState = SamplerState.PointClamp;
            DepthStencilState = DepthStencilState.None;
            RasterizerState = RasterizerState.CullNone;
            SortMode = SpriteSortMode.FrontToBack;
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
    }
}

