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

namespace HitTheBallAndDoNotMiss
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Gameplay mGameplay;
        Title mTitle;

        Screen mCurrentScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
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

            Viewport aViewport = new Viewport();
            aViewport.X = 0;
            aViewport.Y = 0;
            aViewport.Width = 1280;
            aViewport.Height = 720;

            // TODO: use this.Content to load your game content here
            mGameplay = new Gameplay(Content, aViewport, new EventHandler(HandleGameplayScreen), PlayerIndex.One, PlayerIndex.Two);
            mTitle = new Title(Content, aViewport, new EventHandler(HandleTitleScreen));

            mCurrentScreen = mTitle;
        }

        public void HandleGameplayScreen(object theObject, EventArgs e)
        {

        }

        public void HandleTitleScreen(object theObject, EventArgs e)
        {
            switch (mTitle.mMenu.SelectedItem.Text)
            {
                case "Exit Game":
                    {
                        this.Exit();
                        break;
                    }

                case "1 Player":
                    {
                        mGameplay.StartOnePlayerGame(PlayerIndex.One);
                        mCurrentScreen = mGameplay;
                        break;
                    }

                case "2 Player":
                    {
                        mGameplay.StartTwoPlayerGame(PlayerIndex.One, PlayerIndex.Two);
                        mCurrentScreen = mGameplay;
                        break;
                    }
            }
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

            mCurrentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            mCurrentScreen.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
