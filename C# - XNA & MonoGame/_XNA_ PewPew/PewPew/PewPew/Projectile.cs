//using System;
//using System.Collections.Generic;
//using System.Text;

using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Storage;
//using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PewPew
{
    class Projectile : Sprite
    {
        const int MAX_DISTANCE = 500;

        public bool Visible =  false;

        Vector2 mStartPosition;
        Vector2 mSpeed;
        Vector2 mDirection;
        
        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, "bullet");
            Scale = 0.5f;
        }

        public void Update(GameTime theGameTime)
        {
            if (Vector2.Distance(mStartPosition, Position) > MAX_DISTANCE)
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
            if (Visible == true)
            {
                base.Draw(theSpriteBatch);
            }
        }

        public void Fire(Vector2 theStartPosition, Vector2 theSpeed, Vector2 theDirection)
        {
            Position = theStartPosition;
            mStartPosition = theStartPosition;
            mSpeed = theSpeed;
            mDirection = -theDirection;
            Visible = true;
        }



    }
}
