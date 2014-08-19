

 #include "stdafx.h"
 using namespace std;

 # define MAX_VERTICES  10
 # define MAX_EDGES     15
 
 class Vertex
 {
    public:
       int label;

    public:
       Vertex( )   {  }
       ~Vertex( )  {  }

       void SetVertex(const int);
 };

 class Edge
 {
    public:
       int weight;

       Vertex V1;
       Vertex V2;

    public:
       Edge( )   { }
       ~Edge( )  { }

       void SetEdge(const Vertex,const Vertex,const int);
 };

 void Vertex::SetVertex(const int _label)
 {
    label=_label;
 }

 void Edge::SetEdge(const Vertex _V1,const Vertex _V2,const int _weight)
 {
    V1=_V1;
    V2=_V2;

    weight=_weight;
 }

 void gotoxy(int xpos, int ypos)
{
  COORD scrn;   

  HANDLE hOuput = GetStdHandle(STD_OUTPUT_HANDLE);

  scrn.X = xpos; scrn.Y = ypos;

  SetConsoleCursorPosition(hOuput,scrn);
}

int wherex()
{
	CONSOLE_SCREEN_BUFFER_INFO BufInfo;

	GetConsoleScreenBufferInfo(GetStdHandle(STD_OUTPUT_HANDLE),&BufInfo);
	return BufInfo.dwCursorPosition.X;
}

int wherey()
{
	CONSOLE_SCREEN_BUFFER_INFO BufInfo;

	GetConsoleScreenBufferInfo(GetStdHandle(STD_OUTPUT_HANDLE),&BufInfo);
	return BufInfo.dwCursorPosition.Y;
}

void clrscr()
{
	system("cls");
}
 
int _tmain(int argc, _TCHAR* argv[])
{
    clrscr( );
    //textmode(BW80);

    /***************************************************************
		    Sample Input
		    ************
	     Vertices  ,  Edges
		    6  ,  10

	       Vertex_1 , Vertex_2
		    320 , 100
		    170 , 200
		    320 , 250
		    470 , 200
		    220 , 400
		    420 , 400

	      Vertxe_1 ---->  Vertex_2 ,  Weight
		    1  ---->  2        ,  6
		    1  ---->  4        ,  5
		    1  ---->  3        ,  1
		    2  ---->  3        ,  5
		    2  ---->  5        ,  3
		    3  ---->  5        ,  6
		    3  ---->  6        ,  4
		    3  ---->  4        ,  5
		    4  ---->  5        ,  2
		    5  ---->  5        ,  6

		 Answer : 15

    ***************************************************************/

    int vertices=0;
    int edges=0;
	int count=0;

    cout<<"*******************  Input  ********************"<<endl;
    cout<<"Enter the Total Number of Vertices (1-10) = ";
    cin>>vertices;

    vertices=((vertices<1)?1:vertices);
    vertices=((vertices>10)?10:vertices);

    cout<<"Enter the Total Number of Edges (1-15) = ";
    cin>>edges;

    edges=((edges<0)?0:edges);
    edges=((edges>15)?15:edges);

    Vertex  V[MAX_VERTICES];
    Edge    E[MAX_EDGES];

    for(count=0;count<vertices;count++)
       V[count].SetVertex(count);

    cout<<endl;

    int v1;
    int v2;
    int weight;

    cout<<" **********  Edges and their Weights  ********* "<<endl;

    for(int count=0;count<edges;count++)
    {
       cout<<"    ----------       ---------->";

       gotoxy(2,wherey( ));
       cin>>v1;

       gotoxy(35,(wherey( )-1));
       cin>>v2;

       gotoxy(17,(wherey( )-1));
       cin>>weight;

       v1=((v1<1)?1:v1);
       v1=((v1>vertices)?vertices:v1);

       v2=((v2<1)?1:v2);
       v2=((v2>vertices)?vertices:v2);

       weight=((weight<=0)?0:weight);

       E[count].SetEdge(V[(v1-1)],V[(v2-1)],weight);
    }

    cout<<endl<<"Press any key to Apply PRIMS Algorithm...";

    getch( );
    clrscr( );

    int U[MAX_VERTICES]={0};
    int vertex_1[MAX_VERTICES]={0};
    int vertex_2[MAX_VERTICES]={0};
    int edge_weights[MAX_VERTICES]={0};
    int resultant_weights[MAX_VERTICES]={0};

    int flag_1=0;
    int flag_2=0;
    int u_count=1;
    int temp_vertex=0;
    int temp_vertex_1=0;
    int temp_vertex_2=0;
    int lowest_edge_weight=0;

    U[0]=0;

    do
    {
       count=0;

       for(int i=0;i<vertices;i++)
       {
	  vertex_1[i]=0;
	  vertex_2[i]=0;
	  edge_weights[i]=0;
       }

       for(int i=0;i<u_count;i++)
       {
	  flag_1=0;

	  for(int j=0;j<edges;j++)
	  {
	     if(E[j].V1.label==U[i] || E[j].V2.label==U[i])
	     {
		flag_2=0;

		if(E[j].V1.label!=U[i])
		{
		   temp_vertex=E[j].V1.label;
		   temp_vertex_1=E[j].V2.label;
		}

		else
		{
		   temp_vertex=E[j].V2.label;
		   temp_vertex_1=E[j].V1.label;
		}

		for(int k=0;k<u_count;k++)
		{
		   if(temp_vertex==U[k])
		   {
		      flag_2=-1;

		      break;
		   }
		}

		if(flag_2!=-1)
		{
		   if(flag_1==0 || lowest_edge_weight>E[j].weight)
		   {
		      lowest_edge_weight=E[j].weight;
		      temp_vertex_2=temp_vertex;
		      flag_1=1;
		   }
		}
	     }
	  }

	  if(flag_1==1)
	  {
	     vertex_2[count]=temp_vertex_2;
	     vertex_1[count]=temp_vertex_1;
	     edge_weights[count]=lowest_edge_weight;

	     count++;
	  }
       }

       flag_1=0;

       for(int i=0;i<count;i++)
       {
	  if(flag_1==0 || lowest_edge_weight>edge_weights[i])
	  {
	     lowest_edge_weight=edge_weights[i];
	     temp_vertex_1=vertex_1[i];
	     temp_vertex_2=vertex_2[i];
	     flag_1=1;
	  }
       }

       if(flag_1==1)
       {
	  U[u_count]=temp_vertex_2;
	  u_count++;

	  for(int i=0;i<edges;i++)
	  {
	     if((E[i].V1.label==temp_vertex_1 && E[i].V2.label==temp_vertex_2) ||
		(E[i].V1.label==temp_vertex_2 && E[i].V2.label==temp_vertex_1) )
	     {
		resultant_weights[(u_count-2)]=E[i].weight;

		break;
	     }
	  }
       }

       else
	  break;
    }
    while(1);

    cout<<"*******************  Input  ********************"<<endl;
    cout<<" V = { ";

    for(count=1;count<vertices;count++)
       cout<<count<<",";

    cout<<count<<" } "<<endl;

    cout<<" E = { ";

    for(count=0;count<edges;count++)
    {
       cout<<"("<<(E[count].V1.label+1)<<","<<(E[count].V2.label+1)<<")";

       if(count<(edges-1))
	  cout<<",";
    }

    cout<<" } "<<endl<<endl;

    cout<<"*******************  Result  ********************"<<endl;
    cout<<" U = { ";

    for(count=0;count<(u_count-1);count++)
       cout<<(U[count]+1)<<",";

    cout<<(U[count]+1)<<" }"<<endl;
    cout<<" Total Cost = ";

    int cost=0;

    for(count=0;count<(u_count-1);count++)
    {
       cost+=resultant_weights[count];

       if(count<(u_count-2))
	  cout<<resultant_weights[count]<<"+";
    }

    cout<<resultant_weights[(count-1)]<<" =  "<<cost<<endl<<endl;

    cout<<endl<<" Press any Key to Exit...";

    getch( );
    return 0;
}

