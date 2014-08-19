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
    public class Sprite
    {
        protected Texture2D mTexture;
        string mAssetName;
        public Vector2 Position;
        public Rectangle SourceRectangle;
        public Color mColor;
        public float Rotation;
        Vector2 mOrigin;
        public Vector2 Scale;
        SpriteEffects mSpriteEffects;
        float mLayerDepth;
        //Vector2 mStretch;

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)(SourceRectangle.Width * Scale.X), (int)(SourceRectangle.Height * Scale.Y));
            }
        }        

        public Sprite(ContentManager theContent, string theAssetName)
        {
            mTexture = theContent.Load<Texture2D>(theAssetName);
            mAssetName = theAssetName;
            Position = Vector2.Zero;
            SourceRectangle = new Rectangle(0, 0, mTexture.Width, mTexture.Height);
            mColor = Color.White;
            Rotation = 0.0f;
            mOrigin = Vector2.Zero; //new Vector2(mSourceRectangle.Width / 2, mSourceRectangle.Height / 2);
            Scale = new Vector2(1.0f, 1.0f);
            mSpriteEffects = SpriteEffects.None;
            mLayerDepth = 0.0f;
            //mStretch = new Vector2(mTexture.Width, mTexture.Height);
        }

        public virtual void Draw(SpriteBatch theBatch)
        {
            theBatch.Draw(mTexture, Position, SourceRectangle, mColor, Rotation, mOrigin, Scale, mSpriteEffects, mLayerDepth);
        }

        public Vector2 Center(Rectangle theArea)
        {
            //int aXPosition = (int)MathHelper.Clamp(((theArea.Width) / 2) - (int)(CollisionRectangle.Width / 2) + theArea.X, theArea.X, theArea.Width);
            //int aYPosition = (int)MathHelper.Clamp(((theArea.Height) / 2) - (int)(CollisionRectangle.Height / 2) + theArea.Y, theArea.Y, theArea.Height);

            int aXPosition = (theArea.Width / 2) - (CollisionRectangle.Width / 2);
            int aYPosition = (theArea.Height / 2) - (CollisionRectangle.Height / 2);
            //012345678901234567890
            //'  x     aa      x '
            //width/2 - a/2 + x
            //a=2, width=34

            return new Vector2(aXPosition, aYPosition);
        }
    }
}
