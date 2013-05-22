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
    class BossTest : Sprite
    {
        const string SHIP_ASSETNAME = "boss-test";
        const int START_POSITION_X = 450;
        const int START_POSITION_Y = 70;
        const int SHIP_SPEED = 170;
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

        const double fireRate = 1.5;
        double fireTime = 0;

        public bool isHit = false;

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;

            foreach (Projectile aProjectile in mProjectiles)
            {
                aProjectile.LoadContent(theContentManager);
            }

            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, SHIP_ASSETNAME);
            Source = new Rectangle(0, 0, Source.Width, Source.Height);

        }

        public void Update(GameTime theGameTime, GraphicsDeviceManager graphics)
        {
            fireTime += 0.1;
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            UpdateProjectile(theGameTime, aCurrentKeyboardState);

            mPreviousKeyboardState = aCurrentKeyboardState;

            base.Update(theGameTime, mSpeed, mDirection);
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
                        aProjectile.Fire(Position + new Vector2(Size.Width / 4, Size.Height), new Vector2(-200, -200), new Vector2(0, 1));
                        break;
                    }
                }*/

                if (aCreateNew == true)
                {
                    Projectile aProjectile = new Projectile();
                    aProjectile.isEnemyProjectile = true;
                    aProjectile.LoadContent(mContentManager);
                    
                    aProjectile.Fire(Position + new Vector2(Size.Width / 4, Size.Height), new Vector2(-200, -200), new Vector2(0, 1));
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
