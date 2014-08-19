using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace PewPew.Screens
{
    class MainMenu : Screen
    {
        //Background texture
        Texture2D mMainMenuBackground;

        public MainMenu(ContentManager theContent, EventHandler theScreenEvent): base(theScreenEvent)
        {
            //Load the background texture
            mMainMenuBackground = theContent.Load<Texture2D>("BackgroundMenu");
        }

        //Update all of the elements that need updating
        public override void Update(GameTime theTime)
        {
            //Check to see if the Player one controller has pressed the "B" button, if so, then
            //call the screen event associated with this screen
            if (GamePad.GetState(PlayerOne).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.B) == true)
            {                
                ScreenEvent.Invoke(this, new EventArgs());
            }

            base.Update(theTime);
        }

        //Draw all of the elements that make up the MainMenu Screen
        public override void Draw(SpriteBatch theBatch)
        {
            theBatch.Draw(mMainMenuBackground, Vector2.Zero, Color.White); 
            base.Draw(theBatch);
        }
    }
}
