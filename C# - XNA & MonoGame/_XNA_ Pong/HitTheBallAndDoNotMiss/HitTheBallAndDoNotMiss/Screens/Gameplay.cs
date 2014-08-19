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
    class Gameplay : Screen
    {
        Paddle mPlayerOnePaddle;
        Paddle mPlayerTwoPaddle;
        Ball mBall;
        Sprite mBackground;
        Text mPlayerOneScore;
        Text mPlayerTwoScore;
        int mScoreOne = 0;
        int mScoreTwo = 0;
        //Sprite mSquare;
        double mGetReadyTimer = 1.0f;
        Sprite mMessageBox;

        Text mPlayerOneScoredText;
        Text mPlayerTwoScoredText;
        Text mStartText;
        Text mGetReadyText;
        Text mPlayerOneWins;
        Text mPlayerTwoWins;

        SafeArea mSafeArea;

        Song mBackgroundMusic;
        SoundEffect mGameOver;
        SoundEffect mScore;

        enum State
        {
            Start,
            Playing,
            PlayerOneScored,
            PlayerTwoScored,
            PlayerOneWins,
            PlayerTwoWins,
            PlayAgain
        }
        State mCurrentState = State.Start;

        //ContentManager mContent;
        //Viewport mViewport;

        public Gameplay(ContentManager theContent, Viewport theViewport, EventHandler theScreenEvent,  PlayerIndex thePlayerOne, PlayerIndex thePlayerTwo)
            : base(theContent, theViewport, theScreenEvent)
        {
            GraphicsDeviceClearColor = Color.SlateGray;

            mContent = theContent;
            mViewport = theViewport;
            

            mBall = new Ball(theContent, "Sprites/Ball");

            mBackground = new Sprite(theContent, "Sprites/PongBackground");

            mPlayerOneScore = new Text(theContent, "Player One", "Fonts/ScoreFont", new Vector2(550, 75), Color.White);
            mPlayerTwoScore = new Text(theContent, "Player Two", "Fonts/ScoreFont", new Vector2(700, 75), Color.White);

            mPlayerOneScoredText = new Text(theContent, "Player One Scored!", "Fonts/ScoreFont", new Vector2(0, 0), Color.DarkBlue);
            mPlayerOneScoredText.Center(new Rectangle(0, 0, 1280, 670), Text.Alignment.Both);

            mPlayerTwoScoredText = new Text(theContent, "Player Two Scored!", "Fonts/ScoreFont", new Vector2(0, 0), Color.DarkBlue);
            mPlayerTwoScoredText.Center(new Rectangle(0, 0, 1280, 670), Text.Alignment.Both);

            mStartText = new Text(theContent, "Get ready to begin!", "Fonts/ScoreFont", new Vector2(0, 0), Color.DarkBlue);
            mStartText.Center(new Rectangle(0, 0, 1280, 720), Text.Alignment.Both);

            mGetReadyText = new Text(theContent, "Get ready!", "Fonts/ScoreFont", new Vector2(0, 0), Color.DarkBlue);
            mGetReadyText.Center(new Rectangle(0, 0, 1280, 720), Text.Alignment.Both);
            mGetReadyText.Position.Y += 50;

            mPlayerOneWins = new Text(theContent, "Player One Wins!", "Fonts/ScoreFont", new Vector2(0, 0), Color.DarkBlue);
            mPlayerOneWins.Center(new Rectangle(0, 0, 1280, 720), Text.Alignment.Both);

            mPlayerTwoWins = new Text(theContent, "Player Two Wins!", "Fonts/ScoreFont", new Vector2(0, 0), Color.DarkBlue);
            mPlayerTwoWins.Center(new Rectangle(0, 0, 1280, 720), Text.Alignment.Both);

            //mSquare = new Sprite(Content, "Square");
            //mSquare.Scale = new Vector2(750, 250);
            //mSquare.Position = mSquare.Center(new Rectangle(0, 0, 1280, 720));

            mMessageBox = new Sprite(theContent, "Sprites/MessageBox");
            mMessageBox.Position = mMessageBox.Center(new Rectangle(0, 0, 1280, 720));

            mSafeArea = new SafeArea(theContent);

            mBackgroundMusic = theContent.Load<Song>("SoundFX/Pong");
            MediaPlayer.Play(mBackgroundMusic);
            MediaPlayer.IsRepeating = true;

            mGameOver = theContent.Load<SoundEffect>("SoundFX/GameOver");
            mScore = theContent.Load<SoundEffect>("SoundFX/Score001");

        }

        public void StartOnePlayerGame(PlayerIndex thePlayerOne)
        {
            mPlayerOnePaddle = new Paddle(mContent, "Sprites/Square", new Input(thePlayerOne), Paddle.PaddlePosition.Left);
            mPlayerTwoPaddle = new Paddle(mContent, "Sprites/Square", new Input(thePlayerOne), Paddle.PaddlePosition.Right);
            mPlayerTwoPaddle.mInput = new AIInput(mBall, mPlayerTwoPaddle, mViewport);
        }

        public void StartTwoPlayerGame(PlayerIndex thePlayerOne, PlayerIndex thePlayerTwo)
        {
            mPlayerOnePaddle = new Paddle(mContent, "Sprites/Square", new Input(thePlayerOne), Paddle.PaddlePosition.Left);
            mPlayerTwoPaddle = new Paddle(mContent, "Sprites/Square", new Input(thePlayerTwo), Paddle.PaddlePosition.Right);
        }


        public override void Update(GameTime theGametime)
        {
            switch (mCurrentState)
            {
                case State.Start:
                    {
                        UpdateReadyTimer(theGametime);
                        break;
                    }
                case State.Playing:
                    {
                        UpdatePlaying(theGametime);
                        break;
                    }
                case State.PlayerOneScored:
                    {
                        UpdateReadyTimer(theGametime);
                        break;
                    }
                case State.PlayerTwoScored:
                    {
                        UpdateReadyTimer(theGametime);
                        break;
                    }
            }

            mPlayerOneScore.mText = mScoreOne.ToString();
            mPlayerTwoScore.mText = mScoreTwo.ToString();

            mSafeArea.Update(theGametime);

            base.Update(theGametime);
        }

        private void UpdatePlaying(GameTime theGametime)
        {
            mPlayerOnePaddle.Update(theGametime);
            mPlayerTwoPaddle.Update(theGametime);
            mBall.Update(theGametime, mPlayerOnePaddle, mPlayerTwoPaddle);

            if (mBall.OutOfBounds == MoveableSprite.Direction.Left)
            {
                mScoreTwo += 1;
                mCurrentState = State.PlayerTwoScored;
                mGetReadyTimer = 1.0f;
                mScore.Play();
            }
            else if (mBall.OutOfBounds == MoveableSprite.Direction.Right)
            {
                mScoreOne += 1;
                mCurrentState = State.PlayerOneScored;
                mGetReadyTimer = 1.0f;
                mScore.Play();
            }

            if (mScoreOne == 3)
            {
                mCurrentState = State.PlayerOneWins;
                mGameOver.Play();
            }
            else if (mScoreTwo == 3)
            {
                mCurrentState = State.PlayerTwoWins;
                mGameOver.Play();
            }
        }

        private void UpdateReadyTimer(GameTime theGametime)
        {
            mGetReadyTimer -= theGametime.ElapsedGameTime.TotalSeconds;
            if (mGetReadyTimer < 0)
            {
                mCurrentState = State.Playing;
                mBall.Start();
            }
        }

        public override void Draw(SpriteBatch theBatch)
        {
            mBackground.Draw(theBatch);

            if (mCurrentState == State.Playing)
            {
                mPlayerOnePaddle.Draw(theBatch);
                mPlayerTwoPaddle.Draw(theBatch);
                mBall.Draw(theBatch);
            }

            mPlayerOneScore.Draw(theBatch);
            mPlayerTwoScore.Draw(theBatch);

            if (mCurrentState == State.Start)
            {
                mMessageBox.Draw(theBatch);
                mStartText.Draw(theBatch);
            }

            if (mCurrentState == State.PlayerOneScored)
            {
                mMessageBox.Draw(theBatch);
                mGetReadyText.Draw(theBatch);
                mPlayerOneScoredText.Draw(theBatch);
            }

            if (mCurrentState == State.PlayerTwoScored)
            {
                mMessageBox.Draw(theBatch);
                mGetReadyText.Draw(theBatch);
                mPlayerTwoScoredText.Draw(theBatch);
            }

            if (mCurrentState == State.PlayerOneWins)
            {
                mMessageBox.Draw(theBatch);
                mPlayerOneWins.Draw(theBatch);
            }

            if (mCurrentState == State.PlayerTwoWins)
            {
                mMessageBox.Draw(theBatch);
                mPlayerTwoWins.Draw(theBatch);
            }

            mSafeArea.Draw(theBatch);

            base.Draw(theBatch);
        }
    }
}
