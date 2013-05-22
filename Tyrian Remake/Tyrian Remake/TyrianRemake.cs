#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using Tyrian_Remake.CollisionDetection;
#endregion

namespace Tyrian_Remake
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TyrianRemake : Game
    {
        #region Declarations
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch ScreenBatch;

        //The screens and the current screen
        Screens.ControllerDetectScreen mControllerScreen;
        Screens.TitleScreen mTitleScreen;
        Screens.InGameScreen mInGameScreen;
        Screen mCurrentScreen;

        //Default screen size
        const int defaultWidth = 1280, defaultHeight = 720;

        //Get Primary Monitor Resolution
        double primaryScreenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
        double primaryScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;

        //Calculate scale for default window resolution compared to actual screen resolution
        double resolutionScale = Math.Abs(defaultWidth / System.Windows.SystemParameters.PrimaryScreenWidth);

        //Font
        SpriteFont TextFont;

        //Ship
        Texture2D mShipTexture;
        Rectangle shipBodySourceRectangle;

        Vector2 shipPosition;
        Vector2 shipOrigin;

        Sprites.PlayerShip mPlayerShipSprite;
        Sprites.Projectile mProjectileSprite;
        Sprites.BossTest mBossTestSprite;

        //Game Music
        SoundEffect GameMusic;
        public SoundEffectInstance GameMusicInstance;

        // For when a collision is detected
        bool playerHit = false;

        #endregion


        public TyrianRemake() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Set some fancy shit
            graphics.PreferMultiSampling = true;
            this.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicClamp;
            graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;


            //Initialize screen size to an ideal resolution for the XBox 360
            this.graphics.PreferredBackBufferWidth = defaultWidth;
            this.graphics.PreferredBackBufferHeight = defaultHeight;

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
            System.Reflection.FieldInfo field = type.GetField("window", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            OpenTK.GameWindow window = (OpenTK.GameWindow)field.GetValue(this.Window);


            MonoGameExtensions.SetPosition(this.Window, new Point(0,0));

            // Calculate position and origin to draw in the center of the screen
            shipPosition = new Vector2(this.Window.ClientBounds.Width / 2, this.Window.ClientBounds.Height / 2);
            shipOrigin = new Vector2(24 / 1.0f, 28);

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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Initialize the various screens in the game
            mControllerScreen = new Screens.ControllerDetectScreen(this.Content, new EventHandler(ControllerDetectScreenEvent));
            mTitleScreen = new Screens.TitleScreen(this.Content, new EventHandler(TitleScreenEvent));
            mInGameScreen = new Screens.InGameScreen(this.Content, new EventHandler(InGameScreenEvent));

            //Set the current screen
            mCurrentScreen = mControllerScreen;

            //Load Font
            TextFont = Content.Load<SpriteFont>("Downlink");

            //Load Ship
            mShipTexture = Content.Load<Texture2D>("ship-1");

            //Default Ship State Texture
            shipBodySourceRectangle = new Rectangle(96, 0, 48, 56);

            mPlayerShipSprite = new Sprites.PlayerShip();
            mProjectileSprite = new Sprites.Projectile();

            mPlayerShipSprite.Scale = 1.0f;
            mPlayerShipSprite.Source = shipBodySourceRectangle;

            mBossTestSprite = new Sprites.BossTest();

            mProjectileSprite.LoadContent(Content);
            mPlayerShipSprite.LoadContent(Content);
            mBossTestSprite.LoadContent(Content);

            //Game Music
            GameMusic = Content.Load<SoundEffect>("Rock Garden");
            GameMusicInstance = GameMusic.CreateInstance();

        }
        #endregion

        #region Screen / GameState Events
        //Screen Events
        //This event fires when the Controller detect screen is returning control back to the main game class
        public void ControllerDetectScreenEvent(object obj, EventArgs e)
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
        }

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //By taking advantage of Polymorphism, we can call update on the current screen class, 
            //but the Update in the subclass is the one that will be executed.
            mCurrentScreen.Update(gameTime);

            mPlayerShipSprite.Update(gameTime, graphics);
            mProjectileSprite.Update(gameTime);

            mBossTestSprite.Update(gameTime, graphics);


            if (mCurrentScreen is Screens.InGameScreen)
            {
                //Sound
                if (mTitleScreen.MenuMusicInstance.State == SoundState.Playing)
                    mTitleScreen.MenuMusicInstance.Stop();

                if (GameMusicInstance.State != SoundState.Playing)
                {
                    GameMusicInstance.Volume = 0.75f;
                    GameMusicInstance.IsLooped = true;
                    GameMusicInstance.Play();
                }

                playerHit = false; mBossTestSprite.isHit = false;

                //Hit testing for enemy projectiles
                foreach (Sprites.Projectile projectile in mBossTestSprite.mProjectiles)
                {
                    if (projectile.Visible == true)
                    {
                        if (_2DCollisionDetection.IntersectPixels(mPlayerShipSprite.BoundingRectangle, mPlayerShipSprite.mSpriteTextureData, projectile.BoundingRectangle, projectile.mSpriteTextureData))
                        {
                            playerHit = true;
                            projectile.Disposable = true;
                        }
                    }
                }

                
                //Hit testing for player projectiles
                foreach (Sprites.Projectile projectile in mPlayerShipSprite.mProjectiles)
                {
                    if (projectile.Visible == true)
                    {
                        if (_2DCollisionDetection.IntersectPixels(mBossTestSprite.BoundingRectangle, mBossTestSprite.mSpriteTextureData, projectile.BoundingRectangle, projectile.mSpriteTextureData))
                        {
                            mBossTestSprite.isHit = true;
                            projectile.Disposable = true;
                        }
                    }
                }
            }


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

            float screenscale = (float)graphics.GraphicsDevice.Viewport.Width / defaultWidth;
            var SpriteScale = Matrix.CreateScale(screenscale*0.67f, screenscale*0.67f, 1);


            ScreenBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, SpriteScale);

            //Again, using Polymorphism, we can call draw on the current screen class
            //and the Draw in the subclass is the one that will be executed.
            mCurrentScreen.Draw(ScreenBatch);

            Vector2 textPos = new Vector2(20, 20);
            ScreenBatch.DrawString(TextFont, "Running!", textPos, Color.Peru);

            ScreenBatch.End();


            SpriteScale = Matrix.CreateScale(screenscale, screenscale, 1);


            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, SpriteScale);


            if (mCurrentScreen is Screens.InGameScreen)
            {
                //spriteBatch.Draw(mShipTexture, shipPosition, shipBodySourceRectangle, Color.White, 0.0f, shipOrigin, 4.0f, SpriteEffects.None, 0.0f);
                
                //Draw Player Ship here
                mPlayerShipSprite.Draw(spriteBatch);

                //draw Boss test sprite
                mBossTestSprite.Draw(spriteBatch);

                if (playerHit) // Change the background to red when the player is hit by an enemy projectile
                {
                    GraphicsDevice.Clear(Color.DarkRed);
                }
                else if (mBossTestSprite.isHit) // Change the background to green when the enemy is hit by a player projectile
                {
                    GraphicsDevice.Clear(Color.DarkGreen);
                }
                else
                {
                    GraphicsDevice.Clear(Color.Black);
                }                

            }

            spriteBatch.End();

            //this.Window.Title = "Running!";

            base.Draw(gameTime);
        }
        #endregion

    }
}
