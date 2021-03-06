﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ChatLogParser
{

    /*To-Do:
     * Separate and write channels in multiple tabs and ability to save each channel to a different file.
     * 
     * Analysis straight from chatlog.
     * 
     * Check out if the database used for fails could be done in XML
     * Look into some XML datastrucutres and parsers (IO stuff)!
     * NOTE: Output still needs to be in TxT or HTML for posting purposes.
     * 
     * Rewrite App to have worker thread and UI thread with delegates and multiple threads.
     * Make UI responsive while the parse is ongoing.
     * 
     * Possibly make an importable database from ensidiafails later on for each boss and encounter
     * and make the app recognize them for each boss so the Output can be configured for more detailed information.
    */

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private static string input;

        private void btnLoad_Click(object sender, EventArgs e)
        {
            StreamReader streamReader = new StreamReader(txtPath.Text.ToString(), System.Text.Encoding.Default);
            string text = streamReader.ReadToEnd();
            streamReader.Close();

            txtData.Text = text;
            input = text;
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            //check for empty input string
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            //Find a goddamn dataset that can manage unknown length of data...
            //This shit doesnt compile yet - need to find an arraytype to handle
            //fail types for each member in detail.
            //Generalize some methods for easier use on multiple places.
            string[] sLines = new string[input.Split('\n').Length];
            sLines = input.Split('\n');
            List<string> sCharname = new List<string>();
            List<int> sOverall = new List<int>();
            List<string> sFails = new List<string>();
            string[ , , ] blob = new string[1,1,1];
            int k = 0;

            for(int i = 0; i<=sLines.Length;i++)
            {
                if (sLines[i].Contains("-------------"))
                {}
                else if (sLines[i].Contains("channel."))
                {}
                else
                {
                    //sLines[i].Substring(43).Split(new Char[] {" ","(",")",","});
                    string[] temp = new string[sLines[i].Substring(43).Split(new Char[] { ' ', '(', ')', ',' }).Length];
                    temp = sLines[i].Substring(42).Split(new Char[] { ' ', '(', ')', ',', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    sCharname.Add(temp[0]);
                    sOverall.Add(Convert.ToInt32(temp[2].Remove(temp[2].Length - 1)));
                    for (int j = 3; j <= temp.Length-1;j++)
                    {
                        List<string> sFailDetails = new List<string>();
                        List<int> iFailCount = new List<int>();
                        int Num;
                        bool isNum = int.TryParse(temp[j].Remove(temp[j].Length - 1).Trim(), out Num);
                        //Nevertheless failed 5 3x Unleashed Dark 2x Unleashed Light
                        //string[,] temp2 = new string[];

                        if (isNum)
                        {
                            iFailCount.Add(Convert.ToInt32(temp[j].Remove(temp[j].Length - 1)));
                            k = 0;
                        }
                        else 
                        {
                            if (sFailDetails.Count() > 0)
                            {
                                if (j + 1 < temp.Length)
                                    isNum = int.TryParse(temp[j + 1].Remove(temp[j + 1].Length - 1).Trim(), out Num);
                                else
                                    isNum = true;

                                if (!isNum)
                                {
                                    string kek = sFailDetails[k];
                                    sFailDetails.RemoveAt(k);
                                    sFailDetails.Add(kek += " " + temp[j]);
                                    k++;
                                }
                            }
                            else
                            {
                                sFailDetails.Add(temp[j]);
                            }

                            if (j == temp.Length)
                            {
                                string[,] details = new string[iFailCount.Count(),sFailDetails.Count()];
                                for(int l = 0; l <= i;l++ )
                                {
                                    if(blob[l,l,l]==sCharname[i])
                                    {
                                        for(int m = 0; m <= iFailCount.Count()-1;m++ )
                                        {
                                            details[m,m] = new string[iFailCount.Count(), sFailDetails.Count()]{iFailCount[m].ToString(),sFailDetails[m]};
                                        }

                                        blob[l,l,l] = new string[l,l,l]{sCharname[i]}, {sOverall[i].ToString()},{details}};
                                    }
                                else
                                {
                                        string[,,] tempblob = new string[l,l,l];
                                        blob = new string[l,l,l];
                                    }
                                }
                            }

                        }

                        //sFailDetails.Add(temp[j]);
                    }
                    
                    //sFails.Add(sLines[i]);

                }
            }
        }
    }
}
