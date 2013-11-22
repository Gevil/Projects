using System;
using System.Linq;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Tyrian_Remake.Sprites;

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

        ContentManager _content;
        SpriteFont _gameFont;

        float _pauseAlpha;

        //Textures
        Texture2D _mShipTexture;
        Rectangle _shipBodySourceRectangle;

        PlayerShip _mPlayerShipSprite;
        Projectile _mProjectileSprite;
        BossTest _mBossTestSprite;

        //Game Music
        SoundEffect _gameMusic;
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
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("Downlink");

            //Load Ship
            _mShipTexture = _content.Load<Texture2D>("ship-1");

            //Default Ship State Texture
            _shipBodySourceRectangle = new Rectangle(96, 0, 48, 56);

            _mPlayerShipSprite = new PlayerShip();
            _mProjectileSprite = new Projectile();

            _mPlayerShipSprite.Scale = 1.0f;
            _mPlayerShipSprite.Source = _shipBodySourceRectangle;

            _mBossTestSprite = new BossTest();

            _mProjectileSprite.LoadContent(_content);
            _mPlayerShipSprite.LoadContent(_content);
            _mBossTestSprite.LoadContent(_content);

            //Game Music
            _gameMusic = _content.Load<SoundEffect>("Rock Garden");
            GameMusicInstance = _gameMusic.CreateInstance();

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
            _content.Unload();
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
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            }
            else
            {
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);
                //play dat music
                GameMusicInstance.Play();
            }

            if (IsActive)
            {
                _mPlayerShipSprite.Update(gameTime, ScreenManager.GraphicsDeviceManager);
                _mProjectileSprite.Update(gameTime);

                _mBossTestSprite.Update(gameTime, ScreenManager.GraphicsDeviceManager);

                //play dat music
                if (GameMusicInstance.State != SoundState.Playing)
                {
                    GameMusicInstance.Volume = 0.75f;
                    GameMusicInstance.IsLooped = true;
                    GameMusicInstance.Play();
                }

                //Do stuff
                _mPlayerShipSprite.IsHit = false; _mBossTestSprite.IsHit = false;

                //Hit testing for enemy projectiles
                foreach (var projectile in _mBossTestSprite.MProjectiles.Where(projectile => projectile.Visible).Where(
                    projectile => CollisionDetection.IntersectPixels(
                        _mPlayerShipSprite.BoundingRectangle,
                        _mPlayerShipSprite.MSpriteTextureData,
                        projectile.BoundingRectangle,
                        projectile.MSpriteTextureData)))
                {
                    _mPlayerShipSprite.IsHit = true;
                    projectile.Disposable = true;
                }

                //Hit testing for player projectiles
                foreach (var projectile in _mPlayerShipSprite.MProjectiles.Where(
                    projectile => projectile.Visible).Where(
                    projectile => CollisionDetection.IntersectPixels(
                        _mBossTestSprite.BoundingRectangle,
                        _mBossTestSprite.MSpriteTextureData,
                        projectile.BoundingRectangle,
                        projectile.MSpriteTextureData)))
                {
                    _mBossTestSprite.IsHit = true;
                    projectile.Disposable = true;
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
            if (ControllingPlayer == null) return;
            var playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            var gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
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
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            //Draw Player Ship here
            _mPlayerShipSprite.Draw(spriteBatch);

            //draw Boss test sprite
            _mBossTestSprite.Draw(spriteBatch);

            if (_mPlayerShipSprite.IsHit) // Change the background to red when the player is hit by an enemy projectile
            {
                ScreenManager.GraphicsDevice.Clear(Color.DarkRed);
            }
            else if (_mBossTestSprite.IsHit) // Change the background to green when the enemy is hit by a player projectile
            {
                ScreenManager.GraphicsDevice.Clear(Color.DarkGreen);
            }
            else
            {
                ScreenManager.GraphicsDevice.Clear(Color.Black);
            }

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (!(TransitionPosition > 0) && !(_pauseAlpha > 0)) return;

            //Linear interpolation
            var alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

            ScreenManager.FadeBackBufferToBlack(alpha);
        }


        #endregion
    }
}