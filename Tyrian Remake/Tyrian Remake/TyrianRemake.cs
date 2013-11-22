#region Using Statements
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameWindow = OpenTK.GameWindow;

#endregion

namespace Tyrian_Remake
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TyrianRemake : Game
    {
        #region Declarations

        readonly GraphicsDeviceManager _graphics;

        SpriteBatch _spriteBatch;
        SpriteBatch _screenBatch;

        //The screens and the current screen
        /*Screens.ControllerDetectScreen mControllerScreen;
        Screens.TitleScreen mTitleScreen;
        Screens.InGameScreen mInGameScreen;
        Screen mCurrentScreen;*/

        bool _writeInfo = true;

#if ZUNE
        int DefaultWidth = 272;
        int DefaultHeight = 480;
#elif IPHONE
        int DefaultWidth = 320;
        int DefaultHeight = 480;
#else
        //Default screen size
        const int DefaultWidth = 1280;
        const int DefaultHeight = 720;
#endif


        //Get Primary Monitor Resolution
        double _primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
        double _primaryScreenWidth = SystemParameters.PrimaryScreenWidth;

        //Calculate scale for default window resolution compared to actual screen resolution
        double _resolutionScale = Math.Abs(DefaultWidth / SystemParameters.PrimaryScreenWidth);

        //Font
        SpriteFont _textFont;

        #endregion


        public TyrianRemake()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Create the screen manager component.
            var screenManager = new ScreenManager(this, _graphics);

            Components.Add(screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);


            //Set some fancy shit
            _graphics.PreferMultiSampling = true;
            GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicClamp;
            _graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;


            //Initialize screen size to an ideal resolution for the XBox 360
            _graphics.PreferredBackBufferWidth = DefaultWidth;
            _graphics.PreferredBackBufferHeight = DefaultHeight;

            //this.Window.ClientBounds.Location = new Point(Convert.ToInt32(primaryScreenWidth * resolutionScale), Convert.ToInt32(primaryScreenHeight * resolutionScale));

        }


        #region Initialize
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            Type type = typeof(OpenTKGameWindow);
            FieldInfo field = type.GetField("window", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                GameWindow window = (GameWindow)field.GetValue(Window);
            }


            Window.SetPosition(new Point(0,0));

        }
        #endregion



        #region Load Content
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _screenBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Initialize the various screens in the game
            /*mControllerScreen = new Screens.ControllerDetectScreen(this.Content, new EventHandler(ControllerDetectScreenEvent));
            mTitleScreen = new Screens.TitleScreen(this.Content, new EventHandler(TitleScreenEvent));
            mInGameScreen = new Screens.InGameScreen(this.Content, new EventHandler(InGameScreenEvent));

            //Set the current screen
            mCurrentScreen = mControllerScreen;*/

            //Load Font
            _textFont = Content.Load<SpriteFont>("Downlink");
        }
        #endregion

        #region Screen / GameState Events
        //Screen Events
        //This event fires when the Controller detect screen is returning control back to the main game class
        /*public void ControllerDetectScreenEvent(object obj, EventArgs e)
        {
            //Switch to the title screen, the Controller detect screen is finished being displayed
            mCurrentScreen = mTitleScreen;
        }

        //This event is fired when the Title screen is returning control back to the main game class
        public void TitleScreenEvent(object obj, EventArgs e)
        {
            //Switch to the controller detect screen, the Title screen is finished being displayed
            mCurrentScreen = mInGameScreen;
        }

        //This event is fired when the InGame screen is returning control back to the main game class
        public void InGameScreenEvent(object obj, EventArgs e)
        {
            //Switch to the InGame screen, the Title screen is finished being displayed
            mCurrentScreen = mInGameScreen;
        }*/

        #endregion


        #region UnloadContent (Non-ContentManager)
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        #endregion


        #region Update Logic
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (_writeInfo)
            {
                WriteLog("Update passed once.");

                _writeInfo = false;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        #endregion




        #region Draw / Render
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            var screenscale = (float)_graphics.GraphicsDevice.Viewport.Width / DefaultWidth;
            var spriteScale = Matrix.CreateScale(screenscale*0.67f, screenscale*0.67f, 1);


            _screenBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, spriteScale);

            //Again, using Polymorphism, we can call draw on the current screen class
            //and the Draw in the subclass is the one that will be executed.
            //mCurrentScreen.Draw(ScreenBatch);

            var textPos = new Vector2(20, 20);
            _screenBatch.DrawString(_textFont, "Running!", textPos, Color.Peru);

            _screenBatch.End();


            spriteScale = Matrix.CreateScale(screenscale, screenscale, 1);


            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, spriteScale);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion

        private static void WriteLog(string output)
        {
            Debug.WriteLine(output);
        }
    }
}
