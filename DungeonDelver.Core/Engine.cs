using DungeonDelver.Core.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Toolbox.Assets;
using Toolbox.Input;

namespace DungeonDelver.Core
{
    public class Engine : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Color ClearColor { get; set; }

        public static Engine Instance { get; private set; }

        public static int WindowWidth { get; private set; }
        public static int WindowHeight { get; private set; }
        public static int GameWidth { get; private set; }
        public static int GameHeight { get; private set; }
        public static int Scale { get; private set; }
        public static string Title { get; private set; }

        public static AssetStore<string> Assets { get; private set; }

        private const float DefaultTimeRate = 1f;
        public static float TimeRate = DefaultTimeRate;
        public static float DeltaTime { get; private set; }
        public static float RawDeltaTime { get; private set; }

        private RenderTarget2D _renderTarget;
        private Rectangle _renderTargetDestination;

        public static int FPS;
        public static int UPS;

        private TimeSpan _counterElapsed = TimeSpan.Zero;
        private int _fpsCounter;
        private int _upsCounter;

        private readonly DelayedInputHandler _inputHandler = new DelayedInputHandler(20);
        private Game _game;

        public Engine(int windowWidth, int windowHeight, int gameWidth, int gameHeight, int scale, string title)
        {
            Instance = this;
            Assets = new AssetStore<string>();
            ClearColor = Color.Black;

            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            GameWidth = gameWidth;
            GameHeight = gameHeight;
            Scale = scale;
            Title = title;

            _graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = true,
                PreferMultiSampling = false,
                GraphicsProfile = GraphicsProfile.Reach,
                PreferredBackBufferWidth = WindowWidth,
                PreferredBackBufferHeight = WindowHeight,
                PreferredBackBufferFormat = SurfaceFormat.Color,
                PreferredDepthStencilFormat = DepthFormat.None
            };


            Content.RootDirectory = "Content";
            Window.Title = Title;
            Window.AllowUserResizing = false;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            UpdateView();
            _inputHandler.InputFireEvent += OnInputEvent;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            double memBefore = GC.GetTotalMemory(false) / 1048576f;
            DataLoader.Initialize();
            double memAfter = GC.GetTotalMemory(false) / 1048576f;
            Console.WriteLine($"Loaded {memAfter - memBefore:F} MB of assets");

            _game = new Game();
        }

        private void OnInputEvent(object sender, KeyEventArgs e)
        {
            _game.Input(e.Key);
        }

        protected override void Update(GameTime gameTime)
        {
            RawDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            DeltaTime = RawDeltaTime * TimeRate;

            _inputHandler.Update(Keyboard.GetState());
            _game.Update(DeltaTime);
            _upsCounter++;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            RenderCore();

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_renderTarget, _renderTargetDestination, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);

            _fpsCounter++;
            _counterElapsed += gameTime.ElapsedGameTime;

#if DEBUG
            if (_counterElapsed >= TimeSpan.FromSeconds(1))
            {
                Window.Title = $"{Title} {_fpsCounter}fps | {_upsCounter}ups - {GC.GetTotalMemory(false) / 1048576f:F} MB";
                FPS = _fpsCounter;
                UPS = _upsCounter;
                _fpsCounter = 0;
                _upsCounter = 0;
                _counterElapsed -= TimeSpan.FromSeconds(1);
            }
#endif
        }

        private void RenderCore()
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(ClearColor);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _game.Render(_spriteBatch);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
        }

        private void UpdateView()
        {
            _renderTargetDestination = new Rectangle()
            {
                Width = WindowWidth,
                Height = WindowHeight
            };

            _renderTarget = new RenderTarget2D(GraphicsDevice, GameWidth, GameHeight);
        }
    }
}
