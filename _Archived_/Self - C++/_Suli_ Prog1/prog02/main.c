#include <stdio.h>
#include <stdlib.h>

int main(int argc, char *argv[])
{
    int a;
    printf("Irjon be egy szamot!");
    scanf("%d", &a);
    
    if(a < 0)
    {
         printf("%d negat�v szam", a);
    }
    else if ( a > 0)
    {
        printf("%d pozitiv szam", a);
    }
    else if ( a == 0)
    {
        printf("%d nulla!", a);
    }
    else
    {
        printf("rossz �rt�ket adott meg a-nak");
    }
         
    
    getchar();
    getchar();
    return 0;
}
