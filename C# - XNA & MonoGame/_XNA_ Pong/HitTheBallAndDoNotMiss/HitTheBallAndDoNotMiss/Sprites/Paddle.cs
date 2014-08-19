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
    class Paddle : MoveableSprite
    {
        public Input mInput;

        public enum PaddlePosition
        {
            Left,
            Right
        }
        public PaddlePosition mPaddlePosition;

        public Paddle(ContentManager theContent, string theAssetName, Input theInput, PaddlePosition thePosition)
            : base(theContent, theAssetName)
        {
            SourceRectangle = new Rectangle(0, 0, 1, 1);
            Scale = new Vector2(10.0f, 100.0f);
            mColor = Color.SlateBlue;

            mPaddlePosition = thePosition;
            switch (thePosition)
            {
                case PaddlePosition.Left:
                    {
                        Boundary = new Rectangle(140, 185, 10, 833);
                        break;
                    }

                case PaddlePosition.Right:
                    {
                        Boundary = new Rectangle(1130, 185, 10, 833);
                        break;
                    }
            }

            Position = Center(Boundary);

            mInput = theInput;          
        }

        public Vector2 mMovement = new Vector2(0, 0);

        public void Update(GameTime theGametime)
        {
            mInput.BeginUpdate();
            mMovement = Vector2.Zero;

            if (mInput.Up())
            {
                mMovement = new Vector2(0, -3);
            }

            if (mInput.Down())
            {
                mMovement = new Vector2(0, 3);
            }

            Move(mMovement);
            BoundaryCollisionCheck(false);

            mInput.EndUpdate();
        }
    }
}
