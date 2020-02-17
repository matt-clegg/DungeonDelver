using DungeonDelver.Core.Data;
using DungeonDelver.Core.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime;
using Toolbox.Assets;
using Toolbox.Input;

namespace DungeonDelver.Core
{
    public class Engine : Microsoft.Xna.Framework.Game
    {
        public Color ClearColor { get; set; }

        public static Engine Instance { get; private set; }

        public static float ViewScale { get; private set; }
        public static int Scale { get; private set; }
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public static int ViewWidth { get; private set; }
        public static int ViewHeight { get; private set; }
        public static bool Fullscreen { get; private set; }
        public static int ViewPadding {
            get => _viewPadding;
            set {
                _viewPadding = value;
                Instance.UpdateView();
            }
        }

        public static string Title { get; private set; }

        public static AssetStore<string> Assets { get; private set; }

        private const float DefaultTimeRate = 1f;
        public static float TimeRate = DefaultTimeRate;
        public static float DeltaTime { get; private set; }
        public static float RawDeltaTime { get; private set; }

        public static Matrix ScreenMatrix;
        public static Viewport Viewport { get; private set; }
        public static GraphicsDeviceManager Graphics { get; private set; }

        public static int FPS;
        public static int UPS;

        private TimeSpan _counterElapsed = TimeSpan.Zero;
        private int _fpsCounter;
        private int _upsCounter;

        private static int _viewPadding;
        private static bool _resizing;

        private static int _originalViewWidth;
        private static int _originalViewHeight;

        private readonly DelayedInputHandler _inputHandler = new DelayedInputHandler(20);
        private Game _game;

        public static int GameScale => ViewWidth / Width;

        public Engine(int width, int height, int windowWidth, int windowHeight, int scale, string title, bool fullscreen)
        {
            Instance = this;
            Assets = new AssetStore<string>();
            ClearColor = Color.FromNonPremultiplied(10, 10, 10, 255);

            Width = width;
            Height = height;
            Scale = scale;
            Window.Title = Title = title;
            Fullscreen = fullscreen;
            _originalViewWidth = windowWidth;
            _originalViewHeight = windowHeight;

            Graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = false,
                PreferMultiSampling = false,
                GraphicsProfile = GraphicsProfile.HiDef,
                PreferredBackBufferFormat = SurfaceFormat.Color,
                PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8
            };

            Graphics.DeviceReset += OnGraphicsReset;
            Graphics.DeviceCreated += OnGraphicsCreate;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnClientSizeChanged;

            if (fullscreen)
            {
                Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                Graphics.IsFullScreen = true;
            }
            else
            {
                Graphics.PreferredBackBufferWidth = windowWidth;
                Graphics.PreferredBackBufferHeight = windowHeight;
                Graphics.IsFullScreen = false;
            }

            Graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
        }

        protected override void Initialize()
        {
            UpdateView();
            _inputHandler.InputFireEvent += OnInputEvent;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            double memBefore = GC.GetTotalMemory(false) / 1048576f;

            DataLoader.Load();

            Texture2D mouseTexture = Content.Load<Texture2D>("Textures/mouse");
            Mouse.SetCursor(MouseCursor.FromTexture2D(mouseTexture, 0, 0));

            double memAfter = GC.GetTotalMemory(false) / 1048576f;
            Console.WriteLine($"Loaded {memAfter - memBefore:F} MB of assets");

            Util.Draw.Initialize(GraphicsDevice);
            
            _game = new Game();
        }

        private void OnInputEvent(object sender, KeyEventArgs e)
        {
            _game.Input(e.Key);

            if (Controls.F11.IsPressed(e.Key))
            {
                Fullscreen = !Fullscreen;
                if (Fullscreen)
                {
                    SetFullscreen();
                }
                else
                {
                    SetWindowed(_originalViewWidth, _originalViewHeight);
                }
            }

#if !CONSOLE
            if (Controls.Escape.IsPressed(e.Key))
            {
                Exit();
            }
#endif
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
            RenderCore();
            base.Draw(gameTime);

            _fpsCounter++;
            _counterElapsed += gameTime.ElapsedGameTime;

            if (_counterElapsed >= TimeSpan.FromSeconds(1))
            {
                Window.Title = $"{Title} {_fpsCounter}fps | {_upsCounter}ups - {GC.GetTotalMemory(false) / 1048576f:F} MB | d: {Util.Draw.SpriteDraws / 60}";
                Util.Draw.SpriteDraws = 0;
                FPS = _fpsCounter;
                UPS = _upsCounter;
                _fpsCounter = 0;
                _upsCounter = 0;
                _counterElapsed -= TimeSpan.FromSeconds(1);
            }
        }

        private void RenderCore()
        {
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Viewport = Viewport;
            GraphicsDevice.Clear(ClearColor);

            _game.Render();
        }

        public static void SetWindowed(int width, int height)
        {
#if !CONSOLE
            if (width > 0 && height > 0)
            {
                _resizing = true;
                Graphics.PreferredBackBufferWidth = width;
                Graphics.PreferredBackBufferHeight = height;
                Graphics.IsFullScreen = false;
                Graphics.ApplyChanges();
                _resizing = false;
            }
#endif
        }

        public static void SetFullscreen()
        {
#if !CONSOLE
            _resizing = true;
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Graphics.IsFullScreen = true;
            Graphics.ApplyChanges();
            _resizing = false;
#endif
        }

        protected virtual void OnGraphicsReset(object sender, EventArgs e)
        {
            UpdateView();
        }

        protected virtual void OnGraphicsCreate(object sender, EventArgs e)
        {
            UpdateView();
        }

#if !CONSOLE
        protected virtual void OnClientSizeChanged(object sender, EventArgs e)
        {
            if (Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0 && !_resizing)
            {
                _resizing = true;

                Graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                Graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                UpdateView();

                _resizing = false;
            }
        }
#endif

        private void UpdateView()
        {
            float screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            float screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

            if (screenWidth / Width > screenHeight / Height)
            {
                ViewWidth = (int)(screenHeight / Height * Width);
                ViewHeight = (int)screenHeight;
            }
            else
            {
                ViewWidth = (int)screenWidth;
                ViewHeight = (int)(screenWidth / Width * Height);
            }

            float aspect = ViewHeight / (float)ViewWidth;
            ViewWidth -= ViewPadding * 2;
            ViewHeight -= (int)(aspect * ViewPadding * 2);

            ViewScale = ViewWidth / (float)Width;

            ScreenMatrix = Matrix.CreateScale(ViewScale);

            Viewport = new Viewport
            {
                X = (int)(screenWidth / 2 - ViewWidth / 2f),
                Y = (int)(screenHeight / 2 - ViewHeight / 2f),
                Width = ViewWidth,
                Height = ViewHeight,
                MinDepth = 0,
                MaxDepth = 1
            };
        }
    }
}
