using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PewPew.Screens;

namespace PewPew
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PewPew : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        VerticallyScrollingBackground mScrollingBackground;
        PShip mPShipSprite;
        Projectile mProjectileSprite;

        //The screens and the current screen
        //ControllerDetectScreen mControllerScreen;
        Screens.MainMenu mMainMenu;
        Screens.GameScreen mGameScreen;
        Screen mCurrentScreen;

        public PewPew()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 1000;


            /****Fullscreen Code****/
            //this.graphics.PreferredBackBufferWidth = 1920;
            //this.graphics.PreferredBackBufferHeight = 1080;
            //this.graphics.IsFullScreen = true;
        }

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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Initialize the various screens in the game
            mMainMenu = new MainMenu(this.Content, new EventHandler(MainMenuEvent));
            mGameScreen = new GameScreen(this.Content, new EventHandler(GameScreenEvent));
            //Set the current screen
            mCurrentScreen = mMainMenu;

        }

        //This event is fired when the Title screen is returning control back to the main game class
        public void MainMenuEvent(object obj, EventArgs e)
        {
            //Switch to the controller detect screen, the Title screen is finished being displayed
            mCurrentScreen = mMainMenu;
        }

        //This event is fired when the Title screen is returning control back to the main game class
        public void GameScreenEvent(object obj, EventArgs e)
        {
            //Switch to the controller detect screen, the Title screen is finished being displayed
            mCurrentScreen = mGameScreen;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            //Update the scrolling backround. You can scroll to the left or to the right by changing the scroll direction
            mScrollingBackground.Update(gameTime, 160, VerticallyScrollingBackground.VerticalScrollDirection.Down);

            //By taking advantage of Polymorphism, we can call update on the current screen class, 
            //but the Update in the subclass is the one that will be executed.
            mCurrentScreen.Update(gameTime);

            mPShipSprite.Update(gameTime, graphics);
            mProjectileSprite.Update(gameTime);
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            //Again, using Polymorphism, we can call draw on the current screen class
            //and the Draw in the subclass is the one that will be executed.
            mCurrentScreen.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
