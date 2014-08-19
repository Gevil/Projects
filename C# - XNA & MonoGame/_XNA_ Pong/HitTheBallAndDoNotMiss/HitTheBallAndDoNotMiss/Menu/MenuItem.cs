using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace HitTheBallAndDoNotMiss
{
    public class MenuItem
    {
        public bool Selected = false;
        public Vector2 Position;
        public string Text;
        public bool Disabled = false;
        public EventHandler Event;

        SpriteFont mMenuFont;

        public Color SelectedColor = Color.Khaki;
        public Color DisabledColor = Color.DarkGray;
        public Color DefaultColor = Color.Black;

        public MenuItem(ContentManager theContent, string theText, Vector2 thePosition, EventHandler theEvent)
        {
            mMenuFont = theContent.Load<SpriteFont>("Fonts/menuFont");
            Text = theText;
            Position = thePosition;
            Event = theEvent;
        }

        public Vector2 Length()
        {
            return mMenuFont.MeasureString(Text);
        }

        float mSelectedScale = 1.0f;
        float mScaleAdjustment = 0.01f;
        double mAdjustScaleTime = 25;
        public void Update(GameTime theGameTime)
        {
            mAdjustScaleTime -= theGameTime.ElapsedGameTime.TotalMilliseconds;
            if (mAdjustScaleTime < 0)
            {
                mAdjustScaleTime = 25;
                mSelectedScale += mScaleAdjustment;
                if (mSelectedScale > 1.05)
                {
                    mScaleAdjustment = -0.01f;
                }

                if (mSelectedScale < .95)
                {
                    mScaleAdjustment = 0.01f;
                }
            }
        }

        public void Draw(SpriteBatch theBatch)
        {
            if (Disabled == true)
            {
                theBatch.DrawString(mMenuFont, Text, Position, DisabledColor);
            }
            else if (Selected == true)
            {
                Vector2 aLength = mMenuFont.MeasureString(Text);
                Vector2 aCenter = new Vector2(aLength.X / 2, aLength.Y / 2);
                
                theBatch.DrawString(mMenuFont, Text, new Vector2(Position.X + aCenter.X, Position.Y + aCenter.Y), SelectedColor, 0.0f, aCenter, mSelectedScale, SpriteEffects.None, 0);
            }
            else
            {
                theBatch.DrawString(mMenuFont, Text, Position, DefaultColor);
            }
        }
    }
}