class program{
	static void Main(){
		int szam1, szam2;
		string MJ;

		System.Console.Write("Els� sz�m:");
		szam1=int.Parse(System.Console.ReadLine());
		System.Console.Write("M�sodik sz�m:");
		szam2=int.Parse(System.Console.ReadLine());
		System.Console.Write("M�veleti jel:");
		MJ=System.Console.ReadLine();

		System.Console.WriteLine("Eredm�ny:");
		switch(MJ){
			case "+":
				System.Console.WriteLine(szam1+szam2);
				break;
			case "-":
				System.Console.WriteLine(szam1-szam2);
				break;
			case "*":
				System.Console.WriteLine(szam1*szam2);
				break;
			case "/":
				if (szam2==0){
					System.Console.WriteLine("HIBA 0-val nem oszthatsz!!!");
				} else {
					System.Console.WriteLine(szam1/szam2);
				}
				break;
			default:
				System.Console.WriteLine("HIBA");
				break;
		}
	}
}