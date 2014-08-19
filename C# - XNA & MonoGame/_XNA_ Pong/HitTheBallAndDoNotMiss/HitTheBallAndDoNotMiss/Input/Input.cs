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
    public class Input
    {
        GamePadState mCurrentGamePadState;
        GamePadState mPreviousGamePadState;

#if (WINDOWS) 
        KeyboardState mCurrentKeyboardState;
        KeyboardState mPreviousKeyboardState;
#endif

        PlayerIndex mPlayerIndex;

        KeyboardDefinition mKeys = new KeyboardDefinition();

        private void DefineKeys(PlayerIndex thePlayerIndex)
        {
            switch (mPlayerIndex)
            {
                case PlayerIndex.One:
                    {
                        mKeys.Up = Keys.Up;
                        mKeys.Down = Keys.Down;
                        mKeys.Left = Keys.Left;
                        mKeys.Right = Keys.Right;
                        break;
                    }
                case PlayerIndex.Two:
                    {
                        mKeys.Up = Keys.W;
                        mKeys.Down = Keys.S;
                        mKeys.Left = Keys.A;
                        mKeys.Right = Keys.D;
                        break;
                    }
            }
        }

        public enum Thumbstick
        {
            Left,
            Right
        }

        public Input(PlayerIndex thePlayerIndex)
        {
            mPlayerIndex = thePlayerIndex;
            mPreviousGamePadState = GamePad.GetState(mPlayerIndex);

#if (WINDOWS)
            mPreviousKeyboardState = Keyboard.GetState();
#endif
            DefineKeys(mPlayerIndex);
        }
                
        public virtual void BeginUpdate()
        {
            mCurrentGamePadState = GamePad.GetState(mPlayerIndex);

#if (WINDOWS)
            mCurrentKeyboardState = Keyboard.GetState();
#endif
        }

        public virtual Vector2 LeftThumbstick()
        {
            return mCurrentGamePadState.ThumbSticks.Left;
        }

        public virtual Vector2 RightThumbstick()
        {
            return mCurrentGamePadState.ThumbSticks.Right;
        }

        public virtual bool Left()
        {
            return Left(Thumbstick.Left);
        }

        public virtual bool Left(Thumbstick theThumbstick)
        {
            bool aIsPressed = false;

            switch (theThumbstick)
            {
                case Thumbstick.Left:
                    {
                        if (mCurrentGamePadState.DPad.Left == ButtonState.Pressed || mCurrentKeyboardState.IsKeyDown(mKeys.Left))
                        {
                            aIsPressed = true;
                        }
                        else if (mCurrentGamePadState.ThumbSticks.Left.X < 0)
                        {
                            aIsPressed = true;
                        }
                        break;
                    }
                case Thumbstick.Right:
                    {
                        if (mCurrentGamePadState.ThumbSticks.Right.X < 0)
                        {
                            aIsPressed = true;
                        }
                        break;
                    }
            }

            return aIsPressed;        
        }
        
        public virtual bool Right()
        {
            bool aIsPressed = false;
            if (mCurrentGamePadState.DPad.Right == ButtonState.Pressed || mCurrentKeyboardState.IsKeyDown(mKeys.Right))
            {
                aIsPressed = true;
            }
            else if (mCurrentGamePadState.ThumbSticks.Left.X > 0)
            {
                aIsPressed = true;
            }
            return aIsPressed;
        }

        public virtual bool Up()
        {
            bool aIsPressed = false;
            if (mCurrentGamePadState.DPad.Up == ButtonState.Pressed || mCurrentKeyboardState.IsKeyDown(mKeys.Up))
            {
                aIsPressed = true;
            }
            else if (mCurrentGamePadState.ThumbSticks.Left.Y > 0)
            {
                aIsPressed = true;
            }
            return aIsPressed;
        }

        public virtual bool Down()
        {
            bool aIsPressed = false;
            if (mCurrentGamePadState.DPad.Down == ButtonState.Pressed || mCurrentKeyboardState.IsKeyDown(mKeys.Down))
            {
                aIsPressed = true;
            }
            else if (mCurrentGamePadState.ThumbSticks.Left.Y < 0)
            {
                aIsPressed = true;
            }
            return aIsPressed;
        }

        public virtual bool AWithRelease()
        {
            bool aIsPressed = false;
            if (mCurrentGamePadState.Buttons.A == ButtonState.Pressed && mPreviousGamePadState.Buttons.A == ButtonState.Released)
            {
                aIsPressed = true;
            }
            else if (mCurrentKeyboardState.IsKeyDown(Keys.A) == true && mPreviousKeyboardState.IsKeyDown(Keys.A) == false)
            {
                aIsPressed = true;
            }
            else if (mCurrentKeyboardState.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
            {
                aIsPressed = true;
            }
            else if (mCurrentKeyboardState.IsKeyDown(Keys.Enter) == true && mPreviousKeyboardState.IsKeyDown(Keys.Enter) == false)
            {
                aIsPressed = true;
            }

            return aIsPressed;
        }

        public virtual bool DownWithRelease()
        {
            bool aIsPressed = false;
            if (mCurrentGamePadState.DPad.Down == ButtonState.Pressed && mPreviousGamePadState.DPad.Down == ButtonState.Released)
            {
                aIsPressed = true;
            }
            else if (mCurrentGamePadState.ThumbSticks.Left.Y < 0 && mPreviousGamePadState.ThumbSticks.Left.Y >= 0)
            {
                aIsPressed = true;
            }
            else if (mCurrentKeyboardState.IsKeyDown(Keys.Down) == true && mPreviousKeyboardState.IsKeyDown(Keys.Down) == false)
            {
                aIsPressed = true;
            }
            return aIsPressed;
        }

        public virtual bool UpWithRelease()
        {
            bool aIsPressed = false;
            if (mCurrentGamePadState.DPad.Up == ButtonState.Pressed && mPreviousGamePadState.DPad.Up == ButtonState.Released)
            {
                aIsPressed = true;
            }
            else if (mCurrentGamePadState.ThumbSticks.Left.Y > 0 && mPreviousGamePadState.ThumbSticks.Left.Y <= 0)
            {
                aIsPressed = true;
            }
            else if (mCurrentKeyboardState.IsKeyDown(Keys.Up) == true && mPreviousKeyboardState.IsKeyDown(Keys.Up) == false)
            {
                aIsPressed = true;
            }
            return aIsPressed;
        }

        public virtual bool F1WithRelease()
        {
            bool aIsPressed = false;
#if (WINDOWS) 

            if (mCurrentKeyboardState.IsKeyDown(Keys.F1) == true && mPreviousKeyboardState.IsKeyDown(Keys.F1) == false)
            {
                aIsPressed = true;
            }            
#endif

            return aIsPressed;
        }

        public void EndUpdate()
        {
            mPreviousGamePadState = mCurrentGamePadState;
#if (WINDOWS)
            mPreviousKeyboardState = mCurrentKeyboardState;
#endif
        }
    }

}
