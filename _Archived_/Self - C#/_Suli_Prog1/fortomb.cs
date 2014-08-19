class Program
{
	static void Main()
	{
		int db;
		int[] tomb;
		System.Console.WriteLine("Darabszám?");
		db = int.Parse(System.Console.ReadLine());
		tomb=new int[db];
		for(int i= 0; i<db; i++)
		{
			System.Console.WriteLine("A "+(i+1)+". szám?");
			tomb[i]=int.Parse(System.Console.ReadLine());
		}
		int min=tomb[0];
		for(int i=1; i<db; i++)
		{
			if(tomb[i]<min)
			{
				min=tomb[i];
			}
		}
		System.Console.WriteLine("A legkisebb: "+min);
	}
}