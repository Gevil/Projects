using DevExpress.XtraReports.UI;
using System;

namespace Betolto
{
    public partial class KotvenyReport : XtraReport
    {
        public KotvenyReport(Kotveny k)
        {
            InitializeComponent();

            //Header
            txtKotvenyszam.Text = k.Kotvenyszam;

            //Biztosito adatai
            txtBENev.Text = k.BiztositoNeve;
            txtBESzekhely.Text = k.BiztositoSzekhelye;
            txtBENySz.Text = k.NyilvantartSzam;
            txtBEBirosag.Text = k.Birosag;
            txtBESzamlasz.Text = k.Szamlaszam;
            txtBEMVHregsz.Text = k.MVHregsz;
            txtBEAdatkNySz.Text = k.AdatkNySz;
            txtBELevCim.Text = k.LevCim;
            txtBEemail.Text = k.Email;
            txtBEtel.Text = k.TelFax;

            //biztositott adatai
            txtBTNev.Text = k.BiztNeve;
            txtBTCim.Text = k.BiztIranyitoszam +" " + k.BiztHelyiseg + ", " + k.BiztCim;
            txtBTMVHregsz.Text = k.BiztMVHregsz;
            txtBTAdosz.Text = k.BiztAdoszJel;

            //biztositasok összegei es dijai
            txtHTJossz.Text = k.BiztOsszegHagyTJ;
            txtHTJdij.Text = k.BiztDijHagyTJ;

            txtKTJossz.Text = k.BiztOsszegKiegTJ;
            txtKTJdij.Text = k.BiztDijKiegTJ;

            txtTBJossz.Text = k.BiztOsszegTamogB;
            txtTBJdij.Text = k.BiztDijTamogB;

            txtTCJossz.Text = k.BiztOsszegTamogC;
            txtTCJdij.Text = k.BiztDijTamogC;

            //datum
            txtDate.Text = k.Datum.ToShortDateString();

            xrLabel20.Text += k.BiztVihar;
            xrLabel21.Text += k.BiztVihar;

            if(k.BiztKezdDatum != null)
                xrRichText2.Text += ((DateTime)k.BiztKezdDatum).ToShortDateString();


        }
    }
}