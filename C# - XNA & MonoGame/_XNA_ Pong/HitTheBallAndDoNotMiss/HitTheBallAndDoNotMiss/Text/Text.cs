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
    public class Text
    {
        SpriteFont mFont;
        public string mText;
        string mFontAssetName;
        public Vector2 Position;
        public Color mColor;

        public enum Alignment
        {
            Horizonatal,
            Vertical,
            Both
        }

        public Text(ContentManager theContent, string theText, string theFontAssetName, Vector2 theStartingPosition, Color theColor)
        {
            mFont = theContent.Load<SpriteFont>(theFontAssetName);
            mFontAssetName = theFontAssetName;

            mText = theText;
            Position = theStartingPosition;
            mColor = theColor;
        }

        public virtual void Draw(SpriteBatch theBatch)
        {
            theBatch.DrawString(mFont, mText, Position, mColor);
        }

        public void Center(Rectangle theDisplayArea, Alignment theAlignment)
        {
            Vector2 theTextSize = mFont.MeasureString(mText);

            switch (theAlignment)
            {
                case Alignment.Horizonatal:
                    {
                        Position.X = theDisplayArea.X + (theDisplayArea.Width / 2 - theTextSize.X / 2);
                        break;
                    }

                case Alignment.Vertical:
                    {
                        Position.Y = theDisplayArea.Y + (theDisplayArea.Height / 2 - theTextSize.Y / 2);                        
                        break;
                    }

                case Alignment.Both:
                    {
                        Position.X = theDisplayArea.X + (theDisplayArea.Width / 2 - theTextSize.X / 2);
                        Position.Y = theDisplayArea.Y + (theDisplayArea.Height / 2 - theTextSize.Y / 2); 
                        break;
                    }
            }
        }
    }
}
