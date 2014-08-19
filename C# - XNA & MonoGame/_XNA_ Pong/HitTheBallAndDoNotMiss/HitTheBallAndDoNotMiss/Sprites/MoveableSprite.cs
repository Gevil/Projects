using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace HitTheBallAndDoNotMiss
{
    public class MoveableSprite : Sprite
    {
        public Rectangle Boundary;

        public MoveableSprite(ContentManager theContent, string theAssetName)
            : base(theContent, theAssetName)
        {
        }

        public void Move(Vector2 theMovement)
        {
            Position += theMovement;
        }

        public enum Direction
        {
            Left,
            Right,
            Top,
            Bottom,
            None
        }

        public Direction BoundaryCollisionCheck(bool notifyOnly)
        {
            Direction aDirection = Direction.None;

            if (Position.X + CollisionRectangle.Width > Boundary.Width - Boundary.X)
            {
                if (notifyOnly == false)
                {
                    Position.X = Boundary.Width - Boundary.X - CollisionRectangle.Width;
                }
                aDirection = Direction.Right;
            }

            if (Position.Y + CollisionRectangle.Height > Boundary.Height - Boundary.Y)
            {
                if (notifyOnly == false)
                {
                    Position.Y = Boundary.Height - Boundary.Y - CollisionRectangle.Height;
                }
                aDirection = Direction.Bottom;
            }

            if (Position.X < Boundary.X)
            {
                if (notifyOnly == false)
                {
                    Position.X = Boundary.X;
                }
                aDirection = Direction.Left;
            }

            if (Position.Y < Boundary.Y)
            {
                if (notifyOnly == false)
                {
                    Position.Y = Boundary.Y;
                }
                aDirection = Direction.Top;
            }

            return aDirection;
        }
    }
}
