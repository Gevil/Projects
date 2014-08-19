using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace HitTheBallAndDoNotMiss
{
    public class BlinkingText : Text
    {
        double mBlinkSpan;
        double mBlink;
        bool mDrawText = true;

        public BlinkingText(ContentManager theContent, string theText, string theFontAssetName, Vector2 thePosition, Color theColor, double theBlinkSpan)
            : base(theContent, theText, theFontAssetName, thePosition, theColor)
        {
            mBlinkSpan = theBlinkSpan;
        }

        public void Update(GameTime theGameTime)
        {
            mBlink -= theGameTime.ElapsedGameTime.TotalMilliseconds;
            if (mBlink < 0)
            {
                mBlink = mBlinkSpan;
                mDrawText = !mDrawText;
            }
        }

        public override void Draw(SpriteBatch theBatch)
        {
            if (mDrawText == true)
            {
                base.Draw(theBatch);
            }
        }
    }
}
