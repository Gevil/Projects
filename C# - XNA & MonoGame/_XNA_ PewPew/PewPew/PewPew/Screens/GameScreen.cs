using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PewPew.Screens
{
    class GameScreen : Screen
    {
        PShip mPShipSprite;
        Projectile mProjectileSprite;

        //Create a Vertically scrolling background
        VerticallyScrollingBackground mScrollingBackground;

        public GameScreen(ContentManager theContent, EventHandler theScreenEvent): base(theScreenEvent)
        {

        }

        protected override void LoadContent(ContentManager theContentManager, GraphicsDevice theGraphicsDevice)
        {
            mPShipSprite = new PShip();
            mProjectileSprite = new Projectile();

            //Initialize and add the background images to the Scrolling background. You can change the
            //scroll area by passing in a different Viewport. The images will then scale and scroll within
            //that given Viewport.
            mScrollingBackground = new VerticallyScrollingBackground(theGraphicsDevice.Viewport);
            mScrollingBackground.AddBackground("Background1");
            mScrollingBackground.AddBackground("Background2");
            mScrollingBackground.AddBackground("Background3");
            mScrollingBackground.AddBackground("Background4");
            mScrollingBackground.AddBackground("Background5");
            mScrollingBackground.AddBackground("Background6");
            mScrollingBackground.AddBackground("Background7");
            mScrollingBackground.AddBackground("Background8");
            mScrollingBackground.AddBackground("Background9");

            //Load the content for the Scrolling background
            mScrollingBackground.LoadContent(theContentManager);

            mProjectileSprite.LoadContent(theContentManager);
            mPShipSprite.LoadContent(theContentManager);
        }

        protected override void Update(GameTime gameTime, GraphicsDeviceManager theGraphicsDeviceManager)
        {
            //Poll all the gamepads (and the keyboard) to check to see
            //which controller will be the player one controller. When the controlling
            //controller is detected, call the screen event associated with this screen
            for (int aPlayer = 0; aPlayer < 4; aPlayer++)
            {
                if (GamePad.GetState((PlayerIndex)aPlayer).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) == true)
                {
                    PlayerOne = (PlayerIndex)aPlayer;
                    ScreenEvent.Invoke(this, new EventArgs());
                    return;
                }
            }

            //Update the scrolling backround. You can scroll to the left or to the right by changing the scroll direction
            mScrollingBackground.Update(gameTime, 160, VerticallyScrollingBackground.VerticalScrollDirection.Down);

            mPShipSprite.Update(gameTime, theGraphicsDeviceManager);
            mProjectileSprite.Update(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(SpriteBatch theBatch)
        {

            theBatch.Begin();

            //Draw the scrolling background
            mScrollingBackground.Draw(theBatch);

            //Draw Player Ship here
            mPShipSprite.Draw(theBatch);

            theBatch.End();

            base.Draw(theBatch);
        }

    }
}
