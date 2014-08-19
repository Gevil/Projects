class program{
	static void Main(){
		double A, B, C;
		System.Console.WriteLine("A=");
		A=double.Parse(System.Console.ReadLine());
		System.Console.WriteLine("B=");
		B=double.Parse(System.Console.ReadLine());
		System.Console.WriteLine("C=");
		C=double.Parse(System.Console.ReadLine());
		if((A+B>C)&&(A+C>B)&&(B+C>A))
		{
			double K=A+B+C;
			double S=K/2;
			double T = System.Math.Sqrt(S*(S-A)*(S-B)*(S-C));

			System.Console.WriteLine("K="+K);
			System.Console.WriteLine("T="+T);
		}else{
			double V=A*B*C;
			double Ar=2*(A*B+A*C+B*C);
			System.Console.WriteLine("A="+Ar);
			System.Console.WriteLine("V="+V);
		}
	}
}



/*
if((A+B>C)&&(A+C>B)&&(B+C>A))
{
	T=
}
*/