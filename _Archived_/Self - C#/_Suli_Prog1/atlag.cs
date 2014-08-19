class program{
	static void Main(){
		int i, szam, osszeg, min, max;
		i=0; osszeg=0; min = -1; max = -1;

		do {
			System.Console.WriteLine("Irj be egy számot!");
			szam = int.Parse(System.Console.ReadLine());
			if ((szam >= 0) && (szam%2==0)){
				i++;
				osszeg += szam;
				if ((min == -1) || (szam < min)){
					min=szam;
				}
				if ((max==-1) || (szam>max)){
					max=szam;
				}
			}
		}while(szam >= 0);
		if (i>2){
			i-=2;
			osszeg = osszeg-min-max;
			int atlag=osszeg/i;
			System.Console.WriteLine("Átlag = " + atlag);
		}
	}
}