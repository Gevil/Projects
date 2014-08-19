class pbar
{
    static void Main()
    {
         System.Console.WriteLine("ax^2 +  bx + c = 0");
         System.Console.WriteLine("a= ?");
         int a = System.Convert.ToInt32(System.Console.ReadLine());
         System.Console.WriteLine("b= ?");
         int b = System.Convert.ToInt32(System.Console.ReadLine());
         System.Console.WriteLine("c= ?");
         int c = System.Convert.ToInt32(System.Console.ReadLine());
         
         if(a!=0)
         {
              if(b*b>=4*a*c)
              {
                   if(b*b>4*a*c)
                   {
                        double x1 = (-b+System.Math.Sqrt(b*b-4*a*c))/(2*a);
                        double x2 = (-b-System.Math.Sqrt(b*b-4*a*c))/(2*a);
                        System.Console.WriteLine("x1 =" + System.Convert.ToString(x1));
                        System.Console.WriteLine("x2 =" + System.Convert.ToString(x2));
                   }else{
                        int x = -(b/(2*a));
                        System.Console.WriteLine(System.Convert.ToString(x));
                   }
              }else{
                   System.Console.WriteLine("Nincs valós gyök!");
              }
         }else{
              if(b!=0)
              {
                   int x = -(c/b);
                   System.Console.WriteLine(System.Convert.ToString(x));
              }else{
                   if(c!=0)
                   {
                        System.Console.WriteLine("Ellentmondás?!");
                   }else{
                        System.Console.WriteLine("Azonosság?!");
                   }
              }
         }
    }
}