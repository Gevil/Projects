class Program{
	static void Main(){
		int db;
		int[] tomb;
		System.Console.WriteLine("Darabszám?");
		db = int.Parse(System.Console.ReadLine());
		tomb=new int[db];
		for(int i= 0; i<db; i++){
			System.Console.WriteLine("A "+(i+1)+". szám?");
			tomb[i]=int.Parse(System.Console.ReadLine());
		}
		int min=tomb[0];
		for(int i=1; i<db; i++){
			if(tomb[i]<min){
				min=tomb[i];
			}
		}
		System.Console.WriteLine("A legkisebb: "+min);
		int temp;
/*		for(int i=0; i<db; i++){
			min=i;
			for(int j=i; j<db; j++){
				if(tomb[j]<tomb[min]){
					min=j;
				}
			}
			temp=tomb[i];
			tomb[i]=tomb[min];
			tomb[min]=temp;
		}
*/
		for(int i=0; i<db-1; i++){
			for(int j=i+1; j<db; j++){
				if(tomb[i]>tomb[j]){
					temp=tomb[i];
					tomb[i]=tomb[j];
					tomb[j]=temp;
				}
			}
		}
		foreach(int elem in tomb){
			System.Console.WriteLine(elem);
		}
	}
}