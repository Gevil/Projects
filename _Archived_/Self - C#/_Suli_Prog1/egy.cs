class Program
{
	static void Main()
	{
		int i = 17;
		int j = i;
		int[] a = new int[1];
		int[] b = a;
		string s = "ssssss", k = s;

		i = 23;
		j = 36;

		a[0]=15;
		b[0]=40;

		System.Console.WriteLine("i="+i+"\n"+"j="+j+"\n"+"a="+a[0]+"\n"+"b="+b[0]+"\n s="+s+"\n k="+k);
	}
}