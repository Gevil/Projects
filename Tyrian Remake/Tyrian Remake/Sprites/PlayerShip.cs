using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tyrian_Remake.Sprites
{
    class PlayerShip : Sprite
    {
        const string SHIP_ASSETNAME = "ship-1";
        const int START_POSITION_X = 200;
        const int START_POSITION_Y = 200;
        const int SHIP_SPEED = 200;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;

        enum State
        {
            Moving
        }

        State mCurrentState = State.Moving;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        KeyboardState mPreviousKeyboardState;

        Vector2 mStartingPosition = Vector2.Zero;

        public List<Projectile> mProjectiles = new List<Projectile>();

        ContentManager mContentManager;

        const double fireRate = 0.9;
        double fireTime = 0;

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;

            foreach (Projectile aProjectile in mProjectiles)
            {
                aProjectile.LoadContent(theContentManager);
            }
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, SHIP_ASSETNAME);
            Source = new Rectangle(96, 0, 48, 56); //new Rectangle(0, 0, Source.Width, Source.Height);

        }

        public void Update(GameTime theGameTime, GraphicsDeviceManager graphics)
        {
            fireTime += 0.1;
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            UpdateMovement(aCurrentKeyboardState, graphics);
            UpdateProjectile(theGameTime, aCurrentKeyboardState);

            mPreviousKeyboardState = aCurrentKeyboardState;

            base.Update(theGameTime, mSpeed, mDirection);
        }

        private void UpdateMovement(KeyboardState aCurrentKeyboardState, GraphicsDeviceManager graphics)
        {

            if (mCurrentState == State.Moving)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true && Position.X > 0)
                {
                    mSpeed.X = SHIP_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true && Position.X + Source.Width < graphics.GraphicsDevice.Viewport.Width)
                {
                    mSpeed.X = SHIP_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }

                if (aCurrentKeyboardState.IsKeyDown(Keys.Up) == true && Position.Y > 0)
                {
                    mSpeed.Y = SHIP_SPEED;
                    mDirection.Y = MOVE_UP;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Down) == true && Position.Y + Source.Height < graphics.GraphicsDevice.Viewport.Height)
                {
                    mSpeed.Y = SHIP_SPEED;
                    mDirection.Y = MOVE_DOWN;
                }
            }
        }

        private void UpdateProjectile(GameTime theGameTime, KeyboardState aCurrentKeyboardState)
        {
            foreach (Projectile aProjectile in mProjectiles)
            {
                aProjectile.Update(theGameTime);
            }

            if (aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && fireTime >= fireRate  /*&& mPreviousKeyboardState.IsKeyDown(Keys.Space) == false*/)
            {
                fireTime = 0;
                ShootProjectile();
            }
        }

        private void ShootProjectile()
        {
            if (mCurrentState == State.Moving)
            {
                bool aCreateNew = true;
                /*foreach (Projectile aProjectile in mProjectiles)
                {
                    if (aProjectile.Visible == false)
                    {
                        aCreateNew = false;
                        aProjectile.Fire(Position + new Vector2(Size.Width / 4, 0/), new Vector2(-200, -200), new Vector2(0, -1));
                        break;
                    }
                }*/

                if (aCreateNew == true)
                {
                    Projectile aProjectile = new Projectile();
                    aProjectile.LoadContent(mContentManager);
                    aProjectile.isEnemyProjectile = false;
                    aProjectile.Fire(Position + new Vector2(Size.Width / 4, 0 /*Size.Height / 2*/), new Vector2(-200, -200), new Vector2(0, -1));
                    mProjectiles.Add(aProjectile);
                }
            }
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Projectile aProjectile in mProjectiles)
            {
                aProjectile.Draw(theSpriteBatch);
            }

            base.Draw(theSpriteBatch);
        }
    }
}
