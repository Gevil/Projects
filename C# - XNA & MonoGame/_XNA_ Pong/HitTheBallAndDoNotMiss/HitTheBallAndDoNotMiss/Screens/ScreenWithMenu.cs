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
    public class ScreenWithMenu : Screen
    {
        public Menu mMenu;
        public EventHandler MenuSelected;

        public ScreenWithMenu(ContentManager theContent, Viewport theViewport, EventHandler theEventHandler, PlayerIndex theCurrentPlayer) : base(theContent, theViewport, theEventHandler) 
        {
            mMenu = new Menu(theContent, theViewport, theCurrentPlayer);
            CreateMenu();

            MenuSelected += theEventHandler;
        }

        public override void Update(GameTime theGameTime)
        {
            mMenu.Update(theGameTime);
            base.Update(theGameTime);
        }

        public override void Draw(SpriteBatch theBatch)
        {
            mMenu.Draw(theBatch);
            base.Draw(theBatch);
        }

        private void MenuItemSelected(object sender, EventArgs e)
        {
        }

        protected virtual void CreateMenu()
        {
        }

    }
}
