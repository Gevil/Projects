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
    public class Screen
    {
        static SafeArea mSafeArea;

        protected static Viewport mViewport;
        protected static ContentManager mContent;

        public Color GraphicsDeviceClearColor = Color.CornflowerBlue;
        public EventHandler ScreenEvent;
        
        public Screen(ContentManager theContent, Viewport theViewport, EventHandler theScreenEvent)
        {            
            mViewport = theViewport;
            mContent = theContent;

            ScreenEvent = theScreenEvent;

            mSafeArea = new SafeArea(theContent);
        }

        public virtual void Update(GameTime theGametime)
        {
            mSafeArea.Update(theGametime);
        }

        public virtual void Draw(SpriteBatch theBatch)
        {
            mSafeArea.Draw(theBatch);
        }
    }
}
