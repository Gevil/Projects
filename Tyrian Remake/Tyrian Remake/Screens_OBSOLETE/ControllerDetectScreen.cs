using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;

namespace Tyrian_Remake.Screens
{
    class ControllerDetectScreen : Screen
    {
        //Background texture for the screen
        Texture2D mControllerDetectScreenBackground;

        SpriteFont TextFont;

        public ControllerDetectScreen(ContentManager theContent, EventHandler theScreenEvent)
            : base(theScreenEvent)
        {
            //Load the background texture for the screen
            mControllerDetectScreenBackground = theContent.Load<Texture2D>("title-bg-1920-1080");

            theContent.RootDirectory = "Content";
            TextFont = theContent.Load<SpriteFont>("Downlink");
        }

        //Update all of the elements that need updating in the Controller Detect Screen
        public override void Update(GameTime theTime)
        {
            //Poll all the gamepads (and the keyboard) to check to see
            //which controller will be the player one controller. When the controlling
            //controller is detected, call the screen event associated with this screen
            for (int aPlayer = 0; aPlayer < 4; aPlayer++)
            {
                if (GamePad.GetState((PlayerIndex)aPlayer).Buttons.A == ButtonState.Pressed || Keyboard.GetState().GetPressedKeys().Length > 0)
                {
                    PlayerOne = (PlayerIndex)aPlayer;
                    ScreenEvent.Invoke(this, new EventArgs());
                    return;
                }
            }

            base.Update(theTime);
        }

        //Draw all of the elements that make up the Controller Detect Screen
        public override void Draw(SpriteBatch theBatch)
        {
            theBatch.Draw(mControllerDetectScreenBackground, Vector2.Zero, Color.White);

            Vector2 textPos = new Vector2(20, 60);
            theBatch.DrawString(TextFont, "Press Any key to Continue", textPos, Color.Black);

            base.Draw(theBatch);
        }

    }
}
