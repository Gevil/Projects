using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace HitTheBallAndDoNotMiss
{
    public class Menu
    {
        Input mInput;

        SortedList<int, MenuItem> mMenuItems = new SortedList<int, MenuItem>();
        ContentManager mContent;

        Viewport mViewport;
        int mYPosition;

        int mCurrentMenuItem = 0;

        public MenuItem SelectedItem = null;

        //AudioEngine mAudioEngine;
        //WaveBank mWaveBank;
        //SoundBank mSoundBank;

        SoundEffect mMenuSelect;
        SoundEffect mSelect;

        public void ChangeMenuColors(Color theDefault, Color theSelected, Color theDisabled)
        {
            foreach (MenuItem aMenu in mMenuItems.Values)
            {
                aMenu.DefaultColor = theDefault;
                aMenu.SelectedColor = theSelected;
                aMenu.DisabledColor = theDisabled;
            }
        }
        
        public Menu(ContentManager theContent, Viewport theViewport, PlayerIndex theCurrentPlayer)
        {
            mInput = new Input(theCurrentPlayer);

            mContent = theContent;
            mViewport = theViewport;
            mYPosition = mViewport.Height - 100;

            mMenuSelect = theContent.Load<SoundEffect>("MenuSelect");
            mSelect = theContent.Load<SoundEffect>("Select");
        }

        public void AddMenuItem(string theText, bool isDisabled, bool isSelected, EventHandler theEvent)
        {
            MenuItem aMenuItem = new MenuItem(mContent, theText, Vector2.Zero, theEvent);

            //Center menu item in screen
            Vector2 aLength = aMenuItem.Length();
            mYPosition = (int)(mYPosition - aLength.Y - 10);

            aMenuItem.Position = new Vector2((mViewport.Width / 2) - (aLength.X / 2), mYPosition);

            if (isSelected == true)
            {
                mCurrentMenuItem = mMenuItems.Count;

                foreach (MenuItem aMenu in mMenuItems.Values)
                {
                    aMenu.Selected = false;
                }
            }

            aMenuItem.Selected = isSelected;
            aMenuItem.Disabled = isDisabled;

            mMenuItems.Add(mMenuItems.Count, aMenuItem);
        }

        public void AddMenuItem(string theText, bool isDisabled, bool isSelected)
        {
            AddMenuItem(theText, isDisabled, isSelected, null);
        }

        public void AddMenuItem(string theText)
        {
            AddMenuItem(theText, false, false);
        }

        public void Update(GameTime theGameTime)
        {
            mInput.BeginUpdate();

            if (mMenuItems.Count != 0)
            {

                if (mInput.AWithRelease())
                {
                    mSelect.Play();  
                    
                    SelectedItem = mMenuItems[mCurrentMenuItem];
                    if (mMenuItems[mCurrentMenuItem].Event != null)
                    {
                        //SelectedItem = null;
                        mMenuItems[mCurrentMenuItem].Event.Invoke(this, new EventArgs());
                        SelectedItem = null;
                    }
                }

                if (SelectedItem == null)
                {
                    int aPreviousMenuItem = mCurrentMenuItem;
                    mMenuItems[mCurrentMenuItem].Selected = false;

                    if (mInput.DownWithRelease())
                    {
                        do
                        {
                            mCurrentMenuItem -= 1;
                            if (mCurrentMenuItem < 0)
                            {
                                mCurrentMenuItem = aPreviousMenuItem;
                                break;
                            }
                        } while (mMenuItems[mCurrentMenuItem].Disabled == true);

                    }

                    if (mInput.UpWithRelease())
                    {
                        do
                        {
                            mCurrentMenuItem += 1;
                            if (mCurrentMenuItem > mMenuItems.Count - 1)
                            {
                                mCurrentMenuItem = aPreviousMenuItem;
                                break;
                            }
                        } while (mMenuItems[mCurrentMenuItem].Disabled == true);
                    }

                    mCurrentMenuItem = (int)MathHelper.Clamp(mCurrentMenuItem, 0, mMenuItems.Count - 1);
                    mMenuItems[mCurrentMenuItem].Selected = true;

                    if (mCurrentMenuItem != aPreviousMenuItem)
                    {
                        //if (mMenuSelect.IsCreated == false)
                        //{
                        //    mMenuSelect = mSoundBank.GetCue("MenuSelect");
                        //}
                        mMenuSelect.Play();  
                    }
                }
            }

            foreach (MenuItem aMenuItem in mMenuItems.Values)
            {
                aMenuItem.Update(theGameTime);
            }

            mInput.EndUpdate();
        }

        public void Draw(SpriteBatch theBatch)
        {
            foreach (MenuItem aMenuItem in mMenuItems.Values)
            {
                aMenuItem.Draw(theBatch);
            }
        }
    }
}