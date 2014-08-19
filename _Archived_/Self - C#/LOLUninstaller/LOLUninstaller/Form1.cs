using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LOLUninstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            int i = 1;
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(progressBar1.Value < progressBar1.Maximum)
                progressBar1.Value++;

            switch(progressBar1.Value)
            {
                case 1:
                    textBox1.AppendText("Nerfing incredibly overexcited lady's voice in tutorial!\n");
                    break;
                case 5:
                    textBox1.AppendText("Irelia now has a Sudden Death debuff which decreases her HP by 1 every second!\n");
                    break;
                case 9:
                    textBox1.AppendText("Jarvan may no longer buy items from the Shop!\n");
                    break;
                case 14:
                    textBox1.AppendText("Removing Stealth heroes from Game\n");
                    break;
                case 17:
                    textBox1.AppendText("Removing Hooks from Amumu and Blitzcrank\n");
                    break;
                case 23:
                    textBox1.AppendText("Increasing Cooldown of GAP closers on Meelee heroes by 200%\n");
                    break;
                case 29:
                    textBox1.AppendText("Taking Arrow from Ashe and giving it to Corki :P\n");
                    break;
                case 34:
                    textBox1.AppendText("Making Vayne 350% more fat therefore suiting her always rolling around as such.\n");
                    break;
                case 39:
                    textBox1.AppendText("Kassadin becomes 3 times more vulnerable to physical attacks when using it's silence.\n");
                    break;
                case 44:
                    textBox1.AppendText("Udyr now has a permanent Pedo Bear skin and may only be played as Annie's pet.\n");
                    break;
                case 49:
                    textBox1.AppendText("Annie is no longer able to stun on only has 1 skill called 'Play with Udyr'\n");
                    break;
                case 54:
                    textBox1.AppendText("Warvick is no longer playable, instead he is into guest appearances in the 'Little red riding hood'\n");
                    break;
                case 59:
                    textBox1.AppendText("Brand has been renamed to Lighter and adjusted damage accordingly!\n");
                    break;
                case 64:
                    textBox1.AppendText("Garen is now unable to do damage to enemy champions since he is too dizzy from all the spinning.\n");
                    break;
                case 69:
                    textBox1.AppendText("All support heroes have been combined into one called 'Sonatarctica' which now is only playable with an electric guitar where each accord represents 1 skill of each of the heroes combined!\n");
                    break;
                case 76:
                    textBox1.AppendText("Jax is now only playable by players with 800 or lower elo!\n");
                    break;
                case 80:
                    textBox1.AppendText("Master Yi has been removed from each account and price increased to 200 000 RPs!\n");
                    break;
                case 85:
                    textBox1.AppendText("Akali can no longer use attacks after using her gapcloser.\n");
                    break;
                case 88:
                    textBox1.AppendText("Every champion with an elo rating over 1000 now must use offensive language and lock rally as one of their summoner spells in ranked games.\n");
                    break;
                case 94:
                    textBox1.AppendText("Installing Recount!\n");
                    break;
                case 96:
                    textBox1.AppendText("Ready To Play! Good Luck Summoner!\n");
                    break;
            }
        }

    }
}
