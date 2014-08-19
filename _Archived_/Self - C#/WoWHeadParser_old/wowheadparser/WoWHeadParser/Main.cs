using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WoWHeadParser
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void creaturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Creature form0 = new Creature();
            form0.Show();
        }

        private void functionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}