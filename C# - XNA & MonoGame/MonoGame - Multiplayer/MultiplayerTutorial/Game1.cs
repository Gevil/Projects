#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace MultiplayerTutorial
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        NetworkManager NetworkManager;
        KeyboardState PreviousKeyboardState;

        SpriteFont TextFont;

        bool IsNetworking;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


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

            NetworkManager = new NetworkManager();

            IsNetworking = false;

            PreviousKeyboardState = Keyboard.GetState();

            TextFont = Content.Load<SpriteFont>("Font");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            NetworkManager.Update();

            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.H) && PreviousKeyboardState.IsKeyUp(Keys.H) && !IsNetworking)
            {
                NetworkManager.Host();
                IsNetworking = true;
            }

            if (keyboard.IsKeyDown(Keys.C) && PreviousKeyboardState.IsKeyUp(Keys.C) && !IsNetworking)
            {
                NetworkManager.Connect("127.0.0.1");
                IsNetworking = true;
            }


            PreviousKeyboardState = keyboard;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw status text
            spriteBatch.Begin();

            Vector2 textPos = new Vector2(20, 20);

            if (IsNetworking)
            {
                if (NetworkManager.IsHost)
                    spriteBatch.DrawString(TextFont, "Running Server!", textPos, Color.Black);
                else
                    spriteBatch.DrawString(TextFont, "Running client!", textPos, Color.Black);

            }
            else
                spriteBatch.DrawString(TextFont, "Press H to begin hosting. Or C to connect to local server.", textPos, Color.Black);


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
