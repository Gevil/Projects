class Program
{
	static void Main()
	{
		string s = "abc 123 abc";

		string b ="";

		foreach(byte elem in s)
		{
			b += elem.ToString();
		}
		System.Console.WriteLine(b);
	}
}