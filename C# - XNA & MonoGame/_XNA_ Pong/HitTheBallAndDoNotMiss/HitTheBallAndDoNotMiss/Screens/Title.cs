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
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Media;

namespace HitTheBallAndDoNotMiss
{
    class Title : ScreenWithMenu
    {
        Sprite mBackground;
        Sprite mSquare;

        public Title(ContentManager theContent, Viewport theViewport, EventHandler theEvent)
            : base(theContent, theViewport, theEvent, PlayerIndex.One)
        {
            GraphicsDeviceClearColor = Color.SlateGray;
            mBackground = new Sprite(theContent, "Sprites/PongTitle");
            mSquare = new Sprite(theContent, "Sprites/MessageBox");
            mSquare.Position = new Vector2(100, 100);
            mSquare.Scale = new Vector2(1.0f, 1.0f);
            mSquare.Position = mSquare.Center(new Rectangle(0, 0, 1280, 720));
            mSquare.Position.Y += 120;
        }

        protected override void CreateMenu()
        {
            mMenu.AddMenuItem("Exit Game", false, false, this.ScreenEvent);
            mMenu.AddMenuItem("2 Player", false, false, this.ScreenEvent);
            mMenu.AddMenuItem("1 Player", false, true, this.ScreenEvent);

            mMenu.ChangeMenuColors(Color.Black, Color.SlateGray, Color.Tan);
            
            base.CreateMenu();
        }
        
        public override void Update(GameTime theGameTime)
        {
            base.Update(theGameTime);
        }

        public override void Draw(SpriteBatch theBatch)
        {
            mBackground.Draw(theBatch);
            mSquare.Draw(theBatch);
            base.Draw(theBatch);
        }
    }
}
