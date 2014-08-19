class program {
	static void Main(){
		System.Console.WriteLine("Írd be a neved!");
		string nev=System.Console.ReadLine();
		string koszones="";

		switch(nev)
		{
			case "Béla":
				koszones="Helló";
				break;

			case "József":
				koszones="Szia";
				break;

			case "Ágnes":
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