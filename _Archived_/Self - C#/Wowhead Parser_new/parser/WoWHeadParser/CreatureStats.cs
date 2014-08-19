using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
namespace WoWHeadParser
{
    enum Family
    {
        Serpent     =   768,
        NetherRay   =   764,
        WindSerpent =   656,
        Ravager     =   767,
        Dragonhawk  =   763,
        WarpStalker =   766,
        Owl         =   655,
        Hyena       =   654,
        Bat         =   653,
        Turtle      =   251,
        Scorpid     =   236,
        Tallstrider =   218,
        Raptor      =   217,
        Gorilla     =   215,
        Crab        =   214,
        CarrionBird =   213,
        Crocolisk   =   212,
        Boar        =   211,
        Bear        =   210,
        Spider      =   203,
        Cat         =   209,
        Wolf        =   208
    }

    public partial class CreatureStats : WoWHeadParser.FormMain
    {
        public CreatureStats()
        {
            InitializeComponent();
        }

        private void CreatureStats_Load(object sender, EventArgs e)
        {
            //if(File.Exists("creature_template_update.sql"))
            //{
            //    string[] loaded = new string[1];
            //    TextReader reader = new TextReader();
            //}

        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            int min, max;
            if (txtMin.Text == "" | txtMax.Text == "")
            {
                MessageBox.Show("No Input! Please give the range to parse in!", "Input Error!!!");
            }
            else
            {
                if (int.Parse(txtMin.Text) > int.Parse(txtMax.Text))
                {
                    min = int.Parse(txtMax.Text);
                    max = int.Parse(txtMin.Text);
                }
                else
                {
                    min = int.Parse(txtMin.Text);
                    max = int.Parse(txtMax.Text);
                }
                progressBar1.Maximum = max - min;
                ThreadStart Pstart = delegate { WorkerThread(min, max); };
                Thread ParserThread = new Thread(Pstart);
                ParserThread.Start();
            }
        }
        private void txtMin_TextChanged(object sender, EventArgs e)
        {
            if (txtMax.Text == "") { txtMax.Text = txtMin.Text; }
        }

        public static void SetText(System.Windows.Forms.TextBox ctrl, string text)
        {
            if (ctrl.InvokeRequired)
            {
                object[] params_list = new object[] { ctrl, text };
                ctrl.Invoke(new SetTextDelegate(SetText), params_list);
            }
            else { ctrl.AppendText(text); }
        }
        public static void SetInt(System.Windows.Forms.TextBox ctrl, string text)
        {
            if (ctrl.InvokeRequired)
            {
                object[] params_list = new object[] { ctrl, text };
                ctrl.Invoke(new SetTextDelegate(SetInt), params_list);
            }
            else { ctrl.Text = text; }
        }
        public static void UpdatePbar(System.Windows.Forms.ProgressBar name, int step)
        {
            if (name.InvokeRequired)
            {
                object[] params_list = new object[] { name, step };
                name.Invoke(new UpdateProgressBar(UpdatePbar), params_list);
            }
            else { name.Increment(step); }
        }

        public delegate void UpdateProgressBar(System.Windows.Forms.ProgressBar name, int step);
        public delegate void SetTextDelegate(System.Windows.Forms.TextBox ctrl, string text);

        public void WorkerThread(int min, int max)
        {
            string url = "http://www.wowhead.com/?npc=";
            // Create a writer and open the file
            TextWriter writer = new StreamWriter("creature_template_update.sql");

            for (int i = min; i <= max; i++)
            {
                // Pause
                int j = 0; j++;
                if (j == 10) { System.Threading.Thread.Sleep(2000); j = 0; }

                // Garbage Collection
                int k = 0; k++;
                if (k == 250)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                // Declarations
                long minmp = 0, maxmp = 0, minhp = 0, maxhp = 0; int faction = 0;
                byte minlvl = 0, maxlvl = 0, rank = 0, family = 0;

                // Downloading webpage.
                WebClient myWebClient = new WebClient();
                byte[] myDataBuffer = myWebClient.DownloadData(url + i);
                string input = Encoding.ASCII.GetString(myDataBuffer);

                if (input.IndexOf("This NPC doesn't exist or is not yet in the database.") < 0)
                {
                    if (input.IndexOf("<th>Quick Facts</th>") > 0)
                    {
                        input = input.Substring(input.IndexOf("<th>Quick Facts</th>"), input.IndexOf("<th>Screenshots</th>") - input.IndexOf("<th>Quick Facts</th>"));
                        int length = 0;
                        string l2;

                        /***************************************************************************/
                        /*                              Level                                      */
                        /***************************************************************************/
                        length = input.IndexOf("<div>Level: ") + 12;
                        if (input.IndexOf("<div>Level: ") > 0 && input.Substring(length, 5).IndexOf("??") < 0)
                        {
                            l2 = input.Substring(length, 12);

                            if (l2.IndexOf("-") > 0)
                            {
                                minlvl = byte.Parse(l2.Substring(0, l2.IndexOf(" - ")));
                                maxlvl = byte.Parse(l2.Substring(l2.IndexOf("-") + 1, l2.IndexOf("</d") - 1 - l2.IndexOf("-")));
                            }
                            else
                            {
                                minlvl = byte.Parse(l2.Substring(0, l2.IndexOf("</d")));
                                maxlvl = minlvl;
                            }
                        }
                        else if (input.Substring(length + 5).Contains("??")) { minlvl = 70; maxlvl = 75; }

                        /***************************************************************************/
                        /*                               Health                                    */
                        /***************************************************************************/
                        if (input.IndexOf("<div>Health: ") > 0)
                        {
                            length = input.IndexOf("<div>Health: ") + 13;
                            l2 = input.Substring(length, 20).Replace(",", "");
                            if (l2.IndexOf("-") > 0)
                            {
                                minhp = long.Parse(l2.Substring(0, l2.IndexOf(" - ")));
                                maxhp = long.Parse(l2.Substring(l2.IndexOf("-") + 1, l2.IndexOf("</d") - 1 - l2.IndexOf("-")));
                            }
                            else
                            {
                                minhp = long.Parse(l2.Substring(0, l2.IndexOf("</d")));
                                maxhp = minhp;
                            }
                        }
                        else { minhp = 0; maxhp = 0; }

                        /***************************************************************************/
                        /*                                Mana                                     */
                        /***************************************************************************/
                        if (input.IndexOf("<div>Mana: ") > 0)
                        {

                            length = input.IndexOf("<div>Mana: ") + 11;
                            l2 = input.Substring(length, 20).Replace(",", "");
                            if (l2.IndexOf("-") > 0)
                            {
                                minmp = long.Parse(l2.Substring(0, l2.IndexOf(" - ")));
                                maxmp = long.Parse(l2.Substring(l2.IndexOf("-") + 1, l2.IndexOf("</d") - 1 - l2.IndexOf("-")));
                            }
                            else
                            {
                                minmp = long.Parse(l2.Substring(0, l2.IndexOf("</d")));
                                maxmp = minhp;
                            }
                        }
                        else { minmp = 0; maxmp = 0; }

                        /***************************************************************************/
                        /*                              Faction                                    */
                        /***************************************************************************/
                        if (input.IndexOf("?faction=") > 0)
                        {
                            length = input.IndexOf("?faction=") + 9;
                            l2 = input.Substring(length, 5);
                            faction = int.Parse(l2.Substring(0, l2.IndexOf("\"")));
                        }

                        /***************************************************************************/
                        /*                                Rank                                     */
                        /***************************************************************************/
                        if (input.IndexOf("<div>Classification: ") > 0)
                        {
                            length = input.IndexOf("<div>Classification: ") + 21;
                            l2 = input.Substring(length, 10);
                            switch (l2)
                            {
                                case "Rare</div>":
                                    rank = 4;
                                    break;
                                case "Boss</div>":
                                    rank = 3;
                                    break;
                                case "Rare Elite":
                                    rank = 2;
                                    break;
                                case "Elite</div":
                                    rank = 1;
                                    break;
                                default:
                                    rank = 0;
                                    break;
                            }
                        }

                        /***************************************************************************/
                        /*                             Pet Family                                  */
                        /***************************************************************************/
                        if (input.IndexOf("Tameable") > 0)
                        {
                            length = input.IndexOf("Tameable (") + 14;
                            if (input.IndexOf("Sporebat") > 0)
                            { family = 33; }
                            else
                            {
                                int l1 = int.Parse(input.Substring(length + 17, 3));
                                //<a href="/?spells=-3.xxx

                                switch (l1)
                                {
                                    case (int)Family.Serpent:
                                        family = 35;
                                        break;
                                    case (int)Family.NetherRay:
                                        family = 34;
                                        break;
                                    case (int)Family.WindSerpent:
                                        family = 32;
                                        break;
                                    case (int)Family.Ravager:
                                        family = 31;
                                        break;
                                    case (int)Family.Dragonhawk:
                                        family = 30;
                                        break;
                                    case (int)Family.WarpStalker:
                                        family = 27;
                                        break;
                                    case (int)Family.Owl:
                                        family = 26;
                                        break;
                                    case (int)Family.Hyena:
                                        family = 25;
                                        break;
                                    case (int)Family.Bat:
                                        family = 24;
                                        break;
                                    case (int)Family.Turtle:
                                        family = 21;
                                        break;
                                    case (int)Family.Scorpid:
                                        family = 20;
                                        break;
                                    case (int)Family.Tallstrider:
                                        family = 12;
                                        break;
                                    case (int)Family.Raptor:
                                        family = 11;
                                        break;
                                    case (int)Family.Gorilla:
                                        family = 9;
                                        break;
                                    case (int)Family.Crab:
                                        family = 8;
                                        break;
                                    case (int)Family.CarrionBird:
                                        family = 7;
                                        break;
                                    case (int)Family.Crocolisk:
                                        family = 6;
                                        break;
                                    case (int)Family.Boar:
                                        family = 5;
                                        break;
                                    case (int)Family.Bear:
                                        family = 4;
                                        break;
                                    case (int)Family.Spider:
                                        family = 3;
                                        break;
                                    case (int)Family.Cat:
                                        family = 2;
                                        break;
                                    case (int)Family.Wolf:
                                        family = 1;
                                        break;
                                    default:
                                        family = 0;
                                        break;
                                }
                            }
                        }
                    }

                    // Check if data is in the wrong format to output
                    if (minlvl > maxlvl) { byte tmp = maxlvl; maxlvl = minlvl; minlvl = tmp; }
                    if (minhp > maxhp) { long tmp = maxhp; maxhp = minhp; minhp = tmp; }
                    if (minmp > maxmp) { long tmp = maxmp; maxmp = minmp; minmp = tmp; }

                    // Check if data is NULL and therefore do not output
                    string sqlMinLvl, sqlMaxLvl, sqlMinHP, sqlMaxHP, /*sqlMinMP, sqlMaxMP,*/ sqlFaction;
                    string a = "", b = "";

                    if (minlvl == 0 | maxlvl == 0)
                    { 
                        sqlMinLvl = "";
                        sqlMaxLvl = "";
                        a = ""; 
                    }
                    else
                    {
                        sqlMinLvl = "`minlevel`='" + minlvl + "'";
                        sqlMaxLvl = "`maxlevel`='" + maxlvl + "'";
                        a = ", ";
                    }

                    if (minhp == 0 | maxhp == 0) 
                    { 
                        sqlMinHP = "";
                        sqlMaxHP = "";
                        b = ""; 
                    }
                    else
                    {
                        sqlMinHP = "`minhealth`='" + minhp + "'";
                        sqlMaxHP = "`maxhealth`='" + maxhp + "'";
                        b = ", ";
                    }

                    if (faction == 0) { sqlFaction = ""; }
                    else { sqlFaction = ",`faction`='" + faction + "'"; }

                    string output;

                    //Building the string output
                    if (sqlMinLvl == "" && sqlMinHP == "" && minmp == 0)
                    {
                        output = "-- No Level, Health and Mana data available! Id=" + i;
                    }
                    else
                    {
                        output = "UPDATE `creature_template` SET " + sqlMinLvl + a + sqlMaxLvl + a + sqlMinHP + b + sqlMaxHP + b + "`minmana`='" + minmp + "',`maxmana`='" + maxmp + "'" + sqlFaction + ",`rank`='" + rank + "',`family`='" + family + "' WHERE `entry`='" + i + "';";
                    }

                    SetText(this.txtSql, output + "\n");
                    writer.WriteLine(output);
                }
                else { SetText(this.txtSql, "Creature with ID=" + i + " Doesn't exist!\n"); }
                SetInt(this.txtMin, i.ToString());
                UpdatePbar(this.progressBar1, 1);
            }
            // close the stream
            writer.Close();
            MessageBox.Show("Done", "Parsing Completed!!!");
            System.Media.SystemSounds.Beep.Play();
        }
    }
}

