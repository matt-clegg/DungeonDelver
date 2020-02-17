using System;
using System.Collections.Generic;
using System.Text;
using DungeonDelver.Core.Util;
using DungeonDelver.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Toolbox.Graphics;

namespace DungeonDelver.Core.Renderers
{
    public class InterfaceRenderer : Renderer
    {
        private int _mouseX;
        private int _mouseY;

        private readonly Map _map;
        private readonly Camera _camera;
        private readonly Sprite _tileSelection;

        public InterfaceRenderer(Map map, Camera camera) : base(null)
        {
            _map = map;
            _camera = camera;
            Matrix = Microsoft.Xna.Framework.Matrix.Identity * Microsoft.Xna.Framework.Matrix.CreateScale(Engine.Scale);
            _tileSelection = Engine.Assets.GetAsset<Sprite>("tile_selection");
        }

        public override void Update(float delta)
        {
            MouseState mouse = Mouse.GetState();
            _mouseX = mouse.X / Engine.Scale;
            _mouseY = mouse.Y / Engine.Scale;
        }

        protected override void DoRender()
        {
            int x = (int)(_mouseX - (Engine.Viewport.X / Engine.ViewScale));
            int y = (int)(_mouseY - (Engine.Viewport.Y / Engine.ViewScale));

            int tileX = (x  / Game.SpriteWidth) * Game.SpriteWidth;
            int tileY = (y / Game.SpriteHeight) * Game.SpriteHeight;

            //Draw.Text("abcdefghijklmnopqrstuvwxyz", x, y, Color.Red);
            Draw.Sprite(_tileSelection, new Vector2(tileX, tileY), Color.White);

            string name = "tile";

            
            Draw.Text(name, (int)_camera.X, (int)_camera.Y);
        }

    }
}

