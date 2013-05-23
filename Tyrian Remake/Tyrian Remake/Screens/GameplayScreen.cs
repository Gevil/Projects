using System;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Tyrian_Remake
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        float pauseAlpha;

        //Textures
        Texture2D mShipTexture;
        Rectangle shipBodySourceRectangle;

        Sprites.PlayerShip mPlayerShipSprite;
        Sprites.Projectile mProjectileSprite;
        Sprites.BossTest mBossTestSprite;

        //Game Music
        SoundEffect GameMusic;
        public SoundEffectInstance GameMusicInstance;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("Downlink");

            //Load Ship
            mShipTexture = content.Load<Texture2D>("ship-1");

            //Default Ship State Texture
            shipBodySourceRectangle = new Rectangle(96, 0, 48, 56);

            mPlayerShipSprite = new Sprites.PlayerShip();
            mProjectileSprite = new Sprites.Projectile();

            mPlayerShipSprite.Scale = 1.0f;
            mPlayerShipSprite.Source = shipBodySourceRectangle;

            mBossTestSprite = new Sprites.BossTest();

            mProjectileSprite.LoadContent(content);
            mPlayerShipSprite.LoadContent(content);
            mBossTestSprite.LoadContent(content);

            //Game Music
            GameMusic = content.Load<SoundEffect>("Rock Garden");
            GameMusicInstance = GameMusic.CreateInstance();

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw
        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
            {
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            }
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
                //play dat music
                GameMusicInstance.Play();
            }

            if (IsActive)
            {
                mPlayerShipSprite.Update(gameTime, ScreenManager.GraphicsDeviceManager);
                mProjectileSprite.Update(gameTime);

                mBossTestSprite.Update(gameTime, ScreenManager.GraphicsDeviceManager);

                //play dat music
                if (GameMusicInstance.State != SoundState.Playing)
                {
                    GameMusicInstance.Volume = 0.75f;
                    GameMusicInstance.IsLooped = true;
                    GameMusicInstance.Play();
                }

                //Do stuff
                mPlayerShipSprite.isHit = false; mBossTestSprite.isHit = false;

                //Hit testing for enemy projectiles
                foreach (Sprites.Projectile projectile in mBossTestSprite.mProjectiles)
                {
                    if (projectile.Visible == true)
                    {
                        if (CollisionDetection.IntersectPixels(mPlayerShipSprite.BoundingRectangle, mPlayerShipSprite.mSpriteTextureData, projectile.BoundingRectangle, projectile.mSpriteTextureData))
                        {
                            mPlayerShipSprite.isHit = true;
                            projectile.Disposable = true;
                        }
                    }
                }

                //Hit testing for player projectiles
                foreach (Sprites.Projectile projectile in mPlayerShipSprite.mProjectiles)
                {
                    if (projectile.Visible == true)
                    {
                        if (CollisionDetection.IntersectPixels(mBossTestSprite.BoundingRectangle, mBossTestSprite.mSpriteTextureData, projectile.BoundingRectangle, projectile.mSpriteTextureData))
                        {
                            mBossTestSprite.isHit = true;
                            projectile.Disposable = true;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {

            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,Color.Black, 0, 0);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            //Draw Player Ship here
            mPlayerShipSprite.Draw(spriteBatch);

            //draw Boss test sprite
            mBossTestSprite.Draw(spriteBatch);

            if (mPlayerShipSprite.isHit) // Change the background to red when the player is hit by an enemy projectile
            {
                ScreenManager.GraphicsDevice.Clear(Color.DarkRed);
            }
            else if (mBossTestSprite.isHit) // Change the background to green when the enemy is hit by a player projectile
            {
                ScreenManager.GraphicsDevice.Clear(Color.DarkGreen);
            }
            else
            {
                ScreenManager.GraphicsDevice.Clear(Color.Black);
            }

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                //Linear interpolation
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}