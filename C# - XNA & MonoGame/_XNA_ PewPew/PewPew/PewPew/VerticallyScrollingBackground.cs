using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PewPew
{
    class VerticallyScrollingBackground
    {
         //The Sprites that make up the images to be scrolled
        //across the screen.
        List<Sprite> mBackgroundSprites;
        
        //The viewing area for drawing the Scrolling background images within
        Viewport mViewport;

        //The Direction to scroll the background images
        public enum VerticalScrollDirection
        {
            Down,
            Up
        }

        public VerticallyScrollingBackground(Viewport theViewport)
        {
            mBackgroundSprites = new List<Sprite>();
            mViewport = theViewport;
        }

        public void LoadContent(ContentManager theContentManager)
        {

            //Cycle through all of the Background sprites that have been added
            //and load their content and position them.
            foreach (Sprite aBackgroundSprite in mBackgroundSprites)
            {
                //Load the sprite's content and apply it's scale, the scale is calculated by figuring
                //out how far the sprite needs to be stretech to make it fill the height of the viewport
                aBackgroundSprite.LoadContent(theContentManager, aBackgroundSprite.AssetName);
                aBackgroundSprite.Scale = mViewport.Width / aBackgroundSprite.Size.Width;
            }

            for (int i = 0; i < mBackgroundSprites.Count;i++)
            {
                if(i == 0)
                    mBackgroundSprites[i].Position = new Vector2(0, 400);
                else
                    mBackgroundSprites[i].Position = new Vector2(0, mBackgroundSprites[i - 1].Position.Y - mBackgroundSprites[i - 1].Size.Height);
            }
            
        }

        //Adds a background sprite to be scrolled through the screen
        public void AddBackground(string theAssetName)
        {
            Sprite aBackgroundSprite = new Sprite();
            aBackgroundSprite.AssetName = theAssetName;

            mBackgroundSprites.Add(aBackgroundSprite);
        }

        //Update the position of the background images
        public void Update(GameTime theGameTime, int theSpeed, VerticalScrollDirection theDirection)
        {

            for (int i = 0; i < mBackgroundSprites.Count; i++)
            {
                if (i == 0 && mBackgroundSprites[i].Position.Y > mBackgroundSprites[i].Size.Height+400)
                    mBackgroundSprites[i].Position.Y = mBackgroundSprites[mBackgroundSprites.Count-1].Position.Y - mBackgroundSprites[i].Size.Height;
                else if (i > 0 && mBackgroundSprites[i].Position.Y > mBackgroundSprites[i].Size.Height+400)
                    mBackgroundSprites[i].Position.Y = mBackgroundSprites[i - 1].Position.Y - mBackgroundSprites[i - 1].Size.Height;
            }

            //Set the Direction based on movement to the left or right that was passed in
            Vector2 aDirection = Vector2.Zero;
            if (theDirection == VerticalScrollDirection.Down)
            {
                aDirection.Y = 1;
            }
            else if (theDirection == VerticalScrollDirection.Up)
            {
                aDirection.Y = -1;
            }
            
            //Update the postions of each of the Background sprites
            foreach (Sprite aBackgroundSprite in mBackgroundSprites)
            {
                aBackgroundSprite.Update(theGameTime, new Vector2(0, theSpeed), aDirection);
            }
        }

        //Draw the background images to the screen
        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Sprite aBackgroundSprite in mBackgroundSprites)
            {
                aBackgroundSprite.Draw(theSpriteBatch);
            }
        }
    }
}
