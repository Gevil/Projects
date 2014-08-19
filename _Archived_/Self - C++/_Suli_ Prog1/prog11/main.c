#include <stdio.h>
#include <stdlib.h>
#define YES 1
#define NO 0
void swap(int*, int*);
int beolv(char*, int);
void strcopy1(char*, char*);
void strcopy2(char*, char*);
char *honev(int);

int main(int argc, char *argv[])
{
    //precedencia
    //(), [] ...
    //++, --, -, +, !, *, & ...
    
    //pa = &a; //cim
    //b = *pa; //pointer
    
    
    //int a = 5, b = 11;
    //swap(&a, &b);
    
    //printf("%d %d\n", a,b );
    
    //tombok
    
    //char sor[80]; //char *ps;
    // ps = sor;
    // ps = &sor[0];
    /*
    int hossz;
    
    hossz = beolv(sor, 80);
    printf("%s\n", sor);
    */
    
    //copyyyyy
    /*char a[80], b[80];
    
    strcopy2("Ez egy szoveg\n", a);
    strcopy2(a,b);
    printf("%s\n", b);*/
    
    int ho;
    scanf("%d", &ho);
    printf("%s\n", honev(ho));
    
    
    getchar();
    getchar();
    return 0;
}

char *honev(int h)
{
    static char *nev[13]={"Hibas honap!\n",
    "januar\n","februar\n","marcius\n","aprilis\n",
    "majus\n","junius\n","julius\n","augusztus\n",
    "szeptember\n","oktober\n","november\n","december\n"};
    
    return (h<1 || h >12)?nev[0]:nev[h];
}

void strcopy1(char s1[], char s2[])
{
    int i = 0;
    while( (s2[i]=s1[i]) != "\0" ) i++;
}

void strcopy2(char *s1, char *s2)
{
    while(*s2++ = *s1++);
}

void swap(int *x, int*y)
{
    int temp;
    temp = *x;
    *x = *y;
    *y = temp;
}

int beolv(char *s, int h)
{
    int i = 0;
    while((*s = getchar())!="\n" && --h>0)
    {
        i++; s++;
    }
    *s="\0";
    return i;
}
