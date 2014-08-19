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
    class AIInput : Input
    {
        Ball mBall;
        public Paddle mPaddle;

        Viewport mViewport;

        public AIInput(Ball theBall, Paddle thePaddle, Viewport theViewport) : base(PlayerIndex.One)
        {
            mBall = theBall;
            mPaddle = thePaddle;
            mViewport = theViewport;
        }       

        public override bool Up()
        {
            bool aIsPressed = false;
            if (CheckAIMovement() == true)
            {
                if (mBall.mMovement.X > 0 && mBall.Position.Y < mPaddle.Position.Y) //mBall.Position.Y > (mViewport.Height * .12))
                {
                    aIsPressed = true;
                }
                //else if (mBall.Position.X > mViewport.Height * .88)
                //{
                //    aIsPressed = true;
                //}
            }

            return aIsPressed;
        }

        public override bool Down()
        {
            bool aIsPressed = false;
            if (CheckAIMovement() == true)
            {
                if (mBall.mMovement.X > 0 && mBall.Position.Y > mPaddle.Position.Y)  //mBall.Position.Y < (mViewport.Height * .88))
                {
                    aIsPressed = true;
                }
                //else if (mBall.Position.Y < (mViewport.Height * .12))
                //{
                //    aIsPressed = true;
                //}
            }

            return aIsPressed;
        }

        private bool CheckAIMovement()
        {
            bool aIsPressed = false;
            if (mPaddle.mPaddlePosition == Paddle.PaddlePosition.Left)
            {
                bool aIsBallMovingTowardsPaddle = mBall.mMovement.X < 0;
                bool aIsBallOverHalfway = mBall.Position.X < (mViewport.Width/2 - 10);
                if (aIsBallMovingTowardsPaddle && aIsBallOverHalfway)
                {
                    aIsPressed = true;
                }
            }
            else
            {
                bool aIsBallMovingTowardsPaddle = mBall.mMovement.X > 0;
                bool aIsBallOverHalfway = mBall.Position.X > (mViewport.Width/2 + 10);
                if (aIsBallMovingTowardsPaddle && aIsBallOverHalfway)
                {
                    aIsPressed = true;
                }
            }

            return aIsPressed;
        }
    }
}
