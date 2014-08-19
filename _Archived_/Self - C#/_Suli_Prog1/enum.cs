class Program
{
	enum Napszak
	{
		Reggel = 0,
		Délelõtt = 1,
		Este = 4
	}
	static void Main()
	{
		System.Console.WriteLine("Irj be egy egyjegyû számot!");
		byte i = byte.Parse(System.Console.ReadLine());
		switch (i)
		{
		case Napszak.Reggel:
			System.Console.WriteLine("Reggel van!");
			break;
		case Napszak.Délelõtt:
			System.Console.WriteLine("Délelõtt van!");
			break;
		case Napszak.Este:
			System.Console.WriteLine("Este van!");
			break;
		default:
			System.Console.WriteLine("Délután van...?");
			break;
		}
	}
}