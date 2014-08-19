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
    public partial class CreatureStats : WoWHeadParser.FormMain
    {
        public CreatureStats()
        {
            InitializeComponent();
        }

        private void CreatureStats_Load(object sender, EventArgs e)
        {

        }
        private void btnParse_Click(object sender, EventArgs e)
        {
            if (txtMin.Text == "" | txtMax.Text == "")
            {
                MessageBox.Show("No Input! Please give the range to parse in!", "Input Error!!!");
            }else{
                int min = int.Parse(txtMin.Text);
                int max = int.Parse(txtMax.Text);
                progressBar1.Maximum = max - min;
                ThreadStart Pstart = delegate { Parse(min,max); };
                Thread ParserThread = new Thread(Pstart);
                ParserThread.Start();
            }
        }
        private void txtMin_TextChanged(object sender, EventArgs e)
        {
            txtMax.Text = txtMin.Text;
        }

        public static void SetText(System.Windows.Forms.TextBox ctrl, string text)
        {
            if (ctrl.InvokeRequired)
            {
                object[] params_list = new object[] { ctrl, text };
                ctrl.Invoke(new SetTextDelegate(SetText), params_list);
            } else {
                ctrl.AppendText(text);
            }
        }
        public static void SetInt(System.Windows.Forms.TextBox ctrl, string text)
        {
            if (ctrl.InvokeRequired)
            {
                object[] params_list = new object[] { ctrl, text };
                ctrl.Invoke(new SetTextDelegate(SetInt), params_list);
            }
            else
            {
                ctrl.Text = text;
            }
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

        public void Parse(int min, int max)
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
                if (k == 500)
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
                    else if (input.Substring(length+5).Contains("??")){ minlvl = 70; maxlvl = 75; }

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
                        length = input.IndexOf("Tameable (") + 10;
                        l2 = input.Substring(length + 17, 7);
                        //<a href="/?spells

                        switch (l2)
                        {
                            case "=-3.768":
                                family = 35;
                                break;
                            case "=-3.764":
                                family = 34;
                                break;
                            case "=-3.656":
                                family = 32;
                                break;
                            case "=-3.767":
                                family = 31;
                                break;
                            case "=-3.763":
                                family = 30;
                                break;
                            case "=-3.766":
                                family = 27;
                                break;
                            case "=-3.655":
                                family = 26;
                                break;
                            case "=-3.654":
                                family = 25;
                                break;
                            case "=-3.653":
                                family = 24;
                                break;
                            case "=-3.251":
                                family = 21;
                                break;
                            case "=-3.236":
                                family = 20;
                                break;
                            case "=-3.218":
                                family = 12;
                                break;
                            case "=-3.217":
                                family = 11;
                                break;
                            case "=-3.215":
                                family = 9;
                                break;
                            case "=-3.214":
                                family = 8;
                                break;
                            case "=-3.213":
                                family = 7;
                                break;
                            case "=-3.212":
                                family = 6;
                                break;
                            case "=-3.211":
                                family = 5;
                                break;
                            case "=-3.210":
                                family = 4;
                                break;
                            case "=-3.203":
                                family = 3;
                                break;
                            case "=-3.209":
                                family = 2;
                                break;
                            case "=-3.208":
                                family = 1;
                                break;
                            default:
                                family = 0;
                                break;
                        }
                        if (input.Substring(length, 10).Contains("Sporebat"))
                        { family = 33; }
                    }

                    if (minlvl > maxlvl) { byte tmp = maxlvl; maxlvl = minlvl; minlvl = tmp; }
                    if (minhp > maxhp) { long tmp = maxhp; maxhp = minhp; minhp = tmp; }
                    if (minmp > maxmp) { long tmp = maxmp; maxmp = minmp; minmp = tmp; }

                    string sqlMinLvl, sqlMaxLvl, sqlMinHP, sqlMaxHP, /*sqlMinMP, sqlMaxMP,*/ sqlFaction;
                    string a = "", b = "";

                    if (minlvl == 0 | maxlvl == 0) { sqlMinLvl = ""; sqlMaxLvl = ""; a = ""; }
                    else
                    {
                        sqlMinLvl = "`minlevel`='" + minlvl + "'";
                        sqlMaxLvl = "`maxlevel`='" + maxlvl + "'";
                        a = ", ";
                    }
                    if (minhp == 0 | maxhp == 0) { sqlMinHP = ""; sqlMaxHP = ""; b = ""; }
                    else
                    {
                        sqlMinHP = "`minhealth`='" + minhp + "'";
                        sqlMaxHP = "`maxhealth`='" + maxhp + "'";
                        b = ", ";
                    }
                    if (faction == 0) { sqlFaction = ""; }
                    else { sqlFaction = ",`faction`='" + faction + "'"; }

                    string output;
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

