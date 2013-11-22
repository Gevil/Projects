using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tyrian_Remake.Sprites
{
    class PlayerShip : Sprite
    {
        const string ShipAssetname = "ship-1";
        const int StartPositionX = 200;
        const int StartPositionY = 200;
        const int ShipSpeed = 200;
        const int MoveUp = -1;
        const int MoveDown = 1;
        const int MoveLeft = -1;
        const int MoveRight = 1;

        enum State
        {
            Moving
        }

        private const State MCurrentState = State.Moving;

        Vector2 _mDirection = Vector2.Zero;
        Vector2 _mSpeed = Vector2.Zero;

        KeyboardState _mPreviousKeyboardState;

        Vector2 _mStartingPosition = Vector2.Zero;

        public List<Projectile> MProjectiles = new List<Projectile>();

        ContentManager _mContentManager;

        public bool IsHit = false;

        const double FireRate = 0.9;
        double _fireTime = 0;

        public void LoadContent(ContentManager theContentManager)
        {
            _mContentManager = theContentManager;

            foreach (var aProjectile in MProjectiles)
            {
                aProjectile.LoadContent(theContentManager);
            }
            Position = new Vector2(StartPositionX, StartPositionY);
            LoadContent(theContentManager, ShipAssetname);
            Source = new Rectangle(96, 0, 48, 56); //new Rectangle(0, 0, Source.Width, Source.Height);

        }

        public void Update(GameTime theGameTime, GraphicsDeviceManager graphics)
        {
            _fireTime += 0.1;
            var aCurrentKeyboardState = Keyboard.GetState();

            UpdateMovement(aCurrentKeyboardState, graphics);
            UpdateProjectile(theGameTime, aCurrentKeyboardState);

            _mPreviousKeyboardState = aCurrentKeyboardState;

            Update(theGameTime, _mSpeed, _mDirection);
        }

        private void UpdateMovement(KeyboardState aCurrentKeyboardState, GraphicsDeviceManager graphics)
        {

            if (MCurrentState == State.Moving)
            {
                _mSpeed = Vector2.Zero;
                _mDirection = Vector2.Zero;

                if (aCurrentKeyboardState.IsKeyDown(Keys.Left) && Position.X > 0)
                {
                    _mSpeed.X = ShipSpeed;
                    _mDirection.X = MoveLeft;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) && Position.X + Source.Width < graphics.GraphicsDevice.Viewport.Width)
                {
                    _mSpeed.X = ShipSpeed;
                    _mDirection.X = MoveRight;
                }

                if (aCurrentKeyboardState.IsKeyDown(Keys.Up) && Position.Y > 0)
                {
                    _mSpeed.Y = ShipSpeed;
                    _mDirection.Y = MoveUp;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Down) && Position.Y + Source.Height < graphics.GraphicsDevice.Viewport.Height)
                {
                    _mSpeed.Y = ShipSpeed;
                    _mDirection.Y = MoveDown;
                }
            }
        }

        private void UpdateProjectile(GameTime theGameTime, KeyboardState aCurrentKeyboardState)
        {
            foreach (var aProjectile in MProjectiles)
            {
                aProjectile.Update(theGameTime);
            }

            if (!aCurrentKeyboardState.IsKeyDown(Keys.Space) || !(_fireTime >= FireRate)) return;
            _fireTime = 0;
            ShootProjectile();
        }

        private void ShootProjectile()
        {
            var aProjectile = new Projectile();
            aProjectile.LoadContent(_mContentManager);
            aProjectile.IsEnemyProjectile = false;
            aProjectile.Fire(Position + new Vector2(Size.Width / 4, 0 /*Size.Height / 2*/), new Vector2(-200, -200), new Vector2(0, -1));
            MProjectiles.Add(aProjectile);
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (var aProjectile in MProjectiles)
            {
                aProjectile.Draw(theSpriteBatch);
            }

            base.Draw(theSpriteBatch);
        }
    }
}
