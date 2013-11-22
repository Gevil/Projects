using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace Tyrian_Remake.Screens
{
    class InGameScreen : Screen
    {
        //Define content here

        public InGameScreen(ContentManager theContent, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            //Load the background texture for the screen
            
        }

        //Update all of the elements that need updating in the Title Screen
        public override void Update(GameTime theTime)
        {
            base.Update(theTime);
        }

        //Draw all of the elements that make up the Title Screen
        public override void Draw(SpriteBatch theBatch)
        {

            base.Draw(theBatch);
        }

    }
}
