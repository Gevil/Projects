class Program
{
	enum Napszak
	{
		Reggel = 0,
		D�lel�tt = 1,
		Este = 4
	}
	static void Main()
	{
		System.Console.WriteLine("Irj be egy egyjegy� sz�mot!");
		byte i = byte.Parse(System.Console.ReadLine());
		switch (i)
		{
		case Napszak.Reggel:
			System.Console.WriteLine("Reggel van!");
			break;
		case Napszak.D�lel�tt:
			System.Console.WriteLine("D�lel�tt van!");
			break;
		case Napszak.Este:
			System.Console.WriteLine("Este van!");
			break;
		default:
			System.Console.WriteLine("D�lut�n van...?");
			break;
		}
	}
}