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
    public class SafeArea : Sprite
    {
        const string mSafeAreaAssetName = "SafeArea720";

        Input mInput;
        bool mVisible = false;

        public SafeArea(ContentManager theContent) : base(theContent, mSafeAreaAssetName)
        {
            mInput = new Input(PlayerIndex.One);
        }

        public void Update(GameTime theGametime)
        {
            mInput.BeginUpdate();
            if (mInput.F1WithRelease() == true)
            {
                mVisible = !mVisible;
            }

            mInput.EndUpdate();
        }

        public override void Draw(SpriteBatch theBatch)
        {
            if (mVisible == true)
            {
                base.Draw(theBatch);
            }
        }
    }
}
