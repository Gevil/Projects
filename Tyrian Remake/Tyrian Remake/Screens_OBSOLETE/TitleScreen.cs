using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Tyrian_Remake.Screens
{
    class TitleScreen : Screen
    {
        //Background texture for the Title screen
        Texture2D mTitleScreenBackground;
        SoundEffect MenuMusic;
        public SoundEffectInstance MenuMusicInstance;

        public TitleScreen(ContentManager theContent, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            //Load the background texture for the screen
            mTitleScreenBackground = theContent.Load<Texture2D>("title-bg-1920-1080");

            MenuMusic = theContent.Load<SoundEffect>("Tyrian Song");
            MenuMusicInstance = MenuMusic.CreateInstance();
        }

        //Update all of the elements that need updating in the Title Screen
        public override void Update(GameTime theTime)
        {
            //Call the screen event associated with this screen
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                ScreenEvent.Invoke(this, new EventArgs());
                return;
            }

            if (MenuMusicInstance.State != SoundState.Playing)
            {
                MenuMusicInstance.Volume = 1f;
                MenuMusicInstance.IsLooped = true;
                MenuMusicInstance.Play();
            }

            //TODO: Create Menu items with hover animation
            


            base.Update(theTime);
        }

        //Draw all of the elements that make up the Title Screen
        public override void Draw(SpriteBatch theBatch)
        {
            theBatch.Draw(mTitleScreenBackground, Vector2.Zero, Color.White);
            base.Draw(theBatch);
        }


    }
}
