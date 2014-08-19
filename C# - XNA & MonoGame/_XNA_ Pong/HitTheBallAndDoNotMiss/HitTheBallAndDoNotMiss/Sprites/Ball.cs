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
    class Ball: MoveableSprite
    {
        SoundEffect mHit;
        
        public Ball(ContentManager theContent, string theAssetName)
            : base(theContent, theAssetName)
        {
            SourceRectangle = new Rectangle(2, 2, base.mTexture.Width - 2, base.mTexture.Height - 2);
            Scale = new Vector2(0.1f, 0.1f);
            Boundary = new Rectangle(130, 187, 1280, 830);
            mColor = Color.SlateBlue;

            mHit = theContent.Load<SoundEffect>("SoundFX/Hit");
            
            Reset();
        }

        int mVelocity = 3;
        public Vector2 mMovement;
        public Direction OutOfBounds = Direction.Top;

        public void Reset()
        {
            Position = Center(Boundary);
            mVelocity *= -1;
        }

        public void Start()
        {
            mMovement = new Vector2(-mVelocity, mVelocity);
            OutOfBounds = Direction.None;
        }
        
        public void Update(GameTime theGametime, Paddle thePaddleOne, Paddle thePaddleTwo)
        {
            Move(mMovement);

            switch (BoundaryCollisionCheck(true))
            {
                case Direction.Left:
                    {
                        OutOfBounds = Direction.Left;
                        Reset();
                        break;
                    }

                case Direction.Right:
                    {
                        OutOfBounds = Direction.Right;
                        Reset();
                        break;
                    }

                case Direction.Top:
                    {
                        mMovement.Y *= -1;
                        mHit.Play();
                        break;
                    }

                case Direction.Bottom:
                    {
                        mMovement.Y *= -1;
                        mHit.Play();
                        break;
                    }
            }

            CheckCollision(thePaddleOne);
            CheckCollision(thePaddleTwo);
        }

        public void CheckCollision(Paddle thePaddle)
        {   
            if (thePaddle.CollisionRectangle.Intersects(CollisionRectangle))
            {
                bool aMovingRight = (mMovement.X > 0);
                bool aMovingLeft = (mMovement.X < 0);
                bool aLeftOfPaddle = (Position.X < thePaddle.Position.X);
                bool aRightOfPaddle = (Position.X > thePaddle.Position.X);
                if (( aMovingRight && aLeftOfPaddle ) || (aMovingLeft && aRightOfPaddle))
                {
                    mMovement.X *= -1;
                    mHit.Play();
                }
            }
        }
    }
}
