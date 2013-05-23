
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tyrian_Remake
{
    class Sprite
    {
        //The current position of the Sprite
        public Vector2 Position = new Vector2(0, 0);

        //The texture object used when drawing the sprite
        public Texture2D mSpriteTexture;

        //Texture Color Data for collision detection
        public Color[] mSpriteTextureData;

        //BoundingRectangle based on position
        public Rectangle BoundingRectangle;

        //The asset name for the Sprite's Texture
        public string AssetName;

        //The Size of the Sprite (with scale applied)
        public Rectangle Size;

        //The Rectangular area from the original image that 
        //defines the Sprite. 
        Rectangle mSource;
        public Rectangle Source
        {
            get { return mSource; }
            set
            {
                mSource = value;
                Size = new Rectangle(0, 0, (int)(mSource.Width * Scale), (int)(mSource.Height * Scale));
            }
        }


        //The amount to increase/decrease the size of the original sprite. When
        //modified throught he property, the Size of the sprite is recalculated
        //with the new scale applied.
        private float mScale = 1.0f;
        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                //Recalculate the Size of the Sprite with the new scale
                Size = new Rectangle(0, 0, (int)(Source.Width * Scale), (int)(Source.Height * Scale));
            }
        }

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);

            mSpriteTextureData = new Color[mSpriteTexture.Width * mSpriteTexture.Height];

            mSpriteTexture.GetData(mSpriteTextureData);

            AssetName = theAssetName;

            Source = new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height);
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));

            // Get the bounding rectangle of the player ship
            BoundingRectangle = new Rectangle((int)Position.X, (int)Position.Y, Source.Width, Source.Height);

        }

        //Update the Sprite and change it's position based on the passed in speed, direction and elapsed time.
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            //Update Position
            Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;

            // Get the bounding rectangle of the player ship
            BoundingRectangle = new Rectangle((int)Position.X, (int)Position.Y, Source.Width, Source.Height);

            // Update the bounding rectangle position
            //BoundingRectangle.X = (int)Position.X;
            //BoundingRectangle.Y = (int)Position.Y;

        }

        //Draw the sprite to the screen
        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position, Source, Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }

}
