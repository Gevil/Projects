class Program
{
	static void Main()
	{
		string mondat="";
		System.Console.WriteLine("Irj be egy mondatot! \n");
		mondat=System.Console.ReadLine();
		
		mondat.Replace(", "," ");
		mondat.Replace(" - "," ");
		
		string tmp="";
		int count=0;
		foreach(char c in mondat)
		{
			if(c.ToString() == " ")
			{
				count++;
				System.Console.WriteLine("sz� -> "+tmp);
				tmp = "";
			}else{ tmp += c; }
		}
		count++;
		System.Console.WriteLine("sz� -> "+tmp+"\n Szavak sz�ma: " + count);
	}
}