class program{
	static void Main(){
		int szam1, szam2;
		string MJ;

		System.Console.Write("Elsõ szám:");
		szam1=int.Parse(System.Console.ReadLine());
		System.Console.Write("Második szám:");
		szam2=int.Parse(System.Console.ReadLine());
		System.Console.Write("Mûveleti jel:");
		MJ=System.Console.ReadLine();

		System.Console.WriteLine("Eredmény:");
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