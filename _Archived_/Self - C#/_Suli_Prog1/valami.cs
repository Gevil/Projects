class program {
	static void Main(){
		System.Console.WriteLine("�rd be a neved!");
		string nev;
		do {
			nev=System.Console.ReadLine();
		} while ( !(
				(nev != "J�zsef") ||
				(nev != "B�la") ||
				(nev != "�gnes")
				)
			);
		string koszones="";
		switch(nev){
			case "B�la":
				koszones="Hell�";
				break;
			case "J�zsef":
				koszones="Szia";
				break;
			case "�gnes":
				koszones="Hi";
				break;
			default:
				koszones="Ahoi";
				break;
		}
		System.Console.WriteLine(koszones + " "+nev +"!");
		System.Console.ReadLine();
		}	
	}