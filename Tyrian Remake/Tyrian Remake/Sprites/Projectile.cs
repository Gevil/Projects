

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tyrian_Remake.Sprites
{
    class Projectile : Sprite
    {
        const int MAX_DISTANCE = 500;
        const float SPEED_MULTIPLIER = 2.0f;

        public bool Visible = false;

        public bool Disposable = false;

        public bool isEnemyProjectile = false;

        Vector2 mStartPosition;
        Vector2 mSpeed;
        Vector2 mDirection;

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, "projectile-1");
            Scale = 1.5f;
        }

        public void Update(GameTime theGameTime)
        {
            if (Position.Y < 0)
            {
                Visible = false;
            }

            if (Visible == true)
            {
                base.Update(theGameTime, mSpeed, mDirection);
            }
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            if (Visible == true && Disposable == false)
            {
                if (isEnemyProjectile == true)
                {
                    theSpriteBatch.Draw(mSpriteTexture, Position, Source, Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.FlipVertically, 0);
                }
                else
                {
                    theSpriteBatch.Draw(mSpriteTexture, Position, Source, Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    //base.Draw(theSpriteBatch);
                }
            }
        }

        public void Fire(Vector2 theStartPosition, Vector2 theSpeed, Vector2 theDirection)
        {
            Position = theStartPosition;
            mStartPosition = theStartPosition;
            mSpeed = theSpeed * SPEED_MULTIPLIER;
            mDirection = -theDirection;
            Visible = true;
        }
    }
}
