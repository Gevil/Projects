using System;

namespace Betolto
{
    public class Kotveny
    {
        //Kötvényszám
        public string Kotvenyszam { get; set; }


        //Biztositó adatai
        //Név = Baranya megyei Kölcsönös Nonprofit Növénybiztosító Egyesület
        public string BiztositoNeve { get; set; }

        //Székhely = 7623 Pécs, Köztársaság tér 2.
        public string BiztositoSzekhelye { get; set; }

        //Nyilvántartási szám = PK. 60.003/1993/5. 974
        public string NyilvantartSzam { get; set; }

        //Birosag = Baranya megyei Birosag
        public string Birosag { get; set; }

        //Számlaszám = 80500010-10000128
        public string Szamlaszam { get; set; }

        //MVH ügyfélazonosito = 1003257674
        public string MVHregsz { get; set; }

        //Adatkezelési nyilvántartási szám = NAIH – 64504/2013.
        public string AdatkNySz { get; set; }

        //Levelezési Cim = 7632 Pécs, Köztársaság tér 2.
        public string LevCim { get; set; }

        //Email = nonprofitbiztosito@gmail.com
        public string Email { get; set; }

        //Tel-fax = 72/211-780
        public string TelFax { get; set; }



        //Biztositott adatai
        //Neve
        public string BiztNeve { get; set; }

        //Cim Székhely stb
        public string BiztIranyitoszam { get; set; }
        public string BiztHelyiseg { get; set; }
        public string BiztCim { get; set; }

        //MVH Ügyfélazonosito
        public string BiztMVHregsz { get; set; }

        //Adoszam + Adóazonosito jel
        public string BiztAdoszJel { get; set; }

        //
        public string BiztOsszegHagyTJ { get; set; }
        public string BiztOsszegKiegTJ { get; set; }
        public string BiztOsszegTamogB { get; set; }
        public string BiztOsszegTamogC { get; set; }

        public string BiztDijHagyTJ { get; set; }
        public string BiztDijKiegTJ { get; set; }
        public string BiztDijTamogB { get; set; }
        public string BiztDijTamogC { get; set; }

        public string BiztVihar { get; set; }

        public DateTime? BiztKezdDatum { get; set; }

        public DateTime Datum { get; set; }
    }
}