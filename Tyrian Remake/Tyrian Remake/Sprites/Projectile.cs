

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tyrian_Remake.Sprites
{
    class Projectile : Sprite
    {
        const int MaxDistance = 500;
        const float SpeedMultiplier = 2.0f;

        public bool Visible = false;

        public bool Disposable = false;

        public bool IsEnemyProjectile = false;

        Vector2 _mStartPosition;
        Vector2 _mSpeed;
        Vector2 _mDirection;

        public void LoadContent(ContentManager theContentManager)
        {
            LoadContent(theContentManager, "projectile-1");
            Scale = 1.0f;
        }

        public void Update(GameTime theGameTime)
        {
            if (Position.Y < 0)
            {
                Visible = false;
            }

            if (Visible)
            {
                Update(theGameTime, _mSpeed, _mDirection);
            }
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            if (!Visible || Disposable) return;
            theSpriteBatch.Draw(MSpriteTexture, Position, Source, Color.White, 0.0f, Vector2.Zero, Scale,
                IsEnemyProjectile ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
        }

        public void Fire(Vector2 theStartPosition, Vector2 theSpeed, Vector2 theDirection)
        {
            Position = theStartPosition;
            _mStartPosition = theStartPosition;
            _mSpeed = theSpeed * SpeedMultiplier;
            _mDirection = -theDirection;
            Visible = true;
        }
    }
}
