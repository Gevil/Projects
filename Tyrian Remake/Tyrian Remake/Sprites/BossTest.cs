using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tyrian_Remake.Sprites
{
    class BossTest : Sprite
    {
        const string ShipAssetname = "boss-test";
        const int StartPositionX = 450;
        const int StartPositionY = 70;
        const int ShipSpeed = 170;
        const int MoveUp = -1;
        const int MoveDown = 1;
        const int MoveLeft = -1;
        const int MoveRight = 1;

        enum State
        {
            Moving
        }

        private const State MCurrentState = State.Moving;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        KeyboardState _mPreviousKeyboardState;

        Vector2 _mStartingPosition = Vector2.Zero;

        public List<Projectile> MProjectiles = new List<Projectile>();

        ContentManager _mContentManager;

        const double FireRate = 1.5;
        double _fireTime = 0;

        public bool IsHit = false;

        public void LoadContent(ContentManager theContentManager)
        {
            _mContentManager = theContentManager;

            foreach (var aProjectile in MProjectiles)
            {
                aProjectile.LoadContent(theContentManager);
            }

            Position = new Vector2(StartPositionX, StartPositionY);
            LoadContent(theContentManager, ShipAssetname);
            Source = new Rectangle(0, 0, Source.Width, Source.Height);

        }

        public void Update(GameTime theGameTime, GraphicsDeviceManager graphics)
        {
            _fireTime += 0.1;
            var aCurrentKeyboardState = Keyboard.GetState();

            UpdateProjectile(theGameTime, aCurrentKeyboardState);

            _mPreviousKeyboardState = aCurrentKeyboardState;

            Update(theGameTime, mSpeed, mDirection);
        }

        private void UpdateProjectile(GameTime theGameTime, KeyboardState aCurrentKeyboardState)
        {
            foreach (var aProjectile in MProjectiles)
            {
                aProjectile.Update(theGameTime);
            }

            if (aCurrentKeyboardState.IsKeyDown(Keys.Space) != true || !(_fireTime >= FireRate)) return;
            _fireTime = 0;
            ShootProjectile();
        }

        private void ShootProjectile()
        {
            var aProjectile = new Projectile {IsEnemyProjectile = true};
            aProjectile.LoadContent(_mContentManager);
                    
            aProjectile.Fire(Position + new Vector2(Size.Width / 4, Size.Height), new Vector2(-200, -200), new Vector2(0, 1));
            MProjectiles.Add(aProjectile);
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Projectile aProjectile in MProjectiles)
            {
                aProjectile.Draw(theSpriteBatch);
            }

            base.Draw(theSpriteBatch);
        }
    }
}
