using GraphicsEngine.ImGuiMonoGame;
using GraphicsEngine.Interfaces;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace GraphicsEngine
{
    /// <summary>
    /// The <see cref="Game"/> used by JCIW to draw graphics to screen.
    /// </summary>
    public class GUIWindow : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private readonly Platform platform;
        private readonly IPlatformFunctions platformFunctions;
        private ImGuiRenderer imGuiRenderer;
        private bool androidKeyboardOpen;
        private ImFontPtr font;

        public GUIWindow(Platform platform, IPlatformFunctions platformFunctions)
        {
            this.platform = platform;
            this.androidKeyboardOpen = false;
            this.platformFunctions = platformFunctions;

            TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d); // 30 FPS cap limit

            graphics = new GraphicsDeviceManager(this)
            {
                SupportedOrientations = DisplayOrientation.Portrait,
                PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8,
                GraphicsProfile = GraphicsProfile.HiDef,
                PreferMultiSampling = true,
                SynchronizeWithVerticalRetrace = false,
            };

            this.IsMouseVisible = true;
           
            Content.RootDirectory = "Content";

            if (platform == Platform.Android)
            {
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

                graphics.IsFullScreen = true;

                graphics.ApplyChanges();
            }
            else
            {
                if (platform == Platform.Desktop)
                {
                    graphics.PreferredBackBufferWidth = 800;
                    graphics.PreferredBackBufferHeight = 600;
                }
            }
            
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            imGuiRenderer = new ImGuiRenderer(this, platform, platformFunctions);

            if (platform == Platform.Android)
            {
                AndroidCopyToDisk("OpenSans-Regular.ttf");
            }

            font = ImGui.GetIO().Fonts.AddFontFromFileTTF(
                PlatformContentPath("OpenSans-Regular.ttf"), 24 * platformFunctions.ScreenDensity());

            imGuiRenderer.RebuildFontAtlas();

            base.Initialize();
        }

        /// <summary>
        /// Desktop and Android have different content folders. This is used to retrieve the content path.
        /// </summary>
        /// <param name="contentName">The name of the file to append to the content path.</param>
        /// <returns>The full path to the content folder + file name.</returns>
        private string PlatformContentPath(string contentName)
        {
            string path = "";

            if (platform == Platform.Android)
            {
                string appFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string contentFolder = Path.Combine(appFolder, "Content");
                path = Path.Combine(contentFolder, contentName);
            }
            else
            {
                if (platform == Platform.Desktop)
                {
                    string appFolder = AppDomain.CurrentDomain.BaseDirectory;
                    string contentFolder = Path.Combine(appFolder, "Content");
                    path = Path.Combine(contentFolder, contentName);
                }
            }

            return path;
        }

        /// <summary>
        /// Used to Android to copy files to the content folder. Neccessary for loading fonts.
        /// </summary>
        /// <param name="fontName">The name of the embedded resource name.</param>
        private void AndroidCopyToDisk(string fontName)
        {
            string appFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string contentFolder = Path.Combine(appFolder, "Content");
            string filePath = Path.Combine(contentFolder, fontName);

            if (!Directory.Exists(contentFolder))
            {
                Directory.CreateDirectory(contentFolder);
            }

            if (!File.Exists(filePath))
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var resourceName = "GraphicsEngine.Content." + fontName;

                Stream fontStream = assembly.GetManifestResourceStream(resourceName);

                var memoryStream = new MemoryStream();
                fontStream.CopyTo(memoryStream);

                File.WriteAllBytes(filePath, memoryStream.ToArray());
            }
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            if (platform == Platform.Android)
            {
                if (ImGui.KeyboardWanted() == 1)
                {
                    if (!androidKeyboardOpen)
                    {
                        // Open keyboard
                        platformFunctions.OpenKeyboard();

                        androidKeyboardOpen = true;
                    }
                }
                else
                {
                    if (androidKeyboardOpen)
                    {
                        platformFunctions.CloseKeyboard();

                        androidKeyboardOpen = false;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            imGuiRenderer.BeforeLayout(gameTime);

            ImGui.PushFont(font);
            ImGui.ShowDemoWindow();

            imGuiRenderer.AfterLayout();

            base.Draw(gameTime);
        }
    }
}
